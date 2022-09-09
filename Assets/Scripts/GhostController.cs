using System.Collections.Generic;//リストを使用
using UnityEngine;

public class GhostController : MonoBehaviour
{
    //MeshRendererのリスト
    List<MeshRenderer> meshRenderersList = new();

    /// <summary>
    /// 自身の生成直後に呼び出される
    /// </summary>
    private void Start()
    {
        //現在アクティブなブロックからのBlockControllerの取得に失敗したら
        if(!BlockManager.instance.CurrentBlock.TryGetComponent(out BlockController blockController))
        {
            //問題を報告
            Debug.Log("現在アクティブなブロックからのBlockControllerの取得に失敗");

            //以降の処理を行わない
            return;
        }

        //自身のブロックの情報を取得
        BlockDataSO.BlockData myBlockData=blockController.BlockData;

        //適切なy座標を取得
        float posY = myBlockData.isEvenWidth ? 25.5f : 25f;

        //自身を適切な降下位置に移動させる
        transform.position = new Vector3(transform.position.x, posY, 0f);
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        //下方向の他のブロックに触れたら
        if (CheckContactedDown())
        {
            //着地後の処理を行う
            LandingMe();

            //以降の処理を行わない
            return;
        }

        //消化が終わっていないなら
        if(!BlockManager.instance.EndDigestion)
        {
            //以降の処理を行わない
            return;
        }

        //ゴーストを落下させる
        transform.Translate(0f, -1f, 0f);
    }

    /// <summary>
    /// ゴースト自身の初期設定を行う
    /// </summary>
    /// <param name="meshRenderersList"></param>
    public void SetUpGhost(List<MeshRenderer> meshRenderersList)
    {
        //MeshRendererのリストを設定
        this.meshRenderersList = meshRenderersList;
    }

    /// <summary>
    /// 下方向の他のブロックに接触したかどうか調べる
    /// </summary>
    /// <returns>下方向の他のブロックに接触したらtrue</returns>
    private bool CheckContactedDown()
    {
        //4回繰り返す
        for (int i = 0; i < 4; i++)
        {
            //孫からの光線を作成
            Ray ray = new(transform.GetChild(0).transform.GetChild(i).transform.position, Vector3.down);

            //現在アクティブなブロック以外のコライダーに光線が接触したら
            if (Physics.Raycast(ray,out RaycastHit hit, 0.6f)&&hit.transform.root.gameObject!=BlockManager.instance.CurrentBlock)
            {
                //trueを返す
                return true;
            }
        }

        //falseを返す
        return false;
    }

    /// <summary>
    /// 着地後の処理を行う
    /// </summary>
    private void LandingMe()
    {
        //自身を適切な位置に移動させる
        SetMeRightPos();

        //MeshRendererのリストの要素数だけ繰り返す
        for (int i = 0; i < meshRenderersList.Count; i++)
        {
            //MeshRendererを活性化する
            meshRenderersList[i].enabled = true;
        }

        //自身のGhostControllerの取得に成功したら
        if (TryGetComponent(out GhostController ghostController))
        {
            //GhostControllerを非活性化する
            ghostController.enabled = false;
        }
        //自身のGhostControllerの取得に失敗したら
        else
        {
            //問題を報告
            Debug.Log("自身のGhostControllerの取得に失敗");
        }
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
}
