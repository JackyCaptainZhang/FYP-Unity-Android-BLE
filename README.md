## Code Notes

### Tested functions

* Connect to device

  ```c#
  private ConnectToDevice _connectCommand;
  
  public void Connect()
      {
          if (!_isConnected)
          {
              _connectCommand = new ConnectToDevice(_deviceUuid, OnConnected, OnDisconnected);
              BleManager.Instance.QueueCommand(_connectCommand);
          }
          else
          {
              _connectCommand.Disconnect();
          }
      }
  
  private void OnConnected(string deviceUuid)
      {
          _previousColor = _deviceButtonImage.color;
          _deviceButtonImage.color = _onConnectedColor;
  
          _isConnected = true;
          _deviceButtonText.text = "Disconnect";
      }
  
  private void OnDisconnected(string deviceUuid)
      {
          _deviceButtonImage.color = _previousColor;
  
          _isConnected = false;
          _deviceButtonText.text = "Connect";
      }
  ```

* Subscribe to characteristics

  ```c#
  private SubscribeToCharacteristic _subscribeToCharacteristic;
  
  public void SubscribeToExampleService()
      {
      // "0000" (Service UUID) and "0001"(Characteristics UUID) is a part of "0000" + service + "-0000-1000-8000-00805f9b34fb" by default.
          _subscribeToCharacteristic = new SubscribeToCharacteristic(_deviceUuid, "0000", "0001");
          BleManager.Instance.QueueCommand(_subscribeToCharacteristic);
      }
  ```

