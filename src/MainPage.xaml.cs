using JSON_Parser.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.System;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using winHAB.ColorDialog;
using winHAB.communication;
using winHAB.communication.connectiontypes;
using winHAB.communication.httpclient.get;
using winHAB.communication.httpclient.post;
using winHAB.FirstStart;
using winHAB.gui;
using winHAB.openhab_definition;
using winHAB.openHAB_definition.openHAB_Widgets;
using winHAB.openHAB_definition.openHAB_Widgets.openHAB_LinkableWidget;
using winHAB.openHAB_definition.openHAB_Widgets.openHAB_NonLinkableWidget;
using winHAB.SettingsManager;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace winHAB
{


    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public partial class MainPage : Page, INotifyPropertyChanged
    {
        #region Variablen
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private ThreadPoolTimer periodicTimer;
        private PostMessage sendMessage = new PostMessage();
        private Stack<LinkedList<AbstractWidget>> previusWidgets = new Stack<LinkedList<AbstractWidget>>();
        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private PasswordVault passwordVault = new PasswordVault();
        private GetJson jsonDownloader;
        private Boolean settingsSetCorrectly = false;
        private Boolean _leftNavClearer = false;
        public static Boolean _errorOccured = false;
        private Boolean _taskReady { get; set; }
        public static Boolean taskReady { get; set; }
        //{
        //    get { return _taskReady; }
        //    set
        //    {
        //        _taskReady = value;
        //        NotifyPropertyChanged("taskReady");
        //    }
        //}
        #endregion
        #region Only for Debugging
        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Constructor and DispatcherTimer for keep the GUI up-to-date
        public MainPage()
        {

            this.InitializeComponent();
            MainPage.taskReady = true;
            taskObserver.Text = MainPage.taskReady.ToString();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            if (localSettings.Values["connectionType"] != null && localSettings.Values["connectionType"].ToString().Equals(ConnectionTypesEnum.HTTPSwithUsernameAndPassword.ToString()))
            {
                try
                {
                    jsonDownloader = new GetJson(localSettings.Values["url"].ToString(), "WIRD NICHT MEHR GEBRAUCHT!!!", Convert.ToInt32(localSettings.Values["port"].ToString()), localSettings.Values["username"].ToString(), passwordVault.Retrieve("winHAB", localSettings.Values["username"].ToString()).Password, true);
                    //Sets the Event, wich is called, when the Interface of obenHAB is dowloaded and readed correctly
                    jsonDownloader.sh_downloadFinished += jsonDownloader_sh_downloadFinished;
                    settingsSetCorrectly = true;
                }
                catch (Exception ex)
                {

                }
            }
            if (localSettings.Values["connectionType"] != null && localSettings.Values["connectionType"].ToString().Equals(ConnectionTypesEnum.HTTPSwithoutUsernameAndPassword.ToString()))
            {
                try
                {
                    jsonDownloader = new GetJson(localSettings.Values["url"].ToString(), "", Convert.ToInt32(localSettings.Values["port"].ToString()), true);
                    //Sets the Event, wich is called, when the Interface of obenHAB is dowloaded and readed correctly
                    jsonDownloader.sh_downloadFinished += jsonDownloader_sh_downloadFinished;
                    settingsSetCorrectly = true;
                }
                catch (Exception ex)
                {

                }
            }
            if (localSettings.Values["connectionType"] != null && localSettings.Values["connectionType"].ToString().Equals(ConnectionTypesEnum.HTTPwithUsernameAndPassword.ToString()))
            {
                try
                {
                    jsonDownloader = new GetJson(localSettings.Values["username"].ToString(), "", Convert.ToInt32(localSettings.Values["port"].ToString()), localSettings.Values["username"].ToString(), localSettings.Values["password"].ToString());
                    //Sets the Event, wich is called, when the Interface of obenHAB is dowloaded and readed correctly
                    jsonDownloader.sh_downloadFinished += jsonDownloader_sh_downloadFinished;
                    //If everything above was correctly, the GUI can be filled
                    settingsSetCorrectly = true;
                }
                catch (Exception ex)
                {

                }
            }
            if (localSettings.Values["connectionType"] != null && localSettings.Values["connectionType"].ToString().Equals(ConnectionTypesEnum.HTTPwithoutUsernameAndPassword.ToString()))
            {
                try
                {
                    jsonDownloader = new GetJson(localSettings.Values["url"].ToString(), "", Convert.ToInt32(localSettings.Values["port"].ToString()));
                    //Sets the Event, wich is called, when the Interface of obenHAB is dowloaded and readed correctly
                    jsonDownloader.sh_downloadFinished += jsonDownloader_sh_downloadFinished;
                    settingsSetCorrectly = true;
                }
                catch (Exception ex)
                {

                }
            }
            /*
             * After the settings set correctly, the DispatcherTimer can start the work
             */
            //if (settingsSetCorrectly)
            //{
            /*
             * Sets the Timer, to call the JSON-Parser periodicly
             */
            if (localSettings.Values["refreshTime"] != null)
            {
                periodicTimer = ThreadPoolTimer.CreatePeriodicTimer((x) =>
                {
                    //Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    //{
                    //    //Use this function to access the GUI, from another Thread
                    //});
                    //parser.start();//OLD-FUNCTION
                    if (/*!MainPage._errorOccured &&*/ MainPage.taskReady)
                    {

                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                        {
                            //Use this function to access the GUI, from another Thread
                            taskObserver.Text = MainPage.taskReady.ToString();
                        });
                        MainPage.taskReady = false;
                        Task<String> xa = jsonDownloader.connectAndLoad();
                    }
                }, TimeSpan.FromSeconds(Convert.ToInt32(localSettings.Values["refreshTime"])));
            }
            else
            {
                periodicTimer = ThreadPoolTimer.CreatePeriodicTimer((x) =>
                {
                    //Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Task.Run(() =>
                    //{
                    //    //Use this function to access the GUI, from another Thread
                    //});
                    //parser.start();
                    if (/*!MainPage._errorOccured &&*/ MainPage.taskReady)
                    {

                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                        {
                            //Use this function to access the GUI, from another Thread
                            taskObserver.Text = MainPage.taskReady.ToString();
                        });
                        MainPage.taskReady = false;
                        Task<String> xa = jsonDownloader.connectAndLoad();

                    }
                }, TimeSpan.FromSeconds(5));
            }
            //}
            /*
             * DispatcherTimer to keep the Clock alive
             * */
            DispatcherTimer dt = new DispatcherTimer();
            dt.Tick += dt_Tick;
            dt.Start();
        }






        #endregion
        #region Clocktime in the right corner
        void dt_Tick(object sender, object e)
        {
            clockDayDate.Text = DateTime.Now.DayOfWeek.ToString();
            clockTime.Text = DateTime.Now.ToString();
        }
        #endregion

        #region All Methods to fill the GUI the first time and keep the GUI up-to-date
        Boolean once = false;
        LinkedList<AbstractWidget> currentWidgets = new LinkedList<AbstractWidget>();
        //Dictionary<String, Widget> currentWidgets = new Dictionary<String, Widget>();
        Widget tmp;
        int ix = 0;
        private async void jsonDownloader_sh_downloadFinished(JsonOpenHABDataContract result)
        {

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (once != true)
                {
                    LinkedListNode<Widget> first = result.homepage.widget.First;
                    while (first != null)
                    {
                        if (first != null)
                        {
                            if (first.Value.type.Equals("Frame") && !first.Value.label.Equals(""))
                            {
                                currentWidgets.AddFirst(new FrameWidget(first.Value));
                                if (first.Value.linkedPage != null || first.Value.widget != null)
                                {
                                    //leftNav.Items.Add(new FrameWidget(first.Value));
                                    if (first.Value.linkedPage != null)
                                        fillNavigation(first.Value.linkedPage.widget.First);
                                    if (first.Value.widget != null)
                                        fillNavigation(first.Value.widget.First);
                                }
                                else if (first.Value.linkedPage == null && first.Value.widget == null)
                                {
                                    mainNav.Items.Add(new FrameWidget(first.Value));
                                }
                            }

                            if (first.Value.type.Equals("Group"))
                            {
                                currentWidgets.AddFirst(new GroupWidget(first.Value));
                                if (first.Value.linkedPage != null || first.Value.widget != null)
                                {
                                    //leftNav.Items.Add(new GroupWidget(first.Value));
                                    if (first.Value.linkedPage != null)
                                        fillNavigation(first.Value.linkedPage.widget.First);
                                    if (first.Value.widget != null)
                                        fillNavigation(first.Value.widget.First);
                                }
                                else if (first.Value.linkedPage == null && first.Value.widget == null)
                                {
                                    mainNav.Items.Add(new GroupWidget(first.Value));
                                }
                            }
                            if (first.Value.type.Equals("Image"))
                            {
                                currentWidgets.AddFirst(new ImageWidget(first.Value));
                                if (first.Value.linkedPage != null || first.Value.widget != null)
                                {
                                    //leftNav.Items.Add(new ImageWidget(first.Value));
                                    if (first.Value.linkedPage != null)
                                        fillNavigation(first.Value.linkedPage.widget.First);
                                    if (first.Value.widget != null)
                                        fillNavigation(first.Value.widget.First);
                                }
                                else if (first.Value.linkedPage == null && first.Value.widget == null)
                                {
                                    mainNav.Items.Add(new ImageWidget(first.Value));
                                }
                            }
                            if (first.Value.type.Equals("Text"))
                            {
                                currentWidgets.AddFirst(new TextWidget(first.Value));
                                if (first.Value.linkedPage != null || first.Value.widget != null)
                                {
                                    //leftNav.Items.Add(new TextWidget(first.Value));
                                    if (first.Value.linkedPage != null)
                                        fillNavigation(first.Value.linkedPage.widget.First);
                                    if (first.Value.widget != null)
                                        fillNavigation(first.Value.widget.First);
                                }
                                else if (first.Value.linkedPage == null && first.Value.widget == null)
                                {
                                    mainNav.Items.Add(new TextWidget(first.Value));
                                }
                            }
                            //If the Label is empty
                            else if (first.Value.label.Equals(""))
                            {
                                if (first.Value.linkedPage != null)
                                    fillNavigation(first.Value.linkedPage.widget.First);
                                if (first.Value.widget != null)
                                    fillNavigation(first.Value.widget.First);
                            }
                            //else if (first.Value.linkedPage == null && first.Value.widget == null)
                            //{
                            //    mainNav.Items.Add(new TextWidget(first.Value));
                            //}
                        }
                        first = first.Next;
                    }
                    once = true;
                }
                updateUI();
            });
        }

        /**
         * Linkable Widgets START
         * */
        LinkedList<FrameWidget> currentFrameWidgets = new LinkedList<FrameWidget>();
        LinkedList<GroupWidget> currentGroupWidgets = new LinkedList<GroupWidget>();
        LinkedList<ImageWidget> currentImageWidgets = new LinkedList<ImageWidget>();
        LinkedList<TextWidget> currentTextWidgets = new LinkedList<TextWidget>();
        /*
         *Linkable Widgets END 
         * */

        /**
         * NonLinkable Widgets START
         * */
        LinkedList<ChartWidget> currentChartWidget = new LinkedList<ChartWidget>();
        LinkedList<ColorWidget> currentColorpickerWidget = new LinkedList<ColorWidget>();
        LinkedList<ListWidget> currentListWidget = new LinkedList<ListWidget>();
        LinkedList<SelectionWidget> currentSelectionWidget = new LinkedList<SelectionWidget>();
        LinkedList<SetpointWidget> currentSetpointWidget = new LinkedList<SetpointWidget>();
        LinkedList<SliderWidget> currentSliderWidget = new LinkedList<SliderWidget>();
        LinkedList<SwitchWidget> currentSwitchWidgets = new LinkedList<SwitchWidget>();
        LinkedList<VideoWidget> currentVideoWidget = new LinkedList<VideoWidget>();
        LinkedList<WebViewWidget> currentWebViewWidget = new LinkedList<WebViewWidget>();
        /*
         * NonLinkable Widgets END
         * */

        /// <summary>
        /// Function to clear the current widgets, to prevent the lists of overflow
        /// </summary>
        public void clearWidgetLists()
        {
            currentFrameWidgets.Clear();
            currentGroupWidgets.Clear();
            currentImageWidgets.Clear();
            currentTextWidgets.Clear();
            currentChartWidget.Clear();
            currentColorpickerWidget.Clear();
            currentListWidget.Clear();
            currentSelectionWidget.Clear();
            currentSetpointWidget.Clear();
            currentSliderWidget.Clear();
            currentSwitchWidgets.Clear();
            currentVideoWidget.Clear();
            currentWebViewWidget.Clear();
        }
        public void updateUI()
        {
            currentWidgets.Clear();
            clearWidgetLists();
            int counter = 0;
            AbstractWidget tmp1;
            while (counter != mainNav.Items.Count)
            {
                if (mainNav.Items[counter].GetType() == typeof(FrameWidget))
                {
                    currentFrameWidgets.AddFirst(mainNav.Items[counter] as FrameWidget);
                }
                if (mainNav.Items[counter].GetType() == typeof(GroupWidget))
                {
                    currentGroupWidgets.AddFirst(mainNav.Items[counter] as GroupWidget);
                }
                if (mainNav.Items[counter].GetType() == typeof(ImageWidget))
                {
                    currentImageWidgets.AddFirst(mainNav.Items[counter] as ImageWidget);
                }
                if (mainNav.Items.GetType() == typeof(TextWidget))
                {
                    currentTextWidgets.AddFirst(mainNav.Items[counter] as TextWidget);
                }
                if (mainNav.Items[counter].GetType() == typeof(ChartWidget))
                {
                    currentChartWidget.AddFirst(mainNav.Items[counter] as ChartWidget);
                }
                if (mainNav.Items[counter].GetType() == typeof(ColorWidget))
                {
                    currentColorpickerWidget.AddFirst(mainNav.Items[counter] as ColorWidget);
                }
                if (mainNav.Items[counter].GetType() == typeof(ListWidget))
                {
                    currentListWidget.AddFirst(mainNav.Items[counter] as ListWidget);
                }
                if (mainNav.Items[counter].GetType() == typeof(SelectionWidget))
                {
                    currentSelectionWidget.AddFirst(mainNav.Items[counter] as SelectionWidget);
                }
                if (mainNav.Items[counter].GetType() == typeof(SetpointWidget))
                {
                    currentSetpointWidget.AddFirst(mainNav.Items[counter] as SetpointWidget);
                }
                if (mainNav.Items[counter].GetType() == typeof(SliderWidget))
                {
                    currentSliderWidget.AddFirst(mainNav.Items[counter] as SliderWidget);
                }
                if (mainNav.Items[counter].GetType() == typeof(SwitchWidget))
                {
                    currentSwitchWidgets.AddFirst(mainNav.Items[counter] as SwitchWidget);
                }
                if (mainNav.Items[counter].GetType() == typeof(VideoWidget))
                {
                    currentVideoWidget.AddFirst(mainNav.Items[counter] as VideoWidget);
                }
                if (mainNav.Items[counter].GetType() == typeof(WebViewWidget))
                {
                    currentWebViewWidget.AddFirst(mainNav.Items[counter] as WebViewWidget);
                }
                counter++;
            }
            //LinkedListNode<AbstractWidget> first = currentWidgets.First;
            LinkedListNode<SwitchWidget> first = currentSwitchWidgets.First;
            Widget tmp;
            while (first != null)
            {
                JsonOpenHABDataContract.widgets.TryGetValue(first.Value.widgetId, out tmp);
                if (tmp != null)
                {
                    //if (mainNav.Items[0].GetType() == typeof(SwitchWidget))
                    //{
                    //    xx = (SwitchWidget)mainNav.Items[0];
                    //    xx.icon = tmp.icon;
                    //}
                    first.Value.icon = tmp.icon;
                    first.Value.label = tmp.label;
                    //if(!first.Value.item.state.Equals(tmp.item.state))
                    //{
                    first.Value.item = tmp.item;
                    if (tmp.item.state.Equals("ON"))
                    {
                        first.Value.guiState = true;
                    }
                    else if (tmp.item.state.Equals("OFF"))
                    {
                        first.Value.guiState = false;
                    }
                    //}

                }
                first = first.Next;
            }
            LinkedListNode<SliderWidget> first1 = currentSliderWidget.First;
            //Widget tmp;
            while (first1 != null)
            {
                JsonOpenHABDataContract.widgets.TryGetValue(first1.Value.widgetId, out tmp);
                if (tmp != null)
                {
                    //if (mainNav.Items[0].GetType() == typeof(SwitchWidget))
                    //{
                    //    xx = (SwitchWidget)mainNav.Items[0];
                    //    xx.icon = tmp.icon;
                    //}
                    first1.Value.icon = tmp.icon;
                    first1.Value.label = tmp.label;
                    //if(!first.Value.item.state.Equals(tmp.item.state))
                    //{
                    first1.Value.item = tmp.item;
                    //if (tmp.item.state.Equals("ON"))
                    //{
                    //    first1.Value.guiState = true;
                    //}
                    //else if (tmp.item.state.Equals("OFF"))
                    //{
                    //    first1.Value.guiState = false;
                    //}
                    //}

                }
                first1 = first1.Next;
            }
            JsonOpenHABDataContract.widgets.Clear();
            MainPage.taskReady = true;
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                //Use this function to access the GUI, from another Thread
                taskObserver.Text = MainPage.taskReady.ToString();
            });
        }
        #endregion

        #region Eventhandler to regocnize SelectionChanged in leftNav and mainNav
        private void leftNav_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int tmp = leftNav.Items.Count, cnt = 0; ;
            AbstractWidget current = null;
            if (leftNav.SelectedItem != null)
            {
                if (leftNav.SelectedItem != null && leftNav.SelectedItem.GetType() == typeof(FrameWidget))
                {
                    mainNav.Items.Clear();
                    FrameWidget currentFrameWidget = (FrameWidget)leftNav.SelectedItem;
                    if (currentFrameWidget.linkedPage != null)
                    {
                        currentFrameWidget.hasChildren = true;
                        LinkedListNode<Widget> first = currentFrameWidget.linkedPage.widget.First;
                        backupNavigation();
                        fillNavigation(first);
                    }
                    if (currentFrameWidget.widget != null)
                    {
                        currentFrameWidget.hasChildren = true;
                        LinkedListNode<Widget> first = currentFrameWidget.widget.First;
                        backupNavigation();
                        fillNavigation(first);
                    }
                }
                if (leftNav.SelectedItem != null && leftNav.SelectedItem.GetType() == typeof(GroupWidget))
                {
                    //Clears the mainNav to load new Items of GroupWidget
                    mainNav.Items.Clear();
                    GroupWidget currentFrameWidget = (GroupWidget)leftNav.SelectedItem;
                    Boolean isAnyGroup = false;
                    if (currentFrameWidget.linkedPage != null)
                    {
                        LinkedListNode<Widget> first = currentFrameWidget.linkedPage.widget.First;
                        LinkedListNode<Widget> tmpWidget = first;

                        while (tmpWidget != null)
                        {
                            if (tmpWidget.Value.type.Equals("Group"))
                                isAnyGroup = true;
                            tmpWidget = tmpWidget.Next;

                        }
                        if (isAnyGroup)
                        {
                            backupNavigation();
                        }
                        fillNavigation(first);

                    }
                    if (currentFrameWidget.widget != null)
                    {
                        LinkedListNode<Widget> first = currentFrameWidget.widget.First;
                        backupNavigation();
                        fillNavigation(first);
                    }
                }
                //if (leftNav.SelectedItem != null && leftNav.SelectedItem.GetType() == typeof(ImageWidget))
                //{
                //    ImageWidget currentFrameWidget = (ImageWidget)leftNav.SelectedItem;
                //    if (currentFrameWidget.linkedPage != null)
                //    {
                //        ;
                //    }
                //    if (currentFrameWidget.widget != null)
                //    {
                //        LinkedListNode<Widget> first = currentFrameWidget.widget.First;
                //        while (first != null)
                //        {

                //            if (first.Value.type.Equals("Group"))
                //            {
                //                int counter = 0;
                //                LinkedList<AbstractWidget> tmpWidgets = new LinkedList<AbstractWidget>();
                //                AbstractWidget tmpWidget;
                //                while (counter != leftNav.Items.Count)
                //                {
                //                    if (leftNav.Items[counter].GetType() == typeof(FrameWidget))
                //                        tmpWidgets.AddFirst((FrameWidget)leftNav.Items[counter]);
                //                    if (leftNav.Items[counter].GetType() == typeof(GroupWidget))
                //                        tmpWidgets.AddFirst((GroupWidget)leftNav.Items[counter]);
                //                    if (leftNav.Items[counter].GetType() == typeof(ImageWidget))
                //                        tmpWidgets.AddFirst((ImageWidget)leftNav.Items[counter]);
                //                    if (leftNav.Items[counter].GetType() == typeof(TextWidget))
                //                        tmpWidgets.AddFirst((TextWidget)leftNav.Items[counter]);
                //                    counter++;
                //                    //if (counter == leftNav.Items.Count)
                //                    //    leftNav.Items.Clear();
                //                }
                //                if (!clear)
                //                    leftNav.Items.Clear();
                //                clear = true;
                //                previusWidgets.Push(tmpWidgets);
                //            }
                //            leftNav.Items.Add(new GroupWidget(first.Value));
                //            first = first.Next;
                //        }
                //    }
                //}
                if (leftNav.SelectedItem != null && leftNav.SelectedItem.GetType() == typeof(TextWidget))
                {
                    mainNav.Items.Clear();
                    TextWidget currentFrameWidget = (TextWidget)leftNav.SelectedItem;
                    Boolean isAnyGroup = false;
                    if (currentFrameWidget.linkedPage != null)
                    {
                        LinkedListNode<Widget> first = currentFrameWidget.linkedPage.widget.First;

                        LinkedListNode<Widget> tmpWidget = first;

                        while (tmpWidget != null)
                        {
                            if (tmpWidget.Value.type.Equals("Group"))
                                isAnyGroup = true;
                            tmpWidget = tmpWidget.Next;

                        }
                        if (isAnyGroup)
                        {
                            backupNavigation();
                        }
                        fillNavigation(first);
                        //}
                    }
                    if (currentFrameWidget.widget != null)
                    {
                        LinkedListNode<Widget> first = currentFrameWidget.widget.First;
                        backupNavigation();
                        fillNavigation(first);
                    }
                }
            }
            _leftNavClearer = false;
        }

        private void mainNav_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainNav.SelectedItem.GetType() == typeof(FrameWidget))
            {
                FrameWidget currentFrameWidget = (FrameWidget)mainNav.SelectedItem;
            }
            if (mainNav.SelectedItem.GetType() == typeof(GroupWidget))
            {
                GroupWidget currentFrameWidget = (GroupWidget)mainNav.SelectedItem;
                if (currentFrameWidget.widget != null)
                {
                    LinkedListNode<Widget> first = currentFrameWidget.widget.First;
                    while (first != null)
                    {
                        mainNav.Items.Add(first.Value);
                        first = first.Next;
                    }
                }
                if (currentFrameWidget.linkedPage != null)
                {
                    LinkedListNode<Widget> first = currentFrameWidget.linkedPage.widget.First;
                    while (first != null)
                    {
                        if (first.Value.type.Equals("Group"))
                        {
                            leftNav.Items.Add(first.Value);
                        }
                        first = first.Next;
                    }
                }
            }
            if (mainNav.SelectedItem.GetType() == typeof(ImageWidget))
            {
                ImageWidget currentFrameWidget = (ImageWidget)mainNav.SelectedItem;
            }
            if (mainNav.SelectedItem.GetType() == typeof(TextWidget))
            {
                TextWidget currentFrameWidget = (TextWidget)mainNav.SelectedItem;
            }
        }


        #endregion
        #region Functions to change icons in mainNav and leftNav
        private LinkedList<AbstractWidget> temp_widgets = new LinkedList<AbstractWidget>();
        private void fillNavigation(LinkedListNode<Widget> widget)
        {
            LinkedListNode<Widget> first = widget;
            while (first != null)
            {
                if (first.Value.type.Equals("Frame") && (first.Value.widget != null && first.Value.widget.Count >= 1 || first.Value.linkedPage != null && first.Value.linkedPage.widget.Count >= 1))
                {
                    //if (first.Value.linkedPage != null && first.Value.linkedPage.widget.First.Value.type.Equals("Frame"))
                    //{
                    //    mainNav.Items.Add(new FrameWidget(first.Value));
                    //}
                    //if (first.Value.widget != null && first.Value.widget.First.Value.type.Equals("Frame"))
                    //{
                    //    mainNav.Items.Add(new FrameWidget(first.Value));
                    //}
                    //leftNav.Items.Add(new FrameWidget(first.Value));
                    mainNav.Items.Add(new FrameWidget(first.Value));
                    fillNavigation(first.Value.widget.First);
                    temp_widgets.AddFirst(new FrameWidget(first.Value));
                }
                else if (first.Value.type.Equals("Frame"))
                {
                    mainNav.Items.Add(new FrameWidget(first.Value));
                }
                if (first.Value.type.Equals("Group") && (first.Value.widget != null && first.Value.widget.Count >= 1 || first.Value.linkedPage != null && first.Value.linkedPage.widget.Count >= 1))// && first.Value.linkedPage.widget.First.Value.type.Equals("Group"))
                {
                    leftNav.Items.Add(new GroupWidget(first.Value));
                    temp_widgets.AddFirst(new GroupWidget(first.Value));
                }
                else if (first.Value.type.Equals("Group"))
                {
                    mainNav.Items.Add(new GroupWidget(first.Value));
                }
                if (first.Value.type.Equals("Image") && (first.Value.widget != null && first.Value.widget.Count >= 1 || first.Value.linkedPage != null && first.Value.linkedPage.widget.Count >= 1))
                {
                    leftNav.Items.Add(new ImageWidget(first.Value));
                    temp_widgets.AddFirst(new ImageWidget(first.Value));
                }
                else if (first.Value.type.Equals("Image"))
                {
                    mainNav.Items.Add(new ImageWidget(first.Value));
                }
                if (first.Value.type.Equals("Text") && (first.Value.widget != null && first.Value.widget.Count >= 1 || first.Value.linkedPage != null && first.Value.linkedPage.widget.Count >= 1))// && first.Value.linkedPage.widget.First.Value.type.Equals("Frame"))
                {
                    leftNav.Items.Add(new TextWidget(first.Value));
                    temp_widgets.AddFirst(new TextWidget(first.Value));
                }
                else if (first.Value.type.Equals("Text"))
                {
                    mainNav.Items.Add(new TextWidget(first.Value));
                }
                if (first.Value.type.Equals("Slider") && (first.Value.widget != null && first.Value.widget.Count >= 1 || first.Value.linkedPage != null && first.Value.linkedPage.widget.Count >= 1))
                {
                    leftNav.Items.Add(new SliderWidget(first.Value));
                    temp_widgets.AddFirst(new SliderWidget(first.Value));
                }
                else if (first.Value.type.Equals("Slider"))
                {
                    mainNav.Items.Add(new SliderWidget(first.Value));
                }
                if (first.Value.type.Equals("Switch") && (first.Value.widget != null && first.Value.widget.Count >= 1 || first.Value.linkedPage != null && first.Value.linkedPage.widget.Count >= 1))
                {
                    leftNav.Items.Add(new SwitchWidget(first.Value));
                    temp_widgets.AddFirst(new SwitchWidget(first.Value));
                }
                else if (first.Value.type.Equals("Switch"))
                {
                    mainNav.Items.Add(new SwitchWidget(first.Value));
                }
                if (first.Value.type.Equals("Video") && (first.Value.widget != null && first.Value.widget.Count >= 1 || first.Value.linkedPage != null && first.Value.linkedPage.widget.Count >= 1))
                {
                    leftNav.Items.Add(new VideoWidget(first.Value));
                    temp_widgets.AddFirst(new VideoWidget(first.Value));
                }
                else if (first.Value.type.Equals("Video"))
                {
                    mainNav.Items.Add(new VideoWidget(first.Value));
                }
                if (first.Value.type.Equals("NumberItem") && (first.Value.widget != null && first.Value.widget.Count >= 1 || first.Value.linkedPage != null && first.Value.linkedPage.widget.Count >= 1))
                {
                    leftNav.Items.Add(new NumberWidget(first.Value));
                    temp_widgets.AddFirst(new NumberWidget(first.Value));
                }
                else if (first.Value.type.Equals("NumberItem"))
                {
                    mainNav.Items.Add(new NumberWidget(first.Value));
                }
                if (first.Value.type.Equals("Colorpicker") && (first.Value.widget != null && first.Value.widget.Count >= 1 || first.Value.linkedPage != null && first.Value.linkedPage.widget.Count >= 1))
                {
                    leftNav.Items.Add(new ColorWidget(first.Value));
                    temp_widgets.AddFirst(new ColorWidget(first.Value));
                }
                else if (first.Value.type.Equals("Colorpicker"))
                {
                    mainNav.Items.Add(new ColorWidget(first.Value));
                }
                if (first.Value.type.Equals("Selection") && (first.Value.widget != null && first.Value.widget.Count >= 1 || first.Value.linkedPage != null && first.Value.linkedPage.widget.Count >= 1))
                {
                    leftNav.Items.Add(new SelectionWidget(first.Value));
                    temp_widgets.AddFirst(new SelectionWidget(first.Value));
                }
                else if (first.Value.type.Equals("Selection"))
                {
                    mainNav.Items.Add(new SelectionWidget(first.Value));
                }
                if (first.Value.type.Equals("Setpoint") && (first.Value.widget != null && first.Value.widget.Count >= 1 || first.Value.linkedPage != null && first.Value.linkedPage.widget.Count >= 1))
                {
                    leftNav.Items.Add(new SetpointWidget(first.Value));
                    temp_widgets.AddFirst(new SetpointWidget(first.Value));
                }
                else if (first.Value.type.Equals("Setpoint"))
                {
                    mainNav.Items.Add(new SetpointWidget(first.Value));
                }
                if (first.Value.type.Equals("Chart") && (first.Value.widget != null && first.Value.widget.Count >= 1 || first.Value.linkedPage != null && first.Value.linkedPage.widget.Count >= 1))
                {
                    leftNav.Items.Add(new ChartWidget(first.Value));
                    temp_widgets.AddFirst(new ChartWidget(first.Value));
                }
                else if (first.Value.type.Equals("Chart"))
                {
                    mainNav.Items.Add(new ChartWidget(first.Value));
                }
                //if (!first.Value.type.Equals("Frame") || first.Value.type.Equals("Group") || first.Value.type.Equals("Image") || first.Value.type.Equals("Switch") || first.Value.type.Equals("Slider") || first.Value.type.Equals("Text") || first.Value.type.Equals("Chart"))
                //{

                //}
                //else if(first.Value.widget != null || first.Value.linkedPage != null)
                //{
                //    leftNav.Items.Add(first.Value.type);
                //}
                //else if(first.Value.widget == null && first.Value.linkedPage == null)
                //{
                //    mainNav.Items.Add(first.Value);
                //}
                //if (first.Value.type.Equals("Webview") && (first.Value.widget != null || first.Value.linkedPage != null))
                //    leftNav.Items.Add(new WebViewWidget(first.Value));
                //else if (first.Value.type.Equals("Webview"))
                //    mainNav.Items.Add(new WebViewWidget(first.Value));
                first = first.Next;
            }
            //Frames.Source = temp_widgets;
        }
        private void backupNavigation()
        {
            //if (first.Value.type.Equals("Group"))
            //{

            int counter = 0;
            LinkedList<AbstractWidget> tmpWidgets = new LinkedList<AbstractWidget>();
            while (counter != leftNav.Items.Count)
            {
                if (leftNav.Items[counter].GetType() == typeof(FrameWidget))
                    tmpWidgets.AddFirst(leftNav.Items[counter] as FrameWidget);
                if (leftNav.Items[counter].GetType() == typeof(GroupWidget))
                    tmpWidgets.AddFirst(leftNav.Items[counter] as GroupWidget);
                if (leftNav.Items[counter].GetType() == typeof(ImageWidget))
                    tmpWidgets.AddFirst(leftNav.Items[counter] as ImageWidget);
                if (leftNav.Items[counter].GetType() == typeof(TextWidget))
                    tmpWidgets.AddFirst(leftNav.Items[counter] as TextWidget);
                counter++;

            }
            if (!_leftNavClearer)
                leftNav.Items.Clear();
            _leftNavClearer = true;
            previusWidgets.Push(tmpWidgets);
            //}
        }
        #endregion

        #region Button to call the Settingsflyout
        private void OpenSettingsFlyout_Tapped(object sender, TappedRoutedEventArgs e)
        {
            WinHABSettingsFlyout winHABSettingsFlyout = new WinHABSettingsFlyout();
            winHABSettingsFlyout.Show();
            //this.Frame.Navigate(typeof(Settings.Settings));
        }
        #endregion

        #region Eventhandler for toggling switches
        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            Type xType = sender.GetType();
            ToggleSwitch x = (ToggleSwitch)sender;
            Item xx = (Item)x.Tag;
            if (xx.state.Equals("ON") != x.IsOn && xx.state.Equals("OFF") == x.IsOn)
            {
                if (xx.state.Equals("ON"))
                {
                    sendMessage.sendInstruction(xx, "OFF");
                }
                else if (xx.state.Equals("OFF"))
                {
                    sendMessage.sendInstruction(xx, "ON");
                }
            }
        }
        #endregion
        #region Eventhandler for backward and forwardbutton
        private void backButton_Tapped(object sender, TappedRoutedEventArgs e)
        {

            //if (previusWidgets.Pop() != null)
            //{

            try
            {

                LinkedList<AbstractWidget> popList = previusWidgets.Pop(); mainNav.Items.Clear();
                leftNav.Items.Clear();
                previusWidgets.Reverse();
                LinkedListNode<AbstractWidget> first = popList.Last;
                while (first != null)
                {
                    if (first.Value.GetType() == typeof(FrameWidget))
                        leftNav.Items.Add(new FrameWidget(first.Value as FrameWidget));
                    if (first.Value.GetType() == typeof(GroupWidget))
                        leftNav.Items.Add(new GroupWidget(first.Value as GroupWidget));
                    if (first.Value.GetType() == typeof(ImageWidget))
                        leftNav.Items.Add(new ImageWidget(first.Value as ImageWidget));
                    if (first.Value.GetType() == typeof(TextWidget))
                        leftNav.Items.Add(new TextWidget(first.Value as TextWidget));
                    first = first.Previous;
                }
                //fillNavigation(first);
            }
            catch (Exception ex)//Catches exception if the Stack is empty
            {

            }
            //}
        }

        private void forwardButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ConfigureFirstStart));
        }
        #endregion

        /// <summary>
        /// Is used to restart the HttpClient when the Connection was lost or an error ocurred
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //MessageDialog dialog = new MessageDialog("An Error appeared, while refreshing the page manualy.", "Refreshingerror");
            //dialog.Options = MessageDialogOptions.AcceptUserInputAfterDelay;

            //IUICommand command = await dialog.ShowAsync();
            MainPage.taskReady = true;
            MainPage._errorOccured = false;
        }

        private void colorDialogButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ColorDialog.ColorDialog));
        }


        /// <summary>
        /// If there is a Widget, that contains a Slider.
        /// The Slider-Event is read and send to the PostMessage-Object 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderWidgetsldr_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (sender.GetType() == typeof(Slider))
            {
                Slider slider = sender as Slider;
                Item item = slider.Tag as Item;
                int i = 0;
                sendMessage.sendInstruction(item, Convert.ToString(slider.Value));
            }
        }

        private void colorPickerButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Button button = sender as Button;
            Item item = button.Tag as Item;
            ColorDialogFlyout colorDialogSettingsFlyout = new ColorDialogFlyout(item);
            colorDialogSettingsFlyout.Show();
        }

    }
}



