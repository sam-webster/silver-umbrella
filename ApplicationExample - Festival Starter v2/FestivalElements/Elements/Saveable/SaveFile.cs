using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using FestivalElements.Elements.Drawable;

namespace FestivalElements.Elements.Saveable
{
    public class SaveFile
    {
        // public List<object> saveObjects;
        public SaveableCanvas saveCanvas;
        public List<SaveableFE> saveElements;
        public SaveFile()
        {

        }
        public SaveFile(Canvas c)
        {
            //saveObjects = new List<object>();
            saveElements = new List<SaveableFE>();
            foreach(FestivalElement child in c.Children)
            {
                SaveableFE saveFE = new SaveableFE(child);
                saveElements.Add(saveFE);
            }
            saveCanvas = new SaveableCanvas(c);
            //saveObjects.Add(new SaveableCanvas(c));
            //saveObjects.Add(saveElements);
        }
    }
}
