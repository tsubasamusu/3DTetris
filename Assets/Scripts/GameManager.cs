using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;

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
}
