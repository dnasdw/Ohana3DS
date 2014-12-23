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
    Public Current_File As String

    Public Structure Inserted_File
        Dim File_Name As String
        Dim Index As Integer
    End Structure
    Public Inserted_Files As List(Of Inserted_File)

    Public Compression_Percentage As Single
    Public Fast_Compression As Boolean = False
    Public Function Load(File_Name As String) As Boolean
        Current_File = File_Name
        Dim InFile As New FileStream(File_Name, FileMode.Open)

        If StrReverse(ReadMagic(InFile, 0, 4)) = "GARC" Then
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
                    Dim File_Magic As String = ReadMagic(InFile, Data_Offset + .Start_Offset + If(.Compressed, 5, 0), 4)
                    Dim Format As String = Guess_Format(File_Magic)
                    .Name = "file_" & Current_File & Format
                End With
                Offset += 16
            Next
        ElseIf Read32(InFile, 4) = &H80 Then
            Dim Temp As New List(Of GARC_File)
            Dim Index As Integer
            For i As Integer = 4 To &H80 Step 4
                Dim File As GARC_File
                With File
                    .Start_Offset = Read32(InFile, i)
                    If .Start_Offset = InFile.Length Then Exit For
                    .End_Offset = Read32(InFile, i + 4)
                    .Length = .End_Offset - .Start_Offset
                    InFile.Seek(.Start_Offset, SeekOrigin.Begin)
                    If InFile.ReadByte() = &H11 Then
                        .Compressed = True
                        .Uncompressed_Length = Read24(InFile, .Start_Offset + 1)
                    Else
                        .Uncompressed_Length = .Length
                    End If

                    Dim File_Magic As String = ReadMagic(InFile, .Start_Offset + If(.Compressed, 5, 0), 4)
                    Dim Format As String = Guess_Format(File_Magic)
                    .Name = "file_" & Index & Format
                    Index += 1
                End With
                Temp.Add(File)
            Next
            Files = Temp.ToArray()
        Else
            InFile.Close()
            Return False
        End If

        InFile.Close()

        Inserted_Files = New List(Of Inserted_File)
        Return True
    End Function
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
    Public Sub Extract(InFile As FileStream, Output_File As String, File_Index As Integer)
        Dim GARC As Boolean = StrReverse(ReadMagic(InFile, 0, 4)) = "GARC"
        With Files(File_Index)
            InFile.Seek(If(GARC, Data_Offset, 0) + .Start_Offset, SeekOrigin.Begin)
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
    End Sub
    Public Sub Insert()
        Dim Original_File As New FileStream(Current_File, FileMode.Open)
        If StrReverse(ReadMagic(Original_File, 0, 4)) = "GARC" Then
            Original_File.Seek(0, SeekOrigin.Begin)
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
        ElseIf Read32(Original_File, 4) = &H80 Then
            Dim Stream As New MemoryStream
            Dim New_Data As New BinaryWriter(Stream)
            New_Data.Write(Read32(Original_File, 0)) 'Magic
            Dim Out_Offset As Integer = &H80
            Dim Index As Integer
            For i As Integer = 4 To &H80 Step 4
                Dim In_Offset As Integer = Read32(Original_File, i)
                If In_Offset = Original_File.Length Then
                    New_Data.Seek(i, SeekOrigin.Begin)
                    Dim Len As Long = Stream.Length
                    If (Len And &HFF) > 0 Then Len = (Len And &HFFFFFF00) + &H100
                    New_Data.Write(Convert.ToInt32(Len))
                    If Len <> Stream.Length Then
                        New_Data.Seek(Convert.ToInt32(Len - 1), SeekOrigin.Begin)
                        New_Data.Write(Convert.ToByte(0))
                    End If
                    Exit For
                End If
                Dim Length As Integer
                Dim Copy_Original As Boolean = True
                For j As Integer = 0 To Inserted_Files.Count - 1
                    If Inserted_Files(j).Index = Index Then
                        Dim Data() As Byte = File.ReadAllBytes(Inserted_Files(j).File_Name)
                        If Files(Index).Compressed Then Data = LZSS_Compress(Data)
                        Length = Data.Length
                        If (Length And &HFF) > 0 Then Length = (Length And &HFFFFFF00) + &H100
                        New_Data.Seek(Out_Offset, SeekOrigin.Begin)
                        New_Data.Write(Data, 0, Data.Length)

                        Copy_Original = False
                    End If
                Next
                If Copy_Original Then
                    Length = Read32(Original_File, i + 4) - In_Offset
                    Dim Buff(Length - 1) As Byte
                    Original_File.Seek(In_Offset, SeekOrigin.Begin)
                    Original_File.Read(Buff, 0, Buff.Length)
                    New_Data.Seek(Out_Offset, SeekOrigin.Begin)
                    New_Data.Write(Buff, 0, Buff.Length)
                End If
                New_Data.Seek(i, SeekOrigin.Begin)
                New_Data.Write(Convert.ToInt32(Out_Offset))
                Out_Offset += Length
                Index += 1
            Next
            Original_File.Close()
            File.WriteAllBytes(Current_File, Stream.ToArray())
        End If
    End Sub
    Public Function LZSS_Decompress(InData() As Byte) As Byte()
        Dim Decompressed_Size As Integer = (Read32(InData, 0) And &HFFFFFF00) >> 8
        Dim Data(Decompressed_Size - 1) As Byte
        Dim Dic(&HFFF) As Byte

        Dim Source_Offset As Integer = 4, Destination_Offset As Integer
        Dim BitCount As Integer = 8
        Dim Dictionary_Offset As Integer
        Dim BitFlags As Integer
        While Destination_Offset < Decompressed_Size
            If BitCount = 8 Then
                BitFlags = InData(Source_Offset)
                Source_Offset += 1
                BitCount = 0
            End If

            If (BitFlags And Power_Of_Two(7 - BitCount)) = 0 Then
                Dic(Dictionary_Offset) = InData(Source_Offset)
                Source_Offset += 1
                Data(Destination_Offset) = Dic(Dictionary_Offset)
                Dictionary_Offset = (Dictionary_Offset + 1) And &HFFF
                Destination_Offset += 1
            Else
                Dim Back, Length As Integer
                Dim Indicator As Integer = (InData(Source_Offset) And &HF0) >> 4
                Select Case Indicator
                    Case 0
                        Dim Byte_1 As Integer = InData(Source_Offset)
                        Dim Byte_2 As Integer = InData(Source_Offset + 1)
                        Dim Byte_3 As Integer = InData(Source_Offset + 2)

                        Back = ((Byte_2 And &HF) << 8) Or Byte_3
                        Length = (((Byte_1 And &HF) << 4) Or (Byte_2 >> 4)) + &H11
                        Source_Offset += 3
                    Case 1
                        Dim Byte_1 As Integer = InData(Source_Offset)
                        Dim Byte_2 As Integer = InData(Source_Offset + 1)
                        Dim Byte_3 As Integer = InData(Source_Offset + 2)
                        Dim Byte_4 As Integer = InData(Source_Offset + 3)

                        Back = ((Byte_3 And &HF) << 8) Or Byte_4
                        Length = (((Byte_1 And &HF) << 12) Or (Byte_2 << 4) Or (Byte_3 >> 4)) + &H111
                        Source_Offset += 4
                    Case Else
                        Dim Byte_1 As Integer = InData(Source_Offset)
                        Dim Byte_2 As Integer = InData(Source_Offset + 1)

                        Back = ((Byte_1 And &HF) << 8) Or Byte_2
                        Length = Indicator + 1
                        Source_Offset += 2
                End Select
                Back += 1

                If Destination_Offset + Length > Decompressed_Size Then Length = Decompressed_Size - Destination_Offset
                Dim Old_Offset As Integer = Dictionary_Offset
                For i As Integer = 0 To Length - 1
                    Dic(Dictionary_Offset) = Dic((Old_Offset - Back + i) And &HFFF)
                    Data(Destination_Offset) = Dic(Dictionary_Offset)
                    Destination_Offset += 1
                    Dictionary_Offset = (Dictionary_Offset + 1) And &HFFF
                Next
            End If
            BitCount += 1
        End While

        Return Data
    End Function
    Public Function LZSS_Compress(InData() As Byte) As Byte()
        Dim Dic(&HFFF) As Byte
        Dim Dictionary_Offset As Integer

        Dim Data(Convert.ToInt32(InData.Length + ((InData.Length) / 8) + 3)) As Byte
        Dim Source_Offset, Destination_Offset, BitCount As Integer
        Data(0) = &H11
        Data(1) = Convert.ToByte(InData.Length And &HFF)
        Data(2) = Convert.ToByte((InData.Length And &HFF00) >> 8)
        Data(3) = Convert.ToByte((InData.Length And &HFF0000) >> 16)
        Destination_Offset = 4
        Dim BitsPtr As Integer

        While Source_Offset < InData.Length
            If BitCount = 0 Then
                BitsPtr = Destination_Offset
                Destination_Offset += 1
                BitCount = 8
            End If

            Dim DicPos As Integer = 0
            Dim Found_Data As Integer = 0
            Dim Compressed_Data As Boolean = False
            Dim Index As Integer = Array.IndexOf(Dic, InData(Source_Offset))
            If Index <> -1 Then
                Do
                    Dim DataSize As Integer = 0
                    For j As Integer = 0 To 15
                        If Source_Offset + j >= InData.Length Then Exit For
                        If Dic((Index + j) And &HFFF) = InData(Source_Offset + j) Then
                            DataSize += 1
                        Else
                            Exit For
                        End If
                    Next
                    If DataSize >= 3 Then
                        If Index + DataSize < Dictionary_Offset Or Index > Dictionary_Offset + DataSize Then
                            If DataSize > Found_Data Then
                                Compressed_Data = True
                                Found_Data = DataSize
                                DicPos = Index
                            End If
                        End If
                    End If
                    If Fast_Compression Then Exit Do
                    Index = Array.IndexOf(Dic, InData(Source_Offset), Index + 1)
                    If Index = -1 Then Exit Do
                Loop
            End If

            If Compressed_Data And ((DicPos < Source_Offset) Or (Source_Offset > &HFFF)) Then
                Dim Back As Integer = Dictionary_Offset - DicPos - 1
                Data(BitsPtr) = Data(BitsPtr) Or Convert.ToByte((2 ^ (BitCount - 1))) 'Comprimido, define bit
                Data(Destination_Offset) = Convert.ToByte((((Found_Data - 1) And &HF) * &H10) + ((Back And &HF00) / &H100))
                Data(Destination_Offset + 1) = Convert.ToByte(Back And &HFF)
                Destination_Offset += 2
                For j As Integer = 0 To Found_Data - 1
                    Dic(Dictionary_Offset) = InData(Source_Offset)
                    Dictionary_Offset = (Dictionary_Offset + 1) And &HFFF
                    Source_Offset += 1
                Next
            Else
                Data(Destination_Offset) = InData(Source_Offset)
                Dic(Dictionary_Offset) = Data(Destination_Offset)
                Dictionary_Offset = (Dictionary_Offset + 1) And &HFFF
                Destination_Offset += 1
                Source_Offset += 1
            End If

            Compression_Percentage = Convert.ToSingle((Source_Offset / InData.Length) * 100)

            BitCount -= 1
        End While

        ReDim Preserve Data(Destination_Offset - 1)

        Return Data
    End Function
End Class
