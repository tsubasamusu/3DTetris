using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private float currentFallSpeed;//ブロックの今の降下速度

    private GameObject mainCamera;//メインカメラゲームオブジェクト

    private BlockDataSO.BlockData myBlockData;//自分のブロックのデータ

    private bool isSideLimit;//ステージの端にいるかどうか

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
        if(CheckContactedDown())
        {
            //BlockManagerから適切な処理を呼び出す
            BlockManager.instance.StoppedCurrentBlock();

            //以降の処理を行わない
            return;
        }

        //ブロックの落下速度を設定
        currentFallSpeed = Input.GetKey(KeyCode.DownArrow) ? GameData.instance.SpecialFallSpeed : GameData.instance.NormalFallSpeed;

        //右矢印が押されたら
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            //ステージの端にいたら
            if(CheckContactedSide())
            {
                //カメラから見て右にいたら
                if (mainCamera.transform.position.z<0f&&transform.position.x>0f||mainCamera.transform.position.z>=0f&&transform.position.x<0f)
                {
                    //以降の処理を行わない
                    return;
                }
            }

            //カメラの位置に応じて移動方向を設定
            float moveValue = mainCamera.transform.position.z < 0f ? 1f : -1f;

            //カメラから見て右に移動する
            transform.Translate(new Vector3(moveValue, 0f, 0f));
        }
        //左矢印が押されたら
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //ステージの端にいたら
            if (CheckContactedSide())
            {
                //カメラから見て左にいたら
                if (mainCamera.transform.position.z < 0f && transform.position.x < 0f || mainCamera.transform.position.z >= 0f && transform.position.x > 0f)
                {
                    //以降の処理を行わない
                    return;
                }
            }

            //カメラの位置に応じて移動方向を設定
            float moveValue = mainCamera.transform.position.z < 0f ? -1f : 1f;

            //カメラから見て左に移動する
            transform.Translate(new Vector3(moveValue,0f,0f));
        }

        //上矢印が押されたら
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            //TODO:ホールド・使用する処理
        }

        //自身が回転できない座標にいたら
        if (Mathf.Abs(transform.position.x)>myBlockData.maxRotPosX)
        {
            //TODO:SoundManagerから「ブッブー」という効果音を鳴らす処理を呼び出す

            //以降の処理を行わない
            return;
        }

        //左クリックされたら
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //カメラの位置に応じて回転方向を設定
            float rotateValue = mainCamera.transform.position.z < 0 ? 90f : -90f;

            //カメラから見て反時計回りに回転させる
            transform.GetChild(0).transform.eulerAngles = new Vector3(0f,0f, transform.GetChild(0).transform.eulerAngles.z+rotateValue);
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
    /// 横方向の他のブロックに接触したかどうか調べる
    /// </summary>
    /// <returns>横方向の他のブロックに接触したらtrue</returns>
    private bool CheckContactedSide()
    {
        //TODO:各ブロックから横に光線を発射し、接触判定を取得する処理

        //（仮）
        return false;
    }

    /// <summary>
    /// 下方向の他のブロックに接触したかどうか調べる
    /// </summary>
    /// <returns>下方向の他のブロックに接触したらtrue</returns>
    private bool CheckContactedDown()
    {
        //TODO:各ブロックから下に光線を発射し、接触判定を取得する処理

        //（仮）
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
}
