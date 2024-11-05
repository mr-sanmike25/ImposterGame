using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;

public class LevelManager : MonoBehaviourPunCallbacks
{
    public static LevelManager instance;

    PhotonView m_PV;

    LevelManagerState m_currentState;

    #region UnityMethods
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            m_PV = GetComponent<PhotonView>();
        }
        else
        {
            Destroy(instance);
        }
    }
    private void Start()
    {
        m_PV = GetComponent<PhotonView>();
        SetLevelManagerState(LevelManagerState.Waiting);

        if (PhotonNetwork.IsMasterClient)
        {

        }
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    #endregion UnityMethods

    #region FSMMethods
    /// <summary>
    /// Inicializa el estado de Playing
    /// </summary>
    void Playing()
    {
        SetNewRoleEvent();
        AssignRole();
    }
    #endregion FSMMethods

    #region LocalMethods

    /// <summary>
    /// Levanta el evento cuando los jugadores estén listos para la partida
    /// </summary>
    private void SetNewRoleEvent()
    {
        byte m_ID = 1; //Código del evento 1 - 199
        object content = "Listo para la partida"; //Se puede descomponer cualquier cosa.
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(m_ID, content, raiseEventOptions, SendOptions.SendReliable);
    }
    #endregion LocalMethods

    #region PublicMethods

    public void SetLevelManagerState(LevelManagerState p_newState)
    {
        if (p_newState == m_currentState)
        {
            return;
        }
        m_currentState = p_newState;

        switch (m_currentState)
        {
            case LevelManagerState.None:
                break;
            case LevelManagerState.Waiting:
                break;
            case LevelManagerState.Playing:
                Playing();
                break;
        }
    }

    /*public LevelManagerState GetLevelManagerState()
    {
        return m_currentState;
    }*/

    //Falta asignar cuántos roles hay según el jugador
    public static void AssignRole()
    {
        print("Asignación de rol");
        Player[] m_playersArray = PhotonNetwork.PlayerList;
        GameplayRole[] m_gameplayRole = { GameplayRole.Innocent, GameplayRole.Traitor };

        m_gameplayRole = m_gameplayRole.OrderBy(x => Random.value).ToArray();

        for(int i = 0; i > m_playersArray.Length; ++i)
        {
            Hashtable m_playerProperties = new Hashtable();
            m_playerProperties["Role"] = m_gameplayRole[i % m_gameplayRole.Length].ToString();
            m_playersArray[i].SetCustomProperties(m_playerProperties);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 4)
        {
            SetLevelManagerState(LevelManagerState.Playing);
        }
    }

    #endregion PublicMethods

    #region Getters&Setters
    public LevelManagerState GetCurrentState
    {
        get { return m_currentState; }
    }
    #endregion Getters&Setters
}

#region Enums
public enum LevelManagerState
{
    None,
    Waiting,
    Playing
}

public enum GameplayRole
{
    Innocent,
    Traitor,
}
#endregion Enums