Imports System.Threading
Imports System.Net.Sockets
Imports System.Text.RegularExpressions
Imports ChatCommon

Partial Module Chat

    Private clients As New Hashtable()
    Private acceptConnectionsThread As Thread = New Thread(AddressOf AcceptConnections)

    Sub Main()

        acceptConnectionsThread.Start()

        While True

            HandleCommands(Nothing, Console.ReadLine(), True)

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


                If Not ValidUsername(username) Then

                    Console.WriteLine("Login request: '" + username + "' failed: Invalid username.")
                    Send(clientSocket, "/e Username is invalid")
                    Continue While

                End If

                If clients.ContainsKey(username) Then

                    Console.WriteLine("Login request: '" + username + "' failed: Username exists.")
                    Send(clientSocket, "/e Username is already taken")
                    Continue While

                End If

                Console.WriteLine("Login request: '" + username + "' successful.")
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
        Console.WriteLine("Server stopped")

    End Sub

    Public Function ValidUsername(username As String) As Boolean

        If username Is Nothing Then
            Return False
        End If

        If username.Length <= 0 Or username.Length > 30 Then
            Return False
        End If

        If Not IsAlphaNumeric(username) Then
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

    Public Sub Whisper(sender As Client, recipient As String, message As String)

        If sender Is Nothing Then
            Return
        End If

        If Not clients.ContainsKey(recipient) Then

            sender.Send("/e user '{0}' could not be found.", recipient)
            Return

        End If

        Dim recipientClient As Client = clients(recipient)

        recipientClient.Send("/w {0} {1}", sender.username, message)

    End Sub

    Public Sub Send(socket As TcpClient, message As String)

        Console.WriteLine(message)

        Dim bytes() As Byte = Text.Encoding.ASCII.GetBytes(message)
        Dim networkStream As NetworkStream = socket.GetStream()

        networkStream.Write(bytes, 0, bytes.Length)
        networkStream.Flush()

    End Sub

    Private Function Disconnect(client As Client, ByRef errorMsg As String) As Boolean

        If client Is Nothing Then
            errorMsg = "Client argument is null."
            Return False
        End If

        If Not client.socket.Connected Then
            errorMsg = "Client is not connected."
            Return False
        End If

        client.socket.Client.Disconnect(False)

        Return True

    End Function

    Private Function Kick(username As String, ByRef errorMsg As String) As Boolean

        If username Is Nothing Then
            errorMsg = "Username is null."
            Return False
        End If

        If Not clients.ContainsKey(username) Then
            errorMsg = "A user with the name " + username + " does not exist."
            Return False
        End If

        Dim client As Client = clients(username)

        client.Send("/kick")
        client.socket.Client.Disconnect(False)

        Return True

    End Function

    Private Sub HandleCommands(sender As Client, input As String, asServer As Boolean)

        Dim command As String = ParseCommand(input)
        Dim errorMsg As String = ""

        Select Case command

            Case "dc disconnect"

                If Not Disconnect(sender, errorMsg) Then
                    Console.WriteLine(errorMsg)
                End If

            Case "kick"

                Dim username As String = ParseCommandArgs(input)

                If Not Kick(username, errorMsg) Then
                    Console.WriteLine(errorMsg)
                End If

            Case "s", "say"

                Dim message As String = ParseCommandArgs(input)

                Broadcast(sender, message)

            Case "w", "whisper"

                Dim args As String() = ParseCommandArgs(input, 1, True)

                Whisper(sender, args(0), args(1))

        End Select

    End Sub

End Module
