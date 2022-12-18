
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace Deadlock
{
    public partial class ReproduceErrorForm : Form
    {
        #region Error Source
        WaveInEvent? micStream; WaveOutEvent? virtualCable;

        public bool isEnabled = false;
        public void Start()
        {
            micStream = new();
            WaveInProvider waveIn = new(micStream);

            virtualCable = new();
            virtualCable.Init(waveIn);

            micStream.StartRecording();
            virtualCable.Play();

        }

        private void Dispose(IDisposable? disposable)
        {
            disposable?.Dispose();
        }

        public void Stop()
        {
            Dispose(micStream);
            Dispose(virtualCable);

        }
        public void Restart()
        {   
            
            Stop();
            if (isEnabled) Start();
        }
        #endregion

        MMDeviceEnumerator enumerator = new();
        public ReproduceErrorForm()
        {
            InitializeComponent();
            enumerator.RegisterEndpointNotificationCallback(new NotificationClientImplementation(this));
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        { 
            if(checkBox1.Checked)
            {
                isEnabled = true;
                Start();
            } else
            {
                isEnabled = false;
                Stop();
            }

        }
    }

    class NotificationClientImplementation : NAudio.CoreAudioApi.Interfaces.IMMNotificationClient
    {
        ReproduceErrorForm form;
        public void OnDefaultDeviceChanged(DataFlow dataFlow, Role deviceRole, string defaultDeviceId)
        { 
            form.Invoke(form.Restart);
        }

        public void OnDeviceAdded(string deviceId)
        {
        }

        public void OnDeviceRemoved(string deviceId)
        {

        }

        public void OnDeviceStateChanged(string deviceId, DeviceState newState)
        {

        }

        public NotificationClientImplementation(ReproduceErrorForm form)
        {
            this.form = form;
        }

        public void OnPropertyValueChanged(string deviceId, PropertyKey propertyKey)
        {

        }

    }
}