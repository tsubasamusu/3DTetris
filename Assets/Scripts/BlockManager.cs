using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTweenを使用

public class BlockManager : MonoBehaviour
{
    public static BlockManager instance;//インスタンス

    [SerializeField]
    private BlockGenerator blockGenerator;//BlockGenerator

    //[HideInInspector]
    public List<GameObject> cubeList=new List<GameObject>();//現在、ステージ上に蓄積されている立方体のリスト

    private GameObject currentBlock;//現在アクティブなブロック

    /// <summary>
    /// 現在アクティブなブロックの設定用
    /// </summary>
    public GameObject CurrentBlock
    { set { currentBlock = value; } }

    /// <summary>
    /// Startメソッドより前に呼び出される
    /// </summary>
    private void Awake()
    {
        //以降、シングルトンに必須の記述
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 現在アクティブなブロックの動きが止まった時に呼び出される
    /// </summary>
    public void StoppedCurrentBlock()
    {
        //4回繰り返す
        for(int i = 0; i < 4; i++)
        {
            //現在アクティブなブロックの孫0号をリストに追加
            cubeList.Add(currentBlock.transform.GetChild(0).transform.GetChild(0).gameObject);

            //現在アクティブなブロックの孫0号の親を自身に設定
            currentBlock.transform.GetChild(0).transform.GetChild(0).transform.SetParent(transform);
        }

        //現在アクティブなブロックからBlockControllerを取得出来たら
        if(currentBlock.TryGetComponent(out BlockController blockController))
        {
            //BlockControllerを非活性化（無駄な処理を防ぐ）
            blockController.enabled = false;
        }
        //現在アクティブなブロックからBlockControllerを取得出来なかったら
        else
        {
            //問題を報告
            Debug.Log("現在アクティブなブロックからのBlockControllerの取得に失敗");
        }

        //立方体の消化を行うか確認する
        CheckDigested();

        //ブロックを1度生成し、生成したブロックの情報を取得
        currentBlock = blockGenerator.GenerateBlock();
    }

    /// <summary>
    /// 立方体の消化を行うかどうか確認し、場合に応じて消化を行う
    /// </summary>
    private void CheckDigested()
    {
        //消化した回数
        int digestedCount = 0;

        //20回繰り返す
        for (int i = 1; i < 21; i++)
        {
            //同じy座標の立方体のリストを作成
            List<GameObject> samePosYList = cubeList.FindAll(x => (x.transform.position.y > (i - 0.5f)) && (x.transform.position.y < (i + 0.5f)));
            
            //同じy座標の立方体の数が10より小さかったら（横一列が揃っていなかったら）
            if(samePosYList.Count< 10)
            {
                //次の繰り返し処理へ移る
                continue;
            }

            //10回繰り返す
            for(int j = 0; j < 10; j++)
            {
                //対象の立方体を取得
                GameObject cube = samePosYList[0];

                //対象の立方体をcubeListから取り除く
                cubeList.Remove(cubeList.Find(x => x == cube));

                //対象の立方体をsamePosYListから取り除く
                samePosYList.RemoveAt(0);

                //対象の立方体を消す
                Destroy(cube);
            }

            //消化した回数を記録
            digestedCount++;

            //ステージに蓄積されている立方体の数だけ繰り返す
            for (int k = 0; k < cubeList.Count; k++)
            {
                //消化された列より上にある立方体なら
                if (cubeList[k].transform.position.y > i)
                {
                    //消化した回数だけ落下させる
                    cubeList[k].transform.DOMoveY(cubeList[k].transform.position.y - digestedCount, 0.5f);
                }
            }
        }
    }
}
