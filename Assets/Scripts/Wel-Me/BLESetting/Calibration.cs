using UnityEngine;
using UnityEngine.UI;

public class Calibration : MonoBehaviour
{
    private bool calibrated;
    private bool MinROMcalibrated;
    private bool MaxROMcalibrated;
    private bool singleClickPracticed;
    private bool doubleClickPracticed;

    private int singleClickCount = 0;
    private int doubleClickCount = 0;

    private bool practicingSingleClick = false; // New flag to indicate single click practice
    private bool practicingDoubleClick = false; // New flag to indicate double click practice

    private GameManager gameManager;

    public Text instructionText;
    public Text ROMDataText;
    public Text EMGDataText;
    public Text calibrationResultText;
    public GameObject confirmButton;
    public Text confirmButtonText;

    private void OnEnable()
    {
        calibrated = false;
        MinROMcalibrated = false;
        MaxROMcalibrated = false;
        singleClickPracticed = false;
        doubleClickPracticed = false;
        singleClickCount = 0;
        doubleClickCount = 0;
        practicingSingleClick = false; // Reset practice flags
        practicingDoubleClick = false; // Reset practice flags
        gameManager = FindObjectOfType<GameManager>();
        gameManager.BLEDeviceSTOPSendingData();
        ROMDataText.text = "ROM Data: ";
        EMGDataText.text = "EMG Control: ";
        calibrationResultText.text = $"Calibration Results:\nMin ROM: {BleDataStorage.MinROM}\nMax ROM: {BleDataStorage.MaxROM}\nSingle Clicks Practiced: {singleClickCount}\nDouble Clicks Practiced: {doubleClickCount}";
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
            calibrationResultText.text = $"Calibration Results:\nMin ROM: {BleDataStorage.MinROM}\nMax ROM: {BleDataStorage.MaxROM}\nSingle Clicks Practiced: {singleClickCount}\nDouble Clicks Practiced: {doubleClickCount}";
        }
        else if (!MinROMcalibrated)
        {
            BleDataStorage.MinROM = BleDataStorage.Float1;
            MinROMcalibrated = true;
            instructionText.text = "Calibrate Max ROM. Press Confirm when done.";
            confirmButtonText.text = "Confirm Max ROM";
            calibrationResultText.text = $"Calibration Results:\nMin ROM: {BleDataStorage.MinROM}\nMax ROM: {BleDataStorage.MaxROM}\nSingle Clicks Practiced: {singleClickCount}\nDouble Clicks Practiced: {doubleClickCount}";
        }
        else if (!MaxROMcalibrated)
        {
            BleDataStorage.MaxROM = BleDataStorage.Float1;
            MaxROMcalibrated = true;
            instructionText.text = "Practice single click twice. Perform a single click and press Confirm.";
            confirmButtonText.text = "Confirm Single Click";
            practicingSingleClick = true; // Start single click practice
            calibrationResultText.text = $"Calibration Results:\nMin ROM: {BleDataStorage.MinROM}\nMax ROM: {BleDataStorage.MaxROM}\nSingle Clicks Practiced: {singleClickCount}\nDouble Clicks Practiced: {doubleClickCount}";
        }
        else if (!singleClickPracticed)
        {
            if (singleClickCount >= 2)
            {
                singleClickPracticed = true;
                instructionText.text = "Practice double click twice. Perform a double click and press Confirm.";
                confirmButtonText.text = "Confirm Double Click";
                practicingSingleClick = false; // End single click practice
                practicingDoubleClick = true; // Start double click practice
            }
            else
            {
                instructionText.text = "Practice single click twice. Perform a single click and press Confirm.";
            }
            calibrationResultText.text = $"Calibration Results:\nMin ROM: {BleDataStorage.MinROM}\nMax ROM: {BleDataStorage.MaxROM}\nSingle Clicks Practiced: {singleClickCount}\nDouble Clicks Practiced: {doubleClickCount}";
        }
        else if (!doubleClickPracticed)
        {
            if (doubleClickCount >= 2)
            {
                doubleClickPracticed = true;
                instructionText.text = "Calibration completed.";
                confirmButtonText.text = "Enter Game";
                practicingDoubleClick = false; // End double click practice
                gameManager.BLEDeviceSTOPSendingData();
            }
            else
            {
                instructionText.text = "Practice double click twice. Perform a double click and press Confirm.";
            }
            calibrationResultText.text = $"Calibration Results:\nMin ROM: {BleDataStorage.MinROM}\nMax ROM: {BleDataStorage.MaxROM}\nSingle Clicks Practiced: {singleClickCount}\nDouble Clicks Practiced: {doubleClickCount}";
        }
        else
        {
            gameManager.goBackToModeMenu();
        }
    }

    private void Update()
    {
        ROMDataText.text = "ROM Data: " + BleDataStorage.Float1.ToString();

        // Display EMG Control
        if (BleDataStorage.Float2 == 101)
        {
            EMGDataText.text = "EMG Control: Single click";
        }
        if (BleDataStorage.Float2 == 202)
        {
            EMGDataText.text = "EMG Control: Double click";
        }
        
        // Handle click practice
        if (practicingSingleClick && BleDataStorage.Float2 == 101)
        {
            singleClickCount++;
            BleDataStorage.Float2 = 0;  // Reset to avoid multiple counts in one click
        }

        if (practicingDoubleClick && BleDataStorage.Float2 == 202)
        {
            doubleClickCount++;
            BleDataStorage.Float2 = 0;  // Reset to avoid multiple counts in one click
        }
    }
}
