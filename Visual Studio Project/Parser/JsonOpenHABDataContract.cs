using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.UI.Xaml.Data;
using winHAB.openHAB_definition.openHAB_Widgets;

namespace JSON_Parser.Parser
{

    /// <summary>
    /// This class provies the standard-dataformat for the openHAB json-file
    /// </summary>
    #region SmartHomeDataContract
    [DataContract]
    public class JsonOpenHABDataContract
    {
        [DataMember]
        public LinkedList<Sitemap> sitemap { get; set; }
        [DataMember]
        public String name { get; set; }
        [DataMember]
        public String link { get; set; }
        [DataMember]
        public Homepage homepage { get; set; }
        public static Dictionary<String, Widget> widgets = new Dictionary<String, Widget>();
        public static Int32 widgetCounter = 0;
        public static Dictionary<Int32, Item> items = new Dictionary<int, Item>();
        public static Int32 itemCounter = 0;
    }
    #endregion

    #region Sitemap
    public class Sitemap
    {
        [DataMember]
        public String name { get; set; }
        [DataMember]
        public String link { get; set; }
        [DataMember]
        public LinkedList<Homepage> homepage { get; set; }
    }
    #endregion
    #region Homepage
    [DataContract]
    public class Homepage
    {
        [DataMember]
        public String id { get; set; }
        [DataMember]
        public String title { get; set; }
        [DataMember]
        public String link { get; set; }
        [DataMember]
        public LinkedList<Widget> widget { get; set; }

    }
    #endregion
    #region Widget
    //[DataContract]
    public class Widget : INotifyPropertyChanged
    {
        public Boolean guiState { get; set; }
        [DataMember]
        private String _widgetId { get; set; }
        [DataMember]
        public String widgetId
        {
            get
            {
                return _widgetId;
            }
            set
            {
                _widgetId = value;
                try { 
                JsonOpenHABDataContract.widgets.Add(this.widgetId, this);
                }   catch
                {}
                NotifyPropertyChanged("widgetId");
            }
        }
        [DataMember]
        private String _type { get; set; }
        [DataMember]
        public String type { get { return _type; } set { _type = value; } }
        [DataMember]
        private String _label { get; set; }
        [DataMember]
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
        [DataMember]
        private String _icon { get; set; }
        private Boolean _once = false;
        [DataMember]
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
        //public String _type { get; set; }
        //[DataMember]
        //public String type
        //{
        //    get
        //    {
        //        return _type;
        //    }
        //    set
        //    {
        //        _type = value;
        //    }
        //}

        //[DataMember(Name = "widget")]
        //public Dictionary<String,Widget> x { get; set; }
        [DataMember]
        public LinkedList<Widget> widget { get; set; }// = new LinkedList<Widget>();// { get; set; }
        //[DataMember(Name = "widget",IsRequired=true)]
        //public List<Widget> widget {get;set;}// = new LinkedList<Widget>();// { get; set; }
        [DataMember]
        public LinkedPage linkedPage { get; set; }
        [DataMember(Name = "item")]
        public Item item { get; set; }
        [DataMember]
        private String _url { get; set; }
        [DataMember]
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
    [DataContract]
    public class Item : INotifyPropertyChanged
    {
        [DataMember]
        private String _type { get; set; }
        [DataMember]
        public String type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                JsonOpenHABDataContract.items.Add(JsonOpenHABDataContract.itemCounter++, this);
            }
        }
        [DataMember]
        private String _name { get; set; }
        [DataMember]
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
        [DataMember]
        private String _state { get; set; }
        [DataMember]
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
        [DataMember]
        private String _link { get; set; }
        [DataMember]
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
    [DataContract]
    public class LinkedPage
    {
        [DataMember]
        public String id { get; set; }
        [DataMember]
        public String title { get; set; }
        [DataMember]
        public String icon { get; set; }
        [DataMember]
        public LinkedList<Widget> widget { get; set; }
    }
    #endregion
}
