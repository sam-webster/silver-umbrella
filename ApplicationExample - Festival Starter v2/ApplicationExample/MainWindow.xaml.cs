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
using System.Xml.Serialization;
using FestivalElements.Elements.Drawable;
using FestivalElements.Elements.Saveable;
using FestivalElements.Tools;
using SaveSystem;
using System.Windows.Controls.Primitives;
using Microsoft.Win32;

namespace ApplicationExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FestivalElement fe, selectedFE;
        private FestivalElement.FestivalElementType _CurrentFEType;
        private Tools.ToolType _CurrentTool;
        Boolean lmbDown; // checks if left mous button is down
        HitTestResult result;
        Brush colour;
        double origScaleX, origScaleY, zoom;
        double canvasX, canvasY;
        Boolean initialised, colourChange;
        ResizeCanvas resizeCanvasWindow;
        string savePath;

        Tools.ToolType CurrentTool
        {
            get { return _CurrentTool; }
            set
            {
                // runs a method which needs to run when the tool is changed
                OnToolChange(value);
                _CurrentTool = value;
            }
        }


        FestivalElement.FestivalElementType CurrentFEType
        {
            get { return _CurrentFEType; }
            set
            {
                // when the CurrentFEType is changed preview needs to be updated
                _CurrentFEType = value;
                UpdatePreview();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            initialised = true; // stops colour preview from breaking
            CurrentTool = Tools.ToolType.CreateElement;
            CurrentFEType = FestivalElement.FestivalElementType.Bin;
            canvasPreview.Children.Clear();
            gridSelectionTools.Visibility = Visibility.Collapsed;
            zoom = 1;
            KeyboardNavigation.SetDirectionalNavigation(this, KeyboardNavigationMode.None);
            savePath = "";
            this.Title = "Festival Designer - New";
        }

        // methods which run from mouse activity
        #region Mouse Methods
        private void paper_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mWindow.Focus();
            Point pt = e.GetPosition((UIElement)sender);
            result = VisualTreeHelper.HitTest(paper, pt);
            // checks which tool is in use
            // tools do what they are called
            switch (CurrentTool)
            {
                case Tools.ToolType.CreateElement:
                    fe = GetFEType(CurrentFEType, colour, origScaleX, origScaleY);

                    fe.X = e.GetPosition(paper).X;
                    fe.Y = e.GetPosition(paper).Y;
                    fe.Angle = 0;

                    fe.RenderTransform = fe.GetTransform();

                    paper.Children.Add(fe);
                    break;
                case Tools.ToolType.Move:
                    

                    if (result != null)
                    {
                        if (result.VisualHit.GetType() != typeof(Canvas))
                        {
                            fe = (FestivalElement)result.VisualHit;
                        }
                    }
                    break;
                case Tools.ToolType.Erase:

                    if (result != null)
                    {
                        if (result.VisualHit.GetType() != typeof(Canvas))
                        {
                            fe = (FestivalElement)result.VisualHit;
                            paper.Children.Remove(fe);
                        }
                    }
                    break;
                case Tools.ToolType.ColourPick:
                    if (result != null)
                    {
                        if (result.VisualHit.GetType() != typeof(Canvas))
                        {
                            // will get element colour
                            fe = (FestivalElement)result.VisualHit;
                            Color c = (fe.BGBrush as SolidColorBrush).Color;
                            colourChange = true;
                            sliderAlpha.Value = c.A;
                            sliderRed.Value = c.R;
                            sliderGreen.Value = c.G;
                            sliderBlue.Value = c.B;
                            colourChange = false;
                            GetColourFromSlider();
                        }
                        else
                        {
                            // will get background colour
                            Color c = (paper.Background as SolidColorBrush).Color;
                            colourChange = true;
                            sliderAlpha.Value = c.A;
                            sliderRed.Value = c.R;
                            sliderGreen.Value = c.G;
                            sliderBlue.Value = c.B;
                            colourChange = false;
                            GetColourFromSlider();
                        }
                    }
                    break;
                case Tools.ToolType.ReColour:
                    if (result != null)
                    {
                        if (result.VisualHit.GetType() != typeof(Canvas))
                        {
                            fe = (FestivalElement)result.VisualHit;
                            fe.BGBrush = colour;
                            fe.InvalidateVisual();
                        }
                        else
                        {
                            paper.Background = colour;
                        }
                    }
                    break;
                case Tools.ToolType.Select:
                    if (result != null)
                    {
                        if (result.VisualHit.GetType() != typeof(Canvas))
                        {
                            if (selectedFE != null)
                            {
                                selectedFE.OnDeSelection();
                            }
                            selectedFE = (FestivalElement)result.VisualHit;
                            selectedFE.OnSelection();
                            if(gridSelectionTools.Visibility != Visibility.Visible)
                            {
                                gridSelectionTools.Visibility = Visibility.Visible;
                                UpdateElementList();
                            }
                            GetSelectionData();
                        }
                        
                    }
                    break;
            }

            lmbDown = true;
        }
        /// <summary>
        /// updates the variable to say that left mouse is no longer down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void paper_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            lmbDown = false;
        }
        /// <summary>
        /// runs when mouse is moved over the canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void paper_MouseMove(object sender, MouseEventArgs e)
        {
            if (lmbDown) // only run if left mouse down
            {
                switch (CurrentTool)
                {
                    case Tools.ToolType.CreateElement:
                        if (fe != null)
                        {
                            fe.X = e.GetPosition(paper).X;
                            fe.Y = e.GetPosition(paper).Y;

                            fe.RenderTransform = fe.GetTransform();
                        }
                        break;
                    case Tools.ToolType.Move:
                        if (result != null)
                        {
                            fe.X = e.GetPosition(paper).X;
                            fe.Y = e.GetPosition(paper).Y;

                            fe.RenderTransform = fe.GetTransform();
                        }
                        break;
                    case Tools.ToolType.Erase:
                        Point pt = e.GetPosition((UIElement)sender);

                        result = VisualTreeHelper.HitTest(paper, pt);

                        if (result != null)
                        {
                            if (result.VisualHit.GetType() != typeof(Canvas))
                            {
                                fe = (FestivalElement)result.VisualHit;
                                paper.Children.Remove(fe);
                            }
                        }
                        break;
                    case Tools.ToolType.ColourPick:
                        break;
                    case Tools.ToolType.ReColour:
                        break;
                    case Tools.ToolType.Select:
                        break;
                }
            }
        }
        #endregion
        // updates CurrentTool and CurrentFEType when buttons are pressed
        #region Button Pressed
        private void buttonBin_Click(object sender, RoutedEventArgs e)
        {
            CurrentTool = Tools.ToolType.CreateElement;
            CurrentFEType = FestivalElement.FestivalElementType.Bin;
        }

        private void buttonShop_Click(object sender, RoutedEventArgs e)
        {
            CurrentTool = Tools.ToolType.CreateElement;
            CurrentFEType = FestivalElement.FestivalElementType.Shop;
        }

        private void buttonBush_Click(object sender, RoutedEventArgs e)
        {
            CurrentTool = Tools.ToolType.CreateElement;
            CurrentFEType = FestivalElement.FestivalElementType.Bush;
        }

        private void buttonStage_Click(object sender, RoutedEventArgs e)
        {
            CurrentTool = Tools.ToolType.CreateElement;
            CurrentFEType = FestivalElement.FestivalElementType.Stage;
        }

        private void buttonToilet_Click(object sender, RoutedEventArgs e)
        {
            CurrentTool = Tools.ToolType.CreateElement;
            CurrentFEType = FestivalElement.FestivalElementType.Toilet;
        }

        private void buttonBeerTent_Click(object sender, RoutedEventArgs e)
        {
            CurrentTool = Tools.ToolType.CreateElement;
            CurrentFEType = FestivalElement.FestivalElementType.BeerTent;
        }

        private void buttonFoodTent_Click(object sender, RoutedEventArgs e)
        {
            CurrentTool = Tools.ToolType.CreateElement;
            CurrentFEType = FestivalElement.FestivalElementType.FoodTent;
        }

        private void buttonCampZone_Click(object sender, RoutedEventArgs e)
        {
            CurrentTool = Tools.ToolType.CreateElement;
            CurrentFEType = FestivalElement.FestivalElementType.CampingZone;
        }

        private void buttonFairGround_Click(object sender, RoutedEventArgs e)
        {
            CurrentTool = Tools.ToolType.CreateElement;
            CurrentFEType = FestivalElement.FestivalElementType.FairGround;
        }

        private void buttonMove_Click(object sender, RoutedEventArgs e)
        {
            CurrentTool = Tools.ToolType.Move;
        }

        private void buttonSelect_Click(object sender, RoutedEventArgs e)
        {
            CurrentTool = Tools.ToolType.Select;
        }

        private void buttonEraser_Click(object sender, RoutedEventArgs e)
        {
            CurrentTool = Tools.ToolType.Erase;
        }

        private void buttonColourPicker_Click(object sender, RoutedEventArgs e)
        {
            CurrentTool = Tools.ToolType.ColourPick;
        }

        private void buttonReColour_Click(object sender, RoutedEventArgs e)
        {
            CurrentTool = Tools.ToolType.ReColour;
        }

        private void buttonSetBG_Click(object sender, RoutedEventArgs e)
        {
            paper.Background = colour;
        }

        private void menuItemCentralise_Click(object sender, RoutedEventArgs e)
        {
            canvasX = 0;
            canvasY = 0;
            paper.RenderTransform = CanvasTransform(zoom, canvasX, canvasY);
        }
        #endregion
        // methods which run when sliders are changed
        #region Slider Methods
        private void sliderColour_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!colourChange)
            {
                GetColourFromSlider();
            }
        }

        private void sliderScale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ScaleFromSlider();
        }
        #endregion
        // methods which run when textboxes lose focus
        #region TextBox Methods
        private void textBoxColour_LostFocus(object sender, RoutedEventArgs e)
        {
            byte a, r, g, b;
            if (Byte.TryParse(textBoxAlpha.Text, out a) && (Byte.TryParse(textBoxRed.Text, out r)) && (Byte.TryParse(textBoxGreen.Text, out g)) && (Byte.TryParse(textBoxBlue.Text, out b)))
            {
                colourChange = true;
                sliderAlpha.Value = a;
                sliderRed.Value = r;
                sliderGreen.Value = g;
                sliderBlue.Value = b;
                colourChange = false;
            }
                GetColourFromSlider();
        }
        private void textBoxScale_LostFocus(object sender, RoutedEventArgs e)
        {
            double s;
            if (Double.TryParse(textBoxScale.Text, out s))
            {
                sliderScale.Value = s;
            }
            else
            {
                ScaleFromSlider();
            }
        }

        private void textBoxSName_LostFocus(object sender, RoutedEventArgs e)
        {
            string name = textBoxSName.Text;
            if (name!="" && name.Length < 30)
            {
                selectedFE.Name = name;
                UpdateElementList();
            }
            else
            {
                textBoxSName.Text = selectedFE.Name;
            }
        }

        private void textBoxSScaleX_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateScale();
        }

        private void textBoxSScaleY_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateScale();
        }

        private void textBoxSRotation_LostFocus(object sender, RoutedEventArgs e)
        {
            double angle;
            if(Double.TryParse(textBoxSRotation.Text, out angle))
            {
                while (angle > 360)
                {
                    angle -= 360;
                }
                while (angle <= 0)
                {
                    angle += 360;
                }
                selectedFE.Angle = angle;
                selectedFE.RenderTransform = selectedFE.GetTransform();
            }
            textBoxSRotation.Text = selectedFE.Angle.ToString();
        }

        private void textBoxSX_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdatePosition();
        }

        private void textBoxSY_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdatePosition();
        }
        #endregion
        // buttons in the File and View menus
        #region Menu Items
        private void menuItemNew_Click(object sender, RoutedEventArgs e)
        {
            savePath = "";
            this.Title = "Festival Designer - New";
            paper.Children.Clear();
            paper.Background = Brushes.White;
        }
        /// <summary>
        /// runs Save() if there is already a filename or SaveAs() id not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemSave_Click(object sender, RoutedEventArgs e)
        {
            if (savePath == "")
            {
                SaveAs();
            }
            else
            {
                Save();
            }
        }

        private void menuItemSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveAs();
        }

        private void menuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void menuItemZoomIn_Click(object sender, RoutedEventArgs e)
        {
            zoom *= 1.2;
            paper.RenderTransform = CanvasTransform(zoom, canvasX, canvasY);
        }

        private void menuItemZoomOut_Click(object sender, RoutedEventArgs e)
        {
            zoom /= 1.2;
            paper.RenderTransform = CanvasTransform(zoom, canvasX, canvasY);
        }

        private void menuItemResetZoom_Click(object sender, RoutedEventArgs e)
        {
            zoom = 1;
            paper.RenderTransform = CanvasTransform(zoom, canvasX, canvasY);
        }
        private void menuItemResizeCanvas_Click(object sender, RoutedEventArgs e)
        {
            resizeCanvasWindow = new ResizeCanvas(paper.Width, paper.Height, this);
            resizeCanvasWindow.Show();
        }
        #endregion

        #region Key Down
        /// <summary>
        /// used to move canvas when arrow keys are held down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Down)
            {
                canvasY += 50;
            }
            else if (e.Key == Key.Up)
            {
                canvasY -= 50;
            }
            else if (e.Key == Key.Left)
            {
                canvasX -= 50;
            }
            else if (e.Key == Key.Right)
            {
                canvasX += 50;
            }
            paper.RenderTransform = CanvasTransform(zoom, canvasX, canvasY); // transforms the canvas
            mWindow.Focus();
        }
        #endregion

        #region Other Windows
        /// <summary>
        /// this is to get the new canvas size from resizeCanvasWindow and to close that window
        /// </summary>
        /// <param name="cWidth">new canvas width</param>
        /// <param name="cHeight">new canvas hieght</param>
        public void SetNewCanvasSize(double cWidth, double cHeight)
        {
            if(cWidth > 0 && cHeight > 0)
            {
                paper.Width = cWidth;
                paper.Height = cHeight;
            }
            resizeCanvasWindow.Close();
        }
        #endregion

        #region Other
        /// <summary>
        /// gets the type of FestivalElement to be created
        /// </summary>
        /// <param name="type">type of FestivalElement to be loaded</param>
        /// <param name="nColour">colour of new FE</param>
        /// <param name="nScaleX">x scale of new FE</param>
        /// <param name="nScaleY">y scale of new FE</param>
        /// <returns></returns>
        FestivalElement GetFEType(FestivalElement.FestivalElementType type, Brush nColour, double nScaleX, double nScaleY)
        {
            switch (type)
            {
                case FestivalElement.FestivalElementType.Square:
                    return new Square(nColour, nScaleX, nScaleY);
                case FestivalElement.FestivalElementType.Triangle:
                    return new Triangle(nColour, nScaleX, nScaleY);
                case FestivalElement.FestivalElementType.Bush:
                    return new Bush(nColour, nScaleX, nScaleY);
                case FestivalElement.FestivalElementType.Stage:
                    return new Stage(nColour, nScaleX, nScaleY);
                case FestivalElement.FestivalElementType.BeerTent:
                    return new BeerTent(nColour, nScaleX, nScaleY);
                case FestivalElement.FestivalElementType.Bin:
                    return new Bin(nColour, nScaleX, nScaleY);
                case FestivalElement.FestivalElementType.CampingZone:
                    return new CampingZone(nColour, nScaleX, nScaleY);
                case FestivalElement.FestivalElementType.FairGround:
                    return new FairGround(nColour, nScaleX, nScaleY);
                case FestivalElement.FestivalElementType.FoodTent:
                    return new FoodTent(nColour, nScaleX, nScaleY);
                case FestivalElement.FestivalElementType.Shop:
                    return new Shop(nColour, nScaleX, nScaleY);
                case FestivalElement.FestivalElementType.Toilet:
                    return new Toilet(nColour, nScaleX, nScaleY);
                default:
                    return new Bush(nColour, nScaleX, nScaleY);
            }
            
        }
        /// <summary>
        /// gets the brush colour from the colour sliders
        /// </summary>
        void GetColourFromSlider()
        {
            byte a, r, g, b;
            a = (byte)sliderAlpha.Value;
            r = (byte)sliderRed.Value;
            g = (byte)sliderGreen.Value;
            b = (byte)sliderBlue.Value;
            colour = new SolidColorBrush(Color.FromArgb(a, r, g, b));
            if (initialised) // otherwise this method runs when initialisating
            {
                textBoxAlpha.Text = a.ToString();
                textBoxRed.Text = r.ToString();
                textBoxGreen.Text = g.ToString();
                textBoxBlue.Text = b.ToString();
                colourPreview.Fill = colour;
                UpdatePreview();
            }
        }

        void ScaleFromSlider()
        {
            double s;
            s = sliderScale.Value;
            origScaleX = s / 16;
            origScaleY = s / 16;
            if (initialised)
            {
                textBoxScale.Text = s.ToString();
            }
        }
        /// <summary>
        /// gets data from selectedFE
        /// </summary>
        void GetSelectionData()
        {
            textBoxSName.Text = selectedFE.Name;
            textBoxSScaleX.Text = selectedFE.ScaleX.ToString();
            textBoxSScaleY.Text = selectedFE.ScaleY.ToString();
            textBoxSX.Text = selectedFE.X.ToString();
            textBoxSY.Text = selectedFE.Y.ToString();
            textBoxSRotation.Text = selectedFE.Angle.ToString();

            if (selectedFE.ListBoxID > -1) // only run if element has an listbox ID
            {
                listBoxFestivalElements.SelectedIndex = selectedFE.ListBoxID; // selects selectedFE in listbox
            }
        }
        /// <summary>
        /// updates scale of selectedFE is textbox is valid
        /// </summary>
        void UpdateScale()
        {
            double sX, sY;
            if(Double.TryParse(textBoxSScaleX.Text, out sX))
            {
                if (sX <= 50)
                {
                    selectedFE.ScaleX = sX;
                }
            }
            if (Double.TryParse(textBoxSScaleY.Text, out sY))
            {
                if (sY <= 50)
                {
                    selectedFE.ScaleY = sY;
                }
            }
            textBoxSScaleX.Text = selectedFE.ScaleX.ToString();
            textBoxSScaleY.Text = selectedFE.ScaleY.ToString();
            selectedFE.RenderTransform = selectedFE.GetTransform();
        }
        /// <summary>
        /// moves selectedFE to new position. resets textbox if input is invalid
        /// </summary>
        void UpdatePosition()
        {
            double x, y;
            if (Double.TryParse(textBoxSX.Text, out x))
            {
                if (x <= paper.Width && x >= 0)
                {
                    selectedFE.X = x;
                }
            }
            if (Double.TryParse(textBoxSY.Text, out y))
            {
                if (y <= paper.Height && y >= 0)
                {
                    selectedFE.Y = y;
                }
            }
            textBoxSX.Text = selectedFE.X.ToString();
            textBoxSY.Text = selectedFE.Y.ToString();
            selectedFE.RenderTransform = selectedFE.GetTransform();
        }

        void OnToolChange(Tools.ToolType newType)
        {
            if(CurrentTool == Tools.ToolType.Select)
            {
                // only show select panel if tool is select
                // if select is clicked when element is selected then selection is cleared
                if (selectedFE != null)
                {
                    selectedFE.OnDeSelection();
                }
                selectedFE = null;
                gridSelectionTools.Visibility = Visibility.Collapsed;
            }
            if(newType != Tools.ToolType.CreateElement)
            {
                CurrentFEType = FestivalElement.FestivalElementType.None;
                canvasPreview.Children.Clear();
            }
        }

        /// <summary>
        /// updates the preview element
        /// </summary>
        void UpdatePreview()
        {
            canvasPreview.Children.Clear();
            FestivalElement previewFE = GetFEType(CurrentFEType, colour, 2, 2);
            
            previewFE.X = canvasPreview.ActualWidth / 2;
            previewFE.Y = canvasPreview.ActualHeight / 2;
            previewFE.RenderTransform = previewFE.GetTransform();
            previewFE.IsPreview = true;

            canvasPreview.Children.Add(previewFE);
        }

        void Save()
        {
            SaveFile saveFile = new SaveFile(paper);
            XmlWriter xw = new XmlWriter();
            
            xw.WriteXMLSaveFile(saveFile, savePath);
            
        }

        private void listBoxFestivalElements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxFestivalElements.SelectedIndex > -1)
            {
                foreach(FestivalElement child in paper.Children)
                {
                    if(child.ListBoxID == listBoxFestivalElements.SelectedIndex)
                    {
                        // this selects the element corresponding to the listbox item
                        selectedFE.OnDeSelection();
                        selectedFE = child;
                        selectedFE.OnSelection();
                        GetSelectionData();
                    }
                }
            }
            
        }

        void SaveAs()
        {
            SaveFile saveFile = new SaveFile(paper);
            XmlWriter xw = new XmlWriter();
            String path;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Festival Design Save (*.fds)|*.fds|Portable Nextwork Graphics (*.png)|*.png";

            if (saveFileDialog.ShowDialog() == true)
            {
                path = saveFileDialog.FileName;
                string ext = System.IO.Path.GetExtension(path);
                if(ext == ".png")
                {
                    SavePNG(path);
                }
                else if (ext == ".fds")
                {
                    savePath = path;
                    xw.WriteXMLSaveFile(saveFile, path);
                    this.Title = "Festival Designer - " + savePath;
                }
            }
            
        }

        private void menuItemSaveAsPNG_Click(object sender, RoutedEventArgs e)
        {
            SavePNG("");
        }

        // https://jasonkemp.ca/blog/how-to-save-xaml-as-an-image/
        void SavePNG(string sPath)
        {
            Point p = paper.TranslatePoint(new Point(0, 0), mWindow);
            Rect rect = new Rect(p.X, p.Y, paper.ActualWidth, paper.ActualHeight);
            Int32Rect cropRect = new Int32Rect((int)p.X, (int)p.Y, (int)paper.ActualWidth, (int)paper.ActualHeight);
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)rect.Right,
              (int)rect.Bottom, 96d, 96d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(paper);

            CroppedBitmap crop = new CroppedBitmap(rtb, cropRect);

            //endcode as PNG
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(crop));

            //save to memory stream
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            pngEncoder.Save(ms);
            ms.Close();
            // System.IO.File.WriteAllBytes("logo.png", ms.ToArray());
            // Console.WriteLine("Done");


            String path;
            if (sPath == "")
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Portable Network Graphics (*.png)|*.png";

                if (saveFileDialog.ShowDialog() == true)
                {
                    path = saveFileDialog.FileName;
                    System.IO.File.WriteAllBytes(path, ms.ToArray());
                }
            }
            else
            {
                path = sPath;
                System.IO.File.WriteAllBytes(path, ms.ToArray());
            }
        }
        /// <summary>
        /// Opens a openfile dialog and let's the user load a file
        /// </summary>
        void Load()
        {
            XmlReader xr = new XmlReader();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Festival Design Save (*.fds)|*.fds"; // this will let you only open .fds files
            if (openFileDialog.ShowDialog() == true) // opens a standard windows open file dialog
            {
                savePath = openFileDialog.FileName;
                SaveFile saveFile = xr.ReadXMLSaveFile(savePath);
                SaveableCanvas c = saveFile.saveCanvas;

                // set the canvas data as it is in the save file
                paper.Background = new SolidColorBrush(Color.FromArgb(c.A, c.R, c.G, c.B));
                paper.Height = c.CanvasHeight;
                paper.Width = c.CanvasWidth;
                zoom = c.Zoom;
                canvasX = c.XPos;
                canvasY = c.YPos;

                paper.Children.Clear();
                foreach (SaveableFE savedFE in saveFile.saveElements) // cycle through the saved elements
                {
                    paper.Children.Add(LoadFE(savedFE)); // load the element to canvas
                }
                paper.RenderTransform = CanvasTransform(zoom, canvasX, canvasY); // transform canvas to saved state
                this.Title = "Festival Designer - " + savePath;
            }
        }
        /// <summary>
        /// Use this to load a FestivalElement from a SaveFE
        /// </summary>
        /// <param name="savedFE">SaveFE to be loaded</param>
        /// <returns>The FestivalElement which was saved</returns>
        FestivalElement LoadFE(SaveableFE savedFE)
        {
            FestivalElement lfe;
            switch (savedFE.FEType)
            {
                case "Square":
                    lfe = new Square(Brushes.Black, 1, 1);
                    break;
                case "Triangle":
                    lfe = new Triangle(Brushes.Black, 1, 1);
                    break;
                case "Bush":
                    lfe = new Bush(Brushes.Black, 1, 1);
                    break;
                case "Stage":
                    lfe = new Stage(Brushes.Black, 1, 1);
                    break;
                case "BeerTent":
                    lfe = new BeerTent(Brushes.Black, 1, 1);
                    break;
                case "Bin":
                    lfe = new Bin(Brushes.Black, 1, 1);
                    break;
                case "CampingZone":
                    lfe = new CampingZone(Brushes.Black, 1, 1);
                    break;
                case "FairGround":
                    lfe = new FairGround(Brushes.Black, 1, 1);
                    break;
                case "FoodTent":
                    lfe = new FoodTent(Brushes.Black, 1, 1);
                    break;
                case "Shop":
                    lfe = new Shop(Brushes.Black, 1, 1);
                    break;
                case "Toilet":
                    lfe = new Toilet(Brushes.Black, 1, 1);
                    break;
                default:
                    lfe = new Bush(Brushes.Black, 1, 1);
                    break;
            }
            lfe.LoadData(savedFE);
            lfe.RenderTransform = lfe.GetTransform();
            return lfe;
        }
        /// <summary>
        /// This is used to update the canvas's transform
        /// </summary>
        /// <param name="z">the canvas zoom</param>
        /// <param name="x">x for translation</param>
        /// <param name="y">y for translation</param>
        /// <returns></returns>
        TransformGroup CanvasTransform(double z, double x, double y)
        {
            TransformGroup t = new TransformGroup();
            t.Children.Add(new ScaleTransform(z, z, paper.Width/2, paper.Height/2)); // this adds the zoom
            t.Children.Add(new TranslateTransform(x, y)); // this adds the translate
            return t;
        }
        /// <summary>
        /// Updates the listbox
        /// </summary>
        void UpdateElementList()
        {
            listBoxFestivalElements.Items.Clear();
            foreach (FestivalElement child in paper.Children)
            {
                child.ListBoxID = listBoxFestivalElements.Items.Add(child.Name);
            }    
        }
        #endregion
    }
}
