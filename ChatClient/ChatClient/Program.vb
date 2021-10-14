Imports System
Imports System.Net.Sockets

Module Program


    Public Const HostName As String = "2.27.89.249"
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
        Program.username = username
        Console.WriteLine("Client Started")
        clientSocket.Connect("127.0.0.1", 8888)
        serverStream = clientSocket.GetStream()
        Dim outStream As Byte() = Text.Encoding.ASCII.GetBytes(username + "$")
        serverStream.Write(outStream, 0, outStream.Length)
        serverStream.Flush()

        Dim ctThread As New Threading.Thread(AddressOf GetMessage)
        ctThread.Start()

        Console.WriteLine("Client Socket Program - Server Connected ...")

    End Sub

    Sub SendMessage(message As String)
        Dim outStream As Byte() = Text.Encoding.ASCII.GetBytes(message + "$")
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
                Dim data As String = Text.Encoding.ASCII.GetString(inStream)
                Console.WriteLine(data)
            Catch ex As Exception
                Console.WriteLine(ex.ToString)
                Exit While
            End Try


        End While

    End Sub

End Module
