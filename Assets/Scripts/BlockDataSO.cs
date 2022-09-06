using System.Collections;
using System.Collections.Generic;//リストを使用
using UnityEngine;
using System;//Serializable属性を使用

//アセットメニューで「Create BlockDataSO」を選択すると「BlockDataSO」を作成できる
[CreateAssetMenu(fileName = "BlockDataSO", menuName = "Create BlockDataSO")]
public class BlockDataSO : ScriptableObject
{
    /// <summary> 
    /// ブロックの名前
    /// </summary> 
    public enum BlockName
    {
        I,
        J,
        L,
        O,
        S,
        T,
        Z
    }

    /// <summary> 
    ///ブロックのデータを管理する
    /// </summary> 
    [Serializable]
    public class BlockData
    {　　　　　　　　　
        public BlockName name;//名前
        public GameObject prefab;//プレファブ
        public Sprite sprite;//スプライト
        [Header("オブジェクトの幅が奇数かどうか")]
        public bool isOddWidth;//オブジェクトの幅が奇数かどうか
        public float maxRotPosX;//回転可能なxの最大値
        public float minRotPosX;//回転可能なxの最小値
    }

    public List<BlockData> blockDataList = new List<BlockData>();//ブロックのデータのリスト
}