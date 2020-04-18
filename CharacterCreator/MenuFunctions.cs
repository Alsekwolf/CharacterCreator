using System.Threading.Tasks;
using CharacterCreator.CommonFunctions;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using MenuAPI;
using Newtonsoft.Json;
using static CharacterCreator.CommonFunctions.Functions;

namespace CharacterCreator
{
    public class MenuFunctions 
    {
        internal static bool MenuIsOperating { get; set; }
        internal static bool MenuIsOpen { get; set; }
        private static CreatorMenu CreatorInstance { get; set; }
        internal static Menu CharacterCreatorMenu { get; set; }
        
        internal static DataManager.MultiplayerPedData CurrentCharacter = new DataManager.MultiplayerPedData();
        
        public static async void PrepMenu(bool male, bool editPed = false, string loadCharacterJson = null)
        {
            CreatorInstance = new CreatorMenu();
            CharacterCreatorMenu = CreatorInstance.GetMenu();
            MenuIsOperating = true;
            TickManger1();
            TickManger2();
            
            if (editPed)
            {
                CurrentCharacter = JsonConvert.DeserializeObject<DataManager.MultiplayerPedData>(loadCharacterJson);

                await CreatorMenu.SpawnSavedPed(true);

                CreatorInstance.MakeCreateCharacterMenu(male: CurrentCharacter.IsMale, editPed: true);
            }
        }

        public static async Task LoadPed(bool restoreWeapons, string loadCharacterJson)
        {
            CurrentCharacter = JsonConvert.DeserializeObject<DataManager.MultiplayerPedData>(loadCharacterJson);
            
            await CreatorMenu.SpawnSavedPed(restoreWeapons);
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
                //CreatorMenu = CreatorInstance.GetMenu();
                MenuIsOpen = true;
                CharacterCreatorMenu.OpenMenu();
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
                //CreatorMenu = CreatorInstance.GetMenu();
                MenuIsOpen = false;    
                CharacterCreatorMenu.CloseMenu();
            }
        }

        public static void ClearCamera()
        {
            if (CameraFunctions.camera != null)
            {
                CameraFunctions.ClearCamera();
            }
        }

        public static void EndMenu()
        {
            MenuIsOperating = false;
            var currentMenu = MenuController.GetCurrentMenu();
            if (IsOpen())
            {
                currentMenu.CloseMenu();
            }
            
            BaseScript.TriggerEvent("CharacterCreator:MenuEnded");
        }
    }
}