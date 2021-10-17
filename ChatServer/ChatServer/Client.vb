Imports System.Net.Sockets
Imports System.Threading
Imports ChatCommon

Partial Module Chat

    Public Class Client

        Public socket As TcpClient
        Public username As String
        Private ReadOnly thread As New Thread(AddressOf Run)

        Public Sub New(socket As TcpClient, username As String)

            Me.socket = socket
            Me.username = username

            thread.Start()

        End Sub

        Private Sub Run()
            Dim networkStream As NetworkStream
            Dim bytes(65536) As Byte
            While socket.Client.Connected
                Try

                    networkStream = socket.GetStream()
                    networkStream.Read(bytes, 0, socket.ReceiveBufferSize)
                    Dim input As String = GetString(bytes)

                    HandleCommands(Me, input, False)

                    networkStream.Flush()
                Catch ex As Exception

                End Try

            End While

            clients.Remove(username)
            socket.Close()
            networkStream.Flush()
            networkStream.Close()

            Broadcast(username + " disconnected")

        End Sub

        Public Sub Send(message As String, ParamArray args As Object())

            Send(String.Format(message, args))

        End Sub

        Public Sub Send(message As String)

            If Not socket.Connected Then
                Return
            End If

            Dim bytes() As Byte = Text.Encoding.ASCII.GetBytes(message)
            Dim networkStream As NetworkStream = socket.GetStream()
            networkStream.Write(bytes, 0, bytes.Length)
            networkStream.Flush()

        End Sub
    End Class

End Module