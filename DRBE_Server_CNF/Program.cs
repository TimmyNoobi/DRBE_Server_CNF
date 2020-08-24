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
using System.Threading.Tasks;

namespace DRBE_Server_CNF
{
    class Program
    {

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;


        //static public Request_Management request_manager = new Request_Management();



        static private bool Matlab_onflag = false;
        static private bool AMT_onflag = true;



        static void Main(string[] args)
        {
            int i = 0;
            string strtemp = "";
            var handle = GetConsoleWindow();
            // Hide
            //ShowWindow(handle, SW_HIDE);

            // Show
            //ShowWindow(handle, SW_SHOW);

            Console.WriteLine("Please keep this window on");

            AMT_read_bg.DoWork += new DoWorkEventHandler(AMT_read_bg_DoWork);
            AMT_read_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AMT_read_bg_RunWorkerCompleted);

            UI_search_bg.DoWork += new DoWorkEventHandler(UI_search_bg_DoWork);
            UI_search_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(UI_search_bg_RunWorkerCompleted);
            UI_search_bg.RunWorkerAsync();

            UI_read_bg.DoWork += new DoWorkEventHandler(UI_read_bg_DoWork);
            UI_read_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(UI_read_bg_RunWorkerCompleted);

            ML_search_bg.DoWork += new DoWorkEventHandler(ML_search_bg_DoWork);
            ML_search_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ML_search_bg_RunWorkerCompleted);
            ML_search_bg.RunWorkerAsync();

            ML_read_bg.DoWork += new DoWorkEventHandler(ML_read_bg_DoWork);
            ML_read_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ML_read_bg_RunWorkerCompleted);

            UN_search_bg.DoWork += new DoWorkEventHandler(UN_search_bg_DoWork);
            UN_search_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(UN_search_bg_RunWorkerCompleted);
            UN_search_bg.RunWorkerAsync();

            UN_read_bg.DoWork += new DoWorkEventHandler(UN_read_bg_DoWork);
            UN_read_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(UN_read_bg_RunWorkerCompleted);

            if (Matlab_onflag)
            {
                Process mp = new Process();
                mp.StartInfo.FileName = "CMD.exe";
                mp.StartInfo.RedirectStandardInput = true;
                mp.StartInfo.RedirectStandardOutput = true;
                mp.StartInfo.CreateNoWindow = true;
                mp.StartInfo.UseShellExecute = false;


                string cmdtxt = "\"C:\\Program Files\\MATLAB\\R2019b\\bin\\matlab.exe\"" + " -nosplash -nodesktop" + " -r " + "run('C:\\Users\\timli\\Desktop\\DRBE_Code\\Matlab(Simulation)\\2020-02-11_LL-DRBE-Dumbbell-Sim\\DRBE_Matlab_Client.m')";
                string cmdtxt2 = "run('C:\\Users\\timli\\Desktop\\DRBE_Code\\Matlab(Client)\\DRBE_Matlab_Client.m'); ";
                mp.Start();
                mp.StandardInput.WriteLine(cmdtxt);
            }
            if(AMT_onflag)
            {
                AMT_read_bg.RunWorkerAsync();
            }


            //System.Diagnostics.Process.Start(@"C:\Program Files\MATLAB\R2019b\bin\matlab.exe");


            //UWP_Port.Start(UWP_Port_addr, UWP_Port_port, request_manager);
            //Thread.Sleep(500);

            //Python_Port.Start(Python_Port_addr, Python_Port_port, request_manager);
            //Thread.Sleep(500);

            //Matlab_Port.Start(Matlab_Port_addr, Matlab_Port_port, request_manager);
            //Thread.Sleep(500);

            while(true)
            {
                Thread.Sleep(500);
            }
            //while (true)
            //{
            //    if(UWP_Port.UI_wirte_stream_connected_flag != UI_Port_flag_old)
            //    {
            //        if (UWP_Port.UI_wirte_stream_connected_flag)
            //        {
            //            UI_Port_flag_old = true;
            //            Matlab_Port.Connect_UI(UWP_Port.client, UWP_Port.nwStreamwrite);
            //        }
            //        else
            //        {
            //            UI_Port_flag_old = false;
            //            Matlab_Port.UI_Client_inserted_flag = false;
            //        }
            //    }
            //    if (Matlab_Port.Matlab_wirte_stream_connected_flag)
            //    {
            //        UWP_Port.Connect_Matlab(Matlab_Port.client, Matlab_Port.nwStreamwrite);
            //    }
            //    else
            //    {
            //        UWP_Port.Matlab_Client_inserted_flag = false;
            //    }
            //    Thread.Sleep(50);
            //}
        }
        #region AMT
        static private BackgroundWorker AMT_read_bg = new BackgroundWorker();
        static private void AMT_read_bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Search Completed, tid " + Thread.CurrentThread.ManagedThreadId);
        }

        static private void AMT_read_bg_DoWork(object sender, DoWorkEventArgs e)
        {
            string output = "";
            int i = 0;
            string strtemp = "";

            Process myProcess = new Process();
            //myProcess.StartInfo = new ProcessStartInfo(@"C:\Users\timli\AppData\Local\Packages\106b18ec-5180-4642-8a0e-198353957681_kbtfgvzxh186t\LocalState\DRBE_CPP1.exe");
            myProcess.StartInfo = new ProcessStartInfo(@"C:\Users\timli\source\repos\DRBE_CPP1\Debug\DRBE_CPP1.exe");

            myProcess.StartInfo.RedirectStandardInput = true;
            //myProcess.StartInfo.WorkingDirectory = workingDirectory;
            //myProcess.StartInfo.FileName = programFilePath;
            //myProcess.StartInfo.Arguments = commandLineArgs;
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.CreateNoWindow = false;
            myProcess.StartInfo.RedirectStandardOutput = true;
            myProcess.StartInfo.RedirectStandardError = true;
            try
            {
                myProcess.Start();
                myProcess.StandardInput.WriteLine("1900000000,60,500000,0.001,6,2,5,16,1,800,6,20,2,1,4,10");
                myProcess.StandardInput.WriteLine("1900000000 60 500000 0.001 6 2 5 16 1 800 6 20 2 1 4 10");
            }
            catch(Exception ex)
            {

            }

            while (true)
            {
                try
                {

                    output = myProcess.StandardOutput.ReadLine();
                    //string outerr = myProcess.StandardError.ReadToEnd();
                    //string str = "";

                    
                    if(output!=null)
                    {
                        i = 0;
                        while (i < output.Length)
                        {
                            if (output[i] == ',')
                            {
                                strtemp = "";
                            }
                            else
                            {
                                strtemp += output[i].ToString();
                            }
                            i++;
                        }
                        Console.WriteLine(output);
                    }
                    else
                    {
                        Thread.Sleep(20);
                    }




                    // reading errors and output async...

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                }
                finally
                {
                }
            }
        }
        #endregion

        #region UI Communication

        //Initiate Server Port
        //static public UI_Server UWP_Port = new UI_Server();   
        static public string UWP_Port_addr = "127.0.0.1";
        static public int UWP_Port_port = 8200;

        //Initiate TCP Listener + Client + read + write stream
        static private TcpListener UI_listener;
        static private TcpClient UI_client;
        static private NetworkStream UI_nwStreamread;
        static private NetworkStream UI_nwStreamwrite;

        static public bool UI_wirte_stream_connected_flag = false;
        static private BackgroundWorker UI_search_bg = new BackgroundWorker();
        static private BackgroundWorker UI_read_bg = new BackgroundWorker();
        //If client connected, read byte by byte
        //If not connected, stop listener, search, read.   Stop for 2 second, restart search and read thread.
        static private void UI_Reading()
        {
            byte[] result = new byte[1];
            int bytesRead = 0;

            while (true)
            {
                try
                {
                    if (UI_client.Connected)
                    {
                        try
                        {
                            bytesRead = UI_nwStreamread.Read(result, 0, 1);
                            int i = 0;
                            if (i < bytesRead)
                            {
                                UI_Packet_receiver(result[i]);
                                i++;
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            UI_wirte_stream_connected_flag = false;
                            Console.WriteLine("Restart server port: " + UI_listener.LocalEndpoint.ToString());

                            UI_listener.Stop();
                            UI_search_bg.Dispose();
                            UI_read_bg.Dispose();
                            Thread.Sleep(2000);


                            UI_search_bg = new BackgroundWorker();
                            UI_read_bg = new BackgroundWorker();
                            UI_search_bg.DoWork += new DoWorkEventHandler(UI_search_bg_DoWork);
                            UI_search_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(UI_search_bg_RunWorkerCompleted);
                            UI_search_bg.RunWorkerAsync();

                            UI_read_bg.DoWork += new DoWorkEventHandler(UI_read_bg_DoWork);
                            UI_read_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(UI_read_bg_RunWorkerCompleted);

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

        static private int UI_Packet_receiver_index = 0;
        static private int UI_Packet_len = 0;

        static private bool UI_Read_done = false;
        static private byte UI_device = 0;
        static private List<byte> UI_Packet_receiver_result = new List<byte>();

        static private byte UI_Packet_command = 0x00;


        //Store byte in Packet_receiver_result;
        //Byte index tracked in Packet_receiver_index, start from 1;
        //Index 1, device
        //Index 2, Packet len, MSB
        //Index 3, Len, LSB
        //Index 4, Command
        //Index len + 7, device, match for correction
        static private void UI_Packet_receiver(byte x)
        {
            //Console.WriteLine(x.ToString());
            UI_Packet_receiver_result.Add(x);
            UI_Packet_receiver_index++;
            if (UI_Packet_receiver_index == 1)
            {
                UI_device = x;
                UI_Packet_len = 0;
            } //device
            else if (UI_Packet_receiver_index == 2)
            {
                UI_Packet_len = x;
            }//length MS
            else if (UI_Packet_receiver_index == 3)
            {
                UI_Packet_len = UI_Packet_len * 255 + x;
                Console.WriteLine("Packet Length: " + UI_Packet_len.ToString());
            } // length LS
            else if (UI_Packet_receiver_index == 4)
            {
                UI_Packet_command = x;
            }
            else if (UI_Packet_receiver_index == UI_Packet_len + 7)
            {
                if (UI_device != x)
                {
                    Console.WriteLine("Error: " + BitConverter.ToString(UI_Packet_receiver_result.ToArray()));
                }
                else
                {
                    if (UI_Packet_command == 0x10 && ML_wirte_stream_connected_flag)
                    {
                        Console.WriteLine("Send to Matlab: " + UI_Packet_receiver_result.Count.ToString());
                        ML_nwStreamwrite.Write(UI_Packet_receiver_result.ToArray(), 0, UI_Packet_receiver_result.Count);
                        ML_nwStreamwrite.Flush();
                    }
                    Console.WriteLine("Received: " + BitConverter.ToString(UI_Packet_receiver_result.ToArray()));
                    UI_Packet_receiver_result[3] = 0x0F;
                    UI_nwStreamread.Write(UI_Packet_receiver_result.ToArray(), 0, UI_Packet_receiver_result.Count);
                    UI_nwStreamread.Flush();
                    //Console.WriteLine("Sent: " + BitConverter.ToString(Packet_receiver_result.ToArray()));
                    //requestManagement.Request(Packet_receiver_result);
                    UI_Packet_receiver_result = new List<byte>();

                }
                UI_Packet_receiver_index = 0;

            }
            else
            {

            }


            if (UI_Read_done)
            {

            }
        }



        static private void UI_search_bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Search Completed, tid " + Thread.CurrentThread.ManagedThreadId);
        }

        static private void UI_search_bg_DoWork(object sender, DoWorkEventArgs e)
        {
            UI_Connect_Server();
        }

        static private void UI_read_bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Read exits, tid " + Thread.CurrentThread.ManagedThreadId);
            //done = true;
        }

        static private void UI_read_bg_DoWork(object sender, DoWorkEventArgs e)
        {
            UI_Reading();
        }



        //New Listener with port addr and port, Listener start
        //try accept client
        //If connected, set UI_nwStreamread, UI_nwStreamwrite
        //Set UI_wirte_stream_connected_flag to true;
        static public void UI_Connect_Server()
        {
            try
            {
                UI_listener = new TcpListener(IPAddress.Parse(UWP_Port_addr), UWP_Port_port);
                UI_listener.Start();
                Console.WriteLine("Try to listen to port: " + UI_listener.LocalEndpoint.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Server Start: " + ex.ToString());
            }
            while (true)
            {
                try
                {
                    UI_client = UI_listener.AcceptTcpClient();
                    if (UI_client.Connected)
                    {
                        Console.WriteLine("You are connected to TC: " + UI_client.Client.LocalEndPoint.ToString());
                        UI_nwStreamread = UI_client.GetStream();
                        UI_nwStreamwrite = UI_client.GetStream();
                        UI_wirte_stream_connected_flag = true;
                        UI_read_bg.RunWorkerAsync();
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Client Search: " + ex.ToString());
                    UI_wirte_stream_connected_flag = false;
                    Thread.Sleep(1000);
                }
            }

        }

        #endregion

        #region ML Communication

        //Initiate Server Port
        //static public UI_Server UWP_Port = new UI_Server();   
        static public string ML_Port_addr = "127.0.0.1";
        static public int ML_Port_port = 8100;

        //Initiate TCP Listener + Client + read + write stream
        static private TcpListener ML_listener;
        static private TcpClient ML_client;
        static private NetworkStream ML_nwStreamread;
        static private NetworkStream ML_nwStreamwrite;

        static private BackgroundWorker ML_search_bg = new BackgroundWorker();
        static private BackgroundWorker ML_read_bg = new BackgroundWorker();
        static public bool ML_wirte_stream_connected_flag = false;

        //If client connected, read byte by byte
        //If not connected, stop listener, search, read.   Stop for 2 second, restart search and read thread.
        static private void ML_Reading()
        {
            byte[] result = new byte[1];
            int bytesRead = 0;

            while (true)
            {
                try
                {
                    if (ML_client.Connected)
                    {
                        try
                        {
                            bytesRead = ML_nwStreamread.Read(result, 0, 1);
                            int i = 0;
                            if (i < bytesRead)
                            {
                                ML_Packet_receiver(result[i]);
                                i++;
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            ML_wirte_stream_connected_flag = false;
                            Console.WriteLine("Restart server port: " + ML_listener.LocalEndpoint.ToString());

                            ML_listener.Stop();
                            ML_search_bg.Dispose();
                            ML_read_bg.Dispose();
                            Thread.Sleep(2000);


                            ML_search_bg = new BackgroundWorker();
                            ML_read_bg = new BackgroundWorker();
                            ML_search_bg.DoWork += new DoWorkEventHandler(ML_search_bg_DoWork);
                            ML_search_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ML_search_bg_RunWorkerCompleted);
                            ML_search_bg.RunWorkerAsync();

                            ML_read_bg.DoWork += new DoWorkEventHandler(ML_read_bg_DoWork);
                            ML_read_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ML_read_bg_RunWorkerCompleted);

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

        static private int ML_Packet_receiver_index = 0;
        static private int ML_Packet_len = 0;

        static private bool ML_Read_done = false;
        static private byte ML_device = 0;
        static private List<byte> ML_Packet_receiver_result = new List<byte>();

        static private byte ML_Packet_command = 0x00;


        //Store byte in Packet_receiver_result;
        //Byte index tracked in Packet_receiver_index, start from 1;
        //Index 1, device
        //Index 2, Packet len, MSB
        //Index 3, Len, LSB
        //Index 4, Command
        //Index len + 7, device, match for correction
        static private void ML_Packet_receiver(byte x)
        {
            //Console.WriteLine(x.ToString());
            ML_Packet_receiver_result.Add(x);
            ML_Packet_receiver_index++;
            if (ML_Packet_receiver_index == 1)
            {
                ML_device = x;
                ML_Packet_len = 0;
            } //device
            else if (ML_Packet_receiver_index == 2)
            {
                ML_Packet_len = x;
            }//length MS
            else if (ML_Packet_receiver_index == 3)
            {
                ML_Packet_len = ML_Packet_len * 255 + x;
                Console.WriteLine("Packet Length: " + ML_Packet_len.ToString());
            } // length LS
            else if (ML_Packet_receiver_index == 4)
            {
                ML_Packet_command = x;
            }
            else if (ML_Packet_receiver_index == ML_Packet_len + 7)
            {
                if (ML_device != x)
                {
                    Console.WriteLine("Error: " + BitConverter.ToString(ML_Packet_receiver_result.ToArray()));
                }
                else
                {
                    if (ML_Packet_command == 0x10 && ML_wirte_stream_connected_flag)
                    {
                        Console.WriteLine("Send to Matlab: " + ML_Packet_receiver_result.Count.ToString());
                        ML_nwStreamwrite.Write(ML_Packet_receiver_result.ToArray(), 0, ML_Packet_receiver_result.Count);
                        ML_nwStreamwrite.Flush();
                    }
                    Console.WriteLine("Received: " + BitConverter.ToString(ML_Packet_receiver_result.ToArray()));
                    ML_Packet_receiver_result[3] = 0x0F;
                    ML_nwStreamread.Write(ML_Packet_receiver_result.ToArray(), 0, ML_Packet_receiver_result.Count);
                    ML_nwStreamread.Flush();
                    //Console.WriteLine("Sent: " + BitConverter.ToString(Packet_receiver_result.ToArray()));
                    //requestManagement.Request(Packet_receiver_result);
                    ML_Packet_receiver_result = new List<byte>();

                }
                ML_Packet_receiver_index = 0;

            }
            else
            {

            }


            if (ML_Read_done)
            {

            }
        }



        static private void ML_search_bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Search Completed, tid " + Thread.CurrentThread.ManagedThreadId);
        }

        static private void ML_search_bg_DoWork(object sender, DoWorkEventArgs e)
        {
            ML_Connect_Server();
        }

        static private void ML_read_bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Read exits, tid " + Thread.CurrentThread.ManagedThreadId);
            //done = true;
        }

        static private void ML_read_bg_DoWork(object sender, DoWorkEventArgs e)
        {
            ML_Reading();
        }



        //New Listener with port addr and port, Listener start
        //try accept client
        //If connected, set UI_nwStreamread, UI_nwStreamwrite
        //Set UI_wirte_stream_connected_flag to true;
        static public void ML_Connect_Server()
        {
            try
            {
                ML_listener = new TcpListener(IPAddress.Parse(ML_Port_addr), ML_Port_port);
                ML_listener.Start();
                Console.WriteLine("Try to listen to port: " + ML_listener.LocalEndpoint.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Server Start: " + ex.ToString());
            }
            while (true)
            {
                try
                {
                    ML_client = ML_listener.AcceptTcpClient();
                    if (ML_client.Connected)
                    {
                        Console.WriteLine("You are connected to TC: " + ML_client.Client.LocalEndPoint.ToString());
                        ML_nwStreamread = ML_client.GetStream();
                        ML_nwStreamwrite = ML_client.GetStream();
                        ML_wirte_stream_connected_flag = true;
                        ML_read_bg.RunWorkerAsync();
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Client Search: " + ex.ToString());
                    ML_wirte_stream_connected_flag = false;
                    Thread.Sleep(1000);
                }
            }

        }

        #endregion

        #region ML Communication

        //Initiate Server Port
        //static public UI_Server UWP_Port = new UI_Server();   
        static public string UN_Port_addr = "127.0.0.1";
        static public int UN_Port_port = 8300;

        //Initiate TCP Listener + Client + read + write stream
        static private TcpListener UN_listener;
        static private TcpClient UN_client;
        static private NetworkStream UN_nwStreamread;
        static private NetworkStream UN_nwStreamwrite;

        static private BackgroundWorker UN_search_bg = new BackgroundWorker();
        static private BackgroundWorker UN_read_bg = new BackgroundWorker();
        static public bool UN_wirte_stream_connected_flag = false;

        //If client connected, read byte by byte
        //If not connected, stop listener, search, read.   Stop for 2 second, restart search and read thread.
        static private void UN_Reading()
        {
            byte[] result = new byte[1];
            int bytesRead = 0;

            while (true)
            {
                try
                {
                    if (UN_client.Connected)
                    {
                        try
                        {
                            bytesRead = UN_nwStreamread.Read(result, 0, 1);
                            int i = 0;
                            if (i < bytesRead)
                            {
                                UN_Packet_receiver(result[i]);
                                i++;
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            UN_wirte_stream_connected_flag = false;
                            Console.WriteLine("Restart server port: " + UN_listener.LocalEndpoint.ToString());

                            UN_listener.Stop();
                            UN_search_bg.Dispose();
                            UN_read_bg.Dispose();
                            Thread.Sleep(2000);


                            UN_search_bg = new BackgroundWorker();
                            UN_read_bg = new BackgroundWorker();
                            UN_search_bg.DoWork += new DoWorkEventHandler(UN_search_bg_DoWork);
                            UN_search_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(UN_search_bg_RunWorkerCompleted);
                            UN_search_bg.RunWorkerAsync();

                            UN_read_bg.DoWork += new DoWorkEventHandler(UN_read_bg_DoWork);
                            UN_read_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(UN_read_bg_RunWorkerCompleted);

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

        static private int UN_Packet_receiver_index = 0;
        static private int UN_Packet_len = 0;

        static private bool UN_Read_done = false;
        static private byte UN_device = 0;
        static private List<byte> UN_Packet_receiver_result = new List<byte>();

        static private byte UN_Packet_command = 0x00;


        //Store byte in Packet_receiver_result;
        //Byte index tracked in Packet_receiver_index, start from 1;
        //Index 1, device
        //Index 2, Packet len, MSB
        //Index 3, Len, LSB
        //Index 4, Command
        //Index len + 7, device, match for correction
        static private void UN_Packet_receiver(byte x)
        {
            //Console.WriteLine(x.ToString());
            UN_Packet_receiver_result.Add(x);
            UN_Packet_receiver_index++;
            if (UN_Packet_receiver_index == 1)
            {
                UN_device = x;
                UN_Packet_len = 0;
            } //device
            else if (UN_Packet_receiver_index == 2)
            {
                UN_Packet_len = x;
            }//length MS
            else if (UN_Packet_receiver_index == 3)
            {
                UN_Packet_len = UN_Packet_len * 255 + x;
                Console.WriteLine("Packet Length: " + UN_Packet_len.ToString());
            } // length LS
            else if (UN_Packet_receiver_index == 4)
            {
                UN_Packet_command = x;
            }
            else if (UN_Packet_receiver_index == UN_Packet_len + 7)
            {
                if (UN_device != x)
                {
                    Console.WriteLine("Error: " + BitConverter.ToString(UN_Packet_receiver_result.ToArray()));
                }
                else
                {
                    if (UN_Packet_command == 0x10 && ML_wirte_stream_connected_flag)
                    {
                        Console.WriteLine("Send to Matlab: " + ML_Packet_receiver_result.Count.ToString());
                        UN_nwStreamwrite.Write(ML_Packet_receiver_result.ToArray(), 0, ML_Packet_receiver_result.Count);
                        UN_nwStreamwrite.Flush();
                    }
                    Console.WriteLine("Received: " + BitConverter.ToString(UN_Packet_receiver_result.ToArray()));
                    UN_Packet_receiver_result[3] = 0x0F;
                    UN_nwStreamread.Write(UN_Packet_receiver_result.ToArray(), 0, UN_Packet_receiver_result.Count);
                    UN_nwStreamread.Flush();
                    //Console.WriteLine("Sent: " + BitConverter.ToString(Packet_receiver_result.ToArray()));
                    //requestManagement.Request(Packet_receiver_result);
                    UN_Packet_receiver_result = new List<byte>();

                }
                UN_Packet_receiver_index = 0;

            }
            else
            {

            }


            if (ML_Read_done)
            {

            }
        }



        static private void UN_search_bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Search Completed, tid " + Thread.CurrentThread.ManagedThreadId);
        }

        static private void UN_search_bg_DoWork(object sender, DoWorkEventArgs e)
        {
            UN_Connect_Server();
        }

        static private void UN_read_bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Read exits, tid " + Thread.CurrentThread.ManagedThreadId);
            //done = true;
        }

        static private void UN_read_bg_DoWork(object sender, DoWorkEventArgs e)
        {
            UN_Reading();
        }



        //New Listener with port addr and port, Listener start
        //try accept client
        //If connected, set UI_nwStreamread, UI_nwStreamwrite
        //Set UI_wirte_stream_connected_flag to true;
        static public void UN_Connect_Server()
        {
            try
            {
                UN_listener = new TcpListener(IPAddress.Parse(UN_Port_addr), UN_Port_port);
                UN_listener.Start();
                Console.WriteLine("Try to listen to port: " + UN_listener.LocalEndpoint.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Server Start: " + ex.ToString());
            }
            while (true)
            {
                try
                {
                    UN_client = UN_listener.AcceptTcpClient();
                    if (UN_client.Connected)
                    {
                        Console.WriteLine("You are connected to TC: " + UN_client.Client.LocalEndPoint.ToString());
                        UN_nwStreamread = UN_client.GetStream();
                        UN_nwStreamwrite = UN_client.GetStream();
                        UN_wirte_stream_connected_flag = true;
                        UN_read_bg.RunWorkerAsync();
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Client Search: " + ex.ToString());
                    UN_wirte_stream_connected_flag = false;
                    Thread.Sleep(1000);
                }
            }

        }

        #endregion 
    }
}
