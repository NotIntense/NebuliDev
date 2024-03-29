﻿using CommandSystem;
using Nebuli.API.Features;
using Nebuli.Loader;
using System;

namespace Nebuli.API.Internal.Commands;

[CommandHandler(typeof(GameConsoleCommandHandler))]
public class ForceInstall : ICommand
{
    public string Command => "forceinstall";

    public string[] Aliases { get; } = new[] { "fi, ForceInstall" };

    public string Description => "Force installs Nebuli from a specific URL.";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        try
        {
            if (!Loader.Loader.Configuration.AllowExternalDownloadURLS)
            {
                response = "External URL downloading isnt enabled in the config!";
                return false;
            }

            Updater fi = new();
            fi.ForceInstall(arguments.Array[1]);
        }
        catch (Exception e)
        {
            Log.Error($"Error occured while force installing! Full error --> \n{e}");
            response = e.Message;
            return false;
        }
        response = null;
        return true;
    }
}
