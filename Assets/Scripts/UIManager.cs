using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UIを使用
using DG.Tweening;//DOTweenを使用
using System;//Serializable属性を使用

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// ロゴの種類
    /// </summary>
    public enum LogoType
    {
        Title,GameClear,GameOver//列挙子
    }

    /// <summary>
    /// ロゴのデータの管理用
    /// </summary>
    [Serializable]
    public class LogoData
    {
        public LogoType logoType;//ロゴの種類
        public Sprite sprLogo;//スプライト
    }

    public static UIManager instance;//インスタンス

    [SerializeField]
    private Image imgBackGround;//背景

    [SerializeField]
    private Image imgLogo;//ロゴ

    [SerializeField]
    private Image imgHold;//保存されたブロック

    [SerializeField]
    private Text txtScore;//得点

    [SerializeField]
    private Text txtTimeLimit;//制限時間

    [SerializeField]
    private Image[] imgNextBlocks;//次のブロックの配列

    [SerializeField]
    private LogoData[] logoDatas;//ロゴのデータの配列

    /// <summary>
    /// Startメソッドより前に呼び出される
    /// </summary>
    private void Awake()
    {
        //以下、シングルトンに必須の記述
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
    /// ゲームスタート演出を行う
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator PlayGameStart()
    {
        //（仮）
        yield return null;
    }
}
