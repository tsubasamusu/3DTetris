using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static BlockManager instance;//�C���X�^���X

    [SerializeField]
    private BlockGenerator blockGenerator;//BlockGenerator

    [HideInInspector]
    public List<GameObject> cubeList=new List<GameObject>();//���݁A�X�e�[�W��ɒ~�ς���Ă��闧���̂̃��X�g

    private GameObject currentBlock;//���݃A�N�e�B�u�ȃu���b�N

    /// <summary>
    /// ���݃A�N�e�B�u�ȃu���b�N�̐ݒ�p
    /// </summary>
    public GameObject CurrentBlock
    { set { currentBlock = value; } }

    /// <summary>
    /// Start���\�b�h���O�ɌĂяo�����
    /// </summary>
    private void Awake()
    {
        //�ȍ~�A�V���O���g���ɕK�{�̋L�q
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
    /// ���݃A�N�e�B�u�ȃu���b�N�̓������~�܂������ɌĂяo�����
    /// </summary>
    public void StoppedCurrentBlock()
    {
        //���݃A�N�e�B�u�ȃu���b�N�̑��̐������J��Ԃ�
        for(int i = 0; i < currentBlock.transform.GetChild(0).transform.childCount; i++)
        {
            //���݃A�N�e�B�u�ȃu���b�N�̑S�Ă̑������X�g�ɒǉ�
            cubeList.Add(currentBlock.transform.GetChild(0).transform.GetChild(i).gameObject);
        }

        //���݃A�N�e�B�u�ȃu���b�N����BlockController���擾�o������
        if(currentBlock.TryGetComponent(out BlockController blockController))
        {
            //BlockController��񊈐����i���ʂȏ�����h���j
            blockController.enabled = false;
        }
        //���݃A�N�e�B�u�ȃu���b�N����BlockController���擾�o���Ȃ�������
        else
        {
            //�����
            Debug.Log("���݃A�N�e�B�u�ȃu���b�N�����BlockController�̎擾�Ɏ��s");
        }

        //TODO:�u���b�N�̏����̊m�F

        //�u���b�N��1�x�������A���������u���b�N�̏����擾
        currentBlock = blockGenerator.GenerateBlock();
    }
}
