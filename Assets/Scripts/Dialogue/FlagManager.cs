using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class FlagCompletionEvent : UnityEvent<FlagManager.EventFlag>
{
}

public class FlagManager : Singleton<FlagManager>
{
    public enum EventFlag
    {
        Invalid = 0,
        TutHellHoundSit = 1,
        TutHellHoundSleep = 2,
        TutHellHoundShock = 3,
        StartingSeq = 4,
        StartingSeq1 = 5,
        StartingSeq1Huh = 6,
        StartingSeq1Cat = 7,
        StartingSeq2 = 8,
        StartingSeq2Ew = 9,
        StartingSeq2Crops = 10,
        StartingSeq2NoQ = 11,
        StartingSeq2Repeat = 12,
        StartingSeq2Pet = 13,
        StartingSeq2PetBackAway = 14,
        StartingSeq2PetHead = 15,
        StartingSeq2PetTummy = 16,
        CameraReset = 17,
        StartingSeq2PetZoom1 = 18,
        StartingSeq2PetZoom2 = 19,
        StartingSeq2PetZoom3 = 20,
        SeedPerson_PersonWithoutGlasses_MoveHandToFace = 21,
        SeedPerson_PersonWithoutGlasses_ImmaCry = 22,
        SeedPerson_PersonWithoutGlasses_SayWhat = 23,
        SeedPerson_PersonWithoutGlasses_WeirdHandGesture = 24,
        SeedPerson_PersonWithoutGlasses_HandBehindHead = 25,
        SeedPerson_PersonWithoutGlasses_WalkForward = 26,
        SeedPerson_PersonWithoutGlasses_WalkBackward = 27,
        SeedPerson_SeedAppear = 28,
        SeedPerson_SeedToPlayer = 29,
        PlantSpot0_ShowSeedling = 30,
        PlantSpot0_ShowBud = 31,
        PlantSpot0_ShowClosedFlower = 32,
        PlantSpot0_ShowOpenFlower = 33,
        PlantSpot0_Harvest = 34,
        PlantSpot0_GivePerson = 35,
        PlantSpot1_ShowSeedling = 36,
        PlantSpot1_ShowBud = 37,
        PlantSpot1_ShowClosedFlower = 38,
        PlantSpot1_ShowOpenFlower = 39,
        PlantSpot1_Harvest = 40,
        PlantSpot1_GivePerson = 41,
        PlantSpot2_ShowSeedling = 42,
        PlantSpot2_ShowBud = 43,
        PlantSpot2_ShowClosedFlower = 44,
        PlantSpot2_ShowOpenFlower = 45,
        PlantSpot2_Harvest = 46,
        PlantSpot2_GivePerson = 47,
        CantFindGlassesPerson_introduction = 56,
        CantFindGlassesPerson_seedStage0 = 55,
        CantFindGlassesPerson_seedStage0_Pickle = 48,
        CantFindGlassesPerson_seedStage0_Help = 49,
        CantFindGlassesPerson_seedStage0_Uh = 50,
        CantFindGlassesPerson_seedStage0_Eat = 51,
        CantFindGlassesPerson_seedStage0_Magic = 62,
        CantFindGlassesPerson_seedStage0_Plant = 63,
        CantFindGlassesPerson_seedStage0_Eat2 = 64,
        CantFindGlassesPerson_seedStage0_Awkward = 65,
        CantFindGlassesPerson_seedStage0_AteTheSeed = 66,
        CantFindGlassesPerson_seedStage_Planted = 61,
        CantFindGlassesPerson_seedStage1 = 57,
        CantFindGlassesPerson_seedStage1_Joke = 58,
        CantFindGlassesPerson_seedStage1_SoundDifferent = 52,
        CantFindGlassesPerson_seedStage1_SoundDifferent_Confident = 53,
        CantFindGlassesPerson_seedStage1_SoundDifferent_Plant = 54,
        CantFindGlassesPerson_seedStage1_GiveTime = 59,
        CantFindGlassesPerson_seedStage1_MagicSpell = 60,
        TutHellHound_AteSeed0 = 72,
        TutHellHound_AteSeed0_Great = 67,
        TutHellHound_AteSeed0_Silence = 68,
        TutHellHound_AteSeed0_DoJustThat = 69,
        TutHellHound_AteSeed0_Uh = 70,
        TutHellHound_AteSeed0_Confess = 71,
        TutHellHound_AteSeed0_Lie = 73,
        ShowDialogueImagePanel = 74,
        HideDialogueImagePanel = 84,
        SeedPerson_ImagePanel_PersonWithoutGlasses_Idle = 75,
        SeedPerson_ImagePanel_PersonWithoutGlasses_LookRight = 76,
        Dontselecthitthisdoesnothing = 77,
        SeedPerson_ImagePanel_PersonWithoutGlasses_SlightSmile = 78,
        SeedPerson_ImagePanel_PersonWithoutGlasses_ThinkingFace = 79,
        SeedPerson_ImagePanel_PersonWithoutGlasses_SmallAngie = 80,
        SeedPerson_ImagePanel_PersonWithoutGlasses_BigAngie = 81,
        SeedPerson_ImagePanel_PersonWithoutGlasses_HeadScratch = 82,
        SeedPerson_ImagePanel_PersonWithoutGlasses_Ashamed = 83,
        BGM_CoolChill = 85,
        BGM_HahaDogBark = 86,
        BGM_LowFi = 87,
        BGM_Mystery = 88,
        BGM_Perion = 89,
        BGM_SettingUpStory = 90,
        BGM_StopMusic = 91,
        CantFindGlassesPerson_seedStage1_CantHear = 92,
        CantFindGlassesPerson_seedStage1_CantHear2 = 93,
        CantFindGlassesPerson_seedStage1_CantHear3 = 94,
        CantFindGlassesPerson_seedStage1_CantHear4 = 123,
        CantFindGlassesPerson_seedStage1_Magitech = 95,
        CantFindGlassesPerson_seedStage1_Magitech_Agree = 96,
        CantFindGlassesPerson_seedStage1_Magitech_Agree_PushOn = 97,
        CantFindGlassesPerson_seedStage1_Magitech_Agree_PushOn_Yep = 98,
        CantFindGlassesPerson_seedStage1_Magitech_Agree_PushOn_Yep_VideoGame = 99,
        CantFindGlassesPerson_seedStage1_Magitech_Agree_PushOn_Yep_VideoGame_Protagonist = 100,
        CantFindGlassesPerson_seedStage1_Magitech_Agree_PushOn_Yep_VideoGame_Antagonist = 101,
        CantFindGlassesPerson_seedStage1_Magitech_Agree_PushOn_Yep_VideoGame_NPC = 102,
        CantFindGlassesPerson_seedStage1_Magitech_Agree_PushOn_Yep_Book = 103,
        CantFindGlassesPerson_seedStage1_Magitech_Confused = 104,
        CantFindGlassesPerson_seedStage1_Magitech_Ignore = 105,
        CantFindGlassesPerson_seedStage1_Different = 106,
        CantFindGlassesPerson_seedStage1_Different_Sound = 107,
        CantFindGlassesPerson_seedStage1_Different_Sound_Plant = 108,
        CantFindGlassesPerson_seedStage1_Different_Confidence = 109,
        CantFindGlassesPerson_seedStage1_Different_Confidence_Prod = 110,
        CantFindGlassesPerson_seedStage1_Different_Confidence_Prod_Prod = 111,
        CantFindGlassesPerson_seedStage1_Different_Confidence_Prod_Prod_Sympathize = 113,
        CantFindGlassesPerson_seedStage1_EndConvo_Good = 114,
        CantFindGlassesPerson_seedStage1_Different_Confidence_Prod_Prod_Empathize = 115,
        CantFindGlassesPerson_seedStage1_Different_Confidence_Prod_Prod_SelfInsert = 116,
        CantFindGlassesPerson_seedStage1_Different_Confidence_Prod_Prod_Acknowledge = 117,
        CantFindGlassesPerson_seedStage1_Different_Confidence_Prod_Prod_Sympathize_Salvage = 118,
        CantFindGlassesPerson_seedStage1_Different_Confidence_Prod_Prod_Joke = 119,
        CantFindGlassesPerson_seedStage1_Different_Confidence_Prod_Confront = 120,
        CantFindGlassesPerson_seedStage1_Different_Confidence_Prod_Confront_Salvage = 121,
        CantFindGlassesPerson_seedStage1_EndConvo_Bad = 122,
        CantFindGlassesPerson_seedStage1_NotEnoughRapport = 124,
        CantFindGlassesPerson_seedStage2 = 125,
        CantFindGlassesPerson_seedStageIntermission = 126,
        Platform_PC = 127,
        Platform_Web = 128,
        StartingSeq1Repeat = 129,
        // next = 127
        TOTALCOUNT,
    }

    [SerializeField]
    EventFlag flagToMarkAsComplete = EventFlag.Invalid;

    [SerializeField]
    FlagCompletionEvent flagCompletedEvent = new FlagCompletionEvent();

    public void AddListener(UnityAction<EventFlag> eventCompletedCb)
    {
        flagCompletedEvent.AddListener(eventCompletedCb);
    }

    public void RemoveListener(UnityAction<EventFlag> eventCompletedCb)
    {
        flagCompletedEvent.RemoveListener(eventCompletedCb);
    }

    HashSet<EventFlag> completedFlags_ = new HashSet<EventFlag>();

    [SerializeField]
    EventFlag[] completedFlags_DEBUG;

    [SerializeField]
    EventFlag flagToToggle;

    public void TurnInspectorFlagOn()
    {
        Debug.Log("Turned ON " + flagToToggle);
        completedFlags_.Add(flagToToggle);
        flagCompletedEvent.Invoke(flagToToggle);
    }

    public void TurnInspectorFlagOff()
    {
        completedFlags_.Remove(flagToToggle);
        Debug.Log("Turned OFF " + flagToToggle);
    }

    public bool SetFlagCompletion(EventFlag flag)
    {
        if (completedFlags_.Contains(flag))
        {
            Debug.LogWarning("Flipping Flag " + flag + " eventhough it's already flipped.");
            return false;
        }

        completedFlags_.Add(flag);
        flagCompletedEvent.Invoke(flag);
        return true;
    }

    public bool UnsetFlagCompletion(EventFlag flag)
    {
        if (!completedFlags_.Remove(flag))
        {
            Debug.LogWarning("Unsetting  Flag " + flag + " eventhough it doesn't exist.");
            return false;
        }

        return true;
    }

    public bool SetFlagCompletion(EventFlag[] flags)
    {
        foreach (EventFlag f in flags)
        {
            if (completedFlags_.Contains(f))
            {
                Debug.LogWarning("Flipping Flag " + f + " eventhough it's already flipped.");
            }

            completedFlags_.Add(f);
            flagCompletedEvent.Invoke(f);
        }
        return true;
    }

    public bool GetFlagCompletion(EventFlag flag)
    {
        return completedFlags_.Contains(flag);
    }

    public bool GetFlagCompletion(EventFlag[] flags)
    {
        foreach (EventFlag f in flags)
        {
            if (!completedFlags_.Contains(f))
                return false;
        }

        return true;
    }

    public bool GetFlagUnmet(EventFlag[] flags)
    {
        foreach (EventFlag f in flags)
        {
            if (completedFlags_.Contains(f))
                return false;
        }

        return true;
    }

    public void ClearAllFlags()
    {
        completedFlags_ = new HashSet<EventFlag>();
        SetFlagCompletion(EventFlag.Invalid);

        if (Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.OSXPlayer ||
            Application.platform == RuntimePlatform.LinuxPlayer)
        {
            SetFlagCompletion(EventFlag.Platform_Web);
        }

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            SetFlagCompletion(EventFlag.Platform_PC);
        }
    }

    private void Start()
    {
        completedFlags_DEBUG = new EventFlag[(int)EventFlag.TOTALCOUNT];

        SetFlagCompletion(EventFlag.Invalid);

#if UNITY_EDITOR
        SetFlagCompletion(flagToMarkAsComplete);
#endif

        if (Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.OSXPlayer ||
            Application.platform == RuntimePlatform.LinuxPlayer)
        {
            SetFlagCompletion(EventFlag.Platform_Web);
        }

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            SetFlagCompletion(EventFlag.Platform_PC);
        }
    }

    private void Update()
    {
        if (Debug.isDebugBuild || Application.isEditor)
        {
            completedFlags_.CopyTo(completedFlags_DEBUG);

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.D))
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (EventFlag f in completedFlags_)
                {
                    stringBuilder.Append(f + ", ");
                }
                Debug.Log(stringBuilder.ToString());
            }
        }
    }

}
