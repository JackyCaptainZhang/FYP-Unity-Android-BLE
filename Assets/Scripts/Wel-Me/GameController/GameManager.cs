using UnityEngine;
using Android.BLE.Commands;
using System.Text;
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
    [SerializeField]
    private Canvas CalibrationCanvas;

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
        ROMGameCanvas.gameObject.SetActive(false);
        EMGGameCanvas.gameObject.SetActive(false);
        CalibrationCanvas.gameObject.SetActive(false);
        dataDisplayCanvas.gameObject.SetActive(true);
    }

    public void OnROMButtonClicked()
    {
        BLEDeviceSTARTSendingData();
        modeSelectCanvas.gameObject.SetActive(false);
        BLEListCanvas.gameObject.SetActive(false);
        dataDisplayCanvas.gameObject.SetActive(false);
        CalibrationCanvas.gameObject.SetActive(false);
        EMGGameCanvas.gameObject.SetActive(false);
        ROMGameCanvas.gameObject.SetActive(true);
        DifficultyManager.inROMGame = true;
        DifficultyManager.inEMGGame = false;
    }

    public void OnEMGButtonClicked()
    {
        BLEDeviceSTARTSendingData();
        modeSelectCanvas.gameObject.SetActive(false);
        BLEListCanvas.gameObject.SetActive(false);
        dataDisplayCanvas.gameObject.SetActive(false);
        CalibrationCanvas.gameObject.SetActive(false);
        ROMGameCanvas.gameObject.SetActive(false);
        EMGGameCanvas.gameObject.SetActive(true);
        DifficultyManager.inROMGame = false;
        DifficultyManager.inEMGGame = true;
    }

    public void goBackToModeMenu()
    {
        BLEDeviceSTOPSendingData();
        BLEListCanvas.gameObject.SetActive(false);
        dataDisplayCanvas.gameObject.SetActive(false);
        ROMGameCanvas.gameObject.SetActive(false);
        CalibrationCanvas.gameObject.SetActive(false);
        EMGGameCanvas.gameObject.SetActive(false);
        modeSelectCanvas.gameObject.SetActive(true);
        DifficultyManager.inROMGame = false;
        DifficultyManager.inEMGGame = false;
    }

    public void goBackToBLEMenu()
    {
        modeSelectCanvas.gameObject.SetActive(false);
        dataDisplayCanvas.gameObject.SetActive(false);
        ROMGameCanvas.gameObject.SetActive(false);
        EMGGameCanvas.gameObject.SetActive(false);
        CalibrationCanvas.gameObject.SetActive(false);
        BLEListCanvas.gameObject.SetActive(true);
        DifficultyManager.inROMGame = false;
        DifficultyManager.inEMGGame = false;
    }

    public void calibrateBTN()
    {
        modeSelectCanvas.gameObject.SetActive(false);
        BLEListCanvas.gameObject.SetActive(false);
        dataDisplayCanvas.gameObject.SetActive(false);
        ROMGameCanvas.gameObject.SetActive(false);
        EMGGameCanvas.gameObject.SetActive(false);
        CalibrationCanvas.gameObject.SetActive(true);
        DifficultyManager.inROMGame = false;
        DifficultyManager.inEMGGame = false;
    }


    #endregion



    private void Start()
    {
        // Always start with the BLE list page
        modeSelectCanvas.gameObject.SetActive(false);
        dataDisplayCanvas.gameObject.SetActive(false);
        ROMGameCanvas.gameObject.SetActive(false);
        EMGGameCanvas.gameObject.SetActive(false);
        CalibrationCanvas.gameObject.SetActive(false);
        BLEListCanvas.gameObject.SetActive(true);
        DifficultyManager.inROMGame = false;
        DifficultyManager.inEMGGame = false;
    }

    /// <summary>
    /// Always check the connection status.
    /// Always go back to BLEListCanvas when connection is lost.
    /// </summary>
    private void Update()
    {
        if (!DeviceButton._isConnected)
        {
            modeSelectCanvas.gameObject.SetActive(false);
            dataDisplayCanvas.gameObject.SetActive(false);
            ROMGameCanvas.gameObject.SetActive(false);
            EMGGameCanvas.gameObject.SetActive(false);
            CalibrationCanvas.gameObject.SetActive(false);
            BLEListCanvas.gameObject.SetActive(true);
            DifficultyManager.inROMGame = false;
            DifficultyManager.inEMGGame = false;
        }
    }

    #region BLE functions

    // This two functions send START or END command to BLE device to control the data transmission.
    public void BLEDeviceSTARTSendingData()
    {
        string base64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes("START"));
        _writeToCharacteristic = new WriteToCharacteristic(DeviceButton.connectted_deviceUuid, service_UUID, characteristic_Write_UUID, base64Data, true);
        _writeToCharacteristic.Start();
    }

    public void BLEDeviceSTOPSendingData()
    {
        string base64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes("END"));
        _writeToCharacteristic = new WriteToCharacteristic(DeviceButton.connectted_deviceUuid, service_UUID, characteristic_Write_UUID, base64Data, true);
        _writeToCharacteristic.Start();
    }

    #endregion
}