using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;

namespace ProcessACeP
{
    class Program
    {
        static void Main(string[] args)
        {
            string sanctpath = @".\en-1962.txt";
            string calendar = ""; 

            if (File.Exists(sanctpath))
            {
                calendar = File.ReadAllText(sanctpath);
            }
                       
            string batFileName = @".\processACeP.bat";

            try {
                string[] fileArray = Directory.GetFiles(@"..\source", "*.*");

                if (fileArray.Count() > 0)
                {
                    // Check if file already exists. If yes, delete it.     
                    if (File.Exists(batFileName))
                    {
                        File.Delete(batFileName);
                    }

                    // Create a new file     
                    using (StreamWriter sw = File.CreateText(batFileName))
                    {
                        sw.WriteLine("rem Do Dithering");
                        sw.WriteLine("mkdir ..\\dithered");
                        sw.WriteLine("del ..\\dithered\\*.png /Q");
                        foreach (string fileName in fileArray)
                        {
                            string caption = "";
                            string colour = "black";

                            string outFileName = fileName.Replace("..\\source\\", "");
                            outFileName = outFileName.Substring(0, outFileName.LastIndexOf(".")) + ".png";

                            caption = GetCaption(outFileName, calendar, out colour);
                            if (caption != "")
                            {
                                int stringwidth = MeasureStringWidth(caption, new Font("Times New Roman", 12));
                                int bmwidth = 448;
                                int bmheight = 572;

                                if (stringwidth > 448)
                                {
                                    bmheight -= 28;
                                }

                                string captionFileName = "..\\dithered\\" + outFileName + ".caption.txt";
                                using (StreamWriter swc = File.CreateText(captionFileName))
                                {
                                    swc.Write(caption);
                                }

                                sw.WriteLine("magick.exe \"{0}\" -resize {4}x{5} -gravity Center -extent 448x600 -fill {3} -background rgba(255,255,255,0.00) -font Times-New-Roman -pointsize 12 -size 448x28 -gravity South caption:@{6} -composite -rotate \"90\" -gamma 0.454545 -background White -dither FloydSteinberg -remap \".\\acep_palette.gif\" \"..\\dithered\\{1}\"", fileName, outFileName, caption, colour, bmwidth, bmheight, captionFileName);
                            }
                            else
                            {
                                sw.WriteLine("magick.exe \"{0}\" -rotate \"90\" -gamma 0.454545 -resize 600x448 -background White -gravity center -extent 600x448 -dither FloydSteinberg -remap \".\\acep_palette.gif\" \"..\\dithered\\{1}\"", fileName, outFileName);
                            }
                        }

                        sw.WriteLine("rem Make ACeP");
                        sw.WriteLine("mkdir ..\\ACeP");
                        sw.WriteLine("del ..\\ACeP\\*.* /Q");
                        sw.WriteLine("copy .\\ACepView.exe ..\\ACeP");
                        //sw.WriteLine("copy .\\acep_palette.txt ..\\ACeP");
                        foreach (string fileName in fileArray)
                        {
                            string inFileName = fileName.Replace("..\\source\\", "");
                            inFileName = inFileName.Substring(0, inFileName.LastIndexOf("."));
                            string ACePfilename = inFileName + ".ACeP";
                            inFileName += ".png";
                            sw.WriteLine(".\\makeACeP.exe \"..\\dithered\\{0}\" \"..\\ACeP\\{1}\"", inFileName, ACePfilename);
                        }
                        sw.WriteLine("del ..\\dithered\\*.caption.txt");
                    }
                }
            }

            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

        }

        private static int MeasureStringWidth(string s, Font font)
        {
            SizeF result;
            using (var image = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(image))
                {
                    result = g.MeasureString(s, font);
                }
            }

            return (int)result.Width;
        }

        static string GetCaption(string filename, string calendar, out string colour)
        {
            if (calendar == "")
            {
                colour = "";
                return "";
            }

            colour = "black";

            filename = filename.Substring(0, filename.LastIndexOf("."));

            string caption = "";

            try {
                string[] filenameparts = filename.Split('-');
                int day = Int32.Parse(filenameparts[0]);
                int month = Int32.Parse(filenameparts[1]);
                int subpart = 0;
                if (filenameparts.Count() == 3)
                {
                    subpart = Int32.Parse(filenameparts[2]) - 1;
                }

                string[] calendarmonthsstrings = calendar.Split('=');
                string[] thismonth = calendarmonthsstrings[month].Trim().Split('\n');

                int i = 1;
                int currday = 1;
                bool bFound = false;
                bool bGotCandidate = false;

                while (i < thismonth.Count() && !bFound)
                {
                    string[] daystring = thismonth[i].Split(':');
                    string currdaystring = daystring[0].Substring(0, daystring[0].IndexOf(' '));   // get the first number, which is the day of the month
                    currday = Int32.Parse(currdaystring);

                    if (currday > day)
                    {
                        //if (bGotCandidate)
                        //{
                        //    bFound = true;  // idea is that if there is no entry for a subpart, the entry for the last subpart will
                        //}                   // be used, so that more than one picture can be used for the same saint's feastday or commemoration
                        break;
                    }

                    if (day == currday) {
                        caption = daystring[1].Trim();
                        if (daystring[0].IndexOf('R') != -1)
                        {
                            colour = "red";
                        }
                        bGotCandidate = true;

                        if (subpart > 0)
                        {
                            i++;
                            subpart--;
                            continue;
                        }
                        else
                        {
                            if (bGotCandidate)
                            {
                                bFound = true;
                            }
                        }
                    }
                    i++;
                }
            }

            catch (Exception Ex)
            {
               Console.WriteLine(Ex.ToString());
               return "";
            }

            return caption;
        }
    }
}
