using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using LibUsbDotNet.Info;

namespace traqpaq_GUI
{
    /// <summary>
    /// Class for the return value of the send_command function
    /// Contains the ErrorCode and the read register
    /// </summary>
    public class cmdReturnClass
    {
        ErrorCode errorCode;
        byte[] readBuffer;

        public cmdReturnClass(ErrorCode ec, byte[] readBuffer)
        {
            this.errorCode = ec;
            this.readBuffer = readBuffer;
        }
    }
}
