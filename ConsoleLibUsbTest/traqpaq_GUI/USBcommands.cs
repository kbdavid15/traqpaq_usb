using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace traqpaq_GUI
{
    static class USBCommand
    {
        public const byte USB_CMD_REQ_APPL_VER = 0x00;
        public const byte USB_CMD_REQ_HARDWARE_VER = 0x01;
        public const byte USB_CMD_REQ_SERIAL_NUMBER = 0x02;
        public const byte USB_CMD_REQ_TESTER_ID = 0x03;
        public const byte USB_CMD_REQ_BATTERY_VOLTAGE = 0x04;
        public const byte USB_CMD_REQ_BATTERY_TEMPERATURE = 0x05;
        public const byte USB_CMD_REQ_BATTERY_INSTANT = 0x06;
        public const byte USB_CMD_REQ_BATTERY_ACCUM = 0x07;
        public const byte USB_CMD_REQ_BATTERY_UPDATE = 0x08;
        public const byte USB_CMD_READ_SAVED_TRACKS = 0x11;
        public const byte USB_CMD_READ_RECORDTABLE = 0x12;
        public const byte USB_CMD_READ_RECORDDATA = 0x13;
        public const byte USB_CMD_ERASE_RECORDDATA = 0x17;
        public const byte USB_CMD_WRITE_USERPREFS = 0x18;
        public const byte USB_CMD_WRITE_SAVED_TRACKS = 0x19;
        public const byte USB_CMD_READ_OTP = 0x1C;
        public const byte USB_CMD_WRITE_OTP = 0x1D;
        public const byte USB_DBG_DF_SECTOR_ERASE = 0x39;
        public const byte USB_DBG_DF_BUSY = 0x3A;
        public const byte USB_DBG_DF_CHIP_ERASE = 0x3B;
        public const byte USB_DBG_DF_IS_FLASH_FULL = 0x3C;
        public const byte USB_DBG_DF_USED_SPACE = 0x3D;
        public const byte USB_DBG_GPS_LATITUDE = 0x40;
        public const byte USB_DBG_GPS_LONGITUDE = 0x41;
        public const byte USB_DBG_GPS_COURSE = 0x42;
    }
}
