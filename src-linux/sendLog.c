#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <unistd.h>

#include "sendLog.h"
#include "handledata.h"
#include "get_firewall_mac.h"

void sendLog()
{
    //打开日志文件
    char buff[350];
    char *src_ip = "1.1.1.1";
    while (1)
    {
        FILE *fp;
        fp = fopen("/var/log/kern.log", "r+");
        if (fp == NULL)
        {
            sleep(3);
            fp = fopen("/var/log/kern.log", "r+");
        }
        //else
          //  printf("open successful\n");
        // 发送日志
        while (fgets(buff, 350, fp) != NULL)
        {
            if (buff[strlen(buff) - 1] == '\n')
            //printf("%s",buff);
            {
                char *p = strstr(buff, "MAC");
                char *q = strstr(buff, "PHYSIN");
                char ETH_name[4]="br0";
		        u_char Firewall_mac[6];
		        GetMac(ETH_name,Firewall_mac);//获取防火墙设备MAC地址
	            u_char firewall_mac[20];
	           sprintf(firewall_mac,"%02X:%02X:%02X:%02X:%02X:%02X", Firewall_mac[0],Firewall_mac[1],Firewall_mac[2],Firewall_mac[3],Firewall_mac[4],Firewall_mac[5]);
               strncat(buff, "$", strlen("$"));
               strncat(buff,firewall_mac,strlen(firewall_mac));
                if (p != NULL & q != NULL&strlen(MAC)!=0 & strlen(DST_IP)!=0)
                {
                    send_udp(DST_IP, src_ip, MAC, buff, 8000, 8000);
                    *p = 0;
                    *q = 0;
                }
            }
        }
        fclose(fp);
        system(":>/var/log/kern.log");
        sleep(1);
    }
}