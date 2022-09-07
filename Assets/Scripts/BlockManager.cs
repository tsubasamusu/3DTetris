using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;

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
        //20回繰り返す
        for(int i = 1; i < 21; i++)
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
                //対象の立方体を一定時間後に消す
                Destroy(cubeList.FindAll(x => (x.transform.position.y > (i - 0.5f)) && (x.transform.position.y < (i + 0.5f)))[j],0.25f);

                //対象の立方体をリストから取り除く
                cubeList.Remove(cubeList.Find(x => (x.transform.position.y > (i - 0.5f)) && (x.transform.position.y < (i + 0.5f))));
            }
        }
    }
}
