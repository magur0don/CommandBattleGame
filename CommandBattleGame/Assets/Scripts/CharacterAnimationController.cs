using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    public Animator CharacterAnimator = null;

    private const string AttackAnimationName = "Attack";

    private const string IdleAnimationStateName = "Idle";

    private const string GotoMoveAnimationStateName = "GotoMove";

    public Transform CharacterRoot = null;

    public Transform AttackRoot = null;

    private CharacterParamManager targetCharacterParamManager = null;

    private CharacterParamManager characterParamManager = null;

    public GameMainManager GameMainManager = null;

    public bool AnimationEnd = false;
    /// <summary>
    /// 操作するキャラクタータイプ
    /// </summary>
    public enum CharacterType
    {
        Invalide,
        Attacker,
        SpellCaster,
        Healer,
    }

    public CharacterType characterType = CharacterAnimationController.CharacterType.Invalide;

    private void Awake()
    {
        CharacterAnimator = GetComponent<Animator>();
        characterParamManager = GetComponent<CharacterParamManager>();
    }


    public IEnumerator StartAttackAnimation(int attackId)
    {
        AnimationEnd = false;

        // 敵だった場合はGameMainManager.GameStateを無視する
        if (!characterParamManager.IsEnemy)
        {
            // GameManagerがActionになるまで待つ
            yield return new WaitUntil(() => GameMainManager.GameState == GameMainManager.State.Action);
        }


        // キャラクタータイプがAttackerだったら行って攻撃、返ってくるの処理
        if (characterType == CharacterType.Attacker)
        {
            yield return StartCoroutine(StartMove());
            yield return StartCoroutine(StartAnimation(attackId));
            yield return StartCoroutine(ReturnMove());
        }
        else
        {
            // 1フレーム待つ
            yield return null;
            yield return StartCoroutine(StartAnimation(attackId));
        }

        // 今のステートのアニメーションが終わるまで待つ
        yield return new WaitWhile(() =>
       CharacterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f);

        // Attackステートのアニメーションが終わるまで待つ
        yield return new WaitWhile(() =>
        CharacterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f &&
        CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName(AttackAnimationName));

        AnimationEnd = true;

    }

    IEnumerator StartMove()
    {
        SetAttackAnimtion(1);
        var distance_two = Vector3.Distance(transform.position, AttackRoot.position);
        var elapsedTime = 0f;
        float waitTime = 1f;
        while (elapsedTime < waitTime)
        {
            this.transform.position = Vector3.Lerp(transform.position, AttackRoot.position, (elapsedTime / waitTime) / distance_two);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    // アタックアニメーション
    IEnumerator StartAnimation(int attackNo)
    {
        SetAttackAnimtion(attackNo);
        if (characterType == CharacterType.Attacker)
        {
            // GotoMoveの間待つ
            yield return new WaitWhile(() => CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName(GotoMoveAnimationStateName));
        }
        else
        {
            //1フレーム待つ
            yield return null;
        }
        // Attackの値を0に戻す
        CharacterAnimator.SetInteger(AttackAnimationName, 0);
    }

    // 戻るときの移動
    IEnumerator ReturnMove()
    {
        var distance_two = Vector3.Distance(transform.position, CharacterRoot.position);
        var elapsedTime = 0f;
        float waitTime = 1f;
        while (this.transform != CharacterRoot && elapsedTime < waitTime)
        {
            this.transform.position = Vector3.Lerp(transform.position, CharacterRoot.position, (elapsedTime / waitTime) / distance_two);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void SetAttackAnimtion(int attackNo)
    {
        if (CharacterAnimator == null)
        {
            return;
        }
        CharacterAnimator.SetInteger(AttackAnimationName, attackNo);
    }


    public void Hit()
    {
        if (characterParamManager.IsEnemy)
        {
            // 0~2まで選ばれる
            var pos = Random.Range(0, 3);
            targetCharacterParamManager = GameMainManager.CharacterParamManagers[pos];
        }
        else
        {
            targetCharacterParamManager = AttackRoot.transform.parent.GetComponentInChildren<CharacterParamManager>();
        }

        // キャラクターのアタックをそのままダメージとしてつかう
        var damage = characterParamManager.CharacterAttack;

        // MPを消費する魔法や技
        if (characterParamManager.ButtonNo == 1)
        {
            damage *= 10;
            characterParamManager.CharacterMP -= 10;

        }

        // ヒーラーの回復
        if (characterParamManager.ButtonNo == 3 && characterParamManager.CharacterType == CharacterParam.GameCharacterType.Healer)
        {
            damage *= -10;
            characterParamManager.CharacterMP -= 10;
            var min = GameMainManager.CharacterParamManagers[0].CharacterHP;
            targetCharacterParamManager = GameMainManager.CharacterParamManagers[0];

            for (int i = 0; i < GameMainManager.CharacterParamManagers.Length; i++)
            {
                if (GameMainManager.CharacterParamManagers[i].CharacterHP < min)
                {
                    min = GameMainManager.CharacterParamManagers[i].CharacterHP;
                    targetCharacterParamManager = GameMainManager.CharacterParamManagers[i];
                }
            }
        }

        targetCharacterParamManager.Damage(damage);
    }
}

