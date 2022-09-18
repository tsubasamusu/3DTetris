using System.Collections.Generic;//リストを使用
using System.Collections;//IEnumeratorを使用
using UnityEngine;

public class GhostController : MonoBehaviour
{
    //MeshRendererのリスト
    List<MeshRenderer> meshRenderersList = new();

    /// <summary>
    /// ゴースト自身の初期設定を行う
    /// </summary>
    /// <param name="meshRenderersList">自身のMeshRendererのリスト</param>
    /// <returns>待ち時間</returns>
    public IEnumerator SetUpGhost(List<MeshRenderer> meshRenderersList)
    {
        //MeshRendererのリストを設定
        this.meshRenderersList = meshRenderersList;

        //自身のブロックの情報を取得
        BlockDataSO.BlockData myBlockData = BlockManager.instance.CurrentBlock.BlockData;

        //適切なy座標を取得
        float posY = myBlockData.isEvenWidth ? 25.5f : 25f;

        //自身を適切な降下位置に移動させる
        transform.position = new Vector3(transform.position.x, posY, 0f);

        //ブロックの消化が終わるまで待つ
        yield return new WaitUntil(() => BlockManager.instance.EndDigestion);

        //無限に繰り返す
        while(true)
        {
            //下方向のブロックに触れるか、ブロックをすり抜けて最下層に行ってしまったら
            if(CheckContactedDown()||transform.position.y<0f)
            {
                //繰り返し処理から抜け出す
                break;
            }

            //ゴーストを落下させる
            transform.Translate(0f, -1f, 0f);
        }

        //着地後の処理を行う
        LandingMe();
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
            if (Physics.Raycast(ray,out RaycastHit hit, 0.6f)&&!hit.transform.root.gameObject.TryGetComponent(out BlockController _))
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

        //GhostControllerを消す
        Destroy(this);
    }

    /// <summary>
    /// 着地後に自身を適切な位置に移動させる
    /// </summary>
    private void SetMeRightPos()
    {
        //ゴーストが埋もれているか調べて、正常な位置に移動させる
        CheckBuried();

        //現在アクティブなブロックがなかったら（nullエラー回避）
        if(BlockManager.instance.CurrentBlock==null)
        {
            //以降の処理を行わない
            return;
        }

        //プレーヤーが操作しているブロックと横方向でズレていたら
        if (Mathf.Abs(transform.position.x-BlockManager.instance.CurrentBlock.transform.position.x)>0.5f)
        {
            //座標を再設定
            transform.position = new Vector3(BlockManager.instance.CurrentBlock.transform.position.x, transform.position.y, 0f);

            //無限に繰り返す
            while (true)
            {
                //下方向のブロックに触れるか、ブロックをすり抜けて最下層に行ってしまったら
                if (CheckContactedDown() || transform.position.y < 0f)
                {
                    //繰り返し処理から抜け出す
                    break;
                }

                //ゴーストを落下させる
                transform.Translate(0f, -1f, 0f);
            }

            //ゴーストが埋もれているか調べて、正常な位置に移動させる
            CheckBuried();
        }

        //自身のy座標の小数部分（誤差）を取得
        float excess = transform.position.y % 0.5f;

        //誤差を修正するための値を取得
        float valueY = excess < 0.25 ? -excess : 0.5f - excess;

        //座標を再設定
        transform.position = new Vector3(transform.position.x, transform.position.y + valueY, 0f);
    }

    /// <summary>
    /// ゴーストが埋もれているか調べて、正常な位置に移動させる
    /// </summary>
    private void CheckBuried()
    {
        //4回（孫の数）繰り返す
        for (int i = 0; i < 4; i++)
        {
            //ステージの下枠に埋まっていたら
            if (Mathf.Abs(transform.GetChild(0).transform.GetChild(i).transform.position.y - 0f) < 0.5f)
            {
                //ゴーストを1つ上に移動させる
                transform.Translate(0f, 1f, 0f);
            }

            //ステージ上に蓄積されているブロックの数だけ繰り返す
            for (int j = 0; j < BlockManager.instance.cubeList.Count; j++)
            {
                //他のブロックに埋まっていたら
                if (Mathf.Abs(transform.GetChild(0).transform.GetChild(i).transform.position.y - BlockManager.instance.cubeList[j].transform.position.y) < 0.5f
                    && Mathf.Abs(transform.GetChild(0).transform.GetChild(i).transform.position.x - BlockManager.instance.cubeList[j].transform.position.x) < 0.5)
                {
                    //ゴーストを1つ上に移動させる
                    transform.Translate(0f, 1f, 0f);
                }
            }
        }
    }
}
