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
    private Image imgButton;//ボタンのイメージ

    [SerializeField]
    private Text txtScore;//得点

    [SerializeField]
    private Text txtTimeLimit;//制限時間

    [SerializeField]
    private Text txtButton;//ボタンのテキスト

    [SerializeField]
    private Button button;//ボタン

    [SerializeField]
    private Transform resultTran;//結果表示位置

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
        yield return PlayGameClear();

        Debug.Log("END");
    }

    /// <summary>
    /// ゲームスタート演出を行う
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator PlayGameStart()
    {
        //演出終了判定用
        bool end = false;

        //ボタン判定用
        bool clicked = false;

        //ボタンを非活性化
        button.interactable = false;

        //背景を白色に設定
        imgBackGround.color = Color.white;

        //ボタンを青色に設定
        imgButton.color = Color.blue;

        //背景を表示
        imgBackGround.DOFade(1f, 0f);

        //ロゴをタイトルに設定
        imgLogo.sprite = GetLogoSprite(LogoType.Title);

        //ボタンを表示
        imgButton.DOFade(1f, 1f);

        //ロゴを表示する
        imgLogo.DOFade(1f, 1f);

        //ボタンのテキストを設定し表示
        txtButton.DOText("Game Start", 1f).OnComplete(() =>

        //ボタンを活性化
        button.interactable = true);

        //ボタンが押された際の処理を登録
        button.onClick.AddListener(() => ClickedButton());

        //ボタンが押されるまで待つ
        yield return new WaitUntil(() => clicked == true);

        //背景を非表示にする
        imgBackGround.DOFade(0f, 1f);

        //ロゴを非表示にする
        imgLogo.DOFade(0f, 1f);

        //ボタンのイメージを非表示にする
        imgButton.DOFade(0f, 1f);

        //ボタンのテキストを非表示にする
        txtButton.DOFade(0f, 1f).OnComplete(() =>

        //演出が終わった状態に切り替える
        end = true);

        //演出が終わるまで待つ
        yield return new WaitUntil(() => end == true);

        //ボタンが押された際の処理
        void ClickedButton()
        {
            //ボタンが押された状態に切り替える
            clicked = true;

            //ボタンを非活性化
            button.interactable = false;
        }
    }

    /// <summary>
    /// ゲームオーバー演出を行う
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator PlayGameOver()
    {
        //演出終了判定用
        bool end = false;

        //ボタン判定用
        bool clicked = false;

        //背景を黒色に設定
        imgBackGround.color = Color.black;

        //ロゴを「GameOver」に設定
        imgLogo.sprite = GetLogoSprite(LogoType.GameOver);

        ///ボタンの色を赤色に設定
        imgButton.color = Color.red;

        ///ボタンが押された際の処理を登録
        button.onClick.AddListener(() => ClickedButton());

        //背景を表示
        imgBackGround.DOFade(1f, 1f);

        //ロゴを表示
        imgLogo.DOFade(1f, 1f);

        //ボタンを表示
        imgButton.DOFade(1f, 1f);

        //ボタンのテキストを設定し、表示
        txtButton.DOText("Restart", 1f).OnComplete(() =>

        //ボタンを活性化
        button.interactable = true);

        //ボタンが押されるまで待つ
        yield return new WaitUntil(() => clicked == true);

        //背景を白色に変更
        imgBackGround.DOColor(Color.white, 1f);

        //ロゴを非表示にする
        imgLogo.DOFade(0f, 1f);

        //ボタンのイメージを非表示にする
        imgButton.DOFade(0f, 1f);

        //ボタンのテキストを非表示にする
        txtButton.DOFade(0f, 1f).OnComplete(() =>

        //演出が終わった状態に切り替える
        end = true);

        //演出が終わるまで待つ
        yield return new WaitUntil(() => end == true);

        //ボタンが押された際の処理
        void ClickedButton()
        {
            //ボタンが押された状態に切り替える
            clicked = true;
        }
    }

    /// <summary>
    /// ゲームクリア演出を行う
    /// </summary>
    /// <returns>待ち時間</returns>
    public IEnumerator PlayGameClear()
    {
        //演出終了判定用
        bool end = false;

        //ボタン判定用
        bool clicked = false;

        //背景を白色に設定
        imgBackGround.color = Color.white;

        //ロゴを「GameClear」に設定
        imgLogo.sprite = GetLogoSprite(LogoType.GameClear);

        ///ボタンの色を黄色に設定
        imgButton.color = Color.yellow;

        ///ボタンが押された際の処理を登録
        button.onClick.AddListener(() => ClickedButton());

        //背景を表示
        imgBackGround.DOFade(1f, 1f);

        //ロゴを表示
        imgLogo.DOFade(1f, 1f);

        //ボタンを表示
        imgButton.DOFade(1f, 1f);

        //得点を結果表示位置に移動させる
        txtScore.transform.DOMove(resultTran.position, 1f);

        //ボタンのテキストを設定し、表示
        txtButton.DOText("Restart", 1f).OnComplete(() =>

        //ボタンを活性化
        button.interactable = true);

        //ボタンが押されるまで待つ
        yield return new WaitUntil(() => clicked == true);

        //ロゴを非表示にする
        imgLogo.DOFade(0f, 1f);

        //ボタンのイメージを非表示にする
        imgButton.DOFade(0f, 1f);

        //得点を非表示にする
        txtScore.DOFade(0f, 1f);

        //ボタンのテキストを非表示にする
        txtButton.DOFade(0f, 1f).OnComplete(() =>

        //演出が終わった状態に切り替える
        end = true);

        //演出が終わるまで待つ
        yield return new WaitUntil(() => end == true);

        //ボタンが押された際の処理
        void ClickedButton()
        {
            //ボタンが押された状態に切り替える
            clicked = true;
        }
    }

    /// <summary>
    /// 得点の表示を設定する
    /// </summary>
    /// <param name="score">得点</param>
    public void SetTxtScore(int score)
    {
        //得点のテキストを設定
        txtScore.text = score.ToString();
    }

    /// <summary>
    /// 制限時間の表示を設定する
    /// </summary>
    /// <param name="remainingTime">残り時間</param>
    public void SetTxtTimeLimit(float remainingTime)
    {
        //制限時間の表示を残り時間に設定
        txtTimeLimit.text = remainingTime.ToString();
    }

    /// <summary>
    /// 保存されているブロックの表示を設定する
    /// </summary>
    /// <param name="blockSprite">ブロックのスプライト</param>
    public void SetImgHoldBllock(Sprite blockSprite)
    {
        //保存されているブロックのスプライトを設定
        imgHold.sprite = blockSprite;
    }

    /// <summary>
    /// 生成予定のブロックの表示を設定する
    /// </summary>
    /// <param name="blockDatas">生成予定のブロックのデータのリスト</param>
    public void SetImgNextBlocks(BlockDataSO.BlockData[] blockDatas)
    {
        //用意してあるUIの数だけ繰り返す
        for (int i = 0; i < imgNextBlocks.Length; i++)
        {
            //生成予定のブロックのイメージのスプライトを設定
            imgNextBlocks[i].sprite = blockDatas[i].sprite;
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
