using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SoundFXData", menuName = "Audio/Sound FX Data")]
public class SoundFXData : ScriptableObject
{
    public List<SoundFXItem> soundFXList;
}

[System.Serializable]
public class SoundFXItem
{
    public SoundType type;
    public AudioClip clip;
}