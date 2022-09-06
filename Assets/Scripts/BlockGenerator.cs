using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField]
    private BlockDataSO blockDataSO;//BlockDataSO

    [HideInInspector]
    public BlockDataSO.BlockData[] nextBlockDatas;//�����\��̃u���b�N�̃f�[�^�̔z��

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    private void Start()
    {
        //�����\��̃u���b�N�̃f�[�^�̔z��̗v�f����ݒ�
        nextBlockDatas = new BlockDataSO.BlockData[GameData.instance.AppointmentsNumber];

        //�����\��̃u���b�N�̃f�[�^�̔z��̗v�f�������J��Ԃ�
        for (int i = 0; i < nextBlockDatas.Length; i++)
        {
            //�����\��̃u���b�N�̃f�[�^�̔z��̊e�v�f�Ƀ����_���ȃf�[�^������
            nextBlockDatas[i] = blockDataSO.blockDataList[Random.Range(0, blockDataSO.blockDataList.Count)];
        }
    }

    /// <summary>
    /// �u���b�N�𐶐�����
    /// </summary>
    /// <returns>���������u���b�N�̃f�[�^</returns>
    public BlockDataSO.BlockData GenerateBlock()
    {
        //�u���b�N�𐶐�
        GameObject generatedBlock= Instantiate(nextBlockDatas[0].prefab);

        //�������W��x������ݒ�
        float x = nextBlockDatas[0].isOddWidth ? 0.5f : 0f;

        //���������u���b�N�̈ʒu��ݒ�
        generatedBlock.transform.position = new Vector3(x, 25f, 0f);

        //�����\��̃u���b�N�̃f�[�^���X�V����
        UpdateNextBlockDatas();

        //���������u���b�N�̃f�[�^��Ԃ�
        return blockDataSO.blockDataList.Find(x => x.prefab == generatedBlock);
    }

    /// <summary>
    /// �����\��̃u���b�N�̃f�[�^���X�V����
    /// </summary>
    private void UpdateNextBlockDatas()
    {
        //�����\��̃u���b�N�̃f�[�^�̔z��̗v�f�������J��Ԃ�
        for (int i = 0; i < nextBlockDatas.Length; i++)
        {
            //�Ō�̌J��Ԃ������ɂȂ�����
            if (i == (nextBlockDatas.Length - 1))
            {
                //�����\��̃u���b�N�̃f�[�^�̔z��̍Ō�̗v�f�Ƀ����_���ȃf�[�^������
                nextBlockDatas[i] = blockDataSO.blockDataList[Random.Range(0, blockDataSO.blockDataList.Count)];

                //�J��Ԃ��������I������
                break;
            }

            //�f�[�^��1��O�ɂ��炷
            nextBlockDatas[i] = nextBlockDatas[i + 1];
        }
    }
}