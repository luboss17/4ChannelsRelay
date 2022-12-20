using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RelayModule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string comAddr="Channel10";//Assume Com Address is Channel 10
        Relay relays;
        private string On = "On";
        private string Off = "Off";
        public MainWindow()
        {
            InitializeComponent();

            //Initialize 3 relays for RelaysComboBox
            for (int i = 0; i < 4; i++)
                RelaysComboBox.Items.Add(i);
            
            //Init OnOff ComboBox
            OnOff_ComboBox.Items.Add(On);
            OnOff_ComboBox.Items.Add(Off);

            //Init relay Constructor
            relays = new Relay(comAddr);
        }

        private void RelaysComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int relayIndex=RelaysComboBox.SelectedIndex;
            if (relayIndex>=0)
            {
                //Clear relay status
                Status_TxtBox.Text="";

                //When Select new Relays, update firmware and ADC value automatically
                Firmware_TxtBox.Text = relays.ReadFirmwareVersion();
                ADC_TxtBox.Text=relays.ReadAdcValue(relayIndex).ToString();
            }
        }

        private void Firmware_Btn_Click(object sender, RoutedEventArgs e)
        {
            Firmware_TxtBox.Text = relays.ReadFirmwareVersion();
        }

        private void ADC_btn_Click(object sender, RoutedEventArgs e)
        {
            int relayIndex = RelaysComboBox.SelectedIndex;
            ADC_TxtBox.Text = relays.ReadAdcValue(relayIndex).ToString();
        }

        private async void Write_btn_Click(object sender, RoutedEventArgs e)
        {
                Task task = WriteAsync(relays, OnOff_ComboBox.Text, RelaysComboBox.SelectedIndex);
                await task;
        }

        private async void Read_btn_Click(object sender, RoutedEventArgs e)
        {
            Task<byte> task = ReadAsync(relays, RelaysComboBox.SelectedIndex);
            byte relayStatus = await task;
            Status_TxtBox.Text =relayStatus.ToString();
        }
        static async Task<byte> ReadAsync(Relay relays, int relayInt)
        {
            byte state=0;
            await Task.Run(() =>
            {
                state= relays.Read(relayInt);
            });
            return state;
        }
        static async Task WriteAsync(Relay relays,string OnOff, int relayInt)
        {
            await Task.Run(() =>
            {
                relays.Write(OnOff, relayInt);
            });
        }
    }
}
