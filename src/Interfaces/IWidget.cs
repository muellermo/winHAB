//using JSON_Parser.Parser;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Runtime.Serialization;
//using System.Text;
//using System.Threading.Tasks;

//namespace winHAB.interfaces
//{
//    interface IWidget : INotifyPropertyChanged
//    {
//        [DataMember]
//        String _widgetId { get; set; }
//        [DataMember]
//        String widgetId
//        {
//            get
//            {
//                return _widgetId;
//            }
//            set
//            {
//                _widgetId = value;
//                //SmartHomeDataContract.widgets.Add(this.widgetId, this);
//                //NotifyPropertyChanged("widgetId");
//            }
//        }
//        [DataMember]
//        String _type { get; set; }
//        [DataMember]
//        String type { get { return _type; } set { _type = value; } }
//        [DataMember]
//        String _label { get; set; }
//        [DataMember]
//        String label
//        {
//            get
//            {
//                return _label;
//            }
//            set
//            {
//                _label = value;
//                NotifyPropertyChanged("label");
//            }
//        }
//        [DataMember]
//        String _icon { get; set; }
//        //Boolean _once;
//        [DataMember]
//        String icon
//        {
//            get { return _icon; }
//            set
//            {
//                if (_once != true)
//                {
//                    _icon = "/Assets/images/" + value + ".png";
//                    _once = true;
//                }
//                else
//                    _icon = value;
//                NotifyPropertyChanged("icon");
//            }
//        }
//        [DataMember]
//        LinkedList<Widget> widget { get; set; }
//        [DataMember]
//        LinkedPage linkedPage { get; set; }
//        [DataMember]
//        Item item { get; set; }

//        event PropertyChangedEventHandler PropertyChanged;
//        void NotifyPropertyChanged(String propertyName);
//        //{
//        //    if (PropertyChanged != null)
//        //    {
//        //        PropertyChanged(this,
//        //            new PropertyChangedEventArgs(propertyName));
//        //    }
//        //}
//    }
//}
