using UnityEngine;

[CreateAssetMenu(fileName = "LevelStorage", menuName = "Scriptable Objects/Level Storage")]
public class LevelStorage : ScriptableObject
{
    [SerializeField] private GridLevel gridLevel;
    [SerializeField] private Storage pathStorage;

    public GridLevel GridLevel => gridLevel;
    public Storage PathStorage => pathStorage;

    public bool IsValid()
    {
        return gridLevel != null && pathStorage != null;
    }
}
