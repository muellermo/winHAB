using System;
namespace winHAB
{
    interface IMainPage
    {
        void clearWidgetLists();
        void Connect(int connectionId, object target);
        void InitializeComponent();
        void NotifyPropertyChanged(string propertyName);
        event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        void updateUI();
    }
}
