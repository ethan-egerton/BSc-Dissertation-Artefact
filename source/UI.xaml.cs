using midism.classes;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace midism
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class UI : Page
    {
        public UI()
        {
            InitializeComponent();
        }

        private bool loaded = false;

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await MIDISignalTextBoxListener();
        }

        private async Task MIDISignalTextBoxListener()
        {
            int signalCache = GlobalVariables.MIDIsignal;
            int velocityCache = GlobalVariables.MIDIactivation;

            while (true)
            {
                if (signalCache != GlobalVariables.MIDIsignal || velocityCache != GlobalVariables.MIDIactivation)
                {
                    signalCache = GlobalVariables.MIDIsignal;
                    MIDISignalBox.Text = signalCache.ToString();

                    velocityCache = GlobalVariables.MIDIactivation;
                    VelocityBox.Text = velocityCache.ToString();
                }

                if (GlobalVariables.MIDIrecordings.Count != 0)
                {
                    for (int i = 0; i < GlobalVariables.MIDIrecordings.Count; i++)
                    {
                        int rowIndex = GlobalVariables.MIDIrecordings[i];
                        UIElement element = GetChildElementAtPosition(TranslationGrid, rowIndex, 3);
                        if (element is TextBox textBox)
                        {
                            textBox.Text = signalCache.ToString();
                        }
                    }
                }

                await Task.Delay(50);
            }
        }


        // Combobox1
        private void Combobox1_DropDownOpened(object sender, EventArgs e)
        {
            var DeviceHandlerClass = new DeviceHandler();
            var deviceNames = DeviceHandlerClass.GetAllDeviceNames();
            var comboBoxIndex = Combobox1.SelectedIndex;

            Combobox1.Items.Clear();
            Combobox1.Items.Add("None");

            foreach (var name in deviceNames)
            {
                Combobox1.Items.Add(name);
            }

            if (comboBoxIndex > Combobox1.Items.Count)
            {
                Combobox1.SelectedIndex = 0;
            }
            else
            {
                Combobox1.SelectedIndex = comboBoxIndex;
            }
        }

        private void Combobox1_Loaded(object sender, RoutedEventArgs e)
        {
            Combobox1.Items.Add("None");
        }

        private void Combobox1_SelectionChanged(object sender, EventArgs e)
        {
            if (Combobox1.SelectedValue != null)
            {
                GlobalVariables.selectedDevice = Combobox1.SelectedValue.ToString();
            }

        }

        // Add Translation Button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            loaded = false;
            var DeviceHandlerClass = new DeviceHandler();
            var deviceName = Combobox1.SelectedValue as string;

            if (deviceName == "None" || deviceName == null)
            {
                return;
            }

            DeviceHandlerClass.AddListenerAndEvent(deviceName);

            var rowIndex = GlobalVariables.translationCount + 1;
            GlobalVariables.translationCount += 1;

            // Row Border
            Border rowBorder = new Border
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(176, 127, 109)),
                BorderThickness = new Thickness(0, 0, 0, 1),
                Background = Brushes.Transparent
            };

            // Add new row
            RowDefinition newRow = new RowDefinition();
            newRow.Height = new GridLength(50);

            TranslationGrid.RowDefinitions.Add(newRow);
            Grid.SetRow(rowBorder, rowIndex);
            Grid.SetColumnSpan(rowBorder, 6);
            TranslationGrid.Children.Add(rowBorder);

            // Numarate Row
            TextBlock rowNumber = new TextBlock();
            rowNumber.Name = "index" + rowIndex;
            rowNumber.Text = GlobalVariables.translationCount.ToString();
            rowNumber.FontSize = 12;
            rowNumber.VerticalAlignment = VerticalAlignment.Center;
            rowNumber.HorizontalAlignment = HorizontalAlignment.Center;

            Grid.SetColumn(rowNumber, 0);
            Grid.SetRow(rowNumber, rowIndex);
            TranslationGrid.Children.Add(rowNumber);


            // Add listening device combo
            ComboBox deviceSelector = new ComboBox();
            deviceSelector.Name = "deviceSelector" + rowIndex;
            deviceSelector.Loaded += DeviceSelector_Loaded;
            deviceSelector.DropDownOpened += DeviceSelector_DropDownOpened;
            deviceSelector.DropDownClosed += setTranslationsCombo;
            deviceSelector.SelectedIndex = deviceSelector.Items.IndexOf(deviceName);
            deviceSelector.VerticalAlignment = VerticalAlignment.Center;
            deviceSelector.HorizontalAlignment = HorizontalAlignment.Center;
            deviceSelector.VerticalContentAlignment = VerticalAlignment.Center;
            deviceSelector.HorizontalContentAlignment = HorizontalAlignment.Center;
            deviceSelector.Width = 140;
            deviceSelector.Height = 25;

            Grid.SetColumn(deviceSelector, 1);
            Grid.SetRow(deviceSelector, rowIndex);
            TranslationGrid.Children.Add(deviceSelector);

            // Add MIDI signal box

            CheckBox recordButton = new CheckBox();
            recordButton.Name = "recordButton" + rowIndex;
            recordButton.VerticalAlignment = VerticalAlignment.Center;
            recordButton.HorizontalAlignment = HorizontalAlignment.Center;
            recordButton.VerticalContentAlignment = VerticalAlignment.Center;
            recordButton.HorizontalContentAlignment = HorizontalAlignment.Center;
            recordButton.Checked += Checkbox_Changed;
            recordButton.Unchecked += Checkbox_Changed;

            TextBox MIDIsignalText = new TextBox();
            MIDIsignalText.Name = "MIDISignal" + rowIndex;
            MIDIsignalText.PreviewTextInput += TextBox_PreviewTextInput;
            MIDIsignalText.TextChanged += setTranslationsTextbox;
            MIDIsignalText.Width = 120;
            MIDIsignalText.Height = 25;
            MIDIsignalText.VerticalAlignment = VerticalAlignment.Center;
            MIDIsignalText.HorizontalAlignment = HorizontalAlignment.Left;
            MIDIsignalText.VerticalContentAlignment = VerticalAlignment.Center;
            MIDIsignalText.HorizontalContentAlignment = HorizontalAlignment.Left;

            Grid.SetColumn(recordButton, 2);
            Grid.SetColumn(MIDIsignalText, 3);
            Grid.SetRow(recordButton, rowIndex);
            Grid.SetRow(MIDIsignalText, rowIndex);
            TranslationGrid.Children.Add(recordButton);
            TranslationGrid.Children.Add(MIDIsignalText);

            // Add reaction box
            ComboBox typeSelector = new ComboBox();
            typeSelector.Name = "typeSelector" + rowIndex;
            typeSelector.SelectionChanged += setTranslationsCombo;
            typeSelector.VerticalAlignment = VerticalAlignment.Center;
            typeSelector.HorizontalAlignment = HorizontalAlignment.Left;
            typeSelector.VerticalContentAlignment = VerticalAlignment.Center;
            typeSelector.HorizontalContentAlignment = HorizontalAlignment.Center;
            typeSelector.Width = 160;
            typeSelector.Height = 25;
            typeSelector.Items.Add("None");
            typeSelector.Items.Add("Keyboard Stroke");
            typeSelector.Items.Add("Windows Parameter");
            typeSelector.SelectedIndex = 0;
            typeSelector.SelectionChanged += typeSelector_SelectionChanged;

            Grid.SetColumn(typeSelector, 4);
            Grid.SetRow(typeSelector, rowIndex);
            TranslationGrid.Children.Add(typeSelector);

            loaded = true;
        }

        // Device Selector Combobox
        private void DeviceSelector_Loaded(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                DeviceSelector_DropDownOpened(sender, e);
                comboBox.SelectedIndex = Combobox1.SelectedIndex;
            }
        }

        private void DeviceSelector_DropDownOpened(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                var DeviceHandlerClass = new DeviceHandler();
                var deviceNames = DeviceHandlerClass.GetAllDeviceNames();
                var comboBoxIndex = comboBox.SelectedIndex;

                comboBox.Items.Clear();
                comboBox.Items.Add("None");

                foreach (var name in deviceNames)
                {
                    comboBox.Items.Add(name);
                }

                if (comboBoxIndex > comboBox.Items.Count)
                {
                    comboBox.SelectedIndex = 0;
                }
                else
                {
                    comboBox.SelectedIndex = comboBoxIndex;
                }
            }
        }

        private void Checkbox_Changed(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                int rowIndex = FindRowIndexByContent(TranslationGrid, checkBox.Name[checkBox.Name.Length - 1].ToString());

                if (checkBox.IsChecked == true)
                {
                    GlobalVariables.MIDIrecordings.Add(rowIndex);
                }
                else
                {
                    GlobalVariables.MIDIrecordings.Remove(rowIndex);
                }
            }
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        // Type Selector Combobox
        private void typeSelector_SelectionChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                int rowIndex = FindRowIndexByContent(TranslationGrid, comboBox.Name[comboBox.Name.Length - 1].ToString());

                UIElement element = GetChildElementAtPosition(TranslationGrid, rowIndex, 6);
                if (element != null)
                {
                    TranslationGrid.Children.Remove(element);
                }

                if (comboBox.SelectedValue.ToString() == "None" || comboBox.SelectedValue == null)
                {
                    return;
                }

                if (comboBox.SelectedValue.ToString() == "Keyboard Stroke")
                {
                    TextBox keystrokeTextbox = new TextBox();
                    keystrokeTextbox.Name = "KeyboardText" + rowIndex;
                    keystrokeTextbox.TextChanged += setTranslationsTextbox;
                    keystrokeTextbox.Width = 120;
                    keystrokeTextbox.Height = 25;
                    keystrokeTextbox.VerticalAlignment = VerticalAlignment.Center;
                    keystrokeTextbox.HorizontalAlignment = HorizontalAlignment.Left;
                    keystrokeTextbox.VerticalContentAlignment = VerticalAlignment.Center;
                    keystrokeTextbox.HorizontalContentAlignment = HorizontalAlignment.Left;

                    Grid.SetColumn(keystrokeTextbox, 6);
                    Grid.SetRow(keystrokeTextbox, rowIndex);
                    TranslationGrid.Children.Add(keystrokeTextbox);
                }

                if (comboBox.SelectedValue.ToString() == "Windows Parameter")
                {
                    ComboBox windowsCombo = new ComboBox();
                    windowsCombo.Name = "WindowsCombo" + rowIndex;
                    windowsCombo.Items.Add("None");
                    windowsCombo.Items.Add("Volume");
                    windowsCombo.SelectedIndex = 0;
                    windowsCombo.Width = 120;
                    windowsCombo.Height = 25;
                    windowsCombo.SelectionChanged += setTranslationsCombo;
                    windowsCombo.VerticalAlignment = VerticalAlignment.Center;
                    windowsCombo.HorizontalAlignment = HorizontalAlignment.Left;
                    windowsCombo.VerticalContentAlignment = VerticalAlignment.Center;
                    windowsCombo.HorizontalContentAlignment = HorizontalAlignment.Left;

                    Grid.SetColumn(windowsCombo, 6);
                    Grid.SetRow(windowsCombo, rowIndex);
                    TranslationGrid.Children.Add(windowsCombo);
                }

            }
        }

        // Utils
        private static int FindRowIndexByContent(Grid grid, string content)
        {
            // Iterate through the children of the grid
            foreach (UIElement child in grid.Children)
            {
                // Check if the child is a TextBlock and its content matches
                if (child is TextBlock textBlock && textBlock.Text == content)
                {
                    // Get the row index of the child
                    int rowIndex = Grid.GetRow(child);
                    return rowIndex;
                }
            }
            return -1; // Content not found
        }

        private static UIElement GetChildElementAtPosition(Grid grid, int row, int column)
        {
            foreach (UIElement child in grid.Children)
            {
                int childRow = Grid.GetRow(child);
                int childColumn = Grid.GetColumn(child);
                if (childRow == row && childColumn == column)
                {
                    return child;
                }
            }
            return null; // Element not found at the specified position
        }

        private void setTranslationsTextbox(object sender, TextChangedEventArgs e)
        {
            setTranslations();
        }

        private void setTranslationsCombo(object sender, SelectionChangedEventArgs e)
        {
            setTranslations();
        }

        private void setTranslationsCombo(object sender, EventArgs e)
        {
            setTranslations();
        }

        private void setTranslations()
        {
            if (loaded == false)
            {
                return;
            }

            Dictionary<int, List<string>> translations = [];

            for (int i = 1; i != GlobalVariables.translationCount + 1; i++)
            {
                List<string> translationData = [];

                UIElement element = GetChildElementAtPosition(TranslationGrid, i, 1);
                if (element is ComboBox comboBox)
                {
                    translationData.Add(comboBox.SelectedValue.ToString());
                }

                element = GetChildElementAtPosition(TranslationGrid, i, 3);
                if (element is TextBox textBox)
                {
                    translationData.Add(textBox.Text);
                }

                element = GetChildElementAtPosition(TranslationGrid, i, 4);
                if (element is ComboBox comboBox1)
                {
                    translationData.Add(comboBox1.SelectedValue.ToString());
                }

                element = GetChildElementAtPosition(TranslationGrid, i, 6);
                if (element is TextBox textBox1)
                {
                    try
                    {
                        translationData.Add(textBox1.Text);
                    }
                    catch (Exception) { throw; }
                }
                if (element is ComboBox comboBox2)
                {
                    if (comboBox2.SelectedValue != null)
                    {
                        translationData.Add(comboBox2.SelectedValue.ToString());
                    }
                }

                translations.Add(i,translationData);
                System.Diagnostics.Debug.WriteLine(string.Join(", ", translationData));
            }


            GlobalVariables.translations = translations;
        }
    }
}
