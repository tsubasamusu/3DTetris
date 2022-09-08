using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;//リストを使用
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
    private Text txtGameStart;//ゲームスタートテキスト

    [SerializeField]
    private Button btnGameStart;//ゲームスタートボタン

    [SerializeField]
    private Image[] imgNextBlocks;//次のブロックの配列

    [SerializeField]
    private List<LogoData> logoDatasList=new();//ロゴのデータのリスト

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

    //（仮）
    private IEnumerator Start()
    {
        yield return PlayGameStart();

        Debug.Log("押された");
    }

    /// <summary>
    /// ゲームスタート演出を行う
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator PlayGameStart()
    {
        //ゲームスタートボタン判定用
        bool clicked=false;

        //ゲームスタートボタンを非活性化
        btnGameStart.interactable = false;

        //背景を白色に設定
        imgBackGround.color = Color.white;

        //背景を表示
        imgBackGround.DOFade(1f, 0f);

        //ロゴをタイトルに設定
        imgLogo.sprite = GetLogoSprite(LogoType.Title);

        //ゲームスタートボタンからのイメージの取得に成功したら
        if(btnGameStart.TryGetComponent(out Image imgGameStart))
        {
            //ゲームスタートボタンを表示
            imgGameStart.DOFade(1f, 1f);
        }
        //ゲームスタートボタンからのイメージの取得に失敗したら
        else
        {
            //問題を報告
            Debug.Log("ゲームスタートボタンからのイメージの取得に失敗");
        }

        //ロゴを表示する
        imgLogo.DOFade(1f, 1f);

        //ゲームスタートボタンのテキストを表示
        txtGameStart.DOText("Game Start",1f).OnComplete(()=>

        //ゲームスタートボタンを活性化
        btnGameStart.interactable = true);

        //ゲームスタートボタンが押された際の処理を登録
        btnGameStart.onClick.AddListener(()=>ClickedBtnGameStart());

        //ゲームスタートボタンが押されるまで待つ
        yield return new WaitUntil(()=>clicked==true);

        //ゲームスタートボタンが押された際の処理
        void ClickedBtnGameStart()
        {
            //ゲームスタートボタンが押された状態に切り替える
            clicked=true;
        }
    }

    /// <summary>
    /// 指定したロゴのスプライトを取得する
    /// </summary>
    /// <param name="logoType">ロゴの種類</param>
    /// <returns>指定したロゴのスプライト</returns>
    private Sprite GetLogoSprite(LogoType logoType)
    {
        //指定されているロゴのスプライトを返す
        return logoDatasList.Find(x => x.logoType == logoType).sprLogo;
    }
}
