Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text

Public Class GDIPlus
  Inherits IGraphics

  Private _graphics As Graphics

  Public Overrides Property PageScale As Single
    Get
      Return _graphics.PageScale
    End Get
    Set(value As Single)
      _graphics.PageScale = value
    End Set
  End Property

  Public Overrides Property Transform As Matrix
    Get
      Return _graphics.Transform
    End Get
    Set(value As Matrix)
      _graphics.Transform = value
    End Set
  End Property

  Public Overrides Property PageUnit As GraphicsUnit
    Get
      Return _graphics.PageUnit
    End Get
    Set(value As GraphicsUnit)
      _graphics.PageUnit = value
    End Set
  End Property

  Public Overrides Property SmoothingMode As SmoothingMode
    Get
      Return _graphics.SmoothingMode
    End Get
    Set(value As SmoothingMode)
      _graphics.SmoothingMode = value
    End Set
  End Property

  Public Overrides Property TextContrast As Integer
    Get
      Return _graphics.TextContrast
    End Get
    Set(value As Integer)
      _graphics.TextContrast = value
    End Set
  End Property

  Public Overrides Property TextRenderingHint As TextRenderingHint
    Get
      Return _graphics.TextRenderingHint
    End Get
    Set(value As TextRenderingHint)
      _graphics.TextRenderingHint = value
    End Set
  End Property

  Public Overrides Property CompositingQuality As CompositingQuality
    Get
      Return _graphics.CompositingQuality
    End Get
    Set(value As CompositingQuality)
      _graphics.CompositingQuality = value
    End Set
  End Property

  Public Overrides Property InterpolationMode As InterpolationMode
    Get
      Return _graphics.InterpolationMode
    End Get
    Set(value As InterpolationMode)
      _graphics.InterpolationMode = value
    End Set
  End Property

  Public Overrides ReadOnly Property DpiX As Single
    Get
      Return _graphics.DpiX
    End Get
  End Property

  Public Overrides ReadOnly Property DpiY As Single
    Get
      Return _graphics.DpiY
    End Get
  End Property

  Public Overrides Property PixelOffsetMode As PixelOffsetMode
    Get
      Return _graphics.PixelOffsetMode
    End Get
    Set(value As PixelOffsetMode)
      _graphics.PixelOffsetMode = value
    End Set
  End Property

  Public Overrides Property RenderingOrigin As Drawing.Point
    Get
      Return _graphics.RenderingOrigin
    End Get
    Set(value As Drawing.Point)
      _graphics.RenderingOrigin = value
    End Set
  End Property

  Public Overrides Property CompositingMode As CompositingMode
    Get
      Return _graphics.CompositingQuality
    End Get
    Set(value As CompositingMode)
      _graphics.CompositingMode = value
    End Set
  End Property

  Public Sub New(graphics As Graphics)
    _graphics = graphics
  End Sub

  Public Overrides Sub DrawImage(image As Image, destPoints() As PointF, srcRect As RectangleF, srcUnit As GraphicsUnit, imageAttr As ImageAttributes)
    _graphics.DrawImage(image, destPoints, srcRect, srcUnit, imageAttr)
  End Sub

  Public Overrides Sub DrawImage(image As Image, destPoints() As PointF, srcRect As RectangleF, srcUnit As GraphicsUnit, imageAttr As ImageAttributes, callback As Graphics.DrawImageAbort)
    _graphics.DrawImage(image, destPoints, srcRect, srcUnit, imageAttr, callback)
  End Sub

  Public Overrides Sub DrawImage(image As Image, destPoints() As PointF, srcRect As RectangleF, srcUnit As GraphicsUnit, imageAttr As ImageAttributes, callback As Graphics.DrawImageAbort, callbackData As Integer)
    _graphics.DrawImage(image, destPoints, srcRect, srcUnit, imageAttr, callback, callbackData)
  End Sub

  Public Overrides Sub DrawImage(image As Image, destPoints() As Drawing.Point, srcRect As Rectangle, srcUnit As GraphicsUnit)
    _graphics.DrawImage(image, destPoints, srcRect, srcUnit)
  End Sub

  Public Overrides Sub DrawImage(image As Image, destPoints() As Drawing.Point, srcRect As Rectangle, srcUnit As GraphicsUnit, imageAttr As ImageAttributes)
    _graphics.DrawImage(image, destPoints, srcRect, srcUnit, imageAttr)
  End Sub

  Public Overrides Sub DrawImage(image As Image, destPoints() As Drawing.Point, srcRect As Rectangle, srcUnit As GraphicsUnit, imageAttr As ImageAttributes, callback As Graphics.DrawImageAbort)
    _graphics.DrawImage(image, destPoints, srcRect, srcUnit, imageAttr, callback)
  End Sub

  Public Overrides Sub DrawImage(image As Image, destPoints() As Drawing.Point, srcRect As Rectangle, srcUnit As GraphicsUnit, imageAttr As ImageAttributes, callback As Graphics.DrawImageAbort, callbackData As Integer)
    _graphics.DrawImage(image, destPoints, srcRect, srcUnit, imageAttr, callback, callbackData)
  End Sub

  Public Overrides Sub DrawImage(image As Image, destRect As Rectangle, srcX As Single, srcY As Single, srcWidth As Single, srcHeight As Single, srcUnit As GraphicsUnit)
    _graphics.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit)
  End Sub

  Public Overrides Sub DrawImage(image As Image, destRect As Rectangle, srcX As Single, srcY As Single, srcWidth As Single, srcHeight As Single, srcUnit As GraphicsUnit, imageAttrs As ImageAttributes)
    _graphics.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs)
  End Sub

  Public Overrides Sub DrawImage(image As Image, destRect As Rectangle, srcX As Single, srcY As Single, srcWidth As Single, srcHeight As Single, srcUnit As GraphicsUnit, imageAttrs As ImageAttributes, callback As Graphics.DrawImageAbort, callbackData As IntPtr)
    _graphics.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, callback, callbackData)
  End Sub

  Public Overrides Sub DrawImage(image As Image, destPoints() As PointF, srcRect As RectangleF, srcUnit As GraphicsUnit)
    _graphics.DrawImage(image, destPoints, srcRect, srcUnit)
  End Sub

  Public Overrides Sub DrawImage(image As Image, destRect As Rectangle, srcX As Integer, srcY As Integer, srcWidth As Integer, srcHeight As Integer, srcUnit As GraphicsUnit)
    _graphics.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit)
  End Sub

  Public Overrides Sub DrawImage(image As Image, destRect As Rectangle, srcX As Integer, srcY As Integer, srcWidth As Integer, srcHeight As Integer, srcUnit As GraphicsUnit, imageAttr As ImageAttributes)
    _graphics.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttr)
  End Sub

  Public Overrides Sub DrawImage(image As Image, destRect As Rectangle, srcX As Integer, srcY As Integer, srcWidth As Integer, srcHeight As Integer, srcUnit As GraphicsUnit, imageAttr As ImageAttributes, callback As Graphics.DrawImageAbort)
    _graphics.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttr, callback)
  End Sub

  Public Overrides Sub DrawImage(image As Image, destRect As Rectangle, srcX As Integer, srcY As Integer, srcWidth As Integer, srcHeight As Integer, srcUnit As GraphicsUnit, imageAttrs As ImageAttributes, callback As Graphics.DrawImageAbort, callbackData As IntPtr)
    _graphics.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, callback, callbackData)
  End Sub

  Public Overrides Sub DrawImage(image As Image, destRect As Rectangle, srcX As Single, srcY As Single, srcWidth As Single, srcHeight As Single, srcUnit As GraphicsUnit, imageAttrs As ImageAttributes, callback As Graphics.DrawImageAbort)
    _graphics.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, callback)
  End Sub

  Public Overrides Sub DrawImage(image As Image, destRect As Rectangle, srcRect As Rectangle, srcUnit As GraphicsUnit)
    _graphics.DrawImage(image, destRect, srcRect, srcUnit)
  End Sub

  Public Overrides Sub DrawImage(image As Image, destPoints() As Drawing.Point)
    _graphics.DrawImage(image, destPoints)
  End Sub

  Public Overrides Sub DrawImage(image As Image, x As Integer, y As Integer, srcRect As Rectangle, srcUnit As GraphicsUnit)
    _graphics.DrawImage(image, x, y, srcRect, srcUnit)
  End Sub

  Public Overrides Sub DrawImage(image As Image, point As PointF)
    _graphics.DrawImage(image, point)
  End Sub

  Public Overrides Sub DrawImage(image As Image, x As Single, y As Single)
    _graphics.DrawImage(image, x, y)
  End Sub

  Public Overrides Sub DrawImage(image As Image, rect As RectangleF)
    _graphics.DrawImage(image, rect)
  End Sub

  Public Overrides Sub DrawImage(image As Image, x As Single, y As Single, width As Single, height As Single)
    _graphics.DrawImage(image, x, y, width, height)
  End Sub

  Public Overrides Sub DrawImage(image As Image, x As Integer, y As Integer)
    _graphics.DrawImage(image, x, y)
  End Sub

  Public Overrides Sub DrawImage(image As Image, destRect As RectangleF, srcRect As RectangleF, srcUnit As GraphicsUnit)
    _graphics.DrawImage(image, destRect, srcRect, srcUnit)
  End Sub

  Public Overrides Sub DrawImage(image As Image, rect As Rectangle)
    _graphics.DrawImage(image, rect)
  End Sub

  Public Overrides Sub DrawImage(image As Image, destPoints() As PointF)
    _graphics.DrawImage(image, destPoints)
  End Sub

  Public Overrides Sub DrawImage(image As Image, x As Single, y As Single, srcRect As RectangleF, srcUnit As GraphicsUnit)
    _graphics.DrawImage(image, x, y, srcRect, srcUnit)
  End Sub

  Public Overrides Sub DrawImage(image As Image, x As Integer, y As Integer, width As Integer, height As Integer)
    _graphics.DrawImage(image, x, y, width, height)
  End Sub

  Public Overrides Sub DrawImage(image As Image, point As Drawing.Point)
    _graphics.DrawImage(image, point)
  End Sub

  Public Overrides Sub DrawIcon(icon As Icon, x As Integer, y As Integer)
    _graphics.DrawIcon(icon, x, y)
  End Sub

  Public Overrides Sub DrawIcon(icon As Icon, targetRect As Rectangle)
    _graphics.DrawIcon(icon, targetRect)
  End Sub

  Public Overrides Sub DrawIconUnstretched(icon As Icon, targetRect As Rectangle)
    _graphics.DrawIconUnstretched(icon, targetRect)
  End Sub

  Public Overrides Sub DrawImageUnscaled(image As Image, point As Drawing.Point)
    _graphics.DrawImageUnscaled(image, point)
  End Sub

  Public Overrides Sub DrawImageUnscaled(image As Image, x As Integer, y As Integer)
    _graphics.DrawImageUnscaled(image, x, y)
  End Sub

  Public Overrides Sub DrawImageUnscaled(image As Image, rect As Rectangle)
    _graphics.DrawImageUnscaled(image, rect)
  End Sub

  Public Overrides Sub DrawImageUnscaled(image As Image, x As Integer, y As Integer, width As Integer, height As Integer)
    _graphics.DrawImageUnscaled(image, x, y, width, height)
  End Sub

  Public Overrides Sub DrawImageUnscaledAndClipped(image As Image, rect As Rectangle)
    _graphics.DrawImageUnscaledAndClipped(image, rect)
  End Sub

  Public Overrides Sub TranslateClip(dx As Integer, dy As Integer)
    _graphics.TranslateClip(dx, dy)
  End Sub

  Public Overrides Sub TranslateClip(dx As Single, dy As Single)
    _graphics.TranslateClip(dx, dy)
  End Sub

  Public Overrides Sub SetClip(g As Graphics)
    _graphics.SetClip(g)
  End Sub

  Public Overrides Sub SetClip(g As Graphics, combineMode As CombineMode)
    _graphics.SetClip(g, combineMode)
  End Sub

  Public Overrides Sub SetClip(rect As Rectangle)
    _graphics.SetClip(rect)
  End Sub

  Public Overrides Sub SetClip(rect As Rectangle, combineMode As CombineMode)
    _graphics.SetClip(rect, combineMode)
  End Sub

  Public Overrides Sub SetClip(rect As RectangleF)
    _graphics.SetClip(rect)
  End Sub

  Public Overrides Sub SetClip(rect As RectangleF, combineMode As CombineMode)
    _graphics.SetClip(rect, combineMode)
  End Sub

  Public Overrides Sub SetClip(path As GraphicsPath, combineMode As CombineMode)
    _graphics.SetClip(path, combineMode)
  End Sub

  Public Overrides Sub SetClip(region As Region, combineMode As CombineMode)
    _graphics.SetClip(region, combineMode)
  End Sub

  Public Overrides Sub SetClip(path As GraphicsPath)
    _graphics.SetClip(path)
  End Sub

  Public Overrides Sub IntersectClip(rect As Rectangle)
    _graphics.IntersectClip(rect)
  End Sub

  Public Overrides Sub IntersectClip(rect As RectangleF)
    _graphics.IntersectClip(rect)
  End Sub

  Public Overrides Sub IntersectClip(region As Region)
    _graphics.IntersectClip(region)
  End Sub

  Public Overrides Sub ExcludeClip(rect As Rectangle)
    _graphics.ExcludeClip(rect)
  End Sub

  Public Overrides Sub ExcludeClip(region As Region)
    _graphics.ExcludeClip(region)
  End Sub

  Public Overrides Sub ResetClip()
    _graphics.ResetClip()
  End Sub

  Public Overrides Sub DrawString(s As String, font As Font, brush As Brush, layoutRectangle As RectangleF, format As StringFormat)
    _graphics.DrawString(s, font, brush, layoutRectangle, format)
  End Sub

  Public Overrides Sub DrawString(s As String, font As Font, brush As Brush, x As Single, y As Single)
    _graphics.DrawString(s, font, brush, x, y)
  End Sub

  Public Overrides Sub DrawString(s As String, font As Font, brush As Brush, point As PointF)
    _graphics.DrawString(s, font, brush, point)
  End Sub

  Public Overrides Sub DrawString(s As String, font As Font, brush As Brush, x As Single, y As Single, format As StringFormat)
    _graphics.DrawString(s, font, brush, x, y, format)
  End Sub

  Public Overrides Sub DrawString(s As String, font As Font, brush As Brush, point As PointF, format As StringFormat)
    _graphics.DrawString(s, font, brush, point, format)
  End Sub

  Public Overrides Sub DrawString(s As String, font As Font, brush As Brush, layoutRectangle As RectangleF)
    _graphics.DrawString(s, font, brush, layoutRectangle)
  End Sub

  Public Overrides Sub DrawLine(pen As Pen, pt1 As Drawing.Point, pt2 As Drawing.Point)
    _graphics.DrawLine(pen, pt1, pt2)
  End Sub

  Public Overrides Sub DrawLine(pen As Pen, x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer)
    _graphics.DrawLine(pen, x1, y1, x2, y2)
  End Sub

  Public Overrides Sub DrawLine(pen As Pen, pt1 As PointF, pt2 As PointF)
    _graphics.DrawLine(pen, pt1, pt2)
  End Sub

  Public Overrides Sub DrawLine(pen As Pen, x1 As Single, y1 As Single, x2 As Single, y2 As Single)
    _graphics.DrawLine(pen, x1, y1, x2, y2)
  End Sub

  Public Overrides Sub DrawLines(pen As Pen, points() As Drawing.Point)
    _graphics.DrawLines(pen, points)
  End Sub

  Public Overrides Sub DrawLines(pen As Pen, points() As PointF)
    _graphics.DrawLines(pen, points)
  End Sub

  Public Overrides Sub DrawArc(pen As Pen, x As Single, y As Single, width As Single, height As Single, startAngle As Single, sweepAngle As Single)
    _graphics.DrawArc(pen, x, y, width, height, startAngle, sweepAngle)
  End Sub

  Public Overrides Sub DrawArc(pen As Pen, rect As RectangleF, startAngle As Single, sweepAngle As Single)
    _graphics.DrawArc(pen, rect, startAngle, sweepAngle)
  End Sub

  Public Overrides Sub DrawArc(pen As Pen, x As Integer, y As Integer, width As Integer, height As Integer, startAngle As Integer, sweepAngle As Integer)
    _graphics.DrawArc(pen, x, y, width, height, startAngle, sweepAngle)
  End Sub

  Public Overrides Sub DrawArc(pen As Pen, rect As Rectangle, startAngle As Single, sweepAngle As Single)
    _graphics.DrawArc(pen, rect, startAngle, sweepAngle)
  End Sub

  Public Overrides Sub DrawBezier(pen As Pen, x1 As Single, y1 As Single, x2 As Single, y2 As Single, x3 As Single, y3 As Single, x4 As Single, y4 As Single)
    _graphics.DrawBezier(pen, x1, y1, x2, y2, x3, y3, x4, y4)
  End Sub

  Public Overrides Sub DrawBezier(pen As Pen, pt1 As PointF, pt2 As PointF, pt3 As PointF, pt4 As PointF)
    _graphics.DrawBezier(pen, pt1, pt2, pt3, pt4)
  End Sub

  Public Overrides Sub DrawBezier(pen As Pen, pt1 As Drawing.Point, pt2 As Drawing.Point, pt3 As Drawing.Point, pt4 As Drawing.Point)
    _graphics.DrawBezier(pen, pt1, pt2, pt3, pt4)
  End Sub

  Public Overrides Sub DrawBeziers(pen As Pen, points() As PointF)
    _graphics.DrawBeziers(pen, points)
  End Sub

  Public Overrides Sub DrawBeziers(pen As Pen, points() As Drawing.Point)
    _graphics.DrawBeziers(pen, points)
  End Sub

  Public Overrides Sub DrawRectangle(pen As Pen, rect As Rectangle)
    _graphics.DrawRectangle(pen, rect)
  End Sub

  Public Overrides Sub DrawRectangle(pen As Pen, x As Single, y As Single, width As Single, height As Single)
    _graphics.DrawRectangle(pen, x, y, width, height)
  End Sub

  Public Overrides Sub DrawRectangle(pen As Pen, x As Integer, y As Integer, width As Integer, height As Integer)
    _graphics.DrawRectangle(pen, x, y, width, height)
  End Sub

  Public Overrides Sub DrawRectangles(pen As Pen, rects() As RectangleF)
    _graphics.DrawRectangles(pen, rects)
  End Sub

  Public Overrides Sub DrawRectangles(pen As Pen, rects() As Rectangle)
    _graphics.DrawRectangles(pen, rects)
  End Sub

  Public Overrides Sub DrawEllipse(pen As Pen, rect As RectangleF)
    _graphics.DrawEllipse(pen, rect)
  End Sub

  Public Overrides Sub DrawEllipse(pen As Pen, x As Single, y As Single, width As Single, height As Single)
    _graphics.DrawEllipse(pen, x, y, width, height)
  End Sub

  Public Overrides Sub DrawEllipse(pen As Pen, rect As Rectangle)
    _graphics.DrawEllipse(pen, rect)
  End Sub

  Public Overrides Sub DrawEllipse(pen As Pen, x As Integer, y As Integer, width As Integer, height As Integer)
    _graphics.DrawEllipse(pen, x, y, width, height)
  End Sub

  Public Overrides Sub CopyFromScreen(upperLeftSource As Drawing.Point, upperLeftDestination As Drawing.Point, blockRegionSize As Size)
    _graphics.CopyFromScreen(upperLeftSource, upperLeftDestination, blockRegionSize)
  End Sub

  Public Overrides Sub CopyFromScreen(sourceX As Integer, sourceY As Integer, destinationX As Integer, destinationY As Integer, blockRegionSize As Size)
    _graphics.CopyFromScreen(sourceX, sourceY, destinationX, destinationY, blockRegionSize)
  End Sub

  Public Overrides Sub CopyFromScreen(upperLeftSource As Drawing.Point, upperLeftDestination As Drawing.Point, blockRegionSize As Size, copyPixelOperation As CopyPixelOperation)
    _graphics.CopyFromScreen(upperLeftSource, upperLeftDestination, blockRegionSize, copyPixelOperation)
  End Sub

  Public Overrides Sub CopyFromScreen(sourceX As Integer, sourceY As Integer, destinationX As Integer, destinationY As Integer, blockRegionSize As Size, copyPixelOperation As CopyPixelOperation)
    _graphics.CopyFromScreen(sourceX, sourceY, destinationX, destinationY, blockRegionSize, copyPixelOperation)
  End Sub

  Public Overrides Sub ResetTransform()
    _graphics.ResetTransform()
  End Sub

  Public Overrides Sub MultiplyTransform(matrix As Matrix)
    _graphics.MultiplyTransform(matrix)
  End Sub

  Public Overrides Sub MultiplyTransform(matrix As Matrix, order As MatrixOrder)
    _graphics.MultiplyTransform(matrix, order)
  End Sub

  Public Overrides Sub TranslateTransform(dx As Single, dy As Single)
    _graphics.TranslateTransform(dx, dy)
  End Sub

  Public Overrides Sub TranslateTransform(dx As Single, dy As Single, order As MatrixOrder)
    _graphics.TranslateTransform(dx, dy, order)
  End Sub

  Public Overrides Sub ScaleTransform(sx As Single, sy As Single)
    _graphics.ScaleTransform(sx, sy)
  End Sub

  Public Overrides Sub ScaleTransform(sx As Single, sy As Single, order As MatrixOrder)
    _graphics.ScaleTransform(sx, sy, order)
  End Sub

  Public Overrides Sub RotateTransform(angle As Single)
    _graphics.RotateTransform(angle)
  End Sub

  Public Overrides Sub RotateTransform(angle As Single, order As MatrixOrder)
    _graphics.RotateTransform(angle, order)
  End Sub

  Public Overrides Sub TransformPoints(destSpace As CoordinateSpace, srcSpace As CoordinateSpace, pts() As PointF)
    _graphics.TransformPoints(destSpace, srcSpace, pts)
  End Sub

  Public Overrides Sub TransformPoints(destSpace As CoordinateSpace, srcSpace As CoordinateSpace, pts() As Drawing.Point)
    _graphics.TransformPoints(destSpace, srcSpace, pts)
  End Sub

  Public Overrides Sub EndContainer(container As GraphicsContainer)
    _graphics.EndContainer(container)
  End Sub

  Public Overrides Sub DrawPie(pen As Pen, x As Single, y As Single, width As Single, height As Single, startAngle As Single, sweepAngle As Single)
    _graphics.DrawPie(pen, x, y, width, height, startAngle, sweepAngle)
  End Sub

  Public Overrides Sub DrawPie(pen As Pen, rect As RectangleF, startAngle As Single, sweepAngle As Single)
    _graphics.DrawPie(pen, rect, startAngle, sweepAngle)
  End Sub

  Public Overrides Sub DrawPie(pen As Pen, x As Integer, y As Integer, width As Integer, height As Integer, startAngle As Integer, sweepAngle As Integer)
    _graphics.DrawPie(pen, x, y, width, height, startAngle, sweepAngle)
  End Sub

  Public Overrides Sub DrawPie(pen As Pen, rect As Rectangle, startAngle As Single, sweepAngle As Single)
    _graphics.DrawPie(pen, rect, startAngle, sweepAngle)
  End Sub

  Public Overrides Sub FillEllipse(brush As Brush, rect As RectangleF)
    _graphics.FillEllipse(brush, rect)
  End Sub

  Public Overrides Sub FillEllipse(brush As Brush, x As Single, y As Single, width As Single, height As Single)
    _graphics.FillEllipse(brush, x, y, width, height)
  End Sub

  Public Overrides Sub FillEllipse(brush As Brush, rect As Rectangle)
    _graphics.FillEllipse(brush, rect)
  End Sub

  Public Overrides Sub FillEllipse(brush As Brush, x As Integer, y As Integer, width As Integer, height As Integer)
    _graphics.FillEllipse(brush, x, y, width, height)
  End Sub

  Public Overrides Sub FillPie(brush As Brush, rect As Rectangle, startAngle As Single, sweepAngle As Single)
    _graphics.FillPie(brush, rect, startAngle, sweepAngle)
  End Sub

  Public Overrides Sub FillPie(brush As Brush, x As Single, y As Single, width As Single, height As Single, startAngle As Single, sweepAngle As Single)
    _graphics.FillPie(brush, x, y, width, height, startAngle, sweepAngle)
  End Sub

  Public Overrides Sub FillPie(brush As Brush, x As Integer, y As Integer, width As Integer, height As Integer, startAngle As Integer, sweepAngle As Integer)
    _graphics.FillPie(brush, x, y, width, height, startAngle, sweepAngle)
  End Sub

  Public Overrides Sub FillPath(brush As Brush, path As GraphicsPath)
    _graphics.FillPath(brush, path)
  End Sub

  Public Overrides Sub FillClosedCurve(brush As Brush, points() As PointF)
    _graphics.FillClosedCurve(brush, points)
  End Sub

  Public Overrides Sub FillClosedCurve(brush As Brush, points() As PointF, fillmode As FillMode)
    _graphics.FillClosedCurve(brush, points, fillmode)
  End Sub

  Public Overrides Sub FillClosedCurve(brush As Brush, points() As Drawing.Point)
    _graphics.FillClosedCurve(brush, points)
  End Sub

  Public Overrides Sub FillClosedCurve(brush As Brush, points() As Drawing.Point, fillmode As FillMode)
    _graphics.FillClosedCurve(brush, points, fillmode)
  End Sub

  Public Overrides Sub FillClosedCurve(brush As Brush, points() As Drawing.Point, fillmode As FillMode, tension As Single)
    _graphics.FillClosedCurve(brush, points, fillmode)
  End Sub

  Public Overrides Sub FillClosedCurve(brush As Brush, points() As PointF, fillmode As FillMode, tension As Single)
    _graphics.FillClosedCurve(brush, points, fillmode, tension)
  End Sub

  Public Overrides Sub FillPolygon(brush As Brush, points() As Drawing.Point, fillMode As FillMode)
    _graphics.FillPolygon(brush, points, fillMode)
  End Sub

  Public Overrides Sub FillPolygon(brush As Brush, points() As Drawing.Point)
    _graphics.FillPolygon(brush, points)
  End Sub

  Public Overrides Sub FillPolygon(brush As Brush, points() As PointF)
    _graphics.FillPolygon(brush, points)
  End Sub

  Public Overrides Sub FillPolygon(brush As Brush, points() As PointF, fillMode As FillMode)
    _graphics.FillPolygon(brush, points, fillMode)
  End Sub

  Public Overrides Sub FillRegion(brush As Brush, region As Region)
    _graphics.FillRegion(brush, region)
  End Sub

  Public Overrides Sub DrawPolygon(pen As Pen, points() As PointF)
    _graphics.DrawPolygon(pen, points)
  End Sub

  Public Overrides Sub DrawPolygon(pen As Pen, points() As Drawing.Point)
    _graphics.DrawPolygon(pen, points)
  End Sub

  Public Overrides Sub DrawPath(pen As Pen, path As GraphicsPath)
    _graphics.DrawPath(pen, path)
  End Sub

  Public Overrides Sub DrawCurve(pen As Pen, points() As PointF)
    _graphics.DrawCurve(pen, points)
  End Sub

  Public Overrides Sub DrawCurve(pen As Pen, points() As PointF, tension As Single)
    _graphics.DrawCurve(pen, points, tension)
  End Sub

  Public Overrides Sub DrawCurve(pen As Pen, points() As PointF, offset As Integer, numberOfSegments As Integer)
    _graphics.DrawCurve(pen, points, offset, numberOfSegments)
  End Sub

  Public Overrides Sub DrawCurve(pen As Pen, points() As PointF, offset As Integer, numberOfSegments As Integer, tension As Single)
    _graphics.DrawCurve(pen, points, offset, numberOfSegments, tension)
  End Sub

  Public Overrides Sub DrawCurve(pen As Pen, points() As Drawing.Point)
    _graphics.DrawCurve(pen, points)
  End Sub

  Public Overrides Sub DrawCurve(pen As Pen, points() As Drawing.Point, tension As Single)
    _graphics.DrawCurve(pen, points, tension)
  End Sub

  Public Overrides Sub DrawCurve(pen As Pen, points() As Drawing.Point, offset As Integer, numberOfSegments As Integer, tension As Single)
    _graphics.DrawCurve(pen, points, offset, numberOfSegments, tension)
  End Sub

  Public Overrides Sub DrawClosedCurve(pen As Pen, points() As PointF, tension As Single, fillmode As FillMode)
    _graphics.DrawClosedCurve(pen, points, tension, fillmode)
  End Sub

  Public Overrides Sub DrawClosedCurve(pen As Pen, points() As Drawing.Point)
    _graphics.DrawClosedCurve(pen, points)
  End Sub

  Public Overrides Sub DrawClosedCurve(pen As Pen, points() As Drawing.Point, tension As Single, fillmode As FillMode)
    _graphics.DrawClosedCurve(pen, points, tension, fillmode)
  End Sub

  Public Overrides Sub DrawClosedCurve(pen As Pen, points() As PointF)
    _graphics.DrawClosedCurve(pen, points)
  End Sub

  Public Overrides Sub Clear(color As Color)
    _graphics.Clear(color)
  End Sub

  Public Overrides Sub FillRectangle(brush As Brush, rect As RectangleF)
    _graphics.FillRectangle(brush, rect)
  End Sub

  Public Overrides Sub FillRectangle(brush As Brush, x As Single, y As Single, width As Single, height As Single)
    _graphics.FillRectangle(brush, x, y, width, height)
  End Sub

  Public Overrides Sub FillRectangle(brush As Brush, rect As Rectangle)
    _graphics.FillRectangle(brush, rect)
  End Sub

  Public Overrides Sub FillRectangle(brush As Brush, x As Integer, y As Integer, width As Integer, height As Integer)
    _graphics.FillRectangle(brush, x, y, width, height)
  End Sub

  Public Overrides Sub FillRectangles(brush As Brush, rects() As RectangleF)
    _graphics.FillRectangles(brush, rects)
  End Sub

  Public Overrides Sub FillRectangles(brush As Brush, rects() As Rectangle)
    _graphics.FillRectangles(brush, rects)
  End Sub

  Public Overrides Sub Restore(gstate As GraphicsState)
    _graphics.Restore(gstate)
  End Sub

  Public Overrides Sub Dispose()
    _graphics.Dispose()
  End Sub

  Public Overrides Function GetNearestColor(color As Color) As Color
    Return _graphics.GetNearestColor(color)
  End Function

  Public Overrides Function MeasureString(text As String, font As Font, layoutArea As SizeF, stringFormat As StringFormat, ByRef charactersFitted As Integer, ByRef linesFilled As Integer) As SizeF
    Return _graphics.MeasureString(text, font, layoutArea, stringFormat, charactersFitted, linesFilled)
  End Function

  Public Overrides Function MeasureString(text As String, font As Font, origin As PointF, stringFormat As StringFormat) As SizeF
    Return _graphics.MeasureString(text, font, origin, stringFormat)
  End Function

  Public Overrides Function MeasureString(text As String, font As Font, layoutArea As SizeF) As SizeF
    Return _graphics.MeasureString(text, font, layoutArea)
  End Function

  Public Overrides Function MeasureString(text As String, font As Font, layoutArea As SizeF, stringFormat As StringFormat) As SizeF
    Return _graphics.MeasureString(text, font, layoutArea, stringFormat)
  End Function

  Public Overrides Function MeasureString(text As String, font As Font) As SizeF
    Return _graphics.MeasureString(text, font)
  End Function

  Public Overrides Function MeasureString(text As String, font As Font, width As Integer) As SizeF
    Return _graphics.MeasureString(text, font, width)
  End Function

  Public Overrides Function MeasureString(text As String, font As Font, width As Integer, format As StringFormat) As SizeF
    Return _graphics.MeasureString(text, font, width, format)
  End Function

  Public Overrides Function MeasureCharacterRanges(text As String, font As Font, layoutRect As RectangleF, stringFormat As StringFormat) As Region()
    Return _graphics.MeasureCharacterRanges(text, font, layoutRect, stringFormat)
  End Function

  Public Overrides Function IsVisible(x As Integer, y As Integer) As Boolean
    Return _graphics.IsVisible(x, y)
  End Function

  Public Overrides Function IsVisible(point As Drawing.Point) As Boolean
    Return _graphics.IsVisible(point)
  End Function

  Public Overrides Function IsVisible(x As Single, y As Single) As Boolean
    Return _graphics.IsVisible(x, y)
  End Function

  Public Overrides Function IsVisible(point As PointF) As Boolean
    Return _graphics.IsVisible(point)
  End Function

  Public Overrides Function IsVisible(x As Integer, y As Integer, width As Integer, height As Integer) As Boolean
    Return _graphics.IsVisible(x, y, width, height)
  End Function

  Public Overrides Function IsVisible(rect As RectangleF) As Boolean
    Return _graphics.IsVisible(rect)
  End Function

  Public Overrides Function IsVisible(rect As Rectangle) As Boolean
    Return _graphics.IsVisible(rect)
  End Function

  Public Overrides Function BeginContainer() As GraphicsContainer
    Return _graphics.BeginContainer()
  End Function

  Public Overrides Function BeginContainer(dstrect As RectangleF, srcrect As RectangleF, unit As GraphicsUnit) As GraphicsContainer
    Return _graphics.BeginContainer(dstrect, srcrect, unit)
  End Function

  Public Overrides Function Save() As GraphicsState
    Return _graphics.Save()
  End Function
End Class
