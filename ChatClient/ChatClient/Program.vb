Imports System
Imports System.Net.Sockets

Module Program


    Public Const HostName As String = "127.0.0.1" '"2.27.89.249"
    Public Const Port As Integer = 8888
    Private clientSocket As New TcpClient()
    Private serverStream As NetworkStream
    Private username As String

    Sub Main()

        Console.WriteLine("Enter a username to connect")
        Connect(Console.ReadLine())

        While True
            SendMessage(Console.ReadLine())
        End While

    End Sub


    Sub Connect(username As String)
        Console.WriteLine("Client Started")
        clientSocket.Connect(HostName, 8888)
        serverStream = clientSocket.GetStream()
        Dim outStream As Byte() = Text.Encoding.ASCII.GetBytes(username)
        Console.WriteLine("Sending {0} bytes", outStream.Length)
        serverStream.Write(outStream, 0, outStream.Length)
        serverStream.Flush()

        Dim ctThread As New Threading.Thread(AddressOf GetMessage)
        ctThread.Start()

        Console.WriteLine("Client Socket Program - Server Connected ...")

    End Sub

    Sub SendMessage(message As String)
        Dim outStream As Byte() = Text.Encoding.ASCII.GetBytes(message)
        serverStream.Write(outStream, 0, outStream.Length)
        serverStream.Flush()
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
                Console.WriteLine(ex.ToString)
                Exit While
            End Try

        End While

    End Sub

    Private Function HandleInStream(input As String)

        If Not input.StartsWith("/") Then

            Console.WriteLine(input)

        Else

            Dim command As String = ParseCommand(input)

            Select Case command
                Case "s"

                    Dim args As String() = ParseCommandArgs(input, 1, True)

                    Console.WriteLine("{0} says: {1}", args(0), args(1))

                Case "w"

                    Dim args As String() = ParseCommandArgs(input, 2, True)

                    Console.ForegroundColor = ConsoleColor.Magenta
                    Console.WriteLine("{0} whispers: {1}", args(0), args(2))
                    Console.ResetColor()

                Case "e"

                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine(input)
                    Console.ResetColor()

                Case "i"

                    Console.ForegroundColor = ConsoleColor.Yellow
                    Console.WriteLine(input)
                    Console.ResetColor()

                Case "u"
                    Dim arg As String = ParseCommandArgs(input)
                    username = arg
                    Console.WriteLine("Your username is {0}", username)

            End Select

        End If
    End Function

    Private Function ParseCommand(ByRef input As String) As String

        Dim commandEndIndex As Integer = input.IndexOf(" ")
        Dim command As String
        If commandEndIndex <= -1 Then

            command = input.Substring(1)
            input = ""
        Else
            command = input.Substring(1, commandEndIndex - 1)
            input = input.Substring(commandEndIndex + 1)
        End If

        Return command

    End Function

    Private Function ParseCommandArgs(input As String) As String

        Return ParseCommandArgs(input, 0, True)(0)

    End Function

    Private Function ParseCommandArgs(input As String, argCount As Integer) As String()

        Return ParseCommandArgs(input, argCount, False)

    End Function


    Private Function ParseCommandArgs(input As String, argCount As Integer, trailingArg As Boolean) As String()

        Dim argsLength = Math.Max(0, argCount)
        If trailingArg Then
            argsLength += 1
        End If

        Dim args(argsLength - 1) As String

        If input Is Nothing Then
            Return args
        End If

        If input.Length <= 1 Then
            Return args
        End If

        For i = 0 To argCount - 1
            Dim endIndex = input.IndexOf(" ")
            If endIndex <= -1 Then
                endIndex = input.Length
            End If

            args(i) = input.Substring(0, endIndex)

            input = input.Substring(Math.Min(input.Length, endIndex + 1))
        Next

        If trailingArg Then

            args(argsLength - 1) = input

        End If

        Return args

    End Function

    Public Function GetString(bytes As Byte()) As String

        If bytes Is Nothing Then
            Return ""
        End If

        Dim length = Array.IndexOf(Of Byte)(bytes, 0)

        If length = -1 Then
            length = bytes.Length
        End If

        Return Text.Encoding.ASCII.GetString(bytes).Substring(0, length)

    End Function

End Module
