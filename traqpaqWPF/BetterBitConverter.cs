using System;
using System.Linq;

namespace traqpaqWPF
{
    public static class BetterBitConverter
    {
        #region GetBytes
        //
        // Summary:
        //     Returns the specified Boolean value as an array of bytes.
        //
        // Parameters:
        //   value:
        //     A Boolean value.
        //
        // Returns:
        //     An array of bytes with length 1.
        public static byte[] GetBytes(bool value)
        {
            return BitConverter.GetBytes(value);
        }

        //
        // Summary:
        //     Returns the specified Unicode character value as an array of bytes.
        //
        // Parameters:
        //   value:
        //     A character to convert.
        //
        // Returns:
        //     An array of bytes with length 2.
        public static byte[] GetBytes(char value)
        {
            byte[] b = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(b);
            return b;
        }

        //
        // Summary:
        //     Returns the specified double-precision floating point value as an array of
        //     bytes.
        //
        // Parameters:
        //   value:
        //     The number to convert.
        //
        // Returns:
        //     An array of bytes with length 8.
        public static byte[] GetBytes(double value)
        {
            byte[] b = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(b);
            return b;
        }

        //
        // Summary:
        //     Returns the specified single-precision floating point value as an array of
        //     bytes.
        //
        // Parameters:
        //   value:
        //     The number to convert.
        //
        // Returns:
        //     An array of bytes with length 4.
        public static byte[] GetBytes(float value)
        {
            byte[] b = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(b);
            return b;
        }

        //
        // Summary:
        //     Returns the specified 32-bit signed integer value as an array of bytes.
        //
        // Parameters:
        //   value:
        //     The number to convert.
        //
        // Returns:
        //     An array of bytes with length 4.
        public static byte[] GetBytes(int value)
        {
            byte[] b = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(b);
            return b;
        }

        //
        // Summary:
        //     Returns the specified 64-bit signed integer value as an array of bytes.
        //
        // Parameters:
        //   value:
        //     The number to convert.
        //
        // Returns:
        //     An array of bytes with length 8.
        public static byte[] GetBytes(long value)
        {
            byte[] b = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(b);
            return b;
        }

        //
        // Summary:
        //     Returns the specified 16-bit signed integer value as an array of bytes.
        //
        // Parameters:
        //   value:
        //     The number to convert.
        //
        // Returns:
        //     An array of bytes with length 2.
        public static byte[] GetBytes(short value)
        {
            byte[] b = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(b);
            return b;
        }

        //
        // Summary:
        //     Returns the specified 32-bit unsigned integer value as an array of bytes.
        //
        // Parameters:
        //   value:
        //     The number to convert.
        //
        // Returns:
        //     An array of bytes with length 4.
        public static byte[] GetBytes(uint value)
        {
            byte[] b = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(b);
            return b;
        }

        //
        // Summary:
        //     Returns the specified 64-bit unsigned integer value as an array of bytes.
        //
        // Parameters:
        //   value:
        //     The number to convert.
        //
        // Returns:
        //     An array of bytes with length 8.
        public static byte[] GetBytes(ulong value)
        {
            byte[] b = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(b);
            return b;
        }

        //
        // Summary:
        //     Returns the specified 16-bit unsigned integer value as an array of bytes.
        //
        // Parameters:
        //   value:
        //     The number to convert.
        //
        // Returns:
        //     An array of bytes with length 2.
        public static byte[] GetBytes(ushort value)
        {
            byte[] b = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(b);
            return b;
        }
        #endregion

        #region Bytes to Other
        //
        // Summary:
        //     Returns a Boolean value converted from one byte at a specified position in
        //     a byte array.
        //
        // Parameters:
        //   value:
        //     An array of bytes.
        //
        //   startIndex:
        //     The starting position within value.
        //
        // Returns:
        //     true if the byte at startIndex in value is nonzero; otherwise, false.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     startIndex is less than zero or greater than the length of value minus 1.
        public static bool ToBoolean(byte[] value, int startIndex)
        {
            return BitConverter.ToBoolean(value, startIndex);
        }

        //
        // Summary:
        //     Returns a double-precision floating point number converted from eight bytes
        //     at a specified position in a byte array.
        //
        // Parameters:
        //   value:
        //     An array of bytes.
        //
        //   startIndex:
        //     The starting position within value.
        //
        // Returns:
        //     A double precision floating point number formed by eight bytes beginning
        //     at startIndex.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     startIndex is greater than or equal to the length of value minus 7, and is
        //     less than or equal to the length of value minus 1.
        //
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     startIndex is less than zero or greater than the length of value minus 1.

        public static double ToDouble(byte[] value, int startIndex)
        {
            if (BitConverter.IsLittleEndian)
            {
                value = value.Skip(startIndex).Take(8).ToArray();
                Array.Reverse(value);
                return BitConverter.ToDouble(value, 0);
            }
            else
                return BitConverter.ToDouble(value, startIndex);            
        }

        //
        // Summary:
        //     Returns a 16-bit signed integer converted from two bytes at a specified position
        //     in a byte array.
        //
        // Parameters:
        //   value:
        //     An array of bytes.
        //
        //   startIndex:
        //     The starting position within value.
        //
        // Returns:
        //     A 16-bit signed integer formed by two bytes beginning at startIndex.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     startIndex equals the length of value minus 1.
        //
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     startIndex is less than zero or greater than the length of value minus 1.
        public static short ToInt16(byte[] value, int startIndex)
        {
            if (BitConverter.IsLittleEndian)
            {
                value = value.Skip(startIndex).Take(2).ToArray();
                Array.Reverse(value);
                return BitConverter.ToInt16(value, 0);
            }
            else
                return BitConverter.ToInt16(value, startIndex);
        }

        //
        // Summary:
        //     Returns a 32-bit signed integer converted from four bytes at a specified
        //     position in a byte array.
        //
        // Parameters:
        //   value:
        //     An array of bytes.
        //
        //   startIndex:
        //     The starting position within value.
        //
        // Returns:
        //     A 32-bit signed integer formed by four bytes beginning at startIndex.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     startIndex is greater than or equal to the length of value minus 3, and is
        //     less than or equal to the length of value minus 1.
        //
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     startIndex is less than zero or greater than the length of value minus 1.
        public static int ToInt32(byte[] value, int startIndex)
        {
            if (BitConverter.IsLittleEndian)
            {
                value = value.Skip(startIndex).Take(4).ToArray();
                Array.Reverse(value);
                return BitConverter.ToInt32(value, 0);
            }
            else
                return BitConverter.ToInt32(value, startIndex);
        }

        //
        // Summary:
        //     Returns a 64-bit signed integer converted from eight bytes at a specified
        //     position in a byte array.
        //
        // Parameters:
        //   value:
        //     An array of bytes.
        //
        //   startIndex:
        //     The starting position within value.
        //
        // Returns:
        //     A 64-bit signed integer formed by eight bytes beginning at startIndex.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     startIndex is greater than or equal to the length of value minus 7, and is
        //     less than or equal to the length of value minus 1.
        //
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     startIndex is less than zero or greater than the length of value minus 1.
        public static long ToInt64(byte[] value, int startIndex)
        {
            if (BitConverter.IsLittleEndian)
            {
                value = value.Skip(startIndex).Take(8).ToArray();
                Array.Reverse(value);
                return BitConverter.ToInt64(value, 0);
            }
            else
                return BitConverter.ToInt64(value, startIndex);
        }

        //
        // Summary:
        //     Returns a single-precision floating point number converted from four bytes
        //     at a specified position in a byte array.
        //
        // Parameters:
        //   value:
        //     An array of bytes.
        //
        //   startIndex:
        //     The starting position within value.
        //
        // Returns:
        //     A single-precision floating point number formed by four bytes beginning at
        //     startIndex.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     startIndex is greater than or equal to the length of value minus 3, and is
        //     less than or equal to the length of value minus 1.
        //
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     startIndex is less than zero or greater than the length of value minus 1.
        public static float ToSingle(byte[] value, int startIndex)
        {
            if (BitConverter.IsLittleEndian)
            {
                value = value.Skip(startIndex).Take(4).ToArray();
                Array.Reverse(value);
                return BitConverter.ToSingle(value, 0);
            }
            else
                return BitConverter.ToSingle(value, startIndex);
        }

        //
        // Summary:
        //     Converts the numeric value of each element of a specified array of bytes
        //     to its equivalent hexadecimal string representation.
        //
        // Parameters:
        //   value:
        //     An array of bytes.
        //
        // Returns:
        //     A System.String of hexadecimal pairs separated by hyphens, where each pair
        //     represents the corresponding element in value; for example, "7F-2C-4A".
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        public static string ToString(byte[] value)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(value);
                return BitConverter.ToString(value);
            }
            else
                return BitConverter.ToString(value);
        }

        //
        // Summary:
        //     Converts the numeric value of each element of a specified subarray of bytes
        //     to its equivalent hexadecimal string representation.
        //
        // Parameters:
        //   value:
        //     An array of bytes.
        //
        //   startIndex:
        //     The starting position within value.
        //
        // Returns:
        //     A System.String of hexadecimal pairs separated by hyphens, where each pair
        //     represents the corresponding element in a subarray of value; for example,
        //     "7F-2C-4A".
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     startIndex is less than zero or greater than the length of value minus 1.
        public static string ToString(byte[] value, int startIndex)
        {
            if (BitConverter.IsLittleEndian)
            {
                value = value.Skip(startIndex).Take(value.Length - startIndex).ToArray();
                Array.Reverse(value);
                return BitConverter.ToString(value);
            }
            else
                return BitConverter.ToString(value, startIndex);
        }

        //
        // Summary:
        //     Converts the numeric value of each element of a specified subarray of bytes
        //     to its equivalent hexadecimal string representation.
        //
        // Parameters:
        //   value:
        //     An array of bytes.
        //
        //   startIndex:
        //     The starting position within value.
        //
        //   length:
        //     The number of array elements in value to convert.
        //
        // Returns:
        //     A System.String of hexadecimal pairs separated by hyphens, where each pair
        //     represents the corresponding element in a subarray of value; for example,
        //     "7F-2C-4A".
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     startIndex or length is less than zero.-or-startIndex is greater than zero
        //     and is greater than or equal to the length of value.
        //
        //   System.ArgumentException:
        //     The combination of startIndex and length does not specify a position within
        //     value; that is, the startIndex parameter is greater than the length of value
        //     minus the length parameter.
        public static string ToString(byte[] value, int startIndex, int length)
        {
            if (BitConverter.IsLittleEndian)
            {
                value = value.Skip(startIndex).Take(length).ToArray();
                Array.Reverse(value);
                return BitConverter.ToString(value);
            }
            else
                return BitConverter.ToString(value, startIndex, length);
        }

        public static string ToASCIIString(byte[] value, int startIndex, int maxLength)
        {
            string returnString = "";
            for (int i = startIndex; i < maxLength; i++)
            {
                if (value[i] == 0x00) break;
                returnString += System.Text.Encoding.ASCII.GetString(value, i, 1);
            }
            return returnString;
        }

        //
        // Summary:
        //     Returns a 16-bit unsigned integer converted from two bytes at a specified
        //     position in a byte array.
        //
        // Parameters:
        //   value:
        //     The array of bytes.
        //
        //   startIndex:
        //     The starting position within value.
        //
        // Returns:
        //     A 16-bit unsigned integer formed by two bytes beginning at startIndex.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     startIndex equals the length of value minus 1.
        //
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     startIndex is less than zero or greater than the length of value minus 1.
        public static ushort ToUInt16(byte[] value, int startIndex)
        {
            if (BitConverter.IsLittleEndian)
            {
                value = value.Skip(startIndex).Take(2).ToArray();
                Array.Reverse(value);
                return BitConverter.ToUInt16(value, 0);
            }
            else
                return BitConverter.ToUInt16(value, startIndex);
        }

        //
        // Summary:
        //     Returns a 32-bit unsigned integer converted from four bytes at a specified
        //     position in a byte array.
        //
        // Parameters:
        //   value:
        //     An array of bytes.
        //
        //   startIndex:
        //     The starting position within value.
        //
        // Returns:
        //     A 32-bit unsigned integer formed by four bytes beginning at startIndex.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     startIndex is greater than or equal to the length of value minus 3, and is
        //     less than or equal to the length of value minus 1.
        //
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     startIndex is less than zero or greater than the length of value minus 1.
        public static uint ToUInt32(byte[] value, int startIndex)
        {
            if (BitConverter.IsLittleEndian)
            {
                value = value.Skip(startIndex).Take(4).ToArray();
                Array.Reverse(value);
                return BitConverter.ToUInt32(value, 0);
            }
            else
                return BitConverter.ToUInt32(value, startIndex);
        }

        //
        // Summary:
        //     Returns a 64-bit unsigned integer converted from eight bytes at a specified
        //     position in a byte array.
        //
        // Parameters:
        //   value:
        //     An array of bytes.
        //
        //   startIndex:
        //     The starting position within value.
        //
        // Returns:
        //     A 64-bit unsigned integer formed by the eight bytes beginning at startIndex.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     startIndex is greater than or equal to the length of value minus 7, and is
        //     less than or equal to the length of value minus 1.
        //
        //   System.ArgumentNullException:
        //     value is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     startIndex is less than zero or greater than the length of value minus 1.
        public static ulong ToUInt64(byte[] value, int startIndex)
        {
            if (BitConverter.IsLittleEndian)
            {
                value = value.Skip(startIndex).Take(8).ToArray();
                Array.Reverse(value);
                return BitConverter.ToUInt64(value, 0);
            }
            else
                return BitConverter.ToUInt64(value, startIndex);
        }

        #endregion
    }
}
