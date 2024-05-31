using UnityEngine;
using Android.BLE;
using Android.BLE.Commands;
using UnityEngine.Android;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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
    private GameObject DataDisplay_Button;
    [SerializeField]
    private GameObject ROM_Button;
    [SerializeField]
    private GameObject EMG_Button;
    [SerializeField]
    private GameObject Back_Button;


    [SerializeField]
    private int _scanTime = 5;

    [SerializeField]
    private Text dataDisplayBox;

    [SerializeField]
    private Camera game_Cam;
    [SerializeField]
    private Camera menu_Cam;
    [SerializeField]
    private Canvas gameCanvas;
    [SerializeField]
    private Canvas menuCanvas;


    private float _scanTimer = 0f;

    private bool _isScanning = false;
    private ReadFromCharacteristic _readFromCharacteristic;
    private SubscribeToCharacteristic _subscribeToCharacteristic;
    private WriteToCharacteristic _writeToCharacteristic;
    public static bool _isSubscribed = false;
    private bool dataDisplayMode = false;
    private bool EMGGameMode = false;
    private bool ROMGameMode = false;
    private bool BacktoMenu = false;


    /* Replace these Characteristics with YOUR device's characteristics
       "0000" (Service UUID) and "0001"(Characteristics UUID) is a part of "0000" + service + "-0000-1000-8000-00805f9b34fb" by default. */
    private string service_UUID = "0000";
    private string characteristic_UUID = "0001";


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
    /// Define different click actions for different senerio
    /// </summary>
    #region Button Controllers

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


    public void OnDataDisplayButtonClicked()
    {
        BLEDeviceSTARTSendingData();
        Debug.Log("Data Display Button was clicked!");
        dataDisplayMode = true;
        EMGGameMode = false;
        ROMGameMode = false;
        BacktoMenu = false;
    }

    public void OnROMButtonClicked()
    {
        BLEDeviceSTARTSendingData();
        Debug.Log("ROM Button was clicked!");
        ROMGameMode = true;
        dataDisplayMode = false;
        EMGGameMode = false;
        BacktoMenu = false;
    }

    public void OnEMGButtonClicked()
    {
        BLEDeviceSTARTSendingData();
        Debug.Log("EMG Button was clicked!");
        EMGGameMode = true;
        dataDisplayMode = false;
        ROMGameMode = false;
        BacktoMenu = false;
    }

    public void goBackToMenu()
    {
        BLEDeviceSTOPSendingData();
        Debug.Log("Menu Button was clicked!");
        BacktoMenu = true;
        EMGGameMode = false;
        dataDisplayMode = false;
        ROMGameMode = false;
    }


    #endregion

    /// <summary>
    /// Call BLE functions
    /// </summary>
    #region BLE functions

    void SubscribeToExampleService() // Subscribe function
    {
        _isSubscribed = true;
        _subscribeToCharacteristic = new SubscribeToCharacteristic(DeviceButton.connectted_deviceUuid, service_UUID, characteristic_UUID);
        _subscribeToCharacteristic.Start();
    }

    void UnsubscribeFromExampleService() // Unsubscribe function
    {
        _isSubscribed = false;
        _subscribeToCharacteristic.End();
    }


    void BLEDeviceSTARTSendingData()
    {
        _writeToCharacteristic = new WriteToCharacteristic(DeviceButton.connectted_deviceUuid, service_UUID, characteristic_UUID, "Start sending data");
        _writeToCharacteristic.Start();
    }

    void BLEDeviceSTOPSendingData()
    {
        _writeToCharacteristic = new WriteToCharacteristic(DeviceButton.connectted_deviceUuid, service_UUID, characteristic_UUID, "Stop sending data");
        _writeToCharacteristic.Start();
    }

    #endregion

    private void Start()
    {
        menu_Cam.enabled = true;
        game_Cam.enabled = false;
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
            ModeChooseUIScene();
        }
        else
        {
            OnUnsubscribedUIScene();
        }


        if (!DeviceButton._isConnected)
        {
            
            OnDisconnectedUIScene();
            _isSubscribed = false;
        }
        

        if (dataDisplayMode)
        {
            DataDisplayUIScene();
        }

        if (EMGGameMode)
        {
            EMGGameUIScene();
        }

        if (ROMGameMode)
        {
            ROMGameUIScene();
        }

        if (BacktoMenu)
        {
            ModeChooseUIScene();
        }
    }



    /// <summary>
    /// Define different UI for different senerio
    /// </summary>
    #region UI Scene Controllers

    /// <summary>
    /// Function that bound to Event OnDeviceDiscovered.
    /// </summary>
    private void OnDeviceFound(string name, string device)
    {
        DeviceButton button = Instantiate(_deviceButton, _deviceList_item).GetComponent<DeviceButton>();
        button.Show(name, device);
    }

    public void OnDisconnectedUIScene()
    {
        _buttonText.text = "Scan";
        _deviceList.gameObject.SetActive(true);
        _scanbutton.gameObject.SetActive(true);
        game_Cam.enabled = false;
        menu_Cam.enabled = true;
        dataDisplayMode = false;
        EMGGameMode = false;
        ROMGameMode = false;
        gameCanvas.sortingOrder = 0;
        menuCanvas.sortingOrder = 1;       
    }


    public void OnUnsubscribedUIScene()
    {
        _buttonText.text = "Subscribe";
        DataDisplay_Button.gameObject.SetActive(false);
        ROM_Button.gameObject.SetActive(false);
        EMG_Button.gameObject.SetActive(false);
    }

    public void ModeChooseUIScene()
    {
        _deviceList.gameObject.SetActive(false);
        _scanbutton.gameObject.SetActive(false);
        DataDisplay_Button.gameObject.SetActive(true);
        ROM_Button.gameObject.SetActive(true);
        EMG_Button.gameObject.SetActive(true);
        Back_Button.gameObject.SetActive(false);
        game_Cam.enabled = false;
        menu_Cam.enabled = true;
        gameCanvas.sortingOrder = 0;
        menuCanvas.sortingOrder = 1;
    }

    public void DataDisplayUIScene()
    {
        menu_Cam.enabled = false;
        game_Cam.enabled = true;
        dataDisplayBox.gameObject.SetActive(true);
        dataDisplayBox.text = BleAdapter.decodedMessage; // Display the received message from BleAdapter
        DataDisplay_Button.gameObject.SetActive(false);
        ROM_Button.gameObject.SetActive(false);
        EMG_Button.gameObject.SetActive(false);
        Back_Button.gameObject.SetActive(true);
        gameCanvas.sortingOrder = 1;
        menuCanvas.sortingOrder = 0;
    }

    public void EMGGameUIScene()
    {
        menu_Cam.enabled = false;
        game_Cam.enabled = true;
        dataDisplayBox.gameObject.SetActive(false);
        DataDisplay_Button.gameObject.SetActive(false);
        ROM_Button.gameObject.SetActive(false);
        EMG_Button.gameObject.SetActive(false);
        Back_Button.gameObject.SetActive(true);
        gameCanvas.sortingOrder = 1;
        menuCanvas.sortingOrder = 0;
    }

    public void ROMGameUIScene()
    {
        menu_Cam.enabled = false;
        game_Cam.enabled = true;
        dataDisplayBox.gameObject.SetActive(false);
        DataDisplay_Button.gameObject.SetActive(false);
        ROM_Button.gameObject.SetActive(false);
        EMG_Button.gameObject.SetActive(false);
        Back_Button.gameObject.SetActive(true);
        gameCanvas.sortingOrder = 1;
        menuCanvas.sortingOrder = 0;
    }

    #endregion
}
