Imports System

Module Stack
    Sub Main2(args As String())

        While True

            Dim read As String = Console.ReadLine

            If read.ToLower.StartsWith("push ") And read.Length > 2 Then
                Dim content As String = read.Substring(5)
                If Push(content) Then

                Else
                    Console.WriteLine("Stack overflow")
                End If

            ElseIf read.ToLower = "pop" Then
                Dim popped = Pop()
                If popped <> Nothing Then
                    Console.WriteLine(popped)
                Else
                    Console.WriteLine("Stack underflow")
                End If

            ElseIf read.ToLower = "show" Then
                Show()

            ElseIf read.ToLower = "size" Then
                Console.WriteLine(pointer)

            ElseIf read.ToLower = "clear" Then
                pointer = 0

            ElseIf read.ToLower = "exit" Then
                Console.WriteLine("Exiting")
                Exit While

            Else
                Console.WriteLine("Unknown command")

            End If
        End While

    End Sub

    Dim stack(4) As String
    Dim pointer

    Function Push(item As String) As Boolean

        If pointer >= stack.Length Then
            Return False
        End If

        stack(pointer) = item
        pointer += 1

        Return True

    End Function

    Function Pop() As String

        If pointer <= 0 Then

            Return Nothing

        End If
        pointer -= 1

        Dim item As String = stack(pointer)

        Return item

    End Function

    Sub Show()

        For i = 0 To pointer - 1
            Console.WriteLine(stack(i))
        Next

    End Sub

End Module
