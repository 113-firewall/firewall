using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWclient
{
    class ResetIP:IResetIP
    {
        bool IResetIP.ResetIP(forms.FWDeviceForm fw_dev)
        {
            string cmd = "/etc/init.d/networking restart";
            fw_dev.setDev_port(22222);
            SendInfo sendResetcmd = new SendInfo(fw_dev);

            if (sendResetcmd.SendConfigInfo(cmd))
                return true;
            else
            {
                return false;
            }
        }

    }
}
