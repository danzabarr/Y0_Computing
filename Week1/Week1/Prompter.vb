''' <summary>
''' Library for prompting the user for input via the console.
''' Standardises type parsing and input validation.
''' 
''' </summary>
Module Prompter

    'PROMPT FNS

    ''' <summary>
    ''' Prompts for a date, i.e. '01/10/2021 17:16:42'
    ''' </summary>
    Function PromptDateTime(msg As String) As Date
        Return Prompt(msg, AddressOf ParseDateTime, AddressOf ValidateAlways)
    End Function

    ''' <summary>
    ''' Prompts for a decimal value.
    ''' </summary>
    Function PromptDecimal(msg As String) As Decimal
        Return Prompt(msg, AddressOf ParseDecimal, AddressOf ValidateAlways)
    End Function

    ''' <summary>
    ''' Prompts for a decimal value in the range 0-360
    ''' </summary>
    Function PromptAngleDegrees(msg As String) As Decimal
        Return Prompt(msg, AddressOf ParseDecimal, AddressOf ValidateAngleDegrees)
    End Function


    ''' <summary>
    ''' Prompts for an integer in the range 0-100
    ''' </summary>
    Function PromptMark(msg As String) As Integer
        Return Prompt(msg, AddressOf ParseInteger, AddressOf ValidateMark)
    End Function


    'VALIDATION FNS
    ''' <summary>
    ''' Don't validate input.
    ''' </summary>
    Function ValidateAlways(input As String) As Boolean
        Return True
    End Function

    ''' <summary>
    ''' Validate if the value is between 0-100 (inclusive)
    ''' </summary>
    Function ValidateMark(mark As Integer) As Boolean
        Return mark >= 0 And mark <= 100
    End Function

    ''' <summary>
    ''' Validate if the value is between 0-360 (inclusive)
    ''' </summary>
    Function ValidateAngleDegrees(input As Decimal) As Boolean
        Return input >= 0 And input <= 360
    End Function


    'PARSE FNS
    Function ParseDateTime(input As String) As Date
        Return Date.Parse(input)
    End Function

    Function ParseInteger(input As String) As Integer
        Return input
    End Function

    Function ParseDecimal(input As String) As Decimal
        Return input
    End Function



    'DELEGATE FNS

    Delegate Function Parse(Of t)(ByVal input As String) As t

    Delegate Function Validate(Of t)(ByVal input As t) As Boolean



    'PROMPT MAIN
    Private Function Prompt(Of t)(promptMessage As String, parser As Parse(Of t), validation As Validate(Of t)) As t
        Console.WriteLine(promptMessage)
        Dim input As String = Console.ReadLine()

        Dim inputT As t

        Try

            inputT = parser(input)

        Catch ex As Exception

            Console.WriteLine("Failed to parse input")
            Return Prompt(promptMessage, parser, validation)

        End Try

        If Not validation(inputT) Then

            Console.WriteLine("Invalid input")
            Return Prompt(promptMessage, parser, validation)

        End If

        Return inputT

    End Function

End Module
