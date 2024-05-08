﻿using System;
using DiscordRPC;
using DiscordRPC.Message;

namespace Pong.Game;

// The UWP class must have all the public methods as this class. This will be called from the shared project.
public class PlatformSpecific
{
    private const string ClientId = "1226792720207450143";
    
    public DiscordRpcClient Client { get; private set; }

    public PlatformSpecific()
    {
        Client = new DiscordRpcClient(ClientId)
        {
            SkipIdenticalPresence = true
        };

        Client.OnReady += RpcReady;

        Client.Initialize();
        UpdateRpc(0, 0, false);
    }

    private void RpcReady(object e, ReadyMessage message)
    {
        // This is to reset the current presence when the client resets, because SkipIdenticalPresence is true.
        if (Client.CurrentPresence != null)
            Client.SetPresence(null);
    }

    private void UpdateRpc(int scoreLeft, int scoreRight, bool gameStarted)
    {
        if (!Client.IsInitialized)
            return;

        var newPresence = new RichPresence
        {
            State = gameStarted ? "Playing Pong" : "Waiting for start",
            Details = $"Score: {scoreLeft} : {scoreRight}",
            Assets = new Assets()
            {
                LargeImageKey = "logo-large",
                LargeImageText = "Pong by Yesser Studios",
                SmallImageKey = "logo-small"
            }
        };

        Client.SetPresence(newPresence);
        Client.Invoke();
    }

    public void UpdateFinished(int scoreLeft, int scoreRight, bool gameStarted)
    {
        UpdateRpc(scoreLeft, scoreRight, gameStarted);
        Console.WriteLine($"Updated: {scoreLeft} : {scoreRight}; {gameStarted}");
    }

    public void Closing()
    {
        Client.Dispose();
        Console.WriteLine("Closing");
    }
}