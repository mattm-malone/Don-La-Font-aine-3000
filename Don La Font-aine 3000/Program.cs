using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
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
        /// 
        [STAThread]
        static void Main() {
            string startupPath = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;

            // Setup the window
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form form = new Form();
            form.Text = "Don La Font-aine 3000";
            PictureBox pictureBox = new PictureBox();
            string input = Microsoft.VisualBasic.Interaction.InputBox("What is your movie title?", "Don La Font-aine 3000", "", -1, -1);

            // Get a random image in the img directory, then makes it usable for putting strings on it.
            // Any image will work, but preferably have it in 2:3 standard poster size. All included images are 667x1000 for reference.
            Bitmap img = (Bitmap)Image.FromFile(getRandomImgFile(startupPath + "\\img"));
            Graphics g = Graphics.FromImage(img);
            form.Size = img.Size;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Gets a random font file then converts to font object.
            string fontPath = getRandomFontFile(startupPath + "\\fonts");
            var imgFont = new PrivateFontCollection();
            imgFont.AddFontFile(@fontPath);
            Font selectedFont = new Font((FontFamily)imgFont.Families[0], getBestFontSize(g, input, imgFont, img.Width));

            Color contrastingColor = getContrastingColor(img);
            Brush fontColor = new SolidBrush(contrastingColor);

            int widthOfString = (int)(g.MeasureString(input, selectedFont).Width);
            
            // Draws the string in the center of image.
            g.DrawString(input, selectedFont, fontColor, (img.Width-widthOfString)/2, 100);
            g.Flush();

            pictureBox.Image = img;
            pictureBox.Dock = DockStyle.Fill;
            form.Controls.Add(pictureBox);
            Application.Run(form);
        }

        public static Brush randomColorGen()
        {
            Random numRand = new Random();
            Brush colorChosen = null;
            int colorChooser = numRand.Next(1, 3);
            if (colorChooser == 1)
            {
                colorChosen = Brushes.AliceBlue;
            }
            else if (colorChooser == 2)
            {
                colorChosen = Brushes.AntiqueWhite;
            }
            else if (colorChooser == 3)
            {
                colorChosen = Brushes.Chocolate;
            }
            return colorChosen;
        }

        /// <summary>
        /// Finds a font size that will fit within 70% the image's width.
        /// Starts at 72 and dwindles down as far as it takes to fit in.
        /// </summary>
        /// 
        public static float getBestFontSize(Graphics graphics, string stringToMeasure, PrivateFontCollection fontFamily, int imageWidth)
        {
            float fontSize = 72.0f;
            imageWidth = (int)(imageWidth * 0.80);

            Font font = new Font((FontFamily)fontFamily.Families[0], fontSize);
            int widthOfString = (int)(graphics.MeasureString(stringToMeasure, font).Width);
            
            while (widthOfString > imageWidth)
            {
                fontSize -= 2.0f;
                try
                {
                    font = new Font((FontFamily)fontFamily.Families[0], fontSize);
                    widthOfString = (int)(graphics.MeasureString(stringToMeasure, font).Width);
                }
                catch { }
            }

            return fontSize;
        }

        public static string getRandomFontFile(string directory) {
            string file = null;
            var extensions = new string[] { ".ttf" };
            try
            {
                var di = new DirectoryInfo(directory);
                var rgFiles = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
                Random R = new Random();
                file = rgFiles.ElementAt(R.Next(0, rgFiles.Count())).FullName;
            }
            catch { }
            return file;
        }


        public static string getRandomImgFile(string directory)
        {
            string file = null;
            var extensions = new string[] { ".png", ".jpg", ".gif", ".bmp", "jpeg" };
            try
            {
                var di = new DirectoryInfo(directory);
                var rgFiles = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
                Random R = new Random();
                file = rgFiles.ElementAt(R.Next(0, rgFiles.Count())).FullName;
            }
            catch { }
            return file;
        }

        public static Color getContrastingColor(Bitmap bmp)
        {
            //Used for tally
            int r = 0;
            int g = 0;
            int b = 0;
            int total = 0;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color clr = bmp.GetPixel(x, y);
                    r += clr.R;
                    g += clr.G;
                    b += clr.B;
                    total++;
                }
            }

            //Average/dominant RGB color for entire image.
            r /= total;
            g /= total;
            b /= total;

            //r += 128;
            //if (r > 255) r -= 256;

            //g += 128;
            //if (g > 255) g -= 256;

            //b += 128;
            //if (b > 255) b -= 256;

            //Inverses the average
            r = 255 - r;
            g = 255 - g;
            b = 255 - b;

            return Color.FromArgb(r, g, b);
        }



    }

}
