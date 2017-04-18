using FWclient.forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    /// <summary>
    /// 将防火墙设备恢复成有IP模式
    /// </summary>
    /// <param name="fw_dev">防火墙设备</param>
    interface IResetIP
    {
        bool ResetIP(FWDeviceForm fw_dev);
    }
}
