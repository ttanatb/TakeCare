using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DialogueModelSO", order = 1)]

public class DialogueModelSO : ScriptableObject
{
    public DialogueModel[] DialogueModels;
}
