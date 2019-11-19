﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Mcdonald.View
{
    class Client
    {
            public Byte[] buffer = new Byte[100];
            public Socket socket;
            string address = "10.80.163.138";
            UInt16 port = 80;
            Boolean isConnect = false;
            public bool IsConnected
            {
                get { return isConnect; }
            }

            

            public bool ConnectServer()
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);


                try
                {
                    socket.Connect(address, port);
                    isConnect = true;
                }
                catch
                {
                    isConnect = false;
                }


                if (isConnect)
                {
                    socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(handleDataReceive), socket);
                    Debug.WriteLine("연결 성공");
                    return true;
                }
                else
                {
                    Debug.WriteLine("연결 실패");
                     return true;
            
                }
            }

            public void SendMessage(String message)
            {

                buffer = Encoding.UTF8.GetBytes(message);

                try
                {
                    socket.BeginSend(buffer, 0, buffer.Length, 0, new AsyncCallback(handleDataSend), socket);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("전송 중 오류\n메세지: " + ex.Message);
                }
            }

            private void handleDataReceive(IAsyncResult ar)
            {

                Int32 recvBytes;
                try
                {
                    recvBytes = socket.EndReceive(ar);
                }
                catch
                {
                    return;
                }

                if (recvBytes > 0)
                {
                    Byte[] msgByte = new Byte[recvBytes];
                    Array.Copy(buffer, msgByte, recvBytes);
                    checkMsg(Encoding.UTF8.GetString(msgByte));

                    Debug.WriteLine("메세지 받음 : ", Encoding.UTF8.GetString(msgByte));
                }

                try
                {
                    socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(handleDataReceive), socket);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("자료 수신 오류 : " + ex.Message);
                    return;
                }
            }

            private void handleDataSend(IAsyncResult ar)
            {
                Int32 sentBytes;

                try
                {
                    sentBytes = socket.EndSend(ar);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("자료 송신 오류 : " + ex.Message);
                    return;
                }

                if (sentBytes > 0)
                {
                    Byte[] msgByte = new Byte[sentBytes];
                    Array.Copy(buffer, msgByte, sentBytes);

                    Debug.WriteLine("메세지 보냄 : " + Encoding.UTF8.GetString(msgByte));
                }
            }


            private void checkMsg(string msg)
            {
                if(msg == "200")
                {
                    Debug.WriteLine("성공");
                }
            }
        }
    }
