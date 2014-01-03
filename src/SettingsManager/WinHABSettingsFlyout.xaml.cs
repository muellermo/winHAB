using JSON_Parser.SettingsManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using winHAB.communication;

// Die Elementvorlage "Einstellungs-Flyout" ist unter http://go.microsoft.com/fwlink/?LinkId=273769 dokumentiert.

namespace winHAB.SettingsManager
{
    public sealed partial class WinHABSettingsFlyout
    {
        private Settings settings = new Settings();
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        PasswordVault passwordVault = new PasswordVault();
        public WinHABSettingsFlyout()
        {
            this.InitializeComponent();
            
            expertGrid.Visibility = Visibility.Collapsed;
            if (localSettings.Values["username"] != null)
            {
                userNameTextBox.Text = localSettings.Values["username"].ToString();
                //Do this only when a Password is needed
                if (Convert.ToInt32(localSettings.Values["connectionTypeChooser"].ToString()).Equals(0) && Convert.ToInt32(localSettings.Values["connectionTypeChooser"].ToString()).Equals(2))
                    try
                    {
                        //only on the first startup, if the Pasword isnt stored 
                        passwordTextBox.Password = passwordVault.Retrieve("winHAB", localSettings.Values["username"].ToString()).Password;
                    }
                    catch
                    {

                    }
            }
            if (localSettings.Values["password"] != null)
            {

            }
            if (localSettings.Values["url"] != null)
            {
                urlTextBox.Text = localSettings.Values["url"].ToString();
            }
            if (localSettings.Values["port"] != null)
            {
                portTextBox.Text = localSettings.Values["port"].ToString();
            }
            if (localSettings.Values["refreshTime"] != null)
            {
                refreshTrackBar.Value = Convert.ToInt32(localSettings.Values["refreshTime"].ToString());
            }
            if (localSettings.Values["connectionTypeChooser"] != null)
            {
                connectionTypeChooser.SelectedIndex = Convert.ToInt32(localSettings.Values["connectionTypeChooser"].ToString());
            }


        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (expertViewSwitch.IsOn)
            {
                expertGrid.Visibility = Visibility.Visible;
            }
            else if (!expertViewSwitch.IsOn)
            {
                expertGrid.Visibility = Visibility.Collapsed;
            }
        }


        private void RichTextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
        private void realySave_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //HTTPSwithUsernameAndPassword
            if (connectionTypeChooser.SelectedIndex.Equals(0))
            {
                settings.username = userNameTextBox.Text;
                settings.password = passwordTextBox.Password;
                settings.sh_url = urlTextBox.Text;
                settings.connectionType = ConnectionTypesEnum.HTTPSwithUsernameAndPassword.ToString();
                settings.connectionTypeChooser = 0;
                if (!portTextBox.Text.Equals(""))
                {
                    settings.sh_port = Convert.ToInt32(portTextBox.Text);
                }
                if (refreshTrackBar.Value != 5)
                {
                    settings.refresh_time = Convert.ToInt32(refreshTrackBar.Value);
                }
                settings.saveSettings();
            }
            //HTTPSwithoutUsernameAndPassword
            if (connectionTypeChooser.SelectedIndex.Equals(1))
            {
                settings.sh_url = urlTextBox.Text;
                settings.connectionType = ConnectionTypesEnum.HTTPSwithoutUsernameAndPassword.ToString();
                settings.connectionTypeChooser = 1;
                if (!portTextBox.Text.Equals(""))
                {
                    settings.sh_port = Convert.ToInt32(portTextBox.Text);
                }
                if (refreshTrackBar.Value != 5)
                {
                    settings.refresh_time = Convert.ToInt32(refreshTrackBar.Value);
                }
                settings.saveSettings();
            }
            //HTTPwithUsernameAndPassword
            if (connectionTypeChooser.SelectedIndex.Equals(2))
            {
                settings.username = userNameTextBox.Text;
                settings.password = passwordTextBox.Password;
                settings.sh_url = urlTextBox.Text;
                settings.connectionType = ConnectionTypesEnum.HTTPwithUsernameAndPassword.ToString();
                settings.connectionTypeChooser = 2;
                if (!portTextBox.Text.Equals(""))
                {
                    settings.sh_port = Convert.ToInt32(portTextBox.Text);
                }
                if (refreshTrackBar.Value != 5)
                {
                    settings.refresh_time = Convert.ToInt32(refreshTrackBar.Value);
                }
                settings.saveSettings();
            }
            //HTTPwithoutUsernameAndPassword
            if (connectionTypeChooser.SelectedIndex.Equals(3))
            {
                settings.sh_url = urlTextBox.Text;
                settings.connectionType = ConnectionTypesEnum.HTTPwithoutUsernameAndPassword.ToString();
                settings.connectionTypeChooser = 3;
                if (!portTextBox.Text.Equals(""))
                {
                    settings.sh_port = Convert.ToInt32(portTextBox.Text);
                }
                if (refreshTrackBar.Value != 5)
                {
                    settings.refresh_time = Convert.ToInt32(refreshTrackBar.Value);
                }
                settings.saveSettings();
            }
            Application.Current.Exit();
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SecuritySettingsFlyout securitySettings = new SecuritySettingsFlyout();
            securitySettings.Show();
        }

        private void generateURIButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ComboBoxItem x = protocollChooser.SelectedItem as ComboBoxItem;
            urlTextBox.Text = x.Content.ToString() + "://" + urlTextBox1.Text +":"+ portTextBox.Text.ToString() + "/rest/sitemaps/" + sitemapTextBox.Text + "?type=json";
        }
    }
}
