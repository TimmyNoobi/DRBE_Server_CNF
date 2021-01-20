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
using System.Net.NetworkInformation;
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
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        static readonly IntPtr HWND_TOP = new IntPtr(0);
        const UInt32 SWP_FRAMECHANGED = 0x0020;  /* The frame changed: send WM_NCCALCSIZE */
        const UInt32 SWP_SHOWWINDOW = 0x0040;
        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;


        //static public Request_Management request_manager = new Request_Management();



        static private bool Matlab_onflag = false;
        static private bool AMT_onflag = true;
        static private bool Unity_onflag = true;

        static private bool UI_Write_DONE = true;

        static bool Program_close_flag = false;
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
            Unity_read_bg.DoWork += new DoWorkEventHandler(Unity_read_bg_DoWork);
            Unity_read_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Unity_read_bg_RunWorkerCompleted);
            //Unity_read_bg.RunWorkerAsync();

            FPGA_search_bg.DoWork += new DoWorkEventHandler(FPGA_search_bg_DoWork);
            FPGA_search_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(FPGA_search_bg_RunWorkerCompleted);
            FPGA_search_bg.RunWorkerAsync();

            AMT_read_bg.DoWork += new DoWorkEventHandler(AMT_read_bg_DoWork);
            AMT_read_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AMT_read_bg_RunWorkerCompleted);
            AMT_read_bg.RunWorkerAsync();

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



            //System.Diagnostics.Process.Start(@"C:\Program Files\MATLAB\R2019b\bin\matlab.exe");


            //UWP_Port.Start(UWP_Port_addr, UWP_Port_port, request_manager);
            //Thread.Sleep(500);

            //Python_Port.Start(Python_Port_addr, Python_Port_port, request_manager);
            //Thread.Sleep(500);

            //Matlab_Port.Start(Matlab_Port_addr, Matlab_Port_port, request_manager);
            //Thread.Sleep(500);

            while (!Program_close_flag)
            {
                Thread.Sleep(500);
            }
            Thread.Sleep(500);
            Console.WriteLine("Closing");
            myProcess.Kill();
            myProcess.WaitForExit();
            Environment.Exit(0);
            //return;
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


        static private List<double> GE_STATS = new List<double>();
        static private List<byte> GE_STATS_B = new List<byte>();
        static private List<byte> Total_AMT_buffer = new List<byte>();
        static int AMT_current_copy = 0;
        static Process myProcess = new Process();
        
        static private void AMT_read_bg_DoWork(object sender, DoWorkEventArgs e)
        {
            UInt16 len = 0;
            string output = "";
            int i = 0;
            string strtemp = "";
            string smode = "N";
            
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
                //myProcess.StandardInput.WriteLine("1900000000,60,500000,0.001,6,2,5,16,1,800,6,20,2,1,4,10");
                //myProcess.StandardInput.WriteLine("1900000000 60 500000 0.001 6 2 5 16 1 800 6 20 2 1 4 10");
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
                        //Console.WriteLine(output);
                        i = 0;
                        while (i < output.Length)
                        {
                            if (output[i] == '{')
                            {
                                strtemp = "";
                            }
                            else if(output[i] == '}')
                            {
                                if(strtemp == "G" && smode == "N")
                                {
                                    smode = "G";
                                    GE_STATS.Clear();
                                    GE_STATS_B.Clear();

                                }
                                else if (strtemp == "G" && smode == "G")
                                {
                                    smode = "N";
                                    len = (UInt16)GE_STATS_B.Count();
                                    GE_STATS_B.Insert(0, 0x41);
                                    GE_STATS_B.Insert(1, BitConverter.GetBytes(len)[1]);
                                    GE_STATS_B.Insert(2, BitConverter.GetBytes(len)[0]);

                                    Total_AMT_buffer.AddRange(GE_STATS_B);
                                    AMT_current_copy++;
                                    //Console.WriteLine("AMT write back: " + AMT_current_copy.ToString() + "   " + UI_amt_copy_no.ToString());
                                    if (UI_wirte_stream_connected_flag && AMT_current_copy==UI_amt_copy_no && UI_amt_copy_no > 0)
                                    {
                                        //Console.WriteLine("AMT write back inside: " + AMT_current_copy.ToString() + "   " + UI_amt_copy_no.ToString());
                                        UI_nwStreamread.Write(Total_AMT_buffer.ToArray(), 0, Total_AMT_buffer.Count);
                                        UI_nwStreamread.Flush();
                                        UI_Write_DONE = true;
                                        Total_AMT_buffer.Clear();
                                        AMT_current_copy = 0;
                                        UI_amt_copy_no = 0;
                                        //Console.WriteLine("Send to UI");
                                    }
                                }
                                else if (strtemp == "P")
                                {
                                    smode = "P";
                                }
                                else if(smode == "G")
                                {
                                    GE_STATS.Add(S_D(strtemp));
                                    //Console.WriteLine((GE_STATS.Count - 1).ToString() + ":" + GE_STATS[GE_STATS.Count - 1].ToString());
                                    GE_STATS_B.AddRange(BitConverter.GetBytes(GE_STATS[GE_STATS.Count - 1]));
                                }
                            }
                            else
                            {
                                strtemp += output[i].ToString();
                            }
                            i++;
                        }
                        //Console.WriteLine(output);
                    }
                    else
                    {
                        //Console.WriteLine("Bad");
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

        #region FPGA
        static public string FPGA_Port_addr = "192.168.1.11";

        static public int FPGA_Port_port = 7;
        static private BackgroundWorker FPGA_search_bg = new BackgroundWorker();
        static private BackgroundWorker FPGA_read_bg = new BackgroundWorker();
        static private void FPGA_search_bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Search Completed, tid " + Thread.CurrentThread.ManagedThreadId);
        }

        static public bool FPGA_Connected_flag = false;

        static public NetworkStream FPGA_Stream;
        static private void FPGA_search_bg_DoWork(object sender, DoWorkEventArgs e)
        {
            TcpClient client;
            IPGlobalProperties ipGlobalProperties;
            TcpConnectionInformation[] tcpConnInfoArray;
            int port = 7; //<--- This is your value
            while (true)
            {
                if(!FPGA_Connected_flag)
                {
                    try
                    {
                        Console.WriteLine("Try To Connect FPGA");
                        client = new TcpClient(FPGA_Port_addr, FPGA_Port_port);
                        Console.WriteLine("FPGA Connect Success:  " + client.Connected.ToString());
                        FPGA_Connected_flag = client.Connected;
                        FPGA_Stream = client.GetStream();

                        FPGA_read_bg.DoWork += new DoWorkEventHandler(FPGA_read_bg_DoWork);
                        FPGA_read_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(FPGA_read_bg_RunWorkerCompleted);
                        FPGA_read_bg.RunWorkerAsync();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: FPGA not Found");
                        Thread.Sleep(1000);


                    }
                }else
                {
                    Thread.Sleep(500);
                }

            }
        }


        static private void FPGA_read_bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("FPGA Read Completed, tid " + Thread.CurrentThread.ManagedThreadId);
        }
        static private int FPGA_mode = 0;
        static private List<byte> FPGA_Sc_Update_Data = new List<byte>();
        static private void FPGA_read_bg_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                try
                {
                    Byte[] data = new Byte[256];
                    String responseData = String.Empty;
                    Int32 bytes = FPGA_Stream.Read(data, 0, data.Length);

                    //responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    //Console.WriteLine("Received: {0}", responseData);

                    List<byte> rdata = new List<byte>(data);

                    FPGA_Sc_Updater(rdata.GetRange(0,bytes));
                    
                }
                catch(Exception ex)
                {
                    FPGA_Connected_flag = false;
                    Console.WriteLine(ex.ToString());
                    break;
                }
            }
        }
        static int FPGA_Sc_Update_Delay = 1000;
        static private void FPGA_Sc_Updater(List<byte> x)
        {
            int len = 0;
            int i = 0;
            FPGA_Sc_Update_Data.AddRange(x);
            if(FPGA_Sc_Update_Data.Count>=3)
            {
                len = FPGA_Sc_Update_Data[1] * 255 + FPGA_Sc_Update_Data[2];
                if(FPGA_Sc_Update_Data.Count>= len)
                {
                    Console.WriteLine("Received a frame: " + BitConverter.ToString(FPGA_Sc_Update_Data.ToArray()));
                    i = 4;
                    Console.Write("Convert to double: " );
                    while (i<FPGA_Sc_Update_Data.Count)
                    {
                        Console.Write(BitConverter.ToDouble(FPGA_Sc_Update_Data.ToArray(), i) + " , ");
                        i += 8;
                    }
                    Console.WriteLine("");
                    if (FPGA_mode == 1)
                    {
                        FPGA_Stream.Write(FPGA_Sc_Update_Data.ToArray(), 0, FPGA_Sc_Update_Data.Count);
                        Console.WriteLine("Send FPGA update ");
                        UN_nwStreamread.Write(FPGA_Sc_Update_Data.ToArray(), 0, FPGA_Sc_Update_Data.Count);
                        UN_nwStreamread.Flush();

                        Thread.Sleep(FPGA_Sc_Update_Delay);
                    }else if(FPGA_mode == 2)
                    {
                        //FPGA_Stream.Write(FPGA_Sc_Update_Data.ToArray(), 0, FPGA_Sc_Update_Data.Count);
                        //Console.WriteLine("Send FPGA update ");
                        FPGA_mode = 0;
                    }
                    FPGA_Sc_Update_Data.Clear();
                }
            }


        }
        //static void FPGAConnect(String server, String message)
        //{
        //    try
        //    {
        //        // Create a TcpClient.
        //        // Note, for this client to work you need to have a TcpServer
        //        // connected to the same address as specified by the server, port
        //        // combination.
        //        Int32 port = 7;
        //        TcpClient client = new TcpClient(server, port);

        //        // Translate the passed message into ASCII and store it as a Byte array.
        //        Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

        //        // Get a client stream for reading and writing.
        //        //  Stream stream = client.GetStream();

        //        NetworkStream stream = client.GetStream();

        //        // Send the message to the connected TcpServer.
        //        stream.Write(data, 0, data.Length);

        //        Console.WriteLine("Sent: {0}", message);

        //        // Receive the TcpServer.response.

        //        // Buffer to store the response bytes.
        //        data = new Byte[256];

        //        // String to store the response ASCII representation.
        //        String responseData = String.Empty;

        //        // Read the first batch of the TcpServer response bytes.
        //        Int32 bytes = stream.Read(data, 0, data.Length);
        //        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
        //        Console.WriteLine("Received: {0}", responseData);

        //        // Close everything.
        //        stream.Close();
        //        client.Close();
        //    }
        //    catch (ArgumentNullException e)
        //    {
        //        Console.WriteLine("ArgumentNullException: {0}", e);
        //    }
        //    catch (SocketException e)
        //    {
        //        Console.WriteLine("SocketException: {0}", e);
        //    }

        //    Console.WriteLine("\n Press Enter to continue...");
        //    Console.Read();
        //}
        #endregion

        #region Unity
        static private BackgroundWorker Unity_read_bg = new BackgroundWorker();
        static private void Unity_read_bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Search Completed, tid " + Thread.CurrentThread.ManagedThreadId);
        }


        static private List<double> Unity_STATS = new List<double>();
        static private List<byte> Unity_STATS_B = new List<byte>();
        static Process UNProcess;
        static private void Unity_read_bg_DoWork(object sender, DoWorkEventArgs e)
        {


            int i = 0;

            UNProcess = new Process();
            UNProcess.StartInfo = new ProcessStartInfo(@"C:\Users\timli\TimT1\WTestbuild\TimT1.exe");

            UNProcess.StartInfo.RedirectStandardInput = true;
            //myProcess.StartInfo.WorkingDirectory = workingDirectory;
            //myProcess.StartInfo.FileName = programFilePath;
            UNProcess.StartInfo.Arguments = "-popupwindow";
            UNProcess.StartInfo.UseShellExecute = false;
            UNProcess.StartInfo.CreateNoWindow = false;
            UNProcess.StartInfo.RedirectStandardOutput = true;
            UNProcess.StartInfo.RedirectStandardError = true;

            try
            {
                UNProcess.Start();
                
                while (UNProcess.MainWindowHandle == IntPtr.Zero)
                {

                    Thread.Sleep(20);

                }
                //SetWindowPos(myProcess.MainWindowHandle, HWND_TOPMOST, 0, 0, 500, 500, 0x0200);
                //Console.WriteLine("Window Changed \r\n \r\n");
                while (Unity_on_flag)
                {
                    SetWindowPos(UNProcess.MainWindowHandle, HWND_TOPMOST, 10, 175, 1520, 600, 0x0200);
                //    Console.WriteLine("Window Changed \r\n \r\n");
                    Thread.Sleep(500);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString() + "\r\n \r\n");
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

                            Console.WriteLine("Restart server port UI: " + UI_listener.LocalEndpoint.ToString());

                            Program_close_flag = true;

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

        static int UI_Scan_No = 0;
        static int UI_amt_copy_no = 0;
        //Store byte in Packet_receiver_result;
        //Byte index tracked in Packet_receiver_index, start from 1;
        //Index 1, device
        //Index 2, Packet len, MSB
        //Index 3, Len, LSB
        //Index 4, Command
        //Index len + 7, device, match for correction

        static bool Unity_on_flag = false;
        static private void UI_Packet_receiver(byte x)
        {
            int i = 0;
            string temp = "";
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
            else if((UI_Packet_receiver_index == UI_Packet_len)  && (UI_device == 0x02))
            {
                if(UI_Packet_command== 0x00)
                {
                    UI_Scan_No++;
                }else if(UI_Packet_command == 0x01)
                {
                    UI_amt_copy_no = UI_Scan_No + 1;
                    UI_Scan_No = 0;
                }
                i = 4;
                while(i<UI_Packet_receiver_result.Count)
                {
                    temp += BitConverter.ToDouble(UI_Packet_receiver_result.GetRange(i,8).ToArray(),0).ToString() + " ";
                    i += 8;
                }
                myProcess.StandardInput.WriteLine(temp);
                Console.WriteLine("Send to AMT: " + temp);
                UI_Packet_receiver_result = new List<byte>();
                temp = "";
                UI_Packet_receiver_index = 0;
            }
            else if ((UI_Packet_receiver_index == UI_Packet_len) && (UI_device == 0x06))
            {
                if(UI_Packet_command == 0x01)
                {
                    Unity_on_flag = true;
                    Unity_read_bg.RunWorkerAsync();
                    Console.WriteLine("Unity Start");
                }else if(UI_Packet_command == 0x02)
                {
                    Unity_on_flag = false;
                    UNProcess.Kill();
                    UNProcess.WaitForExit();
                    //Unity_read_bg.DoWork += new DoWorkEventHandler(Unity_read_bg_DoWork);
                    //Unity_read_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Unity_read_bg_RunWorkerCompleted);
                }
                UI_Packet_receiver_index = 0;
                UI_Packet_receiver_result = new List<byte>();
            }
            else if ((UI_Packet_receiver_index == UI_Packet_len) && (UI_device == 0x07))
            {
                UN_nwStreamread.Write(UI_Packet_receiver_result.ToArray(), 0, UI_Packet_receiver_result.Count);
                UN_nwStreamread.Flush();
                Console.WriteLine("Try to send: " + BitConverter.ToString(UI_Packet_receiver_result.ToArray()));
                UI_Packet_receiver_result = new List<byte>();
                UI_Packet_receiver_index = 0;
            }
            else if ((UI_Packet_receiver_index == UI_Packet_len) && (UI_device == 0x08) && UI_Packet_command == 2)
            {
                Console.WriteLine("Start Playing");
                if(FPGA_Connected_flag)
                {
                    FPGA_Stream.Write(UI_Packet_receiver_result.ToArray(), 0, UI_Packet_receiver_result.Count);
                    FPGA_mode = 1;
                }
                UI_Packet_receiver_result.Clear();
                UI_Packet_receiver_index = 0;

            }
            else if ((UI_Packet_receiver_index == UI_Packet_len) && (UI_device == 0x08) && UI_Packet_command == 3)
            {
                Console.WriteLine("Stopped Playing");
                FPGA_mode = 0;
                UI_Packet_receiver_result.Clear();
                UI_Packet_receiver_index = 0;

            }
            else if ((UI_Packet_receiver_index == UI_Packet_len) && (UI_device == 0x08) && UI_Packet_command == 4)
            {
                Console.WriteLine("Set Speed: ");
                FPGA_Sc_Update_Delay = UI_Packet_receiver_result[4] * 255 + UI_Packet_receiver_result[5];
                UI_Packet_receiver_result.Clear();
                UI_Packet_receiver_index = 0;

            }
            else if ((UI_Packet_receiver_index == UI_Packet_len) && (UI_device == 0x08) && UI_Packet_command == 5)
            {
                Console.WriteLine("Send Debug Signal: " + BitConverter.ToString(UI_Packet_receiver_result.ToArray()));
                if (FPGA_Connected_flag)
                {
                    FPGA_Stream.Write(UI_Packet_receiver_result.ToArray(), 0, UI_Packet_receiver_result.Count);
                    FPGA_mode = 2;
                }
                UI_Packet_receiver_result.Clear();
                UI_Packet_receiver_index = 0;

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
                        
                        //AMT_read_bg.RunWorkerAsync();
                        
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

        #region UN Communication

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
            Console.Write(x.ToString());
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
            } // length LS
            else if (UN_Packet_receiver_index == 4)
            {
                UN_Packet_command = x;
            }
            else if (UN_Packet_receiver_index == UN_Packet_len + 7)
            {

                UN_Packet_receiver_index = 0;
            }
            else
            {

            }

            if(UN_Packet_receiver_index == 3 && UN_device == 0x22)
            {
                UI_nwStreamread.Write(UN_Packet_receiver_result.ToArray(), 0, UN_Packet_receiver_result.Count);
                UI_nwStreamread.Flush();
                Console.WriteLine("Packet sent: " + BitConverter.ToString(UN_Packet_receiver_result.ToArray()));
                UN_Packet_receiver_result.Clear();
                UN_Packet_receiver_index = 0;
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
                //UN_listener = new TcpListener(IPAddress.Any, 7);
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

        #region others
        static private List<byte> D_Fixed(double x, int ger, int fra)
        {
            List<byte> result = new List<byte>();

            int i = 0;
            int dger = (int)x;
            double dfra = x - (int)x;
            UInt32 temp = 0;
            int refger = (int)Math.Pow(2, ger);
            double reffra = 1 / 2;

            while (i < ger)
            {
                if (dger >= refger)
                {
                    temp += 1;
                    dger = dger - refger;
                }
                temp = temp << 1;
                refger = refger / 2;
                i++;
            }
            i = 0;
            while (i < fra)
            {
                if (dfra > reffra)
                {
                    temp += 1;
                    dfra -= reffra;
                    reffra /= 2;
                }

                temp = temp << 1;
                i++;
            }

            result = new List<byte>(BitConverter.GetBytes(temp));


            return result;
        }

        static private double D_Fixed_d(double x, int ger, int fra)
        {
            double result = 0;

            int i = 0;
            int dger = (int)x;
            double dfra = x - (int)x;
            UInt32 temp = 0;
            int refger = (int)Math.Pow(2, ger);
            double reffra = 1 / 2;

            while (i < ger)
            {
                if (dger >= refger)
                {
                    temp += 1;
                    dger = dger - refger;
                }
                temp = temp << 1;
                refger = refger / 2;
                i++;
            }
            i = 0;
            while (i < fra)
            {
                if (dfra > reffra)
                {
                    temp += 1;
                    dfra -= reffra;
                    reffra /= 2;
                }

                temp = temp << 1;
                i++;
            }

            result = temp;


            return result;
        }
        static private byte S_B(string x)
        {
            byte result = 0;
            int temp = S_H(x);
            result = BitConverter.GetBytes(temp)[0];
            //ShowDialog(BitConverter.ToString(BitConverter.GetBytes(temp)), result.ToString());
            return result;
        }
        static private double S_D(string x)
        {
            double result = 0;
            double sign = 1;
            string before = "";
            string after = "";
            int i = 0;
            int tenpower = 1;
            int len = x.Length;
            int beforeflag = 0;
            if (len >= 1)
            {
                if (x[0] == '-')
                {
                    sign = -1;
                }
            }
            while (i < len)
            {
                if (beforeflag == 0 && x[i] != '.')
                {
                    before += x[i].ToString();
                }
                else if (beforeflag == 1 && x[i] != '.')
                {
                    after += x[i].ToString();
                    tenpower = tenpower * 10;
                }
                else if (x[i] == '.')
                {
                    beforeflag = 1;
                }
                else if (x[i] == '-')
                {
                    //sign = -1;
                }
                else
                {
                    //sign = -1;
                }
                i++;
            }
            result = S_I_vd(before) + (S_I_vd(after)) / tenpower;
            result = result * sign;
            return result;
        }
        static private double S_I_vd(string x)
        {
            double result = 0;
            int index = 0;
            int rindex = 0;
            index = x.Length;
            while (index > 0)
            {
                if (C_I(x[rindex]) != -1)
                {
                    result = result * 10 + C_I(x[rindex]);
                }
                else
                {

                }

                rindex++;
                index--;
            }
            return result;
        }
        static private int S_I(string x)
        {
            int result = 0;
            int index = 0;
            int rindex = 0;
            index = x.Length;
            while (index > 0)
            {
                if (C_I(x[rindex]) != -1)
                {
                    result = result * 10 + C_I(x[rindex]);
                }
                else
                {

                }

                rindex++;
                index--;
            }
            return result;
        }
        static private int C_I(char x)
        {
            int reint = 0;
            if (x == '0')
            {
                reint = 0;
            }
            else if (x == '1')
            {
                reint = 1;
            }
            else if (x == '2')
            {
                reint = 2;

            }
            else if (x == '3')
            {
                reint = 3;
            }
            else if (x == '4')
            {
                reint = 4;
            }
            else if (x == '5')
            {
                reint = 5;
            }
            else if (x == '6')
            {
                reint = 6;
            }
            else if (x == '7')
            {
                reint = 7;
            }
            else if (x == '8')
            {
                reint = 8;
            }
            else if (x == '9')
            {
                reint = 9;
            }
            else
            {
                reint = -1;
            }
            return reint;
        }
        static private int S_H(string x)
        {
            int result = 0;
            int index = 0;
            int rindex = 0;
            index = x.Length;
            while (index > 0)
            {
                if (C_H(x[rindex]) != -1)
                {
                    result = result * 16 + C_H(x[rindex]);
                }
                else
                {

                }

                rindex++;
                index--;
            }
            return result;
        }
        static private int C_H(char x)
        {
            int reint = 0;
            if (x == '0')
            {
                reint = 0;
            }
            else if (x == '1')
            {
                reint = 1;
            }
            else if (x == '2')
            {
                reint = 2;

            }
            else if (x == '3')
            {
                reint = 3;
            }
            else if (x == '4')
            {
                reint = 4;
            }
            else if (x == '5')
            {
                reint = 5;
            }
            else if (x == '6')
            {
                reint = 6;
            }
            else if (x == '7')
            {
                reint = 7;
            }
            else if (x == '8')
            {
                reint = 8;
            }
            else if (x == '9')
            {
                reint = 9;
            }
            else if (x == 'a' || x == 'A')
            {
                reint = 10;
            }
            else if (x == 'b' || x == 'B')
            {
                reint = 11;
            }
            else if (x == 'c' || x == 'C')
            {
                reint = 12;
            }
            else if (x == 'd' || x == 'D')
            {
                reint = 13;
            }
            else if (x == 'e' || x == 'E')
            {
                reint = 14;
            }
            else if (x == 'f' || x == 'F')
            {
                reint = 15;
            }
            else
            {
                reint = -1;
            }
            return reint;
        }
        #endregion
    }
}
