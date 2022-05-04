using LinqLabs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Starter
{
    public partial class FrmLinq_To_Entity : Form
    {
        public FrmLinq_To_Entity()
        {
            InitializeComponent();

            this.dbContext.Database.Log = Console.Write; //委派物件log指向Console.Write方法  //在輸出視窗中顯示T-SQL指令碼 //平常不會寫此，因為號效能
        }

        NorthwindEntities dbContext = new NorthwindEntities(); //記憶體的BD:dbContext //需using LinqLabs : NorthwindEntities在namespace LinqLabs下
        private void button1_Click(object sender, EventArgs e)
        {        
            var q = from p in dbContext.Products
                    where p.UnitPrice > 30
                    select p;

            this.dataGridView1.DataSource = q.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //從Category找product
            this.dataGridView1.DataSource = this.dbContext.Categories.First().Products.ToList();

            MessageBox.Show(this.dbContext.Products.First().Category.CategoryName);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //使用預存程序:Sales_by_Year
            //從1998/1/1到目前的訂單ID
            this.dataGridView1.DataSource = this.dbContext.Sales_by_Year(new DateTime(1998, 1, 1), DateTime.Now).ToList();
        }

        //[orderby , then by]
        private void button22_Click(object sender, EventArgs e)
        {
            //查詢運算式寫法
            var q = from p in this.dbContext.Products.AsEnumerable()  //加了.AsEnumerable(): 變成select * from products(執行T-SQL)，抓回記憶體中
                    orderby p.UnitsInStock descending, p.ProductID descending
                    //select p;
                    select new
                    {
                        p.ProductID,
                        p.ProductName,
                        p.UnitPrice,
                        p.UnitsInStock,
                        TotalPrice = $"{p.UnitPrice* p.UnitsInStock:c2}"
                        //System.NotSupportedException: 'LINQ to Entities 無法辨識方法 'System.String Format(System.String, System.Object)' 方法
                        //解決方法: 上面加.AsEnumerable()             
                    };


            this.dataGridView1.DataSource = q.ToList();
            //============================================
            //方法語法寫法
            var q2 = this.dbContext.Products.OrderByDescending(p => p.UnitsInStock).ThenByDescending(p => p.ProductID);

            this.dataGridView2.DataSource = q2.ToList();

        }

        private void button16_Click(object sender, EventArgs e)
        {
            //物件化查詢: EntityDataModel
            var q = from p in this.dbContext.Products
                    select new { p.Category, p.Category.CategoryName, p.ProductName, p.UnitPrice };

            this.dataGridView3.DataSource = q.ToList();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            var q = from c in dbContext.Categories
                    join p in this.dbContext.Products
                    on c.CategoryID equals p.CategoryID
                    select new
                    {
                        c.CategoryID,
                        c.CategoryName,
                        p.ProductName,
                        p.UnitPrice
                    };
            this.dataGridView1.DataSource = q.ToList();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            //inner join:  Products中無categoryID=13的產品，因此結果不會出現categoryID=13的資訊

            var q = from c in this.dbContext.Categories
                    from p in c.Products  //物件化查詢
                    select new
                    {
                        c.CategoryID,
                        c.CategoryName,
                        p.ProductName,
                        p.UnitPrice
                    };
            this.dataGridView1.DataSource = q.ToList();
        }

        //join - groupby: 尋找各分類的平均單價
        private void button11_Click(object sender, EventArgs e)
        {
            var q = from p in this.dbContext.Products
                    group p by p.Category.CategoryName into g
                    select new
                    {
                        CategoryName = g.Key,
                        AvgUnitPrice = g.Average(p => p.UnitPrice)
                    };
            this.dataGridView1.DataSource = q.ToList();
        }

        //尋找 低中高 價產品
        private void button8_Click(object sender, EventArgs e)
        {
            var q = from o in this.dbContext.Orders
                    group o by o.OrderDate.Value.Year into g  //o.OrderDate可為null，在DB設計中若有可為空值的數值型態會加上"?"，因此需.Value才能點到Year
                    select new
                    {
                        Year = g.Key,
                        Count = g.Count()
                    };
            this.dataGridView1.DataSource = q.ToList();
        }

        //[新增修改刪除資料到實體DB]
        private void button55_Click(object sender, EventArgs e)
        {
            //[新增資料到實體DB]
            Product prod = new Product { ProductName = DateTime.Now.ToLongTimeString(), Discontinued = true };
            this.dbContext.Products.Add(prod);  //新增資料
            this.dbContext.SaveChanges();  //更新到實體DB
        }
    }
}
