using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static BlockManager instance;//�C���X�^���X

    [SerializeField]
    private BlockGenerator blockGenerator;//BlockGenerator

    //[HideInInspector]
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
        //4��J��Ԃ�
        for(int i = 0; i < 4; i++)
        {
            //���݃A�N�e�B�u�ȃu���b�N�̑�0�������X�g�ɒǉ�
            cubeList.Add(currentBlock.transform.GetChild(0).transform.GetChild(0).gameObject);

            //���݃A�N�e�B�u�ȃu���b�N�̑�0���̐e�����g�ɐݒ�
            currentBlock.transform.GetChild(0).transform.GetChild(0).transform.SetParent(transform);
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

        //�����̂̏������s�����m�F����
        CheckDigested();

        //�u���b�N��1�x�������A���������u���b�N�̏����擾
        currentBlock = blockGenerator.GenerateBlock();
    }

    /// <summary>
    /// �����̂̏������s�����ǂ����m�F���A�ꍇ�ɉ����ď������s��
    /// </summary>
    private void CheckDigested()
    {
        //20��J��Ԃ�
        for(int i = 1; i < 21; i++)
        {
            //����y���W�̗����̂̃��X�g���쐬
            List<GameObject> samePosYList = cubeList.FindAll(x => (x.transform.position.y > (i - 0.5f)) && (x.transform.position.y < (i + 0.5f)));
            
            //����y���W�̗����̂̐���10��菬����������i����񂪑����Ă��Ȃ�������j
            if(samePosYList.Count< 10)
            {
                //���̌J��Ԃ������ֈڂ�
                continue;
            }

            //10��J��Ԃ�
            for(int j = 0; j < 10; j++)
            {
                //�Ώۂ̗����̂���莞�Ԍ�ɏ���
                Destroy(cubeList.FindAll(x => (x.transform.position.y > (i - 0.5f)) && (x.transform.position.y < (i + 0.5f)))[j],0.25f);

                //�Ώۂ̗����̂����X�g�����菜��
                cubeList.Remove(cubeList.Find(x => (x.transform.position.y > (i - 0.5f)) && (x.transform.position.y < (i + 0.5f))));
            }
        }
    }
}
