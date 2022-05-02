using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHomeWork
{
    public partial class Frm作業_1 : Form
    {
        public Frm作業_1()
        {
            InitializeComponent();
            this.productsTableAdapter1.Fill(this.nwDataSet11.Products); //北風products資料表
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }
        //Northwind - Products
        int endPage = 0;
        int startPage = 0;
        int countPage = 0;
        private void button13_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView2.DataSource = null;
            lblMaster.Text = "";

            startPage = endPage;
            countPage = Convert.ToInt32(textBox1.Text);
            endPage = startPage + countPage;

            var q = (from p in this.nwDataSet11.Products
                     select p).Skip(startPage).Take(countPage);

            this.dataGridView2.DataSource = q.ToList();
            lblDetails.Text = "共 " + this.dataGridView2.Rows.Count.ToString() + " 筆產品";  //產品數量
        }
        private void button12_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView2.DataSource = null;
            lblMaster.Text = "";

            endPage = startPage;
            countPage = Convert.ToInt32(textBox1.Text);
            startPage = startPage - countPage;

            var q = (from p in this.nwDataSet11.Products
                     select p).Skip(startPage).Take(countPage);

            this.dataGridView2.DataSource = q.ToList();
            lblDetails.Text = "共 " + this.dataGridView2.Rows.Count.ToString() + " 筆產品";  //產品數量
        }
        //================================================================================================
        //檔案
        private void button14_Click(object sender, EventArgs e)
        {
            this.dataGridView2.DataSource = null;

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files =  dir.GetFiles();

            this.dataGridView1.DataSource = files;
            IEnumerable<System.IO.FileInfo> q = from f in files
                    where f.Extension == ".log"
                    select f;
            this.dataGridView1.DataSource = q.ToList();
            lblMaster.Text = "共 " + this.dataGridView1.Rows.Count.ToString() + " 筆.Log擋";  //檔案數量
            lblDetails.Text = "";
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            this.dataGridView2.DataSource = null;

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files = dir.GetFiles();

            this.dataGridView1.DataSource = files;
            IEnumerable<System.IO.FileInfo> q = from f in files
                                                where f.CreationTime.Year == 2019
                                                select f;
            this.dataGridView1.DataSource = q.ToList();
            lblMaster.Text = "共 " + this.dataGridView1.Rows.Count.ToString() + " 筆2019年建立的檔案";  //檔案數量
            lblDetails.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.dataGridView2.DataSource = null;

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files = dir.GetFiles();

            this.dataGridView1.DataSource = files;
            IEnumerable<System.IO.FileInfo> q = from f in files
                                                where f.Length > 100000
                                                select f;
            this.dataGridView1.DataSource = q.ToList();
            lblMaster.Text = "共 " + this.dataGridView1.Rows.Count.ToString() + " 筆大於100000大小的檔案";
            lblDetails.Text = "";
        }
        //================================================================================================
        //Northwind - Orders
        private void button6_Click(object sender, EventArgs e)
        {
            this.comboBox1.Items.Clear();
            this.dataGridView1.DataSource = null;

            this.ordersTableAdapter1.Fill(this.nwDataSet11.Orders);
            this.dataGridView1.DataSource = this.nwDataSet11.Orders;


            lblMaster.Text = "共 " + this.dataGridView1.Rows.Count.ToString() + " 筆訂單";  //訂單數量
            //=====================================================
            //北風orders加入年份到comboBox1

            var q = from p in this.nwDataSet11.Orders
                    select p.OrderDate.Year;  //取得的year為int型態

            foreach (int y in q.Distinct()) 
            {
                this.comboBox1.Items.Add(y);
            }

            comboBox1.SelectedIndex = 0;
        }

        //Northwind - Orders
        private void button1_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView2.DataSource = null;

            this.ordersTableAdapter1.Fill(this.nwDataSet11.Orders);
            this.order_DetailsTableAdapter1.Fill(this.nwDataSet11.Order_Details);

            //訂單
            var q1 = from p1 in this.nwDataSet11.Orders
                    where p1.OrderDate.Year == Convert.ToInt32(comboBox1.Text) && !p1.IsShippedDateNull() //去除空值資料
                     select p1;
            this.dataGridView1.DataSource = q1.ToList();  //訂單資料
            lblMaster.Text = "共 " + this.dataGridView1.Rows.Count.ToString() + " 筆訂單";  //訂單數量

            //訂單明細
            var q2 = from o in this.nwDataSet11.Orders
                     join d in this.nwDataSet11.Order_Details  //將shippedDate的nullvalue屬性改為empty
                     on o.OrderID equals d.OrderID
                     where o.OrderDate.Year == Convert.ToInt32(comboBox1.Text)
                     select new { o.OrderID, d.ProductID, d.UnitPrice, d.Quantity, d.Discount };

            this.dataGridView2.DataSource = q2.ToList();  //訂單明細資料
            lblDetails.Text = "共 " + this.dataGridView2.Rows.Count.ToString() + " 筆訂單明細";  //訂單明細數量
        }
    }
}
