using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for Drawing_MainWindow.xaml
    /// </summary>
    public partial class Drawing_MainWindow : Window
    {
        public Drawing_MainWindow()
        {
            InitializeComponent();
            w3.tblck.Text = "AaBb...";
            w3.tblck.FontSize = 12 + w3.slValue.Value;
        }
        bool flag = false;

        Point currentPoint = new Point();
        private void Canvas_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LineStrokeThickness = 5 + (int)w1.slValue.Value;
            if (e.ButtonState == MouseButtonState.Pressed)
                currentPoint = e.GetPosition(this);
        }
        public int LineStrokeThickness = new int();
        private void Canvas_MouseMove_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!flag)
                {
                    SolidColorBrush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(w2.hexcolor));
                    SolidColorBrush brush2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString(w2.hexcolor2));
                    paintSurface.Background = brush2;
                    if (w1.brushfancy)
                    {
                        Line line = new Line();
                        line.StrokeThickness = LineStrokeThickness;
                        line.Stroke = brush;
                        line.X1 = currentPoint.X;
                        line.Y1 = currentPoint.Y;
                        line.X2 = e.GetPosition(this).X;
                        line.Y2 = e.GetPosition(this).Y;

                        currentPoint = e.GetPosition(this);

                        paintSurface.Children.Add(line);
                    }
                    else
                    {
                        Ellipse myEllipse = new Ellipse();
                        myEllipse.Fill = brush;
                        myEllipse.StrokeThickness = LineStrokeThickness;
                        myEllipse.Stroke = brush;
                        myEllipse.SetValue(Canvas.LeftProperty, (double)e.GetPosition(this).X);
                        myEllipse.SetValue(Canvas.TopProperty, (double)e.GetPosition(this).Y);
                        // Set the width and height of the Ellipse.
                        myEllipse.Width = LineStrokeThickness;
                        myEllipse.Height = LineStrokeThickness;
                        paintSurface.Children.Add(myEllipse);
                    }
                }
                else
                {
                    Point pt = e.GetPosition(paintSurface);
                    HitTestResult result = VisualTreeHelper.HitTest(paintSurface, pt);

                    if (result != null)
                    {
                        //textBoxHitTestResult.Text = result.VisualHit.ToString();
                        paintSurface.Children.Remove(result.VisualHit as UIElement);

                    }

                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            w1.Close();
            w2.Close();
            w3.Close();
            this.Close();
        }
        Window1 w1 = new Window1();
        private void Brush_Click(object sender, RoutedEventArgs e)
        {
            flag = false;
            w1.Visibility = Visibility.Visible;
        }

        private void Eraser_Click(object sender, RoutedEventArgs e)
        {
            flag = true;

        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            paintSurface.Children.Clear();
        }
        Window2 w2 = new Window2();
        private void ColorS_Click(object sender, RoutedEventArgs e)
        {
            w2.Visibility = Visibility.Visible;
        }
        Window3 w3 = new Window3();
        private void Text_Click(object sender, RoutedEventArgs e)
        {
            w3.Visibility = Visibility.Visible;

        }
        private void addtext(Point point)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = w3.tb1.Text;
            textBlock.FontFamily = w3.tb1.FontFamily;
            textBlock.FontSize = w3.tb1.FontSize;
            //SolidColorBrush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(w2.hexcolor));
            //textBlock.Foreground = brush;
            Canvas.SetLeft(textBlock, point.X);
            Canvas.SetTop(textBlock, point.Y);
            paintSurface.Children.Add(textBlock);
        }

        private void paintSurface_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (w3.flag)
            {
                w3.flag = false;
                addtext(e.GetPosition(this));
                return;
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(paintSurface);
            double dpi = 96d;
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, System.Windows.Media.PixelFormats.Default);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(paintSurface);
                dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }
            rtb.Render(dv);
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
            try
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                pngEncoder.Save(ms);
                ms.Close();

                System.IO.File.WriteAllBytes("canvas.PNG", ms.ToArray());
            }
            catch (Exception err)
            {
            }
            Main.NeedtoSendDrawing = true;
            this.Close();
            w1.Close();
            w2.Close();
            w3.Close();
        }
    }
}
