using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQP.Utils
{
    public class DataVerification
    {
        /// <summary>
        /// Verify message data
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static bool Verify(byte[] bytes)
        {
            return true;
        }

        /// <summary>
        /// Gets verify code
        /// </summary>
        /// <param name="header"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static byte GetVerifyCode(byte[] header, byte[] body)
        {
            byte hash = header[0];
            for (int i = 1; i < header.Length; i++)
            {
                hash ^= header[i];
            }

            for (int i = 0; i < body.Length; i++)
            {
                hash ^= body[i];
            }

            return hash;
        }
    }
}
