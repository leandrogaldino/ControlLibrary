Imports System.ComponentModel
Imports System.Data.Common
Imports System.Drawing
Imports System.Linq.Expressions
Imports System.Windows.Forms

Public Class Utility
    ''' <summary>
    ''' Retorna se o controle especificado esta totalmente visível em seu controle pai.
    ''' </summary>
    ''' <param name="Parent">O controle pai.</param>
    ''' <param name="Child">O controle a ser testado.</param>
    ''' <returns></returns>
    Public Shared Function IsControlFullyVisible(ByVal Parent As Control, ByVal Child As Control) As Boolean
        Dim myBounds As Rectangle = Parent.ClientRectangle
        If Not myBounds.Contains(Child.Location) Then
            Return False
        End If
        If myBounds.Right < Child.Right Then
            Return False
        End If
        If myBounds.Height < Child.Bottom Then
            Return False
        End If
        Return True
    End Function
    ''' <summary>
    ''' Altera uma cor por outra na imagem.
    ''' </summary>
    ''' <param name="Image">A imagem a ser recolorida.</param>
    ''' <param name="FromColor">A cor na imagem a ser substituida.</param>
    ''' <param name="ToColor">A cor que deverá substituir a cor antiga.</param>
    ''' <returns></returns>
    Public Shared Function GetRecoloredImage(ByVal Image As Image, ByVal FromColor As Integer, ByVal ToColor As Integer) As Bitmap
        Dim bmp As Bitmap = Image
        For x As Integer = 0 To bmp.Width - 1
            For y As Integer = 0 To bmp.Height - 1
                If bmp.GetPixel(x, y) = Color.FromArgb(FromColor) Then
                    bmp.SetPixel(x, y, Color.FromArgb(ToColor))
                End If
            Next
        Next
        Return bmp
    End Function
    Public Shared Function GetImageColors(ByVal Img As Image) As List(Of Color)
        Dim Lst As New List(Of Color)
        Dim bmp As Bitmap = Img
        For x As Integer = 0 To bmp.Width - 1
            For y As Integer = 0 To bmp.Height - 1
                Lst.Add(bmp.GetPixel(x, y))
            Next
        Next
        Return Lst.Distinct().ToList
    End Function

    ''' <summary>
    ''' Depura um ComandoSQL, substituindo os parâmetros na query.
    ''' </summary>
    ''' <param name="Command">O SqlCommand contendo a query e os paramêtros.</param>
    Public Shared Sub DebugQuery(ByVal Command As DbCommand)
        Dim Query As String = Command.CommandText
        For Each Parameter As DbParameter In Command.Parameters
            Query = Query.Replace(Parameter.ParameterName, "'" & Parameter.Value & "'")
        Next
        Debug.Print(Query)
    End Sub
    ''' <summary>
    ''' Cria uma lista com suporte a associação de dados.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    Public Class SortableList(Of T)
        Inherits BindingList(Of T)
        Private originalList As List(Of T)
        Private sortDirection As ListSortDirection
        Private sortProperty As PropertyDescriptor
        Private populateBaseList As Action(Of SortableList(Of T), List(Of T)) = Sub(a, b) a.ResetItems(b)
        Shared cachedOrderByExpressions As Dictionary(Of String, Func(Of List(Of T), IEnumerable(Of T))) = New Dictionary(Of String, Func(Of List(Of T), IEnumerable(Of T)))()
        Public Sub New()
            originalList = New List(Of T)()
        End Sub
        Public Sub New(ByVal enumerable As IEnumerable(Of T))
            originalList = enumerable.ToList()
            populateBaseList(Me, originalList)
        End Sub
        Public Sub New(ByVal list As List(Of T))
            originalList = list
            populateBaseList(Me, originalList)
        End Sub
        Protected Overrides Sub ApplySortCore(ByVal prop As PropertyDescriptor, ByVal direction As ListSortDirection)
            sortProperty = prop
            Dim orderByMethodName = If(direction = ListSortDirection.Ascending, "OrderBy", "OrderByDescending")
            Dim cacheKey = GetType(T).GUID.ToString & prop.Name & orderByMethodName
            If Not cachedOrderByExpressions.ContainsKey(cacheKey) Then
                CreateOrderByMethod(prop, orderByMethodName, cacheKey)
            End If
            ResetItems(cachedOrderByExpressions(cacheKey)(originalList).ToList())
            ResetBindings()
            sortDirection = If(direction = ListSortDirection.Ascending, ListSortDirection.Descending, ListSortDirection.Ascending)
        End Sub
        Private Sub CreateOrderByMethod(ByVal prop As PropertyDescriptor, ByVal orderByMethodName As String, ByVal cacheKey As String)
            Dim sourceParameter = Expression.Parameter(GetType(List(Of T)), "source")
            Dim lambdaParameter = Expression.Parameter(GetType(T), "lambdaParameter")
            Dim accesedMember = GetType(T).GetProperty(prop.Name)
            Dim propertySelectorLambda = Expression.Lambda(Expression.MakeMemberAccess(lambdaParameter, accesedMember), lambdaParameter)
            Dim orderByMethod = GetType(Enumerable).GetMethods().Where(Function(a) a.Name = orderByMethodName AndAlso a.GetParameters().Length = 2).Single().MakeGenericMethod(GetType(T), prop.PropertyType)
            Dim orderByExpression = Expression.Lambda(Of Func(Of List(Of T), IEnumerable(Of T)))(Expression.[Call](orderByMethod, New Expression() {sourceParameter, propertySelectorLambda}), sourceParameter)
            cachedOrderByExpressions.Add(cacheKey, orderByExpression.Compile())
        End Sub
        Protected Overrides Sub RemoveSortCore()
            ResetItems(originalList)
        End Sub

        Private Sub ResetItems(ByVal items As List(Of T))
            MyBase.ClearItems()

            For i As Integer = 0 To items.Count - 1
                MyBase.InsertItem(i, items(i))
            Next
        End Sub
        Protected Overrides ReadOnly Property SupportsSortingCore As Boolean
            Get
                Return True
            End Get
        End Property
        Protected Overrides ReadOnly Property SortDirectionCore As ListSortDirection
            Get
                Return sortDirection
            End Get
        End Property
        Protected Overrides ReadOnly Property SortPropertyCore As PropertyDescriptor
            Get
                Return sortProperty
            End Get
        End Property
        Protected Overrides Sub OnListChanged(ByVal e As ListChangedEventArgs)
            originalList = MyBase.Items.ToList()
        End Sub
    End Class

End Class