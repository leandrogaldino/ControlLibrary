


Public Class FrmImagePickerTest
    Private Sub FrmImagePickerTest_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ImagePicker1.ImagesInfo.Add(New ControlLibrary.ImagePicker.ImageInfo With {.Location = "C:\Users\leand\Pictures\1.PNG", .Selected = True})
    End Sub
End Class