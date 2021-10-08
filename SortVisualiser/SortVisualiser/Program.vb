
Imports System

Module Program

    Public Const Length As Integer = 50
    Private List As List(Of Integer)
    Public Const FilledChar As Char = "#"
    Public Const AnimationIntervalMillis = 1
    Public Const Update = AnimationIntervalMillis > 0

    Sub Main()

        InitialiseList()

        List = Shuffle(List)
        DrawList()

        'SelectionSort()
        'QuickSort()
        'InsertionSort()
        'BubbleSort()
        'MergeSort()
        'HeapSort()

        Console.SetCursorPosition(0, Length)
    End Sub

    Public Delegate Sub Sort()
    Public Sub TimeSort(sort As Sort)
        Dim timer As New Stopwatch()
        timer.Start()
        sort()
        timer.Stop()
        Console.WriteLine(sort.Method.Name + ": " + timer.ElapsedTicks.ToString)
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

    Sub Sleep()
        If AnimationIntervalMillis > 0 Then
            Threading.Thread.Sleep(AnimationIntervalMillis)
        End If
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
        If Update Then UpdateColumn(column)
    End Sub

    Sub InsertionSort()
        Dim i, j, value As Integer

        For i = 1 To Length - 1
            value = List(i)
            j = i - 1

            While List(j) > value
                SetValue(j + 1, List(j))
                Sleep()

                j -= 1

                If j < 0 Then
                    Exit While
                End If

            End While

            SetValue(j + 1, value)
            Sleep()
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
                    Sleep()
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
                Sleep()

                i += 1
            Else
                SetValue(k, arrayR(j))
                Sleep()

                j += 1

            End If
            k += 1

        End While

        'Copy remaining elements
        'of L[] if any
        While i < n1
            SetValue(k, arrayL(i))
            Sleep()
            i += 1
            k += 1
        End While

        ' Copy remaining elements
        ' of R[] if any
        While j < n2
            SetValue(k, arrayR(j))
            Sleep()
            j += 1
            k += 1
        End While

    End Sub

    Sub MergeSort()
        MergeSort(0, Length - 1)
    End Sub

    Sub MergeSort(l As Integer, r As Integer)

        If l < r Then
            ' Find the middle
            ' point
            Dim m As Integer = Int(l + ((r - l) / 2))

            ' Sort first And
            ' second halves
            MergeSort(l, m)
            MergeSort(m + 1, r)

            Merge(l, m, r)
        End If
    End Sub

    Public Sub QuickSort()
        QuickSort(0, Length - 1)
    End Sub

    Public Sub QuickSort(ByVal first As Long, ByVal last As Long)

        Dim lo As Integer = first
        Dim hi As Integer = last
        Dim mid As Integer = List((first + last) \ 2)

        Do

            'Find the index of the first value after "Low" which is
            'greater or equal to the mid value
            While List(lo) < mid
                lo += 1
            End While

            'Find the index of the last value before "High" which is
            'less or equal to the mid value
            While List(hi) > mid
                hi -= 1
            End While

            'If the index "Low" is less or equal than "High"
            'then the value at "Low" is higher than the midpoint,
            'and the value at "High" is lower than the midpoint,
            'and therefore should be swapped.
            If lo <= hi Then
                Swap(lo, hi)
                Sleep()
                lo += 1
                hi -= 1
            End If

        Loop While lo <= hi

        If first < hi Then QuickSort(first, hi)
        If lo < last Then QuickSort(lo, last)
    End Sub

    ''' <summary>
    ''' Finds the smallest item in the list and places it at the start,
    ''' then the next smallest and places it at the next position, etc.
    ''' O(n^2)
    ''' </summary>
    Sub SelectionSort()
        Dim min As Integer

        For i As Integer = 0 To Length - 2
            min = i

            For j As Integer = i + 1 To Length - 1
                If List(j) < List(min) Then
                    min = j
                End If
            Next

            Dim temp As Integer = List(min)

            SetValue(min, List(i))
            SetValue(i, temp)
            Sleep()

        Next
    End Sub

    Sub HeapSort()
        Dim heapSize As Integer = Length

        For p As Integer = (heapSize - 1) \ 2 To 0 Step -1
            MaxHeapify(heapSize, p)
        Next

        For i As Integer = Length - 1 To 1 Step -1
            Dim temp As Integer = List(i)
            SetValue(i, List(0))
            SetValue(0, temp)
            Sleep()

            heapSize -= 1
            MaxHeapify(heapSize, 0)
        Next
    End Sub

    Sub MaxHeapify(heapSize As Integer, index As Integer)
        Dim l As Integer = (index + 1) * 2 - 1
        Dim r As Integer = (index + 1) * 2
        Dim largest As Integer

        If l < heapSize AndAlso List(l) > List(index) Then
            largest = l
        Else
            largest = index
        End If

        If r < heapSize AndAlso List(r) > List(largest) Then
            largest = r
        End If

        If largest <> index Then
            Dim temp As Integer = List(index)
            SetValue(index, List(largest))
            SetValue(largest, temp)
            Sleep()

            MaxHeapify(heapSize, largest)
        End If
    End Sub

End Module
