Imports System

Module Program
    Sub Main(args As String())

        'RPGDiceProblem()
        'DivisibleProblem()
        'CashMachineProblem()
        'DiceGameProblem()
        'MonthProblem()

    End Sub

    Function PromptInteger(prompt As String) As Integer
        Console.WriteLine(prompt)
        Try

            Dim input As Integer = Console.ReadLine()

            Return input

        Catch ex As Exception

            Console.WriteLine("Invalid input")

            Return PromptInteger(prompt)

        End Try

    End Function

    Function RollDice(sides As Integer) As Integer

        Return New Random().Next(sides) + 1

    End Function

    Sub RPGDiceProblem()

        While True

            Dim input As Integer = PromptInteger("Enter a number of sides for the dice")

            If input <= 0 Then

                Console.WriteLine("Enter a number greater than 0")
                Continue While

            End If

            Dim roll = RollDice(input)

            Console.WriteLine("Rolling a {0} sided dice: {1}", input, roll)
            Console.WriteLine()

        End While

    End Sub

    Sub DivisibleProblem()

        While True


            Dim dividend As Integer = PromptInteger("Enter the dividend")

            Dim divisor As Integer = PromptInteger("Enter the divisor")

            Dim quotient As Integer = dividend / divisor

            Dim remainder As Integer = dividend Mod divisor

            Console.WriteLine("{0} ÷ {1} = {2} and {3}", dividend, divisor, quotient, remainder)
            Console.WriteLine()

        End While
    End Sub

    Sub CashMachineProblem()

        Dim balance As Integer = 231

        While True

            Console.WriteLine("Your current available balance is £{0}", balance)
            Dim withdrawAmount = PromptInteger("Enter an amount to withdraw")

            If withdrawAmount <= 0 Then
                Console.WriteLine("Invalid amount: Enter an amount greater than 0.")
                Continue While
            End If

            If withdrawAmount > 250 Then
                Console.WriteLine("Invalid amount: Maximum withdrawal limit is £250.")
                Continue While
            End If

            If withdrawAmount Mod 10 <> 0 Then
                Console.WriteLine("Invalid amount: Enter an amount which is a multiple of £10.")
                Continue While
            End If

            If withdrawAmount > balance Then
                Console.WriteLine("Invalid amount: You have insufficient funds.")
                Continue While
            End If

            Console.WriteLine("Dispensing £{0}", withdrawAmount)
            Console.WriteLine("")

            balance -= withdrawAmount

        End While

    End Sub

    Sub DiceGameProblem()

        Dim r1 As Integer = RollDice(6)
        Dim r2 As Integer = RollDice(6)
        Dim r3 As Integer = RollDice(6)

        Dim score As Integer

        If r1 = r2 = r3 Then

            score = r1 + r2 + r3

        ElseIf r1 = r2 Then

            score = r1 + r2 - r3

        ElseIf r2 = r3 Then

            score = r2 + r3 - r1

        ElseIf r1 = r3 Then

            score = r1 + r3 - r2

        Else

            score = 0

        End If

        Console.WriteLine(r1)
        Console.WriteLine(r2)
        Console.WriteLine(r3)

        Console.WriteLine(score)

    End Sub


    Public Months As String() = {
        "January",
        "February",
        "March",
        "April",
        "May",
        "June",
        "July",
        "August",
        "September",
        "October",
        "November",
        "December"
    }

    Function GetMonth(number As Integer) As String

        If number < 1 Or number > 12 Then
            Return Nothing
        End If

        Return Months(number - 1)

    End Function

    Function IsLeapYear(year As Integer) As Boolean

        If year Mod 400 = 0 Then
            Return True
        End If

        If year Mod 100 = 0 Then
            Return False
        End If

        If year Mod 4 = 0 Then
            Return True
        End If

        Return False

    End Function

    Function GetMonthLength(month As Integer, year As Integer) As Integer

        Select Case month
            Case 1 : Return 31  'January
            Case 2              'February
                If IsLeapYear(year) Then
                    Return 29
                Else
                    Return 28
                End If
            Case 3 : Return 31  'March
            Case 4 : Return 30  'April
            Case 5 : Return 31  'May
            Case 6 : Return 30  'June
            Case 7 : Return 31  'July
            Case 8 : Return 31  'August
            Case 9 : Return 30  'September
            Case 10 : Return 31 'October
            Case 11 : Return 30 'November
            Case 12 : Return 31 'December

            Case Else : Return 0

        End Select

    End Function

    Sub MonthProblem()

        Dim year As Integer = PromptInteger("Enter a year")

        Dim month As Integer = PromptInteger("Enter a month number")

        Dim monthName As String = GetMonth(month)
        Dim monthLength As Integer = GetMonthLength(month, year)

        Console.WriteLine("{0} has {1} days", monthName, monthLength)

    End Sub

End Module
