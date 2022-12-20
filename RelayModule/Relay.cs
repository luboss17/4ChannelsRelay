using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace RelayModule
{
    internal class Relay
    {
        /*
         * o	Write
o	Read
o	SetRelayState
o	ReadRelayState
o	ReadFirmwareVersion
o	ReadAdcValue

         */
        private string logPath= Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + "log.txt";//Assign log path to write to
        private string state0, state1, state2, state3;
        private int ADC0, ADC1, ADC2, ADC3;
        private string On="On", Off="Off";
        private string Firmware = "000021-07";//hardcode firmware

        //Constructor, take in ComAddr
        public Relay(string ComAddr)
        {
            state0 = Off;
            state1 = Off;
            state2 = Off;
            state3 = Off;
            //Assign ADC as rand for each relay
            Random rnd = new Random();
            ADC0 = rnd.Next(0, 1023);
            ADC1 = rnd.Next(0, 1023);
            ADC2 = rnd.Next(0, 1023);
            ADC3 = rnd.Next(0, 1023);
        }
        //Write to log file
        private void writeToLog(string log)
        {
            try
            {
                File.AppendAllText(logPath, log);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        //set state for each relay
        //this is hardcode when not connect to real hardware
        private void SetRelayState(string OnOff, int relayInt)
        {
            switch (relayInt)
            {
                case 0:
                    state0 = OnOff;
                    break;
                case 1:
                    state1 = OnOff;
                    break;
                case 2:
                    state2 = OnOff;
                    break;
                case 3:
                    state3 = OnOff;
                    break;
            }
                
        }
        //get the state of relay 
        private string ReadRelayState(int relayInt)
        {
            string returnState = "";
            switch(relayInt)
            {
                case 0:
                    returnState = state0;
                    break;
                case 1:
                    returnState = state1;
                    break;
                case 2:
                    returnState = state2;
                    break;
                case 3:
                    returnState = state3;
                    break;
                default:
                    returnState = Off;
                    break;
            }
            return returnState;
        }
        //Write to Log file the command+ state of relay
        //Return status of the relay: 0=disable,1=enable
        public byte Read(int relayInt)
        {
            string log;
            string state="";
            state= ReadRelayState(relayInt);
            log = $"Relay Read {relayInt}, state";
            writeToLog(log);//write to log file
            if (state == On)
                return 1;
            else//if state is Off, or if relayInt is invalid, return 0=disable
                return 0;
        }
        //pass in On or Off as string, and the relay number
        //Write to Log file the command
        public void Write(string OnOFF, int relayInt)
        {
            string command;
            string state;
            //check whether passed in para is on or off
            if (OnOFF.ToLower() == "on")
                state = On;
            else if (OnOFF.ToLower() == "off")
                state = Off;
            else
            {
                throw new Exception($"{OnOFF} is not a valid state");
                return;
            }
            SetRelayState(state, relayInt);//set new state for the relay
            command = $"Relay {state} {relayInt}";
            writeToLog(command);//Write to log the command
        }
        
        public string ReadFirmwareVersion()
        {
            return Firmware;
        }
        //get ADC value, return -1 if invalid
        public int ReadAdcValue(int relayInt)
        {
            int returnADC;
            switch (relayInt)
            {
                case 0:
                    returnADC = ADC0;
                    break;
                case 1:
                    returnADC = ADC1;
                    break;
                case 2:
                    returnADC = ADC2;
                    break;
                case 3:
                    returnADC = ADC3;
                    break;
                default:
                    returnADC = -1;
                    break;
            }
            return returnADC;
        }
    }
}
