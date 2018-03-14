using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FestivalElements.Interfaces;

namespace FestivalElements.Elements.Drawable
{
    public class BeerTent : FestivalElement, ICountable
    {
        public override bool IsPreview
        {
            set
            {
                _IsPreview = value;
                if (value)
                {
                    _NextElementID--; // revert incrementation
                }
            }
        }
        private static long _NextElementID, _NumberOfElement; // static to subclass rather than FestivalElement to make names better
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

        public BeerTent()
        {
            // geometry for this element's background
            using (StreamGeometryContext geometryContext = base.BGGeometry.Open())
            {
                geometryContext.BeginFigure(new Point(-16.0d, -16.0d), true, true);

                pc = new PointCollection();
                pc.Add(new Point(16.0d, -16.0d));
                pc.Add(new Point(16.0d, 16.0d));
                pc.Add(new Point(-16.0d, 16.0d));

                geometryContext.PolyLineTo(pc, true, true);
            }

            // geometry for the icon of this element
            using (StreamGeometryContext geometryContext2 = base.IconGeometry.Open())
            {
                geometryContext2.BeginFigure(new Point(-16.0d, -16.0d), true, true);

                pc2 = new PointCollection();
                pc2.Add(new Point(16.0d, -16.0d));
                pc2.Add(new Point(16.0d, 16.0d));
                pc2.Add(new Point(-16.0d, 16.0d));

                geometryContext2.PolyLineTo(pc, true, true);
            }
            string uri = "pack://application:,,,/Assets/AlcoholIcon.png"; // image location of icon
            _NextElementID++;
            name = "BeerTent " + _NextElementID; //  sets element name
            _FEType = "BeerTent"; // type used for Save/ Load

            ImageSource imageSource = new ImageSourceConverter().ConvertFromString(uri) as ImageSource;
            _IconBrush = new ImageBrush(new BitmapImage(new Uri(imageSource.ToString())));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nColour"></param>
        /// <param name="nScaleX"></param>
        /// <param name="nScaleY"></param>
        public BeerTent(Brush nColour, double nScaleX, double nScaleY) : base(nColour, nScaleX, nScaleY)
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
            using (StreamGeometryContext geometryContext2 = base.IconGeometry.Open())
            {
                geometryContext2.BeginFigure(new Point(-16.0d, -16.0d), true, true);

                pc2 = new PointCollection();
                pc2.Add(new Point(16.0d, -16.0d));
                pc2.Add(new Point(16.0d, 16.0d));
                pc2.Add(new Point(-16.0d, 16.0d));

                geometryContext2.PolyLineTo(pc, true, true);
            }
            string uri = "pack://application:,,,/Assets/AlcoholIcon.png";
            _NextElementID++;
            name = "BeerTent " + _NextElementID;
            _FEType = "BeerTent";

            ImageSource imageSource = new ImageSourceConverter().ConvertFromString(uri) as ImageSource;
            _IconBrush = new ImageBrush(new BitmapImage(new Uri(imageSource.ToString())));
        }

    }
}
