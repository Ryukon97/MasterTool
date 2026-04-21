using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "SOfileManager", menuName = "Scriptable Objects/SOfileManager")]
public class SOfileManager : ScriptableObject
{
    public List<DialogueDataSO> StortFlow;
}
