Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports SharpDX
Imports SharpDX.Mathematics
Imports SharpDX.DXGI
Imports SharpDX.Direct2D1
Imports System.Numerics
Imports System.Reflection
Imports Utility.Memory
Imports System.Runtime.InteropServices

Public Class Direct2D
  Inherits IGraphics

  Public Overrides Property PageScale As Single
    Get
      Throw New NotImplementedException()
    End Get
    Set(value As Single)
      Throw New NotImplementedException()
    End Set
  End Property

  Public Overrides Property Transform As Matrix
    Get
      Throw New NotImplementedException()
    End Get
    Set(value As Matrix)
      Throw New NotImplementedException()
    End Set
  End Property

  Public Overrides Property PageUnit As GraphicsUnit
    Get
      Throw New NotImplementedException()
    End Get
    Set(value As GraphicsUnit)
      Throw New NotImplementedException()
    End Set
  End Property

  Public Overrides Property SmoothingMode As SmoothingMode
    Get
      Throw New NotImplementedException()
    End Get
    Set(value As SmoothingMode)
      Throw New NotImplementedException()
    End Set
  End Property

  Public Overrides Property TextContrast As Integer
    Get
      Throw New NotImplementedException()
    End Get
    Set(value As Integer)
      Throw New NotImplementedException()
    End Set
  End Property

  Public Overrides Property TextRenderingHint As TextRenderingHint
    Get
      Throw New NotImplementedException()
    End Get
    Set(value As TextRenderingHint)
      Throw New NotImplementedException()
    End Set
  End Property

  Public Overrides Property CompositingQuality As CompositingQuality
    Get
      Throw New NotImplementedException()
    End Get
    Set(value As CompositingQuality)
      Throw New NotImplementedException()
    End Set
  End Property

  Public Overrides Property InterpolationMode As Drawing2D.InterpolationMode
    Get
      Throw New NotImplementedException()
    End Get
    Set(value As Drawing2D.InterpolationMode)
      Throw New NotImplementedException()
    End Set
  End Property

  Public Overrides ReadOnly Property DpiX As Single
    Get
      Throw New NotImplementedException()
    End Get
  End Property

  Public Overrides ReadOnly Property DpiY As Single
    Get
      Throw New NotImplementedException()
    End Get
  End Property

  Public Overrides Property PixelOffsetMode As PixelOffsetMode
    Get
      Throw New NotImplementedException()
    End Get
    Set(value As PixelOffsetMode)
      Throw New NotImplementedException()
    End Set
  End Property

  Public Overrides Property RenderingOrigin As Drawing.Point
    Get
      Throw New NotImplementedException()
    End Get
    Set(value As Drawing.Point)
      Throw New NotImplementedException()
    End Set
  End Property

  Public Overrides Property CompositingMode As CompositingMode
    Get
      Throw New NotImplementedException()
    End Get
    Set(value As CompositingMode)
      Throw New NotImplementedException()
    End Set
  End Property

#Region "Custom Direct2D Logic"
  Private _graphics As Graphics
  Private _graphicsForTransform As Graphics
  Private ReadOnly Property GraphicsForTransform As Graphics
    Get
      If _graphicsForTransform Is Nothing Then
        Dim bm = New Drawing.Bitmap(1, 1)

        _graphicsForTransform = Graphics.FromImage(bm)
      End If

      Return _graphicsForTransform
    End Get
  End Property
  Private Shared _targets As New List(Of DeviceContextRenderTarget)
  Private Shared _factories As New List(Of Direct2D1.Factory)
  Private Shared _targetHDCs As New List(Of IntPtr)
  Private Shared _targetBeingUsed As New List(Of Boolean)
  Private Shared _targetBoundss As New List(Of RectangleF)
  Private Shared _dashCapDictionary As Dictionary(Of DashCap, CapStyle) = New Dictionary(Of DashCap, CapStyle) From {{DashCap.Flat, CapStyle.Flat}, {DashCap.Round, CapStyle.Round}, {DashCap.Triangle, CapStyle.Triangle}}
  Private Shared _lineCapDictionary As Dictionary(Of LineCap, CapStyle) = New Dictionary(Of LineCap, CapStyle) From {{LineCap.Flat, CapStyle.Flat}, {LineCap.Round, CapStyle.Round}, {LineCap.Triangle, CapStyle.Triangle}, {LineCap.Square, CapStyle.Square}}

  Private _selectedTargetIdx As Integer = -1
  Private _hdc As IntPtr
  Private _hasBegunDrawing As Boolean = False



  Shared Sub New()
    'Notify
  End Sub

  Public Sub New(graphics As Graphics)
    _graphics = graphics

    'BeginDraw()
  End Sub

  Public Overrides Sub Dispose()
    EndDraw()
  End Sub

  ' True if Ready to draw.
  ' CAUTION: Even if returns False, Caller must call EndDraw, so that ReleaseHDC is called.
  Public Function BeginDraw() As Boolean
    _hasBegunDrawing = True
    ' CAUTION: After this, must call EndDraw or ReleaseHDC when done drawing.
    Dim canvas = _graphics.VisibleClipBounds
    Me._hdc = _graphics.GetHdc()


    Dim ready = EnsureReady(canvas)
    If Not ready Then
      ' Initialization failed.
      Return False
    End If

    Try
      Dim success As Boolean = True
      _targets(_selectedTargetIdx).BeginDraw()

      Return success

    Catch ex As Exception
      Return False
    End Try
  End Function

  Public Function Test_Render(img As System.Drawing.Bitmap, success As Boolean) As Boolean
    Try
      _targets(_selectedTargetIdx).Transform = New Interop.RawMatrix3x2(1, 0, 0, 1, 0, 0)
      _targets(_selectedTargetIdx).Clear(New Interop.RawColor4(Color.Transparent.R, Color.Transparent.G, Color.Transparent.B, Color.Transparent.A))
      Dim brushProperties = New BrushProperties()
      'Dim brush As Direct2D1.Brush = New SolidColorBrush(_targets(_selectedTargetIdx), New Interop.RawColor4(Color.Black.R, Color.Black.G, Color.Black.B, Color.Black.A))
      'Dim ellipse As Direct2D1.Ellipse = New Direct2D1.Ellipse() With {
      '        .Point = New Interop.RawVector2(100, 100),
      '        .RadiusX = 80, .RadiusY = 80}
      'Target.DrawEllipse(ellipse, brush)
      'Target.FillEllipse(ellipse, brush)
      '_targets(_selectedTargetIdx).DrawLine(New Interop.RawVector2(100, 0), New Interop.RawVector2(200, 100), brush, 1)
      'DrawLine(New Pen(Color.Green), New Drawing.Point(100, 0), New Drawing.Point(200, 100))

      Dim diskImg = Drawing.Image.FromFile("C:\Users\anton\Documents\devnull\z.bmp")
      DrawImageUnscaled(img, 0, 0, img.Width, img.Height)
    Catch ex As Exception
      success = False
    End Try

    Return success
  End Function

  ' True if rendering succeeds.
  ' "success" is accumulation, included in the return value.
  Public Function EndDraw() As Boolean
    If Not _hasBegunDrawing Then Return True

    If _selectedTargetIdx <> -1 AndAlso _targets(_selectedTargetIdx) IsNot Nothing Then
      ' Wrap EndDraw in Try, because "ReleaseHDC" must always be called.
      Dim success As Boolean
      Try
        ' EndDraw is always called (even if "success" is already False).
        Dim tag1 As Long
        Dim tag2 As Long
        success = _targets(_selectedTargetIdx).TryEndDraw(tag1, tag2).Success
      Catch ex As Exception
        success = False
      End Try

      ReleaseHDC()
      ' TBD: This could be moved out elsewhere.
      'EnsureFactoryReleased()

      If Not success Then
        'Trouble()
      End If
      Return success
    Else

      ReleaseHDC()
      ' TBD: This could be moved out elsewhere.
      'EnsureFactoryReleased()
      Return True
    End If

  End Function

  ' CAUTION: Caller must call EndDraw or ReleaseHDC when done drawing.
  Private Function EnsureReady(canvas As RectangleF) As Boolean
    Dim makeNew As Boolean = False
    Dim oldSelectedTargetIdx = _selectedTargetIdx
    'Either we haven't made a target yet or the current target is being used, so we have to make a new target.
    If _selectedTargetIdx = -1 OrElse (_targetHDCs(_selectedTargetIdx) <> Nothing AndAlso _hdc <> _targetHDCs(_selectedTargetIdx)) Then
      'Find a suitable target index to use.
      For i As Integer = 0 To _targets.Count - 1
        If _targetHDCs(i) = Nothing OrElse Not _targetBeingUsed(i) Then
          _selectedTargetIdx = i
          Exit For
        End If
      Next
      makeNew = _selectedTargetIdx = -1 OrElse _selectedTargetIdx = oldSelectedTargetIdx


    End If

    Dim ready As Boolean

    If makeNew Then
      Dispose()
      Me._hdc = _graphics.GetHdc()

      ready = InitializeDevice(canvas)
    ElseIf (Not SameBounds(canvas) AndAlso _hdc = _targetHDCs(_selectedTargetIdx)) OrElse _targetHDCs(_selectedTargetIdx) <> _hdc Then
      Try
        'TestStr = Me.Hdc.ToString()
        _targets(_selectedTargetIdx).BindDeviceContext(Me._hdc, New Interop.RawRectangle(canvas.Left, canvas.Top, canvas.Right, canvas.Bottom))
        _targetHDCs(_selectedTargetIdx) = _hdc
        _targetBoundss(_selectedTargetIdx) = canvas
        _targetBeingUsed(_selectedTargetIdx) = True
        ready = True

      Catch ex As Exception
        ReleaseHDC()
        ready = False
      End Try

    End If

    Return ready
  End Function

  ' AFTER set Me.Bounds.
  ' CAUTION: Caller must call g.ReleaseHdc(Me.Hdc) when done drawing.
  Private Function InitializeDevice(canvas As RectangleF) As Boolean
    Try
      '' Stand-alone D2D window (NOT to GDI)
      ' ...width As Integer, height As Integer
      'Dim windowProperties As New WindowRenderTargetProperties(handle, New Size(600, 600))
      'Dim target1 As New WindowRenderTarget(factory, windowProperties)

      Dim targetProperties As New RenderTargetProperties
      targetProperties.Type = RenderTargetType.Default
      targetProperties.PixelFormat = New Direct2D1.PixelFormat(Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied)
      targetProperties.Usage = RenderTargetUsage.GdiCompatible
      ' Equivalent to "ID2D1Factory::CreateDCRenderTarget".
      Dim factory = New Direct2D1.Factory()
      _targets.Add(New DeviceContextRenderTarget(factory, targetProperties))
      _factories.Add(factory)
      _targetHDCs.Add(_hdc)
      _targetBoundss.Add(canvas)
      _targetBeingUsed.Add(True)
      _selectedTargetIdx = _targets.Count - 1
      Try
        'TestStr = Me.Hdc.ToString()
        _targets(_selectedTargetIdx).BindDeviceContext(Me._hdc, New Interop.RawRectangle(_targetBoundss(_selectedTargetIdx).Left, _targetBoundss(_selectedTargetIdx).Top, _targetBoundss(_selectedTargetIdx).Right, _targetBoundss(_selectedTargetIdx).Bottom))

        'If Not result.IsSuccess Then
        'ReleaseHDC(g)
        'End If
        Return True 'result.IsSuccess

      Catch ex As Exception
        ReleaseHDC()
        Return False
      End Try

    Catch ex As Exception
      Return False
    End Try
  End Function

  Private Sub ReleaseHDC()
    Try
      _graphics.ReleaseHdc(Me._hdc)
    Finally
      Me._hdc = Nothing
      If _selectedTargetIdx <> -1 Then
        _targetBeingUsed(_selectedTargetIdx) = False
        '_targetHDCs(_selectedTargetIdx) = Nothing
      End If

    End Try
  End Sub

  Private Function SameBounds(newBounds As RectangleF) As Boolean
    ' TBD: Does Equals do what we need?
    Return (newBounds.Equals(_targetBoundss))
  End Function

  Private Function DrawingRectToD2DRectF(rect As Rectangle) As Interop.RawRectangleF
    Return New Interop.RawRectangleF(rect.Left, rect.Top, rect.Right, rect.Bottom)
  End Function

  Private Function DrawingColorToD2DColor(color As Color) As Interop.RawColor4
    Return New Interop.RawColor4(color.R / 255, color.G / 255, color.B / 255, color.A / 255)
  End Function

  Private Function DrawingBrushToD2DBrush(brush As Drawing.Brush) As Direct2D1.Brush
    If brush.GetType() = GetType(SolidBrush) Then
      Dim sbrush As SolidBrush = CType(brush, SolidBrush)
      Dim color = sbrush.Color
      Return New Direct2D1.SolidColorBrush(_targets(_selectedTargetIdx), DrawingColorToD2DColor(color))
    Else
      Debug.Print("Don't support the following brush type in Direct2D yet: " + brush.GetType().Name)
      'Return black brush..
      Return New Direct2D1.SolidColorBrush(_targets(_selectedTargetIdx), New Interop.RawColor4(0, 0, 0, 255))
    End If
  End Function

  Private Function GetDirect2DBitmap(bmp As System.Drawing.Bitmap) As SharpDX.Direct2D1.Bitmap
    Dim bmpData As System.Drawing.Imaging.BitmapData = bmp.LockBits(
                New System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb)
    Dim stream As New SharpDX.DataStream(bmpData.Scan0, bmpData.Stride * bmpData.Height, True, False)
    Dim pFormat As New SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, Direct2D1.AlphaMode.Premultiplied)
    Dim bmpProps As New SharpDX.Direct2D1.BitmapProperties(pFormat)

    Dim result As New SharpDX.Direct2D1.Bitmap(
                _targets(_selectedTargetIdx),
                New SharpDX.Size2(bmp.Width, bmp.Height),
                stream,
                bmpData.Stride,
                bmpProps)

    bmp.UnlockBits(bmpData)

    stream.Dispose()


    Return result
  End Function

  Private Overloads Function MeasureString(message As String, textFormat As DirectWrite.TextFormat, width As Single, align As ContentAlignment) As System.Drawing.SizeF

    Dim fac = New DirectWrite.Factory
    Try
      'Dim layout As SharpDX.DirectWrite.TextLayout =
      '         New SharpDX.DirectWrite.TextLayout(fac, message, textFormat, width, textFormat.FontSize)

      'Return New System.Drawing.SizeF(layout.Metrics.Width, layout.Metrics.Height)
      Return New SizeF(1, 1)
    Catch ex As Exception

    Finally
      fac.Dispose()
    End Try


  End Function

  'https://stackoverflow.com/questions/42253592/direct2d-drawing-arc-missing-pixels-and-irritating-antialiasing
  Private Function GetArc(x As Integer, y As Integer, width As Integer, height As Integer, startAngle As Integer, sweepAngle As Integer, ByRef geoSink As GeometrySink) As Geometry
    Dim geo = New PathGeometry(_factories(_selectedTargetIdx))
    geoSink = geo.Open()

    Dim arc = New ArcSegment()
    If sweepAngle > 180 Then
      arc.ArcSize = ArcSize.Large
    Else
      arc.ArcSize = ArcSize.Small
    End If
    'TODO: figure out the end point using geometry...
    arc.Point = New Interop.RawVector2(0, 0)
    arc.RotationAngle = startAngle
    arc.SweepDirection = SweepDirection.Clockwise
    arc.Size = New Size2F(width, height)

    geoSink.BeginFigure(New Interop.RawVector2(x, y), FigureBegin.Filled)
    geoSink.AddArc(arc)
    geoSink.EndFigure(FigureEnd.Closed)
    geoSink.Close()
    Return geo
  End Function

  Private Function GetGraphicsPath(points() As Drawing.Point, ByRef geoSink As GeometrySink, fillOrHollow As FigureBegin)
    Dim geo = New PathGeometry(_factories(_selectedTargetIdx))
    geoSink = geo.Open()

    geoSink.SetFillMode(Direct2D1.FillMode.Alternate)

    For i As Integer = 0 To points.Length - 1
      Dim point = points(i)

      If i = 0 Then
        geoSink.BeginFigure(New Interop.RawVector2(point.X, point.Y), fillOrHollow)
      Else
        geoSink.AddLine(New Interop.RawVector2(point.X, point.Y))
      End If

    Next

    geoSink.EndFigure(FigureEnd.Closed)
    geoSink.Close()
    Return geo
  End Function

  Private Function GetGraphicsPath(path As GraphicsPath, ByRef geoSink As GeometrySink, fillOrHollow As FigureBegin) As Geometry
    Dim geo = New PathGeometry(_factories(_selectedTargetIdx))
    geoSink = geo.Open()
    Dim fillMode As Direct2D1.FillMode
    If path.FillMode = Drawing2D.FillMode.Alternate Then
      fillMode = Direct2D1.FillMode.Alternate
    ElseIf path.FillMode = Drawing2D.FillMode.Winding Then
      fillMode = Direct2D1.FillMode.Winding
    End If

    geoSink.SetFillMode(fillMode)

    Dim bezierSegmentPts As New List(Of Interop.RawVector2)
    Dim pathPoints = path.PathPoints
    Dim pathTypes = path.PathTypes
    For i As Integer = 0 To path.PointCount - 1
      Dim point = pathPoints(i)
      Dim typeByte = pathTypes(i)
      Dim type As PathPointType = typeByte

      If type = PathPointType.Start Then
        geoSink.BeginFigure(New Interop.RawVector2(point.X, point.Y), fillOrHollow)

      ElseIf type = PathPointType.Line Then
        geoSink.AddLine(New Interop.RawVector2(point.X, point.Y))

      ElseIf type = PathPointType.Bezier Then
        'Keep making segments until the type is no longer bezier
        bezierSegmentPts.Add(New Interop.RawVector2(point.X, point.Y))

        If bezierSegmentPts.Count = 3 Then
          Dim bezierSeg = New BezierSegment()
          bezierSeg.Point1 = bezierSegmentPts(0)
          bezierSeg.Point2 = bezierSegmentPts(1)
          bezierSeg.Point3 = bezierSegmentPts(2)
          geoSink.AddBezier(bezierSeg)
          bezierSegmentPts.Clear()
        End If

      ElseIf type And PathPointType.CloseSubpath = PathPointType.CloseSubpath Then
        'Check if we are finishing a bezier segment or not
        If bezierSegmentPts.Count = 2 Then
          bezierSegmentPts.Add(New Interop.RawVector2(point.X, point.Y))
          Dim bezierSeg = New BezierSegment()
          bezierSeg.Point1 = bezierSegmentPts(0)
          bezierSeg.Point2 = bezierSegmentPts(1)
          bezierSeg.Point3 = bezierSegmentPts(2)
          geoSink.AddBezier(bezierSeg)
          bezierSegmentPts.Clear()
        Else
          geoSink.AddLine(New Interop.RawVector2(point.X, point.Y))
        End If

        geoSink.EndFigure(FigureEnd.Closed)

      Else
        Throw New NotImplementedException("Implement graphics path type")
      End If

    Next
    geoSink.Close()
    Return geo
  End Function

  Private Function GetStrokeStyleFromPen(pen As Pen) As StrokeStyle
    Dim strokeStyleProperties = New StrokeStyleProperties()
    strokeStyleProperties.DashCap = _dashCapDictionary(pen.DashCap)
    strokeStyleProperties.DashStyle = pen.DashStyle
    strokeStyleProperties.LineJoin = pen.LineJoin

    If _lineCapDictionary.ContainsKey(pen.StartCap) Then
      strokeStyleProperties.StartCap = _lineCapDictionary(pen.StartCap)
    Else
      Debug.Print("Did not find a suitable start cap in direct2D.")
      strokeStyleProperties.StartCap = CapStyle.Square
    End If

    If _lineCapDictionary.ContainsKey(pen.EndCap) Then
      strokeStyleProperties.EndCap = _lineCapDictionary(pen.EndCap)
    Else
      Debug.Print("Did not find a suitable end cap in direct2D.")
      strokeStyleProperties.EndCap = CapStyle.Square
    End If

    strokeStyleProperties.DashOffset = pen.DashOffset
    strokeStyleProperties.MiterLimit = pen.MiterLimit

    Dim dashPattern As Single() = {}
    If pen.DashStyle = Drawing2D.DashStyle.Custom Then
      dashPattern = pen.DashPattern
    End If

    Return New StrokeStyle(_factories(_selectedTargetIdx), strokeStyleProperties, dashPattern)

  End Function
#End Region

  Public Overrides Sub DrawImage(image As Drawing.Image, destPoints() As PointF, srcRect As RectangleF, srcUnit As GraphicsUnit, imageAttr As ImageAttributes)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, destPoints() As PointF, srcRect As RectangleF, srcUnit As GraphicsUnit, imageAttr As ImageAttributes, callback As Graphics.DrawImageAbort, callbackData As Integer)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, destPoints() As Drawing.Point, srcRect As Rectangle, srcUnit As GraphicsUnit)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, destPoints() As Drawing.Point, srcRect As Rectangle, srcUnit As GraphicsUnit, imageAttr As ImageAttributes)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, destPoints() As Drawing.Point, srcRect As Rectangle, srcUnit As GraphicsUnit, imageAttr As ImageAttributes, callback As Graphics.DrawImageAbort)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, destPoints() As Drawing.Point, srcRect As Rectangle, srcUnit As GraphicsUnit, imageAttr As ImageAttributes, callback As Graphics.DrawImageAbort, callbackData As Integer)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, destRect As Rectangle, srcX As Single, srcY As Single, srcWidth As Single, srcHeight As Single, srcUnit As GraphicsUnit)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, destRect As Rectangle, srcX As Single, srcY As Single, srcWidth As Single, srcHeight As Single, srcUnit As GraphicsUnit, imageAttrs As ImageAttributes)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, destRect As Rectangle, srcX As Single, srcY As Single, srcWidth As Single, srcHeight As Single, srcUnit As GraphicsUnit, imageAttrs As ImageAttributes, callback As Graphics.DrawImageAbort, callbackData As IntPtr)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, destPoints() As PointF, srcRect As RectangleF, srcUnit As GraphicsUnit)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, destRect As Rectangle, srcX As Integer, srcY As Integer, srcWidth As Integer, srcHeight As Integer, srcUnit As GraphicsUnit)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim d2Img = GetDirect2DBitmap(image)
    _targets(_selectedTargetIdx).DrawBitmap(d2Img, DrawingRectToD2DRectF(destRect), 1.0F, BitmapInterpolationMode.Linear, DrawingRectToD2DRectF(New Rectangle(srcX, srcY, srcWidth, srcHeight)))
    'd2Img.Dispose()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, destRect As Rectangle, srcX As Integer, srcY As Integer, srcWidth As Integer, srcHeight As Integer, srcUnit As GraphicsUnit, imageAttr As ImageAttributes)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, destRect As Rectangle, srcX As Integer, srcY As Integer, srcWidth As Integer, srcHeight As Integer, srcUnit As GraphicsUnit, imageAttr As ImageAttributes, callback As Graphics.DrawImageAbort)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, destRect As Rectangle, srcX As Integer, srcY As Integer, srcWidth As Integer, srcHeight As Integer, srcUnit As GraphicsUnit, imageAttrs As ImageAttributes, callback As Graphics.DrawImageAbort, callbackData As IntPtr)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, destRect As Rectangle, srcX As Single, srcY As Single, srcWidth As Single, srcHeight As Single, srcUnit As GraphicsUnit, imageAttrs As ImageAttributes, callback As Graphics.DrawImageAbort)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, destRect As Rectangle, srcRect As Rectangle, srcUnit As GraphicsUnit)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, destPoints() As Drawing.Point)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, x As Integer, y As Integer, srcRect As Rectangle, srcUnit As GraphicsUnit)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, point As PointF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, x As Single, y As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, rect As RectangleF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, x As Single, y As Single, width As Single, height As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, x As Integer, y As Integer)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, destRect As RectangleF, srcRect As RectangleF, srcUnit As GraphicsUnit)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, rect As Rectangle)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim d2Img = GetDirect2DBitmap(image)
    _targets(_selectedTargetIdx).DrawBitmap(d2Img, DrawingRectToD2DRectF(rect), 1.0F, BitmapInterpolationMode.Linear)
    'd2Img.Dispose()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, destPoints() As PointF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, x As Single, y As Single, srcRect As RectangleF, srcUnit As GraphicsUnit)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, x As Integer, y As Integer, width As Integer, height As Integer)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim d2Img = GetDirect2DBitmap(image)
    _targets(_selectedTargetIdx).DrawBitmap(d2Img, DrawingRectToD2DRectF(New Rectangle(x, y, width, height)), 1.0F, BitmapInterpolationMode.Linear)
    'd2Img.Dispose()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, point As Drawing.Point)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImage(image As Drawing.Image, destPoints() As PointF, srcRect As RectangleF, srcUnit As GraphicsUnit, imageAttr As ImageAttributes, callback As Graphics.DrawImageAbort)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawIcon(icon As Icon, x As Integer, y As Integer)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawIcon(icon As Icon, targetRect As Rectangle)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawIconUnstretched(icon As Icon, targetRect As Rectangle)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImageUnscaled(image As Drawing.Image, point As Drawing.Point)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawImageUnscaled(image As Drawing.Image, x As Integer, y As Integer)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim d2Img = GetDirect2DBitmap(image)

    _targets(_selectedTargetIdx).DrawBitmap(d2Img, DrawingRectToD2DRectF(New Rectangle(x, y, image.Width, image.Height)), 1.0F, BitmapInterpolationMode.Linear)
    'd2Img.Dispose()
  End Sub

  Public Overrides Sub DrawImageUnscaled(image As Drawing.Image, rect As Rectangle)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim d2Img = GetDirect2DBitmap(image)
    _targets(_selectedTargetIdx).DrawBitmap(d2Img, DrawingRectToD2DRectF(New Rectangle(0, 0, rect.Width, rect.Height)), 1.0F, BitmapInterpolationMode.Linear, DrawingRectToD2DRectF(New Rectangle(rect.X, rect.Y, image.Width, image.Height)))
    'd2Img.Dispose()
  End Sub

  Public Overrides Sub DrawImageUnscaled(image As Drawing.Image, x As Integer, y As Integer, width As Integer, height As Integer)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim d2Img = GetDirect2DBitmap(image)
    _targets(_selectedTargetIdx).DrawBitmap(d2Img, DrawingRectToD2DRectF(New Rectangle(x, y, width, height)), 1.0F, BitmapInterpolationMode.Linear)
    'd2Img.Dispose()
  End Sub

  Public Overrides Sub DrawImageUnscaledAndClipped(image As Drawing.Image, rect As Rectangle)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub TranslateClip(dx As Integer, dy As Integer)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub TranslateClip(dx As Single, dy As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub SetClip(g As Graphics)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub SetClip(g As Graphics, combineMode As Drawing2D.CombineMode)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub SetClip(rect As Rectangle)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub SetClip(rect As Rectangle, combineMode As Drawing2D.CombineMode)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub SetClip(rect As RectangleF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub SetClip(rect As RectangleF, combineMode As Drawing2D.CombineMode)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub SetClip(path As GraphicsPath, combineMode As Drawing2D.CombineMode)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub SetClip(region As Region, combineMode As Drawing2D.CombineMode)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub SetClip(path As GraphicsPath)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub IntersectClip(rect As Rectangle)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub IntersectClip(rect As RectangleF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub IntersectClip(region As Region)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub ExcludeClip(rect As Rectangle)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub ExcludeClip(region As Region)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub ResetClip()
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawString(s As String, font As Font, brush As Drawing.Brush, layoutRectangle As RectangleF, format As StringFormat)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawString(s As String, font As Font, brush As Drawing.Brush, x As Single, y As Single)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    'Font sucks.. so just going to draw the text on a bitmap using GDI.. and drawing the bitmap.
    'Dim strSize = MeasureString(s, font)
    'Using textBMP = New Drawing.Bitmap(CInt(strSize.Width), CInt(strSize.Height))
    '  Using gdiPlusGraphics = Graphics.FromImage(textBMP)
    '    gdiPlusGraphics.Clear(Color.White)
    '    gdiPlusGraphics.DrawString(s, font, brush, 0, 0)
    '  End Using
    '  Using makeTransparent As Drawing.Bitmap = Utilities.Drawing.MakeTextImageTransparent(textBMP)
    '    DrawImageUnscaled(makeTransparent, x, y)
    '  End Using
    'End Using

    'Dim txtFormat = New DirectWrite.TextFormat(font.ToHfont)
    'Dim size = MeasureString(s, txtFormat, _targetBoundss(_selectedTargetIdx).Width, ContentAlignment.MiddleLeft)
    '_target.DrawText(s, txtFormat, DrawingRectToD2DRectF(New Rectangle(x, y, size.Width, size.Height)), DrawingBrushToD2DBrush(brush))
  End Sub

  Public Overrides Sub DrawString(s As String, font As Font, brush As Drawing.Brush, point As PointF)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    'Font sucks.. so just going to draw the text on a bitmap using GDI.. and drawing the bitmap.
    'Dim strSize = MeasureString(s, font)
    'Using textBMP = New Drawing.Bitmap(CInt(strSize.Width), CInt(strSize.Height))
    '  Using gdiPlusGraphics = Graphics.FromImage(textBMP)
    '    gdiPlusGraphics.Clear(Color.White)
    '    gdiPlusGraphics.DrawString(s, font, brush, 0, 0)
    '  End Using
    '  Using makeTransparent As Drawing.Bitmap = Utilities.Drawing.MakeTextImageTransparent(textBMP)
    '    DrawImageUnscaled(makeTransparent, point.X, point.Y)
    '  End Using
    'End Using

    'Dim txtFormat = New DirectWrite.TextFormat(font.ToHfont)
    'Dim size = MeasureString(s, txtFormat, _targetBoundss(_selectedTargetIdx).Width, ContentAlignment.MiddleLeft)
    '_target.DrawText(s, txtFormat, DrawingRectToD2DRectF(New Rectangle(point.X, point.Y, size.Width, size.Height)), DrawingBrushToD2DBrush(brush))
  End Sub

  Public Overrides Sub DrawString(s As String, font As Font, brush As Drawing.Brush, x As Single, y As Single, format As StringFormat)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    'Font sucks.. so just going to draw the text on a bitmap using GDI.. and drawing the bitmap.
    'Dim strSize = MeasureString(s, font)
    'Using textBMP = New Drawing.Bitmap(CInt(strSize.Width), CInt(strSize.Height))
    '  Using gdiPlusGraphics = Graphics.FromImage(textBMP)
    '    gdiPlusGraphics.Clear(Color.White)
    '    gdiPlusGraphics.DrawString(s, font, brush, 0, 0, format)
    '  End Using
    '  Using makeTransparent As Drawing.Bitmap = Utilities.Drawing.MakeTextImageTransparent(textBMP)
    '    DrawImageUnscaled(makeTransparent, x, y)
    '  End Using

    'End Using

    'Dim txtFormat = New DirectWrite.TextFormat(font.ToHfont)
    'Dim size = MeasureString(s, txtFormat, _targetBoundss(_selectedTargetIdx).Width, format.Alignment)
    '_target.DrawText(s, txtFormat, DrawingRectToD2DRectF(New Rectangle(x, y, size.Width, size.Height)), DrawingBrushToD2DBrush(brush))
  End Sub

  Public Overrides Sub DrawString(s As String, font As Font, brush As Drawing.Brush, point As PointF, format As StringFormat)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawString(s As String, font As Font, brush As Drawing.Brush, layoutRectangle As RectangleF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawLine(pen As Pen, pt1 As Drawing.Point, pt2 As Drawing.Point)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim brush = DrawingBrushToD2DBrush(pen.Brush)
    Dim strokeStyle = GetStrokeStyleFromPen(pen)
    _targets(_selectedTargetIdx).DrawLine(New Interop.RawVector2(pt1.X, pt1.Y), New Interop.RawVector2(pt2.X, pt2.Y), brush, pen.Width, strokeStyle)
    strokeStyle.Dispose()
    brush.Dispose()
  End Sub

  Public Overrides Sub DrawLine(pen As Pen, x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim brush = DrawingBrushToD2DBrush(pen.Brush)
    Dim strokeStyle = GetStrokeStyleFromPen(pen)
    _targets(_selectedTargetIdx).DrawLine(New Interop.RawVector2(x1, y1), New Interop.RawVector2(x2, y2), brush, pen.Width, strokeStyle)
    strokeStyle.Dispose()
    brush.Dispose()
  End Sub

  Public Overrides Sub DrawLine(pen As Pen, pt1 As PointF, pt2 As PointF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawLine(pen As Pen, x1 As Single, y1 As Single, x2 As Single, y2 As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawLines(pen As Pen, points() As Drawing.Point)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    For i As Integer = 1 To points.Length - 1
      DrawLine(pen, points(i - 1), points(i))
    Next
  End Sub

  Public Overrides Sub DrawLines(pen As Pen, points() As PointF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawArc(pen As Pen, x As Single, y As Single, width As Single, height As Single, startAngle As Single, sweepAngle As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawArc(pen As Pen, rect As RectangleF, startAngle As Single, sweepAngle As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawArc(pen As Pen, x As Integer, y As Integer, width As Integer, height As Integer, startAngle As Integer, sweepAngle As Integer)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim geoSink As GeometrySink = Nothing
    Dim geo = GetArc(x, y, width, height, startAngle, sweepAngle, geoSink)
    Dim brush = DrawingBrushToD2DBrush(pen.Brush)
    Dim strokeStyle = GetStrokeStyleFromPen(pen)
    _targets(_selectedTargetIdx).DrawGeometry(geo, brush, pen.Width, strokeStyle)

    geo.Dispose()
    geoSink.Dispose()
    strokeStyle.Dispose()
    brush.Dispose()
  End Sub

  Public Overrides Sub DrawArc(pen As Pen, rect As Rectangle, startAngle As Single, sweepAngle As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawBezier(pen As Pen, x1 As Single, y1 As Single, x2 As Single, y2 As Single, x3 As Single, y3 As Single, x4 As Single, y4 As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawBezier(pen As Pen, pt1 As PointF, pt2 As PointF, pt3 As PointF, pt4 As PointF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawBezier(pen As Pen, pt1 As Drawing.Point, pt2 As Drawing.Point, pt3 As Drawing.Point, pt4 As Drawing.Point)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawBeziers(pen As Pen, points() As PointF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawBeziers(pen As Pen, points() As Drawing.Point)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawRectangle(pen As Pen, rect As Rectangle)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim brush = DrawingBrushToD2DBrush(pen.Brush)
    Dim strokeStyle = GetStrokeStyleFromPen(pen)
    _targets(_selectedTargetIdx).DrawRectangle(DrawingRectToD2DRectF(rect), brush, pen.Width, strokeStyle)
    strokeStyle.Dispose()
    brush.Dispose()
  End Sub

  Public Overrides Sub DrawRectangle(pen As Pen, x As Single, y As Single, width As Single, height As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawRectangle(pen As Pen, x As Integer, y As Integer, width As Integer, height As Integer)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim brush = DrawingBrushToD2DBrush(pen.Brush)
    Dim strokeStyle = GetStrokeStyleFromPen(pen)
    _targets(_selectedTargetIdx).DrawRectangle(DrawingRectToD2DRectF(New Rectangle(x, y, width, height)), brush, pen.Width, strokeStyle)
    strokeStyle.Dispose()
    brush.Dispose()
  End Sub

  Public Overrides Sub DrawRectangles(pen As Pen, rects() As RectangleF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawRectangles(pen As Pen, rects() As Rectangle)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawEllipse(pen As Pen, rect As RectangleF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawEllipse(pen As Pen, x As Single, y As Single, width As Single, height As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawEllipse(pen As Pen, rect As Rectangle)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim brush = DrawingBrushToD2DBrush(pen.Brush)
    Dim strokeStyle = GetStrokeStyleFromPen(pen)
    Dim xCenter As Single = (rect.Left + rect.Right) / 2.0
    Dim yCenter As Single = (rect.Top + rect.Bottom) / 2.0
    Dim ellipse As Direct2D1.Ellipse = New Direct2D1.Ellipse() With {
                            .Point = New Interop.RawVector2(xCenter, yCenter),
                            .RadiusX = rect.Width / 2.0, .RadiusY = rect.Height / 2.0}
    _targets(_selectedTargetIdx).DrawEllipse(ellipse, brush, pen.Width, strokeStyle)
    strokeStyle.Dispose()
    brush.Dispose()
  End Sub

  Public Overrides Sub DrawEllipse(pen As Pen, x As Integer, y As Integer, width As Integer, height As Integer)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim rect = New Rectangle(x, y, width, height)
    Dim brush = DrawingBrushToD2DBrush(pen.Brush)
    Dim strokeStyle = GetStrokeStyleFromPen(pen)
    Dim xCenter As Single = (rect.Left + rect.Right) / 2.0
    Dim yCenter As Single = (rect.Top + rect.Bottom) / 2.0
    Dim ellipse As Direct2D1.Ellipse = New Direct2D1.Ellipse() With {
                            .Point = New Interop.RawVector2(xCenter, yCenter),
                            .RadiusX = rect.Width / 2.0, .RadiusY = rect.Height / 2.0}
    _targets(_selectedTargetIdx).DrawEllipse(ellipse, brush, pen.Width, strokeStyle)
    strokeStyle.Dispose()
    brush.Dispose()
  End Sub

  Public Overrides Sub CopyFromScreen(upperLeftSource As Drawing.Point, upperLeftDestination As Drawing.Point, blockRegionSize As Size)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub CopyFromScreen(sourceX As Integer, sourceY As Integer, destinationX As Integer, destinationY As Integer, blockRegionSize As Size)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub CopyFromScreen(upperLeftSource As Drawing.Point, upperLeftDestination As Drawing.Point, blockRegionSize As Size, copyPixelOperation As CopyPixelOperation)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub CopyFromScreen(sourceX As Integer, sourceY As Integer, destinationX As Integer, destinationY As Integer, blockRegionSize As Size, copyPixelOperation As CopyPixelOperation)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub ResetTransform()
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    'Can't figure out how GDI+ is doing this.. so making GDI+ do all the transformations for me..
    GraphicsForTransform.ResetTransform()
    Dim transform = GraphicsForTransform.Transform
    _targets(_selectedTargetIdx).Transform = New Interop.RawMatrix3x2(transform.Elements(0), transform.Elements(1), transform.Elements(2), transform.Elements(3), transform.Elements(4), transform.Elements(5))

    '_targets(_selectedTargetIdx).Transform = New Interop.RawMatrix3x2(1, 0, 0, 1, 0, 0)
  End Sub

  Public Overrides Sub MultiplyTransform(matrix As Matrix)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub MultiplyTransform(matrix As Matrix, order As MatrixOrder)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub TranslateTransform(dx As Single, dy As Single)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim target As DeviceContextRenderTarget = _targets(_selectedTargetIdx)

    'Can't figure out how GDI+ is doing this.. so making GDI+ do all the transformations for me..
    GraphicsForTransform.TranslateTransform(dx, dy)
    Dim transform = GraphicsForTransform.Transform
    target.Transform = New Interop.RawMatrix3x2(transform.Elements(0), transform.Elements(1), transform.Elements(2), transform.Elements(3), transform.Elements(4), transform.Elements(5))

    'Dim mat = New Matrix3x2(target.Transform.M11, target.Transform.M12, target.Transform.M21, target.Transform.M22, target.Transform.M31, target.Transform.M32)
    'mat += Matrix3x2.CreateTranslation(dx, dy)
    '_targets(_selectedTargetIdx).Transform = New Interop.RawMatrix3x2(mat.M11, mat.M12, mat.M21, mat.M22, mat.M31, mat.M32)
  End Sub

  Public Overrides Sub TranslateTransform(dx As Single, dy As Single, order As MatrixOrder)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub ScaleTransform(sx As Single, sy As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub ScaleTransform(sx As Single, sy As Single, order As MatrixOrder)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub RotateTransform(angle As Single)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim target As DeviceContextRenderTarget = _targets(_selectedTargetIdx)

    'Can't figure out how GDI+ is doing this.. so making GDI+ do all the transformations for me..
    GraphicsForTransform.RotateTransform(angle)
    Dim transform = GraphicsForTransform.Transform
    target.Transform = New Interop.RawMatrix3x2(transform.Elements(0), transform.Elements(1), transform.Elements(2), transform.Elements(3), transform.Elements(4), transform.Elements(5))

    'Dim mat = New Matrix3x2(target.Transform.M11, target.Transform.M12, target.Transform.M21, target.Transform.M22, target.Transform.M31, target.Transform.M32)
    'Dim rotMat = Matrix3x2.CreateRotation(angle * Math.PI / 180, Vector2.Zero)
    'target.Transform = New Interop.RawMatrix3x2(rotMat.M11, rotMat.M12, rotMat.M21, rotMat.M22, mat.M31, mat.M32)
  End Sub

  Public Overrides Sub RotateTransform(angle As Single, order As MatrixOrder)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub TransformPoints(destSpace As CoordinateSpace, srcSpace As CoordinateSpace, pts() As PointF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub TransformPoints(destSpace As CoordinateSpace, srcSpace As CoordinateSpace, pts() As Drawing.Point)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub EndContainer(container As GraphicsContainer)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawPie(pen As Pen, x As Single, y As Single, width As Single, height As Single, startAngle As Single, sweepAngle As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawPie(pen As Pen, rect As RectangleF, startAngle As Single, sweepAngle As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawPie(pen As Pen, x As Integer, y As Integer, width As Integer, height As Integer, startAngle As Integer, sweepAngle As Integer)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawPie(pen As Pen, rect As Rectangle, startAngle As Single, sweepAngle As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub FillEllipse(brush As Drawing.Brush, rect As RectangleF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub FillEllipse(brush As Drawing.Brush, x As Single, y As Single, width As Single, height As Single)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim rect = New Rectangle(x, y, width, height)
    Dim d2brush = DrawingBrushToD2DBrush(brush)
    Dim xCenter As Single = (rect.Left + rect.Right) / 2.0
    Dim yCenter As Single = (rect.Top + rect.Bottom) / 2.0
    Dim ellipse As Direct2D1.Ellipse = New Direct2D1.Ellipse() With {
                            .Point = New Interop.RawVector2(xCenter, yCenter),
                            .RadiusX = rect.Width / 2.0, .RadiusY = rect.Height / 2.0}
    _targets(_selectedTargetIdx).FillEllipse(ellipse, d2brush)
    d2brush.Dispose()
  End Sub

  Public Overrides Sub FillEllipse(brush As Drawing.Brush, rect As Rectangle)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub FillEllipse(brush As Drawing.Brush, x As Integer, y As Integer, width As Integer, height As Integer)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim rect = New Rectangle(x, y, width, height)
    Dim d2brush = DrawingBrushToD2DBrush(brush)
    Dim xCenter As Single = (rect.Left + rect.Right) / 2.0
    Dim yCenter As Single = (rect.Top + rect.Bottom) / 2.0
    Dim ellipse As Direct2D1.Ellipse = New Direct2D1.Ellipse() With {
                            .Point = New Interop.RawVector2(xCenter, yCenter),
                            .RadiusX = rect.Width / 2.0, .RadiusY = rect.Height / 2.0}
    _targets(_selectedTargetIdx).FillEllipse(ellipse, d2brush)
    d2brush.Dispose()
  End Sub

  Public Overrides Sub FillPie(brush As Drawing.Brush, rect As Rectangle, startAngle As Single, sweepAngle As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub FillPie(brush As Drawing.Brush, x As Single, y As Single, width As Single, height As Single, startAngle As Single, sweepAngle As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub FillPie(brush As Drawing.Brush, x As Integer, y As Integer, width As Integer, height As Integer, startAngle As Integer, sweepAngle As Integer)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim geoSink As GeometrySink = Nothing
    Dim geo = GetArc(x, y, width, height, startAngle, sweepAngle, geoSink)
    Dim d2Brush = DrawingBrushToD2DBrush(brush)
    _targets(_selectedTargetIdx).FillGeometry(geo, d2Brush)
    d2Brush.Dispose()
    geo.Dispose()
    geoSink.Dispose()
  End Sub

  Public Overrides Sub FillPath(brush As Drawing.Brush, path As GraphicsPath)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim geoSink As GeometrySink = Nothing
    Dim geo = GetGraphicsPath(path, geoSink, FigureBegin.Filled)

    Dim d2Brush = DrawingBrushToD2DBrush(brush)
    _targets(_selectedTargetIdx).FillGeometry(geo, d2Brush)

    d2Brush.Dispose()
    geo.Dispose()
    geoSink.Dispose()
  End Sub

  Public Overrides Sub FillClosedCurve(brush As Drawing.Brush, points() As PointF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub FillClosedCurve(brush As Drawing.Brush, points() As PointF, fillmode As Drawing2D.FillMode)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub FillClosedCurve(brush As Drawing.Brush, points() As Drawing.Point)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub FillClosedCurve(brush As Drawing.Brush, points() As Drawing.Point, fillmode As Drawing2D.FillMode)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub FillClosedCurve(brush As Drawing.Brush, points() As Drawing.Point, fillmode As Drawing2D.FillMode, tension As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub FillClosedCurve(brush As Drawing.Brush, points() As PointF, fillmode As Drawing2D.FillMode, tension As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub FillPolygon(brush As Drawing.Brush, points() As Drawing.Point, fillMode As Drawing2D.FillMode)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub FillPolygon(brush As Drawing.Brush, points() As Drawing.Point)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim geoSink As GeometrySink = Nothing
    Dim geo = GetGraphicsPath(points, geoSink, FigureBegin.Filled)

    Dim d2Brush = DrawingBrushToD2DBrush(brush)
    _targets(_selectedTargetIdx).FillGeometry(geo, d2Brush)

    d2Brush.Dispose()
    geo.Dispose()
    geoSink.Dispose()
  End Sub

  Public Overrides Sub FillPolygon(brush As Drawing.Brush, points() As PointF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub FillPolygon(brush As Drawing.Brush, points() As PointF, fillMode As Drawing2D.FillMode)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub FillRegion(brush As Drawing.Brush, region As Region)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawPolygon(pen As Pen, points() As PointF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawPolygon(pen As Pen, points() As Drawing.Point)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim geoSink As GeometrySink = Nothing
    Dim geo = GetGraphicsPath(points, geoSink, FigureBegin.Hollow)
    Dim strokeStyle = GetStrokeStyleFromPen(pen)
    Dim d2Brush = DrawingBrushToD2DBrush(pen.Brush)
    _targets(_selectedTargetIdx).DrawGeometry(geo, d2Brush, pen.Width, strokeStyle)

    d2Brush.Dispose()
    strokeStyle.Dispose()
    geo.Dispose()
    geoSink.Dispose()
  End Sub

  Public Overrides Sub DrawPath(pen As Pen, path As GraphicsPath)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim geoSink As GeometrySink = Nothing
    Dim geo = GetGraphicsPath(path, geoSink, FigureBegin.Hollow)
    Dim strokeStyle = GetStrokeStyleFromPen(pen)
    Dim d2Brush = DrawingBrushToD2DBrush(pen.Brush)
    _targets(_selectedTargetIdx).DrawGeometry(geo, d2Brush, pen.Width, strokeStyle)

    d2Brush.Dispose()
    strokeStyle.Dispose()
    geo.Dispose()
    geoSink.Dispose()
  End Sub

  Public Overrides Sub DrawCurve(pen As Pen, points() As PointF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawCurve(pen As Pen, points() As PointF, tension As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawCurve(pen As Pen, points() As PointF, offset As Integer, numberOfSegments As Integer)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawCurve(pen As Pen, points() As PointF, offset As Integer, numberOfSegments As Integer, tension As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawCurve(pen As Pen, points() As Drawing.Point)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawCurve(pen As Pen, points() As Drawing.Point, tension As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawCurve(pen As Pen, points() As Drawing.Point, offset As Integer, numberOfSegments As Integer, tension As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawClosedCurve(pen As Pen, points() As PointF, tension As Single, fillmode As Drawing2D.FillMode)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawClosedCurve(pen As Pen, points() As Drawing.Point)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawClosedCurve(pen As Pen, points() As Drawing.Point, tension As Single, fillmode As Drawing2D.FillMode)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub DrawClosedCurve(pen As Pen, points() As PointF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub Clear(color As Color)
    If Not _hasBegunDrawing Then
      'Have GDI clear first.. THEN start drawing.  This will hopefully allow us to have transparent backgrounds.
      _graphics.Clear(color)
      BeginDraw()
    Else
      If color.A < 255 Then Debug.Print("WARNING!!! I see you're trying to clear Direct2D with a transparent color.  This does not work well.. just letting you know.")
      _targets(_selectedTargetIdx).Clear(New Interop.RawColor4(color.R, color.G, color.B, color.A))
    End If

  End Sub

  Public Overrides Sub FillRectangle(brush As Drawing.Brush, rect As RectangleF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub FillRectangle(brush As Drawing.Brush, x As Single, y As Single, width As Single, height As Single)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub FillRectangle(brush As Drawing.Brush, rect As Rectangle)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim d2brush = DrawingBrushToD2DBrush(brush)
    _targets(_selectedTargetIdx).FillRectangle(DrawingRectToD2DRectF(rect), d2brush)
    d2brush.Dispose()
  End Sub

  Public Overrides Sub FillRectangle(brush As Drawing.Brush, x As Integer, y As Integer, width As Integer, height As Integer)
    If Not _hasBegunDrawing Then
      BeginDraw()
    End If

    Dim d2brush = DrawingBrushToD2DBrush(brush)
    _targets(_selectedTargetIdx).FillRectangle(DrawingRectToD2DRectF(New Rectangle(x, y, width, height)), d2brush)
    d2brush.Dispose()
  End Sub

  Public Overrides Sub FillRectangles(brush As Drawing.Brush, rects() As RectangleF)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub FillRectangles(brush As Drawing.Brush, rects() As Rectangle)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Sub Restore(gstate As GraphicsState)
    Throw New NotImplementedException()
  End Sub

  Public Overrides Function GetNearestColor(color As Color) As Color
    Throw New NotImplementedException()
  End Function

  Public Overrides Function MeasureString(text As String, font As Font, layoutArea As SizeF, stringFormat As StringFormat, ByRef charactersFitted As Integer, ByRef linesFilled As Integer) As SizeF
    Throw New NotImplementedException()
  End Function

  Public Overrides Function MeasureString(text As String, font As Font, origin As PointF, stringFormat As StringFormat) As SizeF
    Throw New NotImplementedException()
  End Function

  Public Overrides Function MeasureString(text As String, font As Font, layoutArea As SizeF) As SizeF
    Throw New NotImplementedException()
  End Function

  Public Overrides Function MeasureString(text As String, font As Font, layoutArea As SizeF, stringFormat As StringFormat) As SizeF
    Throw New NotImplementedException()
  End Function

  Public Overrides Function MeasureString(text As String, font As Font) As SizeF
    'Font sucks.. so just going to draw the text on a bitmap using GDI.. and drawing the bitmap.

    Return GraphicsForTransform.MeasureString(text, font)
    'Return New SizeF(1, 1)

    'Dim txtFormat = New DirectWrite.TextFormat(font.ToHfont)
    'Return MeasureString(text, txtFormat, _targetBoundss(_selectedTargetIdx).Width, ContentAlignment.MiddleLeft)
  End Function

  Public Overrides Function MeasureString(text As String, font As Font, width As Integer) As SizeF
    'Font sucks.. so just going to draw the text on a bitmap using GDI.. and drawing the bitmap.

    Return GraphicsForTransform.MeasureString(text, font, width)
    'Return New SizeF(1, 1)

    'Dim txtFormat = New DirectWrite.TextFormat(font.ToHfont)
    'Return MeasureString(text, txtFormat, width, ContentAlignment.MiddleLeft)
  End Function

  Public Overrides Function MeasureString(text As String, font As Font, width As Integer, format As StringFormat) As SizeF
    Throw New NotImplementedException()
  End Function

  Public Overrides Function MeasureCharacterRanges(text As String, font As Font, layoutRect As RectangleF, stringFormat As StringFormat) As Region()
    Throw New NotImplementedException()
  End Function

  Public Overrides Function IsVisible(x As Integer, y As Integer) As Boolean
    Throw New NotImplementedException()
  End Function

  Public Overrides Function IsVisible(point As Drawing.Point) As Boolean
    Throw New NotImplementedException()
  End Function

  Public Overrides Function IsVisible(x As Single, y As Single) As Boolean
    Throw New NotImplementedException()
  End Function

  Public Overrides Function IsVisible(point As PointF) As Boolean
    Throw New NotImplementedException()
  End Function

  Public Overrides Function IsVisible(x As Integer, y As Integer, width As Integer, height As Integer) As Boolean
    Throw New NotImplementedException()
  End Function

  Public Overrides Function IsVisible(rect As RectangleF) As Boolean
    Throw New NotImplementedException()
  End Function

  Public Overrides Function IsVisible(rect As Rectangle) As Boolean
    Throw New NotImplementedException()
  End Function

  Public Overrides Function BeginContainer() As GraphicsContainer
    Throw New NotImplementedException()
  End Function

  Public Overrides Function BeginContainer(dstrect As RectangleF, srcrect As RectangleF, unit As GraphicsUnit) As GraphicsContainer
    Throw New NotImplementedException()
  End Function

  Public Overrides Function Save() As GraphicsState
    Throw New NotImplementedException()
  End Function

End Class


'Public Class cDirect2DRenderer

'#Region "=== Shared ==="
'  Public Shared Sub TestRendering(gr As Graphics, canvasSize As System.Drawing.Size)
'    Dim renderer As New cDirect2DRenderer

'    ' CAUTION: After this, must call EndDraw or ReleaseHDC when done drawing.
'    Dim success As Boolean = renderer.BeginDraw(gr, canvasSize)

'    ' Render some Direct2D content.
'    success = renderer.Test_Render(success)

'    success = renderer.EndDraw(gr, success)
'    If Not success Then
'      'TODO: Log error.
'    End If

'    renderer.Dispose() : renderer = Nothing
'  End Sub
'#End Region


'#Region "=== Fields, Constructor, Dispose ==="

'#End Region

'End Class
