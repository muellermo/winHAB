﻿using JSON_Parser.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winHAB.openHAB_definition.openHAB_Widgets.openHAB_NonLinkableWidget
{
    class VideoWidget : AbstractWidget, INotifyPropertyChanged
    {
        public VideoWidget(VideoWidget widget)
        {
            this.widgetId = widget.widgetId;
            this.type = widget.type;
            this.label = widget.label;
            this.icon = widget.icon;
            this.widget = widget.widget;
            this.linkedPage = widget.linkedPage;
            this.item = widget.item;
            this._once = true;
            this.url = widget.url;
            base.widgetId = widget.widgetId;
            base.type = widget.type;
            base.label = widget.label;
            base.icon = widget.icon;
            base.widget = widget.widget;
            base.linkedPage = widget.linkedPage;
            base.item = widget.item;
            base._once = true;

        }

        public VideoWidget(Widget widget)
        {
            this.widgetId = widget.widgetId;
            this.type = widget.type;
            this.label = widget.label;
            this.icon = widget.icon;
            //this.widget = widget.widget;
            this.linkedPage = widget.linkedPage;
            this.item = widget.item;
            this.url = widget.url;
            this._once = true;
            base.widgetId = widget.widgetId;
            base.type = widget.type;
            base.label = widget.label;
            base.icon = widget.icon;
            //base.widget = widget.widget;
            base.linkedPage = widget.linkedPage;
            base.item = widget.item;
            base._once = true;
        }
        private String _widgetId { get; set; }

        public String widgetId
        {
            get
            {
                return _widgetId;
            }
            set
            {
                _widgetId = value;

                NotifyPropertyChanged("widgetId");
            }
        }

        private String _type { get; set; }

        public String type { get { return _type; } set { _type = value; } }

        private String _label { get; set; }

        public String label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
                NotifyPropertyChanged("label");
            }
        }

        private String _icon { get; set; }
        private Boolean _once = false;

        public String icon
        {
            get { return _icon; }
            set
            {
                if (_once != true)
                {
                    _icon = value;
                    _once = true;
                }
                else
                    _icon = value;
                NotifyPropertyChanged("icon");
            }
        }

        private String _url { get; set; }

        public String url { get { return _url; } set { _url = value; } }
        public LinkedList<Widget> widget { get; set; }

        public LinkedPage linkedPage { get; set; }

        public Item item { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public override void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
