﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Don_La_Font_aine_3000
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //Jessica was here 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form form = new Form();
            form.Text = "Image Viewer";
            PictureBox pictureBox = new PictureBox();
            string directory = System.IO.Path.GetDirectoryName(Application.ExecutablePath)s

            var starwars = new PrivateFontCollection();
            // Provide the path to the font on the filesystem
            starwars.AddFontFile(@"C:\Users\Matt\source\repos\Don La Font-aine 3000\Don La Font-aine 3000\bin\fonts\Starjout.ttf");

            var spaceFont = new Font((FontFamily)starwars.Families[0], 18f);

            Bitmap img = DrawFilledRectangle(300, 300);
            Graphics g = Graphics.FromImage(img);

            g.SmoothingMode = SmoothingMode.None;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawString("ok we get it", spaceFont, Brushes.Yellow, 50, 100);

            g.Flush();
            pictureBox.Image = img;
            pictureBox.Dock = DockStyle.Fill;
            form.Controls.Add(pictureBox);
            Application.Run(form);
        }

        public static Bitmap DrawFilledRectangle(int x, int y)
        {
            Bitmap bmp = new Bitmap(x, y);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                Rectangle ImageSize = new Rectangle(0, 0, x, y);
                graph.FillRectangle(Brushes.Black, ImageSize);
            }
            return bmp;
        }

    }

}
