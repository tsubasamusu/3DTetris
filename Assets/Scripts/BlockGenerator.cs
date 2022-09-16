using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField]
    private BlockDataSO blockDataSO;//BlockDataSO

    [HideInInspector]
    public BlockDataSO.BlockData[] nextBlockDatas;//生成予定のブロックのデータの配列

    private bool stop;//ブロックの生成停止判定用

    private bool setUp;//初期設定が完了したかどうか

    /// <summary>
    /// BlockGeneratorの初期設定を行う
    /// </summary>
    public void SetUpBlockGenerator()
    {
        //生成予定のブロックのデータの配列の要素数を設定
        nextBlockDatas = new BlockDataSO.BlockData[GameData.instance.AppointmentsNumber];

        //生成予定のブロックのデータの配列の要素数だけ繰り返す
        for (int i = 0; i < nextBlockDatas.Length; i++)
        {
            //生成予定のブロックのデータの配列の各要素にランダムなデータを入れる
            nextBlockDatas[i] = blockDataSO.blockDataList[Random.Range(0, blockDataSO.blockDataList.Count)];
        }

        //初期設定完了状態に切り替える
        setUp = true;
    }

    /// <summary>
    /// ブロックを生成する
    /// </summary>
    /// <param name="blockData">生成したいブロックのデータ</param>
    /// <returns>生成したブロック</returns>
    public GameObject GenerateBlock(BlockDataSO.BlockData blockData = null)
    {
        //初期設定が完了していないなら
        if(!setUp)
        {
            //以降の処理を行わない
            return null;
        }

        //ブロックの生成停止命令が出ていたら
        if(stop)
        {
            //以降の処理を行わない
            return null;
        }

        //生成するブロックの元のデータを設定
        BlockDataSO.BlockData originalData = blockData == null ? nextBlockDatas[0] : blockData;

        //ブロックを生成
        GameObject generatedBlock = Instantiate(originalData.prefab);

        //生成座標のx成分を設定
        float x = originalData.isEvenWidth ? 0f : 0.5f;

        //生成したブロックの位置を設定
        generatedBlock.transform.position = new Vector3(x, 25f, 0f);

        //生成したブロックからBlockDetailを取得出来たら
        if (generatedBlock.TryGetComponent(out BlockController blockController))
        {
            //生成したブロックの初期設定を行う
            blockController.SetUpBlockController(originalData);
        }
        //取得に失敗したら
        else
        {
            //問題を報告
            Debug.Log("生成したブロックからのBlockControllerの取得に失敗");
        }

        //生成するブロックが指定されていたら
        if(blockData != null)
        {
            //以降の処理を行わない
            return generatedBlock;
        }

        //生成予定のブロックのデータを更新する
        UpdateNextBlockDatas();

        //生成予定のブロックの表示を設定
        UIManager.instance.SetImgNextBlocks(nextBlockDatas);

        //生成したブロックを返す
        return generatedBlock;
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

    /// <summary>
    /// ブロックの生成を止める
    /// </summary>
    public void StopGenerateBlock()
    {
        //ブロックの生成を停止する
        stop = true;
    }
}
