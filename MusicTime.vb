Imports MySql.Data.MySqlClient

Public Class MusicTime
    Private connectionString As String = "Server=srv1049.hstgr.io;Database=u842356047_musicplayerdb;User Id=u842356047_gregcreeper95;Password=Minecraft0711@@@!!!;"

    Public Function CalculateTotalDuration() As String
        Dim totalDurationInSeconds As Integer = 0

        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()

                Dim query As String = "SELECT SUM(Duration) FROM Musics"
                Dim cmd As New MySqlCommand(query, conn)

                Dim result As Object = cmd.ExecuteScalar()

                If result IsNot Nothing Then
                    totalDurationInSeconds = Convert.ToInt32(result)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur lors du calcul de la durée totale: " & ex.Message)
        End Try

        ' Convertir la durée totale en minutes et secondes
        Dim minutes As Integer = totalDurationInSeconds \ 60
        Dim seconds As Integer = totalDurationInSeconds Mod 60

        ' Retourner la durée formatée en "minutes:secondes"
        Return String.Format("{0}:{1:00}", minutes, seconds)
    End Function

    Public Function GetMusicDuration(songName As String) As String
        Dim durationInSeconds As Integer = 0

        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()

                Dim query As String = "SELECT Duration FROM Musics WHERE MusicName = @SongName"
                Dim cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@SongName", songName)

                Dim result As Object = cmd.ExecuteScalar()

                If result IsNot Nothing Then
                    durationInSeconds = Convert.ToInt32(result)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur lors de la récupération de la durée de la musique: " & ex.Message)
        End Try

        ' Convertir la durée en minutes et secondes
        Dim minutes As Integer = durationInSeconds \ 60
        Dim seconds As Integer = durationInSeconds Mod 60

        ' Retourner la durée formatée en "minutes:secondes"
        Return String.Format("{0}:{1:00}", minutes, seconds)
    End Function
End Class
