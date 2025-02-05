Imports System.Data.SqlClient
Imports System.Net.Http
Imports NAudio.Wave

Public Class Form1
    Inherits Form

    Private sidebar As Panel
    Private btnLibrary As Button
    Private btnPlaylists As Button
    Private btnSettings As Button
    Private mainPanel As Panel
    Private playerControls As Panel
    Private btnPlay As Button
    Private btnPause As Button
    Private btnNext As Button
    Private btnPrev As Button
    Private searchBox As TextBox
    Private resultsList As ListBox
    Private waveOut As WaveOutEvent
    Private audioFileReader As AudioFileReader

    ' Connexion à la base de données
    Private connectionString As String = "Data Source=YourServer;Initial Catalog=YourDatabase;Integrated Security=True"

    Public Sub New()
        InitializeComponent()

        ' Initialiser l'interface
        Me.Text = "Music Player"
        Me.Size = New Size(800, 600)
        Me.StartPosition = FormStartPosition.CenterScreen

        ' Sidebar
        sidebar = New Panel() With {
            .Size = New Size(200, Me.Height),
            .BackColor = Color.DarkGray,
            .Dock = DockStyle.Left
        }
        Me.Controls.Add(sidebar)

        btnLibrary = New Button() With {.Text = "Library", .Dock = DockStyle.Top}
        btnPlaylists = New Button() With {.Text = "Playlists", .Dock = DockStyle.Top}
        btnSettings = New Button() With {.Text = "Settings", .Dock = DockStyle.Top}
        sidebar.Controls.AddRange(New Control() {btnLibrary, btnPlaylists, btnSettings})

        ' Main Panel
        mainPanel = New Panel() With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.White
        }
        Me.Controls.Add(mainPanel)

        ' Search Box
        searchBox = New TextBox() With {
            .Dock = DockStyle.Top,
            .Text = "Search for a song...",
            .ForeColor = Color.Gray
        }
        AddHandler searchBox.Enter, AddressOf searchBox_Enter
        AddHandler searchBox.Leave, AddressOf searchBox_Leave
        AddHandler searchBox.TextChanged, AddressOf SearchBox_TextChanged
        mainPanel.Controls.Add(searchBox)

        ' Results List
        resultsList = New ListBox() With {
            .Dock = DockStyle.Fill
        }
        AddHandler resultsList.DoubleClick, AddressOf ResultsList_DoubleClick
        mainPanel.Controls.Add(resultsList)

        ' Player Controls
        playerControls = New Panel() With {
            .Size = New Size(Me.Width, 50),
            .Dock = DockStyle.Bottom,
            .BackColor = Color.LightGray
        }
        Me.Controls.Add(playerControls)

        btnPrev = New Button() With {.Text = "⏮", .Dock = DockStyle.Left}
        btnPlay = New Button() With {.Text = "▶", .Dock = DockStyle.Left}
        btnPause = New Button() With {.Text = "⏸", .Dock = DockStyle.Left}
        btnNext = New Button() With {.Text = "⏭", .Dock = DockStyle.Left}
        playerControls.Controls.AddRange(New Control() {btnPrev, btnPlay, btnPause, btnNext})

        ' Initialiser le lecteur audio
        waveOut = New WaveOutEvent()
    End Sub

    ' Recherche de musique dans la base de données
    Private Sub SearchBox_TextChanged(sender As Object, e As EventArgs)
        ' Effectuer la recherche dans la base de données
        Dim searchQuery As String = searchBox.Text
        If searchQuery.Length >= 3 Then ' Limiter la recherche aux chaînes de 3 caractères ou plus
            LoadSearchResults(searchQuery)
        Else
            resultsList.Items.Clear()
        End If
    End Sub

    ' Charger les résultats de recherche
    Private Sub LoadSearchResults(searchQuery As String)
        resultsList.Items.Clear()
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT SongName FROM Songs WHERE SongName LIKE @SearchQuery"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@SearchQuery", "%" & searchQuery & "%")
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                While reader.Read()
                    resultsList.Items.Add(reader("SongName").ToString())
                End While
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur de connexion à la base de données: " & ex.Message)
        End Try
    End Sub

    ' Lorsque l'utilisateur double-clique sur une chanson dans la liste
    Private Sub ResultsList_DoubleClick(sender As Object, e As EventArgs)
        If resultsList.SelectedItem IsNot Nothing Then
            Dim selectedSong As String = resultsList.SelectedItem.ToString()
            PlaySong(selectedSong)
        End If
    End Sub

    ' Jouer la chanson
    Private Sub PlaySong(songName As String)
        ' Rechercher l'URL du fichier audio dans la base de données
        Dim audioFilePath As String = GetSongFilePath(songName)
        If Not String.IsNullOrEmpty(audioFilePath) Then
            ' Lancer la lecture en streaming
            PlayAudioFromUrl(audioFilePath)
        End If
    End Sub

    ' Récupérer le chemin du fichier audio dans la base de données
    Private Function GetSongFilePath(songName As String) As String
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT SongFilePath FROM Songs WHERE SongName = @SongName"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@SongName", songName)
                Dim filePath As Object = cmd.ExecuteScalar()
                If filePath IsNot Nothing Then
                    Return filePath.ToString()
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur de récupération du fichier: " & ex.Message)
        End Try
        Return String.Empty
    End Function

    ' Lecture audio en streaming via NAudio
    Private Sub PlayAudioFromUrl(audioUrl As String)
        Try
            ' Télécharger le flux audio en streaming
            Dim client As New HttpClient()
            Dim audioStream As IO.Stream = client.GetStreamAsync(audioUrl).Result

            ' Lire le flux audio
            audioFileReader = New AudioFileReader(audioStream)
            waveOut.Init(audioFileReader)
            waveOut.Play()
        Catch ex As Exception
            MessageBox.Show("Erreur lors de la lecture de l'audio: " & ex.Message)
        End Try
    End Sub

    ' Contrôle des boutons Play, Pause, etc.
    Private Sub btnPlay_Click(sender As Object, e As EventArgs)
        waveOut.Play()
    End Sub

    Private Sub btnPause_Click(sender As Object, e As EventArgs)
        waveOut.Pause()
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs)
        ' Implémenter la logique pour la chanson suivante
    End Sub

    Private Sub btnPrev_Click(sender As Object, e As EventArgs)
        ' Implémenter la logique pour la chanson précédente
    End Sub

    ' Gestion du texte de remplacement pour la recherche (simule un placeholder)
    Private Sub searchBox_Enter(sender As Object, e As EventArgs)
        If searchBox.Text = "Search for a song..." Then
            searchBox.Text = ""
            searchBox.ForeColor = Color.Black
        End If
    End Sub

    Private Sub searchBox_Leave(sender As Object, e As EventArgs)
        If String.IsNullOrEmpty(searchBox.Text) Then
            searchBox.Text = "Search for a song..."
            searchBox.ForeColor = Color.Gray
        End If
    End Sub
End Class
