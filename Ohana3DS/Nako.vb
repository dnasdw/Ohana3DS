Imports System.IO
Public Class Nako
    Public Structure GARC_File
        Dim Bits As Integer
        Dim Start_Offset As Integer
        Dim End_Offset As Integer
        Dim Length As Integer
        Dim Uncompressed_Length As Integer
        Dim Compressed As Boolean
    End Structure
    Public Files() As GARC_File
    Private Data_Offset As Integer
    Private Current_File As String
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
        Dim FATB_Offset As Integer = &H20 + FATO_Length
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
            End With
            Offset += 16
        Next

        InFile.Close()
    End Sub
    Public Sub Extract(Output_File As String, File_Index As Integer)
        Dim InFile As New FileStream(Current_File, FileMode.Open)

        With Files(File_Index)
            InFile.Seek(Data_Offset + .Start_Offset, SeekOrigin.Begin)
            Dim Data(.Length - 1) As Byte
            InFile.Read(Data, 0, .Length)

            Dim Format As String
            Dim Magic As String = Nothing
            For i As Integer = If(.Compressed, 5, 0) To If(.Compressed, 8, 3)
                Magic &= Chr(Data(i))
            Next
            Dim Temp As String = Left(Magic, 2)
            Select Case Temp
                Case "PC", "PT", "PB", "PF", "PK", "PO", "GR"
                    Format = "." & LCase(Temp)
                Case Else
                    If Left(Magic, 3) = "BCH" Then
                        Format = ".bch"
                    ElseIf Left(Magic, 4) = "CGFX" Then
                        Format = ".cgfx"
                    Else
                        Format = ".bin"
                    End If
            End Select

            Dim OutFile As New FileStream(Output_File & Format, FileMode.Create)

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
    Public Shared Function LZSS_Decompress(InData() As Byte) As Byte()
        If InData(0) <> &H11 Then MsgBox(Hex(InData(0)))
        Dim Decompressed_Size As Integer = (Read32(InData, 0) And &HFFFFFF00) >> 8
        Dim Data(Decompressed_Size - 1) As Byte
        Dim Dic(&HFFF) As Byte

        Dim SrcPtr As Integer = 4, DstPtr As Integer
        Dim BitCount As Integer = 8
        Dim Dictionary_Pointer As Integer
        Dim BitFlags As Byte
        While DstPtr < Decompressed_Size And SrcPtr < InData.Length
            If BitCount = 8 Then
                BitFlags = InData(SrcPtr)
                SrcPtr += 1
                BitCount = 0
            End If

            If (BitFlags And (2 ^ (7 - BitCount))) = 0 Then
                Dic(Dictionary_Pointer) = InData(SrcPtr)
                SrcPtr += 1
                Data(DstPtr) = Dic(Dictionary_Pointer)
                Dictionary_Pointer = (Dictionary_Pointer + 1) And &HFFF
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
                Dim Original_Dictionary_Pointer As Integer = Dictionary_Pointer
                For i As Integer = 0 To Len - 1
                    Dic(Dictionary_Pointer) = Dic((Original_Dictionary_Pointer - Back + i) And &HFFF)
                    Data(DstPtr) = Dic(Dictionary_Pointer)
                    DstPtr += 1
                    Dictionary_Pointer = (Dictionary_Pointer + 1) And &HFFF
                Next
            End If
            BitCount += 1
        End While

        Return Data
    End Function
End Class
