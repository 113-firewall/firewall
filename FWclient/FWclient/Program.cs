#define debug

using FWclient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MySql.Data.MySqlClient;
using System.IO;

namespace FWclient
{
    /// <summary>
    /// 接口测试
    /// </summary>
    class Test
    {
        static void Main(string[] args)
        {

            //ReceiveLog test1 = new ReceiveLog();
            //test1.Save_DisplayLog(true);

            //while (true)
            //{
            //    test1.queue_func();
            //}

            //string[] code = new string[] { "01" };

            //IResetIP res = new ResetIP();
            //if (res.ResetIP(new ProtecDeviceForm("172.16.10.19", ""), "172.16.10.19"))
            //    Console.WriteLine("success");
            //else
            //    Console.WriteLine("fail");

            //IRulesManage test = new RulesManage();

            //test.ChangeModbusTcpRules("any", "any", "12292", "12292", 6, 1000, 2000, "172.16.10.219", true, true);

            //IConfigureNAT nat = new ConfigureNAT();

            //if (nat.ConfigSNAT(new FWDeviceForm("172.16.10.19", 2222, ""),"eth3", "192.168.1.239", "192.168.1.2", false))
            //{
            //    Console.WriteLine("NAT config success");
            //}
            //else
            //    Console.WriteLine("NAT config failed");


            IDevicesCheck devConfirm = new DevicesCheck();
            List<FWDeviceForm> fws = devConfirm.CheckDevices("172.16.10.16", "172.16.10.20");
         
            Console.WriteLine("打印扫描结果 :");

            IRulesManage list = new RulesManage();
            list.ChangeWhiteLists("172.19.18.119","123.23.43.34","172.16.10.19","525","502",true,true); 

            foreach (FWDeviceForm fw in fws)
            {
                string fwip = fw.getDev_IP();
                string fwmac = fw.getDev_MAC();
                List<ProtecDeviceForm> protecDev_list = fw.getProtecDev_list();

                Console.WriteLine("防火墙设备IP : {0} 防火墙设备MAC : {1}", fwip, fwmac);
               
                Console.WriteLine("关联的受保护设备 :");

                if (fwip != "0.0.0.0")
                {
                   
                    foreach (var item in protecDev_list)
                    {
                        string dev_type = item.getDev_type();
                        Console.WriteLine("IP {0}   MAC {1} 设备制造商 {2}", item.getDev_IP(), item.getDev_MAC(), dev_type);

                    }
                }
                else Console.WriteLine("防火墙为无IP模式");
            }

           

            //INoIPConfig inoi = new NoIPConfig();
            //if (inoi.NoipConfig(new FWDeviceForm("172.16.10.219", 22222, "", "", "")))
            //{
            //    Console.WriteLine("成功");
            //}
            //else
            //{
            //    Console.WriteLine("失败");
            //}
            //Console.WriteLine("测试");
            //IResetIP test3 = new ResetIP();
            //test3.ResetIP(new FWDeviceForm("172.16.10.123", 22222, "", "", ""));

#if debug
            Console.ReadLine();
#endif
        }
    }
}
