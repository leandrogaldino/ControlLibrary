Imports System.Data.Common
Imports System.Drawing
Imports System.Windows.Forms

Public Class Utility
    ''' <summary>
    ''' Retorna se o controle especificado esta totalmente visível em seu controle pai.
    ''' </summary>
    ''' <param name="Parent">O controle pai.</param>
    ''' <param name="Child">O controle a ser testado.</param>
    ''' <returns></returns>
    Public Shared Function IsControlFullyVisible(ByVal Parent As Control, ByVal Child As Control) As Boolean
        Dim myBounds As Rectangle = Parent.ClientRectangle
        If Not myBounds.Contains(Child.Location) Then
            Return False
        End If
        If myBounds.Right < Child.Right Then
            Return False
        End If
        If myBounds.Height < Child.Bottom Then
            Return False
        End If
        Return True
    End Function
    ''' <summary>
    ''' Altera uma cor por outra na imagem.
    ''' </summary>
    ''' <param name="Image">A imagem a ser recolorida.</param>
    ''' <param name="FromColor">A cor na imagem a ser substituida.</param>
    ''' <param name="ToColor">A cor que deverá substituir a cor antiga.</param>
    ''' <returns></returns>
    Public Shared Function GetRecoloredImage(ByVal Image As Image, ByVal FromColor As Integer, ByVal ToColor As Integer) As Bitmap
        Dim bmp As Bitmap = Image
        For x As Integer = 0 To bmp.Width - 1
            For y As Integer = 0 To bmp.Height - 1
                If bmp.GetPixel(x, y) = Color.FromArgb(FromColor) Then
                    bmp.SetPixel(x, y, Color.FromArgb(ToColor))
                End If
            Next
        Next
        Return bmp
    End Function
    Public Shared Function GetImageColors(ByVal Img As Image) As List(Of Integer)
        Dim Lst As New List(Of Integer)
        Dim bmp As Bitmap = Img
        For x As Integer = 0 To bmp.Width - 1
            For y As Integer = 0 To bmp.Height - 1
                Lst.Add(bmp.GetPixel(x, y).ToArgb())
            Next
        Next
        Return Lst.Distinct().ToList
    End Function

    ''' <summary>
    ''' Depura um ComandoSQL, substituindo os parâmetros na query.
    ''' </summary>
    ''' <param name="Command">O SqlCommand contendo a query e os paramêtros.</param>
    Public Shared Sub DebugQuery(ByVal Command As DbCommand)
        Dim Query As String = Command.CommandText
        For Each Parameter As DbParameter In Command.Parameters
            Query = Query.Replace(Parameter.ParameterName, "'" & Parameter.Value & "'")
        Next
        Debug.Print(Query)
    End Sub
End Class