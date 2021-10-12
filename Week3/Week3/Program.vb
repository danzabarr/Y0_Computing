Imports System

Module Program
    Sub Main(args As String())

        'SquareNumbers()
        'TimesTable()
        'GreenBottles()
        'MultiplicationTable()
        'Fibonacci()
        'MenuSelection()
        'CompoundInterest()


    End Sub

    Sub MenuSelection()
        Dim input As Integer

        Do
            Console.WriteLine("Enter 0 - 3")
            input = Console.ReadLine()

        Loop Until input >= 0 And input < 4


        Console.WriteLine("Lets go")

    End Sub

    Sub CompoundInterest()


        Dim p = 100
        Dim r = 0.04


        For i = 1 To 20
            Console.WriteLine(CalculateCompoundInterest(p, r, i))
        Next

    End Sub

    Function CalculateCompoundInterest(p As Decimal, r As Decimal, t As Decimal)

        Return p * Math.Pow(1 + r, t)

    End Function

    Sub SquareNumbers()

        For i = 1 To 20

            Console.WriteLine("{0} squared is {1}", i, i * i)

        Next

    End Sub

    Sub TimesTable()
        Dim input As Integer = Console.ReadLine()

        For i = 1 To 12

            Console.WriteLine("{0} x {1} = {2}", i, input, i * input)

        Next

    End Sub

    Sub GreenBottles()

        Dim input As Integer = Console.ReadLine()

        For i = input To 1 Step -1
            Console.WriteLine("{0} green bottles sitting on the wall", i)
        Next

    End Sub

    Sub MultiplicationTable()
        For i = 1 To 12
            For j = 1 To 12
                Console.Write((i * j).ToString.PadLeft(4))
            Next
            Console.WriteLine()
        Next
    End Sub

    Sub Fibonacci()

        Dim n1 As Integer = 0
        Dim n2 As Integer = 1
        For i = 1 To 20

            Console.WriteLine(n1)

            Dim t = n1
            n1 = n2
            n2 = t + n2

        Next

    End Sub


End Module
