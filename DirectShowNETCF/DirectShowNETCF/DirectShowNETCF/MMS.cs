/*
 * In Progress
 */
using System;
using System.IO;
using System.Threading;
using System.Net.Sockets;

namespace DirectShowNETCF.MMS
{
    public class MMSDowloader : IDisposable
    {
        private string file_ = null;
        private string url_ = null;
        private int streams_ = 0;
        private Int64 length_ = 0;
        private int padding_ = 0;
        private Thread receiver_ = null;
        private MMSWriter writer_ = null;
        private bool started_ = false;

        #region constants
        const string initial_1 = " HTTP/1.0\r\nAccept: */*\r\nUser-Agent: NSPlayer/4.1.0.3856\r\nHost: ";
        const string initial_2 = "Pragma: no-cache,rate=1.000000,stream-time=0,stream-offset=0:0,request-context=2,max-duration=0\r\nPragma: xClientGUID={c77e7400-738a-11d2-9add-0020af0a3278}\r\nPragma: xPlayStrm=1\r\n";
        const string initial_3 = "Pragma: no-cache,rate=1.000000,stream-time=0,stream-offset=0:0,request-context=1,max-duration=0\r\nPragma: xClientGUID={c77e7400-738a-11d2-9add-0020af0a3278}\r\nConnection: Keep-Alive\r\n\r\n";
        #endregion

        #region Public declarations
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AFileName">Full path where file should be saved</param>
        /// <param name="AUrl">url where should we save from</param>
        public MMSDowloader(string AFileName, string AUrl)
        {
            file_ = AFileName;
            url_ = AUrl;
        }

        /// <summary>
        /// start recording stream to file
        /// </summary>
        public void start()
        {
            receiver_ = new Thread(receive);
            receiver_.Priority = ThreadPriority.BelowNormal;
            receiver_.IsBackground = true;
            started_ = true;
            receiver_.Start();
        }

        /// <summary>
        /// stop recording... use it if you want to abort downloading
        /// </summary>
        public void stop()
        {
        }

        /// <summary>
        /// Destructor
        /// </summary>
        public void Dispose()
        {
            stop();
            if (writer_ != null)
            {
                writer_.Dispose();
                writer_ = null;
            }
        }
        #endregion

        #region Private Declarations
        /// <summary>
        /// parses url for host, port and uri
        /// </summary>
        /// <param name="host">host</param>
        /// <param name="port">port</param>
        /// <returns>uri</returns>
        private string parseUrl(out string host, out string port)
        {
            try
            {
                string url = url_;
                port = "80";
                int pos = url.IndexOf("://");
                if (pos > -1)
                {
                    url = url.Substring(pos + 3);
                }
                pos = url.IndexOf('/');
                if (pos == -1)
                {
                    host = url;
                    pos = url.IndexOf(':');
                    if (pos > -1)
                    {
                        host = url.Substring(0, pos);
                        port = url.Substring(pos + 1);
                    }
                    else
                        port = "80";

                    return "/";
                }
                string res_ = url.Substring(pos + 1, url.Length - pos + 1);
                host = url.Substring(0, pos);
                pos = host.IndexOf(':');
                if (pos == -1)
                {
                    host = url;
                    pos = url.IndexOf(':');
                    if (pos > -1)
                    {
                        host = url.Substring(0, pos);
                        port = url.Substring(pos + 1);
                    }
                    else
                        port = "80";
                }
                return res_;
            }
            catch
            {
                host = null;
                port = null;
                return null;
            }
        }

        /// <summary>
        /// Parcing ASF header searching for file length, padding and streams count
        /// </summary>
        /// <param name="array">array that contain header</param>
        /// <param name="size">header size</param>
        private void getPadding(byte[] array, int size)
        {
            streams_ = 0;
            int i = 30;
            while (i < size)
            {
                Int64 guid_1, guid_2;
                
                guid_2 = array[i] | (array[i + 1] << 8) |
                         (array[i + 2] << 16) | (array[i + 3] << 24) |
                         (array[i + 4] << 32) | (array[i + 5] << 40) |
                         (array[i + 6] << 48) | (array[i + 7] << 56);
                i += 8;

                guid_1 = array[i] | (array[i + 1] << 8) |
                         (array[i + 2] << 16) | (array[i + 3] << 24) |
                         (array[i + 4] << 32) | (array[i + 5] << 40) |
                         (array[i + 6] << 48) | (array[i + 7] << 56);
                i += 8;

                length_ = array[i] | (array[i + 1] << 8) |
                         (array[i + 2] << 16) | (array[i + 3] << 24) |
                         (array[i + 4] << 32) | (array[i + 5] << 40) |
                         (array[i + 6] << 48) | (array[i + 7] << 56);
                i += 8;

                if ((guid_1 == 0x6553200cc000e48e) && (guid_2 == 0x11cfa9478cabdca1))
                {
                    padding_ = ((array[i + 68] | (array[i + 69] << 8)) | (array[i + 70] << 16)) | (array[i + 71] << 24);
                }

                if ((guid_1 == 0x6553200cc000e48e) && (guid_2 == 0x11cfa9b7b7dc0791))
                {
                    streams_++;
                    int id = array[i + 48] | array[i + 49] << 8;
                    if (streams_ < id)
                        streams_ = id;
                }
                i += (int)length_;
                i -= 24;
            }
        }

        /// <summary>
        /// Converts string to byte array that will be sended by socket
        /// </summary>
        /// <param name="toPrepare">string that should be converted</param>
        /// <returns></returns>
        private byte[] prepareString(string toPrepare)
        {
            char[] chs = toPrepare.ToCharArray();
            byte[] bts = new byte[chs.Length];
            for (int i = 0; i < chs.Length; i++)
            {
                bts[i] = (byte)chs[i];
            }
            return bts;
        }

        /// <summary>
        /// main method of thread
        /// </summary>
        private void receive()
        {
            try
            {
                string host, port;
                string uri = parseUrl(out host, out port);
                TcpClient client = new TcpClient();
                client.SendTimeout = 30000;
                client.ReceiveTimeout = 30000;
                client.Connect(host, int.Parse(port));
                if (!client.Client.Connected)
                {
                    return;
                }
                
                string Initial = "GET " + uri + initial_1 + host + ":" + port + "\r\n" + initial_3;

                if (client.Client.Send(prepareString(Initial)) != Initial.Length)
                {
                    return;
                }

                
            }
            catch
            {
            }
        }
        #endregion

    }

    public class MMSWriter : IDisposable
    {
        private FileStream stream_ = null;
        private uint length_ = 0xffffffff;
        public MMSWriter(string AName)
        {
            stream_ = new FileStream(AName, FileMode.Create);
        }

        public void Dispose()
        {
            stream_.Close();
            stream_ = null;
        }

        public int Write(byte[] ABuff, int APadding)
        {
            stream_.Write(ABuff, 0, ABuff.Length);
            if (APadding > ABuff.Length)
            {
                byte[] bt = new byte[APadding - ABuff.Length];
                stream_.Write(bt, 0, bt.Length);
            }
            return 1;
        }
    }
}
