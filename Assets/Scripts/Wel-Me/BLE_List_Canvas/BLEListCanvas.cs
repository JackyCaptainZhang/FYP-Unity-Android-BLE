using UnityEngine;
using Android.BLE;
using Android.BLE.Commands;
using UnityEngine.UI;


public class BLEListCanvas : MonoBehaviour
{

    public GameObject _deviceButton;
    public Transform _deviceList_item;
    public Transform _deviceList;
    public Transform _scanbutton;
    public Text _buttonText;
    public Canvas CalibrationCanvas;
    public Canvas bleListCanvas;
    public int _scanTime = 5;

    private float _scanTimer = 0f;
    private bool _isScanning = false;
    private SubscribeToCharacteristic _subscribeToCharacteristic;
    public static bool _isSubscribed = false;
    private GameManager gameManager;


    private void OnEnable()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Update the UI elements for BLE List canvas.
    /// </summary>
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
            _buttonText.text = "Enter Calibration";
        }
        
        if (!DeviceButton._isConnected)
        {
            _buttonText.text = "Scan";
            _deviceList.gameObject.SetActive(true);
            _scanbutton.gameObject.SetActive(true);
            _isSubscribed = false;
        }
       

    }


    /// <summary>
    /// Function for the multi-functional button in BLE List Canvas (Scan/Subscribe).
    /// </summary>
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
        _subscribeToCharacteristic = new SubscribeToCharacteristic(DeviceButton.connectted_deviceUuid, GameManager.service_UUID, GameManager.characteristic_Notify_UUID, true);
        _subscribeToCharacteristic.Start();
        _isSubscribed = true;
        gameManager.calibrateBTN();
    }

    #endregion


}
