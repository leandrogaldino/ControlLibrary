Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Public Class Cryptography
    Private Shared RC2CSP As RC2CryptoServiceProvider
    Private Shared DesProvider As DESCryptoServiceProvider
    Shared Sub New()
        RC2CSP = New RC2CryptoServiceProvider()
        DesProvider = New DESCryptoServiceProvider()
    End Sub
    Public Shared Function GetMD5Hash(ByVal TextString As String) As String
        Using Hasher As MD5 = MD5.Create()
            Dim Bytes As Byte() = Hasher.ComputeHash(Encoding.UTF8.GetBytes(TextString))
            Dim SBuilder As New StringBuilder()
            For n As Integer = 0 To Bytes.Length - 1
                SBuilder.Append(Bytes(n).ToString("X2"))
            Next n
            Return SBuilder.ToString()
        End Using
    End Function
    Public Shared Function RijndaelManagedEncryption(ByVal PlainText As String, ByVal Key As String, ByVal Salt As Byte()) As String
        Dim Text As Byte() = Encoding.ASCII.GetBytes(PlainText)
        Dim RijndaelCipher As RijndaelManaged = New RijndaelManaged()
        Dim DeriveBytes As New Rfc2898DeriveBytes(Key, Salt)
        Dim Encryptor As ICryptoTransform = RijndaelCipher.CreateEncryptor(DeriveBytes.GetBytes(32), DeriveBytes.GetBytes(16))
        Dim Stream As MemoryStream = New MemoryStream()
        Dim EncStream As CryptoStream = New CryptoStream(Stream, Encryptor, CryptoStreamMode.Write)
        EncStream.Write(Text, 0, Text.Length)
        EncStream.FlushFinalBlock()
        Dim CipherBytes As Byte() = Stream.ToArray()
        Stream.Close()
        EncStream.Close()
        Return Convert.ToBase64String(CipherBytes)
    End Function
    Public Shared Function RijndaelManagedDecryption(ByVal PlainText As String, ByVal Key As String, ByVal Salt As Byte()) As String
        Dim Text As Byte() = Convert.FromBase64String(PlainText)
        Dim RijndaelCipher As RijndaelManaged = New RijndaelManaged()
        Dim DeriveBytes As New Rfc2898DeriveBytes(Key, Salt)
        Dim Decryptor As ICryptoTransform = RijndaelCipher.CreateDecryptor(DeriveBytes.GetBytes(32), DeriveBytes.GetBytes(16))
        Dim Stream As MemoryStream = New MemoryStream(Text)
        Dim CryptoStream As CryptoStream = New CryptoStream(Stream, Decryptor, CryptoStreamMode.Read)
        Text = New Byte(Text.Length - 1) {}
        Dim DecryptedCount As Integer = CryptoStream.Read(Text, 0, Text.Length)
        Stream.Close()
        CryptoStream.Close()
        Return StripNullCharacters(Encoding.ASCII.GetString(Text))
    End Function
    Public Shared Function RC2ProviderEncryption(ByVal PlainText As String) As String
        Dim Text As Byte() = Encoding.ASCII.GetBytes(PlainText)
        Dim Key As Byte() = RC2CSP.Key
        Dim IV As Byte() = RC2CSP.IV
        Dim Encryptor As ICryptoTransform = RC2CSP.CreateEncryptor(Key, IV)
        Dim Encrypted As Byte() = GenericEncryptor(Text, Encryptor)
        Return Convert.ToBase64String(Encrypted)
    End Function
    Public Shared Function RC2ProviderDecryption(ByVal PlainText As String) As String
        Dim Text As Byte() = Convert.FromBase64String(PlainText)
        Dim Key As Byte() = RC2CSP.Key
        Dim IV As Byte() = RC2CSP.IV
        Dim Decryptor As ICryptoTransform = RC2CSP.CreateDecryptor(Key, IV)
        Dim DecStr As String = GenericDecryptor(Text, Decryptor)
        Return DecStr
    End Function
    Public Shared Function DESProviderEncryption(ByVal PlainText As String) As String
        Dim Text As Byte() = Encoding.ASCII.GetBytes(PlainText)
        Dim Encryptor As ICryptoTransform = DesProvider.CreateEncryptor()
        Dim Encrypted As Byte() = GenericEncryptor(Text, Encryptor)
        Return Convert.ToBase64String(Encrypted)
    End Function
    Public Shared Function DESProviderDecryption(ByVal PlainText As String) As String
        Dim Text As Byte() = Convert.FromBase64String(PlainText)
        Dim Stream As MemoryStream = New MemoryStream(Text)
        Dim Decryptor As ICryptoTransform = DesProvider.CreateDecryptor()
        Dim DecStr As String = GenericDecryptor(Text, Decryptor)
        Return DecStr
    End Function
    Private Shared Function GenericEncryptor(ByVal Text As Byte(), ByVal Encryptor As ICryptoTransform) As Byte()
        Dim StreamEncrypt As MemoryStream = New MemoryStream()
        Dim CryptoStreamEncrypt As CryptoStream = New CryptoStream(StreamEncrypt, Encryptor, CryptoStreamMode.Write)
        CryptoStreamEncrypt.Write(Text, 0, Text.Length)
        CryptoStreamEncrypt.FlushFinalBlock()
        Dim Encrypted As Byte() = StreamEncrypt.ToArray()
        Return Encrypted
    End Function
    Private Shared Function GenericDecryptor(ByVal Cypher As Byte(), ByVal Decryptor As ICryptoTransform) As String
        Dim StreamDecrypt As MemoryStream = New MemoryStream(Cypher)
        Dim CryptoStreamDecrypt As CryptoStream = New CryptoStream(StreamDecrypt, Decryptor, CryptoStreamMode.Read)
        Dim Reader As StreamReader = New StreamReader(CryptoStreamDecrypt, Encoding.ASCII)
        Dim DecStr As String = Reader.ReadToEnd()
        StreamDecrypt.Close()
        CryptoStreamDecrypt.Close()
        Reader.Close()
        Return DecStr
    End Function
    Private Shared Function StripNullCharacters(ByVal vstrStringWithNulls As String) As String
        Dim IntPosition As Integer
        Dim StrStringWithOutNulls As String
        IntPosition = 1
        StrStringWithOutNulls = vstrStringWithNulls
        Do While IntPosition > 0
            IntPosition = InStr(IntPosition, vstrStringWithNulls, vbNullChar)
            If IntPosition > 0 Then
                StrStringWithOutNulls = Left$(StrStringWithOutNulls, IntPosition - 1) &
                                  Right$(StrStringWithOutNulls, Len(StrStringWithOutNulls) - IntPosition)
            End If
            If IntPosition > StrStringWithOutNulls.Length Then
                Exit Do
            End If
        Loop
        Return StrStringWithOutNulls
    End Function

End Class
