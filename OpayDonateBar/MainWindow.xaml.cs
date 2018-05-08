using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpayDonateBar
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();  
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            //Setting w = new Setting();
            //w.Show();
        }

        private void Window_Load(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("Config"))
            {
                MessageBoxResult result = MessageBox.Show("設定檔不存在!!"); 
                if (result == MessageBoxResult.OK)
                {
                    this.Hide();
                    Setting w = new Setting();//視窗名稱 變數名稱 = new 視窗名稱();
                    w.Show();
                }
            }
        }

 
    }
}
