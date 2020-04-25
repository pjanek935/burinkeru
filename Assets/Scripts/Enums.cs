using System.Collections;
using System.Collections.Generic;

public static class Enums
{
    public enum ParameterType
    {
        ATTACK_TYPE, SENDER_TYPE
    }

    public enum WeaponActionType
    {
        NONE = 0, UPPERCUT, STAB, SLASH, SHOOT, RELOAD, BLOCK, DOWNWARD_SMASH
    }
        
    public enum SenderType
    {
        OBJECT, NPC
    }

    public enum AnimationTrigger
    {
        BLOCK, 
    }
}
