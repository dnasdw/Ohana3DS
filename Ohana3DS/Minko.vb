Imports System.IO
Imports System.Text
Public Class Minko
    Public Strings() As String
    Public Sub Extract_Strings(File_Name As String)
        Dim Data() As Byte = File.ReadAllBytes(File_Name)
        Dim Text_Sections As Integer = Read16(Data, 0)
        Dim Lines As Integer = Read16(Data, 2)
        Dim Total_Size As Integer = Read32(Data, 4)
        Dim Initial_Key As Integer = Read32(Data, 8)
        Dim Section_Data As Integer = Read32(Data, 12)

        Dim Key As Integer = &H7C89
        ReDim Strings(Lines - 1)

        For Line As Integer = 0 To Lines - 1
            Dim CurrKey As Integer = Key
            Dim Offset As Integer = Read32(Data, Line * 8 + Section_Data + 4) + Section_Data
            Dim Length As Integer = Read16(Data, Line * 8 + Section_Data + 8)
            Dim Start_Offset As Integer = Offset

            Dim CurrLine As String = Nothing

            While Offset < Start_Offset + Length * 2
                Dim Current_Data As Integer = (Read16(Data, Offset) Xor CurrKey) And &HFFFF
                CurrKey = (((CurrKey << 3) Or (CurrKey >> 13)) And &HFFFF)
                Offset += 2
                If Current_Data = 0 Then Exit While

                If Current_Data = &H10 Then
                    Dim VarLen As Integer = (Read16(Data, Offset) Xor CurrKey) And &HFFFF
                    CurrKey = (((CurrKey << 3) Or (CurrKey >> 13)) And &HFFFF)
                    Dim VarType As Integer = (Read16(Data, Offset + 2) Xor CurrKey) And &HFFFF
                    CurrKey = (((CurrKey << 3) Or (CurrKey >> 13)) And &HFFFF)
                    Offset += 4

                    CurrLine &= "\code:0x" & Hex(VarLen).PadLeft(4, "0"c)
                    CurrLine &= "\0x" & Hex(VarType).PadLeft(4, "0"c)
                    VarLen -= 1
                    While VarLen > 0
                        Dim VarData As Integer = (Read16(Data, Offset) Xor CurrKey) And &HFFFF
                        CurrKey = (((CurrKey << 3) Or (CurrKey >> 13)) And &HFFFF)
                        Offset += 2
                        VarLen -= 1

                        CurrLine &= "\0x" & Hex(VarData).PadLeft(4, "0"c)
                    End While
                Else
                    Select Case Current_Data
                        Case &HE07F : Current_Data = &H202F
                        Case &HE08D : Current_Data = &H2026
                        Case &HE08E : Current_Data = &H2642
                        Case &HE08F : Current_Data = &H2640
                    End Select
                    CurrLine &= ChrW(Current_Data)
                End If
            End While

            Key = (Key + &H2983) And &HFFFF
            Strings(Line) = CurrLine
        Next
    End Sub
    Public Sub Insert_Strings(Texts() As String, Out_File As String)
        Dim Data As New BinaryWriter(New FileStream(Out_File, FileMode.Create))
        Data.Write(Convert.ToUInt16(1))
        Data.Write(Convert.ToUInt16(Texts.Length))
        Data.Write(Convert.ToUInt32(0)) 'Tamanho total
        Data.Write(Convert.ToUInt32(0))
        Data.Write(Convert.ToUInt32(&H10))

        Dim Key As Integer = &H7C89

        Dim Header_Offset As Integer = &H14
        Dim Base As Integer = Header_Offset + Texts.Count * 8
        Data.Seek(Base, SeekOrigin.Begin)

        Dim Total_Bytes As Integer
        For Each Line As String In Texts
            Dim CurrKey As Integer = Key

            Dim Byte_Count As Integer = 0
            Dim Block_Length As Integer = 0
            Dim CurrOffset As Integer = Total_Bytes
            For i As Integer = 0 To Line.Length - 1
                Dim Code_Mode As Boolean = False

                If i + 6 < Line.Length Then
                    If Line.Substring(i, 6) = "\code:" Then
                        Code_Mode = True

                        Dim VarLen As Integer = Convert.ToInt32(Line.Substring(i + 8, 4), 16)
                        Dim Value As Integer = (&H10 Xor CurrKey) And &HFFFF
                        CurrKey = (((CurrKey << 3) Or (CurrKey >> 13)) And &HFFFF)
                        Data.Write(Convert.ToUInt16(Value))
                        Value = ((VarLen And &HFFFF) Xor CurrKey) And &HFFFF
                        CurrKey = (((CurrKey << 3) Or (CurrKey >> 13)) And &HFFFF)
                        Data.Write(Convert.ToUInt16(Value))
                        Byte_Count += 4

                        Dim StrPos As Integer = 12
                        For j As Integer = 0 To VarLen - 1
                            Value = Convert.ToInt32(Line.Substring(i + StrPos + 3, 4), 16)
                            Value = ((Value And &HFFFF) Xor CurrKey) And &HFFFF
                            CurrKey = (((CurrKey << 3) Or (CurrKey >> 13)) And &HFFFF)
                            Data.Write(Convert.ToUInt16(Value))
                            StrPos += 7
                            Byte_Count += 2
                            Block_Length += 1
                        Next

                        i += StrPos - 1
                    End If
                End If

                If Not Code_Mode Then
                    Dim Character As String = Line.Substring(i, 1)
                    Dim CharCode As Integer = AscW(Character)
                    CharCode = ((CharCode And &HFFFF) Xor CurrKey) And &HFFFF
                    CurrKey = (((CurrKey << 3) Or (CurrKey >> 13)) And &HFFFF)
                    Data.Write(Convert.ToUInt16(CharCode))
                    Byte_Count += 2
                End If
            Next

            Dim Zero As Integer = (0 Xor CurrKey) And &HFFFF
            Data.Write(Convert.ToUInt16(Zero))
            Byte_Count += 2
            Total_Bytes += Byte_Count
            If Total_Bytes Mod 4 > 0 Then
                Data.Write(Convert.ToUInt16(0))
                Total_Bytes += 2
            End If

            Data.Seek(Header_Offset, SeekOrigin.Begin)
            Data.Write(Convert.ToUInt32(CurrOffset + 4 + Texts.Count * 8))
            Data.Write(Convert.ToUInt32(Byte_Count \ 2))
            Header_Offset += 8

            Data.Seek(Base + Total_Bytes, SeekOrigin.Begin)

            Key = (Key + &H2983) And &HFFFF
        Next

        Data.Seek(4, SeekOrigin.Begin)
        Data.Write(Convert.ToUInt32(Total_Bytes + Base - &H10))
        Data.Seek(&H10, SeekOrigin.Begin)
        Data.Write(Convert.ToUInt32(Total_Bytes + Base - &H10))

        Data.Close()
    End Sub
End Class
