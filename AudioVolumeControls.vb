Imports ModernTrackbar
Imports NAudio.Wave

Public Class AudioVolumeControls
    Private waveOut As WaveOutEvent
    Private volumeControl As ModernTrackbar_TheSecondOff.VolumeTrackbar
    Private tooltip As ToolTip

    Public Sub New(waveOutInstance As WaveOutEvent, volumeControl As ModernTrackbar_TheSecondOff.VolumeTrackbar)
        Me.waveOut = waveOutInstance
        Me.volumeControl = volumeControl
        Me.tooltip = New ToolTip()
        InitializeTooltip()
    End Sub

    Private Sub InitializeTooltip()
        tooltip.SetToolTip(volumeControl, "Volume: 0%")
        AddHandler volumeControl.Scroll, AddressOf VolumeControl_Scroll
    End Sub

    Private Sub VolumeControl_Scroll(sender As Object, e As EventArgs)
        Dim currentVolume As Integer = volumeControl.Value
        Dim percentage As Integer = (currentVolume / volumeControl.Maximum) * 100
        tooltip.SetToolTip(volumeControl, "Volume: " & percentage & "%")
        SetVolume(currentVolume / 100.0F)
    End Sub

    ' Méthode pour ajuster le volume du son
    Public Sub SetVolume(volume As Single)
        If waveOut IsNot Nothing Then
            If volume < 0.0F OrElse volume > 1.0F Then
                Throw New ArgumentOutOfRangeException("volume", "Le volume doit être compris entre 0.0 et 1.0.")
            End If
            waveOut.Volume = volume
        Else
            Throw New InvalidOperationException("Le lecteur audio n'a pas été initialisé.")
        End If
    End Sub

    ' Méthode pour obtenir le volume actuel
    Public Function GetVolume() As Single
        If waveOut IsNot Nothing Then
            Return waveOut.Volume
        Else
            Throw New InvalidOperationException("Le lecteur audio n'a pas été initialisé.")
        End If
    End Function

    ' 🔁 Nouvelle méthode pour mettre à jour le tooltip à l'initialisation
    Public Sub UpdateTooltip()
        Dim currentVolume As Integer = volumeControl.Value
        Dim percentage As Integer = (currentVolume / volumeControl.Maximum) * 100
        tooltip.SetToolTip(volumeControl, "Volume: " & percentage & "%")
        SetVolume(currentVolume / 100.0F)
    End Sub
End Class
