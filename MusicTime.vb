Imports MySql.Data.MySqlClient
Imports System.Net.Http
Imports Newtonsoft.Json.Linq

Public Class MusicTime

    Private Async Function GetConnectionStringFromServer() As Task(Of String)
        Dim url As String = "https://musicplayer.creepergreg951.eu/connection.php"

        Using client As New HttpClient()
            Dim parameters As New Dictionary(Of String, String) From {
            {"action", "getConnection"}
        }

            Dim content As New FormUrlEncodedContent(parameters)
            Dim response As HttpResponseMessage = Await client.PostAsync(url, content)

            If response.IsSuccessStatusCode Then
                Dim result As String = Await response.Content.ReadAsStringAsync()
                Dim json As JObject = JObject.Parse(result)

                Dim server = json("servername").ToString()
                Dim user = json("dbusername").ToString()
                Dim pass = json("dbpassword").ToString()
                Dim db = json("dbname").ToString()

                Return $"Server={server};Database={db};User Id={user};Password={pass};"
            Else
                Throw New Exception("Impossible de récupérer la chaîne de connexion depuis le serveur.")
            End If
        End Using
    End Function

    Public Async Function CalculateTotalDuration() As Task(Of String)
        Dim totalDurationInSeconds As Integer = 0

        Try
            Dim connStr As String = Await GetConnectionStringFromServer()

            Using conn As New MySqlConnection(connStr)
                Await conn.OpenAsync()

                Dim query As String = "SELECT SUM(Duration) FROM Musics"
                Using cmd As New MySqlCommand(query, conn)
                    Dim result As Object = Await cmd.ExecuteScalarAsync()

                    If result IsNot Nothing Then
                        totalDurationInSeconds = Convert.ToInt32(result)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur lors du calcul de la durée totale: " & ex.Message)
        End Try

        Dim minutes As Integer = totalDurationInSeconds \ 60
        Dim seconds As Integer = totalDurationInSeconds Mod 60
        Return String.Format("{0}:{1:00}", minutes, seconds)
    End Function

    Public Async Function GetMusicDuration(songName As String) As Task(Of String)
        Dim durationInSeconds As Integer = 0

        Try
            Dim connectionString As String = Await GetConnectionStringFromServer()

            Using conn As New MySqlConnection(connectionString)
                Await conn.OpenAsync()

                Dim query As String = "SELECT Duration FROM Musics WHERE MusicName = @SongName"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@SongName", songName)

                    Dim result As Object = Await cmd.ExecuteScalarAsync()

                    If result IsNot Nothing Then
                        durationInSeconds = Convert.ToInt32(result)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur lors de la récupération de la durée de la musique: " & ex.Message)
        End Try

        Dim minutes As Integer = durationInSeconds \ 60
        Dim seconds As Integer = durationInSeconds Mod 60
        Return String.Format("{0}:{1:00}", minutes, seconds)
    End Function
End Class
