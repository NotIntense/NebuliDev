﻿namespace Nebuli.API.Features.Enum;

/// <summary>
/// Gets a list of possible player authentication types.
/// </summary>
public enum AuthenticationType
{
    /// <summary>
    /// A unknown authentication type.
    /// </summary>
    Unknown,

    /// <summary>
    /// Authenticated via Steam.
    /// </summary>
    Steam,

    /// <summary>
    /// Authenticated via Discord.
    /// </summary>
    Discord,

    /// <summary>
    /// Is a Northwood Studios staff member.
    /// </summary>
    Northwood,

    /// <summary>
    /// Is a localhost of the server.
    /// </summary>
    LocalHost,

    /// <summary>
    /// The authentication type given to the dedicated server.
    /// </summary>
    DedicatedServer
}
