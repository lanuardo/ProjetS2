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
using System.IO;



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
            string launcherPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            string musicPath = System.IO.Path.Combine(launcherPath, "sound/invasion_music_no_cpr2.wav");
            musicplayer = new System.Media.SoundPlayer();
            musicplayer.SoundLocation = musicPath;
            musicplayer.PlayLooping();


        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string gamePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            string buildPath = System.IO.Path.Combine(gamePath, "build/bui.exe");

            Process.Start(buildPath);
        }
        private void Website_Click(object sender, RoutedEventArgs e)
        {
            string launcherPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            string fullPath = System.IO.Path.Combine(launcherPath, "WebSite/index.html");

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
