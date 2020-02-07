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
        private static Creator Creator { get; set; }

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
            
            Creator = new Creator();
            var mainMenu = Creator.GetMenu();
            Tick += Creator.OnTick;
            
            await EditingPed(male, editPed);
        }
        
        private static async Task EditingPed(bool male, bool editPed = false)
        {
            await BaseScript.Delay(0);
            
            if (!editPed)
            {
                Creator.currentCharacter = new DataManager.MultiplayerPedData();
                Creator.currentCharacter.DrawableVariations.clothes = new Dictionary<int, KeyValuePair<int, int>>();
                Creator.currentCharacter.PropVariations.props = new Dictionary<int, KeyValuePair<int, int>>();
                Creator.currentCharacter.PedHeadBlendData = Game.PlayerPed.GetHeadBlendData();
                Creator.currentCharacter.Version = 1;
                Creator.currentCharacter.ModelHash = male ? (uint)GetHashKey("mp_m_freemode_01") : (uint)GetHashKey("mp_f_freemode_01");
                Creator.currentCharacter.IsMale = male;

                SetPedComponentVariation(Game.PlayerPed.Handle, 3, 15, 0, 0);
                SetPedComponentVariation(Game.PlayerPed.Handle, 8, 15, 0, 0);
                SetPedComponentVariation(Game.PlayerPed.Handle, 11, 15, 0, 0);
            }
            if (Creator.currentCharacter.DrawableVariations.clothes == null)
            {
                Creator.currentCharacter.DrawableVariations.clothes = new Dictionary<int, KeyValuePair<int, int>>();
            }
            if (Creator.currentCharacter.PropVariations.props == null)
            {
                Creator.currentCharacter.PropVariations.props = new Dictionary<int, KeyValuePair<int, int>>();
            }

            //MainMenu.Appearance = new AppearanceMenu();
            //MainMenu.AppearanceMenu = MainMenu.Appearance.GetMenu(male, editPed);
            //AppearanceMenu.CreateMenu(male);

            //MainMenu.AppearanceMenu.RefreshIndex();
            //MainMenu.InheritanceMenu.RefreshIndex();
        }
    }
}