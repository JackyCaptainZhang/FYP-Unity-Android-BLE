using Android.BLE.Events;
using UnityEngine;
using System.Text;
using System;

namespace Android.BLE
{
    /// <summary>
    /// The adapter between the Java library and Unity's .NET event environment.
    /// This adapter is bound to BleManager
    /// </summary>
    public class BleAdapter : MonoBehaviour
    {
        public delegate void MessageReceived(BleObject obj);
        public delegate void ErrorReceived(string errorMessage);

        // Unity .NET Events
        public event MessageReceived OnMessageReceived;
        public event ErrorReceived OnErrorReceived;

        public static string decodedMessage = "";
        private StringBuilder messageBuffer = new StringBuilder();

        /// <summary>
        /// Sets the name to "BleAdapter" to receive messages from the Java library.
        /// </summary>
        private void Awake() => gameObject.name = nameof(BleAdapter);

        /// <summary>
        /// The method that the Java plugin will send their JSON messages to.
        /// All objects are sent in JSON format.
        /// </summary>
        public void OnBleMessage(string jsonMessage)
        {
            LogMessage("Received JSON: " + jsonMessage);

            BleObject obj = JsonUtility.FromJson<BleObject>(jsonMessage);

            // Trigger the OnErrorReceived Event if there is error message in received obj
            // All functions that are bound to OnErrorReceived Event will be called
            if (obj.HasError)
            {
                OnErrorReceived?.Invoke(obj.ErrorMessage);
                LogError("Error: " + obj.ErrorMessage);
            }
            // Trigger the OnMessageReceived Event if there is no error message in received obj
            // All functions that are bound to OnMessageReceived Event will be called
            else
            {
                OnMessageReceived?.Invoke(obj);

                // Decode the base64 message in obj into string
                try
                {
                    byte[] data = obj.GetByteMessage();
                    string decodedPart = Encoding.UTF8.GetString(data).Replace("\n", "").Replace("\r", "");  // Remove new lines
                    messageBuffer.Append(decodedPart);
                    //LogMessage("Current Buffer: " + messageBuffer.ToString());

                    // Check if the message contains the end marker
                    if (decodedPart.Contains("+END"))
                    {
                        decodedMessage = messageBuffer.ToString();
                        messageBuffer.Clear();  // Clear the buffer for the next message

                        // Process the decoded message to extract hex values
                        ExtractHexValues(decodedMessage);
                    }
                }
                catch (FormatException fe)
                {
                    LogError("Base64 String could not be decoded: " + fe.Message);
                }
            }
        }

        private void ExtractHexValues(string message)
        {
            string cleanedMessage = message.Replace("START+", "").Replace("+END", "").Replace("+", "");
            //LogMessage("Cleaned Message: " + cleanedMessage);

            if (cleanedMessage.Length == 16)
            {
                string hex1 = cleanedMessage.Substring(0, 8);
                string hex2 = cleanedMessage.Substring(8, 8);
                LogMessage("Hex1: " + hex1);
                LogMessage("Hex2: " + hex2);

                float float1 = HexToFloat(hex1);
                float float2 = HexToFloat(hex2);

                // Store the values in the public static class
                BleDataStorage.Float1 = float1;
                BleDataStorage.Float2 = float2;

                LogMessage("Float1: " + float1);
                LogMessage("Float2: " + float2);
            }
            else
            {
                LogError("The message does not contain enough data for two hex values.");
            }
        }

        private float HexToFloat(string hex)
        {
            uint num = uint.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            byte[] floatVals = BitConverter.GetBytes(num);
            return BitConverter.ToSingle(floatVals, 0);
        }

        public void LogMessage(string log) => Debug.Log(log);
        public void LogError(string log) => Debug.LogError(log);
    }
}