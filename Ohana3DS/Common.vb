Imports System.IO
Module Common
    Public MyOhana As New Ohana
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
End Module
