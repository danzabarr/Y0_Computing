Public Module Chat

    Public Const HostName As String = "127.0.0.1" '"2.27.89.249"
    Public Const Port As Integer = 8888

    ''' <summary>
    ''' Converts an array of bytes to ASCII encoded string, removing any trailing null characters.
    ''' </summary>
    ''' <param name="bytes">
    ''' Array of bytes to convert.
    ''' </param>
    ''' <returns>
    ''' String representation of the array of bytes.
    ''' </returns>
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

    Function ParseCommand(ByRef input As String) As String

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

    Function ParseCommandArgs(input As String) As String

        Return ParseCommandArgs(input, 0, True)(0)

    End Function

    Function ParseCommandArgs(input As String, argCount As Integer) As String()

        Return ParseCommandArgs(input, argCount, False)

    End Function

    Function ParseCommandArgs(input As String, argCount As Integer, trailingArg As Boolean) As String()

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

End Module
