using System;

namespace traqpaq_GUI
{
    public enum usbCommand : byte
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
        USB_DBG_GPS_LATITUDE = 0x40,
        USB_DBG_GPS_LONGITUDE = 0x41,
        USB_DBG_GPS_COURSE = 0x42,
    }

    /// <summary>
    /// Used as a container for constants
    /// </summary>
    public static class Constants
    {
        //TODO create constant class
    }
}
