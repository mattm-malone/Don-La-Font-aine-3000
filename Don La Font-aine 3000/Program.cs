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
        [STAThread]
        static void Main() {
            //Setup the window
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form form = new Form();
            form.Size = new Size(590, 332);
            form.Text = "Don La Font-aine 3000";
            PictureBox pictureBox = new PictureBox();

            string input = Microsoft.VisualBasic.Interaction.InputBox("What is your movie title?", "Don La Font-aine 3000", "", -1, -1);
            string startupPath = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;

            string fontPath = getRandomFontFile(startupPath+"\\fonts");
            var imgFont = new PrivateFontCollection();
            imgFont.AddFontFile(@fontPath);

            var spaceFont = new Font((FontFamily)imgFont.Families[0], 24f);

            Bitmap img = (Bitmap)Image.FromFile(getRandomImgFile(startupPath+ "\\img"));
            Graphics g = Graphics.FromImage(img);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawString(input, spaceFont, randomColorGen(), 0, 100);

            g.Flush();
            pictureBox.Image = img;
            pictureBox.Dock = DockStyle.Fill;
            form.Controls.Add(pictureBox);
            Application.Run(form);
        }

        public static Bitmap DrawFilledRectangle(int x, int y) {
            Bitmap bmp = new Bitmap(x, y);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                Rectangle ImageSize = new Rectangle(0, 0, x, y);
                graph.FillRectangle(Brushes.Black, ImageSize);
            }
            return bmp;
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

    }

}
