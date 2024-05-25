using UnityEngine;
using Android.BLE;
using Android.BLE.Commands;
using UnityEngine.Android;
using System.Text;
using UnityEngine.UI;

/// <summary>
/// This script mainly focus on the UI logic
/// </summary>

public class ExampleBleInteractor : MonoBehaviour
{
    [SerializeField]
    private GameObject _deviceButton;
    [SerializeField]
    private Transform _deviceList_item;
    [SerializeField]
    private Transform _deviceList;
    [SerializeField]
    private Transform _scanbutton;
    [SerializeField]
    private Text _buttonText;

    [SerializeField]
    private int _scanTime = 5;

    [SerializeField]
    private Text dataDisplayBox;

    [SerializeField]
    public Camera cameraA;
    [SerializeField]
    public Camera cameraB;

    private float _scanTimer = 0f;

    private bool _isScanning = false;
    private ReadFromCharacteristic _readFromCharacteristic;
    private SubscribeToCharacteristic _subscribeToCharacteristic;
    public static bool _isSubscribed = false;


    public void ScanForDevices()
    {
        if (!_isScanning)
        {
            _isScanning = true;
            // Queue command for DiscoverDevices
            // Bind OnDeviceFound() to Event OnDeviceDiscovered
            // Send _scanTime to system
            BleManager.Instance.QueueCommand(new DiscoverDevices(OnDeviceFound, _scanTime * 1000));
        }
    }

    /// <summary>
    /// Define different click actions for different senerio
    /// </summary>
    public void onClick()
    {
        if (!DeviceButton._isConnected)
        {
            ScanForDevices();
        }
        else if (!_isSubscribed)
        {
            SubscribeToExampleService();
        }
        else
        {
            UnsubscribeFromExampleService();
        }
    }

    public void SubscribeToExampleService()
    {
        // Replace these Characteristics with YOUR device's characteristics
        // "0000" (Service UUID) and "0001"(Characteristics UUID) is a part of "0000" + service + "-0000-1000-8000-00805f9b34fb" by default.
        _isSubscribed = true;
        _subscribeToCharacteristic = new SubscribeToCharacteristic(DeviceButton.connectted_deviceUuid, "0000", "0001");
        _subscribeToCharacteristic.Start();
    }

    public void UnsubscribeFromExampleService() // Unsubscribe function
    {
        _isSubscribed = false;
        _subscribeToCharacteristic.End();
    }


    private void Start()
    {
       
    }


    private void Update()
    {
        if(_isScanning)
        {
            _scanTimer += Time.deltaTime;  // Set the timer
            if(_scanTimer > _scanTime)
            {
                _scanTimer = 0f;
                _isScanning = false;
            }
        }

        if (_isSubscribed)
        {
            _buttonText.text = "Unsubscribe";
            cameraA.enabled = true;
            cameraB.enabled = false;
            dataDisplayBox.text = BleAdapter.decodedMessage; // Display the received message from BleAdapter
        }
        else
        {
            _buttonText.text = "Subscribe";
            cameraA.enabled = false;
            cameraB.enabled = true;
        }

        if (DeviceButton._isConnected) {
            
        }
        else
        {
            _buttonText.text = "Scan";
            _deviceList.gameObject.SetActive(true);
            _scanbutton.gameObject.SetActive(true);
            cameraA.enabled = false;
            cameraB.enabled = true;
        }
    }

    /// <summary>
    /// Function that bound to Event OnDeviceDiscovered.
    /// </summary>
    private void OnDeviceFound(string name, string device)  
    {
        DeviceButton button = Instantiate(_deviceButton, _deviceList_item).GetComponent<DeviceButton>();
        button.Show(name, device);
       
    }
}
