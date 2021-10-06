
Imports System

Module Program

    Public Const Length As Integer = 30
    Private List As List(Of Integer)
    Public Const FilledChar As Char = "#"
    Public Const AnimationIntervalMillis = 50

    Sub Main()

        InitialiseList()

        List = Shuffle(List)
        DrawList()

        'InsertionSort()
        'BubbleSort()
        'MergeSort(0, Length - 1, 0)

        Console.SetCursorPosition(0, Length)

    End Sub

    Private Sub InitialiseList()

        List = New List(Of Integer)

        For i = 0 To Length - 1

            List.Add(i)

        Next

    End Sub

    Function Shuffle(Of T)(collection As IEnumerable(Of T)) As List(Of T)
        Dim r As New Random()
        Shuffle = collection.OrderBy(Function(a) r.Next()).ToList()
    End Function

    Sub DrawList()

        For x = 0 To Length - 1
            DrawColumn(x, List(x))
        Next

    End Sub



    Sub DrawColumn(x As Integer, value As Integer)

        For y = 0 To Length - 1

            Console.SetCursorPosition(x, Length - y - 1)

            If y < value Then
                Console.Write(FilledChar)
            Else
                Console.Write(" ")
            End If

        Next

    End Sub

    Sub UpdateColumn(x As Integer)

        DrawColumn(x, List(x))

    End Sub

    Sub SetValue(column As Integer, value As Integer)
        List(column) = value
        UpdateColumn(column)
    End Sub

    Sub InsertionSort()
        Dim i, j, value As Integer

        For i = 1 To Length - 1
            value = List(i)
            j = i - 1

            While List(j) > value
                SetValue(j + 1, List(j))
                Threading.Thread.Sleep(AnimationIntervalMillis)

                j -= 1

                If j < 0 Then
                    Exit While
                End If

            End While

            SetValue(j + 1, value)
            Threading.Thread.Sleep(AnimationIntervalMillis)
        Next

    End Sub

    Sub Swap(i As Integer, j As Integer)
        Dim temp As Integer = List(i)
        SetValue(i, List(j))
        SetValue(j, temp)
    End Sub

    Sub BubbleSort()
        Dim i, j As Integer
        For i = 0 To Length - 2
            For j = 0 To Length - i - 2
                If List(j) > List(j + 1) Then
                    Swap(j, j + 1)
                    Threading.Thread.Sleep(AnimationIntervalMillis)
                End If
            Next
        Next
    End Sub

    Sub Merge(l As Integer, m As Integer, r As Integer)
        Dim n1 As Integer = m - l + 1
        Dim n2 As Integer = r - m

        Dim arrayL(n1 - 1) As Integer
        Dim arrayR(n2 - 1) As Integer
        Dim i, j As Integer

        'Copy data to temp arrays
        For i = 0 To n1 - 1
            arrayL(i) = List(l + i)
        Next

        For j = 0 To n2 - 1
            arrayR(j) = List(m + 1 + j)
        Next

        'Merge the temp arrays

        'Initial indexes of first
        'And second subarrays
        i = 0
        j = 0

        ' Initial index of merged
        ' subarray array
        Dim k As Integer = l
        While i < n1 And j < n2
            If arrayL(i) <= arrayR(j) Then
                SetValue(k, arrayL(i))
                Threading.Thread.Sleep(AnimationIntervalMillis)

                i += 1
            Else
                SetValue(k, arrayR(j))
                Threading.Thread.Sleep(AnimationIntervalMillis)

                j += 1

            End If
            k += 1

        End While

        'Copy remaining elements
        'of L[] if any
        While i < n1
            SetValue(k, arrayL(i))
            Threading.Thread.Sleep(AnimationIntervalMillis)
            i += 1
            k += 1
        End While

        ' Copy remaining elements
        ' of R[] if any
        While j < n2
            SetValue(k, arrayR(j))
            Threading.Thread.Sleep(AnimationIntervalMillis)
            j += 1
            k += 1
        End While

    End Sub

    ' Main function that
    ' sorts arr[l..r] using
    ' merge()
    Sub MergeSort(l As Integer, r As Integer, depth As Integer)

        If l < r Then
            ' Find the middle
            ' point
            Dim m As Integer = Int(l + ((r - l) / 2))

            ' Sort first And
            ' second halves
            depth += 1
            MergeSort(l, m, depth)
            MergeSort(m + 1, r, depth)

            Merge(l, m, r)
        End If
    End Sub

End Module
