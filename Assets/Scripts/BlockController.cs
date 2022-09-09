using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private float currentFallSpeed;//ブロックの今の降下速度

    private GameObject mainCamera;//メインカメラゲームオブジェクト

    private BlockDataSO.BlockData myBlockData;//自分のブロックのデータ

    /// <summary>
    /// 自身の生成開始直後に呼び出される
    /// </summary>
    private void Start()
    {
        //メインカメラゲームオブジェクトを取得
        mainCamera = GameObject.FindWithTag("MainCamera");
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        //下方向の他のブロックに触れたら
        if (CheckContactedDown())
        {
            //自身を適切な位置に移動させる
            SetMeRightPos();

            //BlockManagerから適切な処理を呼び出す
            BlockManager.instance.StoppedCurrentBlock();

            //以降の処理を行わない
            return;
        }

        //ブロックの落下速度を設定
        currentFallSpeed = Input.GetKey(KeyCode.DownArrow) ? GameData.instance.SpecialFallSpeed : GameData.instance.NormalFallSpeed;

        //右矢印が押されたら
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //カメラから見て右にいて、他のコライダーに触れていたら
            if ((mainCamera.transform.position.z < 0f && CheckContactedRight()) || (mainCamera.transform.position.z >= 0f && CheckContactedLeft()))
            {
                //以降の処理を行わない
                return;
            }

            //カメラの位置に応じて移動方向を設定
            float moveValue = mainCamera.transform.position.z < 0f ? 1f : -1f;

            //カメラから見て右に移動する
            transform.Translate(new Vector3(moveValue, 0f, 0f));
        }
        //左矢印が押されたら
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //カメラから見て左にいて、他のコライダーに触れていたら
            if ((mainCamera.transform.position.z < 0f && CheckContactedLeft()) || (mainCamera.transform.position.z >= 0f && CheckContactedRight()))
            {
                //以降の処理を行わない
                return;
            }

            //カメラの位置に応じて移動方向を設定
            float moveValue = mainCamera.transform.position.z < 0f ? -1f : 1f;

            //カメラから見て左に移動する
            transform.Translate(new Vector3(moveValue, 0f, 0f));
        }

        //上矢印が押されたら
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //ブロックの保存・使用を行う
            BlockManager.instance.HoldBlock(myBlockData);
        }

        //自身が回転できない座標にいたら
        if (Mathf.Abs(transform.position.x) > (5f - myBlockData.rotLength) || transform.position.y < (0.5f + myBlockData.rotLength) || !CheckLengthToOtherCube())
        {
            //プレイヤーがブロックを回転させようとしたら
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                //効果音を再生
                SoundManager.instance.PlaySound(SoundDataSO.SoundName.CannotRotSE);
            }

            //以降の処理を行わない
            return;
        }

        //左クリックされたら
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //カメラの位置に応じて回転方向を設定
            float rotateValue = mainCamera.transform.position.z < 0 ? 90f : -90f;

            //カメラから見て反時計回りに回転させる
            transform.GetChild(0).transform.eulerAngles = new Vector3(0f, 0f, transform.GetChild(0).transform.eulerAngles.z + rotateValue);
        }
        //右クリックされたら
        else if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            //カメラの位置に応じて回転方向を設定
            float rotateValue = mainCamera.transform.position.z < 0 ? -90f : 90f;

            //カメラから見て時計回りに回転させる
            transform.GetChild(0).transform.eulerAngles = new Vector3(0f, 0f, transform.GetChild(0).transform.eulerAngles.z + rotateValue);
        }
    }

    /// <summary>
    /// 一定時間ごとに呼び出される
    /// </summary>
    private void FixedUpdate()
    {
        //ブロックを落下させる
        transform.Translate(new Vector3(0f,-currentFallSpeed,0f));
    }

    /// <summary>
    /// 正面から見て右方向の他のブロックに接触したかどうか調べる
    /// </summary>
    /// <returns>正面から見て右方向の他のブロックに接触したらtrue</returns>
    private bool CheckContactedRight()
    {
        //自身の孫の数だけ繰り返す
        for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
        {
            //孫からの光線を作成
            Ray ray = new(transform.GetChild(0).transform.GetChild(i).transform.position, new Vector3(1f, 0f, 0f));

            //光線が他のコライダーに接触しなかったら
            if (!Physics.Raycast(ray,out RaycastHit hit,0.5f))
            {
                //次の繰り返し処理へ移る
                continue;
            }

            //触れた相手が孫ではなかった回数
            int isNotGrandchildCount = 0;

            //自身の孫の数だけ繰り返す
            for (int j = 0; j < transform.GetChild(0).transform.childCount; j++)
            {
                //触れた相手が自身の孫の1人なら
                if (hit.transform.gameObject == transform.GetChild(0).transform.GetChild(j).gameObject)
                {
                    //次の繰り返し処理へ移る
                    continue;
                }

                //回数を記録
                isNotGrandchildCount++;

                //触れた相手が自身の全ての孫以外なら
                if (isNotGrandchildCount == transform.GetChild(0).transform.childCount)
                {
                    //trueを返す
                    return true;
                }
            }
        }

        //falseを返す
        return false;
    }

    /// <summary>
    /// 正面から見て左方向の他のブロックに接触したかどうか調べる
    /// </summary>
    /// <returns>正面から見て左方向の他のブロックに接触したらtrue</returns>
    private bool CheckContactedLeft()
    {
        //自身の孫の数だけ繰り返す
        for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
        {
            //孫からの光線を作成
            Ray ray = new(transform.GetChild(0).transform.GetChild(i).transform.position, new Vector3(-1f, 0f, 0f));

            //光線が他のコライダーに接触しなかったら
            if (!Physics.Raycast(ray, out RaycastHit hit, 0.5f))
            {
                //次の繰り返し処理へ移る
                continue;
            }

            //触れた相手が孫ではなかった回数
            int isNotGrandchildCount = 0;

            //自身の孫の数だけ繰り返す
            for (int j = 0; j < transform.GetChild(0).transform.childCount; j++)
            {
                //触れた相手が自身の孫の1人なら
                if (hit.transform.gameObject == transform.GetChild(0).transform.GetChild(j).gameObject)
                {
                    //次の繰り返し処理へ移る
                    continue;
                }

                //回数を記録
                isNotGrandchildCount++;

                //触れた相手が自身の全ての孫以外なら
                if (isNotGrandchildCount == transform.GetChild(0).transform.childCount)
                {
                    //trueを返す
                    return true;
                }
            }
        }

        //falseを返す
        return false;
    }

    /// <summary>
    /// 下方向の他のブロックに接触したかどうか調べる
    /// </summary>
    /// <returns>下方向の他のブロックに接触したらtrue</returns>
    private bool CheckContactedDown()
    {
        //自身の孫の数だけ繰り返す
        for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
        {
            //孫からの光線を作成
            Ray ray = new(transform.GetChild(0).transform.GetChild(i).transform.position, Vector3.down);

            //光線が他のコライダーに接触しなかったら
            if (!Physics.Raycast(ray, out RaycastHit hit, 0.5f))
            {
                //次の繰り返し処理へ移る
                continue;
            }

            //触れた相手が孫ではなかった回数
            int isNotGrandchildCount = 0;

            //自身の孫の数だけ繰り返す
            for (int j = 0; j < transform.GetChild(0).transform.childCount; j++)
            {
                //触れた相手が自身の孫の1人なら
                if (hit.transform.gameObject == transform.GetChild(0).transform.GetChild(j).gameObject)
                {
                    //次の繰り返し処理へ移る
                    continue;
                }

                //回数を記録
                isNotGrandchildCount++;

                //触れた相手が自身の全ての孫以外なら
                if (isNotGrandchildCount == transform.GetChild(0).transform.childCount)
                {
                    //trueを返す
                    return true;
                }
            }
        }

        //falseを返す
        return false;
    }

    /// <summary>
    /// 自分のブロックの初期設定を行う
    /// </summary>
    /// <param name="yourBlockData">対象のブロックのデータ</param>
    public void SetUpBlock(BlockDataSO.BlockData yourBlockData)
    {
        //自分のブロックのデータを取得
        myBlockData = yourBlockData;
    }

    /// <summary>
    /// 着地後に自身を適切な位置に移動させる
    /// </summary>
    private void SetMeRightPos()
    {
        //自身のy座標の小数部分（誤差）を取得
        float excess = transform.position.y % 0.5f;

        //誤差を修正するための値を取得
        float valueY = excess < 0.25 ? -excess : 0.5f - excess;

        //座標を再設定
        transform.position = new Vector3(transform.position.x, transform.position.y + valueY, 0f);
    }

    /// <summary>
    /// 他の立方体との距離を確認する
    /// </summary>
    /// <returns>他の立方体との距離を確認したうえで回転可能ならtrue</returns>
    private bool CheckLengthToOtherCube()
    {
        //現在ステージに蓄積されている立方体の数だけ繰り返す
        for(int i = 0; i < BlockManager.instance.cubeList.Count; i++)
        {
            //他の立方体とのx方向の距離が近すぎるかつ、
            if(Mathf.Abs(BlockManager.instance.cubeList[i].transform.position.x-transform.position.x)<(myBlockData.rotLength+1)&&
                //他の立方体とのy方向の距離が近すぎたら
                Mathf.Abs(BlockManager.instance.cubeList[i].transform.position.y - transform.position.y)< (myBlockData.rotLength+1))
            {
                //falseを返す
                return false;
            }
        }

        //trueを返す
        return true;
    }
}
