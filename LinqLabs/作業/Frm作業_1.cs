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
            this.productsTableAdapter1.Fill(this.nwDataSet11.Products);
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        int endPage = 0;
        int startPage = 0;
        int countPage = 0;
        private void button13_Click(object sender, EventArgs e)
        {
            this.dataGridView2.DataSource = null;
            
            this.dataGridView1.DataSource = this.nwDataSet11.Products;

            startPage = endPage;
            countPage = Convert.ToInt32(textBox1.Text);
            endPage = startPage + countPage;

            var q = (from p in this.nwDataSet11.Products
                     select p).Skip(startPage).Take(countPage);

            this.dataGridView2.DataSource = q.ToList();          
        }
        private void button12_Click(object sender, EventArgs e)
        {
            this.dataGridView2.DataSource = null;

            endPage = startPage;
            countPage = Convert.ToInt32(textBox1.Text);
            startPage = startPage - countPage;

            var q = (from p in this.nwDataSet11.Products
                     select p).Skip(startPage).Take(countPage);

            this.dataGridView2.DataSource = q.ToList();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files =  dir.GetFiles();

            this.dataGridView1.DataSource = files;
            IEnumerable<System.IO.FileInfo> q = from f in files
                    where f.Extension == ".log"
                    select f;
            this.dataGridView1.DataSource = q.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files = dir.GetFiles();

            this.dataGridView1.DataSource = files;
            IEnumerable<System.IO.FileInfo> q = from f in files
                                                where f.CreationTime.Year == 2019
                                                select f;
            this.dataGridView1.DataSource = q.ToList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files = dir.GetFiles();

            this.dataGridView1.DataSource = files;
            IEnumerable<System.IO.FileInfo> q = from f in files
                                                where f.Length > 100000
                                                select f;
            this.dataGridView1.DataSource = q.ToList();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.comboBox1.Items.Clear();
            this.dataGridView2.DataSource = null;
            this.ordersTableAdapter1.Fill(this.nwDataSet11.Orders);
            this.dataGridView1.DataSource = this.nwDataSet11.Orders;

            var q = from p in this.nwDataSet11.Orders
                    select p.OrderDate.Year;  //取得的year為int型態

            foreach (int y in q.Distinct()) 
            {
                this.comboBox1.Items.Add(y);
            }

            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.dataGridView2.DataSource = null;
            this.ordersTableAdapter1.Fill(this.nwDataSet11.Orders);
            this.dataGridView1.DataSource = this.nwDataSet11.Orders;

            var q = from p in this.nwDataSet11.Orders
                    where p.OrderDate.Year == Convert.ToInt32(comboBox1.Text) && !p.IsShippedDateNull() //去除空值資料
                    select p;
            this.dataGridView2.DataSource = q.ToList();  //擴充功能

        }
    }
}
