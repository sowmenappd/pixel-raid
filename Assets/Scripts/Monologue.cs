using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Monologue {
    [TextArea(2, 4)][Tooltip("All text strings are to be played in array sequence")]
    public string[] text;
}
