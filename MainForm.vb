Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Net.Http
Imports NAudio.Wave
Imports System.Speech.Synthesis
Imports System.Windows.Forms
Imports System.Diagnostics
Imports Newtonsoft.Json.Linq
Imports System.Net

Public Class MainForm
    Inherits Form
    Private waveOut As WaveOutEvent
    Private mediaReader As MediaFoundationReader
    Private currentStream As Stream
    Private karaokeSynth As SpeechSynthesizer

    Private isPlaying As Boolean = False

    Private WithEvents currentTimeLabel As Label
    Private WithEvents totalTimeLabel As Label
    Private WithEvents remainingTimeLabel As Label
    Private WithEvents audioTrackBar As TrackBar
    Private audioPlayer As AudioPlayer

    Private currentUserID As Integer ' ID de l'utilisateur récupéré du fichier JSON
    Private offlineMode As Boolean = False
    Private connectionString As String = "Server=srv1049.hstgr.io;Database=u842356047_musicplayerdb;User Id=u842356047_gregcreeper95;Password=Minecraft0711@@@!!!;"

    Public Sub New()
        Debug.WriteLine("Initialisation de MainForm...")
        InitializeComponent()
        Me.Text = "Music Player"
        Me.StartPosition = FormStartPosition.CenterScreen

        ' Configuration du FlowLayoutPanel présent dans la Form
        FlowLayoutMusicPanel.AutoScroll = True
        FlowLayoutMusicPanel.WrapContents = True
        FlowLayoutMusicPanel.FlowDirection = FlowDirection.LeftToRight

        ' Initialisation des composants audio
        waveOut = New WaveOutEvent()
        karaokeSynth = New SpeechSynthesizer()

        ' Récupérer l'ID utilisateur à partir du fichier JSON
        currentUserID = GetUserIDFromJson()

        ' Charger les playlists
        Debug.WriteLine("Chargement des playlists...")
        LoadPlaylists()
    End Sub

    Public Sub UpdateMusicData(currentPos As Integer, totalPos As Integer)
        audioPlayer.UpdateMusicProgress(currentPos, totalPos)
    End Sub

    ' Fonction pour changer la position de la musique avec la TrackBar
    Private Sub audioTrackBar_Scroll(sender As Object, e As EventArgs) Handles audioTrackBar.Scroll
        ' Met à jour la position de la musique selon la valeur de la TrackBar
        audioPlayer.UpdateMusicProgress(audioTrackBar.Value, audioTrackBar.Maximum)
    End Sub

    Private Sub LoadPlaylists()
        ' Fonction pour charger les playlists, ici seulement un exemple
        Debug.WriteLine("Chargement des playlists...")
    End Sub

    Private Function GetMusicLink(songName As String) As String
        Try
            Debug.WriteLine("Recherche du lien de la chanson: " & songName)
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

    ' Fonction pour créer dynamiquement un panneau pour chaque musique
    Private Function CreateMusicPanel(imageUrl As String, songName As String, artist As String, album As String, totalListens As Integer, personalListens As Integer) As Panel
        ' Créer le panneau principal
        Dim musicPanel As New Panel()
        musicPanel.Size = New Size(200, 250)
        musicPanel.BorderStyle = BorderStyle.FixedSingle
        musicPanel.Tag = songName
        AddHandler musicPanel.Click, AddressOf MusicItem_Click ' Ajoute l'événement Click au panneau

        ' Ajouter l'image
        Dim musicPicture As New PictureBox()
        musicPicture.Size = New Size(180, 120)
        musicPicture.Location = New Point(10, 10)
        musicPicture.SizeMode = PictureBoxSizeMode.StretchImage
        If Not String.IsNullOrEmpty(imageUrl) Then
            Try
                ' Si l'URL est un lien HTTP, on la charge via WebClient
                If imageUrl.StartsWith("http") Then
                    ' Utiliser WebClient pour télécharger l'image depuis l'URL
                    Dim webClient As New WebClient()
                    Dim imageBytes As Byte() = webClient.DownloadData(imageUrl)
                    Using ms As New MemoryStream(imageBytes)
                        musicPicture.Image = Image.FromStream(ms)
                    End Using
                Else
                    ' Sinon, chargez depuis un fichier local
                    musicPicture.Image = Image.FromFile(imageUrl)
                End If
            Catch ex As Exception
                ' Si l'image ne peut pas être chargée, afficher une image par défaut
                musicPicture.BackColor = Color.Gray ' Placeholder si l'image n'existe pas
                Debug.WriteLine("Erreur lors du chargement de l'image: " & ex.Message)
            End Try
        Else
            ' Si l'URL est vide ou invalide, afficher une couleur de fond par défaut
            musicPicture.BackColor = Color.Gray
        End If
        ' Définir le même Tag pour l'image pour qu'elle soit associée à la chanson
        musicPicture.Tag = songName
        AddHandler musicPicture.Click, AddressOf MusicItem_Click ' Ajouter l'événement Click pour l'image
        musicPanel.Controls.Add(musicPicture)

        ' Ajouter le nom de la musique
        Dim songLabel As New Label()
        songLabel.Text = songName
        songLabel.Font = New Font("Arial", 10, FontStyle.Bold)
        songLabel.AutoSize = False
        songLabel.TextAlign = ContentAlignment.MiddleCenter
        songLabel.Size = New Size(180, 20)
        songLabel.Location = New Point(10, 140)
        songLabel.Tag = songName
        AddHandler songLabel.Click, AddressOf MusicItem_Click ' Ajouter l'événement Click pour le label
        musicPanel.Controls.Add(songLabel)

        ' Ajouter l'artiste
        Dim artistLabel As New Label()
        artistLabel.Text = artist
        artistLabel.Font = New Font("Arial", 8)
        artistLabel.AutoSize = False
        artistLabel.TextAlign = ContentAlignment.MiddleCenter
        artistLabel.Size = New Size(180, 20)
        artistLabel.Location = New Point(10, 170)
        musicPanel.Controls.Add(artistLabel)

        ' Ajouter le nombre d'écoutes totales
        Dim totalListensLabel As New Label()
        totalListensLabel.Text = "Écoutes totales : " & totalListens
        totalListensLabel.Font = New Font("Arial", 8)
        totalListensLabel.AutoSize = False
        totalListensLabel.TextAlign = ContentAlignment.MiddleCenter
        totalListensLabel.Size = New Size(180, 20)
        totalListensLabel.Location = New Point(10, 200)
        musicPanel.Controls.Add(totalListensLabel)

        ' Ajouter les écoutes personnelles
        Dim personalListensLabel As New Label()
        personalListensLabel.Text = "Vos écoutes : " & personalListens
        personalListensLabel.Font = New Font("Arial", 8)
        personalListensLabel.AutoSize = False
        personalListensLabel.TextAlign = ContentAlignment.MiddleCenter
        personalListensLabel.Size = New Size(180, 20)
        personalListensLabel.Location = New Point(10, 220)
        musicPanel.Controls.Add(personalListensLabel)

        ' Retourner le panneau
        Return musicPanel
    End Function

    Private Sub btnPlay_Click(sender As Object, e As EventArgs) Handles btnPlay.Click
        ' Si la musique est déjà en cours de lecture, on ne fait rien
        If Not isPlaying Then
            Debug.WriteLine("Lecture de la musique en cours...")
            waveOut.Play() ' Démarrer la lecture
            isPlaying = True

            audioPlayer.TogglePlayPause()
            ' Mettre à jour l'état des boutons
            btnPlay.Enabled = False ' Désactiver le bouton Play
            btnPause.Enabled = True ' Activer le bouton Pause
        End If
    End Sub

    Private Sub btnPause_Click(sender As Object, e As EventArgs) Handles btnPause.Click
        ' Si la musique est en cours de lecture, on la met en pause
        If isPlaying Then
            Debug.WriteLine("Mise en pause de la musique...")
            waveOut.Pause() ' Mettre la lecture en pause
            isPlaying = False

            audioPlayer.TogglePlayPause()
            ' Mettre à jour l'état des boutons
            btnPlay.Enabled = True ' Activer le bouton Play
            btnPause.Enabled = False ' Désactiver le bouton Pause
        End If
    End Sub

    Private Sub SearchBox_TextChanged(sender As Object, e As EventArgs) Handles searchBox.TextChanged
        If searchBox.Text.Length >= 3 Then
            Debug.WriteLine("Recherche en cours: " & searchBox.Text)
            LoadSearchResults(searchBox.Text)
        Else
            FlowLayoutMusicPanel.Controls.Clear()
        End If
    End Sub

    ' Charger les résultats de recherche dans le FlowLayoutPanel
    Private Sub LoadSearchResults(searchQuery As String)
        FlowLayoutMusicPanel.Controls.Clear() ' Vide le FlowLayoutPanel avant de le remplir

        Try
            Debug.WriteLine("Connexion à la base de données pour la recherche de musiques...")

            Using conn As New MySqlConnection(connectionString)
                conn.Open()

                Dim query As String = ""
                query = "SELECT m.MusicName, m.Artist, a.AlbumName AS Album, m.MusicImage, " &
                        "m.ListenedCounter AS TotalListens, " &
                        "IFNULL(l.ListenCount, 0) AS PersonalListens " &
                        "FROM Musics m " &
                        "LEFT JOIN UserMusicListenCount l ON m.MusicID = l.MusicID AND l.UserID = @UserID " &
                        "LEFT JOIN Albums a ON m.AlbumID = a.AlbumID " &
                        "WHERE m.MusicName LIKE @SearchQuery"


                Dim cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@SearchQuery", "%" & searchQuery & "%")
                cmd.Parameters.AddWithValue("@UserID", currentUserID)

                Dim reader As MySqlDataReader = cmd.ExecuteReader()

                While reader.Read()
                    Dim songName As String = reader("MusicName").ToString()
                    Dim artist As String = reader("Artist").ToString()
                    Dim album As String = reader("Album").ToString()
                    Dim totalListens As Integer = Convert.ToInt32(reader("TotalListens"))
                    Dim personalListens As Integer = Convert.ToInt32(reader("PersonalListens"))
                    Dim imagePath As String = reader("MusicImage").ToString()

                    ' Ajouter le panneau de musique au FlowLayoutPanel
                    Dim musicPanel As Panel = CreateMusicPanel(imagePath, songName, artist, album, totalListens, personalListens)
                    FlowLayoutMusicPanel.Controls.Add(musicPanel)
                End While
            End Using
        Catch ex As Exception
            Debug.WriteLine("Erreur de connexion à la base de données: " & ex.Message)
            MessageBox.Show("Erreur de connexion à la base de données: " & ex.Message)
        End Try
    End Sub

    Private Sub MusicItem_Click(sender As Object, e As EventArgs)
        Dim clickedControl As Control = CType(sender, Control)
        If clickedControl.Tag IsNot Nothing Then
            Dim songName As String = clickedControl.Tag.ToString() ' Récupère le nom de la chanson
            Debug.WriteLine("Lecture de la chanson sélectionnée: " & songName)
            PlaySong(songName) ' Appelle la fonction de lecture
        Else
            Debug.WriteLine("Le contrôle cliqué n'a pas de Tag défini.")
        End If
    End Sub

    Private Sub PlaySong(songName As String)
        Dim musicLink As String = GetMusicLink(songName)

        If Not String.IsNullOrEmpty(musicLink) Then
            Try
                Debug.WriteLine("Lecture du flux audio à partir de l'URL: " & musicLink)

                ' Arrêter le lecteur précédent si nécessaire
                If waveOut.PlaybackState = PlaybackState.Playing Then
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
            Debug.WriteLine("Mise à jour des compteurs d'écoutes...")

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
                Debug.WriteLine("Table 'UserMusicListenCount' vérifiée et créée si nécessaire.")

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
                        Debug.WriteLine("Compteur mis à jour.")
                    Else
                        ' Si l'enregistrement n'existe pas, l'ajouter avec une valeur de compteur initiale de 1
                        Debug.WriteLine("Enregistrement non trouvé. Insertion du nouveau compteur.")
                        Dim insertQuery As String = "INSERT INTO UserMusicListenCount (UserID, MusicID, ListenCount) VALUES (@UserID, @MusicID, 1)"
                        Dim insertCmd As New MySqlCommand(insertQuery, conn)
                        insertCmd.Parameters.AddWithValue("@UserID", currentUserID)
                        insertCmd.Parameters.AddWithValue("@MusicID", musicID)
                        insertCmd.ExecuteNonQuery()
                        Debug.WriteLine("Compteur initialisé à 1.")
                    End If

                    ' Mettre à jour le compteur global des écoutes dans la table Musics
                    Dim updateGlobalCountQuery As String = "UPDATE Musics SET ListenedCounter = ListenedCounter + 1 WHERE MusicID = @MusicID"
                    Dim updateGlobalCountCmd As New MySqlCommand(updateGlobalCountQuery, conn)
                    updateGlobalCountCmd.Parameters.AddWithValue("@MusicID", musicID)
                    updateGlobalCountCmd.ExecuteNonQuery()
                    Debug.WriteLine("Compteur global des écoutes mis à jour.")
                Else
                    Debug.WriteLine("Aucun MusicID trouvé pour la chanson : " & songName)
                End If
            End Using

            Debug.WriteLine("Compteurs mis à jour avec succès.")
        Catch ex As Exception
            Debug.WriteLine("Erreur lors de la mise à jour des compteurs: " & ex.Message)
            MessageBox.Show("Erreur lors de la mise à jour des compteurs: " & ex.Message)
        End Try
    End Sub

    Private Function GetUserIDFromJson() As Integer
        ' Charger le fichier JSON contenant l'ID de l'utilisateur depuis ApplicationData
        Dim userInfoFile As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MusicPlayer", "userinfo")

        If File.Exists(userInfoFile) Then
            ' Lire le contenu du fichier JSON
            Dim jsonData As String = File.ReadAllText(userInfoFile)

            ' Utiliser Newtonsoft.Json pour analyser le contenu JSON
            Dim jsonObject As JObject = JObject.Parse(jsonData)

            ' Extraire l'ID de l'utilisateur (UserID)
            Dim userID As Integer = jsonObject("UserID").Value(Of Integer)()

            ' Retourner l'ID de l'utilisateur
            Return userID
        Else
            ' Si le fichier JSON n'existe pas, afficher une erreur ou retourner 0
            MessageBox.Show("Le fichier userinfo est introuvable.")
            Return 0 ' Valeur par défaut si le fichier n'est pas trouvé
        End If
    End Function

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class