Imports NAudio.Wave

Public Class AudioVolumeControls
    Private waveOut As WaveOutEvent

    ' Constructeur qui prend une instance existante de WaveOutEvent
    Public Sub New(waveOutInstance As WaveOutEvent)
        waveOut = waveOutInstance
    End Sub

    ' Méthode pour ajuster le volume du son
    ' La valeur du volume doit être comprise entre 0.0 et 1.0
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
End Class
