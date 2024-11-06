using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Unity.VisualScripting;
using ExitGames.Client.Photon.StructWrapping;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviourPunCallbacks, IOnEventCallback {
    #region References

    [SerializeField, HideInInspector] protected Rigidbody rb;
    //[SerializeField] TextMeshPro m_nickname;
    [SerializeField, HideInInspector] protected Animator m_myAnim;

    [SerializeField] Transform m_cam;
    [SerializeField] BoxCollider m_boxCollider;
    [SerializeField] GameObject m_triggerCollision;
    [SerializeField] ParticleSystem m_particleSystem;
    [SerializeField] GameObject m_particleGO;

    [Header("UI Player")]
    [SerializeField] TextMeshProUGUI m_currentRoleText;

    PhotonView m_PV;

    #endregion

    #region Knobs

    [SerializeField] protected float playerSpeed;

    #endregion

    #region RuntimeVariables

    [SerializeField] protected float playerInputHorizontal;
    [SerializeField] protected float playerInputForward;
    Vector3 m_moveDirection;
    Vector3 m_moveDirWithCam;
    float angle;
    [SerializeField] int m_life;
    //Player m_otherPlayer;

    #endregion

    #region UnityMethods

    private void Start()
    {
        m_PV = GetComponent<PhotonView>();
        //m_PV.Owner.NickName = PhotonNetwork.NickName; // NO PEDIRLO NUNCA MÁS DE UNA VEZ.
        //m_nickname.text = m_PV.Owner.NickName;
        gameObject.name = m_PV.Owner.NickName;
        m_myAnim.SetBool("IsMoving", false);
        m_myAnim.SetBool("IsIdle", true);
        PhotonPeer.RegisterType(typeof(Color), (byte)'C', TypeTransformer.SerializeColor, TypeTransformer.DeserializeColor);
        m_life = 1;
        m_boxCollider.enabled = false;
        m_triggerCollision.SetActive(true);
        m_particleGO.SetActive(false);
        m_particleSystem.Stop();
    }

    private void Update()
    {
        if (!m_PV.IsMine)
        {
            return;
        }
        //ActivateCollCor();
        //print("Live: " + m_life);

        /*if (LevelManager.instance.GetCurrentState == LevelManagerState.Playing)
        {
            GetNewGameplayRole() ;
        }*/
    }

    private void FixedUpdate()
    {
        if (!m_PV.IsMine)
        {
            return;
        }
        //m_nickname.transform.position = new Vector3(transform.position.x, transform.position.y + 4.5f, transform.position.z);
        PlayerMov();
    }

    private void OnTriggerStay(Collider p_other)
    {
        if (p_other.CompareTag("NPC") && Input.GetKey(KeyCode.E))
        {
            p_other.GetComponent<NPCMovement>().DestroyNPC();
            //PhotonNetwork.Destroy(p_other.gameObject);
        }
        else if (p_other.CompareTag("Damage"))
        {
            if (m_PV.IsMine)
            {
                print("Moriste");

                m_PV.RPC("TakingDamage", RpcTarget.All, 1);

                //p_other.GetComponent<PlayerController>().DestroyThisPlayer();
            }
            //m_otherPlayer = other.GetComponent<PhotonView>().Owner;
            //DamageOtherPlayer(m_otherPlayer);

            //if (m_PV.IsMine)
            //{

            //}

            //TakeDamageFunct();

            //TakingDamage(1);
        }
    }

    /*private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Damage")
        {
            if (m_PV.IsMine)
            {
                m_PV.RPC("TakingDamage", RpcTarget.All, 1);
            }
        }
    }*/

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    #endregion

    #region PublicMethods

    // Esta es para que, cuando se llame a una función, lo que hacen estas funciones, se llaman automáticamente.
    // En este caso, esta se le manda a todos los que andan en la partida.
    //public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    //{
    //    if (changedProps.ContainsKey("damage"))
    //    {
    //        //Modificar la vida del usuario actual.
    //        m_life -= (int)changedProps["damage"];
    //    }
    //}

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        switch (eventCode)
        {
            case 1: // Evento de asignación de roles.
                print("Ahhhhhhhhhh");
                GetNewGameplayRole();
                break;
            case 2:
                break;
            case 3:
                break;
        }

        /*if (eventCode == 1)
        {
            string data = (string)photonEvent.CustomData;
            //Hacer algo con el string

            GetNewGameplayRole();
        }*/
    }

    #endregion

    #region LocalMethods

    void PlayerMov()
    {
        if (m_PV.IsMine)
        {
            playerInputHorizontal = Input.GetAxisRaw("Horizontal");
            playerInputForward = Input.GetAxisRaw("Vertical");
            m_moveDirection = new Vector3(playerInputHorizontal, 0, playerInputForward).normalized;
            //rb.velocity = m_moveDirection * (playerSpeed) /** Time.fixedDeltaTime*/;

            if (m_moveDirection.magnitude > 0.1f)
            {
                angle = Mathf.Atan2(m_moveDirection.x, m_moveDirection.z) * Mathf.Rad2Deg + m_cam.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0, angle + 25.0f, 0);
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
            m_triggerCollision.transform.rotation = Quaternion.Euler(0, angle + 25.0f, 0);
            rb.Move(rb.position + playerSpeed * m_moveDirWithCam.normalized * Time.fixedDeltaTime * m_moveDirection.magnitude, Quaternion.Euler(0.0f, angle, 0.0f));
        }
    }

    void ShowNickname(string p_nickname)
    {
        if (m_PV.IsMine)
        {
            gameObject.GetComponentInChildren<TextMeshPro>().text = p_nickname;
        }
    }

    /*void setNewColorPlayer(Color p_newColor)
    {
        gameObject.GetComponent<SpriteRenderer>().color = p_newColor;

        if (m_PV.IsMine)
        {
            Hashtable playerProperties = new Hashtable();
            playerProperties["playerColor"] = p_newColor;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        }
    }*/

    /*void DamageOtherPlayer(Player p_otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Hashtable playerStats = new Hashtable();
            playerStats["damage"] = 1;
            p_otherPlayer.SetCustomProperties(playerStats);
        }
    }*/

    [PunRPC]
    void TakingDamage(int p_damage)
    {
        if (m_PV.IsMine)
        {
            print("Se murió");

            m_life -= p_damage;

            if (m_life <= 0)
            {
                //Destroy(gameObject);
                StartCoroutine(WaitForParticleSystem());
                //PhotonNetwork.LeaveRoom();
            }
        }
    }

    IEnumerator WaitForParticleSystem()
    {
        m_particleGO.SetActive(true);
        m_particleSystem.Play();
        yield return new WaitForSeconds(m_particleSystem.main.duration);
        PhotonNetwork.Destroy(gameObject);
    }

    void ActivateCollCor()
    {
        m_PV.RPC("ActivateDamageCollision", RpcTarget.AllBuffered);
    }

    void ActivateDamageCollision()
    {
        if (Input.GetKey(KeyCode.E))
        {
            //m_myAnim.SetBool("IsAttacking", true);
            m_triggerCollision.SetActive(true);
            //m_boxCollider.enabled = true;
        }
        else
        {
            //m_myAnim.SetBool("IsAttacking", false);
            m_triggerCollision.SetActive(false);
            //m_boxCollider.enabled = false;
        }
    }

    void GetNewGameplayRole()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Role", out object role))
        {
            //LevelManager.AssignRole();

            string m_newPlayerRole = role.ToString();

            switch (m_newPlayerRole)
            {
                case "INNOCENT":
                    //Soy inocente
                    m_currentRoleText.text = "Innocent";
                    m_currentRoleText.color = Color.blue;
                    break;
                case "TRAITOR":
                    //Soy una rata
                    m_currentRoleText.text = "Traitor";
                    m_currentRoleText.color = Color.red;
                    break;
            }
        }
    }

    #endregion
}
