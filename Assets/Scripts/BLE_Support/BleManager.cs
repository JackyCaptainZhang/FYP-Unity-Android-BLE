using Android.BLE.Commands;
using System.Collections.Generic;
using UnityEngine;

namespace Android.BLE
{
    /// <summary>
    /// This manager that handles all BLE interactions for the plugin
    /// </summary>
    public class BleManager : MonoBehaviour
    {

        /// Gets a Singleton instance of the BleManager
        /// or creates one if it doesn't exist.
        public static BleManager Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                else
                {
                    CreateBleManagerObject();
                    return _instance;
                }
            }
        }
        private static BleManager _instance;

        // True if theBleManageris initialized.
        public static bool IsInitialized { get => _initialized; }
        private static bool _initialized = false;

        [SerializeField]
        private BleAdapter _adapter;

        // True if Initialize() method is called on Unity Awake
        [Tooltip("Use Initialize() if you want to Initialize manually")]
        public bool InitializeOnAwake = true;

        // True if all interactions with the BleManager should be logged.
        [Header("Logging")]
        [Tooltip("Logs all messages coming through the BleManager")]
        public bool LogAllMessages = false;

        // Determain if Android log messages should be passed through Unity Debug.Log(object)
        [Tooltip("Passes messages through to the Unity Debug.Log system")]
        public bool UseUnityLog = true;

        // Determain if Unity log messages should be passed through Android LogCat
        [Tooltip("Passes messages through to Android's Logcat")]
        public bool UseAndroidLog = false;

        // The Java library's BleManager hook.
        internal static AndroidJavaObject _bleLibrary = null;

        // The queue stack for command that is executed on after one
        private readonly Queue<BleCommand> _commandQueue = new Queue<BleCommand>();

        // The stack for command that is running in parallel
        private readonly List<BleCommand> _parrallelStack = new List<BleCommand>();

        // The command that is active now
        private static BleCommand _activeCommand = null;

 
        // Timer to track the _activeCommand runtime
        private static float _activeTimer = 0f;

        /// <summary>
        /// Will get called when the game start.
        /// </summary>
        private void Awake()
        {
            _instance = this;
            // Initialize the BleManager and BleAdapter
            if (InitializeOnAwake)
                Initialize();
            // Bind the function to Unity Event system
            _adapter.OnMessageReceived += OnBleMessageReceived;
            _adapter.OnErrorReceived += OnErrorReceived;
        }

        /// <summary>
        /// Destoy function in Unity
        /// Will get called when the game quit
        /// </summary>
        private void OnDestroy() => DeInitialize();


        /// <summary>
        /// Put commands in queue for execution.
        /// </summary>
        public void QueueCommand(BleCommand command)
        {

            CheckForLog("Queueing Command: " + command.GetType().Name);

            // If command is run in parallel
            if (command.RunParallel)
            {
                // Add command to _parrallelStack
                // And execute the command immediately
                _parrallelStack.Add(command);
                command.Start();
            }
            // If command is run in queue
            else
            {
                // If no command is active now
                if (_activeCommand == null)
                {
                    //Reset the timer and run this command immediately
                    _activeTimer = 0f;
                    _activeCommand = command;
                    _activeCommand.Start();
                }
                // If there is active command now
                // Add command to queue stack
                else
                    _commandQueue.Enqueue(command);
            }
        }



        private void Update()
        {
            _activeTimer += Time.deltaTime;

            // Checks if the _activeCommand has timed out
            if (_activeCommand != null && _activeTimer > _activeCommand.Timeout)
            {
                CheckForLog("Timed Out: " + _activeCommand + " - " + _activeCommand.Timeout);

                // Resets timers and ends the current _activeCommand
                _activeTimer = 0f;
                _activeCommand.EndOnTimeout();

                if (_commandQueue.Count > 0)
                {
                    // Sets a new _activeCommand
                    _activeCommand = _commandQueue.Dequeue();
                    _activeCommand?.Start();

                    if (_activeCommand != null)
                        CheckForLog("Executing new Command: " + _activeCommand.GetType().Name);
                }
                else
                    _activeCommand = null;
            }
        }

        /// <summary>
        /// Initialized the BleManager.
        /// Sets up the Java Library hooks and prepares BleAdapter to receive messages.
        /// </summary>
        public void Initialize()
        {
            if (!_initialized)
            {
                // Creates a new instance
                if (_instance == null)
                    CreateBleManagerObject();

                // Prepares a BleAdapter to receive messages
                #region BLE Adapter initialize

                if (_adapter == null)
                {
                    _adapter = FindObjectOfType<BleAdapter>();
                    if (_adapter == null)
                    {
                        GameObject bleAdapter = new GameObject(nameof(BleAdapter));
                        bleAdapter.transform.SetParent(Instance.transform);

                        _adapter = bleAdapter.AddComponent<BleAdapter>();
                    }
                }

                #endregion

                // Binds to the com.velorexe.unityandroidble.UnityAndroidBLE Singleton
                #region Android Library initialize

                if (_bleLibrary == null)
                {
                    AndroidJavaClass librarySingleton = new AndroidJavaClass("com.velorexe.unityandroidble.UnityAndroidBLE");
                    _bleLibrary = librarySingleton.CallStatic<AndroidJavaObject>("getInstance");
                }

                #endregion
            }
        }


        /// <summary>
        /// Creates a new GameObject instance for the BleManager to attach to.
        /// </summary>
        private static void CreateBleManagerObject()
        {
            GameObject managerObject = new GameObject();
            managerObject.name = "BleManager";

            managerObject.AddComponent<BleManager>();
        }

        /// <summary>
        /// Ends all currently running BleCommand and
        /// destroy the Java library hooks to save ram
        /// </summary>
        public void DeInitialize()
        {
            foreach (BleCommand command in _parrallelStack)
                command.End();

            _bleLibrary?.Dispose();

            if (_adapter != null)
                Destroy(_adapter.gameObject);
        }


        /// <summary>
        /// Calls a method from the Java plugin that matches the given command.
        /// This method allows for the interaction from unity to plugin
        /// </summary>
        internal static void SendCommand(string command, params object[] parameters)

        {
            if (Instance.LogAllMessages)
                CheckForLog("Calling Command: " + command);
            _bleLibrary?.Call(command, parameters);
        }


        /// <summary>
        /// Gets called when a new BleObject obj is received from plugin.
        /// This method allows for the interaction from plugin to unity
        /// </summary>
        private void OnBleMessageReceived(BleObject obj)
        {
            CheckForLog(JsonUtility.ToJson(obj, true));

            // Checks the message back from the plugin
            // It shows if the command sent from Unity is executed correctly or not by the plugin
            // You can control the response behaviour of each command by overwritting CommandReceived() method
            // CommandReceived() will return true if the command is successfully executed by plugin

            // For command that is in queue stack
            // If command is successfully executed by plugin, end the active command
            if (_activeCommand != null && _activeCommand.CommandReceived(obj))
            {
                _activeCommand.End();
                // Read and execute the next command in queue if there is any
                if (_commandQueue.Count > 0)
                {
                    _activeCommand = _commandQueue.Dequeue();
                    _activeCommand?.Start();

                    if (_activeCommand != null)
                        CheckForLog("Executing next Command: " + _activeCommand.GetType().Name);
                }
                else
                    // Set _activeCommand to Null if there is no next command in the queue
                    _activeCommand = null;
            }

            // For command that is in parallel stack
            // Just remove the commands that have executed successfully.
            for (int i = 0; i < _parrallelStack.Count; i++)
            {
                if (_parrallelStack[i].CommandReceived(obj))
                {
                    _parrallelStack[i].End();
                    _parrallelStack.RemoveAt(i);
                }
            }
        }


        #region Log system

        private void OnErrorReceived(string errorMessage)
        {
            CheckForLog(errorMessage);
        }

        private static void CheckForLog(string logMessage)
        {
            if (Instance.UseUnityLog)
                Debug.LogWarning(logMessage);
            if (Instance.UseAndroidLog)
                AndroidLog(logMessage);
        }

        public static void AndroidLog(string message)
        {
            if (_initialized)
                _bleLibrary?.CallStatic("androidLog", message);
        }

        #endregion

        
    }
}