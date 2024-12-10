using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioScriptable : ScriptableObject
{
    public List<Sound> SoundDatabase = new List<Sound>();
}
