using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class NetworkEventHandler : NetworkBehaviour, INetworkRunnerCallbacks
{
    // Start is called before the first frame update
    [Networked]
    TickTimer startTimer { set; get; }

    private bool isGameStarted;
    private int player_count=0;

    private PlayerSpawnerPrototype prototype;
    void Start()
    {
        prototype = FindObjectOfType<PlayerSpawnerPrototype>();        
    }
    public override void FixedUpdateNetwork()
    {
        if (startTimer.Expired(Runner))
        {
            RPC_STARTGAME();
            startTimer = TickTimer.None;
        }
    }

    public void StartButton()
    {
        isGameStarted = true;
        startTimer = TickTimer.CreateFromSeconds(Runner,3);
        RPC_CREATEPLANES();

    }

    public void OnPlayerJoined(NetworkRunner runner, Fusion.PlayerRef player)
    {       
        player_count++;
        Debug.Log(player_count);
    }

    public void OnPlayerLeft(NetworkRunner runner,Fusion.PlayerRef player)
    {
        player_count--;
    }

    void OnGUI()
    {
       
        if (Runner==null) return;
        if(isGameStarted==false && (Runner.IsSharedModeMasterClient ||Runner.IsSinglePlayer))
        {
            GUI.TextArea(new Rect(0, 0, 100, 50), "Connected Players:" + player_count);
            if (GUI.Button(new Rect(100, 0, 100, 50), "Start")) 
            {
                StartButton();
            }
        }
        if(startTimer.Expired(Runner) == false && startTimer.IsRunning==true)
        {
            GUI.TextArea(new Rect(Screen.width/2-50 , Screen.height/2-25, 100, 50), "Game Starting in " + startTimer.RemainingTime(Runner));
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_CREATEPLANES()
    {
        var playerList = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in playerList)
        {
            var tempPlayerController = player.GetComponent<PlayerController>();
            if (tempPlayerController.HasStateAuthority)
            {
                //tempPlayerController.isGameStarted = true;
                tempPlayerController.PlaneController.GetComponentInChildren<PlaneController>().CreatePlane(5, 1f);
            }

        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_STARTGAME()
    {       
        var playerList = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in playerList)
        {            
            var tempPlayerController = player.GetComponent<PlayerController>();
            if (tempPlayerController.HasStateAuthority)
            {
                tempPlayerController.isGameStarted = true;
                //tempPlayerController.PlaneController.GetComponentInChildren<PlaneController>().CreatePlane(5, 1f);
            }
           
        }
    }




    // NOT USED CALLBACKS
    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }
}
