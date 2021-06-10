Imports System.ComponentModel
Imports System.Reflection
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


    Public Function GetSelectCommand() As String
        Dim Q As New Text.StringBuilder
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
            If i < Wheres.Count - 1 Then
                Q.AppendLine(vbTab & Wheres(i).Column.Name & " " & GetComparsionOperator(Wheres(i).ComparsionOperator) & " " & If(Wheres(i).Value Is Nothing, "@Value", Wheres(i).Value) & " " & GetLogicalOperator(Wheres(i).LogicalOperator))
            Else
                Q.AppendLine(vbTab & Wheres(i).Column.Name & " " & GetComparsionOperator(Wheres(i).ComparsionOperator) & " " & If(Wheres(i).Value Is Nothing, "@Value;", Wheres(i).Value & ";"))
            End If
        Next i






        Return Q.ToString
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
                If p.GetCustomAttributes.Any(Function(x) x.GetType.Equals(GetType(Model.DisplayColumns))) Then
                    Columns = TryCast(p.GetCustomAttributes(GetType(Model.DisplayColumns), True)(0), Model.DisplayColumns).Columns
                End If
                FillRelatedTable(p, Columns)
            End If
        Next p
    End Sub

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
                    If p.GetCustomAttributes.Any(Function(x) x.GetType.Equals(GetType(Model.DisplayColumns))) Then
                        Columns = TryCast(p.GetCustomAttributes(GetType(Model.DisplayColumns), True)(0), Model.DisplayColumns).Columns
                    End If
                    FillRelatedTable(p, Columns)
                End If
            Next p
            RelatedTables.Add(Table)
        End If

        'If Not IsCollection(obj) Then RelatedTables.Insert(0, Table)
        'If Not IsCollection(obj) Then RelatedTables.Add(Table)
    End Sub
    Public Property MainTable As New Model.Table
    Public Property RelatedTables As New List(Of Model.Table)
    Public Property Wheres As New List(Of Model.WhereClause)

    Public Class Model
        Public Class WhereClause
            Public Property Column As Column
            Public Property ComparsionOperator As ComparsionOperators
            Public Property Value As Object
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
        Public Class DisplayColumns
            Inherits Attribute
            Public Property Columns As String()
            Public Sub New(ByVal Columns() As String)
                Me.Columns = Columns
            End Sub

        End Class
    End Class
End Class
