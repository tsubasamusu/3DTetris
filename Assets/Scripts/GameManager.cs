using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//LoadScene���\�b�h���g�p

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private BlockGenerator blockGenerator;//BlockGenerator

    private bool isGameEnd;//�Q�[���I������p

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator Start()
    {
        //�Q�[���X�^�[�g���o���I���܂ő҂�
        yield return UIManager.instance.PlayGameStart();

        //�C���[�W�̌����̊m�F�̏������s��
        UIManager.instance.PrepareCheck();

        //�u���b�N�𐶐����A���������u���b�N��BlockManager�ɓn��
        BlockManager.instance.CurrentBlock = blockGenerator.GenerateBlock();

        //���_���u0�v�ɐݒ�
        UIManager.instance.UpdateTxtScore(0);

        //�������Ԃ̌������J�n����
        StartCoroutine(ReduceTimeLimit());  
    }

    /// <summary>
    /// �������Ԃ����炵�Ă���
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator ReduceTimeLimit()
    {
        //�e�L�X�g�̐F��ύX�������̔���p
        bool changedColor = false;

        //�������Ԃ̏����l���擾
        float timeLimit = GameData.instance.TimeLimit;

        //�����ɌJ��Ԃ�
        while(true)
        {
            //�Q�[�����I��������
            if(isGameEnd)
            {
                //�J��Ԃ��������I���
                break;
            }

            //�������Ԃ��I��������
            if(timeLimit<=0f)
            {
                //�Q�[���N���A�������s��
                StartCoroutine(GameClear());

                //�J��Ԃ��������I���
                break;
            }

            //�������Ԃ�10�b��؂�����
            if (timeLimit < 10f && !changedColor)
            {
                //�������Ԃ̃e�L�X�g�̐F��ς���
                UIManager.instance.SetTxtTimeLimitColor(Color.red);

                //�e�L�X�g�̐F��ύX������Ԃɐ؂�ւ���
                changedColor = true;
            }

            //�������Ԃ����炵�Ă���
            timeLimit-= Time.deltaTime;

            //�������Ԃ�\������
            UIManager.instance.SetTxtTimeLimit(timeLimit);

            //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
            yield return null;
        }
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
        //�Q�[���I���̏������s��
        PrepareGameEnd();

        //�Q�[���I�[�o�[���o���I���܂ő҂�
        yield return UIManager.instance.PlayGameOver();

        //Main�V�[����ǂݍ���
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// �Q�[���N���A�������s��
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator GameClear()
    {
        //�Q�[���I���̏������s��
        PrepareGameEnd();

        //�Q�[���N���A���o���I���܂ő҂�
        yield return UIManager.instance.PlayGameClear();

        //Main�V�[����ǂݍ���
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// �Q�[���I���̏������s��
    /// </summary>
    private void PrepareGameEnd()
    {
        //�Q�[���I����Ԃɐ؂�ւ���
        isGameEnd = true;

        //�u���b�N�̐������~�߂�
        blockGenerator.StopGenerateBlock();

        //���݃A�N�e�B�u�ȃu���b�N������
        Destroy(BlockManager.instance.CurrentBlock);
    }
}
