using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Data
{
    public static class SafeConvert
    {
        #region ValidateSQLDateTime

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Validates the SQL date time value is not set to DateTime.MinValue. </summary>
        ///
        /// <param name="value">    The date time value to check. </param>
        ///
        /// <returns>
        /// the date time value if it is a valid SQL Date or clsSQLCommand.c_MinSQLDateValue.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static DateTime ValidateSQLDateTime(DateTime value)
        {
            if (value != DateTime.MinValue)
                return (value);
            else
                return (System.Data.SqlTypes.SqlDateTime.MinValue.Value);
        }

        #endregion ValidateSQLDateTime

        #region ToDateTime

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Converts a value to a date time. </summary>
        ///
        /// <param name="value">    The Object to convert. </param>
        ///
        /// <returns>   value as a DateTime. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static DateTime ToDateTime(object value)
        {
            return (ToDateTime(value, DateTime.MinValue));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Converts a value to a date time. </summary>
        ///
        /// <param name="value">    The date time value to convert. </param>
        /// <param name="Default">  default Date/Time returned on error. </param>
        ///
        /// <returns>   value as a DateTime. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static DateTime ToDateTime(object value, DateTime Default)
        {
            DateTime dt;

            if ((value != null) && DateTime.TryParse(value.ToString(), out dt))
                return (dt);

            return (Default);
        }

        #endregion ToDateTime

        #region ToSQLDateTime

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Converts a value to a SQL date time. </summary>
        ///
        /// <param name="value">    The value to convert. </param>
        ///
        /// <returns>   value as a DateTime. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static DateTime ToSQLDateTime(object value)
        {
            return (ToDateTime(value, System.Data.SqlTypes.SqlDateTime.MinValue.Value));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Converts a value to a SQL date time. </summary>
        ///
        /// <param name="value">    The value to convert. </param>
        /// <param name="Default">  default Date/Time returned on error.</param>
        ///
        /// <returns>   value as a DateTime. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static DateTime ToSQLDateTime(object value, DateTime Default)
        {
            DateTime dt;

            if ((value != null) && DateTime.TryParse(value.ToString(), out dt))
            {
                if (dt != DateTime.MinValue)
                    return (dt);
            }

            return (Default);
        }

        #endregion ToSQLDateTime

        #region ToBoolean

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Converts a value to a Boolean. </summary>
        ///
        /// <param name="value">    The value to convert. </param>
        ///
        /// <returns>   value as a Boolean. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static bool ToBoolean(string value)
        {
            return (ToBoolean(value, false));
        }

        //BB 28-09-2011
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   ToBool converts from a string value or 1 and 0. </summary>
        ///
        /// <param name="value">    The value to convert </param>
        /// <param name="Default">  The default value to return on error. </param>
        ///
        /// <returns>   The given data converted to a Boolean. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static bool ToBoolean(string value, bool Default)
        {
            bool ct;

            if (!string.IsNullOrEmpty(value))
            {
                //try convertion to bool
                if (!string.IsNullOrEmpty(value) && bool.TryParse(value, out ct))
                    return (ct);

                //else if numeric return true if not 0
                int val;
                if (int.TryParse(value, out val))
                    return (val != 0);
            }

            return (Default);
        }

        #endregion ToBoolean

        #region ToInt16

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Converts a value to an Int16. </summary>
        ///
        /// <param name="value">    The value to convert </param>
        ///
        /// <returns>   value as a short. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static short ToInt16(string value)
        {
            return (ToInt16(value, 0));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Converts a value to an Int16. </summary>
        ///
        /// <param name="value">    The value to convert </param>
        /// <param name="Default">  The default value to return on error. </param>
        ///
        /// <returns>   value as a short. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static short ToInt16(string value, short Default)
        {
            short ct;


            if (!string.IsNullOrEmpty(value) && short.TryParse(value, out ct))
                return (ct);

            return (Default);
        }

        #endregion ToInt16

        #region ToInt32

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Converts a value to an Int32. </summary>
        ///
        /// <param name="value">    The value to convert </param>
        ///
        /// <returns>   value as an int. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static int ToInt32(string value)
        {
            return (ToInt32(value, 0));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Converts a value to an Int32. </summary>
        ///
        /// <param name="value">    The value to convert </param>
        /// <param name="Default">  The default value to return on error. </param>
        ///
        /// <returns>   value as an int. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static int ToInt32(string value, int Default)
        {
            int ct;

            if (!string.IsNullOrEmpty(value) && int.TryParse(value.ToString(), out ct))
                return (ct);

            return (Default);
        }

        #endregion ToInt32

        #region ToInt64

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Converts a value to an Int64. </summary>
        ///
        /// <param name="value">    The value to convert </param>
        ///
        /// <returns>   value as a long. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static long ToInt64(string value)
        {
            return (ToInt64(value, 0));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Converts a value to an Int64. </summary>
        ///
        /// <param name="value">    The value to convert </param>
        /// <param name="Default">  The default value to return on error. </param>
        ///
        /// <returns>   value as a long. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static long ToInt64(string value, long Default)
        {
            long ct;

            if (!string.IsNullOrEmpty(value) && long.TryParse(value, out ct))
                return (ct);

            return (Default);
        }

        #endregion ToInt64

        #region ToUInt16

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Converts a value to an UInt16. </summary>
        ///
        /// <param name="value">    The value to convert </param>
        ///
        /// <returns>   value as an UShort. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static ushort ToUInt16(string value)
        {
            return (ToUInt16(value, 0));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Converts a value to an UInt16. </summary>
        ///
        /// <param name="value">    The value to convert </param>
        /// <param name="Default">  The default value to return on error. </param>
        ///
        /// <returns>   value as an UShort. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static ushort ToUInt16(string value, ushort Default)
        {
            ushort ct;

            if (string.IsNullOrEmpty(value))
                return (Default);

            if (ushort.TryParse(value, out ct))
                return (ct);

            return ((ushort)ToInt16(value, (short)Default));
        }

        #endregion ToUInt16

        #region ToUInt32

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Converts a value to an UInt32. </summary>
        ///
        /// <param name="value">    The value to convert </param>
        ///
        /// <returns>   value as an UInt. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static uint ToUInt32(string value)
        {
            return (ToUInt32(value, 0));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Converts a value to an UInt32. </summary>
        ///
        /// <param name="value">    The date time value to check. </param>
        /// <param name="Default">  Date/Time of the default. </param>
        ///
        /// <returns>   value as an UInt. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static uint ToUInt32(string value, uint Default)
        {
            uint ct;

            if (string.IsNullOrEmpty(value))
                return (Default);

            if (uint.TryParse(value.ToString(), out ct))
                return (ct);

            return ((uint)ToInt32(value, (int)Default));
        }

        #endregion ToUInt32

        #region ToUInt64

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Converts a value to an UInt64. </summary>
        ///
        /// <param name="value">    The value to convert </param>
        ///
        /// <returns>   value as an ULong. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static ulong ToUInt64(string value)
        {
            return (ToUInt64(value, 0));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Converts a value to an UInt64. </summary>
        ///
        /// <param name="value">    The value to convert </param>
        /// <param name="Default">  The default value to return on error. </param>
        ///
        /// <returns>   value as an ULong. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static ulong ToUInt64(string value, ulong Default)
        {
            ulong ct;

            if (string.IsNullOrEmpty(value))
                return (Default);

            if (ulong.TryParse(value, out ct))
                return (ct);

            return ((ulong)ToInt64(value, (long)Default));
        }

        #endregion ToUInt64

        #region ToDDMMYYYYOnly

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get a packed date (byte) object. </summary>
        ///
        /// <param name="Date"> Date/Time of the date. </param>
        ///
        /// <returns>   Date as an int. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static int ToDDMMYYYYOnly(DateTime Date)
        {
            int[] datm = ToDDMMYYYY(Date);
            return (datm[0]);
        }

        #endregion ToDDMMYYYYOnly

        #region ToDDMMYYYY

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get a packed date/time (byte) object. </summary>
        ///
        /// <param name="Date"> Date/Time of the date. </param>
        ///
        /// <returns>   Date and Time as an Int[2] array. 
        ///             Date is in Int[0] in the format DD/MM/YY
        ///             Time is in Int[1] as Seconds since midnight
        ///             </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static int[] ToDDMMYYYY(DateTime Date)
        {
            int[] datm = new int[2];

            if (Date != DateTime.MinValue)
            {
                datm[0] = (int)((((Date.Day << 8) | Date.Month) << 16) | Date.Year);   //pack date
                datm[1] = (int)Date.TimeOfDay.TotalSeconds; //pack time
            }

            return (datm);
        }

        #endregion ToDDMMYYYY

        #region ToDDMMYY

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get a packed date/time (byte) object. </summary>
        ///
        /// <param name="Date"> Date/Time of the date. </param>
        ///
        /// <returns>   Date as an int in the format DD/MM/YY. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static int ToDDMMYY(DateTime Date)
        {
            if (Date != DateTime.MinValue)
                return ((((Date.Day * 100) + Date.Month) * 100) + (Date.Year % 100));   //pack date

            return (0);
        }

        #endregion ToDDMMYY

        #region FromDDMMYY

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Unpack a packed date (byte) object. </summary>
        ///
        /// <param name="DDMMYY">   The date in ddmmyy format. </param>
        ///
        /// <returns> A date/time object. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static DateTime FromDDMMYY(int DDMMYY)
        {
            DateTime datm;

            if (DDMMYY != 0)
            {
                try
                {
                    datm = DateTime.Now;

                    int Year = (int)(DDMMYY % 10000); //year and month
                    int Month = (int)(Year / 100);   //month
                    int Day = (int)(DDMMYY / 10000);  //day

                    Year %= 100;                     //year 

                    Year += ((datm.Year / 100) * 100);

                    datm = new DateTime(Year, Month, Day);

                }
                catch (Exception)
                {
                    datm = DateTime.MinValue;
                }
            }
            else
                datm = DateTime.MinValue;

            return (datm);
        }

        #endregion FromDDMMYY

        #region FromDDMMYYYY

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get an unpacked date/time (byte) object. </summary>
        ///
        /// <param name="DDMMYYYY"> The date in ddmmyyyy format. </param>
        /// <param name="lSeconds"> The time in seconds. </param>
        ///
        /// <returns> A date/time object. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static DateTime FromDDMMYYYY(int DDMMYYYY, int lSeconds)
        {
            DateTime datm;

            if (DDMMYYYY != 0)
            {
                try
                {
                    int Day = (int)((DDMMYYYY >> 24) & 0xFF);
                    int Month = (int)((DDMMYYYY >> 16) & 0xFF);
                    int Year = (int)(DDMMYYYY & 0xFFFF);

                    datm = new DateTime(Year, Month, Day);

                    if (lSeconds > 0 && lSeconds < 86400)
                        datm = datm.AddSeconds(lSeconds);
                }
                catch (Exception)
                {
                    datm = DateTime.MinValue;
                }
            }
            else
                datm = DateTime.MinValue;

            return (datm);
        }

        #endregion FromDDMMYYYY

        #region ToDDMMYYYYValueOnly

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get a packed date (value) object. </summary>
        ///
        /// <param name="Date"> Date/Time of the date to pack. </param>
        ///
        /// <returns>   Date as an int. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static int ToDDMMYYYYValueOnly(DateTime Date)
        {
            int[] datm = ToDDMMYYYYValue(Date);
            return (datm[0]);
        }

        #endregion ToDDMMYYYYValueOnly

        #region ToDDMMYYYYValue

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get a packed date and time (value) object. </summary>
        ///
        /// <param name="Date"> Date/Time of the date to pack. </param>
        ///
        /// <returns>   Date as a two byte int array. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static int[] ToDDMMYYYYValue(DateTime Date)
        {
            int[] datm = new int[2];

            if (Date != null && Date != DateTime.MinValue)
            {
                datm[0] = (int)((((Date.Day * 100) + Date.Month) * 10000) + Date.Year);   //pack date
                datm[1] = (int)Date.TimeOfDay.TotalSeconds; //pack time
            }

            return (datm);
        }

        #endregion ToDDMMYYYYValue

        #region FromDDMMYYYYValue

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get an unpacked date time (value) object. </summary>
        ///
        /// <param name="DDMMYYYY"> The Date in ddmmyyyy format. </param>
        /// <param name="lSeconds"> The time in seconds. </param>
        ///
        /// <returns> A date/time object. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static DateTime FromDDMMYYYYValue(int DDMMYYYY, int lSeconds)
        {
            DateTime datm;

            if (DDMMYYYY != 0)
            {
                try
                {
                    int Year = (int)(DDMMYYYY % 1000000); //year and month
                    int Month = (int)(Year / 10000);   //month
                    int Day = (int)(DDMMYYYY / 1000000);  //day

                    Year %= 10000;                     //year 

                    datm = new DateTime(Year, Month, Day);

                    if (lSeconds > 0 && lSeconds < 86400)
                        datm = datm.AddSeconds(lSeconds);
                }
                catch (Exception)
                {
                    datm = DateTime.MinValue;
                }
            }
            else
                datm = DateTime.MinValue;

            return (datm);
        }

        #endregion FromDDMMYYYYValue

        #region ToDATM

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get a packed date/time object as a DATM (DDMSSSSS).</summary>
        ///
        /// <param name="Date"> Date/Time to pack. </param>
        ///
        /// <remarks> DATM is used to represent DateTime in all Hardware where seconds are required but not the current year.</remarks>
        /// <returns>   Date as an UInt32. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static UInt32 ToDATM(DateTime Date)
        {
            UInt32 datm = 0;

            if (Date != null && Date != DateTime.MinValue)
            {
                datm = (UInt32)((((Date.Day * 100) + Date.Month) << 20));   //pack day and month
                datm |= (UInt32)Date.TimeOfDay.TotalSeconds;   //pack seconds
            }

            return (datm);
        }

        #endregion ToDATM

        #region FromDATM

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get an unpacked date/time object. </summary>
        ///
        /// <param name="Datm"> The DATM to unpack. </param>
        ///
        /// <returns>   A date/time object. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static DateTime FromDATM(UInt32 Datm)
        {
            DateTime datm;

            if (Datm != 0)
            {
                try
                {
                    datm = DateTime.Now;

                    UInt32 Date = (Datm >> 20) & 0xFFF;
                    UInt32 Time = (Datm & 0xFFFFF);

                    int Day = (int)(Date / 100);
                    int Month = (int)(Date % 100);
                    int Year = datm.Year;

                    if (Month > datm.Month)
                        Year -= 1;

                    datm = new DateTime(Year, Month, Day);

                    if (Time > 0)
                        datm = datm.AddSeconds(Time);
                }
                catch (Exception)
                {
                    datm = DateTime.MinValue;
                }
            }
            else
                datm = DateTime.MinValue;

            return (datm);
        }

        #endregion FromDATM

        #region ToDATMY

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Pack a date/time to a DATMY (YYMDDMMM).</summary>
        ///             
        /// <param name="Date"> Date/Time of the date. </param>
        ///
        /// <remarks> DATMY is used to represent DateTime in new Hardware where year is required but not seconds. </remarks> 
        /// 
        /// <returns>   Date as an UInt32. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static UInt32 ToDATMY(DateTime Date)
        {
            UInt32 datmy = 0;

            if (Date != null && Date != DateTime.MinValue)
            {
                datmy = (UInt32)((((((Date.Year & 0xFF) << 4) | Date.Month) << 8) | Date.Day) << 12);	//pack 2 digit year, month and day
                datmy |= (UInt32)Date.TimeOfDay.TotalMinutes;											//pack total Minutes
            }

            return (datmy);
        }

        #endregion ToDATMY

        #region FromDATMY

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get a Date / Time object from a DATMY. </summary>
        ///
        /// <param name="Datmy">    The DATMY to convert. </param>
        ///
        /// <returns>  A date/time object. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static DateTime FromDATMY(UInt32 Datmy)
        {
            DateTime datm;

            if (Datmy != 0)
            {
                try
                {
                    UInt32 UYear = (UInt32)((DateTime.Now.Year / 100) * 100);

                    int Year = (int)(UYear | ((Datmy >> 24) & 0xFF));
                    int Month = (int)((Datmy >> 20) & 0x0F);
                    int Day = (int)((Datmy >> 12) & 0xFF);
                    int Time = (int)(Datmy & 0x0FFF);

                    datm = new DateTime(Year, Month, Day);

                    if (Time > 0)
                        datm = datm.AddMinutes(Time);
                }
                catch (Exception)
                {
                    datm = DateTime.MinValue;
                }
            }
            else
                datm = DateTime.MinValue;

            return (datm);
        }

        #endregion FromDATMY

        #region EncodeBytesToHex

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Encode bytes to a hexadecimal string. </summary>
        ///
        /// <param name="HexEncoded">   The hexadecimal bytes to encoded. </param>
        ///
        /// <returns>   Hexadecimal string. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static string EncodeBytesToHex(byte[] HexEncoded)
        {
            return (EncodeBytesToHex(HexEncoded, 0, 0));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Encodes each byte in a byte array to a two character hexadecimal string. </summary>
        ///
        /// <param name="HexEncoded">   The hexadecimal bytes to encoded. </param>
        /// <param name="offset">       The start position in the byte array. </param>
        /// <param name="len">          The length of bytes to encode. </param>
        ///
        /// <returns>   Hexadecimal string. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static string EncodeBytesToHex(byte[] HexEncoded, int offset, int len)
        {
            if (HexEncoded == null || HexEncoded.Length < 1)
                return (string.Empty);

            if (len < 1)
                len = HexEncoded.Length;

            if (offset < 1)
                offset = 0;

            int max = (len * 2) + offset;

            if (HexEncoded.Length < max)
                max = HexEncoded.Length;

            StringBuilder sb = new StringBuilder();

            for (int i = offset; i < max; i++)
                sb.AppendFormat("{0:X2}", HexEncoded[i]);

            return (sb.ToString());
        }

        #endregion EncodeBytesToHex

        #region DecodeHexToBytes

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Decode each two characters in a hexadecimal string to a byte in a byte array. </summary>
        ///
        /// <param name="HexEncoded">   The hexadecimal string to decode. </param>
        ///
        /// <returns>  The decoded byte array. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static byte[] DecodeHexToBytes(string HexEncoded)
        {
            return (DecodeHexToBytes(HexEncoded, 0, HexEncoded.Length));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>  Decode each two characters in a hexadecimal string to a byte in a byte array. </summary>
        ///
        /// <param name="HexEncoded">   The hexadecimal string to decode. </param>
        /// <param name="offset">       The start position in the string. </param>
        /// <param name="len">          The number of characters in the string to decode. </param>
        ///
        /// <returns>   . </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static byte[] DecodeHexToBytes(string HexEncoded, int offset, int len)
        {
            return (DecodeHexToBytes(ASCIIEncoding.ASCII.GetBytes(HexEncoded), offset, len));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Decode each two characters in a hexadecimal byte array to a byte in a byte array.
        /// </summary>
        ///
        /// <param name="HexEncoded">   The hexadecimal encoded. </param>
        ///
        /// <returns>   The decoded byte array. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static byte[] DecodeHexToBytes(byte[] HexEncoded)
        {
            return (DecodeHexToBytes(HexEncoded, 0, HexEncoded.Length));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Decode each two characters in a hexadecimal byte array to a byte in a byte array.
        /// </summary>
        ///
        /// <param name="HexEncoded">   The hexadecimal string as an ASCII byte array to decode. </param>
        /// <param name="offset">       The start position in the byte array. </param>
        /// <param name="len">          The number of characters in the byte array to decode. </param>
        ///
        /// <returns>   The decoded byte array. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static byte[] DecodeHexToBytes(byte[] HexEncoded, int offset, int len)
        {
            if (HexEncoded == null || HexEncoded.Length < 1)
                return (null);

            //validate inputs
            //ensure the length is in even byte pairs
            if ((len % 2) != 0)
                len -= 1;           //if odd reduce length by 1

            if (len < 1)
                len = HexEncoded.Length;

            if (offset < 0)
                offset = 0;

            //calculate number of bytes to decode
            int max = (len + offset);

            //validate number of bytes to decode is not longer than max length
            if (HexEncoded.Length < max)
                max = HexEncoded.Length;

            //allocate new array as half the encoded string length
            byte[] data = new byte[len / 2];

            int x = 0;
            for (int i = offset; i < max; i += 2)
            {
                try
                {
                    //for each two characters convert to hex byte and store in new array
                    string val = ASCIIEncoding.ASCII.GetString(HexEncoded, i, 2);
                    data[x] = Convert.ToByte(val, 16);

                }
                catch (Exception)
                {
                    //on error zero byte
                    data[x] = 0;
                }

                //move to next array byte
                x++;
            }

            return (data);
        }

        #endregion DecodeHexToBytes

        #region DecodeHexToByte

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>  Decode the first two characters in a hexadecimal byte array to a byte. </summary>
        ///
        /// <param name="HexEncoded">   The hexadecimal string as an ASCII byte array to decode. </param>
        ///
        /// <returns>  The decoded byte. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static byte DecodeHexToByte(byte[] HexEncoded)
        {
            return (DecodeHexToByte(HexEncoded, 0));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>  Decode two characters in a hexadecimal byte array to a byte. </summary>
        ///
        /// <param name="HexEncoded">   The hexadecimal string as an ASCII byte array to decode. </param>
        /// <param name="offset">       The start position in the byte array. </param>
        ///
        /// <returns>  The decoded byte. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static byte DecodeHexToByte(byte[] HexEncoded, int offset)
        {
            if (HexEncoded != null && ((HexEncoded.Length - offset) > 1))
                return (DecodeHexToByte(ASCIIEncoding.ASCII.GetString(HexEncoded, offset, 2), 0));
            return (0);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>  Decode the first two characters in a hexadecimal string to a byte. </summary>
        ///
        /// <param name="HexValue">   The hexadecimal string as an ASCII byte array to decode. </param>
        ///
        /// <returns>  The decoded byte. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static byte DecodeHexToByte(string HexValue)
        {
            return (DecodeHexToByte(HexValue, 0));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>  Decode two characters in a hexadecimal string to a byte. </summary>
        ///
        /// <param name="HexValue">   The hexadecimal string to decode. </param>
        /// <param name="offset">       The start position in the string. </param>
        ///
        /// <returns>  The decoded byte. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static byte DecodeHexToByte(string HexValue, int offset)
        {
            try
            {
                if (!string.IsNullOrEmpty(HexValue) && ((HexValue.Length - offset) > 1))
                    return (Convert.ToByte(HexValue.Substring(offset, 2), 16));
            }
            catch (Exception)
            { }

            return (0);
        }

        #endregion DecodeHexToByte

        #region ParseEnum

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Parse enum. </summary>
        ///
        /// <param name="EnumType">         Type of the enum. </param>
        /// <param name="EnumValueName">    Name of the enum value. </param>
        /// <param name="DefaultValue">     An enum constant representing the default value option. </param>
        ///
        /// <returns>  The Parsed Enum or the default value on error. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static Enum ParseEnum(Type EnumType, string EnumValueName, Enum DefaultValue)
        {
            try
            {
                if (!string.IsNullOrEmpty(EnumValueName))
                {
                    Enum ret;
                    ret = Enum.Parse(EnumType, EnumValueName, true) as Enum;
                    if (ret != null)
                        return (ret);
                }
            }
            catch (Exception)
            { }

            return (DefaultValue);
        }

        #endregion ParseEnum

        #region LimitString

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Truncates a string to a given maximum length. </summary>
        ///
        /// <remarks>
        /// Set MaxLen <b>positive</b> to limit to left x characters of the string,<br />
        /// Set MaxLen <b>negative</b> to limit to right x characters of the string.
        /// </remarks>
        ///
        /// <param name="str">      The string to limit. </param>
        /// <param name="MaxLen">   maximum length of the string. </param>
        ///
        /// <returns>   The limited string. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static string LimitString(string str, int MaxLen)
        {
            int start = 0;

            if (!string.IsNullOrEmpty(str) && MaxLen != 0)
            {
                if (MaxLen < 0)
                {
                    MaxLen = Math.Abs(MaxLen);
                    start = str.Length - MaxLen;

                    if (start < 0)
                        start = 0;
                }

                if (start > 0 || (str.Length > (start + MaxLen)))
                    return (str.Substring(start, MaxLen));
            }

            return (str);
        }

        #endregion LimitString

        #region LimitBuffer

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Truncates a buffer to a maximum length. </summary>
        ///
        /// <param name="buff">     The buffer to limit. </param>
        /// <param name="MaxLen">   maximum length of the buffer. </param>
        ///
        /// <returns>  The limited buffer. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static byte[] LimitBuffer(byte[] buff, int MaxLen)
        {
            if (buff == null || (buff.Length < MaxLen + 1))
                return (buff);

            byte[] ret = new byte[MaxLen];
            Buffer.BlockCopy(buff, 0, ret, 0, ret.Length);

            return (ret);
        }

        #endregion LimitBuffer

        #region ToString

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Convert a byte array into a string. </summary>
        ///
        /// <remarks>
        /// <see cref="M:Syastem.Text.ASCIIEncoding.ASCII.GetString">Wraps
        /// ASCIIEncoding.ASCII.GetString()</see>
        /// </remarks>
        ///
        /// <param name="buff"> The buffer to convert to a string. </param>
        ///
        /// <returns>   The byte array as a string. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static string ToString(byte[] buff)
        {
            if (buff != null && buff.Length > 0)
            {
                string ret = ASCIIEncoding.ASCII.GetString(buff);
                if (ret != null)
                    return (ret.Trim('\0')); //remove null chars
            }

            return (string.Empty);
        }

        #endregion ToString

        #region DecodeBytesToDataSet

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Decode an XML byte array into a Dataset. </summary>
        ///
        /// <param name="XMLData">  The XML data to convert. </param>
        ///
        /// <returns>  A new DataSet. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static DataSet DecodeBytesToDataSet(byte[] XMLData)
        {
            return (DecodeBytesToDataSet(XMLData, XmlReadMode.InferSchema, null));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Decode an XML byte array into a Dataset. </summary>
        ///
        /// <remarks>
        /// If the passed in Dataset is <b>null</b> a new dataset will be created and returned.
        /// </remarks>
        ///
        /// <param name="XMLData">  The XML data to convert. </param>
        /// <param name="ds">       An existing Dataset to fill. </param>
        ///
        /// <returns>   The filled DataSet. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static DataSet DecodeBytesToDataSet(byte[] XMLData, DataSet ds)
        {
            return (DecodeBytesToDataSet(XMLData, XmlReadMode.InferSchema, ds));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Decode an XML byte array into a Dataset. </summary>
        ///
        /// <param name="XMLData">  The XML data to convert. </param>
        /// <param name="XmlMode">  The XML mode to use. See <see cref="T:System.Data.XmlReadMode"/>.</param>
        ///
        /// <returns>  A new DataSet. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static DataSet DecodeBytesToDataSet(byte[] XMLData, XmlReadMode XmlMode)
        {
            return (DecodeBytesToDataSet(XMLData, XmlMode, null));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Decode bytes to data set. </summary>
        ///
        /// <remarks>
        /// If the passed in Dataset is <b>null</b> a new dataset will be created and returned.
        /// </remarks>
        ///
        /// <param name="XMLData">  The XML data to convert. </param>
        /// <param name="XmlMode">  The XML mode to use. See <see cref="T:System.Data.XmlReadMode"/>. </param>
        /// <param name="ds">       An existing Dataset to fill. </param>
        ///
        /// <returns>   The filled DataSet. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static DataSet DecodeBytesToDataSet(byte[] XMLData, XmlReadMode XmlMode, DataSet ds)
        {
            //validate input
            if (XMLData != null && XMLData.Length > 0)
            {
                try
                {
                    //Create a memory stream 
                    MemoryStream XmlStream = new MemoryStream(XMLData);
                    using (XmlStream)
                    {
                        if (XmlStream.Length > 0)
                        {
                            if (ds == null)
                                ds = new DataSet();

                            if (ds != null)
                                ds.ReadXml(XmlStream, XmlMode);

                            return (ds);
                        }
                    }
                    XmlStream.Close();
                }
                catch (Exception ex)
                {
                    Logging.ExceptionLogger.Logger.LogException(MethodBase.GetCurrentMethod(), ex);
                }
            }

            return (ds);
        }

        #endregion DecodeBytesToDataSet

        #region EncodeDataSetToBytes

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Encode a dataset to a byte array. </summary>
        ///
        /// <param name="XmlSettings">  The Dataset to encode as an XML byte array. </param>
        ///
        /// <returns>  XML byte array. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static byte[] EncodeDataSetToBytes(DataSet XmlSettings)
        {
            return (EncodeDataSetToBytes(XmlSettings, XmlWriteMode.IgnoreSchema));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Encode a dataset to a byte array. </summary>
        ///
        /// <param name="XmlSettings">  The Dataset to encode as an XML byte array. </param>
        /// <param name="XmlMode">      The XML mode to use. See <see cref="T:System.Data.XmlWriteMode"/>. </param>
        ///
        /// <returns>   XML byte array. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static byte[] EncodeDataSetToBytes(DataSet XmlSettings, XmlWriteMode XmlMode)
        {
            byte[] XMLData = null;

            if (XmlSettings != null)
            {
                try
                {
                    //create a memory stream
                    MemoryStream XmlStream = new MemoryStream();
                    using (XmlStream)
                    {
                        //write dataset to stream as XML
                        XmlSettings.WriteXml(XmlStream, XmlMode);

                        //if stream is not empty then convert to byte array
                        if (XmlStream.Length > 0)
                            XMLData = XmlStream.ToArray();
                    }

                    //clean up
                    XmlStream.Close();
                }
                catch (Exception ex)
                {
                    Logging.ExceptionLogger.Logger.LogException(MethodBase.GetCurrentMethod(), ex);
                }
            }

            return (XMLData);
        }

        #endregion EncodeDataSetToBytes

        #region DecodeImageFromBytes

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Decode an image from a byte array. </summary>
        ///
        /// <param name="ImageData">    Information describing the image. </param>
        ///
        /// <returns>  Decoded Image </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static Image DecodeImageFromBytes(byte[] ImageData)
        {
            return (DecodeImageFromBytes(ImageData, false));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Decode an image from a byte array. </summary>
        ///
        /// <param name="ImageData">    Information describing the image. </param>
        /// <param name="UseEmbededColor">  <b>true</b> to use embedded colour from image data.<br/> 
        ///                                 <see cref="M:System.Drawing.Image.FromStream">Image.FromStream</see></param>
        ///
        /// <returns>  Decoded Image </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static Image DecodeImageFromBytes(byte[] ImageData, bool UseEmbededColor)
        {
            if (ImageData != null)
            {
                MemoryStream ms = new MemoryStream(ImageData);
                if (ms != null)
                {
                    using (ms)
                    {
                        return (Image.FromStream(ms, UseEmbededColor));
                    }
                }
            }

            return (null);
        }

        #endregion DecodeImageFromBytes

        #region ResizeImage

        /// <summary>   Values that represent Image Ratio as a integer value, (Ratio * 1000). </summary>
        public enum enumImageRatio
        {
            /// <summary>   An enum constant representing the ratio 1 x 1 option. </summary>
            Ratio_1x1 = (int)(1000),
            /// <summary>   An enum constant representing the ratio 4 x 3 option. </summary>
            Ratio_4x3 = (int)((4.0 / 3) * 1000),
            /// <summary>   An enum constant representing the ratio 2 x 1 option. </summary>
            Ratio_2x1 = (int)(2 * 1000),
            /// <summary>   An enum constant representing the ratio 16 x 9 option. </summary>
            Ratio_16x9 = (int)((16.0 / 9) * 1000),
            /// <summary>   An enum constant representing the ratio 1 x 2 option. </summary>
            Ratio_1x2 = (int)(0.5 * 1000),
            /// <summary>   An enum constant representing the ratio 3 x 4 option. </summary>
            Ratio_3x4 = (int)((3.0 / 4) * 1000),
            /// <summary>   An enum constant representing the ratio 9 x 16 option. </summary>
            Ratio_9x16 = (int)((9.0 / 16) * 1000)
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Resize image. </summary>
        ///
        /// <param name="CurrentImage"> The Image to resize. </param>
        /// <param name="Ratio">        The new Image ratio. </param>
        ///
        /// <returns>  The Image resized to the new ratio. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static Image ResizeImage(Image CurrentImage, enumImageRatio Ratio)
        {
            //get current image size
            int ImageHeight = CurrentImage.Height;
            int ImageWidth = CurrentImage.Width;

            //calculate new scale ratio
            double ScaleRatio = Math.Abs(((int)Ratio / 1000.0));

            //A positive ratio denotes Width / Height, a Negative ratio denotes Height / Width
            bool WidthHeightRatio = ((int)Ratio > 0);

            //Find longest side, Height or Width, and reduce, (or increase), it to fit the new Image ratio
            // Generally we will be reducing the image so selecting the longest side will be better than selecting the short side
            if (ImageHeight > ImageWidth)
            {
                //use ratio to calculate the new Height based on the current width 
                // if WidthHeightRatio 
                // then ImageHeight = ImageWidth / ratio 
                // else ImageHeight = ImageWidth * ratio
                ImageHeight = (int)(ImageWidth * (WidthHeightRatio ? (1 / ScaleRatio) : ScaleRatio));
            }
            else
            {
                //use ratio to calculate the new Width based on the current Height 
                // if WidthHeightRatio 
                // then ImageWidth = ImageHeight * ratio 
                // else ImageHeight = ImageHeight / ratio
                ImageWidth = (int)(ImageHeight * (WidthHeightRatio ? ScaleRatio : (1 / ScaleRatio)));
            }

            //resize the image to the new size
            return (ResizeImage(CurrentImage, ImageWidth, ImageHeight));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Resize image. </summary>
        ///
        /// <remarks>
        /// This uses the aspect ratio of the current Image to best fit the image inside the new image
        /// size.
        /// </remarks>
        ///
        /// <param name="CurrentImage">     The current image. </param>
        /// <param name="MaxNewImageSize">  Maximum Size of the new image. </param>
        ///
        /// <returns>   The Image resized to fit in the new size. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static Image ResizeImage(Image CurrentImage, Size MaxNewImageSize)
        {
            int NewImageHeight;
            int NewImageWidth;

            //calculate current scale ratio of image
            double ScaleRatio = (CurrentImage.Height / (double)CurrentImage.Width);

            //set width to new max width and calculate height based on current aspect ratio
            NewImageWidth = MaxNewImageSize.Width;
            NewImageHeight = (int)(NewImageWidth * ScaleRatio);

            //if calculated Height is larger than max height
            if (NewImageHeight > MaxNewImageSize.Height)
            {
                //set the Height to be max height and calculate the width
                NewImageHeight = MaxNewImageSize.Height;
                NewImageWidth = (int)(NewImageHeight * (1 / ScaleRatio));
            }

            //resize the image to best fit sizes
            return (ResizeImage(CurrentImage, NewImageWidth, NewImageHeight));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Resize image. </summary>
        ///
        /// <param name="CurrentImage"> The current image. </param>
        /// <param name="ImageWidth">   Width of the new image. </param>
        /// <param name="ImageHeight">  Height of the new image. </param>
        ///
        /// <returns>    The Image resized to the new size. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private static Image ResizeImage(Image CurrentImage, int ImageWidth, int ImageHeight)
        {
            //create a new bitmap object
            Bitmap NewImage = new Bitmap(ImageWidth, ImageHeight);

            //Create a graphics object for the new Image
            using (Graphics g = Graphics.FromImage((Image)NewImage))
            {
                //select best scaling mode
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                //resize the current image into the new bitmap
                g.DrawImage(CurrentImage, 0, 0, ImageWidth, ImageHeight);
            }

            //return new image resized to have new width and height
            return ((Image)NewImage);
        }

        #endregion ResizeImage

        #region EncodeImageToBytes

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Encode image to bytes. </summary>
        ///
        /// <param name="Img">  The image to encode. </param>
        ///
        /// <returns>   The image as a byte array. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static byte[] EncodeImageToBytes(Image Img)
        {
            return (EncodeImageToBytes(Img, ImageFormat.Jpeg));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Encode image to a byte array. </summary>
        ///
        /// <param name="Img">          The image. </param>
        /// <param name="imgFormat">    The image format to use.<br/>
        ///                             <see cref="T:System.Drawing.Imaging.ImageFormat">See
        ///                             Imaging.ImageFormat</see>. </param>
        ///
        /// <returns>   The image as a byte array. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static byte[] EncodeImageToBytes(Image Img, ImageFormat imgFormat)
        {
            if (Img != null)
            {
                MemoryStream ms = new MemoryStream();
                if (ms != null)
                {
                    using (ms)
                    {
                        Img.Save(ms, imgFormat);
                        return (ms.ToArray());
                    }
                }
            }

            return (null);
        }

        #endregion EncodeImageToBytes

    }
}
