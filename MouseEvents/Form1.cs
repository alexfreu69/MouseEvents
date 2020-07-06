using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MouseEvents
{
    public partial class Form1 : Form
    {
        HiResTimer timer;
        Int64 lastval = 0;
        int iDoubleCount = 0;
        int iMissCount = 0;
        Int64 lastDelta = 0;

        public Form1()
        {
            InitializeComponent();
            MouseWheel += new System.Windows.Forms.MouseEventHandler(Form1_MouseWheel);
        }

        private void JumpDown()
        {

            //listBox_Messages->TopIndex =
            //    listBox_Messages->Items->Add(message);
            listBox1.TopIndex = listBox1.Items.Count - 1;

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            listBox1.Items.Add("MOUSEDOWN " + getButton(e.Button));
            JumpDown();
        }

        private string getButton(MouseButtons e)
        {
            switch (e)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    return "LEFT";
                case System.Windows.Forms.MouseButtons.Right:
                    return "RIGHT";
                case System.Windows.Forms.MouseButtons.Middle:
                    return "MIDDLE";
                case System.Windows.Forms.MouseButtons.XButton1:
                    return "XBUTTON1";
                case System.Windows.Forms.MouseButtons.XButton2:
                    return "XBUTTON2";
                default:
                    return ((int)e).ToString("X8");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            listBox1.Items.Add("MOUSECLICK " + getButton(e.Button)+ " " + GetDelta());
            if (lastDelta > 1000)
            {
                iMissCount++;
                lblMissCount.Text = iMissCount.ToString();
            }
            JumpDown();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            listBox1.Items.Add("MOUSEUP " + getButton(e.Button));
            JumpDown();
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            listBox1.Items.Add("MOUSEDOUBLECLICK " + getButton(e.Button) + " " + GetDelta());
            if (lastDelta<100)
            {
                iDoubleCount++;
                lblDblCount.Text = iDoubleCount.ToString();
            }

            JumpDown();

        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            listBox1.Items.Add("MOUSEWHEEL " + e.Delta);
            JumpDown();
        }

        private void Form1_Scroll(object sender, ScrollEventArgs e)
        {
            listBox1.Items.Add("Scrollbox");
            JumpDown();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Focus();
            timer = new HiResTimer();
        }

        private string GetDelta()
        {
            if (lastval == 0) {
                lastval = timer.Value;
                return "0";
            }
            Int64 delta = ((timer.Value - lastval) * 1000) / timer.Frequency;
            lastDelta = delta;
            lastval = timer.Value;
            return delta.ToString()+ " ms";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            picLED.Visible = !picLED.Visible;
        }
    }

    public class HiResTimer
    {
        private bool isPerfCounterSupported = false;
        private Int64 frequency = 0;

        // Windows CE native library with QueryPerformanceCounter().
        private const string lib = "kernel32.dll";
        [DllImport(lib)]
        private static extern int QueryPerformanceCounter(ref Int64 count);
        [DllImport(lib)]
        private static extern int QueryPerformanceFrequency(ref Int64 frequency);

        public HiResTimer()
        {
            // Query the high-resolution timer only if it is supported.
            // A returned frequency of 1000 typically indicates that it is not
            // supported and is emulated by the OS using the same value that is
            // returned by Environment.TickCount.
            // A return value of 0 indicates that the performance counter is
            // not supported.
            int returnVal = QueryPerformanceFrequency(ref frequency);

            if (returnVal != 0 && frequency != 1000)
            {
                // The performance counter is supported.
                isPerfCounterSupported = true;
            }
            else
            {
                // The performance counter is not supported. Use
                // Environment.TickCount instead.
                frequency = 1000;
            }
        }

        public Int64 Frequency
        {
            get
            {
                return frequency;
            }
        }

        public Int64 Value
        {
            get
            {
                Int64 tickCount = 0;

                if (isPerfCounterSupported)
                {
                    // Get the value here if the counter is supported.
                    QueryPerformanceCounter(ref tickCount);
                    return tickCount;
                }
                else
                {
                    // Otherwise, use Environment.TickCount.
                    return (Int64)Environment.TickCount;
                }
            }
        }
 /*       
        static void Main()
        {
            HiResTimer timer = new HiResTimer();

            // This example shows how to use the high-resolution counter to 
            // time an operation. 

            // Get counter value before the operation starts.
            Int64 counterAtStart = timer.Value;

            // Perform an operation that takes a measureable amount of time.
            for (int count = 0; count < 10000; count++)
            {
                count++;
                count--;
            }

            // Get counter value when the operation ends.
            Int64 counterAtEnd = timer.Value;

            // Get time elapsed in tenths of a millisecond.
            Int64 timeElapsedInTicks = counterAtEnd - counterAtStart;
            Int64 timeElapseInTenthsOfMilliseconds =
                (timeElapsedInTicks * 10000) / timer.Frequency;

            MessageBox.Show("Time Spent in operation (tenths of ms) "
                           + timeElapseInTenthsOfMilliseconds +
                           "\nCounter Value At Start: " + counterAtStart +
                           "\nCounter Value At End : " + counterAtEnd +
                           "\nCounter Frequency : " + timer.Frequency);
        }
    */
    }
}
