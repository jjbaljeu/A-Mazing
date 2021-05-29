Public Module GlobalVars
    Public InputImage, AMazeImage As Bitmap
    Public Const MazePath = "C:\Gegevens\Projecten\Mazes\Samples\"
    Public Const ImagesPath = "C:\Gegevens\Projecten\Mazes\Samples\Images\"

End Module

Structure Position
    Dim X As Integer
    Dim Y As Integer
End Structure

Public Class Form1
    Private Sub ButtonLoadMaze_Click(sender As Object, e As EventArgs) Handles ButtonLoadMaze.Click
        Dim ImageOpenDialog As New OpenFileDialog
        ImageOpenDialog.Filter = "PNG|*.png|JPEG|*.jpg|Bitmap|*.bmp"
        ImageOpenDialog.InitialDirectory = MazePath
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
            ButtonGrayMaze.Enabled = True
            ButtonPrunning.Enabled = True
            ButtonAMaze.Enabled = True
            ButtonAMazeII.Enabled = True
        Catch
            MsgBox("Not a valid image file.")
        End Try

    End Sub

    Private Sub ButtonAMaze_Click(sender As Object, e As EventArgs) Handles ButtonAMaze.Click

        Dim CurrentPosition As Position
        Dim StartPositionMaze As Position
        Dim Path(1), PathNew() As Position
        Dim PathList As New List(Of Array)

        RichTextBoxMessages.Text = RichTextBoxMessages.Text & vbCrLf & "------------------"

        'Locate the startposition in the first row
        CurrentPosition.Y = 0
        For CurrentPosition.X = 1 To AMazeImage.Width - 2
            If (AMazeImage.GetPixel(CurrentPosition.X, CurrentPosition.Y).R = 255) Then
                StartPositionMaze = CurrentPosition
                Path(0) = StartPositionMaze
                AMazeImage.SetPixel(StartPositionMaze.X, StartPositionMaze.Y, Color.FromArgb(200, 200, 200)) 'used to be 0 0 255
                CurrentPosition.X = StartPositionMaze.X
                CurrentPosition.Y = StartPositionMaze.Y + 1
                Path(1) = CurrentPosition
                PathList.Add(Path)
                Exit For
            End If
        Next
        If PathList.Count = 0 Then
            RichTextBoxMessages.Text = RichTextBoxMessages.Text & vbCrLf & "No start position found."
            Exit Sub
        End If

        Dim StartTimeSolving, EndTimeSolving As Date

        Dim FileCounter As Integer 'Used to make animated GIF
        FileCounter = 1            'Used to make animated GIF

        StartTimeSolving = DateTime.Now
        Do While PathList.Count > 0
            CurrentPosition = PathList(0)(PathList(0).Length - 1)

            'Mark current position as 'no exit' i.e. path not available since it's already part of a path
            AMazeImage.SetPixel(CurrentPosition.X, CurrentPosition.Y, Color.FromArgb(200, 200, 200)) 'used to be 0 0 255 changed to make animated GIF

            'Scan surrounding for available exits i.e. possible paths
            'if found then add this as a new path to the list

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

            FileCounter = FileCounter + 1
            ' AMazeImage.Save(ImagesPath & "Maze" & FileCounter & ".jpg") 'Used to make animated GIF


        Loop

        EndTimeSolving = DateTime.Now
        RichTextBoxMessages.Text = RichTextBoxMessages.Text & vbCrLf & "Solving time: " & vbTab & (EndTimeSolving - StartTimeSolving).TotalMilliseconds & " mSec"
        RichTextBoxMessages.Text = RichTextBoxMessages.Text & vbCrLf & "Length: " & vbTab & vbTab & PathList(PathList.Count - 1).Length

        'The last path in the list contains the shortest route, color is path green
        Dim CheckPosition As Position
        If PathList.Count() > 0 Then
            For PathCounter = PathList.Last().Length - 1 To 0 Step -1
                CheckPosition = PathList.Last()(PathCounter)
                AMazeImage.SetPixel(CheckPosition.X, CheckPosition.Y, Color.FromArgb(255, 0, 0)) 'used to be 0 255 0
                '                AMazeImage.Save(ImagesPath & "Maze" & FileCounter & ".jpg") 'Used to make animated GIF
                FileCounter = FileCounter + 1 'Used to make animated GIF
            Next
        Else
            RichTextBoxMessages.Text = RichTextBoxMessages.Text & vbCrLf & "No end position found."
        End If

        '        AMazeImage.Save(ImagesPath & "Maze" & FileCounter & ".jpg") 'Used to make animated GIF


        'Update the screen and enable the option to save the solution
        PictureBoxMazeInput.Refresh()
        ButtonSaveMaze.Enabled = True
        ButtonPrunning.Enabled = False
        ButtonAMaze.Enabled = False
        ButtonAMazeII.Enabled = False

    End Sub

    Private Sub ButtonSaveMaze_Click(sender As Object, e As EventArgs) Handles ButtonSaveMaze.Click
        Dim SaveDialog As New SaveFileDialog
        SaveDialog.CreatePrompt = True
        SaveDialog.DefaultExt = "jpg"
        SaveDialog.Filter = "File Images (*.jpg;*.jpeg;) | *.jpg;*.jpeg; |PNG Images | *.png |GIF Images | *.GIF"
        SaveDialog.InitialDirectory = MazePath

        If SaveDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                AMazeImage.Save(SaveDialog.FileName)
                ButtonSaveMaze.Enabled = False
            Catch
                MsgBox("Unable to save file.")
            End Try

        End If

    End Sub

    Private Sub ButtonGrayMaze_Click(sender As Object, e As EventArgs) Handles ButtonGrayMaze.Click 'Used to make animated GIF

        Dim CurrentPosition As Position

        For CurrentPosition.Y = 0 To AMazeImage.Height - 1
            For CurrentPosition.X = 0 To AMazeImage.Width - 1
                If (AMazeImage.GetPixel(CurrentPosition.X, CurrentPosition.Y).R = 0) Then
                    AMazeImage.SetPixel(CurrentPosition.X, CurrentPosition.Y, Color.FromArgb(128, 128, 128))
                End If
            Next
        Next
        PictureBoxMazeInput.Refresh()
        ButtonSaveMaze.Enabled = True

    End Sub

    Private Sub ButtonPrunning_Click(sender As Object, e As EventArgs) Handles ButtonPrunning.Click
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
                            NoOfExits = 0
                            If AMazeImage.GetPixel(CurrentPosition.X, CurrentPosition.Y - 1).R = 255 Then NoOfExits = NoOfExits + 1
                            If AMazeImage.GetPixel(CurrentPosition.X, CurrentPosition.Y + 1).R = 255 Then NoOfExits = NoOfExits + 1
                            If AMazeImage.GetPixel(CurrentPosition.X - 1, CurrentPosition.Y).R = 255 Then NoOfExits = NoOfExits + 1
                            If AMazeImage.GetPixel(CurrentPosition.X + 1, CurrentPosition.Y).R = 255 Then NoOfExits = NoOfExits + 1
                            If NoOfExits <= 1 Then
                                AMazeImage.SetPixel(CurrentPosition.X, CurrentPosition.Y, Color.FromArgb(200, 200, 200)) 'Used to be 0 0 255 changed to make animated GIF
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

        'Update the screen and enable the option to save the solution
        PictureBoxMazeInput.Refresh()
        ButtonSaveMaze.Enabled = True
        ButtonPrunning.Enabled = False
        ButtonAMaze.Enabled = False
        ButtonAMazeII.Enabled = False
    End Sub

    Private Sub ButtonAMazeII_Click(sender As Object, e As EventArgs) Handles ButtonAMazeII.Click

        Dim CurrentPosition = New Position()
        Dim NewPosition = New Position()
        Dim Path As New List(Of Position)
        Dim AlternativePaths As New List(Of Integer)
        Dim ExitFound As Integer
        Dim FileCounter As Integer            'Used to make animated GIF
        FileCounter = 1
        Dim StartTimeSolving, EndTimeSolving As Date


        RichTextBoxMessages.Text = RichTextBoxMessages.Text & vbCrLf & "------------------"

        'Locate the startposition in the first row
        CurrentPosition.Y = 0
        For CurrentPosition.X = 1 To AMazeImage.Width - 2
            If (AMazeImage.GetPixel(CurrentPosition.X, CurrentPosition.Y).R = 255) Then
                AMazeImage.SetPixel(CurrentPosition.X, CurrentPosition.Y, Color.FromArgb(200, 200, 200)) 'Used to be 0 0 128 changed to make animated GIF
                Path.Add(CurrentPosition)
                CurrentPosition.Y = CurrentPosition.Y + 1
                Path.Add(CurrentPosition)
                ExitFound = 1
                Exit For
            End If
        Next
        If ExitFound = 0 Then
            RichTextBoxMessages.Text = RichTextBoxMessages.Text & vbCrLf & "No start position found."
            Exit Sub
        End If

        StartTimeSolving = DateTime.Now

        Do While (Path.Count > 0)
            CurrentPosition = Path(Path.Count - 1)
            ExitFound = 0

            'Mark current position as 'no exit' i.e. path not available since it's already part of a path
            AMazeImage.SetPixel(CurrentPosition.X, CurrentPosition.Y, Color.FromArgb(200, 200, 200)) 'Used to be 0 0 255 changed to make animated GIF

            'Scan surrounding for available exits i.e. possible paths if found then add this as a 
            'New path to the list

            'Check down position first since it's the only possiblity for an exit that ends the maze
            If (AMazeImage.GetPixel(CurrentPosition.X, CurrentPosition.Y + 1).R = 255) Then
                NewPosition.X = CurrentPosition.X
                NewPosition.Y = CurrentPosition.Y + 1
                Path.Add(NewPosition)
                ExitFound = ExitFound + 1

                'Check if the path exits at the maze end (bottom row), if so quit checking
                If (NewPosition.Y = AMazeImage.Height - 1) Then Exit Do
            End If

            'Check left position
            If (AMazeImage.GetPixel(CurrentPosition.X - 1, CurrentPosition.Y).R = 255) Then
                If (ExitFound = 0) Then
                    NewPosition.X = CurrentPosition.X - 1
                    NewPosition.Y = CurrentPosition.Y
                    Path.Add(NewPosition)
                End If
                ExitFound = ExitFound + 1
            End If

            'Check right position
            If (AMazeImage.GetPixel(CurrentPosition.X + 1, CurrentPosition.Y).R = 255) Then
                If (ExitFound = 0) Then
                    NewPosition.X = CurrentPosition.X + 1
                    NewPosition.Y = CurrentPosition.Y
                    Path.Add(NewPosition)
                End If
                ExitFound = ExitFound + 1
            End If

            'Check up position
            If (AMazeImage.GetPixel(CurrentPosition.X, CurrentPosition.Y - 1).R = 255) Then
                If (ExitFound = 0) Then
                    NewPosition.X = CurrentPosition.X
                    NewPosition.Y = CurrentPosition.Y - 1
                    Path.Add(NewPosition)
                End If
                ExitFound = ExitFound + 1
            End If

            If (ExitFound > 1) Then AlternativePaths.Add(Path.Count - 2) 'Add current entry (current position) To list Of alternative route possibilities
            If (ExitFound = 0) Then
                If (AlternativePaths.Count > 0) Then ' Check Then If alternative Then route possibilities exist
                    Path.RemoveRange(AlternativePaths(AlternativePaths.Count - 1) + 1, Path.Count - AlternativePaths(AlternativePaths.Count - 1) - 1)
                    AlternativePaths.RemoveAt(AlternativePaths.Count - 1)
                Else
                    Path.Clear()
                End If
            End If

            ' FileCounter = FileCounter + 1;
            ' AMazeImage.Save(ImagesPath & "Maze" & FileCounter & ".jpg") 'Used to make animated GIF
        Loop

        EndTimeSolving = DateTime.Now
        If (Path.Count() > 0) Then
            RichTextBoxMessages.Text = RichTextBoxMessages.Text & vbCrLf & "Solving time (II): " & vbTab & (EndTimeSolving - StartTimeSolving).TotalMilliseconds & " mSec"
            RichTextBoxMessages.Text = RichTextBoxMessages.Text & vbCrLf & "Length: " & vbTab & vbTab & Path.Count

            'Path contains the route, color this path green
            For PathCounter = 0 To Path.Count() - 1
                CurrentPosition = Path(PathCounter)
                AMazeImage.SetPixel(CurrentPosition.X, CurrentPosition.Y, Color.FromArgb(0, 255, 0))
            Next
            ' FileCounter = FileCounter + 1
            ' AMazeImage.Save(ImagesPath & "Maze" & FileCounter & ".jpg") 'Used to make animated GIF
        Else
            RichTextBoxMessages.Text = RichTextBoxMessages.Text & vbCrLf & "No end position found."
        End If
        'Update the screen And enable the option to save the solution
        PictureBoxMazeInput.Refresh()
        ButtonSaveMaze.Enabled = True
        ButtonPrunning.Enabled = False
        ButtonAMaze.Enabled = False
        ButtonAMazeII.Enabled = False

    End Sub
End Class
