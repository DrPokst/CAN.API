using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAN.API.Core
{
    enum CMD2
    {
        FLASH_START_WRITE = 0x01,
        JUMP_TO_USER_APP = 0x02,
        JUMP_TO_BOOTLOADER = 0x03,
        RESET_SINGLE = 0x04,
        RESET_ALL = 0x05,
        SET_CAN_BUS_SPEED = 0x06,
        TURN_ON_SINGLE_LED = 0x07,
        TURN_OFF_SINGLE_LED = 0x08,
        TURN_ON_ALL_LED = 0x09,
        TURN_OFF_ALL_LED = 0x10,
        START_REELS_REGISTRATION = 0x11,
        STOP_REELS_REGISTRATION = 0x12,
        START_REELS_TAKING = 0x13,
        CHANGE_CAN_ID = 0x14,
        SEND_NODE_ID = 0x15,
        STAY_IN_BOOTLOADER = 0x16,
        NODE_STATE = 0x17,
        NODE_BOOTLOADER_VERSION = 0x18,
        NODE_FIRMWARE_VERSION = 0x19,
        CLEAR_EEPROM_MEMORY = 0x20,
        BOOTLOADER_FLAG_SET = 0x27,
        BOOTLOADER_FLAG_RESET = 0x28,
        START_NODE_ID_NUMERATION = 0x29

    }

}
