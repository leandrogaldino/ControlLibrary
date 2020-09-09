<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmImagePickerTest
    Inherits System.Windows.Forms.Form

    'Descartar substituições de formulário para limpar a lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Exigido pelo Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'OBSERVAÇÃO: o procedimento a seguir é exigido pelo Windows Form Designer
    'Pode ser modificado usando o Windows Form Designer.  
    'Não o modifique usando o editor de códigos.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.Color.Red
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox1.Location = New System.Drawing.Point(0, 0)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(689, 310)
        Me.TextBox1.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 310)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(689, 103)
        Me.Panel1.TabIndex = 1
        '
        'FrmImagePickerTest
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(689, 413)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "FrmImagePickerTest"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Panel1 As Panel
End Class
