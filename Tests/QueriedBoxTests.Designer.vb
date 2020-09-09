<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
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
        Dim OtherField1 As ControlLibrary.QueriedBox.OtherField = New ControlLibrary.QueriedBox.OtherField()
        Dim OtherField2 As ControlLibrary.QueriedBox.OtherField = New ControlLibrary.QueriedBox.OtherField()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.QueriedBox1 = New ControlLibrary.QueriedBox()
        Me.QueriedBox2 = New ControlLibrary.QueriedBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(35, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Nome"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(256, 10)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Patrimônio"
        '
        'QueriedBox1
        '
        Me.QueriedBox1.ConnectionString = Nothing
        Me.QueriedBox1.DbProvider = Nothing
        Me.QueriedBox1.DecimalOnly = False
        Me.QueriedBox1.DecimalPlaces = 2
        Me.QueriedBox1.Dependents.Add(Me.QueriedBox2)
        Me.QueriedBox1.DropDownAutoStretchRight = False
        Me.QueriedBox1.FieldHeader = "Patrimonio do Compressor"
        Me.QueriedBox1.JoinField = ""
        Me.QueriedBox1.JoinPKField = ""
        Me.QueriedBox1.JoinTable = ""
        Me.QueriedBox1.Location = New System.Drawing.Point(259, 26)
        Me.QueriedBox1.MainField = "NAME"
        Me.QueriedBox1.MainPKField = "ID"
        Me.QueriedBox1.MainTable = "PERSON"
        Me.QueriedBox1.Name = "QueriedBox1"
        Me.QueriedBox1.Size = New System.Drawing.Size(241, 20)
        Me.QueriedBox1.TabIndex = 0
        '
        'QueriedBox2
        '
        Me.QueriedBox2.ConnectionString = Nothing
        Me.QueriedBox2.DbProvider = Nothing
        Me.QueriedBox2.DecimalOnly = False
        Me.QueriedBox2.DecimalPlaces = 2
        Me.QueriedBox2.Dependents.Add(Me.QueriedBox1)
        Me.QueriedBox2.DropDownAutoStretchRight = False
        Me.QueriedBox2.FieldHeader = "Nome do Compressor"
        Me.QueriedBox2.JoinField = "NAME"
        Me.QueriedBox2.JoinPKField = "ID"
        Me.QueriedBox2.JoinTable = "COMPRESSOR"
        Me.QueriedBox2.Location = New System.Drawing.Point(12, 26)
        Me.QueriedBox2.MainField = "COMPRESSORID"
        Me.QueriedBox2.MainPKField = "ID"
        Me.QueriedBox2.MainTable = "PERSONCOMPRESSOR"
        Me.QueriedBox2.Name = "QueriedBox2"
        OtherField1.FieldHeader = "NSERIE"
        OtherField1.JoinField = Nothing
        OtherField1.JoinPKField = Nothing
        OtherField1.JoinTable = Nothing
        OtherField1.MainField = "SERIALNUMBER"
        OtherField2.FieldHeader = "PESSOA"
        OtherField2.JoinField = "NAME"
        OtherField2.JoinPKField = "ID"
        OtherField2.JoinTable = "PERSON"
        OtherField2.MainField = "PERSONID"
        Me.QueriedBox2.OtherFields.Add(OtherField1)
        Me.QueriedBox2.OtherFields.Add(OtherField2)
        Me.QueriedBox2.Size = New System.Drawing.Size(241, 20)
        Me.QueriedBox2.TabIndex = 0
        '
        'QueriedBoxTests
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(759, 450)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.QueriedBox1)
        Me.Controls.Add(Me.QueriedBox2)
        Me.Name = "QueriedBoxTests"
        Me.Text = "QueriedBoxTexts"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents QueriedBox2 As ControlLibrary.QueriedBox
    Friend WithEvents Label1 As Label
    Friend WithEvents QueriedBox1 As ControlLibrary.QueriedBox
    Friend WithEvents Label2 As Label
End Class
