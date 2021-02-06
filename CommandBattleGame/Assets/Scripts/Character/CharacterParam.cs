using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParam
{
    public enum GameCharacterType
    {
        Invalide,
        Attacker,
        SpellCaster,
        Healer,
    }
    
    public string Name;
    public int HitPoint;
    public int MagicPoint;
    public float Speed;
    public GameCharacterType CharacterType;
    
    public int Attack;

    public bool IsEnemy;

    public int CharacterPos;

    public Action FirstButtonAction;
    public Action SecondButtonAction;
    public Action ThirdButtonAction;
    public Action FourthButtonAction;
}
