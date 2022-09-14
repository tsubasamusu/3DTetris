using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//LoadSceneメソッドを使用

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private BlockGenerator blockGenerator;//BlockGenerator

    private bool isGameEnd;//ゲーム終了判定用

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator Start()
    {
        //BlockGeneratorの初期設定を行う
        blockGenerator.SetUpBlockGenerator();

        //ゲームスタート演出が終わるまで待つ
        yield return UIManager.instance.PlayGameStart();

        //イメージの向きの確認の準備を行う
        UIManager.instance.PrepareCheck();

        //ブロックを生成し、生成したブロックをBlockManagerに渡す
        BlockManager.instance.CurrentBlock = blockGenerator.GenerateBlock(this);

        //得点を「0」に設定
        UIManager.instance.UpdateTxtScore(0);

        //制限時間の減少を開始する
        StartCoroutine(ReduceTimeLimit());

        ///BGMを再生
        SoundManager.instance.PlaySound(SoundDataSO.SoundName.BGM, true);
    }

    /// <summary>
    /// 制限時間を減らしていく
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator ReduceTimeLimit()
    {
        //テキストの色を変更したかの判定用
        bool changedColor = false;

        //制限時間の初期値を取得
        float timeLimit = GameData.instance.TimeLimit;

        //無限に繰り返す
        while(true)
        {
            //ゲームが終了したら
            if(isGameEnd)
            {
                //繰り返し処理を終わる
                break;
            }

            //制限時間が終了したら
            if(timeLimit<=0f)
            {
                //ゲームクリア処理を行う
                StartCoroutine(GameClear());

                //繰り返し処理を終わる
                break;
            }

            //制限時間が10秒を切ったら
            if (timeLimit < 10f && !changedColor)
            {
                //制限時間のテキストの色を変える
                UIManager.instance.SetTxtTimeLimitColor(Color.red);

                //効果音を再生
                SoundManager.instance.PlaySound(SoundDataSO.SoundName.TenTimeLimitSE);

                //テキストの色を変更した状態に切り替える
                changedColor = true;
            }

            //制限時間を減らしていく
            timeLimit-= Time.deltaTime;

            //制限時間を表示する
            UIManager.instance.SetTxtTimeLimit(timeLimit);

            //次のフレームへ飛ばす（実質、Updateメソッド）
            yield return null;
        }
    }

    /// <summary>
    /// ゲームオーバー処理の準備を行う
    /// </summary>
    public void PrepareGameOver()
    {
        //ゲームオーバー処理を行う
        StartCoroutine(GameOver());
    }

    /// <summary>
    /// ゲームオーバー処理を行う
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator GameOver()
    {
        //ゲーム終了の準備を行う
        PrepareGameEnd();

        //効果音を再生
        SoundManager.instance.PlaySound(SoundDataSO.SoundName.GameOverSE);

        //ゲームオーバー演出が終わるまで待つ
        yield return UIManager.instance.PlayGameOver();

        //Mainシーンを読み込む
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// ゲームクリア処理を行う
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator GameClear()
    {
        //ゲーム終了の準備を行う
        PrepareGameEnd();

        //効果音を再生
        SoundManager.instance.PlaySound(SoundDataSO.SoundName.GameClearSE);

        //ゲームクリア演出が終わるまで待つ
        yield return UIManager.instance.PlayGameClear();

        //Mainシーンを読み込む
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// ゲーム終了の準備を行う
    /// </summary>
    private void PrepareGameEnd()
    {
        //ゲーム終了状態に切り替える
        isGameEnd = true;

        //ブロックの生成を止める
        blockGenerator.StopGenerateBlock();

        //現在アクティブなブロックを消す
        Destroy(BlockManager.instance.CurrentBlock);

        //マウスカーソルを表示する
        Cursor.visible = true;

        //BGMを止める
        SoundManager.instance.StopSound(0.5f);
    }
}
