Imports ControlLibrary
Public Class FrmCodeCompilerTest
    Private _File As New IO.FileInfo("Code.txt")
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TxtCode.Text = _File.OpenText.ReadToEnd
        BtnCompile.Enabled = False

        Dim b As Byte() = {15, 193, 255, 0, 2, 19, 25, 6}
        Dim k As String = "mykey"

        Dim c = Cryptography.RC2ProviderEncryption("LEANDRO")
        Dim d = Cryptography.RC2ProviderDecryption(c)

    End Sub

    Private Sub BtnCompile_Click(sender As Object, e As EventArgs) Handles BtnCompile.Click
        If Not IsNumeric(TxtPar1.Text) Then
            MessageBox.Show("it must be a number here.")
            TxtPar1.Select()
        ElseIf Not IsNumeric(TxtPar2.Text) Then
            MessageBox.Show("it must be a number here.")
            TxtPar2.Select()
        Else
            Dim Helper As New CodeCompiler.CompilerHelper With {
                .ReferencedAssemblies = {"System.dll"},
                .Parameters = {TxtPar1.Text, TxtPar2.Text},
                .ClassName = "ClassTest",
                .MethodName = "MethodTest",
                .Language = CodeCompiler.Languages.VisualBasic,
                .GenerateInMemory = True
            }
            Using C As New CodeCompiler("Code.txt", Helper)
                TxtReturn.Text = C.Compile()
            End Using
        End If
        Dim F As New IO.FileInfo("Code.txt")

    End Sub

    Private Sub TxtPar1_TextChanged(sender As Object, e As EventArgs) Handles TxtPar2.TextChanged, TxtPar1.TextChanged
        BtnCompile.Enabled = If(TxtPar1.Text = Nothing Or TxtPar2.Text = Nothing, False, True)

    End Sub

    Public Sub abc(ParamArray ByVal A() As String)

    End Sub









    Private Sub BtnClean_Click(sender As Object, e As EventArgs) Handles BtnClean.Click
        TxtPar1.Text = Nothing
        TxtPar2.Text = Nothing
        TxtReturn.Text = Nothing
    End Sub
End Class
