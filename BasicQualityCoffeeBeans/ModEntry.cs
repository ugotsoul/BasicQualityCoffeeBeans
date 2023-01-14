using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace BasicQualityCoffeeBeans
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>The mod configuration from the player.</summary>
        private ModConfig Config;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {

            this.Config = this.Helper.ReadConfig<ModConfig>();
            helper.Events.Player.InventoryChanged += this.CheckItem;
            helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>

        private void CheckItem(object sender, InventoryChangedEventArgs e)
        {
            foreach (Item item in e.Added)
            {
                if (item.ParentSheetIndex == 433 && (item as StardewValley.Object).Quality > 0)
                {
                    this.Monitor.Log($"Picked up {item.DisplayName} item with quality of {(item as StardewValley.Object).quality}", LogLevel.Debug);

                    (item as StardewValley.Object).Quality = 0;
                }
            }
        }

        private void OnButtonsChanged(object sender, ButtonsChangedEventArgs e)
        {
            if (this.Config.ToggleKey.JustPressed())
            {
                // remove quality from all coffee beans in your items
                var coffeeBeansChanged = 0;

                Utility.iterateAllItems(item =>
                {
                    if (item.ParentSheetIndex == 433 && (item as StardewValley.Object).Quality > 0)
                    {
                        (item as StardewValley.Object).Quality = 0;
                        coffeeBeansChanged++;
                    }
                });

                if (coffeeBeansChanged > 0) 
                    this.Monitor.Log($"Changed quality of {coffeeBeansChanged} coffee beans into basic beans", LogLevel.Debug);
            }
        }

    }
}