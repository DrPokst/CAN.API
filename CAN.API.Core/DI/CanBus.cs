using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CAN.API.Core
{
    public class CanBus : ICanBus
    {
        public void Reset_Single(int NodeId)
        {
            CanTx transmit = new();
            byte[] data = new byte[] { (byte)CMD2.RESET_SINGLE, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            transmit.TransmitMessage(data, (byte)NodeId, (byte)CMD.SET);
        }

        public void Reset_All()
        {
            CanTx transmit = new();
            byte[] data = new byte[] { (byte)CMD2.RESET_ALL, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            transmit.TransmitMessage(data, 0, (byte)CMD.SET);
        }

        public void Flash_Start_Write(int NodeId, int appLenght, uint CRC)
        {
            byte[] appLenghtArray = BitConverter.GetBytes(appLenght);
            byte[] CRCBytes = BitConverter.GetBytes(CRC);

            CanTx transmit = new();
            byte[] data = new byte[] { (byte)CMD2.FLASH_START_WRITE, appLenghtArray[0], appLenghtArray[1], appLenghtArray[2], CRCBytes[0], CRCBytes[1], CRCBytes[2], CRCBytes[3] };
            transmit.TransmitMessage(data, (byte)NodeId, (byte)CMD.SET);
        }

        public void Send_Data(int NodeId, byte[] data)
        {
            CanTx transmit = new();
            transmit.TransmitMessage(data, (byte)NodeId, (byte)CMD.WRITE);
        }

        public void Jump_To_User_App(int NodeId)
        {
            CanTx transmit = new();
            byte[] data = new byte[] { (byte)CMD2.JUMP_TO_USER_APP, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            transmit.TransmitMessage(data, (byte)NodeId, (byte)CMD.SET);
        }

        public void Jump_To_Bootloader(int NodeId)
        {
            CanTx transmit = new();
            byte[] data = new byte[] { (byte)CMD2.JUMP_TO_BOOTLOADER, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            transmit.TransmitMessage(data, (byte)NodeId, (byte)CMD.SET);
        }

        public void Set_Can_Bus_Speed(int NodeId, int CanBusSpeed)
        {
            CanTx transmit = new();
            byte[] data = new byte[] { (byte)CMD2.SET_CAN_BUS_SPEED, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            transmit.TransmitMessage(data, (byte)NodeId, (byte)CMD.SET);
        }

        public void Turn_On_Single_Led(int reelLocation, string color, byte brightness)
        {
           
            var calculatedIdAndSlotNr = Helpers.IdCalculation(reelLocation);
            byte[] colorBytes = Helpers.GetBytesFromByteString(color).ToArray();
            
            int check = colorBytes.Length;

            if (check > 3) brightness = colorBytes[3];  //checking for brightness in color byte array 
            CanTx transmit = new();

            byte[] data = new byte[] { (byte)CMD2.TURN_ON_SINGLE_LED, (byte)calculatedIdAndSlotNr.Item1, 0x00, 0x00, colorBytes[0], colorBytes[1], colorBytes[2], brightness};
            transmit.TransmitMessage(data, (byte)calculatedIdAndSlotNr.Item2, (byte)CMD.SET);
        }

        public void Turn_Off_Single_Led(int reelLocation)
        {
            CanTx transmit = new();
            var calculatedIdAndSlotNr = Helpers.IdCalculation(reelLocation);

            byte[] data = new byte[] { (byte)CMD2.TURN_OFF_SINGLE_LED, (byte)calculatedIdAndSlotNr.Item1, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            transmit.TransmitMessage(data, (byte)calculatedIdAndSlotNr.Item2, (byte)CMD.SET);
        }

        public void Turn_On_All_Led(string color, byte brightness)
        {
            CanTx transmit = new();
            byte[] colorBytes = Helpers.GetBytesFromByteString(color).ToArray();

            int check = colorBytes.Length;

            if (check > 3) brightness = colorBytes[3];  //checking for brightness in color byte array 

            byte[] data = new byte[] { (byte)CMD2.TURN_ON_ALL_LED, 0x00, 0x00, 0x00, colorBytes[0], colorBytes[1], colorBytes[2], brightness };
            transmit.TransmitMessage(data, 0, (byte)CMD.SET);
        }

        public void Turn_Off_All_Led()
        {
            CanTx transmit = new();

            byte[] data = new byte[] { (byte)CMD2.TURN_OFF_ALL_LED, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            transmit.TransmitMessage(data, 0, (byte)CMD.SET);
        }

        public void Start_Reels_Registration(int reelId)
        {
            byte[] reelIdArray = BitConverter.GetBytes(reelId);

            CanTx transmit = new();
            byte[] data = new byte[] { (byte)CMD2.START_REELS_REGISTRATION, 0x00, reelIdArray[0], reelIdArray[1], 0x00, 0x00, 0x00, 0x00 };
            transmit.TransmitMessage(data, 0, (byte)CMD.SET);
        }

        public void Stop_Reels_Registration()
        {
            CanTx transmit = new();
            byte[] data = new byte[] { (byte)CMD2.STOP_REELS_REGISTRATION, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            transmit.TransmitMessage(data, 0, (byte)CMD.SET);
        }
        public void Start_Reels_Taking(int reelLocation, int reelId, string color, byte brightness)
        {
            var calculatedIdAndSlotNr = Helpers.IdCalculation(reelLocation);
            byte[] reelIdArray= BitConverter.GetBytes(reelId);
            byte[] colorBytes = Helpers.GetBytesFromByteString(color).ToArray();
            int check = colorBytes.Length;
            if (check > 3) brightness = colorBytes[3];

            CanTx transmit = new();
            byte[] data = new byte[] { (byte)CMD2.START_REELS_TAKING, (byte)calculatedIdAndSlotNr.Item1, reelIdArray[0], reelIdArray[1], colorBytes[0], colorBytes[1], colorBytes[2], brightness };
            transmit.TransmitMessage(data, (byte)calculatedIdAndSlotNr.Item2, (byte)CMD.SET);
        }

        public void Change_Can_Id() //dar ne 
        {
            CanTx transmit = new();
            byte[] data = new byte[] { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            transmit.TransmitMessage(data, 0, (byte)CMD.SET);
        }

        public void Send_Node_Id()  //dar ne 
        {
            CanTx transmit = new();
            byte[] data = new byte[] { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            transmit.TransmitMessage(data, 0, (byte)CMD.SET);
        }

        public void Stay_In_Bootloader() //dar ne 
        {
            CanTx transmit = new();
            byte[] data = new byte[] { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            transmit.TransmitMessage(data, 0, (byte)CMD.SET);
        }

        public void Node_State()
        {
            CanTx transmit = new();
            byte[] data = new byte[] { (byte)CMD2.NODE_STATE, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            transmit.TransmitMessage(data, 0, (byte)CMD.GET);
        }

        public void Node_Bootloader_Version()
        {
            CanTx transmit = new();
            byte[] data = new byte[] { (byte)CMD2.NODE_BOOTLOADER_VERSION, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            transmit.TransmitMessage(data, 0, (byte)CMD.GET);
        }

        public void Node_Firmware_Version()
        {
            CanTx transmit = new();
            byte[] data = new byte[] { (byte)CMD2.NODE_FIRMWARE_VERSION, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            transmit.TransmitMessage(data, 0, (byte)CMD.GET);
        }

        public void Clear_EEPROM_Memory(int NodeId)
        {
            CanTx transmit = new();
            byte[] data = new byte[] { (byte)CMD2.CLEAR_EEPROM_MEMORY, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            transmit.TransmitMessage(data, (byte)NodeId, (byte)CMD.SET);
        }

        public void Bootloader_Flag_Set(int NodeId)
        {
            CanTx transmit = new();
            byte[] data = new byte[] { (byte)CMD2.BOOTLOADER_FLAG_SET, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            transmit.TransmitMessage(data, (byte)NodeId, (byte)CMD.SET);
        }

        public void Bootloader_Flag_Reset(int NodeId)
        {
            CanTx transmit = new();
            byte[] data = new byte[] { (byte)CMD2.BOOTLOADER_FLAG_RESET, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            transmit.TransmitMessage(data, (byte)NodeId, (byte)CMD.SET);
        }

        public void Start_Node_ID_Numeration(int NumerationStartID)
        {
            CanTx transmit = new();
            byte[] NumerationStartIDArray = BitConverter.GetBytes(NumerationStartID);
            byte[] data = new byte[] { (byte)CMD2.START_NODE_ID_NUMERATION, NumerationStartIDArray[0], NumerationStartIDArray[1], 0x00, 0x00, 0x00, 0x00, 0x00 };
            transmit.TransmitMessage(data, 0, (byte)CMD.SET);
        }
    }
}
