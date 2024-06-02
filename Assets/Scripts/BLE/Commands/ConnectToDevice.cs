namespace Android.BLE.Commands
{
    /// <summary>
    /// Command to connect to a given BLE device.
    /// </summary>
    public class ConnectToDevice : BleCommand
    {
        // The UUID of the device
        protected readonly string _deviceAddress;


        // True if the BLE device is connected.
        public bool IsConnected { get => _isConnected; }
        private bool _isConnected = false;

        // A delegate that indicates a connection change on the BLE device.
        public delegate void ConnectionChange(string deviceAddress);

        // The .NET events that indicates a ConnectionChange
        public readonly ConnectionChange OnConnected, OnDisconnected;

       

        /// <summary>
        /// Connects to a BLE device with the given UUID and sends a notification
        /// if the device is connected or has disconnected.
        /// </summary>
        // onConnected : The ConnectionChange that will be triggered if the device is connected
        // onDisconnected: The ConnectionChange that will be triggered if the device has disconnected
        public ConnectToDevice(string deviceAddress,
            ConnectionChange onConnected,
            ConnectionChange onDisconnected) : base(true, true)
        {
            _deviceAddress = deviceAddress;

            OnConnected += onConnected;
            OnDisconnected += onDisconnected;
        }


        public override void Start() => BleManager.SendCommand("connectToDevice", _deviceAddress);
        public void Disconnect() => BleManager.SendCommand("disconnectDevice", _deviceAddress);


        /// <summary>
        /// Work with OnBleMessageReceived(obj) function in BleManager
        /// </summary>
        public override bool CommandReceived(BleObject obj)
        {
            if (string.Equals(obj.Device, _deviceAddress))
            {
                switch (obj.Command)
                // If message from plugin is "DeviceConnected" or "Authenticated",
                // indicating that connection is successful.
                // Function OnConnected will be triggered.
                {
                    case "DeviceConnected":
                    case "Authenticated":
                        {
                            _isConnected = true;
                            OnConnected?.Invoke(obj.Device);
                        }
                        break;
                    // If message from plugin is "DisconnectedFromGattServer",
                    // indicating that connection is disconnected.
                    // Function OnDisconnected will be triggered.
                    case "DisconnectedFromGattServer":
                        {
                            OnDisconnected?.Invoke(obj.Device);
                            BLEListCanvas._isSubscribed = false;
                        }
                        break;
                }
            }
            return false;
        }

       
    }
}