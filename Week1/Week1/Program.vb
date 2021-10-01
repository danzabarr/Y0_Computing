''' <summary>
''' See also Prompter.vb
''' </summary>
Module Program
    Sub Main(args As String())
        'SimpleAdder()
        'TestMarks()
        'TemperatureConverter()
        'HeightAndWeight()
        CircleProperties()
    End Sub

    'SIMPLE ADDER
    Sub SimpleAdder()

        Dim first = PromptDecimal("Enter the first number")
        Dim second = PromptDecimal("Enter the second number")
        Dim sum As Decimal = first + second

        Console.WriteLine("Answer: " + sum)

    End Sub

    'TEST MARKS
    Sub TestMarks()

        Dim mark1 = PromptMark("Enter the first mark out of 100")
        Dim mark2 = PromptMark("Enter the second mark out of 100")
        Dim mark3 = PromptMark("Enter the third mark out of 100")

        Dim sum = mark1 + mark2 + mark3

        Dim average As Decimal = sum / 3

        Console.WriteLine("The total mark was: " + sum.ToString)
        Console.WriteLine("The mark on average was: " + average.ToString)

    End Sub

    'TEMPERATURE CONVERTER
    Sub TemperatureConverter()

        Dim input = PromptDecimal("Enter degrees farenheit:")
        Dim centigrade = ToCentigrade(input)

        Console.WriteLine("Degrees centigrade: " + centigrade.ToString)

    End Sub

    Function ToCentigrade(farenheit As Decimal) As Decimal
        Return (farenheit - 32) * (5 / 9)
    End Function


    'HEIGHT AND WEIGHT
    Sub HeightAndWeight()

        Dim heightInInches = PromptDecimal("Enter your height in inches")
        Dim heightInCentimeters = InchesToCentimeters(heightInInches)
        Console.WriteLine("Your height in centimeters is " + heightInCentimeters.ToString + "cm")

        Dim weightInStones = PromptDecimal("Enter your weight in stones")
        Dim weightInKilos = StoneToKilos(weightInStones)
        Console.WriteLine("Your weight in kilos is " + weightInKilos.ToString + "kg")

    End Sub

    Function InchesToCentimeters(inches As Decimal) As Decimal
        Return inches * 2.54
    End Function

    Function StoneToKilos(stone As Decimal) As Decimal
        Return stone * 6.364
    End Function

    Sub CircleProperties()
        Dim diameter = PromptDecimal("Enter the diameter of the circle")
        Dim arcAngle = PromptAngleDegrees("Enter the arc angle in degrees")

        Dim radius As Decimal = diameter / 2
        Dim area As Decimal = Math.PI * radius * radius
        Dim circ As Decimal = Math.PI * diameter
        Dim arcLength As Decimal = circ * (arcAngle / 360)

        Console.WriteLine("-------------------------------------")

        Console.WriteLine("Radius:         " + radius.ToString)
        Console.WriteLine("Diameter:       " + diameter.ToString)
        Console.WriteLine("Area:           " + area.ToString)
        Console.WriteLine("Circumference:  " + circ.ToString)
        Console.WriteLine("Arc Angle    :  " + arcAngle.ToString)
        Console.WriteLine("Arc Length   :  " + arcLength.ToString)


    End Sub

    Sub Dice()

        Console.WriteLine("00000000000000000")
        Console.WriteLine("0               0")
        Console.WriteLine("0   #       #   0")
        Console.WriteLine("0               0")
        Console.WriteLine("0       #       0")
        Console.WriteLine("0               0")
        Console.WriteLine("0   #       #   0")
        Console.WriteLine("0               0")
        Console.WriteLine("00000000000000000")

    End Sub

    Sub Objective1Task()
        'Start of message
        Console.WriteLine("Hello World" & " this is really not my first program.")
        'Blank line
        Console.WriteLine()
        'End of message
        Console.WriteLine("I am learning to code...")
        Console.WriteLine("...and it is fun")
        'Wait for user to exit
        Console.ReadLine()

    End Sub
End Module
