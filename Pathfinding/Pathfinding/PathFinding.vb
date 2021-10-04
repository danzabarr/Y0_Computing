Imports System.Drawing

Module PathFinding
    Enum PathResult
        Success
        AtDestination
        FailureNoStart
        FailureNoEnd
        FailureNoPath
        FailureTooManyTries
        FailureTooFar
    End Enum

    Public neighbourOffsets As (Point, Decimal)() = {
        (New Point(-1, 0), 1),
        (New Point(0, -1), 1),
        (New Point(+1, 0), 1),
        (New Point(0, +1), 1)
    }

    Public Class Node

        Public position As Point
        Public pathDistance, pathCost, pathCrowFliesDistance As Integer
        Public pathSteps, pathTurns, pathEndDirection As Integer
        Public pathParent As Node
        Public cost As Decimal
        Public untraversable As Boolean

        Public Overrides Function ToString() As String

            Return position.ToString

        End Function


        Sub New(position As Point, untraversable As Boolean, cost As Decimal)
            Me.position = position
            Me.untraversable = untraversable
            Me.cost = cost
        End Sub

        Sub ClearPathingData()
            pathDistance = 0
            pathCost = 0
            pathCrowFliesDistance = 0
            pathSteps = 0
            pathTurns = 0
            pathEndDirection = 0
            pathParent = Nothing
        End Sub

    End Class



    Public Class Path

        Public ReadOnly result As PathResult
        Public ReadOnly nodes As List(Of Node)
        Public ReadOnly distance, cost, crowFliesDistance As Decimal
        Public ReadOnly steps, turns As Integer
        Public ReadOnly Property StartNode As Node
            Get
                If nodes Is Nothing Then
                    Return Nothing

                ElseIf nodes.Count <= 0 Then
                    Return Nothing

                End If
                Return nodes(0)
            End Get
        End Property

        Public ReadOnly Property EndNode As Node
            Get
                If nodes Is Nothing Then
                    Return Nothing

                ElseIf nodes.Count <= 0 Then
                    Return Nothing

                End If
                Return nodes(nodes.Count - 1)
            End Get
        End Property

        Public ReadOnly Property Length As Integer
            Get
                If nodes Is Nothing Then
                    Return 0
                End If
                Return nodes.Count
            End Get
        End Property

        ''' <summary>
        ''' Constructor for a failed path.
        ''' </summary>
        Private Sub New(result As PathResult)
            Me.result = result
        End Sub

        ''' <summary>
        ''' Constructor for a successful path.
        ''' </summary>
        Private Sub New(nodes As List(Of Node), distance As Decimal, crowFliesDistance As Decimal, cost As Decimal, steps As Integer, turns As Integer)
            result = PathResult.Success
            Me.nodes = nodes
            Me.distance = distance
            Me.crowFliesDistance = crowFliesDistance
            Me.cost = cost
            Me.steps = steps
            Me.turns = turns
        End Sub

        ''' <summary>
        ''' Writes details about the path to the console.
        ''' </summary>
        Public Sub PrintDetails()

            Console.WriteLine("Result:              " + result.ToString)
            Console.WriteLine(String.Concat("Start:               ", StartNode))
            Console.WriteLine(String.Concat("End:                 ", EndNode))
            Console.WriteLine("Euclidean Distance: " + crowFliesDistance.ToString)
            Console.WriteLine("Path Distance:            " + distance.ToString)
            Console.WriteLine("Cost:                " + cost.ToString)
            Console.WriteLine("Steps:               " + steps.ToString)
            Console.WriteLine("Turns:               " + turns.ToString)

        End Sub
        Public Shared Function FindPath(ByRef startNode As Node, ByRef endNode As Node, maxDistance As Decimal, maxTries As Integer, costFunction As CostFunction, getNode As GetNode) As Path

            'Check start node exists
            If startNode Is Nothing Then

                Return New Path(PathResult.FailureNoStart)

            End If

            'Check end node exists
            If endNode Is Nothing Then

                Return New Path(PathResult.FailureNoEnd)

            End If

            'Check end node is traversable
            If endNode.untraversable Then

                Return New Path(PathResult.FailureNoPath)

            End If

            'Quick return if start equals end
            If startNode Is endNode Then

                Return New Path(PathResult.AtDestination)

            End If

            'Check the distance between start end is less than the max
            Dim d As Decimal = PathFinding.Distance(startNode, endNode)

            If d > maxDistance Then

                Return New Path(PathResult.FailureTooFar)

            End If

            'Initialise empty lists
            Dim visited As New List(Of Node)
            Dim open As New List(Of Node)
            Dim closed As New List(Of Node)

            'Set start node crow flies distance
            startNode.pathCrowFliesDistance = d

            'Add the start node to the open and visited lists
            open.Add(startNode)
            visited.Add(startNode)


            Console.ForegroundColor = ConsoleColor.Yellow

            'Initialise tries counter to 0
            Dim tries As Integer = 0

            'Begin loop
            While True

                'Increment tries
                tries += 1

                'If tries is greater than the max, return failure
                If tries > maxTries Then

                    For Each node As Node In visited
                        node.ClearPathingData()
                    Next

                    Return New Path(PathResult.FailureTooManyTries)

                End If

                'If open list is empty, return failure
                If open.Count = 0 Then
                    For Each node As Node In visited
                        node.ClearPathingData()
                    Next

                    Return New Path(PathResult.FailureNoPath)

                End If

                'Find the node in the open list with the best heuristic score
                Dim currentNode As Node = Nothing
                Dim currentCost As Decimal = Decimal.MaxValue

                For Each node As Node In open
                    Dim nodeCost = costFunction(node.pathDistance, node.pathCost, node.pathCrowFliesDistance, node.pathSteps, node.pathTurns)
                    If nodeCost < currentCost Then
                        currentCost = nodeCost
                        currentNode = node
                    End If
                Next

                'If the current node is equal to the end node, exit the loop; the search is complete
                If currentNode Is endNode Then

                    Exit While

                End If

                'If the path to the current node is greater than the max, return failure
                If currentNode.pathDistance > maxDistance Then

                    For Each node As Node In visited
                        node.ClearPathingData()
                    Next

                    Return New Path(PathResult.FailureTooFar)

                End If

                'Remove the current node from the open list, add it to the closed list
                open.Remove(currentNode)
                closed.Add(currentNode)

                'For each of the neighbour offsets
                For i = 0 To neighbourOffsets.Length - 1

                    'Determine the coordinates of the neighbour
                    Dim offset As (Point, Decimal) = neighbourOffsets(i)
                    Dim nX = offset.Item1.X + currentNode.position.X
                    Dim nY = offset.Item1.Y + currentNode.position.Y

                    'Access the neighbour node from the lookup function
                    Dim neighbour As Node = getNode(nX, nY)

                    'Continue if the node is null
                    If neighbour Is Nothing Then

                        Continue For

                    End If

                    'Continue if the node is untraversable
                    If neighbour.untraversable Then

                        Continue For

                    End If

                    '
                    Dim neighbourDistance As Decimal = offset.Item2

                    'Calculate the path stats for the current neighbour
                    Dim nextPathDistance As Decimal = currentNode.pathDistance + neighbourDistance
                    Dim nextPathCrowFliesDistance As Decimal = PathFinding.Distance(neighbour, endNode)
                    Dim nextPathCost As Decimal = currentNode.pathCost + neighbour.cost
                    Dim nextPathSteps As Integer = currentNode.pathSteps + 1
                    Dim nextPathTurns As Integer = currentNode.pathTurns + If(currentNode.pathSteps = 0 Or currentNode.pathEndDirection = i, 0, 1)

                    'Calculate the total cost using the cost function
                    Dim nextTotalCost As Decimal = costFunction(nextPathDistance, nextPathCost, nextPathCrowFliesDistance, nextPathSteps, nextPathTurns)

                    'If the total cost of the neighbour node is less than the current one,
                    'remove it from the open and closed lists
                    If nextTotalCost < costFunction(neighbour.pathDistance, neighbour.pathCost, neighbour.pathCrowFliesDistance, neighbour.pathSteps, neighbour.pathTurns) Then

                        open.Remove(neighbour)
                        closed.Remove(neighbour)

                    End If

                    'If the node is neither in the open list nor the closed list,
                    'add it to the open list and update the path stats.
                    If Not open.Contains(neighbour) And Not closed.Contains(neighbour) Then

                        neighbour.pathDistance = nextPathDistance
                        neighbour.pathCrowFliesDistance = nextPathCrowFliesDistance
                        neighbour.pathCost = nextPathCost
                        neighbour.pathSteps = nextPathSteps
                        neighbour.pathTurns = nextPathTurns

                        neighbour.pathParent = currentNode
                        neighbour.pathEndDirection = i

                        open.Add(neighbour)

                        'If the node has not already been visited, add to the visited list and write to the console.
                        If Not visited.Contains(neighbour) Then

                            visited.Add(neighbour)
                            If neighbour IsNot startNode And neighbour IsNot endNode Then
                                Console.SetCursorPosition(neighbour.position.X, neighbour.position.Y)
                                Console.Write(".")

                            End If

                        End If

                    End If

                Next

            End While


            'If the function has not returned by now, a valid path to the end node was found

            'Initialise a list of nodes for the path, and a current node
            Dim nodes As New List(Of Node)
            Dim current As Node = endNode

            'Loop back from the end node via parents of parents and insert at the start of the list.
            While Not current.pathParent Is Nothing

                nodes.Insert(0, current)
                current = current.pathParent

            End While

            'Finally insert the start node.
            nodes.Insert(0, startNode)

            'Construct a path object to return.
            Dim result As New Path(nodes, endNode.pathDistance, PathFinding.Distance(startNode, endNode), endNode.cost, endNode.pathSteps, endNode.pathTurns)

            'Clear all the pathing data for the visited nodes.
            For Each node As Node In visited
                node.ClearPathingData()
            Next

            'Return the result.
            Return result

        End Function
    End Class

    ''' <summary>
    ''' Delegate for calculating the total cost of a path given its stats.
    ''' </summary>
    Public Delegate Function CostFunction(distance As Decimal, cost As Decimal, crowFliesDistance As Decimal, steps As Integer, turns As Integer) As Decimal

    ''' <summary>
    ''' Delegate for retrieving a node at a given coordinate.
    ''' </summary>
    Public Delegate Function GetNode(x As Integer, y As Integer) As Node

    Public Function StandardCostFunction(distance As Decimal, cost As Decimal, crowFliesDistance As Decimal, steps As Integer, turns As Integer) As Decimal

        Return distance + cost + crowFliesDistance + turns

    End Function


    ''' <summary>
    ''' Returns the distance between two points.
    ''' </summary>
    Public Function Distance(x1 As Decimal, y1 As Decimal, x2 As Decimal, y2 As Decimal) As Decimal

        Return Math.Sqrt(SquareDistance(x1, y1, x2, y2))

    End Function

    ''' <summary>
    ''' Returns the square distance between two points.
    ''' </summary>
    Public Function SquareDistance(x1 As Decimal, y1 As Decimal, x2 As Decimal, y2 As Decimal) As Decimal

        Return (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)

    End Function

    ''' <summary>
    ''' Returns the euclidean distance ('as the crow flies') between two nodes.
    ''' </summary>
    Public Function Distance(startNode As Node, endNode As Node) As Decimal

        If startNode Is Nothing Or endNode Is Nothing Then

            Return 0

        End If

        Return Distance(startNode.position.X, startNode.position.Y, endNode.position.X, endNode.position.Y)

    End Function


End Module
