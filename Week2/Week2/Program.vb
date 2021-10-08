Imports System

Module Program

    Sub Main()

        'CaseStatement()
        'AirlineTicket()
        'NameSeparator()
        'NitrateProblem()
        PeriodicTable()

    End Sub

    Sub CaseStatement()

        Console.WriteLine("Enter score")

        Dim input = Console.ReadLine()
        Dim output As String

        Select Case input
            Case < 50
                output = "Fail"
            Case < 60
                output = "Third"
            Case < 70
                output = "Second"
            Case < 80
                output = "Upper Second"
            Case Else
                output = "First"

        End Select

        Console.WriteLine(output)

    End Sub

    Sub AirlineTicket()

        Console.WriteLine("Enter the city departing from")
        Dim first As String = Console.ReadLine()

        Console.WriteLine("Enter the city travelling to")
        Dim second As String = Console.ReadLine()

        Dim n1 As String = Left(first, 4).ToUpper
        Dim n2 As String = Left(second, 4).ToUpper

        Dim output = n1 + "-" + n2

        Console.WriteLine(output)

    End Sub

    Sub NameSeparator()

        Console.WriteLine("Enter your full name")

        Dim input = Console.ReadLine()

        Dim split As String() = input.Split(" ")

        Console.WriteLine("Forename: " + split(0).ToString)
        Console.WriteLine("Surname:  " + split(1).ToString)

    End Sub

    Function PromptNitrate() As Decimal

        Console.WriteLine("Enter nitrate levels")

        Dim input As String = Console.ReadLine()

        Try
            Dim nitrate As Integer = input

            If nitrate < 1 Or nitrate > 50 Then
                Console.WriteLine("Value out of range, enter a value between 1 and 50 (inclusive)")
                Return PromptNitrate()
            End If

            Return nitrate

        Catch ex As Exception

            Console.WriteLine("Invalid input")
            Return PromptNitrate()

        End Try

        Return 0

    End Function

    Sub NitrateProblem()

        Dim nitrate = PromptNitrate()

        Dim dose As Decimal

        Select Case nitrate
            Case > 10
                dose = 3
            Case > 2.5
                dose = 2
            Case > 1
                dose = 1
            Case Else
                dose = 0.5

        End Select

        Console.WriteLine("Dose " + dose.ToString + "ml")

    End Sub

    Class Element

        Public ReadOnly Name As String
        Public ReadOnly Symbol As String
        Public ReadOnly AtomicWeight As Decimal

        Sub New(name As String, symbol As String, atomicWeight As Decimal)
            Me.Name = name
            Me.Symbol = symbol
            Me.AtomicWeight = atomicWeight
        End Sub

    End Class

    Sub PeriodicTable()
        Dim elements As New Dictionary(Of String, Element) From
        {
            {"li", New Element("Lithium", "Li", 6.941)},
            {"o", New Element("Oxygen", "O", 15.9994)},
            {"na", New Element("Sodium", "Na", 22.98977)},
            {"fe", New Element("Iron", "Fe", 55.847)}
        }

        Dim input = Console.ReadLine().Trim().ToLower()
        Dim output As Element

        If elements.TryGetValue(input, output) Then

            Console.WriteLine(output.Name)
            Console.WriteLine(output.AtomicWeight)

        End If

    End Sub

End Module
