Imports NAudio.Wave
Imports System.Speech.Synthesis
Imports MySql.Data.MySqlClient
Imports System.IO

Public Class MusicController
    Private waveOut As WaveOutEvent
    Private mediaReader As MediaFoundationReader
    Private currentStream As Stream
    Private karaokeSynth As SpeechSynthesizer
    Private volumeController As AudioVolumeControls
    Private currentUserID As Integer
    Private connectionString As String = "Server=srv1049.hstgr.io;Database=u842356047_musicplayerdb;User Id=u842356047_gregcreeper95;Password=Minecraft0711@@@!!!;"

    Public Sub New(userID As Integer)
        waveOut = New WaveOutEvent()
        karaokeSynth = New SpeechSynthesizer()
        volumeController = New AudioVolumeControls(waveOut)
        currentUserID = userID
    End Sub

    Public Function GetMusicLink(songName As String) As String
        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT MusicLink FROM Musics WHERE MusicName = @MusicName"
                Dim cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@MusicName", songName)
                Dim link As Object = cmd.ExecuteScalar()
                If link IsNot Nothing Then
                    Return link.ToString()
                End If
            End Using
        Catch ex As MySqlException
            MessageBox.Show("Erreur de connexion ou de requête MySQL: " & ex.Message)
        Catch ex As Exception
            MessageBox.Show("Erreur générale: " & ex.Message)
        End Try
        Return String.Empty
    End Function

    Public Sub PlaySong(songName As String)
        Dim musicLink As String = GetMusicLink(songName)

        If Not String.IsNullOrEmpty(musicLink) Then
            Try
                ' Arrêter le lecteur précédent si nécessaire
                If waveOut IsNot Nothing AndAlso waveOut.PlaybackState = PlaybackState.Playing Then
                    waveOut.Stop()
                End If

                ' Créer un lecteur audio pour le streaming
                mediaReader = New MediaFoundationReader(musicLink)

                ' Connecter le lecteur audio à l'output
                waveOut.Init(mediaReader)

                ' Démarrer la lecture
                waveOut.Play()

                ' Mettre à jour les compteurs d'écoutes
                UpdateListenCounters(songName)

            Catch ex As Exception
                MessageBox.Show("Erreur lors de la lecture de la musique: " & ex.Message)
            End Try
        Else
            MessageBox.Show("Aucun lien de musique trouvé.")
        End If
    End Sub

    Private Sub UpdateListenCounters(songName As String)
        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()

                ' Vérifier si la table UserMusicListenCount existe, sinon la créer
                Dim checkTableQuery As String = "
            CREATE TABLE IF NOT EXISTS UserMusicListenCount (
                UserID INT NOT NULL,
                MusicID INT NOT NULL,
                ListenCount INT DEFAULT 0,
                PRIMARY KEY (UserID, MusicID),
                FOREIGN KEY (UserID) REFERENCES Users(UserID),
                FOREIGN KEY (MusicID) REFERENCES Musics(MusicID)
            );"
                Dim checkTableCmd As New MySqlCommand(checkTableQuery, conn)
                checkTableCmd.ExecuteNonQuery()

                ' Vérifier si le UserID existe dans la table Users
                Dim checkUserQuery As String = "SELECT COUNT(*) FROM Users WHERE UserID = @UserID"
                Dim checkUserCmd As New MySqlCommand(checkUserQuery, conn)
                checkUserCmd.Parameters.AddWithValue("@UserID", currentUserID)
                Dim userExists As Integer = Convert.ToInt32(checkUserCmd.ExecuteScalar())

                If userExists = 0 Then
                    MessageBox.Show("UserID inexistant dans la table Users.")
                    Return
                End If

                ' Récupérer l'ID de la musique
                Dim musicIDQuery As String = "SELECT MusicID FROM Musics WHERE MusicName = @SongName"
                Dim musicIDCmd As New MySqlCommand(musicIDQuery, conn)
                musicIDCmd.Parameters.AddWithValue("@SongName", songName)
                Dim musicID As Object = musicIDCmd.ExecuteScalar()

                ' Vérifier si un MusicID a été trouvé
                If musicID IsNot Nothing Then
                    ' Vérifier si l'enregistrement existe pour l'utilisateur et la musique
                    Dim checkRecordQuery As String = "SELECT ListenCount FROM UserMusicListenCount WHERE UserID = @UserID AND MusicID = @MusicID"
                    Dim checkRecordCmd As New MySqlCommand(checkRecordQuery, conn)
                    checkRecordCmd.Parameters.AddWithValue("@UserID", currentUserID)
                    checkRecordCmd.Parameters.AddWithValue("@MusicID", musicID)

                    Dim existingCount As Object = checkRecordCmd.ExecuteScalar()

                    If existingCount IsNot Nothing Then
                        ' Si l'enregistrement existe, mettre à jour le compteur
                        Dim updateQuery As String = "UPDATE UserMusicListenCount SET ListenCount = ListenCount + 1 WHERE UserID = @UserID AND MusicID = @MusicID"
                        Dim updateCmd As New MySqlCommand(updateQuery, conn)
                        updateCmd.Parameters.AddWithValue("@UserID", currentUserID)
                        updateCmd.Parameters.AddWithValue("@MusicID", musicID)
                        updateCmd.ExecuteNonQuery()
                    Else
                        ' Si l'enregistrement n'existe pas, l'ajouter avec une valeur de compteur initiale de 1
                        Dim insertQuery As String = "INSERT INTO UserMusicListenCount (UserID, MusicID, ListenCount) VALUES (@UserID, @MusicID, 1)"
                        Dim insertCmd As New MySqlCommand(insertQuery, conn)
                        insertCmd.Parameters.AddWithValue("@UserID", currentUserID)
                        insertCmd.Parameters.AddWithValue("@MusicID", musicID)
                        insertCmd.ExecuteNonQuery()
                    End If

                    ' Mettre à jour le compteur global des écoutes dans la table Musics
                    Dim updateGlobalCountQuery As String = "UPDATE Musics SET ListenedCounter = ListenedCounter + 1 WHERE MusicID = @MusicID"
                    Dim updateGlobalCountCmd As New MySqlCommand(updateGlobalCountQuery, conn)
                    updateGlobalCountCmd.Parameters.AddWithValue("@MusicID", musicID)
                    updateGlobalCountCmd.ExecuteNonQuery()
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur lors de la mise à jour des compteurs: " & ex.Message)
        End Try
    End Sub

    Public Sub TogglePlayPause()
        If waveOut.PlaybackState = PlaybackState.Playing Then
            waveOut.Pause()
        ElseIf waveOut.PlaybackState = PlaybackState.Paused Then
            waveOut.Play()
        End If
    End Sub

    Public Sub SetVolume(normalizedVolume As Single)
        volumeController.SetVolume(normalizedVolume)
    End Sub
End Class
