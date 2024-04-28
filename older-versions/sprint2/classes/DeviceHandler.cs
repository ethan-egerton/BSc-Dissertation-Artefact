using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Tools;
using System;
using System.Reflection;
using System.Windows.Forms;


namespace midism.classes
{

    public class DeviceHandler()
    {

        public List<string> GetAllDeviceNames()
        {
            List<string> deviceNames = [];

            foreach (var inputDevice in InputDevice.GetAll())
            {
                deviceNames.Add(inputDevice.Name);
            }

            return deviceNames;
        }


        // Check and add perm threaded listener if not listening
        // 
        public async void AddListenerAndEvent(string deviceName)
        {
            // Add perm listener
            if (!GlobalVariables.activeListeners.Contains(deviceName))
            {
                GlobalVariables.activeListeners.Add(deviceName);
                await AddPermListener(deviceName);
            }
        }

        async Task AddPermListener(string deviceName)
        {
            var inputDevice = InputDevice.GetByName(deviceName);
            while (true)
            {
                if (inputDevice.IsListeningForEvents != true)
                {
                    inputDevice.StartEventsListening();
                    inputDevice.EventReceived += OnEventReceived;
                }
                await Task.Delay(500);
            }
        }

        private void OnEventReceived(object sender, MidiEventReceivedEventArgs e)
        {
            var midiDevice = (MidiDevice)sender;
            List<int> noteData = NoteParser(e.Event.ToString());
            int noteNumber = noteData[0];
            int activation = 0;

            try
            {
                activation = noteData[1];
            } catch (Exception ex) { }


            if (GlobalVariables.selectedDevice == midiDevice.Name)
            {
                GlobalVariables.MIDIsignal = noteNumber;
            }

            System.Diagnostics.Debug.WriteLine(noteNumber);
            System.Diagnostics.Debug.WriteLine(activation);

            foreach (KeyValuePair<int, List<String>> pair in GlobalVariables.translations)
            {
                string name = pair.Value[0];
                int MIDIsignal = int.Parse(pair.Value[1]);
                string type = pair.Value[2];
                string value = pair.Value[3];

                if (name != midiDevice.Name ||
                    MIDIsignal != noteNumber ||
                    MIDIsignal.GetType() != typeof(int) ||
                    MIDIsignal == 0 ||
                    type == "None")
                {
                    continue;
                }

                if (type == "Keyboard Stroke")
                {
                    if (value.Contains("ALT+"))
                    {
                        value = value.Remove(value.IndexOf("ALT+"), "ALT+".Length);
                        value = "%" + value;
                    }
                    if (value.Contains("CONTROL+"))
                    {
                        value = value.Remove(value.IndexOf("CONTROL+"), "CONTROL+".Length);
                        value = "^" + value;
                    }
                    if (value.Contains("SHIFT+"))
                    {
                        value = value.Remove(value.IndexOf("SHIFT+"), "SHIFT+".Length);
                        value = "+" + value;
                    }

                    System.Diagnostics.Debug.WriteLine(value);
                    SendKeys.SendWait(value);
                }
            }
        }

        private List<int> NoteParser(string notes)
        {
            List<int> result = new List<int>();

            string searchString1 = "(";
            string searchString2 = ",";
            string searchString3 = ")";

            int index1 = notes.IndexOf(searchString1);
            int index2 = notes.IndexOf(searchString2);
            int index3 = notes.IndexOf(searchString3);
            if (index1 == -1 || index2 == -1 || index3 == -1 || notes == null) { return [0, 0]; };

            if (int.TryParse(notes[(index1 + 1)..index2], out int number))
            {
                result.Add(number);
            }

            if (int.TryParse(notes[(index2 + 2)..index3], out int number2))
            {
                result.Add(number2);
            }

            return result;
        }
    }
}
