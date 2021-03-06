﻿using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TestQP.Constants;
using TestQP.Constants.Enums;
using TestQP.Models;
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

        private string _currentStationId = ConfigurationManager.AppSettings["StationId"];
        private string _currentStationName;
        private string _output;
        private bool _isRunning;
        private string _winTitle = "模拟站台显示屏";

        private int _clientToken;
        private System.Timers.Timer _heartBeatTimer = new System.Timers.Timer(60 * 1000);

        private TcpClient _client = null;
        private ObservableCollection<BusRouteInfo> _busRoutes = new ObservableCollection<BusRouteInfo>();
        public ObservableCollection<BusRouteInfo> BusRoutes
        {
            get { return _busRoutes; }
        }
        private ResponseObject _data = null;

        private int _clientSquenceNO = 1;

        private CancellationTokenSource _cancelToken = new CancellationTokenSource();

        #endregion

        #region Constructor

        public MainWindowViewModel()
        {
            Initialize();
        }

        #endregion

        #region Pulic properties

        public string CurrentStationId
        {
            get { return _currentStationId; }
            set
            {
                _currentStationId = value;
                OnPropertyChanged(() => CurrentStationId);
            }
        }

        public string CurrentStationName
        {
            get { return _currentStationName; }
            set
            {
                _currentStationName = value;
                OnPropertyChanged(() => CurrentStationName);
            }
        }

        public string Output
        {
            get { return _output; }
            set
            {
                _output = value;
                OnPropertyChanged(() => Output);
            }
        }

        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                OnPropertyChanged(() => IsRunning);
            }
        }

        public string WinTitle
        {
            get { return _winTitle; }
            set { _winTitle = value; OnPropertyChanged(() => WinTitle); }
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

           // InitializeData();

            ConnectCommand.Execute(null);
        }

        private void InitializeEvents()
        {
            Provider.EventAggregator.GetEvent<TestQP.Events.Events.LogEvent>().Subscribe(DisplayLogs);
            _heartBeatTimer.Elapsed += (o, e) => { HeartBeat(); };
        }

        private void InitializeCommands()
        {
            ConnectCommand = new DelegateCommand<object>(OnConnect);
            DisConnectCommand = new DelegateCommand<object>(OnDisConnect);
            HeartBeatCommand = new DelegateCommand<object>(OnHeartBeat);
        }

        private void InitializeData()
        {
            Task.Factory.StartNew(() =>
            {
                _data = new RestfulHelper().GetStationInfo(CurrentStationId);
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    WinTitle = string.Format("模拟站台：【{0}-{1}】", _data.name, _data.identifier);
                    CurrentStationName = _data.name;

                    BusRoutes.Clear();
                }));

                foreach (var item in _data.lines)
                {
                    var routeInfo = new BusRouteInfo()
                    {
                        RouteId = item.buslineId,
                        RouteNo = item.name,
                        StartStation = item.from,
                        EndStation = item.to,
                        StartStationTimeRange = string.Format("{0} - {1}", item.first, item.last),
                        EndStationTimeRange = string.Format("{0} - {1}", item.first, item.last),
                    };

                    // routeInfo.Stations = item.stops.Select(x => new StationPoint() { Name = x.name, Order = x.order }).ToList();

                    var tempList = new List<StationPoint>();
                    routeInfo.Stations = tempList;
                    foreach (var stopItem in item.stops)
                    {
                        if (tempList.Any(x => x.Order == stopItem.order)) continue;
                        tempList.Add(new StationPoint()
                        {
                            Name = stopItem.name,
                            Order = stopItem.order,
                            IsCurrentStation = stopItem.order == item.order
                        });
                    }

                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        BusRoutes.Add(routeInfo);
                       // OnPropertyChanged(()=> BusRoutes);
                    }));
                }
            });
        }

        #endregion

        #region Command implements

        public void OnConnect(object payload)
        {
            Dowork();
        }

        public void OnDisConnect(object payload)
        {
            _cancelToken.Cancel();
            HeartBeat();
        }

        public void OnHeartBeat(object payload)
        {
            HeartBeat();
        }

        #endregion

        #region Private methods

        private void Dowork()
        {
            _cancelToken = new CancellationTokenSource();
            IsRunning = true;
            LogHelper.LogInfo("Begin to work...");
          

            Task.Factory.StartNew(() =>
            {
                try
                {
                    InitializeData();

                    while (true)
                    {
                        if (_cancelToken.IsCancellationRequested)
                        {
                            LogHelper.LogInfo("Client disconnect requested!");
                            break;
                        }

                        _client = null;

                        try
                        {
                            _client = new TcpClient(_serverAddress, _serverPort);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogError(ex.Message, ex);
                        }

                        if (_client == null)
                        {
                            LogHelper.LogInfo("连接失败，20s后尝试重连！");
                            Thread.Sleep(20 * 1000);
                            continue;
                        }

                        LogHelper.LogInfo("Connected!");
                        LogOn();

                        if (!_client.Connected)
                        {
                            LogHelper.LogInfo("登录失败，请检查登录密码后重试！");
                        }
                        else
                        {
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

                                if (_cancelToken.IsCancellationRequested) break;
                            }

                            stream.Close();
                            _client.Close();
                        }

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

                Application.Current.Dispatcher.Invoke(new Action(() => { IsRunning = false; }));
            }, _cancelToken.Token);
        }

        private void HeartBeat()
        {
            if (_client == null || !_client.Connected) return;

            var message = new Message();
            message.MessageBody = new HeartBeatBody();
            message.Header = new MessageHeader()
            {
                MessageId = FunctionEnum.CLIENT_HEART_BEAT,
                Token = _clientToken,
                SequenceNO = Interlocked.Increment(ref _clientSquenceNO)
            };

            LogHelper.LogInfo(string.Format("时间到了，心跳一下! Token:{0}, SeqeneceNo:{1}", message.Header.Token, message.Header.SequenceNO));
            SendMessage(message.GetMessageBytes());
        }

        private void LogOn()
        {
            if (_client == null || !_client.Connected) return;

            var message = new Message();
            message.MessageBody = new ClientLogonBody()
            {
                Password = "123456",
                RuntimeVersion = "Android 5.1.1",
                FrontEndVersion = "1.0.0"
            };
            message.Header = new MessageHeader()
            {
                MessageId = FunctionEnum.CLIENT_LOGON,
                Token = 0x00,
                SequenceNO = Interlocked.Increment(ref _clientSquenceNO)
            };
            LogHelper.LogInfo(string.Format("赶紧登录一下! Token:{0}, SeqeneceNo:{1}", message.Header.Token, message.Header.SequenceNO));
            SendMessage(message.GetMessageBytes());
        }

        private void SendGeneralAnswer(FunctionEnum function, int sequenceNo)
        {
            var message = new Message();
            message.MessageBody = new ClientGeneralAnsBody()
            {
                AnsMessageId = function,
                Result = MessageResultEnum.SUCCEED,
                SequenceNO = sequenceNo
            };
            message.Header = new MessageHeader()
            {
                MessageId = FunctionEnum.CLIENT_ANS,
                Token = _clientToken,
                SequenceNO = 0
            };

            LogHelper.LogInfo(string.Format("回复实时路线信息! Token:{0}, SeqeneceNo:{1}", message.Header.Token, sequenceNo));
            SendMessage(message.GetMessageBytes());
        }

        private void SendMessage(byte[] msg)
        {
            if (_client == null || !_client.Connected) return;

            var encodedMsg = BytesCoder.Encode(msg);
            var stream = _client.GetStream();
            stream.Write(encodedMsg, 0, encodedMsg.Length);
            string data = BitConverter.ToString(encodedMsg).Replace("-", " ");
            LogHelper.LogDebug(string.Format("Sent: {0}", data));
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
            LogHelper.LogDebug(string.Format("Received: {0}", data));
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
                    case FunctionEnum.SERVER_ANS:
                        {
                            var body = msg.MessageBody as ServerGeneralAnsBody;
                            LogHelper.LogInfo(string.Format("收到服务器通用应答！结果：{0}，回应功能：{1}，流水号：{2}",
                                body.Result, body.AnsMessageId, body.SequenceNO));
                        }
                        break;
                    case FunctionEnum.SERVER_LOGIN_ANS:
                        {
                            var body = (msg.MessageBody as ServerLogonAnsBody);
                            _clientToken = body.Token;
                            LogHelper.LogInfo(string.Format("登录成功，登录结果:{0}，服务端系统时间为：{1}，Token:{2}",
                                body.Status.ToString(),
                                body.Time.ToString("yyyy-MM-dd HH:mm:ss"),
                                body.Token));

                            if (body.Status == LogonResultEnmu.SUCCEED) _heartBeatTimer.Start();
                            else _heartBeatTimer.Stop();
                        }
                        break;
                    case FunctionEnum.SERVER_REALTIME_DATA:
                        {
                            var body = msg.MessageBody as RealTimeDataBody;
                            var str = string.Empty;
                            foreach (var bus in body.BusLocations)
                            {
                                str += string.Format(" 【站点序号：{0}，站内：{1}，过站：{2}，闪烁：{3}，本站车数量{4}】 ",
                                      bus.StationNo, bus.IsInstation, bus.IsPassed, bus.IsBling, bus.StationBusCount);
                            }

                            LogHelper.LogInfo(string.Format("收到服务器实时信息！ 路线ID：{0}，路线方向{1}，班车数量：{2},定制信息：【{3}】,详情：{4} "
                                , body.RouteId, body.RouteDirection, body.BusCount, body.CustomedInfo, str));

                            //Anser it
                            SendGeneralAnswer(FunctionEnum.SERVER_REALTIME_DATA, msg.Header.SequenceNO);
                            UpdateViewData(body);
                        }

                        break;
                    default:
                        LogHelper.LogDebug("收到服务器信息....暂时不认识.......XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX.");
                        break;
                }
            }
        }

        private void UpdateViewData(RealTimeDataBody body)
        {
            var locations = body.BusLocations;
            var routeInfo = BusRoutes.FirstOrDefault(x => x.RouteId == body.RouteId);
            if (routeInfo != null)
            {
                routeInfo.CustomedInfo = body.CustomedInfo;
                routeInfo.BusCount = body.BusCount;

                foreach (var item in routeInfo.Stations)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        item.CurrentStationBusCount = 0;
                        item.BusRelativeLocation = BusRelativeEnum.NO_BUS;
                        // item.IsBling = false;
                    }));
                }

                foreach (var item in locations)
                {
                    //item.CustomedString
                    var stationPoint = routeInfo.Stations.FirstOrDefault(x => x.Order == item.StationNo);
                    if (stationPoint != null)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            stationPoint.CurrentStationBusCount = item.StationBusCount;
                            stationPoint.BusRelativeLocation = item.IsInstation ? BusRelativeEnum.IN_STATION
                                : item.IsPassed ? BusRelativeEnum.RIRGHT_STATION : BusRelativeEnum.IN_STATION;// 左侧 显示为站内
                            //stationPoint.IsBling = true;
                        }));
                    }
                }
            }
        }

        #endregion
    }
}
