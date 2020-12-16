using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newChargeState", menuName = "Data/State Data/chargeState Data")]
public class D_chargeState : ScriptableObject
{
    public float chargeSpeed = 3f;
    public float chargeTime = 0.5f;
}
