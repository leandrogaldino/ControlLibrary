Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Windows.Forms.Design
Public Class DecimalBox
    Inherits TextBox
#Region "ENUMS"
    Public Enum DecimalTextBoxBorderStyles
        Custom
        FixedSingle
        Fixed3D
        None
    End Enum
#End Region
#Region "FIELDS"
    Private _DesignerHost As IDesignerHost
    Private _BorderStyle As DecimalTextBoxBorderStyles = DecimalTextBoxBorderStyles.Custom
    Private _BorderColorDefault As Color = SystemColors.WindowFrame
    Private _BorderColorFocused As Color = SystemColors.HotTrack
    Private _DecimalOnly As Boolean = True
    Private _DecimalPlaces As Integer = 2
    Private _DecimalValue As Decimal = 0
    Private ReadOnly _DecimalList As New List(Of String) From {
        "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", ",", ".", "+", "-"
    }
    Private _SuspendValueChange As Boolean
#End Region
#Region "PROPERTIES"

#Region "NEW PROPERTIES"
    ''' <summary>
    ''' Especifica se o controle deve aceitar somente números.
    ''' </summary>
    <Category("Comportamento")>
    <Description("Especifica se o controle deve aceitar somente números.")>
    Public Property DecimalOnly As Boolean
        Get
            Return _DecimalOnly
        End Get
        Set(value As Boolean)
            _SuspendValueChange = True
            _DecimalOnly = value
            If value Then
                TextAlign = HorizontalAlignment.Right
                If IsNumeric(Text) Then
                    _DecimalValue = CDec(Text)
                    MyBase.Text = FormatNumber(Text, DecimalPlaces, TriState.True)
                Else
                    _DecimalValue = 0
                    If Not MyBase.Text = Nothing Then
                        MyBase.Text = FormatNumber(0, DecimalPlaces, TriState.True)
                    End If

                End If
            Else
                TextAlign = HorizontalAlignment.Left
                _DecimalValue = 0
                'MyBase.Text = Nothing
            End If
            _SuspendValueChange = False
        End Set
    End Property
    ''' <summary>
    ''' Especifica a quantidade de casas decimais a serem mostradas no controle caso a propriedade DecimalOnly seja verdadeira (A propriedade DecimalValue guarda todas as casas decimais).
    ''' </summary>
    <Category("Comportamento")>
    <Description("Especifica a quantidade de casas decimais a serem mostradas no controle caso a propriedade DecimalOnly seja verdadeira (A propriedade DecimalValue guarda todas as casas decimais).")>
    Public Property DecimalPlaces As Integer
        Get
            Return _DecimalPlaces
        End Get
        Set(value As Integer)
            _SuspendValueChange = True
            If value > 99 Then value = 99
            _DecimalPlaces = value
            If DecimalOnly Then
                If Not MyBase.Text = Nothing Then
                    MyBase.Text = FormatNumber(DecimalValue, value, TriState.True)
                End If

            End If
            _SuspendValueChange = False
        End Set
    End Property
    ''' <summary>
    ''' Retorna o valor armazenado no controle com todas as casas decimais se a propriedade DecimalOnly for verdadeira.
    ''' </summary>
    <Category("Aparência")>
    <Description("Retorna o valor armazenado no controle com todas as casas decimais se a propriedade DecimalOnly for verdadeira.")>
    Public ReadOnly Property DecimalValue As Decimal
        Get
            Return _DecimalValue
        End Get
    End Property
#End Region
#Region "OVERRIDED PROPERTIES"
    Public Overrides Property Multiline As Boolean
        Get
            Return MyBase.Multiline
        End Get
        Set(value As Boolean)
            If value Then
                DecimalOnly = False
            End If
            MyBase.Multiline = value
        End Set
    End Property




    <RefreshProperties(RefreshProperties.All)>
    Public Overrides Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(value As String)
            If DesignMode Then
                If DecimalOnly Then
                    If IsNumeric(value) Then
                        If Not _SuspendValueChange Then _DecimalValue = CDec(value)
                        MyBase.Text = FormatNumber(value, DecimalPlaces, TriState.True)
                    Else
                        _DecimalValue = 0
                        'MyBase.Text = FormatNumber(0, DecimalPlaces, TriState.True)
                        MyBase.Text = Nothing
                    End If
                Else
                    _DecimalValue = 0
                    MyBase.Text = value
                End If
            Else
                If DecimalOnly Then
                    If IsNumeric(value) Then
                        _DecimalValue = CDec(value)
                        MyBase.Text = FormatNumber(value, DecimalPlaces, TriState.True)
                    End If
                Else
                    If value = Nothing Then
                        _DecimalValue = 0
                        MyBase.Text = Nothing
                    Else
                        _DecimalValue = 0
                        MyBase.Text = value
                    End If


                End If
            End If
        End Set
    End Property
#End Region
#End Region
#Region "PUBLIC SUBS"
    Public Sub New()
        TextAlign = HorizontalAlignment.Right
    End Sub
#End Region
#Region "OVERRIDED SUBS"
    Protected Overrides Sub OnTextChanged(e As EventArgs)

        If Not _SuspendValueChange And Not DesignMode Then
            If DecimalOnly And IsNumeric(Text) Then
                _DecimalValue = CDec(Text)
            Else
                _DecimalValue = FormatNumber(0, DecimalPlaces, TriState.True)
            End If
        End If


        MyBase.OnTextChanged(e)
    End Sub
    <DebuggerStepThrough>
    Protected Overrides Sub OnLostFocus(e As EventArgs)
        MyBase.OnLostFocus(e)
        _SuspendValueChange = True
        If DecimalOnly Then
            If Not IsNumeric(Text) Then
                _DecimalValue = 0
                If Not MyBase.Text = Nothing Then
                    MyBase.Text = FormatNumber(0, DecimalPlaces, True)
                End If

            Else
                If Text <> FormatNumber(_DecimalValue, DecimalPlaces, True) Then _DecimalValue = CDec(Text)
                MyBase.Text = FormatNumber(Text, DecimalPlaces, TriState.True)
            End If
        Else
            _DecimalValue = 0
        End If
        _SuspendValueChange = False
    End Sub

    Protected Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        MyBase.OnHandleCreated(e)
        If DesignMode AndAlso Site IsNot Nothing Then
            _DesignerHost = TryCast(Site.GetService(GetType(IDesignerHost)), IDesignerHost)
            If _DesignerHost IsNot Nothing Then
                Dim designer = CType(_DesignerHost.GetDesigner(Me), ControlDesigner)
                If designer IsNot Nothing Then
                    Dim actions = designer.ActionLists(0)
                    designer.ActionLists.Clear()
                    designer.ActionLists.Add(New DecimalTextBoxControlDesignerActionList(designer, actions))
                End If
            End If
        End If
    End Sub
    Protected Overrides Sub OnKeyPress(e As KeyPressEventArgs)
        MyBase.OnKeyPress(e)
        If DecimalOnly And Not Char.IsControl(e.KeyChar) Then
            If Not _DecimalList.Contains(e.KeyChar) Then e.Handled = True
        End If
    End Sub
#End Region
#Region "INTERNAL CLASSES"
    Private Class DecimalTextBoxControlDesignerActionList
        Inherits DesignerActionList
        Private Control As DecimalBox
        Private Designer As ControlDesigner
        Private ActionList As DesignerActionList
        Public Sub New(ByVal Designer As ControlDesigner, ByVal ActionList As DesignerActionList)
            MyBase.New(Designer.Component)
            Me.Designer = Designer
            Me.ActionList = ActionList
            Control = CType(Designer.Control, DecimalBox)
        End Sub
        Public Overrides Function GetSortedActionItems() As DesignerActionItemCollection
            Dim Items = New DesignerActionItemCollection From {
                New DesignerActionPropertyItem("DecimalOnly", "Decimal Only", "Comportamento", "Especifica se o controle deve aceitar somente números."),
                New DesignerActionPropertyItem("DecimalPlaces", "Decimal Places", "Comportamento", "Especifica a quantidade de casas decimais a serem mostradas no controle caso a propriedade DecimalOnly seja verdadeira (A propriedade DecimalValue guarda todas as casas decimais).")
            }
            Return Items
        End Function

        Public Property DecimalOnly As Boolean
            Get
                Return Control.DecimalOnly
            End Get
            Set(ByVal value As Boolean)
                TypeDescriptor.GetProperties(Component)("DecimalOnly").SetValue(Component, value)
            End Set
        End Property
        Public Property DecimalPlaces As Integer
            Get
                Return Control.DecimalPlaces
            End Get
            Set(ByVal value As Integer)
                TypeDescriptor.GetProperties(Component)("DecimalPlaces").SetValue(Component, value)
            End Set
        End Property
    End Class
#End Region
End Class
