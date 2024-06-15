using UnityEngine;
using UnityEngine.UI;

public class Calibration : MonoBehaviour
{
    private bool calibrated;
    private bool MinROMcalibrated;
    private bool MaxROMcalibrated;
    private bool MinEMGcalibrated;
    private bool MaxEMGcalibrated;

    private GameManager gameManager;


    public Text instructionText;
    public Text ROMDataText;
    public Text EMGDataText;
    public Text calibrationResultText;
    public GameObject confirmButton;
    public Text confirmButtonText;

    /// <summary>
    /// This method is called each time when this canvas is activated.
    /// </summary>
    private void OnEnable()
    {
    calibrated = false;
    MinROMcalibrated = false;
    MaxROMcalibrated = false;
    MinEMGcalibrated = false;
    MaxEMGcalibrated = false;
    gameManager = FindObjectOfType<GameManager>();
    gameManager.BLEDeviceSTOPSendingData();
    ROMDataText.text = "ROM Data: ";
    EMGDataText.text = "EMG Data: ";
    calibrationResultText.text = $"Calibration Results:\nMin ROM: {BleDataStorage.MinROM}\nMax ROM: {BleDataStorage.MaxROM}\nMin EMG: {BleDataStorage.MinEMG}\nMax EMG: {BleDataStorage.MaxEMG}";
    instructionText.text = "Let's Start calibration. Press Confirm button to start.";
    confirmButtonText.text = "Start";
    }


    public void ConfirmBTN()
    {
        if (!calibrated)
        {
            gameManager.BLEDeviceSTARTSendingData();
            calibrated = true;
            instructionText.text = "Calibrate Min ROM. Press Confirm when done.";
            confirmButtonText.text = "Confirm Min ROM";
            calibrationResultText.text = $"Calibration Results:\nMin ROM: {BleDataStorage.MinROM}\nMax ROM: {BleDataStorage.MaxROM}\nMin EMG: {BleDataStorage.MinEMG}\nMax EMG: {BleDataStorage.MaxEMG}";
        }
        else if (!MinROMcalibrated)
        {
            BleDataStorage.MinROM = BleDataStorage.Float1;
            MinROMcalibrated = true;
            instructionText.text = "Calibrate Max ROM. Press Confirm when done.";
            confirmButtonText.text = "Confirm Max ROM";
            calibrationResultText.text = $"Calibration Results:\nMin ROM: {BleDataStorage.MinROM}\nMax ROM: {BleDataStorage.MaxROM}\nMin EMG: {BleDataStorage.MinEMG}\nMax EMG: {BleDataStorage.MaxEMG}";
        }
        else if (!MaxROMcalibrated)
        {
            BleDataStorage.MaxROM = BleDataStorage.Float1;
            MaxROMcalibrated = true;
            instructionText.text = "Calibrate Min EMG. Press Confirm when done.";
            confirmButtonText.text = "Confirm Min EMG";
            calibrationResultText.text = $"Calibration Results:\nMin ROM: {BleDataStorage.MinROM}\nMax ROM: {BleDataStorage.MaxROM}\nMin EMG: {BleDataStorage.MinEMG}\nMax EMG: {BleDataStorage.MaxEMG}";
        }
        else if (!MinEMGcalibrated)
        {
            BleDataStorage.MinEMG = BleDataStorage.Float2;
            MinEMGcalibrated = true;
            instructionText.text = "Calibrate Max EMG. Press Confirm when done.";
            confirmButtonText.text = "Confirm Max EMG";
            calibrationResultText.text = $"Calibration Results:\nMin ROM: {BleDataStorage.MinROM}\nMax ROM: {BleDataStorage.MaxROM}\nMin EMG: {BleDataStorage.MinEMG}\nMax EMG: {BleDataStorage.MaxEMG}";
        }
        else if (!MaxEMGcalibrated)
        {
            BleDataStorage.MaxEMG = BleDataStorage.Float2;
            MaxEMGcalibrated = true;
            instructionText.text = "Calibration completed.";
            confirmButtonText.text = "Enter Game";
            gameManager.BLEDeviceSTOPSendingData();
            calibrationResultText.text = $"Calibration Results:\nMin ROM: {BleDataStorage.MinROM}\nMax ROM: {BleDataStorage.MaxROM}\nMin EMG: {BleDataStorage.MinEMG}\nMax EMG: {BleDataStorage.MaxEMG}";
        }
        else
        {
            gameManager.goBackToModeMenu();
        }
    }


    private void Update()
    {
        ROMDataText.text = "ROM Data: " + BleDataStorage.Float1.ToString();
        EMGDataText.text = "EMG Data: " + BleDataStorage.Float2.ToString();

    }

}
