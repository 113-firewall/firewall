/*
author : WangBin
function : Handle_data接口
modify date : 
*/
#ifndef HANDLEDATA_H
#define HANDLEDATA_H

extern void handle_data();	/*nflog拦截数据包并根据数据包内容进行处理*/
extern char MAC[20]; 
extern char DST_IP[20];

#endif // !HANDLEDATA_H
