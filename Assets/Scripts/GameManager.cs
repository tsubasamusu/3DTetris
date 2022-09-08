using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//LoadScene���\�b�h���g�p

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private BlockGenerator blockGenerator;//BlockGenerator

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator Start()
    {
        //�Q�[���X�^�[�g���o���I���܂ő҂�
        yield return UIManager.instance.PlayGameStart();

        //�u���b�N�𐶐����A���������u���b�N��BlockManager�ɓn��
        BlockManager.instance.CurrentBlock = blockGenerator.GenerateBlock();

        //���_���u0�v�ɐݒ�
        UIManager.instance.UpdateTxtScore(0);
    }

    /// <summary>
    /// �Q�[���I�[�o�[�����̏������s��
    /// </summary>
    public void PrepareGameOver()
    {
        //�Q�[���I�[�o�[�������s��
        StartCoroutine(GameOver());
    }

    /// <summary>
    /// �Q�[���I�[�o�[�������s��
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator GameOver()
    {
        //�Q�[���I�[�o�[���o���I���܂ő҂�
        yield return UIManager.instance.PlayGameOver();

        //Main�V�[����ǂݍ���
        SceneManager.LoadScene("Main");
    }
}
