using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQP.Utils
{
    public class LogHelper
    {
        #region Private fields

        private static readonly ILog _logger = LogManager.GetLogger("SYSTEM");

        #endregion

        #region Constructor

        #endregion

        #region Public properties

        #endregion

        #region Commands

        #endregion

        #region Initialize methods

        #endregion

        #region Command implements

        #endregion

        #region Public methods

        public static void LogDebug(string message)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug(message);
                LogToUI(message);
            }
        }

        public static void LogInfo(string message)
        {
            if (_logger.IsInfoEnabled)
            {
                _logger.Info(message);
                LogToUI(message);
            }
        }

        public static void LogError(string message, Exception ex)
        {
            if (_logger.IsErrorEnabled)
            {
                _logger.Error(message, ex);
                LogToUI(message);
            }
        }

        #endregion

        #region Private methods

        private static void LogToUI(string message)
        {
            Provider.EventAggregator.GetEvent<TestQP.Events.Events.LogEvent>().Publish(message);
        }

        #endregion
    }
}
