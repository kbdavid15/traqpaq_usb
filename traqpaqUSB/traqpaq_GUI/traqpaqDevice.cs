﻿using System;
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
        public Battery battery;
        public SavedTrackReader trackReader;

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

            // create the battery object
            this.battery = new Battery(this);

            // create a saved track reader to get a list of saved tracks
            this.trackReader = new SavedTrackReader(this);

        }

        /*************************************************
         * Methods for talking to the device             *
         * These functions should not be called directly *
         *************************************************/

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

        public class Battery
        {
            byte[] VoltageRead = new byte[2];
            byte[] TemperatureRead = new byte[2];
            byte[] InstCurrentRead = new byte[2];
            byte[] AccumCurrentRead = new byte[2];
            byte[] SetChargeStateFlagRead = new byte[1];
            
            public double Voltage { get; set; }
            public double Temperature { get; set; }
            public double CurrentInst { get; set; }
            public double CurrentAccum { get; set; }
            public bool ChargeStateFlag { get; set; }

            private TraqpaqDevice traqpaq;

            public Battery(TraqpaqDevice parent) 
            {
                this.traqpaq = parent;
            }            

            /// <summary>
            /// Request current battery voltage
            /// </summary>
            /// <returns>True if request was successful, false otherwise</returns>
            public bool reqBatteryVoltage()
            {
                if (traqpaq.sendCommand(usbCommand.USB_CMD_REQ_BATTERY_VOLTAGE, VoltageRead))
                {   // Battery voltage is a word, so concantenate the 2 bytes into an int
                    int temp = VoltageRead[0] << 8 | VoltageRead[1];
                    // convert to Volts
                    //TODO convert the voltage to volts
                    //this.Voltage = temp;
                    return true;
                }
                else return false;                    
            }

            /// <summary>
            /// Request battery temperature
            /// </summary>
            /// <returns>True if request was successful, false otherwise</returns>
            public bool reqBatteryTemp()
            {
                if (traqpaq.sendCommand(usbCommand.USB_CMD_REQ_BATTERY_TEMPERATURE, TemperatureRead))
                {
                    //TODO set the Temperature property
                    int temp = TemperatureRead[0] << 8 | TemperatureRead[1];

                    return true;
                }
                else return false;
            }

            /// <summary>
            /// Request battery instantaneous current draw
            /// </summary>
            /// <returns>True if request was successful, false otherwise</returns>
            public bool reqBatteryInstCurrent()
            {
                if (traqpaq.sendCommand(usbCommand.USB_CMD_REQ_BATTERY_INSTANT, InstCurrentRead))
                {
                    //TODO set the CurrentInst property
                    int temp = InstCurrentRead[0] << 8 | InstCurrentRead[1];
                    return true;
                }
                else return false;
            }

            /// <summary>
            /// Request battery accumulated current draw
            /// </summary>
            /// <returns>True if request was successful, false otherwise</returns>
            public bool reqBatteryAccumCurrent()
            {
                if (traqpaq.sendCommand(usbCommand.USB_CMD_REQ_BATTERY_ACCUM, AccumCurrentRead))
                {
                    //TODO set the CurrentAccum property
                    int temp = AccumCurrentRead[0] << 8 | AccumCurrentRead[1];
                    return true;
                }
                else return false;
            }

            /// <summary>
            /// Set accumulated battery current to full-charge state
            /// </summary>
            /// <returns>True if request was successful, false otherwise</returns>
            public bool setBatteryFullChargeState()
            {
                if (traqpaq.sendCommand(usbCommand.USB_CMD_REQ_BATTERY_UPDATE, SetChargeStateFlagRead))
                {
                    this.ChargeStateFlag = (SetChargeStateFlagRead[0] > 0);     //true if success
                    return true; 
                }
                else return false;
            }
        }

        public class SavedTrackReader
        {//TODO saved tracks class            
            private List<SavedTrack> trackList = new List<SavedTrack>();
            private TraqpaqDevice traqpaq;

            public SavedTrackReader(TraqpaqDevice parent)
            {
                this.traqpaq = parent;
                // get all the saved tracks and add them to the list
                getAllTracks();
            }

            /// <summary>
            /// Get all the saved tracks on the device and add them to the trackList
            /// </summary>
            private void getAllTracks()
            {
                bool isEmpty = false;
                SavedTrack track;
                short index = 0;

                while (!isEmpty)
                {   // keep reading tracks until one is empty
                    track = new SavedTrack(this, index);
                    if (track.readTrack())
                    {
                        trackList.Add(track);
                        isEmpty = track.isEmpty;
                        index++;
                    }
                    else isEmpty = false;   // track read failed
                }
            }

            //TODO use this function to write default saved track info to flash
            public void writeSavedTracks()
            {
            }

            private class SavedTrack
            {
                private byte[] trackReadBuff = new byte[32];
                SavedTrackReader parent;
                short index;
                public string trackName { get; set; }
                public int Longitute { get; set; }
                public int Latitude { get; set; }
                public short Heading { get; set; }
                public bool isEmpty { get; set; }

                public SavedTrack(SavedTrackReader parent, short index)
                {
                    this.parent = parent;
                    this.index = index;
                }
                /// <summary>
                /// Read saved track data from the device
                /// </summary>
                /// <returns>True if successful, false if there was an error</returns>
                public bool readTrack()
                {
                    byte upperIndex = (byte)((index >> 8) & 0xFF);
                    byte lowerIndex = (byte)(index & 0xFF);
                    if (parent.traqpaq.sendCommand(usbCommand.USB_CMD_READ_SAVED_TRACKS, trackReadBuff, upperIndex, lowerIndex))
                    {
                        trackName = Encoding.ASCII.GetString(trackReadBuff, 0, 20);
                        Longitute = trackReadBuff[20] << 24 | trackReadBuff[21] << 16 | trackReadBuff[22] << 8 | trackReadBuff[23];
                        Latitude = trackReadBuff[24] << 24 | trackReadBuff[25] << 16 | trackReadBuff[26] << 8 | trackReadBuff[27];
                        Heading = (short)(trackReadBuff[28] << 8 | trackReadBuff[29]);
                        isEmpty = (trackReadBuff[30] == 0xFF);  // true if empty
                        return true;
                    }
                    else return false;
                }
            }
        }

        public class RecordTable
        {   //TODO record table class
            public void readRecordTable()
            {

            }
        }

        public class RecordData
        {   //TODO record data class
            RecordTable recordTable;
            public RecordData(RecordTable recordTable)
            {
                this.recordTable = recordTable;
            }
            public void readRecordData()
            {
            }

            public void eraseAllRecordData()
            {
            }
        }
                
/*
        public byte[] writeDefaultPrefs()
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



        /*
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
        */
    }
}
