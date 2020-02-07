using System.Collections.Generic;
using static CharacterCreator.Creator;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using MenuAPI;

namespace CharacterCreator.SubMenus
{
    public class Inheritance
    {
        public static Menu InheritanceMenu;
        public static MenuItem InheritanceButton;
        
        static List<string> parents = new List<string>();
        public static MenuListItem inheritanceDads = new MenuListItem("Father", parents, 0, "Select a father.");
        public static MenuListItem inheritanceMoms = new MenuListItem("Mother", parents, 0, "Select a mother.");
        static List<float> mixValues = new List<float>() { 0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f };
        public static MenuSliderItem inheritanceShapeMix = new MenuSliderItem("Head Shape Mix", "Select how much of your head shape should be inherited from your father or mother. All the way on the left is your dad, all the way on the right is your mom.", 0, 10, 5, true) { SliderLeftIcon = MenuItem.Icon.MALE, SliderRightIcon = MenuItem.Icon.FEMALE };
        public static MenuSliderItem inheritanceSkinMix = new MenuSliderItem("Body Skin Mix", "Select how much of your body skin tone should be inherited from your father or mother. All the way on the left is your dad, all the way on the right is your mom.", 0, 10, 5, true) { SliderLeftIcon = MenuItem.Icon.MALE, SliderRightIcon = MenuItem.Icon.FEMALE };
        
        public static void CreateMenu()
        {
            InheritanceMenu = new Menu("Character Inheritance", "Character Inheritance Options");
            InheritanceButton = new MenuItem("Character Inheritance", "Character inheritance options.");
            
            //Creating inheritance Menu
            MenuController.AddMenu(InheritanceMenu);
            //Labels for buttons
            InheritanceButton.Label = "→→→";
            //adding button items
            CreatorMenu.AddMenuItem(InheritanceButton);
            //adding inheritance as a sub menu to the main menu
            MenuController.BindMenuItem(CreatorMenu, InheritanceMenu, InheritanceButton);
            
            for (int i = 0; i < 46; i++)
            {
                parents.Add($"#{i}");
            }
            
            InheritanceMenu.AddMenuItem(inheritanceDads);
            InheritanceMenu.AddMenuItem(inheritanceMoms);
            InheritanceMenu.AddMenuItem(inheritanceShapeMix);
            InheritanceMenu.AddMenuItem(inheritanceSkinMix);

            void SetHeadBlend()
            {
                SetPedHeadBlendData(Game.PlayerPed.Handle, inheritanceDads.ListIndex, inheritanceMoms.ListIndex, 0, inheritanceDads.ListIndex, inheritanceMoms.ListIndex, 0, mixValues[inheritanceShapeMix.Position], mixValues[inheritanceSkinMix.Position], 0f, false);
            }

            InheritanceMenu.OnListIndexChange += (_menu, listItem, oldSelectionIndex, newSelectionIndex, itemIndex) =>
            {
                SetHeadBlend();
            };

            InheritanceMenu.OnSliderPositionChange += (sender, item, oldPosition, newPosition, itemIndex) =>
            {
                SetHeadBlend();
            };
        }
    }
}