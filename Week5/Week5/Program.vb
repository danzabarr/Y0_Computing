Imports System

Module Program

    Sub Main()

        'VatProblem()
        'ConversionProblem()
        'DartsProblem()
        SnakeEyesProblem()

    End Sub

    Sub VatProblem()

        Dim price As Decimal = PromptPrice("Enter a price of an item")
        Dim excludingVat = price / 1.2
        Dim vat = price - price / 1.2

        Console.WriteLine("Price (exc VAT): " + Strings.FormatCurrency(excludingVat))
        Console.WriteLine("VAT:             " + Strings.FormatCurrency(vat))
        Console.WriteLine("Price (inc VAT): " + Strings.FormatCurrency(price))

    End Sub

    Function PromptPrice(prompt As String) As Decimal

        Console.WriteLine(prompt)

        Try
            Dim price As Decimal = Console.ReadLine()

            Return price

        Catch ex As Exception

            Return PromptPrice(prompt)

        End Try

    End Function

    Class Pair

        Public ReadOnly unit1 As String
        Public ReadOnly unit2 As String
        Public ReadOnly rate As Decimal

        Sub New(unit1 As String, unit2 As String, rate As String)
            Me.unit1 = unit1
            Me.unit2 = unit2
            Me.rate = rate
        End Sub

    End Class

    Sub ConversionProblem()

        Dim pairs() =
        {
            ("grams", "ounces", 0.035274),
            ("kilos", "pounds", 2.20462),
            ("kilos", "ounces", 35.27396),
            ("tonnes", "tons", 0.9842),
            ("tonnes", "tons", 157.47304)
        }

        For i = 0 To pairs.Length - 1

            Console.WriteLine($"({i * 2 + 0}) {pairs(i).Item1} to {pairs(i).Item2}")
            Console.WriteLine($"({i * 2 + 1}) {pairs(i).Item2} to {pairs(i).Item1}")
            Console.WriteLine()

        Next

        While True

            Try
                Dim index As Integer = Console.ReadLine
                Dim pair = pairs(index / 2)
                If index Mod 2 = 0 Then
                    DoConversion(pair.Item3, pair.Item1, pair.Item2)
                Else
                    DoConversion(1 / pair.Item3, pair.Item2, pair.Item1)
                End If

            Catch ex As Exception
                Console.WriteLine("Invalid selection")
            End Try

        End While

    End Sub

    Sub DoConversion(rate As Decimal, unit1 As String, unit2 As String)
        Dim a As Decimal = PromptWeight($"Enter {unit1}:")
        Dim b As Decimal = a * rate
        Console.WriteLine(b.ToString + $" {unit2}")
    End Sub

    Function PromptWeight(prompt As String) As Decimal

        Console.WriteLine(prompt)

        Try

            Dim weight As Decimal = Console.ReadLine()

            Return weight

        Catch ex As Exception

            Return PromptWeight(prompt)

        End Try

    End Function

    Dim dartsTurn, dartsP1Score, dartsP2Score As Integer

    Sub DartsProblem()

        NewDartsGame()

        While True

            Console.WriteLine($"Player {dartsTurn}'s turn")

            Dim score As Integer = PromptDartScore("Enter the total score of three darts.")

            Select Case dartsTurn
                Case 1
                    Dim resultingScore As Integer = dartsP1Score - score

                    If resultingScore > 0 Then
                        dartsP1Score = resultingScore
                    End If

                    PrintDartsScore()

                    If resultingScore = 0 Then
                        Console.WriteLine($"Player {dartsTurn} wins!")
                        Console.WriteLine()
                        NewDartsGame()
                        Continue While
                    End If

                Case 2
                    Dim resultingScore As Integer = dartsP2Score - score

                    If resultingScore > 0 Then
                        dartsP2Score = resultingScore
                    End If

                    PrintDartsScore()

                    If resultingScore = 0 Then
                        Console.WriteLine($"Player {dartsTurn} wins!")
                        Console.WriteLine()
                        NewDartsGame()
                        Continue While
                    End If

            End Select

            dartsTurn = dartsTurn Mod 2 + 1

        End While

    End Sub

    Sub NewDartsGame()
        Console.WriteLine("New Game")
        dartsP1Score = 501
        dartsP2Score = 501
        dartsTurn = 1
        PrintDartsScore()
    End Sub

    Sub PrintDartsScore()
        Console.WriteLine($"P1: {dartsP1Score}")
        Console.WriteLine($"P2: {dartsP2Score}")
    End Sub

    Function PromptDartScore(prompt As String) As Integer

        Console.WriteLine(prompt)

        Try

            Dim score As Integer = Console.ReadLine

            If score < 3 Or score > 180 Then

                Console.WriteLine("Invalid score")
                Return PromptDartScore(prompt)

            End If
            Return score

        Catch ex As Exception

            Console.WriteLine("Invalid score")
            Return PromptDartScore(prompt)

        End Try

    End Function

    Dim snakeEyesP1Score, snakeEyesP2Score As Integer
    Dim snakeEyesCurrentScore As Integer
    Dim snakeEyesTurn As Integer

    Sub SnakeEyesProblem()
        NewSnakeEyesGame()

        While True
            Console.WriteLine($"Player {snakeEyesTurn}'s Turn")

            While True

                Dim roll1 As Integer = RollDice(6)
                Dim roll2 As Integer = RollDice(6)
                Dim total As Integer = roll1 + roll2
                Console.WriteLine($"{roll1} + {roll2} = {total}")

                If roll1 = 1 And roll2 = 1 Then
                    Console.WriteLine("Snake eyes!")
                    snakeEyesCurrentScore = 0

                    Select Case snakeEyesTurn
                        Case 1
                            snakeEyesP1Score = 0
                        Case 2
                            snakeEyesP2Score = 0
                    End Select

                    PrintSnakeEyesScore()
                    Exit While

                ElseIf roll1 = 1 Or roll2 = 1 Then

                    Console.WriteLine("Your turn is over!")
                    snakeEyesCurrentScore = 0
                    PrintSnakeEyesScore()
                    Exit While

                End If

                snakeEyesCurrentScore += total

                PrintSnakeEyesScore()

                Console.WriteLine("(G)amble or (B)ank?")

                Select Case Console.ReadLine
                    Case "G"
                        Continue While

                    Case "B"
                        Select Case snakeEyesTurn
                            Case 1
                                snakeEyesP1Score += snakeEyesCurrentScore
                            Case 2
                                snakeEyesP2Score += snakeEyesCurrentScore
                        End Select
                        snakeEyesCurrentScore = 0
                        PrintSnakeEyesScore()
                        Exit While
                End Select

            End While

            snakeEyesTurn = snakeEyesTurn Mod 2 + 1
            snakeEyesCurrentScore = 0
            Console.WriteLine()
        End While
    End Sub

    Sub NewSnakeEyesGame()
        Console.WriteLine("New Game")
        snakeEyesTurn = 1
        snakeEyesP1Score = 0
        snakeEyesP2Score = 0
        snakeEyesCurrentScore = 0
    End Sub

    Sub PrintSnakeEyesScore()
        Console.WriteLine($"Running total: {snakeEyesCurrentScore}")
        Select Case snakeEyesTurn
            Case 1
                Console.WriteLine($"Player 1 Bank: {snakeEyesP1Score}")
            Case 2
                Console.WriteLine($"Player 2 Bank: {snakeEyesP2Score}")
        End Select
    End Sub

    Function RollDice(sides As Integer) As Integer
        Return New Random().Next(0, sides) + 1
    End Function

End Module
