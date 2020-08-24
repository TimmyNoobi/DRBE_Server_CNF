using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using FTD2XX_NET;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.InteropServices;

namespace DRBE_Server_CNF
{
    public class Matlab_Server
    {
        public TcpListener listener;
        public NetworkStream nwStreamread;
        public NetworkStream nwStreamwrite;
        public TcpClient client;
        public bool Matlab_wirte_stream_connected_flag = false;

        private BackgroundWorker search_bg = new BackgroundWorker();
        private BackgroundWorker read_bg = new BackgroundWorker();

        private Request_Management requestManagement;

        public string addr = "";
        public int port = 0;

        private TcpClient UI_Client;
        private NetworkStream UI_nwStreamwrite;

        public bool UI_Client_inserted_flag = false;

        public bool Matlab_connected_flag = false;
        public void Connect_UI(TcpClient x, NetworkStream y)
        {
            UI_Client = x;
            UI_nwStreamwrite = y;
            UI_Client_inserted_flag = true;
        }
        public void Start(string x, int y, Request_Management z)
        {
            addr = x;
            port = y;
            requestManagement = z;
            Initialize();
        }

        public void Initialize()
        {

            search_bg.DoWork += new DoWorkEventHandler(search_bg_DoWork);
            search_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(search_bg_RunWorkerCompleted);
            search_bg.RunWorkerAsync();

            read_bg.DoWork += new DoWorkEventHandler(read_bg_DoWork);
            read_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(read_bg_RunWorkerCompleted);

        }

        private void search_bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Search Completed, tid " + Thread.CurrentThread.ManagedThreadId);
        }

        private void search_bg_DoWork(object sender, DoWorkEventArgs e)
        {
            Connect_Server();
        }

        private void read_bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Read exits, tid " + Thread.CurrentThread.ManagedThreadId);
            //done = true;
        }

        private void read_bg_DoWork(object sender, DoWorkEventArgs e)
        {
            Reading();
        }
        public void Connect_Server()
        {
            try
            {
                listener = new TcpListener(IPAddress.Parse(addr), port);
                listener.Start();
                Console.WriteLine("Try to listen to port: " + listener.LocalEndpoint.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Server Start: " + ex.ToString());
            }
            while (true)
            {
                try
                {
                    client = listener.AcceptTcpClient();
                    if (client.Connected)
                    {
                        Console.WriteLine("You are connected to TC: " + client.Client.LocalEndPoint.ToString());
                        nwStreamread = client.GetStream();
                        nwStreamwrite = client.GetStream();
                        Matlab_wirte_stream_connected_flag = true;
                        read_bg.RunWorkerAsync();
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Client Search: " + ex.ToString());
                    Thread.Sleep(1000);
                }
            }

        }






        private void Reading()
        {
            byte[] result = new byte[1];
            int bytesRead = 0;

            while (true)
            {
                try
                {
                    if (client.Connected)
                    {
                        Matlab_connected_flag = true;
                        try
                        {
                            bytesRead = nwStreamread.Read(result, 0, 1);
                            int i = 0;
                            if (i < bytesRead)
                            {
                                Packet_receiver(result[i]);
                                i++;
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            Matlab_connected_flag = false;
                            Matlab_wirte_stream_connected_flag = false;
                            Console.WriteLine("Restart server port: " + listener.LocalEndpoint.ToString());

                            listener.Stop();
                            search_bg.Dispose();
                            read_bg.Dispose();
                            Thread.Sleep(2000);


                            search_bg = new BackgroundWorker();
                            read_bg = new BackgroundWorker();
                            search_bg.DoWork += new DoWorkEventHandler(search_bg_DoWork);
                            search_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(search_bg_RunWorkerCompleted);
                            search_bg.RunWorkerAsync();

                            read_bg.DoWork += new DoWorkEventHandler(read_bg_DoWork);
                            read_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(read_bg_RunWorkerCompleted);

                            break;
                        }

                    }
                    else
                    {
                        Console.WriteLine("Client disconnect");
                        Thread.Sleep(2000);
                        break;

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Thread.Sleep(2000);
                }

            }
        }





        enum B0
        {
            UWP = 0x01,
            Matlab = 0x02,
            Python = 0x03,

            RISKV = 0x11,
            DSSC = 0x12,
            FPGA = 0x13

        }

        private int Packet_receiver_index = 0;
        private int Packet_len = 0;
        private bool Read_done = false;
        private byte device = 0;
        private List<byte> Packet_receiver_result = new List<byte>();

        private byte Packet_command = 0x00;
        private void Packet_receiver(byte x)
        {
            Console.WriteLine(x.ToString());
            Packet_receiver_result.Add(x);
            Packet_receiver_index++;
            if (Packet_receiver_index == 1)
            {
                device = x;
                Packet_len = 0;
            } //device
            else if (Packet_receiver_index == 2)
            {
                Packet_len = x;
            }//length MS
            else if (Packet_receiver_index == 3)
            {
                Packet_len = Packet_len * 255 + x;
                Console.WriteLine("Packet Length: " + Packet_len.ToString());
            } // length LS
            else if (Packet_receiver_index == 4)
            {
                Packet_command = x;

            }
            else if (Packet_receiver_index == Packet_len + 7)
            {
                if (device != x)
                {
                    Console.WriteLine("Error: " + BitConverter.ToString(Packet_receiver_result.ToArray()));
                }
                else
                {
                    if (Packet_command == 0x10 && UI_Client_inserted_flag)
                    {
                        UI_nwStreamwrite.Write(Packet_receiver_result.ToArray(), 0, Packet_receiver_result.Count);
                        UI_nwStreamwrite.Flush();
                    }
                    Console.WriteLine("Received: " + BitConverter.ToString(Packet_receiver_result.ToArray()));
                    //Packet_receiver_result[3] = 0x0F;
                    //nwStreamread.Write(Packet_receiver_result.ToArray(), 0, Packet_receiver_result.Count);
                    //nwStreamread.Flush();
                    //Console.WriteLine("Sent: " + BitConverter.ToString(Packet_receiver_result.ToArray()));
                    //requestManagement.Request(Packet_receiver_result);
                    Packet_receiver_result = new List<byte>();

                }
                Packet_receiver_index = 0;

            }
            else
            {

            }


            if (Read_done)
            {

            }
        }

    }
}
