using Ex2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms; // Assicurati di aggiungere questa direttiva

namespace HistogramCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            const int n = 1000000;
            const int k = 50;

            var list = new List<double>();

            var random = new Random();
            for (var i = 0; i < n; i++)
            {
                list.Add(random.NextDouble());
            }

            var dict = new Dictionary<double, int>();
            for (var i = 1; i <= k; i++)
            {
                dict[i / (double)k] = 0;
            }

            for (var j = 0; j < n; j++)
            {
                for (var i = 1; i <= k; i++)
                {
                    if (list[j] > (i - 1) / (double)k && list[j] <= i / (double)k)
                    {
                        dict[i / (double)k]++;
                    }
                }
            }

            var sortedKeys = new List<double>(dict.Keys);
            sortedKeys.Sort();

            // Calcola i punti medi degli intervalli
            var intervals = new List<string>();
            foreach (var key in sortedKeys)
            {
                intervals.Add($"{(key - 1 / k):F2} - {key:F2}");
            }

            // Calcola i conteggi
            var counts = new List<int>();
            foreach (var key in sortedKeys)
            {
                counts.Add(dict[key]);
            }

            int width = 800; // Larghezza dell'istogramma
            int height = 400; // Altezza dell'istogramma
            Bitmap histogramBitmap = new Bitmap(width, height);

            // Crea un oggetto Graphics per disegnare sull'immagine Bitmap
            using (Graphics g = Graphics.FromImage(histogramBitmap))
            {
                // Imposta il colore delle barre dell'istogramma
                Color barColor = Color.DeepSkyBlue;
                Color borderColor = Color.Black;

                // Calcola la larghezza di ciascuna barra dell'istogramma
                float barWidth = (float)width / k;

                // Calcola l'altezza massima per le barre dell'istogramma
                int maxCount = counts.Max();
                float scale = height / (float)maxCount;

                // Disegna le barre dell'istogramma
                for (int i = 0; i < k; i++)
                {
                    float barHeight = counts[i] * scale;
                    float x = i * barWidth;
                    float y = height - barHeight;

                    RectangleF barRectangle = new RectangleF(x, y, barWidth, barHeight);
                    g.FillRectangle(new SolidBrush(barColor), barRectangle);
                    g.DrawRectangle(new Pen(borderColor), Rectangle.Round(barRectangle));
                }
            }

            PictureBox pictureBox = new PictureBox();
            pictureBox.Size = new Size(width, height); // Imposta le dimensioni desiderate
            pictureBox.Image = histogramBitmap;

            Form form = new Form();
            form.ClientSize = new Size(width, height); // Imposta le dimensioni desiderate per il form
            form.Controls.Add(pictureBox);

            histogramBitmap.Save("histogram.png", ImageFormat.Png);

            Application.Run(form);
        }
    }
}
