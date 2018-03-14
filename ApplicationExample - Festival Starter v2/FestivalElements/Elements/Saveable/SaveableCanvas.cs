using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace FestivalElements.Elements.Saveable
{
    public class SaveableCanvas
    {
        public double CanvasWidth, CanvasHeight, XPos, YPos, Zoom;
        public byte A, R, G, B;

        public SaveableCanvas()
        {

        }

        public SaveableCanvas(Canvas canvas)
        {
            CanvasWidth = canvas.Width;
            CanvasHeight = canvas.Height;
            XPos = 0;
            YPos = 0;
            Zoom = 1;
            A = (canvas.Background as SolidColorBrush).Color.A;
            R = (canvas.Background as SolidColorBrush).Color.R;
            G = (canvas.Background as SolidColorBrush).Color.G;
            B = (canvas.Background as SolidColorBrush).Color.B;
        }
    }
}
