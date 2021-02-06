using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameParamUIPresenter : MonoBehaviour
{
    public CharacterHpViewer CharacterHpViewer = null;

    public CharacterMpViewer CharacterMpViewer = null;

    public WaitGaugeViewer WaitGaugeViewer = null;

    public CenterUIViewer CenterUIViewer = null;

    public CharacterNameViewer CharacterNameViewer = null;
    public void SetCharactersParamViewer(CharacterParam[] CharacterParams)
    {
        for (int i = 0; i < 3; i++)
        {
            if (CharacterParams[i] != null)
            {
                CharacterHpViewer.CharacterMaxHps[i] = CharacterParams[i].HitPoint;
                CharacterHpViewer.CharacterHps[i] = CharacterParams[i].HitPoint;
                CharacterMpViewer.CharacterMaxMps[i] = CharacterParams[i].MagicPoint;
                CharacterMpViewer.CharacterMps[i] = CharacterParams[i].MagicPoint;
                WaitGaugeViewer.CharacterSpeeds[i] = CharacterParams[i].Speed;
                CharacterNameViewer.SetNameText(i, CharacterParams[i].Name);
            }
        }

        WaitGaugeViewer.Init();
    }
}
