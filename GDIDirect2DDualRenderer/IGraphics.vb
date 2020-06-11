Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Graphics
Imports System.Drawing.Imaging
Imports System.Drawing.Text


'Choosing to use a class here because we can have static methods in a class, but not an interface.  This will act as a pseudo interface.
Public MustInherit Class IGraphics
  Implements IDisposable

  Public Enum GraphicsEngine
    GDIPlus
    Direct2D
  End Enum

  Public Class TemporarilyChangeGraphicsEngine
    Implements IDisposable

    Private _oldEngine As GraphicsEngine
    Public Sub New(graphicsEngine As GraphicsEngine)
      _oldEngine = CurrentGraphicsEngine

      CurrentGraphicsEngine = graphicsEngine
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
      If Not disposedValue Then
        If disposing Then
          CurrentGraphicsEngine = _oldEngine

        End If

      End If
      disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
      Dispose(True)
    End Sub
#End Region
  End Class

  Public MustOverride Property PageScale As Single
  Public MustOverride Property Transform As Matrix
  Public MustOverride Property PageUnit As GraphicsUnit
  Public MustOverride Property SmoothingMode As SmoothingMode
  Public MustOverride Property TextContrast As Integer
  Public MustOverride Property TextRenderingHint As TextRenderingHint
  Public MustOverride Property CompositingQuality As CompositingQuality
  Public MustOverride Property InterpolationMode As InterpolationMode
  Public MustOverride ReadOnly Property DpiX As Single
  Public MustOverride ReadOnly Property DpiY As Single
  Public MustOverride Property PixelOffsetMode As PixelOffsetMode
  Public MustOverride Property RenderingOrigin As System.Drawing.Point
  Public MustOverride Property CompositingMode As CompositingMode
  Public Property Clip As Region
  Public ReadOnly Property ClipBounds As RectangleF
  Public ReadOnly Property VisibleClipBounds As RectangleF
  Public ReadOnly Property IsVisibleClipEmpty As Boolean
  Public ReadOnly Property IsClipEmpty As Boolean

  Public Shared Property CurrentGraphicsEngine As GraphicsEngine = GraphicsEngine.GDIPlus

  Public Class BitmapChangedEventArgs
    Inherits EventArgs

    Public Property ChangedBitMap As Bitmap

    Public Sub New(bmp As Bitmap)
      ChangedBitMap = bmp
    End Sub
  End Class

  Public Delegate Sub BitMapChangedHandler(ByVal sender As Object, ByVal e As BitmapChangedEventArgs)
  Public Shared Event BitMapPotentiallyChanged As BitMapChangedHandler

  Public Shared Sub BitmapChanged(image As Image)
    Dim hc = image.GetHashCode()
    RaiseEvent BitMapPotentiallyChanged(Nothing, New BitmapChangedEventArgs(image))
  End Sub

  Public Shared Function FromImage(image As Image) As IGraphics
    'Notify classes listening who have an internal bitmap dictionary that a bitmap probably changed, and they should do something about it.
    'This is because bitmaps don't change their hashcode even though their pixels probably changed.
    BitmapChanged(image)

    Select Case CurrentGraphicsEngine
      Case GraphicsEngine.GDIPlus
        Return New GDIPlus(Graphics.FromImage(image))
      Case GraphicsEngine.Direct2D
        Return New Direct2D(Graphics.FromImage(image))
      Case Else
        Throw New NotImplementedException()
    End Select

  End Function

  Public Shared Function FromHwnd(hwnd As IntPtr) As IGraphics
    Select Case CurrentGraphicsEngine
      Case GraphicsEngine.GDIPlus
        Return New GDIPlus(Graphics.FromHwnd(hwnd))
      Case GraphicsEngine.Direct2D
        Return New Direct2D(Graphics.FromHwnd(hwnd))
      Case Else
        Throw New NotImplementedException()
    End Select
  End Function

  Public Shared Function ConvertGraphics(graphics As Graphics) As IGraphics
    Select Case CurrentGraphicsEngine
      Case GraphicsEngine.GDIPlus
        Return New GDIPlus(graphics)
      Case GraphicsEngine.Direct2D
        Return New Direct2D(graphics)
      Case Else
        Throw New NotImplementedException()
    End Select
  End Function

  Public MustOverride Sub DrawImage(image As Image, destPoints() As PointF, srcRect As RectangleF, srcUnit As GraphicsUnit, imageAttr As ImageAttributes)
  Public MustOverride Sub DrawImage(image As Image, destPoints() As PointF, srcRect As RectangleF, srcUnit As GraphicsUnit, imageAttr As ImageAttributes, callback As DrawImageAbort)
  Public MustOverride Sub DrawImage(image As Image, destPoints() As PointF, srcRect As RectangleF, srcUnit As GraphicsUnit, imageAttr As ImageAttributes, callback As DrawImageAbort, callbackData As Integer)
  Public MustOverride Sub DrawImage(image As Image, destPoints() As System.Drawing.Point, srcRect As Rectangle, srcUnit As GraphicsUnit)
  Public MustOverride Sub DrawImage(image As Image, destPoints() As System.Drawing.Point, srcRect As Rectangle, srcUnit As GraphicsUnit, imageAttr As ImageAttributes)
  Public MustOverride Sub DrawImage(image As Image, destPoints() As System.Drawing.Point, srcRect As Rectangle, srcUnit As GraphicsUnit, imageAttr As ImageAttributes, callback As DrawImageAbort)
  Public MustOverride Sub DrawImage(image As Image, destPoints() As System.Drawing.Point, srcRect As Rectangle, srcUnit As GraphicsUnit, imageAttr As ImageAttributes, callback As DrawImageAbort, callbackData As Integer)
  Public MustOverride Sub DrawImage(image As Image, destRect As Rectangle, srcX As Single, srcY As Single, srcWidth As Single, srcHeight As Single, srcUnit As GraphicsUnit)
  Public MustOverride Sub DrawImage(image As Image, destRect As Rectangle, srcX As Single, srcY As Single, srcWidth As Single, srcHeight As Single, srcUnit As GraphicsUnit, imageAttrs As ImageAttributes)
  Public MustOverride Sub DrawImage(image As Image, destRect As Rectangle, srcX As Single, srcY As Single, srcWidth As Single, srcHeight As Single, srcUnit As GraphicsUnit, imageAttrs As ImageAttributes, callback As DrawImageAbort, callbackData As IntPtr)
  Public MustOverride Sub DrawImage(image As Image, destPoints() As PointF, srcRect As RectangleF, srcUnit As GraphicsUnit)
  Public MustOverride Sub DrawImage(image As Image, destRect As Rectangle, srcX As Integer, srcY As Integer, srcWidth As Integer, srcHeight As Integer, srcUnit As GraphicsUnit)
  Public MustOverride Sub DrawImage(image As Image, destRect As Rectangle, srcX As Integer, srcY As Integer, srcWidth As Integer, srcHeight As Integer, srcUnit As GraphicsUnit, imageAttr As ImageAttributes)
  Public MustOverride Sub DrawImage(image As Image, destRect As Rectangle, srcX As Integer, srcY As Integer, srcWidth As Integer, srcHeight As Integer, srcUnit As GraphicsUnit, imageAttr As ImageAttributes, callback As DrawImageAbort)
  Public MustOverride Sub DrawImage(image As Image, destRect As Rectangle, srcX As Integer, srcY As Integer, srcWidth As Integer, srcHeight As Integer, srcUnit As GraphicsUnit, imageAttrs As ImageAttributes, callback As DrawImageAbort, callbackData As IntPtr)
  Public MustOverride Sub DrawImage(image As Image, destRect As Rectangle, srcX As Single, srcY As Single, srcWidth As Single, srcHeight As Single, srcUnit As GraphicsUnit, imageAttrs As ImageAttributes, callback As DrawImageAbort)
  Public MustOverride Sub DrawImage(image As Image, destRect As Rectangle, srcRect As Rectangle, srcUnit As GraphicsUnit)
  Public MustOverride Sub DrawImage(image As Image, destPoints() As System.Drawing.Point)
  Public MustOverride Sub DrawImage(image As Image, x As Integer, y As Integer, srcRect As Rectangle, srcUnit As GraphicsUnit)
  Public MustOverride Sub DrawIcon(icon As Icon, x As Integer, y As Integer)
  Public MustOverride Sub DrawIcon(icon As Icon, targetRect As Rectangle)
  Public MustOverride Sub DrawIconUnstretched(icon As Icon, targetRect As Rectangle)
  Public MustOverride Sub DrawImage(image As Image, point As PointF)
  Public MustOverride Sub DrawImage(image As Image, x As Single, y As Single)
  Public MustOverride Sub DrawImage(image As Image, rect As RectangleF)
  Public MustOverride Sub DrawImage(image As Image, x As Single, y As Single, width As Single, height As Single)
  Public MustOverride Sub DrawImage(image As Image, x As Integer, y As Integer)
  Public MustOverride Sub DrawImage(image As Image, destRect As RectangleF, srcRect As RectangleF, srcUnit As GraphicsUnit)
  Public MustOverride Sub DrawImage(image As Image, rect As Rectangle)
  Public MustOverride Sub DrawImageUnscaled(image As Image, point As System.Drawing.Point)
  Public MustOverride Sub DrawImageUnscaled(image As Image, x As Integer, y As Integer)
  Public MustOverride Sub DrawImageUnscaled(image As Image, rect As Rectangle)
  Public MustOverride Sub DrawImageUnscaled(image As Image, x As Integer, y As Integer, width As Integer, height As Integer)
  Public MustOverride Sub DrawImageUnscaledAndClipped(image As Image, rect As Rectangle)
  Public MustOverride Sub DrawImage(image As Image, destPoints() As PointF)
  Public MustOverride Sub DrawImage(image As Image, x As Single, y As Single, srcRect As RectangleF, srcUnit As GraphicsUnit)
  Public MustOverride Sub DrawImage(image As Image, x As Integer, y As Integer, width As Integer, height As Integer)
  Public MustOverride Sub TranslateClip(dx As Integer, dy As Integer)
  Public MustOverride Sub SetClip(g As Graphics)
  Public MustOverride Sub SetClip(g As Graphics, combineMode As CombineMode)
  Public MustOverride Sub SetClip(rect As Rectangle)
  Public MustOverride Sub SetClip(rect As Rectangle, combineMode As CombineMode)
  Public MustOverride Sub SetClip(rect As RectangleF)
  Public MustOverride Sub SetClip(rect As RectangleF, combineMode As CombineMode)
  Public MustOverride Sub SetClip(path As GraphicsPath, combineMode As CombineMode)
  Public MustOverride Sub SetClip(region As Region, combineMode As CombineMode)
  Public MustOverride Sub IntersectClip(rect As Rectangle)
  Public MustOverride Sub IntersectClip(rect As RectangleF)
  Public MustOverride Sub IntersectClip(region As Region)
  Public MustOverride Sub ExcludeClip(rect As Rectangle)
  Public MustOverride Sub ExcludeClip(region As Region)
  Public MustOverride Sub ResetClip()
  Public MustOverride Sub TranslateClip(dx As Single, dy As Single)
  Public MustOverride Sub SetClip(path As GraphicsPath)
  Public MustOverride Sub DrawImage(image As Image, point As System.Drawing.Point)
  Public MustOverride Sub DrawString(s As String, font As Font, brush As Brush, layoutRectangle As RectangleF, format As StringFormat)
  Public MustOverride Sub DrawLine(pen As Pen, pt1 As System.Drawing.Point, pt2 As System.Drawing.Point)
  Public MustOverride Sub DrawLines(pen As Pen, points() As System.Drawing.Point)
  Public MustOverride Sub DrawArc(pen As Pen, x As Single, y As Single, width As Single, height As Single, startAngle As Single, sweepAngle As Single)
  Public MustOverride Sub DrawArc(pen As Pen, rect As RectangleF, startAngle As Single, sweepAngle As Single)
  Public MustOverride Sub DrawArc(pen As Pen, x As Integer, y As Integer, width As Integer, height As Integer, startAngle As Integer, sweepAngle As Integer)
  Public MustOverride Sub DrawArc(pen As Pen, rect As Rectangle, startAngle As Single, sweepAngle As Single)
  Public MustOverride Sub DrawBezier(pen As Pen, x1 As Single, y1 As Single, x2 As Single, y2 As Single, x3 As Single, y3 As Single, x4 As Single, y4 As Single)
  Public MustOverride Sub DrawBezier(pen As Pen, pt1 As PointF, pt2 As PointF, pt3 As PointF, pt4 As PointF)
  Public MustOverride Sub DrawBeziers(pen As Pen, points() As PointF)
  Public MustOverride Sub DrawLine(pen As Pen, x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer)
  Public MustOverride Sub DrawBezier(pen As Pen, pt1 As System.Drawing.Point, pt2 As System.Drawing.Point, pt3 As System.Drawing.Point, pt4 As System.Drawing.Point)
  Public MustOverride Sub DrawRectangle(pen As Pen, rect As Rectangle)
  Public MustOverride Sub DrawRectangle(pen As Pen, x As Single, y As Single, width As Single, height As Single)
  Public MustOverride Sub DrawRectangle(pen As Pen, x As Integer, y As Integer, width As Integer, height As Integer)
  Public MustOverride Sub DrawRectangles(pen As Pen, rects() As RectangleF)
  Public MustOverride Sub DrawRectangles(pen As Pen, rects() As Rectangle)
  Public MustOverride Sub DrawEllipse(pen As Pen, rect As RectangleF)
  Public MustOverride Sub DrawEllipse(pen As Pen, x As Single, y As Single, width As Single, height As Single)
  Public MustOverride Sub DrawEllipse(pen As Pen, rect As Rectangle)
  Public MustOverride Sub DrawEllipse(pen As Pen, x As Integer, y As Integer, width As Integer, height As Integer)
  Public MustOverride Sub DrawBeziers(pen As Pen, points() As System.Drawing.Point)
  Public MustOverride Sub DrawLines(pen As Pen, points() As PointF)
  Public MustOverride Sub DrawLine(pen As Pen, pt1 As PointF, pt2 As PointF)
  Public MustOverride Sub DrawLine(pen As Pen, x1 As Single, y1 As Single, x2 As Single, y2 As Single)
  Public MustOverride Sub CopyFromScreen(upperLeftSource As System.Drawing.Point, upperLeftDestination As System.Drawing.Point, blockRegionSize As Size)
  Public MustOverride Sub CopyFromScreen(sourceX As Integer, sourceY As Integer, destinationX As Integer, destinationY As Integer, blockRegionSize As Size)
  Public MustOverride Sub CopyFromScreen(upperLeftSource As System.Drawing.Point, upperLeftDestination As System.Drawing.Point, blockRegionSize As Size, copyPixelOperation As CopyPixelOperation)
  Public MustOverride Sub CopyFromScreen(sourceX As Integer, sourceY As Integer, destinationX As Integer, destinationY As Integer, blockRegionSize As Size, copyPixelOperation As CopyPixelOperation)
  Public MustOverride Sub ResetTransform()
  Public MustOverride Sub MultiplyTransform(matrix As Matrix)
  Public MustOverride Sub MultiplyTransform(matrix As Matrix, order As MatrixOrder)
  Public MustOverride Sub TranslateTransform(dx As Single, dy As Single)
  Public MustOverride Sub TranslateTransform(dx As Single, dy As Single, order As MatrixOrder)
  Public MustOverride Sub ScaleTransform(sx As Single, sy As Single)
  Public MustOverride Sub ScaleTransform(sx As Single, sy As Single, order As MatrixOrder)
  Public MustOverride Sub RotateTransform(angle As Single)
  Public MustOverride Sub RotateTransform(angle As Single, order As MatrixOrder)
  Public MustOverride Sub TransformPoints(destSpace As CoordinateSpace, srcSpace As CoordinateSpace, pts() As PointF)
  Public MustOverride Sub TransformPoints(destSpace As CoordinateSpace, srcSpace As CoordinateSpace, pts() As System.Drawing.Point)
  Public MustOverride Sub EndContainer(container As GraphicsContainer)
  Public MustOverride Sub DrawPie(pen As Pen, x As Single, y As Single, width As Single, height As Single, startAngle As Single, sweepAngle As Single)
  Public MustOverride Sub DrawPie(pen As Pen, rect As RectangleF, startAngle As Single, sweepAngle As Single)
  Public MustOverride Sub DrawPie(pen As Pen, x As Integer, y As Integer, width As Integer, height As Integer, startAngle As Integer, sweepAngle As Integer)
  Public MustOverride Sub FillEllipse(brush As Brush, rect As RectangleF)
  Public MustOverride Sub FillEllipse(brush As Brush, x As Single, y As Single, width As Single, height As Single)
  Public MustOverride Sub FillEllipse(brush As Brush, rect As Rectangle)
  Public MustOverride Sub FillEllipse(brush As Brush, x As Integer, y As Integer, width As Integer, height As Integer)
  Public MustOverride Sub FillPie(brush As Brush, rect As Rectangle, startAngle As Single, sweepAngle As Single)
  Public MustOverride Sub FillPie(brush As Brush, x As Single, y As Single, width As Single, height As Single, startAngle As Single, sweepAngle As Single)
  Public MustOverride Sub FillPie(brush As Brush, x As Integer, y As Integer, width As Integer, height As Integer, startAngle As Integer, sweepAngle As Integer)
  Public MustOverride Sub FillPath(brush As Brush, path As GraphicsPath)
  Public MustOverride Sub FillClosedCurve(brush As Brush, points() As PointF)
  Public MustOverride Sub FillPolygon(brush As Brush, points() As System.Drawing.Point, fillMode As FillMode)
  Public MustOverride Sub FillClosedCurve(brush As Brush, points() As PointF, fillmode As FillMode)
  Public MustOverride Sub FillClosedCurve(brush As Brush, points() As System.Drawing.Point)
  Public MustOverride Sub FillClosedCurve(brush As Brush, points() As System.Drawing.Point, fillmode As FillMode)
  Public MustOverride Sub FillClosedCurve(brush As Brush, points() As System.Drawing.Point, fillmode As FillMode, tension As Single)
  Public MustOverride Sub FillRegion(brush As Brush, region As Region)
  Public MustOverride Sub DrawString(s As String, font As Font, brush As Brush, x As Single, y As Single)
  Public MustOverride Sub DrawString(s As String, font As Font, brush As Brush, point As PointF)
  Public MustOverride Sub DrawString(s As String, font As Font, brush As Brush, x As Single, y As Single, format As StringFormat)
  Public MustOverride Sub DrawString(s As String, font As Font, brush As Brush, point As PointF, format As StringFormat)
  Public MustOverride Sub DrawString(s As String, font As Font, brush As Brush, layoutRectangle As RectangleF)
  Public MustOverride Sub FillClosedCurve(brush As Brush, points() As PointF, fillmode As FillMode, tension As Single)
  Public MustOverride Sub DrawPie(pen As Pen, rect As Rectangle, startAngle As Single, sweepAngle As Single)
  Public MustOverride Sub FillPolygon(brush As Brush, points() As System.Drawing.Point)
  Public MustOverride Sub FillPolygon(brush As Brush, points() As PointF)
  Public MustOverride Sub DrawPolygon(pen As Pen, points() As PointF)
  Public MustOverride Sub DrawPolygon(pen As Pen, points() As System.Drawing.Point)
  Public MustOverride Sub DrawPath(pen As Pen, path As GraphicsPath)
  Public MustOverride Sub DrawCurve(pen As Pen, points() As PointF)
  Public MustOverride Sub DrawCurve(pen As Pen, points() As PointF, tension As Single)
  Public MustOverride Sub DrawCurve(pen As Pen, points() As PointF, offset As Integer, numberOfSegments As Integer)
  Public MustOverride Sub DrawCurve(pen As Pen, points() As PointF, offset As Integer, numberOfSegments As Integer, tension As Single)
  Public MustOverride Sub DrawCurve(pen As Pen, points() As System.Drawing.Point)
  Public MustOverride Sub DrawCurve(pen As Pen, points() As System.Drawing.Point, tension As Single)
  Public MustOverride Sub FillPolygon(brush As Brush, points() As PointF, fillMode As FillMode)
  Public MustOverride Sub DrawCurve(pen As Pen, points() As System.Drawing.Point, offset As Integer, numberOfSegments As Integer, tension As Single)
  Public MustOverride Sub DrawClosedCurve(pen As Pen, points() As PointF, tension As Single, fillmode As FillMode)
  Public MustOverride Sub DrawClosedCurve(pen As Pen, points() As System.Drawing.Point)
  Public MustOverride Sub DrawClosedCurve(pen As Pen, points() As System.Drawing.Point, tension As Single, fillmode As FillMode)
  Public MustOverride Sub Clear(color As Color)
  Public MustOverride Sub FillRectangle(brush As Brush, rect As RectangleF)
  Public MustOverride Sub FillRectangle(brush As Brush, x As Single, y As Single, width As Single, height As Single)
  Public MustOverride Sub FillRectangle(brush As Brush, rect As Rectangle)
  Public MustOverride Sub FillRectangle(brush As Brush, x As Integer, y As Integer, width As Integer, height As Integer)
  Public MustOverride Sub FillRectangles(brush As Brush, rects() As RectangleF)
  Public MustOverride Sub FillRectangles(brush As Brush, rects() As Rectangle)
  Public MustOverride Sub DrawClosedCurve(pen As Pen, points() As PointF)
  Public MustOverride Sub Restore(gstate As GraphicsState)
  Public MustOverride Function GetNearestColor(color As Color) As Color
  Public MustOverride Function MeasureString(text As String, font As Font, layoutArea As SizeF, stringFormat As StringFormat, ByRef charactersFitted As Integer, ByRef linesFilled As Integer) As SizeF
  Public MustOverride Function MeasureString(text As String, font As Font, origin As PointF, stringFormat As StringFormat) As SizeF
  Public MustOverride Function MeasureString(text As String, font As Font, layoutArea As SizeF) As SizeF
  Public MustOverride Function MeasureString(text As String, font As Font, layoutArea As SizeF, stringFormat As StringFormat) As SizeF
  Public MustOverride Function MeasureString(text As String, font As Font) As SizeF
  Public MustOverride Function MeasureString(text As String, font As Font, width As Integer) As SizeF
  Public MustOverride Function MeasureString(text As String, font As Font, width As Integer, format As StringFormat) As SizeF
  Public MustOverride Function MeasureCharacterRanges(text As String, font As Font, layoutRect As RectangleF, stringFormat As StringFormat) As Region()
  Public MustOverride Function IsVisible(x As Integer, y As Integer) As Boolean
  Public MustOverride Function IsVisible(point As System.Drawing.Point) As Boolean
  Public MustOverride Function IsVisible(x As Single, y As Single) As Boolean
  Public MustOverride Function IsVisible(point As PointF) As Boolean
  Public MustOverride Function BeginContainer() As GraphicsContainer
  Public MustOverride Function BeginContainer(dstrect As RectangleF, srcrect As RectangleF, unit As GraphicsUnit) As GraphicsContainer
  Public MustOverride Function IsVisible(x As Integer, y As Integer, width As Integer, height As Integer) As Boolean
  Public MustOverride Function Save() As GraphicsState
  Public MustOverride Function IsVisible(rect As RectangleF) As Boolean
  Public MustOverride Function IsVisible(rect As Rectangle) As Boolean

#Region "IDisposable Support"
  Public MustOverride Sub Dispose() Implements IDisposable.Dispose
#End Region




End Class

