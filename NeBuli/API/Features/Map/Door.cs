﻿using Interactables.Interobjects.DoorUtils;
using MapGeneration;
using Nebuli.Enum;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nebuli.API.Features.Map
{
    public class Door
    {
        public static readonly Dictionary<DoorVariant, Door> Dictionary = new();
        internal Door(DoorVariant door)
        {
            Base = door;
            Dictionary.Add(door, this);
        }

        public static IEnumerable<Door> Collection => Dictionary.Values;

        /// <summary>
        /// Gets a list of all the current doors on the server.
        /// </summary>
        public static List<Door> Doors => Collection.ToList();
        /// <summary>
        /// Gets the doors base.
        /// </summary>
        public DoorVariant Base { get; }

        /// <summary>
        /// Gets the doors GameObject.
        /// </summary>
        public GameObject GameObject => Base.gameObject;

        /// <summary>
        /// Gets the doors current position.
        /// </summary>
        public Vector3 Position => Base.transform.position;

        /// <summary>
        /// Determines if the door can be seen through. (UNKNOWN)
        /// </summary>
        public bool CanSeeThrough => Base.CanSeeThrough;

        /// <summary>
        /// Gets or sets whether the door is opened or not.
        /// </summary>
        public bool IsOpened
        {
            get => Base.NetworkTargetState;
            set => Base.NetworkTargetState = value;
        }

        /// <summary>
        /// Gets the doors current room.
        /// </summary>
        public Room CurrentRoom
        {
            get => Room.Get(Position);
        }

        /// <summary>
        /// Gets the doors current zone.
        /// </summary>
        public FacilityZone CurrentZone
        {
            get => Room.Get(Position).Zone;
        }

        /// <summary>
        /// Gets the doors active locks.
        /// </summary>
        public ushort ActiveLocks
        {
            get => Base.ActiveLocks; 
            set => Base.ActiveLocks = value;
        }

        /// <summary>
        /// Gets a door given the <see cref="DoorVariant"/>.
        /// </summary>
        /// <param name="doorVariant">The <see cref="DoorVariant"/> to use to find the door.</param>
        /// <returns></returns>
        public static Door Get(DoorVariant doorVariant)
        {
            return Dictionary.TryGetValue(doorVariant, out Door door) ? door : new Door(doorVariant);
        }
    }
}
