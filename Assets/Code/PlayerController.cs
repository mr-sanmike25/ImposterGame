using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviourPunCallbacks
{
    #region References

    [SerializeField] protected Rigidbody rb;
    [SerializeField] TextMeshPro m_nickname;
    PhotonView m_PV;

    #endregion

    #region Knobs

    [SerializeField] protected float playerSpeed;

    #endregion

    #region RuntimeVariables

    [SerializeField] protected float playerInputHorizontal;
    [SerializeField] protected float playerInputForward;
    SpriteRenderer m_spriteRenderer;

    #endregion

    #region UnityMethods

    private void Start()
    {
        m_PV = GetComponent<PhotonView>();
        m_PV.Owner.NickName = PhotonNetwork.NickName; // NO PEDIRLO NUNCA MÁS DE UNA VEZ.
        gameObject.name = m_PV.Owner.NickName;
        PhotonPeer.RegisterType(typeof(Color), (byte)'C', TypeTransformer.SerializeColor, TypeTransformer.DeserializeColor);
    }

    private void FixedUpdate()
    {
        if (m_PV.IsMine)
        {
            playerInputHorizontal = Input.GetAxis("Horizontal");
            playerInputForward = Input.GetAxis("Vertical");
            rb.velocity = new Vector3(playerInputHorizontal, 0.0f, playerInputForward) * (playerSpeed) /** Time.fixedDeltaTime*/;
        }
    }

    #endregion

    #region LocalMethods

    void ShowNickname(string p_nickname)
    {
        if (m_PV.IsMine)
        {
            gameObject.GetComponentInChildren<TextMeshPro>().text = p_nickname;
        }
    }

    void setNewColorPlayer(Color p_newColor)
    {
        gameObject.GetComponent<SpriteRenderer>().color = p_newColor;

        if (m_PV.IsMine)
        {
            Hashtable playerProperties = new Hashtable();
            playerProperties["playerColor"] = p_newColor;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        }
    }

    // Esta es para que, cuando se llame a una función, lo que hacen estas funciones, se llaman automáticamente.
    // En este caso, esta se le manda a todos los que andan en la partida.
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!m_PV.IsMine && targetPlayer == m_PV.Owner)
        {
            setNewColorPlayer((Color)changedProps["playerColor"]);
        }
    }

    #endregion
}
