using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace winHAB.Parser
{
    public class XMLOpenHABDataContract
    {
        #region SmartHomeDataContract
        public class XMLOpenHABDataContractl
        {
            [XmlElement]
            public String name { get; set; }
            [XmlElement]
            public String link { get; set; }
            [XmlElement]
            public Homepage homepage { get; set; }
            public static Dictionary<String, Widget> widgets = new Dictionary<String, Widget>();
            public static Int32 widgetCounter = 0;
            public static Dictionary<Int32, Item> items = new Dictionary<int, Item>();
            public static Int32 itemCounter = 0;
        }
        #endregion
        #region Homepage

        public class Homepage
        {
            [XmlElement]
            public String id { get; set; }
            [XmlElement]
            public String title { get; set; }
            [XmlElement]
            public String link { get; set; }
            [XmlElement]
            public LinkedList<Widget> widget { get; set; }
        }
        #endregion
        #region Widget

        public class Widget : INotifyPropertyChanged
        {
            public Boolean guiState { get; set; }
            [XmlElement]
            private String _widgetId { get; set; }
            [XmlElement]
            public String widgetId
            {
                get
                {
                    return _widgetId;
                }
                set
                {
                    _widgetId = value;
                    //JsonSmartHomeDataContract.widgets.Add(this.widgetId, this);
                    NotifyPropertyChanged("widgetId");
                }
            }
            [XmlElement]
            private String _type { get; set; }
            [XmlElement]
            public String type { get { return _type; } set { _type = value; } }
            [XmlElement]
            private String _label { get; set; }
            [XmlElement]
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
            [XmlElement]
            private String _icon { get; set; }
            private Boolean _once = false;
            [XmlElement]
            public String icon
            {
                get { return _icon; }
                set
                {
                    if (_once != true)
                    {
                        _icon = "/Assets/images/" + value + ".png";
                        _once = true;
                    }
                    else
                        _icon = value;
                    NotifyPropertyChanged("icon");
                }
            }
            //[DataMember]
            //public String _name { get; set; }
            //[DataMember]
            //public String name
            //{
            //    get
            //    {
            //        return _name;
            //    }
            //    set
            //    {
            //        _name = value;
            //    }
            //}
            [XmlElement]
            public LinkedList<Widget> widget { get; set; }//= new LinkedList<Widget>();// { get; set; }
            [XmlElement]
            public LinkedPage linkedPage { get; set; }
            [XmlElement]
            public Item item { get; set; }
            [XmlElement]
            private String _url { get; set; }
            [XmlElement]
            public String url { get { return _url; } set { _url = value; } }
            public event PropertyChangedEventHandler PropertyChanged;
            public void NotifyPropertyChanged(String propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                        new PropertyChangedEventArgs(propertyName));
                }
            }
        }
        #endregion
        #region Item

        public class Item : INotifyPropertyChanged
        {
            [XmlElement]
            private String _type { get; set; }
            [XmlElement]
            public String type
            {
                get
                {
                    return _type;
                }
                set
                {
                    _type = value;
                    //JsonSmartHomeDataContract.items.Add(JsonSmartHomeDataContract.itemCounter++, this);
                }
            }
            [XmlElement]
            private String _name { get; set; }
            [XmlElement]
            public String name
            {
                get
                {
                    return _name;
                }
                set
                {
                    _name = value;
                }
            }
            public Boolean guiState { get; set; }
            [XmlElement]
            private String _state { get; set; }
            [XmlElement]
            public String state
            {
                get
                {
                    return _state;
                }
                set
                {
                    //if (value.Equals("ON"))
                    //    guiState = true;
                    //else if (value.Equals("OFF"))
                    //    guiState = false;
                    _state = value;
                    // Call NotifyPropertyChanged when the source property 
                    // is updated.
                    NotifyPropertyChanged("state");
                }
            }
            [XmlElement]
            private String _link { get; set; }
            [XmlElement]
            public String link
            {
                get
                {
                    return _link;
                }
                set
                {
                    _link = value;
                }
            }
            //[DataMember]
            //private String _url { get; set; }
            //[DataMember]
            //public String url { get { return _url; } set { _url = value; } }

            public event PropertyChangedEventHandler PropertyChanged;
            public void NotifyPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                        new PropertyChangedEventArgs(propertyName));
                }
            }
        }
        #endregion
        #region LinkedPage

        public class LinkedPage
        {
            [XmlElement]
            public String id { get; set; }
            [XmlElement]
            public String title { get; set; }
            [XmlElement]
            public String icon { get; set; }
            [XmlElement]
            public LinkedList<Widget> widget { get; set; }
        }
        #endregion
    }

}