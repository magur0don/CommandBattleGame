using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterNameViewer : MonoBehaviour
{
    /// <summary>
    /// 名前を表示するためのTextMeshProUGUIの配列
    /// </summary>
    public TextMeshProUGUI[] CharacterNameTexts = new TextMeshProUGUI[3];

    /// <summary>
    /// UIに名前をセットする
    /// </summary>
    public void SetNameText(int characterPos, string characterName)
    {
        CharacterNameTexts[characterPos].text = characterName;
    }
}
