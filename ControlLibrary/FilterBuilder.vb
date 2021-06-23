﻿Imports System.ComponentModel
Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms

Public Class FilterBuilder

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        Dim Dgv As DataGridView = TcTables.TabPages(TcTables.SelectedIndex).Controls.OfType(Of DataGridView).First
        Dim Where As New Model.WhereClause
        Where.Column = CType(Dgv.SelectedRows(0).Cells(0).Value, Model.Column)
        Where.ComparsionOperator.Value = CbxOperador.SelectedValue
        Where.ComparsionOperator.Display = CbxOperador.Text

        If FrmFilter.Controls.Contains(TxtValue1) Then
            Where.Parameter.Value = TxtValue1.Text
        ElseIf FrmFilter.Controls.Contains(DtxValue1) Then
            Where.Parameter.Value = DtxValue1.Text
        ElseIf FrmFilter.Controls.Contains(DbxValue1) Then
            Where.Parameter.Value = DbxValue1.Text
        ElseIf FrmFilter.Controls.Contains(TbtValue1) Then
            Where.Parameter.Value = If(TbtValue1.State = ToggleButton.ToggleButtonStates.ON, "Sim", "Não")
        End If

        If CbxOperador.SelectedValue = "BETWEEN" Then

            If FrmFilter.Controls.Contains(DtxValue2) Then
                Where.Parameter2.Value = DtxValue2.Text
            ElseIf FrmFilter.Controls.Contains(DbxValue2) Then
                Where.Parameter2.Value = DbxValue2.Text
            End If

        End If
        Where.LogicalOperator.Display = If(RbAnd.Checked, "e", "ou")
        Where.LogicalOperator.Value = If(RbAnd.Checked, "AND", "OR")
        If Where.Column.DataType = "Date" AndAlso Where.Parameter. 'quando deixa a data em branco ele leva so a mascara
        Wheres.Add(Where) Then

            LbxWheres.DataSource = Wheres.ToList
    End Sub















    Private Sub CbxOperador_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxOperador.SelectedIndexChanged
        If CbxOperador.Text = "Entre" Then
            LblValue2.Visible = True
            If FrmFilter.Controls.Contains(DtxValue2) Then
                DtxValue2.Visible = True
            End If
            If FrmFilter.Controls.Contains(DbxValue2) Then
                DbxValue2.Visible = True
            End If


            RbAnd.Location = _AndLocation2
            RbOr.Location = _OrLocation2
            BtnAdd.Location = _AddLocation2


        Else
            LblValue2.Visible = False
            If FrmFilter.Controls.Contains(DtxValue2) Then DtxValue2.Visible = False
            If FrmFilter.Controls.Contains(DbxValue2) Then DbxValue2.Visible = False

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
            _DataType = CType(Cell.Value, Model.Column).DataType
            OperatorList = New List(Of Model.Operator)

            OperatorList.Add(New Model.Operator With {.Display = "Igual", .Value = "="})
            OperatorList.Add(New Model.Operator With {.Display = "Diferente", .Value = "<>"})

            If _DataType = "Text" Then
                OperatorList.Add(New Model.Operator With {.Display = "Contém", .Value = "LIKE"})
            End If

            If _DataType = "Integer" Or _DataType = "Decimal" Or _DataType = "Date" Then

                OperatorList.Add(New Model.Operator With {.Display = "Entre", .Value = "BETWEEN"})
                OperatorList.Add(New Model.Operator With {.Display = "Menor", .Value = "<"})
                OperatorList.Add(New Model.Operator With {.Display = "Menor ou Igual", .Value = "<="})
                OperatorList.Add(New Model.Operator With {.Display = "Maior", .Value = ">"})
                OperatorList.Add(New Model.Operator With {.Display = "Maior ou Igual", .Value = ">="})
            End If


            If FrmFilter.Controls.Contains(TxtValue1) Then FrmFilter.Controls.Remove(TxtValue1)
            If FrmFilter.Controls.Contains(DtxValue1) Then FrmFilter.Controls.Remove(DtxValue1)
            If FrmFilter.Controls.Contains(DtxValue2) Then FrmFilter.Controls.Remove(DtxValue2)
            If FrmFilter.Controls.Contains(DbxValue1) Then FrmFilter.Controls.Remove(DbxValue1)
            If FrmFilter.Controls.Contains(DbxValue2) Then FrmFilter.Controls.Remove(DbxValue2)
            If FrmFilter.Controls.Contains(TbtValue1) Then FrmFilter.Controls.Remove(TbtValue1)

            If _DataType = "Text" Then


                TxtValue1 = New TextBox
                TxtValue1.Size = New Size(144, 23)
                TxtValue1.Location = _ValueLocation
                TxtValue1.Name = "TxtValue1"

                FrmFilter.Controls.Add(TxtValue1)
                TxtValue1.Select()
            ElseIf _DataType = "Date" Then
                DtxValue1 = New DateBox
                DtxValue1.Size = New Size(144, 23)
                DtxValue1.Location = _ValueLocation

                DtxValue2 = New DateBox
                DtxValue2.Size = New Size(144, 23)
                DtxValue2.Location = _value2Location
                DtxValue2.Visible = False

                FrmFilter.Controls.AddRange({DtxValue1, DtxValue2})
                DtxValue1.Select()

            ElseIf _DataType = "Integer" Then

                DbxValue1 = New DecimalBox
                DbxValue1.Size = New Size(144, 23)
                DbxValue1.Location = _ValueLocation
                DbxValue1.DecimalPlaces = 0


                DbxValue2 = New DecimalBox
                DbxValue2.Size = New Size(144, 23)
                DbxValue2.Location = _value2Location
                DbxValue2.DecimalPlaces = 0
                DbxValue2.Visible = False

                FrmFilter.Controls.AddRange({DbxValue1, DbxValue2})
                DbxValue2.Select()

            ElseIf _DataType = "Decimal" Then

                DbxValue1 = New DecimalBox
                DbxValue1.Size = New Size(144, 23)
                DbxValue1.Location = _ValueLocation
                DbxValue1.DecimalPlaces = DecimalPlaces


                DbxValue2 = New DecimalBox
                DbxValue2.Size = New Size(144, 23)
                DbxValue2.Location = _value2Location
                DbxValue2.DecimalPlaces = DecimalPlaces
                DbxValue2.Visible = False

                FrmFilter.Controls.AddRange({DbxValue1, DbxValue2})
                DbxValue2.Select()
            ElseIf _DataType = "Boolean" Then
                TbtValue1 = New ToggleButton
                TbtValue1.Size = New Size(144, 23)
                TbtValue1.OnStyle.Text = "Sim"
                TbtValue1.OffStyle.Text = "Não"
                TbtValue1.Location = _ValueLocation
                TbtValue1.State = ToggleButton.ToggleButtonStates.ON

                FrmFilter.Controls.Add(TbtValue1)
                TbtValue1.Select()
            End If


            CbxOperador.ValueMember = "Value"
            CbxOperador.DisplayMember = "Display"
            CbxOperador.DataSource = OperatorList
            CbxOperador.SelectedIndex = 0






        End If
    End Sub

























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
    Public Property FilterName As String
    Public Property MainTable As New Model.Table
    Public Property RelatedTables As New List(Of Model.Table)
    Public Property Wheres As New List(Of Model.WhereClause)
    Public Property DecimalPlaces As Integer = 2
    Public Property FilterLimit As Long = 1000
    Public Property FreeWhereClause As String

    Public Sub New(ByVal Obj As Object)
        Dim DataTypes = IntegerTypes.Concat(TextTypes).Concat(DecimalTypes).Concat(DateTypes).Concat(BooleanTypes)
        MainTable.Name = Obj.GetType.Name
        MainTable.DisplayName = GetTableDisplayName(Obj.GetType.GetTypeInfo)
        Dim Columns() As String
        For Each p In Obj.GetType.GetProperties
            If DataTypes.Contains(p.PropertyType.Name) Then
                MainTable.Columns.Add(New Model.Column With {.Name = MainTable.Name & "." & p.Name, .DisplayName = MainTable.DisplayName & "." & GetColumnDisplayName(p), .DataType = GetColumnType(p.PropertyType.Name), .Visible = True})
            Else
                Columns = {}
                If p.GetCustomAttributes.Any(Function(x) x.GetType.Equals(GetType(Model.DisplayColumn))) Then
                    Columns = TryCast(p.GetCustomAttributes(GetType(Model.DisplayColumn), True)(0), Model.DisplayColumn).ColumnName
                End If
                FillRelatedTable(p, Columns)
            End If
        Next p
    End Sub

    Private Function HasConstructor() As Boolean
        For Each Where In Wheres
            If IsConstructor(Where.Parameter.Value) Or IsConstructor(Where.Parameter2.Value) Then
                Return True
            End If
        Next Where
        Return False
    End Function
    Private Function IsConstructor(ByVal s As String) As Boolean
        If Strings.Left(s, 1) = "[" And Strings.Right(s, 1) = "]" Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Function GetConstrucorCaption(ByVal s As String) As String
        Return Strings.Mid(s, 2, s.Length - 2)
    End Function


    Private Query As String
    Private ValueCounter As Integer

    Public Function GetResult() As Model.Result
        Dim Result As New Model.Result

        Dim Salt As Integer = 46
        Dim LabelTop As Integer = 9
        Dim ControlTop As Integer = 28
        ValueCounter = 0

        Query = "SELECT " & vbNewLine
        For Each Column In MainTable.Columns
            Query += vbTab & Column.Name & ", " & vbNewLine
        Next Column
        For Each Table In RelatedTables
            For Each Column In Table.Columns
                If Column.Visible Then
                    Query += vbTab & Column.Name & ", " & vbNewLine
                End If
            Next Column
        Next Table
        Query = Strings.Left(Query, Query.Length - 1)
        Query += "FROM "
        Query += MainTable.Name & vbNewLine
        RelatedTables.ForEach(Sub(x) Query += "JOIN " & x.Name & " ON " & x.Name & ".ID = " & MainTable.Name & "." & x.Name & "ID")
        If FreeWhereClause = Nothing Then
            If Wheres.Count > 0 Then
                Query += vbNewLine & "WHERE" & vbNewLine
                'se houver constructor entao apresenta o form para substituir o que esta nos values pelos valores reais.
                If HasConstructor() Then
                    ConstructorForm = New Form
                    ConstructorForm.Size = New Size(400, 150 - Salt)
                    ConstructorForm.MaximizeBox = False
                    ConstructorForm.MinimizeBox = False
                    ConstructorForm.FormBorderStyle = FormBorderStyle.FixedSingle
                    ConstructorForm.ShowIcon = False
                    ConstructorForm.ShowInTaskbar = False
                    ConstructorForm.Font = New Font("Century Gothic", 9.75)
                    ConstructorForm.BackColor = Color.White
                    ConstructorForm.Text = "Parâmetros do Filtro - " & FilterName


                    For Each Where In Wheres


                        If IsConstructor(Where.Parameter.Value) Then
                            ConstructorLabel = New Label
                            ConstructorLabel.AutoSize = True
                            ConstructorLabel.Text = Strings.Mid(Where.Parameter.Value, 2, Where.Parameter.Value.Length - 2)
                            ConstructorLabel.Location = New Point(12, LabelTop)
                            ConstructorForm.Controls.Add(ConstructorLabel)
                            LabelTop += Salt

                            If Where.Column.DataType = "Integer" Then

                                ConstructorDecimalBox = New DecimalBox
                                ConstructorDecimalBox.Width = 100
                                ConstructorDecimalBox.Location = New Point(15, ControlTop)
                                ConstructorDecimalBox.DecimalPlaces = 0
                                ConstructorDecimalBox.Tag = Where.Parameter
                                ConstructorForm.Controls.Add(ConstructorDecimalBox)
                                ControlTop += Salt
                                ConstructorForm.Height += Salt

                            ElseIf Where.Column.DataType = "Decimal" Then

                                ConstructorDecimalBox = New DecimalBox
                                ConstructorDecimalBox.Width = 100
                                ConstructorDecimalBox.Location = New Point(15, ControlTop)
                                ConstructorDecimalBox.DecimalPlaces = DecimalPlaces
                                ConstructorDecimalBox.Tag = Where.Parameter
                                ConstructorForm.Controls.Add(ConstructorDecimalBox)
                                ControlTop += Salt
                                ConstructorForm.Height += Salt



                            ElseIf Where.Column.DataType = "Date" Then

                                ConstructorDateBox = New DateBox
                                ConstructorDateBox.Width = 100
                                ConstructorDateBox.Location = New Point(15, ControlTop)
                                ConstructorDateBox.Tag = Where.Parameter
                                ConstructorForm.Controls.Add(ConstructorDateBox)
                                ControlTop += Salt
                                ConstructorForm.Height += Salt

                            ElseIf Where.Column.DataType = "Text" Then
                                ConstructorTextBox = New TextBox
                                ConstructorTextBox.Width = ConstructorForm.Width - 47
                                ConstructorTextBox.Location = New Point(15, ControlTop)
                                ConstructorTextBox.Tag = Where.Parameter
                                ConstructorForm.Controls.Add(ConstructorTextBox)
                                ControlTop += Salt
                                ConstructorForm.Height += Salt

                            ElseIf Where.Column.DataType = "Boolean" Then
                                ConstructorToggle = New ToggleButton
                                ConstructorToggle.Width = 100
                                ConstructorToggle.Location = New Point(15, ControlTop)
                                ConstructorToggle.Tag = Where.Parameter
                                ConstructorToggle.OnStyle.Text = "Sim"
                                ConstructorToggle.OffStyle.Text = "Não"
                                ConstructorToggle.State = ToggleButton.ToggleButtonStates.ON
                                ConstructorForm.Controls.Add(ConstructorToggle)
                                ControlTop += Salt
                                ConstructorForm.Height += Salt


                            End If

                        Else 'se nao tiver constructor no no parameter


                            Where.Parameter.Name = "@VALUE" & ValueCounter
                            ValueCounter += 1



                        End If




                        If Where.ComparsionOperator.Value = "BETWEEN" Then

                            If IsConstructor(Where.Parameter2.Value) Then



                                ConstructorLabel = New Label
                                ConstructorLabel.AutoSize = True
                                ConstructorLabel.Text = Strings.Mid(Where.Parameter2.Value, 2, Where.Parameter2.Value.Length - 2)
                                ConstructorLabel.Location = New Point(12, LabelTop)
                                ConstructorForm.Controls.Add(ConstructorLabel)
                                LabelTop += Salt




                                If Where.Column.DataType = "Integer" Then


                                    ConstructorDecimalBox = New DecimalBox
                                    ConstructorDecimalBox.Width = 100
                                    ConstructorDecimalBox.Location = New Point(15, ControlTop)
                                    ConstructorDecimalBox.DecimalPlaces = 0
                                    ConstructorDecimalBox.Tag = Where.Parameter2
                                    ConstructorForm.Controls.Add(ConstructorDecimalBox)
                                    ControlTop += Salt
                                    ConstructorForm.Height += Salt

                                ElseIf Where.Column.DataType = "Decimal" Then



                                    ConstructorDecimalBox = New DecimalBox
                                    ConstructorDecimalBox.Width = 100
                                    ConstructorDecimalBox.Location = New Point(15, ControlTop)
                                    ConstructorDecimalBox.DecimalPlaces = DecimalPlaces
                                    ConstructorDecimalBox.Tag = Where.Parameter2
                                    ConstructorForm.Controls.Add(ConstructorDecimalBox)
                                    ControlTop += Salt
                                    ConstructorForm.Height += Salt


                                ElseIf Where.Column.DataType = "Date" Then


                                    ConstructorDateBox = New DateBox
                                    ConstructorDateBox.Width = 100
                                    ConstructorDateBox.Location = New Point(15, ControlTop)
                                    ConstructorDateBox.Tag = Where.Parameter2
                                    ConstructorForm.Controls.Add(ConstructorDateBox)
                                    ControlTop += Salt
                                    ConstructorForm.Height += Salt
                                End If

                            Else 'se nao tiver constructor no no Parameter2


                                Where.Parameter2.Name = "@VALUE" & ValueCounter
                                ValueCounter += 1
                            End If


                        End If
                    Next Where

                    ConstructorButton = New Button
                    ConstructorButton.UseVisualStyleBackColor = True
                    ConstructorButton.Size = New Size(60, 30)

                    ConstructorButton.Location = New Point(15, ControlTop - 10)
                    ConstructorButton.Width = ConstructorForm.Width - 47
                    ConstructorButton.Text = "Filtrar"
                    ConstructorButton.DialogResult = DialogResult.OK
                    'ConstructorButton.Dock = DockStyle.Bottom
                    ConstructorForm.Controls.Add(ConstructorButton)
                    ConstructorForm.AcceptButton = ConstructorButton
                    If ConstructorForm.ShowDialog() = DialogResult.OK Then


                        For Each c As Control In ConstructorForm.Controls

                            If c.Tag IsNot Nothing AndAlso c.Tag.GetType Is GetType(Model.Parameter) Then

                                If c.GetType = GetType(DateBox) Then
                                    If c.Text = "  /  /" Then
                                        CType(c.Tag, Model.Parameter).Value = "1900-01-01"
                                    Else
                                        CType(c.Tag, Model.Parameter).Value = CDate(c.Text).ToString("yyyy-MM-dd")
                                    End If
                                ElseIf c.GetType = GetType(DecimalBox) Then


                                    CType(c.Tag, Model.Parameter).Value = CDec(c.Text)
                                ElseIf c.GetType = GetType(ToggleButton) Then

                                    CType(c.Tag, Model.Parameter).Value = If(CType(c, ToggleButton).State = ToggleButton.ToggleButtonStates.ON, True, False)
                                Else
                                    If c.Text = Nothing Then
                                        For Each w In Wheres
                                            If w.Parameter.Equals(c.Tag) Then
                                                Wheres.Remove(w)
                                                Exit For
                                            End If
                                        Next

                                    End If
                                    CType(c.Tag, Model.Parameter).Value = c.Text
                                End If
                                CType(c.Tag, Model.Parameter).Name = "@VALUE" & ValueCounter

                                ValueCounter += 1

                                'Result.Parameters.Add(c.Tag)
                            End If


                        Next c



                        For Each Where In Wheres
                            If Where.ComparsionOperator.Value = "BETWEEN" Then



                                If Where.Parameter.Name.ToString <> Nothing And Where.Parameter.Value.ToString <> Nothing And Where.Parameter2.Name.ToString <> Nothing And Where.Parameter2.Value.ToString <> Nothing Then
                                    Result.Parameters.Add(Where.Parameter)
                                    Query += vbTab & Where.Column.Name & " " & Where.ComparsionOperator.Value & " " & Where.Parameter.Name

                                    Result.Parameters.Add(Where.Parameter2)
                                    Query += " AND " & Where.Parameter2.Name
                                End If

                            Else
                                If Where.Parameter.Name.ToString <> Nothing And Where.Parameter.Value.ToString <> Nothing Then
                                    Result.Parameters.Add(Where.Parameter)
                                    Query += vbTab & Where.Column.Name & " " & Where.ComparsionOperator.Value & " " & Where.Parameter.Name
                                End If

                            End If




                            If Where IsNot Wheres.Last Then
                                Query += " " & Where.LogicalOperator.Value & vbNewLine
                            Else
                                Query += " LIMIT " & FilterLimit & ";"
                            End If
                        Next Where


                    Else
                        Return Nothing
                    End If

                End If
            Else
                Query += ";"

            End If
        Else
            Query += vbNewLine & "WHERE" & vbNewLine & FreeWhereClause & ";"
        End If



        Result.CommandText = Query.ToString

        Return Result
    End Function



    Private Sub FillRelatedTable(ByVal obj As Object, ByVal DisplayColumns() As String)
        Dim DataTypes = IntegerTypes.Concat(TextTypes).Concat(DateTypes).Concat(BooleanTypes)
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
                    Table.Columns.Add(New Model.Column With {.Name = Table.Name & "." & p.Name, .DisplayName = MainTable.DisplayName & "." & GetColumnDisplayName(p), .DataType = GetColumnType(p.PropertyType.Name), .Visible = IsVisible})
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

    Private Function GetColumnType(ByVal SistemType As String) As String
        If IntegerTypes.Contains(SistemType) Then
            Return "Integer"
        ElseIf DecimalTypes.Contains(SistemType) Then
            Return "Decimal"
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
        Public Class [Operator]
            Public Property Value As String
            Public Property Display As String
        End Class
        Public Class WhereClause
            Public Property Column As Column
            Public Property ComparsionOperator As New [Operator]
            Public Property Parameter As New Parameter
            Public Property Parameter2 As New Parameter
            Public Property LogicalOperator As New [Operator]
            Public Overrides Function ToString() As String
                Dim p1 As String = " é "
                Dim p2 As String = String.Empty
                Dim s As String
                Select Case ComparsionOperator.Value
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
                If ComparsionOperator.Value <> "BETWEEN" Then
                    s = Column.DisplayName & p1 & ComparsionOperator.Display & p2 & " " & If(Parameter.Value = Nothing, "{Vazio}", Parameter.Value) & vbTab & " " & LogicalOperator.Display
                Else
                    s = Column.DisplayName & " Está " & ComparsionOperator.Display & " " & Parameter.Value & " e " & Parameter2.Value & vbTab & " " & LogicalOperator.Display
                End If
                Return s
            End Function
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
                Return DisplayName.Split(".").ElementAt(1)
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

        Public Class Result
            Public Property CommandText As String
            Public Property Parameters As New List(Of Parameter)
        End Class


        Public Class Parameter
            Public Property Name As String
            Public Property Value As Object
        End Class
    End Class

    Public Sub ShowDialog()
        InitializeComponent()
        FrmFilter.ShowDialog()
    End Sub

    Private _DataType As String
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


        LblValue = New Label
        LblValue.AutoSize = True
        LblValue.Text = "Valor"
        LblValue.Location = New Point(285, 91)

        LblValue2 = New Label
        LblValue2.AutoSize = True
        LblValue2.Text = "Valor 2"
        LblValue2.Location = New Point(285, 138)


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
        TsBar.BackColor = Color.Gainsboro
        TsBar.RenderMode = ToolStripRenderMode.System
        TsBar.Items.AddRange({BtnDelete, BtnClean})

        PnDgvWheres = New Panel
        PnDgvWheres.BackColor = Color.White
        PnDgvWheres.Location = New Point(12, 339)
        PnDgvWheres.Size = New Size(415, 152)
        PnDgvWheres.Controls.AddRange({LbxWheres, TsBar})


        FrmFilter = New Form
        FrmFilter.Font = New Font("Century Gothic", 9.75)
        FrmFilter.Text = "Criador de Filtros"
        FrmFilter.Size = New Size(460, 600)
        FrmFilter.BackColor = Color.White
        FrmFilter.ShowIcon = False
        FrmFilter.ShowInTaskbar = False
        FrmFilter.MinimizeBox = False
        FrmFilter.MaximizeBox = False
        FrmFilter.FormBorderStyle = FormBorderStyle.FixedSingle
        FrmFilter.Controls.AddRange({TcTables, LblOperator, CbxOperador, LblValue, LblValue2, RbAnd, RbOr, BtnAdd, PnDgvWheres})


    End Sub
    Private _ValueLocation As New Point(288, 111)
    Private _value2Location As New Point(288, 158)
    Private _AndLocation As New Point(288, 140)
    Private _AndLocation2 As New Point(288, 187)
    Private _OrLocation As New Point(327, 140)
    Private _OrLocation2 As New Point(327, 187)
    Private _AddLocation As New Point(288, 167)
    Private _AddLocation2 As New Point(288, 214)
    Friend WithEvents FrmFilter As Form
    Friend WithEvents TcTables As TabControl
    Friend WithEvents TpTable As TabPage
    Friend WithEvents DgvColumns As DataGridView
    Friend WithEvents TxtValue1 As TextBox
    Friend WithEvents DbxValue1 As DecimalBox
    Friend WithEvents DbxValue2 As DecimalBox
    Friend WithEvents DtxValue1 As DateBox
    Friend WithEvents DtxValue2 As DateBox
    Friend WithEvents TbtValue1 As ToggleButton
    Friend WithEvents RbAnd As RadioButton
    Friend WithEvents RbOr As RadioButton
    Friend WithEvents BtnAdd As Button
    Friend WithEvents LblOperator As Label
    Friend WithEvents CbxOperador As ComboBox
    Friend WithEvents LblValue As Label
    Friend WithEvents LblValue2 As Label
    Friend WithEvents LbxWheres As ListBox
    Friend WithEvents PnDgvWheres As Panel
    Friend WithEvents TsBar As ToolStrip
    Friend WithEvents BtnDelete As ToolStripButton
    Friend WithEvents BtnClean As ToolStripButton

    Friend WithEvents ConstructorForm As Form
    Friend WithEvents ConstructorDateBox As DateBox
    Friend WithEvents ConstructorDecimalBox As DecimalBox
    Friend WithEvents ConstructorTextBox As TextBox
    Friend WithEvents ConstructorLabel As Label
    Friend WithEvents ConstructorButton As Button
    Friend WithEvents ConstructorToggle As ToggleButton
End Class
