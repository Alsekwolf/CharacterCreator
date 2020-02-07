using System.Collections.Generic;
using System.Threading.Tasks;
using CharacterCreator.CommonFunctions;
using CharacterCreator.SubMenus;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using MenuAPI;

namespace CharacterCreator
{
    public class MainMenu
    {
        private Menu _mainMenu;
        public static InheritanceMenu Inheritance { get; private set; }
        public static Menu InheritanceMenu = null;
        
        public static AppearanceMenu Appearance { get; private set; }
        public static Menu AppearanceMenu = null;

        public static bool DontCloseMenus { get { return MenuController.PreventExitingMenu; } set { MenuController.PreventExitingMenu = value; } }
        public static bool DisableBackButton { get { return MenuController.DisableBackButton; } set { MenuController.DisableBackButton = value; } }
        public static DataManager.MultiplayerPedData currentCharacter = new DataManager.MultiplayerPedData();

        private void CreateMenu()
        {
            //debug code
            MenuController.MenuToggleKey = Control.SelectCharacterMichael;
            MenuController.EnableMenuToggleKeyOnController = false;
            // debug code
            
            _mainMenu = new Menu("Create Character", "Create A New Character") { Visible = true };
            MenuController.AddMenu(_mainMenu);

            
            #region InheritanceMenu
            Inheritance = new InheritanceMenu();
            InheritanceMenu = Inheritance.GetMenu();
            
            MenuItem inheritanceButton = new MenuItem("Character Inheritance", "Character inheritance options.");
            inheritanceButton.Label = "→→→";
            _mainMenu.AddMenuItem(inheritanceButton);
            MenuController.BindMenuItem(_mainMenu, InheritanceMenu, inheritanceButton);
            #endregion

            #region AppearanceMenu
            Appearance = new AppearanceMenu();
            AppearanceMenu = Appearance.GetMenu();
            
            MenuItem appearanceButton = new MenuItem("Character Appearance", "Character appearance options.");
            appearanceButton.Label = "→→→";
            _mainMenu.AddMenuItem(appearanceButton);
            MenuController.BindMenuItem(_mainMenu, AppearanceMenu, appearanceButton);
            #endregion
            
            MenuItem saveButton = new MenuItem("Save Character", "Save your character.");
            MenuItem exitNoSave = new MenuItem("Exit Without Saving", "Are you sure? All unsaved work will be lost.");
            _mainMenu.AddMenuItem(saveButton);
            _mainMenu.AddMenuItem(exitNoSave);
            
            /*
             ########################################################
                                 Event handlers
             ########################################################
            */
            
            // handle button presses for the createCharacter menu.
            _mainMenu.OnItemSelect += async (sender, item, index) =>
            {
                if (item == saveButton) // save ped
                {
                    if (await SavePed())
                    {
                        while (!MenuController.IsAnyMenuOpen())
                        {
                            await BaseScript.Delay(0);
                        }

                        while (IsControlPressed(2, 201) || IsControlPressed(2, 217) || IsDisabledControlPressed(2, 201) || IsDisabledControlPressed(2, 217))
                            await BaseScript.Delay(0);
                        await BaseScript.Delay(100);

                        _mainMenu.GoBack();
                    }
                }
                else if (item == exitNoSave) // exit without saving
                {
                    bool confirm = false;
                    AddTextEntry("warning_message_first_line", "Are you sure you want to exit the character creator?");
                    AddTextEntry("warning_message_second_line", "You will lose all (unsaved) customization!");
                    _mainMenu.CloseMenu();

                    // wait for confirmation or cancel input.
                    while (true)
                    {
                        await BaseScript.Delay(0);
                        int unk = 1;
                        int unk2 = 1;
                        SetWarningMessage("warning_message_first_line", 20, "warning_message_second_line", true, 0, ref unk, ref unk2, true, 0);
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

                    // if confirmed to discard changes quit the editor.
                    if (confirm)
                    {
                        while (IsControlPressed(2, 201) || IsControlPressed(2, 217) || IsDisabledControlPressed(2, 201) || IsDisabledControlPressed(2, 217))
                            await BaseScript.Delay(0);
                        await BaseScript.Delay(100);
                    }
                    else // otherwise cancel and go back to the editor.
                    {
                        await BaseScript.Delay(100);
                        _mainMenu.OpenMenu();
                    }
                }
                else if (item == inheritanceButton) // update the inheritance menu anytime it's opened to prevent some weird glitch where old data is used.
                {
                    var data = Game.PlayerPed.GetHeadBlendData();
                    SubMenus.InheritanceMenu.inheritanceDads.ListIndex = data.FirstFaceShape;
                    SubMenus.InheritanceMenu.inheritanceMoms.ListIndex = data.SecondFaceShape;
                    SubMenus.InheritanceMenu.inheritanceShapeMix.Position = (int)(data.ParentFaceShapePercent * 10f);
                    SubMenus.InheritanceMenu.inheritanceSkinMix.Position = (int)(data.ParentSkinTonePercent * 10f);
                    InheritanceMenu.RefreshIndex();
                }
            };
        }
        
        /// <summary>
        /// Create the menu if it doesn't exist, and then returns it.
        /// </summary>
        /// <returns>The Menu</returns>
        public Menu GetMenu()
        {
            if (_mainMenu == null)
            {
                CreateMenu();
            }
            return _mainMenu;
        }
        
        public async Task OnTick()
        {
            await BaseScript.Delay(500);
            var currentMenu = MenuController.GetCurrentMenu();
            if (_mainMenu != null)
            {
                bool IsOpen()
                {
                    return
                        _mainMenu.Visible ||
                        InheritanceMenu.Visible;

                }
                if (IsOpen())
                {
                    if (currentMenu == _mainMenu)
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

        /// <summary>
        /// Saves the mp character and quits the editor if successful.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> SavePed()
        {
            //TODO: this
            await BaseScript.Delay(1);
            return true;
        }
    }
}