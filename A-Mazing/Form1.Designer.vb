<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ButtonLoadMaze = New System.Windows.Forms.Button()
        Me.PictureBoxMazeInput = New System.Windows.Forms.PictureBox()
        Me.ButtonAMaze = New System.Windows.Forms.Button()
        Me.ButtonSaveMaze = New System.Windows.Forms.Button()
        Me.NumericUpDownPrunning = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.RichTextBoxMessages = New System.Windows.Forms.RichTextBox()
        CType(Me.PictureBoxMazeInput, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDownPrunning, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonLoadMaze
        '
        Me.ButtonLoadMaze.Location = New System.Drawing.Point(51, 31)
        Me.ButtonLoadMaze.Name = "ButtonLoadMaze"
        Me.ButtonLoadMaze.Size = New System.Drawing.Size(166, 46)
        Me.ButtonLoadMaze.TabIndex = 0
        Me.ButtonLoadMaze.Text = "Load Maze"
        Me.ButtonLoadMaze.UseVisualStyleBackColor = True
        '
        'PictureBoxMazeInput
        '
        Me.PictureBoxMazeInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBoxMazeInput.Location = New System.Drawing.Point(52, 112)
        Me.PictureBoxMazeInput.Name = "PictureBoxMazeInput"
        Me.PictureBoxMazeInput.Size = New System.Drawing.Size(378, 293)
        Me.PictureBoxMazeInput.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBoxMazeInput.TabIndex = 1
        Me.PictureBoxMazeInput.TabStop = False
        '
        'ButtonAMaze
        '
        Me.ButtonAMaze.Enabled = False
        Me.ButtonAMaze.Location = New System.Drawing.Point(328, 31)
        Me.ButtonAMaze.Name = "ButtonAMaze"
        Me.ButtonAMaze.Size = New System.Drawing.Size(181, 46)
        Me.ButtonAMaze.TabIndex = 2
        Me.ButtonAMaze.Text = "A-Maze"
        Me.ButtonAMaze.UseVisualStyleBackColor = True
        '
        'ButtonSaveMaze
        '
        Me.ButtonSaveMaze.Enabled = False
        Me.ButtonSaveMaze.Location = New System.Drawing.Point(591, 31)
        Me.ButtonSaveMaze.Name = "ButtonSaveMaze"
        Me.ButtonSaveMaze.Size = New System.Drawing.Size(154, 46)
        Me.ButtonSaveMaze.TabIndex = 3
        Me.ButtonSaveMaze.Text = "Save Maze"
        Me.ButtonSaveMaze.UseVisualStyleBackColor = True
        '
        'NumericUpDownPrunning
        '
        Me.NumericUpDownPrunning.Location = New System.Drawing.Point(668, 127)
        Me.NumericUpDownPrunning.Maximum = New Decimal(New Integer() {500, 0, 0, 0})
        Me.NumericUpDownPrunning.Name = "NumericUpDownPrunning"
        Me.NumericUpDownPrunning.Size = New System.Drawing.Size(76, 26)
        Me.NumericUpDownPrunning.TabIndex = 4
        Me.NumericUpDownPrunning.Value = New Decimal(New Integer() {15, 0, 0, 0})
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(526, 127)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(127, 20)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Prunning passes"
        '
        'RichTextBoxMessages
        '
        Me.RichTextBoxMessages.Location = New System.Drawing.Point(462, 174)
        Me.RichTextBoxMessages.Name = "RichTextBoxMessages"
        Me.RichTextBoxMessages.Size = New System.Drawing.Size(315, 192)
        Me.RichTextBoxMessages.TabIndex = 6
        Me.RichTextBoxMessages.Text = ""
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.RichTextBoxMessages)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.NumericUpDownPrunning)
        Me.Controls.Add(Me.ButtonSaveMaze)
        Me.Controls.Add(Me.ButtonAMaze)
        Me.Controls.Add(Me.PictureBoxMazeInput)
        Me.Controls.Add(Me.ButtonLoadMaze)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.PictureBoxMazeInput, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDownPrunning, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ButtonLoadMaze As Button
    Friend WithEvents PictureBoxMazeInput As PictureBox
    Friend WithEvents ButtonAMaze As Button
    Friend WithEvents ButtonSaveMaze As Button
    Friend WithEvents NumericUpDownPrunning As NumericUpDown
    Friend WithEvents Label1 As Label
    Friend WithEvents RichTextBoxMessages As RichTextBox
End Class
