Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms

Public Class FilterBuilder2
    Public Sub New(ObjType As Type)
        LoadFilter(ObjType)
    End Sub
    Private Sub LoadFilter(ObjType As Type, Optional IsRelatedTable As Boolean = False, Optional PropertyName As String = Nothing,
                           Optional IsChild As Boolean = False, Optional ShowColumnName As String = Nothing,
                           Optional ShowColumnAlias As String = Nothing)
        Dim Table As New Model.Table
        Dim Column As Model.Column
        Table.TableName = ObjType.Name
        Table.TableAlias = If(PropertyName = Nothing, GetTableAlias(ObjType), PropertyName)
        Table.ShowColumnNameInObject = ShowColumnName
        Table.ShowColumnAliasInObject = ShowColumnAlias
        For Each p As PropertyInfo In ObjType.GetProperties
            If Not IsCollection(p) Then
                If GetPrimitiveTypes.Contains(p.PropertyType.Name) Then
                    Column = New Model.Column With {
                        .Table = Table,
                        .ColumnName = p.Name,
                        .ColumnAlias = GetColumnAlias(p),
                        .ColumnType = p.PropertyType.Name,
                        .ColumnTypeAlias = GetColumnTypeAlias(p.PropertyType.Name)
                    }
                    Table.Columns.Add(Column)
                Else
                    If Not IsChild Then
                        LoadFilter(p.PropertyType, True, GetColumnAlias(p), True, GetShowColumnName(p), GetShowColumnAlias(p))
                    Else
                        IsChild = False
                    End If

                End If
            End If
        Next p
        If Not IsRelatedTable Then
            MainTable = Table
            'MainTable.Relation = Nothing
        Else
            RelatedTables.Add(Table)
            'RelatedTables.Insert(0, Table)
        End If
    End Sub
    Public Property FilterName As String
    Public Property MainTable As New Model.Table
    Public Property RelatedTables As New List(Of Model.Table)
    Public Property Wheres As New ObjectModel.Collection(Of Model.WhereClause)

    Private BooleanTypes As New List(Of String) From {
        "Boolean"
    }
    Private DateTypes As New List(Of String) From {
        "Date",
        "DateTime"
    }
    Private TextTypes As New List(Of String) From {
        "String",
        "Char"
    }
    Private IntegerTypes As New List(Of String) From {
        "SByte",
        "Byte",
        "Short",
        "UShort",
        "Integer",
        "Int16",
        "Int32",
        "Int64",
        "UInteger",
        "Long",
        "ULong",
        "Single"
    }
    Private DecimalTypes As New List(Of String) From {
        "Double",
        "Decimal"
    }
    'Retorna se a coleção é de tipo primitivo.
    Private Function IsPrimitiveCollection(p As Reflection.PropertyInfo) As Boolean
        Dim t As Type = p.PropertyType
        Dim intIndexer = t.GetMethod("get_Item", {GetType(Integer)})
        Return GetPrimitiveTypes.Contains(intIndexer.ReturnType.Name)
    End Function
    'Retornar uma lista com todos os tipos primitivos.
    Private Function GetPrimitiveTypes() As List(Of String)
        Return BooleanTypes.Concat(DateTypes).Concat(TextTypes).Concat(IntegerTypes).Concat(DecimalTypes).ToList
    End Function
    'Verifica se a propriedade é uma coleção.
    Private Function IsCollection(p As Reflection.PropertyInfo) As Boolean
        If p.PropertyType.Name = "String" Then
            Return False
        Else
            If GetType(IEnumerable).IsAssignableFrom(p.PropertyType) Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
    'Retorna o valor do atributo ShowColumn na propriedade (ColumnName).
    Private Function GetShowColumnName(p As Reflection.PropertyInfo) As String
        For Each Attr In p.GetCustomAttributes(True)
            If Attr.GetType Is GetType(Model.Attributes.ShowColumn) Then
                Return CType(Attr, Model.Attributes.ShowColumn).ColumnName
            End If
        Next
        Return Nothing
    End Function
    'Retorna o valor do atributo ShowColumn na propriedade (ColumnAlias).
    Private Function GetShowColumnAlias(p As Reflection.PropertyInfo) As String
        For Each Attr In p.GetCustomAttributes(True)
            If Attr.GetType Is GetType(Model.Attributes.ShowColumn) Then
                Return CType(Attr, Model.Attributes.ShowColumn).ColumnAlias
            End If
        Next
        Return Nothing
    End Function
    'Retorna o valor do atributo ColumnAlias na propriedade.
    Private Function GetColumnAlias(p As Reflection.PropertyInfo) As String
        For Each Attr In p.GetCustomAttributes(True)
            If Attr.GetType Is GetType(Model.Attributes.ColumnAlias) Then
                Return CType(Attr, Model.Attributes.ColumnAlias).Value
            End If
        Next
        Return p.Name
    End Function
    'Retorna o valor do atributo TableAlias na classe.
    Private Function GetTableAlias(t As Reflection.TypeInfo) As String
        For Each Attr In t.GetCustomAttributes(True)
            If Attr.GetType Is GetType(Model.Attributes.TableAlias) Then
                Return CType(Attr, Model.Attributes.TableAlias).Value
            End If
        Next
        Return t.Name
    End Function
    Private Function GetColumnTypeAlias(ByVal ColumnType As String) As String
        If IntegerTypes.Contains(ColumnType) Then
            Return "Inteiro"
        ElseIf DecimalTypes.Contains(ColumnType) Then
            Return "Decimal"
        ElseIf TextTypes.Contains(ColumnType) Then
            Return "Texto"
        ElseIf DateTypes.Contains(ColumnType) Then
            Return "Data"
        ElseIf BooleanTypes.Contains(ColumnType) Then
            Return "Sim/Não"
        Else
            Return Nothing
        End If
    End Function


    Private _ValueLocation As New Point(288, 111)
    Private _value2Location As New Point(288, 158)
    Private _AndLocation As New Point(288, 140)
    Private _AndLocation2 As New Point(288, 187)
    Private _OrLocation As New Point(327, 140)
    Private _OrLocation2 As New Point(327, 187)
    Private _AddLocation As New Point(288, 167)
    Private _AddLocation2 As New Point(288, 214)
    Private _DescriptionLocation As New Point(288, 313)
    Private WithEvents FrmFilter As Form
    Private WithEvents TcTables As TabControl
    Private WithEvents TpTable As TabPage
    Private WithEvents DgvColumns As DataGridView
    Private WithEvents TxtValue1 As TextBox
    Private WithEvents TxtValue2 As TextBox
    Private WithEvents RbAnd As RadioButton
    Private WithEvents RbOr As RadioButton
    Private WithEvents BtnAdd As Button
    Private WithEvents LblOperator As Label
    Private WithEvents CbxOperador As ComboBox
    Private WithEvents LblValue As Label
    Private WithEvents LblValue2 As Label
    Private WithEvents LbxWheres As ListBox
    Private WithEvents PnDgvWheres As Panel
    Private WithEvents TsBar As ToolStrip
    Private WithEvents BtnDelete As ToolStripButton
    Private WithEvents BtnClean As ToolStripButton
    Private WithEvents LblDescription As Label
    Private WithEvents BtnClose As Button
    Private WithEvents BtnExecute As Button
    Private WithEvents BtnSave As Button
    Private WithEvents ConstructorForm As Form
    Private WithEvents ConstructorDateBox As DateBox
    Private WithEvents ConstructorDecimalBox As DecimalBox
    Private WithEvents ConstructorTextBox As TextBox
    Private WithEvents ConstructorLabel As Label
    Private WithEvents ConstructorButton As Button
    Private WithEvents ConstructorToggle As ToggleButton
    Private Class CustomToolstripRender
        Inherits ToolStripSystemRenderer
        Public Sub New()
        End Sub
        Protected Overrides Sub OnRenderToolStripBorder(ByVal e As ToolStripRenderEventArgs)
        End Sub
    End Class
    Public Function ShowDialog() As FilterDialogResult
        InitializeComponent()
        Dim Dr As DialogResult = FrmFilter.ShowDialog
        If Dr = DialogResult.OK Then
            Return FilterDialogResult.Save

        ElseIf Dr = DialogResult.Yes Then
            Return FilterDialogResult.Execute
        Else
            Return FilterDialogResult.Cancel
        End If
    End Function

    Public Enum FilterDialogResult
        Save
        Execute
        Cancel
    End Enum

    Private _DataType As String
    Private Sub BtnAdd_TextChanged(sender As Object, e As EventArgs) Handles TxtValue1.TextChanged, TxtValue2.TextChanged
        Dim Dgv As DataGridView = TcTables.TabPages(TcTables.SelectedIndex).Controls.OfType(Of DataGridView).First
        Dim Column As Model.Column
        Column = CType(Dgv.SelectedRows(0).Cells(0).Value, Model.Column)





        If CbxOperador.SelectedValue = "BETWEEN" Then
            If TxtValue1.Text = Nothing Or TxtValue2.Text = Nothing Then
                BtnAdd.Enabled = False
            Else
                BtnAdd.Enabled = True
            End If
        Else

            If Column.ColumnType = "Boolean" Then
                If UCase(TxtValue1.Text) <> "SIM" And UCase(TxtValue1.Text) <> "NÃO" Then
                    BtnAdd.Enabled = False
                Else
                    TxtValue1.Text = If(UCase(TxtValue1.Text) = "SIM", "Sim", "Não")
                    TxtValue1.Select(3, 0)
                    BtnAdd.Enabled = True
                End If
            Else
                If TxtValue1.Text = Nothing Then
                    BtnAdd.Enabled = False
                Else
                    BtnAdd.Enabled = True
                End If
            End If


        End If
    End Sub
    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        Dim Dgv As DataGridView = TcTables.TabPages(TcTables.SelectedIndex).Controls.OfType(Of DataGridView).First
        Dim Where As New Model.WhereClause
        Where.Column = CType(Dgv.SelectedRows(0).Cells(0).Value, Model.Column)
        Where.RelationalOperator.Value = CbxOperador.SelectedValue

        Where.LogicalOperator.Value = If(RbAnd.Checked, "AND", "OR")

        Where.Parameter.Value = TxtValue1.Text

        If CbxOperador.SelectedValue = "BETWEEN" Then
            Where.Parameter2.Value = TxtValue2.Text
        End If

        If Where.RelationalOperator.Value = "BETWEEN" Then
            If Where.Column.ColumnType = "Date" Then
                If Where.Parameter.Value = " /  /" Then
                    Where.Parameter.Value = New Date(1990, 1, 1)
                End If
                If Where.Parameter2.Value = " /  /" Then
                    Where.Parameter2.Value = Today
                End If
            End If
        End If


        Wheres.Add(Where)

        LbxWheres.DataSource = Wheres.ToList
    End Sub

    Private Sub CbxOperador_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxOperador.SelectedIndexChanged
        If CbxOperador.Text = "Entre" Then
            LblValue2.Visible = True
            If FrmFilter.Controls.Contains(TxtValue2) Then
                TxtValue2.Visible = True
            End If



            RbAnd.Location = _AndLocation2
            RbOr.Location = _OrLocation2
            BtnAdd.Location = _AddLocation2


        Else
            LblValue2.Visible = False
            If FrmFilter.Controls.Contains(TxtValue2) Then TxtValue2.Visible = False


            RbAnd.Location = _AndLocation
            RbOr.Location = _OrLocation
            BtnAdd.Location = _AddLocation
        End If

    End Sub

    Private Sub TcTables_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TcTables.SelectedIndexChanged
        Dim Dgv As DataGridView = TcTables.TabPages(TcTables.SelectedIndex).Controls.OfType(Of DataGridView).First
        SelectionChanged(Dgv)
    End Sub

    Private Sub DgvColumns_SelectionChanged(sender As Object, e As EventArgs)
        SelectionChanged(sender)
    End Sub

    Private Sub SelectionChanged(sender As Object)
        Dim Cell As DataGridViewCell
        Dim OperatorList As List(Of Model.Operator)
        If CType(sender, DataGridView).SelectedRows.Count > 0 Then
            Cell = CType(sender, DataGridView).SelectedRows(0).Cells(0)
            _DataType = CType(Cell.Value, Model.Column).ColumnTypeAlias
            OperatorList = New List(Of Model.Operator)

            OperatorList.Add(New Model.Operator With {.Value = "="})
            OperatorList.Add(New Model.Operator With {.Value = "<>"})

            If _DataType = "Texto" Then
                OperatorList.Add(New Model.Operator With {.Value = "LIKE"})
            End If

            If _DataType = "Inteiro" Or _DataType = "Decimal" Or _DataType = "Data" Then

                OperatorList.Add(New Model.Operator With {.Value = "BETWEEN"})
                OperatorList.Add(New Model.Operator With {.Value = "<"})
                OperatorList.Add(New Model.Operator With {.Value = "<="})
                OperatorList.Add(New Model.Operator With {.Value = ">"})
                OperatorList.Add(New Model.Operator With {.Value = ">="})
            End If

            CbxOperador.ValueMember = "Value"
            CbxOperador.DisplayMember = "Display"
            CbxOperador.DataSource = OperatorList
            CbxOperador.SelectedIndex = 0

            TxtValue1.Text = Nothing
            TxtValue2.Text = Nothing

            LblDescription.Text = "Tipo: " & _DataType

            'If _DataType = "Text" Then
            '    LblDescription.Text = "Tipo: Texto"
            'ElseIf _DataType = "Integer" Then
            '    LblDescription.Text = "Tipo: Número Inteiro"
            'ElseIf _DataType = "Decimal" Then
            '    LblDescription.Text = "Tipo: Número Decimal"
            'ElseIf _DataType = "Date" Then
            '    LblDescription.Text = "Tipo: Data"
            'ElseIf _DataType = "Boolean" Then
            '    LblDescription.Text = "Tipo: Sim/Não"
            'End If



        End If
    End Sub
    Private Sub InitializeComponent()
        TcTables = New TabControl
        TcTables.Size = New Size(270, 320)
        TcTables.Location = New Point(12, 12)
        DgvColumns = New DataGridView
        DgvColumns.Dock = DockStyle.Fill
        DgvColumns.BorderStyle = BorderStyle.None
        DgvColumns.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DgvColumns.Columns.Add("Column", "Coluna")
        DgvColumns.ColumnHeadersVisible = False
        DgvColumns.RowHeadersVisible = False
        DgvColumns.ReadOnly = True
        DgvColumns.AllowUserToAddRows = False
        DgvColumns.AllowUserToDeleteRows = False
        DgvColumns.AllowUserToResizeRows = False
        DgvColumns.AllowUserToResizeColumns = False
        DgvColumns.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DgvColumns.BackgroundColor = Color.White
        DgvColumns.CellBorderStyle = DataGridViewCellBorderStyle.None
        AddHandler DgvColumns.SelectionChanged, AddressOf DgvColumns_SelectionChanged
        For Each Column In MainTable.Columns
            DgvColumns.Rows.Add(Column)
            DgvColumns.Rows(DgvColumns.RowCount - 1).Cells(0).ToolTipText = String.Format("Nome Tabela: {0}{1}Apelido Tabela: {2}{3}Nome Campo: {4}{5}Apelido do Campo: {6}", Column.Table.TableName, vbNewLine, Column.Table.TableAlias.Replace(" ", Nothing), vbNewLine, Column.ColumnName, vbNewLine, Column.ColumnAlias.Replace(" ", Nothing))
        Next Column

        TpTable = New TabPage
        TpTable.Text = MainTable.TableAlias
        TpTable.BackColor = Color.White
        TpTable.Controls.Add(DgvColumns)
        TcTables.TabPages.Add(TpTable)

        For Each Table In RelatedTables
            DgvColumns = New DataGridView
            DgvColumns.Dock = DockStyle.Fill
            DgvColumns.BorderStyle = BorderStyle.None
            DgvColumns.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DgvColumns.Columns.Add("Column", "Coluna")
            DgvColumns.ColumnHeadersVisible = False
            DgvColumns.RowHeadersVisible = False
            DgvColumns.ReadOnly = True
            DgvColumns.AllowUserToAddRows = False
            DgvColumns.AllowUserToDeleteRows = False
            DgvColumns.AllowUserToResizeRows = False
            DgvColumns.AllowUserToResizeColumns = False
            DgvColumns.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            DgvColumns.BackgroundColor = Color.White
            DgvColumns.CellBorderStyle = DataGridViewCellBorderStyle.None
            AddHandler DgvColumns.SelectionChanged, AddressOf DgvColumns_SelectionChanged
            For Each Column In Table.Columns
                DgvColumns.Rows.Add(Column)
                DgvColumns.Rows(DgvColumns.RowCount - 1).Cells(0).ToolTipText = String.Format("Nome Tabela: {0}{1}Apelido Tabela: {2}{3}Nome Campo: {4}{5}Apelido do Campo: {6}", Column.Table.TableName, vbNewLine, Column.Table.TableAlias.Replace(" ", Nothing), vbNewLine, Column.ColumnName, vbNewLine, Column.ColumnAlias.Replace(" ", Nothing))
            Next Column

            TpTable = New TabPage
            TpTable.Text = Table.TableAlias
            TpTable.BackColor = Color.White
            TpTable.Controls.Add(DgvColumns)
            TcTables.TabPages.Add(TpTable)
        Next Table

        LblOperator = New Label
        LblOperator.AutoSize = True
        LblOperator.Text = "Operador"
        LblOperator.Location = New Point(285, 41)

        CbxOperador = New ComboBox
        CbxOperador.DropDownStyle = ComboBoxStyle.DropDownList
        CbxOperador.Size = New Size(144, 25)
        CbxOperador.Location = New Point(288, 61)


        LblValue = New Label
        LblValue.AutoSize = True
        LblValue.Text = "Valor"
        LblValue.Location = New Point(285, 91)

        TxtValue1 = New TextBox
        TxtValue1.Width = 144
        TxtValue1.Location = _ValueLocation

        LblValue2 = New Label
        LblValue2.AutoSize = True
        LblValue2.Text = "Valor 2"
        LblValue2.Location = New Point(285, 138)

        TxtValue2 = New TextBox
        TxtValue2.Width = 144
        TxtValue2.Location = _value2Location


        RbAnd = New RadioButton
        RbAnd.Checked = True
        RbAnd.Location = _AndLocation2
        RbAnd.AutoSize = True
        RbAnd.Text = "E"

        RbOr = New RadioButton
        RbOr.Checked = False
        RbOr.Location = _OrLocation2
        RbOr.AutoSize = True
        RbOr.Text = "OU"

        BtnAdd = New Button
        BtnAdd.Text = "Adicionar"
        BtnAdd.Size = New Size(144, 30)
        BtnAdd.Location = _AddLocation2
        BtnAdd.UseVisualStyleBackColor = True
        BtnAdd.Enabled = False

        LblDescription = New Label
        LblDescription.AutoSize = True
        LblDescription.Location = _DescriptionLocation
        LblDescription.ForeColor = Color.DimGray

        LbxWheres = New ListBox
        LbxWheres.Dock = DockStyle.Fill
        LbxWheres.BackColor = Color.White

        BtnDelete = New ToolStripButton
        BtnDelete.DisplayStyle = ToolStripItemDisplayStyle.Text
        BtnDelete.Text = "Deletar"

        BtnClean = New ToolStripButton
        BtnClean.DisplayStyle = ToolStripItemDisplayStyle.Text
        BtnClean.Text = "Limpar"

        TsBar = New ToolStrip
        TsBar.GripStyle = ToolStripGripStyle.Hidden
        TsBar.BackColor = Color.DimGray
        TsBar.ForeColor = Color.White
        TsBar.RenderMode = ToolStripRenderMode.System
        TsBar.Items.AddRange({BtnDelete, BtnClean})
        TsBar.Renderer = New CustomToolstripRender

        PnDgvWheres = New Panel
        PnDgvWheres.BackColor = Color.White
        PnDgvWheres.Location = New Point(12, 339)
        PnDgvWheres.Size = New Size(415, 152)
        PnDgvWheres.Controls.AddRange({LbxWheres, TsBar})

        BtnClose = New Button
        BtnClose.Text = "Fechar"
        BtnClose.Size = New Size(100, 30)
        BtnClose.Location = New Point(328, 500)
        BtnClose.UseVisualStyleBackColor = True
        BtnClose.DialogResult = DialogResult.Cancel

        BtnExecute = New Button
        BtnExecute.Text = "Executar"
        BtnExecute.Size = New Size(100, 30)
        BtnExecute.Location = New Point(222, 500)
        BtnExecute.UseVisualStyleBackColor = True
        BtnExecute.Enabled = False
        BtnClose.DialogResult = DialogResult.Yes


        BtnSave = New Button
        BtnSave.Text = "Salvar"
        BtnSave.Size = New Size(100, 30)
        BtnSave.Location = New Point(116, 500)
        BtnSave.UseVisualStyleBackColor = True
        BtnSave.Enabled = False
        BtnSave.DialogResult = DialogResult.OK

        FrmFilter = New Form
        FrmFilter.Font = New Font("Century Gothic", 9.75)
        FrmFilter.Text = "Criador de Filtros"
        FrmFilter.Size = New Size(460, 585)
        FrmFilter.BackColor = Color.White
        FrmFilter.ShowIcon = False
        FrmFilter.ShowInTaskbar = False
        FrmFilter.MinimizeBox = False
        FrmFilter.MaximizeBox = False
        FrmFilter.FormBorderStyle = FormBorderStyle.FixedSingle
        FrmFilter.Controls.AddRange({TcTables, LblOperator, CbxOperador, LblValue, TxtValue1, LblValue2, TxtValue2, RbAnd, RbOr, BtnAdd, LblDescription, PnDgvWheres, BtnClose, BtnExecute, BtnSave})

    End Sub

    Private Sub FrmFilter_Load() Handles FrmFilter.Load
        LbxWheres.DataSource = Wheres.ToList
    End Sub
    Public Class Model
        Public Class Attributes
            <AttributeUsage(AttributeTargets.Property)>
            Public Class ColumnAlias
                Inherits Attribute
                Friend Value As String
                Public Sub New(Value As String)
                    Me.Value = Value
                End Sub
            End Class
            <AttributeUsage(AttributeTargets.Class)>
            Public Class TableAlias
                Inherits Attribute
                Friend Value As String
                Public Sub New(Value As String)
                    Me.Value = Value
                End Sub
            End Class
            <AttributeUsage(AttributeTargets.Property)>
            Public Class ShowColumn
                Inherits Attribute
                Friend ColumnName As String
                Friend ColumnAlias As String
                Public Sub New(ColumnName As String, ColumnAlias As String)
                    Me.ColumnName = ColumnName
                    Me.ColumnAlias = ColumnAlias
                End Sub
            End Class
        End Class
        Public Class [Operator]
            Private _Value As String
            Private _Display As String
            Public Property Value As String
                Get
                    Return _Value
                End Get
                Set(value As String)
                    _Value = value
                    _Display = GetOperatorAlias(value)
                End Set
            End Property
            Public ReadOnly Property Display As String
                Get
                    Return _Display
                End Get
            End Property
            Private Function GetOperatorAlias([Operator] As String) As String
                Select Case UCase([Operator])
                    Case Is = "="
                        Return "Igual"
                    Case Is = "<>"
                        Return "Diferente"
                    Case Is = "LIKE"
                        Return "Contém"
                    Case Is = "BETWEEN"
                        Return "Entre"
                    Case Is = "<"
                        Return "Menor"
                    Case Is = "<="
                        Return "Menor ou Igual"
                    Case Is = ">"
                        Return "Maior"
                    Case Is = ">="
                        Return "Maior ou Igual"
                    Case Is = "AND"
                        Return "E"
                    Case Is = "OR"
                        Return "Ou"
                    Case Else
                        Return "Nulo"
                End Select
            End Function
        End Class
        Public Class WhereClause
            Public Property Column As Column
            Public Property RelationalOperator As New [Operator]
            Public Property Parameter As New Parameter
            Public Property Parameter2 As New Parameter
            Public Property LogicalOperator As New [Operator]
            Public Overrides Function ToString() As String
                Dim p1 As String = " é "
                Dim p2 As String = String.Empty
                Dim s As String
                Select Case RelationalOperator.Value
                    Case Is = "="
                        p2 = " a "
                    Case Is = "<>"
                        p2 = " de "
                    Case Is = "LIKE"
                        p1 = " "
                        p2 = " "
                    Case Is = "<"
                        p2 = " que "
                    Case Is = ">"
                        p2 = " que "
                    Case Is = "<="
                        p2 = " a "
                    Case Is = ">="
                        p2 = " a "
                End Select
                If RelationalOperator.Value <> "BETWEEN" Then
                    s = Column.ColumnAlias & p1 & RelationalOperator.Display & p2 & " " & If(Parameter.Value = Nothing, "{Vazio}", Parameter.Value) & vbTab & " " & LogicalOperator.Display
                Else
                    s = Column.ColumnAlias & " Está " & RelationalOperator.Display & " " & Parameter.Value & " e " & Parameter2.Value & vbTab & " " & LogicalOperator.Display
                End If
                Return s
            End Function
        End Class
        Public Class Table
            Public Property TableName As String
            Public Property TableAlias As String
            Public Property ShowColumnNameInObject As String
            Public Property ShowColumnAliasInObject As String
            Public Property Columns As New List(Of Column)
            Public Overrides Function ToString() As String
                Return String.Format("[{0}] AS [{1}]", TableName.Replace(" ", Nothing), TableAlias.Replace(" ", Nothing))
            End Function
        End Class
        Public Class Column
            Public Property Table As New Table
            Public Property ColumnName As String
            Public Property ColumnAlias As String
            Public Property ColumnType As String
            Public Property ColumnTypeAlias As String
            Public Overrides Function ToString() As String
                Return ColumnAlias
            End Function
        End Class
        Public Class Result
            Public Property CommandText As String
            Public Property Parameters As New List(Of Parameter)
        End Class
        Public Class Parameter
            Public Property Name As String
            Public Property Value As Object
        End Class
    End Class
End Class









