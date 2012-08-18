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
        #region Constants
        public const byte OTP_SERIAL_LENGTH = 13;
        public const byte RECORD_DATA_PER_PAGE = 15;
        public const double BATT_VOLTAGE_FACTOR = 0.00488;       // (Volts)
        public const double BATT_TEMP_FACTOR = 0.125;            // (°Celsius)
        public const double BATT_INST_CURRENT_FACTOR = 0.625;    // (mAmps)
        public const double BATT_ACCUM_CURRENT_FACTOR = 0.25;    // (mAmp hours)
        public const double SPEED_FACTOR = 0.5144;               // (meters/second)
        public const int TIMEOUT = 250;                          // usb timeout in ms
        //TODO see if the bit converter function Dad found works better than LSLs and LSRs
        // then add more constants, similar to Ryan's memory map, in a separate class
        #endregion

        int PID, VID;   // use to set the PID and VID for the USB device. will be used to locate the device
        public UsbDevice MyUSBDevice;
        public static UsbDeviceFinder traqpaqDeviceFinder;
        UsbEndpointReader reader;
        UsbEndpointWriter writer;
        ErrorCode ec = ErrorCode.None;        
        public Battery battery;
        public SavedTrackReader trackReader;
        public List<TraqpaqDevice.SavedTrackReader.SavedTrack> trackList { get; set; }
        RecordTableReader tableReader;
        public List<RecordTableReader.RecordTable> recordTableList;
        public RecordDataReader dataReader;
        public OTPreader myOTPreader { get; set; }

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

            // create the otp reader object
            this.myOTPreader = new OTPreader(this);

            // create a saved track reader to get a list of saved tracks
            this.trackReader = new SavedTrackReader(this);
            this.trackList = trackReader.trackList;

            // get the record table data
            tableReader = new RecordTableReader(this);
            tableReader.readRecordTable();
            this.recordTableList = tableReader.recordTable;

            //TODO figure out how to use the record data reader class

        }

        #region sendCommand
        /*************************************************
         * Methods for talking to the device             *
         * sendCommand() should not be called directly   *
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
        
        private bool readRecordData(byte[] readBuffer, ushort length, ushort index)
        {
            int bytesRead, bytesWritten;
            byte[] writeBuffer = { (byte)usbCommand.USB_CMD_READ_RECORDDATA, (byte)((length >> 8) & 0xFF),
                                     (byte)(length & 0xFF), (byte)((index >> 8) & 0xFF), (byte)(index & 0xFF) };

            this.ec = writer.Write(writeBuffer, TIMEOUT, out bytesWritten);
            if (this.ec != ErrorCode.None)
                return false;
            // now read the response 64 bytes at a time
            int i = 0;
            while (i < length)
            {
                this.ec = reader.Read(readBuffer, i, 64, TIMEOUT, out bytesRead);
                if (this.ec != ErrorCode.None)
                    return false;
                i += 64;
            }
            return true;
        }
        #endregion

        #region tFlashOTP
        /********************************************************
         * Methods for sending commands to the device           *
         * All these methods call the sendCommand() function.   *
         * These should be used to access the device.           *
         ********************************************************/
        public class OTPreader
        {
            private TraqpaqDevice parent;
            public string ApplicationVersion { get; set; }
            public string HardwareVersion { get; set; }
            public string SerialNumber { get; set; }
            public string TesterID { get; set; }

            public OTPreader(TraqpaqDevice parent) { this.parent = parent; }

            public bool reqApplicationVersion()
            {
                byte[] readBuff = new byte[2];
                if (parent.sendCommand(usbCommand.USB_CMD_REQ_APPL_VER, readBuff))
                {
                    this.ApplicationVersion = readBuff[0] + "." + readBuff[1];
                    return true;
                }
                else return false;
            }

            public bool reqHardwareVersion()
            {
                byte[] readBuff = new byte[1];
                if (parent.sendCommand(usbCommand.USB_CMD_REQ_HARDWARE_VER, readBuff))
                {
                    this.HardwareVersion = readBuff[0].ToString();
                    return true;
                }
                else return false;
            }

            public bool reqSerialNumber()
            {
                byte[] readBuff = new byte[OTP_SERIAL_LENGTH];
                if (parent.sendCommand(usbCommand.USB_CMD_REQ_SERIAL_NUMBER, readBuff))
                {
                    this.SerialNumber = Encoding.ASCII.GetString(readBuff);
                    return true;
                }
                else return false;
            }

            /// <summary>
            /// Request end-of-line Tester ID
            /// </summary>
            /// <returns></returns>
            public bool reqTesterID()
            {
                byte[] readBuff = new byte[1];
                if (parent.sendCommand(usbCommand.USB_CMD_REQ_TESTER_ID, readBuff))
                {
                    this.TesterID = readBuff[0].ToString();
                    return true;
                }
                else return false;
            }

            /// <summary>
            /// Read specified bytes from flash OTP
            /// </summary>
            /// <param name="length">Number of bytes to read</param>
            /// <param name="index">Byte index to start reading from</param>
            /// <returns></returns>
            public byte[] readOTP(byte length, byte index)
            {
                byte[] readBuff = new byte[length];
                if (parent.sendCommand(usbCommand.USB_CMD_READ_OTP, readBuff, length, index))
                {
                    //TODO figure out how to return this value
                    return readBuff;
                }
                return readBuff;
            }

            /// <summary>
            /// Write fixed data in flash OTP
            /// </summary>
            public void writeOTP()
            {
                byte[] readBuff = new byte[256]; // don't know how big to make the buffer
                if (parent.sendCommand(usbCommand.USB_CMD_WRITE_OTP, readBuff))
                {
                    //TODO learn more about this function
                }                
            }
        }
        #endregion

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
                {   // Battery voltage is a word, so concantenate the 2 bytes
                    // convert to Volts
                    //this.Voltage = (VoltageRead[0] << 8 | VoltageRead[1]) * BATT_VOLTAGE_FACTOR;  // measured in volts
                    this.Voltage = BitConverter.ToUInt16(VoltageRead, 0) * BATT_VOLTAGE_FACTOR; // check that this will produce the same result
                    Array.Reverse(VoltageRead); // this computer is little endian
                    this.Voltage = BitConverter.ToUInt16(VoltageRead, 0) * BATT_VOLTAGE_FACTOR;
                    
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
                    this.Temperature = (TemperatureRead[0] << 8 | TemperatureRead[1]) * BATT_TEMP_FACTOR; // measured in °C
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
                    this.CurrentInst = (InstCurrentRead[0] << 8 | InstCurrentRead[1]) * BATT_INST_CURRENT_FACTOR;
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
                    this.CurrentAccum = (AccumCurrentRead[0] << 8 | AccumCurrentRead[1]) * BATT_ACCUM_CURRENT_FACTOR;
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
        {          
            public List<SavedTrack> trackList = new List<SavedTrack>();
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
                        if (!track.isEmpty)
                            trackList.Add(track);   // don't add the track if it is empty
                        isEmpty = track.isEmpty;
                        index++;
                    }
                    else isEmpty = false;   // track read failed
                }
            }

            //TODO test this function
            public bool writeSavedTracks()
            {
                byte[] readBuffer = new byte[1];
                if (traqpaq.sendCommand(usbCommand.USB_CMD_WRITE_SAVED_TRACKS, readBuffer))
                {
                    return readBuffer[0] > 0;
                }
                else return false;
            }

            public class SavedTrack
            {
                private byte[] trackReadBuff = new byte[32];
                SavedTrackReader parent;
                short index;
                public string trackName { get; set; }
                public int Longitute { get; set; }
                public int Latitude { get; set; }
                public ushort Heading { get; set; }
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
                        Heading = (ushort)(trackReadBuff[28] << 8 | trackReadBuff[29]);
                        isEmpty = (trackReadBuff[30] == 0xFF);  // true if empty
                        return true;
                    }
                    else return false;
                }
            }
        }


        public class RecordTableReader
        {            
            TraqpaqDevice traqpaq;
            public List<RecordTable> recordTable = new List<RecordTable>();

            public RecordTableReader(TraqpaqDevice parent)
            {
                this.traqpaq = parent;
            }

            /// <summary>
            /// An individual record table
            /// 16 byte struct is returned in the readBuffer.
            /// The readRecordTable function deciphers the bytes.
            /// </summary>
            public class RecordTable
            {
                RecordTableReader parent;
                byte[] readBuffer = new byte[16];
                public bool RecordEmpty { get; set; }
                public byte TrackID { get; set; }
                public uint DateStamp { get; set; }
                public uint StartAddress { get; set; }
                public uint EndAddress { get; set; }

                public RecordTable(RecordTableReader parent)
                {
                    this.parent = parent;
                }

                public bool readRecordTable(byte index)
                {
                    byte length = 16;
                    if (parent.traqpaq.sendCommand(usbCommand.USB_CMD_READ_RECORDTABLE, readBuffer, length, index))
                    {   // set all the properties
                        int idx = 0;
                        this.RecordEmpty = readBuffer[idx++] == 0xFF;   // true if empty
                        this.TrackID = readBuffer[idx++];
                        idx = 4;    // skip 2 reserved bytes
                        this.DateStamp = (uint)(readBuffer[idx++] << 24 | readBuffer[idx++] << 16 | readBuffer[idx++] << 8 | readBuffer[idx++]);
                        this.StartAddress = (uint)(readBuffer[idx++] << 24 | readBuffer[idx++] << 16 | readBuffer[idx++] << 8 | readBuffer[idx++]);
                        this.EndAddress = (uint)(readBuffer[idx++] << 24 | readBuffer[idx++] << 16 | readBuffer[idx++] << 8 | readBuffer[idx++]);                        
                        return true;
                    }
                    else return false;
                }
            }

            /// <summary>
            /// Populate the record table list. Keep reading records until one is empty
            /// </summary>
            /// <returns></returns>
            public bool readRecordTable()
            {   //TODO test this function
                bool isEmpty = false;
                byte index = 0;
                RecordTable table;

                while (!isEmpty)
                {
                    table = new RecordTable(this);
                    if (table.readRecordTable(index))
                    {
                        isEmpty = table.RecordEmpty;
                        if (!isEmpty)   // only add the table if it is not empty
                            this.recordTable.Add(table);
                        index++;
                    }
                    else return false;
                }
                return true;
            }
        }


        /// <summary>
        /// Reads the record data from the device using the start address and
        /// end address for a record table. This class must be created with a 
        /// record table object.
        /// </summary>
        public class RecordDataReader
        {   //TODO record data class
            RecordTableReader.RecordTable recordTable;
            TraqpaqDevice traqpaq;
            //public List<RecordDataPage> recordData = new List<RecordDataPage>();
            public RecordDataPage[] recordData;
            uint numPages;

            public RecordDataReader(TraqpaqDevice parent, RecordTableReader.RecordTable recordTable)
            {
                this.traqpaq = parent;
                this.recordTable = recordTable;
                // allocate the recordData array, 256 bytes per page
                this.numPages = (recordTable.EndAddress - recordTable.StartAddress) / 256;
                this.recordData = new RecordDataPage[numPages];
            }

            public class RecordDataPage
            {
                RecordDataReader parent;
                byte[] dataPage = new byte[256];
                public int utc { get; set; }
                public ushort hdop { get; set; }
                public byte GPSmode { get; set; }
                public byte Satellites { get; set; }
                public tRecordData[] RecordData { get; set; }                

                public RecordDataPage(RecordDataReader parent)
                {
                    this.parent = parent;
                }

                public struct tRecordData
                {
                    public int Latitude { get; set; }
                    public int Longitude { get; set; }
                    public bool lapDetected { get; set; }
                    public int Altitude { get; set; }
                    public double Speed { get; set; }
                    public int Heading { get; set; }
                }

                public bool readRecordDataPage(int index)
                {
                    ushort length = 256;
                    if (parent.traqpaq.readRecordData(dataPage, length, (ushort)index))
                    {
                        // extract the data from the dataPage byte array
                        //TODO convert props to usable value, see GPS decoder ring
                        int idx = 0;    // use to keep track of the index of the dataPage byte
                        this.utc = dataPage[idx++] << 24 | dataPage[idx++] << 16 | dataPage[idx++] << 8 | dataPage[idx++];
                        this.hdop = (ushort)(dataPage[idx++] << 8 | dataPage[idx++]);
                        this.GPSmode = dataPage[idx++];
                        this.Satellites = dataPage[idx++];
                        idx += 8;   // 8 bytes are reserved
                        // set the array of tRecordData structs
                        for (int i = 0; i < RECORD_DATA_PER_PAGE; i++)
                        {
                            this.RecordData[i].Latitude = dataPage[idx++] << 24 | dataPage[idx++] << 16 | dataPage[idx++] << 8 | dataPage[idx++];
                            this.RecordData[i].Longitude = dataPage[idx++] << 24 | dataPage[idx++] << 16 | dataPage[idx++] << 8 | dataPage[idx++];
                            this.RecordData[i].lapDetected = dataPage[idx++] > 0;   // 0x00 means lap not detected. True if lap is detected
                            idx++;  // reserved byte
                            this.RecordData[i].Altitude = (dataPage[idx++] << 8 | dataPage[idx++]);
                            this.RecordData[i].Speed = (dataPage[idx++] << 8 | dataPage[idx++]) * SPEED_FACTOR;
                            this.RecordData[i].Heading = (dataPage[idx++] << 8 | dataPage[idx++]);
                        }
                    }
                    return true;
                }
            }

            /// <summary>
            /// Read all the pages for a given record
            /// </summary>
            /// <returns>True if successful, false otherwise</returns>
            public bool readRecordData()
            {
                for (int i = 0; i < numPages; i++)
                {
                    this.recordData[i] = new RecordDataPage(this);
                    this.recordData[i].readRecordDataPage((int)this.recordTable.StartAddress + (i* 256));
                }
                return true;
            }

            /// <summary>
            /// Erase all recorded data.
            /// WARNING: Do not call this function until able to record new data to the device!
            /// </summary>
            /// <returns>True if successful, false otherwise</returns>
            public bool eraseAllRecordData()
            {
                byte[] readBuffer = new byte[1];
                if (traqpaq.sendCommand(usbCommand.USB_CMD_ERASE_RECORDDATA, readBuffer))
                    return readBuffer[0] > 0;   // true if successful
                return false;
            }
        }

        /// <summary>
        /// Write the default user preferences to flash.
        /// </summary>
        /// <returns>True if successful, false otherwise</returns>
        public bool writeDefaultPrefs()
        {
            byte[] readBuff = new byte[1];
            if (sendCommand(usbCommand.USB_CMD_WRITE_USERPREFS, readBuff))
                return readBuff[0] > 0x00;  //TODO verify that this is the correct value for success
            else return false;
        }

        #region Debug functions
        /// <summary>
        /// Erase flash sector
        /// </summary>
        /// <param name="index">Index of sector to erase</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool eraseFlash(byte index)
        {
            byte[] readBuff = new byte[1];
            if (sendCommand(usbCommand.USB_DBG_DF_SECTOR_ERASE, readBuff, index))
                return readBuff[0] > 0x00;  //TODO verify that this is the correct value for success
            else return false;
        }

        /// <summary>
        /// Check if flash is busy with at read or write operation
        /// </summary>
        /// <returns>True if busy, false otherwise</returns>
        public bool isFlashBusy()
        {
            byte[] readBuff = new byte[1];
            if (sendCommand(usbCommand.USB_DBG_DF_BUSY, readBuff))
                return readBuff[0] > 0x00;  //TODO verify that this is the correct value for busy
            else return false;
        }

        /// <summary>
        /// Erase entire flash
        /// </summary>
        /// <returns>True if success, false otherwise</returns>
        public bool eraseChip()
        {
            byte[] readBuff = new byte[1];
            if (sendCommand(usbCommand.USB_DBG_DF_CHIP_ERASE, readBuff))
                return readBuff[0] > 0x00;  //TODO verify that this is the correct value for success
            else return false;
        }

        public bool isFlashFull()
        {
            byte[] readBuff = new byte[1];
            if (sendCommand(usbCommand.USB_DBG_DF_IS_FLASH_FULL, readBuff))
                return readBuff[0] > 0x00;  //TODO verify that this is the correct value for success
            else return false;
        }

        /// <summary>
        /// Get percentage of used space in flash
        /// </summary>
        /// <returns>Percentage of used space, or -1 if request fails</returns>
        public int getFlashPercentUsed()
        {
            byte[] readBuff = new byte[1];
            if (sendCommand(usbCommand.USB_DBG_DF_USED_SPACE, readBuff))
                return readBuff[0];  //TODO verify that this is the correct value for success
            else return -1;
        }

        /// <summary>
        /// Read the last received GPS latitude
        /// </summary>
        /// <returns>The last GPS latitude as unsigned integer, or 0 if request fails</returns>
        public uint getLastGPS_lat()
        {
            byte[] readBuff = new byte[4];
            if (sendCommand(usbCommand.USB_DBG_GPS_LATITUDE, readBuff))
                return BitConverter.ToUInt32(readBuff, 0);
            else return 0;
        }

        /// <summary>
        /// Read the last received GPS longitude
        /// </summary>
        /// <returns>The last GPS longitude as unsigned integer, or 0 if request fails</returns>
        public uint getLastGPS_long()
        {
            byte[] readBuff = new byte[4];
            if (sendCommand(usbCommand.USB_DBG_GPS_LONGITUDE, readBuff))
                return BitConverter.ToUInt32(readBuff, 0);
            else return 0;
        }

        /// <summary>
        /// Read the last received GPS heading
        /// </summary>
        /// <returns>The last GPS heading as unsigned integer, or 0 if request fails</returns>
        public uint getLastGPS_heading()
        {
            byte[] readBuff = new byte[4];
            if (sendCommand(usbCommand.USB_DBG_GPS_COURSE, readBuff))
                return BitConverter.ToUInt32(readBuff, 0);
            else return 0;
        }
        #endregion
    }
}
