Public Class Person
    Public Property ID As Long
    Public Property Name As String
    <ControlLibrary.FilterBuilder2.Model.ShowColumn("Street", "Rua")>
    Public Property Address As Address
End Class
