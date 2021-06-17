Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Runtime.InteropServices
Imports System.Windows.Forms.Design


Public Class DateBox


    Inherits MaskedTextBox
    Private _DesignerHost As IDesignerHost
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    <EditorBrowsable(EditorBrowsableState.Never), Browsable(False)>
    Overloads ReadOnly Property Mask As String
        Get
            Return MyBase.Mask
        End Get
    End Property

    Private _Button As Button
    Public Sub New()
        MyBase.Mask = "00/00/0000"

        _Button = New Button
        _Button.Size = New Size(25, ClientSize.Height + 2)
        _Button.Location = New Point(ClientSize.Width - _Button.Width + 1, -1)
        _Button.Cursor = Cursors.[Default]
        _Button.FlatStyle = FlatStyle.Flat
        _Button.FlatAppearance.BorderSize = 0
        _Button.BackgroundImage = Image.FromFile("C:\Users\faturamento\Documents\GitHub\Evaluation\Evaluation\Resources\Calendar.png")
        _Button.BackgroundImageLayout = ImageLayout.Center
        AddHandler _Button.Click, AddressOf Btn_Click
        Controls.Add(_Button)
        SendMessage(Handle, &HD3, CType(2, IntPtr), CType(_Button.Width << 16, IntPtr))
    End Sub

    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        MyBase.OnSizeChanged(e)
        _Button.Size = New Size(25, ClientSize.Height + 2)
        _Button.Location = New Point(ClientSize.Width - _Button.Width + 1, -1)
        _Button.Cursor = Cursors.[Default]
        _Button.FlatStyle = FlatStyle.Flat
        _Button.FlatAppearance.BorderSize = 0
        _Button.BackgroundImage = Image.FromFile("C:\Users\faturamento\Documents\GitHub\Evaluation\Evaluation\Resources\Calendar.png")
        _Button.BackgroundImageLayout = ImageLayout.Center
    End Sub





    Private Sub Btn_Click(ByVal sender As Object, ByVal e As EventArgs)
        MessageBox.Show("hello world")
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
