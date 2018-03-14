using FestivalElements.Elements.Saveable;
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
using System.Xml.Serialization;

namespace FestivalElements.Elements.Drawable
{
    /// <summary>
    /// this class has been adapted from the source code we were given by David Gee at the start of the project
    /// 
    /// for subclass comments look at BeerTent
    /// </summary>
    public abstract class FestivalElement : UIElement
    {
        public enum FestivalElementType
        {
            None,
            Square,
            Triangle,
            Bush,
            FoodTent,
            BeerTent,
            Toilet,
            Stage,
            CampingZone,
            FairGround,
            Bin,
            Shop
        }

        // I have used encapsulation by making the fields private/protected 
        // and there are properties with get/set methods for when they are changed outside of the class.
        protected string _FEType;

        public string FEType
        {
            get { return _FEType; }
        }

        protected int _ListBoxID;

        public int ListBoxID
        {
            get { return _ListBoxID; }
            set { _ListBoxID = value; }
        }

        protected bool _IsPreview;
        public abstract bool IsPreview
        {
            set;
        }

        protected StreamGeometry _BGGeometry, _IconGeometry; // geometries for element background and icon

        public StreamGeometry BGGeometry
        {
            get { return _BGGeometry; }
            set { _BGGeometry = value; }
        }

        public StreamGeometry IconGeometry
        {
            get { return _IconGeometry; }
            set { _IconGeometry = value; }
        }
        protected PointCollection pc, pc2;
        protected Pen _Outline; // outline colour
        protected Brush _BGBrush, _IconBrush; // brushes for element background and icon

        public Brush BGBrush
        {
            get { return _BGBrush; }
            set { _BGBrush = value; }
        }

        public Brush IconBrush
        {
            get { return _IconBrush; }
            set { _IconBrush = value; }
        }

        protected String name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        protected double _ScaleX, _ScaleY, _Angle, _X, _Y;

        public double ScaleX
        {
            get { return _ScaleX; }
            set { _ScaleX = value; }
        }

        public double ScaleY
        {
            get { return _ScaleY; }
            set { _ScaleY = value; }
        }

        public double Angle
        {
            get { return _Angle; }
            set { _Angle = value; }
        }

        public double X
        {
            get { return _X; }
            set { _X = value; }
        }

        public double Y
        {
            get { return _Y; }
            set { _Y = value; }
        }

        protected Boolean _Selected;

        public FestivalElement()
        {
            _BGBrush = Brushes.Black;
            _ScaleX = 1;
            _ScaleY = 1;
            _Outline = new Pen(Brushes.Black, 1);
            _BGGeometry = new StreamGeometry();
            _IconGeometry = new StreamGeometry();

            name = "";
        }
        /// <summary>
        /// constructor method for FestivalElement
        /// </summary>
        /// <param name="nColour"></param>
        /// <param name="nScaleX"></param>
        /// <param name="nScaleY"></param>
        public FestivalElement(Brush nColour, double nScaleX, double nScaleY)
        {
            _ScaleX = nScaleX;
            _ScaleY = nScaleY;
            _Outline = new Pen(Brushes.Black, 1);
            _BGBrush = nColour;

            _BGGeometry = new StreamGeometry();
            _IconGeometry = new StreamGeometry();

            name = "";
        }

        protected override void OnRender(DrawingContext dc)
        {
            this.Render(dc);
        }

        protected virtual void Render(DrawingContext dc)
        {
            dc.DrawGeometry(_BGBrush, _Outline, _BGGeometry);
            dc.DrawGeometry(_IconBrush, new Pen(Brushes.Transparent, 1), _IconGeometry);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            // add action here
            _Outline = new Pen(Brushes.White, 1);
            InvalidateVisual();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (!_Selected)
            {
                _Outline = new Pen(Brushes.Black, 1);
            }
            else
            {
                // different outline if element is selected
                _Outline = new Pen(Brushes.White, 2);
            }
            InvalidateVisual();

            base.OnMouseLeave(e);
        }
        /// <summary>
        /// needs to be run so user can see which element is selected
        /// </summary>
        public void OnSelection()
        {
            _Selected = true;
            _Outline = new Pen(Brushes.White, 2);
            InvalidateVisual();
        }
        /// <summary>
        /// needs to be run so user can see which element is deselected
        /// </summary>
        public void OnDeSelection()
        {
            _Selected = false;
            _Outline = new Pen(Brushes.Black, 1);
            InvalidateVisual();
        }
        /// <summary>
        /// this method gets the compound transformation of a festival element using the position, rotation and scale data
        /// </summary>
        /// <returns>the transform for this element</returns>
        public TransformGroup GetTransform()
        {
            TransformGroup t = new TransformGroup();
            t.Children.Add(new TranslateTransform(_X, _Y));
            t.Children.Add(new ScaleTransform(_ScaleX, _ScaleY, _X, _Y));
            t.Children.Add(new RotateTransform(_Angle, _X, _Y));
            return t;
        }
        /// <summary>
        /// reload data from a saveableFE
        /// </summary>
        /// <param name="saveFE"></param>
        public void LoadData(SaveableFE saveFE)
        {
            _X = saveFE.X;
            _Y = saveFE.Y;
            _ScaleX = saveFE.ScaleX;
            _ScaleY = saveFE.ScaleY;
            _Angle = saveFE.Angle;
            name = saveFE.Name;
            _BGBrush = new SolidColorBrush(Color.FromArgb(saveFE.A, saveFE.R, saveFE.G, saveFE.B));
        }
    }
}
