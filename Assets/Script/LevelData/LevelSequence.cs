using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSequence", menuName = "Scriptable Objects/Level Sequence")]
public class LevelSequence : ScriptableObject
{
    [SerializeField] private List<LevelStorage> levels = new List<LevelStorage>();

    public int Count => levels == null ? 0 : levels.Count;

    public LevelStorage GetLevel(int index)
    {
        if (levels == null || index < 0 || index >= levels.Count)
            return null;

        return levels[index];
    }
}
