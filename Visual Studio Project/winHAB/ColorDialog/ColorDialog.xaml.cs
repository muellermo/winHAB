using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace winHAB.ColorDialog
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class ColorDialog : Page
    {
        public ColorDialog()
        {
            this.InitializeComponent();
            ImageBrush imageBrush = new ImageBrush();
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            Rectangle rectangle = new Rectangle();
            rectangle.Width = 100;
            rectangle.Height = 100;
            colorDialogCanvas.Children.Add(rectangle);
            Image img = new Image();
            
        }

        private void colorDialogCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Pointer i = e.Pointer;        
        }

        private void backButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Image_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(null);
            ImageSource image = colorWheel.Source;

            BitmapImage bmpImage = (BitmapImage)image;// new BitmapImage(new Uri("../Assets/images/colorwheel.png"));
            bmpImage.DecodePixelType = DecodePixelType.Logical;
            WriteableBitmap x = new WriteableBitmap(1000,1000);
        }
    }
}
