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
    public partial class Frm作業_3 : Form
    {
        public Frm作業_3()
        {
            InitializeComponent();
        }

        //依檔案大小分組檔案 (大=>小)
        private void button38_Click(object sender, EventArgs e)
        {
            //顯示所有檔案資訊

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files = dir.GetFiles();
            //System.IO.FileInfo[] files = dir.GetFiles("*.exe"); //指定取得.exe的檔案
            this.dataGridView1.DataSource = files;

            //顯示檔案分組資訊

            var q = from f in files
                    orderby f.Length descending  //按照年份排序
                    group f by FileSize(f.Length) into g  //FileSize(f.Length)定義檔案大小分組依據
                    select new
                    {
                        FileKey = g.Key,        //欄位名稱/屬性
                        FileCount = g.Count(),
                        FileGroup = g
                    };

            this.dataGridView2.DataSource = q.ToList();

            //=================================================================
            // treeView

            this.treeView1.Nodes.Clear(); //清除treeView1

            foreach (var group in q)
            {
                string s = $"{group.FileKey }({ group.FileGroup})";
                TreeNode node = this.treeView1.Nodes.Add(group.FileKey.ToString(), s); //加分組名稱到主node

                foreach (var item in group.FileGroup)
                {
                    node.Nodes.Add(item.ToString());  //加組內成員到次node
                }
            }
        }
        //FileSize(f.Length)方法
        private object FileSize(long size)
        {
            if (size > 1000000)
            {
                return "Large File";
            }
            else if (size > 100000)
            {
                return "Medium File";
            }
            else 
            {
                return "Small File";
            }
        }

        // 依"年"分組檔案 (大=>小)
        private void button6_Click(object sender, EventArgs e)
        {
            //顯示所有檔案資訊
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:/windows");
            System.IO.FileInfo[] files = dir.GetFiles();

            this.dataGridView1.DataSource = files;

            //顯示檔案分組資訊
            var q = from f in files
                    orderby f.CreationTime.Year descending  //按照年份排序
                    group f by FileCreationTime(f.CreationTime.Year) into g                   
                    select new
                    {
                        FileKey = g.Key,        //欄位名稱/屬性
                        FileCount = g.Count(),
                        FileGroup = g
                    };
            this.dataGridView2.DataSource = q.ToList();

            //============================
            //TreeView

            this.treeView1.Nodes.Clear(); //清除treeView1

            foreach (var group in q) 
            {
                string s = $"{ group.FileKey}({group.FileCount})";

                TreeNode node = this.treeView1.Nodes.Add(group.FileKey.ToString(), s);

                foreach (var item in group.FileGroup) 
                {
                    node.Nodes.Add(item.ToString());
                }
            }
        }

        private object FileCreationTime(int year)
        {
            if (year >= 2021)
            {
                return "2021年後建立的檔案";
            }
            else if (year >= 2019)
            {
                return "2019年後建立的檔案";
            }
            else 
            {
                return "2017年後建立的檔案";
            }
        }
        //int[]分三群 - No LINQ
        private void button4_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 3, 5, 8, 99, 151, 200, 1002, 2005 };

            List<int> group1List = new List<int>();
            List<int> group2List = new List<int>();
            List<int> group3List = new List<int>();

            int group1 = 1000;
            int group2 = 100;
            int group3 = 0;

            foreach (int n in nums)
            {
                if (n > group1)
                {
                    group1List.Add(n);

                }
                else if (n > group2)
                {
                    group2List.Add(n);
                }
                else
                {
                    group3List.Add(n);
                }
            }
            int count1 = group1List.Count;
            int count2 = group2List.Count;
            int count3 = group3List.Count;

            TreeNode node1 = this.treeView1.Nodes.Add(group1.ToString());
            TreeNode node2 = this.treeView1.Nodes.Add(group2.ToString());
            TreeNode node3 = this.treeView1.Nodes.Add(group3.ToString());

            foreach (int n in group1List) 
            {
                node1.Nodes.Add(n.ToString());
            }
            foreach (int n in group2List)
            {
                node2.Nodes.Add(n.ToString());
            }
            foreach (int n in group3List)
            {
                node3.Nodes.Add(n.ToString());
            }
        }
    }
}
