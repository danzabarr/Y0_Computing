Imports System
Imports System.Net.Sockets
Module Server

    Public Const Port As Integer = 8888

    Private clients As New Hashtable()

    Sub Main(args As String())
        Dim serverSocket As TcpListener = TcpListener.Create(Port)
        Dim clientSocket As TcpClient
        serverSocket.Start()

        While True
            Try
                clientSocket = serverSocket.AcceptTcpClient()
                Dim bytes(65536) As Byte
                Dim networkStream As NetworkStream = clientSocket.GetStream()
                networkStream.Read(bytes, 0, clientSocket.ReceiveBufferSize)
                Dim data As String = Text.Encoding.ASCII.GetString(bytes)
                data = data.Substring(0, data.IndexOf("$"))

                clients(data) = New Client(clientSocket, data)

                Broadcast(data + " joined")


            Catch ex As Exception
                Console.WriteLine(ex.ToString)
            End Try

        End While

        Try
            clientSocket.Close()
        Catch ex As NullReferenceException

        End Try
        serverSocket.Stop()
        Console.WriteLine("Exit")

    End Sub
    Public Sub Broadcast(message As String)

        Console.WriteLine(message)

        For Each client As DictionaryEntry In clients

            client.Value.Send(message)

        Next

    End Sub

    Public Class Client
        Dim socket As TcpClient
        Dim username As String

        Public Sub New(socket As TcpClient, username As String)

            Me.socket = socket
            Me.username = username

            Dim thread As New Threading.Thread(AddressOf Run)
            thread.Start()

        End Sub

        Private Sub Run()
            Console.WriteLine("Created thread for " + username)
            Dim networkStream As NetworkStream
            Dim bytes(65536) As Byte
            While socket.Client.Connected
                Try

                    networkStream = socket.GetStream()
                    networkStream.Read(bytes, 0, CInt(socket.ReceiveBufferSize))
                    Dim data As String = Text.Encoding.ASCII.GetString(bytes)
                    data = data.Substring(0, data.IndexOf("$"))
                    Broadcast(username + ": " + data)
                    networkStream.Flush()
                Catch ex As Exception

                End Try
            End While

            Broadcast(username + " disconnected")
            socket.Close()
            If networkStream IsNot Nothing Then

                networkStream.Flush()
                networkStream.Close()
            End If

        End Sub

        Public Sub Send(message As String)
            Dim bytes() As Byte = Text.Encoding.ASCII.GetBytes(message)
            Dim networkStream As NetworkStream = socket.GetStream()
            networkStream.Write(bytes, 0, bytes.Length)
            networkStream.Flush()

        End Sub

    End Class

End Module
