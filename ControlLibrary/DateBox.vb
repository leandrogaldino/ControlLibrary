Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports System.Windows.Forms.Design


Public Class DateBox
    Inherits MaskedTextBox
    Private _DesignerHost As IDesignerHost
    Friend WithEvents ControlContainer As New ControlContainer
    Friend WithEvents EvaluationCalendar As New MonthCalendar
    Friend WithEvents Button As New PictureBox
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    <EditorBrowsable(EditorBrowsableState.Never), Browsable(False)>
    Overloads ReadOnly Property Mask As String
        Get
            Return MyBase.Mask
        End Get
    End Property

    ''' <summary>
    ''' Define a imagem do botão do calendário.
    ''' </summary>
    <Category("Aparência")>
    <Description("Define a imagem do botão do calendário.")>
    Public Property ButtonImage As Image = My.Resources.Calendar


    Public Sub New()
        MyBase.Mask = "00/00/0000"
        Button = New PictureBox
        Button.Size = New Size(25, ClientSize.Height + 2)
        Button.Location = New Point(ClientSize.Width - Button.Width + 1, -1)
        Button.Cursor = Cursors.[Default]
        Button.BackgroundImage = My.Resources.Calendar
        Button.TabStop = False
        Button.BackgroundImageLayout = ImageLayout.Center
        Button.BackColor = Color.White
        Controls.Add(Button)
        ControlContainer.HostControl = Button
        EvaluationCalendar.Visible = False
        Controls.Add(EvaluationCalendar)
        ControlContainer.DropDownControl = EvaluationCalendar

        SendMessage(Handle, &HD3, CType(2, IntPtr), CType(Button.Width << 16, IntPtr))
    End Sub


    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        MyBase.OnSizeChanged(e)
        Button.Size = New Size(25, ClientSize.Height + 2)
        Button.Location = New Point(ClientSize.Width - Button.Width + 1, -1)
        Button.Cursor = Cursors.[Default]
        Button.BackgroundImage = ButtonImage
        Button.BackgroundImageLayout = ImageLayout.Center
    End Sub

    Protected Overrides Sub OnBackColorChanged(e As EventArgs)
        MyBase.OnBackColorChanged(e)
        Button.BackColor = BackColor

    End Sub
    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)

        If e.KeyCode = Keys.Enter Then

            EvaluationCalendar.Visible = True
            ControlContainer.ShowDropDown()
            e.SuppressKeyPress = True
        End If
    End Sub

    Protected Overrides Sub OnEnter(e As EventArgs)
        MyBase.OnEnter(e)
        BeginInvoke(CType(Sub()
                              SetMaskedTextBoxSelectAll(CType(Me, MaskedTextBox))
                          End Sub, Action))
    End Sub

    Private Sub SetMaskedTextBoxSelectAll(ByVal TextBox As MaskedTextBox)
        TextBox.SelectAll()
    End Sub

    Protected Overrides Sub OnLeave(e As EventArgs)
        MyBase.OnLeave(e)
        If IsDate(Text) Then Text = CDate(Text).ToString("dd/MM/yyyy")
    End Sub

    Private Sub EvaluationCalendar_DateSelected(sender As Object, e As DateRangeEventArgs) Handles EvaluationCalendar.DateSelected
        ControlContainer.CloseDropDown()
    End Sub
    Private Sub CcoInitialDelivery_Closed(sender As Object) Handles ControlContainer.Closed
        Text = EvaluationCalendar.SelectionStart
    End Sub
    Private Sub CcoInitialDelivery_Dropped(sender As Object) Handles ControlContainer.Dropped
        If Not IsDate(Text) Then
            EvaluationCalendar.SetDate(Today)
        Else
            EvaluationCalendar.SetDate(Text)
        End If
    End Sub
    Private Sub BtnEvaluationDate_Click(sender As Object, e As EventArgs) Handles Button.Click
        EvaluationCalendar.Visible = True
    End Sub

    <DllImport("User32", SetLastError:=True)>
    Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As IntPtr) As Integer
    End Function

    Protected Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        MyBase.OnHandleCreated(e)
        If DesignMode AndAlso Site IsNot Nothing Then
            _DesignerHost = TryCast(Site.GetService(GetType(IDesignerHost)), IDesignerHost)
            If _DesignerHost IsNot Nothing Then
                Dim designer = CType(_DesignerHost.GetDesigner(Me), ControlDesigner)
                If designer IsNot Nothing Then
                    designer.ActionLists.Clear()
                    designer.ActionLists.Add(New DateBoxActionList(designer))
                End If
            End If
        End If
    End Sub
    Private Class DateBoxActionList
        Inherits DesignerActionList
        Public Sub New(ByVal Designer As ControlDesigner)
            MyBase.New(Designer.Component)
        End Sub
        Public Overrides Function GetSortedActionItems() As DesignerActionItemCollection
            Dim Items = New DesignerActionItemCollection
            Return Items
        End Function
    End Class
End Class
