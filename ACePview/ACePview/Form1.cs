using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACePview
{
    public partial class Form1 : Form
    {
        public System.Drawing.Color getcolour(int colour)
        {
            colour = colour > 7 || colour < 0 ? 8 : colour;
/*
            int[,] palette = new int[9, 3]
            {
                { 0, 0, 0 },
                { 255, 255, 255},
                { 67, 138, 28},
                { 100, 64, 255},
                { 191, 0, 0},
                { 255, 243, 56},
                { 232, 126, 0},
                { 194, 164, 244},
                {200, 0, 200 } // if colour specified is not in range 0..7
            };
*/
            int[,] palette = new int[9, 3]
{
                { 0, 0, 0 },
                { 255, 255, 255},
                { 67, 138, 28},
                { 36, 36, 255},
                { 191, 0, 0},
                { 255, 243, 56},
                { 232, 126, 0},
                { 235, 164, 244},
                {200, 0, 200 } // if colour specified is not in range 0..7
};



            /*
                        int[,] palette = new int[9, 3]
            {
                        { 0, 0, 0 },
                        { 255, 255, 255},
                        { 0, 255, 0},
                        { 0, 0, 255},
                        { 255, 0, 0},
                        { 255, 255, 0},
                        { 255, 128, 0},
                        { 255, 0, 128},
                        {200, 0, 200 } // if colour specified is not in range 0..7
            };
            */

            return Color.FromArgb(255, palette[colour, 0], palette[colour, 1], palette[colour, 2]);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();

            for (int i = 1; i < args.Count(); i++)
            {
                string fileName = args[i];
                Bitmap bm = new Bitmap(448, 600);
                using (Graphics gr = Graphics.FromImage(bm))
                {
                    byte[] buff = File.ReadAllBytes(fileName);

                    int x = 0;
                    int y = 0;

                    foreach (byte b in buff)
                    {
                        int pxh = (b & 0xF0) >> 4;
                        int pxl = (b & 0x0F);

                        gr.DrawRectangle(new Pen(getcolour(pxh), 1), x, 600 - y, 1, 1);
                        gr.DrawRectangle(new Pen(getcolour(pxl), 1), x, 600 - (y + 1), 1, 1);

                        y = y + 2;
                        if (y == 600)
                        {
                            y = 0;
                            x = x + 1;
                        }

                        if (x == 448 && y == 600)
                        {
                            break;
                        }
                    }
                }

                acep_picturebox.Image = bm;
                bm.Save(fileName + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }
}
