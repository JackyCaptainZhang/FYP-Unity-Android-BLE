﻿using Android.BLE.Extension;

namespace Android.BLE.Commands
{
    /// <summary>
    /// Command to read from a given Characteristic.
    /// </summary>
    public class ReadFromCharacteristic : BleCommand
    {
        
        // The UUID of the BLE device. 
        public readonly string DeviceAddress;


        // The Service that parents the Characteristic.
        public readonly string Service;

        // The Characteristic to write the message to.
        public readonly string Characteristic;

        // A delegate that indicates a read value from a Characteristic.
        public delegate void ReadCharacteristicValueReceived(byte[] value);

        // The .NET event that sends the read data back to the user.
        public ReadCharacteristicValueReceived OnReadCharacteristicValueReceived;

        // Indicates if the UUID is custom (long-uuid instead of a short-hand).
        public readonly bool CustomGatt;

        /// <summary>
        /// Reads from a given BLE Characteristic.
        /// </summary>
        /// <param name="deviceAddress">The UUID of the device that the BLE should read from.</param>
        /// <param name="serviceAddress">The UUID of the Service that parents the Characteristic.</param>
        /// <param name="characteristicAddress">The UUID of the Characteristic to read from.</param>
        /// <param name="valueReceived">The <see cref="ReadCharacteristicValueReceived"/> that will trigger if a value was read from the Characteristic.</param>
        /// <param name="customGatt"><see langword="true"/> if the GATT Characteristic UUID address is a long-hand, not short-hand.</param>
        public ReadFromCharacteristic(string deviceAddress, string serviceAddress, string characteristicAddress, ReadCharacteristicValueReceived valueReceived, bool customGatt = false) : base(false, false)
        {
            DeviceAddress = deviceAddress;
            Service = serviceAddress;
            Characteristic = characteristicAddress;

            OnReadCharacteristicValueReceived = valueReceived;

            CustomGatt = customGatt;

            _timeout = 1f;
        }

        public override void Start()
        {
            string command = CustomGatt ? "readFromCustomCharacteristic" : "readFromCharacteristic";
            BleManager.SendCommand(command, DeviceAddress, Service, Characteristic);
        }

        public override bool CommandReceived(BleObject obj)
        {
            if (string.Equals(obj.Command, "ReadFromCharacteristic"))
            {
                if ((!CustomGatt && string.Equals(obj.Characteristic.Get4BitUuid(), Characteristic) && string.Equals(obj.Service.Get4BitUuid(), Service))
                    || (CustomGatt && string.Equals(obj.Characteristic, Characteristic) && string.Equals(obj.Service, Service)))
                {
                    OnReadCharacteristicValueReceived?.Invoke(obj.GetByteMessage());
                    return true;
                }
            }

            return false;
        }
  
    }
}