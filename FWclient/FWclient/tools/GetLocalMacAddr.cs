﻿using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    class GetLocalMacAddr
    {
        public static string GetMacAddr()
        {
            try
            {
                string mac = "94-DE-80-26-1A-B5";
                //ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                //ManagementObjectCollection moc = mc.GetInstances();

                //foreach (ManagementObject mo in moc)
                //{
                //    if ((bool)mo["IPEnabled"] == true)
                //    {
                //        mac = mo["MacAddress"].ToString();
                //        break;
                //    }
                //}
                //moc = null;
                //mc = null;
                //ManagementObjectSearcher nisc = new ManagementObjectSearcher("select * from Win32_NetworkAdapterConfiguration");
                //foreach (ManagementObject nic in nisc.Get())
                //{
                //    if (Convert.ToBoolean(nic["ipEnabled"]) == true)
                //    {
                //        Console.WriteLine("{0} - {1}", nic["ServiceName"], nic["MACAddress"]);
                //    }
                //}

                return mac;
            }
            catch
            {

                return "unknow";
            }
            finally
            {

            }
        }
    }
}
