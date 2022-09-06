using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private float currentFallSpeed;//�u���b�N�̍��̍~�����x

    private GameObject mainCamera;//���C���J�����Q�[���I�u�W�F�N�g

    private BlockDataSO.BlockData myBlockData;//�����̃u���b�N�̃f�[�^

    private bool isSideLimit;//�X�e�[�W�̒[�ɂ��邩�ǂ���

    /// <summary>
    /// ���g�̐����J�n����ɌĂяo�����
    /// </summary>
    private void Start()
    {
        //���C���J�����Q�[���I�u�W�F�N�g���擾
        mainCamera = GameObject.FindWithTag("MainCamera");
    }

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //�������̑��̃u���b�N�ɐG�ꂽ��
        if(CheckContactedDown())
        {
            //BlockManager����K�؂ȏ������Ăяo��
            BlockManager.instance.StoppedCurrentBlock();

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�u���b�N�̗������x��ݒ�
        currentFallSpeed = Input.GetKey(KeyCode.DownArrow) ? GameData.instance.SpecialFallSpeed : GameData.instance.NormalFallSpeed;

        //�E��󂪉����ꂽ��
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            //�X�e�[�W�̒[�ɂ�����
            if(CheckContactedSide())
            {
                //�J�������猩�ĉE�ɂ�����
                if (mainCamera.transform.position.z<0f&&transform.position.x>0f||mainCamera.transform.position.z>=0f&&transform.position.x<0f)
                {
                    //�ȍ~�̏������s��Ȃ�
                    return;
                }
            }

            //�J�����̈ʒu�ɉ����Ĉړ�������ݒ�
            float moveValue = mainCamera.transform.position.z < 0f ? 1f : -1f;

            //�J�������猩�ĉE�Ɉړ�����
            transform.Translate(new Vector3(moveValue, 0f, 0f));
        }
        //����󂪉����ꂽ��
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //�X�e�[�W�̒[�ɂ�����
            if (CheckContactedSide())
            {
                //�J�������猩�č��ɂ�����
                if (mainCamera.transform.position.z < 0f && transform.position.x < 0f || mainCamera.transform.position.z >= 0f && transform.position.x > 0f)
                {
                    //�ȍ~�̏������s��Ȃ�
                    return;
                }
            }

            //�J�����̈ʒu�ɉ����Ĉړ�������ݒ�
            float moveValue = mainCamera.transform.position.z < 0f ? -1f : 1f;

            //�J�������猩�č��Ɉړ�����
            transform.Translate(new Vector3(moveValue,0f,0f));
        }

        //���󂪉����ꂽ��
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            //TODO:�z�[���h�E�g�p���鏈��
        }

        //���g����]�ł��Ȃ����W�ɂ�����
        if (Mathf.Abs(transform.position.x)>myBlockData.maxRotPosX)
        {
            //TODO:SoundManager����u�u�b�u�[�v�Ƃ������ʉ���炷�������Ăяo��

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //���N���b�N���ꂽ��
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //�J�����̈ʒu�ɉ����ĉ�]������ݒ�
            float rotateValue = mainCamera.transform.position.z < 0 ? 90f : -90f;

            //�J�������猩�Ĕ����v���ɉ�]������
            transform.GetChild(0).transform.eulerAngles = new Vector3(0f,0f, transform.GetChild(0).transform.eulerAngles.z+rotateValue);
        }
    }

    /// <summary>
    /// ��莞�Ԃ��ƂɌĂяo�����
    /// </summary>
    private void FixedUpdate()
    {
        //�u���b�N�𗎉�������
        transform.Translate(new Vector3(0f,-currentFallSpeed,0f));
    }

    /// <summary>
    /// �������̑��̃u���b�N�ɐڐG�������ǂ������ׂ�
    /// </summary>
    /// <returns>�������̑��̃u���b�N�ɐڐG������true</returns>
    private bool CheckContactedSide()
    {
        //TODO:�e�u���b�N���牡�Ɍ����𔭎˂��A�ڐG������擾���鏈��

        //�i���j
        return false;
    }

    /// <summary>
    /// �������̑��̃u���b�N�ɐڐG�������ǂ������ׂ�
    /// </summary>
    /// <returns>�������̑��̃u���b�N�ɐڐG������true</returns>
    private bool CheckContactedDown()
    {
        //TODO:�e�u���b�N���牺�Ɍ����𔭎˂��A�ڐG������擾���鏈��

        //�i���j
        return false;
    }

    /// <summary>
    /// �����̃u���b�N�̏����ݒ���s��
    /// </summary>
    /// <param name="yourBlockData">�Ώۂ̃u���b�N�̃f�[�^</param>
    public void SetUpBlock(BlockDataSO.BlockData yourBlockData)
    {
        //�����̃u���b�N�̃f�[�^���擾
        myBlockData = yourBlockData;
    }
}
