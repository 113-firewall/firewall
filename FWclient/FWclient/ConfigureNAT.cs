using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    class ConfigureNAT:IConfigureNAT
    {
        private string devIP;
        private string EthIP;
        public bool ConfigSNAT(FWDeviceForm fw_dev,string EthName, string devIP, string EthIP, bool add_delete)
        {
            string flag = "";string configEth_bridge = "";string configInfo = ""; string configEth_IP = "";           
            string rule = "iptables -t nat -A POSTROUTING -s " + devIP + " -o br0 -j SNAT --to-source " + fw_dev.getDev_IP();
            if (add_delete)
            {
                flag = "$";
                configEth_bridge = "brctl delif br0 " + EthName;//先将网口从网桥上删除
                configEth_IP = "ifconfig " + EthName + " " + EthIP + " netmask 255.255.255.0" + " up";
                configInfo = flag + configEth_bridge + " & " + configEth_IP + " & "+rule;
            }
            if (!add_delete)
            {
                flag = "#";
                configEth_bridge = "brctl addif br0 " + EthName;
                configEth_IP = "ifconfig "+EthName+" "+"0.0.0.0 up";                
                configInfo = flag + configEth_IP + " & " + configEth_bridge + " & " + rule;
            }                             
           
            fw_dev.setDev_port(22222);          
            SendInfo sendcmd = new SendInfo(fw_dev);
            if (sendcmd.SendConfigInfo(configInfo))
            {
                return true;
            }

            else
                return false;
        }


      public string getdevIP()
        {
            return devIP;
        }

        public string getEthIP()
        {
            return EthIP;
        }

        public void setdevIp(string devIP)
        {
            this.devIP = devIP;

        }

        public void setEthIP(string EthIP)
        {    
            this.EthIP = EthIP;

        }

    }
}
