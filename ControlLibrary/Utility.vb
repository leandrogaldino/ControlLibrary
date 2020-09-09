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
End Class