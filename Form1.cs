using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serialport
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Serial port listening event
            SerialPortManager.OnMessageReceived += Spm_OnMessageReceived;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            // When button1 clicked, start operation and open serial port
            SerialPortManager.ReturnValue = string.Empty;
            SerialPortManager.IsFirstSending = true;
            SerialPortManager.Open();
        }

        private void Spm_OnMessageReceived(string message, bool isCompleted, bool isSuccess)
        {
            richTextBox1.AppendText(message);
            if (isCompleted)
            {
                SerialPortManager.Close();
                if (isSuccess)
                {
                    // Do somethings here
                    MessageBox.Show("Operation succeeded!");
                }
                else
                {
                    // Do somethings here
                    MessageBox.Show("Operation failed");
                }
                // Finish process here or restart
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SerialPortManager.Close();
        }
    }
}
