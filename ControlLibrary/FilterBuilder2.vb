Public Class FilterBuilder2
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
        Return If(GetPrimitiveTypes.Contains(intIndexer.ReturnType.Name), True, False)
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
        Return Nothing
    End Function
    'Retorna o valor do atributo TableAlias na classe.
    Private Function GetTableAlias(t As Reflection.TypeInfo) As String
        For Each Attr In t.GetCustomAttributes(True)
            If Attr.GetType Is GetType(Model.Attributes.TableAlias) Then
                Return CType(Attr, Model.Attributes.TableAlias).Value
            End If
        Next
        Return Nothing
    End Function



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
                    Case Is = "Like"
                        Return "Contém"
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
                    s = Column.ColumnAlias & p1 & RelationalOperator.Display & p2 & " " & If(Parameter.Value = Nothing, "{Vazio}", Parameter.Value) & vbTab & " " & RelationalOperator.Display
                Else
                    s = Column.ColumnAlias & " Está " & RelationalOperator.Display & " " & Parameter.Value & " e " & Parameter2.Value & vbTab & " " & RelationalOperator.Display
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
            Public Property ColumnName As String
            Public Property ColumnAlias As String
            Public Property ColumnType As String
            Public Overrides Function ToString() As String
                Return ColumnAlias.Split(".").ElementAt(1)
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


