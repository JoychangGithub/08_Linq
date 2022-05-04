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
    public partial class FrmLINQ架構介紹_InsideLINQ : Form
    {
        public FrmLINQ架構介紹_InsideLINQ()
        {
            InitializeComponent();
        }

        private void button30_Click(object sender, EventArgs e)
        {
            System.Collections.ArrayList arrlist = new System.Collections.ArrayList();
            arrlist.Add(3);
            arrlist.Add(4);
            arrlist.Add(1);

            var q = from n in arrlist.Cast<int>()    //因為為object需用<int>將轉成int
                    where n > 2
                    //select n;  //此時無顯示結果，因為此int無屬性，需new屬性
                    select new { N = n };

            //foreach (int i in q)  //顯示方法1:搭配//select n;
            //{
            //    this.listBox1.Items.Add(i);
            //}

            this.dataGridView1.DataSource = q.ToList();  //顯示方法2:搭配select new { N = n };
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;  //解決重複按2次出現的重複繫結問題:方法1 //放到建構子:方法2
            this.productsTableAdapter1.Fill(this.nwDataSet11.Products);
            var q = (from p in this.nwDataSet11.Products
                     orderby p.UnitPrice descending
                     select p).Take(5);  //找前5筆資料
            this.dataGridView1.DataSource=q.ToList();
        }

        //彙總函數運算式
        private void button1_Click(object sender, EventArgs e)
        {

            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            this.listBox1.Items.Add("sum= " + nums.Sum()); //總和
            this.listBox1.Items.Add("min= " + nums.Min());
            this.listBox1.Items.Add("max= " + nums.Max());
            this.listBox1.Items.Add("avg= " + nums.Average());
            this.listBox1.Items.Add("count= " + nums.Count());

            //===============================================
            //北風products例子
            this.productsTableAdapter1.Fill(this.nwDataSet11.Products);
            this.listBox1.Items.Add($"Max UnitPrice={ this.nwDataSet11.Products.Max(p => p.UnitPrice):c2}"); //單價最高 //格式化
            this.listBox1.Items.Add($"Min UnitPrice= {this.nwDataSet11.Products.Min(p => p.UnitPrice):c2}"); //單價最低 //格式化
            this.listBox1.Items.Add($"Sum UnitPrice={ this.nwDataSet11.Products.Sum(p => p.UnitPrice):c2}"); //單價總和 //格式化
            this.listBox1.Items.Add($"Average UnitPrice={this.nwDataSet11.Products.Average(p => p.UnitPrice):c2}"); //單價平均 //格式化

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int[] nums = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //====================================
            //延遲執行:

            int i = 0;
            var q = from n in nums
                    select ++i;

            foreach (var v in q)
            {
                listBox1.Items.Add(String.Format("v={0}, i={1}", v, i));
            }
            listBox1.Items.Add("======================");
            //====================================
            //立即執行:
            //int i = 0;
            //var q = (from n in nums
            //          select ++i).ToList();

            //foreach (var v in q)
            //{
            //    listBox1.Items.Add(String.Format("v={0}, i={1}", v, i));
            //}
            listBox1.Items.Add("======================");
        }
    }
}