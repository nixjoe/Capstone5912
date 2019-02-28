﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public new Camera camera { get; private set; }

    public CinemachineTargetGroup targetGroup;
    public CinemachineVirtualCamera overviewCam;
    public CinemachineFreeLook freeLookCam;
    public int PlayerId;
    private Transform currentRoom;
    private bool thirdPerson;

    private void Awake() {
        camera = GetComponentInChildren<Camera>();
        targetGroup = GetComponentInChildren<CinemachineTargetGroup>();
    }

    public void AddRoomToCamera(Transform room) {
        targetGroup.RemoveMember(currentRoom);
        currentRoom = room;
        targetGroup.AddMember(room, 1f, 10f);
    }

    public void AddPlayerToCamera(Transform player) {
        targetGroup.AddMember(player, 1f, 7f);
        if (freeLookCam.Follow == null) {
            freeLookCam.m_YAxis.m_InputAxisName = freeLookCam.m_YAxis.m_InputAxisName.Replace("#", PlayerId.ToString());
            freeLookCam.m_XAxis.m_InputAxisName = freeLookCam.m_XAxis.m_InputAxisName.Replace("#", PlayerId.ToString());
            freeLookCam.Follow = player;
            freeLookCam.LookAt = player;
            Animator playerAnim = player.GetComponentInChildren<Animator>();
            freeLookCam.GetRig(0).LookAt = playerAnim.GetBoneTransform(HumanBodyBones.Head);
            freeLookCam.GetRig(1).LookAt = playerAnim.GetBoneTransform(HumanBodyBones.Chest);
            freeLookCam.GetRig(2).LookAt = playerAnim.GetBoneTransform(HumanBodyBones.Hips);
        }
    }

    public void RemoveTarget(Transform t) {
        targetGroup.RemoveMember(t);
    }

    public void ClearTargets() {
        foreach (CinemachineTargetGroup.Target target in targetGroup.m_Targets) {
            targetGroup.RemoveMember(target.target);
        }
    }

    public void SwitchToThirdPerson() {
        thirdPerson = true;
        overviewCam.gameObject.SetActive(!thirdPerson);
        freeLookCam.gameObject.SetActive(thirdPerson);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SwitchToOverview() {
        thirdPerson = false;
        overviewCam.gameObject.SetActive(!thirdPerson);
        freeLookCam.gameObject.SetActive(thirdPerson);
        Cursor.lockState = CursorLockMode.None;
    }
}
