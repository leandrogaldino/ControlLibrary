Imports System.ComponentModel

Class DictionaryPropertyGridAdapter
    Implements ICustomTypeDescriptor

    Private _dictionary As IDictionary

    Public Sub New(ByVal d As IDictionary)
        _dictionary = d
    End Sub

    Public Function GetComponentName() As String Implements ICustomTypeDescriptor.GetComponentName
        Return TypeDescriptor.GetComponentName(Me, True)
    End Function

    Public Function GetDefaultEvent() As EventDescriptor Implements ICustomTypeDescriptor.GetDefaultEvent
        Return TypeDescriptor.GetDefaultEvent(Me, True)
    End Function

    Public Function GetClassName() As String Implements ICustomTypeDescriptor.GetClassName
        Return TypeDescriptor.GetClassName(Me, True)
    End Function

    Public Function GetEvents(ByVal attributes As Attribute()) As EventDescriptorCollection Implements ICustomTypeDescriptor.GetEvents
        Return TypeDescriptor.GetEvents(Me, attributes, True)
    End Function

    Private Function GetEvents() As EventDescriptorCollection Implements ICustomTypeDescriptor.GetEvents
        Return TypeDescriptor.GetEvents(Me, True)
    End Function

    Public Function GetConverter() As TypeConverter Implements ICustomTypeDescriptor.GetConverter
        Return TypeDescriptor.GetConverter(Me, True)
    End Function

    Public Function GetPropertyOwner(ByVal pd As PropertyDescriptor) As Object Implements ICustomTypeDescriptor.GetPropertyOwner
        Return _dictionary
    End Function

    Public Function GetAttributes() As AttributeCollection Implements ICustomTypeDescriptor.GetAttributes
        Return TypeDescriptor.GetAttributes(Me, True)
    End Function

    Public Function GetEditor(ByVal editorBaseType As Type) As Object Implements ICustomTypeDescriptor.GetEditor
        Return TypeDescriptor.GetEditor(Me, editorBaseType, True)
    End Function

    Public Function GetDefaultProperty() As PropertyDescriptor Implements ICustomTypeDescriptor.GetDefaultProperty
        Return Nothing
    End Function

    Private Function GetProperties() As PropertyDescriptorCollection Implements ICustomTypeDescriptor.GetProperties
        Return (CType(Me, ICustomTypeDescriptor)).GetProperties(New Attribute(-1) {})
    End Function

    Public Function GetProperties(ByVal attributes As Attribute()) As PropertyDescriptorCollection Implements ICustomTypeDescriptor.GetProperties
        Dim properties As ArrayList = New ArrayList()

        For Each e As DictionaryEntry In _dictionary
            properties.Add(New DictionaryPropertyDescriptor(_dictionary, e.Key))
        Next

        Dim props As PropertyDescriptor() = CType(properties.ToArray(GetType(PropertyDescriptor)), PropertyDescriptor())
        Return New PropertyDescriptorCollection(props)
    End Function
End Class

Class DictionaryPropertyDescriptor
        Inherits PropertyDescriptor

        Private _dictionary As IDictionary
        Private _key As Object

        Friend Sub New(ByVal d As IDictionary, ByVal key As Object)
            MyBase.New(key.ToString(), Nothing)
            _dictionary = d
            _key = key
        End Sub

        Public Overrides ReadOnly Property PropertyType As Type
            Get
                Return _dictionary(_key).[GetType]()
            End Get
        End Property

        Public Overrides Sub SetValue(ByVal component As Object, ByVal value As Object)
            _dictionary(_key) = value
        End Sub

        Public Overrides Function GetValue(ByVal component As Object) As Object
            Return _dictionary(_key)
        End Function

        Public Overrides ReadOnly Property IsReadOnly As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property ComponentType As Type
            Get
                Return Nothing
            End Get
        End Property

        Public Overrides Function CanResetValue(ByVal component As Object) As Boolean
            Return False
        End Function

        Public Overrides Sub ResetValue(ByVal component As Object)
        End Sub

        Public Overrides Function ShouldSerializeValue(ByVal component As Object) As Boolean
            Return False
        End Function
    End Class



