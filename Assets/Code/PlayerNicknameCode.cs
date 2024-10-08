using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerNicknameCode : MonoBehaviour
{
    PhotonView m_PV;

    [SerializeField] TextMeshPro m_nickname;

    private void Start()
    {
        m_PV = GetComponent<PhotonView>();
        //m_PV.Owner.NickName = PhotonNetwork.NickName;
        //m_nickname.text = m_PV.Owner.NickName;
        //ShowNickname();
        //m_PV.RPC("ShowNickname", RpcTarget.AllBuffered, m_PV.Owner.NickName);
    }

    private void Update()
    {
        //ShowNickname();
    }

    //[PunRPC]

}
