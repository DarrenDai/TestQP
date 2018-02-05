using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQP.Sockets.BodyDefinitions
{
    public interface IMessageBody
    {
        byte[] GetBodyBytes();

        void FromBytes(byte[] bytes);

        int GetBodyLength();
    }
}
