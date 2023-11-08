using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace hw5
{
    public partial class Form1 : Form
    {
        private Chart[] charts;
        private Point[] chartLocations;
        private Point[] mouseDownLocations;
        private bool[] isDraggingList;
        private bool[] isResizingList;
        private Size[] resizeStartSize;

        public static Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Dock = DockStyle.Fill;

            textBox1.Text = "5";
            textBox2.Text = "1";
            textBox3.Text = "100";
            textBox4.Text = "50";
            textBox5.Text = "25";

            int numberOfCharts = 2;
            charts = new Chart[numberOfCharts];
            chartLocations = new Point[numberOfCharts];
            mouseDownLocations = new Point[numberOfCharts];
            isDraggingList = new bool[numberOfCharts];
            isResizingList = new bool[numberOfCharts];
            resizeStartSize = new Size[numberOfCharts];

            for (int i = 0; i < numberOfCharts; i++)
            {
                charts[i] = Controls.Find($"chart{i + 1}", true)[0] as Chart;
                chartLocations[i] = new Point(0, 0);
                mouseDownLocations[i] = Point.Empty;
                isDraggingList[i] = false;

                charts[i].MouseDown += Chart_MouseDown;
                charts[i].MouseMove += Chart_MouseMove;
                charts[i].MouseUp += Chart_MouseUp;
            }
        }

        //------------------RESIZE E MOVE CHART------------------------
        private void Chart_MouseDown(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            int chartIndex = Array.IndexOf(charts, chart);

            if (e.Button == MouseButtons.Left)
            {
                if (e.X >= chart.Width - 10 && e.Y >= chart.Height - 10)
                {
                    isResizingList[chartIndex] = true;
                    resizeStartSize[chartIndex] = chart.Size;
                }
                else
                {
                    isDraggingList[chartIndex] = true;
                    mouseDownLocations[chartIndex] = e.Location;
                }
            }
        }

        private void Chart_MouseMove(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            int chartIndex = Array.IndexOf(charts, chart);

            if (isDraggingList[chartIndex])
            {
                int deltaX = e.X - mouseDownLocations[chartIndex].X;
                int deltaY = e.Y - mouseDownLocations[chartIndex].Y;

                chartLocations[chartIndex].X += deltaX;
                chartLocations[chartIndex].Y += deltaY;

                if (chartLocations[chartIndex].X < 0) chartLocations[chartIndex].X = 0;
                if (chartLocations[chartIndex].Y < 0) chartLocations[chartIndex].Y = 0;
                if (chartLocations[chartIndex].X + chart.Width > pictureBox1.Width) chartLocations[chartIndex].X = pictureBox1.Width - chart.Width;
                if (chartLocations[chartIndex].Y + chart.Height > pictureBox1.Height) chartLocations[chartIndex].Y = pictureBox1.Height - chart.Height;

                chart.Location = chartLocations[chartIndex];
            }
            else if (isResizingList[chartIndex])
            {
                int deltaX = e.X - resizeStartSize[chartIndex].Width;
                int deltaY = e.Y - resizeStartSize[chartIndex].Height;

                int newWidth = resizeStartSize[chartIndex].Width + deltaX;
                int newHeight = resizeStartSize[chartIndex].Height + deltaY;

                if (newWidth < 100)
                    newWidth = 100;
                if (newHeight < 100)
                    newHeight = 100;

                chart.Size = new Size(newWidth, newHeight);
            }
            else if (e.X >= chart.Width - 10 && e.Y >= chart.Height - 10)
            {
                chart.BackColor = Color.LightGray;
                chart.Cursor = Cursors.SizeNWSE;
            }
            else
            {
                chart.BackColor = Color.White;
                chart.Cursor = Cursors.Default;
            }
        }

        private void Chart_MouseUp(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            int chartIndex = Array.IndexOf(charts, chart);

            if (e.Button == MouseButtons.Left)
            {
                isDraggingList[chartIndex] = false;
                isResizingList[chartIndex] = false;
            }
        }
        //------------------------------------------------------

        private void fillChart()
        {
            //min value of probability
            float minValue = 0;
            //max value of probability
            float maxValue = 1;

            int numberOfServers = int.Parse(textBox1.Text); //M
            int periodeOfTime = int.Parse(textBox2.Text);   //T
            int numberOfInterval = int.Parse(textBox3.Text);    //N
            int lambda = int.Parse(textBox4.Text);

            int nthAttack_value = int.Parse(textBox5.Text);




            Console.WriteLine("lambda: " + lambda);
            Console.WriteLine("periodeOfTime: " + periodeOfTime);
            Console.WriteLine("numberOfInterval: " + numberOfInterval);

            float probability = (float)(lambda * periodeOfTime) /(float) (numberOfInterval);
            Console.WriteLine("probability: " + probability);


            int[] x = generateX(numberOfInterval);
            int[] y;

            int[] lastValues = new int[numberOfServers];
            int[] nthAttacks = new int[numberOfServers];

            int min;
            int max;

            chart1.Series.Clear();
            chart2.Series.Clear();

            for (int i = 0; i < numberOfServers; i++)
            {
                y = generateCoordinateVector(numberOfInterval, probability, minValue, maxValue);

                lastValues[i] = y[numberOfInterval - 1];

                nthAttacks[i] = y[nthAttack_value];

                var series = new Series($"Server {i + 1}");
                series.ChartType = SeriesChartType.Line;
                chart1.ChartAreas[0].AxisX.Minimum = 0;

                series.Points.DataBindXY(x, y);
                chart1.Series.Add(series);
            }

            int maxLast = lastValues.Max();
            int minLast = lastValues.Min();

            int maxNth = nthAttacks.Max();
            int minNth = nthAttacks.Min();

            if (maxLast >= maxNth) max = maxLast;
            else max = maxNth;

            if (minLast < minNth) min = minLast;
            else min = minNth;

            int axesLength = max - min + 1;

            int[] yHistogram = new int[axesLength];
            int[] xLast = new int[axesLength];
            int[] xNth = new int[axesLength];

            for (int i = 0; i < axesLength; i++)
            {
                yHistogram[i] = maxLast - i;
            }

            for (int i = 0; i < axesLength; i++)
            {
                for (int j = 0; j < lastValues.Length; j++)
                {
                    if (yHistogram[i] == lastValues[j])
                    {
                        xLast[i]++;
                    }
                    if (yHistogram[i] == nthAttacks[j])
                    {
                        xNth[i]++;
                    }
                }
            }

            chart2.Series.Clear();
            chart2.Series.Add("Last Interval");
            chart2.Series.Add("N-th Interval");
            chart2.Series["Last Interval"].ChartType = SeriesChartType.Bar;
            chart2.Series["N-th Interval"].ChartType = SeriesChartType.Bar;

            // Bind data to the chart
            for (int i = 0; i < yHistogram.Length; i++)
            {
                chart2.Series["Last Interval"].Points.AddXY(yHistogram[i], xLast[i]);
                chart2.Series["N-th Interval"].Points.AddXY(yHistogram[i], xNth[i]);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fillChart();
        }


        public static float GenerateRandomDouble(float minValue, float maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentException("minValue must be less than or equal to maxValue");

            float randomValue = (float)random.NextDouble(); // Generates a random double between 0 and 1
            float range = maxValue - minValue;
            float scaledValue = randomValue * range;
            float result = scaledValue + minValue;

            return result;
        }

        public static int[] generateCoordinateVector(int size, float probability, float minValue, float maxValue)
        {
            int[] y = new int[size];
            y[0] = 0;
            int sum = 0;
            float value = 0;


            for (int i = 1; i < size; i++)
            {
                value = GenerateRandomDouble(minValue, maxValue);

                sum += generateY(value, probability);

                y[i] = sum;
            }

            return y;
        }

        public static int[] generateX(int size)
        {
            int[] x = new int[size];
            for (int i = 0; i < size; i++)
            {
                x[i] = i;
            }

            return x;
        }

        public static int generateY(float attack, float probability)
        {
            if (attack < probability) return +1;
            else return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fillChart();
        }
    }
}
