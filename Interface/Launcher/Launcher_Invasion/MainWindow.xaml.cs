using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Launcher_Invasion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Process.Start("path/Invasion.exe");
        }

        private void Website_Click(object sender, RoutedEventArgs e)
        {
            string htmlFilePath = "C:/Users/mcblo/Desktop/PROJET_S2/Launcher/Launcher_Invasion/WebSite/index.html";
            string fullPath = System.IO.Path.GetFullPath(htmlFilePath);
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c start {fullPath}",
                WindowStyle = ProcessWindowStyle.Normal
            };
            Process.Start(psi);
        }
    }
}
