using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinqLabs
{
    public partial class Frm考試 : Form
    {
        public Frm考試()
        {
            InitializeComponent();

            students_scores = new List<Student>()
                                         {
                                            new Student{ Name = "aaa", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Male" },
                                            new Student{ Name = "bbb", Class = "CS_102", Chi = 80, Eng = 80, Math = 100, Gender = "Male" },
                                            new Student{ Name = "ccc", Class = "CS_101", Chi = 60, Eng = 50, Math = 75, Gender = "Female" },
                                            new Student{ Name = "ddd", Class = "CS_102", Chi = 80, Eng = 70, Math = 85, Gender = "Female" },
                                            new Student{ Name = "eee", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Female" },
                                            new Student{ Name = "fff", Class = "CS_102", Chi = 80, Eng = 80, Math = 80, Gender = "Female" },

                                          };
        }

        List<Student> students_scores;

        public class Student
        {
            public string Name { get; set; }
            public string Class { get;  set; }
            public int Chi { get; set; }
            public int Eng { get; internal set; }
            public int Math { get;  set; }
            public string Gender { get; set; }
        }

        private void button36_Click(object sender, EventArgs e)
        {
            #region 搜尋 班級學生成績
           
            // 
            // 共幾個 學員成績 ?						

            // 找出 前面三個 的學員所有科目成績					
            // 找出 後面兩個 的學員所有科目成績					

            // 找出 Name 'aaa','bbb','ccc' 的學員國文英文科目成績						

            // 找出學員 'bbb' 的成績	                          

            // 找出除了 'bbb' 學員的學員的所有成績 ('bbb' 退學)	

            // 找出 'aaa', 'bbb' 'ccc' 學員 國文數學兩科 科目成績  |				
            // 數學不及格 ... 是誰 
            #endregion

        }
  
        private void button37_Click(object sender, EventArgs e)
        {
            //個人 sum, min, max, avg

            //各科 sum, min, max, avg
        }
        private void button33_Click(object sender, EventArgs e)
        {
            // split=> 分成 三群 '待加強'(60~69) '佳'(70~89) '優良'(90~100) 
            // print 每一群是哪幾個 ? (每一群 sort by 分數 descending)
        }

        private void button35_Click(object sender, EventArgs e)
        {
            // 統計 :　所有隨機分數出現的次數/比率; sort ascending or descending
            // 63     7.00%
            // 100    6.00%
            // 78     6.00%
            // 89     5.00%
            // 83     5.00%
            // 61     4.00%
            // 64     4.00%
            // 91     4.00%
            // 79     4.00%
            // 84     3.00%
            // 62     3.00%
            // 73     3.00%
            // 74     3.00%
            // 75     3.00%
        }

        //NW orders
        NorthwindEntities dbContext = new NorthwindEntities();
        private void button34_Click(object sender, EventArgs e)
        {
            // 每年 總銷售分析 圖
          
            var q = from od in this.dbContext.Order_Details
                    group od by od.Order.OrderDate.Value.Year into g
                    orderby g.Key ascending
                    select new
                    {
                        Year = g.Key,
                        TotalPrice = g.Sum(od => od.UnitPrice * od.Quantity)

                    };

            this.dataGridView1.DataSource = q.ToList();
            this.chart2.DataSource = q.ToList();

            this.chart2.Series[0].XValueMember = "Year";  //x軸
            this.chart2.Series[0].YValueMembers = "TotalPrice"; //y軸
            this.chart2.Series[0].Name = "每年銷售金額";
            this.chart2.Series[0].IsValueShownAsLabel = true;
            this.chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            // 哪一年總銷售最好?
            var q5 = (from od in this.dbContext.Order_Details.AsEnumerable()
                    group od by od.Order.OrderDate.Value.Year into g
                    orderby g.Sum(od => od.UnitPrice * od.Quantity) descending
                    select new
                    {
                        Year = g.Key,
                        TotalPrice = $"{g.Sum(od => od.UnitPrice * od.Quantity):c2}"

                    }).First();
            this.label3.Text = q5.ToString().Trim('{', '}') + "銷售最佳";

            //哪一年總銷售最不好?
            var q6= (from od in this.dbContext.Order_Details.AsEnumerable()
                      group od by od.Order.OrderDate.Value.Year into g
                      orderby g.Sum(od => od.UnitPrice * od.Quantity) descending
                      select new
                      {
                          Year = g.Key,
                          TotalPrice = $"{g.Sum(od => od.UnitPrice * od.Quantity):c2}"

                      }).Last();
            this.label4.Text = q6.ToString().Trim('{', '}') + "銷售最差";
            //============================================================================================

            // 每月 總銷售分析 圖

            var q2 = from od in this.dbContext.Order_Details
                     group od by new { od.Order.OrderDate.Value.Month } into g
                     orderby g.Key.Month ascending
                     select new
                     {
                         //Classification = g.Key,
                         Month = g.Key.Month,
                         AveragePrice = g.Average(od => od.UnitPrice * od.Quantity)

                     };

            this.dataGridView2.DataSource = q2.ToList();

            this.chart1.DataSource = q2.ToList();

            this.chart1.Series[0].Color = Color.LemonChiffon;
            this.chart1.Series[0].Name = "每月平均銷售金額";
            this.chart1.Series[0].XValueMember = "Month";  //x軸
            this.chart1.Series[0].YValueMembers = "AveragePrice"; //y軸            
            this.chart1.Series[0].IsValueShownAsLabel = true;
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            //那一個月總銷售最好 ?
            var q3 = (from od in this.dbContext.Order_Details.AsEnumerable()
                     group od by new {od.Order.OrderDate.Value.Month } into g
                     orderby  g.Average(od => od.UnitPrice * od.Quantity) descending
                     select new
                     {
                         //Classification = g.Key,
                         Month = g.Key.Month,
                         AveragePrice = $"{g.Average(od => od.UnitPrice * od.Quantity):c2}"

                     }).First();

            this.label1.Text = q3.ToString().Trim('{','}') + "銷售最佳";

            // 那一個月總銷售最不好 ?
            var q4 = (from od in this.dbContext.Order_Details.AsEnumerable()
                      group od by new { od.Order.OrderDate.Value.Month } into g
                      orderby g.Average(od => od.UnitPrice * od.Quantity) descending
                      select new
                      {
                          //Classification = g.Key,
                          Month = g.Key.Month,
                          AveragePrice = $"{g.Average(od => od.UnitPrice * od.Quantity):c2}"

                      }).Last();

            this.label2.Text = q4.ToString().Trim('{', '}') + "銷售最差";
            //======================================================================

            // 年度最高銷售金額 年度最低銷售金額

            var q7 = from od in this.dbContext.Order_Details
                     group od by od.Order.OrderDate.Value.Year into g
                     select new
                     {
                         Year = g.Key,
                         MaxSellPrice = g.Max(od => od.UnitPrice * od.Quantity),
                         MinSellPrice = g.Min(od => od.UnitPrice * od.Quantity)
                     };
            this.dataGridView3.DataSource = q7.ToList();

            this.chart3.DataSource = q7.ToList();

            this.chart3.Series[0].Color = Color.MediumSeaGreen;
            this.chart3.Series[1].Color = Color.Moccasin;
            this.chart3.Series[0].Name = "年度最高銷售金額";
            this.chart3.Series[1].Name = "年度最低銷售金額";
            this.chart3.Series[0].XValueMember = "Year";  //x軸
            this.chart3.Series[1].XValueMember = "Year";  //x軸
            this.chart3.Series[0].YValueMembers = "MaxSellPrice"; //y軸
            this.chart3.Series[1].YValueMembers = "MinSellPrice"; //y軸

            this.chart3.Series[0].IsValueShownAsLabel = true;
            this.chart3.Series[1].IsValueShownAsLabel = true;
            this.chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            this.chart3.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
        }
    }

}
