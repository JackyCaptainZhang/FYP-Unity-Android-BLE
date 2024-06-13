using UnityEngine;
using Android.BLE;
using Android.BLE.Commands;
using UnityEngine.Android;
using System.Text;
using UnityEngine.UI;
using System;


/// <summary>
/// This script mainly focus on the UI switch logic
/// </summary>

public class GameManager : MonoBehaviour
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




    // Replace these Characteristics with YOUR device's characteristics
    public static string service_UUID = CleanUUID("6E400001-B5A3-F393-­E0A9-­E50E24DCCA9E");
    public static string characteristic_Write_UUID = CleanUUID("6E400002-B5A3-F393-­E0A9-­E50E24DCCA9E");
    public static string characteristic_Notify_UUID = CleanUUID("6E400003-B5A3-F393-­E0A9-­E50E24DCCA9E");

    public static string CleanUUID(string uuid)
    {
        return uuid.Replace("\u00AD", "");
    }

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
    }

    public void OnROMButtonClicked()
    {
        BLEDeviceSTARTSendingData();
        modeSelectCanvas.gameObject.SetActive(false);
        BLEListCanvas.gameObject.SetActive(false);
        dataDisplayCanvas.gameObject.SetActive(false);
        ROMGameCanvas.gameObject.SetActive(true);
        EMGGameCanvas.gameObject.SetActive(false);
        DifficultyManager.GameMode = true;
    }

    public void OnEMGButtonClicked()
    {
        BLEDeviceSTARTSendingData();
        modeSelectCanvas.gameObject.SetActive(false);
        BLEListCanvas.gameObject.SetActive(false);
        dataDisplayCanvas.gameObject.SetActive(false);
        ROMGameCanvas.gameObject.SetActive(false);
        EMGGameCanvas.gameObject.SetActive(true);
        DifficultyManager.GameMode = true;
    }

    public void goBackToModeMenu()
    {
        BLEDeviceSTOPSendingData();
        modeSelectCanvas.gameObject.SetActive(true);
        BLEListCanvas.gameObject.SetActive(false);
        dataDisplayCanvas.gameObject.SetActive(false);
        ROMGameCanvas.gameObject.SetActive(false);
        EMGGameCanvas.gameObject.SetActive(false);
        DifficultyManager.GameMode = false;
    }

    public void goBackToBLEMenu()
    {
        modeSelectCanvas.gameObject.SetActive(false);
        BLEListCanvas.gameObject.SetActive(true);
        dataDisplayCanvas.gameObject.SetActive(false);
        ROMGameCanvas.gameObject.SetActive(false);
        EMGGameCanvas.gameObject.SetActive(false);
    }


    #endregion



    private void Start()
    {
        // Always start with the BLE list page
        modeSelectCanvas.gameObject.SetActive(false);
        BLEListCanvas.gameObject.SetActive(true);
        dataDisplayCanvas.gameObject.SetActive(false);
        ROMGameCanvas.gameObject.SetActive(false);
        EMGGameCanvas.gameObject.SetActive(false);
        DifficultyManager.GameMode = false;
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
            DifficultyManager.GameMode = false;
        }
    }

    #region BLE functions

    void BLEDeviceSTARTSendingData()
    {
        string base64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes("START"));
        _writeToCharacteristic = new WriteToCharacteristic(DeviceButton.connectted_deviceUuid, service_UUID, characteristic_Write_UUID, base64Data, true);
        _writeToCharacteristic.Start();
    }

    void BLEDeviceSTOPSendingData()
    {
        string base64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes("END"));
        _writeToCharacteristic = new WriteToCharacteristic(DeviceButton.connectted_deviceUuid, service_UUID, characteristic_Write_UUID, base64Data, true);
        _writeToCharacteristic.Start();
    }

    #endregion
}