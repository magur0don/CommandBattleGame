using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParamManager : MonoBehaviour
{
    /// <summary>
    /// CharacterParamManagerから参照できるCharacterParam
    /// </summary>
    public CharacterParam CharacterParam = new CharacterParam();


    // 本来はサーバーから送られてくる事が多いのですが、今回は各キャラクター毎に設定します
    public string CharacterName = string.Empty;
    public int CharacterHP = 0;
    public int CharacterMP = 0;
    public float CharacterSpeed = 0;
    public int CharacterAttack = 0;

    /// <summary>
    /// 敵かどうかの判定(Unityで設定する)
    /// </summary>
    public bool IsEnemy = false;

    /// <summary>
    /// キャラクターのタイプ(Unityで設定する)
    /// </summary>
    public CharacterParam.GameCharacterType CharacterType = CharacterParam.GameCharacterType.Invalide;

    /// <summary>
    /// キャラクターのAnimationController
    /// </summary>
    public CharacterAnimationController CharacterAnimationController = null;

    /// <summary>
    /// 敵の行動するまでの時間
    /// </summary>
    private float attackSpan;

    private void Init()
    {
        CharacterParam.Name = CharacterName;
        CharacterParam.HitPoint = CharacterHP;
        CharacterParam.MagicPoint = CharacterMP;
        CharacterParam.Speed = CharacterSpeed;
        CharacterParam.CharacterType = CharacterType;
        CharacterParam.IsEnemy = IsEnemy;

        if (!IsEnemy)
        {
            CharacterParam.FirstButtonAction = FirstButtonAction;
            CharacterParam.SecondButtonAction = SecondButtonAction;
            CharacterParam.ThirdButtonAction = ThirdButtonAction;
            CharacterParam.FourthButtonAction = FourthButtonAction;
        }
        else
        {
            attackSpan = 10f;
        }

        CharacterAnimationController = this.gameObject.GetComponent<CharacterAnimationController>();
    }


    private void Update()
    {
        if (IsEnemy)
        {
            attackSpan -= Time.deltaTime;
            if (attackSpan < 0)
            {
                StartCoroutine(CharacterAnimationController.StartAttackAnimation(2));
                attackSpan =10f;
            }
        }
    }

    private void Awake()
    {
        Init();
    }

    public int ButtonNo = 0;

    private void FirstButtonAction()
    {
        ButtonNo = 0;
        StartCoroutine(CharacterAnimationController.StartAttackAnimation(2));
    }

    private void SecondButtonAction()
    {
        ButtonNo = 1;
        StartCoroutine(CharacterAnimationController.StartAttackAnimation(2));
    }

    private void ThirdButtonAction()
    {
        ButtonNo = 3;
        StartCoroutine(CharacterAnimationController.StartAttackAnimation(2));
    }
    private void FourthButtonAction()
    {
        Debug.Log("dd");
    }

    public CharacterHpViewer characterHpViewer = null;

    public void Damage(int damage)
    {
        var characterPos = CharacterParam.CharacterPos;

        CharacterParam.HitPoint -= damage;
        CharacterHP = CharacterParam.HitPoint;

        if (!IsEnemy)
        {
            // 回復でかつ、最大値を越えてしまった場合
            if (damage < 0 && 
                characterHpViewer.CharacterMaxHps[characterPos] < CharacterHP)
            {
                CharacterHP = characterHpViewer.CharacterMaxHps[characterPos];
                CharacterParam.HitPoint = CharacterHP;
            }
            characterHpViewer.SetHp(characterPos, CharacterHP);
        }
    }
}
