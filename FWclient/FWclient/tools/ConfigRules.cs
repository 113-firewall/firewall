using FWclient.forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    /// <summary>
    /// 定义防火墙规则
    /// </summary>
    class ConfigRules : IConfigRules
    {
        private String default_rules = "iptables -P FORWARD  DROP";
        private DeviceForm devform;

        public ConfigRules(DeviceForm devform)
        {
            this.devform = devform;
        }

        public bool ConfigModbusTcpRules(ModbusTcpRulesForm mtrf, bool add_delete)
        {
           // RulesDataProcess.ModbusTcpRulesDataProcess(mtrf);

            String dpi_pro = "modbusTcp";
            string flag=null;
            if (add_delete == true)
            {
                 flag = "$";
            }             
            
            else if (add_delete == false)
            {
                flag = "#";
            }
            string  dpi_rules_from_master_to_slave0 = "iptables" + " -A" + " " + "FORWARD" + " " + "-p tcp" + " " + "--dport" + " " + "502" + " " + "-s " + mtrf.getSrc_IP() + " " + "-d" + " " + mtrf.getDst_IP() + " " + "-m" + " " + dpi_pro + " " + "--data-addr" + " " + mtrf.getMin_addr() + ":" + mtrf.getMax_addr() + " " + "--modbus-func "+mtrf.getfunc()+" "+"--modbus-data "+mtrf.getMin_data()+":"+mtrf.getMax_data()+" -j" + " " + " ACCEPT";
            string dpi_rules_from_master_to_slave1 = "iptables -A FORWARD -m state --state ESTABLISHED,RELATED -j ACCEPT";
            string dpi_rules_from_master_to_slave_log= "iptables" + " -A" + " " + "FORWARD" + " " + "-p tcp" + " " + "--dport" + " " + "502" + " " + "-s " + mtrf.getSrc_IP() + " " + "-d" + " " + mtrf.getDst_IP() + " " + "-m" + " " + dpi_pro + " " + "--data-addr" + " " + mtrf.getMin_addr() +":"+ mtrf.getMax_addr() + " " + "--modbus-func " + mtrf.getfunc() + " "+"--modbus-data " + mtrf.getMin_data() + ":" + mtrf.getMax_data() + " -j" + " " + "LOG" + " " + "--log-prefix " + "\"" + "ACCEPT&modbusTCP&code_legal " + "\"";
            string rule = flag +default_rules+" && "+ dpi_rules_from_master_to_slave_log+" && "+ dpi_rules_from_master_to_slave0 + " && " + dpi_rules_from_master_to_slave1;
            
            SendInfo sendcmd = new SendInfo(devform);
            
            return sendcmd.SendConfigInfo(rule);
        }

        public bool ConfigOPCRules(OPCRulesForm orf, bool add_delete)
        {
            string flag = null;
            if (add_delete == true)
            {
                flag = "$";
            }

            else if (add_delete == false)
            {
                flag = "#";
            }
            String opc_rules_from_client_to_server0 = "iptables -A FORWARD -p tcp -s " + orf.getSrc_IP() + " -d " + orf.getDst_IP() + " --dport 135 -m state --state ESTABLISHED -j NFQUEUE --queue-num 1";
            String opc_rules_from_client_to_server1 = "iptables -A FORWARD -p tcp -s "+ orf.getDst_IP() +" -d "+ orf.getSrc_IP();
            String opc_rules_from_client_to_server_log= "iptables -A FORWARD -p tcp -s " + orf.getSrc_IP() + " -d " + orf.getDst_IP() + " --dport 135 -m state --state ESTABLISHED -j LOG --log-prefix "+ "\"" + "ACCEPT&OPC&ESTABLISHED " + "\"";
            //String opc_rules_from_server_to_client = "iptables -A FORWARD -p tcp -s " + orf.getDst_IP() + " -d " + orf.getSrc_IP() + " --sport 135 -m state --state ESTABLISHED -j NFQUEUE --queue-num 1";
            string rule = flag + opc_rules_from_client_to_server_log+" && "+opc_rules_from_client_to_server0 + " && "+ opc_rules_from_client_to_server1;
            
            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(rule);
        }

        public bool ConfigDNP3Rules(DNP3RulesForm dnp3rf, bool add_delete)
        {
            string flag = null;
            if (add_delete == true)
            {
                flag = "$";
            }

            else if (add_delete == false)
            {
                flag = "#";
            }
            String dnp3_rules_from_client_to_server_new = "iptables -A FORWARD -p tcp -s " + dnp3rf.getSrc_IP() + " -d " + dnp3rf.getDst_IP() + " --dport 20000 -m state --state NEW -j ACCEPT";
           // String dnp3_rules_from_server_to_client_new = "iptables -A FORWARD -p tcp -s " + dnp3rf.getDst_IP() + " -d " + dnp3rf.getSrc_IP() + " --sport 20000 -m state --state NEW -j ACCEPT";
            String dnp3_rules_from_client_to_server_established = "iptables -A FORWARD -p tcp -s " + dnp3rf.getSrc_IP() + " -d " + dnp3rf.getDst_IP() + " --dport 20000 -m state --state ESTABLISHED -j ACCEPT";
            String dnp3_rules_from_client_to_server_back = "iptables -A FORWARD -p tcp -d " + dnp3rf.getSrc_IP() + " -s " + dnp3rf.getDst_IP();
            string dnp3_rules_from_client_to_server_log = "iptables -A FORWARD -p tcp -s " + dnp3rf.getSrc_IP() + " -d " + dnp3rf.getDst_IP() + " --dport 20000 -m state --state ESTABLISHED -j LOG --log-prefix " + "\"" + "ACCEPT&DNP3&ESTABLISHED " + "\"";
           // String dnp3_rules_from_server_to_client_established = "iptables -A FORWARD -p tcp -s " + dnp3rf.getDst_IP() + " -d " + dnp3rf.getSrc_IP() + " --sport 20000 -m state --state ESTABLISHED -j ACCEPT";
            string rule =flag+ dnp3_rules_from_client_to_server_log + " && " + dnp3_rules_from_client_to_server_new+" && " +dnp3_rules_from_client_to_server_established;

            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(rule);
        }

        public bool ConfigWhiteLists(WhiteLists wl, bool add_delete)
        {
            string flag = null;
            if (add_delete == true)
            {
                flag = "$";
            }

            else if (add_delete == false)
            {
                flag = "#";
            }
            string whiteList_from_client_to_server0 = "iptables -A FORWARD -p tcp -s"+wl.getSrc_IP()+"--dport"+wl.getPort();
           // string whiteList_from_client_to_server1 = "iptables -A FORWARD -p tcp -d" + wl.getSrc_IP() + "--sport" + wl.getPort();
            string changewl = flag+" && "+whiteList_from_client_to_server0 ;

            SendInfo sendcmd = new SendInfo(devform);           
            return sendcmd.SendConfigInfo(changewl);
        }

        public bool ClearAllRules()
        {
            string rule = "iptables -P FORWARD ACCEPT && iptables -F && iptables -X && iptables -Z && iptables-restore</etc/iptables.up.rules";

            SendInfo sendcmd = new SendInfo(devform);
            return sendcmd.SendConfigInfo(rule);
            //sendcmd.SendConfigInfo("kill 'ps -e | grep snort | awk '{print $1}' |head -1"+"!");
        }

    }
}
