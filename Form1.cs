using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Play2048
{
    public partial class Form1 : Form
    {
        int BoardBorder = 0;

        int BoardLineNum = 5;
        int BoardWidth = 70;

        int[] status={0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; i++)
                status[i] = 0;
            init();
            DrawBoard(BoardLineNum, BoardWidth);
        }

        public void init()
        {
            int[] loc = GetRandom(16, 3);
            status[loc[0]] = 2;
            status[loc[1]] = 2;
            status[loc[2]] = 4;
        }

        public void ChangeStatus(int o)
        {
            switch (o)
            {
                case 0://上
                    
                    for (int i = 0; i < 4; i++)
                    {
                        int[] a = { status[12 + i], status[8 + i], status[4 + i], status[i] };
                        a = adjustarray(a);
                        status[i] = a[3];
                        status[4 + i] = a[2];
                        status[8 + i] = a[1];
                        status[12 + i] = a[0];
                    }
                    break;
                case 1://下
                    for (int i = 0; i < 4; i++)
                    {
                        int[] a = { status[i], status[4 + i], status[8 + i], status[12 + i] };
                        a = adjustarray(a);
                        status[i] = a[0];
                        status[4 + i] = a[1];
                        status[8 + i] = a[2];
                        status[12 + i] = a[3];
                    }
                    break;
                case 2://左
                    for (int i = 0; i < 4; i++)
                    {
                        int[] a = { status[4 * i+3], status[4 * i + 2], status[4 * i +1], status[4 * i] };
                        a = adjustarray(a);
                        status[4 * i + 3] = a[0];
                        status[4 * i + 2] = a[1];
                        status[4 * i + 1] = a[2];
                        status[4 * i] = a[3];
                    }
                    break;
                case 3://右
                    for (int i = 0; i < 4; i++)
                    {
                        int[] a = { status[4 * i], status[4 * i + 1], status[4 * i + 2], status[4 * i + 3] };
                        a = adjustarray(a);
                        status[4 * i + 3] = a[3];
                        status[4 * i + 2] = a[2];
                        status[4 * i + 1] = a[1];
                        status[4 * i] = a[0];
                    }
                    break;
                default:
                    break;
            }
        }

        public int[] adjustarray(int[] x)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 2; j >=0; j--)
                {
                    if (x[j + 1] == 0 && x[j]!=0)
                    {
                        x[j + 1] = x[j];
                        x[j] = 0;
                    }
                }
            }
            for (int j = 2; j >=0; j--)
            {
                if (x[j + 1] ==  x[j])
                {
                    x[j + 1] = 2* x[j];
                    x[j] = 0;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 2; j >= 0; j--)
                {
                    if (x[j + 1] == 0 && x[j] != 0)
                    {
                        x[j + 1] = x[j];
                        x[j] = 0;
                    }
                }
            }
            return x;
        }


        public int[] GetRandom(int max, int count)
        {

            int[] index = new int[max];
            for (int i = 0; i < max; i++)
                index[i] = i;
            Random r = new Random();
            //用来保存随机生成的不重复的数 
            int[] result = new int[count];
            int site = max;//设置上限 
            int id;
            for (int j = 0; j < count; j++)
            {
                id = r.Next(0, site - 1);
                //在随机位置取出一个数，保存到结果数组 
                result[j] = index[id];
                //最后一个数复制到当前位置 
                index[id] = index[site - 1];
                //位置的上限减少一 
                site--;
            }

            return result;
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            int[] olds = (int[])status.Clone();
            if (keyData == Keys.Up)
            {
                ChangeStatus(0);
            }
            if (keyData == Keys.Down)
            {
                ChangeStatus(1);
            }
            if (keyData == Keys.Left)
            {
                ChangeStatus(2);
            }
            if (keyData == Keys.Right)
            {
                ChangeStatus(3);
            }
            if (!compareArr(olds,status))
            {
                AddNew();
                DrawBoard(BoardLineNum, BoardWidth);
            }
            return true;
            //return base.ProcessCmdKey(ref msg, keyData);
        }

        public static bool compareArr(int[] arr1, int[] arr2)//比较前后
        {
            for (int i = 0; i < 16; i++)
                if (arr1[i] != arr2[i])
                    return false;
            return true;

        }

        public void AddNew()//随机生成新数字
        {
            int num = 0;
            for (int i = 0; i < 16; i++)
                if (status[i] == 0)
                    num++;
            if (num == 0)
                return;
            int p=GetRandom(num, 1)[0];
            for (int i = 0; i < 16; i++)
                if (status[i] == 0)
                {
                    if (p == 0)
                    {
                        if (GetRandom(100, 1)[0] < 10)//生成4的概率
                            status[i] = 4;
                        else
                            status[i] = 2;
                        return;
                    }
                    p--;
                }

                    
        }

        public int SearchNext()//寻找最优解
        {
            int[] oldstatus = status;
            int upoint=0, dpoint=0, lpoint=0, rpoint=0;
            ChangeStatus(0);
            if (compareArr(oldstatus, status))
            {
                upoint = StatusValue(status);
            }
            
            status = oldstatus;
            ChangeStatus(1);
            if (compareArr(oldstatus, status))
            {
                dpoint = StatusValue(status);
            }
            status = oldstatus;
            ChangeStatus(2);
            if (compareArr(oldstatus, status))
            {
                lpoint = StatusValue(status);
            }
            status = oldstatus;
            ChangeStatus(3);
            if (compareArr(oldstatus, status))
            {
                rpoint = StatusValue(status);
            }
            int maxindex, maxpoint;
            if (dpoint < upoint)
            {
                maxindex = 0; maxpoint = upoint; 
            }
            else
            {
                maxindex = 1; maxpoint = dpoint;
            }
            if (lpoint > maxpoint)
            {
                maxindex = 2; maxpoint = lpoint;
            }
            if (rpoint > maxpoint)
            {
                maxindex = 3; maxpoint = rpoint;
            }
            return maxindex;
        }

        public int StatusValue(int[] x)//评价当前局面
        {
            int point =0;
            for (int i = 0; i < 16; i++)
                if (x[i] == 0)
                    point += 100;//每个空格100分
            //尽量将大数字放入左下角


            return point;
        }

        public void DrawBoard(int linenum, int gap)//显示当前的局面
        {
            //行数-1
            int horC = linenum - 1;
            Pen pen = new Pen(Color.Black, 1);
            Image img = new Bitmap(horC * gap + BoardBorder * 2+1, horC * gap + BoardBorder * 2+1);
            Graphics gra = Graphics.FromImage(img);
            gra = Graphics.FromImage(img);
            gra.Clear(Color.White);
            gra.DrawRectangle(pen, BoardBorder, BoardBorder, horC * gap, horC * gap);
            for (int i = 0; i < horC; i++)
            {
                gra.DrawLine(pen, BoardBorder, i * gap + BoardBorder, horC * gap + BoardBorder, i * gap + BoardBorder);
                gra.DrawLine(pen, i * gap + BoardBorder, BoardBorder, i * gap + BoardBorder, horC * gap + BoardBorder);
            }
            for (int i = 0; i < 16; i++)
            {
                if (status[i] != 0)
                {
                    String drawString = status[i].ToString();
                    Font drawFont = new Font("Arial", 16);
                    SolidBrush drawBrush = new SolidBrush(Color.Black);
                    int newX = (int)BoardBorder + ((((i % 4)) * BoardWidth + BoardWidth / 2) / BoardWidth) * BoardWidth + 50-drawString.Length * 12;
                    int newY = (int)BoardBorder + ((((i / 4)) * BoardWidth + BoardWidth / 2) / BoardWidth) * BoardWidth + BoardWidth / 3;
                    gra.DrawString(drawString, drawFont, drawBrush, newX, newY);
                }
            }
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.Image = img;
            gra.Dispose();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 1200; i++)
            {
                ChangeStatus(SearchNext());
                AddNew();
                DrawBoard(BoardLineNum, BoardWidth);
                //int[] olds = (int[])status.Clone();
                //ChangeStatus(1);
                //if (compareArr(olds, status))
                //{
                //    ChangeStatus(2);
                //    if (compareArr(olds, status))
                //    {
                //        ChangeStatus(3);
                //        if (compareArr(olds, status))
                //        {
                //            ChangeStatus(0);
                //            if (compareArr(olds, status))
                //            {
                //                MessageBox.Show("结束");
                //                break;
                //            }
                //            else
                //            {
                //                AddNew();
                //                DrawBoard(BoardLineNum, BoardWidth);
                //                continue;
                //            }
                //        }
                //        else
                //        {
                //            AddNew();
                //            DrawBoard(BoardLineNum, BoardWidth);
                //            continue;
                //        }
                //    }
                //    else
                //    {
                //        AddNew();
                //        DrawBoard(BoardLineNum, BoardWidth);
                //        continue;
                //    }
                //}
                //else
                //{
                //    AddNew();
                //    DrawBoard(BoardLineNum, BoardWidth);
                //    continue;
                //}

            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            init();
            DrawBoard(BoardLineNum, BoardWidth);
            
        }

    }
}
