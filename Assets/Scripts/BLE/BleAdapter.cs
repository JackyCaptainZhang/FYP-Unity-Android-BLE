﻿using Android.BLE.Events;
using UnityEngine;
using System.Text;
using System;

namespace Android.BLE
{
    /// <summary>
    /// The adapter between the Java library and Unity's .NET environment.
    /// </summary>
    public class BleAdapter : MonoBehaviour
    {
        // .NET Events
        public event MessageReceived OnMessageReceived;
        public event ErrorReceived OnErrorReceived;

        // Unity Events
        public BleMessageReceived UnityOnMessageReceived;
        public BleErrorReceived UnityOnErrorReceived;

        /// <summary>
        /// Sets the name to "BleAdapter" to receive messages from the Java library.
        /// </summary>
        private void Awake() => gameObject.name = nameof(BleAdapter);

        /// <summary>
        /// The method that the Java library will send their JSON messages to.
        /// </summary>
        /// <param name="jsonMessage">The <see cref="BleObject"/> in JSON format.</param>
        public void OnBleMessage(string jsonMessage)
        {
            Debug.Log("Received JSON: " + jsonMessage);
            BleObject obj = JsonUtility.FromJson<BleObject>(jsonMessage);
            if (obj.HasError)
            {
                OnErrorReceived?.Invoke(obj.ErrorMessage);
                UnityOnErrorReceived?.Invoke(obj.ErrorMessage);
                Debug.LogError("Error: " + obj.ErrorMessage);
            }
            else
            {
                OnMessageReceived?.Invoke(obj);
                UnityOnMessageReceived?.Invoke(obj);
                string decodedMessage = "";
                try
                {
                    byte[] data = Convert.FromBase64String(obj.Base64Message);  // Base64 decoding
                    decodedMessage = Encoding.UTF8.GetString(data);  // change to string message
                }
                catch (FormatException fe)
                {
                    Debug.LogError("Base64 String could not be decoded: " + fe.Message);
                }

                Debug.Log("Received Message: " + decodedMessage);
            }
        }

        public void LogMessage(string log) => Debug.Log(log);

        public delegate void MessageReceived(BleObject obj);
        public delegate void ErrorReceived(string errorMessage);
    }
}