using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField]
    private BlockGenerator blockGenerator;//BlockGenerator

    private BlockDataSO.BlockData currentBlockData;//���݃v���[���[�����삵�Ă���u���b�N�̃f�[�^

    /// <summary>
    /// �u���b�N�̊Ǘ����J�n����
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator StartBlockManagement()
    {
        //�u���b�N��1�x�������A���������u���b�N�̏����擾
        currentBlockData= blockGenerator.GenerateBlock();

        //�����ɌJ��Ԃ�
        while (true)
        {
            //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
            yield return null;
        }
    }
}
