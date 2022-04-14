using System;
using System.Collections.Generic;

/// <summary>
/// Add anything that needs saving here and GlobalControl.cs will save and load it
/// </summary>
[Serializable]
public class SaveValues
{
    public string SaveFile { get; set; }
    public bool MusicMuted { get; set; }
    public float MusicVolume { get; set; }
}
