Imports System
Imports System.Drawing

Module Program

    Public Const StartColor As Integer = &HFF00FF00 'GREEN
    Public Const EndColor As Integer = &HFFFF0000 'RED
    Public Const Empty As Integer = &HFFFFFFFF 'WHITE

    Public Const MapPath As String = "../../../map.png"

    Private Map As Node(,)
    Private StartNode, EndNode As Node
    Private result As Path

    Sub Main(args As String())

        LoadMap()

        If Map Is Nothing Then

            Return

        End If

        PrintMap()

        result = Path.FindPath(StartNode, EndNode, 1000, 10000, AddressOf StandardCostFunction, AddressOf GetNode)

        DrawPath()

        result.PrintDetails()

    End Sub


    Public Function GetNode(x As Integer, y As Integer)

        If x < 0 Or y < 0 Or x >= Map.GetLength(0) Or y >= Map.GetLength(1) Then

            Return Nothing

        End If

        Return Map(x, y)

    End Function


    Public Sub PrintMap()

        For y = 0 To Map.GetLength(1) - 2
            For x = 0 To Map.GetLength(0) - 2

                Dim node As Node = Map(x, y)

                Console.SetCursorPosition(x, y)

                If node.untraversable Then

                    Console.Write("#")

                End If

            Next
        Next

        Console.ForegroundColor = ConsoleColor.Green

        Console.SetCursorPosition(StartNode.position.X, StartNode.position.Y)
        Console.Write("O")

        Console.SetCursorPosition(EndNode.position.X, EndNode.position.Y)
        Console.Write("X")

        Console.ForegroundColor = ConsoleColor.White

        Console.SetCursorPosition(0, Map.GetLength(1))

    End Sub

    Public Sub DrawPath()

        If result.nodes Is Nothing Then
            Return
        End If

        Console.ForegroundColor = ConsoleColor.Green

        For Each node As Node In result.nodes

            Console.SetCursorPosition(node.position.X, node.position.Y)

            If node Is result.StartNode Then
                Console.Write("0")
            ElseIf node Is result.EndNode Then
                Console.Write("X")
            Else
                Console.Write("*")
            End If

            Threading.Thread.Sleep(1)

        Next

        Console.ForegroundColor = ConsoleColor.White
        Console.SetCursorPosition(0, Map.GetLength(1))

    End Sub

    '''<summary>
    ''' Loads a bitmap image from a supplied file path.
    ''' </summary>
    ''' <returns>
    ''' A Bitmap object representing the image.
    ''' </returns>
    Public Function LoadBitmap(ByVal path As String) As Bitmap

        If String.IsNullOrEmpty(path) Then

            Console.WriteLine("ERROR: File path was null or empty")
            Return Nothing

        End If

        Dim fileInfo As New IO.FileInfo(path)

        If Not fileInfo.Exists Then

            Console.WriteLine("ERROR: File does not exist.")
            Return Nothing

        End If

        Dim extension As String = fileInfo.Extension

        If Not extension.Equals(".jpg") And Not extension.Equals(".png") Then

            Console.WriteLine("ERROR: File is not a supported type")
            Return Nothing

        End If

        Return New Bitmap(path)

    End Function

    Sub LoadMap()

        Dim image = LoadBitmap(MapPath)

        If image Is Nothing Then
            Console.WriteLine("Failed to load map.")
            Return
        End If

        Map = New Node(image.Width, image.Height) {}

        For y = 0 To image.Height - 1
            For x = 0 To image.Width - 1

                Dim argb = image.GetPixel(x, y).ToArgb()

                If argb = Empty Then

                    Map(x, y) = New Node(New Point(x, y), False, 0)

                ElseIf argb = StartColor Then

                    StartNode = New Node(New Point(x, y), False, 0)
                    Map(x, y) = StartNode

                ElseIf argb = EndColor Then

                    EndNode = New Node(New Point(x, y), False, 0)
                    Map(x, y) = EndNode

                Else

                    Map(x, y) = New Node(New Point(x, y), True, 0)

                End If

            Next
        Next

    End Sub

End Module
