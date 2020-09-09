<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmCodeCompilerTest
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
        Me.TxtCode = New System.Windows.Forms.TextBox()
        Me.BtnCompile = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtReturn = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BtnClean = New System.Windows.Forms.Button()
        Me.TxtPar1 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TxtPar2 = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'TxtCode
        '
        Me.TxtCode.Location = New System.Drawing.Point(13, 38)
        Me.TxtCode.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TxtCode.Multiline = True
        Me.TxtCode.Name = "TxtCode"
        Me.TxtCode.ReadOnly = True
        Me.TxtCode.Size = New System.Drawing.Size(567, 194)
        Me.TxtCode.TabIndex = 4
        '
        'BtnCompile
        '
        Me.BtnCompile.Location = New System.Drawing.Point(333, 296)
        Me.BtnCompile.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.BtnCompile.Name = "BtnCompile"
        Me.BtnCompile.Size = New System.Drawing.Size(112, 35)
        Me.BtnCompile.TabIndex = 2
        Me.BtnCompile.Text = "Compile"
        Me.BtnCompile.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 9)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 20)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Code"
        '
        'TxtReturn
        '
        Me.TxtReturn.Location = New System.Drawing.Point(225, 260)
        Me.TxtReturn.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TxtReturn.Name = "TxtReturn"
        Me.TxtReturn.ReadOnly = True
        Me.TxtReturn.Size = New System.Drawing.Size(355, 26)
        Me.TxtReturn.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(221, 235)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 20)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Return"
        '
        'BtnClean
        '
        Me.BtnClean.Location = New System.Drawing.Point(468, 296)
        Me.BtnClean.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.BtnClean.Name = "BtnClean"
        Me.BtnClean.Size = New System.Drawing.Size(112, 35)
        Me.BtnClean.TabIndex = 3
        Me.BtnClean.Text = "Clean"
        Me.BtnClean.UseVisualStyleBackColor = True
        '
        'TxtPar1
        '
        Me.TxtPar1.Location = New System.Drawing.Point(12, 260)
        Me.TxtPar1.Name = "TxtPar1"
        Me.TxtPar1.Size = New System.Drawing.Size(100, 26)
        Me.TxtPar1.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 237)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 20)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Number 1"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(115, 237)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(79, 20)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Number 2"
        '
        'TxtPar2
        '
        Me.TxtPar2.Location = New System.Drawing.Point(118, 260)
        Me.TxtPar2.Name = "TxtPar2"
        Me.TxtPar2.Size = New System.Drawing.Size(100, 26)
        Me.TxtPar2.TabIndex = 1
        '
        'FrmCodeCompiler
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(594, 343)
        Me.Controls.Add(Me.TxtPar2)
        Me.Controls.Add(Me.TxtPar1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.TxtReturn)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.BtnClean)
        Me.Controls.Add(Me.BtnCompile)
        Me.Controls.Add(Me.TxtCode)
        Me.Font = New System.Drawing.Font("Century Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmCodeCompiler"
        Me.ShowIcon = False
        Me.Text = "CodeCompiler"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TxtCode As TextBox
    Friend WithEvents BtnCompile As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents TxtReturn As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents BtnClean As Button
    Friend WithEvents TxtPar1 As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents TxtPar2 As TextBox
End Class
