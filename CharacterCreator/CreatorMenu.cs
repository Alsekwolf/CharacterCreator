using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlsekLib;
using CharacterCreator.CommonFunctions;
using CitizenFX.Core;
using MenuAPI;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;

namespace CharacterCreator
{
    internal class CreatorMenu
    {
        // Variables
        public static Menu createCharacterMenu = new Menu("Create Character", "Create A New Character");
        public static Menu inheritanceMenu = new Menu($"{Game.Player.Name}", "Character Inheritance Options");
        public static Menu appearanceMenu = new Menu($"{Game.Player.Name}", "Character Appearance Options");
        public static Menu faceShapeMenu = new Menu($"{Game.Player.Name}", "Character Face Shape Options");
        public static Menu tattoosMenu = new Menu($"{Game.Player.Name}", "Character Tattoo Options");
        public static Menu clothesMenu = new Menu($"{Game.Player.Name}", "Character Clothing Options");
        public static Menu propsMenu = new Menu($"{Game.Player.Name}", "Character Props Options");
        
        // Need to be able to disable/enable these buttons from another class.
        public MenuItem createMaleBtn = new MenuItem("Create Male Character", "Create a new male character.") { Label = "→→→" };
        public MenuItem createFemaleBtn = new MenuItem("Create Female Character", "Create a new female character.") { Label = "→→→" };
        public MenuItem exitBtn = new MenuItem("Exit Creator", "Completely exits the creator");

        private bool isEdidtingPed = false;
        private static readonly List<string> facial_expressions = new List<string>() { "mood_Normal_1", "mood_Happy_1", "mood_Angry_1", "mood_Aiming_1", "mood_Injured_1", "mood_stressed_1", "mood_smug_1", "mood_sulk_1", };
        
        /// <summary>
        /// Makes or updates the character creator menu. Also has an option to load data from the <see cref="currentCharacter"/> data, to allow for editing an existing ped.
        /// </summary>
        /// <param name="male"></param>
        /// <param name="editPed"></param>
        public void MakeCreateCharacterMenu(bool male, bool editPed = false)
        {
            isEdidtingPed = editPed;
            if (!editPed)
            {
                MenuFunctions.CurrentCharacter = new DataManager.MultiplayerPedData();
                MenuFunctions.CurrentCharacter.DrawableVariations.clothes = new Dictionary<int, KeyValuePair<int, int>>();
                MenuFunctions.CurrentCharacter.PropVariations.props = new Dictionary<int, KeyValuePair<int, int>>();
                MenuFunctions.CurrentCharacter.PedHeadBlendData = Game.PlayerPed.GetHeadBlendData();
                MenuFunctions.CurrentCharacter.Version = 1;
                MenuFunctions.CurrentCharacter.ModelHash = male ? (uint)GetHashKey("mp_m_freemode_01") : (uint)GetHashKey("mp_f_freemode_01");
                MenuFunctions.CurrentCharacter.IsMale = male;

                SetPedComponentVariation(Game.PlayerPed.Handle, 3, 15, 0, 0);
                SetPedComponentVariation(Game.PlayerPed.Handle, 8, 15, 0, 0);
                SetPedComponentVariation(Game.PlayerPed.Handle, 11, 15, 0, 0);
            }
            if (MenuFunctions.CurrentCharacter.DrawableVariations.clothes == null)
            {
                MenuFunctions.CurrentCharacter.DrawableVariations.clothes = new Dictionary<int, KeyValuePair<int, int>>();
            }
            if (MenuFunctions.CurrentCharacter.PropVariations.props == null)
            {
                MenuFunctions.CurrentCharacter.PropVariations.props = new Dictionary<int, KeyValuePair<int, int>>();
            }

            // Set the facial expression to default in case it doesn't exist yet, or keep the current one if it does.
            MenuFunctions.CurrentCharacter.FacialExpression = MenuFunctions.CurrentCharacter.FacialExpression ?? facial_expressions[0];

            // Set the facial expression on the ped itself.
            SetFacialIdleAnimOverride(Game.PlayerPed.Handle, MenuFunctions.CurrentCharacter.FacialExpression ?? facial_expressions[0], null);

            // Set the facial expression item list to the correct saved index.
            if (createCharacterMenu.GetMenuItems().ElementAt(6) is MenuListItem li)
            {
                int index = facial_expressions.IndexOf(MenuFunctions.CurrentCharacter.FacialExpression ?? facial_expressions[0]);
                if (index < 0)
                {
                    index = 0;
                }
                li.ListIndex = index;
            }

            appearanceMenu.ClearMenuItems();
            tattoosMenu.ClearMenuItems();
            clothesMenu.ClearMenuItems();
            propsMenu.ClearMenuItems();

            #region appearance menu.
            List<string> opacity = new List<string>() { "0%", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%" };

            List<string> overlayColorsList = new List<string>();
            for (int i = 0; i < GetNumHairColors(); i++)
            {
                overlayColorsList.Add($"Color #{i + 1}");
            }

            int maxHairStyles = GetNumberOfPedDrawableVariations(Game.PlayerPed.Handle, 2);
            //if (MenuFunctions.CurrentCharacter.ModelHash == (uint)PedHash.FreemodeFemale01)
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
            int currentHairStyle = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.HairStyle : GetPedDrawableVariation(Game.PlayerPed.Handle, 2);
            int currentHairColor = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.HairColor : 0;
            int currentHairHighlightColor = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.HairHighlightColor : 0;

            // 0 blemishes
            int currentBlemishesStyle = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.BlemishesStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 0) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 0) : 0;
            float currentBlemishesOpacity = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.BlemishesOpacity : 0f;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 0, currentBlemishesStyle, currentBlemishesOpacity);

            // 1 beard
            int currentBeardStyle = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.BeardStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 1) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 1) : 0;
            float currentBeardOpacity = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.BeardOpacity : 0f;
            int currentBeardColor = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.BeardColor : 0;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 1, currentBeardStyle, currentBeardOpacity);
            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 1, 1, currentBeardColor, currentBeardColor);

            // 2 eyebrows
            int currentEyebrowStyle = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.EyebrowsStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 2) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 2) : 0;
            float currentEyebrowOpacity = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.EyebrowsOpacity : 0f;
            int currentEyebrowColor = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.EyebrowsColor : 0;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 2, currentEyebrowStyle, currentEyebrowOpacity);
            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 2, 1, currentEyebrowColor, currentEyebrowColor);

            // 3 ageing
            int currentAgeingStyle = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.AgeingStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 3) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 3) : 0;
            float currentAgeingOpacity = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.AgeingOpacity : 0f;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 3, currentAgeingStyle, currentAgeingOpacity);

            // 4 makeup
            int currentMakeupStyle = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.MakeupStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 4) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 4) : 0;
            float currentMakeupOpacity = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.MakeupOpacity : 0f;
            int currentMakeupColor = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.MakeupColor : 0;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 4, currentMakeupStyle, currentMakeupOpacity);
            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 4, 2, currentMakeupColor, currentMakeupColor);

            // 5 blush
            int currentBlushStyle = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.BlushStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 5) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 5) : 0;
            float currentBlushOpacity = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.BlushOpacity : 0f;
            int currentBlushColor = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.BlushColor : 0;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 5, currentBlushStyle, currentBlushOpacity);
            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 5, 2, currentBlushColor, currentBlushColor);

            // 6 complexion
            int currentComplexionStyle = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.ComplexionStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 6) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 6) : 0;
            float currentComplexionOpacity = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.ComplexionOpacity : 0f;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 6, currentComplexionStyle, currentComplexionOpacity);

            // 7 sun damage
            int currentSunDamageStyle = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.SunDamageStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 7) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 7) : 0;
            float currentSunDamageOpacity = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.SunDamageOpacity : 0f;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 7, currentSunDamageStyle, currentSunDamageOpacity);

            // 8 lipstick
            int currentLipstickStyle = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.LipstickStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 8) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 8) : 0;
            float currentLipstickOpacity = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.LipstickOpacity : 0f;
            int currentLipstickColor = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.LipstickColor : 0;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 8, currentLipstickStyle, currentLipstickOpacity);
            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 8, 2, currentLipstickColor, currentLipstickColor);

            // 9 moles/freckles
            int currentMolesFrecklesStyle = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.MolesFrecklesStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 9) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 9) : 0;
            float currentMolesFrecklesOpacity = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.MolesFrecklesOpacity : 0f;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 9, currentMolesFrecklesStyle, currentMolesFrecklesOpacity);

            // 10 chest hair
            int currentChesthairStyle = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.ChestHairStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 10) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 10) : 0;
            float currentChesthairOpacity = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.ChestHairOpacity : 0f;
            int currentChesthairColor = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.ChestHairColor : 0;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 10, currentChesthairStyle, currentChesthairOpacity);
            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 10, 1, currentChesthairColor, currentChesthairColor);

            // 11 body blemishes
            int currentBodyBlemishesStyle = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.BodyBlemishesStyle : GetPedHeadOverlayValue(Game.PlayerPed.Handle, 11) != 255 ? GetPedHeadOverlayValue(Game.PlayerPed.Handle, 11) : 0;
            float currentBodyBlemishesOpacity = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.BodyBlemishesOpacity : 0f;
            SetPedHeadOverlay(Game.PlayerPed.Handle, 11, currentBodyBlemishesStyle, currentBodyBlemishesOpacity);

            int currentEyeColor = editPed ? MenuFunctions.CurrentCharacter.PedAppearance.EyeColor : 0;
            SetPedEyeColor(Game.PlayerPed.Handle, currentEyeColor);

            MenuListItem hairStyles = new MenuListItem("Hair Style", hairStylesList, currentHairStyle, "Select a hair style.");
            //MenuListItem hairColors = new MenuListItem("Hair Color", overlayColorsList, currentHairColor, "Select a hair color.");
            MenuListItem hairColors = new MenuListItem("Hair Color", overlayColorsList, currentHairColor, "Select a hair color.") { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };
            //MenuListItem hairHighlightColors = new MenuListItem("Hair Highlight Color", overlayColorsList, currentHairHighlightColor, "Select a hair highlight color.");
            MenuListItem hairHighlightColors = new MenuListItem("Hair Highlight Color", overlayColorsList, currentHairHighlightColor, "Select a hair highlight color.") { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };

            MenuListItem blemishesStyle = new MenuListItem("Blemishes Style", blemishesStyleList, currentBlemishesStyle, "Select a blemishes style.");
            //MenuSliderItem blemishesOpacity = new MenuSliderItem("Blemishes Opacity", "Select a blemishes opacity.", 0, 10, (int)(currentBlemishesOpacity * 10f), false);
            MenuListItem blemishesOpacity = new MenuListItem("Blemishes Opacity", opacity, (int)(currentBlemishesOpacity * 10f), "Select a blemishes opacity.") { ShowOpacityPanel = true };

            MenuListItem beardStyles = new MenuListItem("Beard Style", beardStylesList, currentBeardStyle, "Select a beard/facial hair style.");
            MenuListItem beardOpacity = new MenuListItem("Beard Opacity", opacity, (int)(currentBeardOpacity * 10f), "Select the opacity for your beard/facial hair.") { ShowOpacityPanel = true };
            MenuListItem beardColor = new MenuListItem("Beard Color", overlayColorsList, currentBeardColor, "Select a beard color.") { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };
            //MenuSliderItem beardOpacity = new MenuSliderItem("Beard Opacity", "Select the opacity for your beard/facial hair.", 0, 10, (int)(currentBeardOpacity * 10f), false);
            //MenuListItem beardColor = new MenuListItem("Beard Color", overlayColorsList, currentBeardColor, "Select a beard color");

            MenuListItem eyebrowStyle = new MenuListItem("Eyebrows Style", eyebrowsStyleList, currentEyebrowStyle, "Select an eyebrows style.");
            MenuListItem eyebrowOpacity = new MenuListItem("Eyebrows Opacity", opacity, (int)(currentEyebrowOpacity * 10f), "Select the opacity for your eyebrows.") { ShowOpacityPanel = true };
            MenuListItem eyebrowColor = new MenuListItem("Eyebrows Color", overlayColorsList, currentEyebrowColor, "Select an eyebrows color.") { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };
            //MenuSliderItem eyebrowOpacity = new MenuSliderItem("Eyebrows Opacity", "Select the opacity for your eyebrows.", 0, 10, (int)(currentEyebrowOpacity * 10f), false);

            MenuListItem ageingStyle = new MenuListItem("Ageing Style", ageingStyleList, currentAgeingStyle, "Select an ageing style.");
            MenuListItem ageingOpacity = new MenuListItem("Ageing Opacity", opacity, (int)(currentAgeingOpacity * 10f), "Select an ageing opacity.") { ShowOpacityPanel = true };
            //MenuSliderItem ageingOpacity = new MenuSliderItem("Ageing Opacity", "Select an ageing opacity.", 0, 10, (int)(currentAgeingOpacity * 10f), false);

            MenuListItem makeupStyle = new MenuListItem("Makeup Style", makeupStyleList, currentMakeupStyle, "Select a makeup style.");
            MenuListItem makeupOpacity = new MenuListItem("Makeup Opacity", opacity, (int)(currentMakeupOpacity * 10f), "Select a makeup opacity") { ShowOpacityPanel = true };
            //MenuSliderItem makeupOpacity = new MenuSliderItem("Makeup Opacity", 0, 10, (int)(currentMakeupOpacity * 10f), "Select a makeup opacity.");
            MenuListItem makeupColor = new MenuListItem("Makeup Color", overlayColorsList, currentMakeupColor, "Select a makeup color.") { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Makeup };

            MenuListItem blushStyle = new MenuListItem("Blush Style", blushStyleList, currentBlushStyle, "Select a blush style.");
            MenuListItem blushOpacity = new MenuListItem("Blush Opacity", opacity, (int)(currentBlushOpacity * 10f), "Select a blush opacity.") { ShowOpacityPanel = true };
            //MenuSliderItem blushOpacity = new MenuSliderItem("Blush Opacity", 0, 10, (int)(currentBlushOpacity * 10f), "Select a blush opacity.");
            MenuListItem blushColor = new MenuListItem("Blush Color", overlayColorsList, currentBlushColor, "Select a blush color.") { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Makeup };

            MenuListItem complexionStyle = new MenuListItem("Complexion Style", complexionStyleList, currentComplexionStyle, "Select a complexion style.");
            //MenuSliderItem complexionOpacity = new MenuSliderItem("Complexion Opacity", 0, 10, (int)(currentComplexionOpacity * 10f), "Select a complexion opacity.");
            MenuListItem complexionOpacity = new MenuListItem("Complexion Opacity", opacity, (int)(currentComplexionOpacity * 10f), "Select a complexion opacity.") { ShowOpacityPanel = true };

            MenuListItem sunDamageStyle = new MenuListItem("Sun Damage Style", sunDamageStyleList, currentSunDamageStyle, "Select a sun damage style.");
            //MenuSliderItem sunDamageOpacity = new MenuSliderItem("Sun Damage Opacity", 0, 10, (int)(currentSunDamageOpacity * 10f), "Select a sun damage opacity.");
            MenuListItem sunDamageOpacity = new MenuListItem("Sun Damage Opacity", opacity, (int)(currentSunDamageOpacity * 10f), "Select a sun damage opacity.") { ShowOpacityPanel = true };

            MenuListItem lipstickStyle = new MenuListItem("Lipstick Style", lipstickStyleList, currentLipstickStyle, "Select a lipstick style.");
            //MenuSliderItem lipstickOpacity = new MenuSliderItem("Lipstick Opacity", 0, 10, (int)(currentLipstickOpacity * 10f), "Select a lipstick opacity.");
            MenuListItem lipstickOpacity = new MenuListItem("Lipstick Opacity", opacity, (int)(currentLipstickOpacity * 10f), "Select a lipstick opacity.") { ShowOpacityPanel = true };
            MenuListItem lipstickColor = new MenuListItem("Lipstick Color", overlayColorsList, currentLipstickColor, "Select a lipstick color.") { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Makeup };

            MenuListItem molesFrecklesStyle = new MenuListItem("Moles and Freckles Style", molesFrecklesStyleList, currentMolesFrecklesStyle, "Select a moles and freckles style.");
            //MenuSliderItem molesFrecklesOpacity = new MenuSliderItem("Moles and Freckles Opacity", 0, 10, (int)(currentMolesFrecklesOpacity * 10f), "Select a moles and freckles opacity.");
            MenuListItem molesFrecklesOpacity = new MenuListItem("Moles and Freckles Opacity", opacity, (int)(currentMolesFrecklesOpacity * 10f), "Select a moles and freckles opacity.") { ShowOpacityPanel = true };

            MenuListItem chestHairStyle = new MenuListItem("Chest Hair Style", chestHairStyleList, currentChesthairStyle, "Select a chest hair style.");
            //MenuSliderItem chestHairOpacity = new MenuSliderItem("Chest Hair Opacity", 0, 10, (int)(currentChesthairOpacity * 10f), "Select a chest hair opacity.");
            MenuListItem chestHairOpacity = new MenuListItem("Chest Hair Opacity", opacity, (int)(currentChesthairOpacity * 10f), "Select a chest hair opacity.") { ShowOpacityPanel = true };
            MenuListItem chestHairColor = new MenuListItem("Chest Hair Color", overlayColorsList, currentChesthairColor, "Select a chest hair color.") { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };

            // Body blemishes
            MenuListItem bodyBlemishesStyle = new MenuListItem("Body Blemishes Style", bodyBlemishesList, currentBodyBlemishesStyle, "Select body blemishes style.");
            MenuListItem bodyBlemishesOpacity = new MenuListItem("Body Blemishes Opacity", opacity, (int)(currentBodyBlemishesOpacity * 10f), "Select body blemishes opacity.") { ShowOpacityPanel = true };

            MenuListItem eyeColor = new MenuListItem("Eye Colors", eyeColorList, currentEyeColor, "Select an eye/contact lens color.");

            appearanceMenu.AddMenuItem(hairStyles);
            appearanceMenu.AddMenuItem(hairColors);
            appearanceMenu.AddMenuItem(hairHighlightColors);

            appearanceMenu.AddMenuItem(blemishesStyle);
            appearanceMenu.AddMenuItem(blemishesOpacity);

            appearanceMenu.AddMenuItem(beardStyles);
            appearanceMenu.AddMenuItem(beardOpacity);
            appearanceMenu.AddMenuItem(beardColor);

            appearanceMenu.AddMenuItem(eyebrowStyle);
            appearanceMenu.AddMenuItem(eyebrowOpacity);
            appearanceMenu.AddMenuItem(eyebrowColor);

            appearanceMenu.AddMenuItem(ageingStyle);
            appearanceMenu.AddMenuItem(ageingOpacity);

            appearanceMenu.AddMenuItem(makeupStyle);
            appearanceMenu.AddMenuItem(makeupOpacity);
            appearanceMenu.AddMenuItem(makeupColor);

            appearanceMenu.AddMenuItem(blushStyle);
            appearanceMenu.AddMenuItem(blushOpacity);
            appearanceMenu.AddMenuItem(blushColor);

            appearanceMenu.AddMenuItem(complexionStyle);
            appearanceMenu.AddMenuItem(complexionOpacity);

            appearanceMenu.AddMenuItem(sunDamageStyle);
            appearanceMenu.AddMenuItem(sunDamageOpacity);

            appearanceMenu.AddMenuItem(lipstickStyle);
            appearanceMenu.AddMenuItem(lipstickOpacity);
            appearanceMenu.AddMenuItem(lipstickColor);

            appearanceMenu.AddMenuItem(molesFrecklesStyle);
            appearanceMenu.AddMenuItem(molesFrecklesOpacity);

            appearanceMenu.AddMenuItem(chestHairStyle);
            appearanceMenu.AddMenuItem(chestHairOpacity);
            appearanceMenu.AddMenuItem(chestHairColor);

            appearanceMenu.AddMenuItem(bodyBlemishesStyle);
            appearanceMenu.AddMenuItem(bodyBlemishesOpacity);

            appearanceMenu.AddMenuItem(eyeColor);

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

            #region clothing options menu
            string[] clothingCategoryNames = new string[12] { "Unused (head)", "Masks", "Unused (hair)", "Upper Body", "Lower Body", "Bags & Parachutes", "Shoes", "Scarfs & Chains", "Shirt & Accessory", "Body Armor & Accessory 2", "Badges & Logos", "Shirt Overlay & Jackets" };
            for (int i = 0; i < 12; i++)
            {
                if (i != 0 && i != 2)
                {
                    int currentVariationIndex = editPed && MenuFunctions.CurrentCharacter.DrawableVariations.clothes.ContainsKey(i) ? MenuFunctions.CurrentCharacter.DrawableVariations.clothes[i].Key : GetPedDrawableVariation(Game.PlayerPed.Handle, i);
                    int currentVariationTextureIndex = editPed && MenuFunctions.CurrentCharacter.DrawableVariations.clothes.ContainsKey(i) ? MenuFunctions.CurrentCharacter.DrawableVariations.clothes[i].Value : GetPedTextureVariation(Game.PlayerPed.Handle, i);

                    int maxDrawables = GetNumberOfPedDrawableVariations(Game.PlayerPed.Handle, i);

                    List<string> items = new List<string>();
                    for (int x = 0; x < maxDrawables; x++)
                    {
                        items.Add($"Drawable #{x} (of {maxDrawables})");
                    }

                    int maxTextures = GetNumberOfPedTextureVariations(Game.PlayerPed.Handle, i, currentVariationIndex);

                    MenuListItem listItem = new MenuListItem(clothingCategoryNames[i], items, currentVariationIndex, $"Select a drawable using the arrow keys and press ~o~enter~s~ to cycle through all available textures. Currently selected texture: #{currentVariationTextureIndex + 1} (of {maxTextures}).");
                    clothesMenu.AddMenuItem(listItem);
                }
            }
            #endregion

            #region props options menu
            string[] propNames = new string[5] { "Hats & Helmets", "Glasses", "Misc Props", "Watches", "Bracelets" };
            for (int x = 0; x < 5; x++)
            {
                int propId = x;
                if (x > 2)
                {
                    propId += 3;
                }

                int currentProp = editPed && MenuFunctions.CurrentCharacter.PropVariations.props.ContainsKey(propId) ? MenuFunctions.CurrentCharacter.PropVariations.props[propId].Key : GetPedPropIndex(Game.PlayerPed.Handle, propId);
                int currentPropTexture = editPed && MenuFunctions.CurrentCharacter.PropVariations.props.ContainsKey(propId) ? MenuFunctions.CurrentCharacter.PropVariations.props[propId].Value : GetPedPropTextureIndex(Game.PlayerPed.Handle, propId);

                List<string> propsList = new List<string>();
                for (int i = 0; i < GetNumberOfPedPropDrawableVariations(Game.PlayerPed.Handle, propId); i++)
                {
                    propsList.Add($"Prop #{i} (of {GetNumberOfPedPropDrawableVariations(Game.PlayerPed.Handle, propId)})");
                }
                propsList.Add("No Prop");


                if (GetPedPropIndex(Game.PlayerPed.Handle, propId) != -1)
                {
                    int maxPropTextures = GetNumberOfPedPropTextureVariations(Game.PlayerPed.Handle, propId, currentProp);
                    MenuListItem propListItem = new MenuListItem($"{propNames[x]}", propsList, currentProp, $"Select a prop using the arrow keys and press ~o~enter~s~ to cycle through all available textures. Currently selected texture: #{currentPropTexture + 1} (of {maxPropTextures}).");
                    propsMenu.AddMenuItem(propListItem);
                }
                else
                {
                    MenuListItem propListItem = new MenuListItem($"{propNames[x]}", propsList, currentProp, "Select a prop using the arrow keys and press ~o~enter~s~ to cycle through all available textures.");
                    propsMenu.AddMenuItem(propListItem);
                }


            }
            #endregion

            #region face features menu
            foreach (MenuSliderItem item in faceShapeMenu.GetMenuItems())
            {
                if (editPed)
                {
                    if (MenuFunctions.CurrentCharacter.FaceShapeFeatures.features == null)
                    {
                        MenuFunctions.CurrentCharacter.FaceShapeFeatures.features = new Dictionary<int, float>();
                    }
                    else
                    {
                        if (MenuFunctions.CurrentCharacter.FaceShapeFeatures.features.ContainsKey(item.Index))
                        {
                            item.Position = (int)(MenuFunctions.CurrentCharacter.FaceShapeFeatures.features[item.Index] * 10f) + 10;
                            SetPedFaceFeature(Game.PlayerPed.Handle, item.Index, MenuFunctions.CurrentCharacter.FaceShapeFeatures.features[item.Index]);
                        }
                        else
                        {
                            item.Position = 10;
                            SetPedFaceFeature(Game.PlayerPed.Handle, item.Index, 0f);
                        }
                    }
                }
                else
                {
                    item.Position = 10;
                    SetPedFaceFeature(Game.PlayerPed.Handle, item.Index, 0f);
                }
            }
            #endregion

            #region Tattoos menu
            List<string> headTattoosList = new List<string>();
            List<string> torsoTattoosList = new List<string>();
            List<string> leftArmTattoosList = new List<string>();
            List<string> rightArmTattoosList = new List<string>();
            List<string> leftLegTattoosList = new List<string>();
            List<string> rightLegTattoosList = new List<string>();
            List<string> badgeTattoosList = new List<string>();

            TattoosData.GenerateTattoosData();
            if (male)
            {
                int counter = 1;
                foreach (var tattoo in MaleTattoosCollection.HEAD)
                {
                    headTattoosList.Add($"Tattoo #{counter} (of {MaleTattoosCollection.HEAD.Count})");
                    counter++;
                }
                counter = 1;
                foreach (var tattoo in MaleTattoosCollection.TORSO)
                {
                    torsoTattoosList.Add($"Tattoo #{counter} (of {MaleTattoosCollection.TORSO.Count})");
                    counter++;
                }
                counter = 1;
                foreach (var tattoo in MaleTattoosCollection.LEFT_ARM)
                {
                    leftArmTattoosList.Add($"Tattoo #{counter} (of {MaleTattoosCollection.LEFT_ARM.Count})");
                    counter++;
                }
                counter = 1;
                foreach (var tattoo in MaleTattoosCollection.RIGHT_ARM)
                {
                    rightArmTattoosList.Add($"Tattoo #{counter} (of {MaleTattoosCollection.RIGHT_ARM.Count})");
                    counter++;
                }
                counter = 1;
                foreach (var tattoo in MaleTattoosCollection.LEFT_LEG)
                {
                    leftLegTattoosList.Add($"Tattoo #{counter} (of {MaleTattoosCollection.LEFT_LEG.Count})");
                    counter++;
                }
                counter = 1;
                foreach (var tattoo in MaleTattoosCollection.RIGHT_LEG)
                {
                    rightLegTattoosList.Add($"Tattoo #{counter} (of {MaleTattoosCollection.RIGHT_LEG.Count})");
                    counter++;
                }
                counter = 1;
                foreach (var tattoo in MaleTattoosCollection.BADGES)
                {
                    badgeTattoosList.Add($"Badge #{counter} (of {MaleTattoosCollection.BADGES.Count})");
                    counter++;
                }
            }
            else
            {
                int counter = 1;
                foreach (var tattoo in FemaleTattoosCollection.HEAD)
                {
                    headTattoosList.Add($"Tattoo #{counter} (of {FemaleTattoosCollection.HEAD.Count})");
                    counter++;
                }
                counter = 1;
                foreach (var tattoo in FemaleTattoosCollection.TORSO)
                {
                    torsoTattoosList.Add($"Tattoo #{counter} (of {FemaleTattoosCollection.TORSO.Count})");
                    counter++;
                }
                counter = 1;
                foreach (var tattoo in FemaleTattoosCollection.LEFT_ARM)
                {
                    leftArmTattoosList.Add($"Tattoo #{counter} (of {FemaleTattoosCollection.LEFT_ARM.Count})");
                    counter++;
                }
                counter = 1;
                foreach (var tattoo in FemaleTattoosCollection.RIGHT_ARM)
                {
                    rightArmTattoosList.Add($"Tattoo #{counter} (of {FemaleTattoosCollection.RIGHT_ARM.Count})");
                    counter++;
                }
                counter = 1;
                foreach (var tattoo in FemaleTattoosCollection.LEFT_LEG)
                {
                    leftLegTattoosList.Add($"Tattoo #{counter} (of {FemaleTattoosCollection.LEFT_LEG.Count})");
                    counter++;
                }
                counter = 1;
                foreach (var tattoo in FemaleTattoosCollection.RIGHT_LEG)
                {
                    rightLegTattoosList.Add($"Tattoo #{counter} (of {FemaleTattoosCollection.RIGHT_LEG.Count})");
                    counter++;
                }
                counter = 1;
                foreach (var tattoo in FemaleTattoosCollection.BADGES)
                {
                    badgeTattoosList.Add($"Badge #{counter} (of {FemaleTattoosCollection.BADGES.Count})");
                    counter++;
                }
            }

            const string tatDesc = "Cycle through the list to preview tattoos. If you like one, press enter to select it, selecting it will add the tattoo if you don't already have it. If you already have that tattoo then the tattoo will be removed.";
            MenuListItem headTatts = new MenuListItem("Head Tattoos", headTattoosList, 0, tatDesc);
            MenuListItem torsoTatts = new MenuListItem("Torso Tattoos", torsoTattoosList, 0, tatDesc);
            MenuListItem leftArmTatts = new MenuListItem("Left Arm Tattoos", leftArmTattoosList, 0, tatDesc);
            MenuListItem rightArmTatts = new MenuListItem("Right Arm Tattoos", rightArmTattoosList, 0, tatDesc);
            MenuListItem leftLegTatts = new MenuListItem("Left Leg Tattoos", leftLegTattoosList, 0, tatDesc);
            MenuListItem rightLegTatts = new MenuListItem("Right Leg Tattoos", rightLegTattoosList, 0, tatDesc);
            MenuListItem badgeTatts = new MenuListItem("Badge Overlays", badgeTattoosList, 0, tatDesc);

            tattoosMenu.AddMenuItem(headTatts);
            tattoosMenu.AddMenuItem(torsoTatts);
            tattoosMenu.AddMenuItem(leftArmTatts);
            tattoosMenu.AddMenuItem(rightArmTatts);
            tattoosMenu.AddMenuItem(leftLegTatts);
            tattoosMenu.AddMenuItem(rightLegTatts);
            tattoosMenu.AddMenuItem(badgeTatts);
            tattoosMenu.AddMenuItem(new MenuItem("Remove All Tattoos", "Click this if you want to remove all tattoos and start over."));
            #endregion

            createCharacterMenu.RefreshIndex();
            appearanceMenu.RefreshIndex();
            inheritanceMenu.RefreshIndex();
            tattoosMenu.RefreshIndex();
            
            BaseScript.TriggerEvent("CharacterCreator:CreationMenuReady");
        }

        /// <summary>
        /// Saves the mp character and quits the editor if successful.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> SavePed()
        {
            
            await BaseScript.Delay(0);
            MenuFunctions.CurrentCharacter.PedHeadBlendData = Game.PlayerPed.GetHeadBlendData();
            string json = JsonConvert.SerializeObject(MenuFunctions.CurrentCharacter);
            
            BaseScript.TriggerServerEvent("CharacterCreator:SavePed", json);
            MenuFunctions.EndMenu();

            return true;
        }

        /// <summary>
        /// Creates the menu.
        /// </summary>
        private void CreateMenu()
        {
            MenuController.EnableMenuToggleKeyOnKeyboard = false;
            MenuController.EnableMenuToggleKeyOnController = false;
            
            // Create the menu.
            MenuFunctions.CharacterCreatorMenu = new Menu(Game.Player.Name, "MP Ped Customization") { Visible = false };
            MenuController.AddMenu(MenuFunctions.CharacterCreatorMenu);

            MenuController.AddMenu(createCharacterMenu);
            MenuController.AddMenu(inheritanceMenu);
            MenuController.AddMenu(appearanceMenu);
            MenuController.AddMenu(faceShapeMenu);
            MenuController.AddMenu(tattoosMenu);
            MenuController.AddMenu(clothesMenu);
            MenuController.AddMenu(propsMenu);
            
            MenuFunctions.CharacterCreatorMenu.AddMenuItem(createMaleBtn);
            MenuController.BindMenuItem(MenuFunctions.CharacterCreatorMenu, createCharacterMenu, createMaleBtn);
            MenuFunctions.CharacterCreatorMenu.AddMenuItem(createFemaleBtn);
            MenuController.BindMenuItem(MenuFunctions.CharacterCreatorMenu, createCharacterMenu, createFemaleBtn);
            MenuFunctions.CharacterCreatorMenu.AddMenuItem(createFemaleBtn);
            MenuController.BindMenuItem(MenuFunctions.CharacterCreatorMenu, createCharacterMenu, createFemaleBtn);

            MenuFunctions.CharacterCreatorMenu.RefreshIndex();

            createCharacterMenu.InstructionalButtons.Add(Control.MoveLeftRight, "Turn Head");
            inheritanceMenu.InstructionalButtons.Add(Control.MoveLeftRight, "Turn Head");
            appearanceMenu.InstructionalButtons.Add(Control.MoveLeftRight, "Turn Head");
            faceShapeMenu.InstructionalButtons.Add(Control.MoveLeftRight, "Turn Head");
            tattoosMenu.InstructionalButtons.Add(Control.MoveLeftRight, "Turn Head");
            clothesMenu.InstructionalButtons.Add(Control.MoveLeftRight, "Turn Head");
            propsMenu.InstructionalButtons.Add(Control.MoveLeftRight, "Turn Head");

            createCharacterMenu.InstructionalButtons.Add(Control.PhoneExtraOption, "Turn Character");
            inheritanceMenu.InstructionalButtons.Add(Control.PhoneExtraOption, "Turn Character");
            appearanceMenu.InstructionalButtons.Add(Control.PhoneExtraOption, "Turn Character");
            faceShapeMenu.InstructionalButtons.Add(Control.PhoneExtraOption, "Turn Character");
            tattoosMenu.InstructionalButtons.Add(Control.PhoneExtraOption, "Turn Character");
            clothesMenu.InstructionalButtons.Add(Control.PhoneExtraOption, "Turn Character");
            propsMenu.InstructionalButtons.Add(Control.PhoneExtraOption, "Turn Character");

            createCharacterMenu.InstructionalButtons.Add(Control.ParachuteBrakeRight, "Turn Camera Right");
            inheritanceMenu.InstructionalButtons.Add(Control.ParachuteBrakeRight, "Turn Camera Right");
            appearanceMenu.InstructionalButtons.Add(Control.ParachuteBrakeRight, "Turn Camera Right");
            faceShapeMenu.InstructionalButtons.Add(Control.ParachuteBrakeRight, "Turn Camera Right");
            tattoosMenu.InstructionalButtons.Add(Control.ParachuteBrakeRight, "Turn Camera Right");
            clothesMenu.InstructionalButtons.Add(Control.ParachuteBrakeRight, "Turn Camera Right");
            propsMenu.InstructionalButtons.Add(Control.ParachuteBrakeRight, "Turn Camera Right");

            createCharacterMenu.InstructionalButtons.Add(Control.ParachuteBrakeLeft, "Turn Camera Left");
            inheritanceMenu.InstructionalButtons.Add(Control.ParachuteBrakeLeft, "Turn Camera Left");
            appearanceMenu.InstructionalButtons.Add(Control.ParachuteBrakeLeft, "Turn Camera Left");
            faceShapeMenu.InstructionalButtons.Add(Control.ParachuteBrakeLeft, "Turn Camera Left");
            tattoosMenu.InstructionalButtons.Add(Control.ParachuteBrakeLeft, "Turn Camera Left");
            clothesMenu.InstructionalButtons.Add(Control.ParachuteBrakeLeft, "Turn Camera Left");
            propsMenu.InstructionalButtons.Add(Control.ParachuteBrakeLeft, "Turn Camera Left");


            MenuItem inheritanceButton = new MenuItem("Character Inheritance", "Character inheritance options.");
            MenuItem appearanceButton = new MenuItem("Character Appearance", "Character appearance options.");
            MenuItem faceButton = new MenuItem("Character Face Shape Options", "Character face shape options.");
            MenuItem tattoosButton = new MenuItem("Character Tattoo Options", "Character tattoo options.");
            MenuItem clothesButton = new MenuItem("Character Clothes", "Character clothes.");
            MenuItem propsButton = new MenuItem("Character Props", "Character props.");
            MenuItem saveButton = new MenuItem("Save Character", "Save your character.");
            MenuItem exitNoSave = new MenuItem("Go back Without Saving", "Are you sure? All unsaved work will be lost.");
            MenuListItem faceExpressionList = new MenuListItem("Facial Expression", new List<string> { "Normal", "Happy", "Angry", "Aiming", "Injured", "Stressed", "Smug", "Sulk" }, 0, "Set a facial expression that will be used whenever your ped is idling.");

            inheritanceButton.Label = "→→→";
            appearanceButton.Label = "→→→";
            faceButton.Label = "→→→";
            tattoosButton.Label = "→→→";
            clothesButton.Label = "→→→";
            propsButton.Label = "→→→";

            createCharacterMenu.AddMenuItem(inheritanceButton);
            createCharacterMenu.AddMenuItem(appearanceButton);
            createCharacterMenu.AddMenuItem(faceButton);
            createCharacterMenu.AddMenuItem(tattoosButton);
            createCharacterMenu.AddMenuItem(clothesButton);
            createCharacterMenu.AddMenuItem(propsButton);
            createCharacterMenu.AddMenuItem(faceExpressionList);
            createCharacterMenu.AddMenuItem(saveButton);
            createCharacterMenu.AddMenuItem(exitNoSave);

            MenuController.BindMenuItem(createCharacterMenu, inheritanceMenu, inheritanceButton);
            MenuController.BindMenuItem(createCharacterMenu, appearanceMenu, appearanceButton);
            MenuController.BindMenuItem(createCharacterMenu, faceShapeMenu, faceButton);
            MenuController.BindMenuItem(createCharacterMenu, tattoosMenu, tattoosButton);
            MenuController.BindMenuItem(createCharacterMenu, clothesMenu, clothesButton);
            MenuController.BindMenuItem(createCharacterMenu, propsMenu, propsButton);

            #region inheritance
            List<string> parents = new List<string>();
            for (int i = 0; i < 46; i++)
            {
                parents.Add($"#{i}");
            }

            var inheritanceDads = new MenuListItem("Father", parents, 0, "Select a father.");
            var inheritanceMoms = new MenuListItem("Mother", parents, 0, "Select a mother.");
            List<float> mixValues = new List<float>() { 0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f };
            var inheritanceShapeMix = new MenuSliderItem("Head Shape Mix", "Select how much of your head shape should be inherited from your father or mother. All the way on the left is your dad, all the way on the right is your mom.", 0, 10, 5, true) { SliderLeftIcon = MenuItem.Icon.MALE, SliderRightIcon = MenuItem.Icon.FEMALE };
            var inheritanceSkinMix = new MenuSliderItem("Body Skin Mix", "Select how much of your body skin tone should be inherited from your father or mother. All the way on the left is your dad, all the way on the right is your mom.", 0, 10, 5, true) { SliderLeftIcon = MenuItem.Icon.MALE, SliderRightIcon = MenuItem.Icon.FEMALE };

            inheritanceMenu.AddMenuItem(inheritanceDads);
            inheritanceMenu.AddMenuItem(inheritanceMoms);
            inheritanceMenu.AddMenuItem(inheritanceShapeMix);
            inheritanceMenu.AddMenuItem(inheritanceSkinMix);

            void SetHeadBlend()
            {
                SetPedHeadBlendData(Game.PlayerPed.Handle, inheritanceDads.ListIndex, inheritanceMoms.ListIndex, 0, inheritanceDads.ListIndex, inheritanceMoms.ListIndex, 0, mixValues[inheritanceShapeMix.Position], mixValues[inheritanceSkinMix.Position], 0f, false);
            }

            inheritanceMenu.OnListIndexChange += (_menu, listItem, oldSelectionIndex, newSelectionIndex, itemIndex) =>
            {
                SetHeadBlend();
            };

            inheritanceMenu.OnSliderPositionChange += (sender, item, oldPosition, newPosition, itemIndex) =>
            {
                SetHeadBlend();
            };
            #endregion

            #region appearance
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

            // manage the list changes for appearance items.
            appearanceMenu.OnListIndexChange += (_menu, listItem, oldSelectionIndex, newSelectionIndex, itemIndex) =>
            {
                if (itemIndex == 0) // hair style
                {
                    ClearPedFacialDecorations(Game.PlayerPed.Handle);
                    MenuFunctions.CurrentCharacter.PedAppearance.HairOverlay = new KeyValuePair<string, string>("", "");

                    if (newSelectionIndex >= GetNumberOfPedDrawableVariations(Game.PlayerPed.Handle, 2))
                    {
                        SetPedComponentVariation(Game.PlayerPed.Handle, 2, 0, 0, 0);
                        MenuFunctions.CurrentCharacter.PedAppearance.HairStyle = 0;
                    }
                    else
                    {
                        SetPedComponentVariation(Game.PlayerPed.Handle, 2, newSelectionIndex, 0, 0);
                        MenuFunctions.CurrentCharacter.PedAppearance.HairStyle = newSelectionIndex;
                        if (hairOverlays.ContainsKey(newSelectionIndex))
                        {
                            SetPedFacialDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(hairOverlays[newSelectionIndex].Key), (uint)GetHashKey(hairOverlays[newSelectionIndex].Value));
                            MenuFunctions.CurrentCharacter.PedAppearance.HairOverlay = new KeyValuePair<string, string>(hairOverlays[newSelectionIndex].Key, hairOverlays[newSelectionIndex].Value);
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

                    MenuFunctions.CurrentCharacter.PedAppearance.HairColor = hairColor;
                    MenuFunctions.CurrentCharacter.PedAppearance.HairHighlightColor = hairHighlightColor;
                }
                else if (itemIndex == 33) // eye color
                {
                    int selection = ((MenuListItem)_menu.GetMenuItems()[itemIndex]).ListIndex;
                    SetPedEyeColor(Game.PlayerPed.Handle, selection);
                    MenuFunctions.CurrentCharacter.PedAppearance.EyeColor = selection;
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
                            MenuFunctions.CurrentCharacter.PedAppearance.BlemishesStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.BlemishesOpacity = opacity;
                            break;
                        case 5: // beards
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 1, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.BeardStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.BeardOpacity = opacity;
                            break;
                        case 7: // beards color
                            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 1, 1, selection, selection);
                            MenuFunctions.CurrentCharacter.PedAppearance.BeardColor = selection;
                            break;
                        case 8: // eyebrows
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 2, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.EyebrowsStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.EyebrowsOpacity = opacity;
                            break;
                        case 10: // eyebrows color
                            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 2, 1, selection, selection);
                            MenuFunctions.CurrentCharacter.PedAppearance.EyebrowsColor = selection;
                            break;
                        case 11: // ageing
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 3, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.AgeingStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.AgeingOpacity = opacity;
                            break;
                        case 13: // makeup
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 4, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.MakeupStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.MakeupOpacity = opacity;
                            break;
                        case 15: // makeup color
                            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 4, 2, selection, selection);
                            MenuFunctions.CurrentCharacter.PedAppearance.MakeupColor = selection;
                            break;
                        case 16: // blush style
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 5, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.BlushStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.BlushOpacity = opacity;
                            break;
                        case 18: // blush color
                            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 5, 2, selection, selection);
                            MenuFunctions.CurrentCharacter.PedAppearance.BlushColor = selection;
                            break;
                        case 19: // complexion
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 6, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.ComplexionStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.ComplexionOpacity = opacity;
                            break;
                        case 21: // sun damage
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 7, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.SunDamageStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.SunDamageOpacity = opacity;
                            break;
                        case 23: // lipstick
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 8, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.LipstickStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.LipstickOpacity = opacity;
                            break;
                        case 25: // lipstick color
                            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 8, 2, selection, selection);
                            MenuFunctions.CurrentCharacter.PedAppearance.LipstickColor = selection;
                            break;
                        case 26: // moles and freckles
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 9, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.MolesFrecklesStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.MolesFrecklesOpacity = opacity;
                            break;
                        case 28: // chest hair
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 10, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.ChestHairStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.ChestHairOpacity = opacity;
                            break;
                        case 30: // chest hair color
                            SetPedHeadOverlayColor(Game.PlayerPed.Handle, 10, 1, selection, selection);
                            MenuFunctions.CurrentCharacter.PedAppearance.ChestHairColor = selection;
                            break;
                        case 31: // body blemishes
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 11, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.BodyBlemishesStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.BodyBlemishesOpacity = opacity;
                            break;
                    }
                }
            };

            // manage the slider changes for opacity on the appearance items.
            appearanceMenu.OnListIndexChange += (_menu, listItem, oldSelectionIndex, newSelectionIndex, itemIndex) =>
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
                            MenuFunctions.CurrentCharacter.PedAppearance.BlemishesStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.BlemishesOpacity = opacity;
                            break;
                        case 6: // beards
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 1, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.BeardStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.BeardOpacity = opacity;
                            break;
                        case 9: // eyebrows
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 2, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.EyebrowsStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.EyebrowsOpacity = opacity;
                            break;
                        case 12: // ageing
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 3, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.AgeingStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.AgeingOpacity = opacity;
                            break;
                        case 14: // makeup
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 4, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.MakeupStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.MakeupOpacity = opacity;
                            break;
                        case 17: // blush style
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 5, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.BlushStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.BlushOpacity = opacity;
                            break;
                        case 20: // complexion
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 6, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.ComplexionStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.ComplexionOpacity = opacity;
                            break;
                        case 22: // sun damage
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 7, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.SunDamageStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.SunDamageOpacity = opacity;
                            break;
                        case 24: // lipstick
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 8, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.LipstickStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.LipstickOpacity = opacity;
                            break;
                        case 27: // moles and freckles
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 9, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.MolesFrecklesStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.MolesFrecklesOpacity = opacity;
                            break;
                        case 29: // chest hair
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 10, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.ChestHairStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.ChestHairOpacity = opacity;
                            break;
                        case 32: // body blemishes
                            SetPedHeadOverlay(Game.PlayerPed.Handle, 11, selection, opacity);
                            MenuFunctions.CurrentCharacter.PedAppearance.BodyBlemishesStyle = selection;
                            MenuFunctions.CurrentCharacter.PedAppearance.BodyBlemishesOpacity = opacity;
                            break;
                    }
                }
            };
            #endregion

            #region clothes
            clothesMenu.OnListIndexChange += (_menu, listItem, oldSelectionIndex, newSelectionIndex, realIndex) =>
            {
                int componentIndex = realIndex + 1;
                if (realIndex > 0)
                {
                    componentIndex += 1;
                }

                int textureIndex = GetPedTextureVariation(Game.PlayerPed.Handle, componentIndex);
                int newTextureIndex = 0;
                SetPedComponentVariation(Game.PlayerPed.Handle, componentIndex, newSelectionIndex, newTextureIndex, 0);
                if (MenuFunctions.CurrentCharacter.DrawableVariations.clothes == null)
                {
                    MenuFunctions.CurrentCharacter.DrawableVariations.clothes = new Dictionary<int, KeyValuePair<int, int>>();
                }

                int maxTextures = GetNumberOfPedTextureVariations(Game.PlayerPed.Handle, componentIndex, newSelectionIndex);

                MenuFunctions.CurrentCharacter.DrawableVariations.clothes[componentIndex] = new KeyValuePair<int, int>(newSelectionIndex, newTextureIndex);
                listItem.Description = $"Select a drawable using the arrow keys and press ~o~enter~s~ to cycle through all available textures. Currently selected texture: #{newTextureIndex + 1} (of {maxTextures}).";
            };

            clothesMenu.OnListItemSelect += (sender, listItem, listIndex, realIndex) =>
            {
                int componentIndex = realIndex + 1; // skip face options as that fucks up with inheritance faces
                if (realIndex > 0) // skip hair features as that is done in the appeareance menu
                {
                    componentIndex += 1;
                }

                int textureIndex = GetPedTextureVariation(Game.PlayerPed.Handle, componentIndex);
                int newTextureIndex = (GetNumberOfPedTextureVariations(Game.PlayerPed.Handle, componentIndex, listIndex) - 1) < (textureIndex + 1) ? 0 : textureIndex + 1;
                SetPedComponentVariation(Game.PlayerPed.Handle, componentIndex, listIndex, newTextureIndex, 0);
                if (MenuFunctions.CurrentCharacter.DrawableVariations.clothes == null)
                {
                    MenuFunctions.CurrentCharacter.DrawableVariations.clothes = new Dictionary<int, KeyValuePair<int, int>>();
                }

                int maxTextures = GetNumberOfPedTextureVariations(Game.PlayerPed.Handle, componentIndex, listIndex);

                MenuFunctions.CurrentCharacter.DrawableVariations.clothes[componentIndex] = new KeyValuePair<int, int>(listIndex, newTextureIndex);
                listItem.Description = $"Select a drawable using the arrow keys and press ~o~enter~s~ to cycle through all available textures. Currently selected texture: #{newTextureIndex + 1} (of {maxTextures}).";
            };
            #endregion

            #region props
            propsMenu.OnListIndexChange += (_menu, listItem, oldSelectionIndex, newSelectionIndex, realIndex) =>
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
                    if (MenuFunctions.CurrentCharacter.PropVariations.props == null)
                    {
                        MenuFunctions.CurrentCharacter.PropVariations.props = new Dictionary<int, KeyValuePair<int, int>>();
                    }
                    MenuFunctions.CurrentCharacter.PropVariations.props[propIndex] = new KeyValuePair<int, int>(-1, -1);
                    listItem.Description = $"Select a prop using the arrow keys and press ~o~enter~s~ to cycle through all available textures.";
                }
                else
                {
                    SetPedPropIndex(Game.PlayerPed.Handle, propIndex, newSelectionIndex, textureIndex, true);
                    if (MenuFunctions.CurrentCharacter.PropVariations.props == null)
                    {
                        MenuFunctions.CurrentCharacter.PropVariations.props = new Dictionary<int, KeyValuePair<int, int>>();
                    }
                    MenuFunctions.CurrentCharacter.PropVariations.props[propIndex] = new KeyValuePair<int, int>(newSelectionIndex, textureIndex);
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

            propsMenu.OnListItemSelect += (sender, listItem, listIndex, realIndex) =>
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
                    if (MenuFunctions.CurrentCharacter.PropVariations.props == null)
                    {
                        MenuFunctions.CurrentCharacter.PropVariations.props = new Dictionary<int, KeyValuePair<int, int>>();
                    }
                    MenuFunctions.CurrentCharacter.PropVariations.props[propIndex] = new KeyValuePair<int, int>(-1, -1);
                    listItem.Description = $"Select a prop using the arrow keys and press ~o~enter~s~ to cycle through all available textures.";
                }
                else
                {
                    SetPedPropIndex(Game.PlayerPed.Handle, propIndex, listIndex, newTextureIndex, true);
                    if (MenuFunctions.CurrentCharacter.PropVariations.props == null)
                    {
                        MenuFunctions.CurrentCharacter.PropVariations.props = new Dictionary<int, KeyValuePair<int, int>>();
                    }
                    MenuFunctions.CurrentCharacter.PropVariations.props[propIndex] = new KeyValuePair<int, int>(listIndex, newTextureIndex);
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
                //propsMenu.UpdateScaleform();
            };
            #endregion

            #region face shape data
            /*
            Nose_Width  
            Nose_Peak_Hight  
            Nose_Peak_Lenght  
            Nose_Bone_High  
            Nose_Peak_Lowering  
            Nose_Bone_Twist  
            EyeBrown_High  
            EyeBrown_Forward  
            Cheeks_Bone_High  
            Cheeks_Bone_Width  
            Cheeks_Width  
            Eyes_Openning  
            Lips_Thickness  
            Jaw_Bone_Width 'Bone size to sides  
            Jaw_Bone_Back_Lenght 'Bone size to back  
            Chimp_Bone_Lowering 'Go Down  
            Chimp_Bone_Lenght 'Go forward  
            Chimp_Bone_Width  
            Chimp_Hole  
            Neck_Thikness  
            */

            List<float> faceFeaturesValuesList = new List<float>()
            {
               -1.0f,    // 0
               -0.9f,    // 1
               -0.8f,    // 2
               -0.7f,    // 3
               -0.6f,    // 4
               -0.5f,    // 5
               -0.4f,    // 6
               -0.3f,    // 7
               -0.2f,    // 8
               -0.1f,    // 9
                0.0f,    // 10
                0.1f,    // 11
                0.2f,    // 12
                0.3f,    // 13
                0.4f,    // 14
                0.5f,    // 15
                0.6f,    // 16
                0.7f,    // 17
                0.8f,    // 18
                0.9f,    // 19
                1.0f     // 20
            };

            var faceFeaturesNamesList = new string[20]
            {
                "Nose Width",               // 0
                "Noes Peak Height",         // 1
                "Nose Peak Length",         // 2
                "Nose Bone Height",         // 3
                "Nose Peak Lowering",       // 4
                "Nose Bone Twist",          // 5
                "Eyebrows Height",          // 6
                "Eyebrows Depth",           // 7
                "Cheekbones Height",        // 8
                "Cheekbones Width",         // 9
                "Cheeks Width",             // 10
                "Eyes Opening",             // 11
                "Lips Thickness",           // 12
                "Jaw Bone Width",           // 13
                "Jaw Bone Depth/Length",    // 14
                "Chin Height",              // 15
                "Chin Depth/Length",        // 16
                "Chin Width",               // 17
                "Chin Hole Size",           // 18
                "Neck Thickness"            // 19
            };

            for (int i = 0; i < 20; i++)
            {
                MenuSliderItem faceFeature = new MenuSliderItem(faceFeaturesNamesList[i], $"Set the {faceFeaturesNamesList[i]} face feature value.", 0, 20, 10, true);
                faceShapeMenu.AddMenuItem(faceFeature);
            }

            faceShapeMenu.OnSliderPositionChange += (sender, sliderItem, oldPosition, newPosition, itemIndex) =>
            {
                if (MenuFunctions.CurrentCharacter.FaceShapeFeatures.features == null)
                {
                    MenuFunctions.CurrentCharacter.FaceShapeFeatures.features = new Dictionary<int, float>();
                }
                float value = faceFeaturesValuesList[newPosition];
                MenuFunctions.CurrentCharacter.FaceShapeFeatures.features[itemIndex] = value;
                SetPedFaceFeature(Game.PlayerPed.Handle, itemIndex, value);
            };

            #endregion

            #region tattoos
            void CreateListsIfNull()
            {
                if (MenuFunctions.CurrentCharacter.PedTatttoos.HeadTattoos == null)
                {
                    MenuFunctions.CurrentCharacter.PedTatttoos.HeadTattoos = new List<KeyValuePair<string, string>>();
                }
                if (MenuFunctions.CurrentCharacter.PedTatttoos.TorsoTattoos == null)
                {
                    MenuFunctions.CurrentCharacter.PedTatttoos.TorsoTattoos = new List<KeyValuePair<string, string>>();
                }
                if (MenuFunctions.CurrentCharacter.PedTatttoos.LeftArmTattoos == null)
                {
                    MenuFunctions.CurrentCharacter.PedTatttoos.LeftArmTattoos = new List<KeyValuePair<string, string>>();
                }
                if (MenuFunctions.CurrentCharacter.PedTatttoos.RightArmTattoos == null)
                {
                    MenuFunctions.CurrentCharacter.PedTatttoos.RightArmTattoos = new List<KeyValuePair<string, string>>();
                }
                if (MenuFunctions.CurrentCharacter.PedTatttoos.LeftLegTattoos == null)
                {
                    MenuFunctions.CurrentCharacter.PedTatttoos.LeftLegTattoos = new List<KeyValuePair<string, string>>();
                }
                if (MenuFunctions.CurrentCharacter.PedTatttoos.RightLegTattoos == null)
                {
                    MenuFunctions.CurrentCharacter.PedTatttoos.RightLegTattoos = new List<KeyValuePair<string, string>>();
                }
                if (MenuFunctions.CurrentCharacter.PedTatttoos.BadgeTattoos == null)
                {
                    MenuFunctions.CurrentCharacter.PedTatttoos.BadgeTattoos = new List<KeyValuePair<string, string>>();
                }
            }

            void ApplySavedTattoos()
            {
                // remove all decorations, and then manually re-add them all. what a retarded way of doing this R*....
                ClearPedDecorations(Game.PlayerPed.Handle);

                foreach (var tattoo in MenuFunctions.CurrentCharacter.PedTatttoos.HeadTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }
                foreach (var tattoo in MenuFunctions.CurrentCharacter.PedTatttoos.TorsoTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }
                foreach (var tattoo in MenuFunctions.CurrentCharacter.PedTatttoos.LeftArmTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }
                foreach (var tattoo in MenuFunctions.CurrentCharacter.PedTatttoos.RightArmTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }
                foreach (var tattoo in MenuFunctions.CurrentCharacter.PedTatttoos.LeftLegTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }
                foreach (var tattoo in MenuFunctions.CurrentCharacter.PedTatttoos.RightLegTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }
                foreach (var tattoo in MenuFunctions.CurrentCharacter.PedTatttoos.BadgeTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }

                if (!string.IsNullOrEmpty(MenuFunctions.CurrentCharacter.PedAppearance.HairOverlay.Key) && !string.IsNullOrEmpty(MenuFunctions.CurrentCharacter.PedAppearance.HairOverlay.Value))
                {
                    // reset hair value
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(MenuFunctions.CurrentCharacter.PedAppearance.HairOverlay.Key), (uint)GetHashKey(MenuFunctions.CurrentCharacter.PedAppearance.HairOverlay.Value));
                }
            }

            tattoosMenu.OnIndexChange += (sender, oldItem, newItem, oldIndex, newIndex) =>
            {
                CreateListsIfNull();
                ApplySavedTattoos();
            };

            #region tattoos menu list select events
            tattoosMenu.OnListIndexChange += (sender, item, oldIndex, tattooIndex, menuIndex) =>
            {
                CreateListsIfNull();
                ApplySavedTattoos();
                if (menuIndex == 0) // head
                {
                    var Tattoo = MenuFunctions.CurrentCharacter.IsMale ? MaleTattoosCollection.HEAD.ElementAt(tattooIndex) : FemaleTattoosCollection.HEAD.ElementAt(tattooIndex);
                    KeyValuePair<string, string> tat = new KeyValuePair<string, string>(Tattoo.collectionName, Tattoo.name);
                    if (!MenuFunctions.CurrentCharacter.PedTatttoos.HeadTattoos.Contains(tat))
                    {
                        SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tat.Key), (uint)GetHashKey(tat.Value));
                    }
                }
                else if (menuIndex == 1) // torso
                {
                    var Tattoo = MenuFunctions.CurrentCharacter.IsMale ? MaleTattoosCollection.TORSO.ElementAt(tattooIndex) : FemaleTattoosCollection.TORSO.ElementAt(tattooIndex);
                    KeyValuePair<string, string> tat = new KeyValuePair<string, string>(Tattoo.collectionName, Tattoo.name);
                    if (!MenuFunctions.CurrentCharacter.PedTatttoos.TorsoTattoos.Contains(tat))
                    {
                        SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tat.Key), (uint)GetHashKey(tat.Value));
                    }
                }
                else if (menuIndex == 2) // left arm
                {
                    var Tattoo = MenuFunctions.CurrentCharacter.IsMale ? MaleTattoosCollection.LEFT_ARM.ElementAt(tattooIndex) : FemaleTattoosCollection.LEFT_ARM.ElementAt(tattooIndex);
                    KeyValuePair<string, string> tat = new KeyValuePair<string, string>(Tattoo.collectionName, Tattoo.name);
                    if (!MenuFunctions.CurrentCharacter.PedTatttoos.LeftArmTattoos.Contains(tat))
                    {
                        SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tat.Key), (uint)GetHashKey(tat.Value));
                    }
                }
                else if (menuIndex == 3) // right arm
                {
                    var Tattoo = MenuFunctions.CurrentCharacter.IsMale ? MaleTattoosCollection.RIGHT_ARM.ElementAt(tattooIndex) : FemaleTattoosCollection.RIGHT_ARM.ElementAt(tattooIndex);
                    KeyValuePair<string, string> tat = new KeyValuePair<string, string>(Tattoo.collectionName, Tattoo.name);
                    if (!MenuFunctions.CurrentCharacter.PedTatttoos.RightArmTattoos.Contains(tat))
                    {
                        SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tat.Key), (uint)GetHashKey(tat.Value));
                    }
                }
                else if (menuIndex == 4) // left leg
                {
                    var Tattoo = MenuFunctions.CurrentCharacter.IsMale ? MaleTattoosCollection.LEFT_LEG.ElementAt(tattooIndex) : FemaleTattoosCollection.LEFT_LEG.ElementAt(tattooIndex);
                    KeyValuePair<string, string> tat = new KeyValuePair<string, string>(Tattoo.collectionName, Tattoo.name);
                    if (!MenuFunctions.CurrentCharacter.PedTatttoos.LeftLegTattoos.Contains(tat))
                    {
                        SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tat.Key), (uint)GetHashKey(tat.Value));
                    }
                }
                else if (menuIndex == 5) // right leg
                {
                    var Tattoo = MenuFunctions.CurrentCharacter.IsMale ? MaleTattoosCollection.RIGHT_LEG.ElementAt(tattooIndex) : FemaleTattoosCollection.RIGHT_LEG.ElementAt(tattooIndex);
                    KeyValuePair<string, string> tat = new KeyValuePair<string, string>(Tattoo.collectionName, Tattoo.name);
                    if (!MenuFunctions.CurrentCharacter.PedTatttoos.RightLegTattoos.Contains(tat))
                    {
                        SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tat.Key), (uint)GetHashKey(tat.Value));
                    }
                }
                else if (menuIndex == 6) // badges
                {
                    var Tattoo = MenuFunctions.CurrentCharacter.IsMale ? MaleTattoosCollection.BADGES.ElementAt(tattooIndex) : FemaleTattoosCollection.BADGES.ElementAt(tattooIndex);
                    KeyValuePair<string, string> tat = new KeyValuePair<string, string>(Tattoo.collectionName, Tattoo.name);
                    if (!MenuFunctions.CurrentCharacter.PedTatttoos.BadgeTattoos.Contains(tat))
                    {
                        SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tat.Key), (uint)GetHashKey(tat.Value));
                    }
                }
            };

            tattoosMenu.OnListItemSelect += (sender, item, tattooIndex, menuIndex) =>
            {
                CreateListsIfNull();

                if (menuIndex == 0) // head
                {
                    var Tattoo = MenuFunctions.CurrentCharacter.IsMale ? MaleTattoosCollection.HEAD.ElementAt(tattooIndex) : FemaleTattoosCollection.HEAD.ElementAt(tattooIndex);
                    KeyValuePair<string, string> tat = new KeyValuePair<string, string>(Tattoo.collectionName, Tattoo.name);
                    if (MenuFunctions.CurrentCharacter.PedTatttoos.HeadTattoos.Contains(tat))
                    {
                        SubtitleLib.Custom($"Tattoo #{tattooIndex + 1} has been ~r~removed~s~.");
                        MenuFunctions.CurrentCharacter.PedTatttoos.HeadTattoos.Remove(tat);
                    }
                    else
                    {
                        SubtitleLib.Custom($"Tattoo #{tattooIndex + 1} has been ~g~added~s~.");
                        MenuFunctions.CurrentCharacter.PedTatttoos.HeadTattoos.Add(tat);
                    }
                }
                else if (menuIndex == 1) // torso
                {
                    var Tattoo = MenuFunctions.CurrentCharacter.IsMale ? MaleTattoosCollection.TORSO.ElementAt(tattooIndex) : FemaleTattoosCollection.TORSO.ElementAt(tattooIndex);
                    KeyValuePair<string, string> tat = new KeyValuePair<string, string>(Tattoo.collectionName, Tattoo.name);
                    if (MenuFunctions.CurrentCharacter.PedTatttoos.TorsoTattoos.Contains(tat))
                    {
                        SubtitleLib.Custom($"Tattoo #{tattooIndex + 1} has been ~r~removed~s~.");
                        MenuFunctions.CurrentCharacter.PedTatttoos.TorsoTattoos.Remove(tat);
                    }
                    else
                    {
                        SubtitleLib.Custom($"Tattoo #{tattooIndex + 1} has been ~g~added~s~.");
                        MenuFunctions.CurrentCharacter.PedTatttoos.TorsoTattoos.Add(tat);
                    }
                }
                else if (menuIndex == 2) // left arm
                {
                    var Tattoo = MenuFunctions.CurrentCharacter.IsMale ? MaleTattoosCollection.LEFT_ARM.ElementAt(tattooIndex) : FemaleTattoosCollection.LEFT_ARM.ElementAt(tattooIndex);
                    KeyValuePair<string, string> tat = new KeyValuePair<string, string>(Tattoo.collectionName, Tattoo.name);
                    if (MenuFunctions.CurrentCharacter.PedTatttoos.LeftArmTattoos.Contains(tat))
                    {
                        SubtitleLib.Custom($"Tattoo #{tattooIndex + 1} has been ~r~removed~s~.");
                        MenuFunctions.CurrentCharacter.PedTatttoos.LeftArmTattoos.Remove(tat);
                    }
                    else
                    {
                        SubtitleLib.Custom($"Tattoo #{tattooIndex + 1} has been ~g~added~s~.");
                        MenuFunctions.CurrentCharacter.PedTatttoos.LeftArmTattoos.Add(tat);
                    }
                }
                else if (menuIndex == 3) // right arm
                {
                    var Tattoo = MenuFunctions.CurrentCharacter.IsMale ? MaleTattoosCollection.RIGHT_ARM.ElementAt(tattooIndex) : FemaleTattoosCollection.RIGHT_ARM.ElementAt(tattooIndex);
                    KeyValuePair<string, string> tat = new KeyValuePair<string, string>(Tattoo.collectionName, Tattoo.name);
                    if (MenuFunctions.CurrentCharacter.PedTatttoos.RightArmTattoos.Contains(tat))
                    {
                        SubtitleLib.Custom($"Tattoo #{tattooIndex + 1} has been ~r~removed~s~.");
                        MenuFunctions.CurrentCharacter.PedTatttoos.RightArmTattoos.Remove(tat);
                    }
                    else
                    {
                        SubtitleLib.Custom($"Tattoo #{tattooIndex + 1} has been ~g~added~s~.");
                        MenuFunctions.CurrentCharacter.PedTatttoos.RightArmTattoos.Add(tat);
                    }
                }
                else if (menuIndex == 4) // left leg
                {
                    var Tattoo = MenuFunctions.CurrentCharacter.IsMale ? MaleTattoosCollection.LEFT_LEG.ElementAt(tattooIndex) : FemaleTattoosCollection.LEFT_LEG.ElementAt(tattooIndex);
                    KeyValuePair<string, string> tat = new KeyValuePair<string, string>(Tattoo.collectionName, Tattoo.name);
                    if (MenuFunctions.CurrentCharacter.PedTatttoos.LeftLegTattoos.Contains(tat))
                    {
                        SubtitleLib.Custom($"Tattoo #{tattooIndex + 1} has been ~r~removed~s~.");
                        MenuFunctions.CurrentCharacter.PedTatttoos.LeftLegTattoos.Remove(tat);
                    }
                    else
                    {
                        SubtitleLib.Custom($"Tattoo #{tattooIndex + 1} has been ~g~added~s~.");
                        MenuFunctions.CurrentCharacter.PedTatttoos.LeftLegTattoos.Add(tat);
                    }
                }
                else if (menuIndex == 5) // right leg
                {
                    var Tattoo = MenuFunctions.CurrentCharacter.IsMale ? MaleTattoosCollection.RIGHT_LEG.ElementAt(tattooIndex) : FemaleTattoosCollection.RIGHT_LEG.ElementAt(tattooIndex);
                    KeyValuePair<string, string> tat = new KeyValuePair<string, string>(Tattoo.collectionName, Tattoo.name);
                    if (MenuFunctions.CurrentCharacter.PedTatttoos.RightLegTattoos.Contains(tat))
                    {
                        SubtitleLib.Custom($"Tattoo #{tattooIndex + 1} has been ~r~removed~s~.");
                        MenuFunctions.CurrentCharacter.PedTatttoos.RightLegTattoos.Remove(tat);
                    }
                    else
                    {
                        SubtitleLib.Custom($"Tattoo #{tattooIndex + 1} has been ~g~added~s~.");
                        MenuFunctions.CurrentCharacter.PedTatttoos.RightLegTattoos.Add(tat);
                    }
                }
                else if (menuIndex == 6) // badges
                {
                    var Tattoo = MenuFunctions.CurrentCharacter.IsMale ? MaleTattoosCollection.BADGES.ElementAt(tattooIndex) : FemaleTattoosCollection.BADGES.ElementAt(tattooIndex);
                    KeyValuePair<string, string> tat = new KeyValuePair<string, string>(Tattoo.collectionName, Tattoo.name);
                    if (MenuFunctions.CurrentCharacter.PedTatttoos.BadgeTattoos.Contains(tat))
                    {
                        SubtitleLib.Custom($"Badge #{tattooIndex + 1} has been ~r~removed~s~.");
                        MenuFunctions.CurrentCharacter.PedTatttoos.BadgeTattoos.Remove(tat);
                    }
                    else
                    {
                        SubtitleLib.Custom($"Badge #{tattooIndex + 1} has been ~g~added~s~.");
                        MenuFunctions.CurrentCharacter.PedTatttoos.BadgeTattoos.Add(tat);
                    }
                }

                ApplySavedTattoos();

            };

            // eventhandler for when a tattoo is selected.
            tattoosMenu.OnItemSelect += (sender, item, index) =>
            {
                NotifyLib.Success("All tattoos have been removed.");
                MenuFunctions.CurrentCharacter.PedTatttoos.HeadTattoos.Clear();
                MenuFunctions.CurrentCharacter.PedTatttoos.TorsoTattoos.Clear();
                MenuFunctions.CurrentCharacter.PedTatttoos.LeftArmTattoos.Clear();
                MenuFunctions.CurrentCharacter.PedTatttoos.RightArmTattoos.Clear();
                MenuFunctions.CurrentCharacter.PedTatttoos.LeftLegTattoos.Clear();
                MenuFunctions.CurrentCharacter.PedTatttoos.RightLegTattoos.Clear();
                MenuFunctions.CurrentCharacter.PedTatttoos.BadgeTattoos.Clear();
                ClearPedDecorations(Game.PlayerPed.Handle);
            };

            #endregion
            #endregion


            // handle list changes in the character creator menu.
            createCharacterMenu.OnListIndexChange += (sender, item, oldListIndex, newListIndex, itemIndex) =>
            {
                if (item == faceExpressionList)
                {
                    MenuFunctions.CurrentCharacter.FacialExpression = facial_expressions[newListIndex];
                    SetFacialIdleAnimOverride(Game.PlayerPed.Handle, MenuFunctions.CurrentCharacter.FacialExpression ?? facial_expressions[0], null);
                }
            };

            // handle button presses for the createCharacter menu.
            createCharacterMenu.OnItemSelect += async (sender, item, index) =>
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
                    }
                }
                
                else if (item == exitBtn) // exit Creator
                {
                    bool confirm = false;
                    AddTextEntry("warning_message_first_line", "Are you sure you want to exit the character creator?");
                    AddTextEntry("warning_message_second_line", "You will lose all (unsaved) customization!");
                    createCharacterMenu.CloseMenu();

                    // wait for confirmation or cancel input.
                    while (true)
                    {
                        await BaseScript.Delay(0);
                        int unk = 1;
                        int unk2 = 1;
                        SetWarningMessage("warning_message_first_line", 20, "warning_message_second_line", true, 0,
                            ref unk, ref unk2, true, 0);
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
                        createCharacterMenu.OpenMenu();
                    }
                }
                else if (item == exitNoSave) // exit without saving
                {
                    bool confirm = false;
                    AddTextEntry("warning_message_first_line", "Are you sure you want to exit the character creator?");
                    AddTextEntry("warning_message_second_line", "You will lose all (unsaved) customization!");
                    createCharacterMenu.CloseMenu();

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
                        createCharacterMenu.OpenMenu();
                    }
                }
                else if (item == inheritanceButton) // update the inheritance menu anytime it's opened to prevent some weird glitch where old data is used.
                {
                    var data = Game.PlayerPed.GetHeadBlendData();
                    inheritanceDads.ListIndex = data.FirstFaceShape;
                    inheritanceMoms.ListIndex = data.SecondFaceShape;
                    inheritanceShapeMix.Position = (int)(data.ParentFaceShapePercent * 10f);
                    inheritanceSkinMix.Position = (int)(data.ParentSkinTonePercent * 10f);
                    inheritanceMenu.RefreshIndex();
                }
            };

            // eventhandler for whenever a menu item is selected in the main mp characters menu.
            MenuFunctions.CharacterCreatorMenu.OnItemSelect += async (sender, item, index) =>
            {
                if (item == createMaleBtn)
                {
                    uint model = (uint)GetHashKey("mp_m_freemode_01");

                    if (!HasModelLoaded(model))
                    {
                        RequestModel(model);
                        while (!HasModelLoaded(model))
                        {
                            await BaseScript.Delay(0);
                        }
                    }

                    int maxHealth = Game.PlayerPed.MaxHealth;
                    int maxArmour = Game.Player.MaxArmor;
                    int health = Game.PlayerPed.Health;
                    int armour = Game.PlayerPed.Armor;

                    //SetPlayerModel(Game.Player.Handle, model);
                    await SetPlayerSkin.SetPlayerSkinFunction(model, new SetPlayerSkin.PedInfo() { version = -1 });
                    //TODO: weapon loadouts here
                    
                    Game.Player.MaxArmor = maxArmour;
                    Game.PlayerPed.MaxHealth = maxHealth;
                    Game.PlayerPed.Health = health;
                    Game.PlayerPed.Armor = armour;

                    ClearPedDecorations(Game.PlayerPed.Handle);
                    ClearPedFacialDecorations(Game.PlayerPed.Handle);
                    SetPedDefaultComponentVariation(Game.PlayerPed.Handle);
                    SetPedHairColor(Game.PlayerPed.Handle, 0, 0);
                    SetPedEyeColor(Game.PlayerPed.Handle, 0);
                    ClearAllPedProps(Game.PlayerPed.Handle);

                    MakeCreateCharacterMenu(male: true);
                }
                else if (item == createFemaleBtn)
                {
                    uint model = (uint)GetHashKey("mp_f_freemode_01");

                    if (!HasModelLoaded(model))
                    {
                        RequestModel(model);
                        while (!HasModelLoaded(model))
                        {
                            await BaseScript.Delay(0);
                        }
                    }

                    int maxHealth = Game.PlayerPed.MaxHealth;
                    int maxArmour = Game.Player.MaxArmor;
                    int health = Game.PlayerPed.Health;
                    int armour = Game.PlayerPed.Armor;

                    //SetPlayerModel(Game.Player.Handle, model);
                    await SetPlayerSkin.SetPlayerSkinFunction(model, new SetPlayerSkin.PedInfo() { version = -1 });
                    //TODO: weapon loadouts here

                    Game.Player.MaxArmor = maxArmour;
                    Game.PlayerPed.MaxHealth = maxHealth;
                    Game.PlayerPed.Health = health;
                    Game.PlayerPed.Armor = armour;

                    ClearPedDecorations(Game.PlayerPed.Handle);
                    ClearPedFacialDecorations(Game.PlayerPed.Handle);
                    SetPedDefaultComponentVariation(Game.PlayerPed.Handle);
                    SetPedHairColor(Game.PlayerPed.Handle, 0, 0);
                    SetPedEyeColor(Game.PlayerPed.Handle, 0);
                    ClearAllPedProps(Game.PlayerPed.Handle);

                    MakeCreateCharacterMenu(male: false);
                }

            };
        }

        /// <summary>
        /// Spawns the ped from the data inside <see cref="MenuFunctions.CurrentCharacter"/>.
        /// Character data MUST be set BEFORE calling this function.
        /// </summary>
        /// <returns></returns>
        public static async Task SpawnSavedPed(bool restoreWeapons)
        {
            if (MenuFunctions.CurrentCharacter.Version < 1)
            {
                return;
            }
            if (IsModelInCdimage(MenuFunctions.CurrentCharacter.ModelHash))
            {
                if (!HasModelLoaded(MenuFunctions.CurrentCharacter.ModelHash))
                {
                    RequestModel(MenuFunctions.CurrentCharacter.ModelHash);
                    while (!HasModelLoaded(MenuFunctions.CurrentCharacter.ModelHash))
                    {
                        await BaseScript.Delay(0);
                    }
                }
                int maxHealth = Game.PlayerPed.MaxHealth;
                int maxArmour = Game.Player.MaxArmor;
                int health = Game.PlayerPed.Health;
                int armour = Game.PlayerPed.Armor;

                SetPlayerModel(Game.Player.Handle, MenuFunctions.CurrentCharacter.ModelHash);
                //TODO: weapon loadouts here

                Game.Player.MaxArmor = maxArmour;
                Game.PlayerPed.MaxHealth = maxHealth;
                Game.PlayerPed.Health = health;
                Game.PlayerPed.Armor = armour;

                ClearPedDecorations(Game.PlayerPed.Handle);
                ClearPedFacialDecorations(Game.PlayerPed.Handle);
                SetPedDefaultComponentVariation(Game.PlayerPed.Handle);
                SetPedHairColor(Game.PlayerPed.Handle, 0, 0);
                SetPedEyeColor(Game.PlayerPed.Handle, 0);
                ClearAllPedProps(Game.PlayerPed.Handle);

                #region headblend
                var data = MenuFunctions.CurrentCharacter.PedHeadBlendData;
                SetPedHeadBlendData(Game.PlayerPed.Handle, data.FirstFaceShape, data.SecondFaceShape, data.ThirdFaceShape, data.FirstSkinTone, data.SecondSkinTone, data.ThirdSkinTone, data.ParentFaceShapePercent, data.ParentSkinTonePercent, 0f, data.IsParentInheritance);

                while (!HasPedHeadBlendFinished(Game.PlayerPed.Handle))
                {
                    await BaseScript.Delay(0);
                }
                #endregion

                #region appearance
                var appData = MenuFunctions.CurrentCharacter.PedAppearance;
                // hair
                SetPedComponentVariation(Game.PlayerPed.Handle, 2, appData.HairStyle, 0, 0);
                SetPedHairColor(Game.PlayerPed.Handle, appData.HairColor, appData.HairHighlightColor);
                if (!string.IsNullOrEmpty(appData.HairOverlay.Key) && !string.IsNullOrEmpty(appData.HairOverlay.Value))
                {
                    SetPedFacialDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(appData.HairOverlay.Key), (uint)GetHashKey(appData.HairOverlay.Value));
                }
                // blemishes
                SetPedHeadOverlay(Game.PlayerPed.Handle, 0, appData.BlemishesStyle, appData.BlemishesOpacity);
                // bread
                SetPedHeadOverlay(Game.PlayerPed.Handle, 1, appData.BeardStyle, appData.BeardOpacity);
                SetPedHeadOverlayColor(Game.PlayerPed.Handle, 1, 1, appData.BeardColor, appData.BeardColor);
                // eyebrows
                SetPedHeadOverlay(Game.PlayerPed.Handle, 2, appData.EyebrowsStyle, appData.EyebrowsOpacity);
                SetPedHeadOverlayColor(Game.PlayerPed.Handle, 2, 1, appData.EyebrowsColor, appData.EyebrowsColor);
                // ageing
                SetPedHeadOverlay(Game.PlayerPed.Handle, 3, appData.AgeingStyle, appData.AgeingOpacity);
                // makeup
                SetPedHeadOverlay(Game.PlayerPed.Handle, 4, appData.MakeupStyle, appData.MakeupOpacity);
                SetPedHeadOverlayColor(Game.PlayerPed.Handle, 4, 2, appData.MakeupColor, appData.MakeupColor);
                // blush
                SetPedHeadOverlay(Game.PlayerPed.Handle, 5, appData.BlushStyle, appData.BlushOpacity);
                SetPedHeadOverlayColor(Game.PlayerPed.Handle, 5, 2, appData.BlushColor, appData.BlushColor);
                // complexion
                SetPedHeadOverlay(Game.PlayerPed.Handle, 6, appData.ComplexionStyle, appData.ComplexionOpacity);
                // sundamage
                SetPedHeadOverlay(Game.PlayerPed.Handle, 7, appData.SunDamageStyle, appData.SunDamageOpacity);
                // lipstick
                SetPedHeadOverlay(Game.PlayerPed.Handle, 8, appData.LipstickStyle, appData.LipstickOpacity);
                SetPedHeadOverlayColor(Game.PlayerPed.Handle, 8, 2, appData.LipstickColor, appData.LipstickColor);
                // moles and freckles
                SetPedHeadOverlay(Game.PlayerPed.Handle, 9, appData.MolesFrecklesStyle, appData.MolesFrecklesOpacity);
                // chest hair 
                SetPedHeadOverlay(Game.PlayerPed.Handle, 10, appData.ChestHairStyle, appData.ChestHairOpacity);
                SetPedHeadOverlayColor(Game.PlayerPed.Handle, 10, 1, appData.ChestHairColor, appData.ChestHairColor);
                // body blemishes 
                SetPedHeadOverlay(Game.PlayerPed.Handle, 11, appData.BodyBlemishesStyle, appData.BodyBlemishesOpacity);
                // eyecolor
                SetPedEyeColor(Game.PlayerPed.Handle, appData.EyeColor);
                #endregion

                #region Face Shape Data
                for (var i = 0; i < 19; i++)
                {
                    SetPedFaceFeature(Game.PlayerPed.Handle, i, 0f);
                }

                if (MenuFunctions.CurrentCharacter.FaceShapeFeatures.features != null)
                {
                    foreach (var t in MenuFunctions.CurrentCharacter.FaceShapeFeatures.features)
                    {
                        SetPedFaceFeature(Game.PlayerPed.Handle, t.Key, t.Value);
                    }
                }
                else
                {
                    MenuFunctions.CurrentCharacter.FaceShapeFeatures.features = new Dictionary<int, float>();
                }

                #endregion

                #region Clothing Data
                if (MenuFunctions.CurrentCharacter.DrawableVariations.clothes != null && MenuFunctions.CurrentCharacter.DrawableVariations.clothes.Count > 0)
                {
                    foreach (var cd in MenuFunctions.CurrentCharacter.DrawableVariations.clothes)
                    {
                        SetPedComponentVariation(Game.PlayerPed.Handle, cd.Key, cd.Value.Key, cd.Value.Value, 0);
                    }
                }
                #endregion

                #region Props Data
                if (MenuFunctions.CurrentCharacter.PropVariations.props != null && MenuFunctions.CurrentCharacter.PropVariations.props.Count > 0)
                {
                    foreach (var cd in MenuFunctions.CurrentCharacter.PropVariations.props)
                    {
                        if (cd.Value.Key > -1)
                        {
                            SetPedPropIndex(Game.PlayerPed.Handle, cd.Key, cd.Value.Key, cd.Value.Value > -1 ? cd.Value.Value : 0, true);
                        }
                    }
                }
                #endregion

                #region Tattoos

                if (MenuFunctions.CurrentCharacter.PedTatttoos.HeadTattoos == null)
                {
                    MenuFunctions.CurrentCharacter.PedTatttoos.HeadTattoos = new List<KeyValuePair<string, string>>();
                }
                if (MenuFunctions.CurrentCharacter.PedTatttoos.TorsoTattoos == null)
                {
                    MenuFunctions.CurrentCharacter.PedTatttoos.TorsoTattoos = new List<KeyValuePair<string, string>>();
                }
                if (MenuFunctions.CurrentCharacter.PedTatttoos.LeftArmTattoos == null)
                {
                    MenuFunctions.CurrentCharacter.PedTatttoos.LeftArmTattoos = new List<KeyValuePair<string, string>>();
                }
                if (MenuFunctions.CurrentCharacter.PedTatttoos.RightArmTattoos == null)
                {
                    MenuFunctions.CurrentCharacter.PedTatttoos.RightArmTattoos = new List<KeyValuePair<string, string>>();
                }
                if (MenuFunctions.CurrentCharacter.PedTatttoos.LeftLegTattoos == null)
                {
                    MenuFunctions.CurrentCharacter.PedTatttoos.LeftLegTattoos = new List<KeyValuePair<string, string>>();
                }
                if (MenuFunctions.CurrentCharacter.PedTatttoos.RightLegTattoos == null)
                {
                    MenuFunctions.CurrentCharacter.PedTatttoos.RightLegTattoos = new List<KeyValuePair<string, string>>();
                }
                if (MenuFunctions.CurrentCharacter.PedTatttoos.BadgeTattoos == null)
                {
                    MenuFunctions.CurrentCharacter.PedTatttoos.BadgeTattoos = new List<KeyValuePair<string, string>>();
                }

                foreach (var tattoo in MenuFunctions.CurrentCharacter.PedTatttoos.HeadTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }
                foreach (var tattoo in MenuFunctions.CurrentCharacter.PedTatttoos.TorsoTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }
                foreach (var tattoo in MenuFunctions.CurrentCharacter.PedTatttoos.LeftArmTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }
                foreach (var tattoo in MenuFunctions.CurrentCharacter.PedTatttoos.RightArmTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }
                foreach (var tattoo in MenuFunctions.CurrentCharacter.PedTatttoos.LeftLegTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }
                foreach (var tattoo in MenuFunctions.CurrentCharacter.PedTatttoos.RightLegTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }
                foreach (var tattoo in MenuFunctions.CurrentCharacter.PedTatttoos.BadgeTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }
                #endregion
            }

            // Set the facial expression, or set it to 'normal' if it wasn't saved/set before.
            SetFacialIdleAnimOverride(Game.PlayerPed.Handle, MenuFunctions.CurrentCharacter.FacialExpression ?? facial_expressions[0], null);
        }

        /// <summary>
        /// Create the menu if it doesn't exist, and then returns it.
        /// </summary>
        /// <returns>The Menu</returns>
        public Menu GetMenu()
        {
            if (MenuFunctions.CharacterCreatorMenu == null)
            {
                CreateMenu();
            }
            return MenuFunctions.CharacterCreatorMenu;
        }

    }
}