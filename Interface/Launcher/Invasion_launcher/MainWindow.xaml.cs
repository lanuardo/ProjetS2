using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Invasion_launcher
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {
        private bool isMuted = false;

        private System.Media.SoundPlayer musicplayer;
        public MainWindow()
        {
            InitializeComponent();

            musicplayer = new System.Media.SoundPlayer();
            musicplayer.SoundLocation = "C:/Users/mcblo/Downloads/invasion_music2.wav";
            musicplayer.PlayLooping();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Process.Start("C:/Users/alexl/epita/projets2/ProjetS2/My project/Build/Invasion.exe");
        }
        private void Website_Click(object sender, RoutedEventArgs e)
        {
            string htmlFilePath = "C:/Users/alexl/epita/projets2/ProjetS2/Site/WebSite/index.html";
            string fullPath = System.IO.Path.GetFullPath(htmlFilePath);
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c start {fullPath}",
                WindowStyle = ProcessWindowStyle.Normal
            };
            Process.Start(psi);
        }


        private void SoundMute_Click(object sender, RoutedEventArgs e)
        {
            if(musicplayer != null)
            {
                if (isMuted)
                {
                    // Activer le son
                    musicplayer.Play();
                    isMuted = false;
                }
                else
                {
                    // Couper le son
                    musicplayer.Stop();
                    isMuted = true;
                }
            }
        }
    }
}
