Imports System.ComponentModel
Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms
Imports System.Xml

Public Class FilterBuilder
    Private Function GetFieldAlias(ByVal pi As PropertyInfo) As String
        Dim DisplayName As String = String.Empty
        If pi.GetCustomAttributes(GetType(DisplayNameAttribute), True).Cast(Of DisplayNameAttribute).Count > 0 Then
            DisplayName = pi.GetCustomAttributes(GetType(DisplayNameAttribute), True).Cast(Of DisplayNameAttribute)().FirstOrDefault().DisplayName
        Else
            DisplayName = pi.Name
        End If
        Return DisplayName
    End Function
    Private Function GetTableAlias(ByVal pi As TypeInfo) As String
        Dim DisplayName As String = String.Empty
        If pi.GetCustomAttributes(GetType(DisplayNameAttribute), True).Cast(Of DisplayNameAttribute).Count > 0 Then
            DisplayName = pi.GetCustomAttributes(GetType(DisplayNameAttribute), True).Cast(Of DisplayNameAttribute)().FirstOrDefault().DisplayName
        Else
            DisplayName = pi.Name
        End If
        Return DisplayName
    End Function


    Private Sub Load(ObjType As Type, Optional IsRelatedTable As Boolean = False, Optional Relation As String = Nothing)
        Dim DataTypes = IntegerTypes.Concat(TextTypes).Concat(DecimalTypes).Concat(DateTypes).Concat(BooleanTypes)
        Dim Table As New Model.Table
        Dim Column As Model.Column
        Table.TableName = ObjType.Name
        Table.Relation = If(Relation = Nothing, ObjType.Name, Relation)
        Table.TableAlias = GetTableAlias(ObjType.GetTypeInfo)
        For Each p As PropertyInfo In ObjType.GetProperties
            If Not IsCollection(p) Then
                If DataTypes.Contains(p.PropertyType.Name) Then
                    Column = New Model.Column With {
                        .Name = Table.TableAlias & "." & p.Name,
                        .DisplayName = Table.TableAlias & "." & GetFieldAlias(p),
                        .DataType = GetColumnType(p.PropertyType.Name)
                    }
                    Table.Columns.Add(Column)
                Else
                    Load(p.PropertyType, True, p.Name)
                End If
            End If
        Next p
        If Not IsRelatedTable Then
            MainTable = Table
        Else
            RelatedTables.Insert(0, Table)
        End If
    End Sub
    Public Sub New(Path As String)
        Dim Document As New XmlDocument
        Dim Where As Model.WhereClause
        Dim ObjName As String
        Dim ObjType As Type
        Document = New XmlDocument
        Document.Load(Path)
        Where = New Model.WhereClause
        ObjName = Document.SelectNodes("Filter/Object").Item(0).InnerText
        ObjType = Type.GetType(String.Format("{0}, {1}", ObjName, ObjName.Split(".").ElementAt(0)))


        Load(ObjType)

        FilterName = Document.SelectNodes("Filter/FilterName").Item(0).InnerText
        FreeWhereClause = Document.SelectNodes("Filter/FreeWhere").Item(0).InnerText

        For Each Element As XmlElement In Document.SelectNodes("Filter/Wheres/Where")

            Where = New Model.WhereClause
            Where.Column = MainTable.Columns.Find(Function(x) x.Name = Element("Column").InnerText)

            Where.ComparsionOperator.Value = Element("Operator").InnerText

            Where.LogicalOperator.Value = Element("Operator2").InnerText
            Where.Parameter.Value = Element("Value").InnerText
            Where.Parameter2.Value = Element("Value2").InnerText



            Wheres.Add(Where)


        Next
    End Sub
    Public Sub New(ObjType As Type)
        Load(ObjType)
    End Sub

    Public Sub Save(ByVal Path As String)

        Using Writer As New XmlTextWriter(Path, Nothing)
            Writer.Formatting = Formatting.Indented
            Writer.WriteStartElement("Filter")
            Writer.WriteElementString("Object", Assembly.GetEntryAssembly.GetName.Name & "." & Me.MainTable.TableName)
            Writer.WriteElementString("FilterName", FilterName)
            Writer.WriteElementString("FreeWhere", FreeWhereClause)
            Writer.WriteStartElement("Wheres")
            For Each Where In Wheres
                Writer.WriteStartElement("Where")


                Writer.WriteElementString("Column", Where.Column.Name)
                Writer.WriteElementString("Operator", Where.ComparsionOperator.Value)
                Writer.WriteElementString("Operator2", Where.LogicalOperator.Value)
                Writer.WriteElementString("Value", Where.Parameter.Value)
                Writer.WriteElementString("Value2", Where.Parameter2.Value)

                Writer.WriteEndElement()
            Next Where

            Writer.WriteEndElement()
            Writer.WriteEndElement()
            Writer.Close()
        End Using


    End Sub



    Public Function GetResult() As Model.Result
        Dim Result As New Model.Result
        Dim Salt As Integer = 46
        Dim LabelTop As Integer = 9
        Dim ControlTop As Integer = 28



        ValueCounter = 0

        Query = "SELECT " & vbNewLine



        For Each Column In MainTable.Columns
            Query += String.Format("{0}[{1}].[{2}] AS [{3}],{4}", vbTab, Column.Name.Split(".").ElementAt(0), Column.Name.Split(".").ElementAt(1), Column.DisplayName.Split(".").ElementAt(1), vbNewLine)
        Next Column



        For Each Table In RelatedTables


            For Each Column In Table.Columns





                Query += String.Format("{0}[{1}].[{2}] AS [{3}],{4}", vbTab, Column.Name.Split(".").ElementAt(0), Column.Name.Split(".").ElementAt(1), Column.DisplayName.Split(".").ElementAt(1), vbNewLine)




            Next Column
        Next Table
        Query = Strings.Left(Query, Query.Length - 3)
        Query += String.Format("{0}FROM [{1}]{2}", vbNewLine, MainTable.TableName, vbNewLine)




        For Each Table In RelatedTables


            Query += String.Format("JOIN [{0}] AS [{1}] ON [{2}].[ID] = [{3}].[{4}ID]{5}", Table.TableName, Table.TableAlias, Table.TableAlias, Table.Relation, Table.TableName, vbNewLine)


        Next


        If Strings.Right(Query, 2) = vbNewLine Then Query = Strings.Left(Query, Query.Length - 2)

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
                End If

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

                If HasConstructor() Then
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
                                If c.GetType Is GetType(ToggleButton) Then
                                    CType(c.Tag, Model.Parameter).Value = CInt(CType(c, ToggleButton).State)
                                Else
                                    CType(c.Tag, Model.Parameter).Value = c.Text
                                End If
                                CType(c.Tag, Model.Parameter).Name = "@VALUE" & ValueCounter
                                ValueCounter += 1
                            End If
                        Next c

                        For Each Where In Wheres

                            If Where.Column.DataType = "Integer" Or Where.Column.DataType = "Decimal" Then
                                If Where.Parameter.Value = Nothing OrElse Where.Parameter.Value < -999999999 Then Where.Parameter.Value = -999999999
                            End If
                            If Where.Column.DataType = "Date" Then
                                If Not IsDate(Where.Parameter.Value) OrElse Where.Parameter.Value < CDate("1900-01-01") Then Where.Parameter.Value = New Date(1900, 1, 1)
                            End If

                            Result.Parameters.Add(Where.Parameter)
                            Query += vbTab & Where.Column.Name & " " & Where.ComparsionOperator.Value & " " & Where.Parameter.Name

                            If Where.ComparsionOperator.Value = "BETWEEN" Then
                                If Where.Column.DataType = "Integer" Or Where.Column.DataType = "Decimal" Then
                                    If Where.Parameter2.Value = Nothing OrElse Where.Parameter2.Value > 999999999 Then Where.Parameter2.Value = 999999999
                                End If

                                If Where.Column.DataType = "Date" Then
                                    If Not IsDate(Where.Parameter2.Value) Then Where.Parameter2.Value = Today
                                End If

                                Result.Parameters.Add(Where.Parameter2)
                                Query += " AND " & Where.Parameter2.Name
                            End If



                            If Where IsNot Wheres.Last Then
                                Query += " " & Where.LogicalOperator.Value & vbNewLine
                            Else
                                Query += " LIMIT " & FilterLimit & ";"
                            End If
                        Next



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

            If Column.DataType = "Boolean" Then
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
        Where.ComparsionOperator.Value = CbxOperador.SelectedValue
        Where.ComparsionOperator.Display = CbxOperador.Text
        Where.LogicalOperator.Display = If(RbAnd.Checked, "e", "ou")
        Where.LogicalOperator.Value = If(RbAnd.Checked, "AND", "OR")

        Where.Parameter.Value = TxtValue1.Text

        If CbxOperador.SelectedValue = "BETWEEN" Then
            Where.Parameter2.Value = TxtValue2.Text
        End If

        If Where.ComparsionOperator.Value = "BETWEEN" Then
            If Where.Column.DataType = "Date" Then
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

            CbxOperador.ValueMember = "Value"
            CbxOperador.DisplayMember = "Display"
            CbxOperador.DataSource = OperatorList
            CbxOperador.SelectedIndex = 0

            TxtValue1.Text = Nothing
            TxtValue2.Text = Nothing

            If _DataType = "Text" Then
                LblDescription.Text = "Tipo: Texto"
            ElseIf _DataType = "Integer" Then
                LblDescription.Text = "Tipo: Número Inteiro"
            ElseIf _DataType = "Decimal" Then
                LblDescription.Text = "Tipo: Número Decimal"
            ElseIf _DataType = "Date" Then
                LblDescription.Text = "Tipo: Data"
            ElseIf _DataType = "Boolean" Then
                LblDescription.Text = "Tipo: Sim/Não"
            End If



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
    Public Property Wheres As New ObjectModel.Collection(Of Model.WhereClause)
    Public Property DecimalPlaces As Integer = 2
    Public Property FilterLimit As Long = 1000
    Public Property FreeWhereClause As String


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
    Private Query As String
    Private ValueCounter As Integer



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
            Public Property TableName As String
            Public Property TableAlias As String
            Public Property Relation As String
            Public Property Columns As New List(Of Column)
            Public Overrides Function ToString() As String
                Return TableAlias
            End Function
        End Class
        Public Class Column
            Public Property Name As String
            Public Property DisplayName As String
            Public Property DataType As String
            Public Overrides Function ToString() As String
                Return DisplayName.Split(".").ElementAt(1)
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
                DgvColumns.Rows(DgvColumns.RowCount - 1).Cells(0).ToolTipText = Column.Name
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
End Class

