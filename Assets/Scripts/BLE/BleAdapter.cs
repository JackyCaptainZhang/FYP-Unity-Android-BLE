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
                    decodedMessage = Encoding.UTF8.GetString(data);  // change to string message
                }
                catch (FormatException fe)
                {
                    LogError("Base64 String could not be decoded: " + fe.Message);
                }
                if (!string.Equals(decodedMessage, "")) // Print out the message if it is not empty
                {
                    LogMessage("Received Message: " + decodedMessage); 
                }
            }
        }

        public void LogMessage(string log) => Debug.Log(log);
        public void LogError(string log) => Debug.LogError(log);


    }
}