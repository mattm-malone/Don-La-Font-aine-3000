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
            int widthOfString = (int)(g.MeasureString(input, selectedFont).Width);

            Color averageColor = getAverageColorOfArea(img, ((img.Width - widthOfString) / 2), ((img.Width - widthOfString) / 2) + widthOfString, 100, 100 + (int)(g.MeasureString(input, selectedFont).Height));
            Color contrastingColor = findContrastingColor(averageColor);
            Brush fontColor = new SolidBrush(contrastingColor);

            
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
                Random R = new Random((int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber)));
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
                Random R = new Random((int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber)));
                file = rgFiles.ElementAt(R.Next(0, rgFiles.Count())).FullName;
            }
            catch { }
            return file;
        }

        public static Color getAverageColorOfArea(Bitmap bmp, int x0, int x1, int y0, int y1)
        {
            //Used for tally
            int r = 0;
            int g = 0;
            int b = 0;
            int total = 0;

            for (int x = x0; x < x1; x++)
            {
                for (int y = y0; y < y1; y++)
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
            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Returns a complementary or contrasting color to make the drawn string
        /// contrast against the background.
        /// </summary>
        /// <returns></returns>
        public static Color findContrastingColor(Color baseColor)
        {
            // First convert to HSB
            float hue, saturation, brightness;
            float[] hsbvals = new float[3];
            Color contrast;

            int cmax = (baseColor.R > baseColor.G) ? baseColor.R : baseColor.G;
            if (baseColor.B > cmax) cmax = baseColor.B;
            int cmin = (baseColor.R < baseColor.G) ? baseColor.R : baseColor.G;
            if (baseColor.B < cmin) cmin = baseColor.B;

            brightness = cmax / 255.0f;
            if (cmax != 0)
            {
                saturation = ((float)(cmax - cmin)) / ((float)cmax);
            }
            else
            {
                saturation = 0;
            }

            if (saturation == 0)
            {
                hue = 0;
            }
            else
            {
                float redc = ((float)(cmax - baseColor.R)) / ((float)(cmax - cmin));
                float greenc = ((float)(cmax - baseColor.G)) / ((float)(cmax - cmin));
                float bluec = ((float)(cmax - baseColor.B)) / ((float)(cmax - cmin));
                if (baseColor.R == cmax)
                {
                    hue = bluec - greenc;
                }
                else if (baseColor.G == cmax)
                {
                    hue = 2.0f + redc - bluec;
                }
                else
                {
                    hue = 4.0f + greenc - redc;
                }
                hue = hue / 6.0f;
                if (hue < 0)
                {
                    hue += 1.0f;
                }
            }
            // Add 180 degrees to the hue.
            hue = (hue + 0.5f);
            if (hue > 1.0f)
            {
                hue -= 1.0f;
            }
            brightness = 1.0f - brightness;

            // Now we have our final hsb, then convert back.
            int r = 0, g = 0, b = 0;

            if (saturation == 0)
            {
                r = g = b = (int)(brightness * 255.0f + 0.5f);
            }
            else
            {
                float h = (hue - (float)Math.Floor(hue)) * 0.6f;
                float f = h - (float)Math.Floor(h);
                float p = brightness * (1.0f - saturation);
                float q = brightness * (1.0f - saturation * f);
                float t = brightness * (1.0f - (saturation * (1.0f - f)));
                switch ((int)h)
                {
                    case 0:
                        r = (int)(brightness * 255.0f + 0.5f);
                        g = (int)(t * 255.0f + 0.5f);
                        b = (int)(p * 255.0f + 0.5f);
                        break;
                    case 1:
                        r = (int)(q * 255.0f + 0.5f);
                        g = (int)(brightness * 255.0f + 0.5f);
                        b = (int)(p * 255.0f + 0.5f);
                        break;
                    case 2:
                        r = (int)(p * 255.0f + 0.5f);
                        g = (int)(brightness * 255.0f + 0.5f);
                        b = (int)(t * 255.0f + 0.5f);
                        break;
                    case 3:
                        r = (int)(p * 255.0f + 0.5f);
                        g = (int)(q * 255.0f + 0.5f);
                        b = (int)(brightness * 255.0f + 0.5f);
                        break;
                    case 4:
                        r = (int)(t * 255.0f + 0.5f);
                        g = (int)(p * 255.0f + 0.5f);
                        b = (int)(brightness * 255.0f + 0.5f);
                        break;
                    case 5:
                        r = (int)(brightness * 255.0f + 0.5f);
                        g = (int)(p * 255.0f + 0.5f);
                        b = (int)(q * 255.0f + 0.5f);
                        break;
                }
            }
            return Color.FromArgb(r, g, b);
        }
    }

}
