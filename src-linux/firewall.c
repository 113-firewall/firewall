/*
author : WangBin
function : 
modify date : 
*/

#include <sys/types.h>
#include <stdlib.h>
#include <stdio.h>
#include <unistd.h>
#include <pthread.h>
#include <mysql.h>

#include "handledata.h"
#include "sendLog.h"


int main(int argc, char const *argv[])
{
		/* 连接mysql规则数据库*/
            MYSQL *mysql=malloc(sizeof(MYSQL));MYSQL_RES *my_res;MYSQL_ROW my_row;
			int res;int rows;int i;
            mysql_init(mysql);
	  if (mysql_real_connect(mysql,"localhost", "root", "123", "firewallrules",0,NULL,0))
	           printf("Connection success\n");
	   else
	  {
		printf("Connected...\n");
	  }
	        char query[]="select rules from firewallrules" ;
		    res= mysql_real_query(mysql,query,(unsigned int)strlen(query));
			if( res != 0 )  
                  printf("Select fail");  
		    my_res=mysql_store_result(mysql);
			rows=mysql_num_rows(my_res);
			char reset[]="iptables -F && iptables-restore</etc/iptables.up.rules";
			system(reset);//将规则全部清空后加载内置规则
			while(1)
			{
			my_row=mysql_fetch_row(my_res);
			if(my_row==NULL) break;
			for(i=0;i<rows;i++)			
				{
					system((char*)my_row[i]);//将规则数据库中的规则逐条执行
				}
				break;
			}
			mysql_free_result(my_res);
			mysql_close(mysql);

	    pthread_t send;
	    pthread_create(&send,NULL,(void *)sendLog,NULL);
	    handle_data();
		//pthread_t handle;
		pthread_join(send,NULL);
		
}