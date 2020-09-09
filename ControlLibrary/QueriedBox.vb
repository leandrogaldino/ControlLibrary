Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Data.Common
Imports System.Drawing
Imports System.Drawing.Design
Imports System.Reflection
Imports System.Windows.Forms
Imports System.Windows.Forms.Design
#Region "README"
'Para o DbProvider Funcionar com o SQLite, é necessario colocar esse código no App.config.
'<system.data>
'<DbProviderFactories>
'<remove invariant = "System.Data.SQLite" ></remove>
'<add name = "ADO.NET Provider for SQLite"
'invariant="System.Data.SQLite" 
'description="ADO.NET Provider for SQLite " 
'type="System.Data.SQLite.SQLiteFactory, System.Data.SQLite">
'</add>          
'</DbProviderFactories>
'</system.data>
#End Region
Public Class QueriedBox
    Inherits TextBox
#Region "EVENTS"
    <Category("Propriedade Alterada")>
    Public Event FreezedPrimaryKeyChanged(sender As Object, e As EventArgs)
    <Category("Ação")>
    Public Event HyperlinkClicked(sender As Object, e As EventArgs)
#End Region
#Region "FIELDS"
    Private DropDownResultsForm As FormDropDownResults
    Private _CtrlHyperLink As Boolean = False
    Private _IsHyperLink As Boolean = False
    Private _FreezedValue As String
    Private _UnFreezeColor As Color
    Private _IsFreezed As Boolean
    Private _FreezedPrimaryKey As Long
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
    Private _DropDownAutoStretchRight As Boolean
    Private _DropDownStretchRight As Integer
    Private _QueryEnabled As Boolean = True
    Private _DesignerHost As IDesignerHost
#End Region
#Region "OVERRIDED PROPERTIES"
    Public Overrides Property Multiline As Boolean
        Get
            Return MyBase.Multiline
        End Get
        Set(value As Boolean)
            If value Then
                QueryEnabled = False
            End If
            MyBase.Multiline = value
        End Set
    End Property
#End Region
#Region "PROPERTIES"
#Region "QUERY"
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
    <Editor(GetType(OtherFieldCollectionDialogUIEditor), GetType(UITypeEditor))>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    <MergableProperty(False)>
    Public Property OtherFields As New Collection(Of OtherField)
    ''' <summary>
    ''' Define o nome do campo da tabela a ser pesquisado.
    ''' </summary>
    <Category("Query")>
    <Description("Define condições para a pesquisa. Deve ser definida com a sintaxe SQL.")>
    <Editor(GetType(ConditionCollectionDialogUIEditor), GetType(UITypeEditor))>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    <MergableProperty(False)>
    Public Property Conditions As New Collection(Of Condition)
    ''' <summary>
    ''' Define os parâmetos utilizados nas condições da Query.
    ''' </summary>
    <Category("Query")>
    <Description("Define os parâmetos utilizados nas condições da Query.")>
    <Editor(GetType(ParameterCollectionDialogUIEditor), GetType(UITypeEditor))>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    <MergableProperty(False)>
    Public Property Parameters As New Collection(Of Parameter)



#End Region
#Region "APARENCIA"
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
    ''' Define o quanto o painel será esticado automaticamente para a direita até mostrar todos os resultados.
    ''' </summary>
    <Category("Aparência")>
    <DefaultValue(GetType(Integer), "120")>
    <Description("Define o quanto o painel será esticado automaticamente para a direita até mostrar todos os resultados.")>
    Public Property DropDownAutoStretchRight As Boolean
        Get
            Return _DropDownAutoStretchRight
        End Get
        Set(value As Boolean)
            _DropDownAutoStretchRight = value
            If value Then DropDownStretchRight = 0
        End Set
    End Property
    ''' <summary>
    ''' Define o quanto o painel será esticado para baixo.
    ''' </summary>
    <Category("Aparência")>
    <DefaultValue(GetType(Integer), "120")>
    <Description("Define o quanto o painel será esticado para baixo.")>
    Public Property DropDownStretchDown As Integer = 120
    ''' <summary>
    ''' Define o quanto o painel será esticado para a esquerda.
    ''' </summary>
    <Category("Aparência")>
    <DefaultValue(GetType(Integer), "0")>
    <Description("Define o quanto o painel será esticado para a esquerda.")>
    Public Property DropDownStretchLeft As Integer
    ''' <summary>
    ''' Define o quanto o painel será esticado para a direita.
    ''' </summary>
    <Category("Aparência")>
    <DefaultValue(GetType(Integer), "0")>
    <Description("Define o quanto o painel será esticado para a direita.")>
    Public Property DropDownStretchRight As Integer
        Get
            Return _DropDownStretchRight
        End Get
        Set(value As Integer)
            If DropDownAutoStretchRight Then value = 0
            _DropDownStretchRight = value
        End Set
    End Property
#End Region
#Region "COMPORTAMENTO"
    ''' <summary>
    ''' Define se será mostrado o inicio do conteúdo ao sair do controle.
    ''' </summary>
    <Category("Comportamento")>
    <DefaultValue(GetType(Boolean), "True")>
    <Description("Define se será mostrado o inicio do conteúdo ao sair do controle.")>
    Public Property ShowStartOnLeave As Boolean = True
    ''' <summary>
    ''' Define os dependentes desse controle, quando um registro do painel de pesquisa é selecionado, seus campos são refletidos nos controles dependentes.
    ''' </summary>
    <Category("Comportamento")>
    <Description("Define quais controles estarão ligados a esse controle nas pesquisas.")>
    <Editor(GetType(DependentsDropDownUIEditor), GetType(UITypeEditor))>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    <MergableProperty(False)>
    Public Property Dependents() As New Collection(Of QueriedBox)
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
    Public Property QueryEnabled As Boolean
        Get
            Return _QueryEnabled
        End Get
        Set(value As Boolean)
            If value Then Multiline = False
            _QueryEnabled = value
        End Set
    End Property
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
#End Region
#Region "DADOS"
    ''' <summary>
    ''' Define a ConnectionString a ser utilizada para o acesso aos dados, essa propriedade sobrepõe o campo compartilhado SearchBox.DefaultConnectionString.
    ''' </summary>
    <Category("Dados")>
    <Description("Define a ConnectionString a ser utilizada para o acesso aos dados, essa propriedade sobrepõe o campo compartilhado SearchBox.DefaultConnectionString.")>
    Public Property ConnectionString As String
    ''' <summary>
    ''' Define a ConnectionString a ser utilizada em todos os controles para o acesso aos dados, essa propriedade é sobreposta caso a propriedade ConnectionString do controle for informada.
    ''' </summary>
    ''' <returns></returns>
    <Description("Define a ConnectionString a ser utilizada em todos os controles para o acesso aos dados, essa propriedade é sobreposta caso a propriedade ConnectionString do controle for informada.")>
    Public Shared Property DefaultConnectionString As String

    ''' <summary>
    ''' Define o provedor SQL (Namespace) a ser utilizado para o acesso aos dados, essa propriedade sobrepõe o campo compartilhado SearchBox.DefaultDbProvider. Ex: MySql.Data.MySqlClient, System.Data.SQLite.
    ''' </summary>
    <Category("Dados")>
    <Description("Define o provedor SQL (Namespace) a ser utilizado para o acesso aos dados, essa propriedade sobrepõe o campo compartilhado SearchBox.DefaultDbProvider. Ex: MySql.Data.MySqlClient, System.Data.SQLite.")>
    Public Property DbProvider As String


    ''' <summary>
    ''' Define o provedor SQL (Namespace) a ser utilizado em todos os controles para o acesso aos dados, essa propriedade é sobreposta caso a propriedade DbProvider do controle for informada.
    ''' </summary>
    ''' <returns></returns>
    <Description("Define o provedor SQL (Namespace) a ser utilizado em todos os controles para o acesso aos dados, essa propriedade é sobreposta caso a propriedade DbProvider do controle for informada.")>
    Public Shared Property DefaultDbProvider As String



#End Region
#Region "HIDDEN PROPERTIES"
    ''' <summary>
    ''' Retorna se existe um registro selecionado.
    ''' </summary>
    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Always)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property IsFreezed As Boolean
        Get
            Return _IsFreezed
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
            Return _FreezedPrimaryKey
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
            Return _FreezedValue
        End Get
    End Property
    ''' <summary>
    ''' Retorna a cor padrão de quando um registro é deselecionado (Mesma cor de fundo do controle).
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    <EditorBrowsable(EditorBrowsableState.Always)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property UnFreezeColor As Color
        Get
            Return _UnFreezeColor
        End Get
    End Property
#End Region
#End Region
#Region "OVERRIDED SUBS"
    Protected Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        MyBase.OnHandleCreated(e)
        If DesignMode AndAlso Site IsNot Nothing Then
            _DesignerHost = TryCast(Site.GetService(GetType(IDesignerHost)), IDesignerHost)
            If _DesignerHost IsNot Nothing Then
                Dim designer = CType(_DesignerHost.GetDesigner(Me), ControlDesigner)
                If designer IsNot Nothing Then
                    Dim actions = designer.ActionLists(0)
                    designer.ActionLists.Clear()
                    designer.ActionLists.Add(New QueriedTextBoxControlDesignerActionList(designer, actions))
                End If
            End If
        End If
    End Sub
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        Timer = New Timer With {
            .Interval = QueryInterval
        }
        _UnFreezeColor = ForeColor
        If Not DesignMode AndAlso Parent IsNot Nothing Then
            AddHandler Parent.FindForm.Deactivate, AddressOf Form_Deactivate
        End If
    End Sub
    'Protected Overrides Sub OnParentChanged(e As EventArgs)
    '    MyBase.OnParentChanged(e)
    '    If FindForm() IsNot Nothing Then
    '        AddHandler FindForm.Deactivate, AddressOf Parent_Deactivate
    '    End If
    'End Sub
    Protected Overrides Sub OnEnter(e As EventArgs)
        MyBase.OnEnter(e)
        SelectionStart = Text.Length
    End Sub
    Protected Overrides Sub OnLeave(e As EventArgs)
        MyBase.OnLeave(e)
        If QueryEnabled Then
            If _IsHyperLink Then FormatTextBox(False)
            If DropDownResultsForm IsNot Nothing AndAlso DropDownResultsForm.DgvResults.SelectedRows.Count = 1 Then
                If UCase(Text) = UCase(DropDownResultsForm.DgvResults.SelectedRows(0).Cells(FieldHeader).Value.ToString) Then
                    AutoFreeze()
                End If
            End If
            CloseDropDown()
        End If
        If ShowStartOnLeave Then
            SelectionStart = 0
        End If
    End Sub
    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
        Dim Chars As Integer
        Dim CharsDif As Integer
        If Not DesignMode Then
            If QueryEnabled Then
                If Text <> FreezedValue Then AutoUnfreeze()
                If Parent IsNot Nothing Then
                    If DropDownResultsForm Is Nothing Then
                        DropDownResultsForm = New FormDropDownResults(Me)
                        CType(DropDownResultsForm, FormDropDownResults).Textbox = Me
                        DropDownResultsForm.Location = Me.Parent.PointToScreen(New Point(Me.Left - DropDownStretchLeft, Me.Bottom))
                        DropDownResultsForm.Width = DropDownStretchLeft + Me.Width + If(DropDownAutoStretchRight, 0, DropDownStretchRight)
                        DropDownResultsForm.Height = DropDownStretchDown
                        AddHandler DropDownResultsForm.FormClosed, AddressOf DropDownResultsForm_FormClosed
                        DropDownResultsForm.Show()
                    End If
                    Chars = Text.Replace("%", Nothing).Count
                    CharsDif = CharactersToQuery - Chars
                    If Chars = 0 Then
                        CloseDropDown()
                    Else
                        If Chars < CharactersToQuery Then
                            DropDownResultsForm.DgvResults.DataSource = Nothing
                            DropDownResultsForm.LblCharsRemaining.Visible = True
                            DropDownResultsForm.DgvResults.Visible = False
                            If CharsDif > 1 Then
                                DropDownResultsForm.LblCharsRemaining.Text = String.Format("Digite mais {0} caracteres para consultar.", CharactersToQuery - Chars)
                            ElseIf CharsDif = 1 Then
                                DropDownResultsForm.LblCharsRemaining.Text = String.Format("Digite mais {0} caractere para consultar.", CharactersToQuery - Chars)
                            End If
                        ElseIf Chars >= CharactersToQuery Then
                            Timer.Stop() : Timer.Start()
                            DropDownResultsForm.LblCharsRemaining.Visible = False
                            DropDownResultsForm.DgvResults.Visible = True
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Protected Overrides Sub OnForeColorChanged(e As EventArgs)
        MyBase.OnForeColorChanged(e)
        If QueryEnabled Then
            _UnFreezeColor = ForeColor
        End If
    End Sub
    Protected Overrides Sub OnPreviewKeyDown(e As PreviewKeyDownEventArgs)
        MyBase.OnPreviewKeyDown(e)
        Dim Row As Long
        If QueryEnabled Then
            If AllowHyperlink Then FormatTextBox(e.Control)
            If DropDownResultsForm IsNot Nothing AndAlso DropDownResultsForm.DgvResults.SelectedRows.Count = 1 Then
                Select Case e.KeyCode
                    Case Is = Keys.Tab
                        If DropDownResultsForm IsNot Nothing AndAlso DropDownResultsForm.DgvResults.SelectedRows.Count = 1 Then
                            If UCase(Text) = UCase(DropDownResultsForm.DgvResults.SelectedRows(0).Cells(FieldHeader).Value.ToString) Then
                                AutoFreeze()
                            End If
                        End If
                        CloseDropDown()
                    Case Is = Keys.Enter
                        AutoFreeze()
                        Me.Select(TextLength, 0)
                        CloseDropDown()
                    Case Is = Keys.Escape
                        CloseDropDown()
                    Case Is = Keys.Down
                        Row = DropDownResultsForm.DgvResults.SelectedRows(0).Index
                        If DropDownResultsForm IsNot Nothing AndAlso DropDownResultsForm.DgvResults.Rows.Count > Row + 1 Then
                            DropDownResultsForm.DgvResults.Rows(Row + 1).Selected = True
                            If DropDownResultsForm.DgvResults.SelectedRows(0).Index = DropDownResultsForm.DgvResults.Rows.Count - 1 Then
                                DropDownResultsForm.DgvResults.FirstDisplayedScrollingRowIndex = DropDownResultsForm.DgvResults.SelectedRows(0).Index
                                Row += 1
                            Else
                                Row += 2
                            End If
                            If DropDownResultsForm.DgvResults.Rows(Row).Displayed = False Then
                                If Row >= 3 Then
                                    DropDownResultsForm.DgvResults.FirstDisplayedScrollingRowIndex = DropDownResultsForm.DgvResults.SelectedRows(0).Index - 2
                                End If
                            End If
                        End If
                    Case Is = Keys.Up
                        Row = DropDownResultsForm.DgvResults.SelectedRows(0).Index
                        If DropDownResultsForm.DgvResults.Visible = True And Row > 0 Then
                            DropDownResultsForm.DgvResults.Rows(Row - 1).Selected = True
                            If DropDownResultsForm.DgvResults.Rows(Row - 1).Displayed = False Then
                                DropDownResultsForm.DgvResults.FirstDisplayedScrollingRowIndex = DropDownResultsForm.DgvResults.SelectedRows(0).Index
                            End If
                        End If
                    Case Is = Keys.Home
                        If DropDownResultsForm.DgvResults.Visible = True Then
                            DropDownResultsForm.DgvResults.Rows(0).Selected = True
                            If DropDownResultsForm.DgvResults.Rows(0).Displayed = False Then
                                DropDownResultsForm.DgvResults.FirstDisplayedScrollingRowIndex = 0
                            End If
                        End If
                    Case Is = Keys.End
                        If DropDownResultsForm.DgvResults.Visible = True Then
                            DropDownResultsForm.DgvResults.Rows(DropDownResultsForm.DgvResults.Rows.Count - 1).Selected = True
                            If DropDownResultsForm.DgvResults.SelectedRows(0).Index = DropDownResultsForm.DgvResults.Rows.Count - 1 Then
                                DropDownResultsForm.DgvResults.FirstDisplayedScrollingRowIndex = DropDownResultsForm.DgvResults.SelectedRows(0).Index
                                Row += 1
                            Else
                                Row += 2
                            End If
                            If DropDownResultsForm.DgvResults.Rows(Row).Displayed = False Then
                                If Row >= 3 Then
                                    DropDownResultsForm.DgvResults.FirstDisplayedScrollingRowIndex = DropDownResultsForm.DgvResults.SelectedRows(0).Index - 2
                                End If
                            End If
                        End If
                End Select
            End If
        End If
    End Sub
    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)
        If QueryEnabled Then
            Select Case e.KeyCode
                Case Is = Keys.Enter
                    e.SuppressKeyPress = True
                Case Is = Keys.Down
                    e.Handled = True
                Case Is = Keys.Up
                    e.Handled = True
                Case Is = Keys.Home, Keys.End
                    If DropDownResultsForm IsNot Nothing AndAlso DropDownResultsForm.DgvResults.SelectedRows.Count = 1 Then e.Handled = True
            End Select
        End If
    End Sub
    Protected Overrides Sub OnKeyUp(e As KeyEventArgs)
        MyBase.OnKeyUp(e)
        If QueryEnabled Then
            FormatTextBox(False)
        End If
    End Sub
    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        MyBase.OnMouseClick(e)
        If QueryEnabled Then
            If _IsHyperLink AndAlso e.Button = MouseButtons.Left Then
                RaiseEvent HyperlinkClicked(Me, EventArgs.Empty)
                FormatTextBox(False)
            End If
        End If
    End Sub
#End Region
#Region "PUBLIC SUBS"
    ''' <summary>
    ''' Congela manualmente um registro usando as propriedades definidas no controle.
    ''' </summary>
    ''' <param name="ID">ID do regristro a ser selecionado no banco de dados.</param>
    ''' <param name="FreezeDependents">Especifica se os Dependentes desse controle serão congelados.</param>
    Public Sub Freeze(ByVal ID As Long, Optional ByVal FreezeDependents As Boolean = False)
        Dim OldPrimaryKey As Long = _FreezedPrimaryKey
        Dim Query As String
        Dim TableResults As New DataTable
        If QueryEnabled Then
            If JoinTable = Nothing Then
                Query = String.Format("SELECT {0}.{1} FROM {2} WHERE {3}.ID = @ID", MainTable, MainField, MainTable, MainTable)
            Else
                Query = String.Format("SELECT {0}.{1} FROM {2} JOIN {3} ON {4}.{5} = {6}.{7} WHERE {8}.ID = @ID", JoinTable, JoinField,
                                      MainTable, JoinTable, JoinTable, JoinPKField, MainTable, MainPKField, MainTable)
            End If
            TableResults = ExecuteQuery(Query, New Dictionary(Of String, Object) From {{"@ID", ID}})
            If TableResults.Rows.Count = 1 Then
                QueryEnabled = False
                Text = TableResults.Rows(0).Item(If(JoinTable = Nothing, MainField, JoinField)).ToString
                ForeColor = FreezeColor
                Me.Select(Me.TextLength, 0)
                _IsFreezed = True
                _CtrlHyperLink = True
                _FreezedPrimaryKey = ID
                _FreezedValue = TableResults.Rows(0).Item(If(JoinTable = Nothing, MainField, JoinField)).ToString
                QueryEnabled = True
                If FreezeDependents Then
                    For i = 0 To Dependents.Count - 1
                        If Dependents(i).JoinTable = Nothing Then
                            Query = String.Format("SELECT {0}.{1} FROM {2} WHERE {3}.ID = @ID", Dependents(i).MainTable, Dependents(i).MainField, Dependents(i).MainTable, Dependents(i).MainTable)
                        Else
                            Query = String.Format("SELECT {0}.{1} FROM {2} JOIN {3} ON {4}.{5} = {6}.{7} WHERE {8}.ID = @ID", Dependents(i).JoinTable, Dependents(i).JoinField,
                                      Dependents(i).MainTable, Dependents(i).JoinTable, Dependents(i).JoinTable, Dependents(i).JoinPKField, Dependents(i).MainTable,
                                      Dependents(i).MainPKField, Dependents(i).MainTable)
                        End If
                        TableResults = ExecuteQuery(Query, New Dictionary(Of String, Object) From {{"@ID", ID}})
                        If TableResults.Rows.Count = 1 Then
                            If Dependents(i).QueryEnabled Then
                                Dependents(i).QueryEnabled = False
                                Dependents(i).Text = TableResults.Rows(0).Item(If(Dependents(i).JoinTable = Nothing, Dependents(i).MainField, Dependents(i).JoinField)).ToString
                                Dependents(i).ForeColor = Dependents(i).FreezeColor
                                Dependents(i)._FreezedValue = TableResults.Rows(0).Item(If(Dependents(i).JoinTable = Nothing, Dependents(i).MainField, Dependents(i).JoinField)).ToString
                                Dependents(i)._IsFreezed = True
                                Dependents(i)._FreezedPrimaryKey = ID
                                Dependents(i)._CtrlHyperLink = True
                                Dependents(i).QueryEnabled = True
                            End If
                        End If
                    Next i
                End If
                CloseDropDown()
                If _FreezedPrimaryKey <> OldPrimaryKey Then
                    RaiseEvent FreezedPrimaryKeyChanged(Me, EventArgs.Empty)
                    If FreezeDependents Then
                        For i = 0 To Dependents.Count - 1
                            Dependents(i).RaiseFreezedPrimaryKeyChanged()
                        Next i
                    End If
                End If
            Else
                Unfreeze(True)
            End If
        End If
    End Sub
    ''' <summary>
    ''' Seleciona manualmente um registro espcificando ignorando as propredades definidas no controle.
    ''' </summary>
    ''' <param name="Table">Nome da tabela do banco de dados.</param>
    ''' <param name="Field">Nome do campo do banco de dados.</param>
    ''' <param name="ID">ID do regristro a ser selecionado no banco de dados.</param>
    Public Sub Freeze(ByVal Table As String, ByVal Field As String, ByVal ID As Long)
        Dim OldPrimaryKey As Long = _FreezedPrimaryKey
        Dim Query As String
        If QueryEnabled Then
            Query = String.Format("SELECT {0} FROM {1} WHERE ID = @ID", Field, Table, ID)
            Dim TableResults As DataTable = ExecuteQuery(Query, New Dictionary(Of String, Object) From {{"@ID", ID}})
            If TableResults.Rows.Count = 1 Then
                QueryEnabled = False
                Text = TableResults.Rows(0).Item(Field).ToString
                ForeColor = FreezeColor
                Me.Select(Me.TextLength, 0)
                _IsFreezed = True
                _CtrlHyperLink = True
                _FreezedPrimaryKey = ID
                QueryEnabled = True
                CloseDropDown()
                If _FreezedPrimaryKey <> OldPrimaryKey Then
                    RaiseEvent FreezedPrimaryKeyChanged(Me, EventArgs.Empty)
                End If
            Else
                Unfreeze(True)
            End If
        End If
    End Sub
    ''' <summary>
    ''' Descongela manualmente um registro.
    ''' </summary>
    ''' <param name="UnFreezeDependents">Especifica se os Dependentes desse controle serão descongelados.</param>
    Public Sub Unfreeze(Optional ByVal UnFreezeDependents As Boolean = False)
        Dim OldPrimaryKey As Long
        If QueryEnabled Then
            QueryEnabled = False
            OldPrimaryKey = _FreezedPrimaryKey
            Text = Nothing
            ForeColor = UnFreezeColor
            _FreezedValue = Nothing
            _IsFreezed = False
            _FreezedPrimaryKey = 0
            QueryEnabled = True
            If UnFreezeDependents Then
                For i = 0 To Dependents.Count - 1
                    If Dependents(i).QueryEnabled Then
                        Dependents(i).QueryEnabled = False
                        Dependents(i).Text = Nothing
                        Dependents(i).ForeColor = UnFreezeColor
                        Dependents(i)._FreezedValue = Nothing
                        Dependents(i)._IsFreezed = False
                        Dependents(i)._FreezedPrimaryKey = 0
                        Dependents(i).QueryEnabled = True
                    End If
                Next i
            End If
            CloseDropDown()
            If _FreezedPrimaryKey <> OldPrimaryKey Then
                RaiseEvent FreezedPrimaryKeyChanged(Me, EventArgs.Empty)
                If UnFreezeDependents Then
                    For i = 0 To Dependents.Count - 1
                        Dependents(i).RaiseFreezedPrimaryKeyChanged()
                    Next i
                End If
            End If
        End If
    End Sub
#End Region
#Region "PRIVATE SUBS"
    Private Sub RaiseFreezedPrimaryKeyChanged()
        RaiseEvent FreezedPrimaryKeyChanged(Me, EventArgs.Empty)
    End Sub
    Private Sub AutoFreeze()
        Dim OldPrimaryKey As Long
        If QueryEnabled Then
            OldPrimaryKey = _FreezedPrimaryKey
            If DropDownResultsForm IsNot Nothing AndAlso DropDownResultsForm.DgvResults.SelectedRows.Count = 1 Then
                QueryEnabled = False
                Text = DropDownResultsForm.DgvResults.SelectedRows(0).Cells(FieldHeader).Value.ToString
                Me.Select(Me.TextLength, 0)
                ForeColor = FreezeColor
                _FreezedValue = Text
                _FreezedPrimaryKey = DropDownResultsForm.DgvResults.SelectedRows(0).Cells("MAINID").Value.ToString
                _IsFreezed = True
                _CtrlHyperLink = True
                QueryEnabled = True
                For i = 0 To Dependents.Count - 1
                    If Dependents(i).QueryEnabled Then
                        Dependents(i).QueryEnabled = False
                        Dependents(i).Text = DropDownResultsForm.DgvResults.SelectedRows(0).Cells(Dependents(i).FieldHeader).Value.ToString
                        Dependents(i).ForeColor = Dependents(i).FreezeColor
                        Dependents(i)._FreezedValue = DropDownResultsForm.DgvResults.SelectedRows(0).Cells(Dependents(i).FieldHeader).Value.ToString
                        Dependents(i)._IsFreezed = True
                        Dependents(i)._FreezedPrimaryKey = DropDownResultsForm.DgvResults.SelectedRows(0).Cells("MAINID").Value.ToString
                        Dependents(i)._CtrlHyperLink = True
                        Dependents(i).QueryEnabled = True
                    End If
                Next i
                CloseDropDown()
                If _FreezedPrimaryKey <> OldPrimaryKey Then
                    RaiseEvent FreezedPrimaryKeyChanged(Me, EventArgs.Empty)
                    For i = 0 To Dependents.Count - 1
                        Dependents(i).RaiseFreezedPrimaryKeyChanged()
                    Next
                End If
            End If
        End If
    End Sub
    Private Sub AutoUnfreeze()
        Dim OldPrimaryKey As Long
        If QueryEnabled Then
            OldPrimaryKey = _FreezedPrimaryKey
            QueryEnabled = False
            ForeColor = UnFreezeColor
            _FreezedValue = Nothing
            _IsFreezed = False
            _FreezedPrimaryKey = 0
            _CtrlHyperLink = False
            QueryEnabled = True
            If Dependents IsNot Nothing Then
                For i = 0 To Dependents.Count - 1
                    If Dependents(i).QueryEnabled Then
                        Dependents(i).QueryEnabled = False
                        Dependents(i).Text = Nothing
                        Dependents(i).ForeColor = Dependents(i).UnFreezeColor
                        Dependents(i)._FreezedValue = Nothing
                        Dependents(i)._IsFreezed = False
                        Dependents(i)._FreezedPrimaryKey = 0
                        Dependents(i)._CtrlHyperLink = False
                        Dependents(i).QueryEnabled = True
                    End If
                Next i
                If _FreezedPrimaryKey <> OldPrimaryKey Then
                    RaiseEvent FreezedPrimaryKeyChanged(Me, EventArgs.Empty)
                    For i = 0 To Dependents.Count - 1
                        Dependents(i).RaiseFreezedPrimaryKeyChanged()
                    Next i
                End If
            End If
        End If
    End Sub
    Private Sub CloseDropDown()
        If DropDownResultsForm IsNot Nothing Then
            DropDownResultsForm.Close()
            DropDownResultsForm = Nothing
        End If
    End Sub
    Private Sub FormatTextBox(ByVal ShowAsLink As Boolean)
        If Not _CtrlHyperLink Then Return
        If ShowAsLink Then
            Font = New Font(Font, FontStyle.Underline)
            Cursor = Cursors.Hand
            For i = 0 To Dependents.Count - 1
                Dependents(i).Font = New Font(Dependents(i).Font, FontStyle.Underline)
                Dependents(i).Cursor = Cursors.Hand
            Next i
            _IsHyperLink = True
        Else
            Font = New Font(Font, FontStyle.Regular)
            Cursor = Cursors.IBeam
            For i = 0 To Dependents.Count - 1
                Dependents(i).Font = New Font(Dependents(i).Font, FontStyle.Regular)
                Dependents(i).Cursor = Cursors.IBeam
            Next i
            _IsHyperLink = False
        End If
    End Sub
    Private Sub Form_Deactivate(sender As Object, e As EventArgs)
        If DropDownResultsForm IsNot Nothing AndAlso DropDownResultsForm.DgvResults.SelectedRows.Count = 1 Then
            If UCase(Text) = UCase(DropDownResultsForm.DgvResults.SelectedRows(0).Cells(FieldHeader).Value.ToString) Then
                AutoFreeze()
            End If
        End If
        CloseDropDown()
    End Sub
    Private Sub DropDownResultsForm_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs)
        DropDownResultsForm.Dispose()
        DropDownResultsForm = Nothing
    End Sub
    Private Sub Timer_Tick(sender As Object, e As EventArgs) Handles Timer.Tick
        Dim FullQuery As New List(Of String)
        Dim Joins As New List(Of String)
        Dim TableResults As New DataTable
        Dim Param As New Dictionary(Of String, Object)
        Dim IsParameter As Boolean = False
        Timer.Interval = QueryInterval
        Timer.Stop()
        ValidateTick()
        FullQuery.Add(String.Format("SELECT {0}.{1} AS 'MAINID',", MainTable, MainPKField))
        If JoinTable = Nothing Then
            FullQuery.Add(String.Format("{0}.{1} AS '{2}'", MainTable, MainField, If(FieldHeader = Nothing, MainField, FieldHeader)))
        Else
            FullQuery.Add(String.Format("{0}.{1} AS '{2}'", JoinTable, JoinField, If(FieldHeader = Nothing, JoinField, FieldHeader)))
            Joins.Add(String.Format("JOIN {0} ON {1}.{2} = {3}.{4}", JoinTable, JoinTable, JoinPKField, MainTable, MainField))
        End If

        If Dependents.Count > 0 Or OtherFields.Count > 0 Then
            FullQuery(FullQuery.Count - 1) += ","
        End If

        For i = 0 To Dependents.Count - 1
            If Dependents(i).JoinTable = Nothing Then
                FullQuery.Add(String.Format("{0}.{1} AS '{2}'", Dependents(i).MainTable, Dependents(i).MainField, If(Dependents(i).FieldHeader = Nothing, Dependents(i).MainField, Dependents(i).FieldHeader)))
            Else
                FullQuery.Add(String.Format("{0}.{1} AS '{2}'", Dependents(i).JoinTable, Dependents(i).JoinField, If(Dependents(i).FieldHeader = Nothing, Dependents(i).MainField, Dependents(i).FieldHeader)))
                Joins.Add(String.Format("JOIN {0} ON {1}.{2} = {3}.{4}", Dependents(i).JoinTable, Dependents(i).JoinTable, Dependents(i).JoinPKField, Dependents(i).MainTable, Dependents(i).MainField))
            End If
            If (i <> Dependents.Count - 1) Or (i = Dependents.Count - 1 And OtherFields.Count > 0) Then
                FullQuery(FullQuery.Count - 1) += ","
            End If
        Next

        For i = 0 To OtherFields.Count - 1
            If OtherFields(i).JoinTable = Nothing Then
                FullQuery.Add(String.Format("{0}.{1} AS '{2}'", MainTable, OtherFields(i).MainField, If(OtherFields(i).FieldHeader = Nothing, OtherFields(i).MainField, OtherFields(i).FieldHeader)))
            Else
                FullQuery.Add(String.Format("{0}.{1} AS '{2}'", OtherFields(i).JoinTable, OtherFields(i).JoinField, If(OtherFields(i).FieldHeader = Nothing, OtherFields(i).JoinField, OtherFields(i).FieldHeader)))
                Joins.Add(String.Format("JOIN {0} ON {1}.{2} = {3}.{4}", OtherFields(i).JoinTable, OtherFields(i).JoinTable, OtherFields(i).JoinPKField, MainTable, OtherFields(i).MainField))
            End If

            If i <> OtherFields.Count - 1 Then
                FullQuery(FullQuery.Count - 1) += ","
            End If
        Next i
        FullQuery.Add(String.Format("FROM {0}", MainTable))
        For i = 0 To Joins.Count - 1
            FullQuery.Add(Joins(i))
        Next i


        If JoinTable = Nothing Then

            FullQuery.Add(String.Format("WHERE {0}.{1} LIKE @VALUE", MainTable, MainField))
        Else

            FullQuery.Add(String.Format("WHERE {0}.{1} LIKE @VALUE", JoinTable, JoinField))
        End If



        'nao pode usar os parametros reservados, impedir que o usuario utilize
        For i = 0 To Conditions.Count - 1
            For j = 0 To Parameters.Count - 1
                If Conditions(i).Operator = "BETWEEN" Then
                    If Conditions(i).Value <> Nothing Then
                        If Conditions(i).Value.Split(";").ElementAt(0) = Parameters(j).ParameterName Or Conditions(i).Value.Split(";").ElementAt(1) = Parameters(j).ParameterName Then
                            IsParameter = True
                        End If
                    End If
                Else
                    If Conditions(i).Value = Parameters(j).ParameterName Then
                        IsParameter = True
                    End If
                End If
            Next j

            If Conditions(i).Operator = "BETWEEN" Then
                If Conditions(i).Value = Nothing Then
                    FullQuery.Add(String.Format("AND {0}.{1} {2} {3} AND {4}", Conditions(i).TableName, Conditions(i).FieldName, Conditions(i).Operator, Nothing, Nothing))
                Else
                    FullQuery.Add(String.Format("AND {0}.{1} {2} {3} AND {4}", Conditions(i).TableName, Conditions(i).FieldName, Conditions(i).Operator, Conditions(i).Value.Split(";").ElementAt(0), Conditions(i).Value.Split(";").ElementAt(1)))
                End If
            Else
                FullQuery.Add(String.Format("AND {0}.{1} {2} {3}", Conditions(i).TableName, Conditions(i).FieldName, Conditions(i).Operator, Conditions(i).Value))
            End If
        Next i
        FullQuery.Add(String.Format("LIMIT {0}", Limit))
        Param.Add("@VALUE", Text & "%")
        For i = 0 To Parameters.Count - 1
            Param.Add(Parameters(i).ParameterName, Parameters(i).ParameterValue)
        Next i
        Try
            TableResults = ExecuteQuery(Join(FullQuery.ToArray), Param)
            If DropDownResultsForm IsNot Nothing Then
                DropDownResultsForm.DgvResults.DataSource = TableResults
                DropDownResultsForm.DgvResults.Columns("MAINID").Visible = False
                If DropDownAutoStretchRight Then
                    For Each c In DropDownResultsForm.DgvResults.Controls
                        If c.GetType() Is GetType(HScrollBar) Then
                            Dim vbar As HScrollBar = DirectCast(c, HScrollBar)
                            If vbar.Visible = True AndAlso DropDownResultsForm.DgvResults.Rows.Count > 0 Then
                                Do Until vbar.Visible = False
                                    DropDownResultsForm.Width += 10
                                Loop
                            End If
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            CloseDropDown()
            Throw ex
        End Try
    End Sub
#End Region
#Region "PRIVATE FUNCTIONS"
    Private Sub ValidateTick()
        Dim FieldHeaders As New List(Of String)
        If DefaultConnectionString = Nothing And ConnectionString = Nothing Then
            DropDownResultsForm.Close() : DropDownResultsForm = Nothing
            Throw New Exception("A propriedade ConnectionString não foi definida.")
        End If
        If DefaultDbProvider = Nothing And DbProvider = Nothing Then
            DropDownResultsForm.Close() : DropDownResultsForm = Nothing
            Throw New Exception("A propriedade DbProvider não foi definida.")
        End If

        If MainTable = Nothing Then
            DropDownResultsForm.Close() : DropDownResultsForm = Nothing
            Throw New Exception("A propriedade MainTable não foi definida.")
        End If
        If MainPKField = Nothing Then
            DropDownResultsForm.Close() : DropDownResultsForm = Nothing
            Throw New Exception("A propriedade MainPKField não foi definida.")
        End If
        If MainField = Nothing Then
            DropDownResultsForm.Close() : DropDownResultsForm = Nothing
            Throw New Exception("A propriedade MainField não foi definida.")
        End If
        If FieldHeader = Nothing Then
            DropDownResultsForm.Close() : DropDownResultsForm = Nothing
            Throw New Exception("A propriedade FieldHeader não foi definida.")
        End If
        If JoinTable <> Nothing Then
            If JoinField = Nothing Or JoinPKField = Nothing Or MainField = Nothing Then
                DropDownResultsForm.Close() : DropDownResultsForm = Nothing
                Throw New Exception("As propriedades Join devem estar todas vazias ou todas preenchidas.")
            End If
        End If
        If JoinField <> Nothing Then
            If JoinTable = Nothing Or JoinPKField = Nothing Or MainField = Nothing Then
                DropDownResultsForm.Close() : DropDownResultsForm = Nothing
                Throw New Exception("As propriedades Join devem estar todas vazias ou todas preenchidas.")
            End If
        End If
        If JoinPKField <> Nothing Then
            If JoinTable = Nothing Or JoinField = Nothing Or MainField = Nothing Then
                DropDownResultsForm.Close() : DropDownResultsForm = Nothing
                Throw New Exception("As propriedades Join devem estar todas vazias ou todas preenchidas.")
            End If
        End If
        FieldHeaders.Add(FieldHeader)
        For i = 0 To Dependents.Count - 1
            FieldHeaders.Add(Dependents(i).FieldHeader)
        Next
        If FieldHeaders.Count <> FieldHeaders.Distinct.Count Then
            DropDownResultsForm.Close() : DropDownResultsForm = Nothing
            Throw New Exception("Existe mais de um controle com o mesmo valor para a propriedade FieldHeader.")
        End If


        For i = 0 To OtherFields.Count - 1
            If OtherFields(i).FieldHeader = Nothing Then
                DropDownResultsForm.Close() : DropDownResultsForm = Nothing
                Throw New Exception("Existe OtherField com a propriedade FieldHeader não definida.")
            End If
            If OtherFields(i).JoinTable <> Nothing Then
                If OtherFields(i).JoinField = Nothing Or OtherFields(i).JoinPKField = Nothing Or OtherFields(i).MainField = Nothing Then
                    DropDownResultsForm.Close() : DropDownResultsForm = Nothing
                    Throw New Exception("Existe OtherField com propriedade do Join não definida.")
                End If
            End If
            If OtherFields(i).JoinField <> Nothing Then
                If OtherFields(i).JoinTable = Nothing Or OtherFields(i).JoinPKField = Nothing Or OtherFields(i).MainField = Nothing Then
                    DropDownResultsForm.Close() : DropDownResultsForm = Nothing
                    Throw New Exception("Existe OtherField com propriedade do Join não definida.")
                End If
            End If
            If OtherFields(i).JoinPKField <> Nothing Then
                If OtherFields(i).JoinTable = Nothing Or OtherFields(i).JoinField = Nothing Or OtherFields(i).MainField = Nothing Then
                    DropDownResultsForm.Close() : DropDownResultsForm = Nothing
                    Throw New Exception("Existe OtherField com propriedade do Join não definida.")
                End If
            End If
        Next i
        Dim ParametersNames As New List(Of String)
        For i = 0 To Parameters.Count - 1
            If Parameters(i).ParameterName = Nothing Then
                DropDownResultsForm.Close() : DropDownResultsForm = Nothing
                ParametersNames.Clear()
                Throw New Exception("Existe Parameter com a propriedade ParameterName não definida.")
            Else
                ParametersNames.Add(Parameters(i).ParameterName)
            End If
        Next
        If ParametersNames.Count <> ParametersNames.Distinct.Count Then
            DropDownResultsForm.Close() : DropDownResultsForm = Nothing
            Throw New Exception("Existe Parameter com a propriedade ParameterName duplicada.")
            Exit Sub
        End If
        For i = 0 To Conditions.Count - 1
            If Conditions(i).TableName = Nothing Then
                DropDownResultsForm.Close() : DropDownResultsForm = Nothing
                Throw New Exception("Existe Condition com a propriedade TableName não definida.")
                Exit Sub
            End If

            If Conditions(i).FieldName = Nothing Then
                DropDownResultsForm.Close() : DropDownResultsForm = Nothing
                Throw New Exception("Existe Condition com a propriedade FieldName não definida.")
                Exit Sub
            End If
            If Conditions(i).Operator = "BETWEEN" And Conditions(i).Value <> Nothing AndAlso Conditions(i).Value.Split(";").Count <> 2 Then
                DropDownResultsForm.Close() : DropDownResultsForm = Nothing
                Throw New Exception("Existe Condition com a propriedade Value não definida para o operador BETWEEN.")
                Exit Sub
            End If
        Next i
    End Sub
    Private Function ExecuteQuery(ByVal Query As String, Optional ByVal Parameters As Dictionary(Of String, Object) = Nothing) As DataTable
        Dim Table As New DataTable
        Dim Par As DbParameter
        Dim Factory As DbProviderFactory = DbProviderFactories.GetFactory(If(DbProvider = Nothing, DefaultDbProvider, DbProvider))
        Using Con As DbConnection = Factory.CreateConnection
            Con.ConnectionString = DefaultConnectionString
            Using Com As DbCommand = Con.CreateCommand
                Com.CommandText = Query
                Com.Connection = Con
                If Parameters IsNot Nothing Then
                    For Each P In Parameters
                        Par = Com.CreateParameter
                        Par.ParameterName = P.Key
                        Par.Value = P.Value
                        Com.Parameters.Add(Par)
                    Next P
                End If
                Using Adp As DbDataAdapter = Factory.CreateDataAdapter
                    Con.Open()
                    Try
                        Adp.SelectCommand = Com
                        Adp.Fill(Table)
                    Catch ex As Exception
                        If DropDownResultsForm IsNot Nothing Then
                            DropDownResultsForm.Close()
                            DropDownResultsForm = Nothing
                        End If
                        Throw ex
                    Finally
                        Con.Close()
                    End Try
                    Return Table
                End Using
            End Using
        End Using
    End Function
#End Region
#Region "PUBLIC FUNCTIONS"
    Public Function DropDownVisible() As Boolean
        If DropDownResultsForm Is Nothing Then
            Return False
        Else
            If DropDownResultsForm.Visible Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
#End Region
#Region "INTERNAL CLASSES"
    Private Class WhereFilterCollection
        Inherits StringConverter
        Public Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
            Return True
        End Function
        Public Overrides Function GetStandardValuesExclusive(ByVal context As ITypeDescriptorContext) As Boolean
            Return True
        End Function
        Public Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
            Dim WhereTable As New List(Of String) From {
                "Main Table",
                "Join Table"
            }
            Return New StandardValuesCollection(WhereTable)
        End Function
    End Class
    Public Class OtherField
        ''' <summary>
        ''' Define o nome do campo a ser pesquisado.
        ''' </summary>
        <Description("Define o nome do campo a ser pesquisado.")>
        Public Property MainField As String
        ''' <summary>
        ''' Define o apelido do campo a ser pesquisado (substitui o nome do campo no título dos resultados).
        ''' </summary>
        <Description("Define o apelido do campo a ser pesquisado (substitui o nome do campo no título dos resultados).")>
        Public Property FieldHeader As String
        ''' <summary>
        ''' Define o nome do campo a ser retornado referente ao campo da tabela principal.
        ''' </summary>
        <Description("Define o nome do campo a ser retornado referente ao campo da tabela principal.")>
        Public Property JoinField As String
        ''' <summary>
        ''' Define o nome do campo da tabela que está atribuído como PRIMARYKEY.
        ''' </summary>
        <Description("Define o nome do campo da tabela que está atribuído como PRIMARYKEY.")>
        Public Property JoinPKField As String
        ''' <summary>
        ''' Define o nome da tabela a ser combinada com a tabela principal.
        ''' </summary>
        <Description("Define o nome da tabela a ser combinada com a tabela principal.")>
        Public Property JoinTable As String
        Public Sub New()
        End Sub
        Public Sub New(ByVal FieldName As String, ByVal FieldHeader As String, Optional ByVal JoinTable As String = Nothing, Optional ByVal JoinField As String = Nothing, Optional ByVal JoinPKField As String = Nothing)
            Me.MainField = FieldName
            Me.FieldHeader = FieldHeader
            Me.JoinTable = JoinTable
            Me.JoinField = JoinField
            Me.JoinPKField = JoinPKField
        End Sub
        Public Overrides Function ToString() As String
            If FieldHeader = Nothing Then
                Return "New Undefined " & MyBase.GetType.Name
            End If
            If JoinTable <> Nothing Then
                If JoinField = Nothing Or JoinPKField = Nothing Or MainField = Nothing Then
                    Return "New Undefined " & MyBase.GetType.Name
                End If
            End If
            If JoinField <> Nothing Then
                If JoinTable = Nothing Or JoinPKField = Nothing Or MainField = Nothing Then
                    Return "New Undefined " & MyBase.GetType.Name
                End If
            End If
            If JoinPKField <> Nothing Then
                If JoinTable = Nothing Or JoinField = Nothing Or MainField = Nothing Then
                    Return "New Undefined " & MyBase.GetType.Name
                End If
            End If
            If JoinTable <> Nothing And JoinField <> Nothing And JoinPKField <> Nothing And MainField <> Nothing Then
                Return If(FieldHeader <> Nothing, JoinTable & "." & JoinField & " AS " & FieldHeader, JoinTable & "." & JoinField & " AS " & JoinField)
            End If
            If MainField <> Nothing Then
                Return If(FieldHeader <> Nothing, MainField & " AS " & FieldHeader, MainField & " AS " & MainField)
            End If
            Return "New Undefined " & MyBase.GetType.Name
        End Function
    End Class
    Public Class Parameter
        ''' <summary>
        ''' Define o nome do parâmetro utilizado na Query.
        ''' </summary>
        <Description("Define o nome do parâmetro utilizado nas condições da Query.")>
        Public Property ParameterName As String
        ''' <summary>
        ''' Define o valor do parâmetro utilizado na Query.
        ''' </summary>
        <Description("Define o valor do parâmetro utilizado nas condições da Query.")>
        Public Property ParameterValue As String
        Public Sub New()
        End Sub
        Public Sub New(ByVal ParameterName As String, ByVal ParameterValue As String)
            Me.ParameterName = ParameterName
            Me.ParameterValue = ParameterValue
        End Sub
        Public Overrides Function ToString() As String
            If ParameterName <> Nothing And ParameterValue <> Nothing Then
                Return ParameterName & " = " & ParameterValue
            ElseIf ParameterName <> Nothing And ParameterValue = Nothing Then
                Return ParameterName & " = Nothing"
            Else
                Return "New Undefined" & MyBase.GetType.Name
            End If
        End Function
    End Class
    Public Class Condition
        Private Shared _OperatorList As New List(Of String) From {
                "=",
                "<>",
                ">",
                ">=",
                "<",
                "<=",
                "BETWEEN",
                "LIKE"
            }
        ''' <summary>
        ''' Define o nome do campo do banco de dados onde será aplicada a condição.
        ''' </summary>
        <Description("Define o nome do campo do banco de dados onde será aplicada a condição.")>
        Public Property TableName As String
        ''' <summary>
        ''' Define o nome do campo do banco de dados onde será aplicada a condição.
        ''' </summary>
        <Description("Define o nome do campo do banco de dados onde será aplicada a condição.")>
        Public Property FieldName As String
        ''' <summary>
        ''' Define o operador da condição. Para o operador BETWEEN, separar os dois valores por ponto e vírgula (;).
        ''' </summary>
        <Description("Define o operador da condição. Para o operador BETWEEN, separar os dois valores por ponto e vírgula (;).")>
        <TypeConverter(GetType(OperatorFilterCollection))>
        Public Property [Operator] As String
        ''' <summary>
        ''' Define o valor a ser testado na condição.
        ''' </summary>
        <Description("Define o valor a ser testado na condição.")>
        Public Property Value As String
        Public Sub New()
        End Sub
        Public Sub New(ByVal FieldName As String, ByVal Operador As String, ByVal Value As String)
            Me.FieldName = FieldName
            Me.Operator = [Operator]
            Me.Value = Value
        End Sub
        Public Overrides Function ToString() As String
            If TableName <> Nothing And FieldName <> Nothing And [Operator] <> Nothing And Value <> Nothing Then
                If [Operator] = "BETWEEN" Then
                    If Value.Split(";").Count = 2 Then
                        Return String.Format("{0}.{1} {2} {3} AND {3}", TableName, FieldName, [Operator], Value.Split(";").ElementAt(0), Value.Split(";").ElementAt(1))
                    Else
                        Return "New Undefined" & MyBase.GetType.Name
                    End If
                Else
                    Return String.Format("{0}.{1} {2} {3}", TableName, FieldName, [Operator], Value)
                End If
            ElseIf TableName <> Nothing And FieldName <> Nothing And [Operator] <> Nothing And Value = Nothing Then
                If [Operator] = "BETWEEN" Then
                    Return String.Format("{0}.{1} {2} {3} AND {4}", TableName, FieldName, [Operator], "Nothing", "Nothing")
                Else
                    Return String.Format("{0}.{1} {2} {3}", TableName, FieldName, [Operator], "Nothing")
                End If
            Else
                Return "New Undefined" & MyBase.GetType.Name
            End If
        End Function
        Class OperatorFilterCollection
            Inherits StringConverter
            Public Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
                Return True
            End Function
            Public Overrides Function GetStandardValuesExclusive(ByVal context As ITypeDescriptorContext) As Boolean
                Return True
            End Function
            Public Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
                Return New StandardValuesCollection(_OperatorList)
            End Function
        End Class
    End Class
    Private Class FormDropDownResults
        Inherits Form
        Public Textbox As Control
        Private _SearchBox As QueriedBox
        Public Sub New(ByVal SearchBox As QueriedBox)
            SuspendLayout()
            _SearchBox = SearchBox
            InitializeComponent()
            BackColor = _SearchBox.DropDownBorderColor
            Font = SearchBox.Font
            FormBorderStyle = FormBorderStyle.None
            Padding = New Padding(1)
            Size = New Size(300, 120)
            DoubleBuffered = True
            TopMost = True
            KeyPreview = True
            ResumeLayout(True)
        End Sub
        Private Sub InitializeComponent()
            PanelContainer = New Panel With {
                .Dock = DockStyle.Fill
            }
            DgvResults = New DataGridView With {
                .AllowUserToAddRows = False,
                .AllowUserToDeleteRows = False,
                .AllowUserToResizeColumns = True,
                .AllowUserToResizeRows = False,
                .AllowUserToOrderColumns = True,
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                .BackgroundColor = Color.White,
                .BorderStyle = Windows.Forms.BorderStyle.None,
                .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                .CellBorderStyle = If(_SearchBox.ShowVerticalGridLines, DataGridViewCellBorderStyle.Single, DataGridViewCellBorderStyle.SingleHorizontal),
                .ColumnHeadersBorderStyle = If(_SearchBox.ShowVerticalGridLines, DataGridViewHeaderBorderStyle.Raised, DataGridViewCellBorderStyle.None),
                .ColumnHeadersVisible = _SearchBox.GridHeaderVisible,
                .DefaultCellStyle = New DataGridViewCellStyle With {
                    .SelectionBackColor = _SearchBox.GridSelectionBackColor,
                    .SelectionForeColor = _SearchBox.GridSelectionForeColor,
                    .BackColor = _SearchBox.GridBackColor,
                    .ForeColor = _SearchBox.GridForeColor
                },
                .ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle With {
                    .BackColor = _SearchBox.GridHeaderBackColor,
                    .ForeColor = _SearchBox.GridHeaderForeColor
                },
                .Dock = DockStyle.Fill,
                .MultiSelect = False,
                .[ReadOnly] = True,
                .RowHeadersVisible = False,
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                .Visible = False,
                .EnableHeadersVisualStyles = False
            }
            DgvResults.ColumnHeadersDefaultCellStyle.Font = New Font(_SearchBox.Font, If(_SearchBox.GridHeadersBold, FontStyle.Bold, FontStyle.Regular))
            EnableDoubleBuffered(DgvResults)
            LblCharsRemaining = New Label With {
                .AutoSize = False,
                .Dock = DockStyle.Fill,
                .TextAlign = ContentAlignment.MiddleCenter,
                .BackColor = _SearchBox.LabelBackColor,
                .ForeColor = _SearchBox.LabelForeColor,
                .Visible = False
            }
            PanelContainer.Controls.AddRange({DgvResults, LblCharsRemaining})
            Controls.Add(PanelContainer)
        End Sub
        Private Sub DgvResults_DataSourceChanged(sender As Object, e As EventArgs) Handles DgvResults.DataSourceChanged
            If DgvResults.Rows.Count = 0 Then
                DgvResults.Visible = False
                LblCharsRemaining.Text = "Nenhum Registro Encontrado."
                LblCharsRemaining.Visible = True
            End If
        End Sub
        Private Sub DropDownResults_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            Application.AddMessageFilter(New PopupWindowHelperMessageFilter(Me, Textbox))
        End Sub
        Private Sub EnableDoubleBuffered(ByVal dgv As DataGridView)
            Dim DgvType As Type = dgv.[GetType]()
            Dim pi As PropertyInfo = DgvType.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
            pi.SetValue(dgv, True, Nothing)
        End Sub
        Protected Overrides ReadOnly Property CreateParams As CreateParams
            Get
                Dim ret As CreateParams = MyBase.CreateParams
                ret.Style = CInt(Flags.WindowStyles.WS_SYSMENU) Or CInt(Flags.WindowStyles.WS_CHILD)
                ret.ExStyle = ret.ExStyle Or CInt(Flags.WindowStyles.WS_EX_NOACTIVATE) Or CInt(Flags.WindowStyles.WS_EX_TOOLWINDOW)
                ret.X = Me.Location.X
                ret.Y = Me.Location.Y
                Return ret
            End Get
        End Property
        Private Sub DataGridView_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles DgvResults.PreviewKeyDown
            If e.KeyCode = Keys.Tab Then
                Close()
                Me.Select()
            End If
        End Sub
        Private Sub DataGridView_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles DgvResults.MouseDoubleClick
            Dim Click As DataGridView.HitTestInfo = DgvResults.HitTest(e.X, e.Y)
            If Click.Type = DataGridViewHitTestType.Cell Then
                _SearchBox.AutoFreeze()
                _SearchBox.Focus()
                Close()
            End If
        End Sub
        Friend WithEvents DgvResults As DataGridView
        Friend WithEvents LblCharsRemaining As Label
        Friend WithEvents PanelContainer As Panel
    End Class
    Private Class PopupWindowHelperMessageFilter
        Implements IMessageFilter
        Private Const WM_LBUTTONDOWN As Integer = &H201
        Private Const WM_RBUTTONDOWN As Integer = &H204
        Private Const WM_MBUTTONDOWN As Integer = &H207
        Private Const WM_NCLBUTTONDOWN As Integer = &HA1
        Private Const WM_NCRBUTTONDOWN As Integer = &HA4
        Private Const WM_NCMBUTTONDOWN As Integer = &HA7
        Private TextBox As Control = Nothing
        Public Property Popup As Form = Nothing
        Public Sub New(ByVal popupW As Form, ByVal textbox As Control)
            Popup = popupW
            Me.TextBox = textbox
        End Sub
        Private Sub OnMouseDown()
            Dim cursorPos As Point = Cursor.Position
            If TextBox.Parent Is Nothing Then Exit Sub
            If Not Popup.Bounds.Contains(cursorPos) Then
                If Not TextBox.Bounds.Contains(TextBox.Parent.PointToClient(cursorPos)) Then
                    If CType(TextBox, QueriedBox).DropDownResultsForm IsNot Nothing AndAlso CType(TextBox, QueriedBox).DropDownResultsForm.DgvResults.SelectedRows.Count = 1 Then
                        If UCase(CType(TextBox, QueriedBox).Text) = UCase(CType(TextBox, QueriedBox).DropDownResultsForm.DgvResults.SelectedRows(0).Cells(CType(TextBox, QueriedBox).FieldHeader).Value.ToString) Then
                            CType(TextBox, QueriedBox).AutoFreeze()
                        End If
                    End If
                    Application.RemoveMessageFilter(Me)
                    Popup.Close()
                End If
            End If
        End Sub
        Private Function IMessageFilter_PreFilterMessage(ByRef m As Message) As Boolean Implements IMessageFilter.PreFilterMessage
            If Popup IsNot Nothing Then
                Select Case m.Msg
                    Case WM_LBUTTONDOWN, WM_RBUTTONDOWN, WM_MBUTTONDOWN, WM_NCLBUTTONDOWN, WM_NCRBUTTONDOWN, WM_NCMBUTTONDOWN
                        OnMouseDown()
                End Select
            End If
            Return False
        End Function
    End Class
    Private Class Flags
        <Flags()>
        Public Enum WindowStyles As UInteger
            WS_OVERLAPPED = 0
            WS_POPUP = 2147483648
            WS_CHILD = 1073741824
            WS_MINIMIZE = 536870912
            WS_VISIBLE = 268435456
            WS_DISABLED = 134217728
            WS_CLIPSIBLINGS = 67108864
            WS_CLIPCHILDREN = 33554432
            WS_MAXIMIZE = 16777216
            WS_BORDER = 8388608
            WS_DLGFRAME = 4194304
            WS_VSCROLL = 2097152
            WS_HSCROLL = 1048576
            WS_SYSMENU = 524288
            WS_THICKFRAME = 262144
            WS_GROUP = 131072
            WS_TABSTOP = 65536
            WS_MINIMIZEBOX = 131072
            WS_MAXIMIZEBOX = 65536
            WS_CAPTION = WS_BORDER Or WS_DLGFRAME
            WS_TILED = WS_OVERLAPPED
            WS_ICONIC = WS_MINIMIZE
            WS_SIZEBOX = WS_THICKFRAME
            WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW
            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED Or WS_CAPTION Or WS_SYSMENU Or WS_THICKFRAME Or WS_MINIMIZEBOX Or WS_MAXIMIZEBOX
            WS_POPUPWINDOW = WS_POPUP Or WS_BORDER Or WS_SYSMENU
            WS_CHILDWINDOW = WS_CHILD
            WS_EX_DLGMODALFRAME = 1
            WS_EX_NOPARENTNOTIFY = 4
            WS_EX_TOPMOST = 8
            WS_EX_ACCEPTFILES = 16
            WS_EX_TRANSPARENT = 32
            WS_EX_MDICHILD = 64
            WS_EX_TOOLWINDOW = 128
            WS_EX_WINDOWEDGE = 256
            WS_EX_CLIENTEDGE = 512
            WS_EX_CONTEXTHELP = 1024
            WS_EX_RIGHT = 4096
            WS_EX_LEFT = 0
            WS_EX_RTLREADING = 8192
            WS_EX_LTRREADING = 0
            WS_EX_LEFTSCROLLBAR = 16384
            WS_EX_RIGHTSCROLLBAR = 0
            WS_EX_CONTROLPARENT = 65536
            WS_EX_STATICEDGE = 131072
            WS_EX_APPWINDOW = 262144
            WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE Or WS_EX_CLIENTEDGE
            WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE Or WS_EX_TOOLWINDOW Or WS_EX_TOPMOST
            WS_EX_LAYERED = 524288
            WS_EX_NOINHERITLAYOUT = 1048576
            WS_EX_LAYOUTRTL = 4194304
            WS_EX_COMPOSITED = 33554432
            WS_EX_NOACTIVATE = 134217728
        End Enum
    End Class
    Private Class FormOtherFieldsEditor
        Inherits Form
        Public OtherFields As New Collection(Of OtherField)
        Friend IsCancelled As Boolean = False
        Public Sub New()
            InitializeComponent()
        End Sub
        Private Sub InitializeComponent()
            LblMembers = New Label With {
                .AutoSize = True,
                .Text = "Members",
                .Location = New Point(9, 9)
            }
            LblProperties = New Label With {
                .AutoSize = True,
                .Text = "Properties",
                .Location = New Point(205, 9)
            }
            LstMembers = New ListBox With {
                .Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left,
                .Location = New Point(12, 25),
                .Size = New Size(180, 238)
            }
            PgrProperties = New PropertyGrid With {
                .Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top,
                .Location = New Point(201, 25),
                .PropertySort = PropertySort.NoSort,
                .Size = New Size(271, 267),
                .ToolbarVisible = False
            }
            BtnAdd = New Button With {
                .Anchor = AnchorStyles.Bottom Or AnchorStyles.Left,
                .Location = New Point(66, 269),
                .Size = New Size(60, 23),
                .Text = "Add"
            }
            BtnRemove = New Button With {
                .Anchor = AnchorStyles.Bottom Or AnchorStyles.Left,
                .Location = New Point(132, 269),
                .Size = New Size(60, 23),
                .Text = "Remove"
            }
            BtnCancel = New Button With {
                .Anchor = AnchorStyles.Bottom Or AnchorStyles.Right,
                .Location = New Point(346, 298),
                .Size = New Size(60, 23),
                .Text = "Cancel"
            }
            BtnOk = New Button With {
                .Anchor = AnchorStyles.Bottom Or AnchorStyles.Right,
                .Location = New Point(412, 298),
                .Size = New Size(60, 23),
                .Text = "OK"
            }
            KeyPreview = True
            MaximizeBox = False
            MinimizeBox = False
            MinimumSize = New Size(500, 250)
            ShowIcon = False
            ShowInTaskbar = False
            Size = New Size(500, 367)
            SizeGripStyle = SizeGripStyle.Hide
            StartPosition = FormStartPosition.CenterParent
            Controls.AddRange({LblMembers, LblProperties, LstMembers, PgrProperties, BtnAdd, BtnRemove, BtnCancel, BtnOk})
        End Sub
        Private Sub FormOtherFieldsEditor_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
            For i = 0 To OtherFields.Count - 1
                LstMembers.Items.Add(OtherFields(i))
            Next i
            If LstMembers.Items.Count > 0 Then LstMembers.SelectedIndex = 0
        End Sub
        Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
            LstMembers.Items.Add(New OtherField)
            LstMembers.SelectedIndex = LstMembers.Items.Count - 1
            PgrProperties.SelectedObject = LstMembers.Items(LstMembers.Items.Count - 1)
        End Sub
        Private Sub BtnRemove_Click(sender As Object, e As EventArgs) Handles BtnRemove.Click
            If LstMembers.SelectedItems.Count = 1 Then
                LstMembers.Items.RemoveAt(LstMembers.SelectedIndex)
            End If
        End Sub
        Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
            IsCancelled = True
            Hide()
        End Sub
        Private Sub BtnOK_Click(sender As Object, e As EventArgs) Handles BtnOk.Click
            Dim LstOtherFields As New List(Of String)
            For i = 0 To LstMembers.Items.Count - 1
                LstOtherFields.Add(LstMembers.Items(i).MainField)
            Next i
            OtherFields.Clear()
            For i = 0 To LstMembers.Items.Count - 1
                OtherFields.Add(LstMembers.Items(i))
            Next i
            IsCancelled = False
            Hide()
        End Sub
        Private Sub LstMembers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LstMembers.SelectedIndexChanged
            PgrProperties.SelectedObject = LstMembers.SelectedItem
        End Sub
        Private Sub PgrProperties_PropertyValueChanged(s As Object, e As PropertyValueChangedEventArgs) Handles PgrProperties.PropertyValueChanged
            Dim SelectedIndex As Integer
            Dim TempListMembers As New Collection(Of OtherField)
            For i = 0 To LstMembers.Items.Count - 1
                TempListMembers.Add(LstMembers.Items(i))
            Next i
            If LstMembers.SelectedItems.Count = 1 Then SelectedIndex = LstMembers.SelectedIndex
            LstMembers.Items.Clear()
            For i = 0 To TempListMembers.Count - 1
                LstMembers.Items.Add(TempListMembers(i))
            Next
            If LstMembers.Items.Count > 0 Then LstMembers.SelectedIndex = SelectedIndex
        End Sub
        Private WithEvents LblMembers As Label
        Private WithEvents LblProperties As Label
        Private WithEvents LstMembers As ListBox
        Private WithEvents PgrProperties As PropertyGrid
        Private WithEvents BtnAdd As Button
        Private WithEvents BtnRemove As Button
        Private WithEvents BtnCancel As Button
        Private WithEvents BtnOk As Button
    End Class
    Private Class FormParametersEditor
        Inherits Form
        Public Parameters As New Collection(Of Parameter)
        Friend IsCancelled As Boolean = False
        Public Sub New()
            InitializeComponent()
        End Sub
        Private Sub InitializeComponent()
            LblMembers = New Label With {
                .AutoSize = True,
                .Text = "Members",
                .Location = New Point(9, 9)
        }
            LblProperties = New Label With {
                .AutoSize = True,
                .Text = "Properties",
                .Location = New Point(205, 9)
        }
            LstMembers = New ListBox With {
                .Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left,
                .Location = New Point(12, 25),
                .Size = New Size(180, 238)
        }
            PgrProperties = New PropertyGrid With {
                .Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top,
                .Location = New Point(201, 25),
                .PropertySort = PropertySort.NoSort,
                .Size = New Size(271, 267),
                .ToolbarVisible = False
        }
            BtnAdd = New Button With {
                .Anchor = AnchorStyles.Bottom Or AnchorStyles.Left,
                .Location = New Point(66, 269),
                .Size = New Size(60, 23),
                .Text = "Add"
        }
            BtnRemove = New Button With {
                .Anchor = AnchorStyles.Bottom Or AnchorStyles.Left,
                .Location = New Point(132, 269),
                .Size = New Size(60, 23),
                .Text = "Remove"
        }
            BtnCancel = New Button With {
                .Anchor = AnchorStyles.Bottom Or AnchorStyles.Right,
                .Location = New Point(346, 298),
                .Size = New Size(60, 23),
                .Text = "Cancel"
        }
            BtnOk = New Button With {
                .Anchor = AnchorStyles.Bottom Or AnchorStyles.Right,
                .Location = New Point(412, 298),
                .Size = New Size(60, 23),
                .Text = "OK"
        }
            KeyPreview = True
            MaximizeBox = False
            MinimizeBox = False
            MinimumSize = New Size(500, 250)
            ShowIcon = False
            ShowInTaskbar = False
            Size = New Size(500, 367)
            SizeGripStyle = SizeGripStyle.Hide
            StartPosition = FormStartPosition.CenterParent
            Controls.AddRange({LblMembers, LblProperties, LstMembers, PgrProperties, BtnAdd, BtnRemove, BtnCancel, BtnOk})
        End Sub
        Private Sub FormParametersEditor_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
            For i = 0 To Parameters.Count - 1
                LstMembers.Items.Add(Parameters(i))
            Next i
            If LstMembers.Items.Count > 0 Then LstMembers.SelectedIndex = 0
        End Sub
        Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
            LstMembers.Items.Add(New Parameter)
            LstMembers.SelectedIndex = LstMembers.Items.Count - 1
            PgrProperties.SelectedObject = LstMembers.Items(LstMembers.Items.Count - 1)
        End Sub
        Private Sub BtnRemove_Click(sender As Object, e As EventArgs) Handles BtnRemove.Click
            If LstMembers.SelectedItems.Count = 1 Then
                LstMembers.Items.RemoveAt(LstMembers.SelectedIndex)
            End If
        End Sub
        Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
            IsCancelled = True
            Hide()
        End Sub
        Private Sub BtnOK_Click(sender As Object, e As EventArgs) Handles BtnOk.Click
            Dim LstParameters As New List(Of String)
            For i = 0 To LstMembers.Items.Count - 1
                LstParameters.Add(LstMembers.Items(i).ParameterName)
            Next i
            Parameters.Clear()
            For i = 0 To LstMembers.Items.Count - 1
                Parameters.Add(LstMembers.Items(i))
            Next i
            IsCancelled = False
            Hide()
        End Sub
        Private Sub LstMembers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LstMembers.SelectedIndexChanged
            PgrProperties.SelectedObject = LstMembers.SelectedItem
        End Sub
        Private Sub PgrProperties_PropertyValueChanged(s As Object, e As PropertyValueChangedEventArgs) Handles PgrProperties.PropertyValueChanged
            Dim SelectedIndex As Integer
            Dim TempListMembers As New Collection(Of Parameter)
            For i = 0 To LstMembers.Items.Count - 1
                TempListMembers.Add(LstMembers.Items(i))
            Next i
            If LstMembers.SelectedItems.Count = 1 Then SelectedIndex = LstMembers.SelectedIndex
            LstMembers.Items.Clear()
            For i = 0 To TempListMembers.Count - 1
                LstMembers.Items.Add(TempListMembers(i))
            Next
            If LstMembers.Items.Count > 0 Then LstMembers.SelectedIndex = SelectedIndex
        End Sub
        Friend WithEvents LblMembers As Label
        Friend WithEvents LblProperties As Label
        Friend WithEvents LstMembers As ListBox
        Friend WithEvents PgrProperties As PropertyGrid
        Friend WithEvents BtnAdd As Button
        Friend WithEvents BtnRemove As Button
        Friend WithEvents BtnCancel As Button
        Friend WithEvents BtnOk As Button
    End Class
    Private Class FormConditionsEditor
        Inherits Form
        Public Conditions As New Collection(Of Condition)
        Friend IsCancelled As Boolean = False
        Public Sub New()
            InitializeComponent()
        End Sub
        Private Sub InitializeComponent()
            LblMembers = New Label With {
                .AutoSize = True,
                .Text = "Members",
                .Location = New Point(9, 9)
            }
            LblProperties = New Label With {
                .AutoSize = True,
                .Text = "Properties",
                .Location = New Point(205, 9)
            }
            LstMembers = New ListBox With {
                .Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left,
                .Location = New Point(12, 25),
                .Size = New Size(180, 238)
            }
            PgrProperties = New PropertyGrid With {
                .Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top,
                .Location = New Point(201, 25),
                .PropertySort = PropertySort.NoSort,
                .Size = New Size(271, 267),
                .ToolbarVisible = False
            }
            BtnAdd = New Button With {
                .Anchor = AnchorStyles.Bottom Or AnchorStyles.Left,
                .Location = New Point(66, 269),
                .Size = New Size(60, 23),
                .Text = "Add"
             }
            BtnRemove = New Button With {
                .Anchor = AnchorStyles.Bottom Or AnchorStyles.Left,
                .Location = New Point(132, 269),
                .Size = New Size(60, 23),
                .Text = "Remove"
            }
            BtnCancel = New Button With {
                .Anchor = AnchorStyles.Bottom Or AnchorStyles.Right,
                .Location = New Point(346, 298),
                .Size = New Size(60, 23),
                .Text = "Cancel"
            }
            BtnOk = New Button With {
                .Anchor = AnchorStyles.Bottom Or AnchorStyles.Right,
                .Location = New Point(412, 298),
                .Size = New Size(60, 23),
                .Text = "OK"
            }
            KeyPreview = True
            MaximizeBox = False
            MinimizeBox = False
            MinimumSize = New Size(500, 250)
            ShowIcon = False
            ShowInTaskbar = False
            Size = New Size(500, 367)
            SizeGripStyle = SizeGripStyle.Hide
            StartPosition = FormStartPosition.CenterParent
            Controls.AddRange({LblMembers, LblProperties, LstMembers, PgrProperties, BtnAdd, BtnRemove, BtnCancel, BtnOk})
        End Sub
        Private Sub FormConditionsEditor_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
            For i = 0 To Conditions.Count - 1
                LstMembers.Items.Add(Conditions(i))
            Next i
            If LstMembers.Items.Count > 0 Then LstMembers.SelectedIndex = 0
        End Sub
        Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
            LstMembers.Items.Add(New Condition)
            LstMembers.SelectedIndex = LstMembers.Items.Count - 1
            PgrProperties.SelectedObject = LstMembers.Items(LstMembers.Items.Count - 1)
        End Sub
        Private Sub BtnRemove_Click(sender As Object, e As EventArgs) Handles BtnRemove.Click
            If LstMembers.SelectedItems.Count = 1 Then
                LstMembers.Items.RemoveAt(LstMembers.SelectedIndex)
            End If
        End Sub
        Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
            IsCancelled = True
            Hide()
        End Sub
        Private Sub BtnOK_Click(sender As Object, e As EventArgs) Handles BtnOk.Click
            Dim LstConditions As New List(Of String)
            For i = 0 To LstMembers.Items.Count - 1
                LstConditions.Add(LstMembers.Items(i).TableName & "." & LstMembers.Items(i).FieldName)
            Next i
            Conditions.Clear()
            For i = 0 To LstMembers.Items.Count - 1
                Conditions.Add(LstMembers.Items(i))
            Next i
            IsCancelled = False
            Hide()
        End Sub
        Private Sub LstMembers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LstMembers.SelectedIndexChanged
            PgrProperties.SelectedObject = LstMembers.SelectedItem
        End Sub
        Private Sub PgrProperties_PropertyValueChanged(s As Object, e As PropertyValueChangedEventArgs) Handles PgrProperties.PropertyValueChanged
            Dim SelectedIndex As Integer
            Dim TempListMembers As New Collection(Of Condition)
            For i = 0 To LstMembers.Items.Count - 1
                TempListMembers.Add(LstMembers.Items(i))
            Next i
            If LstMembers.SelectedItems.Count = 1 Then SelectedIndex = LstMembers.SelectedIndex
            LstMembers.Items.Clear()
            For i = 0 To TempListMembers.Count - 1
                LstMembers.Items.Add(TempListMembers(i))
            Next
            If LstMembers.Items.Count > 0 Then LstMembers.SelectedIndex = SelectedIndex
        End Sub
        Friend WithEvents LblMembers As Label
        Friend WithEvents LblProperties As Label
        Friend WithEvents LstMembers As ListBox
        Friend WithEvents PgrProperties As PropertyGrid
        Friend WithEvents BtnAdd As Button
        Friend WithEvents BtnRemove As Button
        Friend WithEvents BtnCancel As Button
        Friend WithEvents BtnOk As Button
    End Class
    Private Class OtherFieldCollectionDialogUIEditor
        Inherits UITypeEditorBase
        Private WithEvents myForm As FormOtherFieldsEditor
        Protected Overrides Function GetEditControl(ByVal PropertyName As String, ByVal CurrentValue As Object) As Control
            myForm = New FormOtherFieldsEditor()
            myForm.Text = PropertyName & " Collection Editor"
            Return myForm
        End Function
        Protected Overrides Function SetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
            Return UITypeEditorEditStyle.Modal
        End Function
        Protected Overrides Function GetEditedValue(ByVal EditControl As Control, ByVal PropertyName As String, ByVal OldValue As Object) As Object
            If EditControl Is Nothing Then Return OldValue
            If myForm.IsCancelled Then
                Return OldValue
            Else
                Dim tCollection As New Collection(Of OtherField)
                If myForm.OtherFields.Count <> 0 Then
                    For Each c As OtherField In myForm.OtherFields
                        tCollection.Add(c)
                    Next
                End If
                Return tCollection
            End If
        End Function
        Protected Overrides Sub LoadValues(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object)
            Dim thisCtl As QueriedBox = Nothing
            Dim tCollection As Collection(Of OtherField) = CType(value, Collection(Of OtherField))
            For Each obj As Object In context.Container.Components.OfType(Of QueriedBox)
                thisCtl = CType(obj, QueriedBox)
                If thisCtl Is CType(context.Instance, QueriedBox) Then
                    myForm.OtherFields = thisCtl.OtherFields
                End If
            Next
        End Sub
    End Class
    Private Class ParameterCollectionDialogUIEditor
        Inherits UITypeEditorBase
        Private WithEvents myForm As FormParametersEditor
        Protected Overrides Function GetEditControl(ByVal PropertyName As String, ByVal CurrentValue As Object) As Control
            myForm = New FormParametersEditor()
            myForm.Text = PropertyName & " Collection Editor"
            Return myForm
        End Function
        Protected Overrides Function SetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
            Return UITypeEditorEditStyle.Modal
        End Function
        Protected Overrides Function GetEditedValue(ByVal EditControl As Control, ByVal PropertyName As String, ByVal OldValue As Object) As Object
            If EditControl Is Nothing Then Return OldValue
            If myForm.IsCancelled Then
                Return OldValue
            Else
                Dim tCollection As New Collection(Of Parameter)
                If myForm.Parameters.Count <> 0 Then
                    For Each c As Parameter In myForm.Parameters
                        tCollection.Add(c)
                    Next
                End If
                Return tCollection
            End If
        End Function
        Protected Overrides Sub LoadValues(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object)
            Dim thisCtl As QueriedBox = Nothing
            Dim tCollection As Collection(Of Parameter) = CType(value, Collection(Of Parameter))
            For Each obj As Object In context.Container.Components.OfType(Of QueriedBox)
                thisCtl = CType(obj, QueriedBox)
                If thisCtl Is CType(context.Instance, QueriedBox) Then
                    myForm.Parameters = thisCtl.Parameters
                End If
            Next
        End Sub
    End Class
    Private Class ConditionCollectionDialogUIEditor
        Inherits UITypeEditorBase
        Private WithEvents myForm As FormConditionsEditor
        Protected Overrides Function GetEditControl(ByVal PropertyName As String, ByVal CurrentValue As Object) As Control
            myForm = New FormConditionsEditor()
            myForm.Text = PropertyName & " Collection Editor"
            Return myForm
        End Function
        Protected Overrides Function SetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
            Return UITypeEditorEditStyle.Modal
        End Function
        Protected Overrides Function GetEditedValue(ByVal EditControl As Control, ByVal PropertyName As String, ByVal OldValue As Object) As Object
            If EditControl Is Nothing Then Return OldValue
            If myForm.IsCancelled Then
                Return OldValue
            Else
                Dim tCollection As New Collection(Of Condition)
                If myForm.Conditions.Count <> 0 Then
                    For Each c As Condition In myForm.Conditions
                        tCollection.Add(c)
                    Next
                End If
                Return tCollection
            End If
        End Function
        Protected Overrides Sub LoadValues(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object)
            Dim thisCtl As QueriedBox = Nothing
            Dim tCollection As Collection(Of Condition) = CType(value, Collection(Of Condition))
            For Each obj As Object In context.Container.Components.OfType(Of QueriedBox)
                thisCtl = CType(obj, QueriedBox)
                If thisCtl Is CType(context.Instance, QueriedBox) Then
                    myForm.Conditions = thisCtl.Conditions
                End If
            Next
        End Sub
    End Class
    Private Class DependentsDropDownUIEditor
        Inherits UITypeEditorBase
        Private List As CheckedListBox
        Protected Friend Property CheckControlWidth As Integer = 280
        Public Sub New()
            CheckControlWidth = 200
        End Sub
        Protected Overrides Function SetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
            Return UITypeEditorEditStyle.Modal
        End Function
        Protected Overrides Function GetEditControl(ByVal PropertyName As String, ByVal CurrentValue As Object) As Control
            List = New CheckedListBox
            List.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
            List.CheckOnClick = True
            List.Height = 200
            If CheckControlWidth < 400 Then
                List.Width = CheckControlWidth
            End If
            Return List
        End Function
        Protected Overrides Function GetEditedValue(ByVal EditControl As Control, ByVal PropertyName As String, ByVal OldValue As Object) As Object
            Dim tCollection As New Collection(Of QueriedBox)
            If EditControl Is Nothing Then Return OldValue
            If List.CheckedItems.Count <> 0 Then
                For n As Integer = 0 To List.CheckedItems.Count - 1
                    tCollection.Add(CType(List.CheckedItems.Item(n), ListItem).[Control])
                Next
            End If
            Return tCollection
        End Function
        Protected Overrides Sub LoadValues(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object)
            Dim QueriedTextBox As Control = Nothing
            Dim CheckedCollection As Collection(Of QueriedBox) = CType(value, Collection(Of QueriedBox))
            Dim TempCheckedCollection As New Collection(Of QueriedBox)
            For Each obj As Control In GetAllChildren(CType(context.Instance, QueriedBox).FindForm)
                If TypeOf obj Is QueriedBox Then
                    QueriedTextBox = CType(obj, QueriedBox)
                    If QueriedTextBox.Name <> context.Instance.Name Then
                        If CheckedCollection.Contains(QueriedTextBox) Then
                            TempCheckedCollection.Add(QueriedTextBox)
                        End If
                    End If
                End If
            Next obj
            CheckedCollection.Clear()
            For Each obj As Control In TempCheckedCollection
                CheckedCollection.Add(obj)
            Next
            For Each obj As Control In GetAllChildren(CType(context.Instance, QueriedBox).FindForm)
                If TypeOf obj Is QueriedBox Then
                    QueriedTextBox = CType(obj, QueriedBox)
                    Dim bCheck As Boolean
                    Dim ndx As Integer
                    bCheck = CheckedCollection.Contains(QueriedTextBox)
                    If QueriedTextBox.Name <> context.Instance.Name Then
                        ndx = List.Items.Add(New ListItem(QueriedTextBox))
                        List.SetItemChecked(ndx, bCheck)
                        List.Sorted = True
                    End If
                End If
            Next obj
        End Sub
        Private Iterator Function GetAllChildren(ByVal root As Control) As IEnumerable(Of Control)
            Dim stack = New Stack(Of Control)()
            stack.Push(root)
            While stack.Any()
                Dim [next] = stack.Pop()
                For Each child As Control In [next].Controls
                    stack.Push(child)
                Next
                Yield [next]
            End While
        End Function
    End Class
    Private Class QueriedTextBoxControlDesignerActionList
        Inherits DesignerActionList
        Private Control As QueriedBox
        Private Designer As ControlDesigner
        Private ActionList As DesignerActionList
        Public Sub New(ByVal Designer As ControlDesigner, ByVal ActionList As DesignerActionList)
            MyBase.New(Designer.Component)
            Me.Designer = Designer
            Me.ActionList = ActionList
            Control = CType(Designer.Control, QueriedBox)
        End Sub
        Public Overrides Function GetSortedActionItems() As DesignerActionItemCollection
            Dim Items = New DesignerActionItemCollection From {
                New DesignerActionPropertyItem("QueryEnabled", "QueryEnabled", "Comportamento", "Define se as pesquisas estão habilitadas"),
                New DesignerActionPropertyItem("CharactersToQuery", "Characters To Query", "Comportamento", "Define a quantidade de caracteres necessários para iniciar a pesquisa"),
                New DesignerActionPropertyItem("FieldHeader", "Field Header", "Query", "Define o apelido do campo a ser pesquisado (substitui o nome do campo no título dos resultados)"),
                New DesignerActionPropertyItem("MainTable", "Main Table", "Query", "Define o nome da tabela a ser pesquisada"),
                New DesignerActionPropertyItem("MainField", "Main Field", "Query", "Define o nome do campo a ser pesquisado"),
                New DesignerActionPropertyItem("MainPKField", "Main PrimaryKey Field", "Query", "Define o nome do campo da tabela que está atribuído como PRIMARYKEY"),
                New DesignerActionPropertyItem("JoinTable", "Join Table", "Query", "Define o nome da tabela a ser combinada com a tabela principal"),
                New DesignerActionPropertyItem("JoinField", "Join Field", "Query", "Define o nome do campo a ser retornado referente ao campo da tabela principal"),
                New DesignerActionPropertyItem("JoinPKField", "Join PrimaryKey Field", "Query", "Define o nome do campo da tabela que está atribuído como PRIMARYKEY"),
                New DesignerActionPropertyItem("Limit", "Limit", "Query", "Define o máximo de resultados que podem ser retornados pela pesquisa")
            }
            Return Items
        End Function

        Public Property QueryEnabled As Boolean
            Get
                Return Control.QueryEnabled
            End Get
            Set(ByVal value As Boolean)
                TypeDescriptor.GetProperties(Component)("QueryEnabled").SetValue(Component, value)
            End Set
        End Property
        Public Property QueryInterval As Integer
            Get
                Return Control.QueryInterval
            End Get
            Set(ByVal value As Integer)
                TypeDescriptor.GetProperties(Component)("QueryInterval").SetValue(Component, value)
            End Set
        End Property
        Public Property CharactersToQuery As Integer
            Get
                Return Control.CharactersToQuery
            End Get
            Set(ByVal value As Integer)
                TypeDescriptor.GetProperties(Component)("CharactersToQuery").SetValue(Component, value)
            End Set
        End Property
        Public Property FieldHeader As String
            Get
                Return Control.FieldHeader
            End Get
            Set(ByVal value As String)
                TypeDescriptor.GetProperties(Component)("FieldHeader").SetValue(Component, value)
            End Set
        End Property
        Public Property MainTable As String
            Get
                Return Control.MainTable
            End Get
            Set(ByVal value As String)
                TypeDescriptor.GetProperties(Component)("MainTable").SetValue(Component, value)
            End Set
        End Property
        Public Property MainField As String
            Get
                Return Control.MainField
            End Get
            Set(ByVal value As String)
                TypeDescriptor.GetProperties(Component)("MainField").SetValue(Component, value)
            End Set
        End Property
        Public Property MainPKField As String
            Get
                Return Control.MainPKField
            End Get
            Set(ByVal value As String)
                TypeDescriptor.GetProperties(Component)("MainPKField").SetValue(Component, value)
            End Set
        End Property
        Public Property JoinTable As String
            Get
                Return Control.JoinTable
            End Get
            Set(ByVal value As String)
                TypeDescriptor.GetProperties(Component)("JoinTable").SetValue(Component, value)
            End Set
        End Property
        Public Property JoinField As String
            Get
                Return Control.JoinField
            End Get
            Set(ByVal value As String)
                TypeDescriptor.GetProperties(Component)("JoinField").SetValue(Component, value)
            End Set
        End Property
        Public Property JoinPKField As String
            Get
                Return Control.JoinPKField
            End Get
            Set(ByVal value As String)
                TypeDescriptor.GetProperties(Component)("JoinPKField").SetValue(Component, value)
            End Set
        End Property
        Public Property Limit As Integer
            Get
                Return Control.Limit
            End Get
            Set(ByVal value As Integer)
                TypeDescriptor.GetProperties(Component)("Limit").SetValue(Component, value)
            End Set
        End Property
    End Class
#End Region
#Region "COMPONENTS"
    Private WithEvents Timer As Timer
#End Region

End Class