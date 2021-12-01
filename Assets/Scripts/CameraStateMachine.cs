using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateMachine : MonoBehaviour
{
    public enum Zone { BLVein, TutorialVein, Stomach, Brain, Heart, HeartCenter, HeartGame, StomachCutscene, ZOOM };

    public Zone m_Zone = Zone.Heart;

    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(m_Zone)
        {
            case Zone.BLVein:
                {
                    m_Animator.Play("BLVeinFollowCam");
                    break;
                }
            case Zone.TutorialVein:
                {
                    m_Animator.Play("TutorialVeinFollowCam");
                    break;
                }
            case Zone.Stomach:
                {
                    m_Animator.Play("StomachFollowCam");
                    break;
                }
            case Zone.StomachCutscene:
                {
                    m_Animator.Play("StomachCutsceneCam");
                    break;
                }
            case Zone.Heart:
                {
                    m_Animator.Play("HeartFollowCam");
                    break;
                }
            case Zone.HeartCenter:
                {
                    m_Animator.Play("HeartCenterCam");
                    break;
                }
            case Zone.HeartGame:
                {
                    m_Animator.Play("HeartGameCam");
                    break;
                }
            case Zone.Brain:
                {
                    m_Animator.Play("BrainFollowCam");
                    break;
                }
            case Zone.ZOOM:
                {
                    m_Animator.Play("ZOOMCam");
                    break;
                }
        }
    }
}
