using System;
namespace winHAB.Interfaces
{
    /// <summary>
    /// This Interface provides the standard properties and Eventlisteners for propertiechanges
    /// </summary>
    interface ISwitchWidget
    {
        bool guiState { get; set; }
        string icon { get; set; }
        JSON_Parser.Parser.Item item { get; set; }
        string label { get; set; }
        JSON_Parser.Parser.LinkedPage linkedPage { get; set; }
        void NotifyPropertyChanged(string propertyName);
        event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        string type { get; set; }
        System.Collections.Generic.LinkedList<JSON_Parser.Parser.Widget> widget { get; set; }
        string widgetId { get; set; }
    }
}
