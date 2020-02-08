using System.Collections.Generic;
using System.Threading.Tasks;
using CharacterCreator.Menus;
using CitizenFX.Core;
using MenuAPI;
using static CitizenFX.Core.Native.API;

namespace CharacterCreator.CommonFunctions
{
    internal class Functions 
    {
        public static bool IsEdidtingPed = false;
        public static bool isMalePed = false;
        
        public static bool DontCloseMenus { get { return MenuController.PreventExitingMenu; } set { MenuController.PreventExitingMenu = value; } }
        public static bool DisableBackButton { get { return MenuController.DisableBackButton; } set { MenuController.DisableBackButton = value; } }
        public static DataManager.MultiplayerPedData CurrentCharacter = new DataManager.MultiplayerPedData();

        public static async void TickManger1()
        {
            while (MenuFunctions.MenuIsOpen)
            {
                await CameraFunctions.ManageCamera();
            }
        }
        public static async void TickManger2()
        {
            while (MenuFunctions.MenuIsOpen)
            {
                await Functions.OnTick();
            }
        }

        public static void EditingPed(bool male, bool editPed = false)
        {
            IsEdidtingPed = editPed;
            isMalePed = male;
            
            if (!editPed)
            {
                CurrentCharacter = new DataManager.MultiplayerPedData();
                CurrentCharacter.DrawableVariations.Clothes = new Dictionary<int, KeyValuePair<int, int>>();
                CurrentCharacter.PropVariations.Props = new Dictionary<int, KeyValuePair<int, int>>();
                CurrentCharacter.PedHeadBlendData = Game.PlayerPed.GetHeadBlendData();
                CurrentCharacter.Version = 1;
                CurrentCharacter.ModelHash = male ? (uint)GetHashKey("mp_m_freemode_01") : (uint)GetHashKey("mp_f_freemode_01");
                CurrentCharacter.IsMale = male;

                SetPedComponentVariation(Game.PlayerPed.Handle, 3, 15, 0, 0);
                SetPedComponentVariation(Game.PlayerPed.Handle, 8, 15, 0, 0);
                SetPedComponentVariation(Game.PlayerPed.Handle, 11, 15, 0, 0);
            }
            if (CurrentCharacter.DrawableVariations.Clothes == null)
            {
                CurrentCharacter.DrawableVariations.Clothes = new Dictionary<int, KeyValuePair<int, int>>();
            }
            if (CurrentCharacter.PropVariations.Props == null)
            {
                CurrentCharacter.PropVariations.Props = new Dictionary<int, KeyValuePair<int, int>>();
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
                Inheritance.InheritanceMenu.Visible ||
                Clothing.ClothingMenu.Visible ||
                FaceFeatures.FaceFeaturesMenu.Visible ||
                Props.PropsMenu.Visible ||
                Appearance.AppearanceMenu.Visible;
        }
        
        public static async Task OnTick()
        {
            await BaseScript.Delay(0);
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

                    #region DisableMovement

                    Game.DisableControlThisFrame(0, Control.MoveDown);
                    Game.DisableControlThisFrame(0, Control.MoveDownOnly);
                    Game.DisableControlThisFrame(0, Control.MoveLeft);
                    Game.DisableControlThisFrame(0, Control.MoveLeftOnly);
                    Game.DisableControlThisFrame(0, Control.MoveLeftRight);
                    Game.DisableControlThisFrame(0, Control.MoveRight);
                    Game.DisableControlThisFrame(0, Control.MoveRightOnly);
                    Game.DisableControlThisFrame(0, Control.MoveUp);
                    Game.DisableControlThisFrame(0, Control.MoveUpDown);
                    Game.DisableControlThisFrame(0, Control.MoveUpOnly);
                    Game.DisableControlThisFrame(0, Control.NextCamera);
                    Game.DisableControlThisFrame(0, Control.LookBehind);
                    Game.DisableControlThisFrame(0, Control.LookDown);
                    Game.DisableControlThisFrame(0, Control.LookDownOnly);
                    Game.DisableControlThisFrame(0, Control.LookLeft);
                    Game.DisableControlThisFrame(0, Control.LookLeftOnly);
                    Game.DisableControlThisFrame(0, Control.LookLeftRight);
                    Game.DisableControlThisFrame(0, Control.LookRight);
                    Game.DisableControlThisFrame(0, Control.LookRightOnly);
                    Game.DisableControlThisFrame(0, Control.LookUp);
                    Game.DisableControlThisFrame(0, Control.LookUpDown);
                    Game.DisableControlThisFrame(0, Control.LookUpOnly);
                    Game.DisableControlThisFrame(0, Control.Aim);
                    Game.DisableControlThisFrame(0, Control.AccurateAim);
                    Game.DisableControlThisFrame(0, Control.Cover);
                    Game.DisableControlThisFrame(0, Control.Duck);
                    Game.DisableControlThisFrame(0, Control.Jump);
                    Game.DisableControlThisFrame(0, Control.SelectNextWeapon);
                    Game.DisableControlThisFrame(0, Control.PrevWeapon);
                    Game.DisableControlThisFrame(0, Control.WeaponSpecial);
                    Game.DisableControlThisFrame(0, Control.WeaponSpecial2);
                    Game.DisableControlThisFrame(0, Control.WeaponWheelLeftRight);
                    Game.DisableControlThisFrame(0, Control.WeaponWheelNext);
                    Game.DisableControlThisFrame(0, Control.WeaponWheelPrev);
                    Game.DisableControlThisFrame(0, Control.WeaponWheelUpDown);
                    Game.DisableControlThisFrame(0, Control.VehicleExit);
                    Game.DisableControlThisFrame(0, Control.Enter);

                    #endregion
                    
                    DontCloseMenus = true;
                }
                else
                {
                    DisableBackButton = false;
                    DontCloseMenus = false;
                }
            }
        }
        
        internal static async Task ChangeGenderDialog(bool male)
        {
            bool confirm = false;
            AddTextEntry("msg1", "Are you sure you want to change gender?");
            AddTextEntry("msg2", "You will lose all customization!");
            MenuFunctions.CreatorMenu.CloseMenu();

            // wait for confirmation or cancel input.
            while (true)
            {
                await BaseScript.Delay(0);
                int unk = 1;
                int unk2 = 1;
                SetWarningMessage("msg1", 20, "msg2", true, 0, ref unk, ref unk2, true, 0);
                if (IsControlJustPressed(2, 201) || IsControlJustPressed(2, 217)) // continue/accept
                {
                    confirm = true;
                    break;
                }
                else if (IsControlJustPressed(2, 202)) // cancel
                {
                    break;
                }
            }
            // if confirmed, change gender to selected one.
            if (confirm)
            {
                while (IsControlPressed(2, 201) || IsControlPressed(2, 217) || IsDisabledControlPressed(2, 201) || IsDisabledControlPressed(2, 217))
                    await BaseScript.Delay(0);
                await BaseScript.Delay(100);

                if (male)
                {
                    await SetPlayerSkin.SetPlayerSkinFunction("mp_m_freemode_01", new SetPlayerSkin.PedInfo() { version = -1 });
                    
                    //SetPlayerModel(Game.PlayerPed.Handle, currentCharacter.ModelHash);
                    ClearPedDecorations(Game.PlayerPed.Handle);
                    ClearPedFacialDecorations(Game.PlayerPed.Handle);
                    SetPedDefaultComponentVariation(Game.PlayerPed.Handle);
                    SetPedHairColor(Game.PlayerPed.Handle, 0, 0);
                    SetPedEyeColor(Game.PlayerPed.Handle, 0);
                    ClearAllPedProps(Game.PlayerPed.Handle);
                    
                    EditingPed(true, false);
                    
                    Appearance.AppearanceMenu.ClearMenuItems();
                    Appearance.CreateMenu();
                }
                else
                {
                    await SetPlayerSkin.SetPlayerSkinFunction("mp_f_freemode_01", new SetPlayerSkin.PedInfo() { version = -1 });

                    //SetPlayerModel(Game.PlayerPed.Handle, currentCharacter.ModelHash);
                    ClearPedDecorations(Game.PlayerPed.Handle);
                    ClearPedFacialDecorations(Game.PlayerPed.Handle);
                    SetPedDefaultComponentVariation(Game.PlayerPed.Handle);
                    SetPedHairColor(Game.PlayerPed.Handle, 0, 0);
                    SetPedEyeColor(Game.PlayerPed.Handle, 0);
                    ClearAllPedProps(Game.PlayerPed.Handle);
                    
                    EditingPed(false, false);
                    
                    Appearance.AppearanceMenu.ClearMenuItems();
                    Appearance.CreateMenu();
                }
                
                MenuFunctions.CreatorMenu.OpenMenu();
            }
            else // otherwise cancel and go back to the editor.
            {
                MenuFunctions.CreatorMenu.OpenMenu();
            }
        }
    }
}