using Android.BLE.Extension;

namespace Android.BLE.Commands
{
    /// <summary>
    /// Command to Subscribe to a BLE Device's Characteristic
    /// </summary>
    public class SubscribeToCharacteristic : BleCommand
    {
        // UUID of the BLE device.
        public readonly string DeviceAddress;

        // The Service
        public readonly string Service;

        // The Characteristic to write the message to.
        public readonly string Characteristic;

        // We use long version of UUID so it muast be true.
        private readonly bool CustomGatt = true;

        /// <summary>
        /// Subscribes to a given BLE Characteristic.
        /// </summary>
        public SubscribeToCharacteristic(string deviceAddress, string service, string characteristic, bool customGatt = false) : base(true, true)
        {
            DeviceAddress = deviceAddress;
            Service = service;
            Characteristic = characteristic;

        }


        /// <summary>
        /// Start/Subscribe to given characteristic.
        /// </summary>
        public override void Start()
        {
            string command = CustomGatt ? "subscribeToCustomGattCharacteristic" : "subscribeToGattCharacteristic";
            BleManager.SendCommand(command, DeviceAddress, Service, Characteristic);
        }

        /// <summary>
        /// End/Unsubscribe from connected characteristic.
        /// </summary>
        public override void End()
        {
            string command = CustomGatt ? "unsubscribeFromCustomGattCharacteristic" : "unsubscribeFromGattCharacteristic";
            BleManager.SendCommand(command, DeviceAddress, Service, Characteristic);
        }

    }
    }
