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


    'funcao para verificar se a colecao é de tipos primitivos
    'funcao para retornar uma lista com todos os tipos primitivos






    'Verifica se a propriedade é uma coleção
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
    End Class

End Class


