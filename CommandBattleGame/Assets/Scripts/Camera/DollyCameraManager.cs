using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DollyCameraManager : MonoBehaviour
{
    public GameMainManager GameMainManager;
    public CinemachineDollyCart CinemachineDollyCart;
    public CinemachineVirtualCamera CinemachineVirtualCamera;
    public CinemachineSmoothPath[] playerSmoothPaths = new CinemachineSmoothPath[3];
    public float CameraSpeed = 10f;

    public bool IsMoving = false;
    public bool IsMoveEnd = false;

    private Vector3 currentCameraLocalEulerAngles;

    private void Awake()
    {
        currentCameraLocalEulerAngles = CinemachineVirtualCamera.transform.localEulerAngles;
    }

    public void StartCameraAction(int characterPos)
    {
        IsMoving = true;
        IsMoveEnd = false;
        CinemachineDollyCart.m_Speed = CameraSpeed;
        CinemachineDollyCart.m_Path = playerSmoothPaths[characterPos];
    }

    // カメラの終了時
    public void EndCameraAciton()
    {
        // カメラの角度が最初に取得した角度じゃない場合は戻す。
        if (CinemachineVirtualCamera.transform.localEulerAngles != currentCameraLocalEulerAngles)
        {
            CinemachineVirtualCamera.transform.localEulerAngles = currentCameraLocalEulerAngles;
        }

        CinemachineDollyCart.m_Speed = 0f;
        CinemachineDollyCart.m_Position = 0f;
        IsMoving = false;
    }

    //  カメラの移動が終わるまで待つ
    public IEnumerator WaitCameraEnd()
    {
        yield return new WaitWhile(() => CinemachineDollyCart.m_Position != CinemachineDollyCart.m_Path.PathLength);
        yield return new WaitUntil(() => CinemachineDollyCart.m_Position == CinemachineDollyCart.m_Path.PathLength);
        IsMoveEnd = true;
    }

    // Attackerの場合、敵に向かっていくのでカメラを反転させる
    public void AttackerCameraRotate()
    {
        var attackerCameraRotate =  CinemachineVirtualCamera.transform.localEulerAngles;
        attackerCameraRotate.y = -180f;
        CinemachineVirtualCamera.transform.localEulerAngles = attackerCameraRotate;
    }

}
