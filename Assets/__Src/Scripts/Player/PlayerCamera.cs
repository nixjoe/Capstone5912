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
    public NoiseSettings noiseProfile;
    public BoltEntity CameraPlayer;
    private Transform currentRoom;
    private bool thirdPerson;

    private void Awake() {
        camera = GetComponentInChildren<Camera>();
        targetGroup = GetComponentInChildren<CinemachineTargetGroup>();
    }

    public void AddRoomToCamera(Transform room) {
        DeactivateShake();
        targetGroup.RemoveMember(currentRoom);
        currentRoom = room;
        targetGroup.AddMember(room, 1f, 10f);
    }

    public void AddPlayerToCamera(Transform player) {
        targetGroup.AddMember(player, 1f, 7f);
        if (freeLookCam.Follow == null) {
            CameraPlayer = player.GetComponent<BoltEntity>();
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

    public void ActivateShake(float ampGain = 1f, float freqGain = 1f) {
        overviewCam.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = noiseProfile;
        overviewCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = ampGain;
        overviewCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = freqGain;
        freeLookCam.GetRig(0).AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = noiseProfile;
        freeLookCam.GetRig(1).AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = noiseProfile;
        freeLookCam.GetRig(2).AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = noiseProfile;
    }
    
    public void DeactivateShake() {
        overviewCam.DestroyCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        freeLookCam.GetRig(0).DestroyCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        freeLookCam.GetRig(1).DestroyCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        freeLookCam.GetRig(2).DestroyCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
}
