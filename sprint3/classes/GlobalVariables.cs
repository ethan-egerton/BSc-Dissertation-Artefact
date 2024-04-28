namespace midism.classes
{
    public class GlobalVariables
    {
        public static List<string> activeListeners = [];
        public static List<int> MIDIrecordings = [];
        public static int MIDIsignal = 0;
        public static int MIDIactivation = 0;
        public static int translationCount = 0;
        public static string selectedDevice = "";
        public static Dictionary<int, List<string>> translations = [];
    }
}
