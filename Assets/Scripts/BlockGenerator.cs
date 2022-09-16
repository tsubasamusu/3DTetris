using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField]
    private BlockDataSO blockDataSO;//BlockDataSO

    [HideInInspector]
    public BlockDataSO.BlockData[] nextBlockDatas;//�����\��̃u���b�N�̃f�[�^�̔z��

    private bool stop;//�u���b�N�̐�����~����p

    private bool setUp;//�����ݒ肪�����������ǂ���

    /// <summary>
    /// BlockGenerator�̏����ݒ���s��
    /// </summary>
    public void SetUpBlockGenerator()
    {
        //�����\��̃u���b�N�̃f�[�^�̔z��̗v�f����ݒ�
        nextBlockDatas = new BlockDataSO.BlockData[GameData.instance.AppointmentsNumber];

        //�����\��̃u���b�N�̃f�[�^�̔z��̗v�f�������J��Ԃ�
        for (int i = 0; i < nextBlockDatas.Length; i++)
        {
            //�����\��̃u���b�N�̃f�[�^�̔z��̊e�v�f�Ƀ����_���ȃf�[�^������
            nextBlockDatas[i] = blockDataSO.blockDataList[Random.Range(0, blockDataSO.blockDataList.Count)];
        }

        //�����ݒ芮����Ԃɐ؂�ւ���
        setUp = true;
    }

    /// <summary>
    /// �u���b�N�𐶐�����
    /// </summary>
    /// <param name="blockData">�����������u���b�N�̃f�[�^</param>
    /// <returns>���������u���b�N</returns>
    public GameObject GenerateBlock(BlockDataSO.BlockData blockData = null)
    {
        //�����ݒ肪�������Ă��Ȃ��Ȃ�
        if(!setUp)
        {
            //�ȍ~�̏������s��Ȃ�
            return null;
        }

        //�u���b�N�̐�����~���߂��o�Ă�����
        if(stop)
        {
            //�ȍ~�̏������s��Ȃ�
            return null;
        }

        //��������u���b�N�̌��̃f�[�^��ݒ�
        BlockDataSO.BlockData originalData = blockData == null ? nextBlockDatas[0] : blockData;

        //�u���b�N�𐶐�
        GameObject generatedBlock = Instantiate(originalData.prefab);

        //�������W��x������ݒ�
        float x = originalData.isEvenWidth ? 0f : 0.5f;

        //���������u���b�N�̈ʒu��ݒ�
        generatedBlock.transform.position = new Vector3(x, 25f, 0f);

        //���������u���b�N����BlockDetail���擾�o������
        if (generatedBlock.TryGetComponent(out BlockController blockController))
        {
            //���������u���b�N�̏����ݒ���s��
            blockController.SetUpBlockController(originalData);
        }
        //�擾�Ɏ��s������
        else
        {
            //�����
            Debug.Log("���������u���b�N�����BlockController�̎擾�Ɏ��s");
        }

        //��������u���b�N���w�肳��Ă�����
        if(blockData != null)
        {
            //�ȍ~�̏������s��Ȃ�
            return generatedBlock;
        }

        //�����\��̃u���b�N�̃f�[�^���X�V����
        UpdateNextBlockDatas();

        //�����\��̃u���b�N�̕\����ݒ�
        UIManager.instance.SetImgNextBlocks(nextBlockDatas);

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

    /// <summary>
    /// �u���b�N�̐������~�߂�
    /// </summary>
    public void StopGenerateBlock()
    {
        //�u���b�N�̐������~����
        stop = true;
    }
}
