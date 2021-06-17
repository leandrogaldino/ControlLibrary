

Imports System.Runtime.InteropServices

Public Class QueriedBoxTests
    Friend WithEvents EvaluationCalendar As New MonthCalendar
    Private Sub TxtEvaluationDate_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtEvaluationDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            BtnEvaluationDate.PerformClick()
        End If
    End Sub
    Private Sub TxtEvaluationDate_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles TxtEvaluationDate.Enter
        BeginInvoke(CType(Sub()
                              SetMaskedTextBoxSelectAll(CType(sender, MaskedTextBox))
                          End Sub, Action))
    End Sub
    Private Sub SetMaskedTextBoxSelectAll(ByVal TextBox As MaskedTextBox)
        TextBox.SelectAll()
    End Sub
    Private Sub TxtEvaluationDate_Leave(sender As Object, e As EventArgs) Handles TxtEvaluationDate.Leave
        If IsDate(TxtEvaluationDate.Text) Then TxtEvaluationDate.Text = CDate(TxtEvaluationDate.Text).ToString("dd/MM/yyyy")
    End Sub
    Private Sub EvaluationCalendar_DateSelected(sender As Object, e As DateRangeEventArgs) Handles EvaluationCalendar.DateSelected
        CcoEvaluation.CloseDropDown()
    End Sub
    Private Sub CcoInitialDelivery_Closed(sender As Object) Handles CcoEvaluation.Closed
        TxtEvaluationDate.Text = EvaluationCalendar.SelectionStart
    End Sub
    Private Sub CcoInitialDelivery_Dropped(sender As Object) Handles CcoEvaluation.Dropped
        If Not IsDate(TxtEvaluationDate.Text) Then
            EvaluationCalendar.SetDate(Today)
        Else
            EvaluationCalendar.SetDate(TxtEvaluationDate.Text)
        End If
    End Sub
    Private Sub BtnEvaluationDate_Click(sender As Object, e As EventArgs) Handles BtnEvaluationDate.Click
        EvaluationCalendar.Visible = True
    End Sub

    Private Sub QueriedBoxTests_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        EvaluationCalendar.Visible = False
        Controls.Add(EvaluationCalendar)
        CcoEvaluation.DropDownControl = EvaluationCalendar


        Dim btn = New Button()
        btn.Size = New Size(25, TxtEvaluationDate.ClientSize.Height + 2)
        btn.Location = New Point(TxtEvaluationDate.ClientSize.Width - btn.Width + 1, -1)
        btn.Cursor = Cursors.[Default]
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0
        btn.BackgroundImage = Image.FromFile("C:\Users\faturamento\Documents\GitHub\Evaluation\Evaluation\Resources\Calendar.png")
        btn.BackgroundImageLayout = ImageLayout.Center
        AddHandler btn.Click, AddressOf Btn_Click
        AddHandler btn.MouseDown, AddressOf Btn_MouseDown
        AddHandler btn.MouseUp, AddressOf Btn_MouseUp
        TxtEvaluationDate.Controls.Add(btn)
        SendMessage(TxtEvaluationDate.Handle, &HD3, CType(2, IntPtr), CType(btn.Width << 16, IntPtr))


    End Sub


    Private Sub Btn_Click(ByVal sender As Object, ByVal e As EventArgs)

        MessageBox.Show("hello world")
    End Sub

    <DllImport("User32", SetLastError:=True)>
    Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As IntPtr) As Integer
    End Function


    Private c As Integer
    Private Sub Btn_MouseDown(sender As Object, e As MouseEventArgs)
        c = TxtEvaluationDate.SelectionStart
    End Sub
    Private Sub Btn_MouseUp(sender As Object, e As MouseEventArgs)
        BeginInvoke(CType(Sub()
                              SetMaskedTextBoxSelectAll(CType(TxtEvaluationDate, MaskedTextBox))
                          End Sub, Action))
    End Sub


    '<System.Runtime.InteropServices.DllImport("user32.dll")>
    ' Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wp As IntPtr, ByVal lp As IntPtr) As IntPtr


End Class