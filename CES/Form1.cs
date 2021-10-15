using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace CES
{
    public partial class Form1 : Form
    {
        private DataTable dt;
        private BindingSource bs;
        public Form1()
        {
            InitializeComponent();
            bs = new BindingSource();
            DataTable dataTable = new DataTable();
            dt = new DataTable();
            dt.Columns.Add("zadanie", typeof(String));
            dt.Columns.Add("Ti", typeof(int));
            dt.Columns.Add("Pi", typeof(int));
            dt.Columns.Add("Di", typeof(int));
            bs.DataSource = dt;
            dataGridView1.DataSource = bs;
            dataGridView1.Columns[0].ReadOnly = true;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            
            DataTable results = (DataTable)bs.DataSource;
            List<int> t = new List<int>();
            double sum = 0;
            for (int i = 0; i < results.Rows.Count; i++) {
                t.Add(Int32.Parse(results.Rows[i][1].ToString()));
                sum += (double)Int32.Parse(results.Rows[i][2].ToString()) / (double)Int32.Parse(results.Rows[i][1].ToString());
            }

            MessageBox.Show("sum:"+sum+"\nnww:"+ LCM(t));

            if (sum > 1)
            {
                MessageBox.Show("warunek sumy nie spelniony");
                return;
            }
            else {
                do_graph_thingy();
            }

        }
        void do_graph_thingy() {
            DataTable results = (DataTable)bs.DataSource;
            List<int> p = new List<int>();
            List<int> d = new List<int>();
            List<int> t = new List<int>();
            double sum = 0;
            for (int i = 0; i < results.Rows.Count; i++)
            {
                p.Add(Int32.Parse(results.Rows[i][2].ToString()));
                t.Add(Int32.Parse(results.Rows[i][1].ToString()));
                d.Add(Int32.Parse(results.Rows[i][3].ToString()));
                sum += (double)Int32.Parse(results.Rows[i][2].ToString()) / (double)Int32.Parse(results.Rows[i][1].ToString());
            }
            //get frame size
            Afterslicing:
            int H = LCM(t);
            int f = 0;
            for (int i=H; i > 0; i--) {
                if (i >= p.Max() && H % i == 0 && i<=t.Min()) {
                    f = i;
                    break;
                }
            }
            //how many frames
            if (f > 0)
            {
                int f_count = H / f;
                MessageBox.Show("H:" + H + "\nf:" + f + "\nf_count" + f_count + "\n");
                List<Tuple<int, string>> mylist = new List<Tuple<int, string>>();
                //for (int i = 0; i < f_count; i++)
                //{
                //    mylist.Add();
                //}
                for (int i = 0; i < p.Count; i++) {
                    int occur = H / d[i];
                    int rangespertask=f_count / occur;
                    for (int j = 0; j < f_count; j++) {
                    
                    }
                }







            }
            else {
                //tasks needs to be sliced
                for (int j = 0; j < p.Count; j++)
                {
                    if (p[j] > t.Min()) {
                        //task is bigger than min deadline so needs to be sliced
                        int duration = p[j];//np 10 a max 4 
                        p[j] = t.Min();
                        duration = duration - p[j];
                        int addval = duration<=t.Min()?duration:t.Min();
                        while (addval > 0) {
                            t.Add(t[j]);
                            p.Add(addval);
                            d.Add(t[j]);
                            addval = duration - addval <= t.Min() ? duration - addval : t.Min();
                            duration -= t.Min();

                        }
                    }
                }
                goto Afterslicing;
            }
        
        }

        static int LCM(List<int> numbers)
        {
            return numbers.Aggregate(lcm);
        }
        static int lcm(int a, int b)
        {
            return Math.Abs(a * b) / GCD(a, b);
        }
        static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (bs.DataSource is DataTable && e.RowIndex > 0)
            {
                dataGridView1[0, e.RowIndex - 1].Value = "z" + e.RowIndex.ToString();
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            if (dataGridView1.CurrentCell.ColumnIndex == 1|| dataGridView1.CurrentCell.ColumnIndex == 2|| dataGridView1.CurrentCell.ColumnIndex == 3) //Desired Column
            {
                System.Windows.Forms.TextBox tb = e.Control as System.Windows.Forms.TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                }
            }

        }
        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allowed numeric and one dot  ex. 10.23
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)
                 && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.'
                && (sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bs = new BindingSource();
            DataTable dataTable = new DataTable();
            dt = new DataTable();
            dt.Columns.Add("zadanie", typeof(String));
            dt.Columns.Add("Ti", typeof(int));
            dt.Columns.Add("Pi", typeof(int));
            dt.Columns.Add("Di", typeof(int));
            bs.DataSource = dt;
            dataGridView1.DataSource = bs;
            dataGridView1.Columns[0].ReadOnly = true;
        }
    }  
}
