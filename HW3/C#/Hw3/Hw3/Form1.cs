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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Hw3
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

            textBox1.Text = "10";
            textBox2.Text = "10";
            textBox3.Text = "0.6";
            textBox4.Text = "1";

            int numberOfCharts = 5;
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


        //fillChart method
        private void fillChart()
        {

            int numberOfAttacks = int.Parse(textBox2.Text);
            int numberOfSystems = int.Parse(textBox1.Text);
            float probability;

            //int indexNthAttack = 3;

            if (float.TryParse(textBox3.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out probability))
            {
                Console.WriteLine("Conversione riuscita. Valore float: " + probability);
            }
            else
            {
                Console.WriteLine("Conversione non riuscita. L'input non è un valore float valido.");
            }

            int nthAttack_value = int.Parse(textBox4.Text);

            //min value of probability
            float minValue = 0;
            //max value of probability
            float maxValue = 1;



            int[] x = generateX(numberOfAttacks);
            int[] y;
            double[] cumulatedFrequency;
            double[] relativeFrequency;
            double[] normalizedRatio;

            int[] lastValues = new int[numberOfSystems];
            int[] nthAttacks = new int[numberOfSystems];



            int min;
            int max;

            chart1.Series.Clear();
            chart2.Series.Clear();
            chart3.Series.Clear();
            chart4.Series.Clear();

            chart5.Series.Clear();


            (int[], double[], double[], double[]) result;



            for (int i = 0; i < numberOfSystems; i++)
            {
                result = generateCoordinateVector(numberOfAttacks, probability, minValue, maxValue);

                y = result.Item1;
                cumulatedFrequency = result.Item2;
                relativeFrequency = result.Item3;
                normalizedRatio = result.Item4;

                lastValues[i] = y[numberOfAttacks - 1];

                nthAttacks[i] = y[nthAttack_value];
                //Console.WriteLine(lastValues[i]);

                var series = new Series($"Systems {i + 1}");
                series.ChartType = SeriesChartType.Line;
                chart1.ChartAreas[0].AxisX.Minimum = 0;

                series.Points.DataBindXY(x, y);
                chart1.Series.Add(series);

                var series2 = new Series($"Systems {i + 1}");
                series2.ChartType = SeriesChartType.Line;
                chart2.ChartAreas[0].AxisX.Minimum = 0;

                series2.Points.DataBindXY(x, cumulatedFrequency);
                chart2.Series.Add(series2);

                var series3 = new Series($"Systems {i + 1}");
                series3.ChartType = SeriesChartType.Line;
                chart3.ChartAreas[0].AxisX.Minimum = 0;

                series3.Points.DataBindXY(x, relativeFrequency);
                chart3.Series.Add(series3);

                var series4 = new Series($"Systems {i + 1}");
                series4.ChartType = SeriesChartType.Line;
                chart4.ChartAreas[0].AxisX.Minimum = 0;

                series4.Points.DataBindXY(x, normalizedRatio);
                chart4.Series.Add(series4);
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

            chart5.Series.Clear();
            chart5.Series.Add("Last Attack");
            chart5.Series.Add("N-th Attack");
            chart5.Series["Last Attack"].ChartType = SeriesChartType.Bar;
            chart5.Series["N-th Attack"].ChartType = SeriesChartType.Bar;

            // Bind data to the chart
            for (int i = 0; i < yHistogram.Length; i++)
            {
                chart5.Series["Last Attack"].Points.AddXY(yHistogram[i], xLast[i]);
                chart5.Series["N-th Attack"].Points.AddXY(yHistogram[i], xNth[i]);
            }

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            fillChart();
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

        public static (int[], double[], double[], double[]) generateCoordinateVector(int size, float probability, float minValue, float maxValue)
        {
            int[] y = new int[size];
            double[] cumulatedFrequency = new double[size];
            double[] relativeFrequency = new double[size];
            double[] normalizedRatio = new double[size];
            y[0] = 0;
            int sum = 0;
            double sumFrequency = 0;
            float value = 0;


            for (int i = 1; i < size; i++)
            {
                value = GenerateRandomDouble(minValue, maxValue);

                sum += generateY(value, probability);
                sumFrequency += generateFrequencyY(value, probability);

                y[i] = sum;
                cumulatedFrequency[i] = sumFrequency;
                relativeFrequency[i] = sumFrequency / i + 1;
                normalizedRatio[i] = sumFrequency / Math.Sqrt(i + 1);
            }

            return (y, cumulatedFrequency, relativeFrequency, normalizedRatio);
        }

        public static int generateY(float attack, float probability)
        {
            if (attack <= probability) return -1;
            else return +1;
        }

        public static int generateFrequencyY(float attack, float probability)
        {
            if (attack > probability) return +1;
            else return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //timer1.Stop(); // Arresta il Timer se è in esecuzione
            //timer1.Start(); // Riavvia il Timer
            fillChart();

        }


    }
}
