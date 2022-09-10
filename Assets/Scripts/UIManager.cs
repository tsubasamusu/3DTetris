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
    private Transform cameraTran;//カメラの位置情報

    [SerializeField]
    private CanvasGroup canvasGroup;//CanvasGroup

    [SerializeField]
    private Image[] imgNextBlocks;//次のブロックのイメージの配列

    [SerializeField]
    private List<LogoData> logoDatasList=new();//ロゴのデータのリスト

    private int score;//得点

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
        //演出終了判定用
        bool end = false;

        //ボタン判定用
        bool clicked = false;

        //不要なUIを非表示にする
        canvasGroup.alpha = 0f;

        //ボタンを非活性化
        button.interactable = false;

        //背景を白色に設定
        imgBackGround.color = Color.white;

        //ボタンを青色に設定
        imgButton.color = Color.blue;

        //ボタンのテキストを空にする
        txtButton.text = string.Empty;

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

        //効果音を再生
        SoundManager.instance.PlaySound(SoundDataSO.SoundName.BtnGameStartSE);

        //マウスカーソルを非表示にする
        Cursor.visible = false;

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

        //UIを表示する
        canvasGroup.alpha = 1f;

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

        //不要なUIを非表示にする
        canvasGroup.alpha = 0f;

        //得点を非表示にする
        txtScore.DOFade(0f, 0f);

        //背景を黒色に設定
        imgBackGround.color = Color.black;

        //ボタンのテキストを空にする
        txtButton.text=string.Empty;

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

        //ボタンのテキストを可視化
        txtButton.DOFade(1f,0f).OnComplete(() =>

        //ボタンのテキストを設定し、表示
        txtButton.DOText("Restart", 1f));

        //ボタンを表示
        imgButton.DOFade(1f, 1f).OnComplete(() =>

        //ボタンを活性化
        button.interactable = true);

        //ボタンが押されるまで待つ
        yield return new WaitUntil(() => clicked == true);

        //効果音を再生
        SoundManager.instance.PlaySound(SoundDataSO.SoundName.BtnRestartSE);

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

            //ボタンを非活性化
            button.interactable = false;
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

        //不要なUIを非表示にする
        canvasGroup.alpha = 0f;

        //背景を白色に設定
        imgBackGround.color = Color.white;

        //ボタンのテキストを空にする
        txtButton.text = string.Empty;

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

        //ボタンのテキストを可視化
        txtButton.DOFade(1f,0f).OnComplete(()=>

        //ボタンのテキストを設定し、表示
        txtButton.DOText("Restart", 1f));

        //得点を結果表示位置に移動させる
        txtScore.transform.DOMove(resultTran.position, 1f).OnComplete(() =>

        //ボタンを活性化
        button.interactable = true);

        //ボタンが押されるまで待つ
        yield return new WaitUntil(() => clicked == true);

        //効果音を再生
        SoundManager.instance.PlaySound(SoundDataSO.SoundName.BtnRestartSE);

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

            //ボタンを非活性化
            button.interactable = false;
        }
    }

    /// <summary>
    /// 得点の表示を更新する
    /// </summary>
    /// <param name="updateValue">得点の更新量</param>
    public void UpdateTxtScore(int updateValue)
    {
        //アニメーション終了判定用
        bool end = false;

        //得点の表示の更新を開始する
        StartCoroutine(UpdateTxtScore());

        //得点の記録を更新
        DOTween.To(() => score,(x) => score = x,score+updateValue,0.5f).OnComplete(()=>
        
        //アニメーション終了状態に切り替える
        end=true);

        //得点の表示を更新する
        IEnumerator UpdateTxtScore()
        {
            //アニメーションが終了するまで繰り返す
            while (!end)
            {
                //得点のテキストを設定
                txtScore.text = score.ToString()+"\npoint";

                //次のフレームへ飛ばす（実質、Updateメソッド）
                yield return null;
            }
        }
    }

    /// <summary>
    /// 制限時間の表示を設定する
    /// </summary>
    /// <param name="remainingTime">残り時間</param>
    public void SetTxtTimeLimit(float remainingTime)
    {
        //制限時間の表示を残り時間に設定
        txtTimeLimit.text = remainingTime.ToString("F1");
    }

    /// <summary>
    /// 保存されているブロックの表示を設定する
    /// </summary>
    /// <param name="blockSprite">ブロックのスプライト</param>
    public void SetImgHoldBllock(Sprite blockSprite)
    {
        //保存されているブロックのスプライトを設定
        imgHold.sprite = blockSprite;

        //保存したブロックのイメージを表示
        imgHold.DOFade(0f, 0f).OnComplete(() => imgHold.DOFade(1f, 0.5f));
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

            //生成予定のブロックのイメージを表示
            imgNextBlocks[i].DOFade(1f, 0f);
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

    /// <summary>
    /// イメージの向きの確認の準備を行う
    /// </summary>
    public void PrepareCheck()
    {
        //イメージの向きが正しいか確認を開始する
        StartCoroutine(CheckImagesDirection());
    }

    /// <summary>
    /// イメージの向きが正しいか確認する
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator CheckImagesDirection()
    {
        //無限に繰り返す
        while (true)
        {
            //適切な角度を取得
            float angleY = cameraTran.position.z < 0f ? 0f : 180f;

            //角度を設定
            imgHold.transform.eulerAngles= new Vector3(0f, angleY, 0f);

            //生成予定のブロックの数だけ繰り返す
            for (int i = 0; i < imgNextBlocks.Length; i++)
            {
                //角度を設定
                imgNextBlocks[i].transform.eulerAngles=new Vector3(0f, angleY, 0f);
            }

            //次のフレームへ飛ばす（実質、Updateメソッド）
            yield return null;
        }
    }

    /// <summary>
    /// 保存されているブロックのイメージを空にする
    /// </summary>
    public void ClearImgHoldBlock()
    {
        //保存されているブロックを非表示にする
        imgHold.DOFade(0f, 0f);

        //スプライトをnullにする
        imgHold.sprite = null;
    }

    /// <summary>
    /// 制限時間のテキストの色を設定する
    /// </summary>
    /// <param name="color">色</param>
    public void SetTxtTimeLimitColor(Color color)
    {
        ///制限時間のテキストの色を設定
        txtTimeLimit.color = color;
    }
}
