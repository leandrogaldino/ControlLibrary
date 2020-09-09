Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Public Class ImagePicker
    Inherits Control
    Protected Overrides Sub Dispose(disposing As Boolean)
        TmVisibility.Stop()
        TmVisibility.Dispose()
        TtTips.Dispose()
        OfdImage.Dispose()
        SfdImage.Dispose()
        BtnInclude.Dispose()
        BtnRemove.Dispose()
        BtnSave.Dispose()
        LblCounter.Dispose()
        BtnNavFirst.Dispose()
        BtnNavPrevious.Dispose()
        BtnNavNext.Dispose()
        BtnNavLast.Dispose()
        PnNav.Dispose()
        PnCounter.Dispose()
        PnMenu.Dispose()
        PnImage.Dispose()
        MyBase.Dispose(disposing)

    End Sub

    Public Function GetImagesLocation() As String()
        Dim LstLocation As New List(Of String)
        For Each Info In ImagesInfo
            LstLocation.Add(Info.Location)
        Next
        Return LstLocation.ToArray
    End Function
    Public Function GetSelectedImageLocation() As String
        For i = 0 To ImagesInfo.Count - 1
            If ImagesInfo(i).Selected = True Then
                Return ImagesInfo(i).Location
            End If
        Next i
        Return Nothing
    End Function
    Public Function GetSelectedImageIndex() As Integer
        For i = 0 To ImagesInfo.Count - 1
            If ImagesInfo(i).Selected = True Then
                Return i
            End If
        Next i
        Return -1
    End Function
    Public Sub SetSelectedImageIndex(ByVal Index As Integer)
        For i = 0 To ImagesInfo.Count - 1
            If i = Index Then
                ImagesInfo(i).Selected = True
                PnImage.BackgroundImage = GetCopyImage(GetSelectedImageLocation)
                RefreshNavigation()
            Else
                ImagesInfo(i).Selected = False
            End If
        Next i
    End Sub
    Public Class MenuBarIcons
        Public Property Include As Image
        Public Property Remove As Image
        Public Property Save As Image
    End Class
    Public Class MenuBarStyle
        <Browsable(False)>
        Public Property Icons As New MenuBarIcons
        Public Property BackColor As Color
        Public Property MouseDownBackColor As Color
        Public Property MouseOverBackColor As Color

        Public Overrides Function ToString() As String
            Return String.Format("BackColor [{0}], MouseDownBackColor [{1}], MouseOverBackColor [{2}]", BackColor.Name, MouseDownBackColor.Name, MouseOverBackColor.Name)
        End Function
    End Class
    Public Class CounterBarStyle
        Public Property BackColor As Color
        Public Property Font As Font
        Public Property ForeColor As Color
        Public Property Format As String
        Public Property Visible As Boolean

        Public Overrides Function ToString() As String
            Return String.Format("BackColor [{0}], Font [{1}], ForeColor [{2}] Format [{3}] Visible [{4}", BackColor.Name, Font.Name, ForeColor.Name, Format, Visible)
        End Function
    End Class
    Public Class NavigationBarIcons
        Public Property First As Image
        Public Property Previous As Image
        Public Property [Next] As Image
        Public Property Last As Image
    End Class
    Public Class NavigationBarStyle
        Inherits MenuBarStyle
        <Browsable(False)>
        Public Overloads Property Icons As New NavigationBarIcons
        Public Property Visible As Boolean
        Public Overrides Function ToString() As String
            Return String.Format("{0} Visible [{1}]", MyBase.ToString, Visible)
        End Function
    End Class
    Public Class ImageInfo
        Public Property Location As String
        Public Property Selected As Boolean
    End Class
    Public Class ImageInfoCollection
        Inherits Collection(Of ImageInfo)
        Protected Overrides Sub RemoveItem(index As Integer)
            Dim CountIndex As Integer = Items.Count
            Dim RemovingIndex As Integer = index

            'se ela for a ultima imagem da colecao entao...
            If RemovingIndex = CountIndex - 1 Then
                'Se tiver pelo menos duas imagens, a imagem selecionada passa a ser uma antes.
                If Count > 1 Then
                    Items(index - 1).Selected = True
                End If
                'Se for a primeira imagem
            ElseIf RemovingIndex = 0 Then
                'Se tiver pelo menos duas imagens, a imagem selecionada passa a ser uma proxima.
                If Count > 1 Then
                    Items(index + 1).Selected = True
                End If
                'se nao for a ultima nem a primeira
            Else
                If Count > 1 Then
                    Items(index - 1).Selected = True
                End If
            End If



            'se eu estou removendo uma imagem que nao esta selecionada.
            'Else

            'End If
            MyBase.RemoveItem(index)



        End Sub
        'Friend Sub SetSelectedIndex(ByVal Index As Integer)
        '    For i = 0 To Items.Count - 1
        '        If i = Index Then
        '            Items(i).Selected = True
        '        Else
        '            Items(i).Selected = False
        '        End If
        '    Next i
        'End Sub
        'Friend Function GetSelectedImageIndex() As Integer
        '    For i = 0 To Items.Count - 1
        '        If Items(i).Selected = True Then
        '            Return i
        '        End If
        '    Next i
        '    Return -1
        'End Function
        'Friend Function GetSelectedPath() As String
        '    For i = 0 To Items.Count - 1
        '        If Items(i).Selected = True Then
        '            Return Items(i).Location
        '        End If
        '    Next i
        '    Return Nothing
        'End Function
    End Class
    Public Overrides Property MinimumSize As Size
        Get
            Return MyBase.MinimumSize
        End Get
        Set(value As Size)
            If value.Width < 100 Then value.Width = 100
            If value.Height < 100 Then value.Height = 100
            MyBase.MinimumSize = value
        End Set
    End Property
    Public Event ImageIncluded(sender As Object)
    Public Event ImageExported(sender As Object)
    Public Event ImageRemoved(sender As Object)
    Public Property AcceptRepeatedFileNames As Boolean
    Public Property ImagesInfo As New ImageInfoCollection
    Public Property ShowImageNameToolTip As Boolean = False
    Public Property BorderStyle As BorderStyle
        Get
            Return PnImage.BorderStyle
        End Get
        Set(value As BorderStyle)
            PnImage.BorderStyle = value
        End Set
    End Property
    Public Property ImageSizeMode As ImageLayout
        Get
            Return PnImage.BackgroundImageLayout
        End Get
        Set(value As ImageLayout)
            PnImage.BackgroundImageLayout = value
        End Set
    End Property
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    <TypeConverter(GetType(ExpandableObjectConverter))>
    Public Property MenuBar As New MenuBarStyle
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    <TypeConverter(GetType(ExpandableObjectConverter))>
    Public Property CounterBar As New CounterBarStyle
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    <TypeConverter(GetType(ExpandableObjectConverter))>
    Public Property NavigationBar As New NavigationBarStyle

    Private _MaximumImageNumber As Integer = 100
    Public Property MaximumImageNumber As Integer
        Get
            Return _MaximumImageNumber
        End Get
        Set(value As Integer)
            If value < 1 Then value = 1
            _MaximumImageNumber = value
        End Set
    End Property
    Public Property BarsDelay As Integer
        Get
            Return TmVisibility.Interval
        End Get
        Set(value As Integer)
            TmVisibility.Interval = value
        End Set
    End Property
    Public Sub New()
        InitializeComponent()
    End Sub
    Private Sub BtnInclude_Click(sender As Object, e As EventArgs) Handles BtnInclude.Click
        Dim Count As Integer = ImagesInfo.Count - 1
        If OfdImage.ShowDialog = DialogResult.OK Then
            For Each FileName In OfdImage.FileNames
                If AcceptRepeatedFileNames Then

                    If ImagesInfo.Count < MaximumImageNumber Then
                        ImagesInfo.Add(New ImageInfo With {.Location = FileName})
                    End If
                Else
                    If Not ImagesInfo.Any(Function(x) x.Location = FileName) Then
                        If ImagesInfo.Count < MaximumImageNumber Then
                            ImagesInfo.Add(New ImageInfo With {.Location = FileName})
                        End If
                    End If
                End If
            Next FileName
            If Count < ImagesInfo.Count - 1 Then
                If GetSelectedImageIndex() = -1 Then
                    SetSelectedImageIndex(0)
                Else
                    SetSelectedImageIndex(Count + 1)
                End If
            End If
            PnImage.BackgroundImage = GetCopyImage(GetSelectedImageLocation)
            RefreshNavigation()
            'If ImagesInfo.Count > 0 Then
            '    BtnSave.Enabled = True
            'Else
            '    BtnSave.Enabled = False
            'End If
            'If ImagesInfo.Count = MaximumImageNumber Then
            '    BtnInclude.Enabled = False
            'End If
            RaiseEvent ImageIncluded(Me)
        End If
    End Sub
    Private Sub BtnRemove_Click(sender As Object, e As EventArgs) Handles BtnRemove.Click
        For Each Info In ImagesInfo
            If Info.Selected Then
                ImagesInfo.Remove(Info)
                RefreshNavigation()
                If GetSelectedImageIndex() >= 0 Then
                    PnImage.BackgroundImage = GetCopyImage(ImagesInfo(GetSelectedImageIndex).Location)
                Else
                    PnImage.BackgroundImage = Nothing
                End If


                'If ImagesInfo.Count > 0 Then
                '    BtnSave.Enabled = True
                'Else
                '    BtnSave.Enabled = False
                'End If

                'If ImagesInfo.Count < MaximumImageNumber Then
                '    BtnInclude.Enabled = True
                'End If

                RaiseEvent ImageIncluded(Me)
                Exit For
            End If
        Next Info
    End Sub
    Public Sub ExportImage(ByVal Index As Integer, ByVal Destination As String)
        File.Copy(ImagesInfo(Index).Location, Destination)
    End Sub
    Public Sub ExportImage(ByVal ImageInfo As ImageInfo, ByVal Destination As String)
        File.Copy(ImageInfo.Location, Destination)
    End Sub
    Public Sub ExportImages(ByVal Destination As String)
        For Each Info In ImagesInfo
            File.Copy(Info.Location, Destination & "\" & Path.GetFileName(Info.Location))
        Next Info
    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        'If File.Exists(ImagesInfo.GetSelectedPath) Then
        SfdImage.FileName = Path.GetFileName(GetSelectedImageLocation)
        SfdImage.Filter = String.Format("Imagem {0}|*{1}", UCase(Path.GetExtension(GetSelectedImageLocation).Replace(".", "")), LCase(Path.GetExtension(GetSelectedImageLocation)))
        If SfdImage.ShowDialog() = DialogResult.OK Then
            'PnImage.BackgroundImage.Save(SfdImage.FileName)

            ExportImage(GetSelectedImageIndex, SfdImage.FileName)

            'File.Copy(GetSelectedImageLocation, SfdImage.FileName)
            RaiseEvent ImageExported(Me)

        End If
        'End If
    End Sub
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim MenuBarVisible As Boolean
        Dim CounterBarVisible As Boolean
        Dim NavigationBarVisible As Boolean

        'Descomentar essas o if abaixo para que a decoração não aparece no design
        'If DesignMode Then
        MenuBarVisible = PnMenu.Visible
        CounterBarVisible = PnCounter.Visible
        NavigationBarVisible = PnNav.Visible
        PnMenu.Visible = False
        PnCounter.Visible = False
        PnNav.Visible = False
        'End If
        PnMenu.BackColor = MenuBar.BackColor
        BtnInclude.BackgroundImage = MenuBar.Icons.Include
        BtnInclude.FlatAppearance.MouseOverBackColor = MenuBar.MouseOverBackColor
        BtnInclude.FlatAppearance.MouseDownBackColor = MenuBar.MouseDownBackColor
        BtnRemove.BackgroundImage = MenuBar.Icons.Remove
        BtnRemove.FlatAppearance.MouseOverBackColor = MenuBar.MouseOverBackColor
        BtnRemove.FlatAppearance.MouseDownBackColor = MenuBar.MouseDownBackColor
        BtnSave.BackgroundImage = MenuBar.Icons.Save
        BtnSave.FlatAppearance.MouseOverBackColor = MenuBar.MouseOverBackColor
        BtnSave.FlatAppearance.MouseDownBackColor = MenuBar.MouseDownBackColor

        PnCounter.BackColor = CounterBar.BackColor
        LblCounter.Font = CounterBar.Font
        LblCounter.ForeColor = CounterBar.ForeColor
        LblCounter.Text = GetCounterText(CounterBar.Format)

        PnNav.BackColor = NavigationBar.BackColor
        BtnNavFirst.BackgroundImage = NavigationBar.Icons.First
        BtnNavFirst.FlatAppearance.MouseOverBackColor = NavigationBar.MouseOverBackColor
        BtnNavFirst.FlatAppearance.MouseDownBackColor = NavigationBar.MouseDownBackColor
        BtnNavPrevious.BackgroundImage = NavigationBar.Icons.Previous
        BtnNavPrevious.FlatAppearance.MouseOverBackColor = NavigationBar.MouseOverBackColor
        BtnNavPrevious.FlatAppearance.MouseDownBackColor = NavigationBar.MouseDownBackColor
        BtnNavNext.BackgroundImage = NavigationBar.Icons.Next
        BtnNavNext.FlatAppearance.MouseOverBackColor = NavigationBar.MouseOverBackColor
        BtnNavNext.FlatAppearance.MouseDownBackColor = NavigationBar.MouseDownBackColor
        BtnNavLast.BackgroundImage = NavigationBar.Icons.Last
        BtnNavLast.FlatAppearance.MouseOverBackColor = NavigationBar.MouseOverBackColor
        BtnNavLast.FlatAppearance.MouseDownBackColor = NavigationBar.MouseDownBackColor
        PnMenu.Visible = MenuBarVisible
        PnCounter.Visible = CounterBarVisible
        PnNav.Visible = NavigationBarVisible

        MyBase.OnPaint(e)
    End Sub
    Public Sub GoToFirstImage()
        PnImage.BackgroundImage = GetCopyImage(ImagesInfo.First.Location)
        SetSelectedImageIndex(0)
        RefreshNavigation()
    End Sub
    Public Sub GotoPreviousImage()
        PnImage.BackgroundImage = GetCopyImage(ImagesInfo(GetSelectedImageIndex() - 1).Location)
        SetSelectedImageIndex(GetSelectedImageIndex() - 1)
        RefreshNavigation()
    End Sub
    Public Sub GotoNextImage()
        PnImage.BackgroundImage = GetCopyImage(ImagesInfo(GetSelectedImageIndex() + 1).Location)
        SetSelectedImageIndex(GetSelectedImageIndex() + 1)
        RefreshNavigation()
    End Sub
    Public Sub GotoLastImage()
        PnImage.BackgroundImage = GetCopyImage(ImagesInfo.Last.Location)
        SetSelectedImageIndex(ImagesInfo.Count - 1)
        RefreshNavigation()
    End Sub


    Private Sub BtnNavFirst_Click(sender As Object, e As EventArgs) Handles BtnNavFirst.Click
        GoToFirstImage()
    End Sub

    Private Sub BtnNavPrevious_Click(sender As Object, e As EventArgs) Handles BtnNavPrevious.Click
        GotoPreviousImage()
    End Sub

    Private Sub BtnNavNext_Click(sender As Object, e As EventArgs) Handles BtnNavNext.Click
        GotoNextImage()
    End Sub

    Private Sub BtnNavLast_Click(sender As Object, e As EventArgs) Handles BtnNavLast.Click
        GotoLastImage()
    End Sub
    Private Function GetCounterText(ByVal Format As String) As String
        Dim Expression As Regex
        Dim CounterText As String
        Expression = New Regex(Regex.Escape("#"))
        CounterText = Expression.Replace(CounterBar.Format, (GetSelectedImageIndex() + 1).ToString, 1)
        CounterText = Expression.Replace(CounterText, ImagesInfo.Count.ToString, 1)
        Return CounterText
    End Function
    Public Sub RefreshNavigation()
        BtnRemove.Enabled = True
        BtnSave.Enabled = True
        If ImagesInfo.Count > 0 Then
            If ImagesInfo(0).Selected = True And ImagesInfo.Count > 1 Then
                BtnNavFirst.Enabled = False
                BtnNavPrevious.Enabled = False
                BtnNavNext.Enabled = True
                BtnNavLast.Enabled = True
            ElseIf ImagesInfo(0).Selected = True And ImagesInfo.Count = 1 Then
                BtnNavFirst.Enabled = False
                BtnNavPrevious.Enabled = False
                BtnNavNext.Enabled = False
                BtnNavLast.Enabled = False
            ElseIf ImagesInfo(ImagesInfo.Count - 1).Selected = True Then
                BtnNavFirst.Enabled = True
                BtnNavPrevious.Enabled = True
                BtnNavNext.Enabled = False
                BtnNavLast.Enabled = False
            Else
                BtnNavFirst.Enabled = True
                BtnNavPrevious.Enabled = True
                BtnNavNext.Enabled = True
                BtnNavLast.Enabled = True
            End If
            If ShowImageNameToolTip = True Then
                TtTips.SetToolTip(PnImage, Path.GetFileNameWithoutExtension(GetSelectedImageLocation))
            End If
        Else
            BtnNavFirst.Enabled = False
            BtnNavPrevious.Enabled = False
            BtnNavNext.Enabled = False
            BtnNavLast.Enabled = False
            BtnRemove.Enabled = False
            BtnSave.Enabled = False
            TtTips.SetToolTip(PnImage, Nothing)
        End If
        If ImagesInfo.Count = MaximumImageNumber Then
            BtnInclude.Enabled = False
        Else
            BtnInclude.Enabled = True
        End If
        If CounterBar.Format.Count(Function(x) x = "#") = 2 Then
            LblCounter.Text = GetCounterText(CounterBar.Format)
        Else
            LblCounter.Text = "Formato inválido"
        End If
    End Sub


    Private _MinWidth As Integer = 120
    Private _MinHeight As Integer = 120

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        If Width < _MinWidth Then Width = _MinWidth
        If Height < _MinHeight Then Height = _MinHeight
        MyBase.OnResize(e)
    End Sub


    Private Sub InitializeComponent()
        MinimumSize = New Size(_MinWidth, _MinHeight)
        Size = New Size(_MinWidth, _MinHeight)

        TmVisibility = New Timer
        TmVisibility.Interval = 300
        TmVisibility.Enabled = True
        TmVisibility.Start()

        PnImage = New Panel
        PnImage.Dock = DockStyle.Fill
        PnImage.BorderStyle = BorderStyle.FixedSingle
        PnImage.BackgroundImageLayout = ImageLayout.Zoom
        PnImage.Size = Size
        PnImage.Padding = New Padding(10)
        PnImage.BackgroundImageLayout = ImageLayout.Zoom

        MenuBar.Icons.Include = My.Resources.Include
        MenuBar.Icons.Remove = My.Resources.Remove
        MenuBar.Icons.Save = My.Resources.Save
        MenuBar.BackColor = Color.Gainsboro
        MenuBar.MouseOverBackColor = Color.SkyBlue
        MenuBar.MouseDownBackColor = Color.SteelBlue

        PnMenu = New Panel
        PnMenu.Parent = PnImage
        PnMenu.Size = New Size(104, 21)
        PnMenu.Location = New Point(7, 5)
        PnMenu.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
        PnMenu.Visible = False
        PnMenu.BackColor = MenuBar.BackColor
        'PnMenu.BackColor = Color.Red

        BtnInclude = New Button
        BtnInclude.Parent = PnMenu
        BtnInclude.Size = New Size(27, 21)
        BtnInclude.Location = New Point(11.5, 0)
        BtnInclude.Anchor = AnchorStyles.None
        BtnInclude.BackColor = Color.Transparent
        BtnInclude.FlatStyle = FlatStyle.Flat
        BtnInclude.FlatAppearance.BorderSize = 0
        BtnInclude.FlatAppearance.MouseOverBackColor = MenuBar.MouseOverBackColor
        BtnInclude.FlatAppearance.MouseDownBackColor = MenuBar.MouseDownBackColor
        BtnInclude.BackgroundImage = MenuBar.Icons.Include
        BtnInclude.BackgroundImageLayout = ImageLayout.Zoom
        'BtnInclude.BackColor = Color.Pink

        BtnRemove = New Button
        BtnRemove.Parent = PnMenu
        BtnRemove.Size = New Size(27, 21)
        BtnRemove.Location = New Point(39, 0)
        BtnRemove.Anchor = AnchorStyles.None
        BtnRemove.BackColor = Color.Transparent
        BtnRemove.FlatStyle = FlatStyle.Flat
        BtnRemove.FlatAppearance.BorderSize = 0
        BtnRemove.FlatAppearance.MouseOverBackColor = MenuBar.MouseOverBackColor
        BtnRemove.FlatAppearance.MouseDownBackColor = MenuBar.MouseDownBackColor
        BtnRemove.BackgroundImage = MenuBar.Icons.Remove
        BtnRemove.BackgroundImageLayout = ImageLayout.Zoom
        BtnRemove.Enabled = False
        'BtnRemove.BackColor = Color.Green

        BtnSave = New Button
        BtnSave.Parent = PnMenu
        BtnSave.Size = New Size(27, 21)
        BtnSave.Location = New Point(66, 0)
        BtnSave.Anchor = AnchorStyles.None
        BtnSave.BackColor = Color.Transparent
        BtnSave.FlatStyle = FlatStyle.Flat
        BtnSave.FlatAppearance.BorderSize = 0
        BtnSave.FlatAppearance.MouseOverBackColor = MenuBar.MouseOverBackColor
        BtnSave.FlatAppearance.MouseDownBackColor = MenuBar.MouseDownBackColor
        BtnSave.BackgroundImage = MenuBar.Icons.Save
        BtnSave.BackgroundImageLayout = ImageLayout.Zoom
        BtnSave.Enabled = False
        'BtnSave.BackColor = Color.Yellow

        CounterBar.Visible = True
        CounterBar.BackColor = Color.Gainsboro
        CounterBar.Format = "#/#"
        CounterBar.Font = New Font(Font.Name, 12, FontStyle.Bold)
        CounterBar.ForeColor = Color.SteelBlue

        PnCounter = New Panel
        PnCounter.Parent = PnImage
        PnCounter.Size = New Size(104, 21)
        PnCounter.Location = New Point(7, 28)
        PnCounter.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
        PnCounter.Visible = False
        PnCounter.BackColor = CounterBar.BackColor
        'PnCounter.BackColor = Color.Red


        LblCounter = New Label
        LblCounter.Parent = PnCounter
        LblCounter.AutoSize = False
        LblCounter.Size = New Size(83, 15)
        LblCounter.Location = New Point(3, 3)
        LblCounter.Anchor = AnchorStyles.None
        LblCounter.Text = GetCounterText(CounterBar.Format)
        LblCounter.TextAlign = ContentAlignment.MiddleCenter
        LblCounter.Dock = DockStyle.Fill
        LblCounter.Font = CounterBar.Font
        LblCounter.ForeColor = CounterBar.ForeColor

        NavigationBar.Icons.First = My.Resources.NavFirst
        NavigationBar.Icons.Previous = My.Resources.NavPrevious
        NavigationBar.Icons.Next = My.Resources.NavNext
        NavigationBar.Icons.Last = My.Resources.NavLast
        NavigationBar.BackColor = Color.Gainsboro
        NavigationBar.MouseOverBackColor = Color.SkyBlue
        NavigationBar.MouseDownBackColor = Color.SteelBlue
        NavigationBar.Visible = True

        PnNav = New Panel
        PnNav.Parent = PnImage
        PnNav.Size = New Size(104, 21)
        PnNav.Location = New Point(7, 90)
        PnNav.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        PnNav.Visible = False
        PnNav.BackColor = NavigationBar.BackColor
        'PnNav.BackColor = Color.Red

        BtnNavFirst = New Button
        BtnNavFirst.Parent = PnNav
        BtnNavFirst.Size = New Size(26, 21)
        BtnNavFirst.Location = New Point(0, 0)
        BtnNavFirst.Anchor = AnchorStyles.None
        BtnNavFirst.FlatStyle = FlatStyle.Flat
        BtnNavFirst.FlatAppearance.BorderSize = 0
        BtnNavFirst.FlatAppearance.MouseOverBackColor = NavigationBar.MouseOverBackColor
        BtnNavFirst.FlatAppearance.MouseDownBackColor = NavigationBar.MouseDownBackColor
        BtnNavFirst.BackgroundImage = NavigationBar.Icons.First
        BtnNavFirst.BackgroundImageLayout = ImageLayout.Zoom
        BtnNavFirst.BackColor = Color.Transparent
        BtnNavFirst.Enabled = False

        BtnNavPrevious = New Button
        BtnNavPrevious.Parent = PnNav
        BtnNavPrevious.Size = New Size(26, 21)
        BtnNavPrevious.Location = New Point(26, 0)
        BtnNavPrevious.Anchor = AnchorStyles.None
        BtnNavPrevious.FlatStyle = FlatStyle.Flat
        BtnNavPrevious.FlatAppearance.BorderSize = 0
        BtnNavPrevious.FlatAppearance.MouseOverBackColor = NavigationBar.MouseOverBackColor
        BtnNavPrevious.FlatAppearance.MouseDownBackColor = NavigationBar.MouseDownBackColor
        BtnNavPrevious.BackgroundImage = NavigationBar.Icons.Previous
        BtnNavPrevious.BackgroundImageLayout = ImageLayout.Zoom
        BtnNavPrevious.BackColor = Color.Transparent
        BtnNavPrevious.Enabled = False

        BtnNavNext = New Button
        BtnNavNext.Parent = PnNav
        BtnNavNext.Size = New Size(26, 21)
        BtnNavNext.Location = New Point(52, 0)
        BtnNavNext.Anchor = AnchorStyles.None
        BtnNavNext.FlatStyle = FlatStyle.Flat
        BtnNavNext.FlatAppearance.BorderSize = 0
        BtnNavNext.FlatAppearance.MouseOverBackColor = NavigationBar.MouseOverBackColor
        BtnNavNext.FlatAppearance.MouseDownBackColor = NavigationBar.MouseDownBackColor
        BtnNavNext.BackgroundImage = NavigationBar.Icons.Next
        BtnNavNext.BackgroundImageLayout = ImageLayout.Zoom
        BtnNavNext.BackColor = Color.Transparent
        BtnNavNext.Enabled = False

        BtnNavLast = New Button
        BtnNavLast.Parent = PnNav
        BtnNavLast.Size = New Size(26, 21)
        BtnNavLast.Location = New Point(78, 0)
        BtnNavLast.Anchor = AnchorStyles.None
        BtnNavLast.FlatStyle = FlatStyle.Flat
        BtnNavLast.FlatAppearance.BorderSize = 0
        BtnNavLast.FlatAppearance.MouseOverBackColor = NavigationBar.MouseOverBackColor
        BtnNavLast.FlatAppearance.MouseDownBackColor = NavigationBar.MouseDownBackColor
        BtnNavLast.BackgroundImage = NavigationBar.Icons.Last
        BtnNavLast.BackgroundImageLayout = ImageLayout.Zoom
        BtnNavLast.BackColor = Color.Transparent
        BtnNavLast.Enabled = False

        PnImage.Controls.AddRange({PnMenu, PnCounter, PnNav})


        OfdImage = New OpenFileDialog
        OfdImage.Filter = "Imagens (BMP/JPG/PNG)|*.bmp;*.jpg;*.png"
        OfdImage.Title = "Escolha uma imagem"
        OfdImage.Multiselect = True

        SfdImage = New SaveFileDialog
        SfdImage.Title = "Exportar imagem"

        SfdImage = New SaveFileDialog
        SfdImage.Title = "Salvar imagem"

        Controls.Add(PnImage)

        TtTips = New ToolTip
        TtTips.SetToolTip(BtnInclude, "Incluir imagem")
        TtTips.SetToolTip(BtnRemove, "Remover  imagem")
        TtTips.SetToolTip(BtnSave, "Salvar imagem")
        TtTips.SetToolTip(BtnNavFirst, "Primeira")
        TtTips.SetToolTip(BtnNavPrevious, "Voltar")
        TtTips.SetToolTip(BtnNavNext, "Avançar")
        TtTips.SetToolTip(BtnNavLast, "Última")



    End Sub

    'Protected Overrides Sub OnHandleDestroyed(e As EventArgs)
    '    TmVisibility.Stop()
    '    MyBase.OnHandleDestroyed(e)
    'End Sub
    Private Sub PnImage_MouseEnter() Handles PnImage.MouseEnter
        TmVisibility.Stop()
        TmVisibility.Start()
    End Sub
    Private Sub PnImage_MouseLeave() Handles PnImage.MouseLeave
        TmVisibility.Stop()
        TmVisibility.Start()
    End Sub


    Private Sub TmVisibility_Tick(sender As Object, e As EventArgs) Handles TmVisibility.Tick
        'If Not DesignMode Then
        If MouseIsOutOfControl() Then
            PnMenu.Visible = False
            PnCounter.Visible = False
            PnNav.Visible = False
        Else
            PnMenu.Visible = True
            PnCounter.Visible = If(CounterBar.Visible, True, False)
            PnNav.Visible = If(NavigationBar.Visible, True, False)
        End If
        'Else
        'PnMenu.Visible = False
        'PnCounter.Visible = False
        'PnNav.Visible = False
        'End If
    End Sub
    Private Function MouseIsOutOfControl() As Boolean
        If PnImage.ClientRectangle.Contains(PnImage.PointToClient(Cursor.Position)) Then Return False
        If PnMenu.ClientRectangle.Contains(PnMenu.PointToClient(Cursor.Position)) Then Return False
        If BtnInclude.ClientRectangle.Contains(BtnInclude.PointToClient(Cursor.Position)) Then Return False
        If BtnSave.ClientRectangle.Contains(BtnSave.PointToClient(Cursor.Position)) Then Return False
        If BtnRemove.ClientRectangle.Contains(BtnRemove.PointToClient(Cursor.Position)) Then Return False
        If PnCounter.ClientRectangle.Contains(PnCounter.PointToClient(Cursor.Position)) Then Return False
        If LblCounter.ClientRectangle.Contains(LblCounter.PointToClient(Cursor.Position)) Then Return False
        If PnNav.ClientRectangle.Contains(PnNav.PointToClient(Cursor.Position)) Then Return False
        If BtnNavFirst.ClientRectangle.Contains(BtnNavFirst.PointToClient(Cursor.Position)) Then Return False
        If BtnNavPrevious.ClientRectangle.Contains(BtnNavPrevious.PointToClient(Cursor.Position)) Then Return False
        If BtnNavNext.ClientRectangle.Contains(BtnNavNext.PointToClient(Cursor.Position)) Then Return False
        If BtnNavLast.ClientRectangle.Contains(BtnNavLast.PointToClient(Cursor.Position)) Then Return False
        Return True
    End Function
    Private Function GetCopyImage(ByVal path As String) As Image
        Using Img As Image = Image.FromFile(path)
            Dim bitmap As Bitmap = New Bitmap(Img)
            Return bitmap
        End Using
    End Function




    Friend WithEvents PnNav As Panel
    Friend WithEvents PnImage As Panel
    Friend WithEvents PnMenu As Panel
    Friend WithEvents PnCounter As Panel
    Friend WithEvents OfdImage As OpenFileDialog
    Friend WithEvents SfdImage As SaveFileDialog
    Friend WithEvents LblCounter As Label
    Friend WithEvents BtnInclude As Button
    Friend WithEvents BtnSave As Button
    Friend WithEvents BtnRemove As Button
    Friend WithEvents TmVisibility As Timer
    Friend WithEvents BtnNavFirst As Button
    Friend WithEvents BtnNavPrevious As Button
    Friend WithEvents BtnNavNext As Button
    Friend WithEvents BtnNavLast As Button
    Friend WithEvents TtTips As ToolTip
End Class
