Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Design
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports System.Windows.Forms.Design

<DefaultEvent("ColorChanged")>
<DefaultProperty("Color")>
Public Class ColorPicker
    Inherits Control

    Public Event ColorChanged As EventHandler

    Public Sub New()
        MyBase.New()
        ShowEditor()
    End Sub

    Private service As ColorEditorService
    Protected editor As ColorEditor
    Protected colorUI As Control
    Protected tab As TabControl
    Protected palette As Control
    Private _colorUIWnd As ColorUIWnd
    Private _Color As Color = Color.White
    Private _CustomColors As Color()
    Private origColors As Color()
    Private _AllowTabOut As Boolean = True
    Private siblings As Control()
    Private _TabOrderPos As Integer = -1
    Protected Const TAB_PALETTE As String = "palette"
    Protected Const TAB_COMMON As String = "common"
    Protected Const TAB_SYSTEM As String = "system"
    Protected Const EXTRASIZE As Integer = 2

    <DefaultValue(GetType(Color), "White")>
    Public Property Color As Color
        Get
            Return _Color
        End Get
        Set(ByVal value As Color)
            setEditorColor(value)
            OnColorChanged(value)
        End Set
    End Property

    <DefaultValue(GetType(Color), Nothing)>
    Public Property CustomColors As Color()
        Get
            Return _CustomColors
        End Get
        Set(ByVal value As Color())
            ShouldSerializeCustomColors = False

            If value IsNot Nothing Then
                Debug.Assert(value.Length = 16)
                origColors = CType(value.Clone(), Color())
            Else
                origColors = Nothing
            End If

            _CustomColors = value
            setEditorCustomColors()
        End Set
    End Property

    Public Shared ReadOnly Property DefaultCustomColors As Color()
        Get
            Dim initialColors As Color() = New Color(15) {}

            For i As Integer = 0 To initialColors.Length - 1
                initialColors(i) = Color.White
            Next

            Return initialColors
        End Get
    End Property

    <Browsable(False)>
    <DefaultValue(False)>
    Public Property ShouldSerializeCustomColors As Boolean


    <DefaultValue(True)>
    Public Property AllowTabOut As Boolean
        Get
            Return _AllowTabOut
        End Get
        Set(ByVal value As Boolean)
            _AllowTabOut = value
            _TabOrderPos = -1
            siblings = Nothing
        End Set
    End Property

    Public Sub CloseEditor()
        closeEditorInternal()
        service = Nothing
        colorUI = Nothing
        palette = Nothing
        tab = Nothing
        editor = Nothing
    End Sub

    Public Sub ShowEditor()
        ShouldSerializeCustomColors = False

        If service Is Nothing Then
            service = New ColorEditorService()
            AddHandler service.ColorUIAvailable, AddressOf service_ColorUIAvailable
            AddHandler service.ColorChanged, AddressOf service_ColorChanged
        End If

        If editor Is Nothing Then
            editor = New ColorEditor()
        End If

        If colorUI Is Nothing Then
            editor.EditValue(service, _Color)

            If Not _Color.IsKnownColor Then
                setPaletteColor(_Color)
            End If

            restoreEditorServiceReference()
        End If
    End Sub

    Public Sub PaintValue(ByVal color As Color, ByVal canvas As Graphics, ByVal rectangle As Rectangle)
        If editor IsNot Nothing Then
            editor.PaintValue(color, canvas, rectangle)
        End If
    End Sub

    Protected Sub setEditorColor(ByVal newColor As Color)
        If colorUI IsNot Nothing AndAlso newColor <> _Color Then
            _colorUIWnd.PreventSizing = True
            editor.EditValue(service, newColor)
            restoreEditorServiceReference()
            _colorUIWnd.PreventSizing = False

            If Not newColor.IsKnownColor Then
                setPaletteColor(newColor)
            End If

            resetControls()
        End If
    End Sub

    Protected Sub setPaletteColor(ByVal newColor As Color)
        If palette IsNot Nothing Then
            Dim t As Type = palette.[GetType]()
            Dim pInfo As PropertyInfo = t.GetProperty("SelectedColor")
            pInfo.SetValue(palette, newColor, Nothing)
        End If
    End Sub

    Protected Sub setEditorCustomColors()
        If colorUI IsNot Nothing AndAlso _CustomColors IsNot Nothing AndAlso _CustomColors.Length = 16 Then
            Dim t As Type = palette.[GetType]()
            Dim fInfo As FieldInfo = t.GetField("customColors", BindingFlags.NonPublic Or BindingFlags.Instance)
            fInfo.SetValue(palette, Me._CustomColors)
            palette.Refresh()
        End If
    End Sub

    Private Sub service_ColorChanged(ByVal sender As Object, ByVal e As EventArgs)
        If colorUI Is Nothing Then Return
        Dim pageName As String = tab.SelectedTab.Name
        Dim value As Object = Nothing

        Select Case pageName
            Case TAB_COMMON, TAB_SYSTEM
                Dim lb As ListBox = CType(tab.SelectedTab.Controls(0), ListBox)
                value = lb.SelectedItem
            Case TAB_PALETTE
                Dim t As Type = colorUI.[GetType]()
                Dim pInfo As PropertyInfo = t.GetProperty("Value")
                value = pInfo.GetValue(colorUI, Nothing)

                If value IsNot Nothing AndAlso CType(value, Color) <> Color.White Then
                    compareCustomColors()
                End If
        End Select

        If value IsNot Nothing Then
            resetControls()
            OnColorChanged(CType(value, Color))
        End If
    End Sub

    Private Sub compareCustomColors()
        If _CustomColors IsNot Nothing AndAlso origColors IsNot Nothing Then

            For i As Integer = 0 To _CustomColors.Length - 1

                If origColors(i) <> _CustomColors(i) Then
                    ShouldSerializeCustomColors = True
                    Exit For
                End If
            Next
        End If
    End Sub

    Protected Overridable Sub OnColorChanged(ByVal newColor As Color)
        If newColor <> _Color Then
            _Color = newColor
            RaiseEvent ColorChanged(Me, EventArgs.Empty)
        End If
    End Sub

    Private Sub resetControls()
        Dim pageName As String = tab.SelectedTab.Name
        Dim lb As ListBox

        Select Case pageName
            Case TAB_COMMON, TAB_SYSTEM
                lb = CType(tab.TabPages(If(pageName = TAB_COMMON, TAB_SYSTEM, TAB_COMMON)).Controls(0), ListBox)
                lb.SelectedItem = Nothing
                setPaletteColor(Color.Empty)
            Case TAB_PALETTE
                lb = CType(tab.TabPages(TAB_COMMON).Controls(0), ListBox)
                lb.SelectedItem = Nothing
                lb = CType(tab.TabPages(TAB_SYSTEM).Controls(0), ListBox)
                lb.SelectedItem = Nothing
        End Select
    End Sub

    Private Sub restoreEditorServiceReference()
        If colorUI IsNot Nothing AndAlso service IsNot Nothing Then
            Dim t As Type = colorUI.[GetType]()
            Dim fInfo As FieldInfo = t.GetField("edSvc", BindingFlags.NonPublic Or BindingFlags.Instance)
            fInfo.SetValue(colorUI, service)
        End If
    End Sub

    Private Sub service_ColorUIAvailable(ByVal sender As Object, ByVal e As ColorPicker.EditorServiceEventArgs)
        If e.ColorUI IsNot Nothing Then

            If colorUI Is Nothing Then
                addColorUI(e.ColorUI)
                setEditorCustomColors()
                _colorUIWnd = New ColorUIWnd()
                _colorUIWnd.AssignHandle(colorUI.Handle)
            End If
        Else
            RemoveHandler service.ColorUIAvailable, AddressOf service_ColorUIAvailable
            RemoveHandler service.ColorChanged, AddressOf service_ColorChanged
            service = Nothing

            If Me.Controls.Contains(colorUI) Then
                Me.Controls.Remove(colorUI)
            End If



            colorUI = Nothing
            palette = Nothing
            tab = Nothing
            editor = Nothing
            _colorUIWnd = Nothing
        End If
    End Sub

    Protected Overridable Sub addColorUI(ByVal colorUI As Control)
        Me.colorUI = colorUI
        Me.tab = CType(colorUI.Controls(0), TabControl)
        Me.palette = tab.TabPages(0).Controls(0)
        tab.TabPages(0).Name = TAB_PALETTE
        tab.TabPages(1).Name = TAB_COMMON
        tab.TabPages(2).Name = TAB_SYSTEM


        RemoveHandler tab.Deselecting, AddressOf tab_Deselecting
        AddHandler tab.Deselecting, AddressOf tab_Deselecting

        colorUI.Font = Me.Font
        colorUI.Location = New Point(1, 1)
        colorUI.Size = Me.ClientSize

        RemoveHandler palette.MouseUp, AddressOf palette_MouseUp
        AddHandler palette.MouseUp, AddressOf palette_MouseUp





        Me.Controls.Add(colorUI)
    End Sub

    Protected Overrides Sub OnGotFocus(ByVal e As EventArgs)
        MyBase.OnGotFocus(e)

        If colorUI IsNot Nothing AndAlso Not colorUI.ContainsFocus Then
            colorUI.Focus()
        End If
    End Sub

    Private Sub tab_Deselecting(ByVal sender As Object, ByVal e As TabControlCancelEventArgs)
        If _AllowTabOut AndAlso GetAsyncKeyState(Keys.Tab) <> 0 Then

            If (Control.ModifierKeys And (Keys.Alt Or Keys.Control)) = Keys.None Then

                If (Control.ModifierKeys And Keys.Shift) = Keys.None Then

                    If e.TabPageIndex = tab.TabPages.Count - 1 Then
                        e.Cancel = focusNextControl(True)
                    End If
                Else

                    If e.TabPageIndex = 0 Then
                        e.Cancel = focusNextControl(False)
                    End If
                End If
            End If
        End If
    End Sub

    Private Function focusNextControl(ByVal forward As Boolean) As Boolean
        Try
            Dim pos As Integer = TabOrderPos

            If pos > -1 Then

                If forward Then

                    If System.Threading.Interlocked.Increment(pos) >= siblings.Length Then
                        pos = 0
                    End If
                Else

                    If System.Threading.Interlocked.Decrement(pos) <= 0 Then
                        pos = siblings.Length - 1
                    End If
                End If

                Dim ctrlToSelect As Control = siblings(pos)
                ctrlToSelect.Focus()
                Return ctrlToSelect.ContainsFocus
            End If

        Catch
        End Try

        Return False
    End Function

    Private ReadOnly Property TabOrderPos As Integer
        Get

            If _TabOrderPos <> -1 Then
                Return _TabOrderPos
            End If

            _TabOrderPos = -2
            siblings = Nothing
            Dim parent As Control = Me.Parent

            If parent IsNot Nothing AndAlso parent.Controls.Count > 1 Then
                siblings = New Control(parent.Controls.Count - 1) {}
                parent.Controls.CopyTo(siblings, 0)
                Dim tabIndices As Integer() = New Integer(parent.Controls.Count - 1) {}

                For i As Integer = 0 To siblings.Length - 1
                    tabIndices(i) = siblings(i).TabIndex
                Next

                Array.Sort(tabIndices, siblings)
                _TabOrderPos = Array.IndexOf(siblings, Me)
            End If

            Return _TabOrderPos
        End Get
    End Property

    Private Sub palette_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
        If Not palette.Focused Then
            palette.Focus()
        End If
    End Sub



    Protected Overridable Sub closeEditorInternal()
        If service IsNot Nothing Then
            service.CloseDropDownInternal()
        End If

        If colorUI IsNot Nothing Then
            sendKeyDown(tab.SelectedTab.Controls(0), Keys.[Return])
            Debug.Assert(service Is Nothing)
            Debug.Assert(colorUI Is Nothing)
            Debug.Assert(editor Is Nothing)
        End If
    End Sub

    Protected Sub sendKeyDown(ByVal control As Control, ByVal key As Keys)
        Const WM_KEYDOWN As Integer = &H100

        If control IsNot Nothing Then
            SendMessage(control.Handle, WM_KEYDOWN, New IntPtr(CInt(key)), IntPtr.Zero)
        End If
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            CloseEditor()
        End If

        MyBase.Dispose(disposing)
    End Sub

    Protected Overrides Sub OnFontChanged(ByVal e As EventArgs)
        MyBase.OnFontChanged(e)

        If colorUI IsNot Nothing Then
            colorUI.Font = Me.Font
        End If
    End Sub



    Protected Overrides Sub OnClientSizeChanged(ByVal e As EventArgs)
        If colorUI Is Nothing AndAlso Me.ClientSize <> Me.DefaultMinimumSize Then
            Me.ClientSize = Me.DefaultMinimumSize
        End If

        MyBase.OnClientSizeChanged(e)
    End Sub

    Public Overrides Property MinimumSize As Size
        Get
            Return Me.DefaultMinimumSize
        End Get
        Set(ByVal value As Size)
        End Set
    End Property

    Protected Overrides ReadOnly Property DefaultSize As Size
        Get
            Return Me.DefaultMinimumSize
        End Get
    End Property

    Protected Overrides ReadOnly Property DefaultMinimumSize As Size
        Get
            Dim extraSize As Integer = extraSize + 2 * CInt(0)
            Return New Size(202 + extraSize, 220 + extraSize)
        End Get
    End Property









    Private Class ColorUIWnd
        Inherits NativeWindow

        <StructLayout(LayoutKind.Sequential)>
        Private Structure WINDOWPOS
            Public hwnd As IntPtr
            Public hwndInsertAfter As IntPtr
            Public x As Integer
            Public y As Integer
            Public cx As Integer
            Public cy As Integer
            Public flags As Integer
        End Structure

        Private Const WM_WINDOWPOSCHANGING As Integer = &H46
        Private Const SWP_NOSIZE As Integer = &H1
        Public PreventSizing As Boolean

        Protected Overrides Sub WndProc(ByRef m As Message)
            If PreventSizing AndAlso m.Msg = WM_WINDOWPOSCHANGING Then
                Dim wp As WINDOWPOS = CType(Marshal.PtrToStructure(m.LParam, GetType(WINDOWPOS)), WINDOWPOS)
                wp.flags = wp.flags Or SWP_NOSIZE
                Marshal.StructureToPtr(wp, m.LParam, False)
            End If

            MyBase.WndProc(m)
        End Sub
    End Class





















    Private Class ColorEditorService
        Implements IServiceProvider, IWindowsFormsEditorService

        Public Event ColorUIAvailable As EventHandler(Of EditorServiceEventArgs)
        Public Event ColorChanged As EventHandler
        Private closeEditor As Boolean

        Public Sub CloseDropDownInternal()
            closeEditor = True
        End Sub







        Private Function IServiceProvider_GetService(serviceType As Type) As Object Implements IServiceProvider.GetService
            If serviceType = GetType(IWindowsFormsEditorService) Then
                Return Me
            End If

            Return Nothing
        End Function

        Private Sub IWindowsFormsEditorService_CloseDropDown() Implements IWindowsFormsEditorService.CloseDropDown
            If Not closeEditor Then
                RaiseEvent ColorChanged(Me, EventArgs.Empty)
            Else
                RaiseEvent ColorUIAvailable(Me, New EditorServiceEventArgs(Nothing))
            End If
        End Sub

        Private Sub IWindowsFormsEditorService_DropDownControl(control As Control) Implements IWindowsFormsEditorService.DropDownControl
            closeEditor = (control Is Nothing)
            RaiseEvent ColorUIAvailable(Me, New EditorServiceEventArgs(control))
        End Sub

        Private Function IWindowsFormsEditorService_ShowDialog(dialog As Form) As DialogResult Implements IWindowsFormsEditorService.ShowDialog
            Throw New Exception("The method or operation is not implemented.")
        End Function
    End Class

    Private Class EditorServiceEventArgs
        Inherits EventArgs

        Public Sub New(ByVal colorUI As Control)
            _ColorUI = colorUI
        End Sub

        Private _ColorUI As Control

        Public ReadOnly Property ColorUI As Control
            Get
                Return _ColorUI
            End Get
        End Property
    End Class

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Protected Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function
    <DllImport("user32.dll")>
    Protected Shared Function GetAsyncKeyState(ByVal vKey As Keys) As Short
    End Function
End Class