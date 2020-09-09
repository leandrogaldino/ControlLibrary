Imports System.Drawing
Imports System.Net.Mail
Imports System.Text
Imports System.Windows.Forms
Public Class CMessageBox
    Public Enum CMessageBoxType
        [Error]
        Information
        Question
        Done
        Tip
        Warning
    End Enum
    Public Enum CMessageBoxButtons
        AbortRetryIgnore = 1
        OK = 2
        OKCancel = 3
        RetryCancel = 4
        YesNo = 5
        YesNoCancel = 6
    End Enum
    Public Shared Property SmtpMailServer As SmtpClient
    Private Shared _Title As String
    Private Shared _Message As String
    Private Shared _BoxType As CMessageBoxType
    Private Shared _Buttons As CMessageBoxButtons
    Private Shared _Exception As Exception
    Private Shared Function GetImage(ByVal Type As CMessageBoxType) As Image
        Select Case Type
            Case Is = CMessageBoxType.Done
                Return CMessageBoxStyle.DoneImage
            Case Is = CMessageBoxType.Error
                Return CMessageBoxStyle.ErrorImage
            Case Is = CMessageBoxType.Information
                Return CMessageBoxStyle.InformationImage
            Case Is = CMessageBoxType.Question
                Return CMessageBoxStyle.QuestionImage
            Case Is = CMessageBoxType.Tip
                Return CMessageBoxStyle.TipImage
            Case Else
                Return CMessageBoxStyle.WarningImage
        End Select
    End Function
    Private Shared Function GetMailMessage() As String
        Dim MailMessage As New StringBuilder
        MailMessage.AppendLine("===================RELATÓRIO DE ERRO===================")
        MailMessage.AppendLine("")
        MailMessage.AppendLine("DATA:")
        MailMessage.AppendLine(Today.ToLongDateString)
        MailMessage.AppendLine("")
        If LblTitle IsNot Nothing AndAlso LblTitle.Text <> Nothing Then
            MailMessage.AppendLine("TÍTULO:")
            MailMessage.AppendLine(LblTitle.Text)
            MailMessage.AppendLine("")
        End If
        MailMessage.AppendLine("MENSAGEM:")
        MailMessage.AppendLine(LblMessage.Text)
        MailMessage.AppendLine("")
        If TxtException IsNot Nothing AndAlso TxtException.Text <> Nothing Then
            MailMessage.AppendLine("ERRO:")
            MailMessage.AppendLine(TxtException.Text)
            MailMessage.AppendLine("")
        End If
        If TxtInnerException IsNot Nothing AndAlso TxtInnerException.Text <> Nothing Then
            MailMessage.AppendLine("ERRO INTERNO:")
            MailMessage.AppendLine(TxtInnerException.Text)
            MailMessage.AppendLine("")
        End If
        MailMessage.AppendLine("PASSOS:")
        MailMessage.AppendLine(TxtSteps.Text)
        MailMessage.AppendLine("")
        MailMessage.AppendLine("E-MAIL:")
        MailMessage.AppendLine(TxtEmail.Text)
        MailMessage.AppendLine("")
        MailMessage.AppendLine("TELEFONE:")
        MailMessage.AppendLine(TxtPhone.Text)
        Return MailMessage.ToString
    End Function
    Public Shared Function Show(ByVal Message As String) As DialogResult
        _Title = Nothing
        _Message = Message
        _BoxType = Nothing
        _Buttons = Nothing
        _Exception = Nothing
        Return ShowMessage()
    End Function
    Public Shared Function Show(ByVal Message As String, ByVal BoxType As CMessageBoxType) As DialogResult
        _Title = Nothing
        _Message = Message
        _BoxType = BoxType
        _Buttons = Nothing
        _Exception = Nothing
        Return ShowMessage()
    End Function
    Public Shared Function Show(ByVal Message As String, ByVal BoxType As CMessageBoxType, ByVal BoxButtons As CMessageBoxButtons) As DialogResult
        _Title = Nothing
        _Message = Message
        _BoxType = BoxType
        _Buttons = BoxButtons
        _Exception = Nothing
        Return ShowMessage()
    End Function
    Public Shared Function Show(ByVal Message As String, ByVal BoxType As CMessageBoxType, ByVal BoxButtons As CMessageBoxButtons, ByVal ex As Exception) As DialogResult
        _Title = Nothing
        _Message = Message
        _BoxType = BoxType
        _Buttons = BoxButtons
        _Exception = ex
        Return ShowMessage()
    End Function
    Public Shared Function Show(ByVal Title As String, ByVal Message As String) As DialogResult
        _Title = Title
        _Message = Message
        _BoxType = Nothing
        _Buttons = Nothing
        _Exception = Nothing
        Return ShowMessage()
    End Function
    Public Shared Function Show(ByVal Title As String, ByVal Message As String, ByVal BoxType As CMessageBoxType) As DialogResult
        _Title = Title
        _Message = Message
        _BoxType = BoxType
        _Buttons = Nothing
        _Exception = Nothing
        Return ShowMessage()
    End Function
    Public Shared Function Show(ByVal Title As String, ByVal Message As String, ByVal BoxType As CMessageBoxType, ByVal BoxButtons As CMessageBoxButtons) As DialogResult
        _Title = Title
        _Message = Message
        _BoxType = BoxType
        _Buttons = BoxButtons
        _Exception = Nothing
        Return ShowMessage()
    End Function
    Public Shared Function Show(ByVal Title As String, ByVal Message As String, ByVal BoxType As CMessageBoxType, ByVal BoxButtons As CMessageBoxButtons, ByVal ex As Exception) As DialogResult
        _Title = Title
        _Message = Message
        _BoxType = BoxType
        _Buttons = BoxButtons
        _Exception = ex
        Return ShowMessage()
    End Function
    Private Shared Sub InitializeForm()
        Form = New Form With {
            .FormBorderStyle = FormBorderStyle.FixedSingle,
            .BackColor = CMessageBoxStyle.MessageBackColor,
            .Font = CMessageBoxStyle.MessageFont,
            .MaximizeBox = False,
            .MinimizeBox = False,
            .MinimumSize = New Size(350, 243),
            .Size = New Size(350, 243),
            .Padding = New Padding(0, 12, 0, 0),
            .ShowIcon = False,
            .ShowInTaskbar = False,
            .KeyPreview = True
        }
    End Sub
    Private Shared Sub InitializeImage()
        PnImage = New Panel
        PnImage.Size = New Size(310, 42)
        PnImage.Location = New Point(15, 1)
        PnImage.BackgroundImage = GetImage(_BoxType)
        PnImage.BackgroundImageLayout = ImageLayout.Zoom
        Form.Controls.Add(PnImage)
    End Sub
    Private Shared Sub InitializeLabels()
        LblMessage = New Label
        LblMessage.Text = _Message
        LblMessage.Font = CMessageBoxStyle.MessageFont
        LblMessage.ForeColor = CMessageBoxStyle.MessageForeColor
        LblMessage.AutoSize = True
        LblMessage.MaximumSize = New Size(CMessageBoxStyle.MaximumWidth, 0)
        LblMessage.BackColor = Color.Transparent
        If _Title <> Nothing Then
            LblTitle = New Label
            LblTitle.Text = _Title
            LblTitle.Font = CMessageBoxStyle.TitleFont
            LblTitle.ForeColor = CMessageBoxStyle.TitleForeColor
            LblTitle.AutoSize = True
            LblTitle.MaximumSize = New Size(CMessageBoxStyle.MaximumWidth, 0)
            LblTitle.Location = New Point(15, 55)
            LblTitle.BackColor = Color.Transparent
            LblMessage.Location = New Point(12, 97)
            Form.Controls.Add(LblTitle)
        Else
            Form.MinimumSize = New Size(350, 230)
            Form.Size = New Size(350, 230)
            LblMessage.Location = New Point(15, 75)
        End If
        Form.Controls.Add(LblMessage)
    End Sub
    Private Shared Sub InitializeButtons()
        Dim LeftButtonLocation As Point = New Point(85, 9)
        Dim MiddleButtonLocation As Point = New Point(166, 9)
        Dim RightButtonLocation As Point = New Point(247, 9)

        PnButtons = New Panel
        PnButtons.BackColor = CMessageBoxStyle.PanelButtonsBackColor
        PnButtons.Dock = DockStyle.Bottom
        PnButtons.Height = 50
        Form.Controls.Add(PnButtons)

        BtnAbort = New Button
        BtnAbort.DialogResult = DialogResult.Abort
        BtnAbort.Size = New Size(75, 30)
        BtnAbort.TabIndex = 2
        BtnAbort.Text = "Abortar"
        BtnAbort.UseVisualStyleBackColor = True
        BtnAbort.Parent = PnButtons
        BtnAbort.Anchor = AnchorStyles.Right Or AnchorStyles.Bottom

        BtnRetry = New Button
        BtnRetry.DialogResult = DialogResult.Retry
        BtnRetry.Size = New Size(75, 30)
        BtnRetry.TabIndex = 0
        BtnRetry.Text = "Repetir"
        BtnRetry.UseVisualStyleBackColor = True
        BtnRetry.Parent = PnButtons
        BtnRetry.Anchor = AnchorStyles.Right Or AnchorStyles.Bottom

        BtnIgnore = New Button
        BtnIgnore.DialogResult = DialogResult.Ignore
        BtnIgnore.Size = New Size(75, 30)
        BtnIgnore.Text = "Ignorar"
        BtnIgnore.TabIndex = 1
        BtnIgnore.UseVisualStyleBackColor = True
        BtnIgnore.Parent = PnButtons
        BtnIgnore.Anchor = AnchorStyles.Right Or AnchorStyles.Bottom

        BtnOK = New Button
        BtnOK.DialogResult = DialogResult.OK
        BtnOK.Size = New Size(75, 30)
        BtnOK.Text = "OK"
        BtnOK.TabIndex = 0
        BtnOK.UseVisualStyleBackColor = True
        BtnOK.Anchor = AnchorStyles.Right Or AnchorStyles.Bottom

        BtnCancel = New Button
        BtnCancel.Anchor = AnchorStyles.Right Or AnchorStyles.Bottom
        BtnCancel.DialogResult = DialogResult.Cancel
        BtnCancel.Size = New Size(75, 30)
        BtnCancel.Text = "Cancelar"
        BtnCancel.TabIndex = 1
        BtnCancel.UseVisualStyleBackColor = True

        BtnYes = New Button
        BtnYes.Anchor = AnchorStyles.Right Or AnchorStyles.Bottom
        BtnYes.DialogResult = DialogResult.Yes
        BtnYes.Size = New Size(75, 30)
        BtnYes.TabIndex = 0
        BtnYes.Text = "Sim"
        BtnYes.UseVisualStyleBackColor = True

        BtnNo = New Button
        BtnNo.Anchor = AnchorStyles.Right Or AnchorStyles.Bottom
        BtnNo.DialogResult = DialogResult.No
        BtnNo.Size = New Size(75, 30)
        BtnNo.Text = "Não"
        BtnNo.TabIndex = 1
        BtnNo.UseVisualStyleBackColor = True

        Select Case _Buttons
            Case Is = CMessageBoxButtons.AbortRetryIgnore
                BtnAbort.Location = LeftButtonLocation
                BtnRetry.Location = MiddleButtonLocation
                BtnIgnore.Location = RightButtonLocation
                PnButtons.Controls.AddRange({BtnAbort, BtnRetry, BtnIgnore})
                BtnOK.Dispose()
                BtnCancel.Dispose()
                BtnYes.Dispose()
                BtnNo.Dispose()
            Case Is = CMessageBoxButtons.OK
                BtnOK.Location = RightButtonLocation
                PnButtons.Controls.Add(BtnOK)
                BtnAbort.Dispose()
                BtnRetry.Dispose()
                BtnIgnore.Dispose()
                BtnCancel.Dispose()
                BtnYes.Dispose()
                BtnNo.Dispose()
            Case Is = CMessageBoxButtons.OKCancel
                BtnOK.Location = MiddleButtonLocation
                BtnCancel.Location = RightButtonLocation
                PnButtons.Controls.AddRange({BtnOK, BtnCancel})
                BtnAbort.Dispose()
                BtnRetry.Dispose()
                BtnIgnore.Dispose()
                BtnYes.Dispose()
                BtnNo.Dispose()
            Case Is = CMessageBoxButtons.RetryCancel
                BtnRetry.Location = MiddleButtonLocation
                BtnCancel.Location = RightButtonLocation
                PnButtons.Controls.AddRange({BtnRetry, BtnCancel})
                BtnAbort.Dispose()
                BtnIgnore.Dispose()
                BtnOK.Dispose()
                BtnYes.Dispose()
                BtnNo.Dispose()
            Case Is = CMessageBoxButtons.YesNo
                BtnYes.Location = MiddleButtonLocation
                BtnNo.Location = RightButtonLocation
                PnButtons.Controls.AddRange({BtnYes, BtnNo})
                BtnAbort.Dispose()
                BtnRetry.Dispose()
                BtnIgnore.Dispose()
                BtnOK.Dispose()
                BtnCancel.Dispose()

            Case Is = CMessageBoxButtons.YesNoCancel
                BtnYes.Location = LeftButtonLocation
                BtnNo.Location = MiddleButtonLocation
                BtnCancel.Location = RightButtonLocation
                PnButtons.Controls.AddRange({BtnYes, BtnNo, BtnCancel})
                BtnAbort.Dispose()
                BtnRetry.Dispose()
                BtnIgnore.Dispose()
                BtnOK.Dispose()
            Case Else
                BtnOK.Location = RightButtonLocation
                PnButtons.Controls.Add(BtnOK)
                BtnAbort.Dispose()
                BtnRetry.Dispose()
                BtnIgnore.Dispose()
                BtnCancel.Dispose()
                BtnYes.Dispose()
                BtnNo.Dispose()
        End Select

    End Sub
    Private Shared Sub InitializeExceptionHandler()
        If _BoxType = CMessageBoxType.Error Then
            LblMessage.Cursor = Cursors.Hand
            TtTips = New ToolTip
            TtTips.SetToolTip(LblMessage, "Clique aqui para reportar esse erro.")
            UcExceptionDialog = New UserControl With {
                .BackColor = Color.White,
                .Font = New Font("Microsoft Sans Serif", 9.75, FontStyle.Regular),
                .Margin = New Padding(4, 4, 4, 4),
                .Size = New Size(304, 289)
            }
            TcException = New TabControl With {
                .Dock = DockStyle.Fill
            }



            If _Exception IsNot Nothing Then
                TpException = New TabPage With {
                    .Text = "Detalhes do Erro",
                    .UseVisualStyleBackColor = True
                }
                TxtException = New TextBox With {
                    .BorderStyle = BorderStyle.None,
                    .Dock = DockStyle.Fill,
                    .Multiline = True,
                    .ReadOnly = True,
                    .Text = _Exception.Message
                }
                TpException.Controls.Add(TxtException)
                TcException.TabPages.Add(TpException)


                If _Exception.InnerException IsNot Nothing AndAlso _Exception.InnerException.Message <> Nothing Then
                    TpInnerException = New TabPage With {
                        .Text = "Erro Interno",
                        .UseVisualStyleBackColor = True
                    }
                    TxtInnerException = New TextBox With {
                        .BorderStyle = BorderStyle.None,
                        .Dock = DockStyle.Fill,
                        .Multiline = True,
                        .ReadOnly = True,
                        .Text = _Exception.InnerException.Message
                    }
                    TpInnerException.Controls.Add(TxtInnerException)
                    TcException.TabPages.Add(TpInnerException)
                End If
            End If





            TpSupport = New TabPage With {
                    .Text = "Suporte",
                    .UseVisualStyleBackColor = True
                }
            LblExPhone = New Label With {
                .AutoSize = True,
                .Location = New Point(3, 14),
                .Text = "Telefone"
            }
            TxtPhone = New TextBox With {
                .Location = New Point(6, 33),
                .Size = New Size(105, 22),
                .TabIndex = 0
            }
            LblExEmail = New Label With {
                .AutoSize = True,
                .Location = New Point(114, 14),
                .Text = "E-Mail"
            }
            TxtEmail = New TextBox With {
                .Location = New Point(117, 33),
                .Size = New Size(172, 22),
                .TabIndex = 1
            }
            LblExSteps = New Label With {
                .AutoSize = True,
                .Location = New Point(3, 58),
                .Text = "Passos que levaram ao erro"
            }
            TxtSteps = New TextBox With {
                .Location = New Point(6, 77),
                .Multiline = True,
                .Size = New Size(283, 142),
                .TabIndex = 2
            }
            PnExButtons = New Panel With {
                .BackColor = Color.WhiteSmoke,
                .Dock = DockStyle.Bottom,
                .Height = 38,
                .Margin = New Padding(4, 4, 4, 4)
            }
            If CMessageBoxStyle.ShowSaveErrorButton Then
                BtnExSave = New Button With {
                    .Enabled = False,
                    .DialogResult = DialogResult.Abort,
                    .Location = New Point(137, 4),
                    .Size = New Size(75, 30),
                    .Text = "Salvar",
                    .UseVisualStyleBackColor = True,
                    .TabIndex = 3
                }
                TtTips.SetToolTip(BtnExSave, "Salvar em disco.")
            End If
            If CMessageBoxStyle.ShowEmailErrorButton Then
                BtnExEmail = New Button With {
                    .Enabled = False,
                    .DialogResult = DialogResult.Abort,
                    .Location = New Point(218, 4),
                    .Size = New Size(75, 30),
                    .Text = "E-Mail",
                    .UseVisualStyleBackColor = True,
                    .TabIndex = 4
                }
                TtTips.SetToolTip(BtnExEmail, "Enviar e-mail para o suporte.")
            Else
                If BtnExSave IsNot Nothing Then BtnExSave.Location = New Point(218, 4)
            End If
            PnExButtons.Controls.AddRange({BtnExSave, BtnExEmail})
            TpSupport.Controls.AddRange({LblExPhone, LblExEmail, TxtPhone, TxtEmail, LblExSteps, TxtSteps})
            TcException.TabPages.Add(TpSupport)
            UcExceptionDialog.Controls.AddRange({PnExButtons, TcException})
            TcException.BringToFront()
            CcContainer = New ControlContainer With {
                .DropDownControl = UcExceptionDialog,
                .HostControl = LblMessage
            }



        End If
    End Sub
    Private Shared Function ShowMessage() As DialogResult
        InitializeForm()
        InitializeImage()
        InitializeLabels()
        InitializeButtons()
        InitializeExceptionHandler()
        Return Form.ShowDialog
    End Function
    Private Shared Sub KeyDown(sender As Object, e As KeyEventArgs) Handles Form.KeyDown
        If e.KeyCode = Keys.Escape Then
            Form.Dispose()
        End If
    End Sub
    Private Shared Sub TxtStepEmailPhone_TextChanged(sender As Object, e As EventArgs) Handles TxtSteps.TextChanged, TxtEmail.TextChanged, TxtPhone.TextChanged
        If TxtSteps.Text = Nothing Then
            BtnExSave.Enabled = False
            BtnExEmail.Enabled = False
        Else
            If TxtEmail.Text = Nothing And TxtPhone.Text = Nothing Then
                BtnExSave.Enabled = False
                BtnExEmail.Enabled = False
            Else
                BtnExSave.Enabled = True
                BtnExEmail.Enabled = True
            End If
        End If
    End Sub
    Private Shared Sub UcBtnEmail_Click(sender As Object, e As EventArgs) Handles BtnExEmail.Click
        Dim Mail As New MailMessage
        Dim Credential As Net.NetworkCredential
        If SmtpMailServer IsNot Nothing Then
            Try
                CcContainer.DropDownControl.Cursor = Cursors.WaitCursor
                Credential = SmtpMailServer.Credentials
                Mail.Subject = "CMessageBoxError"
                Mail.From = New MailAddress(Credential.UserName)
                Mail.To.Add(Credential.UserName)
                Mail.Body = GetMailMessage()
                SmtpMailServer.Send(Mail)
                CcContainer.DropDownControl.Cursor = Cursors.Default
            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                CcContainer.DropDownControl.Cursor = Cursors.Default
            End Try
        Else
            MsgBox("O servidor SMTP não foi definido.")
        End If
    End Sub
    Private Shared Sub UcBtnSave_Click(sender As Object, e As EventArgs) Handles BtnExSave.Click
        SfdSave = New SaveFileDialog With {
            .Filter = "Arquivos txt (*.txt)|*.txt|Todos os arquivos (*.*)|*.*",
            .RestoreDirectory = True,
            .Title = "Salvar Erro",
            .FileName = "Erro"
            }
        If SfdSave.ShowDialog() = DialogResult.OK Then
            Using Writer As New IO.StreamWriter(SfdSave.FileName)
                Writer.Write(GetMailMessage)
            End Using
        End If
    End Sub
    Private Shared Sub LblMessage_LblTitle_SizeChanged(sender As Object, e As EventArgs) Handles LblMessage.SizeChanged, LblTitle.SizeChanged
        If LblTitle IsNot Nothing Then
            If LblMessage.Text.Length > LblTitle.Text.Length Then
                Form.Width = LblMessage.Width + 40
            Else
                Form.Width = LblTitle.Width + 40
            End If
        Else
            Form.Width = LblMessage.Width + 40
        End If
        Form.Height = LblMessage.Height + 190 + If(LblTitle IsNot Nothing, LblTitle.Height, 0)
    End Sub
    Private Shared Sub LblTitle_SizeChanged(sender As Object, e As EventArgs) Handles LblTitle.SizeChanged
        If LblTitle IsNot Nothing Then LblMessage.Top = LblTitle.Top + LblTitle.Height + 15
    End Sub
    Public Class CMessageBoxStyle
        Public Shared Property MessageBackColor As Color = Color.White
        Public Shared Property PanelButtonsBackColor As Color = Color.WhiteSmoke
        Public Shared Property MessageFont As Font = New Font("Microsoft Sans Serif", 8.25, FontStyle.Regular)
        Public Shared Property TitleFont As Font = New Font("Microsoft Sans Serif", 8.25, FontStyle.Bold)
        Public Shared Property MessageForeColor As Color = SystemColors.ControlText
        Public Shared Property TitleForeColor As Color = SystemColors.ControlText
        Public Shared Property ErrorImage As Image = My.Resources._Error
        Public Shared Property InformationImage As Image = My.Resources.Information
        Public Shared Property QuestionImage As Image = My.Resources.Question
        Public Shared Property DoneImage As Image = My.Resources.Done
        Public Shared Property TipImage As Image = My.Resources.Tip
        Public Shared Property WarningImage As Image = My.Resources.Warning
        Public Shared Property ShowEmailErrorButton As Boolean = True
        Public Shared Property ShowSaveErrorButton As Boolean = True
        Public Shared Property MaximumWidth As Integer = 350
        Public Shared Sub Reset()
            MessageBackColor = Color.White
            PanelButtonsBackColor = Color.WhiteSmoke
            MessageFont = New Font("Microsoft Sans Serif", 8.25, FontStyle.Regular)
            TitleFont = New Font("Microsoft Sans Serif", 8.25, FontStyle.Regular)
            MessageForeColor = SystemColors.ControlText
            TitleForeColor = SystemColors.ControlText
            ErrorImage = My.Resources._Error
            InformationImage = My.Resources.Information
            QuestionImage = My.Resources.Question
            DoneImage = My.Resources.Done
            TipImage = My.Resources.Tip
            WarningImage = My.Resources.Warning
            ShowEmailErrorButton = True
            ShowSaveErrorButton = True
            MaximumWidth = 400
        End Sub
    End Class
    Friend Shared WithEvents SfdSave As SaveFileDialog
    Friend Shared WithEvents TtTips As ToolTip
    Friend Shared WithEvents TxtInnerException As TextBox
    Friend Shared WithEvents TxtException As TextBox
    Friend Shared WithEvents BtnExSave As Button
    Friend Shared WithEvents BtnExEmail As Button
    Friend Shared WithEvents PnExButtons As Panel
    Friend Shared WithEvents TpSupport As TabPage
    Friend Shared WithEvents TpException As TabPage
    Friend Shared WithEvents TpInnerException As TabPage
    Friend Shared WithEvents LblExPhone As Label
    Friend Shared WithEvents TxtPhone As TextBox
    Friend Shared WithEvents LblExEmail As Label
    Friend Shared WithEvents TxtEmail As TextBox
    Friend Shared WithEvents LblExSteps As Label
    Friend Shared WithEvents TxtSteps As TextBox
    Friend Shared WithEvents TcException As TabControl
    Friend Shared WithEvents UcExceptionDialog As UserControl
    Friend Shared WithEvents CcContainer As ControlContainer
    Friend Shared WithEvents Form As Form
    Friend Shared WithEvents PnImage As Panel
    Friend Shared WithEvents LblTitle As Label
    Friend Shared WithEvents LblMessage As Label
    Friend Shared WithEvents PnButtons As Panel
    Friend Shared WithEvents BtnAbort As Button
    Friend Shared WithEvents BtnRetry As Button
    Friend Shared WithEvents BtnIgnore As Button
    Friend Shared WithEvents BtnOK As Button
    Friend Shared WithEvents BtnCancel As Button
    Friend Shared WithEvents BtnYes As Button
    Friend Shared WithEvents BtnNo As Button
End Class