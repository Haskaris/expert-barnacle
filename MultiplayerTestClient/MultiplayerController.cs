using Godot;
using System;
using System.Linq;

public partial class MultiplayerController : Control
{
  [Export]
  private int _port = 8910;
  [Export]
  private string _ip = "127.0.0.1";

  private ENetMultiplayerPeer _peer;
  private int _playerId;

  public override void _Ready()
  {
    Multiplayer.PeerConnected += PlayerConnected;
    Multiplayer.PeerDisconnected += PlayerDisconnected;
    Multiplayer.ConnectedToServer += ConnectionSuccessful;
    Multiplayer.ConnectionFailed += ConnectionFailed;
  }

  private void ConnectionFailed()
  {
    GDPrint.Print("Connection FAILED.");
    GDPrint.Print("Could not connect to server.");
  }

  private void ConnectionSuccessful()
  {
    GDPrint.Print("Connection SUCCESSFUL.");

    _playerId = Multiplayer.GetUniqueId();

    GDPrint.Print(_playerId, "Sending player information to server.");
    GDPrint.Print(_playerId, $"Id: {_playerId}");

    RpcId(1, "SendPlayerInformation", _playerId);
  }

  private void PlayerConnected(long id)
  {
    GDPrint.Print(_playerId, $"Player <{id}> connected.");
  }

  private void PlayerDisconnected(long id)
  {
    GDPrint.Print(_playerId, $"Player <${id}> disconnected.");
  }

  public void ConnectToServer()
  {
    _peer = new ENetMultiplayerPeer();
    var status = _peer.CreateClient(_ip, _port);
    if (status != Error.Ok)
    {
      GDPrint.PrintErr("Creating client FAILED.");
      return;
    }

    _peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
    Multiplayer.MultiplayerPeer = _peer;
  }

  public void OnJoinPressed()
  {
    ConnectToServer();
  }
}
