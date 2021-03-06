//**************************//
//**************************//
//                          //
//       Writed By          //
//    milokz@gmail.com      //
//                          //
//**************************//
//**************************//
//                          //
//  Tested on               //
//  GNU Wget 1.19.4 built   //
//             on mingw32   //
//  cUrl 7.78.0 win32 mingw // 
//                          //
//**************************//
//**************************//

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using dkxce.LibCurlNet;

namespace System.Net
{
    /// <summary>
    ///     Using wGet.exe to HTTPRequest
    ///     Place wGet.exe to application exe folder
    ///     Home Page: http://www.gnu.org/software/wget/wget.html
    ///     Manual: http://www.gnu.org/software/wget/manual/
    /// </summary>
    public class HttpWGetExeRequest
    {
        #region private vars
        private string _Url = "http://localhost/";
        private string _UserAgent = "";
        private string _ExePath = AppDomain.CurrentDomain.BaseDirectory.Trim('\\') + @"\";
        private string _ExeName = "wget.exe";
        private string _HTTPMethod = "GET";
        private string _LogFile = null;
        private bool _Debug = false;
        private bool _Quiet = false;
        private bool _Verbose = true;
        private bool _ServerResponse = false;
        private int _Timeout = 900;
        private bool _NoProxy = false;
        private bool _NoDNSCache = false;
        private string _HTTPUser = null;
        private string _HTTPPassword = null;
        private System.Text.Encoding _LocalEncoding = System.Text.Encoding.UTF8;
        private System.Text.Encoding _RemoteEncoding = System.Text.Encoding.UTF8;
        private bool _NoHttpKeepAlive = false;
        private bool _NoCache = false;
        private bool _NoCookies = false;
        private string _LoadCookiesFile = null;
        private string _SaveCookiesFile = null;
        private bool _IgnoreLength = false;
        private string[] _Headers = null;
        private string _ProxyUser = null;
        private string _ProxyPassword = null;
        private string _Referer = null;
        private bool _SaveHeaders = true;

        private string _POSTDATA = null;
        private string _BODYDATA = null;
        private string _PostFile = null;
        private string _BodyFile = null;
        private bool _NoCheckCertificate = true;
        private bool _HTTPSOnly = false;
        private string _SecureProtocol = "auto";
        private string _OutputFile = "-";
        private bool _ExeWindow = false;
        private string _ExeUser = null;
        private string _ExePassword = null;
        private bool _ContentOnError = true;
        private string _CustomParameters = null;
        #endregion

        /// <summary>
        ///     Using wGet.exe to HTTPRequest
        ///     Place wGet.exe to application exe folder
        ///     Home Page: http://www.gnu.org/software/wget/wget.html
        ///     Manual: http://www.gnu.org/software/wget/manual/
        /// </summary>
        public HttpWGetExeRequest(){ }

        //// <summary>
        ///     Using wGet.exe to HTTPRequest
        ///     Place wGet.exe to application exe folder
        ///     Home Page: http://www.gnu.org/software/wget/wget.html
        ///     Manual: http://www.gnu.org/software/wget/manual/
        /// </summary>
        /// <param name="Url">Url</param>
        public HttpWGetExeRequest(string Url) { this._Url = Url; }

        /// <summary>
        ///     Url to call
        /// </summary>
        public string Url { get { return _Url; } set { _Url = value; } }

        /// <summary>
        ///     Custom Command Line Parameters:
        ///     --no-dns-cache, --execute cmd, --no-if-modified-since, --spider, --compression=gzip
        /// </summary>
        public string CustomParameters { get { return _CustomParameters; } set { _CustomParameters = value; } }

        /// <summary>
        ///   Filename with path of wGet.exe
        /// </summary>
        public string ExeFile { get { return _ExePath + _ExeName; } set { _ExePath = System.IO.Path.GetDirectoryName(value).Trim('\\') + @"\"; _ExeName = System.IO.Path.GetFileName(value); } }

        /// <summary>
        ///     Path without filename to wGet.exe
        /// </summary>
        public string ExePath { get { return _ExePath; } set { _ExePath = value.Trim('\\') + @"\"; } }

        /// <summary>
        ///     FileName without path of wGet.exe
        /// </summary>
        public string ExeName { get { return _ExeName; } set { _ExeName = System.IO.Path.GetFileName(value); } }

        /// <summary>
        ///     HTTP Method: GET/POST/PUT
        /// </summary>
        public string HTTPMethod { get { return _HTTPMethod; } set { _HTTPMethod = value; } }        

        /// <summary>
        ///     If this is set to on, wget will not skip the content when the server responds with a http status code that indicates error.
        /// </summary>
        public bool ContentOnError { get { return _ContentOnError; } set {_ContentOnError = value;} }

        /// <summary>
        ///     Log all messages to logfile. The messages are normally reported to standard error.
        /// </summary>
        public string LogFile { get { return _LogFile; } set { _LogFile = value; } }

        /// <summary>
        ///     Turn on debug output, meaning various information important to the developers of Wget if it does not work properly. Your system administrator may have chosen to compile Wget without debug support
        /// </summary>
        public bool Debug { get { return _Debug; } set { _Debug = value; } }

        /// <summary>
        ///     Turn off Wget’s output.
        /// </summary>
        public bool Quiet { get { return _Quiet; } set { _Quiet = value; } }

        /// <summary>
        ///     Turn on verbose output, with all the available data. The default output is verbose.
        /// </summary>
        public bool Verbose { get { return _Verbose; } set { _Verbose = value; } }

        /// <summary>
        ///     Print the headers sent by HTTP servers to StdOut
        /// </summary>
        public bool ServerResponse { get { return _ServerResponse; } set { _ServerResponse = value; } }

        /// <summary>
        ///     Set the network timeout to seconds seconds
        /// </summary>
        public int Timeout { get { return _Timeout; } set { _Timeout = value; } }

        /// <summary>
        ///     Don’t use proxies
        /// </summary>
        public bool NoProxy { get { return _NoProxy; } set { _NoProxy = value; } }

        /// <summary>
        ///     Do not use DNS cache
        /// </summary>
        public bool NoDNSCache { get { return _NoDNSCache; } set { _NoDNSCache = value; } }
        
        /// <summary>
        ///     Specify the username on an HTTP server
        /// </summary>
        public string HTTPUser { get { return _HTTPUser; } set { _HTTPUser = value; } }

        /// <summary>
        ///     Specify the password on an HTTP server
        /// </summary>
        public string HTTPPassword { get { return _HTTPPassword; } set { _HTTPPassword = value; } }

        /// <summary>
        ///     That affects how Wget converts URLs specified as arguments from locale to UTF−8 for IRI support.
        /// </summary>
        public System.Text.Encoding LocalEncoding { get { return _LocalEncoding; } set { _LocalEncoding = value; } }

        /// <summary>
        ///     Response Server Encoding
        /// </summary>
        public System.Text.Encoding RemoteEncoding { get { return _RemoteEncoding; } set { _RemoteEncoding = value; } }

        /// <summary>
        ///     Turn off the "keep-alive" feature for HTTP downloads.
        /// </summary>
        public bool NoHTTPKeepAlive { get { return _NoHttpKeepAlive; } set { _NoHttpKeepAlive = value; } }

        /// <summary>
        ///     Disable server-side cache
        /// </summary>
        public bool NoCache { get { return _NoCache; } set { _NoCache = value; } }

        /// <summary>
        ///     Disable the use of cookies.
        /// </summary>
        public bool NoCookies { get { return _NoCookies; } set { _NoCookies = value; } }

        /// <summary>
        ///     Load cookies from file before the first HTTP retrieval. file is a textual file in the format originally used by Netscape’s cookies.txt file.
        /// </summary>
        public string LoadCookiesFile { get { return _LoadCookiesFile; } set { _LoadCookiesFile = value; } }

        /// <summary>
        ///     Save cookies to file before exiting. This will not save cookies that have expired or that have no expiry time (so-called "session cookies")
        /// </summary>
        public string SaveCookiesFile { get { return _SaveCookiesFile; } set { _SaveCookiesFile = value; } }

        /// <summary>
        ///     Unfortunately, some HTTP servers ( CGI programs, to be more precise) send out bogus "Content−Length" headers, which makes Wget go wild, as it thinks not all the document was retrieved. You can spot this syndrome if Wget retries getting the same document again and again, each time claiming that the (otherwise normal) connection has closed on the very same byte.
        /// </summary>
        public bool IgnoreLength { get { return _IgnoreLength; } set { _IgnoreLength = value; } }

        /// <summary>
        ///     HTTP Request Headers 'Authorization: oauth'
        /// </summary>
        public string[] Headers { get { return _Headers; } set { _Headers = value; } }

        /// <summary>
        ///     Specify the username for authentication on a proxy server. 
        /// </summary>
        public string ProxyUser { get { return _ProxyUser; } set { _ProxyUser = value; } }

        /// <summary>
        ///     Specify the password for authentication on a proxy server. 
        /// </summary>
        public string ProxyPassword { get { return _ProxyPassword; } set { _ProxyPassword = value; } }

        /// <summary>
        ///     Include ‘Referer: url’ header in HTTP request. Useful for retrieving documents with server-side processing that assume they are always being retrieved by interactive web browsers and only come out properly when Referer is set to one of the pages that point to them.
        /// </summary>
        public string Referer { get { return _Referer; } set { _Referer = value; } }

        /// <summary>
        ///     Save the headers sent by the HTTP server to the file or stdout, preceding the actual contents, with an empty line as the separator.
        /// </summary>
        public bool SaveHeaders { get { return _SaveHeaders; } set { _SaveHeaders = value; } }

        /// <summary>
        ///     Identify as agent-string to the HTTP server.
        /// </summary>
        public string UserAgent { get { return _UserAgent; } set { _UserAgent = value;  } }

        /// <summary>
        ///     Use POST as the method for all HTTP requests and send the specified data in the request body.
        /// </summary>
        public string PostFile { get { return _PostFile; } set { _PostFile = value; } }

        /// <summary>
        ///     Must be set when additional data needs to be sent to the server along with the Method specified using −−method. −−body−data sends string as data, whereas −−body−file sends the contents of file. Other than that, they work in exactly the same way.
        /// </summary>
        public string BodyFile { get { return _BodyFile; } set { _BodyFile = value; } }

        /// <summary>
        ///     Don’t check the server certificate against the available certificate authorities. Also don’t require the URL host name to match the common name presented by the certificate.
        /// </summary>
        public bool NoCheckCertificate { get { return _NoCheckCertificate; } set { _NoCheckCertificate = value; } }

        /// <summary>
        ///     When in recursive mode, only HTTPS links are followed.
        /// </summary>
        public bool HTTPSOnly { get { return _HTTPSOnly; } set { _HTTPSOnly = value; } }

        /// <summary>
        ///     Choose the secure protocol to be used. Legal values are auto, SSLv2, SSLv3, TLSv1, TLSv1_1, TLSv1_2 and PFS . If auto is used, the SSL library is given the liberty of choosing the appropriate protocol automatically, which is achieved by sending a TLSv1 greeting. This is the default.
        /// </summary>
        public string SecureProtocol { get { return _SecureProtocol; } set { _SecureProtocol = value; } }

        /// <summary>
        ///     StdOut (-) or File to save servers response
        /// </summary>
        public string OutputFile { get { return _OutputFile; } set { _OutputFile = value; } }

        /// <summary>
        ///     Show wGet.exe console window StdOut
        /// </summary>
        public bool ExeWindow { get { return _ExeWindow; } set { _ExeWindow = value; } }

        /// <summary>
        ///     Start wGet.exe as user name
        /// </summary>
        public string ExeUser { get { return _ExeUser; } set { _ExeUser = value; } }

        /// <summary>
        ///     Start wGet.exe as user password
        /// </summary>
        public string ExePassword { get { return _ExePassword; } set { _ExePassword = value; } }

        

        /// <summary>
        ///     Use POST as the method for all HTTP requests and send the specified data in the request body. −−post−data sends string as data
        /// </summary>
        public string POSTDATA { get { return _POSTDATA; } set { _POSTDATA = value; } }

        /// <summary>
        ///     Must be set when additional data needs to be sent to the server along with the Method specified using −−method. −−body−data sends string as data
        /// </summary>
        public string BODYDATA { get { return _BODYDATA; } set { _BODYDATA = value; } }

        public string GetExeParams()
        {
            string pars = "--method=" + _HTTPMethod;

            if (!String.IsNullOrEmpty(_LogFile)) pars = " −o \"" + _LogFile + "\"";

            if (_Debug) pars = " -d"; // " −−debug";
            if (_Quiet) pars = " -q"; // " −−quiet";

            if (!_Verbose) pars = " -nv"; // " −−no−verbose";
            if ((String.IsNullOrEmpty(_OutputFile)) || (_OutputFile == "-"))
                pars += " -O -"; // -O or −−output−document=file // The documents will not be written to the appropriate files, but all will be concatenated together and written to file. If − is used as file, documents will be printed to standard output, disabling link conversion. (Use ./− to print to a file literally named −.)
            else if (!String.IsNullOrEmpty(_OutputFile))
                pars += " -O \"" + _OutputFile + "\"";

            if (_ServerResponse) pars += " -S"; // " −−server−response";
            if (_Timeout != 900) pars += " -T " + _Timeout.ToString(); //  " −−timeout=30"; // timeout in seconds

            if (_NoProxy) pars += " −−no−proxy"; // Don’t use proxies, even if the appropriate *_proxy environment variable is defined.
            if (_NoDNSCache) pars += " −−no−dns−cache";

            if (!String.IsNullOrEmpty(_HTTPUser)) pars += " --http-user=" + _HTTPUser;
            if ((!String.IsNullOrEmpty(_HTTPUser)) && (!String.IsNullOrEmpty(_HTTPPassword))) pars += " --http-password=" + _HTTPPassword;

            if ((_LocalEncoding != null) && (_LocalEncoding != System.Text.Encoding.UTF8)) pars += " −−local−encoding=" + _LocalEncoding.EncodingName;// // That affects how Wget converts URLs specified as arguments from locale to UTF−8 for IRI support.
            if ((_RemoteEncoding != null) && (_RemoteEncoding != System.Text.Encoding.UTF8)) pars += " −−remote−encoding=" + _RemoteEncoding.EncodingName; // // default remote server encoding.

            if (_NoHttpKeepAlive) pars += " −−no−http−keep−alive";
            if (_NoCache) pars += " −−no−cache"; // no cache
            if (_NoCookies) pars += " −−no−cookies"; // no cookies
            if (!String.IsNullOrEmpty(_LoadCookiesFile)) pars += " −−load−cookies \"" + _LoadCookiesFile + "\""; // cookies from file
            if (!String.IsNullOrEmpty(_SaveCookiesFile)) pars += " −−save−cookies \"" + SaveCookiesFile + "\""; // save cookies to file            

            if ((_Headers != null) && (_Headers.Length > 0))
                foreach (string header in _Headers)
                    pars += " --header=\"" + header.Replace("\"", "\\\"") + "\""; // headers
            if (!String.IsNullOrEmpty(_Referer)) pars += " −−referer=\"" + _Referer + "\"";
            if (_UserAgent != null) pars += " --user-agent=\"" + _UserAgent + "\""; // user-agent    

            if (!String.IsNullOrEmpty(_ProxyUser)) pars += " −−proxy−user=" + _ProxyUser; // proxy user
            if ((!String.IsNullOrEmpty(_ProxyUser)) && (!String.IsNullOrEmpty(_ProxyPassword))) pars += " −−proxy−password=" + ProxyPassword; // proxy pass

            if (_SaveHeaders) pars += " --save-headers"; // out response headers to stdout  
            if (_IgnoreLength) pars += " −−ignore−length";
            if (_ContentOnError) pars += " --content-on-error";

            if (!String.IsNullOrEmpty(POSTDATA)) pars += " −−post−data=\"" + POSTDATA + "\""; // POST data
            if ((!String.IsNullOrEmpty(_PostFile)) && System.IO.File.Exists(_PostFile)) pars += " −−post−file=\"" + _PostFile + "\"";// 
            if (!String.IsNullOrEmpty(BODYDATA)) pars += " −−body−data=\"" + BODYDATA + "\""; // POST data
            if ((!String.IsNullOrEmpty(_BodyFile)) && System.IO.File.Exists(_BodyFile)) pars += " −−body−file=\"" + _BodyFile + "\"";// 

            if (_NoCheckCertificate) pars += " --no-check-certificate"; // for HTTPS connection
            if (_HTTPSOnly) pars += " −−https−only"; //
            if ((!String.IsNullOrEmpty(_SecureProtocol)) && (_SecureProtocol != "auto")) pars += " −−secure−protocol=" + _SecureProtocol; // auto, SSLv2, SSLv3, TLSv1, TLSv1_1, TLSv1_2 and PFS 

            // pars += " −−certificate={file}"; //
            // pars += " −−certificate−type={type}"; // PEM / DER
            // pars += " −−ca−certificate={file}"; //  

            if (!String.IsNullOrEmpty(_CustomParameters)) pars += " " + _CustomParameters.Trim(' ');

            pars += " \"" + _Url + "\""; // pars

            return pars;
        }

        /// <summary>
        ///     Do all progress to StdOut
        /// </summary>
        public bool ProgressStdOut { get { return _ServerResponse && _ExeWindow && (!_Quiet); } set { _Quiet = !(_ServerResponse = _ExeWindow = value); } }

        /// <summary>
        ///     Call and get server's body response
        /// </summary>
        /// <returns></returns>
        public string GetResponseBody()
        {
            string h;
            return GetResponseBody(out h);         
        }

        /// <summary>
        ///     Call and get server's body response
        /// </summary>
        /// <param name="Headers">Server response headers</param>
        /// <returns></returns>
        public string GetResponseBody(out string Headers)
        {
            Headers = "";

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = ExeFile;
            p.StartInfo.Arguments = GetExeParams();
            p.StartInfo.CreateNoWindow = !_ExeWindow;
            if (!String.IsNullOrEmpty(_ExeUser)) p.StartInfo.UserName = _ExeUser;
            if ((!String.IsNullOrEmpty(_ExeUser)) && (!String.IsNullOrEmpty(_ExePassword))) p.StartInfo.Password = ConvertToSecureString(_ExePassword);
            p.Start();

            string output = "";
            output += p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            string[] ext = new string[] { "No Problem", "Generic Error Code", "Command Line Parse Error", "File I/O Error", "Network Failure", "SSL Certificate Failure", "Username/Password Authentification Failure", "Protocol Error", "Server Response Error" };
            int ExCode = p.ExitCode; // 0 - No problems,  1 - code error, 2 - parse error, 3 - IO error, 4 - Net failure, 5 - SSL failure, 6 - Auth failuer, 7 - proto error, 8 - error response

            string body = output;
            if ((!String.IsNullOrEmpty(output)) && (_SaveHeaders))
            {
                int bs = output.IndexOf("\r\n\r\n");
                if (bs > 0)
                {
                    body = output.Substring(bs + 4);
                    Headers = output.Substring(0, bs);
                };
            };

            if (ExCode == 0)
                return body;
            else
            {
                if (output != "")
                    return body;
                else
                    throw new Exception("Exit Code " + ExCode.ToString() + " - " + ext[ExCode]);
            };
        }

        /// <summary>
        ///     Call and get servers's response
        /// </summary>
        /// <returns></returns>
        public string GetResponse()
        {          
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = ExeFile;
            p.StartInfo.Arguments = GetExeParams();
            p.StartInfo.CreateNoWindow = !_ExeWindow;
            if(!String.IsNullOrEmpty(_ExeUser)) p.StartInfo.UserName = _ExeUser;
            if((!String.IsNullOrEmpty(_ExeUser)) && (!String.IsNullOrEmpty(_ExePassword))) p.StartInfo.Password = ConvertToSecureString(_ExePassword);
            p.Start();

            string output = "";
            output += p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            string[] ext = new string[] { "No Problem", "Generic Error Code", "Command Line Parse Error", "File I/O Error", "Network Failure", "SSL Certificate Failure", "Username/Password Authentification Failure", "Protocol Error", "Server Response Error" };
            int ExCode = p.ExitCode; // 0 - No problems,  1 - code error, 2 - parse error, 3 - IO error, 4 - Net failure, 5 - SSL failure, 6 - Auth failuer, 7 - proto error, 8 - error response

            if (ExCode == 0)
               return output;
            else
            {
                if (output != "")
                    return output;
                else
                    throw new Exception("Exit Code " + ExCode.ToString() + " - " + ext[ExCode]);
            };
        }

        /// <summary>
        ///     wGET HTTP(S) REQUEST
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="headers">http headers</param>
        /// <param name="return_headers">return server response headers</param>
        public static string SimpleRequest(string url, string[] headers)
        {
            string wget = AppDomain.CurrentDomain.BaseDirectory + @"\wget.exe";

            string pars = "--method=GET"; // GET or POST
            // pars += " −−server−response"; // Print the headers sent by HTTP servers and responses sent by FTP servers.
            // pars += " −−timeout=30"; // timeout in seconds
            pars += " --no-check-certificate"; // for HTTPS connection
            // pars += " −−https−only"; //
            // pars += " −−secure−protocol={protocol}"; // auto, SSLv2, SSLv3, TLSv1, TLSv1_1, TLSv1_2 and PFS 
            // pars += " −−certificate={file}"; //
            // pars += " −−certificate−type={type}"; // PEM / DER
            // pars += " −−ca−certificate={file}"; // 
            // pars += " −−no−proxy"; // Don’t use proxies, even if the appropriate *_proxy environment variable is defined.
            // pars += " −−proxy−user=user" ; // proxy user
            // pars += " −−proxy−password=password"; // proxy pass
            pars += " --content-on-error"; // get content if server returns != 200
            pars += " --save-headers"; // out response headers to stdout
            pars += " --user-agent=\"\""; // user-agent
            // pars += " −−no−cache"; // no cache
            // pars += " −−no−cookies"; // no cookies
            // pars += " −−load−cookies {file}"; // cookies from file
            // pars += " −−save−cookies {file}"; // save cookies to file
            // pars += " −−ignore−length"; // ingonre Content-Length
            // pars += " −−http−user={user}"; // USER
            // pars += " −−http-password={password}"; // password
            // pars += " −−local−encoding=encoding";// // That affects how Wget converts URLs specified as arguments from locale to UTF−8 for IRI support.
            // pars += " −−remote−encoding=encoding"; // // default remote server encoding.
            // pars += " −−post−data={string}"; // POST data
            // pars += " −−post−file={file}";// 
            // pars += " −−body−data={data-string}"; // POST data
            // pars += " −−body−file={body-file}";//

            // CUSTOM HEADERS
            if ((headers != null) && (headers.Length > 0))
                foreach (string header in headers)
                    pars += " --header=\"" + header.Replace("\"", "\\\"") + "\""; // headers
            // URL
            // pars += " −−input−file={file}"; // Read URLs from a local or external file. If − is specified as file, URLs are read from the standard input
            pars += " \"" + url + "\""; // pars

            // OUTPUT
            pars += " --quiet"; // Turn off Wget’s output.
            pars += " -O -"; // -O or −−output−document=file // The documents will not be written to the appropriate files, but all will be concatenated together and written to file. If − is used as file, documents will be printed to standard output, disabling link conversion. (Use ./− to print to a file literally named −.)

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = wget;
            p.StartInfo.Arguments = pars;
            p.StartInfo.CreateNoWindow = true;
            //p.StartInfo.UserName = "";
            //p.StartInfo.Password = ("");
            p.Start();

            string output = "";
            output += p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            string[] ext = new string[] { "No Problem", "Generic Error Code", "Command Line Parse Error", "File I/O Error", "Network Failure", "SSL Certificate Failure", "Username/Password Authentification Failure", "Protocol Error", "Server Response Error" };
            int ExCode = p.ExitCode; // 0 - No problems,  1 - code error, 2 - parse error, 3 - IO error, 4 - Net failure, 5 - SSL failure, 6 - Auth failuer, 7 - proto error, 8 - error response

            if (ExCode == 0)
                return output;
            else
            {
                if (output != "")
                    return output;
                else
                    throw new Exception("Exit Code " + ExCode.ToString() + " - " + ext[ExCode]);
            };
        }

        private static System.Security.SecureString ConvertToSecureString(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            unsafe
            {
                fixed (char* passwordChars = password)
                {
                    System.Security.SecureString securePassword = new System.Security.SecureString(passwordChars, password.Length);
                    securePassword.MakeReadOnly();
                    return securePassword;
                }
            }
        }
    }

    /// <summary>
    ///     Using cUrl.exe to HTTPRequest
    ///     Place cUrl.exe to application exe folder
    ///     Home Page: https://curl.se/
    ///     Manual: https://curl.se/docs/
    /// </summary>
    public class HttpCUrlExeRequest
    {
        #region private vars
        private string _Url = "http://localhost/";
        private string _UserAgent = ""; // -A --user-agent
        private string _ExePath = AppDomain.CurrentDomain.BaseDirectory.Trim('\\') + @"\";
        private string _ExeName = "curl.exe";
        private string _HTTPMethod = "GET"; // -G --get
        private string _LogFile = null; // --dump-header
        private bool _Quiet = false; // -s --silent
        private bool _Verbose = false; // -v --verbose
        private int _Timeout = 900; // --connect-timeout
        private string _HTTPUser = null; // -u --user <user:pass>
        private string _HTTPPassword = null; // -u --user <user:pass>
        private bool _NoHttpKeepAlive = false; // --no-keepalive
        private bool _IgnoreLength = false; // --ignore-content-length
        private string[] _Headers = null; // -H, --header <header/@file>
        private string _ProxyUser = null; // -U, --proxy-user <user:password>
        private string _ProxyPassword = null; // -U, --proxy-user <user:password>
        private string _Referer = null; // -e, --referer <URL>
        private bool _SaveHeaders = false; // -i, --include

        private string _POSTDATA = null;
        private string _BODYDATA = null;
        private string _PostFile = null;
        private string _BodyFile = null;
        private bool _NoCheckCertificate = true; // -k, --insecure
        private string _SecureProtocol = String.Empty;
        private string _OutputFile = "-";
        private bool _ExeWindow = false;
        private string _ExeUser = null;
        private string _ExePassword = null;
        private bool _ContentOnError = true;
        private string _CustomParameters = null;
        #endregion

        /// <summary>
        ///     Using curl.exe to HTTPRequest
        ///     Place curl.exe to application exe folder
        /// </summary>
        public HttpCUrlExeRequest() { }

        /// <summary>
        ///     Using curl.exe to HTTPRequest
        ///     Place curl.exe to application exe folder
        /// </summary>
        /// </summary>
        /// <param name="Url">Url</param>
        public HttpCUrlExeRequest(string Url) { this._Url = Url; }

        /// <summary>
        ///     Url to call
        /// </summary>
        public string Url { get { return _Url; } set { _Url = value; } }

        /// <summary>
        ///     Custom Command Line Parameters:
        /// </summary>
        public string CustomParameters { get { return _CustomParameters; } set { _CustomParameters = value; } }

        /// <summary>
        ///   Filename with path of cUrl.exe
        /// </summary>
        public string ExeFile { get { return _ExePath + _ExeName; } set { _ExePath = System.IO.Path.GetDirectoryName(value).Trim('\\') + @"\"; _ExeName = System.IO.Path.GetFileName(value); } }

        /// <summary>
        ///     Path without filename to cUrl.exe
        /// </summary>
        public string ExePath { get { return _ExePath; } set { _ExePath = value.Trim('\\') + @"\"; } }

        /// <summary>
        ///     FileName without path of cUrl.exe
        /// </summary>
        public string ExeName { get { return _ExeName; } set { _ExeName = System.IO.Path.GetFileName(value); } }

        /// <summary>
        ///     HTTP Method: GET/POST/PUT
        /// </summary>
        public string HTTPMethod { get { return _HTTPMethod; } set { _HTTPMethod = value; } }

        /// <summary>
        ///     If this is set to on, cUrl will not skip the content when the server responds with a http status code that indicates error.
        /// </summary>
        public bool ContentOnError { get { return _ContentOnError; } set { _ContentOnError = value; } }
        
        /// <summary>
        ///     Log all messages to logfile. The messages are normally reported to standard error.
        /// </summary>
        public string LogFile { get { return _LogFile; } set { _LogFile = value; } }

        public bool Quiet { get { return _Quiet; } set { _Quiet = value; } }
        public bool Progress { get { return !_Quiet; } set { _Quiet = !value; } }

        /// <summary>
        ///     Turn on verbose output, with all the available data. The default output is verbose.
        /// </summary>
        public bool Verbose { get { return _Verbose; } set { _Verbose = value; } }

        /// <summary>
        ///     Turn on debug output, meaning various information important to the developers of cUrl if it does not work properly. Your system administrator may have chosen to compile cUrl without debug support
        /// </summary>
        public bool Debug { get { return _Verbose; } set { _Verbose = value; } }

        /// <summary>
        ///     Print the headers sent by HTTP servers to StdOut
        /// </summary>
        public bool ServerResponse { get { return _SaveHeaders; } set { _SaveHeaders = value; } }

        /// <summary>
        ///     Set the network timeout to seconds seconds
        /// </summary>
        public int Timeout { get { return _Timeout; } set { _Timeout = value; } }
        
        /// <summary>
        ///     Specify the username on an HTTP server
        /// </summary>
        public string HTTPUser { get { return _HTTPUser; } set { _HTTPUser = value; } }

        /// <summary>
        ///     Specify the password on an HTTP server
        /// </summary>
        public string HTTPPassword { get { return _HTTPPassword; } set { _HTTPPassword = value; } }
        
        /// <summary>
        ///     Turn off the "keep-alive" feature for HTTP downloads.
        /// </summary>
        public bool NoHTTPKeepAlive { get { return _NoHttpKeepAlive; } set { _NoHttpKeepAlive = value; } }

        /// <summary>
        ///     Unfortunately, some HTTP servers ( CGI programs, to be more precise) send out bogus "Content−Length" headers, which makes cUrl go wild, as it thinks not all the document was retrieved. You can spot this syndrome if cUrl retries getting the same document again and again, each time claiming that the (otherwise normal) connection has closed on the very same byte.
        /// </summary>
        public bool IgnoreLength { get { return _IgnoreLength; } set { _IgnoreLength = value; } }

        /// <summary>
        ///     HTTP Request Headers 'Authorization: oauth'
        /// </summary>
        public string[] Headers { get { return _Headers; } set { _Headers = value; } }

        /// <summary>
        ///     Specify the username for authentication on a proxy server. 
        /// </summary>
        public string ProxyUser { get { return _ProxyUser; } set { _ProxyUser = value; } }

        /// <summary>
        ///     Specify the password for authentication on a proxy server. 
        /// </summary>
        public string ProxyPassword { get { return _ProxyPassword; } set { _ProxyPassword = value; } }

        /// <summary>
        ///     Include ‘Referer: url’ header in HTTP request. Useful for retrieving documents with server-side processing that assume they are always being retrieved by interactive web browsers and only come out properly when Referer is set to one of the pages that point to them.
        /// </summary>
        public string Referer { get { return _Referer; } set { _Referer = value; } }

        /// <summary>
        ///     Save the headers sent by the HTTP server to the file or stdout, preceding the actual contents, with an empty line as the separator.
        /// </summary>
        public bool SaveHeaders { get { return _SaveHeaders; } set { _SaveHeaders = value; } }

        /// <summary>
        ///     Identify as agent-string to the HTTP server.
        /// </summary>
        public string UserAgent { get { return _UserAgent; } set { _UserAgent = value; } }

        /// <summary>
        ///     Use POST as the method for all HTTP requests and send the specified data in the request body.
        /// </summary>
        public string PostFile { get { return _PostFile; } set { _PostFile = value; } }

        /// <summary>
        ///     Must be set when additional data needs to be sent to the server along with the Method specified using −−method. −−body−data sends string as data, whereas −−body−file sends the contents of file. Other than that, they work in exactly the same way.
        /// </summary>
        public string BodyFile { get { return _BodyFile; } set { _BodyFile = value; } }

        /// <summary>
        ///     Don’t check the server certificate against the available certificate authorities. Also don’t require the URL host name to match the common name presented by the certificate.
        /// </summary>
        public bool NoCheckCertificate { get { return _NoCheckCertificate; } set { _NoCheckCertificate = value; } }

        /// <summary>
        ///     Choose the secure protocol to be used. Legal values are auto, SSLv2, SSLv3, TLSv1, TLSv1_1, TLSv1_2 and PFS . If auto is used, the SSL library is given the liberty of choosing the appropriate protocol automatically, which is achieved by sending a TLSv1 greeting. This is the default.
        /// </summary>
        public string SecureProtocol { get { return _SecureProtocol; } set { _SecureProtocol = value; } }

        /// <summary>
        ///     StdOut (-) or File to save servers response
        /// </summary>
        public string OutputFile { get { return _OutputFile; } set { _OutputFile = value; } }

        /// <summary>
        ///     Show cUrl.exe console window StdOut
        /// </summary>
        public bool ExeWindow { get { return _ExeWindow; } set { _ExeWindow = value; } }

        /// <summary>
        ///     Start cUrl.exe as user name
        /// </summary>
        public string ExeUser { get { return _ExeUser; } set { _ExeUser = value; } }

        /// <summary>
        ///     Start cUrl.exe as user password
        /// </summary>
        public string ExePassword { get { return _ExePassword; } set { _ExePassword = value; } }



        /// <summary>
        ///     Use POST as the method for all HTTP requests and send the specified data in the request body. −−post−data sends string as data
        /// </summary>
        public string POSTDATA { get { return _POSTDATA; } set { _POSTDATA = value; } }

        /// <summary>
        ///     Must be set when additional data needs to be sent to the server along with the Method specified using −−method. −−body−data sends string as data
        /// </summary>
        public string BODYDATA { get { return _BODYDATA; } set { _BODYDATA = value; } }

        public string GetExeParams()
        {
            string pars = _HTTPMethod == "GET" ? "--get" : "";

            if (!String.IsNullOrEmpty(_LogFile)) pars += " --dump-header \"" + _LogFile + "\"";

            if (_Quiet) pars += " --silent";

            if (_Verbose) pars += " --verbose";

            if ((String.IsNullOrEmpty(_OutputFile)) || (_OutputFile == "-"))
                pars += " --output -";
            else if (!String.IsNullOrEmpty(_OutputFile))
                pars += " --output \"" + _OutputFile + "\"";

            if (_Timeout != 900) pars += " --connect-timeout " + _Timeout.ToString(); //  " −−timeout=30"; // timeout in seconds

            if (!String.IsNullOrEmpty(_HTTPUser)) pars += " -u --user " + _HTTPUser;
            if ((!String.IsNullOrEmpty(_HTTPUser)) && (!String.IsNullOrEmpty(_HTTPPassword))) pars += ":" + _HTTPPassword;

            if (_NoHttpKeepAlive) pars += " --no-keepalive";
            
            if ((_Headers != null) && (_Headers.Length > 0))
                foreach (string header in _Headers)
                    pars += " --header \"" + header.Replace("\"", "\\\"") + "\""; // headers
            if (!String.IsNullOrEmpty(_Referer)) pars += " --referer \"" + _Referer + "\"";
            if (!String.IsNullOrEmpty(_UserAgent)) pars += " --user-agent \"" + _UserAgent + "\""; // user-agent    

            if (!String.IsNullOrEmpty(_ProxyUser)) pars += " --proxy-user " + _ProxyUser; // proxy user
            if ((!String.IsNullOrEmpty(_ProxyUser)) && (!String.IsNullOrEmpty(_ProxyPassword))) pars += ":" + ProxyPassword; // proxy pass

            if (_SaveHeaders) pars += " --include"; // out response headers to stdout  
            if (_IgnoreLength) pars += " --ignore-content-length";
            if (_ContentOnError) pars += " --fail-with-body";

            if (!String.IsNullOrEmpty(POSTDATA)) pars += " --data \"" + POSTDATA + "\""; // POST data
            if ((!String.IsNullOrEmpty(_PostFile)) && System.IO.File.Exists(_PostFile)) pars += " --data \"@" + _PostFile + "\"";// 
            if (!String.IsNullOrEmpty(BODYDATA)) pars += " −--data \"" + BODYDATA + "\""; // POST data
            if ((!String.IsNullOrEmpty(_BodyFile)) && System.IO.File.Exists(_BodyFile)) pars += " --data \"@" + _BodyFile + "\"";// 

            if (_NoCheckCertificate) pars += " --insecure"; // for HTTPS connection
            if ((!String.IsNullOrEmpty(_SecureProtocol)) && (_SecureProtocol != "--ssl-auto-client-cert")) pars += " " + _SecureProtocol; // --ssl-auto-client-cert, --sslv2, --sslv3

            if (!String.IsNullOrEmpty(_CustomParameters)) pars += " " + _CustomParameters.Trim(' ');

            pars += " \"" + _Url + "\""; // pars

            return pars;
        }

        /// <summary>
        ///     Do all progress to StdOut
        /// </summary>
        public bool ProgressStdOut { get { return _ExeWindow && (!_Quiet); } set { _Quiet = !(_ExeWindow = value); } }

        /// <summary>
        ///     Call and get server's body response
        /// </summary>
        /// <returns></returns>
        public string GetResponseBody()
        {
            string h;
            return GetResponseBody(out h);
        }

        /// <summary>
        ///     Call and get server's body response
        /// </summary>
        /// <param name="Headers">Server response headers</param>
        /// <returns></returns>
        public string GetResponseBody(out string Headers)
        {
            Headers = "";

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = ExeFile;
            p.StartInfo.Arguments = GetExeParams();
            p.StartInfo.CreateNoWindow = !_ExeWindow;
            if (!String.IsNullOrEmpty(_ExeUser)) p.StartInfo.UserName = _ExeUser;
            if ((!String.IsNullOrEmpty(_ExeUser)) && (!String.IsNullOrEmpty(_ExePassword))) p.StartInfo.Password = ConvertToSecureString(_ExePassword);                        
            p.Start();

            string output = "";
            output += p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            string[] ext = new string[] { "No Problem", "Generic Error Code", "Command Line Parse Error", "File I/O Error", "Network Failure", "SSL Certificate Failure", "Username/Password Authentification Failure", "Protocol Error", "Server Response Error" };
            int ExCode = p.ExitCode; // 0 - No problems,  1 - code error, 2 - parse error, 3 - IO error, 4 - Net failure, 5 - SSL failure, 6 - Auth failuer, 7 - proto error, 8 - error response

            string body = output;
            if ((!String.IsNullOrEmpty(output)) && (_SaveHeaders))
            {
                int bs = output.IndexOf("\r\n\r\n");
                if (bs > 0)
                {
                    body = output.Substring(bs + 4);
                    Headers = output.Substring(0, bs);
                };
            };

            if (ExCode == 0)
                return body;
            else
            {
                if (output != "")
                    return body;
                else
                    throw new Exception("Exit Code " + ExCode.ToString() + " - " + ext[ExCode]);
            };
        }

        /// <summary>
        ///     Call and get servers's response
        /// </summary>
        /// <returns></returns>
        public string GetResponse()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = ExeFile;
            p.StartInfo.Arguments = GetExeParams();
            p.StartInfo.CreateNoWindow = !_ExeWindow;
            if (!String.IsNullOrEmpty(_ExeUser)) p.StartInfo.UserName = _ExeUser;
            if ((!String.IsNullOrEmpty(_ExeUser)) && (!String.IsNullOrEmpty(_ExePassword))) p.StartInfo.Password = ConvertToSecureString(_ExePassword);
            p.Start();

            string output = "";
            output += p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            string[] ext = new string[] { "No Problem", "Generic Error Code", "Command Line Parse Error", "File I/O Error", "Network Failure", "SSL Certificate Failure", "Username/Password Authentification Failure", "Protocol Error", "Server Response Error" };
            int ExCode = p.ExitCode; // 0 - No problems,  1 - code error, 2 - parse error, 3 - IO error, 4 - Net failure, 5 - SSL failure, 6 - Auth failuer, 7 - proto error, 8 - error response

            if (ExCode == 0)
                return output;
            else
            {
                if (output != "")
                    return output;
                else
                    throw new Exception("Exit Code " + ExCode.ToString() + " - " + ext[ExCode]);
            };
        }

        /// <summary>
        ///     cUrl HTTP(S) REQUEST
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="headers">http headers</param>
        /// <param name="return_headers">return server response headers</param>
        public static string SimpleRequest(string url, string[] headers)
        {
            string curl = AppDomain.CurrentDomain.BaseDirectory + @"\curl.exe";

            string pars = "--get";
            pars += " --insecure"; // for HTTPS connection
            pars += " --fail-with-body"; // get content if server returns != 200
            pars += " --include"; // out response headers to stdout
            pars += " --user-agent \"\""; // user-agent
            
            // CUSTOM HEADERS
            if ((headers != null) && (headers.Length > 0))
                foreach (string header in headers)
                    pars += " --header \"" + header.Replace("\"", "\\\"") + "\""; // headers

            pars += " --silent"; // Turn off cUrl’s output.
            pars += " --output -"; // -O or −−output−document=file // The documents will not be written to the appropriate files, but all will be concatenated together and written to file. If − is used as file, documents will be printed to standard output, disabling link conversion. (Use ./− to print to a file literally named −.)
            pars += " \"" + url + "\""; // pars
            

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = curl;
            p.StartInfo.Arguments = pars;
            p.StartInfo.CreateNoWindow = true;
            //p.StartInfo.UserName = "";
            //p.StartInfo.Password = ("");
            p.Start();

            string output = "";
            output += p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            string[] ext = new string[] { "No Problem", "Generic Error Code", "Command Line Parse Error", "File I/O Error", "Network Failure", "SSL Certificate Failure", "Username/Password Authentification Failure", "Protocol Error", "Server Response Error" };
            int ExCode = p.ExitCode; // 0 - No problems,  1 - code error, 2 - parse error, 3 - IO error, 4 - Net failure, 5 - SSL failure, 6 - Auth failuer, 7 - proto error, 8 - error response

            if (ExCode == 0)
                return output;
            else
            {
                if (output != "")
                    return output;
                else
                    throw new Exception("Exit Code " + ExCode.ToString() + " - " + ext[ExCode]);
            };
        }

        private static System.Security.SecureString ConvertToSecureString(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            unsafe
            {
                fixed (char* passwordChars = password)
                {
                    System.Security.SecureString securePassword = new System.Security.SecureString(passwordChars, password.Length);
                    securePassword.MakeReadOnly();
                    return securePassword;
                }
            }
        }
    }

    /// <summary>
    ///     Get Response doen't work!
    /// </summary>
    public class HttpCUrlLibRequest
    {
        private const String libcurl_dll = "libcurl.dll";

        public class SList : IDisposable
        {
            [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
            private class curl_slist
            {
                /// char*
                [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)]
                public string data;

                /// curl_slist*
                public System.IntPtr next;
            }
            /// <summary>
            /// Read-only copy of the strings stored in the SList
            /// </summary>
            public List<string> Strings
            {
                get
                {
                    if (_handle == IntPtr.Zero)
                        return null;

                    curl_slist slist = new curl_slist();
                    List<string> strings = new List<string>();

                    Marshal.PtrToStructure(_handle, slist);

                    while (true)
                    {
                        strings.Add(slist.data);
                        if (slist.next != IntPtr.Zero)
                            Marshal.PtrToStructure(slist.next, slist);
                        else
                            break;
                    }
                    return strings;
                }
            }

            public SList(IntPtr handle)
            {
                _handle = handle;
            }
            public SList()
            {
            }

            public void Append(string data)
            {
                _handle = curl_slist_append(_handle, data);
            }

            private IntPtr _handle = IntPtr.Zero;
            public IntPtr Handle
            {
                get { return _handle; }
            }

            #region IDisposable Members

            public void Dispose()
            {
                if (_handle != IntPtr.Zero)
                {
                    curl_slist_free_all(_handle);
                    _handle = IntPtr.Zero;
                }
            }

            #endregion
        }

        /// <summary>
        ///     Response Information
        /// </summary>
        public class PerformResponse
        {
            private HttpCUrlLibRequest parent;
            private IntPtr CURL { get { return parent.CURL; } }
            private PerformResponse(){}            

            internal static PerformResponse FromLibRequest(HttpCUrlLibRequest owner)
            {
                PerformResponse res = new PerformResponse();
                res.parent = owner;
                return res;
            }

            /// <summary>
            ///     Last Used URL
            /// </summary>
            /// <returns></returns>
            public string RequestUrl
            {
                get
                {
                    IntPtr ip = IntPtr.Zero;
                    CURLCode code = curl_easy_getinfo(CURL, CURLINFO.CURLINFO_EFFECTIVE_URL, ref ip);
                    if (code == CURLCode.CURLE_OK)
                        return Marshal.PtrToStringAnsi(ip);
                    else
                        return null;
                }
            }

            public string ContentType
            {
                get
                {
                    IntPtr ip = IntPtr.Zero;
                    CURLCode code = curl_easy_getinfo(CURL, CURLINFO.CURLINFO_CONTENT_TYPE, ref ip);
                    if (code == CURLCode.CURLE_OK)
                        return Marshal.PtrToStringAnsi(ip);
                    else
                        return null;
                }
            }

            public double ConnectTime
            {
                get
                {
                    double d = 0;
                    CURLCode code = curl_easy_getinfo_64(CURL, CURLINFO.CURLINFO_CONNECT_TIME, ref d);
                    if (code == CURLCode.CURLE_OK)
                        return d;
                    else
                        return 0;
                }
            }

            public double RedirectTime
            {
                get
                {
                    double d = 0;
                    CURLCode code = curl_easy_getinfo_64(CURL, CURLINFO.CURLINFO_REDIRECT_TIME, ref d);
                    if (code == CURLCode.CURLE_OK)
                        return d;
                    else
                        return 0;
                }
            }

            public double UploadedContentLength
            {
                get
                {
                    double d = 0;
                    CURLCode code = curl_easy_getinfo_64(CURL, CURLINFO.CURLINFO_CONTENT_LENGTH_UPLOAD, ref d);
                    if (code == CURLCode.CURLE_OK)
                        return d;
                    else
                        return 0;
                }
            }

            public double DownloadedContentLength
            {
                get
                {
                    double d = 0;
                    CURLCode code = curl_easy_getinfo_64(CURL, CURLINFO.CURLINFO_CONTENT_LENGTH_DOWNLOAD, ref d);
                    if (code == CURLCode.CURLE_OK)
                        return d;
                    else
                        return 0;
                }
            }

            public int DownloadedHeaderSize
            {
                get
                {
                    int val = 0;
                    CURLCode code = curl_easy_getinfo(CURL, CURLINFO.CURLINFO_HEADER_SIZE, ref val);
                    if (code == CURLCode.CURLE_OK)
                        return val;
                    else
                        return 0;
                }
            }

            public int NumberOfConnects
            {
                get
                {
                    int val = 0;
                    CURLCode code = curl_easy_getinfo(CURL, CURLINFO.CURLINFO_NUM_CONNECTS, ref val);
                    if (code == CURLCode.CURLE_OK)
                        return val;
                    else
                        return 0;
                }
            }

            public int NumberOfRedirects
            {
                get
                {
                    int val = 0;
                    CURLCode code = curl_easy_getinfo(CURL, CURLINFO.CURLINFO_REDIRECT_COUNT, ref val);
                    if (code == CURLCode.CURLE_OK)
                        return val;
                    else
                        return 0;
                }
            }

            public int ReturnCode
            {
                get
                {
                    int val = 0;
                    CURLCode code = curl_easy_getinfo(CURL, CURLINFO.CURLINFO_RESPONSE_CODE, ref val);
                    if (code == CURLCode.CURLE_OK)
                        return val;
                    else
                        return 0;
                }
            }

            public double DownloadedLength
            {
                get
                {
                    double d = 0;
                    CURLCode code = curl_easy_getinfo_64(CURL, CURLINFO.CURLINFO_SIZE_DOWNLOAD, ref d);
                    if (code == CURLCode.CURLE_OK)
                        return d;
                    else
                        return 0;
                }
            }

            public double UploadedLength
            {
                get
                {
                    double d = 0;
                    CURLCode code = curl_easy_getinfo_64(CURL, CURLINFO.CURLINFO_SIZE_UPLOAD, ref d);
                    if (code == CURLCode.CURLE_OK)
                        return d;
                    else
                        return 0;
                }
            }

            public double DownloadSpeed
            {
                get
                {
                    double d = 0;
                    CURLCode code = curl_easy_getinfo_64(CURL, CURLINFO.CURLINFO_SPEED_DOWNLOAD, ref d);
                    if (code == CURLCode.CURLE_OK)
                        return d;
                    else
                        return 0;
                }
            }

            public double UploadSpeed
            {
                get
                {
                    double d = 0;
                    CURLCode code = curl_easy_getinfo_64(CURL, CURLINFO.CURLINFO_SPEED_UPLOAD, ref d);
                    if (code == CURLCode.CURLE_OK)
                        return d;
                    else
                        return 0;
                }
            }
        }

        /// <summary>
        ///     Easy Options
        /// </summary>
        public class EasyOptions
        {
            private HttpCUrlLibRequest parent;
            private IntPtr CURL { get { return parent.CURL; } }
            private CURLCode _last = CURLCode.CURLE_AGAIN;
            private EasyOptions() { }

            internal static EasyOptions FromLibRequest(HttpCUrlLibRequest owner)
            {
                EasyOptions res = new EasyOptions();
                res.parent = owner;
                return res;
            }

            /// <summary>
            ///     Last Code Returned by curl_easy_setopt
            /// </summary>
            public CURLCode LastCode { get { return _last; } }

            /// <summary>
            ///     Reset Options to default
            /// </summary>
            public void Reset()
            {
                curl_easy_reset(CURL);
            }

            /// <summary>
            ///     Reset Options to default
            /// </summary>
            public void Null()
            {
                curl_easy_reset(CURL);
            }

            /// <summary>
            ///     Set Options
            /// </summary>
            /// <param name="opt"></param>
            /// <param name="val"></param>
            /// <returns></returns>
            public CURLCode Set(CURLOPT opt, object val)
            {
                if (val is long)
                    return _last = curl_easy_setopt(CURL, opt, (long)val);
                if (val is IntPtr)
                    return _last = curl_easy_setopt(CURL, opt, (IntPtr)val);
                if (val is bool)
                    return _last = curl_easy_setopt(CURL, opt, (bool)val);
                if (val is string)
                    return _last = curl_easy_setopt(CURL, opt, (string)val);
                if ((val is int) || (val is uint) || (val is byte))
                    return _last = curl_easy_setopt(CURL, opt, (int)val);

                return _last = CURLCode.CURLE_UNKNOWN_OPTION;
            }

            /// <summary>
            ///     Set debug options
            /// </summary>
            /// <param name="val"></param>
            public bool Debug
            {
                set
                {
                    if (value)
                    {
                        _last = Set(CURLOPT.CURLOPT_FAILONERROR, false);
                        _last = Set(CURLOPT.CURLOPT_VERBOSE, true);
                        _last = Set(CURLOPT.CURLOPT_NOPROGRESS, false);
                        _last = Set(CURLOPT.CURLOPT_HEADER, true);
                    }
                    else
                    {
                        _last = Set(CURLOPT.CURLOPT_FAILONERROR, true);
                        _last = Set(CURLOPT.CURLOPT_VERBOSE, false);
                        _last = Set(CURLOPT.CURLOPT_NOPROGRESS, true);
                        _last = Set(CURLOPT.CURLOPT_HEADER, false);
                    };
                }
            }

            /// <summary>
            ///     Set HTTPs verification
            /// </summary>
            public bool SSLVerify
            {
                set
                {
                    if (value)
                    {
                        _last = Set(CURLOPT.CURLOPT_SSL_VERIFYHOST, 2);
                        _last = Set(CURLOPT.CURLOPT_SSL_VERIFYPEER, true);
                    }
                    else
                    {
                        _last = Set(CURLOPT.CURLOPT_SSL_VERIFYHOST, 0);
                        _last = Set(CURLOPT.CURLOPT_SSL_VERIFYPEER, false);
                    };
                }
            }

            public bool FOLLOWLOCATION { set { _last = Set(CURLOPT.CURLOPT_FOLLOWLOCATION, value); } }
            public bool FAILONERROR { set { _last = Set(CURLOPT.CURLOPT_FAILONERROR, value); } }
            public bool VERBOSE { set { _last = Set(CURLOPT.CURLOPT_VERBOSE, value); } }
            public bool IGNORE_CONTENT_LENGTH { set { _last = Set(CURLOPT.CURLOPT_IGNORE_CONTENT_LENGTH, value); } }
            public bool NOPROGRESS { set { _last = Set(CURLOPT.CURLOPT_NOPROGRESS, value); } }
            public bool SSL_VERIFYPEER { set { _last = Set(CURLOPT.CURLOPT_SSL_VERIFYPEER, value); } }
            public bool INSECURE { set { _last = Set(CURLOPT.CURLOPT_SSL_VERIFYPEER, !value); } }
            public string REFERER { set { _last = Set(CURLOPT.CURLOPT_REFERER, value); } }
            public string USERAGENT { set { _last = Set(CURLOPT.CURLOPT_USERAGENT, value); } }
            public int CONNECT_TIMEOUT { set { _last = Set(CURLOPT.CURLOPT_CONNECTTIMEOUT, value); } }
            public int TIMEOUT { set { Set(CURLOPT.CURLOPT_FTP_RESPONSE_TIMEOUT, value); _last = Set(CURLOPT.CURLOPT_TIMEOUT, value); } }
            public string PROXY_USERPWD { set { _last = Set(CURLOPT.CURLOPT_PROXYUSERPWD, value); } }
            public string USERPWD { set { _last = Set(CURLOPT.CURLOPT_USERPWD, value); } }
            public bool HEADER_IN_BODY_OUTPUT { set { _last = Set(CURLOPT.CURLOPT_HEADER, value); } }
            public SList HTTP_HEADER { set { _last = Set(CURLOPT.CURLOPT_HTTPHEADER, value.Handle); } }
            public bool HTTP_POST { set { _last = Set(CURLOPT.CURLOPT_POST, value); } }
            public bool HTTP_PUT { set { _last = Set(CURLOPT.CURLOPT_PUT, value); } }
            public bool HTTP_GET { set { _last = Set(CURLOPT.CURLOPT_HTTPGET, value); } }
            public string COOKIE_UPLOAD_FILE { set { _last = Set(CURLOPT.CURLOPT_COOKIEFILE, value); } }
            public string COOKIE_DNLOAD_FILE { set { _last = Set(CURLOPT.CURLOPT_COOKIEJAR, value); } }
            public string CUSTOMHTTPREQUEST { set { _last = Set(CURLOPT.CURLOPT_CUSTOMREQUEST, value); } }
            public int UPLOADSIZE { set { _last = Set(CURLOPT.CURLOPT_INFILESIZE, value); } }
            public int POSTDATA { set { _last = Set(CURLOPT.CURLOPT_POSTFIELDS, value); } }

            public SList GetCookies()
            {
                IntPtr pCookies = IntPtr.Zero;
                curl_easy_getinfo(CURL, CURLINFO.CURLINFO_COOKIELIST, ref pCookies);
                SList list = new SList(pCookies);
                return list;
            }

            /// <summary>
            ///     Set URL
            /// </summary>
            public string Url
            {
                set
                {
                    _last = Set(CURLOPT.CURLOPT_URL, value);
                }
                get
                {
                    return parent.Response.RequestUrl;
                }
            }
        }

        /// <summary>
        /// Contains values used to initialize libcurl internally. One of
        /// these is passed in the call to <see cref="Curl.GlobalInit"/>.
        /// </summary>
        public enum CURLinitFlag
        {
            /// <summary>
            /// Initialise nothing extra. This sets no bit.
            /// </summary>
            CURL_GLOBAL_NOTHING = 0,
            /// <summary>
            /// Initialize SSL.
            /// </summary>
            CURL_GLOBAL_SSL = 1,
            /// <summary>
            /// Initialize the Win32 socket libraries.
            /// </summary>
            CURL_GLOBAL_WIN32 = 2,
            /// <summary>
            /// Initialize everything possible. This sets all known bits.
            /// </summary>
            CURL_GLOBAL_ALL = 3,
            /// <summary>
            /// Equivalent to <c>CURL_GLOBAL_ALL</c>.
            /// </summary>
            CURL_GLOBAL_DEFAULT = CURL_GLOBAL_ALL
        };

        /// <summary>
        /// One of these is passed as the first parameter to
        /// <see cref="Easy.SetOpt"/>. The <c>Description</c> column of
        /// the table describes the value that should be passed as the second parameter.
        /// </summary>
        public enum CURLOPT
        {
            /// <summary>
            /// Pass a <c>true</c> parameter to enable this. When enabled, libcurl
            /// will automatically set the Referer: field in requests where it follows
            /// a Location: redirect. 
            /// </summary>
            CURLOPT_AUTOREFERER = 58,
            /// <summary>
            /// Pass an <c>int</c> specifying your preferred size for the receive buffer
            /// in libcurl. The main point of this would be that the write callback gets
            /// called more often and with smaller chunks. This is just treated as a
            /// request, not an order. You cannot be guaranteed to actually get the
            /// requested size. (Added in 7.10) 
            /// </summary>
            CURLOPT_BUFFERSIZE = 98,
            /// <summary>
            /// Pass a <c>string</c> naming a file holding one or more certificates
            /// to verify the peer with. This only makes sense when used in combination
            /// with the <c>CURLOPT_SSL_VERIFYPEER</c> option.
            /// </summary>
            CURLOPT_CAINFO = 10065,
            /// <summary>
            /// Pass a <c>string</c> naming a directory holding multiple CA certificates
            /// to verify the peer with. The certificate directory must be prepared
            /// using the openssl c_rehash utility. This only makes sense when used in
            /// combination with the <c>CURLOPT_SSL_VERIFYPEER</c> option. The
            /// <c>CURLOPT_CAPATH</c> function apparently does not work in Windows due
            /// to some limitation in openssl. (Added in 7.9.8) 
            /// </summary>
            CURLOPT_CAPATH = 10097,
            /// <summary>
            /// Pass an <c>int</c>. This option sets what policy libcurl should use when
            /// the connection cache is filled and one of the open connections has to be
            /// closed to make room for a new connection. This must be one of the
            /// <see cref="CURLclosePolicy"/> members. Use
            /// <see cref="CURLclosePolicy.CURLCLOSEPOLICY_LEAST_RECENTLY_USED"/> to make
            /// libcurl close the connection that was least recently used, that connection
            /// is also least likely to be capable of re-use. Use
            /// <see cref="CURLclosePolicy.CURLCLOSEPOLICY_OLDEST"/> to make libcurl close
            /// the oldest connection, the one that was created first among the ones in
            /// the connection cache. The other close policies are not supported yet.
            /// </summary>
            CURLOPT_CLOSEPOLICY = 72,
            /// <summary>
            /// Time-out connect operations after this amount of seconds, if connects
            /// are OK within this time, then fine... This only aborts the connect
            /// phase. [Only works on unix-style/SIGALRM operating systems]
            /// </summary>
            CURLOPT_CONNECTTIMEOUT = 78,
            /// <summary>
            /// Pass a <c>string</c> as parameter. It will be used to set a cookie
            /// in the http request. The format of the string should be NAME=CONTENTS,
            /// where NAME is the cookie name and CONTENTS is what the cookie should contain. 
            /// <para>
            /// If you need to set multiple cookies, you need to set them all using a
            /// single option and thus you need to concatenate them all in one single
            /// string. Set multiple cookies in one string like this:
            /// "name1=content1; name2=content2;" etc. 
            /// </para>
            /// <para>
            /// Using this option multiple times will only make the latest string override
            /// the previously ones.
            /// </para>
            /// </summary>
            CURLOPT_COOKIE = 10022,
            /// <summary>
            /// Pass a <c>string</c> as parameter. It should contain the name of your
            /// file holding cookie data to read. The cookie data may be in Netscape /
            /// Mozilla cookie data format or just regular HTTP-style headers dumped
            /// to a file.
            /// <para>
            /// Given an empty or non-existing file, this option will enable cookies
            /// for this Easy object, making it understand and parse received cookies
            /// and then use matching cookies in future request. 
            /// </para> 
            /// </summary>
            CURLOPT_COOKIEFILE = 10031,
            /// <summary>
            /// Pass a file name as <c>string</c>. This will make libcurl write all
            /// internally known cookies to the specified file when
            /// <see cref="Easy.Cleanup"/> is called. If no cookies are known, no file
            /// will be created. Using this option also enables cookies for this
            /// session, so if you for example follow a location it will make matching
            /// cookies get sent accordingly.
            /// <note>
            /// If the cookie jar file can't be created or written to
            /// (when <see cref="Easy.Cleanup"/> is called), libcurl will not and
            /// cannot report an error for this. Using <c>CURLOPT_VERBOSE</c> or
            /// <c>CURLOPT_DEBUGFUNCTION</c> will get a warning to display, but that
            /// is the only visible feedback you get about this possibly lethal situation.
            /// </note>
            /// </summary>
            CURLOPT_COOKIEJAR = 10082,
            /// <summary>
            /// Pass a <c>bool</c> set to <c>true</c> to mark this as a new cookie
            /// "session". It will force libcurl to ignore all cookies it is about to
            /// load that are "session cookies" from the previous session. By default,
            /// libcurl always stores and loads all cookies, independent of whether they are
            /// session cookies. Session cookies are cookies without expiry date and they
            /// are meant to be alive and existing for this "session" only.
            /// </summary>
            CURLOPT_COOKIESESSION = 96,
            /// <summary>
            /// Convert Unix newlines to CRLF newlines on transfers.
            /// </summary>
            CURLOPT_CRLF = 27,
            /// <summary>
            /// Pass a <c>string</c> as parameter. It will be used instead of GET or
            /// HEAD when doing an HTTP request, or instead of LIST or NLST when
            /// doing an ftp directory listing. This is useful for doing DELETE or
            /// other more or less obscure HTTP requests. Don't do this at will,
            /// make sure your server supports the command first. 
            /// <para>
            /// Restore to the internal default by setting this to <c>null</c>.
            /// </para>
            /// <note>
            /// Many people have wrongly used this option to replace the entire
            /// request with their own, including multiple headers and POST contents.
            /// While that might work in many cases, it will cause libcurl to send
            /// invalid requests and it could possibly confuse the remote server badly.
            /// Use <c>CURLOPT_POST</c> and <c>CURLOPT_POSTFIELDS</c> to set POST data.
            /// Use <c>CURLOPT_HTTPHEADER</c> to replace or extend the set of headers
            /// sent by libcurl. Use <c>CURLOPT_HTTP_VERSION</c> to change HTTP version.
            /// </note>
            /// </summary>
            CURLOPT_CUSTOMREQUEST = 10036,
            /// <summary>
            /// Pass an <c>object</c> referene to whatever you want passed to your
            /// <see cref="Easy.DebugFunction"/> delegate's <c>extraData</c> argument.
            /// This reference is not used internally by libcurl, it is only passed to
            /// the delegate. 
            /// </summary>
            CURLOPT_DEBUGDATA = 10095,
            /// <summary>
            /// Pass a reference to an <see cref="Easy.DebugFunction"/> delegate.
            /// <c>CURLOPT_VERBOSE</c> must be in effect. This delegate receives debug
            /// information, as specified with the <see cref="CURLINFOTYPE"/> argument.
            /// This function must return 0. 
            /// </summary>
            CURLOPT_DEBUGFUNCTION = 20094,
            /// <summary>
            /// Pass an <c>int</c>, specifying the timeout in seconds. Name resolves
            /// will be kept in memory for this number of seconds. Set to zero (0)
            /// to completely disable caching, or set to -1 to make the cached
            /// entries remain forever. By default, libcurl caches this info for 60
            /// seconds.
            /// </summary>
            CURLOPT_DNS_CACHE_TIMEOUT = 92,
            /// <summary>
            /// Not supported.
            /// </summary>
            CURLOPT_DNS_USE_GLOBAL_CACHE = 91,
            /// <summary>
            /// Pass a <c>string</c> containing the path name to the Entropy Gathering
            /// Daemon socket. It will be used to seed the random engine for SSL.
            /// </summary>
            CURLOPT_EDGSOCKET = 10077,
            /// <summary>
            /// Sets the contents of the Accept-Encoding: header sent in an HTTP request,
            /// and enables decoding of a response when a Content-Encoding: header is
            /// received. Three encodings are supported: <c>identity</c>, which does
            /// nothing, <c>deflate</c> which requests the server to compress its
            /// response using the zlib algorithm, and <c>gzip</c> which requests the
            /// gzip algorithm. If a zero-length string is set, then an Accept-Encoding:
            /// header containing all supported encodings is sent.
            /// </summary>
            CURLOPT_ENCODING = 10102,
            /// <summary>
            /// Not supported.
            /// </summary>
            CURLOPT_ERRORBUFFER = 10010,
            /// <summary>
            /// A <c>true</c> parameter tells the library to fail silently if the
            /// HTTP code returned is equal to or larger than 300. The default
            /// action would be to return the page normally, ignoring that code. 
            /// </summary>
            CURLOPT_FAILONERROR = 45,
            /// <summary>
            /// Pass a <c>bool</c>. If it is <c>true</c>, libcurl will attempt to get
            /// the modification date of the remote document in this operation. This
            /// requires that the remote server sends the time or replies to a time
            /// querying command. The <see cref="Easy.GetInfo"/> function with the
            /// <see cref="CURLINFO.CURLINFO_FILETIME"/> argument can be used after a
            /// transfer to extract the received time (if any).
            /// </summary>
            CURLOPT_FILETIME = 69,
            /// <summary>
            /// A <c>true</c> parameter tells the library to follow any Location:
            /// header that the server sends as part of an HTTP header.
            /// <note>
            /// this means that the library will re-send the same request on the
            /// new location and follow new Location: headers all the way until no
            /// more such headers are returned. <c>CURLOPT_MAXREDIRS</c> can be used
            /// to limit the number of redirects libcurl will follow.
            /// </note>
            /// </summary>
            CURLOPT_FOLLOWLOCATION = 52,
            /// <summary>
            /// Pass a <c>bool</c>. Set to <c>true</c> to make the next transfer
            /// explicitly close the connection when done. Normally, libcurl keeps all
            /// connections alive when done with one transfer in case there comes a
            /// succeeding one that can re-use them. This option should be used with
            /// caution and only if you understand what it does. Set to <c>false</c>
            /// to have libcurl keep the connection open for possibly later re-use
            /// (default behavior). 
            /// </summary>
            CURLOPT_FORBID_REUSE = 75,
            /// <summary>
            /// Pass a <c>bool</c>. Set to <c>true</c> to make the next transfer use a
            /// new (fresh) connection by force. If the connection cache is full before
            /// this connection, one of the existing connections will be closed as
            /// according to the selected or default policy. This option should be used
            /// with caution and only if you understand what it does. Set this to
            /// <c>false</c> to have libcurl attempt re-using an existing connection
            /// (default behavior). 
            /// </summary>
            CURLOPT_FRESH_CONNECT = 74,
            /// <summary>
            /// String that will be passed to the FTP server when it requests
            /// account info.
            /// </summary>
            CURLOPT_FTPACCOUNT = 10134,
            /// <summary>
            /// A <c>true</c> parameter tells the library to append to the remote
            /// file instead of overwrite it. This is only useful when uploading
            /// to an ftp site. 
            /// </summary>
            CURLOPT_FTPAPPEND = 50,
            /// <summary>
            /// A <c>true</c> parameter tells the library to just list the names of
            /// an ftp directory, instead of doing a full directory listing that
            /// would include file sizes, dates etc. 
            /// <para>
            /// This causes an FTP NLST command to be sent. Beware that some FTP
            /// servers list only files in their response to NLST; they might not
            /// include subdirectories and symbolic links.
            /// </para>
            /// </summary>
            CURLOPT_FTPLISTONLY = 48,
            /// <summary>
            /// Pass a <c>string</c> as parameter. It will be used to get the IP
            /// address to use for the ftp PORT instruction. The PORT instruction
            /// tells the remote server to connect to our specified IP address.
            /// The string may be a plain IP address, a host name, an network
            /// interface name (under Unix) or just a '-' letter to let the library
            /// use your systems default IP address. Default FTP operations are
            /// passive, and thus won't use PORT. 
            /// <para>
            /// You disable PORT again and go back to using the passive version
            /// by setting this option to NULL.
            /// </para>
            /// </summary>
            CURLOPT_FTPPORT = 10017,
            /// <summary>
            /// When FTP over SSL/TLS is selected (with <c>CURLOPT_FTP_SSL</c>),
            /// this option can be used to change libcurl's default action which
            /// is to first try "AUTH SSL" and then "AUTH TLS" in this order,
            /// and proceed when a OK response has been received.
            /// <para>
            /// Pass a member of the <see cref="CURLftpAuth"/> enumeration.
            /// </para>
            /// </summary>
            CURLOPT_FTPSSLAUTH = 129,
            /// <summary>
            /// Pass a <c>bool</c>. If the value is <c>true</c>, cURL will attempt to
            /// create any remote directory that it fails to CWD into. CWD is the
            /// command that changes working directory. (Added in 7.10.7) 
            /// </summary>
            CURLOPT_FTP_CREATE_MISSING_DIRS = 110,
            /// <summary>
            /// Pass an <c>int</c>. Causes libcurl to set a timeout period (in seconds)
            /// on the amount of time that the server is allowed to take in order to
            /// generate a response message for a command before the session is
            /// considered hung. Note that while libcurl is waiting for a response, this
            /// value overrides <c>CURLOPT_TIMEOUT</c>. It is recommended that if used in
            /// conjunction with <c>CURLOPT_TIMEOUT</c>, you set
            /// <c>CURLOPT_FTP_RESPONSE_TIMEOUT</c> to a value smaller than
            /// <c>CURLOPT_TIMEOUT</c>. (Added in 7.10.8) 
            /// </summary>
            CURLOPT_FTP_RESPONSE_TIMEOUT = 112,
            /// <summary>
            /// Pass a member of the <see cref="CURLftpSSL"/> enumeration.
            /// </summary>
            CURLOPT_FTP_SSL = 119,
            /// <summary>
            /// Pass a <c>bool</c>. If the value is <c>true</c>, it tells curl to use
            /// the EPRT (and LPRT) command when doing active FTP downloads (which is
            /// enabled by CURLOPT_FTPPORT). Using EPRT means that it will first attempt
            /// to use EPRT and then LPRT before using PORT, but if you pass <c>false</c>
            /// to this option, it will not try using EPRT or LPRT, only plain PORT.
            /// (Added in 7.10.5) 
            /// </summary>
            CURLOPT_FTP_USE_EPRT = 106,
            /// <summary>
            /// Pass a <c>bool</c>. If the value is <c>true</c>, it tells curl to use
            /// the EPSV command when doing passive FTP downloads (which it always does
            /// by default). Using EPSV means that it will first attempt to use EPSV
            /// before using PASV, but if you pass <c>false</c> to this option, it will
            /// not try using EPSV, only plain PASV.
            /// </summary>
            CURLOPT_FTP_USE_EPSV = 85,
            /// <summary>
            /// A <c>true</c> parameter tells the library to include the header in
            /// the body output. This is only relevant for protocols that actually
            /// have headers preceding the data (like HTTP).
            /// </summary>
            CURLOPT_HEADER = 42,
            /// <summary>
            /// Object reference to pass to the <see cref="Easy.HeaderFunction"/>
            /// delegate. Note that if you specify the <c>CURLOPT_HEADERFUNCTION</c>,
            /// this is the reference you'll get as the <c>extraData</c> parameter.
            /// </summary>
            CURLOPT_HEADERDATA = 10029,
            /// <summary>
            /// Provide an <see cref="Easy.HeaderFunction"/> delegate reference.
            /// This delegate gets called by libcurl as soon as there is received
            /// header data that needs to be written down. The headers are guaranteed
            /// to be written one-by-one and only complete lines are written. Parsing
            /// headers should be easy enough using this. The size of the data contained
            /// in <c>buf</c> is <c>size</c> multiplied with <c>nmemb</c>.
            /// Return the number of bytes actually written or return -1 to signal
            /// error to the library (it will cause it to abort the transfer with a
            /// <see cref="CURLcode.CURLE_WRITE_ERROR"/> return code). 
            /// </summary>
            CURLOPT_HEADERFUNCTION = 20079,
            /// <summary>
            /// Pass an <see cref="Slist"/> of aliases to be treated as valid HTTP
            /// 200 responses. Some servers respond with a custom header response line.
            /// For example, IceCast servers respond with "ICY 200 OK". By including
            /// this string in your list of aliases, the response will be treated as a
            /// valid HTTP header line such as "HTTP/1.0 200 OK". (Added in 7.10.3) 
            /// <note>
            /// The alias itself is not parsed for any version strings. So if your alias
            /// is "MYHTTP/9.9", libcurl will not treat the server as responding with
            /// HTTP version 9.9. Instead libcurl will use the value set by option
            /// <c>CURLOPT_HTTP_VERSION</c>. 
            /// </note>
            /// </summary>
            CURLOPT_HTTP200ALIASES = 10104,
            /// <summary>
            /// Pass an <c>int</c> as parameter, which is set to a bitmask 
            /// of <see cref="CURLhttpAuth"/>, to tell libcurl what authentication
            /// method(s) you want it to use. If more than one bit is set, libcurl will
            /// first query the site to see what authentication methods it supports and
            /// then pick the best one you allow it to use. Note that for some methods,
            /// this will induce an extra network round-trip. Set the actual name and
            /// password with the <c>CURLOPT_USERPWD</c> option. (Added in 7.10.6) 
            /// </summary>
            CURLOPT_HTTPAUTH = 107,
            /// <summary>
            /// Pass a <c>bool</c>. If it's <c>true</c>, this forces the HTTP request
            /// to get back to GET. Usable if a POST, HEAD, PUT or a custom request
            /// has been used previously using the same <see cref="Easy"/> object.
            /// </summary>
            CURLOPT_HTTPGET = 80,
            CURLOPT_IGNORE_CONTENT_LENGTH = 136,
            /// <summary>
            /// Pass an <see cref="Slist"/> reference containing HTTP headers to pass to
            /// the server in your HTTP request. If you add a header that is otherwise
            /// generated and used by libcurl internally, your added one will be used
            /// instead. If you add a header with no contents as in 'Accept:' (no data
            /// on the right side of the colon), the internally used header will get
            /// disabled. Thus, using this option you can add new headers, replace
            /// internal headers and remove internal headers. The headers included in the
            /// <c>Slist</c> must not be CRLF-terminated, because curl adds CRLF after
            /// each header item. Failure to comply with this will result in strange bugs
            /// because the server will most likely ignore part of the headers you specified. 
            /// <para>
            /// The first line in a request (usually containing a GET or POST) is not
            /// a header and cannot be replaced using this option. Only the lines
            /// following the request-line are headers. 
            /// </para>
            /// <para>
            /// Pass a <c>null</c> to this to reset back to no custom headers.
            /// </para>
            /// <note>
            /// The most commonly replaced headers have "shortcuts" in the options
            /// <c>CURLOPT_COOKIE</c>, <c>CURLOPT_USERAGENT</c> and <c>CURLOPT_REFERER</c>.
            /// </note>
            /// </summary>
            CURLOPT_HTTPHEADER = 10023,
            /// <summary>
            /// Tells libcurl you want a multipart/formdata HTTP POST to be made and you
            /// instruct what data to pass on to the server. Pass a reference to a 
            /// <see cref="MultiPartForm"/> object as parameter.
            /// The best and most elegant way to do this, is to use 
            /// <see cref="MultiPartForm.AddSection"/> as documented.
            /// <para>
            /// Using POST with HTTP 1.1 implies the use of a "Expect: 100-continue"
            /// header. You can disable this header with <c>CURLOPT_HTTPHEADER</c> as usual.
            /// </para> 
            /// </summary>
            CURLOPT_HTTPPOST = 10024,
            /// <summary>
            /// Set the parameter to <c>true</c> to get the library to tunnel all
            /// operations through a given HTTP proxy. Note that there is a big
            /// difference between using a proxy and tunneling through it. If you
            /// don't know what this means, you probably don't want this tunneling option. 
            /// </summary>
            CURLOPT_HTTPPROXYTUNNEL = 61,
            /// <summary>
            /// Pass a member of the <see cref="CURLhttpVersion"/> enumeration. These
            /// values force libcurl to use the specific HTTP versions. This is not
            /// sensible to do unless you have a good reason. 
            /// </summary>
            CURLOPT_HTTP_VERSION = 84,
            /// <summary>
            /// Provide an <see cref="Easy.IoctlFunction"/> delegate reference.
            /// This delegate gets called by libcurl when an IOCTL operation,
            /// such as a rewind of a file being sent via FTP, is required on
            /// the client side.
            /// </summary>
            CURLOPT_IOCTLFUNCTION = 20130,
            /// <summary>
            /// Provide an object, such as a <c>FileStream</c>, upon which
            /// you may need to perform an IOCTL operation. Right now, only
            /// rewind is supported.
            /// </summary>
            CURLOPT_IOCTLDATA = 10131,
            /// <summary>
            /// When uploading a file to a remote site, this option should be used to
            /// tell libcurl what the expected size of the infile is. This value should
            /// be passed as an <c>int</c>. 
            /// </summary>
            CURLOPT_INFILESIZE = 14,
            /// <summary>
            /// When uploading a file to a remote site, this option should be used to
            /// tell libcurl what the expected size of the infile is. This value should
            /// be passed as a <c>long</c>. (Added in 7.11.0) 
            /// </summary>
            CURLOPT_INFILESIZE_LARGE = 30115,
            /// <summary>
            /// Pass a <c>string</c> as parameter. This sets the interface name to use
            /// as the outgoing network interface. The name can be an interface name,
            /// an IP address or a host name.
            /// </summary>
            CURLOPT_INTERFACE = 10062,
            /// <summary>
            /// Pass one of the members of the <see cref="CURLipResolve"/> enumeration.
            /// </summary>
            CURLOPT_IPRESOLVE = 113,
            /// <summary>
            /// Pass a <c>string</c> as parameter. Set the kerberos4 security level;
            /// this also enables kerberos4 awareness. This is a string, 'clear', 'safe',
            /// 'confidential' or 'private'. If the string is set but doesn't match
            /// one of these, 'private' will be used. Set the string to <c>null</c>
            /// to disable kerberos4. The kerberos support only works for FTP.
            /// </summary>
            CURLOPT_KRB4LEVEL = 10063,
            /// <summary>
            /// Last numeric entry in the enumeration. Don't use this in your
            /// application code.
            /// </summary>
            CURLOPT_LASTENTRY = 135,
            /// <summary>
            /// Pass an <c>int</c> as parameter. It contains the transfer speed in bytes
            /// per second that the transfer should be below during
            /// <c>CURLOPT_LOW_SPEED_TIME</c> seconds for the library to consider it
            /// too slow and abort.
            /// </summary>
            CURLOPT_LOW_SPEED_LIMIT = 19,
            /// <summary>
            /// Pass an <c>int</c> as parameter. It contains the time in seconds that
            /// the transfer should be below the <c>CURLOPT_LOW_SPEED_LIMIT</c> for the
            /// library to consider it too slow and abort.
            /// </summary>
            CURLOPT_LOW_SPEED_TIME = 20,
            /// <summary>
            /// Pass an <c>int</c>. The set number will be the persistent connection
            /// cache size. The set amount will be the maximum amount of simultaneously
            /// open connections that libcurl may cache. Default is 5, and there isn't
            /// much point in changing this value unless you are perfectly aware of how
            /// this works and changes libcurl's behaviour. This concerns connections
            /// using any of the protocols that support persistent connections. 
            /// <para>
            /// When reaching the maximum limit, cURL uses the <c>CURLOPT_CLOSEPOLICY</c>
            /// to figure out which of the existing connections to close to prevent the
            /// number of open connections to increase. 
            /// </para>
            /// <note>
            /// if you already have performed transfers with this Easy object, setting a
            /// smaller <c>CURLOPT_MAXCONNECTS</c> than before may cause open connections
            /// to get closed unnecessarily.
            /// </note>
            /// </summary>
            CURLOPT_MAXCONNECTS = 71,
            /// <summary>
            /// Pass an <c>int</c> as parameter. This allows you to specify the maximum
            /// size (in bytes) of a file to download. If the file requested is larger
            /// than this value, the transfer will not start and
            /// <see cref="CURLcode.CURLE_FILESIZE_EXCEEDED"/> will be returned.
            /// <note>
            /// The file size is not always known prior to download, and for such files
            /// this option has no effect even if the file transfer ends up being larger
            /// than this given limit. This concerns both FTP and HTTP transfers. 
            /// </note> 
            /// </summary>
            CURLOPT_MAXFILESIZE = 114,
            /// <summary>
            /// Pass a <c>long</c> as parameter. This allows you to specify the
            /// maximum size (in bytes) of a file to download. If the file requested
            /// is larger than this value, the transfer will not start and
            /// <see cref="CURLcode.CURLE_FILESIZE_EXCEEDED"/> will be returned.
            /// (Added in 7.11.0) 
            /// <note>
            /// The file size is not always known prior to download, and for such files
            /// this option has no effect even if the file transfer ends up being larger
            /// than this given limit. This concerns both FTP and HTTP transfers. 
            /// </note>
            /// </summary>
            CURLOPT_MAXFILESIZE_LARGE = 30117,
            /// <summary>
            /// Pass an <c>int</c>. The set number will be the redirection limit. If
            /// that many redirections have been followed, the next redirect will cause
            /// an error (<c>CURLE_TOO_MANY_REDIRECTS</c>). This option only makes sense
            /// if the <c>CURLOPT_FOLLOWLOCATION</c> is used at the same time.
            /// </summary>
            CURLOPT_MAXREDIRS = 68,
            /// <summary>
            /// This parameter controls the preference of libcurl between using
            /// user names and passwords from your <c>~/.netrc</c> file, relative to
            /// user names and passwords in the URL supplied with <c>CURLOPT_URL</c>. 
            /// <note>
            /// libcurl uses a user name (and supplied or prompted password)
            /// supplied with <c>CURLOPT_USERPWD</c> in preference to any of the
            /// options controlled by this parameter.
            /// </note>
            /// <para>
            /// Pass a member of the <see cref="CURLnetrcOption"/> enumeration.
            /// </para>
            /// <para>
            /// Only machine name, user name and password are taken into account
            /// (init macros and similar things aren't supported).
            /// </para>
            /// <note>
            /// libcurl does not verify that the file has the correct properties
            /// set (as the standard Unix ftp client does). It should only be
            /// readable by user.
            /// </note>
            /// </summary>
            CURLOPT_NETRC = 51,
            /// <summary>
            /// Pass a <c>string</c> as parameter, containing the full path name to the
            /// file you want libcurl to use as .netrc file. If this option is omitted,
            /// and <c>CURLOPT_NETRC</c> is set, libcurl will attempt to find the a
            /// .netrc file in the current user's home directory. (Added in 7.10.9) 
            /// </summary>
            CURLOPT_NETRC_FILE = 10118,
            /// <summary>
            /// A <c>true</c> parameter tells the library to not include the
            /// body-part in the output. This is only relevant for protocols that
            /// have separate header and body parts. On HTTP(S) servers, this
            /// will make libcurl do a HEAD request. 
            /// <para>
            /// To change back to GET, you should use <c>CURLOPT_HTTPGET</c>. To
            /// change back to POST, you should use <c>CURLOPT_POST</c>. Setting
            /// <c>CURLOPT_NOBODY</c> to <c>false</c> has no effect.
            /// </para>
            /// </summary>
            CURLOPT_NOBODY = 44,
            /// <summary>
            /// A <c>true</c> parameter tells the library to shut off progress
            /// reporting.
            /// </summary>
            CURLOPT_NOPROGRESS = 43,
            /// <summary>
            /// Pass a <c>bool</c>. If it is <c>true</c>, libcurl will not use any
            /// functions that install signal handlers or any functions that cause
            /// signals to be sent to the process. This option is mainly here to allow
            /// multi-threaded unix applications to still set/use all timeout options
            /// etc, without risking getting signals. (Added in 7.10)
            /// <para>
            /// Consider using libcurl with ares built-in to enable asynchronous DNS
            /// lookups. It enables nice timeouts for name resolves without signals.
            /// </para> 
            /// </summary>
            CURLOPT_NOSIGNAL = 99,
            /// <summary>
            /// Not supported.
            /// </summary>
            CURLOPT_PASV_HOST = 126,
            /// <summary>
            /// Pass an <c>int</c> specifying what remote port number to connect to,
            /// instead of the one specified in the URL or the default port for the
            /// used protocol. 
            /// </summary>
            CURLOPT_PORT = 3,
            /// <summary>
            /// A <c>true</c> parameter tells the library to do a regular HTTP post.
            /// This will also make the library use the a "Content-Type:
            /// application/x-www-form-urlencoded" header. (This is by far the most
            /// commonly used POST method).
            /// <para>
            /// Use the <c>CURLOPT_POSTFIELDS</c> option to specify what data to post
            /// and <c>CURLOPT_POSTFIELDSIZE</c> to set the data size. Optionally,
            /// you can provide data to POST using the <c>CURLOPT_READFUNCTION</c> and
            /// <c>CURLOPT_READDATA</c> options.
            /// </para>
            /// <para>
            /// You can override the default POST Content-Type: header by setting
            /// your own with <c>CURLOPT_HTTPHEADER</c>. 
            /// </para>
            /// <para>
            /// Using POST with HTTP 1.1 implies the use of a "Expect: 100-continue"
            /// header. You can disable this header with <c>CURLOPT_HTTPHEADER</c> as usual.
            /// </para> 
            /// <para>
            /// If you use POST to a HTTP 1.1 server, you can send data without knowing
            /// the size before starting the POST if you use chunked encoding. You
            /// enable this by adding a header like "Transfer-Encoding: chunked" with
            /// <c>CURLOPT_HTTPHEADER</c>. With HTTP 1.0 or without chunked transfer,
            /// you must specify the size in the request. 
            /// </para>
            /// <note>
            /// if you have issued a POST request and want to make a HEAD or GET instead,
            /// you must explictly pick the new request type using <c>CURLOPT_NOBODY</c>
            /// or <c>CURLOPT_HTTPGET</c> or similar. 
            /// </note>
            /// </summary>
            CURLOPT_POST = 47,
            /// <summary>
            /// Pass a <c>string</c> as parameter, which should be the full data to post
            /// in an HTTP POST operation. You must make sure that the data is formatted
            /// the way you want the server to receive it. libcurl will not convert or
            /// encode it for you. Most web servers will assume this data to be
            /// url-encoded. Take note. 
            /// <para>
            /// This POST is a normal application/x-www-form-urlencoded kind (and
            /// libcurl will set that Content-Type by default when this option is used),
            /// which is the most commonly used one by HTML forms. See also the
            /// <c>CURLOPT_POST</c>. Using <c>CURLOPT_POSTFIELDS</c> implies
            /// <c>CURLOPT_POST</c>. 
            /// </para>
            /// <para>
            /// Using POST with HTTP 1.1 implies the use of a "Expect: 100-continue"
            /// header. You can disable this header with <c>CURLOPT_HTTPHEADER</c> as usual. 
            /// </para>
            /// <note>
            /// to make multipart/formdata posts (aka rfc1867-posts), check out the
            /// <c>CURLOPT_HTTPPOST</c> option.
            /// </note>
            /// </summary>
            CURLOPT_POSTFIELDS = 10015,
            /// <summary>
            /// If you want to post data to the server without letting libcurl do a
            /// <c>strlen()</c> to measure the data size, this option must be used. When
            /// this option is used you can post fully binary data, which otherwise
            /// is likely to fail. If this size is set to zero, the library will use
            /// <c>strlen()</c> to get the size.
            /// </summary>
            CURLOPT_POSTFIELDSIZE = 60,
            /// <summary>
            /// Pass a <c>long</c> as parameter. Use this to set the size of the
            /// <c>CURLOPT_POSTFIELDS</c> data to prevent libcurl from doing
            /// <c>strlen()</c> on the data to figure out the size. This is the large
            /// file version of the <c>CURLOPT_POSTFIELDSIZE</c> option. (Added in 7.11.1) 
            /// </summary>
            CURLOPT_POSTFIELDSIZE_LARGE = 30120,
            /// <summary>
            /// Pass an <see cref="Slist"/> of FTP commands to pass to the server after
            /// your ftp transfer request. Disable this operation again by setting this
            /// option to <c>null</c>. 
            /// </summary>
            CURLOPT_POSTQUOTE = 10039,
            /// <summary>
            /// Pass an <see cref="Slist"/> containing the FTP commands to pass to
            /// the server after the transfer type is set. Disable this operation
            /// again by setting a <c>null</c> to this option.
            /// </summary>
            CURLOPT_PREQUOTE = 10093,
            /// <summary>
            /// Pass an <c>object</c> as parameter, referencing data that should be
            /// associated with this <see cref="Easy"/> object. The object can
            /// subsequently be retrieved using <see cref="Easy.GetInfo"/> with the
            /// <see cref="CURLINFO.CURLINFO_PRIVATE"/> option. libcurl itself does
            /// nothing with this data. (Added in 7.10.3) 
            /// </summary>
            CURLOPT_PRIVATE = 10103,
            /// <summary>
            /// Pass an <c>object</c> reference that will be untouched by libcurl
            /// and passed as the first argument in the progress delegate set with
            /// <c>CURLOPT_PROGRESSFUNCTION</c>.
            /// </summary>
            CURLOPT_PROGRESSDATA = 10057,
            /// <summary>
            /// Pass an <see cref="Easy.ProgressFunction"/> delegate reference. This
            /// delegate gets called by libcurl at a frequent interval during data
            /// transfer. Unknown/unused argument values will be set to zero (like if
            /// you only download data, the upload size will remain 0). Returning a
            /// non-zero value from this delegate will cause libcurl to abort the
            /// transfer and return <see cref="CURLcode.CURLE_ABORTED_BY_CALLBACK"/>.
            /// <note>
            /// <c>CURLOPT_NOPROGRESS</c> must be set to <c>false</c> to make this
            /// function actually get called. 
            /// </note>
            /// </summary>
            CURLOPT_PROGRESSFUNCTION = 20056,
            /// <summary>
            /// Set HTTP proxy to use. The parameter should be a <c>string</c> holding
            /// the host name or dotted IP address. To specify port number in this
            /// string, append <c>:[port]</c> to the end of the host name. The proxy
            /// string may be prefixed with <c>[protocol]://</c> since any such prefix
            /// will be ignored. The proxy's port number may optionally be specified
            /// with the separate option <c>CURLOPT_PROXYPORT</c>. 
            /// <para>
            /// NOTE: when you tell the library to use an HTTP proxy, libcurl will
            /// transparently convert operations to HTTP even if you specify an FTP
            /// URL etc. This may have an impact on what other features of the library
            /// you can use, such as <c>CURLOPT_QUOTE</c> and similar FTP specifics
            /// that don't work unless you tunnel through the HTTP proxy. Such tunneling
            /// is activated with <c>CURLOPT_HTTPPROXYTUNNEL</c>. 
            /// </para>
            /// </summary>
            CURLOPT_PROXY = 10004,
            /// <summary>
            /// Pass a bitmask of <see cref="CURLhttpAuth"/> as the paramter, to tell
            /// libcurl what authentication method(s) you want it to use for your proxy
            /// authentication. If more than one bit is set, libcurl will first query the
            /// site to see what authentication methods it supports and then pick the best
            /// one you allow it to use. Note that for some methods, this will induce an
            /// extra network round-trip. Set the actual name and password with the
            /// <c>CURLOPT_PROXYUSERPWD</c> option. The bitmask can be constructed by
            /// or'ing together the <see cref="CURLhttpAuth"/> bits. As of this writing,
            /// only <see cref="CURLhttpAuth.CURLAUTH_BASIC"/> and
            /// <see cref="CURLhttpAuth.CURLAUTH_NTLM"/> work. (Added in 7.10.7) 
            /// </summary>
            CURLOPT_PROXYAUTH = 111,
            /// <summary>
            /// Pass an <c>int</c> with this option to set the proxy port to connect
            /// to unless it is specified in the proxy string <c>CURLOPT_PROXY</c>.
            /// </summary>
            CURLOPT_PROXYPORT = 59,
            /// <summary>
            /// Pass a <see cref="CURLproxyType"/> to set type of the proxy.
            /// </summary>
            CURLOPT_PROXYTYPE = 101,
            /// <summary>
            /// Pass a <c>string</c> as parameter, which should be
            /// <c>[user name]:[password]</c> to use for the connection to the
            /// HTTP proxy. Use <c>CURLOPT_PROXYAUTH</c> to decide authentication method. 
            /// </summary>
            CURLOPT_PROXYUSERPWD = 10006,
            /// <summary>
            /// A <c>true</c> parameter tells the library to use HTTP PUT to transfer
            /// data. The data should be set with <c>CURLOPT_READDATA</c> and
            /// <c>CURLOPT_INFILESIZE</c>. 
            /// <para>
            /// This option is deprecated and starting with version 7.12.1 you should
            /// instead use <c>CURLOPT_UPLOAD</c>. 
            /// </para>
            /// </summary>
            CURLOPT_PUT = 54,
            /// <summary>
            /// Pass a reference to an <see cref="Slist"/> containing FTP commands to
            /// pass to the server prior to your ftp request. This will be done before
            /// any other FTP commands are issued (even before the CWD command).
            /// Disable this operation again by setting a null to this option.
            /// </summary>
            CURLOPT_QUOTE = 10028,
            /// <summary>
            /// Pass a <c>string</c> containing the file name. The file will be used
            /// to read from to seed the random engine for SSL. The more random the
            /// specified file is, the more secure the SSL connection will become.
            /// </summary>
            CURLOPT_RANDOM_FILE = 10076,
            /// <summary>
            /// Pass a <c>string</c> as parameter, which should contain the
            /// specified range you want. It should be in the format <c>X-Y</c>, where X
            /// or Y may be left out. HTTP transfers also support several intervals,
            /// separated with commas as in <c>X-Y,N-M</c>. Using this kind of multiple
            /// intervals will cause the HTTP server to send the response document
            /// in pieces (using standard MIME separation techniques). Pass a
            /// <c>null</c> to this option to disable the use of ranges. 
            /// </summary>
            CURLOPT_RANGE = 10007,
            /// <summary>
            /// Object reference to pass to the <see cref="Easy.ReadFunction"/>
            /// delegate. Note that if you specify the <c>CURLOPT_READFUNCTION</c>,
            /// this is the reference you'll get as input.
            /// </summary>
            CURLOPT_READDATA = 10009,
            /// <summary>
            /// Pass a reference to an <see cref="Easy.ReadFunction"/> delegate.
            /// This delegate gets called by libcurl as soon as it needs to read data
            /// in order to send it to the peer. The data area referenced by the
            /// <c>buf</c> may be filled with at most <c>size</c> multiplied with
            /// <c>nmemb</c> number of bytes. Your function must return the actual
            /// number of bytes that you stored in that byte array. Returning 0 will
            /// signal end-of-file to the library and cause it to stop the current transfer. 
            /// <para>
            /// If you stop the current transfer by returning 0 "pre-maturely"
            /// (i.e before the server expected it, like when you've told you will
            /// upload N bytes and you upload less than N bytes), you may experience that
            /// the server "hangs" waiting for the rest of the data that won't come. 
            /// </para>
            /// </summary>
            CURLOPT_READFUNCTION = 20012,
            /// <summary>
            /// Pass a <c>string</c> as parameter. It will be used to set the Referer:
            /// header in the http request sent to the remote server. This can be used
            /// to fool servers or scripts. You can also set any custom header with
            /// <c>CURLOPT_HTTPHEADER</c>. 
            /// </summary>
            CURLOPT_REFERER = 10016,
            /// <summary>
            /// Pass an <c>int</c> as parameter. It contains the offset in number of
            /// bytes that you want the transfer to start from. Set this option to 0
            /// to make the transfer start from the beginning (effectively disabling resume). 
            /// </summary>
            CURLOPT_RESUME_FROM = 21,
            /// <summary>
            /// Pass a <c>long</c> as parameter. It contains the offset in number of
            /// bytes that you want the transfer to start from. (Added in 7.11.0) 
            /// </summary>
            CURLOPT_RESUME_FROM_LARGE = 30116,
            /// <summary>
            /// Pass an initialized <see cref="Share"/> reference as a parameter.
            /// Setting this option will make this <see cref="Easy"/> object use the
            /// data from the Share object instead of keeping the data to itself. This
            /// enables several Easy objects to share data. If the Easy objects are used
            /// simultaneously, you MUST use the Share object's locking methods.
            /// See <see cref="Share.SetOpt"/> for details.
            /// </summary>
            CURLOPT_SHARE = 10100,
            /// <summary>
            /// Not supported.
            /// </summary>
            CURLOPT_SOURCE_HOST = 10122,
            /// <summary>
            /// Not supported.
            /// </summary>
            CURLOPT_SOURCE_PATH = 10124,
            /// <summary>
            /// Not supported.
            /// </summary>
            CURLOPT_SOURCE_PORT = 125,
            /// <summary>
            /// When doing a third-party transfer, set the source post-quote list,
            /// as an <see cref="Slist"/>.
            /// </summary>
            CURLOPT_SOURCE_POSTQUOTE = 10128,
            /// <summary>
            /// When doing a third-party transfer, set the source pre-quote list,
            /// as an <see cref="Slist"/>.
            /// </summary>
            CURLOPT_SOURCE_PREQUOTE = 10127,
            /// <summary>
            /// When doing a third-party transfer, set a quote list,
            /// as an <see cref="Slist"/>.
            /// </summary>
            CURLOPT_SOURCE_QUOTE = 10133,
            /// <summary>
            /// Set the source URL for a third-party transfer.
            /// </summary>
            CURLOPT_SOURCE_URL = 10132,
            /// <summary>
            /// When doing 3rd party transfer, set the source user and password, as
            /// a <c>string</c> with format <c>user:password</c>.
            /// </summary>
            CURLOPT_SOURCE_USERPWD = 10123,
            /// <summary>
            /// Pass a <c>string</c> as parameter. The string should be the file name
            /// of your certificate. The default format is "PEM" and can be changed
            /// with <c>CURLOPT_SSLCERTTYPE</c>.
            /// </summary>
            CURLOPT_SSLCERT = 10025,
            /// <summary>
            /// Pass a <c>string</c> as parameter. It will be used as the password
            /// required to use the <c>CURLOPT_SSLCERT</c> certificate. 
            /// <para>
            /// This option is replaced by <c>CURLOPT_SSLKEYPASSWD</c> and should only
            /// be used for backward compatibility. You never needed a pass phrase to
            /// load a certificate but you need one to load your private key.
            /// </para>
            /// </summary>
            CURLOPT_SSLCERTPASSWD = 10026,
            /// <summary>
            /// Pass a <c>string</c> as parameter. The string should be the format of
            /// your certificate. Supported formats are "PEM" and "DER". (Added in 7.9.3) 
            /// </summary>
            CURLOPT_SSLCERTTYPE = 10086,
            /// <summary>
            /// Pass a <c>string</c> as parameter. It will be used as the identifier
            /// for the crypto engine you want to use for your private key.
            /// <note>
            /// If the crypto device cannot be loaded, 
            /// <see cref="CURLcode.CURLE_SSL_ENGINE_NOTFOUND"/> is returned.
            /// </note>
            /// </summary>
            CURLOPT_SSLENGINE = 10089,
            /// <summary>
            /// Sets the actual crypto engine as the default for (asymmetric)
            /// crypto operations.
            /// <note>
            /// If the crypto device cannot be set,
            /// <see cref="CURLcode.CURLE_SSL_ENGINE_SETFAILED"/> is returned. 
            /// </note>
            /// </summary>
            CURLOPT_SSLENGINE_DEFAULT = 90,
            /// <summary>
            /// Pass a <c>string</c> as parameter. The string should be the file name
            /// of your private key. The default format is "PEM" and can be changed
            /// with <c>CURLOPT_SSLKEYTYPE</c>. 
            /// </summary>
            CURLOPT_SSLKEY = 10087,
            /// <summary>
            /// Pass a <c>string</c> as parameter. It will be used as the password
            /// required to use the <c>CURLOPT_SSLKEY</c> private key.
            /// </summary>
            CURLOPT_SSLKEYPASSWD = 10026,
            /// <summary>
            /// Pass a <c>string</c> as parameter. The string should be the format of
            /// your private key. Supported formats are "PEM", "DER" and "ENG". 
            /// <note>
            /// The format "ENG" enables you to load the private key from a crypto
            /// engine. In this case <c>CURLOPT_SSLKEY</c> is used as an identifier
            /// passed to the engine. You have to set the crypto engine with
            /// <c>CURLOPT_SSLENGINE</c>. "DER" format key file currently does not
            /// work because of a bug in OpenSSL. 
            /// </note>
            /// </summary>
            CURLOPT_SSLKEYTYPE = 10088,
            /// <summary>
            /// Pass a member of the <see cref="CURLsslVersion"/> enumeration as the
            /// parameter to set the SSL version to use. By default
            /// the SSL library will try to solve this by itself although some servers
            /// servers make this difficult why you at times may have to use this
            /// option.
            /// </summary>
            CURLOPT_SSLVERSION = 32,
            /// <summary>
            /// Pass a <c>string</c> holding the list of ciphers to use for the SSL
            /// connection. The list must be syntactically correct, it consists of
            /// one or more cipher strings separated by colons. Commas or spaces are
            /// also acceptable separators but colons are normally used, !, - and +
            /// can be used as operators. Valid examples of cipher lists include
            /// 'RC4-SHA', �SHA1+DES�, 'TLSv1' and 'DEFAULT'. The default list is
            /// normally set when you compile OpenSSL.
            /// <para>
            /// You'll find more details about cipher lists on this URL:
            /// http://www.openssl.org/docs/apps/ciphers.html 
            /// </para>
            /// </summary>
            CURLOPT_SSL_CIPHER_LIST = 10083,
            /// <summary>
            /// Object reference to pass to the ssl context delegate set by the option
            /// <c>CURLOPT_SSL_CTX_FUNCTION</c>, this is the pointer you'll get as the
            /// second parameter, otherwise <c>null</c>. (Added in 7.11.0) 
            /// </summary>
            CURLOPT_SSL_CTX_DATA = 10109,
            /// <summary>
            /// Reference to an <see cref="Easy.SSLContextFunction"/> delegate.
            /// This delegate gets called by libcurl just before the initialization of
            /// an SSL connection after having processed all other SSL related options
            /// to give a last chance to an application to modify the behaviour of
            /// openssl's ssl initialization. The <see cref="SSLContext"/> parameter
            /// wraps a pointer to an openssl SSL_CTX. If an error is returned no attempt
            /// to establish a connection is made and the perform operation will return
            /// the error code from this callback function. Set the parm argument with
            /// the <c>CURLOPT_SSL_CTX_DATA</c> option. This option was introduced
            /// in 7.11.0.
            /// <note>
            /// To use this properly, a non-trivial amount of knowledge of the openssl
            /// libraries is necessary. Using this function allows for example to use
            /// openssl callbacks to add additional validation code for certificates,
            /// and even to change the actual URI of an HTTPS request.
            /// </note>
            /// </summary>
            CURLOPT_SSL_CTX_FUNCTION = 20108,
            /// <summary>
            /// Pass an <c>int</c>. Set if we should verify the common name from the
            /// peer certificate in the SSL handshake, set 1 to check existence, 2 to
            /// ensure that it matches the provided hostname. This is by default set
            /// to 2. (default changed in 7.10) 
            /// </summary>
            CURLOPT_SSL_VERIFYHOST = 81,
            /// <summary>
            /// Pass a <c>bool</c> that is set to <c>false</c> to stop curl from
            /// verifying the peer's certificate (7.10 starting setting this option
            /// to non-zero by default). Alternate certificates to verify against
            /// can be specified with the <c>CURLOPT_CAINFO</c> option or a
            /// certificate directory can be specified with the <c>CURLOPT_CAPATH</c>
            /// option. As of 7.10, curl installs a default bundle.
            /// <c>CURLOPT_SSL_VERIFYHOST</c> may also need to be set to 1
            /// or 0 if <c>CURLOPT_SSL_VERIFYPEER</c> is disabled (it defaults to 2). 
            /// </summary>
            CURLOPT_SSL_VERIFYPEER = 64,
            /// <summary>
            /// Not supported.
            /// </summary>
            CURLOPT_STDERR = 10037,
            /// <summary>
            /// Pass a <c>bool</c> specifying whether the TCP_NODELAY option should be
            /// set or cleared (<c>true</c> = set, <c>false</c> = clear). The option is
            /// cleared by default. This will have no effect after the connection has
            /// been established.
            /// <para>
            /// Setting this option will disable TCP's Nagle algorithm. The purpose of
            /// this algorithm is to try to minimize the number of small packets on the
            /// network (where "small packets" means TCP segments less than the Maximum
            /// Segment Size (MSS) for the network). 
            /// </para>
            /// <para>
            /// Maximizing the amount of data sent per TCP segment is good because it
            /// amortizes the overhead of the send. However, in some cases (most notably
            /// telnet or rlogin) small segments may need to be sent without delay. This
            /// is less efficient than sending larger amounts of data at a time, and can
            /// contribute to congestion on the network if overdone. 
            /// </para> 
            /// </summary>
            CURLOPT_TCP_NODELAY = 121,
            /// <summary>
            /// Provide an <see cref="Slist"/> with variables to pass to the telnet
            /// negotiations. The variables should be in the format "option=value".
            /// libcurl supports the options 'TTYPE', 'XDISPLOC' and 'NEW_ENV'. See
            /// the TELNET standard for details. 
            /// </summary>
            CURLOPT_TELNETOPTIONS = 10070,
            /// <summary>
            /// Pass a member of the <see cref="CURLtimeCond"/> enumeration as
            /// parameter. This defines how the <c>CURLOPT_TIMEVALUE</c> time
            /// value is treated. This feature applies to HTTP and FTP. 
            /// <note>
            /// The last modification time of a file is not always known and in such
            /// instances this feature will have no effect even if the given time
            /// condition would have not been met.
            /// </note>
            /// </summary>
            CURLOPT_TIMECONDITION = 33,
            /// <summary>
            /// Pass a <c>int</c> as parameter containing the maximum time in seconds
            /// that you allow the libcurl transfer operation to take. Normally, name
            /// lookups can take a considerable time and limiting operations to less
            /// than a few minutes risk aborting perfectly normal operations. This
            /// option will cause curl to use the SIGALRM to enable time-outing
            /// system calls. 
            /// <note>
            /// this is not recommended to use in unix multi-threaded programs,
            /// as it uses signals unless <c>CURLOPT_NOSIGNAL</c> (see above) is set.
            /// </note>
            /// </summary>
            CURLOPT_TIMEOUT = 13,
            /// <summary>
            /// Pass a <see cref="System.DateTime"/> as parameter. This time will be
            /// used in a condition as specified with <c>CURLOPT_TIMECONDITION</c>. 
            /// </summary>
            CURLOPT_TIMEVALUE = 34,
            /// <summary>
            /// A <c>true</c> parameter tells the library to use ASCII mode for ftp
            /// transfers, instead of the default binary transfer. For LDAP transfers
            /// it gets the data in plain text instead of HTML and for win32 systems
            /// it does not set the stdout to binary mode. This option can be usable
            /// when transferring text data between systems with different views on
            /// certain characters, such as newlines or similar.
            /// </summary>
            CURLOPT_TRANSFERTEXT = 53,
            /// <summary>
            /// A <c>true</c> parameter tells the library it can continue to send
            /// authentication (user+password) when following locations, even when
            /// hostname changed. Note that this is meaningful only when setting
            /// <c>CURLOPT_FOLLOWLOCATION</c>.
            /// </summary>
            CURLOPT_UNRESTRICTED_AUTH = 105,
            /// <summary>
            /// A <c>true</c> parameter tells the library to prepare for an
            /// upload. The <c>CURLOPT_READDATA</c> and <c>CURLOPT_INFILESIZE</c>
            /// or <c>CURLOPT_INFILESIZE_LARGE</c> are also interesting for uploads.
            /// If the protocol is HTTP, uploading means using the PUT request
            /// unless you tell libcurl otherwise. 
            /// <para>
            /// Using PUT with HTTP 1.1 implies the use of a "Expect: 100-continue"
            /// header. You can disable this header with <c>CURLOPT_HTTPHEADER</c> as usual. 
            /// </para>
            /// <para>
            /// If you use PUT to a HTTP 1.1 server, you can upload data without
            /// knowing the size before starting the transfer if you use chunked
            /// encoding. You enable this by adding a header like
            /// "Transfer-Encoding: chunked" with <c>CURLOPT_HTTPHEADER</c>. With
            /// HTTP 1.0 or without chunked transfer, you must specify the size.
            /// </para>
            /// </summary>
            CURLOPT_UPLOAD = 46,
            /// <summary>
            /// The actual URL to deal with. The parameter should be a <c>string</c>.
            /// If the given URL lacks the protocol part ("http://" or "ftp://" etc), it
            /// will attempt to guess which protocol to use based on the given host name.
            /// <para>If the given protocol of the set URL is not supported, libcurl will return
            /// an error <c>CURLcode.</c>(<see cref="CURLcode.CURLE_UNSUPPORTED_PROTOCOL"/>)
            /// when you call Easy's <see cref="Easy.Perform"/> or
            /// Multi's <see cref="Multi.Perform"/>.</para>
            /// <para>Use <see cref="Curl.GetVersionInfo"/> for detailed info
            /// on which protocols that are supported.</para>
            /// </summary>
            CURLOPT_URL = 10002,
            /// <summary>
            /// Pass a <c>string</c> as parameter. It will be used to set the
            /// User-Agent: header in the http request sent to the remote server.
            /// This can be used to fool servers or scripts. You can also set any
            /// custom header with <c>CURLOPT_HTTPHEADER</c>.
            /// </summary>
            CURLOPT_USERAGENT = 10018,
            /// <summary>
            /// Pass a <c>string</c> as parameter, which should be
            /// <c>[user name]:[password]</c> to use for the connection. Use
            /// <c>CURLOPT_HTTPAUTH</c> to decide authentication method. 
            /// <para>
            /// When using HTTP and <c>CURLOPT_FOLLOWLOCATION</c>, libcurl might
            /// perform several requests to possibly different hosts. libcurl will
            /// only send this user and password information to hosts using the
            /// initial host name (unless <c>CURLOPT_UNRESTRICTED_AUTH</c> is set),
            /// so if libcurl follows locations to other hosts it will not send the
            /// user and password to those. This is enforced to prevent accidental
            /// information leakage. 
            /// </para>
            /// </summary>
            CURLOPT_USERPWD = 10005,
            /// <summary>
            /// Set the parameter to <c>true</c> to get the library to display a lot
            /// of verbose information about its operations. Very useful for libcurl
            /// and/or protocol debugging and understanding. The verbose information
            /// will be sent to the <see cref="Easy.DebugFunction"/> delegate, if it's
            /// implemented. You hardly ever want this set in production use, you will
            /// almost always want this when you debug/report problems. 
            /// </summary>
            CURLOPT_VERBOSE = 41,
            /// <summary>
            /// Object reference to pass to the <see cref="Easy.WriteFunction"/>
            /// delegate. Note that if you specify the <c>CURLOPT_WRITEFUNCTION</c>,
            /// this is the object you'll get as input. 
            /// </summary>
            CURLOPT_WRITEDATA = 10001,
            /// <summary>
            /// Pass a reference to an <see cref="Easy.WriteFunction"/> delegate.
            /// The delegate gets called by libcurl as soon as there is data received
            /// that needs to be saved. The size of the data referenced by <c>buf</c>
            /// is <c>size</c> multiplied with <c>nmemb</c>, it will not be zero
            /// terminated. Return the number of bytes actually taken care of. If
            /// that amount differs from the amount passed to your function, it'll
            /// signal an error to the library and it will abort the transfer and
            /// return <c>CURLcode.</c><see cref="CURLcode.CURLE_WRITE_ERROR"/>. 
            /// <note>This function may be called with zero bytes data if the
            /// transfered file is empty.</note>
            /// </summary>
            CURLOPT_WRITEFUNCTION = 20011,
            /// <summary>
            /// Pass a <c>string</c> of the output using full variable-replacement
            /// as described elsewhere.
            /// </summary>
            CURLOPT_WRITEINFO = 10040,
        };

        /// <summary>
        /// Status code returned from <see cref="Easy"/> functions.
        /// </summary>
        public enum CURLCode
        {
            /// <summary>
            /// All fine. Proceed as usual.
            /// </summary>
            CURLE_OK = 0,
            /// <summary>
            /// Aborted by callback. An internal callback returned "abort"
            /// to libcurl. 
            /// </summary>
            CURLE_ABORTED_BY_CALLBACK = 42,
            /// <summary>
            /// Internal error. A function was called in a bad order.
            /// </summary>
            CURLE_BAD_CALLING_ORDER = 44,
            /// <summary>
            /// Unrecognized transfer encoding.
            /// </summary>
            CURLE_BAD_CONTENT_ENCODING = 61,
            /// <summary>
            /// Attempting FTP resume beyond file size.
            /// </summary>
            CURLE_BAD_DOWNLOAD_RESUME = 36,
            /// <summary>
            /// Internal error. A function was called with a bad parameter.
            /// </summary>
            CURLE_BAD_FUNCTION_ARGUMENT = 43,
            /// <summary>
            /// Bad password entered. An error was signaled when the password was
            /// entered. This can also be the result of a "bad password" returned
            /// from a specified password callback. 
            /// </summary>
            CURLE_BAD_PASSWORD_ENTERED = 46,
            /// <summary>
            /// Failed to connect to host or proxy. 
            /// </summary>
            CURLE_COULDNT_CONNECT = 7,
            /// <summary>
            /// Couldn't resolve host. The given remote host was not resolved. 
            /// </summary>
            CURLE_COULDNT_RESOLVE_HOST = 6,
            /// <summary>
            /// Couldn't resolve proxy. The given proxy host could not be resolved.
            /// </summary>
            CURLE_COULDNT_RESOLVE_PROXY = 5,
            /// <summary>
            /// Very early initialization code failed. This is likely to be an
            /// internal error or problem. 
            /// </summary>
            CURLE_FAILED_INIT = 2,
            /// <summary>
            /// Maximum file size exceeded.
            /// </summary>
            CURLE_FILESIZE_EXCEEDED = 63,
            /// <summary>
            /// A file given with FILE:// couldn't be opened. Most likely
            /// because the file path doesn't identify an existing file. Did
            /// you check file permissions? 
            /// </summary>
            CURLE_FILE_COULDNT_READ_FILE = 37,
            /// <summary>
            /// We were denied access when trying to login to an FTP server or
            /// when trying to change working directory to the one given in the URL. 
            /// </summary>
            CURLE_FTP_ACCESS_DENIED = 9,
            /// <summary>
            /// An internal failure to lookup the host used for the new
            /// connection.
            /// </summary>
            CURLE_FTP_CANT_GET_HOST = 15,
            /// <summary>
            /// A bad return code on either PASV or EPSV was sent by the FTP
            /// server, preventing libcurl from being able to continue. 
            /// </summary>
            CURLE_FTP_CANT_RECONNECT = 16,
            /// <summary>
            /// The FTP SIZE command returned error. SIZE is not a kosher FTP
            /// command, it is an extension and not all servers support it. This
            /// is not a surprising error. 
            /// </summary>
            CURLE_FTP_COULDNT_GET_SIZE = 32,
            /// <summary>
            /// This was either a weird reply to a 'RETR' command or a zero byte
            /// transfer complete. 
            /// </summary>
            CURLE_FTP_COULDNT_RETR_FILE = 19,
            /// <summary>
            /// libcurl failed to set ASCII transfer type (TYPE A).
            /// </summary>
            CURLE_FTP_COULDNT_SET_ASCII = 29,
            /// <summary>
            /// Received an error when trying to set the transfer mode to binary.
            /// </summary>
            CURLE_FTP_COULDNT_SET_BINARY = 17,
            /// <summary>
            /// FTP couldn't STOR file. The server denied the STOR operation.
            /// The error buffer usually contains the server's explanation to this. 
            /// </summary>
            CURLE_FTP_COULDNT_STOR_FILE = 25,
            /// <summary>
            /// The FTP REST command returned error. This should never happen
            /// if the server is sane. 
            /// </summary>
            CURLE_FTP_COULDNT_USE_REST = 31,
            /// <summary>
            /// The FTP PORT command returned error. This mostly happen when
            /// you haven't specified a good enough address for libcurl to use.
            /// See <see cref="CURLoption.CURLOPT_FTPPORT"/>. 
            /// </summary>
            CURLE_FTP_PORT_FAILED = 30,
            /// <summary>
            /// When sending custom "QUOTE" commands to the remote server, one
            /// of the commands returned an error code that was 400 or higher. 
            /// </summary>
            CURLE_FTP_QUOTE_ERROR = 21,
            /// <summary>
            /// Requested FTP SSL level failed.
            /// </summary>
            CURLE_FTP_SSL_FAILED = 64,
            /// <summary>
            /// The FTP server rejected access to the server after the password
            /// was sent to it. It might be because the username and/or the
            /// password were incorrect or just that the server is not allowing
            /// you access for the moment etc. 
            /// </summary>
            CURLE_FTP_USER_PASSWORD_INCORRECT = 10,
            /// <summary>
            /// FTP servers return a 227-line as a response to a PASV command.
            /// If libcurl fails to parse that line, this return code is
            /// passed back. 
            /// </summary>
            CURLE_FTP_WEIRD_227_FORMAT = 14,
            /// <summary>
            /// After having sent the FTP password to the server, libcurl expects
            /// a proper reply. This error code indicates that an unexpected code
            /// was returned. 
            /// </summary>
            CURLE_FTP_WEIRD_PASS_REPLY = 11,
            /// <summary>
            /// libcurl failed to get a sensible result back from the server as
            /// a response to either a PASV or a EPSV command. The server is flawed. 
            /// </summary>
            CURLE_FTP_WEIRD_PASV_REPLY = 13,
            /// <summary>
            /// After connecting to an FTP server, libcurl expects to get a
            /// certain reply back. This error code implies that it got a strange
            /// or bad reply. The given remote server is probably not an
            /// OK FTP server. 
            /// </summary>
            CURLE_FTP_WEIRD_SERVER_REPLY = 8,
            /// <summary>
            /// After having sent user name to the FTP server, libcurl expects a
            /// proper reply. This error code indicates that an unexpected code
            /// was returned. 
            /// </summary>
            CURLE_FTP_WEIRD_USER_REPLY = 12,
            /// <summary>
            /// After a completed file transfer, the FTP server did not respond a
            /// proper "transfer successful" code. 
            /// </summary>
            CURLE_FTP_WRITE_ERROR = 20,
            /// <summary>
            /// Function not found. A required LDAP function was not found.
            /// </summary>
            CURLE_FUNCTION_NOT_FOUND = 41,
            /// <summary>
            /// Nothing was returned from the server, and under the circumstances,
            /// getting nothing is considered an error.
            /// </summary>
            CURLE_GOT_NOTHING = 52,
            /// <summary>
            /// This is an odd error that mainly occurs due to internal confusion.
            /// </summary>
            CURLE_HTTP_POST_ERROR = 34,
            /// <summary>
            /// The HTTP server does not support or accept range requests.
            /// </summary>
            CURLE_HTTP_RANGE_ERROR = 33,
            /// <summary>
            /// This is returned if <see cref="CURLoption.CURLOPT_FAILONERROR"/>
            /// is set TRUE and the HTTP server returns an error code that
            /// is >= 400. 
            /// </summary>
            CURLE_HTTP_RETURNED_ERROR = 22,
            /// <summary>
            /// Interface error. A specified outgoing interface could not be
            /// used. Set which interface to use for outgoing connections'
            /// source IP address with <see cref="CURLoption.CURLOPT_INTERFACE"/>. 
            /// </summary>
            CURLE_INTERFACE_FAILED = 45,
            /// <summary>
            /// End-of-enumeration marker; do not use in client applications.
            /// </summary>
            CURLE_LAST = 67,
            /// <summary>
            /// LDAP cannot bind. LDAP bind operation failed.
            /// </summary>
            CURLE_LDAP_CANNOT_BIND = 38,
            /// <summary>
            /// Invalid LDAP URL.
            /// </summary>
            CURLE_LDAP_INVALID_URL = 62,
            /// <summary>
            /// LDAP search failed.
            /// </summary>
            CURLE_LDAP_SEARCH_FAILED = 39,
            /// <summary>
            /// Library not found. The LDAP library was not found.
            /// </summary>
            CURLE_LIBRARY_NOT_FOUND = 40,
            /// <summary>
            /// Malformat user. User name badly specified. *Not currently used*
            /// </summary>
            CURLE_MALFORMAT_USER = 24,
            /// <summary>
            /// This is not an error. This used to be another error code in an
            /// old libcurl version and is currently unused. 
            /// </summary>
            CURLE_OBSOLETE = 50,
            /// <summary>
            /// Operation timeout. The specified time-out period was reached
            /// according to the conditions. 
            /// </summary>
            CURLE_OPERATION_TIMEOUTED = 28,
            /// <summary>
            /// Out of memory. A memory allocation request failed. This is serious
            /// badness and things are severely messed up if this ever occurs. 
            /// </summary>
            CURLE_OUT_OF_MEMORY = 27,
            /// <summary>
            /// A file transfer was shorter or larger than expected. This
            /// happens when the server first reports an expected transfer size,
            /// and then delivers data that doesn't match the previously
            /// given size. 
            /// </summary>
            CURLE_PARTIAL_FILE = 18,
            /// <summary>
            /// There was a problem reading a local file or an error returned by
            /// the read callback. 
            /// </summary>
            CURLE_READ_ERROR = 26,
            /// <summary>
            /// Failure with receiving network data.
            /// </summary>
            CURLE_RECV_ERROR = 56,
            /// <summary>
            /// Failed sending network data.
            /// </summary>
            CURLE_SEND_ERROR = 55,
            /// <summary>
            /// Sending the data requires a rewind that failed.
            /// </summary>
            CURLE_SEND_FAIL_REWIND = 65,
            /// <summary>
            /// Share is in use.
            /// </summary>
            CURLE_SHARE_IN_USE = 57,
            /// <summary>
            /// Problem with the CA cert (path? access rights?) 
            /// </summary>
            CURLE_SSL_CACERT = 60,
            /// <summary>
            /// There's a problem with the local client certificate. 
            /// </summary>
            CURLE_SSL_CERTPROBLEM = 58,
            /// <summary>
            /// Couldn't use specified cipher. 
            /// </summary>
            CURLE_SSL_CIPHER = 59,
            /// <summary>
            /// A problem occurred somewhere in the SSL/TLS handshake. You really
            /// want to use the <see cref="Easy.DebugFunction"/> delegate and read
            /// the message there as it pinpoints the problem slightly more. It
            /// could be certificates (file formats, paths, permissions),
            /// passwords, and others. 
            /// </summary>
            CURLE_SSL_CONNECT_ERROR = 35,
            /// <summary>
            /// Failed to initialize SSL engine.
            /// </summary>
            CURLE_SSL_ENGINE_INITFAILED = 66,
            /// <summary>
            /// The specified crypto engine wasn't found. 
            /// </summary>
            CURLE_SSL_ENGINE_NOTFOUND = 53,
            /// <summary>
            /// Failed setting the selected SSL crypto engine as default!
            /// </summary>
            CURLE_SSL_ENGINE_SETFAILED = 54,
            /// <summary>
            /// The remote server's SSL certificate was deemed not OK.
            /// </summary>
            CURLE_SSL_PEER_CERTIFICATE = 51,
            /// <summary>
            /// A telnet option string was improperly formatted.
            /// </summary>
            CURLE_TELNET_OPTION_SYNTAX = 49,
            /// <summary>
            /// Too many redirects. When following redirects, libcurl hit the
            /// maximum amount. Set your limit with
            /// <see cref="CURLoption.CURLOPT_MAXREDIRS"/>. 
            /// </summary>
            CURLE_TOO_MANY_REDIRECTS = 47,
            /// <summary>
            /// An option set with <see cref="CURLoption.CURLOPT_TELNETOPTIONS"/>
            /// was not recognized/known. Refer to the appropriate documentation. 
            /// </summary>
            CURLE_UNKNOWN_TELNET_OPTION = 48,
            /// <summary>
            /// The URL you passed to libcurl used a protocol that this libcurl
            /// does not support. The support might be a compile-time option that
            /// wasn't used, it can be a misspelled protocol string or just a
            /// protocol libcurl has no code for. 
            /// </summary>
            CURLE_UNSUPPORTED_PROTOCOL = 1,
            /// <summary>
            /// The URL was not properly formatted. 
            /// </summary>
            CURLE_URL_MALFORMAT = 3,
            /// <summary>
            /// URL user malformatted. The user-part of the URL syntax was not
            /// correct. 
            /// </summary>
            CURLE_URL_MALFORMAT_USER = 4,
            /// <summary>
            /// An error occurred when writing received data to a local file,
            /// or an error was returned to libcurl from a write callback. 
            /// </summary>
            CURLE_WRITE_ERROR = 23,
            CURLE_NOT_BUILT_IN = 4,
            CURLE_WEIRD_SERVER_REPLY = 8,
            CURLE_REMOTE_ACCESS_DENIED = 9,
            CURLE_UPLOAD_FAILED = 25,
            CURLE_OPERATION_TIMEDOUT = 28,
            CURLE_UNKNOWN_OPTION = 48,
            CURLE_USE_SSL_FAILED = 64,
            CURLE_LOGIN_DENIED = 67,
            CURLE_REMOTE_FILE_EXISTS = 73,
            CURLE_REMOTE_FILE_NOT_FOUND = 78,
            CURLE_AGAIN = 81,
            CURLE_SSL_ISSUER_ERROR = 83,
            CURLE_NO_CONNECTION_AVAILABLE = 89,
            CURLE_AUTH_ERROR = 94,
            CURLE_PROXY = 97,
            CURLE_SSL_CLIENTCERT = 98
        }

        /// <summary>
        /// Contains return codes from many of the functions in the
        /// <see cref="Share"/> class.
        /// </summary>
        public enum CURLSHcode
        {
            /// <summary>
            /// The function succeeded.
            /// </summary>
            CURLSHE_OK = 0,
            /// <summary>
            /// A bad option was passed to <see cref="Share.SetOpt"/>.
            /// </summary>
            CURLSHE_BAD_OPTION = 1,
            /// <summary>
            /// An attempt was made to pass an option to
            /// <see cref="Share.SetOpt"/> while the Share object is in use.
            /// </summary>
            CURLSHE_IN_USE = 2,
            /// <summary>
            /// The <see cref="Share"/> object's internal handle is invalid.
            /// </summary>
            CURLSHE_INVALID = 3,
            /// <summary>
            /// Out of memory. This is a severe problem.
            /// </summary>
            CURLSHE_NOMEM = 4,
            /// <summary>
            /// End-of-enumeration marker; do not use in application code.
            /// </summary>
            CURLSHE_LAST = 5
        };

        /// <summary>
        /// This enumeration is used to extract information associated with an
        /// <see cref="Easy"/> transfer. Specifically, a member of this
        /// enumeration is passed as the first argument to
        /// <see cref="Easy.GetInfo"/> specifying the item to retrieve in the
        /// second argument, which is a reference to an <c>int</c>, a
        /// <c>double</c>, a <c>string</c>, a <c>DateTime</c> or an <c>object</c>.
        /// </summary>
        public enum CURLINFO
        {
            /// <summary>
            /// The second argument receives the elapsed time, as a <c>double</c>,
            /// in seconds, from the start until the connect to the remote host
            /// (or proxy) was completed. 
            /// </summary>
            CURLINFO_CONNECT_TIME = 0x300005,
            /// <summary>
            /// The second argument receives, as a <c>double</c>, the content-length
            /// of the download. This is the value read from the Content-Length: field. 
            /// </summary>
            CURLINFO_CONTENT_LENGTH_DOWNLOAD = 0x30000F,
            /// <summary>
            /// The second argument receives, as a <c>double</c>, the specified size
            /// of the upload. 
            /// </summary>
            CURLINFO_CONTENT_LENGTH_UPLOAD = 0x300010,
            /// <summary>
            /// The second argument receives, as a <c>string</c>, the content-type of
            /// the downloaded object. This is the value read from the Content-Type:
            /// field. If you get <c>null</c>, it means that the server didn't
            /// send a valid Content-Type header or that the protocol used
            /// doesn't support this. 
            /// </summary>
            CURLINFO_CONTENT_TYPE = 0x100012,
            /// <summary>
            /// The second argument receives, as a <c>string</c>, the last
            /// used effective URL. 
            /// </summary>
            CURLINFO_EFFECTIVE_URL = 0x100001,
            /// <summary>
            /// The second argument receives, as a <c>long</c>, the remote time
            /// of the retrieved document. You should construct a <c>DateTime</c>
            /// from this value, as shown in the <c>InfoDemo</c> sample. If you
            /// get a date in the distant
            /// past, it can be because of many reasons (unknown, the server
            /// hides it or the server doesn't support the command that tells
            /// document time etc) and the time of the document is unknown. Note
            /// that you must tell the server to collect this information before
            /// the transfer is made, by using the 
            /// <see cref="CURLoption.CURLOPT_FILETIME"/> option to
            /// <see cref="Easy.SetOpt"/>. (Added in 7.5) 
            /// </summary>
            CURLINFO_FILETIME = 0x20000E,
            /// <summary>
            /// The second argument receives an <c>int</c> specifying the total size
            /// of all the headers received. 
            /// </summary>
            CURLINFO_HEADER_SIZE = 0x20000B,
            /// <summary>
            /// The second argument receives, as an <c>int</c>, a bitmask indicating
            /// the authentication method(s) available. The meaning of the bits is
            /// explained in the documentation of
            /// <see cref="CURLoption.CURLOPT_HTTPAUTH"/>. (Added in 7.10.8) 
            /// </summary>
            CURLINFO_HTTPAUTH_AVAIL = 0x200017,
            /// <summary>
            /// The second argument receives an <c>int</c> indicating the numeric
            /// connect code for the HTTP request.
            /// </summary>
            CURLINFO_HTTP_CONNECTCODE = 0x200016,
            /// <summary>
            /// End-of-enumeration marker; do not use in client applications.
            /// </summary>
            CURLINFO_LASTONE = 0x1C,
            /// <summary>
            /// The second argument receives, as a <c>double</c>, the time, in
            /// seconds it took from the start until the name resolving was
            /// completed. 
            /// </summary>
            CURLINFO_NAMELOOKUP_TIME = 0x300004,
            /// <summary>
            /// Never used.
            /// </summary>
            CURLINFO_NONE = 0x0,
            /// <summary>
            /// The second argument receives an <c>int</c> indicating the
            /// number of current connections. (Added in 7.13.0)
            /// </summary>
            CURLINFO_NUM_CONNECTS = 0x20001A,
            /// <summary>
            /// The second argument receives an <c>int</c> indicating the operating
            /// system error number: <c>_errro</c> or <c>GetLastError()</c>,
            /// depending on the platform. (Added in 7.12.2)
            /// </summary>
            CURLINFO_OS_ERRNO = 0x200019,
            /// <summary>
            /// The second argument receives, as a <c>double</c>, the time, in
            /// seconds, it took from the start until the file transfer is just about
            /// to begin. This includes all pre-transfer commands and negotiations
            /// that are specific to the particular protocol(s) involved. 
            /// </summary>
            CURLINFO_PRETRANSFER_TIME = 0x300006,
            /// <summary>
            /// The second argument receives a reference to the private data
            /// associated with the <see cref="Easy"/> object (set with the
            /// <see cref="CURLoption.CURLOPT_PRIVATE"/> option to
            /// <see cref="Easy.SetOpt"/>. (Added in 7.10.3) 
            /// </summary>
            CURLINFO_PRIVATE = 0x100015,
            /// <summary>
            /// The second argument receives, as an <c>int</c>, a bitmask
            /// indicating the authentication method(s) available for your
            /// proxy authentication. This will be a bitmask of
            /// <see cref="CURLhttpAuth"/> enumeration constants.
            /// (Added in 7.10.8) 
            /// </summary>
            CURLINFO_PROXYAUTH_AVAIL = 0x200018,
            /// <summary>
            /// The second argument receives an <c>int</c> indicating the total
            /// number of redirections that were actually followed. (Added in 7.9.7) 
            /// </summary>
            CURLINFO_REDIRECT_COUNT = 0x200014,
            /// <summary>
            /// The second argument receives, as a <c>double</c>, the total time, in
            /// seconds, for all redirection steps include name lookup, connect,
            /// pretransfer and transfer before final transaction was started.
            /// <c>CURLINFO_REDIRECT_TIME</c> contains the complete execution
            /// time for multiple redirections. (Added in 7.9.7) 
            /// </summary>
            CURLINFO_REDIRECT_TIME = 0x300013,
            /// <summary>
            /// The second argument receives an <c>int</c> containing the total size
            /// of the issued requests. This is so far only for HTTP requests. Note
            /// that this may be more than one request if
            /// <see cref="CURLoption.CURLOPT_FOLLOWLOCATION"/> is <c>true</c>.
            /// </summary>
            CURLINFO_REQUEST_SIZE = 0x20000C,
            /// <summary>
            /// The second argument receives an <c>int</c> with the last received HTTP
            /// or FTP code. This option was known as <c>CURLINFO_HTTP_CODE</c> in
            /// libcurl 7.10.7 and earlier. 
            /// </summary>
            CURLINFO_RESPONSE_CODE = 0x200002,
            /// <summary>
            /// The second argument receives a <c>double</c> with the total amount of
            /// bytes that were downloaded. The amount is only for the latest transfer
            /// and will be reset again for each new transfer. 
            /// </summary>
            CURLINFO_SIZE_DOWNLOAD = 0x300008,
            /// <summary>
            /// The second argument receives a <c>double</c> with the total amount
            /// of bytes that were uploaded. 
            /// </summary>
            CURLINFO_SIZE_UPLOAD = 0x300007,
            /// <summary>
            /// The second argument receives a <c>double</c> with the average
            /// download speed that cURL measured for the complete download. 
            /// </summary>
            CURLINFO_SPEED_DOWNLOAD = 0x300009,
            /// <summary>
            /// The second argument receives a <c>double</c> with the average
            /// upload speed that libcurl measured for the complete upload. 
            /// </summary>
            CURLINFO_SPEED_UPLOAD = 0x30000A,
            /// <summary>
            /// The second argument receives an <see cref="Slist"/> containing
            /// the names of the available SSL engines.
            /// </summary>
            CURLINFO_SSL_ENGINES = 0x40001B,
            /// <summary>
            /// The second argument receives an <c>int</c> with the result of
            /// the certificate verification that was requested (using the
            /// <see cref="CURLoption.CURLOPT_SSL_VERIFYPEER"/> option in
            /// <see cref="Easy.SetOpt"/>. 
            /// </summary>
            CURLINFO_SSL_VERIFYRESULT = 0x20000D,
            /// <summary>
            /// The second argument receives a <c>double</c> specifying the time,
            /// in seconds, from the start until the first byte is just about to be
            /// transferred. This includes <c>CURLINFO_PRETRANSFER_TIME</c> and
            /// also the time the server needs to calculate the result. 
            /// </summary>
            CURLINFO_STARTTRANSFER_TIME = 0x300011,
            /// <summary>
            /// The second argument receives a <c>double</c> indicating the total transaction
            /// time in seconds for the previous transfer. This time does not include
            /// the connect time, so if you want the complete operation time,
            /// you should add the <c>CURLINFO_CONNECT_TIME</c>. 
            /// </summary>
            CURLINFO_TOTAL_TIME = 0x300003,

            CURLINFO_COOKIELIST = 0x400000 + 28,

        };

        /// <summary>
        /// A member of this enumeration is passed as the first parameter to the
        /// <see cref="Easy.DebugFunction"/> delegate to which libcurl passes
        /// debug messages.
        /// </summary>
        public enum CURLINFOTYPE
        {
            /// <summary>
            /// The data is informational text.
            /// </summary>
            CURLINFO_TEXT = 0,
            /// <summary>
            /// The data is header (or header-like) data received from the peer.
            /// </summary>
            CURLINFO_HEADER_IN = 1,
            /// <summary>
            /// The data is header (or header-like) data sent to the peer.
            /// </summary>
            CURLINFO_HEADER_OUT = 2,
            /// <summary>
            /// The data is protocol data received from the peer.
            /// </summary>
            CURLINFO_DATA_IN = 3,
            /// <summary>
            /// The data is protocol data sent to the peer.
            /// </summary>
            CURLINFO_DATA_OUT = 4,
            /// <summary>
            /// The data is SSL-related data sent to the peer.
            /// </summary>
            CURLINFO_SSL_DATA_IN = 5,
            /// <summary>
            /// The data is SSL-related data received from the peer.
            /// </summary>
            CURLINFO_SSL_DATA_OUT = 6,
            /// <summary>
            /// End of enumeration marker, don't use in a client application.
            /// </summary>
            CURLINFO_END = 7
        };

        // DllImport //
        #region     
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern int SetStdHandle(int device, IntPtr handle); 

        [DllImport(libcurl_dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void curl_global_init(int parameters);

        [DllImport(libcurl_dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void curl_global_init(CURLinitFlag parameters);

        [DllImport(libcurl_dll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr curl_slist_append(IntPtr slist, string data);

        [DllImport(libcurl_dll, CallingConvention = CallingConvention.Cdecl)]
        private static extern CURLSHcode curl_slist_free_all(IntPtr pList);

        [DllImport(libcurl_dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr curl_easy_init();

        [DllImport(libcurl_dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern  void curl_easy_cleanup(IntPtr CURL);

        [DllImport(libcurl_dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void curl_global_cleanup();

        [DllImport(libcurl_dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern CURLCode curl_easy_setopt(IntPtr CURL, CURLOPT option, IntPtr value);

        [DllImport(libcurl_dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern CURLCode curl_easy_setopt(IntPtr CURL, CURLOPT option, CallbackDelegate value);        

        [DllImport(libcurl_dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern CURLCode curl_easy_setopt(IntPtr CURL, CURLOPT option, bool value);

        [DllImport(libcurl_dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern CURLCode curl_easy_setopt(IntPtr CURL, CURLOPT option, string value);

        [DllImport(libcurl_dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern CURLCode curl_easy_setopt(IntPtr CURL, CURLOPT option, int value);

        [DllImport(libcurl_dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern CURLCode curl_easy_setopt(IntPtr CURL, CURLOPT option, long value);

        [DllImport(libcurl_dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern CURLCode curl_easy_perform(IntPtr CURL);

        [DllImport(libcurl_dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern string curl_easy_strerror(ref CURLCode code);

        [DllImport(libcurl_dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern CURLCode curl_easy_getinfo(IntPtr CURL, int option, ref IntPtr p);

        [DllImport(libcurl_dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern CURLCode curl_easy_getinfo(IntPtr CURL, int option, ref int v);

        [DllImport(libcurl_dll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "curl_easy_getinfo")]
        private static extern CURLCode curl_easy_getinfo_64(IntPtr pCurl, CURLINFO info, ref double dblVal);

        [DllImport(libcurl_dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern CURLCode curl_easy_getinfo(IntPtr CURL, CURLINFO option, ref IntPtr p);

        [DllImport(libcurl_dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern CURLCode curl_easy_getinfo(IntPtr CURL, CURLINFO option, ref int v);

        [DllImport(libcurl_dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void curl_easy_reset(IntPtr CURL);

        [DllImport(libcurl_dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern string curl_version();
        #endregion

        #region Object pinning
        /// <summary>
        /// Free the pinned object
        /// </summary>
        /// <param name="handle"></param>
        void FreeHandle(ref IntPtr handle)
        {
            if (handle == IntPtr.Zero)
                return;
            GCHandle handleCallback = GCHandle.FromIntPtr(handle);
            handleCallback.Free();
            handle = IntPtr.Zero;
        }
        
        /// <summary>
        /// Pin the object in memory so the C function can find it
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IntPtr GetHandle(object obj)
        {
            if (obj == null)
                return IntPtr.Zero;
            GCHandle fPtr = GCHandle.Alloc(obj, GCHandleType.Normal);
            return GCHandle.ToIntPtr(fPtr);
        }
        
        /// <summary>
        /// Returns the object passed to a Set...Data function.
        /// Cast back to the original object.
        /// </summary>
        /// <param name="userdata"></param>
        /// <returns></returns>
        public static object GetObject(IntPtr userdata)
        {
            if (userdata == IntPtr.Zero)
                return null;
            GCHandle handle = GCHandle.FromIntPtr(userdata);
            return handle.Target;
        }
        #endregion

        private IntPtr CURL = IntPtr.Zero;
        private PerformResponse _resp;
        private EasyOptions _opts;

        /// <summary>
        ///     Pointer to CURL object of libcurl.dll
        /// </summary>
        public IntPtr hCURL { get { return CURL; } }

        /// <summary>
        ///     Response Results
        /// </summary>
        public PerformResponse Response { get { return _resp; } }

        /// <summary>
        ///     Options
        /// </summary>
        public EasyOptions Options { get { return _opts; } }

        /// <summary>
        ///     libcurl version
        /// </summary>
        public static string Version { get { return curl_version(); } }

        /// <summary>
        ///     Get Error Description From Code
        /// </summary>
        /// <param name="errCode"></param>
        /// <returns></returns>
        public static string StrError(CURLCode errCode) { return curl_easy_strerror(ref errCode); }

        /// <summary>
        ///     Create CURL object
        /// </summary>
        public HttpCUrlLibRequest()
        {
            curl_global_init(CURLinitFlag.CURL_GLOBAL_ALL);
            if ((CURL = curl_easy_init()) == IntPtr.Zero) throw new Exception("Couldn't create CURL object");
            this._resp = PerformResponse.FromLibRequest(this);
            this._opts = EasyOptions.FromLibRequest(this);
        }

        public CURLCode ProcessRequest()
        {
            return curl_easy_perform(CURL);
        }

        public CURLCode Easy_Perform()
        {
            return curl_easy_perform(CURL);
        }


        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        /// <summary>
        ///     Test
        /// </summary>
        public void Test()
        {
            this.Options.INSECURE = true;
            this.Options.HEADER_IN_BODY_OUTPUT = true;
            this.Options.Debug = true;
            this.Options.Url = "http://192.168.33.9/";

            // write to mem
            curl_easy_setopt(CURL, CURLOPT.CURLOPT_WRITEFUNCTION, _cOnWriteCallback); // ---- DOESN't WORK
            // write to std
            //curl_easy_setopt(CURL, CURLOPT.CURLOPT_WRITEFUNCTION, IntPtr.Zero);
            // File*
            //curl_easy_setopt(CURL, CURLOPT.CURLOPT_WRITEDATA, IntPtr.Zero);

            CURLCode res = this.ProcessRequest();
            int code = this.Response.ReturnCode;            
        }

        private delegate Int32 CallbackDelegate(IntPtr ptr, Int32 sz, Int32 nmemb, ref IntPtr userdata);
        Int32 _cOnWriteCallback(IntPtr ptr, Int32 size, Int32 memblock, ref IntPtr userdata)
        {
            Int32 realSize = size * memblock;
            //
            byte[] arr = new byte[realSize];
            Marshal.Copy(ptr, arr, 0, (int)realSize);
            string data = System.Text.Encoding.ASCII.GetString(arr);
            //
            return realSize;
        }

        /// <summary>
        ///     Clear Memory
        /// </summary>
        public void Free()
        {
            if (CURL == IntPtr.Zero) return;
            curl_easy_cleanup(CURL);
            curl_global_cleanup();
            CURL = IntPtr.Zero;
        }

        ~HttpCUrlLibRequest()
        {
            Free();
        }
    }

    /// <summary>
    ///     WebRequest Using libcyrl.dll
    /// </summary>
    public class HttpCUrlNetRequest
    {
        /// <summary>
        ///     Easy Options
        /// </summary>
        public class EasyOptions
        {
            private HttpCUrlNetRequest parent;
            private Easy CURL { get { return parent.easy; } }
            private CURLcode _last = (CURLcode)0;
            private bool _debug = false;
            private EasyOptions() { }

            internal static EasyOptions FromNetRequest(HttpCUrlNetRequest owner)
            {
                EasyOptions res = new EasyOptions();
                res.parent = owner;
                return res;
            }

            /// <summary>
            ///     Last Code Returned by curl_easy_setopt
            /// </summary>
            public CURLcode LastCode { get { return _last; } }

            /// <summary>
            ///     Reset Options to default
            /// </summary>
            public void Reset()
            {
                CURL.Reset();
            }

            /// <summary>
            ///     Reset Options to default
            /// </summary>
            public void Null()
            {
                CURL.Reset();
            }

            /// <summary>
            ///     Reset Options to default
            /// </summary>
            public void SetToDefault()
            {
                CURL.Reset();
            }

            /// <summary>
            ///     Set Options
            /// </summary>
            /// <param name="opt"></param>
            /// <param name="val"></param>
            /// <returns></returns>
            public CURLcode Set(CURLoption opt, object val)
            {
                return _last = CURL.SetOpt(opt, val);
            }

            /// <summary>
            ///     Set debug options
            /// </summary>
            /// <param name="val"></param>
            public bool Debug
            {
                set
                {
                    if (value)
                    {
                        _debug = true;
                        _last = Set(CURLoption.CURLOPT_NOPROGRESS, false);
                        _last = Set(CURLoption.CURLOPT_FAILONERROR, false);
                        _last = Set(CURLoption.CURLOPT_VERBOSE, true);
                        _last = Set(CURLoption.CURLOPT_HEADER, true);
                    }
                    else
                    {
                        _debug = false;
                        _last = Set(CURLoption.CURLOPT_NOPROGRESS, true);
                        _last = Set(CURLoption.CURLOPT_FAILONERROR, true);
                        _last = Set(CURLoption.CURLOPT_VERBOSE, false);
                    };
                }
                get
                {
                    return _debug;
                }
            }

            /// <summary>
            ///     Set HTTPs verification
            /// </summary>
            public bool SSLVerify
            {
                set
                {
                    if (value)
                    {
                        _last = Set(CURLoption.CURLOPT_SSL_VERIFYHOST, 2);
                        _last = Set(CURLoption.CURLOPT_SSL_VERIFYPEER, true);
                    }
                    else
                    {
                        _last = Set(CURLoption.CURLOPT_SSL_VERIFYHOST, 0);
                        _last = Set(CURLoption.CURLOPT_SSL_VERIFYPEER, false);
                    };
                }
            }

            public bool FOLLOWLOCATION { set { _last = Set(CURLoption.CURLOPT_FOLLOWLOCATION, value); } }
            public bool FAILONERROR { set { _last = Set(CURLoption.CURLOPT_FAILONERROR, value); } }
            public bool VERBOSE { set { _last = Set(CURLoption.CURLOPT_VERBOSE, value); } }
            public bool IGNORE_CONTENT_LENGTH { set { _last = Set((CURLoption)136, value); } }
            public bool NOPROGRESS { set { _last = Set(CURLoption.CURLOPT_NOPROGRESS, value); } }
            public bool SSL_VERIFYPEER { set { _last = Set(CURLoption.CURLOPT_SSL_VERIFYPEER, value); } }
            public bool INSECURE { set { _last = Set(CURLoption.CURLOPT_SSL_VERIFYPEER, !value); } }
            public string REFERER { set { _last = Set(CURLoption.CURLOPT_REFERER, value); } }
            public string USERAGENT { set { _last = Set(CURLoption.CURLOPT_USERAGENT, value); } }
            public int CONNECT_TIMEOUT { set { _last = Set(CURLoption.CURLOPT_CONNECTTIMEOUT, value); } }
            public int TIMEOUT { set { Set(CURLoption.CURLOPT_FTP_RESPONSE_TIMEOUT, value); _last = Set(CURLoption.CURLOPT_TIMEOUT, value); } }
            public string PROXY_USERPWD { set { _last = Set(CURLoption.CURLOPT_PROXYUSERPWD, value); } }
            public string USERPWD { set { _last = Set(CURLoption.CURLOPT_USERPWD, value); } }
            public bool HEADER_IN_BODY_OUTPUT { set { _last = Set(CURLoption.CURLOPT_HEADER, value); } }
            public bool NO_PROGRESS { set { _last = Set(CURLoption.CURLOPT_NOPROGRESS, value); } }
            public bool HTTP_POST { set { _last = Set(CURLoption.CURLOPT_POST, value); } }
            public bool HTTP_PUT { set { _last = Set(CURLoption.CURLOPT_PUT, value); } }
            public bool HTTP_GET { set { _last = Set(CURLoption.CURLOPT_HTTPGET, value); } }
            public string COOKIE_UPLOAD_FILE { set { _last = Set(CURLoption.CURLOPT_COOKIEFILE, value); } }
            public string COOKIE_DNLOAD_FILE { set { _last = Set(CURLoption.CURLOPT_COOKIEJAR, value); } }
            public string CUSTOMHTTPREQUEST { set { _last = Set(CURLoption.CURLOPT_CUSTOMREQUEST, value); } }
            public int UPLOADSIZE { set { _last = Set(CURLoption.CURLOPT_INFILESIZE, value); } }
            public int POSTDATA { set { Set(CURLoption.CURLOPT_HTTPPOST, true); _last = Set(CURLoption.CURLOPT_POSTFIELDS, value); } }
            public string HTTP_HEADER { set { _last = Set(CURLoption.CURLOPT_HTTPHEADER, value); } }
            public string HTTP_PROXY { set { Set(CURLoption.CURLOPT_PROXYTYPE, CURLproxyType.CURLPROXY_HTTP); _last = Set(CURLoption.CURLOPT_PROXY, value); } }            

            /// <summary>
            ///     Set URL
            /// </summary>
            public string Url
            {
                set
                {
                    _last = Set(CURLoption.CURLOPT_URL, value);
                }
                get
                {
                    string res = "";
                    CURL.GetInfo(CURLINFO.CURLINFO_EFFECTIVE_URL, ref res);
                    return res;
                }
            }
        }

        /// <summary>
        ///     Response Information
        /// </summary>
        public class PerformResponse
        {
            private HttpCUrlNetRequest parent;
            private Easy CURL { get { return parent.easy; } }
            private CURLcode _last = CURLcode.CURLE_SEND_ERROR;
            private CURLcode _reqC = CURLcode.CURLE_SEND_ERROR;
            private string _charset = "";
            private Dictionary<string, string> _headers = null;
            private System.Text.Encoding _encoding = System.Text.Encoding.ASCII;
            internal void SetLast(CURLcode code) { _last = code; }
            internal void SetReqC(CURLcode code) { _reqC = code; }
            private PerformResponse() { }

            internal static PerformResponse FromNetRequest(HttpCUrlNetRequest owner, CURLcode requestResult)
            {
                PerformResponse res = new PerformResponse();
                res.parent = owner;
                res._last = res._reqC = requestResult;
                return res;
            }

            private void ParseHeaders()
            {
                if (_headers == null)
                    _headers = new Dictionary<string, string>(); // not yet parsed
                else
                    return; // already parsed

                if(parent._recvHBuff.Count == 0) return; // no headers data

                Regex rx = new Regex(@"([\w-]+): (.*)",RegexOptions.IgnoreCase);
                MatchCollection mc = rx.Matches(parent._recvHdr);                
                foreach (Match mx in mc)
                {
                    string val = mx.Groups[2].Value.Trim();
                    if (!_headers.ContainsKey(mx.Groups[1].Value))
                        _headers.Add(mx.Groups[1].Value, val);
                    else
                        _headers[mx.Groups[1].Value] += val;
                    if (String.Compare(mx.Groups[1].Value, "content-type", true) == 0)
                    {
                        string vlow = val.ToLower();
                        int ilow = vlow.IndexOf("charset=");
                        if (ilow >= 0) _charset = val.Substring(ilow + 8).Trim();
                        if (!String.IsNullOrEmpty(_charset)) { try { _encoding = System.Text.Encoding.GetEncoding(_charset); } catch { }; };
                    };
                };
            }

            public CURLcode Last { get { return _last; } }
            public CURLcode Result { get { return _reqC; } }
            public string Charset { get { return _charset; } }
            public IDictionary<string, string> Headers { get { if (this._headers == null) this.ParseHeaders(); return _headers; } }
            public System.Text.Encoding ContentEncoding { get { if (this._headers == null) this.ParseHeaders(); return _encoding; } }

            /// <summary>
            ///     Last Used URL
            /// </summary>
            /// <returns></returns>
            public string Url
            {
                get
                {
                    string url = null;
                    CURLcode code = _last = CURL.GetInfo(CURLINFO.CURLINFO_EFFECTIVE_URL, ref url);
                    return url;
                }
            }

            public string ContentType
            {
                get
                {
                    string ct = null;
                    CURLcode code = _last = CURL.GetInfo(CURLINFO.CURLINFO_CONTENT_TYPE, ref ct);
                    return ct;
                }
            }

            public double ConnectTime
            {
                get
                {
                    double d = 0;
                    CURLcode code = _last = CURL.GetInfo(CURLINFO.CURLINFO_CONNECT_TIME, ref d);
                    return d;
                }
            }

            public double RedirectTime
            {
                get
                {
                    double d = 0;
                    CURLcode code = _last = CURL.GetInfo(CURLINFO.CURLINFO_REDIRECT_TIME, ref d);
                    return d;
                }
            }

            public double UploadedContentLength
            {
                get
                {
                    double d = 0;
                    CURLcode code = _last = CURL.GetInfo(CURLINFO.CURLINFO_CONTENT_LENGTH_UPLOAD, ref d);
                    return d;
                }
            }

            public double DownloadedContentLength
            {
                get
                {
                    double d = 0;
                    CURLcode code = _last = CURL.GetInfo(CURLINFO.CURLINFO_CONTENT_LENGTH_DOWNLOAD, ref d);
                    return d;
                }
            }

            public int DownloadedHeaderSize
            {
                get
                {
                    int i = 0;
                    CURLcode code = CURL.GetInfo(CURLINFO.CURLINFO_HEADER_SIZE, ref i);
                    return i;
                }
            }

            public int NumberOfConnects
            {
                get
                {
                    int i = 0;
                    CURLcode code = CURL.GetInfo(CURLINFO.CURLINFO_NUM_CONNECTS, ref i);
                    return i;
                }
            }

            public int NumberOfRedirects
            {
                get
                {
                    int i = 0;
                    CURLcode code = CURL.GetInfo(CURLINFO.CURLINFO_REDIRECT_COUNT, ref i);
                    return i;
                }
            }

            public int RequestSize
            {
                get
                {
                    int i = 0;
                    CURLcode code = CURL.GetInfo(CURLINFO.CURLINFO_REQUEST_SIZE, ref i);
                    return i;
                }
            }

            public int FileTime
            {
                get
                {
                    int i = 0;
                    CURLcode code = CURL.GetInfo(CURLINFO.CURLINFO_FILETIME, ref i);
                    return i;
                }
            }

            public double TotalTime
            {
                get
                {
                    double d = 0;
                    CURLcode code = CURL.GetInfo(CURLINFO.CURLINFO_TOTAL_TIME, ref d);
                    return d;
                }
            }

            public double NameLookupTime
            {
                get
                {
                    double d = 0;
                    CURLcode code = CURL.GetInfo(CURLINFO.CURLINFO_NAMELOOKUP_TIME, ref d);
                    return d;
                }
            }

            public int OSError
            {
                get
                {
                    int i = 0;
                    CURLcode code = CURL.GetInfo(CURLINFO.CURLINFO_OS_ERRNO, ref i);
                    return i;
                }
            }

            public int ProxyError
            {
                get
                {
                    int i = 0;
                    CURLcode code = CURL.GetInfo((CURLINFO)0x200000 + 59 /*CURLINFO.CURLINFO_PROXY_ERROR*/, ref i);
                    return i;
                }
            }

            public string RemoteIP
            {
                get
                {
                    string url = null;
                    CURLcode code = _last = CURL.GetInfo((CURLINFO)0x100000+32 /*CURLINFO.CURLINFO_PRIMARY_IP*/, ref url);
                    return url;
                }
            }

            public int RemotePort
            {
                get
                {
                    int i = 0;
                    CURLcode code = CURL.GetInfo((CURLINFO)0x200000+40 /*CURLINFO.CURLINFO_PRIMARY_PORT*/, ref i);
                    return i;
                }
            }

            public string RedirectUrl
            {
                get
                {
                    string url = null;
                    CURLcode code = _last = CURL.GetInfo((CURLINFO)0x100000+31 /*CURLINFO.CURLINFO_REDIRECT_URL*/, ref url);
                    return url;
                }
            }

            public string Referer
            {
                get
                {
                    string url = null;
                    CURLcode code = _last = CURL.GetInfo((CURLINFO)0x100000 + 60 /*CURLINFO.CURLINFO_REFERER*/, ref url);
                    return url;
                }
            }

            /// <summary>
            ///     HTTP Code
            /// </summary>
            public int ReturnCode
            {
                get
                {
                    int i = 0;
                    CURLcode code = CURL.GetInfo(CURLINFO.CURLINFO_RESPONSE_CODE, ref i);
                    return i;
                }
            }

            public double DownloadedLength
            {
                get
                {
                    double d = 0;
                    CURLcode code = _last = CURL.GetInfo(CURLINFO.CURLINFO_SIZE_DOWNLOAD, ref d);
                    return d;
                }
            }

            public double UploadedLength
            {
                get
                {
                    double d = 0;
                    CURLcode code = _last = CURL.GetInfo(CURLINFO.CURLINFO_SIZE_UPLOAD, ref d);
                    return d;
                }
            }

            public double DownloadSpeed
            {
                get
                {
                    double d = 0;
                    CURLcode code = _last = CURL.GetInfo(CURLINFO.CURLINFO_SPEED_DOWNLOAD, ref d);
                    return d;
                }
            }

            public double UploadSpeed
            {
                get
                {
                    double d = 0;
                    CURLcode code = _last = CURL.GetInfo(CURLINFO.CURLINFO_SPEED_UPLOAD, ref d);
                    return d;
                }
            }

            /// <summary>
            ///     Response Header Text
            /// </summary>
            public string HeaderText { get { return parent._recvHdr; } }

            /// <summary>
            ///     Response Header bytes
            /// </summary>
            public byte[] HeaderData { get { return parent._recvHBuff.ToArray(); } }

            /// <summary>
            ///     Response Text
            /// </summary>
            public string Text { get { return parent._recvStr; } }

            /// <summary>
            ///     Response bytes
            /// </summary>
            public byte[] Data { get { return parent._recvBuff.ToArray(); } }

            /// <summary>
            ///     Response Body Text
            /// </summary>
            public string BodyText 
            { 
                get 
                {
                    if (String.IsNullOrEmpty(parent._recvStr)) return parent._recvStr;
                    if (parent._recvStr.StartsWith("HTTP/"))
                        return parent._recvStr.Substring(DownloadedHeaderSize);
                    else
                        return parent._recvStr; 
                } 
            }
            
            /// <summary>
            ///     Response Body bytes
            /// </summary>
            public byte[] BodyData
            {
                get
                {
                    if (parent._recvBuff.Count == 0) return parent._recvBuff.ToArray();
                    if (parent._recvStr.StartsWith("HTTP/"))
                    {
                        byte[] res = new byte[parent._recvBuff.Count - DownloadedHeaderSize];
                        Array.Copy(parent._recvBuff.ToArray(), DownloadedHeaderSize, res, 0, res.Length);
                        return res;
                    }
                    else
                        return parent._recvBuff.ToArray();
                }
            }

            /// <summary>
            ///     Search array in another array
            /// </summary>
            /// <param name="where2search"></param>
            /// <param name="what2search"></param>
            /// <returns>index of an array</returns>
            private static int SearchBytes(byte[] where2search, byte[] what2search)
            {
                int len = what2search.Length;
                int limit = where2search.Length - len;
                for (int i = 0; i <= limit; i++)
                {
                    int k = 0;
                    for (; k < len; k++)
                    {
                        if (what2search[k] != where2search[i + k]) break;
                    }
                    if (k == len) return i;
                }
                return -1;
            }
        }

        private bool _debug = false;
        private Easy easy;

        private string _recvHdr = "";
        private List<byte> _recvHBuff = new List<byte>();
        private string _recvStr = "";
        private List<byte> _recvBuff = new List<byte>();
        private EasyOptions _opts;
        
        /// <summary>
        ///     libcurl.dll version
        /// </summary>
        public string Version { get { return Curl.Version; } }

        /// <summary>
        ///     CURL *curl object
        /// </summary>
        public Easy cUrlEasy { get { return easy; } }
        
        /// <summary>
        ///     Options to Set
        /// </summary>
        public EasyOptions Options { get { return _opts; } }

        /// <summary>
        ///     Debug to Console
        /// </summary>
        public bool Debug { get { return _debug; } set { _debug = value; } }
        
        /// <summary>
        ///     Global Init
        /// </summary>
        static HttpCUrlNetRequest() { Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL); }

        /// <summary>
        ///     Global CleanUp
        /// </summary>
        public static void Cleenup() { Curl.GlobalCleanup(); }

        /// <summary>
        ///     Cleanup Easy Object
        /// </summary>
        public void Free() { this.CleanUp(); }

        /// <summary>
        ///     Cleanup Easy Object
        /// </summary>
        public void CleanUp() { easy.Cleanup(); _recvBuff.Clear(); _recvHBuff.Clear(); _recvHdr = ""; _recvStr = ""; }
                
        /// <summary>
        ///     Create HttpCUrlNetRequest Object
        /// </summary>
        public HttpCUrlNetRequest()
        {
            easy = new Easy();
            _opts = EasyOptions.FromNetRequest(this);
            easy.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION, new Easy.WriteFunction(OnWriteData));    // get response
            easy.SetOpt(CURLoption.CURLOPT_HEADERFUNCTION, new Easy.HeaderFunction(OnHeaderData)); // get response header
            easy.SetOpt(CURLoption.CURLOPT_DEBUGFUNCTION, new Easy.DebugFunction(OnDebugData));    // ger debug info
        }        

        /// <summary>
        ///     Url Escape
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SafeString(string data) { return Curl.Escape(data, data.Length); }

        /// <summary>
        ///     Url Unescape
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string UnSafeString(string data) { return Curl.Unescape(data, data.Length); }

        /// <summary>
        ///     receiving response data
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="size"></param>
        /// <param name="nmemb"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        private Int32 OnWriteData(Byte[] buf, Int32 size, Int32 nmemb, Object userData)
        {
            string incbuff = System.Text.Encoding.UTF8.GetString(buf);
            _recvStr = _recvStr + incbuff;
            _recvBuff.AddRange(buf);
            return size * nmemb;
        }

        /// <summary>
        ///     receiving response header
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="size"></param>
        /// <param name="nmemb"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        private Int32 OnHeaderData(Byte[] buf, Int32 size, Int32 nmemb, Object userData)
        {
            string incbuff = System.Text.Encoding.UTF8.GetString(buf);
            _recvHdr = _recvHdr + incbuff;
            _recvHBuff.AddRange(buf);
            return size * nmemb;
        }

        /// <summary>
        ///     receiving debug data
        /// </summary>
        /// <param name="infoType"></param>
        /// <param name="message"></param>
        /// <param name="extraData"></param>
        private void OnDebugData(CURLINFOTYPE infoType, string message, object extraData)
        {
            if (this._debug)
                Console.WriteLine(">>" + infoType.ToString() + ": " + message + (extraData == null ? "" : " -- " + extraData.ToString()));
        }

        /// <summary>
        ///     Get Error Description From Code
        /// </summary>
        /// <param name="errCode"></param>
        /// <returns></returns>
        public string StrError(CURLcode errCode) { return easy.StrError(errCode); }        

        /// <summary>
        ///     Get Response
        /// </summary>
        /// <returns></returns>
        public PerformResponse GetResponse()
        {
            return ProcessRequest();
        }

        /// <summary>
        ///     Get Response
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public PerformResponse GetResponse(string Url)
        {
            this.Options.Url = Url;
            return ProcessRequest();
        }

        /// <summary>
        ///     Get Response
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public PerformResponse ProcessRequest(string Url)
        {
            this.Options.Url = Url;
            return ProcessRequest();
        }

        /// <summary>
        ///     Get Response
        /// </summary>
        /// <returns></returns>
        public PerformResponse ProcessRequest()
        {
            _recvHdr = ""; // Clear Response Header
            _recvStr = ""; // Clear Response Text
            _recvBuff.Clear(); // Clear Response Buffer
            _recvHBuff.Clear(); // Clear Response Header Buffer
            CURLcode _res = easy.Perform(); // Perform Easy
            PerformResponse _resp = PerformResponse.FromNetRequest(this, _res); // Get Response
            return _resp;
        }

        /// <summary>
        ///     Example of usage // Test
        /// </summary>
        public void Test()
        {            
            this.Options.TIMEOUT = 60;
            this.Options.INSECURE = true;
            this.Options.FOLLOWLOCATION = true;            
            this.Options.Debug = true;
            // this.Options.HEADER_IN_BODY_OUTPUT = false;
            
            string http_header = "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            http_header = http_header + "User-Agent: Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36";
            http_header = http_header + "Accept-Encoding: gzip,deflate,sdch";
            http_header = http_header + "Accept-Language: ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
            this.Options.HTTP_HEADER = http_header;
            
            PerformResponse resp = this.ProcessRequest("http://192.168.33.9/");
            if(resp.Result == CURLcode.CURLE_OK)
                Console.WriteLine(resp.Text);
            else
                Console.WriteLine(resp.Result.ToString());
        }
    }
}

