using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Starter
{
    public partial class FrmLINQ_To_XXX : Form
    {
        public FrmLINQ_To_XXX()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //寫法1
            //var q = from n in nums
            //IEnumerable<IGrouping<int, int>> q = from n in nums   //<int:key的型態, int:nums中每個item的型態>  //定義qeury一定要有from...where或from...group
            //                                     group n by n % 2;    //n % 2為key:分為1或0 的2群   //以group結尾

            //寫法2:資料改為偶數或奇數
            IEnumerable<IGrouping<string, int>> q = from n in nums
                                                    group n by n % 2 == 0 ? "偶數" : "奇數";

            this.dataGridView1.DataSource = q.ToList();  //結果的欄位名稱為Key:為IGrouping的屬性 //結果為Key
            //===========================================
            //使用TreeView呈現2群中的和item
            foreach (var group in q) 
            {
                TreeNode node = this.treeView1.Nodes.Add(group.Key.ToString()); //將Key加入主節點
                foreach (var item in group) 
                {
                    node.Nodes.Add(item.ToString());     //將item加入次節點
                }
            }
            //===========================================
            //使用ListView呈現2群中的和item
            foreach (var group in q)
            {
                ListViewGroup lvg = this.listView1.Groups.Add(group.Key.ToString(), group.Key.ToString()); //設定集合名稱
                foreach (var item in group)
                {
                    this.listView1.Items.Add(item.ToString()).Group = lvg;  //設定集合名稱內容並指定放到哪個集合
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //寫法3: 加入彙總函數

            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var q = from n in nums   //型態改為var
                    group n by n % 2 == 0 ? "偶數" : "奇數" into g  //站存到g變數
                    select new   //匿名型別
                    {
                        MyKey = g.Key,
                        MyCount = g.Count(),
                        MyMin = g.Min(),
                        MyAvg = g.Average(),
                         MyGroup = g
                     };

            this.dataGridView1.DataSource = q.ToList();

            //=================================
            //TreeView
            foreach (var group in q)
            {
                string s = $"{group.MyKey}({group.MyCount})";   //加入group的標題描述
                TreeNode node = this.treeView1.Nodes.Add(group.MyKey.ToString(), s); //將Key加入主節點
                foreach (var item in group.MyGroup)
                {
                    node.Nodes.Add(item.ToString());     //將item加入次節點
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var q = from n in nums   //型態改為var
                    group n by MyMethod(n) into g  //站存到g變數  //呼叫MyMethod(int n)方法
                    select new   //匿名型別
                    {
                        MyKey = g.Key,
                        MyCount = g.Count(),
                        MyMin = g.Min(),
                        MyAvg = g.Average(),
                        MyGroup = g
                    };

            this.dataGridView1.DataSource = q.ToList();
            //=================================
            //TreeView
            foreach (var group in q)
            {
                string s = $"{group.MyKey}({group.MyCount})";   //加入group的標題描述
                TreeNode node = this.treeView1.Nodes.Add(group.MyKey.ToString(), s); //將Key加入主節點
                foreach (var item in group.MyGroup)
                {
                    node.Nodes.Add(item.ToString());     //將item加入次節點
                }
            }
            //=================================
            //統計圖呈現
            //series0:MyCount
            this.chart1.DataSource = q.ToList();
            this.chart1.Series[0].XValueMember = "MyKey";
            this.chart1.Series[0].YValueMembers = "MyCount";
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            //series1:MyAvg
            this.chart1.Series[1].XValueMember = "MyKey";
            this.chart1.Series[1].YValueMembers = "MyAvg";
            this.chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
        }


        //設定MyMethod(int n)方法，用以判斷分群依據
        private string MyMethod(int n) 
        {
            if (n < 5)
            {
                return "Small";
            }
            else if (n < 10)
            {
                return "Medium";
            }
            else 
            {
                return "Large";
            }
        }

        //每種附檔名有幾個檔案
        private void button38_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files = dir.GetFiles();
            //System.IO.FileInfo[] adfiles = dir.FullName.

            this.dataGridView1.DataSource = files;

            var q = from n in files
                    group n by n.Extension into g
                    select new
                    {
                        MyKey = g.Key,
                        MyCount = g.Count()
                    };
            this.dataGridView2.DataSource = q.ToList();
        }

        //北風:每年訂單有幾筆
        private void button12_Click(object sender, EventArgs e)
        {
            this.ordersTableAdapter1.Fill(this.nwDataSet11.Orders);
            this.dataGridView1.DataSource = this.nwDataSet11.Orders;

            var q = from p in this.nwDataSet11.Orders
                    group p by p.OrderDate.Year into g
                    select new
                    {
                        MyKey = g.Key,
                        MyCount = g.Count()
                    };
            this.dataGridView2.DataSource = q.ToList();
            //=============================
            //北風:1997年訂單有幾筆
            int count = (from p in this.nwDataSet11.Orders
                      where p.OrderDate.Year == 1997
                      select p).Count();  //方法語法

            //var q2 = from p2 in this.nwDataSet11.Orders
            //        where p2.OrderDate.Year == 1997
            //        select p2;
            //foreach()


            MessageBox.Show("Count=" + count);
        }


        //let
        private void button3_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files = dir.GetFiles();

            int count = (from f in files
                         let s = f.Extension  //用變數s存f.Extension
                         where s == ".exe"  //再從s去找.ex的檔案
                         select f).Count();
            MessageBox.Show("Count=" + count);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string s = "This is a book. this is a pen. this is an apple.";
            char[] chars = { ' ', ',', '?', '.' };
            //string[] words = s.Split(chars);
            string[] words = s.Split(chars, StringSplitOptions.RemoveEmptyEntries);  //去除結果為空字串的部分

            var q = from w in words
                    group w by w.ToUpper() into g
                    select new { MyKey = g.Key, MyCount = g.Count() };
            this.dataGridView1.DataSource = q.ToList();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //集合運算子:Distinct/Union/Intersect
            int[] nums1 = { 1, 2, 3, 5, 11, 2 };
            int[] nums2 = { 1, 3, 66, 77, 111 };

            IEnumerable<int> q;
            q = nums1.Intersect(nums2);  //交集
            q = nums1.Union(nums2);  //連集
            q = nums1.Distinct();  //Distinct()

            //============================
            //切割運算子:Take/Skip
            q = nums1.Take(2); //取前2筆資料

            //============================
            //數量詞作業:Any/All/Contains
            bool result;
            result = nums1.Any(n => n > 100);
            result = nums1.Any(n => n % 2 == 0);
            result = nums1.All(n => n > 3);
            result = nums1.Contains(3);

            //============================
            //單一元素運算子:First/Last/Single/ElementAt/ FirstOrDefault/LastOrDefault/SingleOrDefault/ElementAtOrDefault

            int n1;
            n1 = nums1.First();
            n1 = nums1.Last();
            n1 = nums1.ElementAt(13);  //回傳索引，但無13會exception
            n1 = nums1.ElementAtOrDefault(13);  //回傳索引，無13不會exception，回傳預設值0

            //============================
            //產生作業Generation: Range/Repeat/Empty/DefultIfEmpty

            //列出1~1000
            var q1 = Enumerable.Range(1, 1000).Select(n => new { n });  //new屬性才會顯示在datagridview
            this.dataGridView1.DataSource = q1.ToList();
            
            //重複列出60 1000次
            var q2 = Enumerable.Repeat(60, 1000).Select(n => new { N=n });  //new屬性才會顯示在datagridview
            this.dataGridView2.DataSource = q2.ToList();


        }

        //北風:Products
        private void button10_Click(object sender, EventArgs e)
        {
            this.productsTableAdapter1.Fill(this.nwDataSet11.Products);

            var q = from p in this.nwDataSet11.Products
                    group p by p.CategoryID into g  //在Products中只能找到CategoryID
                    select new { CategoryID = g.Key, MyAvg = g.Average(p => p.UnitPrice) };
            this.dataGridView1.DataSource = q.ToList();

            //========================================
            //使用inner join
            this.categoriesTableAdapter1.Fill(this.nwDataSet11.Categories);

            var q2 = from c in this.nwDataSet11.Categories
                     join p in this.nwDataSet11.Products
                     on c.CategoryID equals p.CategoryID
                     group p by c.CategoryName into g
                     /*select new { CategoryName = g.Key, MyAvg = g.Average(p=>p.UnitPrice) };*/ //todo 格式化
                     select new { CategoryName = g.Key, MyAvg = g.Average(p => p.UnitPrice) };
            this.dataGridView2.DataSource = q2.ToList();
        }
    }

}
