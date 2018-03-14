using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using FestivalElements.Interfaces;

namespace FestivalElements.Elements.Drawable
{
    public class Square : FestivalElement, ICountable
    {
        public override bool IsPreview
        {
            set
            {
                _IsPreview = value;
                if (value)
                {
                    _NextElementID--;
                }
            }
        }
        private static long _NextElementID, _NumberOfElement;
        long ICountable.NextElementID
        {
            get { return _NextElementID; }
        }

        long ICountable.NumberOfElement
        {
            get { return _NumberOfElement; }
        }
        void ICountable.ResetElementCount()
        {
            _NumberOfElement = 0;
        }

        public Square()
        {
            using (StreamGeometryContext geometryContext = base.BGGeometry.Open())
            {
                geometryContext.BeginFigure(new Point(-16.0d, -16.0d), true, true);

                pc = new PointCollection();
                pc.Add(new Point(16.0d, -16.0d));
                pc.Add(new Point(16.0d, 16.0d));
                pc.Add(new Point(-16.0d, 16.0d));

                geometryContext.PolyLineTo(pc, true, true);
            }

            _NextElementID++;
            name = "Square " + _NextElementID;
            _FEType = "Square";
        }

        public Square(Brush nColour, double nScaleX, double nScaleY) : base(nColour, nScaleX, nScaleY)
        {
            using (StreamGeometryContext geometryContext = base.BGGeometry.Open())
            {
                geometryContext.BeginFigure(new Point(-16.0d, -16.0d), true, true);

                pc = new PointCollection();
                pc.Add(new Point(16.0d, -16.0d));
                pc.Add(new Point(16.0d, 16.0d));
                pc.Add(new Point(-16.0d, 16.0d));

                geometryContext.PolyLineTo(pc, true, true);
            }
            
            _NextElementID++;
            name = "Square " + _NextElementID;
            _FEType = "Square";
        }

    }
}
