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
    public class TraqpaqDevice
    {
        public const byte OTP_SERIAL_LENGTH = 13;
        int PID, VID;   // use to set the PID and VID for the USB device.this will be used to locate the device
        public UsbDevice MyUSBDevice;
        public static UsbDeviceFinder traqpaqDeviceFinder;
        UsbEndpointReader reader;
        UsbEndpointWriter writer;
        ErrorCode ec = ErrorCode.None;
        public const int TIMEOUT = 250;  // timeout in ms

        /// <summary>
        /// Class constructor for the traq|paq
        /// </summary>
        public TraqpaqDevice()
        {
            this.PID = 0x1000;
            this.VID = 0xAAAA;

            // find the device
            traqpaqDeviceFinder = new UsbDeviceFinder(this.VID, this.PID);

            // open the device
            this.MyUSBDevice = UsbDevice.OpenUsbDevice(traqpaqDeviceFinder);

            // If the device is open and ready
            if (MyUSBDevice == null) throw new Exception("Device Not Found.");

            this.reader = MyUSBDevice.OpenEndpointReader(ReadEndpointID.Ep01);
            this.writer = MyUSBDevice.OpenEndpointWriter(WriteEndpointID.Ep02);

        }

        /*************************************
         * Methods for talking to the device *
         *************************************/

        /// <summary>
        /// Deprecated. Function sends a bulk transfer to the device.
        /// These functions should NOT be called directly. Use the 
        /// wrapper methods of this class instead.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="readBuffer">Pre allocated byte array</param>
        /// <param name="length">Length of bytes requested. Not preferred</param>
        /// <param name="index">Index command byte. Not preferred method</param>
        /// <returns>False if read or write error. True otherwise</returns>
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

        /// <summary>
        /// Send command with no command bytes in the writeBuffer
        /// </summary>
        /// <param name="cmd">USB command</param>
        /// <param name="readBuffer">Pre-allocated byte array</param>
        /// <returns>False if read or write error. True otherwise</returns>
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

        /// <summary>
        /// Send the command to the device. Preferred method for adding command bytes
        /// </summary>
        /// <param name="cmd">USB command</param>
        /// <param name="readBuffer">Pre-allocated byte array</param>
        /// <param name="commandBytes">command bytes passed in order from byte0..byte1..byte(n)
        ///                            They are appended to the writeBuffer</param>
        /// <returns>False if read or write error. True otherwise.</returns>
        private bool sendCommand(usbCommand cmd, byte[] readBuffer, params byte[] commandBytes)
        {
            int bytesRead, bytesWritten;
            byte[] writeBuffer = new byte[commandBytes.Length + 1];
            writeBuffer[0] = (byte)cmd;
            for (int i = 0; i < commandBytes.Length; i++)
            {
                writeBuffer[i+1] = commandBytes[i];
            }

            this.ec = writer.Write(writeBuffer, TIMEOUT, out bytesWritten);
            if (this.ec != ErrorCode.None)
                return false;

            this.ec = reader.Read(readBuffer, TIMEOUT, out bytesRead);
            if (this.ec != ErrorCode.None)
                return false;

            return true;
        }

        /********************************************************
         * Methods for sending commands to the device           *
         * All these methods call the sendCommand() function.   *
         * These should be used to access the device.           *
         ********************************************************/

        // not sure if I want to create a class for each type. 
        // also don't want to deal with the byte array for each function
        // this could make it cleaner, if say, I want the string version
        // of the application version, I can just say ToString()
        public class ApplicationVersion
        {
            public byte[] Value = new byte[2];
            public ApplicationVersion() { }            
            public override string ToString()
            {
                return Value[0] + "." + Value[1];
            }            
        }

        public class HardwareVersion
        {
            public byte[] Value = new byte[1];
            public HardwareVersion() { }
            public override string ToString()
            {
                return Value[0].ToString();
            }
        }

        public class SerialNumber
        {
            public byte[] Value = new byte[OTP_SERIAL_LENGTH];
            public SerialNumber() { }
            public override string ToString()
            {   // value is in ASCII
                return Encoding.ASCII.GetString(Value);
            }
        }

        public class TesterID
        {
            public byte[] Value = new byte[1];
            public TesterID() { }
            public override string ToString()
            {
                return Value[0].ToString();
            }
        }

        public class BatteryVoltage
        {
            public byte[] Value = new byte[2];
            public string hexValue { get; set; }
            public Int16 decimalValue { get; set; }
            public BatteryVoltage() { }
            public override string ToString()
            {
                // Battery voltage is a word, so concantenate the 2 bytes into an int
                int word = Value[0] << 8 | Value[1];
                this.hexValue = word.ToString();
                // convert to decimal
                this.decimalValue = Convert.ToInt16(hexValue, 16);
                return decimalValue.ToString();
            }
        }

        public class SavedTracks
        {
            //TODO saved tracks class
        }


        public ApplicationVersion reqApplicationVersion()
        {
            ApplicationVersion version = new ApplicationVersion();
            if (!sendCommand(usbCommand.USB_CMD_REQ_APPL_VER, version.Value))
                version = null;
            return version;
        }
        
        public HardwareVersion reqHardwareVersion()
        {
            HardwareVersion version = new HardwareVersion();
            if (!sendCommand(usbCommand.USB_CMD_REQ_HARDWARE_VER, version.Value))
                version = null;
            return version;
        }

        public SerialNumber reqSerialNumber()
        {
            SerialNumber sn = new SerialNumber();
            if (!sendCommand(usbCommand.USB_CMD_REQ_SERIAL_NUMBER, sn.Value))
                sn = null;
            return sn;
        }

        public TesterID reqTesterID()
        {
            TesterID tester = new TesterID();
            if (!sendCommand(usbCommand.USB_CMD_REQ_TESTER_ID, tester.Value))
                tester = null;
            return tester;
        }

        public BatteryVoltage reqBatteryVoltage()
        {
            BatteryVoltage bv = new BatteryVoltage();
            if (!sendCommand(usbCommand.USB_CMD_REQ_BATTERY_VOLTAGE, bv.Value))
                bv = null;
            return bv;
        }

        //TODO battery functions
/* More battery functions. Consider implementing all of these in one class
        public byte[] reqBatteryTemp()
        {
        }

        public byte[] reqBatteryInstCurrent()
        {
        }

        public byte[] reqBatteryAccumCurrent()
        {
        }

        public byte[] setBatteryFullChargeState()
        {
        }
 */
/*
        public byte[] readTracks()
        {
        }

        public byte[] readRecordTable()
        {

        }

        public byte[] readRecordData()
        {
        }

        public byte[] eraseAllRecordData()
        {
        }

        public byte[] writeDefaultPrefs()
        {
        }

        public byte[] writeSavedTracks()
        {
        }

        public byte[] readOTP()
        {

        }

        public byte[] writeOTP()
        {
        }

        public byte[] eraseFlash()
        {
        }

        public byte[] isFlashBusy()
        {
        }

        public byte[] eraseChip()
        {

        }

        public byte[] isFlashFull()
        {
        }

        public byte[] getFlashPercentUsed()
        {
        }

        public byte[] getLastGPS_lat()
        {
        }

        public byte[] getLastGPS_long()
        {
        }

        public byte[] getLastGPS_heading()
        {

        }
        */




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
