Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports ControlLibrary.QueriedBox
#Region "CALENDAR"
Public Class DataGridViewDateTimePickerColumn
    Inherits DataGridViewColumn
    Protected Overrides Sub OnDataGridViewChanged()
        MyBase.OnDataGridViewChanged()
    End Sub
    Public Sub New()
        MyBase.New(New DataGridViewDateTimePickerCell())
    End Sub
    Public Overrides Property CellTemplate As DataGridViewCell
        Get
            Return MyBase.CellTemplate
        End Get
        Set(ByVal value As DataGridViewCell)
            If value IsNot Nothing AndAlso Not value.[GetType]().IsAssignableFrom(GetType(DataGridViewDateTimePickerCell)) Then
                Throw New InvalidCastException("Must be a DataGridViewCalendarCell")
            End If
            MyBase.CellTemplate = value
        End Set
    End Property
End Class
Public Class DataGridViewDateTimePickerCell
    Inherits DataGridViewTextBoxCell
    Public Sub New()
        MyBase.New()
        Me.Style.Format = "d"
    End Sub
    Public Overrides Sub InitializeEditingControl(ByVal rowIndex As Integer, ByVal initialFormattedValue As Object, ByVal dataGridViewCellStyle As DataGridViewCellStyle)
        MyBase.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle)
        Dim ctl As DataGridViewDateTimePickerEditingControl = TryCast(DataGridView.EditingControl, DataGridViewDateTimePickerEditingControl)

        If Value Is Nothing Then
            ctl.Value = CType(Me.DefaultNewRowValue, DateTime)
        Else
            ctl.Value = CType(Me.Value, DateTime)
        End If
    End Sub
    Public Overrides ReadOnly Property EditType As Type
        Get
            Return GetType(DataGridViewDateTimePickerEditingControl)
        End Get
    End Property
    Public Overrides ReadOnly Property ValueType As Type
        Get
            Return GetType(DateTime)
        End Get
    End Property
    Public Overrides ReadOnly Property DefaultNewRowValue As Object
        Get
            Return DateTime.Now
        End Get
    End Property
End Class
Class DataGridViewDateTimePickerEditingControl
    Inherits DateTimePicker
    Implements IDataGridViewEditingControl
    Private _dataGridView As DataGridView
    Private _valueChanged As Boolean = False
    Private _rowIndex As Integer
    Public Sub New()
        Me.Format = DateTimePickerFormat.Short
    End Sub
    Private Property EditingControlDataGridView As DataGridView Implements IDataGridViewEditingControl.EditingControlDataGridView
        Get
            Return _dataGridView
        End Get
        Set(ByVal value As DataGridView)
            _dataGridView = value
        End Set
    End Property
    Private Property EditingControlFormattedValue As Object Implements IDataGridViewEditingControl.EditingControlFormattedValue
        Get
            Return Me.Value.ToShortDateString()
        End Get
        Set(ByVal value As Object)
            If TypeOf value Is String Then
                Try
                    Me.Value = DateTime.Parse(CType(value, String))
                Catch
                    Me.Value = DateTime.Now
                End Try
            End If
        End Set
    End Property
    Private Function GetEditingControlFormattedValue(context As DataGridViewDataErrorContexts) As Object Implements IDataGridViewEditingControl.GetEditingControlFormattedValue
        Return EditingControlFormattedValue
    End Function
    Private ReadOnly Property EditingPanelCursor As Cursor Implements IDataGridViewEditingControl.EditingPanelCursor
        Get
            Return MyBase.Cursor
        End Get
    End Property
    Private Property EditingControlValueChanged As Boolean Implements IDataGridViewEditingControl.EditingControlValueChanged
        Get
            Return _valueChanged
        End Get
        Set(ByVal value As Boolean)
            _valueChanged = value
        End Set
    End Property
    Private ReadOnly Property RepositionEditingControlOnValueChange As Boolean Implements IDataGridViewEditingControl.RepositionEditingControlOnValueChange
        Get
            Return False
        End Get
    End Property
    Private Sub PrepareEditingControlForEdit(selectAll As Boolean) Implements IDataGridViewEditingControl.PrepareEditingControlForEdit
    End Sub
    Private Function EditingControlWantsInputKey(keyData As Keys, dataGridViewWantsInputKey As Boolean) As Boolean Implements IDataGridViewEditingControl.EditingControlWantsInputKey
        Select Case keyData And Keys.KeyCode
            Case Keys.Left, Keys.Up, Keys.Down, Keys.Right, Keys.Home, Keys.[End], Keys.PageDown, Keys.PageUp
                Return True
            Case Else
                Return Not dataGridViewWantsInputKey
        End Select
    End Function
    Private Property EditingControlRowIndex As Integer Implements IDataGridViewEditingControl.EditingControlRowIndex
        Get
            Return _rowIndex
        End Get
        Set(ByVal value As Integer)
            _rowIndex = value
        End Set
    End Property
    Private Sub ApplyCellStyleToEditingControl(dataGridViewCellStyle As DataGridViewCellStyle) Implements IDataGridViewEditingControl.ApplyCellStyleToEditingControl
        Font = dataGridViewCellStyle.Font
        CalendarForeColor = dataGridViewCellStyle.ForeColor
        CalendarMonthBackground = dataGridViewCellStyle.BackColor
    End Sub
    Protected Overrides Sub OnValueChanged(ByVal eventargs As EventArgs)
        _valueChanged = True
        EditingControlDataGridView.NotifyCurrentCellDirty(True)
        MyBase.OnValueChanged(eventargs)
    End Sub
End Class
#End Region
#Region "QUERY"
Public Class DataGridViewQueryColumn
    Inherits DataGridViewColumn
    Public Enum DirectionTypes
        Left
        Right
    End Enum
    Private _CharactersToQuery As Integer = 3
    Private _QueryInterval As Integer = 300
    Private _DropDownBorderColor As Color = SystemColors.HotTrack
    Private _GridBackColor As Color = SystemColors.Window
    Private _GridSelectionBackColor As Color = SystemColors.HotTrack
    Private _GridForeColor As Color = SystemColors.ControlText
    Private _GridSelectionForeColor As Color = SystemColors.Window
    Private _GridHeaderBackColor As Color = SystemColors.Control
    Private _GridHeaderForeColor As Color = SystemColors.ControlText
    Private _LabelBackColor As Color = SystemColors.Window
    Private _LabelForeColor As Color = SystemColors.ControlText
    Private _FreezeColor As Color = Color.Blue
    ''' <summary>
    ''' Define o apelido do campo a ser pesquisado (substitui o nome do campo no título dos resultados).
    ''' </summary>
    <Category("Query")>
    <Description("Define o apelido do campo a ser pesquisado (substitui o nome do campo no título dos resultados).")>
    <MergableProperty(False)>
    Public Property FieldHeader As String
    ''' <summary>
    ''' Define o nome da tabela a ser pesquisada.
    ''' </summary>
    <Category("Query")>
    <Description("Define o nome da tabela a ser pesquisada.")>
    Public Property MainTable As String
    ''' <summary>
    ''' Define o nome do campo a ser pesquisado.
    ''' </summary>
    <Category("Query")>
    <Description("Define o nome do campo a ser pesquisado.")>
    Public Property MainField As String
    ''' <summary>
    ''' Define o nome do campo da tabela a ser pesquisado.
    ''' </summary>
    <Category("Query")>
    <Description("Define o nome do campo da tabela que está atribuído como PRIMARYKEY.")>
    Public Property MainPKField As String
    ''' <summary>
    ''' Define o nome da tabela a ser combinada com a tabela principal.
    ''' </summary>
    <Category("Query")>
    <Description("Define o nome da tabela a ser combinada com a tabela principal.")>
    Public Property JoinTable As String
    ''' <summary>
    ''' Define o nome do campo a ser retornado referente ao campo da tabela principal.
    ''' </summary>
    <Category("Query")>
    <Description("Define o nome do campo a ser retornado referente ao campo da tabela principal.")>
    Public Property JoinField As String
    ''' <summary>
    ''' Define o nome do campo da tabela que está atribuído como PRIMARYKEY.
    ''' </summary>
    <Category("Query")>
    <Description("Define o nome do campo da tabela que está atribuído como PRIMARYKEY.")>
    Public Property JoinPKField As String
    ''' <summary>
    ''' Define o nome do campo da tabela a ser pesquisado.
    ''' </summary>
    <Category("Query")>
    <DefaultValue(GetType(Integer), "1000")>
    <Description("Define o máximo de resultados que podem ser retornados pela pesquisa.")>
    Public Property Limit As Integer = 1000
    ''' <summary>
    ''' Define os outros campos que serão mostrados nos resultados da pesquisa.
    ''' </summary>
    <Category("Query")>
    <Description("Define os outros campos que serão mostrados nos resultados da pesquisa.")>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    <MergableProperty(False)>
    Public Property OtherFields As New Collection(Of OtherField)
    ''' <summary>
    ''' Define o nome do campo da tabela a ser pesquisado.
    ''' </summary>
    <Category("Query")>
    <Description("Define condições para a pesquisa. Deve ser definida com a sintaxe SQL.")>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    <MergableProperty(False)>
    Public Property Conditions As New Collection(Of Condition)
    ''' <summary>
    ''' Define os parâmetos utilizados nas condições da Query.
    ''' </summary>
    <Category("Query")>
    <Description("Define os parâmetos utilizados nas condições da Query.")>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    <MergableProperty(False)>
    Public Property Parameters As New Collection(Of Parameter)
    ''' <summary>
    ''' Define cor do texto para quando um resultado da pesquisa for selecionado.
    ''' </summary>
    <Category("Aparência")>
    <DefaultValue(GetType(Color), "Blue")>
    <Description("Define cor do texto para quando um resultado da pesquisa for selecionado.")>
    Public Property FreezeColor As Color
        Get
            Return _FreezeColor
        End Get
        Set(value As Color)
            If value <> Color.Transparent Then
                _FreezeColor = value
            Else
                Throw New ArgumentException("O controle não dá suporte a cores de texto transparente.")
            End If
        End Set
    End Property
    ''' <summary>
    ''' Define a cor de fundo do grid.
    ''' </summary>
    <Category("Aparência")>
    <DefaultValue(GetType(Color), "Window")>
    <Description("Define a cor de fundo do grid.")>
    Public Property GridBackColor As Color
        Get
            Return _GridBackColor
        End Get
        Set(value As Color)
            If value <> Color.Transparent Then
                _GridBackColor = value
            Else
                Throw New ArgumentException("O controle não dá suporte a cores de fundo transparente.")
            End If
        End Set
    End Property
    ''' <summary>
    ''' Define a cor de seleção do grid.
    ''' </summary>
    <Category("Aparência")>
    <DefaultValue(GetType(Color), "HotTrack")>
    <Description("Define a cor de seleção do grid.")>
    Public Property GridSelectionBackColor As Color
        Get
            Return _GridSelectionBackColor
        End Get
        Set(value As Color)
            If value <> Color.Transparent Then
                _GridSelectionBackColor = value
            Else
                Throw New ArgumentException("O controle não dá suporte a cores de seleção transparente.")
            End If
        End Set
    End Property
    ''' <summary>
    ''' Define a cor do texto do grid.
    ''' </summary>
    <Category("Aparência")>
    <DefaultValue(GetType(Color), "ControlText")>
    <Description("Define a cor do texto do grid.")>
    Public Property GridForeColor As Color
        Get
            Return _GridForeColor
        End Get
        Set(value As Color)
            If value <> Color.Transparent Then
                _GridForeColor = value
            Else
                Throw New ArgumentException("O controle não dá suporte a cores de texto transparente.")
            End If
        End Set
    End Property
    ''' <summary>
    ''' Define a cor de seleção do texto do grid.
    ''' </summary>
    <Category("Aparência")>
    <DefaultValue(GetType(Color), "Window")>
    <Description("Define a cor de seleção do texto do grid.")>
    Public Property GridSelectionForeColor As Color
        Get
            Return _GridSelectionForeColor
        End Get
        Set(value As Color)
            If value <> Color.Transparent Then
                _GridSelectionForeColor = value
            Else
                Throw New ArgumentException("O controle não dá suporte a cores de texto transparente.")
            End If
        End Set
    End Property
    ''' <summary>
    ''' Define a fonte do cabeçalho do grid como negrito.
    ''' </summary>
    <Category("Aparência")>
    <DefaultValue(GetType(Boolean), "False")>
    <Description("Define a fonte do cabeçalho do grid como negrito.")>
    Public Property GridHeadersBold As Boolean = False
    ''' <summary>
    ''' Define o cabeçalho do grid como visível.
    ''' </summary>
    <Category("Aparência")>
    <DefaultValue(GetType(Boolean), "True")>
    <Description("Define o cabeçalho do grid como visível.")>
    Public Property GridHeaderVisible As Boolean = True
    ''' <summary>
    ''' Define a cor de fundo do cabeçalho do grid.
    ''' </summary>
    <Category("Aparência")>
    <DefaultValue(GetType(Color), "Control")>
    <Description("Define a cor de fundo do cabeçalho do grid.")>
    Public Property GridHeaderBackColor As Color
        Get
            Return _GridHeaderBackColor
        End Get
        Set(value As Color)
            If value <> Color.Transparent Then
                _GridHeaderBackColor = value
            Else
                Throw New ArgumentException("O controle não dá suporte a cores de texto transparente.")
            End If
        End Set
    End Property
    ''' <summary>
    ''' Define a cor do texto do cabeçalho do grid.
    ''' </summary>
    <Category("Aparência")>
    <DefaultValue(GetType(Color), "ControlText")>
    <Description("Define a cor do texto do cabeçalho do grid.")>
    Public Property GridHeaderForeColor As Color
        Get
            Return _GridHeaderForeColor
        End Get
        Set(value As Color)
            If value <> Color.Transparent Then
                _GridHeaderForeColor = value
            Else
                Throw New ArgumentException("O controle não dá suporte a cores de texto transparente.")
            End If
        End Set
    End Property
    ''' <summary>
    ''' Define a cor de fundo da label que informa quantos caracteres faltam.
    ''' </summary>
    <Category("Aparência")>
    <DefaultValue(GetType(Color), "Window")>
    <Description("Define a cor de fundo da label que informa quantos caracteres faltam.")>
    Public Property LabelBackColor As Color
        Get
            Return _LabelBackColor
        End Get
        Set(value As Color)
            If value <> Color.Transparent Then
                _LabelBackColor = value
            Else
                Throw New ArgumentException("O controle não dá suporte a cores de texto transparente.")
            End If
        End Set
    End Property
    ''' <summary>
    ''' Define a cor do texto da label que informa quantos caracteres faltam.
    ''' </summary>
    <Category("Aparência")>
    <DefaultValue(GetType(Color), "ControlText")>
    <Description("Define a cor do texto da label que informa quantos caracteres faltam.")>
    Public Property LabelForeColor As Color
        Get
            Return _LabelForeColor
        End Get
        Set(value As Color)
            If value <> Color.Transparent Then
                _LabelForeColor = value
            Else
                Throw New ArgumentException("O controle não dá suporte a cores de texto transparente.")
            End If
        End Set
    End Property
    ''' <summary>
    ''' Define se o Grid mostrará linhas verticais no DropDown.
    ''' </summary>
    ''' <returns></returns>
    <Category("Aparência")>
    <DefaultValue(GetType(Boolean), "True")>
    <Description("Define se o Grid mostrará linhas verticais no DropDown.")>
    Public Property ShowVerticalGridLines As Boolean = True
    ''' <summary>
    ''' Define a cor da borda do DropDown.
    ''' </summary>
    <Category("Aparência")>
    <DefaultValue(GetType(Color), "HotTrack")>
    <Description("Define a cor da borda do DropDown.")>
    Public Property DropDownBorderColor As Color
        Get
            Return _DropDownBorderColor
        End Get
        Set(value As Color)
            If value <> Color.Transparent Then
                _DropDownBorderColor = value
            Else
                Throw New ArgumentException("O controle não dá suporte a cores de borda transparente.")
            End If
        End Set
    End Property

    ''' <summary>
    ''' Define o quanto o painel será esticado para baixo.
    ''' </summary>
    <Category("Aparência")>
    <DefaultValue(GetType(Integer), "120")>
    <Description("Define o quanto o painel será esticado para baixo.")>
    Public Property DropDownStretchDown As Integer = 120

    '''<summary>
    '''Define o tamanho mínimo do painel de resultados.
    '''</summary>
    <Category("Aparência")>
    <DefaultValue(GetType(Integer), "185")>
    <Description("Define o tamanho mínimo do painel de resultados.")>
    Public Property DropDownMinimumSize As Integer = 185

    '''<summary>
    '''Define a direção que o painel será esticado caso o painel seja menor que o tamanho mínimo.
    '''</summary>
    <Category("Aparência")>
    <DefaultValue(GetType(DirectionTypes), "Right")>
    <Description("Define a direção que o painel será esticado caso o painel seja menor que o tamanho mínimo.")>
    Public Property DropDownAutoStretchDirection As DirectionTypes = DirectionTypes.Right
    ''' <summary>
    ''' Define a quantidade de caracteres necessários para iniciar a pesquisa.
    ''' </summary>
    <Category("Comportamento")>
    <DefaultValue(GetType(Integer), "3")>
    <Description("Define a quantidade de caracteres necessários para iniciar a pesquisa.")>
    Public Property CharactersToQuery As Integer
        Get
            Return _CharactersToQuery
        End Get
        Set(value As Integer)
            If value < 1 Then value = 1
            _CharactersToQuery = value
        End Set
    End Property
    ''' <summary>
    ''' Define se as pesquisas estão habilitadas.
    ''' </summary>
    <Category("Comportamento")>
    <DefaultValue(GetType(Boolean), "True")>
    <Description("Define se as pesquisas estão habilitadas.")>
    Public Property QueryEnabled As Boolean = True
    ''' <summary>
    ''' Define o intervalo em que será feita uma nova pesquisa entre cada caractere digitado.
    ''' </summary>
    <Category("Comportamento")>
    <DefaultValue(GetType(Integer), "300")>
    <Description("Define o intervalo em que será feita uma nova pesquisa entre cada caractere digitado.")>
    Public Property QueryInterval As Integer
        Get
            Return _QueryInterval
        End Get
        Set(value As Integer)
            If value < 0 Then value = 0
            _QueryInterval = value
        End Set
    End Property
    ''' <summary>
    ''' Define se será permitido o uso de hyperlinks em registros selecionados com a tecla control.
    ''' </summary>
    ''' <returns></returns>
    <DefaultValue(GetType(Boolean), "True")>
    <Category("Comportamento")>
    <Description("Define se será permitido o uso de hyperlinks em registros selecionados com a tecla control.")>
    Public Property AllowHyperlink As Boolean = True
    ''' <summary>
    ''' Define a ConnectionString a ser utilizada para o acesso aos dados, essa propriedade sobrepõe o campo compartilhado SearchBox.DefaultConnectionString.
    ''' </summary>
    <Category("Dados")>
    <Description("Define a ConnectionString a ser utilizada para o acesso aos dados, essa propriedade sobrepõe o campo compartilhado SearchBox.DefaultConnectionString.")>
    Public Property ConnectionString As String
    ''' <summary>
    ''' Define o provedor SQL (Namespace) a ser utilizado para o acesso aos dados, essa propriedade sobrepõe o campo compartilhado SearchBox.DefaultDbProvider. Ex: MySql.Data.MySqlClient, System.Data.SQLite.
    ''' </summary>
    <Category("Dados")>
    <Description("Define o provedor SQL (Namespace) a ser utilizado para o acesso aos dados, essa propriedade sobrepõe o campo compartilhado SearchBox.DefaultDbProvider. Ex: MySql.Data.MySqlClient, System.Data.SQLite.")>
    Public Property DbProvider As String
    Public Overrides Property CellTemplate As DataGridViewCell
        Get
            Return MyBase.CellTemplate
        End Get
        Set(ByVal value As DataGridViewCell)
            If value IsNot Nothing AndAlso Not value.[GetType]().IsAssignableFrom(GetType(DataGridViewQueryCell)) Then
                Throw New InvalidCastException("Must be a DataGridViewQueryCell")
            End If
            MyBase.CellTemplate = value
        End Set
    End Property
    Public Sub New()
        MyBase.New(New DataGridViewQueryCell())
    End Sub
    Protected Overrides Sub OnDataGridViewChanged()
        MyBase.OnDataGridViewChanged()
    End Sub
    Public Overrides Function Clone() As Object
        Dim DgvColumn As DataGridViewQueryColumn = CType(MyBase.Clone(), DataGridViewQueryColumn)
        DgvColumn.FieldHeader = FieldHeader
        DgvColumn.MainTable = MainTable
        DgvColumn.MainField = MainField
        DgvColumn.MainPKField = MainPKField
        DgvColumn.JoinTable = JoinTable
        DgvColumn.JoinField = JoinField
        DgvColumn.JoinPKField = JoinPKField
        DgvColumn.Limit = Limit
        DgvColumn.OtherFields = OtherFields
        DgvColumn.Conditions = Conditions
        DgvColumn.Parameters = Parameters
        DgvColumn.FreezeColor = FreezeColor
        DgvColumn.GridBackColor = GridBackColor
        DgvColumn.GridSelectionBackColor = GridSelectionBackColor
        DgvColumn.GridForeColor = GridForeColor
        DgvColumn.GridSelectionForeColor = GridSelectionForeColor
        DgvColumn.GridHeadersBold = GridHeadersBold
        DgvColumn.GridHeaderVisible = GridHeaderVisible
        DgvColumn.GridHeaderBackColor = GridHeaderBackColor
        DgvColumn.GridHeaderForeColor = GridHeaderForeColor
        DgvColumn.LabelBackColor = LabelBackColor
        DgvColumn.LabelForeColor = LabelForeColor
        DgvColumn.ShowVerticalGridLines = ShowVerticalGridLines
        DgvColumn.DropDownBorderColor = DropDownBorderColor
        DgvColumn.DropDownStretchDown = DropDownStretchDown
        DgvColumn.CharactersToQuery = CharactersToQuery
        DgvColumn.QueryEnabled = QueryEnabled
        DgvColumn.QueryInterval = QueryInterval
        DgvColumn.AllowHyperlink = AllowHyperlink
        DgvColumn.ConnectionString = ConnectionString
        DgvColumn.DbProvider = DbProvider
        DgvColumn.DropDownAutoStretchDirection = DropDownAutoStretchDirection
        DgvColumn.DropDownMinimumSize = DropDownMinimumSize
        Return DgvColumn
    End Function
End Class
Public Class DataGridViewQueryCell
    Inherits DataGridViewTextBoxCell



    Public Sub New()
        MyBase.New()

    End Sub
    ''' <summary>
    ''' Retorna se existe um registro selecionado.
    ''' </summary>
    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Always)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property IsFreezed As Boolean
        Get
            Return Common.IsFreezed
        End Get
    End Property
    ''' <summary>
    ''' Retorna a chave primária do registro selecionado ou 0 quando não há registro selecionado.
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Always)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property FreezedPrimaryKey As Long
        Get
            Return Common.FreezedPrimaryKey
        End Get
    End Property
    ''' <summary>
    ''' Retorna o valor do registro selecionado.
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Always)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property FreezedValue As String
        Get
            Return Common.FreezedValue
        End Get
    End Property

    Public Overrides Sub InitializeEditingControl(ByVal rowIndex As Integer, ByVal initialFormattedValue As Object, ByVal dataGridViewCellStyle As DataGridViewCellStyle)
        MyBase.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle)
        Dim Control As DataGridViewQueryEditingControl = TryCast(DataGridView.EditingControl, DataGridViewQueryEditingControl)
        Dim DgvColumn As DataGridViewQueryColumn
        DgvColumn = DataGridView.Columns(ColumnIndex)
        Control.FieldHeader = DgvColumn.FieldHeader
        Control.MainTable = DgvColumn.MainTable
        Control.MainField = DgvColumn.MainField
        Control.MainPKField = DgvColumn.MainPKField
        Control.JoinTable = DgvColumn.JoinTable
        Control.JoinField = DgvColumn.JoinField
        Control.JoinPKField = DgvColumn.JoinPKField
        Control.Limit = DgvColumn.Limit
        Control.OtherFields = DgvColumn.OtherFields
        Control.Conditions = DgvColumn.Conditions
        Control.Parameters = DgvColumn.Parameters
        Control.FreezeColor = DgvColumn.FreezeColor
        Control.GridBackColor = DgvColumn.GridBackColor
        Control.GridSelectionBackColor = DgvColumn.GridSelectionBackColor
        Control.GridForeColor = DgvColumn.GridForeColor
        Control.GridSelectionForeColor = DgvColumn.GridSelectionForeColor
        Control.GridHeadersBold = DgvColumn.GridHeadersBold
        Control.GridHeaderVisible = DgvColumn.GridHeaderVisible
        Control.GridHeaderBackColor = DgvColumn.GridHeaderBackColor
        Control.GridHeaderForeColor = DgvColumn.GridHeaderForeColor
        Control.LabelBackColor = DgvColumn.LabelBackColor
        Control.LabelForeColor = DgvColumn.LabelForeColor
        Control.ShowVerticalGridLines = DgvColumn.ShowVerticalGridLines
        Control.DropDownBorderColor = DgvColumn.DropDownBorderColor
        Control.DropDownStretchDown = DgvColumn.DropDownStretchDown
        Control.CharactersToQuery = DgvColumn.CharactersToQuery
        Control.QueryEnabled = DgvColumn.QueryEnabled
        Control.QueryInterval = DgvColumn.QueryInterval
        Control.AllowHyperlink = DgvColumn.AllowHyperlink
        Control.ConnectionString = DgvColumn.ConnectionString
        Control.DbProvider = DgvColumn.DbProvider
        Control.DropDownAutoStretchRight = DgvColumn.DropDownAutoStretchDirection
        Control.DropDownAutoStretchRight = False
        If DataGridView.Columns(DataGridView.CurrentCell.ColumnIndex).Width < DgvColumn.DropDownMinimumSize Then
            If DgvColumn.DropDownAutoStretchDirection = DataGridViewQueryColumn.DirectionTypes.Right Then
                Control.DropDownStretchRight = DgvColumn.DropDownMinimumSize - DataGridView.Columns(DataGridView.CurrentCell.ColumnIndex).Width
            Else
                Control.DropDownStretchLeft = DgvColumn.DropDownMinimumSize - DataGridView.Columns(DataGridView.CurrentCell.ColumnIndex).Width
            End If
        Else
            Control.DropDownStretchRight = 0
            Control.DropDownStretchLeft = 0
        End If
    End Sub

    Public Overrides ReadOnly Property EditType As Type
        Get
            Return GetType(DataGridViewQueryEditingControl)
        End Get
    End Property

    Public Overrides ReadOnly Property ValueType As Type
        Get
            Return GetType(String)
        End Get
    End Property

    Public Overrides ReadOnly Property DefaultNewRowValue As Object
        Get
            Return String.Empty
        End Get
    End Property
End Class
Class DataGridViewQueryEditingControl
    Inherits QueriedBox
    Implements IDataGridViewEditingControl
    Private _dataGridView As DataGridView
    Private _valueChanged As Boolean = False
    Private _rowIndex As Integer

    Public Sub New()

    End Sub
    Private Property EditingControlDataGridView As DataGridView Implements IDataGridViewEditingControl.EditingControlDataGridView
        Get
            Return _dataGridView
        End Get
        Set(ByVal value As DataGridView)
            _dataGridView = value
        End Set
    End Property
    Private Property EditingControlFormattedValue As Object Implements IDataGridViewEditingControl.EditingControlFormattedValue
        Get
            Return Text
        End Get
        Set(ByVal value As Object)
            Text = value
        End Set
    End Property
    Private Function GetEditingControlFormattedValue(context As DataGridViewDataErrorContexts) As Object Implements IDataGridViewEditingControl.GetEditingControlFormattedValue
        Return EditingControlFormattedValue
    End Function
    Private ReadOnly Property EditingPanelCursor As Cursor Implements IDataGridViewEditingControl.EditingPanelCursor
        Get
            Return MyBase.Cursor
        End Get
    End Property
    Private Property EditingControlValueChanged As Boolean Implements IDataGridViewEditingControl.EditingControlValueChanged
        Get
            Return _valueChanged
        End Get
        Set(ByVal value As Boolean)
            If IsFreezed Then
                Common.IsFreezed = True
                Common.FreezedPrimaryKey = FreezedPrimaryKey
            Else
                Common.IsFreezed = False
                Common.FreezedPrimaryKey = 0
            End If
            _valueChanged = value
        End Set
    End Property
    Private ReadOnly Property RepositionEditingControlOnValueChange As Boolean Implements IDataGridViewEditingControl.RepositionEditingControlOnValueChange
        Get
            Return False
        End Get
    End Property
    Private Sub PrepareEditingControlForEdit(selectAll As Boolean) Implements IDataGridViewEditingControl.PrepareEditingControlForEdit
    End Sub
    Private Function EditingControlWantsInputKey(keyData As Keys, dataGridViewWantsInputKey As Boolean) As Boolean Implements IDataGridViewEditingControl.EditingControlWantsInputKey
        Select Case keyData And Keys.KeyCode
            Case Keys.Left, Keys.Up, Keys.Down, Keys.Right, Keys.Home, Keys.[End], Keys.PageDown, Keys.PageUp, Keys.Tab, Keys.Enter
                Return True
            Case Else
                Return Not dataGridViewWantsInputKey
        End Select
    End Function



    Private Property EditingControlRowIndex As Integer Implements IDataGridViewEditingControl.EditingControlRowIndex
        Get
            Return _rowIndex
        End Get
        Set(ByVal value As Integer)
            _rowIndex = value
        End Set
    End Property



    Private Sub ApplyCellStyleToEditingControl(dataGridViewCellStyle As DataGridViewCellStyle) Implements IDataGridViewEditingControl.ApplyCellStyleToEditingControl
        Font = dataGridViewCellStyle.Font
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        _valueChanged = True
        EditingControlDataGridView.NotifyCurrentCellDirty(True)
        MyBase.OnTextChanged(e)
    End Sub



End Class
#End Region
#Region "DECIMAL"
Public Class DataGridViewDecimalColumn
    Inherits DataGridViewColumn
    Public Overrides Function Clone() As Object
        Dim DgvColumn As DataGridViewDecimalColumn = CType(MyBase.Clone(), DataGridViewDecimalColumn)
        DgvColumn.DecimalPlaces = DecimalPlaces
        Return DgvColumn
    End Function
    Private _DecimalPlaces As Integer = 2

    Public Property DecimalPlaces As Integer
        Get
            Return _DecimalPlaces
        End Get
        Set(value As Integer)
            Common.DecimalPlaces = value
            _DecimalPlaces = value
        End Set
    End Property


    Protected Overrides Sub OnDataGridViewChanged()
        MyBase.OnDataGridViewChanged()
    End Sub
    Public Sub New()
        MyBase.New(New DataGridViewDecimalCell())
    End Sub
    Public Overrides Property CellTemplate As DataGridViewCell
        Get
            Return MyBase.CellTemplate
        End Get
        Set(ByVal value As DataGridViewCell)
            If value IsNot Nothing AndAlso Not value.[GetType]().IsAssignableFrom(GetType(DataGridViewDecimalCell)) Then
                Throw New InvalidCastException("Must be a DataGridViewDecimalCell")
            End If
            MyBase.CellTemplate = value
        End Set
    End Property
End Class
Public Class DataGridViewDecimalCell
    Inherits DataGridViewTextBoxCell



    Public Sub New()
        MyBase.New()

    End Sub
    ''' <summary>
    ''' Retorna se existe um registro selecionado.
    ''' </summary>
    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Always)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property DecimalValue As Decimal
        Get
            Return Common.DecimalValue
        End Get
    End Property

    Public Overrides Sub InitializeEditingControl(ByVal rowIndex As Integer, ByVal initialFormattedValue As Object, ByVal dataGridViewCellStyle As DataGridViewCellStyle)
        MyBase.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle)
        Dim Control As DataGridViewDecimalEditingControl = TryCast(DataGridView.EditingControl, DataGridViewDecimalEditingControl)
        Dim DgvColumn As DataGridViewDecimalColumn
        DgvColumn = DataGridView.Columns(ColumnIndex)
        Control.DecimalPlaces = DgvColumn.DecimalPlaces
        Control.TextAlign = HorizontalAlignment.Right

    End Sub

    Public Overrides ReadOnly Property EditType As Type
        Get
            Return GetType(DataGridViewDecimalEditingControl)
        End Get
    End Property

    Public Overrides ReadOnly Property ValueType As Type
        Get
            Return GetType(Decimal)
        End Get
    End Property

    Public Overrides ReadOnly Property DefaultNewRowValue As Object
        Get
            Return FormatNumber(0, Common.DecimalPlaces, TriState.True)
        End Get
    End Property
End Class
Class DataGridViewDecimalEditingControl
    Inherits DecimalBox
    Implements IDataGridViewEditingControl
    Private _dataGridView As DataGridView
    Private _valueChanged As Boolean = False
    Private _rowIndex As Integer

    Public Sub New()

    End Sub
    Private Property EditingControlDataGridView As DataGridView Implements IDataGridViewEditingControl.EditingControlDataGridView
        Get
            Return _dataGridView
        End Get
        Set(ByVal value As DataGridView)
            _dataGridView = value
        End Set
    End Property
    Private Property EditingControlFormattedValue As Object Implements IDataGridViewEditingControl.EditingControlFormattedValue
        Get
            Return Text
        End Get
        Set(ByVal value As Object)
            Text = FormatNumber(value, DecimalPlaces, TriState.True)
        End Set
    End Property
    Private Function GetEditingControlFormattedValue(context As DataGridViewDataErrorContexts) As Object Implements IDataGridViewEditingControl.GetEditingControlFormattedValue
        If IsNumeric(EditingControlFormattedValue) Then
            Return FormatNumber(EditingControlFormattedValue, DecimalPlaces, TriState.True)
        Else
            Return FormatNumber(0, DecimalPlaces, TriState.True)
        End If
    End Function
    Private ReadOnly Property EditingPanelCursor As Cursor Implements IDataGridViewEditingControl.EditingPanelCursor
        Get
            Return MyBase.Cursor
        End Get
    End Property
    Private Property EditingControlValueChanged As Boolean Implements IDataGridViewEditingControl.EditingControlValueChanged
        Get
            Return _valueChanged
        End Get
        Set(ByVal value As Boolean)
            _valueChanged = value
            Common.DecimalValue = FormatNumber(Text, DecimalPlaces, TriState.True)
        End Set
    End Property
    Private ReadOnly Property RepositionEditingControlOnValueChange As Boolean Implements IDataGridViewEditingControl.RepositionEditingControlOnValueChange
        Get
            Return False
        End Get
    End Property
    Private Sub PrepareEditingControlForEdit(selectAll As Boolean) Implements IDataGridViewEditingControl.PrepareEditingControlForEdit
    End Sub
    Private Function EditingControlWantsInputKey(keyData As Keys, dataGridViewWantsInputKey As Boolean) As Boolean Implements IDataGridViewEditingControl.EditingControlWantsInputKey
        Select Case keyData And Keys.KeyCode
            Case Keys.Left, Keys.Up, Keys.Down, Keys.Right, Keys.Home, Keys.[End]
                Return True
            Case Else
                Return Not dataGridViewWantsInputKey
        End Select
    End Function

    Private Property EditingControlRowIndex As Integer Implements IDataGridViewEditingControl.EditingControlRowIndex
        Get
            Return _rowIndex
        End Get
        Set(ByVal value As Integer)
            _rowIndex = value
        End Set
    End Property


    Private Sub ApplyCellStyleToEditingControl(dataGridViewCellStyle As DataGridViewCellStyle) Implements IDataGridViewEditingControl.ApplyCellStyleToEditingControl
        Font = dataGridViewCellStyle.Font
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)

        _valueChanged = True
        EditingControlDataGridView.NotifyCurrentCellDirty(True)
        MyBase.OnTextChanged(e)
    End Sub



End Class
#End Region
#Region "MASKEDTEXTBOX"
Public Class DataGridViewMaskedTextBoxColumn
    Inherits DataGridViewColumn

    Public Sub New()
        MyBase.New(New DataGridViewMaskedTextBoxCell())
    End Sub

    Private maskValue As String = ""

    ''' MaskedTextBox

    Public Property Mask() As String
        Get
            Return Me.maskValue
        End Get
        Set(ByVal value As String)
            Me.maskValue = value
        End Set
    End Property

    Public Overrides Function Clone() As Object
        Dim col As DataGridViewMaskedTextBoxColumn = CType(MyBase.Clone(), DataGridViewMaskedTextBoxColumn)
        col.Mask = Me.Mask
        Return col
    End Function

    'CellTemplate 
    Public Overrides Property CellTemplate() As DataGridViewCell
        Get
            Return MyBase.CellTemplate
        End Get
        Set(ByVal value As DataGridViewCell)
            'DataGridViewMaskedTextBoxCell 
            ' CellTemplate 
            If Not TypeOf value Is DataGridViewMaskedTextBoxCell Then
                Throw New InvalidCastException("DataGridViewMaskedTextBoxCell")
            End If
            MyBase.CellTemplate = value
        End Set
    End Property

    Private Sub InitializeComponent()

    End Sub
End Class
Public Class DataGridViewMaskedTextBoxCell
    Inherits DataGridViewTextBoxCell
    Public Sub New()
    End Sub
    Public Overrides Sub InitializeEditingControl(ByVal rowIndex As Integer, ByVal initialFormattedValue As Object, ByVal dataGridViewCellStyle As DataGridViewCellStyle)
        MyBase.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle)
        Dim MaskedBox As DataGridViewMaskedTextBoxEditingControl = Me.DataGridView.EditingControl
        If Not (MaskedBox Is Nothing) Then
            If Me.Value = "" Then
                MaskedBox.Text = ""
            Else
                MaskedBox.Text = Me.Value.ToString()
            End If
            Dim column As DataGridViewMaskedTextBoxColumn = Me.OwningColumn
            If Not (column Is Nothing) Then
                MaskedBox.Mask = column.Mask
            End If
        End If
    End Sub
    Public Overrides ReadOnly Property EditType() As Type
        Get
            Return GetType(DataGridViewMaskedTextBoxEditingControl)
        End Get
    End Property
    Public Overrides ReadOnly Property ValueType() As Type
        Get
            Return GetType(Object)
        End Get
    End Property
    Public Overrides ReadOnly Property DefaultNewRowValue() As Object
        Get
            Return MyBase.DefaultNewRowValue
        End Get
    End Property
End Class
<ToolboxItem(False)>
Public Class DataGridViewMaskedTextBoxEditingControl
    Inherits MaskedTextBox
    Implements IDataGridViewEditingControl

    'DataGridView
    Private dataGridView As DataGridView
    Private rowIndex As Integer
    Private valueChanged As Boolean

    Public Sub New()
        Me.TabStop = False
    End Sub

    '
    Public Function GetEditingControlFormattedValue(ByVal context As DataGridViewDataErrorContexts) As Object Implements IDataGridViewEditingControl.GetEditingControlFormattedValue
        Return Me.Text
    End Function

    '
    Public Property EditingControlFormattedValue() As Object Implements IDataGridViewEditingControl.EditingControlFormattedValue
        Get
            Return Me.GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting)
        End Get
        Set(ByVal value As Object)
            Me.Text = CStr(value)
        End Set
    End Property


    Public Sub ApplyCellStyleToEditingControl(ByVal dataGridViewCellStyle As DataGridViewCellStyle) Implements IDataGridViewEditingControl.ApplyCellStyleToEditingControl

        Me.Font = dataGridViewCellStyle.Font
        Me.ForeColor = dataGridViewCellStyle.ForeColor
        Me.BackColor = dataGridViewCellStyle.BackColor


        Select Case dataGridViewCellStyle.Alignment
            Case DataGridViewContentAlignment.BottomCenter, DataGridViewContentAlignment.MiddleCenter, DataGridViewContentAlignment.TopCenter
                Me.TextAlign = HorizontalAlignment.Center
            Case DataGridViewContentAlignment.BottomRight, DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.TopRight
                Me.TextAlign = HorizontalAlignment.Right
            Case Else
                Me.TextAlign = HorizontalAlignment.Left
        End Select
    End Sub


    Public Property EditingControlDataGridView() As DataGridView Implements IDataGridViewEditingControl.EditingControlDataGridView
        Get
            Return Me.dataGridView
        End Get
        Set(ByVal value As DataGridView)
            Me.dataGridView = value
        End Set
    End Property


    Public Property EditingControlRowIndex() As Integer Implements IDataGridViewEditingControl.EditingControlRowIndex
        Get
            Return Me.rowIndex
        End Get
        Set(ByVal value As Integer)
            Me.rowIndex = value
        End Set
    End Property

    Public Property EditingControlValueChanged() As Boolean Implements IDataGridViewEditingControl.EditingControlValueChanged
        Get
            Return Me.valueChanged
        End Get
        Set(ByVal value As Boolean)
            Me.valueChanged = value
        End Set
    End Property



    Public ReadOnly Property EditingPanelCursor() As Cursor Implements IDataGridViewEditingControl.EditingPanelCursor
        Get
            Return MyBase.Cursor
        End Get
    End Property

    Public Sub PrepareEditingControlForEdit(ByVal selectAll As Boolean) Implements IDataGridViewEditingControl.PrepareEditingControlForEdit

        If selectAll Then
            Me.SelectAll()
        Else
            Me.SelectionStart = Me.TextLength
        End If
    End Sub


    Public ReadOnly Property RepositionEditingControlOnValueChange() As Boolean Implements IDataGridViewEditingControl.RepositionEditingControlOnValueChange
        Get
            Return False
        End Get
    End Property


    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e)
        Me.valueChanged = True
        Me.dataGridView.NotifyCurrentCellDirty(True)
    End Sub

    Public Function EditingControlWantsInputKey(keyData As Keys, dataGridViewWantsInputKey As Boolean) As Boolean Implements IDataGridViewEditingControl.EditingControlWantsInputKey
        Return False
    End Function
End Class
#End Region