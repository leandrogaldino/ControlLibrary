Imports System.Data.Common
Imports System.Drawing
Imports System.Windows.Forms

Public Class Utility
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
    Public Shared Function RecolorImage(ByVal Image As Image, ByVal FromColor As Color, ByVal ToColor As Color) As Bitmap
        Dim bmp As Bitmap = Image
        For x As Integer = 0 To bmp.Width - 1
            For y As Integer = 0 To bmp.Height - 1
                If bmp.GetPixel(x, y) = FromColor Then
                    bmp.SetPixel(x, y, ToColor)
                End If
            Next
        Next
        Return bmp
    End Function
    Public Shared Sub DebugQuery(ByVal Command As DbCommand)
        Dim Query As String = Command.CommandText
        For Each Parameter As DbParameter In Command.Parameters
            Query = Query.Replace(Parameter.ParameterName, "'" & Parameter.Value & "'")
        Next
        Debug.Print(Query)
    End Sub
End Class