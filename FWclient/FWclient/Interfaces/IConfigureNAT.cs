using FWclient.forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    interface IConfigureNAT
    {
        ///<summary>
        ///配置源地址映射
        ///<summary>
        ///<param name="devIP">要进行源地址映射的设备IP地址</param>
        ///<param name="EthIP">对应设备的防火墙网口IP</param>
        ///<param name="add_delete">添加或者删除映射规则(true 为添加)</param>
        bool ConfigSNAT(FWDeviceForm fw_dev,string devIP,string EthIP,bool add_delete);
    }
}
