Public Module GlobalVars
    Public InputImage, AMazeImage As Bitmap
End Module

Structure Position
    Dim X As Integer
    Dim Y As Integer
End Structure

Public Class Form1
    Private Sub ButtonLoadMaze_Click(sender As Object, e As EventArgs) Handles ButtonLoadMaze.Click
        Dim ImageOpenDialog As New OpenFileDialog
        ImageOpenDialog.Filter = "PNG|*.png|JPEG|*.jpg|Bitmap|*.bmp"
        If ImageOpenDialog.ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub
        Try
            If Not IsNothing(InputImage) Then InputImage.Dispose()
            InputImage = New Bitmap(ImageOpenDialog.FileName)
            AMazeImage = New Bitmap(InputImage.Width, InputImage.Height)

            Dim CurrentX, CurrentY As Integer
            Dim InputPixelColor As Color

            For CurrentX = 0 To InputImage.Width - 1
                For CurrentY = 0 To InputImage.Height - 1
                    InputPixelColor = InputImage.GetPixel(CurrentX, CurrentY)
                    AMazeImage.SetPixel(CurrentX, CurrentY, InputPixelColor)
                Next
            Next

            If Not IsNothing(PictureBoxMazeInput.Image) Then PictureBoxMazeInput.Image.Dispose()
            PictureBoxMazeInput.Image = AMazeImage
            ButtonSaveMaze.Enabled = False
            ButtonAMaze.Enabled = True
        Catch
            MsgBox("Not a valid image file.")
        End Try

    End Sub

    Private Sub ButtonAMaze_Click(sender As Object, e As EventArgs) Handles ButtonAMaze.Click

        Dim CurrentPosition As Position
        Dim NoOfExits, NoOfPasses, MaxPasses As Integer
        Dim DeadEndsFound As Boolean
        Dim StartTimePrunning, EndTimePrunning As Date

        RichTextBoxMessages.Text = RichTextBoxMessages.Text & vbCrLf & "------------------"

        MaxPasses = NumericUpDownPrunning.Value
        If MaxPasses > 0 Then
            NoOfPasses = 0
            StartTimePrunning = DateTime.Now
            Do
                DeadEndsFound = False
                For CurrentPosition.Y = 1 To AMazeImage.Height - 2
                    For CurrentPosition.X = 1 To AMazeImage.Width - 2
                        If (AMazeImage.GetPixel(CurrentPosition.X, CurrentPosition.Y).R = 255) Then
                            NoOfExits = 4
                            If AMazeImage.GetPixel(CurrentPosition.X, CurrentPosition.Y - 1).R = 0 Then NoOfExits = NoOfExits - 1
                            If AMazeImage.GetPixel(CurrentPosition.X, CurrentPosition.Y + 1).R = 0 Then NoOfExits = NoOfExits - 1
                            If AMazeImage.GetPixel(CurrentPosition.X - 1, CurrentPosition.Y).R = 0 Then NoOfExits = NoOfExits - 1
                            If AMazeImage.GetPixel(CurrentPosition.X + 1, CurrentPosition.Y).R = 0 Then NoOfExits = NoOfExits - 1
                            If NoOfExits <= 1 Then
                                AMazeImage.SetPixel(CurrentPosition.X, CurrentPosition.Y, Color.FromArgb(0, 0, 128))
                                DeadEndsFound = True
                            End If
                        End If
                    Next
                Next
                NoOfPasses = NoOfPasses + 1
            Loop While DeadEndsFound And NoOfPasses < MaxPasses
            EndTimePrunning = DateTime.Now
            RichTextBoxMessages.Text = RichTextBoxMessages.Text & vbCrLf & "Prunning passes: " & NoOfPasses
            If NoOfPasses = MaxPasses Then RichTextBoxMessages.Text = RichTextBoxMessages.Text & " (limited)"
            RichTextBoxMessages.Text = RichTextBoxMessages.Text & vbCrLf & "Prunning time: " & vbTab & (EndTimePrunning - StartTimePrunning).TotalMilliseconds & " mSec"
        Else
            RichTextBoxMessages.Text = RichTextBoxMessages.Text & vbCrLf & "No Prunning"
        End If

        Dim StartPositionMaze As Position
        Dim Path(1), PathNew() As Position
        Dim PathList As New List(Of Array)

        'Locate the startposition in the first row
        CurrentPosition.Y = 0
        For CurrentPosition.X = 1 To AMazeImage.Width - 2
            If (AMazeImage.GetPixel(CurrentPosition.X, CurrentPosition.Y).R = 255) Then
                StartPositionMaze = CurrentPosition
                Path(0) = StartPositionMaze
                AMazeImage.SetPixel(StartPositionMaze.X, StartPositionMaze.Y, Color.FromArgb(0, 0, 255))
                CurrentPosition.X = StartPositionMaze.X
                CurrentPosition.Y = StartPositionMaze.Y + 1
                Path(1) = CurrentPosition
                PathList.Add(Path)
                Exit For
            End If
        Next
        If PathList.Count = 0 Then RichTextBoxMessages.Text = RichTextBoxMessages.Text & vbCrLf & "No start position found."

        Dim StartTimeSolving, EndTimeSolving As Date

        StartTimeSolving = DateTime.Now
        Do While PathList.Count > 0
            CurrentPosition = PathList(0)(PathList(0).Length - 1)

            'Mark current position as 'no exit' i.e. path not available since it's already part of a path
            AMazeImage.SetPixel(CurrentPosition.X, CurrentPosition.Y, Color.FromArgb(0, 0, 255))

            'Scan surrounding for available exits i.e. possible paths if found then add this as a 
            'new path to the list

            'Check down position, it's the only possiblity for an end of maze
            If (AMazeImage.GetPixel(CurrentPosition.X, CurrentPosition.Y + 1).R = 255) Then
                PathNew = PathList(0)
                Array.Resize(PathNew, PathNew.Length + 1)
                PathNew(PathNew.Length - 1).X = CurrentPosition.X
                PathNew(PathNew.Length - 1).Y = CurrentPosition.Y + 1
                PathList.Add(PathNew)

                'Check if the path exits at the end of the maze (last row), if so quit checking
                If CurrentPosition.Y + 1 = AMazeImage.Height - 1 Then Exit Do
            End If

            'Check up position
            If (AMazeImage.GetPixel(CurrentPosition.X, CurrentPosition.Y - 1).R = 255) Then
                PathNew = PathList(0)
                Array.Resize(PathNew, PathNew.Length + 1)
                PathNew(PathNew.Length - 1).X = CurrentPosition.X
                PathNew(PathNew.Length - 1).Y = CurrentPosition.Y - 1
                PathList.Add(PathNew)
            End If

            'Check left position
            If (AMazeImage.GetPixel(CurrentPosition.X - 1, CurrentPosition.Y).R = 255) Then
                PathNew = PathList(0)
                Array.Resize(PathNew, PathNew.Length + 1)
                PathNew(PathNew.Length - 1).X = CurrentPosition.X - 1
                PathNew(PathNew.Length - 1).Y = CurrentPosition.Y
                PathList.Add(PathNew)
            End If

            'Check right position
            If (AMazeImage.GetPixel(CurrentPosition.X + 1, CurrentPosition.Y).R = 255) Then
                PathNew = PathList(0)
                Array.Resize(PathNew, PathNew.Length + 1)
                PathNew(PathNew.Length - 1).X = CurrentPosition.X + 1
                PathNew(PathNew.Length - 1).Y = CurrentPosition.Y
                PathList.Add(PathNew)
            End If

            'This path has been processed so remove it from the list
            PathList.RemoveAt(0)
        Loop

        EndTimeSolving = DateTime.Now
        RichTextBoxMessages.Text = RichTextBoxMessages.Text & vbCrLf & "Solving time: " & vbTab & (EndTimeSolving - StartTimeSolving).TotalMilliseconds & " mSec"
        RichTextBoxMessages.Text = RichTextBoxMessages.Text & vbCrLf & "Total time: " & vbTab & ((EndTimePrunning - StartTimePrunning) + (EndTimeSolving - StartTimeSolving)).TotalMilliseconds & " mSec"

        'The last path in the list contains the shortest route, color is path green
        Dim CheckPosition As Position
        If PathList.Count() > 0 Then
            For PathCounter = 0 To PathList.Last().Length - 1
                CheckPosition = PathList.Last()(PathCounter)
                AMazeImage.SetPixel(CheckPosition.X, CheckPosition.Y, Color.FromArgb(0, 255, 0))
            Next
        Else
            RichTextBoxMessages.Text = RichTextBoxMessages.Text & vbCrLf & "No end position found."
        End If

        'Update the screen and enable the option to save the solution
        PictureBoxMazeInput.Refresh()
        ButtonSaveMaze.Enabled = True
        ButtonAMaze.Enabled = False
    End Sub

    Private Sub ButtonSaveMaze_Click(sender As Object, e As EventArgs) Handles ButtonSaveMaze.Click
        Dim SaveDialog As New SaveFileDialog
        SaveDialog.CreatePrompt = True
        SaveDialog.DefaultExt = "jpg"
        SaveDialog.Filter = "File Images (*.jpg;*.jpeg;) | *.jpg;*.jpeg; |PNG Images | *.png |GIF Images | *.GIF"
        SaveDialog.InitialDirectory = "C:\Gegevens\Prive\Tekenen"

        If SaveDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                AMazeImage.Save(SaveDialog.FileName)
                ButtonSaveMaze.Enabled = False
            Catch
                MsgBox("Unable to save file.")
            End Try

        End If

    End Sub

End Class
