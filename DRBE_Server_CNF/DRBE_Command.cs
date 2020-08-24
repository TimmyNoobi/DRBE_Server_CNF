using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRBE_Server_CNF
{
    public class DRBE_Command
    {

        public byte Device_type_byte = 0x00;
        public byte Length_ms_byte = 0x00;
        public byte Length_ls_byte = 0x00;
        public byte Command_byte = 0x00;
        public List<byte> Content_byte = new List<byte>();

        public List<byte> Create_DRBE_Header()
        {
            List<byte> result = new List<byte>();


            return result;
        }

    }
}
