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


        // Subscribes to a given BLE Characteristic.
        public SubscribeToCharacteristic(string deviceAddress, string service, string characteristic, bool customGatt = false) : base(true, true)
        {
            DeviceAddress = deviceAddress;
            Service = service;
            Characteristic = characteristic;

        }



        public override void Start()
        {
            string command = "subscribeToGattCharacteristic";
            BleManager.SendCommand(command, DeviceAddress, Service, Characteristic);
        }

        public override void End()
        {
            string command = "unsubscribeFromGattCharacteristic";
            BleManager.SendCommand(command, DeviceAddress, Service, Characteristic);
        }

        }
    }
