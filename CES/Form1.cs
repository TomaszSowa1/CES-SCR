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
using System.Windows.Forms.DataVisualization.Charting;


namespace CES
{
    public partial class Form1 : Form
    {
        private DataTable dt;
        private BindingSource bs;
        //GanttChart ganttChart1;
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
            //List<int> p = new List<int>();
            //List<int> d = new List<int>();
            //List<int> t = new List<int>();
            List<task> tasks = new List<task>();
            double sum = 0;
            for (int i = 0; i < results.Rows.Count; i++)
            {
                task mytask = new task { name = results.Rows[i][0].ToString(), p = Int32.Parse(results.Rows[i][2].ToString()), d = Int32.Parse(results.Rows[i][3].ToString()), t = Int32.Parse(results.Rows[i][1].ToString()) };
                tasks.Add(mytask);
                sum += (double)Int32.Parse(results.Rows[i][2].ToString()) / (double)Int32.Parse(results.Rows[i][1].ToString());
            }
            //get frame size
            bool afterslicing=false;
            Afterslicing:
            int H = LCM(tasks.Select(m=>m.t).ToList());
            int f = 0;
            for (int i=H; i > 0; i--) {
                if (i >= tasks.Select(m=>m.p).Max() && H % i == 0 && i<=tasks.Select(m=>m.t).Min()) {
                    f = i;
                    break;
                }
            }
            //how many frames
            if (f > 0|| afterslicing)
            {
                int f_count = H / f;
                MessageBox.Show("H:" + H + "\nf:" + f + "\nf_count" + f_count + "\n");
                //chart1.ChartAreas[0].AxisY.IsStartedFromZero = false;
                chart1.Legends.Clear();
                chart1.Legends.Add("Timespans");
                chart1.Legends[0].LegendStyle = LegendStyle.Table;
                chart1.Legends[0].Docking = Docking.Bottom;
                chart1.Legends[0].Alignment = StringAlignment.Center;
                chart1.Legends[0].Title = "Timespans";
                chart1.Legends[0].BorderColor = Color.Black;
                //chart1.ChartAreas[0].AxisY.ScaleView.Size = 25;
                chart1.ChartAreas[0].AxisX.LabelStyle.Format ="";
                chart1.ChartAreas[0].AxisX.Crossing = 25;
                chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.White;
                chart1.ChartAreas[0].BackColor = Color.White;
                chart1.ChartAreas[0].AxisX.Interval = 1;
                chart1.ChartAreas[0].AxisX.MinorGrid.LineColor = Color.White;
                
                chart1.Series.Clear();
                ///
                chart1.ChartAreas[0].AxisY.Minimum = 0;
                chart1.ChartAreas[0].AxisY.Maximum = H;
                //chart1.ChartAreas[0].AxisY.Interval= (int)(H / f_count);
                //chart1.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Number;
                //chart1.ChartAreas[0].AxisY.MajorGrid.Interval = (int)(H / f_count);
                //chart1.ChartAreas[0].AxisY.MajorGrid.IntervalType = DateTimeIntervalType.Number;
                //chart1.ChartAreas[0].AxisY.MinorGrid.Interval = (int)(H / f_count);
                //chart1.ChartAreas[0].AxisY.MinorGrid.IntervalType = DateTimeIntervalType.Number;
                //chart1.ChartAreas[0].AxisY.LabelStyle.Interval = (int)(H / f_count);
                //chart1.ChartAreas[0].AxisY.LabelStyle.IntervalType = DateTimeIntervalType.Number;
                ////chart1.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Milliseconds;
                //chart1.ChartAreas[0].RecalculateAxesScale();
                chart1.ChartAreas[0].AxisX.Interval = 1;
                string seriesname;
                List<int> timeline = new List<int>();
                List<int> timeline2 = new List<int>();
                for (int i= 0; i<f_count; i++){
                    timeline.Add(i*H/f_count);
                    timeline2.Add(i * H / f_count);
                }
                
                foreach (task mytask in tasks) {
                    int count = H/mytask.d;
                    seriesname = Convert.ToString(mytask.name);
                    //chart1.Series[seriesname].
                    //chart1.ChartAreas[0].AxisY.Interval = 25;
                    chart1.Series.Add(seriesname);
                    chart1.Series[seriesname].ChartType = SeriesChartType.RangeBar;
                    chart1.Series[seriesname].YValuesPerPoint = 2;
                    chart1.Series[seriesname]["DrawSideBySide"] = "false";
                    chart1.Series[seriesname].ToolTip = seriesname;
                    chart1.Series[seriesname].YValueType = ChartValueType.Int32;
                    chart1.Series[seriesname].YAxisType = AxisType.Secondary;
                    bool mystop=true;
                    int newmin=0, newmax=(f_count/count)-1;
                    List<double> xValues = new List<double>();
                    List<double> yValues = new List<double>();
                    for (int i = 0; i < f_count; i++)
                    {
                        int newi = 0;
                        if (!mystop&&i<=newmax&&i>=newmin) {
                            mystop = true;
                            newi++;
                        }
                        if (mystop)
                        {
                            if (timeline[i] + mytask.p <= timeline2[i]+ (H / f_count))
                            {
                                //chart
                                chart1.Series[seriesname].Points.AddXY("timeline", timeline[i], timeline[i] + mytask.p);
                                chart1.Series[seriesname].Points[chart1.Series[seriesname].Points.Count-1].XValue = 1;
                                //chart1.Series[seriesname].YAxisType = AxisType.Secondary;
                                timeline[i] = timeline[i] + mytask.p;
                                mystop = false;
                                newmax = newmax+ (f_count / count);
                                newmin = newmin + (f_count / count);
                            }
                            else
                            {
                                //next
                            }
                        }
                    }

                }

            }
            else {
                //tasks needs to be sliced
                for (int j = 0; j < tasks.Count; j++)
                {
                    foreach (task mytask in tasks) {
                        if (mytask.t > tasks.Max(m => m.p)) { 

                        }
                    }
                }
                afterslicing = true;
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
