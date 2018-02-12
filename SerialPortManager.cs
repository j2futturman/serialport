using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serialport
{
    public static class SerialPortManager
    {
        /// <summary>
        /// Your port name
        /// </summary>
        private const string PortName = "COM4";

        /// <summary>
        /// Your port baund rate
        /// </summary>
        private const int PortBaudRate = 115200;

        /// <summary>
        /// Create a new serial port
        /// </summary>
        private static SerialPort port = new SerialPort(PortName, PortBaudRate);

        /// <summary>
        /// Return value of your port that you want to catch
        /// </summary>
        public static string ReturnValue;

        /// <summary>
        /// When serial port started, it's pushing some values that you wrote. If it is first opening then send start command to port.
        /// </summary>
        public static bool IsFirstSending = true;

        /// <summary>
        /// We listen to port as async, this event will send in async method
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isCompleted"></param>
        /// <param name="isSuccess"></param>
        public delegate void MessageReceived(string message, bool isCompleted, bool isSuccess);
        public static event MessageReceived OnMessageReceived;

        /// <summary>
        /// Open port and start listening
        /// </summary>
        public static void Open()
        {
            ReturnValue = string.Empty;
            Close();
            port.Open();
            ListenPort();
            port.DataReceived += Port_DataReceived;
        }

        /// <summary>
        /// Close port
        /// </summary>
        public static void Close()
        {
            if (port != null)
            {
                if (port.IsOpen)
                {
                    port.Close();
                }
            }
        }

        /// <summary>
        /// Serial port data received method. Send start command when it is first opening
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (IsFirstSending)
            {
                port.Write("start");
                IsFirstSending = false;
            }
        }

        /// <summary>
        /// Listen port as async.
        /// </summary>
        /// <returns></returns>
        static async Task ListenPortAsyncTask()
        {
            while (true)
            {
                // Read port value
                string message = port.ReadLine();
                // Throw event
                if (OnMessageReceived != null)
                {
                    if (message.Contains("ReturnValue:"))
                    {
                        // This the string that i want to catch before process finished
                        // Take value and continue for finish
                        ReturnValue = message;
                    }
                    else if (message.Contains("Finished"))
                    {
                        // Finish process as success
                        OnMessageReceived(message, true, true);
                        break;
                    }
                    else if (message.Contains("Error"))
                    {
                        // Finish process as fail
                        OnMessageReceived(message, true, false);
                        break;
                    }
                    else
                    {
                        // On other causes continue reading
                        OnMessageReceived(message, false, false);
                    }
                }
            }
        }

        /// <summary>
        /// Call async method
        /// </summary>
        static async void ListenPort()
        {
            await Task.Run(() => ListenPortAsyncTask());
        }
    }
}
