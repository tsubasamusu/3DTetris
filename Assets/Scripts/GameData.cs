using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;//�C���X�^���X

    [SerializeField]
    private float normalFallSpeed;//�u���b�N�̍~�����x�i���ʁj

    [SerializeField]
    private float specialFallSpeed;//�u���b�N�̍~�����x�i���ʁj

    [SerializeField]
    private float ghostFallSpeed;//�S�[�X�g�̍~�����x

    [SerializeField]
    private float timeLimit;//��������

    [SerializeField, Header("�u���b�N�̐����\��̐�")]
    private int appointmentsNumber;//�u���b�N�̐����\��̐�

    [SerializeField,Header("1�񂠂���̓��_")]
    private int scorePerColumn;//1�񂠂���̓��_

    /// <summary>
    /// �u���b�N�̍~�����x�i���ʁj�擾�p
    /// </summary>
    public float NormalFallSpeed
    { get { return normalFallSpeed; } }

    /// <summary>
    /// �u���b�N�̍~�����x�i���ʁj�擾�p
    /// </summary>
    public float SpecialFallSpeed
    { get { return specialFallSpeed; } }

    /// <summary>
    /// �������Ԏ擾�p
    /// </summary>
    public float TimeLimit
    { get { return timeLimit; } }

    /// <summary>
    /// �u�u���b�N�̐����\��̐��v�擾�p
    /// </summary>
    public int AppointmentsNumber
    { get { return appointmentsNumber; } }

    /// <summary>
    /// �u1�񂠂���̓��_�v�擾�p
    /// </summary>
    public int ScorePerColumn
    { get { return scorePerColumn; } }

    /// <summary>
    /// �S�[�X�g�̍~�����x�擾�p
    /// </summary>
    public float GhostFallSpeed
    { get { return ghostFallSpeed; } }

    /// <summary>
    /// Start���\�b�h���O�ɌĂяo�����
    /// </summary>
    private void Awake()
    {
        //�ȉ��A�V���O���g���ɕK�{�̋L�q
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
