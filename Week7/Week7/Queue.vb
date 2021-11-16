Module Queue

    Dim queue(9) As String
    Dim empty As Integer = True
    Dim head, tail As Integer

    Sub Main3()

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

    Sub Clear()

        head = 0
        tail = 0
        empty = True

    End Sub

    Function Size() As Integer

        If tail = head Then

            If empty Then
                Return 0
            Else
                Return queue.Length
            End If

        End If

        If tail > head Then
            Return tail - head
        Else
            Return queue.Length - head + tail
        End If

    End Function

    Function IsFull() As Boolean

        Return Not empty And head = tail

    End Function

    Function IsEmpty() As Boolean

        Return empty

    End Function

    Function Enqueue(item As String) As Boolean

        If IsFull() Then
            Return False
        End If

        queue(tail) = item
        tail = (tail + 1) Mod queue.Length
        empty = False

        Return True

    End Function


    Function Dequeue() As String

        If IsEmpty() Then
            Return Nothing
        End If

        Dim item As String = queue(head)
        head = (head + 1) Mod queue.Length
        If head = tail Then
            empty = True
        End If

        Return item

    End Function

    Sub Show()

        Dim i As Integer = head

        Do

            Console.WriteLine(queue(i))
            i = (i + 1) Mod queue.Length

        Loop Until i = tail

    End Sub

End Module
