Public Class MusicControls
    Private musicController As MusicController
    Private btnPlayPause As Button
    Private btnPrev As Button
    Private btnNext As Button
    Private isPlaying As Boolean = False

    Public Sub New(musicController As MusicController, playPauseButton As Button, prevButton As Button, nextButton As Button)
        Me.musicController = musicController
        Me.btnPlayPause = playPauseButton
        Me.btnPrev = prevButton
        Me.btnNext = nextButton
        InitializeButtons()
    End Sub

    Private Sub InitializeButtons()
        AddHandler btnPlayPause.Click, AddressOf PlayPauseButton_Click
        AddHandler btnPrev.Click, AddressOf PrevButton_Click
        AddHandler btnNext.Click, AddressOf NextButton_Click
        UpdatePlayPauseButton()
    End Sub

    Private Sub PlayPauseButton_Click(sender As Object, e As EventArgs)
        If isPlaying Then
            musicController.TogglePlayPause()
            isPlaying = False
        Else
            musicController.TogglePlayPause()
            isPlaying = True
        End If
        UpdatePlayPauseButton()
    End Sub

    Private Sub UpdatePlayPauseButton()
        If isPlaying Then
            btnPlayPause.Text = "▶️"
        Else
            btnPlayPause.Text = "||"
        End If
    End Sub

    Private Sub PrevButton_Click(sender As Object, e As EventArgs)
        ' Coming soon :)
    End Sub

    Private Sub NextButton_Click(sender As Object, e As EventArgs)
        ' coming soon :D
    End Sub
End Class
