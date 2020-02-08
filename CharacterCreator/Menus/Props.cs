using System.Collections.Generic;
using CharacterCreator.CommonFunctions;
using CitizenFX.Core;
using MenuAPI;
using static CitizenFX.Core.Native.API;

namespace CharacterCreator.Menus
{
    internal class Props
    {
        public static readonly Menu PropsMenu = new Menu("Character Props", "Character Prop Options");
        public static readonly MenuItem PropsButton = new MenuItem("Character Props", "Character prop options.");
        
        public static void CreateMenu()
        {
            //Creating props Menu
            MenuController.AddMenu(PropsMenu);
            
            #region props options menu
            string[] propNames = new string[5] { "Hats & Helmets", "Glasses", "Misc Props", "Watches", "Bracelets" };
            for (int x = 0; x < 5; x++)
            {
                int propId = x;
                if (x > 2)
                {
                    propId += 3;
                }

                int currentProp = Functions.IsEdidtingPed && Functions.CurrentCharacter.PropVariations.Props.ContainsKey(propId) ? Functions.CurrentCharacter.PropVariations.Props[propId].Key : GetPedPropIndex(Game.PlayerPed.Handle, propId);
                int currentPropTexture = Functions.IsEdidtingPed && Functions.CurrentCharacter.PropVariations.Props.ContainsKey(propId) ? Functions.CurrentCharacter.PropVariations.Props[propId].Value : GetPedPropTextureIndex(Game.PlayerPed.Handle, propId);

                List<string> propsList = new List<string>();
                for (int i = 0; i < GetNumberOfPedPropDrawableVariations(Game.PlayerPed.Handle, propId); i++)
                //for (int i = 0; i < 11; i++)
                {
                    propsList.Add($"Prop #{i} (of {GetNumberOfPedPropDrawableVariations(Game.PlayerPed.Handle, propId)})");
                }
                propsList.Add("No Prop");

                if (GetPedPropIndex(Game.PlayerPed.Handle, propId) != -1)
                {
                    int maxPropTextures = GetNumberOfPedPropTextureVariations(Game.PlayerPed.Handle, propId, currentProp);
                    MenuListItem propListItem = new MenuListItem($"{propNames[x]}", propsList, currentProp, $"Select a prop using the arrow keys and press ~o~enter~s~ to cycle through all available textures. Currently selected texture: #{currentPropTexture + 1} (of {maxPropTextures}).");
                    PropsMenu.AddMenuItem(propListItem);
                }
                else
                {
                    MenuListItem propListItem = new MenuListItem($"{propNames[x]}", propsList, currentProp, "Select a prop using the arrow keys and press ~o~enter~s~ to cycle through all available textures.");
                    PropsMenu.AddMenuItem(propListItem);
                }
            }
            #endregion
            
                        #region props
            PropsMenu.OnListIndexChange += (_menu, listItem, oldSelectionIndex, newSelectionIndex, realIndex) =>
            {
                int propIndex = realIndex;
                if (realIndex == 3)
                {
                    propIndex = 6;
                }
                if (realIndex == 4)
                {
                    propIndex = 7;
                }

                int textureIndex = 0;
                if (newSelectionIndex >= GetNumberOfPedPropDrawableVariations(Game.PlayerPed.Handle, propIndex))
                {
                    SetPedPropIndex(Game.PlayerPed.Handle, propIndex, -1, -1, false);
                    ClearPedProp(Game.PlayerPed.Handle, propIndex);
                    if (Functions.CurrentCharacter.PropVariations.Props == null)
                    {
                        Functions.CurrentCharacter.PropVariations.Props = new Dictionary<int, KeyValuePair<int, int>>();
                    }
                    Functions.CurrentCharacter.PropVariations.Props[propIndex] = new KeyValuePair<int, int>(-1, -1);
                    listItem.Description = $"Select a prop using the arrow keys and press ~o~enter~s~ to cycle through all available textures.";
                }
                else
                {
                    SetPedPropIndex(Game.PlayerPed.Handle, propIndex, newSelectionIndex, textureIndex, true);
                    if (Functions.CurrentCharacter.PropVariations.Props == null)
                    {
                        Functions.CurrentCharacter.PropVariations.Props = new Dictionary<int, KeyValuePair<int, int>>();
                    }
                    Functions.CurrentCharacter.PropVariations.Props[propIndex] = new KeyValuePair<int, int>(newSelectionIndex, textureIndex);
                    if (GetPedPropIndex(Game.PlayerPed.Handle, propIndex) == -1)
                    {
                        listItem.Description = $"Select a prop using the arrow keys and press ~o~enter~s~ to cycle through all available textures.";
                    }
                    else
                    {
                        int maxPropTextures = GetNumberOfPedPropTextureVariations(Game.PlayerPed.Handle, propIndex, newSelectionIndex);
                        listItem.Description = $"Select a prop using the arrow keys and press ~o~enter~s~ to cycle through all available textures. Currently selected texture: #{textureIndex + 1} (of {maxPropTextures}).";
                    }
                }
            };

            PropsMenu.OnListItemSelect += (sender, listItem, listIndex, realIndex) =>
            {
                int propIndex = realIndex;
                if (realIndex == 3)
                {
                    propIndex = 6;
                }
                if (realIndex == 4)
                {
                    propIndex = 7;
                }

                int textureIndex = GetPedPropTextureIndex(Game.PlayerPed.Handle, propIndex);
                int newTextureIndex = (GetNumberOfPedPropTextureVariations(Game.PlayerPed.Handle, propIndex, listIndex) - 1) < (textureIndex + 1) ? 0 : textureIndex + 1;
                if (textureIndex >= GetNumberOfPedPropDrawableVariations(Game.PlayerPed.Handle, propIndex))
                {
                    SetPedPropIndex(Game.PlayerPed.Handle, propIndex, -1, -1, false);
                    ClearPedProp(Game.PlayerPed.Handle, propIndex);
                    if (Functions.CurrentCharacter.PropVariations.Props == null)
                    {
                        Functions.CurrentCharacter.PropVariations.Props = new Dictionary<int, KeyValuePair<int, int>>();
                    }
                    Functions.CurrentCharacter.PropVariations.Props[propIndex] = new KeyValuePair<int, int>(-1, -1);
                    listItem.Description = $"Select a prop using the arrow keys and press ~o~enter~s~ to cycle through all available textures.";
                }
                else
                {
                    SetPedPropIndex(Game.PlayerPed.Handle, propIndex, listIndex, newTextureIndex, true);
                    if (Functions.CurrentCharacter.PropVariations.Props == null)
                    {
                        Functions.CurrentCharacter.PropVariations.Props = new Dictionary<int, KeyValuePair<int, int>>();
                    }
                    Functions.CurrentCharacter.PropVariations.Props[propIndex] = new KeyValuePair<int, int>(listIndex, newTextureIndex);
                    if (GetPedPropIndex(Game.PlayerPed.Handle, propIndex) == -1)
                    {
                        listItem.Description = $"Select a prop using the arrow keys and press ~o~enter~s~ to cycle through all available textures.";
                    }
                    else
                    {
                        int maxPropTextures = GetNumberOfPedPropTextureVariations(Game.PlayerPed.Handle, propIndex, listIndex);
                        listItem.Description = $"Select a prop using the arrow keys and press ~o~enter~s~ to cycle through all available textures. Currently selected texture: #{newTextureIndex + 1} (of {maxPropTextures}).";
                    }
                }
                //PropsMenu.UpdateScaleform();
            };
            #endregion

        }
    }
}