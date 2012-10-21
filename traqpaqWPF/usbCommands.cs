using System;

namespace traqpaqWPF
{
    public enum USBcommand : byte
    {
        USB_CMD_REQ_APPL_VER = 0x00,
        USB_CMD_REQ_HARDWARE_VER = 0x01,
        USB_CMD_REQ_SERIAL_NUMBER = 0x02,
        USB_CMD_REQ_TESTER_ID = 0x03,
        USB_CMD_REQ_BATTERY_VOLTAGE = 0x04,
        USB_CMD_REQ_BATTERY_TEMPERATURE = 0x05,
        USB_CMD_REQ_BATTERY_INSTANT = 0x06,
        USB_CMD_REQ_BATTERY_ACCUM = 0x07,
        USB_CMD_REQ_BATTERY_UPDATE = 0x08,
        USB_CMD_READ_SAVED_TRACKS = 0x11,
        USB_CMD_READ_RECORDTABLE = 0x12,
        USB_CMD_READ_RECORDDATA = 0x13,
        USB_CMD_ERASE_RECORDDATA = 0x17,
        USB_CMD_WRITE_USERPREFS = 0x18,
        USB_CMD_WRITE_SAVED_TRACKS = 0x19,
        USB_CMD_READ_OTP = 0x1C,
        USB_CMD_WRITE_OTP = 0x1D,
        USB_DBG_DF_SECTOR_ERASE = 0x39,
        USB_DBG_DF_BUSY = 0x3A,
        USB_DBG_DF_CHIP_ERASE = 0x3B,
        USB_DBG_DF_IS_FLASH_FULL = 0x3C,
        USB_DBG_DF_USED_SPACE = 0x3D,
        USB_DBG_GPS_CURRENT_POSITION = 0x40,
        USB_DBG_GPS_CURRENT_MODE = 0x41,
        USB_DBG_GPS_INFO_SN = 0x43,
        USB_DBG_GPS_INFO_PN = 0x44,
        USB_DBG_GPS_INFO_SW_VER = 0x45,
        USB_DBG_GPS_INFO_SW_DATE = 0x46,
        USB_DBG_START_RECORDING = 0x50,
        USB_DBG_STOP_RECORDING = 0x51,
        USB_DBG_RECORDING_STATUS = 0x52,
        USB_DBG_ACCEL_GET_STATUS = 0x60,
        USB_DBG_ACCEL_GET_FILT_DATA = 0x61,
        USB_DBG_ACCEL_GET_NORM_DATA = 0x62,
        USB_DBG_ACCEL_GET_ST_DATA = 0x63
    }

    public enum tAccelStatus
    {
        UNKNOWN,
        INCORRECT_DEVID,
        UNINITIALIZED,
        INIT_FAILED,
        PERFORMING_INIT,
        SELF_TEST_FAILED,
        IDLE,
        SAMPLING
    }

    public static class Constants
    {
        // Product ID and Vendor ID (still need to register for valid ones)
        public const int PID = 0x1000;
        public const int VID = 0xAAAA;

        // Dataflash Page Size
        public const int MEMORY_PAGE_SIZE = 256;

        // OTP
        public const byte OTP_SERIAL_LENGTH = 13;

        // Memory Layout
        public const int ADDR_USER_PREFS_START = 0x00000000;
        public const int ADDR_USER_PREFS_END = 0x000000FF;
        public const int ADDR_RECORD_TABLE_START = 0x00000100;
        public const int ADDR_RECORD_TABLE_END = 0x00000FFF;
        public const int ADDR_TRACKLIST_START = 0x00001000;
        public const int ADDR_TRACKLIST_END = 0x00001FFF;
        public const int ADDR_RECORD_DATA_START = 0x00002000;
        public const int ADDR_RECORD_DATA_END = 0x001FFFFF;

        // Record Table Size
        public const int RECORD_TABLE_SIZE = 16;

        // Record Table Offsets
        public const int RECORD_EMPTY = 0;
        public const int RECORD_TRACK_ID = 1;
        public const int RECORD_DATESTAMP = 4;
        public const int RECORD_START_ADDRESS = 8;
        public const int RECORD_END_ADDRESS = 12;

        // Record Page Info Size
        public const int RECORD_PAGE_INFO_SIZE = 16;

        // Record Page Info Offsets
        public const int RECORD_DATA_UTC = 0;
        public const int RECORD_DATA_HDOP = 4;
        public const int RECORD_DATA_MODE = 6;
        public const int RECORD_DATA_SATELLITES = 7;

        // Record Data Size
        public const int RECORD_DATA_SIZE = 16;
        public const int RECORD_DATA_PER_PAGE = 15;


        // Record Data Offsets (to be multiplied by the index of the tRecordData struct)
        public const int RECORD_DATA_LATITUDE = 0;
        public const int RECORD_DATA_LONGITUDE = 4;
        public const int RECORD_DATA_LAP_DETECTED = 8;
        public const int RECORD_DATA_ALTITUDE = 10;
        public const int RECORD_DATA_SPEED = 12;
        public const int RECORD_DATA_COURSE = 14;


        // TRACK LIST
        public const int TRACKLIST_SIZE = 32;	// Size of Tracklist struct - 32 bytes

        public const int TRACKLIST_NAME = 0;
        public const int TRACKLIST_NAME_STRLEN = 20;

        public const int TRACKLIST_LONGITUDE = 20;
        public const int TRACKLIST_LATITUDE = 24;
        public const int TRACKLIST_COURSE = 28;
        public const int TRACKLIST_ISEMPTY = 30;


        // GPS INFO
        public const int GPS_INFO_PART_NUMBER_SIZE = 8;
        public const int GPS_INFO_SW_VERSION_SIZE = 9;
        public const int GPS_INFO_SW_DATE_SIZE = 9;


        //Conversion factors
        public const double BATT_VOLTAGE_FACTOR = 0.00488;       // (Volts)
        public const double BATT_TEMP_FACTOR = 0.125;            // (°Celsius)
        public const double BATT_INST_CURRENT_FACTOR = 0.625;    // (mAmps)
        public const double BATT_ACCUM_CURRENT_FACTOR = 0.25;    // (mAmp hours)
        public const double SPEED_FACTOR = 0.005144;             // (meters/second)
        public const double LATITUDE_LONGITUDE_COORD = 1000000;  // Converts the lat and long as ints to coordinates
        public const double ALTITUDE_FACTOR = 10;                // Converts the altitude to meters
        public const double COURSE_FACTOR = 100;                 // Converts course(heading) to degrees
        public const double UTC_FACTOR = 1000;
        public const double HDOP_FACTOR = 100;

        
        public const int TIMEOUT = 250;                          // usb timeout in ms
    }
}
