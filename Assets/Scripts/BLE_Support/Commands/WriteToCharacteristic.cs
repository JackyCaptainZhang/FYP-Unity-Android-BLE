using System.Text;

namespace Android.BLE.Commands
{
    /// <summary>
    /// Command to write to a BLE Device's Characteristic
    /// </summary>
    public class WriteToCharacteristic : BleCommand
    {
        // The UUID of the BLE device.
        public readonly string DeviceAddress;

        
        // The Service that parents the Characteristic.
        public readonly string Service;

        // The Characteristic to write the message to.
        public readonly string Characteristic;

        // The data that's send encoded in Base64.
        public readonly string Base64Data;

        // Indicates if the UUID is custom (long-uuid instead of a short-hand).
        public readonly bool CustomGatt;

        /// <summary>
        /// Writes to a given BLE Characteristic with the Base64 string <paramref name="data"/>.
        /// </summary>
        public WriteToCharacteristic(string deviceAddress, string serviceAddress, string characteristicAddress, string data, bool customGatt = false) : base(false, false)
        {
            DeviceAddress = deviceAddress;
            Service = serviceAddress;
            Characteristic = characteristicAddress;

            Base64Data = data;

            CustomGatt = customGatt;

            _timeout = 1f;
        }



        public override void Start()
        {
            string command = CustomGatt ? "writeToCustomGattCharacteristic" : "writeToGattCharacteristic";
            BleManager.SendCommand(command, DeviceAddress, Service, Characteristic, Base64Data);
        }
    }
}