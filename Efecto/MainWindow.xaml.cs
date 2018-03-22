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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Win32;

using NAudio.Wave;

namespace Efecto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WaveOutEvent waveOut;
        AudioFileReader reader;
        Efecto1 efectoProvider;
        Delay delayProvider;
        DelayAtenuado delayAtenuadoProvider;

        public MainWindow()
        {
            InitializeComponent();
            waveOut = new WaveOutEvent();
        }

        private void btnExaminar_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if ((bool) fileDialog.ShowDialog())
            {
                txtRuta.Text = fileDialog.FileName;
                reader = new AudioFileReader(fileDialog.FileName);
            }
        }

        private void btnReproducir_Click(object sender, RoutedEventArgs e)
        {
            if (waveOut != null)
            {
                if (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    waveOut.Stop();
                }
                waveOut.Init(reader);
                waveOut.Play();
                sldFactor.IsEnabled = true;
            }

        }

        private void btnEfecto_Click(object sender, RoutedEventArgs e)
        {
            if (waveOut != null && reader != null)
            {
                if (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    waveOut.Stop();
                }
                efectoProvider = new Efecto1(reader, (float)sldFactor.Value);
                waveOut.Init(efectoProvider);
                waveOut.Play();
            }
        }

        private void sldFactor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (efectoProvider != null)
            {
                efectoProvider.Factor = (float)sldFactor.Value;
            }
        }

        private void btnDelay_Click(object sender, RoutedEventArgs e)
        {
            if (waveOut != null && reader != null)
            {
                if (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    waveOut.Stop();
                }
                delayProvider = new Delay(reader);
                waveOut.Init(delayProvider);
                waveOut.Play();
            }
        }

        private void sldDelay_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (waveOut != null && reader != null)
            {
                delayAtenuadoProvider.OffsetTiempoMS = (int)sldDelay.Value; ;
            }
        }

        private void btnDelayAtenuado_Click(object sender, RoutedEventArgs e)
        {
            if (waveOut != null && reader != null)
            {
                if (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    waveOut.Stop();
                }
                delayAtenuadoProvider = new DelayAtenuado(reader, (int)sldDelay.Value, (float)sldAtenuante.Value);
                waveOut.Init(delayAtenuadoProvider);
                waveOut.Play();
                sldAtenuante.IsEnabled = true;
                sldDelay.IsEnabled = true;
            }
        }

        private void sldAtenuante_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (waveOut != null && reader != null)
            {
                delayAtenuadoProvider.Factor = (float)sldAtenuante.Value; ;
            }
        }
    }
}
