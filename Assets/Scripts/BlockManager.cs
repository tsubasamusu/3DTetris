using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField]
    private BlockGenerator blockGenerator;//BlockGenerator

    private BlockDataSO.BlockData currentBlockData;//現在プレーヤーが操作しているブロックのデータ

    /// <summary>
    /// ブロックの管理を開始する
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator StartBlockManagement()
    {
        //ブロックを1度生成し、生成したブロックの情報を取得
        currentBlockData= blockGenerator.GenerateBlock();

        //無限に繰り返す
        while (true)
        {
            //次のフレームへ飛ばす（実質、Updateメソッド）
            yield return null;
        }
    }
}
