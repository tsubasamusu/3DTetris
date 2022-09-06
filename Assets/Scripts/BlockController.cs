using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private float currentFallSpeed;//ブロックの今の降下速度

    private GameObject mainCamera;//メインカメラゲームオブジェクト

    /// <summary>
    /// ゲーム開始直後に呼び出される
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
        //ブロックの落下速度を設定
        currentFallSpeed = Input.GetKey(KeyCode.DownArrow) ? GameData.instance.SpecialFallSpeed : GameData.instance.NormalFallSpeed;

        //右矢印が押されたら
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            //カメラの位置に応じて移動方向を設定
            float moveValue = mainCamera.transform.position.z < 0 ? 1f : -1f;

            //カメラから見て右に移動する
            transform.Translate(new Vector3(moveValue, 0f, 0f));
        }
        //左矢印が押されたら
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //カメラの位置に応じて移動方向を設定
            float moveValue = mainCamera.transform.position.z < 0 ? -1f : 1f;

            //カメラから見て左に移動する
            transform.Translate(new Vector3(moveValue,0f,0f));
        }

        //上矢印が押されたら
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            //TODO:ホールド・使用する処理
        }

        //左クリックされたら
        if(Input.GetKeyDown(KeyCode.Mouse0))
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
}
