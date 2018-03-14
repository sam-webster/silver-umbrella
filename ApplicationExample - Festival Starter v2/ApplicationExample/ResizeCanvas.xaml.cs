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

namespace ApplicationExample
{
    /// <summary>
    /// Interaction logic for ResizeCanvas.xaml
    /// </summary>
    public partial class ResizeCanvas : Window
    {
        private double cWidth, cHeight;
        private MainWindow mainWindow;
        public ResizeCanvas(double canvasWidth, double canvasHeight, MainWindow main)
        {
            InitializeComponent();
            cWidth = canvasWidth;
            cHeight = canvasHeight;
            textBoxCanvasWidth.Text = cWidth.ToString();
            textBoxCanvasHeight.Text = cHeight.ToString();
            mainWindow = main;
        }
        /// <summary>
        /// updates the canvas size in the main window then closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (Double.TryParse(textBoxCanvasWidth.Text, out cWidth))
            {
                if (Double.TryParse(textBoxCanvasHeight.Text, out cHeight))
                {
                    mainWindow.SetNewCanvasSize(cWidth, cHeight);
                }
            }

        }
    }
}
