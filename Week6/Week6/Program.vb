Imports System

Module Program

    Sub Main(args As String())

        TanksProblem()
        'NotebookProblem()
        'CurrencyConversionProblem()
        'TextDiceProblem()

    End Sub

    Dim turn = 0
    Dim size = 8
    Dim p1Board(size, size) As String
    Dim p2Board(size, size) As String

    Sub PrintBoard(board As String(,), showTanks As Boolean)

        Console.ForegroundColor = ConsoleColor.White

        Console.Write(" ")
        For x = 0 To board.GetLength(0) - 2
            Console.Write($" {x}")
        Next
        Console.WriteLine()

        For y = 0 To board.GetLength(1) - 2

            Console.Write(" ")
            For x = 0 To board.GetLength(0) - 2
                Console.Write("+-")
            Next
            Console.WriteLine("+")

            Console.Write(y)

            For x = 0 To board.GetLength(0) - 2
                Console.Write("|")

                Dim value As String = board(x, y)

                Select Case board(x, y)

                    Case "T"
                        If Not showTanks Then
                            Console.Write(" ")
                        Else
                            Console.ForegroundColor = ConsoleColor.Green
                            Console.Write("T")
                            Console.ForegroundColor = ConsoleColor.White
                        End If

                    Case "H"
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.Write("H")
                        Console.ForegroundColor = ConsoleColor.White

                    Case "M"
                        Console.ForegroundColor = ConsoleColor.White
                        Console.Write("M")

                    Case Else
                        Console.Write(" ")

                End Select

            Next

            Console.WriteLine("|")

        Next

        Console.Write(" ")
        For x = 0 To board.GetLength(0) - 2
            Console.Write("+-")
        Next
        Console.WriteLine("+")


    End Sub

    Sub InitializeBoard()

        Dim board As String(,) = p1Board
        If turn = 1 Then
            board = p2Board
        End If

        For y = 0 To board.GetLength(1) - 1
            For x = 0 To board.GetLength(0) - 1
                board(x, y) = " "
            Next
        Next

        PromptPlacements()

    End Sub

    Sub PromptPlacements()

        Dim board As String(,) = p1Board
        If turn = 1 Then
            board = p2Board
        End If

        PrintBoard(board, True)
        PromptPlacement(1, 1)

        Console.Clear()
        PrintBoard(board, True)
        PromptPlacement(1, 2)

        Console.Clear()
        PrintBoard(board, True)
        PromptPlacement(2, 3)

        Console.Clear()
        PrintBoard(board, True)
        PromptPlacement(3, 4)

        Console.Clear()
        PrintBoard(board, True)

        Console.WriteLine()
        Console.WriteLine($"All of player {turn + 1}'s tanks are placed.")
        Console.WriteLine("Press enter to clear the console and hide your board.")
        Console.ReadLine()

        Console.Clear()

    End Sub

    Sub PromptPlacement(w As Integer, h As Integer)

        Console.WriteLine()
        Console.WriteLine($"Player {turn + 1}, place a {w}x{h} tank")

        Try

            Dim input = Console.ReadLine.Replace(" ", "")

            Dim split As String() = input.Split(",")

            If split.Length <> 3 Then
                Console.WriteLine("Invalid position. Enter a position in the format 'xPosition, yPosition, rotation' each as integers.")
                PromptPlacement(w, h)
                Return
            End If

            Dim x As Integer = Integer.Parse(split(0))
            Dim y As Integer = Integer.Parse(split(1))
            Dim r As Integer = Integer.Parse(split(2))

            Dim rw = w
            Dim rh = h

            If (r Mod 2) = 1 Then 'swap width and height
                rw = h
                rh = w
            End If

            If Not PlaceTank(x, y, rw, rh) Then

                Console.WriteLine("Invalid position")
                PromptPlacement(w, h)
                Return

            End If

        Catch ex As Exception
            Console.WriteLine("Invalid position")
            PromptPlacement(w, h)
            Return
        End Try

    End Sub

    Function PlaceTank(x As Integer, y As Integer, w As Integer, h As Integer) As Boolean

        If x < 0 Or x + w > size Or y < 0 Or y + h > size Then

            Console.WriteLine("Placement is off the board.")
            Console.WriteLine($"{x},{y},{w},{h}")
            Return False

        End If

        Dim board As String(,) = p1Board
        If turn = 1 Then
            board = p2Board
        End If

        For yPos = y To y + h - 1
            For xPos = x To x + w - 1

                Dim tile As String = board(xPos, yPos)
                If tile <> " " Then
                    Console.WriteLine($"A tank already exists at {xPos},{yPos}")
                    Return False
                End If
            Next
        Next

        For yPos = y To y + h - 1
            For xPos = x To x + w - 1

                board(xPos, yPos) = "T"

            Next
        Next

        Return True

    End Function

    Sub PromptCoordinate(message As String)

        Dim board As String(,) = p2Board
        If (turn = 1) Then
            board = p1Board
        End If

        Console.WriteLine(message)

        PrintBoard(board, False)

        Dim input = Console.ReadLine().Replace(" ", "")

        Dim split As String() = input.Split(",")

        If split.Length <> 2 Then

            Console.WriteLine("Invalid coordinate")
            PromptCoordinate(message)
            Return

        End If

        Try

            Dim x As Integer = split(0)
            Dim y As Integer = split(1)

            If x < 0 Or y < 0 Or x >= board.GetLength(0) Or y >= board.GetLength(1) Then

                Console.WriteLine("Coordinate off the board")
                PromptCoordinate(message)
                Return

            End If

            Dim current As String = board(x, y)

            Select Case current

                Case "T"
                    Console.Clear()
                    Console.WriteLine($"Player {turn + 1}'s shot HIT at ({x},{y})")
                    board(x, y) = "H"

                Case " "
                    Console.Clear()
                    Console.WriteLine($"Player {turn + 1}'s shot MISSED at ({x},{y})")
                    board(x, y) = "M"

                Case "H"
                    Console.WriteLine($"Player {turn + 1} already hit there, fire at somewhere else.")
                    PromptCoordinate(message)

                Case "M"
                    Console.WriteLine($"Player {turn + 1} already missed there, fire at somewhere else.")
                    PromptCoordinate(message)

            End Select

        Catch ex As Exception

            Console.WriteLine("Invalid coordinate")
            PromptCoordinate(message)
            Return

        End Try

        PrintBoard(board, False)
        Console.WriteLine()

    End Sub

    Sub TanksProblem()

        InitializeBoard()
        NextTurn()

        InitializeBoard()
        NextTurn()

        While True
            PromptCoordinate($"Player {turn + 1}, enter a coordinate to fire a missile at")
            NextTurn()
        End While


    End Sub

    Sub NextTurn()
        turn = (turn + 1) Mod 2
        Console.WriteLine($"Player {turn + 1}'s turn")
    End Sub

    Dim currencies = New Dictionary(Of String, Decimal) From
    {
        {"USD", 1.33884},
        {"EUR", 1.17007},
        {"JPY", 152.632},
        {"INR", 99.6766}
    }

    Sub CurrencyConversionProblem()

        While True

            Dim rate As (String, Decimal) = PromptCurrencyRate("Enter a currency code")

            Dim amount As Decimal = PromptAmount("Enter an value to convert in GBP")

            Dim convertedAmount As Decimal = amount * rate.Item2

            Console.WriteLine($"{FormatCurrency(amount)} (GBP) = {FormatCurrency(convertedAmount)} ({rate.Item1})")

        End While

    End Sub

    Function PromptAmount(message As String) As Decimal

        Console.WriteLine(message)

        Try
            Dim amount As Decimal = Console.ReadLine
            Return amount

        Catch ex As Exception

            Console.WriteLine("Invalid amount")
            Return PromptAmount(message)

        End Try

    End Function

    Function PromptCurrencyRate(message As String) As (String, Decimal)

        Console.WriteLine(message)

        Try

            Dim code As String = Console.ReadLine().ToUpper

            Console.WriteLine(currencies(code))

            Dim rate As Decimal = currencies(code)

            Return (code, rate)

        Catch ex As Exception

            Console.WriteLine("Invalid currency code")
            Return PromptCurrencyRate(message)

        End Try

    End Function

    Sub TextDiceProblem()

        Dim r As New Random()

        Dim numbers As String() =
        {
            "one",
            "two",
            "three",
            "four",
            "five",
            "six"
        }

        Dim randomIndex As Integer = r.Next(6)
        Dim randomString As String = numbers(randomIndex)

        Console.WriteLine(randomString)

    End Sub

    Dim notes(9) As String

    Sub NotebookProblem()

        While True

            PrintNotes()
            Dim index As Integer = PromptIndex("Enter the index of a note to edit.")

            Dim note = PromptNote($"Enter content for note {index}")

            notes(index) = note

            Console.WriteLine()

        End While

    End Sub

    Sub PrintNotes()

        For i = 0 To notes.Length - 1

            Console.WriteLine($"{i}: {notes(i)}")

        Next

    End Sub

    Function PromptIndex(message As String) As Integer

        Console.WriteLine(message)

        Try

            Dim index As Integer = Console.ReadLine()

            If index < 0 Or index >= notes.Length Then
                Console.WriteLine("Index is out of range")
                Return PromptIndex(message)
            End If

            Return index

        Catch ex As Exception

            Console.WriteLine("Invalid index")
            Return PromptIndex(message)

        End Try


    End Function

    Function PromptNote(message As String) As String

        Console.WriteLine(message)

        Return Console.ReadLine()

    End Function

End Module
