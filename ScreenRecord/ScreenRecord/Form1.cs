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

namespace ScreenRecord
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;//双缓存处理
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<KeyValuePair<object, Screen>> kvalue = new List<KeyValuePair<object, Screen>>();
            foreach (var i in ScreenModel.GetAllScreen())
            {
                kvalue.Add(new KeyValuePair<object, Screen>(i.DeviceName, i));
            }
            comboBox1.DataSource = kvalue;
            comboBox1.DisplayMember = "Key";
            comboBox1.ValueMember = "Value";

            this.textBox1.Text= this.textBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            this.timer = new Timer();
            this.timer.Interval = 1000;
            this.timer.Tick += Timer_Tick;
        }


        /// <summary>
        /// 计时
        /// </summary>
        private Timer timer;
        private int seconds = 0;
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.isRecord)
            {
                var ts = new TimeSpan(0, 0, ++seconds);
                this.label1.Text = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }
        }

        Timer time = null;
        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Enabled = false;
            if (time != null)
            {
                time.Stop();
            }
            time = new Timer();
            time.Interval = 100;
            time.Tick += Time_Tick;
            time.Start();
            panel1.Hide();
        }

        private void Time_Tick(object sender, EventArgs e)
        {
            Graphics gs = this.CreateGraphics();
            this.OnPaint(new PaintEventArgs(gs, new Rectangle()));
            Image result = ScreenModel.GetScreenBitmap((Screen)comboBox1.SelectedValue);
            if (isRecord)
            {
                if (AccordRecord)
                    AccordModel.AddBmpInAvi((Image)result.Clone());
                if (AForgeRecord)
                    AForgeModel.AddBmpInAvi((Image)result.Clone());
            }
            Image img = ScreenModel.GetPicThumbnail(result, 50, this.panel1.Height, this.panel1.Width);
            gs.DrawImage(img, panel1.Location);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (time != null)
            {
                time.Stop();
            }
            Graphics gs = this.CreateGraphics();
            this.OnPaint(new PaintEventArgs(gs, new Rectangle()));
            gs.Clear(SystemColors.Control);
            panel1.Show();
            comboBox1.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            if (Directory.Exists(folderBrowserDialog1.SelectedPath))
            {
                this.textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
            else
            {
                MessageBox.Show("路径无效");
            }
        }

        private List<Bitmap> bmpls = new List<Bitmap>();

        private bool isRecord = false;
        private bool AccordRecord = false;
        private void button4_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(this.textBox1.Text))
            {
                this.textBox1.Enabled = false;
                this.button3.Enabled = false;
                bmpls.Clear();
                Screen screen = (Screen)comboBox1.SelectedValue;
                AccordModel.WriteAvi(this.textBox1.Text + @"\" + DateTime.Now.Ticks + ".avi", screen.WorkingArea.Width, screen.WorkingArea.Height);
                isRecord = true;
                AccordRecord = true;
                timer.Start();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (button5.Text == "暂停")
            {
                isRecord = false;
                button5.Text = "继续";
                timer.Stop();
            }
            else
            {
                isRecord = true;
                button5.Text = "暂停";
                timer.Start();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            isRecord = false;
            AccordRecord = false;
            button5.Text = "暂停";
            this.textBox1.Enabled = true;
            this.button3.Enabled = true;
            timer.Stop();
            this.label1.Text = "00:00:00";
            AccordModel.OverAvi();
        }

        private bool AForgeRecord = false;
        private void button7_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(this.textBox1.Text))
            {
                this.textBox1.Enabled = false;
                this.button3.Enabled = false;
                bmpls.Clear();
                Screen screen = (Screen)comboBox1.SelectedValue;
                AForgeModel.WriteAvi(this.textBox1.Text + @"\" + DateTime.Now.Ticks + ".mp4", screen.WorkingArea.Width, screen.WorkingArea.Height, AForge.Video.FFMPEG.VideoCodec.MPEG4);
                isRecord = true;
                AForgeRecord = true;
                timer.Start();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            isRecord = false;
            AForgeRecord = false;
            button5.Text = "暂停";
            this.textBox1.Enabled = true;
            this.button3.Enabled = true;
            timer.Stop();
            this.label1.Text = "00:00:00";
            AForgeModel.OverAvi();
        }
    }
}
