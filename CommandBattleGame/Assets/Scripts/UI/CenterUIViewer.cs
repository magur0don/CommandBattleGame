using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CenterUIViewer : MonoBehaviour
{

    public Button[] CharacterActionButtons = new Button[4];
    public bool CenterUIVisible = false;

    public bool CheckCenterUIVisible(float gaugeRate)
    {
        return gaugeRate >= 1f;
    }

    public void SetCenterUIVisible(bool active)
    {
        this.gameObject.SetActive(active);
    }

    public void SetCharacterActionButtons(CharacterParam characterParam, Action resetWaitGaugeRate)
    {
        for (int i = 0; i < CharacterActionButtons.Length; i++)
        {
            CharacterActionButtons[i].onClick.RemoveAllListeners();
        }

        CharacterActionButtons[0].onClick.AddListener(() =>
        {
            characterParam.FirstButtonAction();
            SetCenterUIVisible(false);
            resetWaitGaugeRate();
        });

        CharacterActionButtons[1].onClick.AddListener(() =>
        {
            characterParam.SecondButtonAction();
            SetCenterUIVisible(false);
            resetWaitGaugeRate();
        });

        CharacterActionButtons[2].onClick.AddListener(() =>
        {
            characterParam.ThirdButtonAction();
            SetCenterUIVisible(false);
            resetWaitGaugeRate();
        });

        CharacterActionButtons[3].onClick.AddListener(() =>
        {
            characterParam.FourthButtonAction();
            SetCenterUIVisible(false);
            resetWaitGaugeRate();
        });
    }
}
