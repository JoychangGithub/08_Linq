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
    public partial class FrmLangForLINQ : Form
    {
        public FrmLangForLINQ()
        {
            InitializeComponent();
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int n1, n2;
            n1 = 100;
            n2 = 200;
            MessageBox.Show(n1 + ", " + n2);
            Swap(ref n1, ref n2);
            MessageBox.Show(n1 + ", " + n2);
            //==========================
            string s1, s2;
            s1 = "aaa";
            s2 = "bbb";
            MessageBox.Show(s1 + ", " + s2);
            Swap(ref s1, ref s2);
            MessageBox.Show(s1 + ", " + s2);
        }

        private void Swap(ref int a, ref int b) //傳址法
        {
            int x = a;
            a = b;
            b =x;
        }
        private void Swap(ref string s1, ref string s2)
        {
            string x = s1;
            s1 = s2;
            s2 = x;
        }
        private void Swap(ref Point p1, ref Point p2)
        {
            Point x = p1;
            p1 = p2;
            p2 = x;
        }

        void SwapObject(ref object n1, ref object n2)
        {
            object x = n2;
            n2 = n1;
            n1 = x;
        }

        private static void SwapAnyType<T>(ref T n1, ref T n2)
        {
            T x = n1;
            n1 = n2;
            n2 = x;
        }

        private void button5_Click(object sender, EventArgs e)
        {

            //object n1, n2;
            //n1 = "aaa";
            //n2 = "bbb";
            //Swap(ref object n1, ref object n2);
            //MessageBox.Show(n1 + ", " + n2);
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string n1, n2;
            n1 = "aaa";
            n2 = "bbb";
            MessageBox.Show(n1 + ", " + n2);
            SwapAnyType<string>(ref  n1, ref n2);  //寫法1
            //SwapAnyType(ref n1, ref n2);  //寫法2: 可省略<string>
            MessageBox.Show(n1 + ", " + n2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.buttonX.Click += buttonX_Click; //註冊事件  //Click事件的委派EventHandler
            //Click事件須符合此委派(有2個參數) public delegate void EventHandler(object sender, EventArgs e);

            //註冊多種方法
            //C# 1.0:具名方法
            this.buttonX.Click += new EventHandler(aaa);  //寫法1
            this.buttonX.Click += bbb; //寫法2

            //C# 2.0:匿名方法
            this.buttonX.Click += delegate (object sender1, EventArgs e1) //不與上方重複命名為sender1, e1
            {
                MessageBox.Show("C# 2.0:匿名方法");
            };

            //C# 3.0:匿名方法 lambda運算式 => goes to
            this.buttonX.Click += (object sender1, EventArgs e1) =>
            {
                MessageBox.Show("C# 3.0:匿名方法 lambda運算式");
            };
        }

        private void bbb(object sender, EventArgs e)
        {
            MessageBox.Show("bbb");
        }

        private void aaa(object sender, EventArgs e)
        {
            MessageBox.Show("aaa");
        }

        private void buttonX_Click(object sender, EventArgs e)
        {
            MessageBox.Show("buttonX_Click");
        }


        //[委派方法]:
        // step1: creat delegate型別   step2: create object (new...)  step3: call method

        delegate bool MyDelegate(int n);  //step1: creat delegate型別  //委派方法的參數為int n，call的方法的參數也須為int n
        private void button9_Click(object sender, EventArgs e)
        {
            //過去寫法
            //bool result = Test(6);
            //MessageBox.Show("result= "+ result);
            //=============================
            //委派寫法: 方法1
            bool result = Test(6);  //Test方法
            MyDelegate delegateObj = new MyDelegate(Test);  //step2: create object (new...)
            result = delegateObj(7);   //step3: call method

            MessageBox.Show("result= " + result);

            //委派寫法: 方法2
            delegateObj = Test1;  //語法糖
            result = delegateObj(3);
            MessageBox.Show("result= " + result);

            //委派寫法: 方法3: c#2.0匿名方法
            delegateObj = delegate (int n)
            {
                return n > 5;
            };
            result = delegateObj(6);
            MessageBox.Show("result= " + result);

            //委派寫法: 方法4: c#3.0匿名方法 lambda運算式=>goes to
            delegateObj = n => n > 5;
            result = delegateObj(1);
            MessageBox.Show("result= " + result);
        }
        bool Test(int n) 
        {
            return n > 5;   //if n>5 return true
        }
        bool Test1(int n)
        {
            return n %2 == 0;   //判斷是否為偶數
        }


        List<int> Mywhere(int[] nums, MyDelegate delegateObj) 
        {
            List<int> list = new List<int>();

            foreach (int n in nums) 
            {
                if (delegateObj(n)) 
                {
                        list.Add(n);                   
                }
            }
            return list;    //List<int>搭配list
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            List<int> Large_list = Mywhere(nums, Test);  //符合Mydelegate的委派方法皆可當參數傳入使用
            foreach (int n in Large_list) 
            {
                this.listBox1.Items.Add(n);
            }

            //c#3.0 簡潔版方法
            List<int> list1 = Mywhere(nums, n => n > 5);
            List<int> oddList = Mywhere(nums, n => n%2==1);
            List<int> evenList = Mywhere(nums, n => n % 2 == 0);
            foreach (int n in list1)
            {
                this.listBox1.Items.Add(n);
            }
            foreach (int n in oddList)
            {
                this.listBox2.Items.Add(n);
            }
        }

        //[yield return]
       IEnumerable<int> MyIterator(int[] nums, MyDelegate delegateObj)
        {
            foreach (int n in nums)
            {
                if (delegateObj(n))  //call method
                {
                    yield return n;   //IEnumerable<int>搭配yield return

                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            IEnumerable<int> q = MyIterator(nums, n => n > 5);  //定義方法n => n > 5
            foreach (int n in q)
            {
                this.listBox1.Items.Add(n);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //整數例子
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            IEnumerable<int> q = nums.Where(n => n > 5);  //call "n => n > 5"方法判斷
            foreach (int n in q)
            {
                this.listBox1.Items.Add(n);
            }

            //====================================
            //字串例子
            string[] words = { "aaa", "bbbb", "ccccc" };
            IEnumerable<string> q1 = words.Where(w => w.Length > 5);  //定義判斷方法
            foreach (string n in q1)
            {
                this.listBox2.Items.Add(n);
            }
            //====================================
            //北風產品價格例子

            this.productsTableAdapter1.Fill(this.nwDataSet11.Products);

            //var q = from p in this.nwDataSet11.Products;

            var q2 = this.nwDataSet11.Products.Where(p=>p.UnitPrice>30);
            this.dataGridView1.DataSource = q2.ToList();

        }

        private void button45_Click(object sender, EventArgs e)
        {
            //var的用法:(1)需有值,  (2)用於匿名型別,  (3)不可用於方法外面
            var n = 100;
            var s = "abs";
            var p = new Point(100, 100);
            //var x;  //無給值判斷:錯誤

            MessageBox.Show(s.ToUpper());
            MessageBox.Show(p.X +", "+p.Y);
        }

        private void button41_Click(object sender, EventArgs e)
        {
            Mypoint pt1 = new Mypoint();  //建構子方法
            pt1.P1 = 100;   //set
            int w = pt1.P1;  //get

            MessageBox.Show(pt1.P1.ToString());

            //=================================
            List<Mypoint> list = new List<Mypoint>();
            //list.Add(new Mypoint());
            //list.Add(new Mypoint(100));
            //list.Add(new Mypoint(100, 200));
            //list.Add(new Mypoint("aaa"));  //為0，因為不為屬性
            //this.dataGridView1.DataSource = list;

            //=================================
            //物件初始化{}
            list.Add(new Mypoint { P1 = 1, P2 = 1, field1 = "aaa", field2 = "bbb" });
            list.Add(new Mypoint { P1 = 1, field1 = "aaa", field2 = "bbb" });
            list.Add(new Mypoint { P1 = 1, field2 = "bbb" });
            this.dataGridView1.DataSource = list;

            //===================================
            //物件初始化:集合初始化
            List<Mypoint> list2 = new List<Mypoint>
            {
                new Mypoint{ P1=1, P2=1, field1="aaa", field2="bbb"},
                new Mypoint{ P1=1, P2=1, field1="aaa", field2="bbb"},
                new Mypoint{ P1=1, P2=1, field1="aaa", field2="bbb"}
            };
            this.dataGridView2.DataSource = list2;
        }

        private void button43_Click(object sender, EventArgs e)
        {
            var x = new { P1 = 99, P2 = 88, P3 = 33 };    //x為匿名型別
            var y = new { UserName = "aaa", Password = "bbb" };   //x為匿名型別
            this.listBox1.Items.Add(x.GetType());  //獲得型別
            this.listBox1.Items.Add(y.GetType());

            //========================================
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var q = from n in nums
                    where n > 5
                    select new { N = n, Squar = n * n, Cube = n * n * n };

            this.dataGridView1.DataSource = q.ToList();


            var q1 = nums.Where(n => n > 5).Select(n => new { N = n, Squar = n * n, Cube = n * n * n });
            this.dataGridView1.DataSource = q1.ToList();

            //========================================

            this.productsTableAdapter1.Fill(this.nwDataSet11.Products);
            var q2 = from p in this.nwDataSet11.Products
                     where p.UnitPrice > 30
                     select new
                     {
                         ID = p.ProductID,
                         產品名稱 = p.ProductName,
                         p.UnitPrice,
                         p.UnitsInStock,
                         TotalPrice = p.UnitPrice * p.UnitsInStock
                     };
            this.dataGridView2.DataSource = q2.ToList();
        }

        private void button32_Click(object sender, EventArgs e)
        {
            string s1 = "abcd";
            int n = s1.WordCount();
            MessageBox.Show("wordCount= " + n);


            string s2 = "123456789"; //寫法1:最傳神
            n = s2.WordCount();
            //n = MyStringExtend.WordCount(s2); //寫法2
            MessageBox.Show("wordCount= " + n);

            char ch = s2.Chars(3);//寫法1:最傳神
            //char ch = MyStringExtend.Chars(s2, 3); //寫法2
            MessageBox.Show("ch= " + ch);
        }

    }
    public class Mypoint
    {
        private int m_p1;  //私有變數

        public string field1 = "xxx";
        public string field2 = "yyy";
        public int P1   //屬性方法(1)
        {
            get
            {
                return m_p1;  //屬性set常搭被私有變數
            }
            set
            {
                m_p1 = value;    //傳入值value
            }
        }
        public int P2 { get; set; } //屬性方法(2)

        public Mypoint() //建構子方法(1)
        {

        }
        public Mypoint(int p1) //建構子方法(2)
        {
            this.P1 = p1;   //this: 某一個Mypoint物件call此方法，  P1為屬性方法，此時才可看到表格內容，因為表格欄位為屬性
        }
        public Mypoint(int p1, int p2) //建構子方法(3)
        {
            this.P1 = p1;  //this: 某一個Mypoint物件call此方法，  P1為屬性方法
            this.P2 = p2;
        }
        public Mypoint(string field1) //建構子方法(3)
        {

        }
    }

    public static class MyStringExtend  //擴充方法比須為靜態類別
    {
        public static int WordCount(this string s)  //擴充方法比須為靜態方法
        {
            return s.Length;
        }
        public static char Chars(this string s, int index) //擴充方法比須為靜態方法
        {
            return s[index];

            // public char this[int index] { get; }
        }

    }
}
