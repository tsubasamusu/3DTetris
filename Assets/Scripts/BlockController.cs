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
            //���g��K�؂Ȉʒu�Ɉړ�������
            SetMeRightPos();

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
        if (Mathf.Abs(transform.position.x)>(5f-myBlockData.rotLength)||transform.position.y<(0.5f+myBlockData.rotLength)||!CheckLengthToOtherCube())
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
        //���g�̑��̐������J��Ԃ�
        for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
        {
            //�����̕�����ݒ�
            Vector3 direction = transform.GetChild(0).transform.GetChild(i).transform.position.x > 0f ? new Vector3(1f,0f,0f) : new Vector3(-1f,0f,0f);

            //������̌������쐬
            Ray ray = new(transform.GetChild(0).transform.GetChild(i).transform.position, direction);

            //���������̃R���C�_�[�ɐڐG���Ȃ�������
            if (!Physics.Raycast(ray,out RaycastHit hit,0.5f))
            {
                //���̌J��Ԃ������ֈڂ�
                continue;
            }

            //�G�ꂽ���肪���ł͂Ȃ�������
            int isNotGrandchildCount = 0;

            //���g�̑��̐������J��Ԃ�
            for (int j = 0; j < transform.GetChild(0).transform.childCount; i++)
            {
                //�G�ꂽ���肪���g�̑���1�l�Ȃ�
                if (hit.transform.gameObject == transform.GetChild(0).transform.GetChild(j).gameObject)
                {
                    //���̌J��Ԃ������ֈڂ�
                    continue;
                }

                //�񐔂��L�^
                isNotGrandchildCount++;

                //�G�ꂽ���肪���g�̑S�Ă̑��ȊO�Ȃ�
                if (isNotGrandchildCount == transform.GetChild(0).transform.childCount)
                {
                    //true��Ԃ�
                    return true;
                }
            }
        }

        //false��Ԃ�
        return false;
    }

    /// <summary>
    /// �������̑��̃u���b�N�ɐڐG�������ǂ������ׂ�
    /// </summary>
    /// <returns>�������̑��̃u���b�N�ɐڐG������true</returns>
    private bool CheckContactedDown()
    {
        //���g�̑��̐������J��Ԃ�
        for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
        {
            //������̌������쐬
            Ray ray = new(transform.GetChild(0).transform.GetChild(i).transform.position, Vector3.down);

            //���������̃R���C�_�[�ɐڐG���Ȃ�������
            if (!Physics.Raycast(ray, out RaycastHit hit, 0.5f))
            {
                //���̌J��Ԃ������ֈڂ�
                continue;
            }

            //�G�ꂽ���肪���ł͂Ȃ�������
            int isNotGrandchildCount = 0;

            //���g�̑��̐������J��Ԃ�
            for (int j = 0; j < transform.GetChild(0).transform.childCount;i++)
            {
                //�G�ꂽ���肪���g�̑���1�l�Ȃ�
                if(hit.transform.gameObject== transform.GetChild(0).transform.GetChild(j).gameObject)
                {
                    //���̌J��Ԃ������ֈڂ�
                    continue;
                }

                //�񐔂��L�^
                isNotGrandchildCount++;

                //�G�ꂽ���肪���g�̑S�Ă̑��ȊO�Ȃ�
                if(isNotGrandchildCount== transform.GetChild(0).transform.childCount)
                {
                    //true��Ԃ�
                    return true;
                }
            }
        }

        //false��Ԃ�
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

    /// <summary>
    /// ���n��Ɏ��g��K�؂Ȉʒu�Ɉړ�������
    /// </summary>
    private void SetMeRightPos()
    {
        //���g��y���W�̏��������i�덷�j���擾
        float excess = transform.position.y % 0.5f;

        //�덷���C�����邽�߂̒l���擾
        float valueY = excess < 0.25 ? -excess : 0.5f - excess;

        //���W���Đݒ�
        transform.position = new Vector3(transform.position.x, transform.position.y + valueY, 0f);
    }

    /// <summary>
    /// ���̗����̂Ƃ̋������m�F����
    /// </summary>
    /// <returns>���̗����̂Ƃ̋������m�F���������ŉ�]�\�Ȃ�true</returns>
    private bool CheckLengthToOtherCube()
    {
        //���݃X�e�[�W�ɒ~�ς���Ă��闧���̂̐������J��Ԃ�
        for(int i = 0; i < BlockManager.instance.cubeList.Count; i++)
        {
            //���̗����̂Ƃ�x�����̋������߂����邩�A
            if(Mathf.Abs(BlockManager.instance.cubeList[i].transform.position.x-transform.position.x)<(myBlockData.rotLength+1)&&
                //���̗����̂Ƃ�y�����̋������߂�������
                Mathf.Abs(BlockManager.instance.cubeList[i].transform.position.y - transform.position.y)< (myBlockData.rotLength+1))
            {
                //false��Ԃ�
                return false;
            }
        }

        //true��Ԃ�
        return true;
    }
}
