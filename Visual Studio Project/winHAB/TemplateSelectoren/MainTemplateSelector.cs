﻿using JSON_Parser.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using winHAB.openhab_definition;
using winHAB.openHAB_definition.openHAB_Widgets;
using winHAB.openHAB_definition.openHAB_Widgets.openHAB_LinkableWidget;
using winHAB.openHAB_definition.openHAB_Widgets.openHAB_NonLinkableWidget;

namespace winHAB.TemplateSelectoren
{
    class MainTemplateSelector:DataTemplateSelector
    {
        public DataTemplate itemTemplate { get; set; }
        public DataTemplate linkedPageTemplate { get; set; }
        #region nonlinkableWidgets
        public DataTemplate chartWidget { get; set; }
        public DataTemplate ColorpickerWidget { get; set; }
        public DataTemplate listWidget { get; set; }
        public DataTemplate selectionWidget { get; set; }
        public DataTemplate setpointWidget { get; set; }
        public DataTemplate sliderWidget { get; set; }
        public DataTemplate switchWidget { get; set; }
        public DataTemplate videoWidget { get; set; }
        public DataTemplate webViewWidget { get; set; }
        public DataTemplate dimmerWidget { get; set; }
        #endregion
        public DataTemplate rollerShutterWidget { get; set; }
        public DataTemplate colorWidget { get; set; }

        #region linkable Widgets
        public DataTemplate frameWidget { get; set; }
        public DataTemplate groupWidget { get; set; }
        public DataTemplate imageWidget { get; set; }
        public DataTemplate textWidget { get; set; }
        #endregion

        protected override Windows.UI.Xaml.DataTemplate SelectTemplateCore(object item, Windows.UI.Xaml.DependencyObject container)
        {
            //if (item.GetType() == typeof(Widget))
            //    return widgetTemplate;
            if (item.GetType() == typeof(FrameWidget))
                return frameWidget;
            if (item.GetType() == typeof(SwitchWidget))
                return null;
            if (item.GetType() == typeof(GroupWidget))
                return groupWidget;
    /*        if (item.GetType() == typeof(ImageWidget))
                return imageWidget;*/
            if (item.GetType() == typeof(TextWidget))
                return textWidget;
            //if (item.GetType() == typeof(DimmerWidget))
            //    return dimmerWidget;
            if(item.GetType() == typeof(SliderWidget))
                return sliderWidget;
            /*if (item.GetType() == typeof(SwitchWidget))
                return switchWidget;
            if (item.GetType() == typeof(RollershutterItem))
                return rollerShutterWidget;
            if (item.GetType() == typeof(StringItem))
                return textWidget;
            if (item.GetType() == typeof(ColorItem))
                return colorWidget;
            if (item.GetType() == typeof(Item))
                return itemTemplate;
            if (item.GetType() == typeof(LinkedPage))
                return linkedPageTemplate; */
            return null;
        }
    }
}