Imports System
Imports System.Net.Sockets
Imports System.Text.RegularExpressions

Module Server

    Public Const Port As Integer = 8888

    Private clients As New Hashtable()

    Public Function GetString(bytes As Byte()) As String

        If bytes Is Nothing Then
            Return ""
        End If

        Dim length = Array.IndexOf(Of Byte)(bytes, 0)

        Console.WriteLine("length of bytes: {0}", bytes.Length)
        Console.WriteLine("index of 0: {0}", length)

        If length = -1 Then
            length = bytes.Length
        End If

        Return Text.Encoding.ASCII.GetString(bytes).Substring(0, length)

    End Function

    Sub Main()

        Dim thread As New Threading.Thread(AddressOf AcceptConnections)
        thread.Start()

        While True
            Dim input As String = Console.ReadLine()

            If Not input.StartsWith("/") Then
                input = "/i " + input
            End If

            Dim args As String() = input.Substring(1).Split(" ")

            Select Case args(0)
                Case "i"

                Case "s"
                    Broadcast(input)

                Case "i"


            End Select

        End While

    End Sub

    Private Sub AcceptConnections()

        Dim serverSocket As TcpListener = TcpListener.Create(Port)
        Dim clientSocket As TcpClient
        serverSocket.Start()
        Console.WriteLine("Server started")

        While True
            Try
                clientSocket = serverSocket.AcceptTcpClient()
                'Assumes username is <= 30 characters
                Dim bytes(29) As Byte
                Dim networkStream As NetworkStream = clientSocket.GetStream()
                networkStream.Read(bytes, 0, 30)
                Dim username As String = GetString(bytes)

                Console.WriteLine("Login request: '" + username + "'")

                If Not ValidUsername(username) Then

                    Console.WriteLine("Username is invalid")

                    Send(clientSocket, "/e Username is invalid")
                    Continue While

                End If

                If clients.ContainsKey(username) Then

                    Console.WriteLine("Username exists")

                    Send(clientSocket, "/e Username is already taken")
                    Continue While

                End If

                Send(clientSocket, "/u " + username)

                clients(username) = New Client(clientSocket, username)
                Broadcast("/i " + username + " joined")

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

    Public Function ValidUsername(username As String) As Boolean

        If username Is Nothing Then
            Console.WriteLine("Null")
            Return False
        End If

        If username.Length <= 0 Or username.Length > 30 Then
            Console.WriteLine("Invalid length")
            Return False
        End If

        If Not IsAlphaNumeric(username) Then
            Console.WriteLine("Non-Alphanumeric")
            Return False
        End If

        Return True

    End Function

    Public Function IsAlphaNumeric(strToCheck As String) As Boolean

        Dim pattern As New Regex("[^a-zA-Z0-9]")
        Return Not pattern.IsMatch(strToCheck)

    End Function


    Public Sub Broadcast(message As String)

        For Each entry As DictionaryEntry In clients
            Dim client As Client = entry.Value
            client.Send(message)
        Next

    End Sub

    Public Sub Broadcast(sender As Client, message As String)

        Console.WriteLine(message)

        For Each entry As DictionaryEntry In clients

            Dim client As Client = entry.Value

            If client Is sender Then
                Continue For
            End If

            client.Send("/s " + sender.username + " " + message)
        Next

    End Sub

    Public Sub Send(socket As TcpClient, message As String)

        Console.WriteLine(message)

        Dim bytes() As Byte = Text.Encoding.ASCII.GetBytes(message)
        Dim networkStream As NetworkStream = socket.GetStream()

        networkStream.Write(bytes, 0, bytes.Length)
        networkStream.Flush()

    End Sub

    Private Sub ParseData(sender As Client, bytes As Byte())

        Dim data As String = GetString(bytes)

        Broadcast(sender, data)

    End Sub

    Public Class Client

        Public socket As TcpClient
        Public username As String

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
                    networkStream.Read(bytes, 0, socket.ReceiveBufferSize)

                    ParseData(Me, bytes)

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

        Public Sub Send(message As String)

            Dim bytes() As Byte = Text.Encoding.ASCII.GetBytes(message)
            Dim networkStream As NetworkStream = socket.GetStream()
            networkStream.Write(bytes, 0, bytes.Length)
            networkStream.Flush()

        End Sub

    End Class

End Module
