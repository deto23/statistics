using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace hw6
{
    public partial class Form1 : Form
    {

        private Chart[] charts;
        private Point[] chartLocations;
        private Point[] mouseDownLocations;
        private bool[] isDraggingList;
        private bool[] isResizingList;
        private Size[] resizeStartSize;

        private static int discarded;
        private static Dictionary<int, int> histogramData = Enumerable.Range(0, 9)
            .ToDictionary(i => -(i + 2) * 10, i => 0);


        public static Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Dock = DockStyle.Fill;

            textBox1.Text = "10";
            textBox2.Text = "250";
            textBox3.Text = "0,5";
            textBox4.Text = "20";

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


        private void Form1_Load(object sender, EventArgs e)
        {
            fillChart();
        }

        private void fillChart()
        {
            //min value of probability
            float minValue = 0;
            //max value of probability
            float maxValue = 1;

            int numberOfSystems = int.Parse(textBox1.Text); //M
            int numberOfAttacks = int.Parse(textBox2.Text);   //N
            float probability = float.Parse(textBox3.Text);  //p
            int securityScore = int.Parse(textBox4.Text);   //S

            discarded = 0;
            histogramData = Enumerable.Range(0, 9)
            .ToDictionary(i => -(i + 2) * 10, i => 0);

        int[] x = generateX(numberOfAttacks);
            int[] y;

            chart1.Series.Clear();

            for (int i = 0; i < numberOfSystems; i++)
            {
                List<int> penetrationScores = new List<int> { -20, -30, -40, -50, -60, -70, -80, -90, -100};

                y = generateCoordinateVector(numberOfAttacks, probability, minValue, maxValue, securityScore, penetrationScores);

                var series = new Series($"Server {i + 1}");
                series.ChartType = SeriesChartType.Line;
                chart1.ChartAreas[0].AxisX.Minimum = 0;

                series.Points.DataBindXY(x, y);
                chart1.Series.Add(series);
            }
            

            float percentage = (((float)discarded / (float)numberOfSystems) * 100);
            Console.WriteLine(percentage);
            label1.Text = "Percentage of unsecure system: " + percentage.ToString() + "%";

            CalculateSystemDiscard(numberOfSystems);

        }

        private void CalculateSystemDiscard(int numberOfSystems)
        {
            CreateHistogramChart(numberOfSystems);
        }

        private void CreateHistogramChart(int numberOfSystems)
        {
            chart2.Series.Clear();

            // Creazione della serie
            var series = new Series("Unsecure Score")
            {
                ChartType = SeriesChartType.Column
            };

            // Aggiunta dei punti dati alla serie
            foreach (var dataPoint in histogramData)
            {
                series.Points.AddXY(dataPoint.Key.ToString(), dataPoint.Value);
            }

            // Aggiunta della serie al grafico
            chart2.Series.Add(series);

            chart2.ChartAreas[0].AxisY.Minimum = 0; // Valore minimo dell'asse y
            chart2.ChartAreas[0].AxisY.Maximum = numberOfSystems; // Valore massimo dell'asse y
            chart2.ChartAreas[0].AxisY.Interval = 1; // Intervallo tra i tick dell'asse y

            // Impostazione delle opzioni del grafico
            chart2.ChartAreas[0].AxisX.Minimum = 0;

            // Visualizzazione del grafico
            chart2.Show();
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

        public static int[] generateCoordinateVector(int size, float probability, float minValue, float maxValue, int securityScore, List<int> penetrationScores)
        {
            int[] y = new int[size];
            y[0] = 0;
            int sum = 0;
            float value;
            bool systemChecked = false;
            bool systemSafe = false;


            for (int i = 1; i < size; i++)
            {
                value = GenerateRandomDouble(minValue, maxValue);

                sum += generateY(value, probability);

                if (sum == securityScore)
                {
                    systemChecked = true;
                    systemSafe = true;
                }
                else if (sum == penetrationScores[0])
                {
                    if (!systemChecked)
                    {
                        systemChecked = true;
                        discarded += 1;
                    }
                    else if (systemSafe == true)
                    {
                        break;
                    }
                    histogramData[penetrationScores[0]] += 1;
                    penetrationScores.RemoveAt(0);
                }

                y[i] = sum;
            }

            return y;
        }

        public static Dictionary<int, int> createDictionary(int[] penetrationScores)
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();

            foreach (int score in penetrationScores)
            {
                dict[score] = 0;
            }

            return dict;
        }

        public static Dictionary<int, int> CheckDict(Dictionary<int, int> dict, int value)
        {
            foreach (var key in dict.Keys.ToList())
            {
                if (value <= key)
                {
                    dict[key] = 1;
                }
            }

            return dict;
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
            if (attack <= probability) return -1;
            else return 1;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            fillChart();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
