﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.BasicMessages;
using InventorySystem.Items.Firearms.Modules;
using PlayerRoles;
using RelativePositioning;
using Nebuli.API.Features.Player;
using Nebuli.API.Features.Enum;
using Nebuli.API.Extensions;
using Nebuli.API.Features.Structs;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Items.Firearms.Attachments.Components;

namespace Nebuli.API.Features.Items
{
    /// <summary>
    /// Represents a firearm item in the game.
    /// </summary>
    public class Firearm : Item
    {
        internal static readonly Dictionary<FirearmType, Firearm> TypeToFirearm = new();

        /// <summary>
        /// Gets the base firearm object.
        /// </summary>
        public new InventorySystem.Items.Firearms.Firearm Base { get; }

        internal static readonly Dictionary<FirearmType, uint> BaseCodes = new();
        internal static Dictionary<FirearmType, AttachmentIdentity[]> AvailableAttachmentsValue { get; } = new();

        public Firearm(InventorySystem.Items.Firearms.Firearm firearm) : base(firearm)
        {
            Base = firearm;
        }

        /// <summary>
        /// Gets the description type of the firearm.
        /// </summary>
        public ItemDescriptionType DescriptionType => Base.DescriptionType;

        /// <summary>
        /// Gets the type of the firearm.
        /// </summary>
        public FirearmType Type => Base.ItemTypeId.ToFirearmType();

        /// <summary>
        /// Gets the firearms max ammo.
        /// </summary>
        public byte MaxAmmo => Base.AmmoManagerModule.MaxAmmo;

        /// <summary>
        /// Gets the attachments of the firearm.
        /// </summary>
        public Attachment[] Attachments => Base.Attachments;

        /// <summary>
        /// Gets the base stats of the firearm.
        /// </summary>
        public FirearmBaseStats Stats => Base.BaseStats;

        /// <summary>
        /// Gets the ammo type of the firearm.
        /// </summary>
        public AmmoType AmmoType => Base.AmmoType.ToAmmoType();

        /// <summary>
        /// Gets the faction affiliation of the firearm.
        /// </summary>
        public Faction Faction => Base.FirearmAffiliation;

        /// <summary>
        /// Gets a value indicating whether the firearm is emitting light.
        /// </summary>
        public bool IsEmittingLight => Base.IsEmittingLight;

        /// <summary>
        /// Gets the status of the firearm.
        /// </summary>
        public FirearmStatus Status => Base.Status;

        /// <summary>
        /// Gets the action module of the firearm.
        /// </summary>
        public IActionModule ActionModule => Base.ActionModule;

        /// <summary>
        /// Gets the available attachments for each firearm type.
        /// </summary>
        public static IReadOnlyDictionary<FirearmType, AttachmentIdentity[]> AvailableAttachments => AvailableAttachmentsValue;

        /// <summary>
        /// Gets the base code of the firearm.
        /// </summary>
        public uint BaseCode => BaseCodes[Type];

        /// <summary>
        /// Gets the players firearms preferences.
        /// </summary>
        public static IReadOnlyDictionary<NebuliPlayer, Dictionary<FirearmType, AttachmentIdentity[]>> PlayerPreferences => AttachmentsServerHandler.PlayerPreferences.Where(kvp => kvp.Key != null).ToDictionary(kvp => NebuliPlayer.Get(kvp.Key), kvp => kvp.Value.ToDictionary(subKvp => subKvp.Key.ToFirearmType(), subKvp => subKvp.Key.ToFirearmType().GetAttachmentIdentifiers(subKvp.Value).ToArray()));


        /// <summary>
        /// Retrieves the attachment identities for enabled attachments of the firearm.
        /// </summary>
        public IEnumerable<AttachmentIdentity> AttachmentIdentities
        {
            get
            {
                foreach (Attachment attachment in Attachments.Where(att => att.IsEnabled))
                    yield return AvailableAttachments[Type].FirstOrDefault(att => att == attachment);
            }
        }

        /// <summary>
        /// Fires a shot from the firearm.
        /// </summary>
        public void Shoot()
        {
            if (Owner == null)
                return;
            InventorySystem.Items.Firearms.Firearm firearm = Owner.Inventory._curInstance as InventorySystem.Items.Firearms.Firearm;
            if (firearm == null)
                return;

            ShotMessage message = new()
            {
                ShooterCameraRotation = Owner.ReferenceHub.PlayerCameraReference.transform.rotation,
                ShooterPosition = new RelativePosition(Owner.ReferenceHub.PlayerCameraReference.transform.position),
                ShooterWeaponSerial = Owner.CurrentItem.Serial,
                TargetNetId = 0,
                TargetPosition = default,
                TargetRotation = Quaternion.identity,
            };

            Physics.Raycast(Owner.ReferenceHub.PlayerCameraReference.transform.position, Owner.ReferenceHub.PlayerCameraReference.transform.forward, out RaycastHit hit, 100f, StandardHitregBase.HitregMask);

            if (hit.transform && hit.transform.TryGetComponentInParent(out NetworkIdentity networkIdentity) && networkIdentity)
            {
                message.TargetNetId = networkIdentity.netId;
                message.TargetPosition = new RelativePosition(networkIdentity.transform.position);
                message.TargetRotation = networkIdentity.transform.rotation;
            }
            else if (hit.transform)
            {
                message.TargetPosition = new RelativePosition(hit.transform.position);
                message.TargetRotation = hit.transform.rotation;
            }
            FirearmBasicMessagesHandler.ServerShotReceived(Owner.ReferenceHub.connectionToClient, message);
        }

        /// <summary>
        /// Reloads the firearm.
        /// </summary>
        public void Reload()
        {
            if (Owner == null)
                return;
            InventorySystem.Items.Firearms.Firearm firearm = Owner.Inventory._curInstance as InventorySystem.Items.Firearms.Firearm;
            if (firearm == null)
                return;

            RequestMessage message = new(Serial, RequestType.Reload);
            FirearmBasicMessagesHandler.ServerRequestReceived(Owner.ReferenceHub.connectionToClient, message);
        }

        /// <summary>
        /// Toggles the flashlight attached to the firearm.
        /// </summary>
        public void ToggleFlashlight()
        {
            if (Owner == null)
                return;
            InventorySystem.Items.Firearms.Firearm firearm = Owner.Inventory._curInstance as InventorySystem.Items.Firearms.Firearm;
            if (firearm == null)
                return;

            RequestMessage message = new(Serial, RequestType.ToggleFlashlight);
            FirearmBasicMessagesHandler.ServerRequestReceived(Owner.ReferenceHub.connectionToClient, message);
        }

        /// <summary>
        /// Unloads the firearm.
        /// </summary>
        public void Unload()
        {
            if (Owner == null)
                return;
            InventorySystem.Items.Firearms.Firearm firearm = Owner.Inventory._curInstance as InventorySystem.Items.Firearms.Firearm;
            if (firearm == null)
                return;

            RequestMessage message = new(Serial, RequestType.Unload);
            FirearmBasicMessagesHandler.ServerRequestReceived(Owner.ReferenceHub.connectionToClient, message);
        }

        /// <summary>
        /// Sets the aiming down sight state of the firearm.
        /// </summary>
        /// <param name="shouldADS">True to aim down sight, false to stop aiming down sight.</param>
        public void SetAimDownSight(bool shouldADS)
        {
            if (Owner == null)
                return;
            InventorySystem.Items.Firearms.Firearm firearm = Owner.Inventory._curInstance as InventorySystem.Items.Firearms.Firearm;
            if (firearm == null)
                return;

            RequestMessage message = new(Serial, shouldADS ? RequestType.AdsIn : RequestType.AdsOut);
            FirearmBasicMessagesHandler.ServerRequestReceived(Owner.ReferenceHub.connectionToClient, message);
        }
    }
}

