using System.Collections.Generic;
using System.Threading.Tasks;
using AlsekLibShared;
using CharacterCreator.CommonFunctions;
using CitizenFX.Core;
using MenuAPI;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;

namespace CharacterCreator.Menus
{
    internal class Creator
    {
        private void CreateMenu()
        {
            MenuController.EnableMenuToggleKeyOnKeyboard = false;
            MenuController.EnableMenuToggleKeyOnController = false;

            MenuFunctions.CreatorMenu = new Menu("Create Character", "Create A New Character") { Visible = false };
            MenuController.AddMenu(MenuFunctions.CreatorMenu);

            #region GenderSelection
            List<string> genderList = new List<string>() { "Male", "Female"};
            MenuListItem genderListItem = new MenuListItem("Gender Selection", genderList, 0, "Select which gender you want");
            MenuFunctions.CreatorMenu.AddMenuItem(genderListItem);
            #endregion
            
            #region InheritanceMenu
            Inheritance.CreateMenu();
            //Labels for buttons
            Inheritance.InheritanceButton.Label = "→→→";
            //adding button items
            MenuFunctions.CreatorMenu.AddMenuItem(Inheritance.InheritanceButton);
            //adding inheritance as a sub menu to the main menu
            MenuController.BindMenuItem(MenuFunctions.CreatorMenu, Inheritance.InheritanceMenu, Inheritance.InheritanceButton);
            #endregion
            
            #region FaceFeaturesMenu
            FaceFeatures.CreateMenu();
            //Labels for buttons
            FaceFeatures.FaceFeaturesButton.Label = "→→→";
            //adding button items
            MenuFunctions.CreatorMenu.AddMenuItem(FaceFeatures.FaceFeaturesButton);
            //adding face features as a sub menu to the main menu
            MenuController.BindMenuItem(MenuFunctions.CreatorMenu, FaceFeatures.FaceFeaturesMenu, FaceFeatures.FaceFeaturesButton);
            #endregion

            #region AppearanceMenu
            Appearance.CreateMenu();
            //Labels for buttons
            Appearance.AppearanceButton.Label = "→→→";
            //adding button items
            MenuFunctions.CreatorMenu.AddMenuItem(Appearance.AppearanceButton);
            //adding appearance as a sub menu to the main menu
            MenuController.BindMenuItem(MenuFunctions.CreatorMenu, Appearance.AppearanceMenu, Appearance.AppearanceButton);
            #endregion
            
            #region ClothingMenu
            Clothing.CreateMenu();
            //Labels for buttons
            Clothing.ClothingButton.Label = "→→→";
            //adding button items
            MenuFunctions.CreatorMenu.AddMenuItem(Clothing.ClothingButton);
            //adding clothing as a sub menu to the main menu
            MenuController.BindMenuItem(MenuFunctions.CreatorMenu, Clothing.ClothingMenu, Clothing.ClothingButton);
            #endregion
            
            #region PropsMenu
            Props.CreateMenu();
            //Labels for buttons
            Props.PropsButton.Label = "→→→";
            //adding button items
            MenuFunctions.CreatorMenu.AddMenuItem(Props.PropsButton);
            //adding props as a sub menu to the main menu
            MenuController.BindMenuItem(MenuFunctions.CreatorMenu, Props.PropsMenu, Props.PropsButton);
            #endregion
            
            MenuItem saveButton = new MenuItem("Save Character", "Save your character.");
            MenuItem exitNoSave = new MenuItem("Exit Without Saving", "Are you sure? All unsaved work will be lost.");
            MenuFunctions.CreatorMenu.AddMenuItem(saveButton);
            MenuFunctions.CreatorMenu.AddMenuItem(exitNoSave);
            
            /*
             ########################################################
                                 Event handlers
             ########################################################
            */
            
            // Handles events for gender selection
            #region GenderSelectionEvents
            MenuFunctions.CreatorMenu.OnListItemSelect += async (_menu, _listItem, _listIndex, _itemIndex) =>
            {
                if (_listIndex == 1)
                {
                    await Functions.ChangeGenderDialog(false);
                }
                
                if (_listIndex == 0)
                {
                    await Functions.ChangeGenderDialog(true);
                }
                
                // Code in here would get executed whenever a list item is pressed.
                DebugLog.Log($"OnListItemSelect: [{_menu}, {_listItem}, {_listIndex}, {_itemIndex}]", false, false, DebugLog.LogLevel.info);
            };
            #endregion
            
            // handle button presses for the createCharacter menu.
            MenuFunctions.CreatorMenu.OnItemSelect += async (sender, item, index) =>
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

                        //MenuFunctions.CreatorMenu.OpenMenu();
                        MenuFunctions.EndMenu();
                    }
                }
                else if (item == exitNoSave) // exit without saving
                {
                    bool confirm = false;
                    AddTextEntry("warning_message_first_line", "Are you sure you want to exit the character creator?");
                    AddTextEntry("warning_message_second_line", "You will lose all (unsaved) customization!");
                    MenuFunctions.CreatorMenu.CloseMenu();

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
                        MenuFunctions.EndMenu();
                    }
                    else // otherwise cancel and go back to the editor.
                    {
                        await BaseScript.Delay(100);
                        MenuFunctions.CreatorMenu.OpenMenu();
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
            if (MenuFunctions.CreatorMenu == null)
            {
                CreateMenu();
            }
            return MenuFunctions.CreatorMenu;
        }

        /// <summary>
        /// Saves the mp character and quits the editor if successful.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> SavePed()
        {
            await BaseScript.Delay(0);
            Functions.CurrentCharacter.PedHeadBlendData = Game.PlayerPed.GetHeadBlendData();
            string json = JsonConvert.SerializeObject(Functions.CurrentCharacter);
            
            BaseScript.TriggerServerEvent("CharacterCreator:SavePed", json);
            MenuFunctions.EndMenu();

            return true;
        }
    }
}