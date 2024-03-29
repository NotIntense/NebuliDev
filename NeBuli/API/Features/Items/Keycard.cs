﻿
namespace Nebuli.API.Features.Items;

public class Keycard : Item
{
    /// <summary>
    /// Gets the keycards base.
    /// </summary>
    public new InventorySystem.Items.Keycards.KeycardItem Base { get; }
    internal Keycard(InventorySystem.Items.Keycards.KeycardItem itemBase) : base(itemBase)
    {
        Base = itemBase;
    }
}
