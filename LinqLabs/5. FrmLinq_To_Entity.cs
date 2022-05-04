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
            var q = from p in dbContext.Products  //var: 真正型別為IQueryable<T>
                   //IEnumerable是把整個資料撈回來存在記憶體再去篩選,
                  //IQueryable是條件都串好再去query資料, 遇到資料量大不管怎樣轉成IQueryable的型別就對了

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
            var q = from p in this.dbContext.Products.AsEnumerable()  //加了.AsEnumerable(): 變成select * from products(執行T-SQL)，抓回記憶體中在執行Query
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
            //inner join:  Products中無categoryID=10的產品，因此結果不會出現categoryID=10的資訊 (寫法1)

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
            //=============================================================
            //寫法2:
            //this.dbContext.Categories.SelectMany(c => c.Products, (c, p) => new { c.CategoryID, c.CategoryName, p.ProductID, p.UnitPrice});
           
            //=============================================================
            //cross join : 此方法不行，會出現錯誤
            var q2 = from c in this.dbContext.Categories
                     from p in this.dbContext.Products
                     select new { c.CategoryID, c.CategoryName, p.ProductID, p.UnitPrice, p.UnitsInStock };
            MessageBox.Show("q2.count() =" + q2.Count());
            this.dataGridView2.DataSource = q2.ToList();
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
            
        }

        //[新增修改刪除資料到實體DB]
        private void button55_Click(object sender, EventArgs e)
        {
            //[新增資料到實體DB]: Add Product
            Product product = new Product { ProductName = "Test " + DateTime.Now.ToString(), Discontinued = true };
            
            this.dbContext.Products.Add(product);  //新增一筆資料到記憶體

            this.dbContext.SaveChanges();  //更新資料到實體資料庫

            this.Read_RefreshDataGridView();
        }

        private void button56_Click(object sender, EventArgs e)
        {
            //update
            var product = (from p in this.dbContext.Products
                           where p.ProductName.Contains("Test")  //選取將要變更的資料
                           select p).FirstOrDefault();    //若找無資料不會exception則是傳回null

            if (product == null) return; //exit method

            product.ProductName = "Test" + product.ProductName;  //變更記憶體資料

            this.dbContext.SaveChanges();  //更新資料到實體資料庫

            this.Read_RefreshDataGridView();
        }

        private void button53_Click(object sender, EventArgs e)
        {
            //delete one product
            var product = (from p in this.dbContext.Products
                           where p.ProductName.Contains("Test")  //選取將要變更的資料
                           select p).FirstOrDefault();  //若找無資料不會exception則是傳回null

            if (product == null) return;

            this.dbContext.Products.Remove(product);  //刪除記憶體資料
            this.dbContext.SaveChanges();  //更新資料到實體資料庫

            this.Read_RefreshDataGridView();
        }
        void Read_RefreshDataGridView()  //Manage dataGridView1
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = this.dbContext.Products.ToList();

        }

        private void button14_Click(object sender, EventArgs e)
        {
            var q = from o in this.dbContext.Orders
                    group o by o.OrderDate.Value.Year into g  //o.OrderDate可為null，在DB設計中若有可為空值的數值型態會加上"?"，因此需.Value才能點到Year
                    select new
                    {
                        Year = g.Key,
                        Count = g.Count()
                    };
            this.dataGridView1.DataSource = q.ToList();
            //====================
            var q2 = from o in this.dbContext.Orders
                     group o by new { o.OrderDate.Value.Year, o.OrderDate.Value.Month } into g
                     select new { Year = g.Key, Count = g.Count() };

            this.dataGridView2.DataSource = q2.ToList();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            ////自訂 compare logic
            var q3 = dbContext.Products.AsEnumerable().OrderBy(p => p, new MyComparer()).ToList();
            this.dataGridView2.DataSource = q3.ToList();
        }
        class MyComparer : IComparer<Product>
        {
            public int Compare(Product x, Product y)
            {
                if (x.UnitPrice < y.UnitPrice)
                    return -1;
                else if (x.UnitPrice > y.UnitPrice)
                    return 1;
                else
                    return string.Compare(x.ProductName[0].ToString(), y.ProductName[0].ToString(), true);

            }
        }
    }
}
