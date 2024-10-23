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

    [SerializeField, HideInInspector] protected Rigidbody rb;
    [SerializeField] TextMeshPro m_nickname;
    [SerializeField, HideInInspector] protected Animator m_myAnim;
    [SerializeField] Transform m_cam;
    PhotonView m_PV;

    #endregion

    #region Knobs

    [SerializeField] protected float playerSpeed;

    #endregion

    #region RuntimeVariables

    [SerializeField] protected float playerInputHorizontal;
    [SerializeField] protected float playerInputForward;
    SpriteRenderer m_spriteRenderer;
    Vector3 m_moveDirection;
    Vector3 m_moveDirWithCam;
    Quaternion m_rotation;
    float angle;

    #endregion

    #region UnityMethods

    private void Start()
    {
        m_PV = GetComponent<PhotonView>();
        //m_PV.Owner.NickName = PhotonNetwork.NickName; // NO PEDIRLO NUNCA MÁS DE UNA VEZ.
        m_nickname.text = m_PV.Owner.NickName;
        gameObject.name = m_PV.Owner.NickName;
        m_myAnim.SetBool("IsMoving", false);
        m_myAnim.SetBool("IsIdle", true);
        PhotonPeer.RegisterType(typeof(Color), (byte)'C', TypeTransformer.SerializeColor, TypeTransformer.DeserializeColor);
    }

    private void FixedUpdate()
    {
        //m_nickname.transform.position = new Vector3(transform.position.x, transform.position.y + 4.5f, transform.position.z);
        if (m_PV.IsMine)
        {
            playerInputHorizontal = Input.GetAxisRaw("Horizontal");
            playerInputForward = Input.GetAxisRaw("Vertical");
            m_moveDirection = new Vector3(playerInputHorizontal, 0, playerInputForward).normalized;
            //rb.velocity = m_moveDirection * (playerSpeed) /** Time.fixedDeltaTime*/;

            if(m_moveDirection.magnitude > 0.1f)
            {
                angle = Mathf.Atan2(m_moveDirection.x, m_moveDirection.z) * Mathf.Rad2Deg + m_cam.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0, angle, 0);
                m_myAnim.SetBool("IsMoving", true);
                m_myAnim.SetBool("IsIdle", false);
                m_myAnim.SetFloat("MovingFloat", m_moveDirection.magnitude);
            }
            else
            {
                //m_myAnim.SetFloat("MovingFloat", _MoveDirection.magnitude);
                m_myAnim.SetBool("IsMoving", false);
                m_myAnim.SetBool("IsIdle", true);
            }

            m_moveDirWithCam = Quaternion.Euler(0.0f, angle, 0.0f) * Vector3.forward;

            rb.Move(rb.position + playerSpeed * m_moveDirWithCam.normalized * Time.fixedDeltaTime * m_moveDirection.magnitude, Quaternion.Euler(0.0f, angle, 0.0f));
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
