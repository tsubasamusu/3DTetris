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

        //�i���j
        BlockManager.instance.CurrentBlock= GenerateBlock();
    }

    /// <summary>
    /// �u���b�N�𐶐�����
    /// </summary>
    /// <returns>���������u���b�N</returns>
    public GameObject GenerateBlock()
    {
        //�u���b�N�𐶐�
        GameObject generatedBlock= Instantiate(nextBlockDatas[0].prefab);

        //�������W��x������ݒ�
        float x = nextBlockDatas[0].isEvenWidth ? 0f : 0.5f;

        //���������u���b�N�̈ʒu��ݒ�
        generatedBlock.transform.position = new Vector3(x, 25f, 0f);

        //���������u���b�N����BlockDetail���擾�o������
        if(generatedBlock.TryGetComponent(out BlockController blockController))
        {
            //���������u���b�N�ɁA���̃u���b�N���g�̏���n��
            blockController.SetUpBlock(nextBlockDatas[0]);
        }
        //�擾�Ɏ��s������
        else
        {
            //�����
            Debug.Log("���������u���b�N�����BlockController�̎擾�Ɏ��s");
        }

        //�����\��̃u���b�N�̃f�[�^���X�V����
        UpdateNextBlockDatas();

        //���������u���b�N��Ԃ�
        return generatedBlock;
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
