using JSON_Parser.Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace winHAB.communication
{
    class GetJSON
    {
        #region Variablen
        private String sh_url { get; set; }
        private String sh_path { get; set; }
        private Int32 sh_port { get; set; }
        private String sh_username { get; set; }
        private String sh_passwort { get; set; }
        private UriBuilder sh_uriBuilder { get; set; }
        public delegate void jsonReady(JsonOpenHABDataContract result);
        public event jsonReady sh_downloadFinished;
        //public String json { get; set; }
        public Boolean secured { get; set; }
        private HttpClient httpClient;
        private HttpBaseProtocolFilter filter;
        private UriBuilder uriToOpenHAB { get; set; }
        private HttpResponseMessage httpResponseMessage { get; set; }
        private JSONParser parser = new JSONParser();
        private Boolean _toastOnce = false;
        #endregion
        #region Constructors
        /// <summary>
        /// Constructor with encryption username and password
        /// </summary>
        /// <param name="sh_url">URL to Server</param>
        /// <param name="sh_path">Path to JSON</param>
        /// <param name="sh_port">Serverport</param>
        /// <param name="sh_username">Username</param>
        /// <param name="sh_password">Passwort</param>
        public GetJSON(String sh_url, String sh_path, Int32 sh_port, String sh_username, String sh_password, Boolean isHTTPSOn)
        {
            this.sh_url = sh_url;
            this.sh_path = sh_path;
            this.sh_port = sh_port;
            this.sh_username = sh_username;
            this.sh_passwort = sh_passwort;
            this.sh_uriBuilder = new UriBuilder("http", this.sh_url, this.sh_port, "/rest/sitemaps/");
            this.secured = isHTTPSOn;
            if (secured)
            {
                try
                {
                    uriToOpenHAB = new UriBuilder(sh_url);
                }
                catch
                {
                    //Happens when the HostName could not parsed
                }
                filter = new HttpBaseProtocolFilter();
                /**
                 * Ignore some Certifcateerrors
                 * */
                filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Untrusted);
                filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Expired);
                filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.InvalidName);
                filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.IncompleteChain);
                filter.ServerCredential = new PasswordCredential("winHAB", this.sh_username, this.sh_passwort);
                /**
                 * Establish HttpClient
                 * */
                httpClient = new HttpClient(filter);
            }
            else if (!secured)
            {

            }
        }
        /// <summary>
        /// Constructur with encryption but WITHOUT username and password
        /// </summary>
        /// <param name="sh_url"></param>
        /// <param name="sh_path"></param>
        /// <param name="sh_port"></param>
        /// <param name="isHTTPSOn"></param>
        public GetJSON(String sh_url, String sh_path, Int32 sh_port, Boolean isHTTPSOn)
        {
            this.sh_url = sh_url;
            this.sh_path = sh_path;
            this.sh_port = sh_port;
            this.secured = isHTTPSOn;
            if (secured)
            {
                uriToOpenHAB = new UriBuilder(this.sh_url);
                filter = new HttpBaseProtocolFilter();
                /**
                 * Ignore some Certifcateerrors
                 * */
                filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Untrusted);
                filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Expired);
                filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.InvalidName);
                filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.IncompleteChain);
                /**
                 * Establish HttpClient
                 * */
                httpClient = new HttpClient(filter);
            }
            else if (!secured)
            {

            }

        }
        /// <summary>
        /// Constructor without encryption but with username and password
        /// </summary>
        /// <param name="sh_url"></param>
        /// <param name="sh_path"></param>
        /// <param name="sh_port"></param>
        /// <param name="sh_username"></param>
        /// <param name="sh_password"></param>
        public GetJSON(String sh_url, String sh_path, Int32 sh_port, String sh_username, String sh_password)
        {
            this.sh_username = sh_username;
            this.sh_passwort = sh_passwort;
            this.sh_url = sh_url;
            this.sh_path = sh_path;
            this.sh_port = sh_port;
            try
            {
                uriToOpenHAB = new UriBuilder(sh_url);
            }
            catch
            {
                //Happens when the HostName could not parsed
            }
            filter = new HttpBaseProtocolFilter();
            filter.ServerCredential = new PasswordCredential("winHAB", this.sh_username, this.sh_passwort);
            /**
             * Establish HttpClient
             * */
            httpClient = new HttpClient(filter);

        }
        /// <summary>
        /// Constructor without encryption, username and password
        /// </summary>
        /// <param name="sh_url"></param>
        /// <param name="sh_path"></param>
        /// <param name="sh_port"></param>
        public GetJSON(String sh_url, String sh_path, Int32 sh_port)
        {
            uriToOpenHAB = new UriBuilder(sh_url);
            this.sh_url = sh_url;
            this.sh_path = sh_path;
            this.sh_port = sh_port;
            try
            {
                uriToOpenHAB = new UriBuilder(sh_url);
            }
            catch
            {
                //Happens when the HostName could not parsed
            }
            httpClient = new HttpClient();
        }
        #endregion




        #region Establish Connection happend here
        private String tmpjson = "";
        public async void connectAndLoad()
        {


            //try
            //{
            //Unsafe Operation, when the 

            //httpResponseMessage = await httpClient.GetStringAsync(uriToOpenHAB.Uri);//.GetAsync(uriToOpenHAB.Uri);
            if (filter != null)
            {
                
                httpClient = new HttpClient(this.filter); if (httpResponseMessage != null)
                    tmpjson = httpResponseMessage.Content.ToString();
            }
            else
            {
                httpClient = new HttpClient(); if (httpResponseMessage != null)
                    tmpjson = httpResponseMessage.Content.ToString();
            }
            httpResponseMessage = await httpClient.GetAsync(uriToOpenHAB.Uri); //httpResponseMessage.Content.ToString();
            String json = httpResponseMessage.Content.ToString();
            //if(json.Contains("\"widget\":{"))
            //{
            //    int index = json.IndexOf("\"widget\":{");
            //    char y = json[index+8];
            //    json.Insert(index, "[");
            //    ;
            //}
            //StorageFolder storageFolder = KnownFolders.DocumentsLibrary;
            //Random rnd = new Random();
            //String fileName =Convert.ToString(rnd.Next())+".txt";
            //StorageFile storageFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            if (!tmpjson.Equals(json))
                System.Diagnostics.Debug.WriteLine("");
            //httpClient.Dispose();
            sh_downloadFinished(parser.parse(json));
            //}
            //catch (Exception ex)
            //{
            //    if (!_toastOnce)
            //    {
            //Show Toast, if there was an Error
            //ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
            //XmlDocument toastXML = ToastNotificationManager.GetTemplateContent(toastTemplate);
            //XmlNodeList toastTextElements = toastXML.GetElementsByTagName("text");
            //toastTextElements[0].AppendChild(toastXML.CreateTextNode("An Error occured, while setting up the Connection" + ex.StackTrace));
            //XmlNodeList toastImageElements = toastXML.GetElementsByTagName("image");
            //((XmlElement)toastImageElements[0]).SetAttribute("src", "ms-appx:///Assets/images/icon.png");
            //((XmlElement)toastImageElements[0]).SetAttribute("alt", "red graphic");
            //ToastNotification toast = new ToastNotification(toastXML);
            //ToastNotificationManager.CreateToastNotifier().Show(toast);
            //_toastOnce = true;


            //MessageDialog dialog = new MessageDialog(ex.Message + "\n" + ex.StackTrace, "Error while sending a request");
            //dialog.Options = MessageDialogOptions.AcceptUserInputAfterDelay;


            //dialog.ShowAsync();
            //}
            /*                    try
                {
                    UriBuilder uri = new UriBuilder(item.link);

                    HttpResponseMessage response = await httpClient.PostAsync(uri.Uri, new HttpStringContent(method));
                }
                catch (Exception ex)
                {
                    MessageDialog dialog = new MessageDialog(ex.Message + "\n" + ex.StackTrace, "Error while sending a request");
                    dialog.Options = MessageDialogOptions.AcceptUserInputAfterDelay;
                    dialog.ShowAsync();
                }
                finally
                {

                }*/
            //}

        }
        #endregion

    }
}