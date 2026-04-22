using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SOfileManager", menuName = "Scriptable Objects/SOfileManager")]
public class SOfileManager : ScriptableObject
{
    public List<DialogueDataSO> StortFlow;
}
