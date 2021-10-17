Imports System.Threading
Imports System.Net.Sockets
Imports ChatCommon

Module Chat

    Private ReadOnly clientSocket As New TcpClient()
    Private serverStream As NetworkStream
    Private username As String
    Private awaitingResponse As Boolean
    Private thread As Thread

    Sub Main()

        Console.WriteLine("Client started.")
        Start()

    End Sub

    Private Sub StopRunning()
        serverStream.Close()
        clientSocket.Close()
        thread.Abort()
    End Sub

    Private Sub Restart()
        StopRunning()
        Start()
    End Sub

    Private Sub Start()
        Console.WriteLine("Connecting...")
        Try
            clientSocket.Connect(HostName, 8888)
            serverStream = clientSocket.GetStream()
            Console.WriteLine("Connected.")
        Catch ex As Exception

            HandleInStream("/e " + ex.Message)

            Return

        End Try

        thread = New Thread(AddressOf GetMessage)
        thread.Start()

        While username Is Nothing And awaitingResponse = False

            Console.WriteLine("Enter a username")
            Dim input As String = Console.ReadLine()
            Dim outStream As Byte() = Text.Encoding.ASCII.GetBytes(input)

            serverStream.Write(outStream, 0, outStream.Length)
            serverStream.Flush()
            awaitingResponse = True

        End While

        While clientSocket.Connected
            Dim input = Console.ReadLine()
            SendMessage(input)
        End While

    End Sub

    Sub SendMessage(message As String)

        Try
            Dim outStream As Byte() = Text.Encoding.ASCII.GetBytes(message)
            serverStream.Write(outStream, 0, outStream.Length)
            serverStream.Flush()
        Catch ex As Exception

        End Try

    End Sub

    Sub GetMessage()

        While True
            Try

                serverStream = clientSocket.GetStream()
                Dim bufferSize As Integer = clientSocket.ReceiveBufferSize
                Dim inStream(65536) As Byte
                serverStream.Read(inStream, 0, bufferSize)
                Dim data As String = GetString(inStream)

                HandleInStream(data)

            Catch ex As Exception

                ConnectionClosedByHost()
                Exit While

            End Try



        End While

    End Sub

    Private Sub ConnectionClosedByHost()

    End Sub

    Private Sub HandleInStream(input As String)

        If Not input.StartsWith("/") Then

            Console.WriteLine(input)

        Else

            Dim command As String = ParseCommand(input)

            Select Case command

                Case "kick"
                    HandleInStream("/i You have been kicked from the server.")
                    Restart()

                Case "s", "say"

                    Dim args As String() = ParseCommandArgs(input, 1, True)

                    Console.WriteLine("{0} says: {1}", args(0), args(1))

                Case "w", "whisper"

                    Dim args As String() = ParseCommandArgs(input, 2, True)

                    Console.ForegroundColor = ConsoleColor.Magenta
                    Console.WriteLine("{0} whispers: {1}", args(0), args(1))
                    Console.ResetColor()

                Case "e", "error"
                    awaitingResponse = False

                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine(input)
                    Console.ResetColor()

                Case "i", "info"
                    awaitingResponse = False

                    Console.ForegroundColor = ConsoleColor.Yellow
                    Console.WriteLine(input)
                    Console.ResetColor()

                Case "u", "username"
                    awaitingResponse = False

                    Dim arg As String = ParseCommandArgs(input)
                    username = arg
                    Console.WriteLine("Your username is {0}", username)

            End Select

        End If

    End Sub

End Module
