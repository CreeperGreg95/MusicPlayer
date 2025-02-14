Imports System.Windows.Forms

Public Class AudioPlayer
    ' Déclare les contrôles de l'interface
    Private WithEvents musicTimer As New Timer()
    Private currentTimeLabel As Label
    Private remainingTimeLabel As Label
    Private audioTrackBar As TrackBar

    Private totalDuration As Integer ' Durée totale de la musique en secondes
    Private currentTime As Integer ' Temps actuel de la musique en secondes
    Private musicPlayerIsPlaying As Boolean = False ' Etat de lecture de la musique

    ' Constructeur de la classe, où tu passes les labels et la TrackBar
    Public Sub New(currentTimeLbl As Label, remainingTimeLbl As Label, audioTrackBarCtrl As TrackBar)
        currentTimeLabel = currentTimeLbl
        remainingTimeLabel = remainingTimeLbl
        audioTrackBar = audioTrackBarCtrl

        ' Initialisation du Timer
        musicTimer.Interval = 1000 ' Intervalle de mise à jour toutes les secondes
        musicTimer.Start()
    End Sub

    ' Mise à jour de la progression de la musique
    Public Sub UpdateMusicProgress(currentPos As Integer, totalPos As Integer)
        currentTime = currentPos

        ' Mise à jour de la trackbar
        audioTrackBar.Maximum = totalDuration
        audioTrackBar.Value = currentTime

        ' Mise à jour des labels avec les temps formatés
        currentTimeLabel.Text = FormatTime(currentTime)
        remainingTimeLabel.Text = FormatTime(totalDuration - currentTime)
    End Sub

    ' Formatage du temps au format MM:SS
    Private Function FormatTime(timeInSeconds As Integer) As String
        Dim minutes As Integer = timeInSeconds \ 60
        Dim seconds As Integer = timeInSeconds Mod 60
        Return String.Format("{0:D2}:{1:D2}", minutes, seconds)
    End Function

    ' Fonction de gestion du Timer
    Private Sub musicTimer_Tick(sender As Object, e As EventArgs) Handles musicTimer.Tick
        If musicPlayerIsPlaying Then
            currentTime += 1 ' Incrémenter le temps actuel
            UpdateMusicProgress(currentTime, totalDuration)
        End If
    End Sub

    ' Fonction pour démarrer ou arrêter la lecture
    Public Sub TogglePlayPause()
        musicPlayerIsPlaying = Not musicPlayerIsPlaying
    End Sub
End Class
