﻿using Prism.Commands;
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
using System.Windows.Input;
using TestQP.Constants;
using TestQP.Sockets;
using TestQP.Sockets.BodyDefinitions;
using TestQP.Utils;

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
            Initialize();
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

        #region Commands

        public ICommand ConnectCommand { get; private set; }

        public ICommand DisConnectCommand { get; private set; }

        public ICommand HeartBeatCommand { get; private set; }


        #endregion

        #region Initialize methods

        private void Initialize()
        {
            InitializeCommands();

            InitializeEvents();
        }

        private void InitializeEvents()
        {
            Provider.EventAggregator.GetEvent<TestQP.Events.Events.LogEvent>().Subscribe(DisplayLogs);
        }

        private void InitializeCommands()
        {
            ConnectCommand = new DelegateCommand<object>(OnConnect);
            DisConnectCommand = new DelegateCommand<object>(OnDisConnect);
            HeartBeatCommand = new DelegateCommand<object>(OnHeartBeat);
        }

        #endregion

        #region Command implements

        public void OnConnect(object payload)
        {
            Dowork();
        }

        public void OnDisConnect(object payload)
        {

        }

        public void OnHeartBeat(object payload)
        {

        }

        #endregion

        #region Private methods

        private void Dowork()
        {
            LogHelper.LogInfo("Begin to work...");
            Task.Factory.StartNew(() =>
            {
                try
                {
                    while (true)
                    {
                        _client = new TcpClient(_serverAddress, _serverPort);
                        LogHelper.LogInfo("Connected!");

                        SendMessage(_client, GetLoginMessage());

                        NetworkStream stream = _client.GetStream();
                        Byte[] buffer = new Byte[1024];
                        string data = string.Empty;
                        int count = 0;

                        //// Loop to receive all the data sent by the server.
                        while ((count = stream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            byte[] msg = new byte[count];
                            Array.Copy(buffer, msg, count);
                            Task.Factory.StartNew(ConsumeMessage, msg);
                        }

                        stream.Close();
                        _client.Close();

                        LogHelper.LogInfo("Client closed!");
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(ex.Message, ex);
                }
                finally
                {
                }
            });
        }

        private byte[] GetLoginMessage()
        {
            var message = new Message();
            message.MessageBody = new ClientLogonBody()
            {
                Password = "123456",
                RuntimeVersion = "Android 5.1.1",
                FrontEndVersion = "1.0.0"
            };
            //var loginBytes = new byte[] { 0x46, 0x72, 0x65, 0x65, 0x52, 0x54, 0x4F, 0x53, 0x20, 0x56, 0x38, 0x2E, 0x32, 0x2E, 0x33, 0x00, 0x00, 0x00, 0x32, 0x2E, 0x30, 0x2E, 0x30, 0x37 };
            //message.MessageBody.FromBytes(loginBytes);
            message.Header = new MessageHeader()
            {
                MessageId = FunctionEnum.CLIENT_LOGON,
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

        private void SendMessage(TcpClient client, byte[] msg)
        {
            var encodedMsg = BytesCoder.Encode(msg);
            var stream = client.GetStream();
            stream.Write(encodedMsg, 0, encodedMsg.Length);
            string data = BitConverter.ToString(encodedMsg).Replace("-", " ");
            LogHelper.LogInfo(string.Format("Sent: {0}", data));
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

        private void DisplayLogs(string text)
        {
            Output += string.Format("{0} {1} \r\n", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), text);
        }

        private void ConsumeMessage(object obj)
        {
            if (!(obj is byte[])) return;
            var msg = obj as byte[];
            // Translate data bytes to a ASCII string.
            var data = BitConverter.ToString(msg).Replace("-", " ");
            //data = System.Text.Encoding.ASCII.GetString(byteBuffer, 0, i);
            LogHelper.LogInfo(string.Format("Received: {0}", data));
            // byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

            List<byte[]> messageList = new List<byte[]>();
            int lastStartIndex = 0;
            for (int i = 0; i < msg.Length; i++)
            {
                if (i == 0 || msg[i] != Constant.FLAG_BYTE) continue;

                if (msg[i] == Constant.FLAG_BYTE)
                {
                    var tempBuff = new byte[i + 1 - lastStartIndex];
                    Array.Copy(msg, lastStartIndex, tempBuff, 0, i + 1 - lastStartIndex);
                    messageList.Add(BytesCoder.Decode(tempBuff));

                    lastStartIndex = ++i;
                }
            }

            LogHelper.LogDebug(string.Format("获取到{0}条消息。分别是：\r\n{1}",
                messageList.Count,
                string.Join("\r\n", messageList.Select(x => string.Format("\t\t\t\t【{0}】", BitConverter.ToString(x))).ToList())));

            AnalysisMessage(messageList);
        }

        private void AnalysisMessage(List<Byte[]> messageList)
        {
            if (messageList == null || messageList.Count == 0)
            {
                return;
            }

            foreach (var item in messageList)
            {
                Message msg = new Message();
                msg.FromBytes(item);
                switch (msg.Header.MessageId)
                {
                    //case FunctionEnum.CLIENT_ANS:
                    //    LogHelper.LogDebug("");
                    //    break;
                    //case FunctionEnum.CLIENT_LOGON:
                    //    break;
                    //case FunctionEnum.CLIENT_HEART_BEAT:
                    //    break;
                    case FunctionEnum.SERVER_ANS:
                        LogHelper.LogDebug("收到服务器通用应答！");
                        break;
                    case FunctionEnum.SERVER_LOGIN_ANS:
                        LogHelper.LogDebug("登陆成功，系统时间为：" + (msg.MessageBody as ServerLogonAnsBody).Time.ToString("yyyy-MM-dd HH:mm:ss"));
                        break;
                    case FunctionEnum.SERVER_REALTIME_DATA:
                        LogHelper.LogDebug("收到服务器实时信息！");
                        break;
                    default:
                        LogHelper.LogDebug("收到服务器信息............");
                        break;
                }
            }
        }

        #endregion
    }
}
