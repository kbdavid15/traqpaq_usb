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

        /*
         * Methods for talking to the device
         */

        private bool sendCommand(usbCommand cmd, byte[] readBuffer, byte length, byte index)
        {
            int bytesRead, bytesWritten;

            byte[] writeBuffer = { (byte)cmd, length, (byte)(index) };

            this.ec = writer.Write(writeBuffer, TIMEOUT, out bytesWritten);
            if (this.ec != ErrorCode.None)
                return false;

            this.ec = reader.Read(readBuffer, TIMEOUT, out bytesRead);
            if (this.ec != ErrorCode.None)
                return false;

            return true;
        }

        private bool sendCommand(usbCommand cmd, byte[] readBuffer)
        {
            int bytesRead, bytesWritten;

            byte[] writeBuffer = { (byte)cmd };

            this.ec = writer.Write(writeBuffer, TIMEOUT, out bytesWritten);
            if (this.ec != ErrorCode.None)
                return false;

            this.ec = reader.Read(readBuffer, TIMEOUT, out bytesRead);
            if (this.ec != ErrorCode.None)
                return false;
            return true;
        }

        private bool sendCommand(usbCommand cmd, byte[] readBuffer, params byte[] args)
        {
            int bytesRead, bytesWritten;
            byte[] writeBuffer = new byte[args.Length + 1];
            writeBuffer[0] = (byte)cmd;
            for (int i = 0; i < args.Length; i++)
            {
                writeBuffer[i+1] = args[i];
            }

            this.ec = writer.Write(writeBuffer, TIMEOUT, out bytesWritten);
            if (this.ec != ErrorCode.None)
                return false;

            this.ec = reader.Read(readBuffer, TIMEOUT, out bytesRead);
            if (this.ec != ErrorCode.None)
                return false;

            return true;
        }


        /// <summary>
        /// Sends the command to the device asking for the software versions
        /// </summary>
        /// <returns>String with the software major and minor versions</returns>
        public string get_sw_version()
        {
            byte[] sw_version = new byte[2];
            if (sendCommand(usbCommand.USB_CMD_REQ_APPL_VER, sw_version))
                return sw_version[0] + "." + sw_version[1];
            else
                return "";
        }

        public byte[] read_recordtable()
        {
            byte[] readBuffer = new byte[16];
            sendCommand(usbCommand.USB_CMD_READ_RECORDTABLE, readBuffer, 16, 0);
            return readBuffer;
        }

    }
}
