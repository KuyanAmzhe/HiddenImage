using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace HiddenImage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog imageDialog = new OpenFileDialog())
            using (SaveFileDialog discoveredImageDialog = new SaveFileDialog())
            {
                imageDialog.Title = "Choose the image you want to decode";
                imageDialog.Filter = "Only PNG files (*.png)|*.png";

                discoveredImageDialog.Title = "Choose where you wanna store new image";
                discoveredImageDialog.Filter = "Only PNG files (*.png)|*.png";

                Bitmap discoveredImage = null;

                if (imageDialog.ShowDialog() == DialogResult.OK)
                    discoveredImage = HiddenImage.DecodeAnImage(new Bitmap(imageDialog.FileName));
                else return;

                if (discoveredImageDialog.ShowDialog() == DialogResult.OK)
                {
                    discoveredImage.Save(discoveredImageDialog.FileName);
                    MessageBox.Show("New image has been saved");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog visibleImageDialog = new OpenFileDialog())
            using (OpenFileDialog hiddenImageDialog = new OpenFileDialog())
            using (SaveFileDialog newImageDialog = new SaveFileDialog())
            {
                visibleImageDialog.Title = "Choose the image you want to hide into";
                visibleImageDialog.Filter = "Only PNG files (*.png)|*.png";

                hiddenImageDialog.Title = "Choose the image you want to hide";
                hiddenImageDialog.Filter = "Only PNG files (*.png)|*.png";

                newImageDialog.Title = "Choose where you wanna store new image";
                newImageDialog.Filter = "Only PNG files (*.png)|*.png";

                Bitmap finalImage = null;

                //Need a check for images. Are images's size okay?
                if (visibleImageDialog.ShowDialog() == DialogResult.OK &&
                    hiddenImageDialog.ShowDialog() == DialogResult.OK)
                    finalImage = HiddenImage.HideAnImage(new Bitmap(visibleImageDialog.FileName), new Bitmap(hiddenImageDialog.FileName));
                else return;

                if (newImageDialog.ShowDialog() == DialogResult.OK)
                {
                    finalImage.Save(newImageDialog.FileName);
                    MessageBox.Show("New image has been saved");
                }
            }
        }
    }
}
