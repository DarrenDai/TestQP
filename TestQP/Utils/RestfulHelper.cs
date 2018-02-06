using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestQP.Utils
{
    public class RestfulHelper
    {
        public ResponseObject GetStationInfo()
        {
            var temp = DoHttpGet("");
            var t = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObject>(temp);

            return t;
        }

        #region Private methods

        private string DoHttpGet(string content)
        {
            string url = string.Format("http://120.26.45.101:30020/publish/device/info?identifier=60000001");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Http.Get;
            request.Timeout = 30 * 1000;
            request.Accept = "application/json";
            request.KeepAlive = true;
            //request.ContentType = "application/json";

            //byte[] buffer = Encoding.UTF8.GetBytes(content);
            //request.ContentLength = buffer.Length;
            //using (Stream requestStream = request.GetRequestStream())
            //{
            //    requestStream.Write(buffer, 0, buffer.Length);
            //}

            #region 获取返回值

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    string retString = myStreamReader.ReadToEnd();
                    return retString;
                }
            }

            #endregion
        }


        #endregion
    }

    [Serializable]
    public class ResponseObject
    {
        public int id { get; set; }

        public string name { get; set; }

        public string identifier { get; set; }

        public List<LineItem> lines { get; set; }
    }

    [Serializable]
    public class LineItem
    {
        public int id { get; set; }

        public int buslineId { get; set; }

        public string name { get; set; }

        public int branch { get; set; }

        public int order { get; set; }

        public string from { get; set; }

        public string to { get; set; }

        public string first { get; set; }

        public string last { get; set; }

        public List<StopItem> stops { get; set; }
    }

    [Serializable]
    public class StopItem
    {
        public int id { get; set; }

        public string name { get; set; }

        public int order { get; set; }
    }
}
