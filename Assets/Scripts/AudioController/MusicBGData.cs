using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MusicBGData", menuName = "Audio/Music BG Data")]
public class MusicBGData : ScriptableObject
{
    public List<MusicItem> musicList;
}

[System.Serializable]
public class MusicItem
{
    public SoundType type;
    public AudioClip clip;
}