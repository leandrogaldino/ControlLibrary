﻿Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Drawing
Public Class ControlContainer
    Inherits Component

#Region "ENUMS"
    Public Enum ControlContainerDropDownState
        Closed
        Closing
        Dropping
        Dropped
    End Enum
#End Region
#Region "EVENTS"
    <Category("Propriedade Alterada")>
    Public Event DropStateChanged(sender As Object)
    <Category("Propriedade Alterada")>
    Public Event Dropping(sender As Object)
    <Category("Propriedade Alterada")>
    Public Event Dropped(sender As Object)
    <Category("Propriedade Alterada")>
    Public Event Closing(sender As Object)
    <Category("Propriedade Alterada")>
    Public Event Closed(sender As Object)
#End Region
#Region "FIELDS"
    Private _DropDownContainer As DropDownContainer
    Private _DropDownControl As Control
    Private _ClosedWhileInControl As Boolean
    Private _DropState As ControlContainerDropDownState
    Private _HostControl As Control
#End Region
#Region "CONSTRUCTOR"
    Public Sub New()
        InitializeDropDown(_DropDownControl)
    End Sub
#End Region
#Region "PROPERTIES"
    Public Property HostControl As Control
        Get
            Return _HostControl
        End Get
        Set(value As Control)
            _HostControl = value
            If _HostControl IsNot Nothing Then
                RemoveHandler _HostControl.Click, AddressOf HostControl_Click
                AddHandler _HostControl.Click, AddressOf HostControl_Click
            End If
        End Set
    End Property
    Public Overridable ReadOnly Property CanDrop As Boolean
        Get
            If _DropDownContainer IsNot Nothing Then Return False
            If _DropDownContainer Is Nothing AndAlso _ClosedWhileInControl Then
                _ClosedWhileInControl = False
                Return False
            End If
            Return Not _ClosedWhileInControl
        End Get
    End Property
    Public ReadOnly Property DropState As ControlContainerDropDownState
        Get
            Return _DropState
        End Get
    End Property
    Public Property DropDownControl As Control
        Get
            Return _DropDownControl
        End Get
        Set(value As Control)
            _DropDownControl = value
            InitializeDropDown(_DropDownControl)
        End Set
    End Property
    Public Property DropDownEnabled As Boolean = True
    Public Property DropDownBorderColor As Color = SystemColors.HotTrack
#End Region
#Region "PRIVATE SUBS"
    Private Sub HostControl_Click(sender As Object, e As EventArgs)
        If DropDownEnabled Then ShowDropDown()
    End Sub
    Private Sub InitializeDropDown(ByVal DropDownControl As Control)
        _DropState = ControlContainerDropDownState.Closed
        If HostControl IsNot Nothing AndAlso HostControl.Controls.Contains(DropDownControl) Then
            HostControl.Controls.Remove(DropDownControl)
        End If
        _DropDownControl = DropDownControl
    End Sub
    Private Sub Parent_Move(ByVal sender As Object, ByVal e As EventArgs)
        CloseDropDown()
    End Sub
    Private Sub DropContainer_Closed(ByVal sender As Object, ByVal e As FormClosedEventArgs)
        RaiseEvent Closing(Me)
        If Not _DropDownContainer.IsDisposed Then
            RemoveHandler _DropDownContainer.DropStateChange, AddressOf DropContainer_DropStateChange
            RemoveHandler _DropDownContainer.FormClosed, AddressOf DropContainer_Closed
            RemoveHandler HostControl.Parent.Move, AddressOf Parent_Move
            _DropDownContainer.Dispose()
        End If
        _DropDownContainer = Nothing
        _ClosedWhileInControl = (HostControl.RectangleToScreen(HostControl.ClientRectangle).Contains(Cursor.Position))
        _DropState = ControlContainerDropDownState.Closed
        HostControl.Invalidate()
        RaiseEvent Closed(Me)
    End Sub
    Private Sub DropContainer_DropStateChange(ByVal state As ControlContainerDropDownState)
        _DropState = state
    End Sub
#End Region
#Region "PRIVATE FUNCTIONS"
    Private Function GetDropDownBounds() As Rectangle
        Dim inflatedDropSize As Size = New Size(_DropDownControl.Width + 2, _DropDownControl.Height + 2)
        Dim screenBounds As Rectangle = New Rectangle(HostControl.Parent.PointToScreen(New Point(HostControl.Bounds.X, HostControl.Bounds.Bottom)), inflatedDropSize)
        Dim workingArea As Rectangle = Screen.GetWorkingArea(screenBounds)
        If screenBounds.X < workingArea.X Then screenBounds.X = workingArea.X
        If screenBounds.Y < workingArea.Y Then screenBounds.Y = workingArea.Y
        If screenBounds.Right > workingArea.Right AndAlso workingArea.Width > screenBounds.Width Then screenBounds.X = workingArea.Right - screenBounds.Width
        If screenBounds.Bottom > workingArea.Bottom AndAlso workingArea.Height > screenBounds.Height Then screenBounds.Y = workingArea.Bottom - screenBounds.Height
        Return screenBounds
    End Function
#End Region
#Region "PUBLIC SUBS"
    Public Sub ShowDropDown()
        If HostControl Is Nothing Then Throw New Exception("O controle hospedeiro não foi definido.")
        If DropDownControl Is Nothing Then Throw New Exception("O controle a ser hospedado não foi definido.")
        If Not CanDrop Then Return
        RaiseEvent Dropping(Me)
        _DropDownContainer = New DropDownContainer(_DropDownControl, DropDownBorderColor) With {
            .Bounds = GetDropDownBounds()
        }
        AddHandler _DropDownContainer.DropStateChange, New DropDownContainer.DropWindowArgs(AddressOf DropContainer_DropStateChange)
        AddHandler _DropDownContainer.FormClosed, AddressOf DropContainer_Closed
        AddHandler HostControl.Parent.Move, New EventHandler(AddressOf Parent_Move)
        _DropState = ControlContainerDropDownState.Dropping
        If HostControl.GetType = GetType(Button) Then
            If CType(HostControl, Button).FlatStyle = FlatStyle.Standard Or CType(HostControl, Button).FlatStyle = FlatStyle.System Then
                _DropDownContainer.Top -= 2
                _DropDownContainer.Left += 1
            Else
                _DropDownContainer.Top -= 1
                _DropDownContainer.Left += 0
            End If
        End If

        _DropDownContainer.Show(HostControl)
        _DropState = ControlContainerDropDownState.Dropped
        HostControl.Invalidate()
        RaiseEvent Dropped(Me)
    End Sub
    Public Sub CloseDropDown()
        If _DropDownContainer IsNot Nothing Then
            _DropState = ControlContainerDropDownState.Closing
            _DropDownContainer.Close()
        End If
    End Sub
#End Region
#Region "INTERNAL CLASSES"
    Friend NotInheritable Class DropDownContainer
        Inherits Form
        Implements IMessageFilter
#Region "EVENTS"
        Public Event DropStateChange As DropWindowArgs
#End Region
#Region "FIELDS"
        Private Const WM_LBUTTONDOWN As Integer = &H201
        Private Const WM_RBUTTONDOWN As Integer = &H204
        Private Const WM_MBUTTONDOWN As Integer = &H207
        Private Const WM_NCLBUTTONDOWN As Integer = &HA1
        Private Const WM_NCRBUTTONDOWN As Integer = &HA4
        Private Const WM_NCMBUTTONDOWN As Integer = &HA7
        Private Const WM_MOUSEHOVER As Integer = &H2A1
        Private Const WM_NCMOUSEHOVER As Integer = &H2A0
        Private _DropDownBorderColor As Color
        Private _DropDownItem As Control
        Private _HostControl As Control
#End Region
#Region "CONSTRUCTOR"
        Public Sub New(ByVal dropDownItem As Control, ByVal DropDownBorderColor As Color)
            Dim DropDownMinSize As New Size(136, 39)
            _DropDownBorderColor = DropDownBorderColor
            _DropDownItem = dropDownItem
            FormBorderStyle = FormBorderStyle.None
            dropDownItem.Location = New Point(1, 1)
            Controls.Add(dropDownItem)
            StartPosition = FormStartPosition.Manual
            ShowInTaskbar = False
            KeyPreview = True
            Size = New Size(200, 200)
            If dropDownItem.Width < DropDownMinSize.Width Or dropDownItem.Height < DropDownMinSize.Height Then
                dropDownItem.Left = (DropDownMinSize.Width - dropDownItem.Width) / 2
                dropDownItem.Top = (DropDownMinSize.Height - dropDownItem.Height) / 2
            End If
            Application.AddMessageFilter(Me)
        End Sub
#End Region
#Region "OVERRIDED SUBS"
        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            ControlPaint.DrawBorder(Me.CreateGraphics, ClientRectangle, _DropDownBorderColor, ButtonBorderStyle.Solid)
        End Sub
        Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
            MyBase.OnKeyDown(e)
            If e.KeyCode = Keys.Escape Then
                Close()
            End If
        End Sub
        Protected Overrides Sub OnClosing(ByVal e As CancelEventArgs)
            Application.RemoveMessageFilter(Me)
            Controls.RemoveAt(0)
            MyBase.OnClosing(e)
        End Sub
#End Region
#Region "PRIVATE FUNCTIONS"
        Private Function IMessageFilter_PreFilterMessage(ByRef m As Message) As Boolean Implements IMessageFilter.PreFilterMessage
            Dim cursorPos As Point = Cursor.Position
            If Visible AndAlso (ActiveForm Is Nothing OrElse Not ActiveForm.Equals(Me)) Then
                Close()
            End If
            Select Case m.Msg
                Case WM_LBUTTONDOWN, WM_RBUTTONDOWN, WM_MBUTTONDOWN, WM_NCLBUTTONDOWN, WM_NCRBUTTONDOWN, WM_NCMBUTTONDOWN
                    If _DropDownItem.Parent IsNot Nothing AndAlso Not Bounds.Contains(cursorPos) Then
                        If Not _DropDownItem.Bounds.Contains(_DropDownItem.Parent.PointToClient(cursorPos)) Then
                            Application.RemoveMessageFilter(Me)
                            Close()
                        End If
                    End If
            End Select
            Return False
        End Function
#End Region
#Region "PUIBLIC SUBS"
        Public Delegate Sub DropWindowArgs(ByVal state As ControlContainerDropDownState)
#End Region
    End Class
#End Region
End Class

