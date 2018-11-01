using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace SRV12
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }
        private static Mutex mutex = new Mutex();
        private Queue<String> list = new Queue<string>();
        private List<Thread> pool = new List<Thread>();

        private void button1_Click(object sender, EventArgs e)
        {
            endthread();
            for (int i = 0; i < Convert.ToInt32(numericUpDown1.Text); i++)
            {
                pool.Add(new Thread(gen));
                pool[i].IsBackground = true;
                pool[i].Start();
            }
        }

        void endthread()
        {
            for (int i = 0; i < pool.Count; i++) pool[i].Abort();
            pool.Clear();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            list.Enqueue(e.KeyCode.ToString());
        }

        void gen()
        {
            while (true)
            {
                Thread.Sleep(3000);
                mutex.WaitOne();
                    this.Invoke((MethodInvoker) (() =>
                    {
                        try
                        {
                            textBox1.Text += list.Dequeue() + Environment.NewLine;
                        }
                        catch { }
                    }));
                mutex.ReleaseMutex();
            }
        }
    }
}
