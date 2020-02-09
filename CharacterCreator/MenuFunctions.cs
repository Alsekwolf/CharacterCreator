using CharacterCreator.CommonFunctions;
using CharacterCreator.Menus;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using MenuAPI;
using Newtonsoft.Json;
using static CharacterCreator.CommonFunctions.Functions;

namespace CharacterCreator
{
    public partial class MenuFunctions 
    {
        internal static bool MenuIsOperating { get; set; }
        private static Creator CreatorInstance { get; set; }
        internal static Menu CreatorMenu { get; set; }

        public static async void PrepMenu(bool male, bool editPed = false, string currentCharacter = null)
        {
            IsEdidtingPed = editPed;
            if (!editPed)
            {
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
            }
            else
            {
                DataManager.MultiplayerPedData loadCharacter = JsonConvert.DeserializeObject<DataManager.MultiplayerPedData>(currentCharacter);
                isMalePed = loadCharacter.IsMale;
                Functions.CurrentCharacter = loadCharacter;
            }
            
            CreatorInstance = new Creator();
            CreatorMenu = CreatorInstance.GetMenu();
            MenuIsOperating = true;
            TickManger1();
            TickManger2();

            EditingPed(male, editPed);
        }

        public static async void OpenMenu()
        {
            await BaseScript.Delay(0);
            while (!MenuIsOperating)
            {
                await BaseScript.Delay(100);
            }

            if (MenuIsOperating)
            {
                CreatorMenu = CreatorInstance.GetMenu();
                CreatorMenu.OpenMenu();
            }
        }
        
        public static async void CloseMenu()
        {
            await BaseScript.Delay(0);
            while (!MenuIsOperating)
            {
                await BaseScript.Delay(100);
            }
            
            if (MenuIsOperating)
            {
                CreatorMenu = CreatorInstance.GetMenu();
                CreatorMenu.CloseMenu();
            }
        }

        public static void EndMenu()
        {
            MenuIsOperating = false;
            var currentMenu = MenuController.GetCurrentMenu();
            //CameraFunctions.ClearCamera();
            if (IsOpen())
            {
                currentMenu.CloseMenu();
            }
            BaseScript.TriggerEvent("CharacterCreator:MenuEnded");
        }
    }
}