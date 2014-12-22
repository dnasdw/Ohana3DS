Imports System.IO
Module Common
    Public MyOhana As New Ohana

    Public Power_Of_Two(7) As Byte
    Public Function Read64(Data As FileStream, Address As Integer) As UInt64
        Data.Seek(Address, SeekOrigin.Begin)
        Return Convert.ToUInt64((Data.ReadByte And &HFF) + _
            ((Data.ReadByte And &HFF) << 8) + _
            ((Data.ReadByte And &HFF) << 16) + _
            ((Data.ReadByte And &HFF) << 24) + _
            ((Data.ReadByte And &HFF) << 32) + _
            ((Data.ReadByte And &HFF) << 40) + _
            ((Data.ReadByte And &HFF) << 48) + _
            ((Data.ReadByte And &HFF) << 56))
    End Function
    Public Function Read32(Data As FileStream, Address As Integer) As Integer
        Data.Seek(Address, SeekOrigin.Begin)
        Return (Data.ReadByte And &HFF) + _
            ((Data.ReadByte And &HFF) << 8) + _
            ((Data.ReadByte And &HFF) << 16) + _
            ((Data.ReadByte And &HFF) << 24)
    End Function
    Public Function Read24(Data As FileStream, Address As Integer) As Integer
        Data.Seek(Address, SeekOrigin.Begin)
        Return (Data.ReadByte And &HFF) + _
            ((Data.ReadByte And &HFF) << 8) + _
            ((Data.ReadByte And &HFF) << 16)
    End Function
    Public Function Read16(Data As FileStream, Address As Integer) As Integer
        Data.Seek(Address, SeekOrigin.Begin)
        Return (Data.ReadByte And &HFF) + _
            ((Data.ReadByte And &HFF) << 8)
    End Function

    Public Function Read32(Data() As Byte, Address As Integer) As Integer
        Return (Data(Address) And &HFF) + _
            ((Data(Address + 1) And &HFF) << 8) + _
            ((Data(Address + 2) And &HFF) << 16) + _
            ((Data(Address + 3) And &HFF) << 24)
    End Function
    Public Function Read24(Data() As Byte, Address As Integer) As Integer
        Return (Data(Address) And &HFF) + _
            ((Data(Address + 1) And &HFF) << 8) + _
            ((Data(Address + 2) And &HFF) << 16)
    End Function
    Public Function Read16(Data() As Byte, Address As Integer) As Integer
        Return (Data(Address) And &HFF) + _
            ((Data(Address + 1) And &HFF) << 8)
    End Function

    Public Function ReadMagic(Data As FileStream, Address As Integer, Length As Integer) As String
        Data.Seek(Address, SeekOrigin.Begin)
        Dim Magic As String = Nothing
        For i As Integer = 0 To Length - 1
            Magic &= Chr(Data.ReadByte)
        Next
        Return Magic
    End Function
    Public Function ReadMagic(Data() As Byte, Address As Integer, Length As Integer) As String
        Dim Magic As String = Nothing
        For Offset As Integer = Address To Address + Length - 1
            Magic &= Chr(Data(Offset))
        Next
        Return Magic
    End Function
End Module
