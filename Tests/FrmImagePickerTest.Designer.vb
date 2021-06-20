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
        Dim MenuBarIcons3 As ControlLibrary.ImagePicker.MenuBarIcons = New ControlLibrary.ImagePicker.MenuBarIcons()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmImagePickerTest))
        Dim NavigationBarIcons3 As ControlLibrary.ImagePicker.NavigationBarIcons = New ControlLibrary.ImagePicker.NavigationBarIcons()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ImagePicker1 = New ControlLibrary.ImagePicker()
        Me.ControlSlider1 = New ControlLibrary.ControlSlider()
        Me.DateBox1 = New ControlLibrary.DateBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Label1"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(387, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(200, 100)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GroupBox1"
        '
        'ImagePicker1
        '
        Me.ImagePicker1.AcceptRepeatedFileNames = False
        Me.ImagePicker1.BarsDelay = 300
        Me.ImagePicker1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ImagePicker1.CounterBar.BackColor = System.Drawing.Color.Gainsboro
        Me.ImagePicker1.CounterBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold)
        Me.ImagePicker1.CounterBar.ForeColor = System.Drawing.Color.SteelBlue
        Me.ImagePicker1.CounterBar.Format = "#/#"
        Me.ImagePicker1.CounterBar.Visible = True
        Me.ImagePicker1.ImageSizeMode = System.Windows.Forms.ImageLayout.Zoom
        Me.ImagePicker1.Location = New System.Drawing.Point(12, 12)
        Me.ImagePicker1.MaximumImageNumber = 100
        Me.ImagePicker1.MenuBar.BackColor = System.Drawing.Color.Gainsboro
        MenuBarIcons3.Include = CType(resources.GetObject("MenuBarIcons3.Include"), System.Drawing.Image)
        MenuBarIcons3.Remove = CType(resources.GetObject("MenuBarIcons3.Remove"), System.Drawing.Image)
        MenuBarIcons3.Save = CType(resources.GetObject("MenuBarIcons3.Save"), System.Drawing.Image)
        Me.ImagePicker1.MenuBar.Icons = MenuBarIcons3
        Me.ImagePicker1.MenuBar.MouseDownBackColor = System.Drawing.Color.SteelBlue
        Me.ImagePicker1.MenuBar.MouseOverBackColor = System.Drawing.Color.SkyBlue
        Me.ImagePicker1.MinimumSize = New System.Drawing.Size(120, 120)
        Me.ImagePicker1.Name = "ImagePicker1"
        Me.ImagePicker1.NavigationBar.BackColor = System.Drawing.Color.Gainsboro
        NavigationBarIcons3.First = CType(resources.GetObject("NavigationBarIcons3.First"), System.Drawing.Image)
        NavigationBarIcons3.Last = CType(resources.GetObject("NavigationBarIcons3.Last"), System.Drawing.Image)
        NavigationBarIcons3.Next = CType(resources.GetObject("NavigationBarIcons3.Next"), System.Drawing.Image)
        NavigationBarIcons3.Previous = CType(resources.GetObject("NavigationBarIcons3.Previous"), System.Drawing.Image)
        Me.ImagePicker1.NavigationBar.Icons = NavigationBarIcons3
        Me.ImagePicker1.NavigationBar.MouseDownBackColor = System.Drawing.Color.SteelBlue
        Me.ImagePicker1.NavigationBar.MouseOverBackColor = System.Drawing.Color.SkyBlue
        Me.ImagePicker1.NavigationBar.Visible = True
        Me.ImagePicker1.ShowImageNameToolTip = False
        Me.ImagePicker1.Size = New System.Drawing.Size(352, 248)
        Me.ImagePicker1.TabIndex = 0
        Me.ImagePicker1.Text = "ImagePicker1"
        '
        'ControlSlider1
        '
        Me.ControlSlider1.Child = Me.Label1
        Me.ControlSlider1.Parent = Me.GroupBox1
        '
        'DateBox1
        '
        Me.DateBox1.ButtonImage = CType(resources.GetObject("DateBox1.ButtonImage"), System.Drawing.Image)
        Me.DateBox1.Location = New System.Drawing.Point(421, 217)
        Me.DateBox1.Name = "DateBox1"
        Me.DateBox1.Size = New System.Drawing.Size(100, 20)
        Me.DateBox1.TabIndex = 4
        '
        'FrmImagePickerTest
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(689, 413)
        Me.Controls.Add(Me.DateBox1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.ImagePicker1)
        Me.Name = "FrmImagePickerTest"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ImagePicker1 As ControlLibrary.ImagePicker
    Friend WithEvents ControlSlider1 As ControlLibrary.ControlSlider
    Friend WithEvents Label1 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents DateBox1 As ControlLibrary.DateBox
End Class
