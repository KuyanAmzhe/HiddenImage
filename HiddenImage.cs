using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiddenImage
{
    static class HiddenImage
    {
        /// <summary>
        /// Hides the hiddenImage into visibleImage
        /// </summary>
        /// <param name="visibleImage"></param>
        /// <param name="hiddenImage"></param>
        public static Bitmap HideAnImage(Bitmap visibleImage, Bitmap hiddenImage)
        {
            //hiddenImage and visibleImage pixel position
            int hidX, hidY, visX = 0, visY = 0;
            for (hidY = 0; hidY < hiddenImage.Height; hidY++)
            {
                for (hidX = 0; hidX < hiddenImage.Width; hidX++)
                {
                    //Hiding bytes as a char array
                    char[][] RGBChars = ColorBytesToCharArray(hiddenImage.GetPixel(hidX, hidY));
                    HideBits(RGBChars, visibleImage, ref visX, ref visY);
                }
            }
            return visibleImage;
        }

        /// <summary>
        /// Reverses the order of string
        /// </summary>
        /// <param name="someString"></param>
        /// <returns></returns>
        private static char[] Reverse(string someString)
        {
            char[] charArray = someString.ToCharArray();
            Array.Reverse(charArray);
            return charArray;
        }

        /// <summary>
        /// Converts pixel's RGB palate to bit char array
        /// </summary>
        /// <param name="pixelColor"></param>
        /// <returns></returns>
        private static char[][] ColorBytesToCharArray(Color pixelColor)
        {
            byte[] RByte = new byte[1] { pixelColor.R };
            byte[] GByte = new byte[1] { pixelColor.G };
            byte[] BByte = new byte[1] { pixelColor.B };

            //Array with an arraies of RBG bits
            BitArray[] RGBBits = new BitArray[3]
            {
                new BitArray(RByte),
                new BitArray(GByte),
                new BitArray(BByte)
            };

            string[] cupStrings = new string[3];

            //Transfers BitArrays to string array
            for (int i = 0; i < RGBBits.Length; i++)
            {
                for (int x = 0; x < RGBBits[i].Length; x++)
                {
                    if (RGBBits[i][x]) cupStrings[i] += "1";
                    else cupStrings[i] += "0";
                }
            }

            //Array of arraies of RGB bits. Order: R, G, B
            char[][] RGBChars = new char[][]
            {
                Reverse(cupStrings[0]),
                Reverse(cupStrings[1]),
                Reverse(cupStrings[2])
            };

            return RGBChars;
        }

        //Hides bits of hidden image
        private static Bitmap HideBits(char[][] bits, Bitmap visibleImage, ref int visX, ref int visY)
        {
            //Quantity of hiding bits
            const int limit = 8;
            //Current bit iterator
            int currentBit = 0;
            for (; visY < visibleImage.Height && currentBit < limit;)
            {
                for (; visX < visibleImage.Width && currentBit < limit; currentBit++, visX ++)
                {
                    char[][] RGBChars = ColorBytesToCharArray(visibleImage.GetPixel(visX, visY));

                    //Changing lowest bits
                    RGBChars[0][7] = bits[0][currentBit];
                    RGBChars[1][7] = bits[1][currentBit];
                    RGBChars[2][7] = bits[2][currentBit];

                    //Converting char arraies to byte array
                    byte[] changedBytes = new byte[]
                    {
                        Convert.ToByte(new string(RGBChars[0]), 2),
                        Convert.ToByte(new string(RGBChars[1]), 2),
                        Convert.ToByte(new string(RGBChars[2]), 2)
                    };

                    //Sets new pixel color
                    visibleImage.SetPixel(visX, visY, Color.FromArgb(changedBytes[0], changedBytes[1], changedBytes[2]));
                }
                //If the X axis iterator reached the border, resets it and moves to another Y axis 
                if (currentBit != limit)
                {
                    visX = 0;
                    visY++;
                }
            }
            return visibleImage;
        }
        
        /// <summary>
        /// Finds a hidden image in visible one
        /// </summary>
        /// <param name="visibleImage"></param>
        /// <returns>Hidden image</returns>
        public static Bitmap DecodeAnImage(Bitmap visibleImage)
        {
            //Width and height of the hidden image
            int widthAndHeight = (int)Math.Sqrt(Math.Pow(visibleImage.Width, 2.0) / 8);

            Bitmap discoveredImage = new Bitmap(widthAndHeight, widthAndHeight);
            //Discovered image and visible image axises
            int descX, descY, visX = 0, visY = 0;

            for (descY = 0; descY < discoveredImage.Height; descY++)
            {
                for (descX = 0; descX < discoveredImage.Width; descX++)
                {
                    Color foundPixelColor = FindHiddenPixelColor(visibleImage, ref visX, ref visY);

                    //Sets new color to pixel
                    discoveredImage.SetPixel(descX, descY, foundPixelColor);
                }
            }

            return discoveredImage;
        }

        /// <summary>
        /// Returns hidden pixel's color
        /// </summary>
        /// <param name="visibleImage"></param>
        /// <param name="visX"></param>
        /// <param name="visY"></param>
        /// <returns></returns>
        private static Color FindHiddenPixelColor(Bitmap visibleImage, ref int visX, ref int visY)
        {
            //Quantity of hiding bits
            const int limit = 8;
            //Current bit iterator
            int currentPixel = 0;

            //Discovered pixel color
            char[][] hiddenPixelColor =
            {
                new char[8],
                new char[8],
                new char[8]
            };

            for (; visY < visibleImage.Height && currentPixel < limit;)
            {
                for (; visX < visibleImage.Width && currentPixel < limit; currentPixel++, visX++)
                {
                    //Color of the visible pixel as a char arraies
                    char[][] RGBChars = ColorBytesToCharArray(visibleImage.GetPixel(visX, visY));

                    //Filling up the arrays
                    hiddenPixelColor[0][currentPixel] = RGBChars[0][7];
                    hiddenPixelColor[1][currentPixel] = RGBChars[1][7];
                    hiddenPixelColor[2][currentPixel] = RGBChars[2][7];
                }
                //If the X axis iterator reached the border, resets it and moves to another Y axis 
                if (currentPixel != limit)
                {
                    visX = 0;
                    visY++;
                }
            }

            byte[] RGBBytes = new byte[]
            {
                Convert.ToByte(new string(hiddenPixelColor[0]), 2),
                Convert.ToByte(new string(hiddenPixelColor[1]), 2),
                Convert.ToByte(new string(hiddenPixelColor[2]), 2)
            };

            return Color.FromArgb(RGBBytes[0], RGBBytes[1], RGBBytes[2]);
        }
    }
}
