using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using LibUsbDotNet.Info;

namespace traqpaq_GUI
{
    class traqpaqDevice
    {
        int pid, vid;   // use to set the PID and VID for the USB device.this will be used to locate the device
        public static UsbDevice MyUSBDevice;
        public static UsbDeviceFinder traqpaqDeviceFinder;
        UsbEndpointReader reader;
        UsbEndpointWriter writer;
        ErrorCode ec = ErrorCode.None;


        public traqpaqDevice()
        {
            // class constructor
            this.pid = 0x1000;
            this.vid = 0xAAAA;

            // find the device
            traqpaqDeviceFinder = new UsbDeviceFinder(this.vid, this.pid);

            // open the device
            MyUSBDevice = UsbDevice.OpenUsbDevice(traqpaqDeviceFinder);

            // If the device is open and ready
            if (MyUSBDevice == null) throw new Exception("Device Not Found.");

            this.reader = MyUSBDevice.OpenEndpointReader(ReadEndpointID.Ep01);
            this.writer = MyUSBDevice.OpenEndpointWriter(WriteEndpointID.Ep02);

        }

        // Methods for talking to the device

        bool sendCommand(byte cmd, byte length, byte index)
        {
            int bytesRead, bytesWritten, timeout;
            timeout = 250;  // timeout in ms
            byte[] writeBuffer = { cmd, length, (byte)(index) };
            byte[] readBuffer = new byte[length];
            
            this.ec = writer.Write(writeBuffer, timeout, out bytesWritten);
            if (this.ec != ErrorCode.None)
                return false;
            
            this.ec = reader.Read(readBuffer, timeout, out bytesRead);
            if (this.ec != ErrorCode.None)
                return false;

            Console.Write(readBuffer);



            return true;
        }

        /// <summary>
        /// Sends the command to the device asking for the software versions
        /// </summary>
        /// <returns>String with the software major and minor versions</returns>
        string get_sw_version()
        {
            string sw_version = "";
            bool error = sendCommand(USBCommand.USB_CMD_REQ_APPL_VER, 16, 0);


            return sw_version;
        }



    }
}
