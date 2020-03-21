using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player/PlayerStats", order = 1)]
public class PlayerStats : ScriptableObject
{
    public int HealthPoints;
    public int DamageAttackPoints;
}
