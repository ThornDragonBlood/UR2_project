using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UR2_Lab_7
{
    public partial class Form1 : Form
    {
        VideoCapture _capture;
        Thread _captureThread;
        SerialPort arduinoSerial = new SerialPort();
        bool enableCoordinateSending = true;
        Thread serialMonitoringThread;
        bool st = false;
        int xpix = 0;
        int ypix = 0;
        int disconver = 0;
        int angleconver = 0;
        double x = 0;
        double y = 0;
        double angle = 0;
        double dis = 0;
        bool ISsqu = false;
        int ISsquint = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _capture = new VideoCapture(1);
            _captureThread = new Thread(DisplayWebcam);
            _captureThread.Start();
            try
            {
                arduinoSerial.PortName = "COM4";
                arduinoSerial.BaudRate = 115200;
                arduinoSerial.Open();
                serialMonitoringThread = new Thread(MonitorSerialData);
                serialMonitoringThread.Start();
                X1.Text = "130";
                Y1.Text = "224";
                SQ.Text = "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Initializing COM port");
                Close();
            }
        }

        private void DisplayWebcam()
        {
            while (_capture.IsOpened)
            {
                Mat sourceFrame = _capture.QueryFrame();
                var blurredImage = new Mat();
                int newHeight = (sourceFrame.Size.Height * emguPictureBox.Size.Width) / sourceFrame.Size.Width;
                Size newSize = new Size(emguPictureBox.Size.Width, newHeight);
                CvInvoke.Resize(sourceFrame, sourceFrame, newSize);
                emguPictureBox.Image = sourceFrame.Bitmap;
                Mat sourceFrameWithArt = sourceFrame.Clone();
                // create an image version of the source frame, will be used when warping the image
                Image<Bgr, byte> sourceFrameWarped = sourceFrame.ToImage<Bgr, byte>();
                // Isolating the ROI: convert to a gray, apply binary threshold:
                Image<Gray, byte> grayImg = sourceFrame.ToImage<Gray, byte>().ThresholdBinary(new Gray(125), new
                Gray(255));
                CvInvoke.GaussianBlur(grayImg.Mat, blurredImage, new Size(9, 9), 0);
                using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
                {
                    // Build list of contours
                    CvInvoke.FindContours(blurredImage, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                    // Selecting largest contour
                    if (contours.Size > 0)
                    {
                        double maxArea = 0;
                        int chosen = 0;
                        int numberc = 0;
                        int numberm = 0;
                        int framwith = 0;
                        int framhight = 0;
                        bool iSq = false;
                        for (int i = 0; i < contours.Size; i++)
                        {
                            VectorOfPoint contour = contours[i];
                            double area = CvInvoke.ContourArea(contour);
                            if (area > maxArea)
                            {
                                maxArea = area;
                                chosen = i;
                            }
                            numberc++;
                            if (area >= 1000 && area < 10000)
                            {
                                // Getting minimal rectangle which contains the contour
                                Rectangle boundingBox = CvInvoke.BoundingRectangle(contours[chosen]);
                                // Draw on the display frame
                                if(area > 2000)
                                {
                                    iSq = true;
                                }
                                if(area > 2000 && st == false)
                                {
                                    ISsqu = true;
                                }else if (st == false)
                                {
                                    ISsqu = false;
                                }
                                Point center = MarkDetectedObject(sourceFrameWithArt, contours[chosen], boundingBox, area, iSq);
                                // Create a slightly larger bounding rectangle, we'll set it as the ROI for later warping
                                sourceFrameWarped.ROI = new Rectangle((int)Math.Min(0, boundingBox.X - 30),
                                (int)Math.Min(0, boundingBox.Y - 30),
                                (int)Math.Max(sourceFrameWarped.Width - 1, boundingBox.X +
                                boundingBox.Width + 30),
                                (int)Math.Max(sourceFrameWarped.Height - 1, boundingBox.Y +
                                boundingBox.Height + 30));
                                // Display the version of the source image with the added artwork, simulating ROI focus:
                                //roiPictureBox.Image = sourceFrameWithArt.Bitmap;
                                // Warp the image, output it
                                //warpedPictureBox.Image = WarpImage(sourceFrameWarped, contours[chosen]).Bitmap;
                                numberm++;
                                if (st == false)
                                {
                                    xpix = center.X;
                                    ypix = center.Y;
                                    framwith = 389;
                                    framhight = 328;
                                    st = true;
                                }
                            }
                        }
                        
                        if(st == true)
                        {
                            // prosses the infomation from the images in to the coordinates
                            x = xpix * 11.5;
                            y = ypix * 6.5;
                            x = (x / 389) - 5.5;
                            y = (y / 328) + 3.5 + 6.5;
                            angle = Math.Atan(x / y) * 57.296;
                            angle = (angle + 90);
                            dis = (Math.Sqrt((x * x) + (y * y)) - 7) * 10.0;
                            disconver = Convert.ToInt32(dis);
                            angleconver = Convert.ToInt32(angle);
                            if(ISsqu == true)
                            {
                                ISsquint = 1;
                            }
                            else
                            {
                                ISsquint = 0;
                            }
                            Invoke(new Action(() =>
                            {
                                //input coordinates to the text boxs to be sended Arduimo
                                X1.Text = "" + disconver.ToString();
                                Y1.Text = "" + angleconver.ToString();
                                SQ.Text = "" + ISsquint.ToString();
                            }));
                        }

                        if (enableCoordinateSending == true)
                        {
                            Send();
                        }

                        // outputs the coordinates to the useser
                        Invoke(new Action(() =>
                        {
                            countc.Text = $"Number of contours " + numberc;
                        }));
                        Invoke(new Action(() =>
                        {
                            countm.Text = $"Number of metal shape contours " + numberm;
                        }));
                        Invoke(new Action(() =>
                        {
                            out_put.Text = $"the angle in degrees " + angle + " the distance from point of rotation " + dis;
                        }));
                    }
                }
            }
        }

        private void MonitorSerialData()
        {
            while (true)
            {
                // block until \n character is received, extract command data
                string msg = arduinoSerial.ReadLine();
                // confirm the string has both < and > characters
                if (msg.IndexOf("<") == -1 || msg.IndexOf(">") == -1)
                {
                    continue;
                }
                // remove everything before (and including) the < character
                msg = msg.Substring(msg.IndexOf("<") + 1);
                // remove everything after (and including) the > character
                msg = msg.Remove(msg.IndexOf(">"));
                // if the resulting string is empty, disregard and move on
                if (msg.Length == 0)
                {
                    continue;
                }
                // parse the command
                if (msg.Substring(0, 1) == "S")
                {
                    // command is to suspend, toggle states accordingly:
                    ToggleFieldAvailability(msg.Substring(1, 1) == "1");
                }
                else if (msg.Substring(0, 1) == "P")
                {
                    // command is to display the point data, output to the text field:
                    Invoke(new Action(() =>
                    {
                        return1.Text = $"Returned Point Data: {msg.Substring(1)}";
                        st = false;
                    }));
                }
                else if (msg.Substring(0, 1) == "t")
                {
                    // command is to display the point data, output to the text field:
                    Invoke(new Action(() =>
                    {
                        return1.Text = $"Returned Point Data: {msg.Substring(1)}";
                    }));
                }
            }
        }

        private static Image<Bgr, Byte> WarpImage(Image<Bgr, byte> frame, VectorOfPoint contour)
        {
            // set the output size:
            var size = new Size(frame.Width, frame.Height);
            using (VectorOfPoint approxContour = new VectorOfPoint())
            {
                CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
                // get an array of points in the contour
                Point[] points = approxContour.ToArray();
                // if array length isn't 4, something went wrong, abort warping process (for demo, draw points instead)
                if (points.Length != 4)
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        frame.Draw(new CircleF(points[i], 5), new Bgr(Color.Red), 5);
                    }
                    return frame;
                }
                IEnumerable<Point> query = points.OrderBy(point => point.Y).ThenBy(point => point.X);
                PointF[] ptsSrc = new PointF[4];
                PointF[] ptsDst = new PointF[] { new PointF(0, 0), new PointF(size.Width - 1, 0), new PointF(0, size.Height - 1),
                new PointF(size.Width - 1, size.Height - 1) };
                for (int i = 0; i < 4; i++)
                {
                    ptsSrc[i] = new PointF(query.ElementAt(i).X, query.ElementAt(i).Y);
                }
                using (var matrix = CvInvoke.GetPerspectiveTransform(ptsSrc, ptsDst))
                {
                    using (var cutImagePortion = new Mat())
                    {
                        CvInvoke.WarpPerspective(frame, cutImagePortion, matrix, size, Inter.Cubic);
                        return cutImagePortion.ToImage<Bgr, Byte>();
                    }
                }
            }
        }
        private static Point MarkDetectedObject(Mat frame, VectorOfPoint contour, Rectangle boundingBox, double area, bool iSq)
        {
            // Write information next to marked object
            Point center = new Point(boundingBox.X + boundingBox.Width / 2, boundingBox.Y + boundingBox.Height / 2);
            // Drawing contour and box around it
            if (iSq == true)
            {
                // Draw around square
                CvInvoke.Polylines(frame, contour, true, new Bgr(Color.Red).MCvScalar);
                CvInvoke.Rectangle(frame, boundingBox, new Bgr(Color.Red).MCvScalar);
                CvInvoke.Circle(frame, center, 1, new Bgr(Color.Red).MCvScalar);
            }
            else
            {
                // Drae around triangle
                CvInvoke.Polylines(frame, contour, true, new Bgr(Color.Blue).MCvScalar);
                CvInvoke.Rectangle(frame, boundingBox, new Bgr(Color.Blue).MCvScalar);
                CvInvoke.Circle(frame, center, 1, new Bgr(Color.Blue).MCvScalar);
            }

            var info = new string[]
            {
                $"Area: {area}",
                $"Position: {center.X}, {center.Y}"
            };
            WriteMultilineText(frame, info, new Point(center.X, boundingBox.Bottom + 12));
            return center;
        }
        private static void WriteMultilineText(Mat frame, string[] lines, Point origin)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                int y = i * 10 + origin.Y; // Moving down on each line
                CvInvoke.PutText(frame, lines[i], new Point(origin.X, y),
                FontFace.HersheyPlain, 0.8, new Bgr(Color.Red).MCvScalar);
            }
        }

        // turn on off coordinate sending
        private void ToggleFieldAvailability(bool suspend)
        {
            Invoke(new Action(() =>
            {
                enableCoordinateSending = !suspend;
                loc.Text = $"State: {(suspend ? "Locked" : "Unlocked")}";
            }));
        }

        // send coordinates to Arduino auto
        private void Send()
        {
            if (!enableCoordinateSending)
            {
                MessageBox.Show("Temporarily locked...");
                return;
            }
            int x = -1;
            int y = -1;
            int squ = -1;
            if (int.TryParse(X1.Text, out x) && int.TryParse(Y1.Text, out y) && int.TryParse(SQ.Text, out squ))
            {
                byte[] buffer = new byte[5]
                {
                    Encoding.ASCII.GetBytes("<")[0],
                    Convert.ToByte(x),
                    Convert.ToByte(y),
                    Convert.ToByte(squ),
                    Encoding.ASCII.GetBytes(">")[0]
                };
                arduinoSerial.Write(buffer, 0, 5);
            }
            else
            {
                MessageBox.Show("X and Y values must be integers", "Unable to parse coordinates");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _captureThread.Abort();
            serialMonitoringThread.Abort();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        // send coordinates to Arduino Manual
        private void button1_Click(object sender, EventArgs e)
        {
            if (!enableCoordinateSending)
            {
                MessageBox.Show("Temporarily locked...");
                return;
            }
            int x = -1;
            int y = -1;
            int squ = -1;
            if (int.TryParse(X1.Text, out x) && int.TryParse(Y1.Text, out y) && int.TryParse(SQ.Text, out squ))
            {
                byte[] buffer = new byte[5]
                {
                    Encoding.ASCII.GetBytes("<")[0],
                    Convert.ToByte(x),
                    Convert.ToByte(y),
                    Convert.ToByte(squ),
                    Encoding.ASCII.GetBytes(">")[0]
                };
                arduinoSerial.Write(buffer, 0, 5);
            }
            else
            {
                MessageBox.Show("X and Y values must be integers", "Unable to parse coordinates");
            }
        }

        private void SQ_TextChanged(object sender, EventArgs e)
        {

        }

        // Up date coordinates in computer
        private void button2_Click(object sender, EventArgs e)
        {
            st = false;
        }
    }
}
