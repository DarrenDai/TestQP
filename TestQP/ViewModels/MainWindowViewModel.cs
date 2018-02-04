using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TestQP.Constants;
using TestQP.Sockets;

namespace TestQP
{
    public class MainWindowViewModel : NotifyObject
    {
        #region Private fields

        private string _serverAddress = ConfigurationManager.AppSettings["ServerAddress"];
        private int _serverPort = int.Parse(ConfigurationManager.AppSettings["ServerPort"]);

        private TcpClient _client = null;

        #endregion

        #region Constructor

        public MainWindowViewModel()
        {
            Dowork();
        }

        #endregion

        #region Pulic properties

        private string _output;

        public string Output
        {
            get { return _output; }
            set
            {
                _output = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Private methods

        private void Dowork()
        {
            LogInfo("Begin to work...");
            Task.Factory.StartNew(() =>
            {
                try
                {
                    while (true)
                    {
                        _client = new TcpClient(_serverAddress, _serverPort);
                        LogInfo("Connected!");

                        SentMessage(_client, GetLoginMessage());

                        NetworkStream stream = _client.GetStream();
                        Byte[] buffer = new Byte[1024];
                        string data = string.Empty;
                        int count = 0;

                        //// Loop to receive all the data sent by the server.
                        while ((count = stream.Read(buffer, 0, buffer.Length)) != 0)
                        {

                            // Translate data bytes to a ASCII string.
                            data = BitConverter.ToString(buffer, 0, count).Replace("-", " ");
                            //data = System.Text.Encoding.ASCII.GetString(byteBuffer, 0, i);
                            LogInfo(string.Format("Received: {0}", data));
                            // byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                        }

                        stream.Close();
                        _client.Close();

                        LogInfo("Client closed!");
                    }
                }
                catch (Exception ex)
                {
                    LogInfo(ex.ToString());
                }
                finally
                {
                }
            });
        }

        private byte[] GetLoginMessage()
        {
            var message = new Message();
            message.MessageBody = new MessageBody();
            message.Header = new MessageHeader()
            {
                MessageId = (UInt16)FunctionEnum.CLIENT_LOGON,
                MessageProperty = 0x0025,
                ProtocolVersion = 0x01,
                Token = 0x00,
                StationId = 0x60000001,
                SequenceNO = 0x0004
            };

            var bytes = message.GetMessageBytes();
            return bytes;
            //return new byte[] { 0x8E, 0x06, 0x02, 0x00, 0x25, 0x01, 0x00, 0x60, 0x00, 0x00, 0x01, 0x00, 0x04, 0x46, 0x72, 0x65, 0x65, 0x52, 0x54, 0x4F, 0x53, 0x20, 0x56, 0x38, 0x2E, 0x32, 0x2E, 0x33, 0x00, 0x00, 0x00, 0x32, 0x2E, 0x30, 0x2E, 0x30, 0x37, 0x78, 0x8E };
        }

        private void SentMessage(TcpClient client, byte[] msg)
        {
            var stream = client.GetStream();
            stream.Write(msg, 0, msg.Length);
            string data = BitConverter.ToString(msg, 0).Replace("-", " ");
            LogInfo(string.Format("Sent: {0}", data));
        }

        private void Connect(String server, int port, String message)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }

        private void LogInfo(string text)
        {
            Output += string.Format("{0} {1} \r\n", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), text);
        }

        #endregion
    }
}
