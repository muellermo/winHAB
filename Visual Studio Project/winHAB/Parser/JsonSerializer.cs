using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Networking.Sockets;
using Windows.Security.Cryptography;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using winHAB.Parser;

namespace JSON_Parser.Parser
{
    public class JsonSerializer
    {
        #region Variablen
        private Windows.Web.Http.HttpClient httpClient;
        //private HttpClient httpClient;// = new HttpClient();
        private HttpClientHandler httpClientHandler = new HttpClientHandler();
        private String sh_url { get; set; }
        private String sh_path { get; set; }
        private Int32 sh_port { get; set; }
        private String sh_username { get; set; }
        private String sh_passwort { get; set; }
        private UriBuilder sh_uriBuilder { get; set; }
        public delegate void jsonReady();
        public event jsonReady sh_jsonReadyEvent;
        public String json { get; set; }
        public DataRecycler dataRecycler { get; set; }
        public JsonOpenHABDataContract sh_jsonresult { get; set; }
        public static JsonSerializer instance { get; set; }
        public LinkedList<Widget> widgetList { get; set; }
        public ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        #endregion
        #region Constructor
        /// <summary>
        /// Instantiate a Class to deserialize the JSON-Stream of an openhab-Server
        /// </summary>
        /// <param name="sh_url">URL to Server</param>
        /// <param name="sh_path">Path to JSON</param>
        /// <param name="sh_port">Serverport</param>
        /// <param name="sh_username">Username</param>
        /// <param name="sh_password">Passwort</param>
        //public JSONParser(String sh_url, String sh_path, Int32 sh_port, String sh_username, String sh_password)
        //{
        //    //HttpBaseProtocolFilter b = new HttpBaseProtocolFilter();
        //    //b.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Expired);
        //    //b.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Untrusted);
        //    ////httpClientHandler = new HttpClientHandler();
        //    HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();

        //    filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Untrusted);
        //    filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Expired);
        //    filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.InvalidName);
        //    filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.IncompleteChain);
        //    //filter.ServerCredential = new Windows.Security.Credentials.PasswordCredential("https", localSettings.Values["username"].ToString(), localSettings.Values["password"].ToString());

        //    httpClientHandler.Credentials = new System.Net.NetworkCredential(localSettings.Values["username"].ToString(), localSettings.Values["password"].ToString());
        //    httpClient = new Windows.Web.Http.HttpClient(filter);
        //    httpClient.DefaultRequestHeaders.Add("Accept-Charset", "utf-8");
        //    this.sh_url = sh_url;
        //    this.sh_path = "/rest/sitemaps/demo?type=json";
        //    this.sh_port = sh_port;
        //    this.sh_username = sh_username;
        //    this.sh_passwort = sh_passwort;
        //    this.sh_uriBuilder = new UriBuilder("https", this.sh_url, this.sh_port, "/rest/sitemaps/demo?type=json");
        //    dataRecycler = new DataRecycler();
        //    JSONParser.instance = this;
        //}
        #endregion



        #region Starts the parsing-Algorithm
        /// <summary>
        /// Starts the Parsing-Algorithm --> needs to be called in an async Method WITHOUT the await-Modifier
        /// </summary>
        public async void start()
        {
            this.readJSON(sh_uriBuilder.Uri);
        }
        #endregion




        #region Send a request to a websocket and reads and parses the JSON-Stream

        /// <summary>
        /// Reads the JSON-String and parses it to a SmartHomeDataContract
        /// </summary>
        /// <param name="sh_uri"></param>

        private async void readJSON(Uri sh_uri)
        {
            sh_jsonresult = null;
            String jhx = sh_uri.Scheme + "://" + sh_uri.Authority + this.sh_path;

            //Auth befor send Request to Server
            //String username = "abc";
            //String password = "cde";
            //String credentials = username + ":" + password;
            //UTF8Encoding encoder = new UTF8Encoding();
            //Byte[] bytes = encoder.GetBytes(credentials);
            //IBuffer buffer = CryptographicBuffer.DecodeFromBase64String(credentials);
            //String strBase64New = CryptographicBuffer.EncodeToBase64String(buffer);
            //httpClient.DefaultRequestHeaders.Add("Authorization", "Basic "+strBase64New);
            Uri uri = new Uri(jhx);

            //httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new Windows.Web.Http.Headers.HttpContentCodingWithQualityHeaderValue("UTF-8"));
            Windows.Web.Http.HttpResponseMessage sh_httpResponseMessage = await httpClient.GetAsync(uri);
            String xxxx = sh_httpResponseMessage.Content.ToString();
            //IHttpContent content = sh_httpResponseMessage.Content;
            //String json = await content.ReadAsStringAsync();
            //StringReader reader = new StringReader(json);
            String xxx = sh_httpResponseMessage.Content.ReadAsStringAsync().GetResults();
            MemoryStream stream = new MemoryStream(UTF8Encoding.Unicode.GetBytes(xxx));//(Encoding.UTF8.GetBytes(xxx));

            //StreamWriter writer = new StreamWriter(stream);
            //writer.Write(json);
            //writer.Flush();
            //stream.Position = 0;
            //FileOutputStream stream ;
            //await sh_httpResponseMessage.Content.WriteToStreamAsync(stream);
            DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
            DataContractJsonSerializer jsonSerielizer = new DataContractJsonSerializer(typeof(JsonOpenHABDataContract));
            //reader.Position = 0;
            sh_jsonresult = (JsonOpenHABDataContract)jsonSerielizer.ReadObject(stream);
            if (!once)
            {
                x = sh_jsonresult;
                once = true;
            }
            sh_jsonReadyEvent();
        }
        public Boolean once = false;
        JsonOpenHABDataContract x;
        #endregion


        #region get the result of parsed JSON
        /// <summary>
        /// This function returns the SmartHomeDataContract-Object, wich contains all Elements, received by JSON
        /// </summary>
        /// <returns>SmartHomeDataContract</returns>
        public JsonOpenHABDataContract getJsonResult()
        {
            return sh_jsonresult;
        }
        #endregion

        public JsonSerializer()
        {

        }

        public void parseStart(String json)
        {
            parse(json);
        }
        public JsonOpenHABDataContract parse(String json)
        {
            //XMLOpenHABDataContract sh_jsonresult1;
            //json = json.Replace("\\\\", "\\");;//.Replace("\\\\", "\\").Replace("\\\"", "\"");
            //JsonOpenHABDataContract.widgets.Clear();
            MemoryStream stream = new MemoryStream(UTF8Encoding.UTF8.GetBytes(json));
            stream.Position = 0;
            //XmlSerializer x = new XmlSerializer(typeof(XMLOpenHABDataContract));
            //try
            //{
            //    sh_jsonresult1 = (XMLOpenHABDataContract)x.Deserialize(stream);
            //}
            //catch (Exception ex)
            //{
            //    int i = 0;
            //}
            DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
            settings.UseSimpleDictionaryFormat = true;
            DataContractJsonSerializer jsonSerielizer = new DataContractJsonSerializer(typeof(JsonOpenHABDataContract), settings);
            ////jsonSerielizer.WriteObject(stream, sh_jsonresult);
            sh_jsonresult = (JsonOpenHABDataContract)jsonSerielizer.ReadObject(stream);
            //readx();
            return sh_jsonresult;
        }
        //public async void readx()
        //{
        //    FileOpenPicker fileOpenPicker = new FileOpenPicker();
        //    fileOpenPicker.FileTypeFilter.Add(".xml");
        //    StorageFile storageFile = await fileOpenPicker.PickSingleFileAsync();
        //    Stream streama = await storageFile.OpenStreamForReadAsync();
        //    XmlReader reader = XmlReader.Create(streama);
        //}
    }
}
