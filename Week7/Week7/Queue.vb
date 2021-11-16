Module Queue

    Dim queue(9) As String
    Dim head, tail As Integer

    Sub Main()

        While True

            Dim read As String = Console.ReadLine

            If read.ToLower.StartsWith("enqueue ") And read.Length > 2 Then

                Dim content As String = read.Substring(8)
                If Enqueue(content) Then

                Else
                    Console.WriteLine("Queue full")
                End If

            ElseIf read.ToLower = "dequeue" Then

                Dim dequeued = Dequeue()
                If dequeued <> Nothing Then
                    Console.WriteLine(dequeued)
                Else
                    Console.WriteLine("Queue empty")
                End If

            ElseIf read.ToLower = "show" Then
                Show()

            ElseIf read.ToLower = "size" Then
                Console.WriteLine(Size())

            ElseIf read.ToLower = "clear" Then
                Clear()

            ElseIf read.ToLower = "exit" Then
                Console.WriteLine("Exiting")
                Exit While

            Else
                Console.WriteLine("Unknown command")

            End If

        End While

    End Sub

    Function Clear()

        head = 0
        tail = 0

    End Function

    Function Size()

        If tail > head Then
            Return tail - head
        Else
            Return queue.Length - head + tail
        End If

    End Function

    Function IsFull()

        Return (tail + 1) Mod queue.Length = head

    End Function

    Function IsEmpty()

        Return head = tail

    End Function

    Function Enqueue(item As String) As Boolean

        If IsFull() Then
            Return False
        End If

        queue(tail) = item
        tail = (tail + 1) Mod queue.Length

        Return True

    End Function


    Function Dequeue() As String

        If IsEmpty() Then
            Return Nothing
        End If

        Dim item As String = queue(head)
        head = (head + 1) Mod queue.Length

        Return item

    End Function

    Sub Show()

        Dim i As Integer = head

        While i <> tail

            Console.WriteLine(queue(i))
            i = (i + 1) Mod queue.Length

        End While

    End Sub

End Module
