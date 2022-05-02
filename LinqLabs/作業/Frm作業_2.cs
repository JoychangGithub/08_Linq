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
    public partial class Frm作業_2 : Form
    {
        public Frm作業_2()
        {
            InitializeComponent();
            this.productPhotoTableAdapter1.Fill(this.awDataSet11.ProductPhoto); //避免以下重複搜尋造成重複繫結
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //避免System.ArgumentException: '這會造成集合中的兩個繫結與相同的屬性產生繫結。
            this.pictureBox1.DataBindings.Clear();

            // 顯示所有腳踏車資料
            //this.dataGridView1.DataSource = null;

            //this.dataGridView1.DataSource = this.awDataSet11.ProductPhoto;
            this.bindingSource1.DataSource = this.awDataSet11.ProductPhoto;
            this.dataGridView1.DataSource = this.bindingSource1;
            int count = this.bindingSource1.Count;
            lblMaster.Text = "共有 " + count + " 筆";

            //lblMaster.Text = "共有 " + this.dataGridView1.Rows.Count.ToString() + " 筆";

            //====================================================
            //details: 顯示腳踏車圖片
            this.pictureBox1.DataBindings.Add("Image", this.bindingSource1, "LargePhoto", true);

            //====================================================
            //設定腳踏車日期區間
            var q1 = from p1 in this.awDataSet11.ProductPhoto
                    select p1.ModifiedDate;

            DateTime minday = DateTime.Today;
            foreach (DateTime d in q1.Distinct())  //設定最早搜尋日期
            {
                if (d < minday)
                {
                    minday = d;
                }
                dateTimePicker1.Value = minday;
            }

            DateTime maxday = minday;
            foreach (DateTime d in q1.Distinct())  //設定最晚搜尋日期
            {
                if (d > maxday)
                {
                    maxday = d;
                }
                dateTimePicker2.Value = maxday;
            }
            //====================================================
            //設定腳踏車年份
            var q2 = from p2 in this.awDataSet11.ProductPhoto
                     orderby p2.ModifiedDate.Year ascending  //使combobox3中的年份由小到大排序
                    select p2.ModifiedDate.Year;

            foreach (int y in q2.Distinct()) 
            {
                comboBox3.Items.Add(y);
            }
            comboBox3.SelectedIndex = 0;
            //====================================================
            //預設為第一季
            comboBox2.SelectedIndex = 0;
        }

        //搜尋某日期區間的腳踏車
        private void button3_Click(object sender, EventArgs e)
        {
            //this.dataGridView1.DataSource = null;
            //this.dataGridView1.DataSource = this.awDataSet11.ProductPhoto;

            //按All腳踏車為 datasource defined

            var q = from p in this.awDataSet11.ProductPhoto
                    where p.ModifiedDate >= dateTimePicker1.Value && p.ModifiedDate <= dateTimePicker2.Value
                    select p;

            //this.dataGridView1.DataSource = q.ToList();
            this.bindingSource1.DataSource = q.ToList();  //繫結照片
            this.dataGridView1.DataSource = this.bindingSource1;

            int count = this.bindingSource1.Count;
            lblMaster.Text = "共有 " + count + " 筆";

            //lblMaster.Text = "共有 " + this.dataGridView1.Rows.Count.ToString() + " 筆";

        }

        //搜尋某年份的腳踏車
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                //按All腳踏車為 datasource defined
                var q = from p in this.awDataSet11.ProductPhoto
                        where p.ModifiedDate.Year == Convert.ToInt32(comboBox3.Text)
                        select p;

                //this.dataGridView1.DataSource = q.ToList();
                this.bindingSource1.DataSource = q.ToList();  //繫結照片
                this.dataGridView1.DataSource = this.bindingSource1;

                int count = this.bindingSource1.Count;
                lblMaster.Text = "共有 " + count + " 筆";

                //lblMaster.Text = "共有 " + this.dataGridView1.Rows.Count.ToString() + " 筆";

            }
            catch
            {
                MessageBox.Show("Please press All 腳踏車 first");
            }
        }

        int month1;
        int month3;
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                string season = comboBox2.Text;

                if (season == "第一季")
                {
                    month1 = 1;
                    month3 = 3;
                }
                else if (season == "第二季")
                {
                    month1 = 4;
                    month3 = 6;
                }
                else if (season == "第三季")
                {
                    month1 = 7;
                    month3 = 9;
                }
                else
                {
                    month1 = 10;
                    month3 = 12;
                }

                var q = from p in this.awDataSet11.ProductPhoto
                        where p.ModifiedDate.Year == Convert.ToInt32(comboBox3.Text) && (p.ModifiedDate.Month >= month1 && p.ModifiedDate.Month <= month3)
                        select p;

                //this.dataGridView1.DataSource = q.ToList();
                this.bindingSource1.DataSource = q.ToList();  //繫結照片
                this.dataGridView1.DataSource = this.bindingSource1;

                int count = this.bindingSource1.Count;
                lblMaster.Text = "共有 " + count + " 筆";
                //lblMaster.Text = "共有 " + this.dataGridView1.Rows.Count.ToString() + " 筆";
            }
            catch
            {
                MessageBox.Show("Please press All 腳踏車 first");
            }
        }
    }
}
