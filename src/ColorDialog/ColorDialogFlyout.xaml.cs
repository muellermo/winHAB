using JSON_Parser.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Einstellungs-Flyout" ist unter http://go.microsoft.com/fwlink/?LinkId=273769 dokumentiert.

namespace winHAB.ColorDialog
{
    public sealed partial class ColorDialogFlyout : SettingsFlyout
    {
        public ColorDialogFlyout(Item item)
        {
            this.InitializeComponent();
            typeText.Text = "Type: "+item.type;
            nameText.Text = "Name: "+item.name;
            stateText.Text = "State: "+item.state;
            linkText.Text = "Link: "+item.link;
        }
    }
}
