using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRBE_Server_CNF
{
    public class Request_Management
    {
        private readonly object requestLock = new object();
        private decimal balance;
        private List<List<byte>> Requests = new List<List<byte>>();
        public void Request(List<byte> x)
        {

            lock (requestLock)
            {
                Requests.Add(new List<byte>(x));
                Console.WriteLine("Request Issued: " + BitConverter.ToString(x.ToArray()));
                while (Requests.Count > 0)
                {
                    Console.WriteLine("Request Executed: " + BitConverter.ToString(Requests[0].ToArray()));
                    Requests.RemoveAt(0);
                }
            }
        }
    }
}
