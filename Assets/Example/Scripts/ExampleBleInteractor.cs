using UnityEngine;
using Android.BLE;
using Android.BLE.Commands;
using UnityEngine.Android;

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
    private int _scanTime = 10;

    [SerializeField]
    public Camera cameraA;
    [SerializeField]
    public Camera cameraB;

    private float _scanTimer = 0f;

    private bool _isScanning = false;


    public void ScanForDevices()
    {
        if (!_isScanning)
        {
            _isScanning = true;
            BleManager.Instance.QueueCommand(new DiscoverDevices(OnDeviceFound, _scanTime * 1000));
        }
    }


    private void Start()
    {
        cameraA.enabled = false;
        cameraB.enabled = true;
    }


    private void Update()
    {
        if(_isScanning)
        {
            _scanTimer += Time.deltaTime;
            if(_scanTimer > _scanTime)
            {
                _scanTimer = 0f;
                _isScanning = false;
            }
        }

        if (DeviceButton._isConnected) {
            //_deviceList.gameObject.SetActive(false);
            //_scanbutton.gameObject.SetActive(false);
            //cameraA.enabled = true;
            //cameraB.enabled = false;
        }
        else
        {
            //_deviceList.gameObject.SetActive(true);
            //_scanbutton.gameObject.SetActive(true);
            //cameraA.enabled = false;
            //cameraB.enabled = true;
        }
    }

    private void OnDeviceFound(string name, string device)
    {
        DeviceButton button = Instantiate(_deviceButton, _deviceList_item).GetComponent<DeviceButton>();
        button.Show(name, device);
       
    }
}
