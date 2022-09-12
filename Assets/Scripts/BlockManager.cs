using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTweenを使用

public class BlockManager : MonoBehaviour
{
    public static BlockManager instance;//インスタンス

    [SerializeField]
    private BlockGenerator blockGenerator;//BlockGenerator

    private GameManager gameManager;//GameManager

    [SerializeField]
    private Material ghostMaterial;//ゴースト用のマテリアル

    [HideInInspector]
    public List<GameObject> cubeList = new();//現在、ステージ上に蓄積されている立方体のリスト

    private GameObject currentBlock;//現在アクティブなブロック

    private BlockDataSO.BlockData holdBlockData;//保存されたブロックのデータ

    private GameObject ghost;//ゴースト

    private bool endDigestion=true;//消化の処理が終わったかどうか

    /// <summary>
    /// 消化終了判定取得用
    /// </summary>
    public bool EndDigestion
    { get { return endDigestion; } }

    /// <summary>
    /// 保存されたブロックのデータの取得用
    /// </summary>
    public BlockDataSO.BlockData HoldBlockData
    { get { return holdBlockData; } }

    /// <summary>
    /// 現在アクティブなブロックの取得・設定用
    /// </summary>
    public GameObject CurrentBlock
    { get { return currentBlock; } set { currentBlock = value; } }

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
    /// BlockManagerの初期設定を行う
    /// </summary>
    /// <param name="gameManager">GameManager</param>
    public void SetUpBlockManager(GameManager gameManager)
    {
        //GameManagerを取得
        this.gameManager = gameManager;
    }

    /// <summary>
    /// 現在アクティブなブロックの動きが止まった時に呼び出される
    /// </summary>
    public void StoppedCurrentBlock()
    {
        //4回繰り返す
        for (int i = 0; i < 4; i++)
        {
            //現在アクティブなブロックの孫0号をリストに追加
            cubeList.Add(currentBlock.transform.GetChild(0).transform.GetChild(0).gameObject);

            //現在アクティブなブロックの孫0号の親を自身に設定
            currentBlock.transform.GetChild(0).transform.GetChild(0).transform.SetParent(transform);
        }

        //現在アクティブなブロックからBlockControllerを取得出来たら
        if (currentBlock.TryGetComponent(out BlockController blockController))
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

        //ステージに蓄積されている立方体の数だけ繰り返す
        for (int j = 0; j < cubeList.Count; j++)
        {
            //その立方体が限界ラインを超えていたら
            if (cubeList[j].transform.position.y > 20.5f)
            {
                //ゲームオーバー処理の準備を行う
                gameManager.PrepareGameOver();

                //以降の処理を行わない
                return;
            }
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
            if (samePosYList.Count < 10)
            {
                //次の繰り返し処理へ移る
                continue;
            }

            //消化が終わっていない状態に切り替える
            endDigestion = false;

            //10回繰り返す
            for (int j = 0; j < 10; j++)
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
                    cubeList[k].transform.DOMoveY(cubeList[k].transform.position.y - digestedCount, 0.5f).
                        
                        //アニメーションが終ったら、消化終了状態に切り替える
                        OnComplete(()=>endDigestion=true);
                }
            }
        }

        //1度でも消化が行われたなら
        if (digestedCount > 0)
        {
            //効果音を再生
            SoundManager.instance.PlaySound(SoundDataSO.SoundName.DigestionSE);

            //得点の表示を更新
            UIManager.instance.UpdateTxtScore(GameData.instance.ScorePerColumn * digestedCount);
        }
    }

    /// <summary>
    /// ブロックの保存・使用を行う
    /// </summary>
    /// <param name="blockData">呼び出し元のブロックのデータ</param>
    public void HoldBlock(BlockDataSO.BlockData blockData)
    {
        //現在アクティブなブロックを消す
        Destroy(currentBlock);

        //保存されているブロックがなければ
        if (holdBlockData == null)
        {
            //ブロックのデータを保存
            holdBlockData = blockData;

            //保存されているブロックの表示を設定
            UIManager.instance.SetImgHoldBllock(blockData.sprite);

            //ブロックを1度生成し、生成したブロックの情報を取得
            currentBlock = blockGenerator.GenerateBlock();
        }
        //保存されているブロックがあれば
        else
        {
            //保存されているブロックのイメージを空にする
            UIManager.instance.ClearImgHoldBlock();

            //保存されているブロックを生成する
            currentBlock = blockGenerator.GenerateBlock(holdBlockData);

            //保存されているブロックのデータを空にする
            holdBlockData = null;
        }
    }

    /// <summary>
    /// ブロックのゴーストを作成する
    /// </summary>
    public void MakeGhost()
    {
        //既にゴーストが存在しているなら
        if (ghost != null)
        {
            //そのゴーストを消す
            Destroy(ghost);
        }

        //MeshRendererのリスト
        List<MeshRenderer> meshRenderersList = new();

        ///ゴーストを生成
        ghost = Instantiate(CurrentBlock);

        //ゴーストからのBlockControllerの取得に成功したら
        if (ghost.TryGetComponent(out BlockController blockController))
        {
            //BlockControllerを非活性化する
            blockController.enabled = false;
        }
        //ゴーストからのBlockControllerの取得に失敗したら
        else
        {
            //問題を報告
            Debug.Log("ゴーストからのBlockControllerの取得に失敗");
        }

        //4回繰り返す
        for (int i = 0; i < 4; i++)
        {
            //ゴーストの孫からのコライダーの取得に成功したら
            if (ghost.transform.GetChild(0).transform.GetChild(i).gameObject.TryGetComponent(out BoxCollider collider))
            {
                //コライダーを非活性化する
                collider.enabled = false;
            }
            //ゴーストの孫からのコライダーの取得に失敗したら
            else
            {
                //問題を報告
                Debug.Log("ゴーストの孫からのコライダーの取得に失敗");
            }

            //ゴーストからのMeshRendererの取得に成功したら
            if (ghost.transform.GetChild(0).transform.GetChild(i).gameObject.TryGetComponent(out MeshRenderer meshRenderer))
            {
                //ゴーストのマテリアルを設定
                meshRenderer.material = ghostMaterial;

                //リストに追加
                meshRenderersList.Add(meshRenderer);

                //ゴーストを非表示にする
                meshRenderer.enabled = false;
            }
            //ゴーストからのMeshRendererの取得に失敗したら
            else
            {
                Debug.Log("ゴーストからのMeshRendererの取得に失敗");
            }
        }

        //生成したゴーストにGhostControllerを取り付ける
        ghost.AddComponent<GhostController>()
            
            //生成したゴーストの初期設定を行う
            .SetUpGhost(meshRenderersList);
    }
}
