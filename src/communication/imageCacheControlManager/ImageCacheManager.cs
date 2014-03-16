using JSON_Parser.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace winHAB.communication.imageCacheControlManager
{
    class ImageCacheManager
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
        private JsonSerializer parser = new JsonSerializer();
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
        public ImageCacheManager(String sh_url, String sh_path, Int32 sh_port, String sh_username, String sh_password, Boolean isHTTPSOn)
        {
            this.sh_url = sh_url;
            this.sh_path = sh_path;
            this.sh_port = sh_port;
            this.sh_username = sh_username;
            this.sh_passwort = sh_passwort;
            //this.sh_uriBuilder = new UriBuilder("http", this.sh_url, this.sh_port, "/rest/sitemaps/");
            this.secured = isHTTPSOn;
            if (secured)
            {
                try
                {
                    uriToOpenHAB = new UriBuilder(sh_url);
                }
                catch (ArgumentNullException ex)
                {
                    //Happens when the HostName could not parsed
                }
                catch (FormatException ex)
                {
                    //Is equal to UriFormatException
                }
                filter = new HttpBaseProtocolFilter();
                /**
                 * Ignore some Certifcateerrors
                 * */
                filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Untrusted);
                filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Expired);
                filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.InvalidName);
                filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.IncompleteChain);
                filter.ServerCredential = new PasswordCredential("winHAB", this.sh_username, "0p3nhab");// this.sh_passwort);
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
        public ImageCacheManager(String sh_url, String sh_path, Int32 sh_port, Boolean isHTTPSOn)
        {
            this.sh_url = sh_url;
            this.sh_path = sh_path;
            this.sh_port = sh_port;
            this.secured = isHTTPSOn;
            if (secured)
            {
                try
                {
                    uriToOpenHAB = new UriBuilder(sh_url);
                }
                catch (ArgumentNullException ex)
                {
                    //Happens when the HostName could not parsed
                }
                catch (FormatException ex)
                {
                    //Is equal to UriFormatException
                }
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
        public ImageCacheManager(String sh_url, String sh_path, Int32 sh_port, String sh_username, String sh_password)
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
            catch (ArgumentNullException ex)
            {
                //Happens when the HostName could not parsed
            }
            catch (FormatException ex)
            {
                //Is equal to UriFormatException
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
        public ImageCacheManager(String sh_url, String sh_path, Int32 sh_port)
        {
            uriToOpenHAB = new UriBuilder(sh_url);
            this.sh_url = sh_url;
            this.sh_path = sh_path;
            this.sh_port = sh_port;
            try
            {
                uriToOpenHAB = new UriBuilder(sh_url);
            }
            catch (ArgumentNullException ex)
            {
                //Happens when the HostName could not parsed
            }
            catch (FormatException ex)
            {
                //Is equal to UriFormatException
            }
            httpClient = new HttpClient();
        }
        #endregion

        #region all Methods to refresh the Imagecach and save them to Appfolder
        #endregion
    }
}
