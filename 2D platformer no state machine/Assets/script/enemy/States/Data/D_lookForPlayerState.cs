using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newLookForPlayer", menuName = "Data/State Data/look For Player Data")]
public class D_lookForPlayerState : ScriptableObject
{
    public int amountOfTurns = 1;
    public float timeBetweenTurns = 0.5f;
}
