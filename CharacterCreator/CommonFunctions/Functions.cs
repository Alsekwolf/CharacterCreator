using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using MenuAPI;
using static CitizenFX.Core.Native.API;

namespace CharacterCreator.CommonFunctions
{
    internal class Functions 
    {
        public static bool DontCloseMenus { get { return MenuController.PreventExitingMenu; } set { MenuController.PreventExitingMenu = value; } }
        public static bool DisableBackButton { get { return MenuController.DisableBackButton; } set { MenuController.DisableBackButton = value; } }

        public static async void TickManger1()
        {
            while (MenuFunctions.MenuIsOperating)
            {
                await CameraFunctions.ManageCamera();
            }
        }
        public static async void TickManger2()
        {
            while (MenuFunctions.MenuIsOperating)
            {
                await Functions.OnTick();
            }
        }

        internal static bool IsOpen()
        {
            return
                MenuFunctions.CharacterCreatorMenu.Visible ||
                CreatorMenu.createCharacterMenu.Visible ||
                CreatorMenu.inheritanceMenu.Visible ||
                CreatorMenu.clothesMenu.Visible ||
                CreatorMenu.faceShapeMenu.Visible ||
                CreatorMenu.propsMenu.Visible ||
                CreatorMenu.tattoosMenu.Visible ||
                CreatorMenu.appearanceMenu.Visible;
        }
        
        public static async Task OnTick()
        {
            await BaseScript.Delay(0);
            var currentMenu = MenuController.GetCurrentMenu();
            if (MenuFunctions.CharacterCreatorMenu != null)
            {
                if (IsOpen())
                {
                    if (currentMenu == MenuFunctions.CharacterCreatorMenu || currentMenu == CreatorMenu.createCharacterMenu)
                    {
                        DisableBackButton = true;
                    }
                    else
                    {
                        DisableBackButton = false;
                    }

                    #region DisableMovement

                    if (MenuFunctions.MenuIsOpen)
                    {
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
                    }

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
    }
}