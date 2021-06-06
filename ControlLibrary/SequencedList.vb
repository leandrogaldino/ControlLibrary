Public Class SequencedList(Of T)
    Inherits CollectionBase
    Implements ICollection(Of T)

    Public Property LastSequence As Long
    Protected Overrides Sub OnInsert(index As Integer, value As Object)
        Dim HasSeq = value.GetType.GetProperties.Any(Function(x) x.Name.ToUpper = "Seq".ToUpper)
        If HasSeq Then
            If value.Seq = 0 Then
                If LastSequence = 0 Then
                    value.Seq = 1
                    LastSequence = 1
                Else
                    LastSequence += 1
                    value.Seq = LastSequence()
                End If
            End If
            MyBase.OnInsert(index, value)
        Else
            Throw New MissingMemberException("'Seq' property not found in " & value.GetType.ToString & ".")
        End If
    End Sub

    Public Function Contains(ByVal item As T) As Boolean
        Return MyBase.List.Contains(item)
    End Function

    Public Function IndexOf(ByVal item As T) As Integer
        Return MyBase.List.IndexOf(item)
    End Function

    Public Sub Insert(ByVal index As Integer, ByVal item As T)
        MyBase.List.Insert(index, item)
    End Sub

    Default Public Property Item(ByVal index As Integer) As T
        Get
            Return CType(
               MyBase.List(index), T)
        End Get

        Set(ByVal Value As T)
            List(index) = Value
        End Set
    End Property

    Public Function Add(ByVal value As T) As Integer
        Return MyBase.List.Add(value)
    End Function

    Public Sub Remove(ByVal item As T)
        MyBase.List.Remove(item)
    End Sub

    Private ReadOnly Property ICollection_Count As Integer Implements ICollection(Of T).Count
        Get
            Return MyBase.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly



    Private Sub ICollection_Add(item As T) Implements ICollection(Of T).Add
        MyBase.List.Add(item)
    End Sub

    Private Sub ICollection_Clear() Implements ICollection(Of T).Clear
        MyBase.Clear()
    End Sub

    Private Function ICollection_Contains(item As T) As Boolean Implements ICollection(Of T).Contains
        Return MyBase.List.Contains(item)
    End Function

    Public Sub CopyTo(array() As T, arrayIndex As Integer) Implements ICollection(Of T).CopyTo
        Me.List.CopyTo(array, arrayIndex)
    End Sub

    Private Function ICollection_Remove(item As T) As Boolean Implements ICollection(Of T).Remove
        If Me.Contains(item) Then
            MyBase.List.Remove(item)
        End If
        Return True
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
        Return MyBase.GetEnumerator
    End Function

End Class
