Imports System.ComponentModel
Imports System.Drawing
Imports System.Reflection
Imports System.Text
Imports System.Windows.Forms

Public Class FilterBuilder
    Public Enum ComparsionOperators
        Equals
        Different
        Contains
        Between
        Less
        LessOrEqual
        Bigger
        BiggerOrEqual
    End Enum
    Public Enum LogicalOperators
        [And]
        [Or]
    End Enum
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
    Private NumericTypes As New List(Of String) From {
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
        "Single",
        "Double",
        "Decimal"
    }
    Public Property MainTable As New Model.Table
    Public Property RelatedTables As New List(Of Model.Table)
    Public Property Wheres As New List(Of Model.WhereClause)
    Public Sub New(ByVal Obj As Object)
        Dim DataTypes = NumericTypes.Concat(TextTypes).Concat(DateTypes).Concat(BooleanTypes)
        MainTable.Name = Obj.GetType.Name
        MainTable.DisplayName = GetTableDisplayName(Obj.GetType.GetTypeInfo)
        Dim Columns() As String
        For Each p In Obj.GetType.GetProperties
            If DataTypes.Contains(p.PropertyType.Name) Then
                MainTable.Columns.Add(New Model.Column With {.Name = MainTable.Name & "." & p.Name, .DisplayName = GetColumnDisplayName(p), .DataType = GetColumnType(p.PropertyType.Name), .Visible = True})
            Else
                Columns = {}
                If p.GetCustomAttributes.Any(Function(x) x.GetType.Equals(GetType(Model.DisplayColumn))) Then
                    Columns = TryCast(p.GetCustomAttributes(GetType(Model.DisplayColumn), True)(0), Model.DisplayColumn).ColumnName
                End If
                FillRelatedTable(p, Columns)
            End If
        Next p
    End Sub
    Public Function GetSelectCommand() As String
        Dim Q As New StringBuilder
        Q.AppendLine("SELECT ")
        For Each Column In MainTable.Columns
            Q.AppendLine(vbTab & Column.Name & ", ")
        Next Column
        For Each Table In RelatedTables
            For Each Column In Table.Columns
                If Column.Visible Then Q.AppendLine(vbTab & Column.Name & ", ")
            Next Column
        Next Table
        Q.Remove(Q.Length - 4, 2)
        Q.Append("FROM ")
        Q.AppendLine(MainTable.Name)
        RelatedTables.ForEach(Sub(x) Q.AppendLine("JOIN " & x.Name & " ON " & x.Name & ".ID = " & MainTable.Name & "." & x.Name & "ID"))
        Q.AppendLine("WHERE")

        For i = 0 To Wheres.Count - 1
            Q.Append(vbTab & Wheres(i).Column.Name & " " & GetComparsionOperator(Wheres(i).ComparsionOperator) & " ")
            If Wheres(i).Value = Nothing Then
                Q.Append("@Value")
            Else
                Q.Append(Wheres(i).Value)
            End If
            If Wheres(i).ComparsionOperator = ComparsionOperators.Between Then
                Q.Append(" AND ")
                If Wheres(i).Value2 = Nothing Then
                    Q.Append("@Value2")
                Else
                    Q.Append(Wheres(i).Value2)
                End If
            End If

            If i < Wheres.Count - 1 Then
                Q.AppendLine(" " & GetLogicalOperator(Wheres(i).LogicalOperator))
            Else
                Q.AppendLine(";")
            End If
        Next i






        Return Q.ToString
    End Function
    Private Sub FillRelatedTable(ByVal obj As Object, ByVal DisplayColumns() As String)
        Dim DataTypes = NumericTypes.Concat(TextTypes).Concat(DateTypes).Concat(BooleanTypes)
        Dim Table As Model.Table
        Dim Columns() As String = {}
        Dim IsVisible As Boolean
        If Not IsCollection(obj) Then
            Table = New Model.Table
            Table.Name = obj.PropertyType.Name
            Table.DisplayName = GetTableDisplayName(obj.PropertyType)
            For Each p In obj.propertytype.GetProperties
                If DataTypes.Contains(p.PropertyType.Name) Then
                    IsVisible = If(DisplayColumns.Count = 0, True, DisplayColumns.Contains(p.name))
                    Table.Columns.Add(New Model.Column With {.Name = Table.Name & "." & p.Name, .DisplayName = GetColumnDisplayName(p), .DataType = GetColumnType(p.PropertyType.Name), .Visible = IsVisible})
                Else
                    If p.GetCustomAttributes.Any(Function(x) x.GetType.Equals(GetType(Model.DisplayColumn))) Then
                        Columns = TryCast(p.GetCustomAttributes(GetType(Model.DisplayColumn), True)(0), Model.DisplayColumn).ColumnName
                    End If
                    FillRelatedTable(p, Columns)
                End If
            Next p
            RelatedTables.Add(Table)
        End If
    End Sub
    Private Function GetDirection(ByVal Direction As ListSortDirection) As String
        Dim Dir As String = String.Empty
        Select Case Direction
            Case = ListSortDirection.Ascending
                Dir = "ASC"
            Case = ListSortDirection.Descending
                Dir = "DSC"
        End Select
        Return Dir
    End Function
    Private Function GetLogicalOperator(ByVal [Operator] As LogicalOperators) As String
        Dim Op As String = String.Empty
        Select Case [Operator]
            Case = LogicalOperators.And
                Op = "AND"
            Case = LogicalOperators.Or
                Op = "OR"
        End Select
        Return Op
    End Function
    Private Function GetComparsionOperator(ByVal [Operator] As ComparsionOperators) As String
        Dim Op As String = String.Empty
        Select Case [Operator]
            Case = ComparsionOperators.Equals
                Op = "="
            Case = ComparsionOperators.Different
                Op = "<>"
            Case = ComparsionOperators.Contains
                Op = "LIKE"
            Case = ComparsionOperators.Between
                Op = "BETWEEN"
            Case = ComparsionOperators.Less
                Op = "<"
            Case = ComparsionOperators.LessOrEqual
                Op = "<="
            Case = ComparsionOperators.Bigger
                Op = ">"
            Case = ComparsionOperators.BiggerOrEqual
                Op = ">="
        End Select
        Return Op
    End Function
    Private Function GetColumnType(ByVal SistemType As String) As String
        If NumericTypes.Contains(SistemType) Then
            Return "Numeric"
        ElseIf TextTypes.Contains(SistemType) Then
            Return "Text"
        ElseIf DateTypes.Contains(SistemType) Then
            Return "Date"
        ElseIf BooleanTypes.Contains(SistemType) Then
            Return "Boolean"
        Else
            Return Nothing
        End If
    End Function
    Private Function GetColumnDisplayName(ByVal pi As PropertyInfo) As String
        Dim DisplayName As String = String.Empty
        If pi.GetCustomAttributes(GetType(DisplayNameAttribute), True).Cast(Of DisplayNameAttribute).Count > 0 Then
            DisplayName = pi.GetCustomAttributes(GetType(DisplayNameAttribute), True).Cast(Of DisplayNameAttribute)().FirstOrDefault().DisplayName
        Else
            DisplayName = pi.Name
        End If
        Return DisplayName
    End Function
    Private Function GetTableDisplayName(ByVal pi As TypeInfo) As String
        Dim DisplayName As String = String.Empty
        If pi.GetCustomAttributes(GetType(DisplayNameAttribute), True).Cast(Of DisplayNameAttribute).Count > 0 Then
            DisplayName = pi.GetCustomAttributes(GetType(DisplayNameAttribute), True).Cast(Of DisplayNameAttribute)().FirstOrDefault().DisplayName
        Else
            DisplayName = pi.Name
        End If
        Return DisplayName
    End Function
    Private Function IsCollection(ByVal pi As PropertyInfo) As Boolean
        If pi.PropertyType.Name = "String" Then
            Return False
        Else
            If GetType(IEnumerable).IsAssignableFrom(pi.PropertyType) Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
    Public Class Model
        Public Class WhereClause
            Public Property Column As Column
            Public Property ComparsionOperator As ComparsionOperators
            Public Property Value As Object
            Public Property Value2 As Object
            Public Property LogicalOperator As LogicalOperators
        End Class
        Public Class Table
            Public Property Name As String
            Public Property DisplayName As String
            Public Property Columns As New List(Of Column)
            Public Overrides Function ToString() As String
                Return DisplayName
            End Function
        End Class
        Public Class Column
            Public Property Name As String
            Public Property DisplayName As String
            Public Property DataType As String
            Public Property Visible As Boolean
            Public Overrides Function ToString() As String
                Return DisplayName
            End Function
        End Class
        Public Class DisplayColumn
            Inherits Attribute
            Public Property ColumnName As String()
            Public Sub New(ByVal ColumnName() As String)
                Me.ColumnName = ColumnName
            End Sub
            Public Sub New(ByVal ColumnName As String)
                Dim C() As String = {ColumnName}
                Me.ColumnName = C
            End Sub
        End Class
    End Class

    Public Sub ShowDialog()
        InitializeComponent()
        FrmFilter.Font = New Font("Century Gothic", 9.75)
        FrmFilter.ShowDialog()
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
            DgvColumns.Rows(DgvColumns.RowCount - 1).Cells(0).ToolTipText = Column.Name
        Next Column

        TpTable = New TabPage
        TpTable.Text = MainTable.DisplayName
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
                DgvColumns.Rows(DgvColumns.RowCount - 1).Cells(0).ToolTipText = Column.Name
            Next Column

            TpTable = New TabPage
            TpTable.Text = Table.DisplayName
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

        FrmFilter = New Form
        FrmFilter.Font = New Font("Century Gothic", 9.75)
        FrmFilter.Text = "Criador de Filtros"
        FrmFilter.Size = New Drawing.Size(460, 600)
        FrmFilter.BackColor = Color.White
        FrmFilter.ShowIcon = False
        FrmFilter.ShowInTaskbar = False
        FrmFilter.MinimizeBox = False
        FrmFilter.MaximizeBox = False
        FrmFilter.FormBorderStyle = FormBorderStyle.FixedSingle
        FrmFilter.Controls.AddRange({TcTables, LblOperator, CbxOperador})


    End Sub
    Private Sub CbxOperador_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxOperador.SelectedIndexChanged
        Dim DataType As String = String.Empty
        Dim Cell As DataGridViewCell
        If CType(sender, DataGridView).SelectedRows.Count > 0 Then
            Cell = CType(sender, DataGridView).SelectedRows(0).Cells(0)
            DataType = CType(Cell.Value, Model.Column).DataType
        End If

        If CbxOperador.Text = "Entre" Then
            If DataType = "Numeric" Then
            ElseIf DataType = "Date" Then
            End If
        Else
            If DataType = "Text then" Then
            ElseIf DataType = "Numeric" Then
            ElseIf DataType = "Date" Then
            End If
        End If
    End Sub

    Private Sub DgvColumns_SelectionChanged(sender As Object, e As EventArgs)
        Dim DataType As String = String.Empty
        Dim Cell As DataGridViewCell
        If CType(sender, DataGridView).SelectedRows.Count > 0 Then
            Cell = CType(sender, DataGridView).SelectedRows(0).Cells(0)
            DataType = CType(Cell.Value, Model.Column).DataType
            If DataType = "Text" Then
                CbxOperador.Items.Clear()
                CbxOperador.Items.Add("Igual")
                CbxOperador.Items.Add("Diferente")
                CbxOperador.Items.Add("Contém")
            ElseIf DataType = "Numeric" Or DataType = "Date" Then
                CbxOperador.Items.Clear()
                CbxOperador.Items.Add("Igual")
                CbxOperador.Items.Add("Diferente")
                CbxOperador.Items.Add("Contém")
                CbxOperador.Items.Add("Entre")
                CbxOperador.Items.Add("Menor")
                CbxOperador.Items.Add("Menor ou Igual")
                CbxOperador.Items.Add("Maior")
                CbxOperador.Items.Add("Maior ou Igual")
            End If
            CbxOperador.SelectedIndex = 0
        End If
    End Sub

    Friend WithEvents FrmFilter As Form
    Friend WithEvents TcTables As TabControl
    Friend WithEvents TpTable As TabPage
    Friend WithEvents DgvColumns As DataGridView
    Friend WithEvents TxtValue As TextBox
    Friend WithEvents TxtValue2 As TextBox
    Friend WithEvents RbAnd As RadioButton
    Friend WithEvents RbOr As RadioButton
    Friend WithEvents BtnAdd As Button
    Friend WithEvents DgvWheres As DataGridView
    Friend WithEvents LblOperator As Label
    Friend WithEvents CbxOperador As ComboBox
End Class
