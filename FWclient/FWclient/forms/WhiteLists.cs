using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    /// <summary>
    /// 白名单功能
    /// </summary>
    class WhiteLists
    {
        private string src_IP;
        private string port;

        public void setsrc_IPAndPort(string src_IP, string port)
        {
            this.src_IP = src_IP;
            this.port= port;
        }

        public string getSrc_IP()
        {
            return src_IP;
        }

        public string getPort()
        {
            return port;
        }

    }
}
