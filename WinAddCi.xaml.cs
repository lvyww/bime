using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace bime
{
    /// <summary>
    /// WinAddCi.xaml 的交互逻辑
    /// </summary>
    public partial class WinAddCi : Window
    {
        public WinAddCi()
        {
            InitializeComponent();
        }


        public static WinAddCi Current
        {
            get
            {
                foreach (var s in App.Current.Windows)
                {
                    if (s is WinAddCi)
                    {
                        return (WinAddCi)s;

                    }

                }

                return null;
            }

        }

        private int HistoryLen = 2;
        private void BtnCancle_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        public void InitText ()
        {
            HistoryLen = 2;
            if (HistoryLen > SendHistory.Count)
                HistoryLen = SendHistory.Count;
            TbName.Text = SendHistory.GetLastCi(HistoryLen);

            string s = MainWindow.Current.ConstructCi(TbName.Text);

                TbCode.Text = s;

            TbName.Focus();
            TbName.SelectAll();
        }
        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (TbCode.Text != "" && TbName.Text!= "")
            {
                MainWindow.Current.UserAdd( TbCode.Text, TbName.Text);
                this.Close();
            }
            else if (TbCode.Text == "")
            {
                TbMsg.Text = ("编码为空！");
            }
            else if (TbName.Text == "")
            {
                TbMsg.Text = ("词条为空！");
            }
            else
            {
                TbMsg.Text = ("添加失败！");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            if (WinCandidate.Current!=null && WinCandidate.Current.Opacity > 0.01)
            {
                this.Top = WinCandidate.Current.Top;
                this.Left = WinCandidate.Current.Left + WinCandidate.Current.Width;
            }
            else
            {
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;

                this.Top = desktopWorkingArea.Bottom / 2 - this.Height ;
                this.Left = desktopWorkingArea.Right / 2 - this.Width /2;
            }
            
        }

        private void Inc_Click(object sender, RoutedEventArgs e)
        {
            HistoryLen++;
            if (HistoryLen > SendHistory.Count)
                HistoryLen = SendHistory.Count;

            TbName.Text = SendHistory.GetLastCi(HistoryLen);



            TbName.Focus();
            TbName.SelectAll();

        }

        private void Dec_Click(object sender, RoutedEventArgs e)
        {
            HistoryLen--;
            if (HistoryLen < 0)
                HistoryLen = 0;

            TbName.Text = SendHistory.GetLastCi(HistoryLen);



            TbName.Focus();
            TbName.SelectAll();
        }

        private void TbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = MainWindow.Current.ConstructCi(TbName.Text);

  
                TbCode.Text = s;
        }

        private void BtnContinue_Click(object sender, RoutedEventArgs e)
        {
            if (TbCode.Text != "" && TbName.Text != "")
            {
                MainWindow.Current.UserAdd(TbCode.Text, TbName.Text);
                TbMsg.Text = "已添加：" + TbName.Text;

                TbName.Text = "";
                TbName.Focus();
                TbCode.Text = "";
                HistoryLen = 0;
            }
            else if (TbCode.Text == "")
            {
                TbMsg.Text = ("编码为空！");
            }
            else if (TbName.Text == "")
            {
                TbMsg.Text = ("词条为空！");
            }
            else
            {
                TbMsg.Text = ("添加失败！");
            }
        }
    }
}
