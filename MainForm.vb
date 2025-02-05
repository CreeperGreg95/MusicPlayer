Imports NAudio.Wave
Imports NAudio.CoreAudioApi
Imports System.Data.SqlClient

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
    Private isPlaying As Boolean = False
    Private isPaused As Boolean = False

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
            .PlaceholderText = "Search for a song..."
        }
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

        ' Initialiser le lecteur audio en streaming
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

    ' Jouer la chanson en streaming
    Private Sub PlaySong(songName As String)
        ' Rechercher l'URL du fichier audio dans la base de données
        Dim audioUrl As String = GetSongFileUrl(songName)
        If Not String.IsNullOrEmpty(audioUrl) Then
            ' Lancer la lecture en streaming avec NAudio
            Try
                Dim stream As New System.Net.Http.HttpClient()
                Dim audioStream As System.IO.Stream = stream.GetStreamAsync(audioUrl).Result

                audioFileReader = New AudioFileReader(audioStream)
                waveOut.Init(audioFileReader)
                waveOut.Play()
                isPlaying = True
                isPaused = False
            Catch ex As Exception
                MessageBox.Show("Erreur de lecture du flux audio: " & ex.Message)
            End Try
        End If
    End Sub

    ' Récupérer l'URL du fichier audio dans la base de données
    Private Function GetSongFileUrl(songName As String) As String
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT SongFileUrl FROM Songs WHERE SongName = @SongName"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@SongName", songName)
                Dim fileUrl As Object = cmd.ExecuteScalar()
                If fileUrl IsNot Nothing Then
                    Return fileUrl.ToString()
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur de récupération de l'URL: " & ex.Message)
        End Try
        Return String.Empty
    End Function

    ' Contrôle des boutons Play, Pause, etc.
    Private Sub btnPlay_Click(sender As Object, e As EventArgs)
        If Not isPlaying Then
            waveOut.Play()
            isPlaying = True
            isPaused = False
        End If
    End Sub

    Private Sub btnPause_Click(sender As Object, e As EventArgs)
        If isPlaying Then
            waveOut.Pause()
            isPlaying = False
            isPaused = True
        End If
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs)
        ' Implémenter la logique pour la chanson suivante
    End Sub

    Private Sub btnPrev_Click(sender As Object, e As EventArgs)
        ' Implémenter la logique pour la chanson précédente
    End Sub
End Class
