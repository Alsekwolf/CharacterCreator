using System.Collections.Generic;
using System.Threading.Tasks;
using CharacterCreator.SubMenus;
using CitizenFX.Core;
using MenuAPI;
using static CitizenFX.Core.Native.API;

namespace CharacterCreator.CommonFunctions
{
    internal class Functions : BaseScript
    {
        public static bool IsEdidtingPed = false;
        public static bool isMalePed = false;
        
        public static bool DontCloseMenus { get { return MenuController.PreventExitingMenu; } set { MenuController.PreventExitingMenu = value; } }
        public static bool DisableBackButton { get { return MenuController.DisableBackButton; } set { MenuController.DisableBackButton = value; } }
        public static DataManager.MultiplayerPedData CurrentCharacter = new DataManager.MultiplayerPedData();

        public static void EditingPed(bool male, bool editPed = false)
        {
            if (!editPed)
            {
                CurrentCharacter = new DataManager.MultiplayerPedData();
                CurrentCharacter.DrawableVariations.clothes = new Dictionary<int, KeyValuePair<int, int>>();
                CurrentCharacter.PropVariations.props = new Dictionary<int, KeyValuePair<int, int>>();
                CurrentCharacter.PedHeadBlendData = Game.PlayerPed.GetHeadBlendData();
                CurrentCharacter.Version = 1;
                CurrentCharacter.ModelHash = male ? (uint)GetHashKey("mp_m_freemode_01") : (uint)GetHashKey("mp_f_freemode_01");
                CurrentCharacter.IsMale = male;

                SetPedComponentVariation(Game.PlayerPed.Handle, 3, 15, 0, 0);
                SetPedComponentVariation(Game.PlayerPed.Handle, 8, 15, 0, 0);
                SetPedComponentVariation(Game.PlayerPed.Handle, 11, 15, 0, 0);
            }
            if (CurrentCharacter.DrawableVariations.clothes == null)
            {
                CurrentCharacter.DrawableVariations.clothes = new Dictionary<int, KeyValuePair<int, int>>();
            }
            if (CurrentCharacter.PropVariations.props == null)
            {
                CurrentCharacter.PropVariations.props = new Dictionary<int, KeyValuePair<int, int>>();
            }

            //MainMenu.Appearance = new AppearanceMenu();
            //MainMenu.AppearanceMenu = MainMenu.Appearance.GetMenu(male, editPed);
            //AppearanceMenu.CreateMenu(male);

            //MainMenu.AppearanceMenu.RefreshIndex();
            //MainMenu.InheritanceMenu.RefreshIndex();
        }
        
        internal static bool IsOpen()
        {
            return
                MenuFunctions.CreatorMenu.Visible ||
                SubMenus.Inheritance.InheritanceMenu.Visible ||
                SubMenus.Appearance.AppearanceMenu.Visible;
        }
        
        public static async Task OnTick()
        {
            Debug.WriteLine("cookies r cool");
            await BaseScript.Delay(100);
            var currentMenu = MenuController.GetCurrentMenu();
            if (MenuFunctions.CreatorMenu != null)
            {
                if (IsOpen())
                {
                    if (currentMenu == MenuFunctions.CreatorMenu)
                    {
                        DisableBackButton = true;
                    }
                    else
                    {
                        DisableBackButton = false;
                    }

                    DontCloseMenus = true;
                }
                else
                {
                    DisableBackButton = false;
                    DontCloseMenus = false;
                }
            }
        }
    }
}