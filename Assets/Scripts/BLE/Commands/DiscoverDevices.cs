using System;

namespace Android.BLE.Commands
{
    /// <summary>
    /// Command to start scanning discovering BLE devices
    /// </summary>
    public class DiscoverDevices : BleCommand
    {

        // Default time that this command will search BLE devices for (in milliseconds).
        private const int StandardDiscoverTime = 10000;

        // The time that this command will search BLE devices for.
        private int _discoverTime;

        // A delegate that indicates a newly discovered BLE device.
        public delegate void DeviceDiscovered(string deviceAddress, string deviceName);

        // The .NET Event that indicates a new BLE device is discovered.
        public readonly DeviceDiscovered OnDeviceDiscovered;


        /// <summary>
        /// Bind the incomming function to Event OnDeviceDiscovered, so the function can be called
        /// when the new device is discovered.
        /// Incomming function OnDeviceDiscovered must include name and UUID of the discovered devices.
        /// </summary>
        public DiscoverDevices(Action<string, string> onDeviceDiscovered, int discoverTime = StandardDiscoverTime) : base(true, false)
        {
            OnDeviceDiscovered += new DeviceDiscovered(onDeviceDiscovered);
            _discoverTime = discoverTime;
        }

        public override void Start() => BleManager.SendCommand("scanBleDevices", _discoverTime);

        public override void End() => BleManager.SendCommand("stopScanBleDevices");

        /// <summary>
        /// Work with OnBleMessageReceived(obj) function in BleManager
        /// </summary>
        public override bool CommandReceived(BleObject obj)
        {
            // The Event OnDeviceDiscovered will be triggered in loop
            // if the message from plugin is "DiscoveredDevice"
            if (string.Equals(obj.Command, "DiscoveredDevice"))
                OnDeviceDiscovered?.Invoke(obj.Device, obj.Name);
            // will only stop the event if the message from plugin is "FinishedDiscovering"
            return string.Equals(obj.Command, "FinishedDiscovering");
            // So all the discovered device can be listed in the game
        }
    }
}