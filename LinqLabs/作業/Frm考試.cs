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
            public string Class { get; set; }
            public int Chi { get; set; }
            public int Eng { get; internal set; }
            public int Math { get; set; }
            public string Gender { get; set; }
        }

        private void button36_Click(object sender, EventArgs e)
        {
            #region 搜尋 班級學生成績

            // 共幾個 學員成績 ?						

            // 找出 前面三個 的學員所有科目成績	
            
            var FirstThree = students_scores.Select(n=> new {n.Name, n.Chi, n.Eng, n.Math }).Take(3);
            dataGridView2.DataSource = FirstThree.ToList();

            // 找出 後面兩個 的學員所有科目成績
            int count = students_scores.Count;
            var LastTwo = students_scores.Select(n => new { n.Name, n.Chi, n.Eng, n.Math }).Skip(count-2);
            dataGridView1.DataSource = LastTwo.ToList();

            // 找出 Name 'aaa','bbb','ccc' 的學員國文英文科目成績
            var SpecificName = students_scores.Select(n => new { n.Name, n.Chi, n.Eng }).Where(n => n.Name == "aaa" || n.Name == "bbb" || n.Name == "ccc");
            dataGridView3.DataSource = SpecificName.ToList();

            // 找出學員 'bbb' 的成績	                          
            var Namebbb = students_scores.Select(n => new { n.Name, n.Chi, n.Eng, n.Math }).Where(n => n.Name == "bbb");
            dataGridView4.DataSource = Namebbb.ToList();

            // 找出除了 'bbb' 學員的學員的所有成績 ('bbb' 退學)	
            var bbb = students_scores.Where(n => n.Name == "bbb");
            var Exceptionbbb = students_scores.Except(bbb);  //取未交集的部分
            dataGridView5.DataSource = Exceptionbbb.ToList();

            // 找出 'aaa', 'bbb' 'ccc' 學員 國文數學兩科 科目成績	
            var SpecificNames = students_scores.Select(n => new { n.Name, n.Chi, n.Math }).Where(n => n.Name == "aaa" || n.Name == "bbb" || n.Name == "ccc");
            dataGridView6.DataSource = SpecificNames.ToList();

            // 數學不及格 ... 是誰 
            var MathFailed = students_scores.Select(n => new { n.Name, n.Chi, n.Eng, n.Math }).Where(n => n.Math < 60);
            dataGridView7.DataSource = MathFailed.ToList();
            #endregion

        }

        private void button37_Click(object sender, EventArgs e)
        {
            //個人 sum, min, max, avg

            var q1 = from s in this.students_scores  //個人 sum
                     select new
                     {
                         s.Name,
                         Sum = s.Chi + s.Eng + s.Math
                     };
            this.dataGridView2.DataSource = q1.ToList();


            var q2 = from s in this.students_scores  //個人 min
                     //where s.Name == "aaa"
                     select new
                     {
                         s.Name,
                         s.Class,
                         s.Chi,
                         s.Eng,
                         s.Math,
                         //Min = new int[] { s.Chi, s.Eng, s.Math}.Min()  //建立只有成績的struct //方法1
                         Min = new List<int> { s.Chi, s.Eng, s.Math }.Min(),  //建立只有成績的list  //方法2
                         Max = new List<int> { s.Chi, s.Eng, s.Math }.Max(),
                         Ave = new List<int> { s.Chi, s.Eng, s.Math }.Average()
                     };


            this.dataGridView1.DataSource = q2.ToList();


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

            var q = from od in this.dbContext.Order_Details.AsEnumerable()
                    group od by od.Order.OrderDate.Value.Year into g
                    orderby g.Key ascending
                    select new
                    {
                        Year = g.Key,
                        TotalPrice = g.Sum(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount))

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
                      orderby g.Sum(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount)) descending
                      select new
                      {
                          Year = g.Key,
                          TotalPrice = $"{g.Sum(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount)):c2}"

                      }).First();
            this.label3.Text = q5.ToString().Trim('{', '}') + "銷售最佳";

            //哪一年總銷售最不好?
            var q6 = (from od in this.dbContext.Order_Details.AsEnumerable()
                      group od by od.Order.OrderDate.Value.Year into g
                      orderby g.Sum(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount)) descending
                      select new
                      {
                          Year = g.Key,
                          TotalPrice = $"{g.Sum(od => od.UnitPrice * od.Quantity):c2}"

                      }).Last();
            this.label4.Text = q6.ToString().Trim('{', '}') + "銷售最差";
            //============================================================================================

            // 每月 總銷售分析 圖

            var q2 = from od in this.dbContext.Order_Details.AsEnumerable()
                     group od by new { od.Order.OrderDate.Value.Month } into g
                     orderby g.Key.Month ascending
                     select new
                     {
                         //Classification = g.Key,
                         Month = g.Key.Month,
                         AveragePrice = g.Average(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount))

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
                      group od by new { od.Order.OrderDate.Value.Month } into g
                      orderby g.Average(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount)) descending
                      select new
                      {
                          //Classification = g.Key,
                          Month = g.Key.Month,
                          AveragePrice = $"{g.Average(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount)):c2}"

                      }).First();

            this.label1.Text = q3.ToString().Trim('{', '}') + "銷售最佳";

            // 那一個月總銷售最不好 ?
            var q4 = (from od in this.dbContext.Order_Details.AsEnumerable()
                      group od by new { od.Order.OrderDate.Value.Month } into g
                      orderby g.Average(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount)) descending
                      select new
                      {
                          //Classification = g.Key,
                          Month = g.Key.Month,
                          AveragePrice = $"{g.Average(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount)):c2}"

                      }).Last();

            this.label2.Text = q4.ToString().Trim('{', '}') + "銷售最差";
            //======================================================================
            // 年度最高銷售金額 年度最低銷售金額
            var q8 = from o in this.dbContext.Orders.AsEnumerable()
                     group o by o.OrderDate.Value.Year into g
                     orderby g.Key
                     select new
                     {
                         Year = g.Key,
                         MaxSellPrice = g.Max(o => o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount))),
                         MinSellPrice = g.Min(o => o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount)))
                     };

            this.dataGridView3.DataSource = q8.ToList();
            this.chart3.DataSource = q8.ToList();

            this.chart3.Series[0].Color = Color.MediumSeaGreen;
            this.chart3.Series[1].Color = Color.Moccasin;
            this.chart3.Series[0].Name = "年度最高銷售金額";
            this.chart3.Series[1].Name = "年度最低銷售金額";
            this.chart3.Series[0].XValueMember = "Year";  //x軸
            this.chart3.Series[1].XValueMember = "Year";  //x軸
            this.chart3.Series[0].YValueMembers = "MaxSellPrice"; //y軸
            this.chart3.Series[1].YValueMembers = "MinSellPrice"; //y軸
            this.chart3.Series[1].YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary; //加x軸

            this.chart3.Series[0].IsValueShownAsLabel = true;
            this.chart3.Series[1].IsValueShownAsLabel = true;
            this.chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            this.chart3.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
        }

        //Dictionary<int, decimal> yearSellDic = new Dictionary<int, decimal>();
        List<int> yearList = new List<int>();
        private void button6_Click(object sender, EventArgs e)
        {
            var q = from od in this.dbContext.Order_Details.AsEnumerable()
                    group od by od.Order.OrderDate.Value.Year into g
                    select new
                    {
                        Year = g.Key,                      
                        TotalPrice = g.Sum(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount)),
                        Group = g
                    };
            this.dataGridView4.DataSource = q.ToList();




            var Q = q.ToList();
            int year;
            decimal sells;
            decimal original;
            decimal rate;
            decimal percentageRate;

            List<decimal> increasingList = new List<decimal>();
            yearList.Add(1996);
            increasingList.Add(0); //
            for (int i = 1; i < Q.Count; i++)
            {
                original = Q[i-1].TotalPrice;
                year = Q[i].Year;
                sells = Q[i].TotalPrice;

                rate = (sells - original) / original;

                yearList.Add(year);

                percentageRate = (sells - original) / original*100;
                increasingList.Add(percentageRate);
                //MessageBox.Show($"{ year}: {rate:f5}%");

                original = sells;
            }

            for (int i = 0; i < increasingList.Count; i++)
            {
                this.chart5.DataSource = increasingList;
                this.chart5.Series[0].Points.AddXY(yearList[i],increasingList[i]);
                this.chart5.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;                
                this.chart5.Series[0].Color = Color.MediumSeaGreen;
                this.chart5.Series[0].Name = "年度最高銷售金額";

                this.chart5.Series[1].Points.AddXY(yearList[i], increasingList[i]);
                this.chart5.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                this.chart5.Series[1].Color = Color.Maroon;
            }
        }
    }
}
