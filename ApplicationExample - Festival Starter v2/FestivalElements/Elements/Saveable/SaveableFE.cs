using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FestivalElements.Elements.Drawable;
using System.Windows.Media;

namespace FestivalElements.Elements.Saveable
{ 
    public class SaveableFE
    {
        public string FEType, Name;
        public double X, Y, ScaleX, ScaleY, Angle;
        public byte A, R, G, B;

        public SaveableFE()
        {
                
        }

        public SaveableFE(FestivalElement fe)
        {
            FEType = fe.FEType;
            Name = fe.Name;
            X = fe.X;
            Y = fe.Y;
            ScaleX = fe.ScaleX;
            ScaleY = fe.ScaleY;
            Angle = fe.Angle;
            A = (fe.BGBrush as SolidColorBrush).Color.A;
            R = (fe.BGBrush as SolidColorBrush).Color.R;
            G = (fe.BGBrush as SolidColorBrush).Color.G;
            B = (fe.BGBrush as SolidColorBrush).Color.B;
        }
    }
}
