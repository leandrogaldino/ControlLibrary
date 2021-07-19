Imports System.ComponentModel
Imports ControlLibrary.FilterBuilder.Model

Public Class Order
    <TypeConverter(GetType(CustomNumberTypeConverter))>
    Public Property ID As Long
    <DisplayName("Usuario")>
    Public Property User As Person
    <DisplayName("Cliente")>
    Public Property Customer As Person
    Public Property Items As List(Of OrderItem)
    Public Property Creationdate As Date
    Public Property IsGood As Boolean
End Class


Public Class CustomNumberTypeConverter
    Inherits TypeConverter



    Public Overrides Function CanConvertFrom(ByVal context As ITypeDescriptorContext, ByVal sourceType As Type) As Boolean
        Return sourceType = GetType(String)
    End Function


    Public Overrides Function ConvertFrom(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object) As Object


        Return value.ToString
    End Function

    Public Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As Type) As Object
        If destinationType = GetType(String) Then Return (CInt(value)).ToString("N0", culture)
        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function
End Class
