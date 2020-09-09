


Public Class FrmImagePickerTest
    Private Sub FrmImagePickerTest_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ControlLibrary.CMessageBox.CMessageBoxStyle.MessageFont = New Font("Verdana", 9.75, FontStyle.Regular)
        ControlLibrary.CMessageBox.CMessageBoxStyle.Reset()


        ' CMessageBox.Show("ERRORTITLE", "ERROR MESSAGE", CMessageBox.CMessageBoxType.Error, CMessageBox.CMessageBoxButtons.AbortRetryIgnore, New Exception("OBJECT IS NOT DEFINED"))
    End Sub
End Class