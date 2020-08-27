using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor;
using UnityEngine.Rendering.Universal.Internal;

public class CsvToDialogueModelSO : MonoBehaviour
{
#if UNITY_EDITOR
    public string filepath = "Assets/Resources/story.tsv";

    public void ReadCSV()
    {
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(filepath);
        string firstLine = reader.ReadLine();
        string secondLine = reader.ReadLine();

        string currCharID = "";
        Dialogue currDialogue = null;
        DialogueModelSO currSO = null;
        int currDialogueIndex = 0;
        int currDialogueModelIndex = 0;
        DialogueModel currDialogueModel = null;

        Dictionary<string, DialogueModelSO> idToModelSO
            = new Dictionary<string, DialogueModelSO>();

        try
        {

            while (true)
            {
                if (reader.EndOfStream)
                    break;

                string line = reader.ReadLine();
                //Debug.Log(line);
                string[] splitByComma = line.Split('\t');

                // Check char id
                if (splitByComma[0] != "")
                {
                    currCharID = splitByComma[0];
                    if (!idToModelSO.ContainsKey(currCharID))
                        idToModelSO.Add(currCharID,
                                        ScriptableObject.CreateInstance<DialogueModelSO>());
                    currSO = idToModelSO[currCharID];
                    currSO.DialogueModels = new DialogueModel[0];
                }


                string completionFlag = splitByComma[1];
                string modelPreReqFlag = splitByComma[2];
                string modelPreReqUnmetFlag = splitByComma[3];
                string name = splitByComma[4];

                if (OneOfThemIsntEmpty(completionFlag, modelPreReqFlag, modelPreReqUnmetFlag, name))
                {
                    // Expand DialogueModels by 1
                    DialogueModel[] temp = new DialogueModel[currSO.DialogueModels.Length];
                    currSO.DialogueModels.CopyTo(temp, 0);

                    currSO.DialogueModels = new DialogueModel[currSO.DialogueModels.Length + 1];
                    temp.CopyTo(currSO.DialogueModels, 0);

                    // Create new
                    currDialogueModelIndex = currSO.DialogueModels.Length - 1;
                    currSO.DialogueModels[currDialogueModelIndex] = new DialogueModel();
                    currDialogueModel = currSO.DialogueModels[currDialogueModelIndex];
                    currDialogueModel.Dialogue = new Dialogue[0];

                    // Convert fields
                    currDialogueModel.FlagsToMarkComplete = FlagsFromString(completionFlag);
                    currDialogueModel.PrereqFlags = FlagsFromString(modelPreReqFlag);
                    currDialogueModel.PrereqUnmetFlags = FlagsFromString(modelPreReqUnmetFlag);
                    currDialogueModel.Name = name;

                    // Audio
                    {
                        string str = splitByComma[23];
                        float outFloat;
                        if (str != "" && float.TryParse(str, out outFloat))
                        {
                            currDialogueModel.Speed = outFloat;
                        }
                    }
                    {
                        string str = splitByComma[24];
                        float outFloat;
                        if (str != "" && float.TryParse(str, out outFloat))
                        {
                            currDialogueModel.Pitch = outFloat;
                        }
                    }
                    {
                        string str = splitByComma[25];
                        float outFloat;
                        if (str != "" && float.TryParse(str, out outFloat))
                        {
                            currDialogueModel.PitchVariance = outFloat;
                        }
                    }
                    {
                        string str = splitByComma[26];
                        float outFloat;
                        if (str != "" && float.TryParse(str, out outFloat))
                        {
                            currDialogueModel.Volume = outFloat;
                        }
                    }
                    {
                        string str = splitByComma[27];
                        float outFloat;
                        if (str != "" && float.TryParse(str, out outFloat))
                        {
                            currDialogueModel.VolumeVariance = outFloat;
                        }
                    }

                    // Audio Clip
                    string audioStr = splitByComma[28];
                    if (audioStr == "")
                    {
                        audioStr = "DialogueTick";
                    }

                    currDialogueModel.TickAudioClip =
                        (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Audio/" + audioStr, typeof(AudioClip));

                }

                string dialogueText = splitByComma[5];
                if (dialogueText == "")
                {
                    Debug.LogWarning("DIALOGUE TEXT SHOULD NEVER BY EMPTY, OFF BY ONE ERROR?");
                }
                else
                {
                    Dialogue[] temp = new Dialogue[currDialogueModel.Dialogue.Length];
                    currDialogueModel.Dialogue.CopyTo(temp, 0);

                    currDialogueModel.Dialogue = new Dialogue[currDialogueModel.Dialogue.Length + 1];
                    temp.CopyTo(currDialogueModel.Dialogue, 0);

                    // Create new
                    currDialogueIndex = currDialogueModel.Dialogue.Length - 1;
                    currDialogueModel.Dialogue[currDialogueIndex] = new Dialogue();
                    currDialogue = currDialogueModel.Dialogue[currDialogueIndex];
                    currDialogue.Text = dialogueText;
                }

                currDialogue.PrereqFlag = FlagsFromString(splitByComma[6]);
                currDialogue.PrereqUnmetFlags = FlagsFromString(splitByComma[7]);

                string[] optionText =
                {
                    splitByComma[8],
                    splitByComma[11],
                    splitByComma[14],
                    splitByComma[17],
                    splitByComma[20],
                };
                string[] preReqForOpt =
    {
                    splitByComma[9],
                    splitByComma[12],
                    splitByComma[15],
                    splitByComma[18],
                    splitByComma[21],
                };
                string[] flagToFlipForOpt =
    {
                    splitByComma[10],
                    splitByComma[13],
                    splitByComma[16],
                    splitByComma[19],
                    splitByComma[22],
                };

                int optionsSize = CountNotEmpty(optionText);
                currDialogue.Options = new DialogueOption[optionsSize];
                for (int i = 0; i < optionsSize; i++)
                {
                    currDialogue.Options[i] = new DialogueOption();
                    DialogueOption opt = currDialogue.Options[i];
                    opt.DialogueText = optionText[i];
                    opt.RequiredFlagsForDialogue = FlagsFromString(preReqForOpt[i]);
                    opt.FlagsToMarkAsComplete = FlagsFromString(flagToFlipForOpt[i]);
                }
            }
        }
        finally
        {
            reader.Close();
        }

        foreach (string id in idToModelSO.Keys)
        {
            AssetDatabase.CreateAsset(idToModelSO[id], "Assets/DialogueModels/" + id + ".asset");
            AssetDatabase.SaveAssets();
        }
    }

    private bool OneOfThemIsntEmpty(params string[] strings)
    {
        foreach (string s in strings)
        {
            if (s != "")
                return true;
        }

        return false;
    }

    private FlagManager.EventFlag[] FlagsFromString(string input)
    {
        if (input == "")
            return new FlagManager.EventFlag[0];

        string[] separatedInput = input.Split(',');
        FlagManager.EventFlag[] flags = new FlagManager.EventFlag[separatedInput.Length];

        for (int i = 0; i < separatedInput.Length; i++)
        {
            flags[i] = (FlagManager.EventFlag)Enum.Parse(
                typeof(FlagManager.EventFlag), separatedInput[i]);
        }

        return flags;
    }

    private int CountNotEmpty(string[] strings)
    {
        int count = 0;
        foreach (string s in strings)
        {
            if (s != "")
                count++;
        }

        return count;
    }
#endif
}
