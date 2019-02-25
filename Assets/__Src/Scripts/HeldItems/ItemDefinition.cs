﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Inventory/Item", order = 1)]
public class ItemDefinition : ScriptableObject
{
    public enum ItemType { Weapon, Active, Passive }
    public enum ItemRarity { Common, Uncommon, Rare, Mythic, Legendary, Ludicrous, Busted, Debug = -1 }

    public string ItemName = "???";
    public string ItemDescription = "Unknown Item";
    public ItemType Type;
    public ItemRarity Rarity;

    public GameObject DroppedModel;
    [HideInInspector]
    public DroppedItem DroppedScript;

    public GameObject HeldModel;
    [HideInInspector]
    public HeldItem HeldScript;

    public float SpeedModifier;
    public float HealthModifier;
    public float FireRateModifier;
    public float ProjectileSpeedModifier;
    public float DamageModifier;
}