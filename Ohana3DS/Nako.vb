Imports System.IO
Imports System.Text
Public Class Nako
    Public Structure GARC_File
        Dim Bits As Integer
        Dim Start_Offset As Integer
        Dim End_Offset As Integer
        Dim Length As Integer
        Dim Uncompressed_Length As Integer
        Dim Compressed As Boolean
        Dim Name As String
    End Structure
    Public Files() As GARC_File
    Private Data_Offset As Integer
    Private FATB_Offset As Integer
    Private Current_File As String

    Public Structure Inserted_File
        Dim File_Name As String
        Dim Index As Integer
    End Structure
    Public Inserted_Files As List(Of Inserted_File)

    Public Compression_Percentage As Single
    Public Fast_Compression As Boolean = False
    Public Sub Load(File_Name As String)
        Current_File = File_Name
        Dim InFile As New FileStream(File_Name, FileMode.Open)

        Dim Magic As String = Nothing
        For Characters As Integer = 0 To 3
            Magic &= Chr(InFile.ReadByte)
        Next
        If StrReverse(Magic) <> "GARC" Then
            MsgBox("Arquivo não é GARC!")
            Exit Sub
        End If

        Data_Offset = Read32(InFile, &H10)
        Dim FATO_Length As Integer = Read32(InFile, &H20)

        '+======+
        '| FATB |
        '+======+
        FATB_Offset = &H20 + FATO_Length
        Dim Total_Files As Integer = Read16(InFile, FATB_Offset + 4)
        ReDim Files(Total_Files - 1)
        Dim Offset As Integer = FATB_Offset + 8
        For Current_File As Integer = 0 To Total_Files - 1
            With Files(Current_File)
                .Bits = Read32(InFile, Offset)
                .Start_Offset = Read32(InFile, Offset + 4)
                .End_Offset = Read32(InFile, Offset + 8)
                .Length = Read32(InFile, Offset + 12)
                InFile.Seek(Data_Offset + .Start_Offset, SeekOrigin.Begin)
                If InFile.ReadByte() = &H11 Then
                    .Compressed = True
                    .Uncompressed_Length = Read24(InFile, Data_Offset + .Start_Offset + 1)
                Else
                    .Uncompressed_Length = .Length
                End If

                Dim File_Magic As String = Encoding.ASCII.GetString(BitConverter.GetBytes(Read32(InFile, Data_Offset + .Start_Offset + If(.Compressed, 5, 0))))
                Dim Format As String = Guess_Format(File_Magic)
                .Name = "file_" & Current_File & Format
            End With
            Offset += 16
        Next

        InFile.Close()

        Inserted_Files = New List(Of Inserted_File)
    End Sub
    Public Function Guess_Format(File_Magic As String) As String
        Dim Format As String
        Dim Temp As String = File_Magic.Substring(0, 2)
        Select Case Temp
            Case "PC", "PT", "PB", "PF", "PK", "PO", "GR", "MM", "AD"
                Format = "." & LCase(Temp)
            Case Else
                If File_Magic.Substring(0, 3) = "BCH" Then
                    Format = ".bch"
                ElseIf File_Magic = "CGFX" Then
                    Format = ".cgfx"
                Else
                    Format = ".bin"
                End If
        End Select
        Return Format
    End Function
    Public Sub Extract(Output_File As String, File_Index As Integer)
        Dim InFile As New FileStream(Current_File, FileMode.Open)

        With Files(File_Index)
            InFile.Seek(Data_Offset + .Start_Offset, SeekOrigin.Begin)
            Dim Data(.Length - 1) As Byte
            InFile.Read(Data, 0, .Length)

            Dim OutFile As New FileStream(Output_File, FileMode.Create)
            If .Compressed Then
                Data = LZSS_Decompress(Data)
                OutFile.Write(Data, 0, Data.Length)
            Else
                OutFile.Write(Data, 0, .Length)
            End If

            OutFile.Close()
        End With

        InFile.Close()
    End Sub
    Public Sub Insert()
        Dim Original_File As New FileStream(Current_File, FileMode.Open)
        Dim Temp_GARC_File As String = Path.GetTempFileName
        Dim Output_File As New BinaryWriter(New FileStream(Temp_GARC_File, FileMode.Create))

        Dim Header((FATB_Offset + 8) - 1) As Byte
        Original_File.Read(Header, 0, Header.Length)
        Output_File.Write(Header)

        Dim Dummy_Place_Holder As Int32 = 0
        For i As Integer = 0 To Files.Count - 1
            Output_File.Write(Dummy_Place_Holder) 'Bits
            Output_File.Write(Dummy_Place_Holder) 'Offset inicial
            Output_File.Write(Dummy_Place_Holder) 'Offset final
            Output_File.Write(Dummy_Place_Holder) 'Tamanho
        Next

        Dim File_Index As Integer
        Dim Offset As Integer
        For Each File As GARC_File In Files
            Dim Copy_Original As Boolean = True
            For i As Integer = 0 To Inserted_Files.Count - 1
                If Inserted_Files(i).Index = File_Index Then
                    Dim Data() As Byte = IO.File.ReadAllBytes(Inserted_Files(i).File_Name)
                    If File.Compressed And Data.Length < &H1000000 Then Data = LZSS_Compress(Data)

                    Output_File.Seek((FATB_Offset + 8) + (File_Index * 16), SeekOrigin.Begin)
                    Output_File.Write(File.Bits)
                    Output_File.Write(Convert.ToInt32(Offset))
                    Output_File.Write(Convert.ToInt32(Offset + Data.Length))
                    Output_File.Write(Convert.ToInt32(Data.Length))

                    Output_File.Seek(Data_Offset + Offset, SeekOrigin.Begin)
                    Output_File.Write(Data)

                    Offset += Data.Length

                    Copy_Original = False
                    Exit For
                End If
            Next

            If Copy_Original Then
                Dim Data(File.Length - 1) As Byte
                Original_File.Seek(Data_Offset + File.Start_Offset, SeekOrigin.Begin)
                Original_File.Read(Data, 0, Data.Length)

                Output_File.Seek((FATB_Offset + 8) + (File_Index * 16), SeekOrigin.Begin)
                Output_File.Write(File.Bits)
                Output_File.Write(Convert.ToInt32(Offset))
                Output_File.Write(Convert.ToInt32(Offset + Data.Length))
                Output_File.Write(Convert.ToInt32(Data.Length))

                Output_File.Seek(Data_Offset + Offset, SeekOrigin.Begin)
                Output_File.Write(Data)

                Offset += Data.Length
            End If

            File_Index += 1
        Next

        Original_File.Close()
        Output_File.Close()

        File.Delete(Current_File)
        File.Copy(Temp_GARC_File, Current_File)
    End Sub
    Public Shared Function LZSS_Decompress(InData() As Byte) As Byte()
        If InData(0) <> &H11 Then MsgBox(Hex(InData(0)))
        Dim Decompressed_Size As Integer = (Read32(InData, 0) And &HFFFFFF00) >> 8
        Dim Data(Decompressed_Size - 1) As Byte
        Dim Dic(&HFFF) As Byte

        Dim SrcPtr As Integer = 4, DstPtr As Integer
        Dim BitCount As Integer = 8
        Dim DicPtr As Integer
        Dim BitFlags As Byte
        While DstPtr < Decompressed_Size And SrcPtr < InData.Length
            If BitCount = 8 Then
                BitFlags = InData(SrcPtr)
                SrcPtr += 1
                BitCount = 0
            End If

            If (BitFlags And Convert.ToByte(2 ^ (7 - BitCount))) = 0 Then
                Dic(DicPtr) = InData(SrcPtr)
                SrcPtr += 1
                Data(DstPtr) = Dic(DicPtr)
                DicPtr = (DicPtr + 1) And &HFFF
                DstPtr += 1
            Else
                Dim Back, Len As Integer
                Back = -1
                Dim Indicator As Integer = (InData(SrcPtr) And &HF0) >> 4
                Select Case Indicator
                    Case 0
                        Dim Byte_1 As Integer = InData(SrcPtr)
                        Dim Byte_2 As Integer = InData(SrcPtr + 1)
                        Dim Byte_3 As Integer = InData(SrcPtr + 2)

                        Back = ((Byte_2 And &HF) << 8) Or Byte_3
                        Len = (((Byte_1 And &HF) << 4) Or (Byte_2 >> 4)) + &H11
                        SrcPtr += 3
                    Case 1
                        Dim Byte_1 As Integer = InData(SrcPtr)
                        Dim Byte_2 As Integer = InData(SrcPtr + 1)
                        Dim Byte_3 As Integer = InData(SrcPtr + 2)
                        Dim Byte_4 As Integer = InData(SrcPtr + 3)

                        Back = ((Byte_3 And &HF) << 8) Or Byte_4
                        Len = (((Byte_1 And &HF) << 12) Or (Byte_2 << 4) Or (Byte_3 >> 4)) + &H111
                        SrcPtr += 4
                    Case Else
                        Dim Byte_1 As Integer = InData(SrcPtr)
                        Dim Byte_2 As Integer = InData(SrcPtr + 1)

                        Back = ((Byte_1 And &HF) << 8) Or Byte_2
                        Len = Indicator + 1
                        SrcPtr += 2
                End Select
                Back += 1

                If DstPtr + Len > Decompressed_Size Then Len = Decompressed_Size - DstPtr
                Dim Original_Dictionary_Pointer As Integer = DicPtr
                For i As Integer = 0 To Len - 1
                    Dic(DicPtr) = Dic((Original_Dictionary_Pointer - Back + i) And &HFFF)
                    Data(DstPtr) = Dic(DicPtr)
                    DstPtr += 1
                    DicPtr = (DicPtr + 1) And &HFFF
                Next
            End If
            BitCount += 1
        End While

        Return Data
    End Function
    Private Function LZSS_Compress(InData() As Byte) As Byte()
        Dim Dic(&HFFF) As Byte
        Dim DicPtr As Integer

        Dim Data(Convert.ToInt32(InData.Length + ((InData.Length) / 8) + 3)) As Byte
        Dim SrcPtr, DstPtr, BitCount As Integer
        Data(0) = &H11
        Data(1) = Convert.ToByte(InData.Length And &HFF)
        Data(2) = Convert.ToByte((InData.Length And &HFF00) >> 8)
        Data(3) = Convert.ToByte((InData.Length And &HFF0000) >> 16)
        DstPtr = 4
        Dim BitsPtr As Integer

        While SrcPtr < InData.Length
            If BitCount = 0 Then
                BitsPtr = DstPtr
                DstPtr += 1
                BitCount = 8
            End If

            Dim DicPos As Integer = 0
            Dim Found_Data As Integer = 0
            Dim Compressed_Data As Boolean = False
            Dim Index As Integer = Array.IndexOf(Dic, InData(SrcPtr))
            If Index <> -1 Then
                Do
                    Dim DataSize As Integer = 0
                    For j As Integer = 0 To 15
                        If SrcPtr + j >= InData.Length Then Exit For
                        If Dic((Index + j) And &HFFF) = InData(SrcPtr + j) Then
                            DataSize += 1
                        Else
                            Exit For
                        End If
                    Next
                    If DataSize >= 3 Then
                        If Index + DataSize < DicPtr Or Index > DicPtr + DataSize Then
                            If DataSize > Found_Data Then
                                Compressed_Data = True
                                Found_Data = DataSize
                                DicPos = Index
                            End If
                        End If
                    End If
                    If Fast_Compression Then Exit Do
                    Index = Array.IndexOf(Dic, InData(SrcPtr), Index + 1)
                    If Index = -1 Then Exit Do
                Loop
            End If

            If Compressed_Data And ((DicPos < SrcPtr) Or (SrcPtr > &HFFF)) Then
                Dim Back As Integer = DicPtr - DicPos - 1
                Data(BitsPtr) = Data(BitsPtr) Or Convert.ToByte((2 ^ (BitCount - 1))) 'Comprimido, define bit
                Data(DstPtr) = Convert.ToByte((((Found_Data - 1) And &HF) * &H10) + ((Back And &HF00) / &H100))
                Data(DstPtr + 1) = Convert.ToByte(Back And &HFF)
                DstPtr += 2
                For j As Integer = 0 To Found_Data - 1
                    Dic(DicPtr) = InData(SrcPtr)
                    DicPtr = (DicPtr + 1) And &HFFF
                    SrcPtr += 1
                Next
            Else
                Data(DstPtr) = InData(SrcPtr)
                Dic(DicPtr) = Data(DstPtr)
                DicPtr = (DicPtr + 1) And &HFFF
                DstPtr += 1
                SrcPtr += 1
            End If

            Compression_Percentage = Convert.ToSingle((SrcPtr / InData.Length) * 100)

            BitCount -= 1
        End While

        ReDim Preserve Data(DstPtr - 1)

        Return Data
    End Function
End Class
