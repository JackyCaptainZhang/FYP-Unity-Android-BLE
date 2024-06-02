using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Android.BLE;
using Android.BLE.Commands;
using UnityEngine.Android;
using System.Text;
using UnityEngine.UI;


public class BLEListCanvas : MonoBehaviour
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
    private Canvas modeSelectCanvas;
    [SerializeField]
    private Canvas bleListCanvas;
    [SerializeField]
    private int _scanTime = 5;

    private float _scanTimer = 0f;
    private bool _isScanning = false;
    private ReadFromCharacteristic _readFromCharacteristic;
    private SubscribeToCharacteristic _subscribeToCharacteristic;
    public static bool _isSubscribed = false;

    /* Replace these Characteristics with YOUR device's characteristics
       "0000" (Service UUID) and "0001"(Characteristics UUID) is a part of "0000" + service + "-0000-1000-8000-00805f9b34fb" by default. */
    private string service_UUID = "0000";
    private string characteristic_UUID = "0001";


    // Update is called once per frame
    void Update()
    {
        if (_isScanning)
        {
            _scanTimer += Time.deltaTime;  // Set the timer
            if (_scanTimer > _scanTime)
            {
                _scanTimer = 0f;
                _isScanning = false;
            }
        }

        if (!_isSubscribed)
        {
            _buttonText.text = "Enter Game";
        }
        
        if (!DeviceButton._isConnected)
        {
            _buttonText.text = "Scan";
            _deviceList.gameObject.SetActive(true);
            _scanbutton.gameObject.SetActive(true);
            _isSubscribed = false;
        }
       

    }



    public void DeviceConfigureClick()
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
            bleListCanvas.gameObject.SetActive(false);
            modeSelectCanvas.gameObject.SetActive(true);
        }
    }



    #region BLE functions

    /// <summary>
    /// Function that scan for device and update the BLE device list.
    /// </summary>
    void ScanForDevices()
    {
        if (!_isScanning)
        {
            _isScanning = true;
            /* Queue command for DiscoverDevices
               Bind OnDeviceFound() to Event OnDeviceDiscovered
               Send _scanTime to system */
            BleManager.Instance.QueueCommand(new DiscoverDevices(OnDeviceFound, _scanTime * 1000));
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

    /// <summary>
    /// Subscribe function.
    /// </summary>
    void SubscribeToExampleService()
    {
        _subscribeToCharacteristic = new SubscribeToCharacteristic(DeviceButton.connectted_deviceUuid, service_UUID, characteristic_UUID);
        _subscribeToCharacteristic.Start();
        _isSubscribed = true;
        bleListCanvas.gameObject.SetActive(false);
        modeSelectCanvas.gameObject.SetActive(true);
    }

    /// <summary>
    /// Unsubscribe function.
    /// </summary>
    void UnsubscribeFromExampleService()
    {
        _isSubscribed = false;
        _subscribeToCharacteristic.End();
    }


    #endregion


}
