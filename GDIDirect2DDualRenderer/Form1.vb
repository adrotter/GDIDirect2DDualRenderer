Public Class Form1

  Private _count As Integer = 0

  Private Sub PictureBox1_Paint(sender As Object, e As PaintEventArgs) Handles PictureBox1.Paint

    Dim newAlpha = _count Mod 255

    IGraphics.CurrentGraphicsEngine = IGraphics.GraphicsEngine.Direct2D



    Using partialTransparentImage As New Drawing.Bitmap(50, 50)


      '                                                                         Change the graphics engine to GDI+ and it will render appropriately.
      Using tempGraphicsEngine As New IGraphics.TemporarilyChangeGraphicsEngine(IGraphics.GraphicsEngine.Direct2D)

        Using g = IGraphics.FromImage(partialTransparentImage)
          g.Clear(Color.Transparent)

          Using transRedBrush As New SolidBrush(Color.FromArgb(newAlpha, Color.Red))
            g.FillRectangle(transRedBrush, New Rectangle(0, 0, partialTransparentImage.Width - 1, partialTransparentImage.Height - 1))
          End Using

          Using redBrush As New SolidBrush(Color.Red)
            Using pen As New Pen(redBrush)
              g.DrawRectangle(pen, New Rectangle(0, 0, partialTransparentImage.Width - 1, partialTransparentImage.Height - 1))
            End Using
          End Using

        End Using

      End Using




      Using paintGraph = New Direct2D(e.Graphics)
        paintGraph.Clear(Color.Yellow)

        paintGraph.DrawImageUnscaled(partialTransparentImage, 10, 10, partialTransparentImage.Width, partialTransparentImage.Height)

      End Using



    End Using
  End Sub

  Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    _count += 15
    lblCount.Text = (_count Mod 255).ToString()
    PictureBox1.Invalidate()
  End Sub

  Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
    _count -= 15

    If _count < 0 Then _count = 0

    lblCount.Text = (_count Mod 255).ToString()
    PictureBox1.Invalidate()
  End Sub
End Class
