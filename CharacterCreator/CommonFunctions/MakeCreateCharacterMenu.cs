using System.Collections.Generic;
using System.Threading.Tasks;
using CharacterCreator.SubMenus;
using CitizenFX.Core;
using MenuAPI;
using static CitizenFX.Core.Native.API;

namespace CharacterCreator.CommonFunctions
{
    public class MakeCreateCharacterMenu : BaseScript
    {
        private static Creator CreatorInstance { get; set; }
        private static Menu CreatorMenu { get; set; }
        

        public static bool IsEdidtingPed = false;
        public static bool isMalePed = false;

        public async void SetupMain(bool male, bool editPed = false)
        {
            IsEdidtingPed = editPed;
            isMalePed = male;
            
            await SetPlayerSkin.SetPlayerSkinFunction("mp_m_freemode_01", new SetPlayerSkin.PedInfo() { version = -1 });
            
            ClearPedDecorations(Game.PlayerPed.Handle);
            ClearPedFacialDecorations(Game.PlayerPed.Handle);
            SetPedDefaultComponentVariation(Game.PlayerPed.Handle);
            SetPedHairColor(Game.PlayerPed.Handle, 0, 0);
            SetPedEyeColor(Game.PlayerPed.Handle, 0);
            ClearAllPedProps(Game.PlayerPed.Handle);
            
            CreatorInstance = new Creator();
            CreatorMenu = CreatorInstance.GetMenu();
            
            
            
            EditingPed(male, editPed);
        }
        
        private static void EditingPed(bool male, bool editPed = false)
        {
            if (!editPed)
            {
                Creator.CurrentCharacter = new DataManager.MultiplayerPedData();
                Creator.CurrentCharacter.DrawableVariations.clothes = new Dictionary<int, KeyValuePair<int, int>>();
                Creator.CurrentCharacter.PropVariations.props = new Dictionary<int, KeyValuePair<int, int>>();
                Creator.CurrentCharacter.PedHeadBlendData = Game.PlayerPed.GetHeadBlendData();
                Creator.CurrentCharacter.Version = 1;
                Creator.CurrentCharacter.ModelHash = male ? (uint)GetHashKey("mp_m_freemode_01") : (uint)GetHashKey("mp_f_freemode_01");
                Creator.CurrentCharacter.IsMale = male;

                SetPedComponentVariation(Game.PlayerPed.Handle, 3, 15, 0, 0);
                SetPedComponentVariation(Game.PlayerPed.Handle, 8, 15, 0, 0);
                SetPedComponentVariation(Game.PlayerPed.Handle, 11, 15, 0, 0);
            }
            if (Creator.CurrentCharacter.DrawableVariations.clothes == null)
            {
                Creator.CurrentCharacter.DrawableVariations.clothes = new Dictionary<int, KeyValuePair<int, int>>();
            }
            if (Creator.CurrentCharacter.PropVariations.props == null)
            {
                Creator.CurrentCharacter.PropVariations.props = new Dictionary<int, KeyValuePair<int, int>>();
            }

            //MainMenu.Appearance = new AppearanceMenu();
            //MainMenu.AppearanceMenu = MainMenu.Appearance.GetMenu(male, editPed);
            //AppearanceMenu.CreateMenu(male);

            //MainMenu.AppearanceMenu.RefreshIndex();
            //MainMenu.InheritanceMenu.RefreshIndex();
        }
        
        public static async Task OnTick()
        {
            await BaseScript.Delay(100);
            var currentMenu = MenuController.GetCurrentMenu();
            if (CreatorMenu != null)
            {
                bool IsOpen()
                {
                    return
                        CreatorMenu.Visible ||
                        Inheritance.InheritanceMenu.Visible;

                }
                if (IsOpen())
                {
                    if (currentMenu == CreatorMenu)
                    {
                        Creator.DisableBackButton = true;
                    }
                    else
                    {
                        Creator.DisableBackButton = false;
                    }

                    Creator.DontCloseMenus = true;
                }
                else
                {
                    Creator.DisableBackButton = false;
                    Creator.DontCloseMenus = false;
                }
            }
        }
    }
}