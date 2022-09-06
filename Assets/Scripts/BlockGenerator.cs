using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField]
    private BlockDataSO blockDataSO;//BlockDataSO

    [HideInInspector]
    public BlockDataSO.BlockData[] nextBlockDatas;//生成予定のブロックのデータの配列

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    private void Start()
    {
        //生成予定のブロックのデータの配列の要素数を設定
        nextBlockDatas = new BlockDataSO.BlockData[GameData.instance.AppointmentsNumber];

        //生成予定のブロックのデータの配列の要素数だけ繰り返す
        for (int i = 0; i < nextBlockDatas.Length; i++)
        {
            //生成予定のブロックのデータの配列の各要素にランダムなデータを入れる
            nextBlockDatas[i] = blockDataSO.blockDataList[Random.Range(0, blockDataSO.blockDataList.Count)];
        }
    }

    /// <summary>
    /// ブロックを生成する
    /// </summary>
    /// <returns>生成したブロックのデータ</returns>
    public BlockDataSO.BlockData GenerateBlock()
    {
        //ブロックを生成
        GameObject generatedBlock= Instantiate(nextBlockDatas[0].prefab);

        //生成座標のx成分
        float x = 
            //生成するブロックの名前に応じて値を変更
            nextBlockDatas[0].name == BlockDataSO.BlockName.T || nextBlockDatas[0].name == BlockDataSO.BlockName.S || nextBlockDatas[0].name == BlockDataSO.BlockName.Z ? 
            //0.5か0を入れる
            0.5f : 0f;

        //生成したブロックの位置を設定
        generatedBlock.transform.position = new Vector3(x, 25f, 0f);

        //生成予定のブロックのデータを更新する
        UpdateNextBlockDatas();

        //生成したブロックのデータを返す
        return blockDataSO.blockDataList.Find(x => x.prefab == generatedBlock);
    }

    /// <summary>
    /// 生成予定のブロックのデータを更新する
    /// </summary>
    private void UpdateNextBlockDatas()
    {
        //生成予定のブロックのデータの配列の要素数だけ繰り返す
        for (int i = 0; i < nextBlockDatas.Length; i++)
        {
            //最後の繰り返し処理になったら
            if (i == (nextBlockDatas.Length - 1))
            {
                //生成予定のブロックのデータの配列の最後の要素にランダムなデータを入れる
                nextBlockDatas[i] = blockDataSO.blockDataList[Random.Range(0, blockDataSO.blockDataList.Count)];

                //繰り返し処理を終了する
                break;
            }

            //データを1つ手前にずらす
            nextBlockDatas[i] = nextBlockDatas[i + 1];
        }
    }
}
