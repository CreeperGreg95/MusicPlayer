Imports System.IO
Imports System.Net.Http
Imports Newtonsoft.Json.Linq
Imports System.Net
Imports MySql.Data.MySqlClient
Imports ModernTrackbar

Public Class MainForm
    Inherits Form
    Private musicController As MusicController
    Private musicControls As MusicControls
    Private audioVolumeControls As AudioVolumeControls
    Private tooltipsApp As TooltipsApp
    Private currentUserID As Integer ' ID de l'utilisateur récupéré du fichier JSON
    Private offlineMode As Boolean = False
    Private connectionString As String = "Server=srv1049.hstgr.io;Database=u842356047_musicplayerdb;User Id=u842356047_gregcreeper95;Password=Minecraft0711@@@!!!;"

    Public Sub New()
        Debug.WriteLine("Initialisation de MainForm...")
        InitializeComponent()
        Me.Text = "Music Player"
        Me.StartPosition = FormStartPosition.CenterScreen

        FlowLayoutMusicPanel.AutoScroll = True
        FlowLayoutMusicPanel.WrapContents = True
        FlowLayoutMusicPanel.FlowDirection = FlowDirection.LeftToRight

        Dim connectionAccount As New ConnectionAccount()
        currentUserID = connectionAccount.GetUserIDFromJson()

        musicController = New MusicController(currentUserID, Me.VolumeTrackbar)
        musicControls = New MusicControls(musicController, btnPlayPause, btnPrev, btnNext)
        ' Correction ici, en s'assurant de passer VolumeTrackbar
        audioVolumeControls = New AudioVolumeControls(musicController.GetWaveOut(), VolumeTrackbar)

        ' 👇 Ajout pour initialiser le tooltip et volume au démarrage
        audioVolumeControls.UpdateTooltip()

        tooltipsApp = New TooltipsApp()
        LoadPlaylists()
        CurrentMusicDuration.Text = "00:00"
        Connection.Hide()
    End Sub

    Private Sub LoadPlaylists()
        ' Fonction pour charger les playlists, ici seulement un exemple
        Debug.WriteLine("Chargement des playlists...")
    End Sub

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
                    ' Sinon, charger depuis un fichier local
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
            musicController.PlaySong(songName) ' Appelle la fonction de lecture

            ' Afficher la durée de la musique sélectionnée
            Dim musicTime As New MusicTime()
            Dim duration As String = musicTime.GetMusicDuration(songName)
            CurrentMusicDuration.Text = duration
        Else
            Debug.WriteLine("Le contrôle cliqué n'a pas de Tag défini.")
        End If
    End Sub
End Class
