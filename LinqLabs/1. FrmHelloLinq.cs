using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Starter
{
    public partial class FrmHelloLinq : Form
    {
        public FrmHelloLinq()
        {
            InitializeComponent();
            this.productsTableAdapter1.Fill(this.nwDataSet11.Products);
            this.ordersTableAdapter1.Fill(this.nwDataSet11.Orders);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //syntax sugar寫法
            foreach (int n in nums) 
            {
                this.listBox1.Items.Add(n);
            }
            this.listBox1.Items.Add("======================");
            //===========================================
            //非syntax sugar寫法:c#內部轉譯作法
            System.Collections.IEnumerator en = nums.GetEnumerator();

            while (en.MoveNext())
            {
                this.listBox1.Items.Add(en.Current);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<int> list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            foreach (int n in list) 
            {
                this.listBox1.Items.Add(n);
            }
            this.listBox1.Items.Add("======================");
            //===========================================
            //非syntax sugar寫法:c#內部轉譯作法
            List<int>.Enumerator en = list.GetEnumerator();
            //var en = list.GetEnumerator();  //當不知道型別時可以使用var

            while (en.MoveNext()) 
            {
                this.listBox1.Items.Add(en.Current);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //step1: define Data Source
            //step2: define Query
            //step3: execute Query

            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };  //step1: define Data Source

            //IEnumerable<int> q = from n in nums
            //                     where (n >= 5 && n <= 8) && (n%2==0)
            //                     select n;

            IEnumerable<int> q = from n in nums
                                 where (n <= 3 || n >= 8) && (n % 2 == 0)
                                 select n;

            foreach (int n in q)
            {
                this.listBox1.Items.Add(n);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11,12 }; 

            IEnumerable<int> q = from n in nums
                                 where IsEven(n)    //呼叫方法
                                 select n;

            foreach (int n in q)
            {
                this.listBox1.Items.Add(n);
            }
        }
        bool IsEven(int n) 
        {
            //if (n % 2 == 0)   //作法1
            //{
            //    return true;
            //}
            //else 
            //{
            //    return false;
            //}
            return n % 2 == 0;  //作法2
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            IEnumerable<Point> q = from n in nums
                                   where n > 5
                                   select new Point(n, n * n);

            //execute query作法1:將query結果加到listBox
            foreach (Point pt in q)  
            {
                this.listBox1.Items.Add(pt.X + ",  " + pt.Y);
            }

            //execute query作法2:將query結果加到dataGridView1 (List可繫結到datagridview)
            List<Point> list = q.ToList();
            this.dataGridView1.DataSource = list;

            //execute query作法3:
            //拉chart工具項
            this.chart1.DataSource = list;
            this.chart1.Series[0].XValueMember = "X";
            this.chart1.Series[0].YValueMembers = "Y";
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] words = { "aaa", "Apple", "pineApple", "xxxapple" };
            IEnumerable<string> q = from w in words
                                    where w.Contains("Apple")
                                    select w;

            foreach (string s in q) 
            {
                this.listBox1.Items.Add(s);
            }
            this.dataGridView1.DataSource = q.ToList();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = this.nwDataSet11.Products;

            //global:: for quickly find namespace for q
            IEnumerable<global::LinqLabs.NWDataSet1.ProductsRow> q = from p in this.nwDataSet11.Products
                    where !p.IsUnitPriceNull() && p.UnitPrice > 30 && p.UnitPrice < 50 && p.ProductName.StartsWith("M")
                    select p;

            this.dataGridView1.DataSource = q.ToList();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = this.nwDataSet11.Orders;

            var q = from p in this.nwDataSet11.Orders
                    where p.OrderDate.Year == 1997 && (p.OrderDate.Month == 1 || p.OrderDate.Month == 2 || p.OrderDate.Month == 3) //某年某季訂單
                    select p;

            this.dataGridView1.DataSource = q.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //IEnumerable <int> q =nums.Where
        }
    }
}
