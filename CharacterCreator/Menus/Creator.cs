using System.Threading.Tasks;
using CharacterCreator.CommonFunctions;
using CharacterCreator.SubMenus;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using MenuAPI;

namespace CharacterCreator
{
    public class Creator
    {
        public static Menu CreatorMenu;
        
        public static bool DontCloseMenus { get { return MenuController.PreventExitingMenu; } set { MenuController.PreventExitingMenu = value; } }
        public static bool DisableBackButton { get { return MenuController.DisableBackButton; } set { MenuController.DisableBackButton = value; } }
        public static DataManager.MultiplayerPedData currentCharacter = new DataManager.MultiplayerPedData();

        private void CreateMenu()
        {
            //debug code
            MenuController.MenuToggleKey = Control.SelectCharacterMichael;
            MenuController.EnableMenuToggleKeyOnController = false;
            // debug code
            
            CreatorMenu = new Menu("Create Character", "Create A New Character") { Visible = true };
            MenuController.AddMenu(CreatorMenu);

            
            #region InheritanceMenu
            Inheritance.CreateMenu();
            #endregion

            #region AppearanceMenu
            Appearance.CreateMenu();
            #endregion
            
            MenuItem saveButton = new MenuItem("Save Character", "Save your character.");
            MenuItem exitNoSave = new MenuItem("Exit Without Saving", "Are you sure? All unsaved work will be lost.");
            CreatorMenu.AddMenuItem(saveButton);
            CreatorMenu.AddMenuItem(exitNoSave);
            
            /*
             ########################################################
                                 Event handlers
             ########################################################
            */
            
            // handle button presses for the createCharacter menu.
            CreatorMenu.OnItemSelect += async (sender, item, index) =>
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

                        CreatorMenu.GoBack();
                    }
                }
                else if (item == exitNoSave) // exit without saving
                {
                    bool confirm = false;
                    AddTextEntry("warning_message_first_line", "Are you sure you want to exit the character creator?");
                    AddTextEntry("warning_message_second_line", "You will lose all (unsaved) customization!");
                    CreatorMenu.CloseMenu();

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
                        CreatorMenu.OpenMenu();
                    }
                }
                else if (item == Inheritance.InheritanceButton) // update the inheritance menu anytime it's opened to prevent some weird glitch where old data is used.
                {
                    var data = Game.PlayerPed.GetHeadBlendData();
                    Inheritance.inheritanceDads.ListIndex = data.FirstFaceShape;
                    Inheritance.inheritanceMoms.ListIndex = data.SecondFaceShape;
                    Inheritance.inheritanceShapeMix.Position = (int)(data.ParentFaceShapePercent * 10f);
                    Inheritance.inheritanceSkinMix.Position = (int)(data.ParentSkinTonePercent * 10f);
                    Inheritance.InheritanceMenu.RefreshIndex();
                }
            };
        }
        
        /// <summary>
        /// Create the menu if it doesn't exist, and then returns it.
        /// </summary>
        /// <returns>The Menu</returns>
        public Menu GetMenu()
        {
            if (CreatorMenu == null)
            {
                CreateMenu();
            }
            return CreatorMenu;
        }
        
        public async Task OnTick()
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