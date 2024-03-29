﻿using InventorySystem.Items.Firearms;
using InventorySystem.Items.Pickups;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FirearmPickupBase = InventorySystem.Items.Firearms.FirearmPickup;

namespace Nebuli.API.Features.Items.Pickups;

/// <summary>
/// Represents a wrapper class for FirearmPickupBase.
/// </summary>
public class FirearmPickup : Pickup
{
    /// <summary>
    /// Gets the collection of <see cref="FirearmPickupBase"/> and their wrapper class <see cref="FirearmPickup"/>.
    /// </summary>
    public new static Dictionary<FirearmPickupBase, FirearmPickup> Dictionary = new();

    /// <summary>
    /// Gets the FirearmPickup base.
    /// </summary>
    public new FirearmPickupBase Base;

    /// <summary>
    /// Gets the PickupSyncInfo of the FirearmPickup.
    /// </summary>
    public new PickupSyncInfo Info => Base.Info;

    internal FirearmPickup(FirearmPickupBase pickupBase) : base(pickupBase)
    {
        Base = pickupBase;
        Dictionary.Add(Base, this);
    }

    /// <summary>
    /// Gets a collection of all the current FirearmPickups.
    /// </summary>
    public new static IEnumerable<FirearmPickup> Collection = Dictionary.Values;

    /// <summary>
    /// Gets a list of all the current FirearmPickups.
    /// </summary>
    public new static List<FirearmPickup> List = Collection.ToList();

    /// <summary>
    /// Gets the Transform of the FirearmPickup.
    /// </summary>
    public Transform Transform => Base.transform;

    /// <summary>
    /// Gets the Rigidbody of the FirearmPickup.
    /// </summary>
    public Rigidbody Rigidbody => Base.Rb;

    /// <summary>
    /// Gets or sets a value indicating whether the FirearmPickup is distributed or not.
    /// </summary>
    public bool Distributed
    {
        get => Base.Distributed;
        set => Base.Distributed = value;
    }

    /// <summary>
    /// Gets or sets the FirearmStatus of the FirearmPickup.
    /// </summary>
    public FirearmStatus FirearmStatus
    {
        get => Base.Status;
        set => Base.Status = value;
    }

    /// <summary>
    /// Gets or sets the amount of ammo for the FirearmPickup.
    /// </summary>
    public byte Ammo
    {
        get => Base.NetworkStatus.Ammo;
        set => Base.NetworkStatus = new(value, Base.NetworkStatus.Flags, Base.NetworkStatus.Attachments);
    }

    /// <summary>
    /// Gets the ItemType of the FirearmPickup.
    /// </summary>
    public new ItemType ItemType => Base.NetworkInfo.ItemId;
}