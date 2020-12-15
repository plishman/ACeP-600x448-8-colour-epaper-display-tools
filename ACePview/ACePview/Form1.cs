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
        const int BM_WIDTH = 600;
        const int BM_HEIGHT = 448;
        const int BM_PX_PER_BYTE = 2;

        const int BITMAP_SIZE_BYTES = (BM_WIDTH * BM_HEIGHT) / BM_PX_PER_BYTE;
        const int PALETTE_NUMENTRIES = 8;
        const int PALETTE_ENTRYSIZE = 4;

        int[,] palette_acep = new int[9, 3]  // natural palette for the display (more muted colours, should give a good match for the display)
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

        int[,] palette_8_mono = new int[9, 3]
        {
            { 255, 255, 255 },  // white = 0
            { 222, 222, 222 },
            { 190, 190, 190 },
            { 148, 148, 148 },
            { 116, 116, 116 },
            { 76, 76, 76 },
            { 42, 42, 42 },
            { 0, 0, 0 },
            {200, 0, 200 } // if colour specified is not in range 0..7
        };

        int[,] palette_black_red_8 = new int[17, 3]
        {
            { 255, 255, 255 },  // white = 0
            { 222, 222, 222 },
            { 190, 190, 190 },
            { 148, 148, 148 },
            { 116, 116, 116 },
            { 76, 76, 76 },
            { 42, 42, 42 },
            { 0, 0, 0 },
            { 255, 255, 255 },
            { 255, 214, 214 },
            {255, 181, 181 },
            {255, 148, 148 },
            {255, 107, 107 },
            {255, 76, 76 },
            {255, 33, 33 },
            {255, 0, 0 },
            {200, 0, 200 } // if colour specified is not in range 0..15 (can't happen, as pixels are in 4 bits in format [r/b][int2][int1][int0]!)
        };
/*
        public void SetPalette()  // read ACePpalette.txt entries into palette array. Format of each line is hex #aarrggbb string
        {
            var lines = File.ReadAllLines(@".\acep_palette.txt");

            int paletteentry = 0;

            foreach (var line in lines)
            {
                string[] p = line.Split('#');
                if (p.Length == 2 && p[1].Length >= 6)
                {
                    string sr = p[1].Substring(0, 2);
                    string sg = p[1].Substring(2, 2);
                    string sb = p[1].Substring(4, 2);

                    byte r = (byte)int.Parse(sr, System.Globalization.NumberStyles.HexNumber);
                    byte g = (byte)int.Parse(sg, System.Globalization.NumberStyles.HexNumber);
                    byte b = (byte)int.Parse(sb, System.Globalization.NumberStyles.HexNumber);

                    palette[paletteentry, 0] = r;
                    palette[paletteentry, 1] = g;
                    palette[paletteentry, 2] = b;
                }

                paletteentry++;
                if (paletteentry >= PALETTE_NUMENTRIES) break;
            }
        }
*/

        public System.Drawing.Color GetColour(int colour, int[,] palette)
        {
            int palette_size = palette.Length;
            colour = colour > palette_size || colour < 0 ? palette_size + 1 : colour;
            return Color.FromArgb(255, palette[colour, 0], palette[colour, 1], palette[colour, 2]);
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
            */


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
        }

        public Form1()
        {
            InitializeComponent();
        }

        enum BMTypes { ACeP, bw, bwr };

        private void Form1_Load(object sender, EventArgs e)
        {
            //SetPalette();

            string[] args = Environment.GetCommandLineArgs();

            for (int i = 1; i < args.Count(); i++)
            {
                int bmwidth = 448;
                int bmheight = 600;
                int[,] palette = palette_acep;

                BMTypes BMType = BMTypes.ACeP;

                string fileName = args[i];
                if (fileName.EndsWith(".ACeP"))
                {
                    bmwidth = 448;
                    bmheight = 600;
                    palette = palette_acep;
                }

                if (fileName.EndsWith(".bw"))
                {
                    bmwidth = 400;
                    bmheight = 300;
                    palette = palette_8_mono;
                    BMType = BMTypes.bw;
                }

                if (fileName.EndsWith(".bwr"))
                {
                    bmwidth = 400;
                    bmheight = 300;
                    palette = palette_black_red_8;
                    BMType = BMTypes.bwr;
                }

                this.ClientSize = new System.Drawing.Size(bmwidth, bmheight);
                acep_picturebox.Size = new System.Drawing.Size(bmwidth, bmheight);

                Bitmap bm = new Bitmap(bmwidth, bmheight);
                using (Graphics gr = Graphics.FromImage(bm))
                {
                    byte[] buff = File.ReadAllBytes(fileName);

                    int x = 0;
                    int y = 0;

                    int bytecounter = 0;

                    foreach (byte b in buff)
                    {
                        /*
                        if (bytecounter < 4 * 8)    // skip over palette entries (8 entries of 4 bytes xRGB) rem Palette will be at end of bitmap, at(600*448)/2 bytes
                        {
                            bytecounter++;
                            continue;
                        }
                        */

                        int pxh = (b & 0xF0) >> 4;
                        int pxl = (b & 0x0F);

                        if (BMType == BMTypes.ACeP)
                        {
                            gr.DrawRectangle(new Pen(GetColour(pxh, palette), 1), x, bmheight - y, 1, 1);
                            gr.DrawRectangle(new Pen(GetColour(pxl, palette), 1), x, bmheight - (y + 1), 1, 1);

                            y = y + 2;
                            if (y == bmheight)
                            {
                                y = 0;
                                x += 1;
                            }
                        }
                        else
                        {
                            gr.DrawRectangle(new Pen(GetColour(pxh, palette), 1), x, y, 1, 1);
                            gr.DrawRectangle(new Pen(GetColour(pxl, palette), 1), x + 1, y, 1, 1);

                            x = x + 2;
                            if (x == bmwidth)
                            {
                                x = 0;
                                y += 1;
                            }
                        }

                        if (x == bmwidth && y == bmheight)
                        {
                            break;
                        }
                        bytecounter++;
                    }
                }

                acep_picturebox.Image = bm;
                bm.Save(fileName + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }
}
