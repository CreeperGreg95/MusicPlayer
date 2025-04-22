Imports NAudio.Wave
Imports System.Speech.Synthesis
Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Net.Http
Imports Newtonsoft.Json.Linq

Public Class MusicController
    Private waveOut As WaveOutEvent
    Private mediaReader As MediaFoundationReader
    Private currentStream As Stream
    Private karaokeSynth As SpeechSynthesizer
    Private volumeController As AudioVolumeControls
    Private connectionString As String
    Private currentUserID As Integer

    Public Sub New(userID As Integer, volumeControl As ModernTrackbar.ModernTrackbar_TheSecondOff.VolumeTrackbar)
        waveOut = New WaveOutEvent()
        karaokeSynth = New SpeechSynthesizer()
        volumeController = New AudioVolumeControls(waveOut, volumeControl)
        currentUserID = userID

        ' Charger la chaîne de connexion depuis le fichier PHP
        LoadConnectionStringAsync()
    End Sub

    ' Méthode asynchrone pour récupérer la chaîne de connexion depuis un fichier PHP
    Private Async Sub LoadConnectionStringAsync()
        Try
            Dim client As New HttpClient()

            ' Préparer la requête POST avec l'action appropriée
            Dim content As New FormUrlEncodedContent(New Dictionary(Of String, String) From {
                {"action", "getConnection"}
            })

            ' Envoyer la requête POST au fichier PHP
            Dim response As HttpResponseMessage = Await client.PostAsync("https://musicplayer.creepergreg951.eu/musics_datastore/connection/connection.php", content)

            If response.IsSuccessStatusCode Then
                ' Lire la réponse JSON
                Dim json As String = Await response.Content.ReadAsStringAsync()

                ' Analyser le JSON
                Dim config As JObject = JObject.Parse(json)

                ' Construire la chaîne de connexion
                connectionString = String.Format("Server={0};Database={1};User Id={2};Password={3};",
                                                 config("servername").ToString(),
                                                 config("dbname").ToString(),
                                                 config("dbusername").ToString(),
                                                 config("dbpassword").ToString())

                Debug.WriteLine("Chaîne de connexion chargée : " & connectionString)
            Else
                Debug.WriteLine("Erreur lors de la récupération de la configuration de la base de données.")
                MessageBox.Show("Erreur lors de la récupération des informations de connexion.")
            End If
        Catch ex As Exception
            Debug.WriteLine("Erreur lors de la récupération de la configuration : " & ex.Message)
            MessageBox.Show("Erreur lors de la récupération des informations de connexion : " & ex.Message)
        End Try
    End Sub

    Public Function GetWaveOut() As WaveOutEvent
        Return waveOut
    End Function

    Public Function GetMusicLink(songName As String) As String
        Try
            Debug.WriteLine("Recherche du lien de la chanson: " & songName)

            If String.IsNullOrEmpty(connectionString) Then
                MessageBox.Show("La chaîne de connexion n'est pas encore chargée.")
                Return String.Empty
            End If

            Using conn As New MySqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT MusicLink FROM Musics WHERE MusicName = @MusicName"
                Debug.WriteLine("Requête SQL: " & query)
                Dim cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@MusicName", songName)
                Dim link As Object = cmd.ExecuteScalar()
                If link IsNot Nothing Then
                    Debug.WriteLine("Lien trouvé: " & link.ToString())
                    Return link.ToString()
                Else
                    Debug.WriteLine("Aucun lien trouvé pour cette chanson.")
                End If
            End Using
        Catch ex As MySqlException
            Debug.WriteLine("Erreur MySQL: " & ex.Message)
            MessageBox.Show("Erreur de connexion ou de requête MySQL: " & ex.Message)
        Catch ex As Exception
            Debug.WriteLine("Erreur générale: " & ex.Message)
            MessageBox.Show("Erreur générale: " & ex.Message)
        End Try
        Return String.Empty
    End Function

    Public Sub PlaySong(songName As String)
        Dim musicLink As String = GetMusicLink(songName)

        If Not String.IsNullOrEmpty(musicLink) Then
            Try
                Debug.WriteLine("Lecture du flux audio à partir de l'URL: " & musicLink)

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
                Debug.WriteLine("Lecture démarrée")

                ' Mettre à jour les compteurs d'écoutes
                UpdateListenCounters(songName)

            Catch ex As Exception
                Debug.WriteLine("Erreur lors de la lecture de la musique: " & ex.Message)
                MessageBox.Show("Erreur lors de la lecture de la musique: " & ex.Message)
            End Try
        Else
            MessageBox.Show("Aucun lien de musique trouvé.")
        End If
    End Sub

    Private Sub UpdateListenCounters(songName As String)
        Try
            If String.IsNullOrEmpty(connectionString) Then
                MessageBox.Show("La chaîne de connexion n'est pas encore chargée.")
                Return
            End If

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

                ' Récupérer l'ID de la musique
                Dim musicIDQuery As String = "SELECT MusicID FROM Musics WHERE MusicName = @SongName"
                Dim musicIDCmd As New MySqlCommand(musicIDQuery, conn)
                musicIDCmd.Parameters.AddWithValue("@SongName", songName)
                Dim musicID As Object = musicIDCmd.ExecuteScalar()

                ' Vérifier si un MusicID a été trouvé
                If musicID IsNot Nothing Then
                    Debug.WriteLine("ID de la musique trouvée : " & musicID.ToString())

                    ' Vérifier si l'enregistrement existe pour l'utilisateur et la musique
                    Dim checkRecordQuery As String = "SELECT ListenCount FROM UserMusicListenCount WHERE UserID = @UserID AND MusicID = @MusicID"
                    Dim checkRecordCmd As New MySqlCommand(checkRecordQuery, conn)
                    checkRecordCmd.Parameters.AddWithValue("@UserID", currentUserID)
                    checkRecordCmd.Parameters.AddWithValue("@MusicID", musicID)

                    Dim existingCount As Object = checkRecordCmd.ExecuteScalar()

                    If existingCount IsNot Nothing Then
                        ' Si l'enregistrement existe, mettre à jour le compteur
                        Debug.WriteLine("Enregistrement trouvé. Mise à jour du compteur.")
                        Dim updateQuery As String = "UPDATE UserMusicListenCount SET ListenCount = ListenCount + 1 WHERE UserID = @UserID AND MusicID = @MusicID"
                        Dim updateCmd As New MySqlCommand(updateQuery, conn)
                        updateCmd.Parameters.AddWithValue("@UserID", currentUserID)
                        updateCmd.Parameters.AddWithValue("@MusicID", musicID)
                        updateCmd.ExecuteNonQuery()
                    Else
                        ' Si l'enregistrement n'existe pas, l'ajouter avec une valeur de compteur initiale de 1
                        Debug.WriteLine("Enregistrement non trouvé. Insertion du nouveau compteur.")
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
            Debug.WriteLine("Erreur lors de la mise à jour des compteurs: " & ex.Message)
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
