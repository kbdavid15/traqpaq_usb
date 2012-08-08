using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using LibUsbDotNet.Info;
using System.Windows.Forms;

namespace traqpaq_GUI
{
    public class traqpaqDevice
    {
        int PID, VID;   // use to set the PID and VID for the USB device.this will be used to locate the device
        public static UsbDevice MyUSBDevice;
        public static UsbDeviceFinder traqpaqDeviceFinder;
        UsbEndpointReader reader;
        UsbEndpointWriter writer;
        ErrorCode ec = ErrorCode.None;
        public const int TIMEOUT = 250;  // timeout in ms

        /// <summary>
        /// Class constructor for the traq|paq
        /// </summary>
        public traqpaqDevice()
        {
            this.PID = 0x1000;
            this.VID = 0xAAAA;

            // find the device
            traqpaqDeviceFinder = new UsbDeviceFinder(this.VID, this.PID);

            // open the device
            MyUSBDevice = UsbDevice.OpenUsbDevice(traqpaqDeviceFinder);

            // If the device is open and ready
            if (MyUSBDevice == null) throw new Exception("Device Not Found.");

            this.reader = MyUSBDevice.OpenEndpointReader(ReadEndpointID.Ep01);
            this.writer = MyUSBDevice.OpenEndpointWriter(WriteEndpointID.Ep02);

        }

        // Methods for talking to the device
        bool sendCommand(usbCommand cmd, byte length, byte index, out byte[] read)
        {
            int bytesRead, bytesWritten;
            
            byte[] writeBuffer = { (byte)cmd, length, (byte)(index) };
            byte[] readBuffer = new byte[length];
            read = readBuffer;

            this.ec = writer.Write(writeBuffer, TIMEOUT, out bytesWritten);
            if (this.ec != ErrorCode.None)
                return false;
            
            this.ec = reader.Read(readBuffer, TIMEOUT, out bytesRead);
            if (this.ec != ErrorCode.None)
                return false;
            read = readBuffer;
            
            return true;
        }

        private cmdReturnClass sendCommand(usbCommand cmd, byte read_length, byte index)
        {
            cmdReturnClass result;
            int bytesRead, bytesWritten;

            byte[] writeBuffer = { (byte)cmd, read_length, (byte)(index) };
            byte[] readBuffer = new byte[read_length];

            this.ec = writer.Write(writeBuffer, TIMEOUT, out bytesWritten);
            if (this.ec != ErrorCode.None)
            {
                result = new cmdReturnClass(this.ec, readBuffer);
                return result;
            }
            this.ec = reader.Read(readBuffer, TIMEOUT, out bytesRead);
            result = new cmdReturnClass(this.ec, readBuffer);
            return result;
        }

        private cmdReturnClass sendCommand(usbCommand cmd, byte write_length, byte index, byte read_length)
        {
            //TODO finish modifying this function
            cmdReturnClass result;
            int bytesRead, bytesWritten;

            byte[] writeBuffer = { (byte)cmd, write_length, (byte)(index) };
            byte[] readBuffer = new byte[read_length];

            this.ec = writer.Write(writeBuffer, TIMEOUT, out bytesWritten);
            if (this.ec != ErrorCode.None)
            {
                result = new cmdReturnClass(this.ec, readBuffer);
                return result;
            }
            this.ec = reader.Read(readBuffer, TIMEOUT, out bytesRead);
            result = new cmdReturnClass(this.ec, readBuffer);
            return result;
        }

        /// <summary>
        /// Sends the command to the device asking for the software versions
        /// </summary>
        /// <returns>String with the software major and minor versions</returns>
        internal string get_sw_version()
        {
            byte[] sw_version;
            bool error = sendCommand(usbCommand.USB_CMD_REQ_APPL_VER, 2, 0, out sw_version);
            return sw_version[0] + "." + sw_version[1];
             
        }
    }
}
