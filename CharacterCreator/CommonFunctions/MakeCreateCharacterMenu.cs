using System.Collections.Generic;
using CharacterCreator.SubMenus;
using CitizenFX.Core;
using MenuAPI;
using static CitizenFX.Core.Native.API;

namespace CharacterCreator.CommonFunctions
{
    public class MakeCreateCharacterMenu
    {
        private static bool isEdidtingPed = false;

        public static void Main(bool male, bool editPed = false)
        {
            isEdidtingPed = editPed;
            if (!editPed)
            {
                MainMenu.currentCharacter = new DataManager.MultiplayerPedData();
                MainMenu.currentCharacter.DrawableVariations.clothes = new Dictionary<int, KeyValuePair<int, int>>();
                MainMenu.currentCharacter.PropVariations.props = new Dictionary<int, KeyValuePair<int, int>>();
                MainMenu.currentCharacter.PedHeadBlendData = Game.PlayerPed.GetHeadBlendData();
                MainMenu.currentCharacter.Version = 1;
                MainMenu.currentCharacter.ModelHash = male ? (uint)GetHashKey("mp_m_freemode_01") : (uint)GetHashKey("mp_f_freemode_01");
                MainMenu.currentCharacter.IsMale = male;

                SetPedComponentVariation(Game.PlayerPed.Handle, 3, 15, 0, 0);
                SetPedComponentVariation(Game.PlayerPed.Handle, 8, 15, 0, 0);
                SetPedComponentVariation(Game.PlayerPed.Handle, 11, 15, 0, 0);
            }
            if (MainMenu.currentCharacter.DrawableVariations.clothes == null)
            {
                MainMenu.currentCharacter.DrawableVariations.clothes = new Dictionary<int, KeyValuePair<int, int>>();
            }
            if (MainMenu.currentCharacter.PropVariations.props == null)
            {
                MainMenu.currentCharacter.PropVariations.props = new Dictionary<int, KeyValuePair<int, int>>();
            }

            AppearanceMenu.SetupMenu(male, editPed);

            MainMenu.AppearanceMenu.RefreshIndex();
            MainMenu.InheritanceMenu.RefreshIndex();
        }
    }
}