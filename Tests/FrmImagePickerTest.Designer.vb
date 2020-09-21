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
        Dim MenuBarIcons1 As ControlLibrary.ImagePicker.MenuBarIcons = New ControlLibrary.ImagePicker.MenuBarIcons()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmImagePickerTest))
        Dim NavigationBarIcons1 As ControlLibrary.ImagePicker.NavigationBarIcons = New ControlLibrary.ImagePicker.NavigationBarIcons()
        Me.ImagePicker1 = New ControlLibrary.ImagePicker()
        Me.SuspendLayout()
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
        MenuBarIcons1.Include = CType(resources.GetObject("MenuBarIcons1.Include"), System.Drawing.Image)
        MenuBarIcons1.Remove = CType(resources.GetObject("MenuBarIcons1.Remove"), System.Drawing.Image)
        MenuBarIcons1.Save = CType(resources.GetObject("MenuBarIcons1.Save"), System.Drawing.Image)
        Me.ImagePicker1.MenuBar.Icons = MenuBarIcons1
        Me.ImagePicker1.MenuBar.MouseDownBackColor = System.Drawing.Color.SteelBlue
        Me.ImagePicker1.MenuBar.MouseOverBackColor = System.Drawing.Color.SkyBlue
        Me.ImagePicker1.MinimumSize = New System.Drawing.Size(120, 120)
        Me.ImagePicker1.Name = "ImagePicker1"
        Me.ImagePicker1.NavigationBar.BackColor = System.Drawing.Color.Gainsboro
        NavigationBarIcons1.First = CType(resources.GetObject("NavigationBarIcons1.First"), System.Drawing.Image)
        NavigationBarIcons1.Last = CType(resources.GetObject("NavigationBarIcons1.Last"), System.Drawing.Image)
        NavigationBarIcons1.Next = CType(resources.GetObject("NavigationBarIcons1.Next"), System.Drawing.Image)
        NavigationBarIcons1.Previous = CType(resources.GetObject("NavigationBarIcons1.Previous"), System.Drawing.Image)
        Me.ImagePicker1.NavigationBar.Icons = NavigationBarIcons1
        Me.ImagePicker1.NavigationBar.MouseDownBackColor = System.Drawing.Color.SteelBlue
        Me.ImagePicker1.NavigationBar.MouseOverBackColor = System.Drawing.Color.SkyBlue
        Me.ImagePicker1.NavigationBar.Visible = True
        Me.ImagePicker1.ShowImageNameToolTip = False
        Me.ImagePicker1.Size = New System.Drawing.Size(352, 248)
        Me.ImagePicker1.TabIndex = 0
        Me.ImagePicker1.Text = "ImagePicker1"
        '
        'FrmImagePickerTest
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(689, 413)
        Me.Controls.Add(Me.ImagePicker1)
        Me.Name = "FrmImagePickerTest"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ImagePicker1 As ControlLibrary.ImagePicker
End Class
