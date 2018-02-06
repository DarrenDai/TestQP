using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQP.Constants
{
    public enum FunctionEnum
    {
        #region Up direction

        /// <summary>
        /// 通用应答
        /// </summary>
        CLIENT_ANS = 0x0601,

        /// <summary>
        /// 设备登录
        /// </summary>
        CLIENT_LOGON = 0x0602,

        /// <summary>
        /// 心跳
        /// </summary>
        CLIENT_HEART_BEAT = 0x06ff,

        #endregion

        #region Down direction

        /// <summary>
        /// 通用应答
        /// </summary>
        SERVER_ANS = 0x0901,

        /// <summary>
        /// 登录应答
        /// </summary>
        SERVER_LOGIN_ANS = 0x09A6,

        /// <summary>
        ///  公交实时数据
        /// </summary>
        SERVER_REALTIME_DATA = 0x09A3,

        #endregion
    }
}
