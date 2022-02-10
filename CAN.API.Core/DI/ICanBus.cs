using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAN.API.Core
{
    public interface ICanBus
    {
        void Reset_Single(int NodeId);
        void Reset_All();
        void Flash_Start_Write(int NodeId, int appLenght, uint CRC);
        void Send_Data(int NodeId, byte[] data);
        void Jump_To_User_App(int NodeId);
        void Jump_To_Bootloader(int NodeId);
        void Set_Can_Bus_Speed(int NodeId, int CanBusSpeed);
        void Turn_On_Single_Led(int reelLocation, string color, byte brightness);
        void Turn_Off_Single_Led(int reelLocation);
        void Turn_On_All_Led(string color, byte brightness);
        void Turn_Off_All_Led();
        void Start_Reels_Registration(int reelId);
        void Stop_Reels_Registration();
        void Start_Reels_Taking(int reelLocation, int reelId, string color, byte brightness);
        void Change_Can_Id();
        void Send_Node_Id();
        void Stay_In_Bootloader();
        void Node_State();
        void Node_Bootloader_Version();
        void Node_Firmware_Version();
        void Clear_EEPROM_Memory(int NodeId);
        void Bootloader_Flag_Set(int NodeId);
        void Bootloader_Flag_Reset(int NodeId);
        void Start_Node_ID_Numeration(int NumerationStartID);


    }
}
