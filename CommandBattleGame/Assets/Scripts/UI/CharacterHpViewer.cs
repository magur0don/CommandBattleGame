using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterHpViewer : MonoBehaviour
{
    /// <summary>
    /// キャラクター達の現在のHp
    /// </summary>
    public int[] CharacterHps = new int[3];
    
    /// <summary>
    /// キャラクター達の最大Hp
    /// </summary>
    public int[] CharacterMaxHps = new int[3];

    /// <summary>
    /// Hpを表示するためのTextMeshProUGUIの配列
    /// </summary>
    public TextMeshProUGUI[] CharacterHPTexts = new TextMeshProUGUI[3];

    /// <summary>
    /// Hpをセットする
    /// </summary>
    /// <param name="characterPos">characterの指定</param>
    /// <param name="hp">現在のhp</param>
    public void SetHp(int characterPos, int hp)
    {
        CharacterHps[characterPos] = hp;
    }
    public void SetMaxHp(int characterPos, int maxHp)
    {
        CharacterMaxHps[characterPos] = maxHp;
    }
    public void HpTextUpDate()
    {
        for (int i = 0; i < 3; i++)
        {
            CharacterHPTexts[i].text = $"{CharacterHps[i]}/{CharacterMaxHps[i]}";
        }
    }

    // Updateで計算をした後にHpの反映が入るようにLateUpdateを使用する
    private void LateUpdate()
    {
        HpTextUpDate();
    }
}
