using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YazLab_III
{

    public partial class Form1 : Form
    {

        public static int type;
        public static int width;
        public static int height;
        public int frame_number = 0;
        public int max_frame = 9999;
        public Boolean islem = false;
        Bitmap global_bmp = new Bitmap(1920, 1080);
        byte[] source;
        private bool mouseDown;
        private Point lastLocation;
        public Boolean iceri_aktarildi = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (iceri_aktarildi == false)
                return;

            islem = true;
            if (!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form1.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void progressBar2_MouseClick(object sender, MouseEventArgs e)
        {

            decimal pos = 0M;
            pos = ((decimal)e.X / (decimal)progressBar2.Width) * progressBar2.Maximum;
            pos = Convert.ToInt32(pos);
            Console.WriteLine(pos);
            if (pos >= progressBar2.Minimum && pos <= progressBar2.Maximum)
            {
                progressBar2.Value = (int)pos;
                frame_number = (int)pos;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            islem = false;
            backgroundWorker1.CancelAsync();
            frame_number = 0;
            progressBar2.Value = 0;
            OpenFileDialog dosya_oku = new OpenFileDialog();
            dosya_oku.Filter = "Yuv Dosyası | *.yuv";
            dosya_oku.Title = "Yuv Dosyası Seçiniz";
            dosya_oku.ShowDialog();
            if (!dosya_oku.FileName.Contains("yuv"))
            {
                MessageBox.Show("Yuv Dosyası Seçiniz");
                return;
            }
            
            source = File.ReadAllBytes(dosya_oku.FileName);
            Form2 _form2 = new Form2();
            _form2.ShowDialog();
            byte[] dataY = null;
            if (type == 0)
            {
                dataY = source.Skip(width * height * 0 * 3).Take(width * height * 3).ToArray();
            }
            if (type == 1)
            {
                dataY = source.Skip(width * height * 0 * 2).Take(width * height * 2).ToArray();
            }
            if (type == 2)
            {
                dataY = source.Skip(width * height * 0 * 3 / 2).Take(width * height * 3 / 2).ToArray();
            }
            // goruntu.Size = new Size(width, height);
            Bitmap bbmp = new Bitmap(width, height);
            int sayac = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    bbmp.SetPixel(j, i, Color.FromArgb(dataY[sayac], dataY[sayac], dataY[sayac]));
                    sayac++;
                }
            }

            goruntu.Image = bbmp;

            int frame = 0;
            if (type == 0)
            {
                frame = source.Length / (width * height * 3);
            }
            if (type == 1)
            {
                frame = source.Length / (width * height * 2);
            }
            if (type == 2)
            {
                frame = source.Length / (width * height * 3 / 2);
            }

            Console.WriteLine("Maximum: " + frame);
            progressBar2.Maximum = frame;
            iceri_aktarildi = true;

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form2 _form2 = new Form2();
            _form2.ShowDialog();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (frame_number >= max_frame - 1)
            {
                frame_number = 0;
            }

            while (islem)
            {
                progressBar2.Value = frame_number;
                byte[] dataY = null;
                int frame = 0;
                if (type == 0)
                {
                    frame = source.Length / (width * height * 3);
                    max_frame = frame;
                    dataY = source.Skip(width * height * frame_number * 3).Take(width * height * 3).ToArray();
                }
                if (type == 1)
                {
                    frame = source.Length / (width * height * 2);
                    max_frame = frame;
                    dataY = source.Skip(width * height * frame_number * 2).Take(width * height * 2).ToArray();
                }
                if (type == 2)
                {
                    frame = source.Length / (width * height * 3 / 2);
                    max_frame = frame;
                    dataY = source.Skip(width * height * frame_number * 3 / 2).Take(width * height * 3 / 2).ToArray();
                }



                Bitmap bbmp = new Bitmap(width, height);
                int sayac = 0;
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        bbmp.SetPixel(j, i, Color.FromArgb(dataY[sayac], dataY[sayac], dataY[sayac]));
                        sayac++;
                    }
                }
                global_bmp = bbmp;
                goruntu.Image = bbmp;
                int milliseconds = 20;
                frame_number++;
                if (frame_number >= frame - 1)
                {
                    islem = false;
                    backgroundWorker1.CancelAsync();
                }
                Thread.Sleep(milliseconds);
            }


        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            islem = false;
            backgroundWorker1.CancelAsync();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

                /*
             islem = false;
             Bitmap temp_bmp = new Bitmap(width , height);
             SaveFileDialog save = new SaveFileDialog();
             save.Title = "Dosyayı Kaydet";
             temp_bmp = global_bmp;
             if (save.ShowDialog () == DialogResult.OK )
             {
                 temp_bmp.Save(save.FileName + ".bmp");
                 MessageBox.Show("Başarıyla Kaydedildi");
                 islem = true;
             }
             */



            FolderBrowserDialog save = new FolderBrowserDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {


                byte[] dataY = null;
                int frame = 0;

                if (source == null)
                {
                    MessageBox.Show("Veri İçeri Aktarılmamış, Aktarıp Tekrar Deneyin");
                    return;
                }

                if (type == 0)
                {
                    frame = source.Length / (width * height * 3);
                }
                if (type == 1)
                {
                    frame = source.Length / (width * height * 2);
                }
                if (type == 2)
                {
                    frame = source.Length / (width * height * 3 / 2);
                }


                for (int i = 0; i < frame; i++)
                {


                    if (type == 0)
                    {
                        dataY = source.Skip(width * height * i * 3).Take(width * height * 3).ToArray();
                    }
                    if (type == 1)
                    {
                        dataY = source.Skip(width * height * i * 2).Take(width * height * 2).ToArray();
                    }
                    if (type == 2)
                    {
                        dataY = source.Skip(width * height * i * 3 / 2).Take(width * height * 3 / 2).ToArray();
                    }



                    Bitmap bbmp = new Bitmap(width, height);
                    int sayac = 0;
                    for (int k = 0; k < height; k++)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            bbmp.SetPixel(j, k, Color.FromArgb(dataY[sayac], dataY[sayac], dataY[sayac]));
                            sayac++;
                        }
                    }

                    bbmp.Save(save.SelectedPath + "/" + i + ".bmp");

                }

            }


        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 1)
            {
                MessageBox.Show("Sadece Bir Dosya Sürükleyebilirsiniz.");
                return;
            }
            if (!files[0].Contains(".yuv"))
            {
                MessageBox.Show("Sadece .yuv Dosyası Sürükleyebilirsiniz.");
                return;
            }
            islem = false;
            backgroundWorker1.CancelAsync();
            frame_number = 0;
            progressBar2.Value = 0;
            source = File.ReadAllBytes(files[0]);
            Form2 _form2 = new Form2();
            _form2.ShowDialog();
            byte[] dataY = null;
            if (type == 0)
            {
                dataY = source.Skip(width * height * 0 * 3).Take(width * height * 3).ToArray();
            }
            if (type == 1)
            {
                dataY = source.Skip(width * height * 0 * 2).Take(width * height * 2).ToArray();
            }
            if (type == 2)
            {
                dataY = source.Skip(width * height * 0 * 3 / 2).Take(width * height * 3 / 2).ToArray();
            }
            // goruntu.Size = new Size(width, height);
            Bitmap bbmp = new Bitmap(width, height);
            int sayac = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    bbmp.SetPixel(j, i, Color.FromArgb(dataY[sayac], dataY[sayac], dataY[sayac]));
                    sayac++;
                }
            }

            goruntu.Image = bbmp;

            int frame = 0;
            if (type == 0)
            {
                frame = source.Length / (width * height * 3);
            }
            if (type == 1)
            {
                frame = source.Length / (width * height * 2);
            }
            if (type == 2)
            {
                frame = source.Length / (width * height * 3 / 2);
            }

            progressBar2.Maximum = frame;
            iceri_aktarildi = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                if (frame_number +5 < max_frame)
                {
                    frame_number += 5;
                }
            }
            if (e.KeyCode == Keys.Left)
            {
                if (frame_number - 5 > 0)
                {
                    frame_number -= 5;
                }
            }

            if (e.KeyCode == Keys.Space)
            {
                if (islem == true )
                {
                    islem = false;
                    backgroundWorker1.CancelAsync();
                }
                else
                {
                    islem = true ;
                    backgroundWorker1.RunWorkerAsync ();
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
       
        }
        int a = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void goruntu_Click(object sender, EventArgs e)
        {

        }
    }
}
