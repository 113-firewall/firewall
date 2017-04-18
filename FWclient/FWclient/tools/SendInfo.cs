//#define debug

using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SharpPcap;
using PacketDotNet;
using FWclient;

namespace FWclient
{
    /// <summary>
    /// 向防火墙设备发送信息
    /// </summary>
    class SendInfo
    {
        private  DeviceForm devform;
        private static bool config_info_confirm;
        private static string FWIPinfo;
        /*初始化设备*/
        public SendInfo(DeviceForm devform)
        {
            this.devform = devform;
        }

        /// <summary>
        /// 发送配置信息
        /// </summary>
        /// <param name="cmd">需要配置的规则</param>
        public bool  SendConfigInfo(string cmd)
        {
            config_info_confirm = false;

            byte[] head = { 0x0f, 0x0e, 0x0d };   //自定义数据包包头
            byte[] body = Encoding.ASCII.GetBytes(cmd + "!");
            byte[] data = head.Concat(body).ToArray();
            UdpClient client =null;
            IPAddress remoteIP = IPAddress.Parse(devform.getDev_IP());
            int remotePort = devform.getDev_port();
            IPEndPoint remotePoint = new IPEndPoint(remoteIP, remotePort);

            CaptureDeviceList devices = CaptureDeviceList.Instance;
            // If no devices were found print an error
            if (devices.Count < 1)
            {
                // Console.WriteLine("No devices were found on this machine");
                return false;
            }
            ICaptureDevice device = devices[0];
            device.OnPacketArrival += new PacketArrivalEventHandler(configDev_OnPacketArrival);
            int readTimeoutMilliseconds = 1000;
            device.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);
            string filter = "ip and udp";
            device.Filter = filter;
            device.StartCapture();

#if debug
            Console.WriteLine("start sending:");
#endif
            client = new UdpClient();
            client.Send(data, data.Length, remotePoint);

            /* 监听无IP配置是否成功返回消息  */
            bool done = false;
            UdpClient listener = new UdpClient(30333);
            IPEndPoint groupEP = null;
            //DateTime beforDT = System.DateTime.Now;
            int n=0;
            try
            {

                while (!done&&n<=10)
                {
                    byte[] content = listener.Receive(ref groupEP);
                    string con = Encoding.Default.GetString(content);
                    if (con == "yes")
                    {
                        done = true; 
                        config_info_confirm = true;
                    }
                    n++;
                    //DateTime afterDT = System.DateTime.Now;
                    //TimeSpan ts = afterDT.Subtract(beforDT);
                    //Console.WriteLine("{0}",ts.TotalSeconds);
                }
            }
            catch (Exception e)
            {
                config_info_confirm = false;
            }
            finally
            {
                listener.Close();
            }

            client.Close();
#if debug
            Console.WriteLine("send successfully!");
#endif
            device.StopCapture();
            device.Close();
           
            return config_info_confirm;
        }

        /// <summary>
        /// 发送扫描设备的数据
        /// </summary>
        public void SendCheckInfo()
        {
            string mac = GetLocalMacAddr.GetMacAddr();  //获取本机MAC地址
            //Console.WriteLine("local mac is {0}", mac);

            byte[] head = { 0x0f, 0x0e, 0x0d };   //自定义数据包包头
            byte[] body = Encoding.ASCII.GetBytes(mac + "!");
            byte[] data = head.Concat(body).ToArray();

            UdpClient client = null;
            IPAddress remoteIP = IPAddress.Parse(devform.getDev_IP());
            int remotePort = devform.getDev_port();
            IPEndPoint remotePoint = new IPEndPoint(remoteIP, remotePort);

            client = new UdpClient(); 
#if debug
            Console.WriteLine("start sending:");
#endif
            client.Send(data, data.Length, remotePoint);
            client.Close();
#if debug
            Console.WriteLine("send successfully!");
#endif     
        }

        private static void configDev_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            //var time = e.Packet.Timeval.Date;
            //var len = e.Packet.Data.Length;

            var packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);

            UdpPacket udpPacket = UdpPacket.GetEncapsulated(packet);

            if (udpPacket != null)
            {
                var ipPacket = (PacketDotNet.IpPacket)udpPacket.ParentPacket;
                //System.Net.IPAddress srcIp = ipPacket.SourceAddress;
                int srcPort = udpPacket.SourcePort;
                int dstPort = udpPacket.DestinationPort;
#if debug
                Console.WriteLine("端口检查");
#endif             
                    if (srcPort == 30332 && dstPort == 30333)
                    {
                        byte[] payload = udpPacket.PayloadData;
                        string content = System.Text.Encoding.Default.GetString(payload); //确认包的内容
                        if (content == "yes")
                        {
                            config_info_confirm = true;
                        Console.WriteLine("{0}",config_info_confirm.ToString());
                        }
                    }

                    if(srcPort==30334 & dstPort==30335)
                {
                    byte[] payload = udpPacket.PayloadData;
                    string content = System.Text.Encoding.Default.GetString(payload); //确认包的内容
                    FWIPinfo = content;
                    Console.WriteLine("{0}",FWIPinfo);
                }
                
            }
        }
    }
}
