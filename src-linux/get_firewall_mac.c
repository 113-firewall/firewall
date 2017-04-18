#include <errno.h>
#include <stdio.h>
#include <sys/socket.h>
#include <net/if_arp.h>
#include <netinet/in.h>
#include <linux/sockios.h>
#include <sys/ioctl.h>
#include <string.h>
#include <sys/types.h>
#include <unistd.h>
#include <sys/ioctl.h>
#include <net/if.h>

#include "get_firewall_mac.h"

void GetMac( char *ifname, char *mac)
  {
    int sock;
    struct ifreq ifr;
    sock = socket(AF_INET,SOCK_STREAM,0);
    if(sock<0)
    {
      perror("socket error!\n");
      return -1;
    }
    //memset(&ifr,0,sizeof(ifr));
    strncpy(ifr.ifr_name,ifname,IFNAMSIZ-1);
    ifr.ifr_name[IFNAMSIZ - 1] = 0;  
    if(ioctl(sock,SIOCGIFHWADDR,&ifr,sizeof(ifr)) == 0)
    {
      memcpy(mac,ifr.ifr_hwaddr.sa_data,6);
      
    }
    else
    {
      perror("ioctl error!\n");
    }
  close(sock);
  }