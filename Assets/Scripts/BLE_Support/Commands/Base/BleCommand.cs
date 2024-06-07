namespace Android.BLE.Commands
{
    /// <summary>
    /// The base class for all BLE commands to inherit from
    /// </summary>
    public abstract class BleCommand
    {

        public float Timeout { get => _timeout; }
        protected float _timeout = 5f;

        // If true, command will run parallel
        public readonly bool RunParallel = false;

        public readonly bool RunContiniously = false;

        /// <summary>
        /// Base initialization of the BleCommand
        /// </summary>
        public BleCommand(bool runParallel = false, bool runContiniously = false)
        {
            RunParallel = runParallel;
            RunContiniously = runContiniously;
        }

        /// <summary>
        /// Starts the the execution of the BleCommand.
        /// Will be overwrittten in each command.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Ends the BleCommand.
        /// Will be overwrittten in each command.
        /// </summary>
        public virtual void End() { }

        /// <summary>
        /// Ends the BleCommand when time runs out, by default calls End().
        /// </summary>
        public virtual void EndOnTimeout() => End();

        /// <summary>
        /// Work with OnBleMessageReceived() in BleManager.
        /// Checks the message back from the plugin.
        /// It shows if the command sent from Unity is executed correctly or not by the plugin.
        /// You can control the response behaviour of each command by overwritting this method.
        /// Will return true if the command is successfully executed by plugin.
        /// </summary>
        public virtual bool CommandReceived(BleObject obj)
        {
            return false;
        }
    }
}