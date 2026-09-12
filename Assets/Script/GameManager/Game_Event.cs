using System;
using UnityEngine;

public class Game_Event : MonoBehaviour
{
    public static Action checkIfPathCanBePlace;

    public static Action MovePathToStartPosition;

    public static Action SetPathInactive;

    public static Action RequestNewPath;

    public static Action<int> CostValue;

}
