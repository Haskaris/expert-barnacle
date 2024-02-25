using Godot;
using System;
using System.Linq;

public partial class MultiplayerController : Control
{
  [Export]
  private int _port = 8910;
  [Export]
  private string _ip = "127.0.0.1";
  [Export]
  private int _maxPlayerCount = 2;

  private ENetMultiplayerPeer _peer;

  public override void _Ready()
  {
    GDPrint.Print("<<< START SERVER >>>");
    Multiplayer.PeerConnected += PlayerConnected;
    Multiplayer.PeerDisconnected += PlayerDisconnected;

    HostGame();
    this.Hide();
  }

  private void HostGame()
  {
    _peer = new ENetMultiplayerPeer();
    var status = _peer.CreateServer(_port, _maxPlayerCount);
    if (status != Error.Ok)
    {
      GDPrint.PrintErr("Server could not be created:");
      GDPrint.PrintErr($"Port: {_port}");
      return;
    }

    _peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
    Multiplayer.MultiplayerPeer = _peer;
    GDPrint.Print("Server started SUCCESSFULLY.");
    GDPrint.Print("Waiting for players to connect ...");
  }

  private void PlayerConnected(long id)
  {
    GDPrint.Print($"Player <{id}> connected.");
  }

  private void PlayerDisconnected(long id)
  {
    GDPrint.Print($"Player <{id}> disconnected.");
  }
}