using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Barcode_Reader
{
    public partial class Form1 : Form
    {
        #region Variables
        Thread thread;
        TcpListener listener;
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void startServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            while (true)
            {
                if (!listener.Pending())
                {
                    Thread.Sleep(500);
                    continue;
                }
                using (TcpClient client = listener.AcceptTcpClient())
                {
                    using (StreamReader reader = new StreamReader(client.GetStream()))
                    {
                        string line = reader.ReadLine();
                        if (line == null) continue;
                        foreach (char c in line.ToCharArray())
                        {
                            SendKeys.SendWait(c.ToString());
                        }
                        SendKeys.SendWait("{ENTER}");
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            notifyIcon1.ShowBalloonTip(3000);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void Exit()
        {
            try
            {
                if (thread.IsAlive)
                    thread.Abort();
            }
            catch
            {

            }
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value == 0)
            {
                MessageBox.Show("You must Choose a port number", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            numericUpDown1.Enabled = false;
            thread = new Thread(() => { startServer(Convert.ToInt32(numericUpDown1.Value)); });
            thread.IsBackground = true;
            thread.Start();
            button1.Enabled = false;
            button4.Enabled = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void openUIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = true;
            try
            {
                listener.Stop();
                thread.Abort();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            button4.Enabled = false;
            button1.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("By Hassan Alzubair", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}