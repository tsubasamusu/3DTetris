using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private float currentFallSpeed;//�u���b�N�̍��̍~�����x

    private GameObject mainCamera;//���C���J�����Q�[���I�u�W�F�N�g

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
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
        //�u���b�N�̗������x��ݒ�
        currentFallSpeed = Input.GetKey(KeyCode.DownArrow) ? GameData.instance.SpecialFallSpeed : GameData.instance.NormalFallSpeed;

        //�E��󂪉����ꂽ��
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            //�J�����̈ʒu�ɉ����Ĉړ�������ݒ�
            float moveValue = mainCamera.transform.position.z < 0 ? 1f : -1f;

            //�J�������猩�ĉE�Ɉړ�����
            transform.Translate(new Vector3(moveValue, 0f, 0f));
        }
        //����󂪉����ꂽ��
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //�J�����̈ʒu�ɉ����Ĉړ�������ݒ�
            float moveValue = mainCamera.transform.position.z < 0 ? -1f : 1f;

            //�J�������猩�č��Ɉړ�����
            transform.Translate(new Vector3(moveValue,0f,0f));
        }

        //���󂪉����ꂽ��
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            //TODO:�z�[���h�E�g�p���鏈��
        }

        //���N���b�N���ꂽ��
        if(Input.GetKeyDown(KeyCode.Mouse0))
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
}