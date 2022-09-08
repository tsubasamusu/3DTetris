using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//LoadSceneメソッドを使用

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private BlockGenerator blockGenerator;//BlockGenerator

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator Start()
    {
        //ゲームスタート演出が終わるまで待つ
        yield return UIManager.instance.PlayGameStart();

        //ブロックを生成し、生成したブロックをBlockManagerに渡す
        BlockManager.instance.CurrentBlock = blockGenerator.GenerateBlock();

        //得点を「0」に設定
        UIManager.instance.UpdateTxtScore(0);
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
        //ゲームオーバー演出が終わるまで待つ
        yield return UIManager.instance.PlayGameOver();

        //Mainシーンを読み込む
        SceneManager.LoadScene("Main");
    }
}
