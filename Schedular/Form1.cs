using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace Schedular
{
   
    public partial class Form1 : MetroForm
    {
        List<Process> ProcessList = new List<Process>();
        public Form1()
        {
            InitializeComponent();
            metroComboBox1.Items.Add("First Come First Serve");
            metroComboBox1.Items.Add("Preemptive SJF");
            metroComboBox1.Items.Add("Non-preemptive SJF");
            metroComboBox1.Items.Add("Preemptive priority");
            metroComboBox1.Items.Add("Non-preemptive priority");
            metroComboBox1.Items.Add("Round Robin");
        }

        private void Form1_Load(object sender, EventArgs e)
        {}

        private void metroButton1_Click_1(object sender, EventArgs e)
        {
            if (metroComboBox1.Text == "First Come First Serve")
            {
                double totalwaitingtime = 0;
                double processwaitingtime = 0;
                double totalexecutiontime = 0;
                double averagewaitingtime = 0;
                metroButton1.Visible = false;
                ProcessList.Sort(FCFS_Sort);
                for (int i = (ProcessList.Count - 1); i >= 0; i-- )
                {
                    if (i == (ProcessList.Count - 1) && ProcessList[ProcessList.Count - 1].arrivaltime == 0)
                    {
                        totalexecutiontime = ProcessList[ProcessList.Count - 1].bursttime;
                        totalwaitingtime = 0;
                        processwaitingtime = 0;
                    }
                    else if (i == (ProcessList.Count - 1) && ProcessList[ProcessList.Count - 1].arrivaltime != 0)
                    {
                        totalexecutiontime = ProcessList[ProcessList.Count - 1].arrivaltime + ProcessList[ProcessList.Count - 1].bursttime;
                        totalwaitingtime = 0;
                        processwaitingtime = 0;
                    }
                    else
                    {
                        processwaitingtime = totalexecutiontime - ProcessList[i].arrivaltime;
                        totalexecutiontime = totalexecutiontime + ProcessList[i].bursttime;
                        totalwaitingtime = totalwaitingtime + processwaitingtime;
                    }
                    averagewaitingtime = totalwaitingtime / (ProcessList.Count);
                }

                int x = 10, y = 10;
                for (int i = (ProcessList.Count - 1); i >= 0; i--)
                {
                    populateTable(x, y, ProcessList[i].bursttime * 20 ,ProcessList[i].name);
                    x = x + (ProcessList[i].bursttime * 20);
                }
                averagewaitingtimelabel.Text = averagewaitingtime.ToString();
                //populateTable(10, 10,"process");
                
            }

                //--------------------------------------------------------------------
                //--------------------------------------------------------------------

            else if (metroComboBox1.Text == "Preemptive SJF")
            {
                QuantumTxtBox.Visible = false;
                PRE_SJF(ProcessList);
                averagewaitingtimelabel.Text = PRE_SJF_averaewaitingtime.ToString();
                int x = 10, y = 10;
                for (int i = 0; i < implementationlist.Count; i++)
                {
                    populateTable(x, y, 20, implementationlist[i]);
                    x = x + 20;
                }
            }

                //--------------------------------------------------------------------
            //--------------------------------------------------------------------

            else if (metroComboBox1.Text == "Non-preemptive SJF")
            {
                metroButton1.Visible = false;
                NON_SJF(ProcessList);
                averagewaitingtimelabel.Text = PRE_SJF_averaewaitingtime.ToString();
                int x = 10, y = 10;
                for (int i = 0; i < implementationlist.Count; i++)
                {
                    populateTable(x, y, 20, implementationlist[i]);
                    x = x + 20;
                }
            }

                //--------------------------------------------------------------------
            //--------------------------------------------------------------------

            else if (metroComboBox1.Text == "Non-preemptive priority")
            {
                ProcessList.Sort(mostpriority); 
            }

                //--------------------------------------------------------------------
            //--------------------------------------------------------------------

            else if(metroComboBox1.Text == "Preemptive priority")
            {
                QuantumTxtBox.Visible = false;
                metroButton1.Visible = false;
                PRE_PRI(ProcessList);
                int x = 10, y = 10;
                for (int i = 0; i < implementationlist.Count; i++)
                {
                    populateTable(x, y, 20, implementationlist[i]);
                    x = x + 20;
                }
                averagewaitingtimelabel.Text = PRE_SJF_averaewaitingtime.ToString();
            }

                //--------------------------------------------------------------------
            //--------------------------------------------------------------------

            else 
            {
                metroButton1.Visible = false;
                RoundRobin(ProcessList);
                int x = 10, y = 10;
                for (int i = 0; i < implementationlist.Count; i++)
                {
                    populateTable(x, y, 20, implementationlist[i]);
                    x = x + 20;
                }
                averagewaitingtimelabel.Text = PRE_SJF_averaewaitingtime.ToString();
            }
        }


        private void metroTextBox1_Click(object sender, EventArgs e)
        {}

      

        private void metroLabel2_Click(object sender, EventArgs e)
        {}

        private void metroButton2_Click(object sender, EventArgs e) //Add
        {
            Process p = new Process();
            if (metroComboBox1.Text == "First Come First Serve" || metroComboBox1.Text == "Preemptive SJF" || metroComboBox1.Text == "Non-preemptive SJF" || metroComboBox1.Text == "Round Robin")
            {
                p.arrivaltime = Convert.ToInt32(metroTextBox2.Text);
                p.bursttime = Convert.ToInt32(metroTextBox3.Text);
                p.name = metroTextBox1.Text;
            }
            if (metroComboBox1.Text == "Preemptive priority" || metroComboBox1.Text == "Non-preemptive priority")
            {
                p.arrivaltime = Convert.ToInt32(metroTextBox2.Text);
                p.bursttime = Convert.ToInt32(metroTextBox3.Text);
                p.priority = Convert.ToInt32(PriorityTxtBox.Text);
                p.name = metroTextBox1.Text;
            }
            ProcessList.Add(p);
            metroTextBox1.Text = metroTextBox2.Text = metroTextBox3.Text = PriorityTxtBox.Text= "";
        }

        private void metroComboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (metroComboBox1.Text == "First Come First Serve")
            {
                metroLabel3.Visible = false;
                PriorityTxtBox.Visible = false;
                metroLabel7.Visible = false;
                QuantumTxtBox.Visible = false;
                metroTextBox3.Visible = true;
                metroLabel6.Visible = true;
                metroButton1.Text = "Show";
            }
            else if (metroComboBox1.Text == "Preemptive SJF" || metroComboBox1.Text == "Non-preemptive SJF")
            {
                metroLabel3.Visible = false;
                PriorityTxtBox.Visible = false;
                QuantumTxtBox.Visible = false;
                metroLabel7.Visible = false;
                
            }
            else if (metroComboBox1.Text == "Preemptive priority" || metroComboBox1.Text == "Non-preemptive priority")
            {
                metroLabel3.Visible = false;
                QuantumTxtBox.Visible = false;
                
                metroLabel7.Visible = true;
                PriorityTxtBox.Visible = true;
            }
            else if (metroComboBox1.Text == "Round Robin")
            {
                metroLabel7.Visible = false;
                PriorityTxtBox.Visible = false;
                metroLabel3.Text = "Enter Quantum time";
                metroLabel3.Visible = true;
                QuantumTxtBox.Visible = true;

            }
        }

        private void metroLabel5_Click(object sender, EventArgs e)
        {}

        //--------------------------------------------------------------------------------//

        public void populateTable(int x, int y, int width , string p)
        {
            using (Graphics g = this.metroPanel1.CreateGraphics())
            {
                Font font1 = new Font("Arial", 10);
                Brush b = new SolidBrush(Color.Red);
                Graphics gr = metroPanel1.CreateGraphics();
                SolidBrush sb = new SolidBrush(Color.Red);
                Pen blackPen = new Pen(Color.Black, 3);
                Rectangle tl = new Rectangle(x, y,width, 30);
                RectangleF tlf = new RectangleF(x, y, width, 30);
                gr.FillRectangle(sb, tl);
                gr.DrawRectangle(blackPen, tl);
                gr.DrawString(p, font1, Brushes.Blue, tlf);
            }

        }
        int FCFS_Sort(Process p1, Process p2)
        {
            if (p1.arrivaltime < p2.arrivaltime)
            { return 1; }
            else if (p1.arrivaltime == p2.arrivaltime)
            { return 0; }
            else { return -1; }
        }

        double PRE_SJF_averaewaitingtime = 0;
        List<string> implementationlist = new List<string>();
        void PRE_SJF(List<Process> t)
        {
            int timer = 0;
            t.Sort(FCFS_Sort);
            int totaltime = 0;
            foreach (Process u in t)
            {totaltime = totaltime + u.bursttime;}
            Process[] copy = new Process[t.Count] ;
            for (int q = 0; q < t.Count; q++ )
            {
                copy[q] = new Process(t[q].arrivaltime, t[q].bursttime, t[q].priority, t[q].name);
            }
            for (int k = 0; k < totaltime; k++)
            {
                List<Process> temp = new List<Process>();
                foreach (Process j in t)
                {
                    if (j.arrivaltime == timer)
                    { temp.Add(j); }
                }
                temp.Sort(shortest);
                temp[temp.Count - 1].bursttime--;
                implementationlist.Add(temp[temp.Count - 1].name);
                timer++;
                foreach (Process x in temp)
                { x.arrivaltime++; }
                
                
            }
            double processwaitingtime = 0;
            double totalwaitingtime = 0;
            Dictionary<string, int> waitingtimes = new Dictionary<string, int>();
            for (int i = 0; i <= implementationlist.Count - 1; i++)
            {
                if (i == 0)
                {
                    processwaitingtime = 0;
                    totalwaitingtime = 0;
                    continue;
                }
                else
                {
                    if (implementationlist[i] == implementationlist[i - 1])
                    {
                        continue;
                    }
                    if (implementationlist[i] != implementationlist[i - 1])
                    {
                        processwaitingtime = i - Array.Find(copy , x => x.name == implementationlist[i]).arrivaltime;
                        Array.Find(copy , x => x.name == implementationlist[i]).arrivaltime = i;
                        totalwaitingtime = totalwaitingtime + processwaitingtime;
                        
                        continue;

                    }
                }
                
            }
            PRE_SJF_averaewaitingtime = totalwaitingtime / t.Count;
            
        }
        void NON_SJF(List<Process> t)
        {
            int timer = 0;
            t.Sort(FCFS_Sort);
            int totaltime = 0;
            foreach (Process u in t)
            { totaltime = totaltime + u.bursttime; }
            List<int> arrivals = new List<int>();
            Process[] copy = new Process[t.Count];
            for (int q = 0; q < t.Count; q++)
            {
                copy[q] = new Process(t[q].arrivaltime, t[q].bursttime, t[q].priority, t[q].name);
            }
            for (int k = 0; k <= t.Count; k++)
            {
                List<Process> temp = new List<Process>();
                foreach (Process j in t)
                {
                    if (j.arrivaltime <= timer)
                    { temp.Add(j); }
                }
                temp.Sort(shortest);
                timer = timer + temp[temp.Count - 1].bursttime;
                temp[temp.Count - 1].bursttime = 0;
                implementationlist.Add(temp[temp.Count - 1].name);
                arrivals.Add(temp[temp.Count - 1].arrivaltime);
                t.Remove(temp[temp.Count - 1]);
                if (t.Count == 1)
                { continue; }
                else if(t.Count == 0)
                { break; }
                else
                { k = k - 1; }
                
                foreach (Process x in temp)
                { x.arrivaltime++; }
            }
            double processwaitingtime = 0;
            double totalwaitingtime = 0;
            Dictionary<string, int> waitingtimes = new Dictionary<string, int>();
            for (int i = 0; i <= implementationlist.Count - 1; i++)
            {
                if (i == 0)
                {
                    processwaitingtime = 0;
                    totalwaitingtime = 0;
                    continue;
                }
                else
                {
                    if (implementationlist[i] == implementationlist[i - 1])
                    {
                        continue;
                    }
                    if (implementationlist[i] != implementationlist[i - 1])
                    {
                        processwaitingtime = Array.Find(copy, D => D.name == implementationlist[i - 1]).bursttime + Array.Find(copy, D => D.name == implementationlist[i - 1]).arrivaltime + processwaitingtime- Array.Find(copy, x => x.name == implementationlist[i]).arrivaltime;
                        totalwaitingtime = totalwaitingtime + processwaitingtime;

                        continue;

                    }
                }

            }
            PRE_SJF_averaewaitingtime = totalwaitingtime / copy.Length;


        }

        void PRE_PRI(List<Process> t)
        {
            int timer = 0;
            t.Sort(FCFS_Sort);
            int totaltime = 0;
            foreach (Process u in t)
            { totaltime = totaltime + u.bursttime; }
            Process[] copy = new Process[t.Count];
            for (int q = 0; q < t.Count; q++)
            {
                copy[q] = new Process(t[q].arrivaltime, t[q].bursttime, t[q].priority, t[q].name);
            }
            for (int k = 0; k < totaltime; k++)
            {
                List<Process> temp = new List<Process>();
                foreach (Process j in t)
                {
                    if (j.arrivaltime == timer)
                    { temp.Add(j); }
                }
                temp.Sort(mostpriority);
                temp[temp.Count - 1].bursttime--;
                implementationlist.Add(temp[temp.Count - 1].name);
                timer++;
                foreach (Process x in temp)
                { x.arrivaltime++; }
            }
            double processwaitingtime = 0;
            double totalwaitingtime = 0;
            Dictionary<string, int> waitingtimes = new Dictionary<string, int>();
            for (int i = 0; i <= implementationlist.Count - 1; i++)
            {
                if (i == 0)
                {
                    processwaitingtime = 0;
                    totalwaitingtime = 0;
                    continue;
                }
                else
                {
                    if (implementationlist[i] == implementationlist[i - 1])
                    {
                        Array.Find(copy, x => x.name == implementationlist[i]).arrivaltime = i + 1;
                        continue;
                    }
                    if (implementationlist[i] != implementationlist[i - 1])
                    {
                        processwaitingtime = i - Array.Find(copy, x => x.name == implementationlist[i]).arrivaltime;
                        Array.Find(copy, x => x.name == implementationlist[i]).arrivaltime = i+1;
                        totalwaitingtime = totalwaitingtime + processwaitingtime;

                        continue;

                    }
                }

            }
            PRE_SJF_averaewaitingtime = totalwaitingtime / t.Count;


        }

        void RoundRobin(List<Process> t)
        {
            int timer = 0;
            int qntm = Convert.ToInt32(QuantumTxtBox.Text);
            int totaltime = 0;
            t.Sort(FCFS_Sort);
            foreach (Process u in t)
            { totaltime = totaltime + u.bursttime; }
            Process[] copy = new Process[t.Count];
            for (int q = 0; q < t.Count; q++)
            {
                copy[q] = new Process(t[q].arrivaltime, t[q].bursttime, t[q].priority, t[q].name);
            }
            for (int k = 0; k < totaltime; k++)
            {
                List<Process> temp = new List<Process>();
                foreach (Process j in t)
                {
                    if (j.arrivaltime <= timer && (j.bursttime != 0))
                    { temp.Add(j); }
                }
                foreach (Process y in temp)
                {
                    for (int x = 0; x < qntm; x++)
                    {

                        if (y.bursttime == 0) { continue; }
                        y.bursttime--;

                        y.arrivaltime++;
                        implementationlist.Add(y.name);
                        if (timer == totaltime) break;
                        timer++;
                    }
                }
                if (timer == totaltime) break;
            }
            double processwaitingtime = 0;
            double totalwaitingtime = 0;
            Dictionary<string, int> waitingtimes = new Dictionary<string, int>();
            for (int i = 0; i <= implementationlist.Count - 1; i++)
            {
                if (i == 0)
                {
                    processwaitingtime = 0;
                    totalwaitingtime = 0;
                    continue;
                }
                else
                {
                    if (implementationlist[i] == implementationlist[i - 1])
                    {
                        Array.Find(copy, x => x.name == implementationlist[i]).arrivaltime = i+1;
                        continue;
                    }
                    if (implementationlist[i] != implementationlist[i - 1])
                    {
                        processwaitingtime = i - Array.Find(copy, x => x.name == implementationlist[i]).arrivaltime;
                        Array.Find(copy, x => x.name == implementationlist[i]).arrivaltime = i+1;
                        totalwaitingtime = totalwaitingtime + processwaitingtime;

                        continue;

                    }
                }

            }
            PRE_SJF_averaewaitingtime = totalwaitingtime / t.Count;

        }
        
        int shortest(Process p1, Process p2)
        {
            if (p1.bursttime == 0) { return -1; }
            if (p2.bursttime == 0) { return 1; }
            if (p1.bursttime == p2.bursttime)
            { return 0; }
            else if (p1.bursttime < p2.bursttime)
            { return 1; }
            else { return -1; }
        }
        int mostpriority(Process p1, Process p2)
        {
            if (p1.bursttime == 0) { return -1; }
            if (p2.bursttime == 0) { return 1; }
            if (p1.priority > p2.priority)
            { return -1; }
            else if(p1.priority<p2.priority)
            { return 1; }
            else { return 0; }
        }

        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {
            //Graphics g;  
            //g = CreateGraphics();  
            //Pen p;  
            //Rectangle r;  
            //p = new Pen(Brushes.Blue); 
            //r = new Rectangle(1, 1, 578, 38);  
            //g.DrawRectangle(p, r);
            //Graphics g = metroPanel1.CreateGraphics();
            //SolidBrush sb = new SolidBrush(Color.Red);
            //Pen blackPen = new Pen(Color.Black, 3);
            //Rectangle tl = new Rectangle(10, 10, 10, 5);
            //g.DrawRectangle(blackPen, tl);
            //g.FillRectangle(sb, tl);
        }

        private void metroLabel8_Click(object sender, EventArgs e)
        {

        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            metroPanel1.Visible = true;
        }

       
    }





























































    class Process
    {
        public int arrivaltime, priority, bursttime;
        public string name;
        public Process()
        {
            arrivaltime = 0;
            bursttime = 0;
            name = "";
            priority = 0;
        }
        public Process(int a, int b, int p, string n)
        {
            arrivaltime = a;
            bursttime = b;
            priority = p;
            name = n;
        }
        public Process(int a, int b, string n)
        {
            arrivaltime = a;
            bursttime = b;
            name = n;
            priority = 0;
        }


    }
}
