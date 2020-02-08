using CharacterCreator.CommonFunctions;
using CharacterCreator.Menus;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using MenuAPI;
using static CharacterCreator.CommonFunctions.Functions;

namespace CharacterCreator
{
    public class MenuFunctions 
    {
        internal static bool MenuIsOpen { get; set; }
        private static Creator CreatorInstance { get; set; }
        internal static Menu CreatorMenu { get; set; }

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
            TickManger1();
            TickManger2();

            EditingPed(male, editPed);
        }
        
        public static void CloseMenu()
        {
            MenuIsOpen = false;
            var currentMenu = MenuController.GetCurrentMenu();
            //CameraFunctions.ClearCamera();
            if (IsOpen())
            {
                currentMenu.CloseMenu();
            }
        }
    }
}