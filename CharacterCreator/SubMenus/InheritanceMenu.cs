using System.Collections.Generic;
using static CharacterCreator.MainMenu;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using MenuAPI;

namespace CharacterCreator.SubMenus
{
    public class InheritanceMenu
    {
        private Menu _inheritanceMenu;
        
        static List<string> parents = new List<string>();
        public static MenuListItem inheritanceDads = new MenuListItem("Father", parents, 0, "Select a father.");
        public static MenuListItem inheritanceMoms = new MenuListItem("Mother", parents, 0, "Select a mother.");
        static List<float> mixValues = new List<float>() { 0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f };
        public static MenuSliderItem inheritanceShapeMix = new MenuSliderItem("Head Shape Mix", "Select how much of your head shape should be inherited from your father or mother. All the way on the left is your dad, all the way on the right is your mom.", 0, 10, 5, true) { SliderLeftIcon = MenuItem.Icon.MALE, SliderRightIcon = MenuItem.Icon.FEMALE };
        public static MenuSliderItem inheritanceSkinMix = new MenuSliderItem("Body Skin Mix", "Select how much of your body skin tone should be inherited from your father or mother. All the way on the left is your dad, all the way on the right is your mom.", 0, 10, 5, true) { SliderLeftIcon = MenuItem.Icon.MALE, SliderRightIcon = MenuItem.Icon.FEMALE };
        
        private void CreateMenu()
        {
            _inheritanceMenu = new Menu("Character Inheritance", "Character Inheritance Options");
            
            for (int i = 0; i < 46; i++)
            {
                parents.Add($"#{i}");
            }
            
            _inheritanceMenu.AddMenuItem(inheritanceDads);
            _inheritanceMenu.AddMenuItem(inheritanceMoms);
            _inheritanceMenu.AddMenuItem(inheritanceShapeMix);
            _inheritanceMenu.AddMenuItem(inheritanceSkinMix);

            void SetHeadBlend()
            {
                SetPedHeadBlendData(Game.PlayerPed.Handle, inheritanceDads.ListIndex, inheritanceMoms.ListIndex, 0, inheritanceDads.ListIndex, inheritanceMoms.ListIndex, 0, mixValues[inheritanceShapeMix.Position], mixValues[inheritanceSkinMix.Position], 0f, false);
            }

            _inheritanceMenu.OnListIndexChange += (_menu, listItem, oldSelectionIndex, newSelectionIndex, itemIndex) =>
            {
                SetHeadBlend();
            };

            _inheritanceMenu.OnSliderPositionChange += (sender, item, oldPosition, newPosition, itemIndex) =>
            {
                SetHeadBlend();
            };
        }
        
        /// <summary>
        /// Create the menu if it doesn't exist, and then returns it.
        /// </summary>
        /// <returns>The Menu</returns>
        public Menu GetMenu()
        {
            if (_inheritanceMenu == null)
            {
                CreateMenu();
            }
            return _inheritanceMenu;
        }
    }
}