using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static BlockManager instance;//インスタンス

    [SerializeField]
    private BlockGenerator blockGenerator;//BlockGenerator

    [HideInInspector]
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
        //現在アクティブなブロックの孫の数だけ繰り返す
        for(int i = 0; i < currentBlock.transform.GetChild(0).transform.childCount; i++)
        {
            //現在アクティブなブロックの全ての孫をリストに追加
            cubeList.Add(currentBlock.transform.GetChild(0).transform.GetChild(i).gameObject);
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

        //TODO:ブロックの消化の確認

        //ブロックを1度生成し、生成したブロックの情報を取得
        currentBlock = blockGenerator.GenerateBlock();
    }
}
