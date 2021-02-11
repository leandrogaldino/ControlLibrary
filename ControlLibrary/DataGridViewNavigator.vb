Imports System.ComponentModel
Imports System.Windows.Forms
Public Class DataGridViewNavigator
    Inherits Component
    Private _DataGridView As New DataGridView
    Private _FirstButton As ToolStripButton
    Private _PreviousButton As ToolStripButton
    Private _NextButton As ToolStripButton
    Private _LastButton As ToolStripButton
    Public Property DataGridView As DataGridView
        Get
            Return _DataGridView
        End Get
        Set(value As DataGridView)
            _DataGridView = value
            If _DataGridView IsNot Nothing Then
                _DataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                If DefinedButtons() Then RefreshButtons()
            End If
        End Set
    End Property
    Private Function DefinedButtons() As Boolean
        If FirstButton IsNot Nothing And
                PreviousButton IsNot Nothing And
                NextButton IsNot Nothing And
                LastButton IsNot Nothing Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Property FirstButton As ToolStripButton
        Get
            Return _FirstButton
        End Get
        Set(value As ToolStripButton)
            _FirstButton = value
            If _FirstButton IsNot Nothing Then
                _FirstButton = value
                If DefinedButtons() Then RefreshButtons()
            End If
        End Set
    End Property
    Public Property PreviousButton As ToolStripButton
        Get
            Return _PreviousButton
        End Get
        Set(value As ToolStripButton)
            _PreviousButton = value
            If _PreviousButton IsNot Nothing Then
                _PreviousButton = value
                If DefinedButtons() Then RefreshButtons()
            End If
        End Set
    End Property
    Public Property NextButton As ToolStripButton
        Get
            Return _NextButton
        End Get
        Set(value As ToolStripButton)
            _NextButton = value
            If NextButton IsNot Nothing Then
                _NextButton = value
                If DefinedButtons() Then RefreshButtons()
            End If
        End Set
    End Property
    Public Property LastButton As ToolStripButton
        Get
            Return _LastButton
        End Get
        Set(value As ToolStripButton)
            _LastButton = value
            If _LastButton IsNot Nothing Then
                _LastButton = value
                If DefinedButtons() Then RefreshButtons()
            End If
        End Set
    End Property
    Public Sub EnsureVisibleRow(ByVal RowToShow As Integer)
        If RowToShow >= 0 AndAlso RowToShow < _DataGridView.RowCount Then
            _DataGridView.Rows(RowToShow).Selected = True
            Dim CountVisible = _DataGridView.DisplayedRowCount(False)
            Dim FirstVisible = _DataGridView.FirstDisplayedScrollingRowIndex
            If RowToShow < FirstVisible Then
                _DataGridView.FirstDisplayedScrollingRowIndex = RowToShow
            ElseIf RowToShow >= FirstVisible + CountVisible Then
                _DataGridView.FirstDisplayedScrollingRowIndex = RowToShow - CountVisible + If(CountVisible > 0, 1, 0)
            End If
        End If
    End Sub

    Public Sub RefreshButtons()
        If _DataGridView.Rows.Count > 0 Then
            _FirstButton.Enabled = If(_DataGridView.SelectedRows(0).Index > 0, True, False)
            _PreviousButton.Enabled = If(_DataGridView.SelectedRows(0).Index > 0, True, False)
            _NextButton.Enabled = If(_DataGridView.SelectedRows(0).Index < _DataGridView.Rows.Count - 1, True, False)
            _LastButton.Enabled = If(_DataGridView.SelectedRows(0).Index < _DataGridView.Rows.Count - 1, True, False)
        Else
            _FirstButton.Enabled = False
            _PreviousButton.Enabled = False
            _NextButton.Enabled = False
            _LastButton.Enabled = False
        End If
    End Sub
    Private Function IsPartialRow() As Boolean
        If _DataGridView.SelectedRows(0).Index > _DataGridView.FirstDisplayedScrollingRowIndex + _DataGridView.DisplayedRowCount(False) - 1 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub MoveToFirst()
        If _DataGridView.SelectedRows.Count = 1 Then
            If _DataGridView.SelectedRows(0).Index > 0 Then
                _DataGridView.Rows(0).Selected = True
                RefreshButtons()
                If _DataGridView.SelectedRows.Count > 0 Then EnsureVisibleRow(_DataGridView.SelectedRows(0).Index)
            End If
        End If
    End Sub
    Public Sub MoveToPrevious()
        If _DataGridView.SelectedRows.Count = 1 Then
            If _DataGridView.SelectedRows(0).Index > 0 Then
                _DataGridView.Rows(_DataGridView.SelectedRows(0).Index - 1).Selected = True
                RefreshButtons()
                If _DataGridView.SelectedRows.Count > 0 Then EnsureVisibleRow(_DataGridView.SelectedRows(0).Index)
            End If
        End If
    End Sub
    Public Sub MoveToNext()
        If _DataGridView.SelectedRows.Count = 1 Then
            If _DataGridView.SelectedRows(0).Index < _DataGridView.Rows.Count - 1 Then
                _DataGridView.Rows(_DataGridView.SelectedRows(0).Index + 1).Selected = True
                RefreshButtons()
                If _DataGridView.SelectedRows.Count > 0 Then EnsureVisibleRow(_DataGridView.SelectedRows(0).Index)
            End If
        End If
    End Sub
    Public Sub MoveToLast()
        If _DataGridView.SelectedRows.Count = 1 Then
            If _DataGridView.SelectedRows(0).Index < _DataGridView.Rows.Count - 1 Then
                _DataGridView.Rows(_DataGridView.Rows.Count - 1).Selected = True
                RefreshButtons()
                If _DataGridView.SelectedRows.Count > 0 Then EnsureVisibleRow(_DataGridView.SelectedRows(0).Index)
            End If
        End If
    End Sub
End Class
