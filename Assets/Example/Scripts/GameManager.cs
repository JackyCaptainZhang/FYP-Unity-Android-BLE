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
    private Canvas BLEListCanvas;
    [SerializeField]
    private Canvas modeSelectCanvas;
    [SerializeField]
    private Canvas dataDisplayCanvas;
    [SerializeField]
    private Canvas ROMGameCanvas;
    [SerializeField]
    private Canvas EMGGameCanvas;

    private WriteToCharacteristic _writeToCharacteristic;

    /* Replace these Characteristics with YOUR device's characteristics
       "0000" (Service UUID) and "0001"(Characteristics UUID) is a part of "0000" + service + "-0000-1000-8000-00805f9b34fb" by default. */
    private string service_UUID = "0000";
    private string characteristic_UUID = "0001";

    /// <summary>
    /// Define different click actions for different senerio
    /// </summary>
    #region Button Controllers

    public void OnDataDisplayButtonClicked()
    {
        BLEDeviceSTARTSendingData();
        modeSelectCanvas.gameObject.gameObject.SetActive(false);
        BLEListCanvas.gameObject.SetActive(false);
        dataDisplayCanvas.gameObject.SetActive(true);
        ROMGameCanvas.gameObject.SetActive(false);
        EMGGameCanvas.gameObject.SetActive(false);
        Debug.Log("Data Display Button was clicked!");

    }

    public void OnROMButtonClicked()
    {
        BLEDeviceSTARTSendingData();
        modeSelectCanvas.gameObject.SetActive(false);
        BLEListCanvas.gameObject.SetActive(false);
        dataDisplayCanvas.gameObject.SetActive(false);
        ROMGameCanvas.gameObject.SetActive(true);
        EMGGameCanvas.gameObject.SetActive(false);
        Debug.Log("ROM Button was clicked!");

    }

    public void OnEMGButtonClicked()
    {
        BLEDeviceSTARTSendingData();
        modeSelectCanvas.gameObject.SetActive(false);
        BLEListCanvas.gameObject.SetActive(false);
        dataDisplayCanvas.gameObject.SetActive(false);
        ROMGameCanvas.gameObject.SetActive(false);
        EMGGameCanvas.gameObject.SetActive(true);
        Debug.Log("EMG Button was clicked!");

    }

    public void goBackToMenu()
    {
        BLEDeviceSTOPSendingData();
        modeSelectCanvas.gameObject.SetActive(true);
        BLEListCanvas.gameObject.SetActive(false);
        dataDisplayCanvas.gameObject.SetActive(false);
        ROMGameCanvas.gameObject.SetActive(false);
        EMGGameCanvas.gameObject.SetActive(false);
        Debug.Log("Menu Button was clicked!");
    }


    #endregion

    

    private void Start()
    {
        // Start with the BLE list page
        modeSelectCanvas.gameObject.SetActive(false);
        BLEListCanvas.gameObject.SetActive(true);
        dataDisplayCanvas.gameObject.SetActive(false);
        ROMGameCanvas.gameObject.SetActive(false);
        EMGGameCanvas.gameObject.SetActive(false);
    }


    private void Update()
    {
        // Go back to BLE list anytime when connection is lost
        if (!DeviceButton._isConnected)
        {
            modeSelectCanvas.gameObject.SetActive(false);
            BLEListCanvas.gameObject.SetActive(true);
            dataDisplayCanvas.gameObject.SetActive(false);
            ROMGameCanvas.gameObject.SetActive(false);
            EMGGameCanvas.gameObject.SetActive(false);
        }
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

}
