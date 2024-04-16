using System.Windows.Forms;
using Melanchall.DryWetMidi.Multimedia;

namespace InputDeviceExample
{
    class Program
    {
        private static InputDevice _inputDevice;

        static void Main()
        {
            foreach (var item in InputDevice.GetAll())
            {
                Console.WriteLine(item.Name);
            }
            _inputDevice = InputDevice.GetByName("Launch Control");
            _inputDevice.EventReceived += OnEventReceived;
            _inputDevice.StartEventsListening();

            Console.WriteLine("Input device is listening for events. Press any key to exit...");
            Console.ReadKey();

            (_inputDevice as IDisposable)?.Dispose();
        }

        private static void OnEventReceived(object sender, MidiEventReceivedEventArgs e)
        {
            if (e.Event.EventType.ToString() != "NoteOn") { return; }
            char splitChar1 = '(';
            char splitChar2 = ',';
            int charIndex1 = e.Event.ToString().IndexOf(splitChar1);
            int charIndex2 = e.Event.ToString().IndexOf(splitChar2) - charIndex1;
            if (charIndex1 != -1)
            {
                string numberString = e.Event.ToString()[(charIndex1 + 1)..];
                numberString = numberString.ToString()[..(charIndex2 - 1)];
                int midiKey = int.Parse(numberString);
                Console.WriteLine($"This is midi key: {numberString}");
                
                // I hate doing this but this is just the prototype to show that it works
                if (midiKey == 9)
                {
                    SendKeys.SendWait("E");
                }
                if (midiKey == 10)
                {
                    SendKeys.SendWait("T");
                }
                if (midiKey == 11)
                {
                    SendKeys.SendWait("H");
                }
                if (midiKey == 12)
                {
                    SendKeys.SendWait("A");
                }
                if (midiKey == 25)
                {
                    SendKeys.SendWait("N");
                }
                if (midiKey == 26)
                {
                    SendKeys.SendWait("A");
                }
                if (midiKey == 27)
                {
                    SendKeys.SendWait("B");
                }
                if (midiKey == 28)
                {
                    SendKeys.SendWait("C");
                }

            }
        }
    }
}
