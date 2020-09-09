Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms
Imports System.Windows.Forms.Design
Public MustInherit Class UITypeEditorBase
    Inherits UITypeEditor
    Protected IEditorService As IWindowsFormsEditorService
    Protected WithEvents EditControl As Control
    Protected m_EscapePressed As Boolean
    Protected Friend Property Caption As String = "UITypeEditor"
    Protected MustOverride Function GetEditControl(ByVal PropertyName As String, ByVal CurrentValue As Object) As Control
    Protected MustOverride Function GetEditedValue(ByVal EditControl As Control, ByVal PropertyName As String, ByVal OldValue As Object) As Object
    Protected MustOverride Sub LoadValues(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object)
    Protected MustOverride Function SetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
    Public NotOverridable Overrides Function GetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
        Return SetEditStyle(context)
    End Function
    Public Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
        Try
            If context IsNot Nothing AndAlso provider IsNot Nothing Then
                IEditorService = DirectCast(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)
                If IEditorService IsNot Nothing Then
                    Dim PropName As String = context.PropertyDescriptor.Name
                    EditControl = Me.GetEditControl(PropName, value)
                    Me.LoadValues(context, provider, value)
                    If EditControl IsNot Nothing Then
                        m_EscapePressed = False
                        If TypeOf EditControl Is Form Then
                            IEditorService.ShowDialog(CType(EditControl, Form))
                        Else
                            IEditorService.DropDownControl(EditControl)
                        End If
                        If m_EscapePressed Then
                            Return value
                        Else
                            Return GetEditedValue(EditControl, PropName, value)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Windows.Forms.MessageBox.Show(ex.Message)
        End Try
        Return MyBase.EditValue(context, provider, value)
    End Function
    Public Function GetIWindowsFormsEditorService() As IWindowsFormsEditorService
        Return IEditorService
    End Function
    Public Sub CloseDropDownWindow()
        If IEditorService IsNot Nothing Then IEditorService.CloseDropDown()
    End Sub
    Private Sub m_EditControl_PreviewKeyDown(ByVal sender As Object, ByVal e As PreviewKeyDownEventArgs) _
              Handles EditControl.PreviewKeyDown
        If e.KeyCode = Keys.Escape Then m_EscapePressed = True
    End Sub
    Protected Friend Sub DisplayError(msg As String)
        Windows.Forms.MessageBox.Show(msg, Caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    End Sub
    Protected Friend Class ListItem
        Public Property [Control] As Control
        Public Property Name As String

        Public Sub New(c As Control)
            [Control] = c
            Name = c.Name
        End Sub
        Public Overrides Function ToString() As String
            Return Name
        End Function
    End Class
End Class