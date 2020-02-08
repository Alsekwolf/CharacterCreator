using System;
using System.Collections.Generic;
using CharacterCreator.CommonFunctions;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using MenuAPI;
using static CharacterCreator.CommonFunctions.Functions;

namespace CharacterCreator
{
    public class MenuFunctions : BaseScript
    {

        private static bool MenuIsOpen { get; set; }
        private static Creator CreatorInstance { get; set; }
        internal static Menu CreatorMenu { get; set; }

        public MenuFunctions()
        {
            API.RegisterCommand("TestC", new Action<int, List<object>, string>( (int source, List<object> args, string rawCommand) =>
                {
                    CloseMenu();
                }), false);
        }
        
        public static async void OpenMenu(bool male, bool editPed = false)
        {
            IsEdidtingPed = editPed;
            isMalePed = male;
            if (male)
            {
                await SetPlayerSkin.SetPlayerSkinFunction("mp_m_freemode_01", new SetPlayerSkin.PedInfo() { version = -1 });
            }
            else
            {
                await SetPlayerSkin.SetPlayerSkinFunction("mp_f_freemode_01", new SetPlayerSkin.PedInfo() { version = -1 });
            }
            
            int maxHealth = Game.PlayerPed.MaxHealth;
            int maxArmour = Game.Player.MaxArmor;
            int health = Game.PlayerPed.Health;
            int armour = Game.PlayerPed.Armor;

            Game.Player.MaxArmor = maxArmour;
            Game.PlayerPed.MaxHealth = maxHealth;
            Game.PlayerPed.Health = health;
            Game.PlayerPed.Armor = armour;
            
            API.ClearPedDecorations(Game.PlayerPed.Handle);
            API.ClearPedFacialDecorations(Game.PlayerPed.Handle);
            API.SetPedDefaultComponentVariation(Game.PlayerPed.Handle);
            API.SetPedHairColor(Game.PlayerPed.Handle, 0, 0);
            API.SetPedEyeColor(Game.PlayerPed.Handle, 0);
            API.ClearAllPedProps(Game.PlayerPed.Handle);
            
            CreatorInstance = new Creator();
            CreatorMenu = CreatorInstance.GetMenu();
            MenuIsOpen = true;
            TickManger();
            //Tick += Functions.OnTick;

            EditingPed(male, editPed);
            Debug.Write("cookies r cool 2");
        }
        
        public static async void CloseMenu()
        {
            MenuIsOpen = false;
            var currentMenu = MenuController.GetCurrentMenu();
            if (IsOpen())
            {
                currentMenu.CloseMenu();
            }
        }
        
        private static async void TickManger()
        {
            while (MenuIsOpen)
            {
                await Functions.OnTick();
            }
        }
    }
}