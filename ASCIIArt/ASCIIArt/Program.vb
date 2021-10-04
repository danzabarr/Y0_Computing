Imports System
Imports System.Drawing

Module Program

    Public Const CharWidth As Integer = 9
    Public Const CharHeight As Integer = 20

    Public Const CharAspectRatio As Decimal = CharWidth / CharHeight

    Public Const MaxColumns As Integer = 110
    Public Const MaxRows As Integer = 50

    Public Const AspectRatio As Decimal = MaxColumns / MaxRows * CharAspectRatio

    Public Const Grayscale As String = " .-:=o+*#%@"

    Private FilePath As String

    Private Image As Bitmap

    Sub Main()

        Console.WriteLine("---------------ASCII ART GENERATOR---------------")

        ReadFilePath()

        'Generate the array of Strings to print'
        Dim lines As String() = CreateASCIIArt()

        'Print the lines'
        For Each line As String In lines
            Console.WriteLine(line)
        Next

    End Sub

    Private Sub ReadFilePath()
        Console.WriteLine("Enter the file path to your image")

        Try
            FilePath = Console.ReadLine()

            'Load the bitmap'
            Image = LoadBitmap(FilePath)

            'Check the image loaded'
            If Image Is Nothing Then

                ReadFilePath()

            End If

        Catch ex As Exception

            ReadFilePath()

        End Try

        Console.WriteLine("Loading image from path: " + FilePath)

    End Sub

    '''<summary>
    ''' Loads a bitmap image from a supplied file path.
    ''' </summary>
    ''' <returns>
    ''' A Bitmap object representing the image.
    ''' </returns>
    Public Function LoadBitmap(ByVal path As String) As Bitmap

        If String.IsNullOrEmpty(path) Then

            Console.WriteLine("ERROR: File path was null or empty")
            Return Nothing

        End If

        Dim fileInfo As New IO.FileInfo(path)

        If Not fileInfo.Exists Then

            Console.WriteLine("ERROR: File does not exist.")
            Return Nothing

        End If

        Dim extension As String = fileInfo.Extension

        If Not extension.Equals(".jpg") And Not extension.Equals(".png") Then

            Console.WriteLine("ERROR: File is not a supported type")
            Return Nothing

        End If

        Return New Bitmap(path)

    End Function

    '''<summary>
    ''' Color to luminance value conversion.
    ''' </summary>
    ''' <returns>
    ''' A luminance value in the range 0-1
    ''' </returns>
    Public Function Luminance(ByVal color As Color) As Decimal

        Return (color.R * 0.2126 + color.G * 0.7152 + color.B * 0.0722) / 255

    End Function

    '''<summary>
    ''' Decimal to ASCII character conversion sampling the Grayscale ramp.
    ''' Value argument clamped between 0-1.
    ''' </summary>
    ''' <returns>
    ''' An ASCII character for the given value.
    ''' </returns>
    Public Function GetASCIIChar(ByVal value As Decimal) As Char
        Dim length = Grayscale.Length

        Dim index As Integer = Math.Floor(value * length)

        index = Math.Clamp(index, 0, length - 1)

        Return Grayscale(index)

    End Function

    ''' <summary>
    ''' Converts the Bitmap image to ASCII art.
    ''' </summary>
    ''' <returns>
    ''' An array of Strings to print.
    ''' </returns>
    Public Function CreateASCIIArt() As String()

        Console.WriteLine()
        Console.WriteLine("Generating ASCII Art...")
        Console.WriteLine()

        Dim imageAspectRatio As Decimal = Image.Width / Image.Height

        Dim columns As Integer = MaxColumns
        Dim rows As Integer = MaxRows

        If imageAspectRatio < AspectRatio Then

            Dim rowsHeight As Decimal = rows * CharHeight
            Dim colsWidth As Decimal = rowsHeight * imageAspectRatio

            columns = Math.Floor(colsWidth / CharWidth)
        Else
            Dim colsWidth As Decimal = columns * CharWidth
            Dim rowsHeight As Decimal = colsWidth / imageAspectRatio

            rows = Math.Floor(rowsHeight / CharHeight)
        End If

        Dim lines(rows) As String

        Dim sampleWidth = Math.Max(Image.Width / columns, 1)
        Dim sampleHeight = Math.Max(Image.Height / rows, 1)

        Dim gridPixelWidth = columns * CharWidth
        Dim gridPixelHeight = rows * CharHeight

        Console.WriteLine("Canvas Size:           " + MaxColumns.ToString + " x " + MaxRows.ToString)
        Console.WriteLine("Canvas Size (Pixels):  " + (MaxColumns * CharWidth).ToString + " x " + (MaxRows * CharHeight).ToString + " (" + ((MaxColumns * CharWidth) / (MaxRows * CharHeight)).ToString + ")")
        Console.WriteLine("Image Size:            " + Image.Width.ToString + " x " + Image.Height.ToString + " (" + imageAspectRatio.ToString + ")")
        Console.WriteLine("Sample Size:           " + sampleWidth.ToString + " x " + sampleHeight.ToString)
        Console.WriteLine("Grid Size:             " + columns.ToString + " x " + rows.ToString)
        Console.WriteLine("Grid Size (Pixels):    " + gridPixelWidth.ToString + " x " + gridPixelHeight.ToString + " (" + (gridPixelWidth / gridPixelHeight).ToString + ")")

        For r = 0 To rows - 1
            Dim line As String = ""
            Dim y As Integer = r * sampleHeight
            For c = 0 To columns - 1
                Dim x As Integer = c * sampleWidth

                Dim luma As Decimal = 0
                Dim samples As Integer = 0

                For x1 = x To Math.Min(x + sampleWidth - 1, Image.Width - 1)
                    For y1 = y To Math.Min(y + sampleHeight - 1, Image.Height - 1)

                        Dim pixel As Color = Image.GetPixel(x1, y1)
                        luma += Luminance(pixel)
                        samples += 1
                    Next
                Next

                If samples > 0 Then
                    luma /= samples
                End If

                Dim character = GetASCIIChar(luma)

                line += character
            Next

            lines(r) = line

        Next

        Return lines

    End Function

End Module
