using System.Collections.Generic;
using CharacterCreator.CommonFunctions;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using MenuAPI;

namespace CharacterCreator.SubMenus
{
    public class Appearance
    {
        public static readonly Menu AppearanceMenu = new Menu("Character Appearance", "Character Appearance Options");
        private static readonly MenuItem AppearanceButton = new MenuItem("Character Appearance", "Character appearance options.");

        public static void CreateMenu()
        {
            Debug.WriteLine("test1");
            var male = Functions.isMalePed;

            AppearanceMenu.ClearMenuItems();

            //Creating appearance Menu
            MenuController.AddMenu(AppearanceMenu);
            //Labels for buttons
            AppearanceButton.Label = "→→→";
            //adding button items
            Creator.CreatorMenu.AddMenuItem(AppearanceButton);
            //adding appearance as a sub menu to the main menu
            MenuController.BindMenuItem(Creator.CreatorMenu, AppearanceMenu, AppearanceButton);
            
            //MenuItem exitNoSave = new MenuItem("test1", "test2.");
            //_appearanceMenu.AddMenuItem(exitNoSave);
            
            Dictionary<int, KeyValuePair<string, string>> hairOverlays = new Dictionary<int, KeyValuePair<string, string>>()
            {
                { 0, new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_001_a") },
                { 1, new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_001_z") },
                { 2, new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_001_z") },
                { 3, new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_003_a") },
                { 4, new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_001_z") },
                { 5, new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_001_z") },
                { 6, new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_001_z") },
                { 7, new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_001_z") },
                { 8, new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_008_a") },
                { 9, new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_001_z") },
                { 10, new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_001_z") },
                { 11, new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_001_z") },
                { 12, new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_001_z") },
                { 13, new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_001_z") },
                { 14, new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_long_a") },
                { 15, new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_long_a") },
                { 16, new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_001_z") },
                { 17, new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_001_a") },
                { 18, new KeyValuePair<string, string>("mpbusiness_overlays", "FM_Bus_M_Hair_000_a") },
                { 19, new KeyValuePair<string, string>("mpbusiness_overlays", "FM_Bus_M_Hair_001_a") },
                { 20, new KeyValuePair<string, string>("mphipster_overlays", "FM_Hip_M_Hair_000_a") },
                { 21, new KeyValuePair<string, string>("mphipster_overlays", "FM_Hip_M_Hair_001_a") },
                { 22, new KeyValuePair<string, string>("multiplayer_overlays", "FM_M_Hair_001_a") },
            };
            
            #region appearance menu.
            List<string> opacityList = new List<string>() { "0%", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%" };

            List<string> overlayColorsList = new List<string>();
            for (int i = 0; i < GetNumHairColors(); i++)
            {
                overlayColorsList.Add($"Color #{i + 1}");
            }

            int maxHairStyles = GetNumberOfPedDrawableVariations(Game.PlayerPed.Handle, 2);
            //if (currentCharacter.ModelHash == (uint)PedHash.FreemodeFemale01)
            //{
            //    maxHairStyles /= 2;
            //}
            List<string> hairStylesList = new List<string>();
            for (int i = 0; i < maxHairStyles; i++)
            {
                hairStylesList.Add($"Style #{i + 1}");
            }
            hairStylesList.Add($"Style #{maxHairStyles + 1}");

            List<string> blemishesStyleList = new List<string>();
            for (int i = 0; i < GetNumHeadOverlayValues(0); i++)
            {
                blemishesStyleList.Add($"Style #{i + 1}");
            }

            List<string> beardStylesList = new List<string>();
            for (int i = 0; i < GetNumHeadOverlayValues(1); i++)
            {
                beardStylesList.Add($"Style #{i + 1}");
            }

            List<string> eyebrowsStyleList = new List<string>();
            for (int i = 0; i < GetNumHeadOverlayValues(2); i++)
            {
                eyebrowsStyleList.Add($"Style #{i + 1}");
            }

            List<string> ageingStyleList = new List<string>();
            for (int i = 0; i < GetNumHeadOverlayValues(3); i++)
            {
                ageingStyleList.Add($"Style #{i + 1}");
            }

            List<string> makeupStyleList = new List<string>();
            for (int i = 0; i < GetNumHeadOverlayValues(4); i++)
            {
                makeupStyleList.Add($"Style #{i + 1}");
            }

            List<string> blushStyleList = new List<string>();
            for (int i = 0; i < GetNumHeadOverlayValues(5); i++)
            {
                blushStyleList.Add($"Style #{i + 1}");
            }

            List<string> complexionStyleList = new List<string>();
            for (int i = 0; i < GetNumHeadOverlayValues(6); i++)
            {
                complexionStyleList.Add($"Style #{i + 1}");
            }

            List<string> sunDamageStyleList = new List<string>();
            for (int i = 0; i < GetNumHeadOverlayValues(7); i++)
            {
                sunDamageStyleList.Add($"Style #{i + 1}");
            }

            List<string> lipstickStyleList = new List<string>();
            for (int i = 0; i < GetNumHeadOverlayValues(8); i++)
            {
                lipstickStyleList.Add($"Style #{i + 1}");
            }

            List<string> molesFrecklesStyleList = new List<string>();
            for (int i = 0; i < GetNumHeadOverlayValues(9); i++)
            {
                molesFrecklesStyleList.Add($"Style #{i + 1}");
            }

            List<string> chestHairStyleList = new List<string>();
            for (int i = 0; i < GetNumHeadOverlayValues(10); i++)
            {
                chestHairStyleList.Add($"Style #{i + 1}");
            }

            List<string> bodyBlemishesList = new List<string>();
            for (int i = 0; i < GetNumHeadOverlayValues(11); i++)
            {
                bodyBlemishesList.Add($"Style #{i + 1}");
            }

            List<string> eyeColorList = new List<string>();
            for (int i = 0; i < 32; i++)
            {
                eyeColorList.Add($"Eye Color #{i + 1}");
            }

            /*

            0               Blemishes             0 - 23,   255  
            1               Facial Hair           0 - 28,   255  
            2               Eyebrows              0 - 33,   255  
            3               Ageing                0 - 14,   255  
            4               Makeup                0 - 74,   255  
            5               Blush                 0 - 6,    255  
            6               Complexion            0 - 11,   255  
            7               Sun Damage            0 - 10,   255  
            8               Lipstick              0 - 9,    255  
            9               Moles/Freckles        0 - 17,   255  
            10              Chest Hair            0 - 16,   255  
            11              Body Blemishes        0 - 11,   255  
            12              Add Body Blemishes    0 - 1,    255  
            
            */


            // hair
            int currentHairStyle = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.hairStyle : GetPedDrawableVariation(Game.PlayerPed.Handle, 2);
            int currentHairColor = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.hairColor : 0;
            int currentHairHighlightColor = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.hairHighlightColor : 0;

            // 0 blemishes
            int currentBlemishesStyle = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.blemishesStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 0) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 0) : 0;
            float currentBlemishesOpacity = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.blemishesOpacity : 0f;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 0, currentBlemishesStyle, currentBlemishesOpacity);

            // 1 beard
            int currentBeardStyle = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.beardStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 1) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 1) : 0;
            float currentBeardOpacity = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.beardOpacity : 0f;
            int currentBeardColor = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.beardColor : 0;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 1, currentBeardStyle, currentBeardOpacity);
            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 1, 1, currentBeardColor, currentBeardColor);

            // 2 eyebrows
            int currentEyebrowStyle = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.eyebrowsStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 2) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 2) : 0;
            float currentEyebrowOpacity = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.eyebrowsOpacity : 0f;
            int currentEyebrowColor = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.eyebrowsColor : 0;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 2, currentEyebrowStyle, currentEyebrowOpacity);
            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 2, 1, currentEyebrowColor, currentEyebrowColor);

            // 3 ageing
            int currentAgeingStyle = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.ageingStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 3) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 3) : 0;
            float currentAgeingOpacity = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.ageingOpacity : 0f;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 3, currentAgeingStyle, currentAgeingOpacity);

            // 4 makeup
            int currentMakeupStyle = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.makeupStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 4) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 4) : 0;
            float currentMakeupOpacity = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.makeupOpacity : 0f;
            int currentMakeupColor = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.makeupColor : 0;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 4, currentMakeupStyle, currentMakeupOpacity);
            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 4, 2, currentMakeupColor, currentMakeupColor);

            // 5 blush
            int currentBlushStyle = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.blushStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 5) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 5) : 0;
            float currentBlushOpacity = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.blushOpacity : 0f;
            int currentBlushColor = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.blushColor : 0;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 5, currentBlushStyle, currentBlushOpacity);
            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 5, 2, currentBlushColor, currentBlushColor);

            // 6 complexion
            int currentComplexionStyle = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.complexionStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 6) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 6) : 0;
            float currentComplexionOpacity = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.complexionOpacity : 0f;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 6, currentComplexionStyle, currentComplexionOpacity);

            // 7 sun damage
            int currentSunDamageStyle = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.sunDamageStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 7) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 7) : 0;
            float currentSunDamageOpacity = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.sunDamageOpacity : 0f;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 7, currentSunDamageStyle, currentSunDamageOpacity);

            // 8 lipstick
            int currentLipstickStyle = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.lipstickStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 8) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 8) : 0;
            float currentLipstickOpacity = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.lipstickOpacity : 0f;
            int currentLipstickColor = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.lipstickColor : 0;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 8, currentLipstickStyle, currentLipstickOpacity);
            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 8, 2, currentLipstickColor, currentLipstickColor);

            // 9 moles/freckles
            int currentMolesFrecklesStyle = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.molesFrecklesStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 9) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 9) : 0;
            float currentMolesFrecklesOpacity = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.molesFrecklesOpacity : 0f;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 9, currentMolesFrecklesStyle, currentMolesFrecklesOpacity);

            // 10 chest hair
            int currentChesthairStyle = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.chestHairStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 10) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 10) : 0;
            float currentChesthairOpacity = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.chestHairOpacity : 0f;
            int currentChesthairColor = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.chestHairColor : 0;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 10, currentChesthairStyle, currentChesthairOpacity);
            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 10, 1, currentChesthairColor, currentChesthairColor);

            // 11 body blemishes
            int currentBodyBlemishesStyle = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.bodyBlemishesStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 11) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 11) : 0;
            float currentBodyBlemishesOpacity = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.bodyBlemishesOpacity : 0f;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 11, currentBodyBlemishesStyle, currentBodyBlemishesOpacity);

            int currentEyeColor = Functions.IsEdidtingPed ? Functions.CurrentCharacter.PedAppearance.eyeColor : 0;
            SetPedEyeColor(Game.PlayerPed.Handle, currentEyeColor);

            MenuListItem hairStyles = new MenuListItem("Hair Style", hairStylesList, currentHairStyle, "Select a hair style.");
            //MenuListItem hairColors = new MenuListItem("Hair Color", overlayColorsList, currentHairColor, "Select a hair color.");
            MenuListItem hairColors = new MenuListItem("Hair Color", overlayColorsList, currentHairColor, "Select a hair color.") { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };
            //MenuListItem hairHighlightColors = new MenuListItem("Hair Highlight Color", overlayColorsList, currentHairHighlightColor, "Select a hair highlight color.");
            MenuListItem hairHighlightColors = new MenuListItem("Hair Highlight Color", overlayColorsList, currentHairHighlightColor, "Select a hair highlight color.") { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };

            MenuListItem blemishesStyle = new MenuListItem("Blemishes Style", blemishesStyleList, currentBlemishesStyle, "Select a blemishes style.");
            //MenuSliderItem blemishesOpacity = new MenuSliderItem("Blemishes Opacity", "Select a blemishes opacity.", 0, 10, (int)(currentBlemishesOpacity * 10f), false);
            MenuListItem blemishesOpacity = new MenuListItem("Blemishes Opacity", opacityList, (int)(currentBlemishesOpacity * 10f), "Select a blemishes opacity.") { ShowOpacityPanel = true };

            MenuListItem beardStyles = new MenuListItem("Beard Style", beardStylesList, currentBeardStyle, "Select a beard/facial hair style.");
            MenuListItem beardOpacity = new MenuListItem("Beard Opacity", opacityList, (int)(currentBeardOpacity * 10f), "Select the opacity for your beard/facial hair.") { ShowOpacityPanel = true };
            MenuListItem beardColor = new MenuListItem("Beard Color", overlayColorsList, currentBeardColor, "Select a beard color.") { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };
            //MenuSliderItem beardOpacity = new MenuSliderItem("Beard Opacity", "Select the opacity for your beard/facial hair.", 0, 10, (int)(currentBeardOpacity * 10f), false);
            //MenuListItem beardColor = new MenuListItem("Beard Color", overlayColorsList, currentBeardColor, "Select a beard color");

            MenuListItem eyebrowStyle = new MenuListItem("Eyebrows Style", eyebrowsStyleList, currentEyebrowStyle, "Select an eyebrows style.");
            MenuListItem eyebrowOpacity = new MenuListItem("Eyebrows Opacity", opacityList, (int)(currentEyebrowOpacity * 10f), "Select the opacity for your eyebrows.") { ShowOpacityPanel = true };
            MenuListItem eyebrowColor = new MenuListItem("Eyebrows Color", overlayColorsList, currentEyebrowColor, "Select an eyebrows color.") { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };
            //MenuSliderItem eyebrowOpacity = new MenuSliderItem("Eyebrows Opacity", "Select the opacity for your eyebrows.", 0, 10, (int)(currentEyebrowOpacity * 10f), false);

            MenuListItem ageingStyle = new MenuListItem("Ageing Style", ageingStyleList, currentAgeingStyle, "Select an ageing style.");
            MenuListItem ageingOpacity = new MenuListItem("Ageing Opacity", opacityList, (int)(currentAgeingOpacity * 10f), "Select an ageing opacity.") { ShowOpacityPanel = true };
            //MenuSliderItem ageingOpacity = new MenuSliderItem("Ageing Opacity", "Select an ageing opacity.", 0, 10, (int)(currentAgeingOpacity * 10f), false);

            MenuListItem makeupStyle = new MenuListItem("Makeup Style", makeupStyleList, currentMakeupStyle, "Select a makeup style.");
            MenuListItem makeupOpacity = new MenuListItem("Makeup Opacity", opacityList, (int)(currentMakeupOpacity * 10f), "Select a makeup opacity") { ShowOpacityPanel = true };
            //MenuSliderItem makeupOpacity = new MenuSliderItem("Makeup Opacity", 0, 10, (int)(currentMakeupOpacity * 10f), "Select a makeup opacity.");
            MenuListItem makeupColor = new MenuListItem("Makeup Color", overlayColorsList, currentMakeupColor, "Select a makeup color.") { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Makeup };

            MenuListItem blushStyle = new MenuListItem("Blush Style", blushStyleList, currentBlushStyle, "Select a blush style.");
            MenuListItem blushOpacity = new MenuListItem("Blush Opacity", opacityList, (int)(currentBlushOpacity * 10f), "Select a blush opacity.") { ShowOpacityPanel = true };
            //MenuSliderItem blushOpacity = new MenuSliderItem("Blush Opacity", 0, 10, (int)(currentBlushOpacity * 10f), "Select a blush opacity.");
            MenuListItem blushColor = new MenuListItem("Blush Color", overlayColorsList, currentBlushColor, "Select a blush color.") { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Makeup };

            MenuListItem complexionStyle = new MenuListItem("Complexion Style", complexionStyleList, currentComplexionStyle, "Select a complexion style.");
            //MenuSliderItem complexionOpacity = new MenuSliderItem("Complexion Opacity", 0, 10, (int)(currentComplexionOpacity * 10f), "Select a complexion opacity.");
            MenuListItem complexionOpacity = new MenuListItem("Complexion Opacity", opacityList, (int)(currentComplexionOpacity * 10f), "Select a complexion opacity.") { ShowOpacityPanel = true };

            MenuListItem sunDamageStyle = new MenuListItem("Sun Damage Style", sunDamageStyleList, currentSunDamageStyle, "Select a sun damage style.");
            //MenuSliderItem sunDamageOpacity = new MenuSliderItem("Sun Damage Opacity", 0, 10, (int)(currentSunDamageOpacity * 10f), "Select a sun damage opacity.");
            MenuListItem sunDamageOpacity = new MenuListItem("Sun Damage Opacity", opacityList, (int)(currentSunDamageOpacity * 10f), "Select a sun damage opacity.") { ShowOpacityPanel = true };

            MenuListItem lipstickStyle = new MenuListItem("Lipstick Style", lipstickStyleList, currentLipstickStyle, "Select a lipstick style.");
            //MenuSliderItem lipstickOpacity = new MenuSliderItem("Lipstick Opacity", 0, 10, (int)(currentLipstickOpacity * 10f), "Select a lipstick opacity.");
            MenuListItem lipstickOpacity = new MenuListItem("Lipstick Opacity", opacityList, (int)(currentLipstickOpacity * 10f), "Select a lipstick opacity.") { ShowOpacityPanel = true };
            MenuListItem lipstickColor = new MenuListItem("Lipstick Color", overlayColorsList, currentLipstickColor, "Select a lipstick color.") { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Makeup };

            MenuListItem molesFrecklesStyle = new MenuListItem("Moles and Freckles Style", molesFrecklesStyleList, currentMolesFrecklesStyle, "Select a moles and freckles style.");
            //MenuSliderItem molesFrecklesOpacity = new MenuSliderItem("Moles and Freckles Opacity", 0, 10, (int)(currentMolesFrecklesOpacity * 10f), "Select a moles and freckles opacity.");
            MenuListItem molesFrecklesOpacity = new MenuListItem("Moles and Freckles Opacity", opacityList, (int)(currentMolesFrecklesOpacity * 10f), "Select a moles and freckles opacity.") { ShowOpacityPanel = true };

            MenuListItem chestHairStyle = new MenuListItem("Chest Hair Style", chestHairStyleList, currentChesthairStyle, "Select a chest hair style.");
            //MenuSliderItem chestHairOpacity = new MenuSliderItem("Chest Hair Opacity", 0, 10, (int)(currentChesthairOpacity * 10f), "Select a chest hair opacity.");
            MenuListItem chestHairOpacity = new MenuListItem("Chest Hair Opacity", opacityList, (int)(currentChesthairOpacity * 10f), "Select a chest hair opacity.") { ShowOpacityPanel = true };
            MenuListItem chestHairColor = new MenuListItem("Chest Hair Color", overlayColorsList, currentChesthairColor, "Select a chest hair color.") { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };

            // Body blemishes
            MenuListItem bodyBlemishesStyle = new MenuListItem("Body Blemishes Style", bodyBlemishesList, currentBodyBlemishesStyle, "Select body blemishes style.");
            MenuListItem bodyBlemishesOpacity = new MenuListItem("Body Blemishes Opacity", opacityList, (int)(currentBodyBlemishesOpacity * 10f), "Select body blemishes opacity.") { ShowOpacityPanel = true };

            MenuListItem eyeColor = new MenuListItem("Eye Colors", eyeColorList, currentEyeColor, "Select an eye/contact lens color.");

            AppearanceMenu.AddMenuItem(hairStyles);
            AppearanceMenu.AddMenuItem(hairColors);
            AppearanceMenu.AddMenuItem(hairHighlightColors);

            AppearanceMenu.AddMenuItem(blemishesStyle);
            AppearanceMenu.AddMenuItem(blemishesOpacity);

            AppearanceMenu.AddMenuItem(beardStyles);
            AppearanceMenu.AddMenuItem(beardOpacity);
            AppearanceMenu.AddMenuItem(beardColor);

            AppearanceMenu.AddMenuItem(eyebrowStyle);
            AppearanceMenu.AddMenuItem(eyebrowOpacity);
            AppearanceMenu.AddMenuItem(eyebrowColor);

            AppearanceMenu.AddMenuItem(ageingStyle);
            AppearanceMenu.AddMenuItem(ageingOpacity);

            AppearanceMenu.AddMenuItem(makeupStyle);
            AppearanceMenu.AddMenuItem(makeupOpacity);
            AppearanceMenu.AddMenuItem(makeupColor);

            AppearanceMenu.AddMenuItem(blushStyle);
            AppearanceMenu.AddMenuItem(blushOpacity);
            AppearanceMenu.AddMenuItem(blushColor);

            AppearanceMenu.AddMenuItem(complexionStyle);
            AppearanceMenu.AddMenuItem(complexionOpacity);

            AppearanceMenu.AddMenuItem(sunDamageStyle);
            AppearanceMenu.AddMenuItem(sunDamageOpacity);

            AppearanceMenu.AddMenuItem(lipstickStyle);
            AppearanceMenu.AddMenuItem(lipstickOpacity);
            AppearanceMenu.AddMenuItem(lipstickColor);

            AppearanceMenu.AddMenuItem(molesFrecklesStyle);
            AppearanceMenu.AddMenuItem(molesFrecklesOpacity);

            AppearanceMenu.AddMenuItem(chestHairStyle);
            AppearanceMenu.AddMenuItem(chestHairOpacity);
            AppearanceMenu.AddMenuItem(chestHairColor);

            AppearanceMenu.AddMenuItem(bodyBlemishesStyle);
            AppearanceMenu.AddMenuItem(bodyBlemishesOpacity);

            AppearanceMenu.AddMenuItem(eyeColor);

            if (male)
            {
                // There are weird people out there that wanted makeup for male characters
                // so yeah.... here you go I suppose... strange...

                /*
                makeupStyle.Enabled = false;
                makeupStyle.LeftIcon = MenuItem.Icon.LOCK;
                makeupStyle.Description = "This is not available for male characters.";

                makeupOpacity.Enabled = false;
                makeupOpacity.LeftIcon = MenuItem.Icon.LOCK;
                makeupOpacity.Description = "This is not available for male characters.";

                makeupColor.Enabled = false;
                makeupColor.LeftIcon = MenuItem.Icon.LOCK;
                makeupColor.Description = "This is not available for male characters.";


                blushStyle.Enabled = false;
                blushStyle.LeftIcon = MenuItem.Icon.LOCK;
                blushStyle.Description = "This is not available for male characters.";

                blushOpacity.Enabled = false;
                blushOpacity.LeftIcon = MenuItem.Icon.LOCK;
                blushOpacity.Description = "This is not available for male characters.";

                blushColor.Enabled = false;
                blushColor.LeftIcon = MenuItem.Icon.LOCK;
                blushColor.Description = "This is not available for male characters.";


                lipstickStyle.Enabled = false;
                lipstickStyle.LeftIcon = MenuItem.Icon.LOCK;
                lipstickStyle.Description = "This is not available for male characters.";

                lipstickOpacity.Enabled = false;
                lipstickOpacity.LeftIcon = MenuItem.Icon.LOCK;
                lipstickOpacity.Description = "This is not available for male characters.";

                lipstickColor.Enabled = false;
                lipstickColor.LeftIcon = MenuItem.Icon.LOCK;
                lipstickColor.Description = "This is not available for male characters.";
                */
            }
            else
            {
                beardStyles.Enabled = false;
                beardStyles.LeftIcon = MenuItem.Icon.LOCK;
                beardStyles.Description = "This is not available for female characters.";

                beardOpacity.Enabled = false;
                beardOpacity.LeftIcon = MenuItem.Icon.LOCK;
                beardOpacity.Description = "This is not available for female characters.";

                beardColor.Enabled = false;
                beardColor.LeftIcon = MenuItem.Icon.LOCK;
                beardColor.Description = "This is not available for female characters.";


                chestHairStyle.Enabled = false;
                chestHairStyle.LeftIcon = MenuItem.Icon.LOCK;
                chestHairStyle.Description = "This is not available for female characters.";

                chestHairOpacity.Enabled = false;
                chestHairOpacity.LeftIcon = MenuItem.Icon.LOCK;
                chestHairOpacity.Description = "This is not available for female characters.";

                chestHairColor.Enabled = false;
                chestHairColor.LeftIcon = MenuItem.Icon.LOCK;
                chestHairColor.Description = "This is not available for female characters.";
            }

            #endregion

            // manage the list changes for appearance items.
            AppearanceMenu.OnListIndexChange += (_menu, listItem, oldSelectionIndex, newSelectionIndex, itemIndex) =>
            {
                if (itemIndex == 0) // hair style
                {
                    ClearPedFacialDecorations(Game.PlayerPed.Handle);
                    Functions.CurrentCharacter.PedAppearance.HairOverlay = new KeyValuePair<string, string>("", "");

                    if (newSelectionIndex >= GetNumberOfPedDrawableVariations(Game.PlayerPed.Handle, 2))
                    {
                        SetPedComponentVariation(Game.PlayerPed.Handle, 2, 0, 0, 0);
                        Functions.CurrentCharacter.PedAppearance.hairStyle = 0;
                    }
                    else
                    {
                        SetPedComponentVariation(Game.PlayerPed.Handle, 2, newSelectionIndex, 0, 0);
                        Functions.CurrentCharacter.PedAppearance.hairStyle = newSelectionIndex;
                        if (hairOverlays.ContainsKey(newSelectionIndex))
                        {
                            SetPedFacialDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(hairOverlays[newSelectionIndex].Key), (uint)GetHashKey(hairOverlays[newSelectionIndex].Value));
                            Functions.CurrentCharacter.PedAppearance.HairOverlay = new KeyValuePair<string, string>(hairOverlays[newSelectionIndex].Key, hairOverlays[newSelectionIndex].Value);
                        }
                    }
                }
                else if (itemIndex == 1 || itemIndex == 2) // hair colors
                {
                    var tmp = (MenuListItem)_menu.GetMenuItems()[1];
                    int hairColor = tmp.ListIndex;
                    tmp = (MenuListItem)_menu.GetMenuItems()[2];
                    int hairHighlightColor = tmp.ListIndex;

                    SetPedHairColor(Game.PlayerPed.Handle, hairColor, hairHighlightColor);

                    Functions.CurrentCharacter.PedAppearance.hairColor = hairColor;
                    Functions.CurrentCharacter.PedAppearance.hairHighlightColor = hairHighlightColor;
                }
                else if (itemIndex == 33) // eye color
                {
                    int selection = ((MenuListItem)_menu.GetMenuItems()[itemIndex]).ListIndex;
                    SetPedEyeColor(Game.PlayerPed.Handle, selection);
                    Functions.CurrentCharacter.PedAppearance.eyeColor = selection;
                }
                else
                {
                    int selection = ((MenuListItem)_menu.GetMenuItems()[itemIndex]).ListIndex;
                    float opacity = 0f;
                    if (_menu.GetMenuItems()[itemIndex + 1] is MenuListItem)
                        opacity = (((float)((MenuListItem)_menu.GetMenuItems()[itemIndex + 1]).ListIndex + 1) / 10f) - 0.1f;
                    else if (_menu.GetMenuItems()[itemIndex - 1] is MenuListItem)
                        opacity = (((float)((MenuListItem)_menu.GetMenuItems()[itemIndex - 1]).ListIndex + 1) / 10f) - 0.1f;
                    else if (_menu.GetMenuItems()[itemIndex] is MenuListItem)
                        opacity = (((float)((MenuListItem)_menu.GetMenuItems()[itemIndex]).ListIndex + 1) / 10f) - 0.1f;
                    else
                        opacity = 1f;
                    switch (itemIndex)
                    {
                        case 3: // blemishes
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 0, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.blemishesStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.blemishesOpacity = opacity;
                            break;
                        case 5: // beards
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 1, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.beardStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.beardOpacity = opacity;
                            break;
                        case 7: // beards color
                            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 1, 1, selection, selection);
                            Functions.CurrentCharacter.PedAppearance.beardColor = selection;
                            break;
                        case 8: // eyebrows
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 2, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.eyebrowsStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.eyebrowsOpacity = opacity;
                            break;
                        case 10: // eyebrows color
                            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 2, 1, selection, selection);
                            Functions.CurrentCharacter.PedAppearance.eyebrowsColor = selection;
                            break;
                        case 11: // ageing
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 3, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.ageingStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.ageingOpacity = opacity;
                            break;
                        case 13: // makeup
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 4, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.makeupStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.makeupOpacity = opacity;
                            break;
                        case 15: // makeup color
                            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 4, 2, selection, selection);
                            Functions.CurrentCharacter.PedAppearance.makeupColor = selection;
                            break;
                        case 16: // blush style
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 5, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.blushStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.blushOpacity = opacity;
                            break;
                        case 18: // blush color
                            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 5, 2, selection, selection);
                            Functions.CurrentCharacter.PedAppearance.blushColor = selection;
                            break;
                        case 19: // complexion
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 6, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.complexionStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.complexionOpacity = opacity;
                            break;
                        case 21: // sun damage
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 7, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.sunDamageStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.sunDamageOpacity = opacity;
                            break;
                        case 23: // lipstick
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 8, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.lipstickStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.lipstickOpacity = opacity;
                            break;
                        case 25: // lipstick color
                            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 8, 2, selection, selection);
                            Functions.CurrentCharacter.PedAppearance.lipstickColor = selection;
                            break;
                        case 26: // moles and freckles
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 9, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.molesFrecklesStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.molesFrecklesOpacity = opacity;
                            break;
                        case 28: // chest hair
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 10, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.chestHairStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.chestHairOpacity = opacity;
                            break;
                        case 30: // chest hair color
                            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 10, 1, selection, selection);
                            Functions.CurrentCharacter.PedAppearance.chestHairColor = selection;
                            break;
                        case 31: // body blemishes
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 11, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.bodyBlemishesStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.bodyBlemishesOpacity = opacity;
                            break;
                    }
                }
            };

            // manage the slider changes for opacity on the appearance items.
            AppearanceMenu.OnListIndexChange += (_menu, listItem, oldSelectionIndex, newSelectionIndex, itemIndex) =>
            {
                if (itemIndex > 2 && itemIndex < 33)
                {

                    int selection = ((MenuListItem)_menu.GetMenuItems()[itemIndex - 1]).ListIndex;
                    float opacity = 0f;
                    if (_menu.GetMenuItems()[itemIndex] is MenuListItem)
                        opacity = (((float)((MenuListItem)_menu.GetMenuItems()[itemIndex]).ListIndex + 1) / 10f) - 0.1f;
                    else if (_menu.GetMenuItems()[itemIndex + 1] is MenuListItem)
                        opacity = (((float)((MenuListItem)_menu.GetMenuItems()[itemIndex + 1]).ListIndex + 1) / 10f) - 0.1f;
                    else if (_menu.GetMenuItems()[itemIndex - 1] is MenuListItem)
                        opacity = (((float)((MenuListItem)_menu.GetMenuItems()[itemIndex - 1]).ListIndex + 1) / 10f) - 0.1f;
                    else
                        opacity = 1f;
                    switch (itemIndex)
                    {
                        case 4: // blemishes
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 0, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.blemishesStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.blemishesOpacity = opacity;
                            break;
                        case 6: // beards
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 1, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.beardStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.beardOpacity = opacity;
                            break;
                        case 9: // eyebrows
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 2, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.eyebrowsStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.eyebrowsOpacity = opacity;
                            break;
                        case 12: // ageing
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 3, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.ageingStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.ageingOpacity = opacity;
                            break;
                        case 14: // makeup
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 4, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.makeupStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.makeupOpacity = opacity;
                            break;
                        case 17: // blush style
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 5, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.blushStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.blushOpacity = opacity;
                            break;
                        case 20: // complexion
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 6, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.complexionStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.complexionOpacity = opacity;
                            break;
                        case 22: // sun damage
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 7, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.sunDamageStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.sunDamageOpacity = opacity;
                            break;
                        case 24: // lipstick
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 8, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.lipstickStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.lipstickOpacity = opacity;
                            break;
                        case 27: // moles and freckles
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 9, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.molesFrecklesStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.molesFrecklesOpacity = opacity;
                            break;
                        case 29: // chest hair
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 10, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.chestHairStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.chestHairOpacity = opacity;
                            break;
                        case 32: // body blemishes
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 11, selection, opacity);
                            Functions.CurrentCharacter.PedAppearance.bodyBlemishesStyle = selection;
                            Functions.CurrentCharacter.PedAppearance.bodyBlemishesOpacity = opacity;
                            break;
                    }
                }
            };
        }

        /// <summary>
        /// Create the menu if it doesn't exist, and then returns it.
        /// </summary>
        /// <returns>The Menu</returns>
        public Menu GetMenu()
        {
            if (AppearanceMenu == null)
            {
                CreateMenu();
            }
            return AppearanceMenu;
        }
    }
}