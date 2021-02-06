using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainManager : MonoBehaviour
{
    public enum State
    {
        Init,
        Start,
        Command,
        CameraAction,
        Action,
        Result,
    }
    public State GameState = State.Init;

    public GameParamUIPresenter gameParamUIPresenter;

    public CharacterParamManager[] CharacterParamManagers = new CharacterParamManager[3];

    private CharacterParam[] characterParams = new CharacterParam[3];

    private int fastCharacterPos = 0;

    public CharacterParamManager EnemyCharacterParamManager = null;

    public DollyCameraManager DollyCameraManager; 

    private void Update()
    {
        if (EnemyCharacterParamManager.CharacterHP < 0)
        {
            GameState = State.Result;
        }

        switch (GameState)
        {
            case State.Init:
                for (int i = 0; i < 3; i++)
                {
                    if (CharacterParamManagers[i] != null)
                    {
                        characterParams[i] = CharacterParamManagers[i].CharacterParam;
                        CharacterParamManagers[i].CharacterParam.CharacterPos = i;
                    }
                }
                gameParamUIPresenter.SetCharactersParamViewer(characterParams);
                GameState = State.Start;
                break;
            case State.Start:
                //3キャラクターのゲージを監視して一番早かったキャラクターのコマンドに進む
                for (int i = 0; i < 3; i++)
                {
                    if (gameParamUIPresenter.CenterUIViewer.CheckCenterUIVisible(gameParamUIPresenter.WaitGaugeViewer.GetWaitGaugeRate(i)))
                    {
                        fastCharacterPos = i;
                        gameParamUIPresenter.CenterUIViewer.SetCenterUIVisible(true);
                        GameState = State.Command;
                    }
                }

                break;
            case State.Command:
                gameParamUIPresenter.CenterUIViewer.SetCharacterActionButtons(characterParams[fastCharacterPos],
                  () =>
                  {
                      gameParamUIPresenter.WaitGaugeViewer.ResetWaitGaugeRate(fastCharacterPos);
                      GameState = State.CameraAction;
                  });

                break;
            case State.CameraAction:
                // カメラが動いてなかった場合は動かす
                if (!DollyCameraManager.IsMoving)
                {
                    DollyCameraManager.StartCameraAction(fastCharacterPos);
                    StartCoroutine(DollyCameraManager.WaitCameraEnd());
                }
                
                // カメラの動きが終わったらGameStateを進める
                if(DollyCameraManager.IsMoveEnd){ 
                    if(characterParams[fastCharacterPos].CharacterType == CharacterParam.GameCharacterType.Attacker){
                        Debug.Log("アタッカ＾");
                        DollyCameraManager.AttackerCameraRotate();
                        }
                    GameState = State.Action;
                }
                break;

            case State.Action:
                // キャラクターのアニメーションを待つ
                if (CharacterParamManagers[fastCharacterPos].CharacterAnimationController.AnimationEnd)
                {
                    // カメラを終了させて初期位置に戻す
                    DollyCameraManager.EndCameraAciton();
                    GameState = State.Start;
                    
                    // 行動側のMPの更新
                    gameParamUIPresenter.CharacterMpViewer.SetMp(fastCharacterPos,CharacterParamManagers[fastCharacterPos].CharacterMP);
                }
                break;
            case State.Result:

                Debug.Log("勝利");

                break;
        }

    }
}
