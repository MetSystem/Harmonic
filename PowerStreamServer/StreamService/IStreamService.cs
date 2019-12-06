using System;
using System.Collections.Generic;
using System.Text;

namespace PowerStreamServer
{
    public interface IStreamService
    {
        void Send(string streamName);
    }
}
