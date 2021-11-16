Module Collections

    MustInherit Class Collection(Of T)
        Implements IEnumerable(Of T)

        Private Protected array As T()

        Sub New(capacity As Integer)

            array = New T(capacity - 1) {}

        End Sub

        Function Capacity() As Integer

            Return array.Length

        End Function

        MustOverride Function Size() As Integer
        MustOverride Function IsEmpty() As Boolean
        MustOverride Function IsFull() As Boolean
        MustOverride Sub Clear()
        Public MustOverride Iterator Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
        Public MustOverride Iterator Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Default Public MustOverride ReadOnly Property Accessor(index As Integer) As T

    End Class

    Class Queue(Of T)
        Inherits Collection(Of T)

        Private empty As Boolean
        Private head As Integer
        Private tail As Integer

        Public Sub New(capacity As Integer)

            MyBase.New(capacity)
            empty = True

        End Sub

        Public Overrides Sub Clear()

            head = 0
            tail = 0
            empty = True

        End Sub

        Public Overrides Function Size() As Integer

            If tail = head Then

                If empty Then
                    Return 0
                Else
                    Return array.length
                End If

            End If

            If tail > head Then
                Return tail - head
            Else
                Return array.Length - head + tail
            End If

        End Function

        Public Overrides Function IsFull() As Boolean

            Return Not empty And head = tail

        End Function

        Public Overrides Function IsEmpty() As Boolean

            Return empty

        End Function

        Public Function Enqueue(item As T) As Boolean

            If IsFull() Then
                Return False
            End If

            array(tail) = item
            empty = False

            tail = (tail + 1) Mod array.Length

            Return True

        End Function

        Function Dequeue() As T

            If IsEmpty() Then
                Return Nothing
            End If

            Dim item As T = array(head)
            head = (head + 1) Mod array.Length

            If head = tail Then
                empty = True
            End If

            Return item

        End Function

        Public Overrides Iterator Function GetEnumerator() As IEnumerator(Of T)

            Dim i As Integer = head

            Do

                Yield array(i)
                i = (i + 1) Mod array.Length

            Loop Until i = tail

        End Function

        Public Overrides Iterator Function IEnumerable_GetEnumerator() As IEnumerator

            Yield GetEnumerator()

        End Function

        Default Public Overrides ReadOnly Property Accessor(index As Integer) As T

            Get

                If empty Then
                    Throw New IndexOutOfRangeException
                End If

                If tail < head Then
                    If index < head And index >= tail Then
                        Throw New IndexOutOfRangeException
                    End If
                End If

                If head < tail Then
                    If index < head Or index >= tail Then
                        Throw New IndexOutOfRangeException
                    End If
                End If

                Return array(index)

            End Get

        End Property

    End Class

    Class Stack(Of T)
        Inherits Collection(Of T)

        Private pointer As Integer

        Public Sub New(capacity As Integer)
            MyBase.New(capacity)
        End Sub

        Default Public Overrides ReadOnly Property Accessor(index As Integer) As T
            Get
                If index < 0 Or index >= pointer Then
                    Throw New IndexOutOfRangeException
                End If

                Return array(index)

            End Get
        End Property

        Public Function Push(item As T) As Boolean

            If pointer >= array.Length Then
                Return False
            End If

            array(pointer) = item
            pointer += 1

            Return True

        End Function

        Public Function Pop() As T

            If pointer <= 0 Then

                Return Nothing

            End If

            pointer -= 1

            Return array(pointer)

        End Function

        Public Overrides Sub Clear()

            pointer = 0

        End Sub

        Public Overrides Function Size() As Integer

            Return pointer

        End Function

        Public Overrides Function IsEmpty() As Boolean

            Return pointer = 0

        End Function

        Public Overrides Function IsFull() As Boolean

            Return pointer = array.Length

        End Function

        Public Overrides Iterator Function GetEnumerator() As IEnumerator(Of T)

            For i = 0 To pointer - 1
                Yield array(i)
            Next

        End Function

        Public Overrides Iterator Function IEnumerable_GetEnumerator() As IEnumerator

            Yield GetEnumerator()

        End Function
    End Class

End Module
