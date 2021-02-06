using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterMpViewer : MonoBehaviour
{
    /// <summary>
    /// キャラクター達の現在のMp
    /// </summary>
    public int[] CharacterMps = new int[3];

    /// <summary>
    /// キャラクター達の最大Mp
    /// </summary>
    public int[] CharacterMaxMps = new int[3];

    /// <summary>
    /// Mpを表示するためのTextMeshProUGUIの配列
    /// </summary>
    public TextMeshProUGUI[] CharacterMPTexts = new TextMeshProUGUI[3];

    public void SetMp(int characterPos, int mp)
    {
        CharacterMps[characterPos] = mp;
    }

    public void SetMaxMp(int characterPos, int maxMp)
    {
        CharacterMaxMps[characterPos] = maxMp;
    }

    public void MpTextUpDate()
    {
        for (int i = 0; i < 3; i++)
        {
            CharacterMPTexts[i].text = $"{CharacterMps[i]}/{CharacterMaxMps[i]}";
        }
    }

    // Updateで計算をした後にMpの反映が入るようにLateUpdateを使用する
    private void LateUpdate()
    {
        MpTextUpDate();
    }
}
