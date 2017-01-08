using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mengsk.Device.Buses.USB.Silicon
{
    public class SiliconUSBBusNative
    {
        const string DLLNAME = "SiUSBXp.dll";

        // Return codes
        public const byte SI_SUCCESS = 0x00;
        public const byte SI_DEVICE_NOT_FOUND = 0xFF;
        public const byte SI_INVALID_HANDLE = 0x01;
        public const byte SI_READ_ERROR = 0x02;
        public const byte SI_RX_QUEUE_NOT_READY = 0x03;
        public const byte SI_WRITE_ERROR = 0x04;
        public const byte SI_RESET_ERROR = 0x05;
        public const byte SI_INVALID_PARAMETER = 0x06;
        public const byte SI_INVALID_REQUEST_LENGTH = 0x07;
        public const byte SI_DEVICE_IO_FAILED = 0x08;
        public const byte SI_INVALID_BAUDRATE = 0x09;
        public const byte SI_FUNCTION_NOT_SUPPORTED = 0x0a;
        public const byte SI_GLOBAL_DATA_ERROR = 0x0b;
        public const byte SI_SYSTEM_ERROR_CODE = 0x0c;
        public const byte SI_READ_TIMED_OUT = 0x0d;
        public const byte SI_WRITE_TIMED_OUT = 0x0e;
        public const byte SI_IO_PENDING = 0x0f;

        // GetProductString() function flags
        public const byte SI_RETURN_SERIAL_NUMBER = 0x00;
        public const byte SI_RETURN_DESCRIPTION = 0x01;
        public const byte SI_RETURN_LINK_NAME = 0x02;
        public const byte SI_RETURN_VID = 0x03;
        public const byte SI_RETURN_PID = 0x04;

        // RX Queue status flags
        public const byte SI_RX_NO_OVERRUN = 0x00;
        public const byte SI_RX_EMPTY = 0x00;
        public const byte SI_RX_OVERRUN = 0x01;
        public const byte SI_RX_READY = 0x02;

        // Buffer size limits
        public const int SI_MAX_DEVICE_STRLEN = 256;
        public const int SI_MAX_READ_SIZE = 4096 * 16;
        public const int SI_MAX_WRITE_SIZE = 4096;

        // Input and Output pin Characteristics
        public const byte SI_HELD_INACTIVE = 0x00;
        public const byte SI_HELD_ACTIVE = 0x01;
        public const byte SI_FIRMWARE_CONTROLLED = 0x02;
        public const byte SI_RECEIVE_FLOW_CONTROL = 0x02;
        public const byte SI_TRANSMIT_ACTIVE_SIGNAL = 0x03;
        public const byte SI_STATUS_INPUT = 0x00;
        public const byte SI_HANDSHAKE_LINE = 0x01;

        // Mask and Latch value bit definitions
        public const byte SI_GPIO_0 = 0x01;
        public const byte SI_GPIO_1 = 0x02;
        public const byte SI_GPIO_2 = 0x04;
        public const byte SI_GPIO_3 = 0x08;

        // GetDeviceVersion() return codes
        public const byte SI_CP2101_VERSION = 0x01;
        public const byte SI_CP2102_VERSION = 0x02;
        public const byte SI_CP2103_VERSION = 0x03;
        public const byte SI_CP2104_VERSION = 0x04;

        [DllImport(DLLNAME)]
        public static extern int SI_GetNumDevices(ref uint lpdwNumDevices);

        [DllImport(DLLNAME)]
        public static extern int SI_GetProductString(uint dwDeviceNum, [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpvDeviceString, uint dwFlags);

        [DllImport(DLLNAME)]
        public static extern int SI_Open(uint dwDevice, ref IntPtr cyHandle);

        [DllImport(DLLNAME)]
        public static extern int SI_Close(IntPtr cyHandle);

        [DllImport(DLLNAME)]
        public static extern int SI_Read(IntPtr cyHandle, byte[] lpBuffer, uint dwBytesToRead, ref uint lpdwBytesReturned, IntPtr o);

        [DllImport(DLLNAME)]
        public static extern int SI_Write(IntPtr cyHandle, byte[] lpBuffer, uint dwBytesToWrite, ref uint lpdwBytesWritten, IntPtr o);

        [DllImport(DLLNAME)]
        public static extern int SI_DeviceIOControl(IntPtr cyHandle, uint dwIoControlCode, byte[] lpInBuffer, uint dwBytesToRead, byte[] lpOutBuffer, uint dwBytesToWrite, ref uint lpdwBytesSucceeded);

        [DllImport(DLLNAME)]
        public static extern int SI_FlushBuffers(IntPtr cyHandle, byte FlushTransmit, byte FlushReceive);

        [DllImport(DLLNAME)]
        public static extern int SI_SetTimeouts(uint dwReadTimeout, uint dwWriteTimeout);

        [DllImport(DLLNAME)]
        public static extern int SI_GetTimeouts(ref uint lpdwReadTimeout, ref uint lpdwWriteTimeout);

        [DllImport(DLLNAME)]
        public static extern int SI_CheckRXQueue(IntPtr cyHandle, ref uint lpdwNumBytesInQueue, ref uint lpdwQueueStatus);

        [DllImport(DLLNAME)]
        public static extern int SI_SetBaudRate(IntPtr cyHandle, uint dwBaudRate);

        [DllImport(DLLNAME)]
        public static extern int SI_SetBaudDivisor(IntPtr cyHandle, ushort wBaudDivisor);

        [DllImport(DLLNAME)]
        public static extern int SI_SetLineControl(IntPtr cyHandle, ushort wLineControl);

        [DllImport(DLLNAME)]
        public static extern int SI_SetFlowControl(IntPtr cyHandle, byte bCTS_MaskCode, byte bRTS_MaskCode, byte bDTR_MaskCode, byte bDSR_MaskCode, byte bDCD_MaskCode, bool bFlowXonXoff);

        [DllImport(DLLNAME)]
        public static extern int SI_GetModemStatus(IntPtr cyHandle, ref byte ModemStatus);

        [DllImport(DLLNAME)]
        public static extern int SI_SetBreak(IntPtr cyHandle, ushort wBreakState);

        [DllImport(DLLNAME)]
        public static extern int SI_ReadLatch(IntPtr cyHandle, ref byte lpbLatch);

        [DllImport(DLLNAME)]
        public static extern int SI_WriteLatch(IntPtr cyHandle, byte bMask, byte bLatch);

        [DllImport(DLLNAME)]
        public static extern int SI_GetPartNumber(IntPtr cyHandle, ref byte lpbPartNum);

        [DllImport(DLLNAME)]
        public static extern int SI_GetDeviceProductString(IntPtr cyHandle, byte[] lpProduct, ref byte lpbLength, bool bConvertToASCII);

        [DllImport(DLLNAME)]
        public static extern int SI_GetDLLVersion(ref uint HighVersion, ref uint LowVersion);

        [DllImport(DLLNAME)]
        public static extern int SI_GetDriverVersion(ref uint HighVersion, ref uint LowVersion);


        public static string FormartErrorCode(int errorCode)
        {
            switch (errorCode)
            {
                case SI_SUCCESS:
                    return "SI_SUCCESS";
                case SI_DEVICE_NOT_FOUND:
                    return "SI_DEVICE_NOT_FOUND";
                case SI_INVALID_HANDLE:
                    return "SI_INVALID_HANDLE";
                case SI_READ_ERROR:
                    return "SI_READ_ERROR";
                case SI_RX_QUEUE_NOT_READY:
                    return "SI_RX_QUEUE_NOT_READY";
                case SI_WRITE_ERROR:
                    return "SI_WRITE_ERROR";
                case SI_RESET_ERROR:
                    return "SI_RESET_ERROR";
                case SI_INVALID_PARAMETER:
                    return "SI_INVALID_PARAMETER";
                case SI_INVALID_REQUEST_LENGTH:
                    return "SI_INVALID_REQUEST_LENGTH";
                case SI_DEVICE_IO_FAILED:
                    return "SI_DEVICE_IO_FAILED";
                case SI_INVALID_BAUDRATE:
                    return "SI_INVALID_BAUDRATE";
                case SI_FUNCTION_NOT_SUPPORTED:
                    return "SI_FUNCTION_NOT_SUPPORTED";
                case SI_GLOBAL_DATA_ERROR:
                    return "SI_GLOBAL_DATA_ERROR";
                case SI_SYSTEM_ERROR_CODE:
                    return "SI_SYSTEM_ERROR_CODE";
                case SI_READ_TIMED_OUT:
                    return "SI_READ_TIMED_OUT";
                case SI_WRITE_TIMED_OUT:
                    return "SI_WRITE_TIMED_OUT";
                case SI_IO_PENDING:
                    return "SI_IO_PENDING";
                default:
                    return errorCode.ToString();
            }
        }

        public static string[] SI_GetAllDeviceProductString(uint dwFlags)
        {
            uint deviceNums = 0;
            int ret = SI_GetNumDevices(ref deviceNums);
            if (ret == SI_SUCCESS)
            {
                string[] productStrings = new string[(int)deviceNums];
                StringBuilder productString = new StringBuilder(SI_MAX_DEVICE_STRLEN);
                for (uint i = 0; i < deviceNums; i++)
                {
                    ret = SI_GetProductString(i, productString, dwFlags);
                    if (ret == SI_SUCCESS)
                    {
                        productStrings[i] = productString.ToString();
                    }
                    else if (ret == SI_DEVICE_NOT_FOUND)
                    {

                    }
                    else
                    {
                        throw new Exception(FormartErrorCode(ret) + " with device number: " + i);
                    }
                }
                return productStrings;
            }

            if (ret == SI_DEVICE_NOT_FOUND)
            {
                return new string[0];
            }
            throw new Exception(FormartErrorCode(ret));
        }
    }
}
