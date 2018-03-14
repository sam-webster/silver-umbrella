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
using FestivalElements.Elements.Drawable;


namespace FestivalElements.Tools
{
    public abstract class Tools
    {
        public enum ToolType
        {
            CreateElement,
            Move,
            Select,
            ColourPick,
            Erase,
            ReColour
        }

        public static void Move(Canvas c, FestivalElement fe, MouseEventArgs e)
        {
            
        }
    }
}
