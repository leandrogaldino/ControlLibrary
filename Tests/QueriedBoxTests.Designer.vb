﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class QueriedBoxTests
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
        Me.DecimalBox1 = New ControlLibrary.DecimalBox()
        Me.SuspendLayout()
        '
        'DecimalBox1
        '
        Me.DecimalBox1.DecimalOnly = True
        Me.DecimalBox1.DecimalPlaces = 2
        Me.DecimalBox1.Location = New System.Drawing.Point(198, 133)
        Me.DecimalBox1.Name = "DecimalBox1"
        Me.DecimalBox1.Size = New System.Drawing.Size(100, 23)
        Me.DecimalBox1.TabIndex = 0
        Me.DecimalBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'QueriedBoxTests
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(692, 361)
        Me.Controls.Add(Me.DecimalBox1)
        Me.Font = New System.Drawing.Font("Century Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "QueriedBoxTests"
        Me.Text = "QueriedBoxTexts"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents DecimalBox1 As ControlLibrary.DecimalBox
End Class
