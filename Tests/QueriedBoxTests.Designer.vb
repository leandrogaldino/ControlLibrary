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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TxtEvaluationDate = New System.Windows.Forms.MaskedTextBox()
        Me.BtnEvaluationDate = New System.Windows.Forms.Button()
        Me.CcoEvaluation = New ControlLibrary.ControlContainer()
        Me.DateBox1 = New Tests.DateBox()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 70)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TxtEvaluationDate
        '
        Me.TxtEvaluationDate.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals
        Me.TxtEvaluationDate.Font = New System.Drawing.Font("Century Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtEvaluationDate.Location = New System.Drawing.Point(12, 31)
        Me.TxtEvaluationDate.Mask = "00/00/0000"
        Me.TxtEvaluationDate.Name = "TxtEvaluationDate"
        Me.TxtEvaluationDate.Size = New System.Drawing.Size(100, 23)
        Me.TxtEvaluationDate.TabIndex = 6
        Me.TxtEvaluationDate.ValidatingType = GetType(Date)
        '
        'BtnEvaluationDate
        '
        Me.BtnEvaluationDate.FlatAppearance.BorderColor = System.Drawing.Color.DimGray
        Me.BtnEvaluationDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnEvaluationDate.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnEvaluationDate.Location = New System.Drawing.Point(118, 31)
        Me.BtnEvaluationDate.Name = "BtnEvaluationDate"
        Me.BtnEvaluationDate.Size = New System.Drawing.Size(28, 23)
        Me.BtnEvaluationDate.TabIndex = 7
        Me.BtnEvaluationDate.TabStop = False
        Me.BtnEvaluationDate.UseVisualStyleBackColor = True
        '
        'CcoEvaluation
        '
        Me.CcoEvaluation.DropDownBorderColor = System.Drawing.SystemColors.HotTrack
        Me.CcoEvaluation.DropDownControl = Nothing
        Me.CcoEvaluation.DropDownEnabled = True
        Me.CcoEvaluation.HostControl = Me.BtnEvaluationDate
        '
        'DateBox1
        '
        Me.DateBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 30.0!)
        Me.DateBox1.Location = New System.Drawing.Point(118, 236)
        Me.DateBox1.Name = "DateBox1"
        Me.DateBox1.Size = New System.Drawing.Size(100, 53)
        Me.DateBox1.TabIndex = 8
        '
        'QueriedBoxTests
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1329, 751)
        Me.Controls.Add(Me.DateBox1)
        Me.Controls.Add(Me.BtnEvaluationDate)
        Me.Controls.Add(Me.TxtEvaluationDate)
        Me.Controls.Add(Me.Button1)
        Me.Name = "QueriedBoxTests"
        Me.Text = "QueriedBoxTexts"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents TxtEvaluationDate As MaskedTextBox
    Friend WithEvents CcoEvaluation As ControlLibrary.ControlContainer
    Friend WithEvents BtnEvaluationDate As Button
    Friend WithEvents DateBox1 As DateBox
End Class
