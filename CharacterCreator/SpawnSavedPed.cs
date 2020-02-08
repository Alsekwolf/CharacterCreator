using System.Collections.Generic;
using System.Threading.Tasks;
using CharacterCreator.CommonFunctions;
using CitizenFX.Core;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;

namespace CharacterCreator
{
    public partial class MenuFunctions 
    {
        public static async Task LoadPed(string json)
        { 
            DataManager.MultiplayerPedData loadCharacter = JsonConvert.DeserializeObject<DataManager.MultiplayerPedData>(json);

            
            if (IsModelInCdimage(loadCharacter.ModelHash))
            {
                if (!HasModelLoaded(loadCharacter.ModelHash))
                {
                    RequestModel(loadCharacter.ModelHash);
                    while (!HasModelLoaded(loadCharacter.ModelHash))
                    {
                        await BaseScript.Delay(0);
                    }
                }

                // for some weird reason, using SetPlayerModel here does not work, it glitches out and makes the player have what seems to be both male
                // and female ped at the same time.. really fucking weird. Only the CommonFunctions.SetPlayerSkin function seems to work some how. I really have no clue.
                await SetPlayerSkin.SetPlayerSkinFunction(loadCharacter.ModelHash, new SetPlayerSkin.PedInfo() { version = -1 }, false);
                // SetPlayerModel(Game.PlayerPed.Handle, currentCharacter.IsMale ? (uint)GetHashKey("mp_m_freemode_01") : (uint)GetHashKey("mp_f_freemode_01"));
                // SetPlayerModel(Game.PlayerPed.Handle, currentCharacter.ModelHash);

                ClearPedDecorations(Game.PlayerPed.Handle);
                ClearPedFacialDecorations(Game.PlayerPed.Handle);
                SetPedDefaultComponentVariation(Game.PlayerPed.Handle);
                SetPedHairColor(Game.PlayerPed.Handle, 0, 0);
                SetPedEyeColor(Game.PlayerPed.Handle, 0);
                ClearAllPedProps(Game.PlayerPed.Handle);

                #region headblend
                var data = loadCharacter.PedHeadBlendData;
                SetPedHeadBlendData(Game.PlayerPed.Handle, data.FirstFaceShape, data.SecondFaceShape, data.ThirdFaceShape, data.FirstSkinTone, data.SecondSkinTone, data.ThirdSkinTone, data.ParentFaceShapePercent, data.ParentSkinTonePercent, 0f, data.IsParentInheritance);

                while (!HasPedHeadBlendFinished(Game.PlayerPed.Handle))
                {
                    await BaseScript.Delay(0);
                }
                #endregion

                #region appearance
                var appData = loadCharacter.PedAppearance;
                
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

                if (loadCharacter.FaceShapeFeatures.features != null)
                {
                    foreach (var t in loadCharacter.FaceShapeFeatures.features)
                    {
                        SetPedFaceFeature(Game.PlayerPed.Handle, t.Key, t.Value);
                    }
                }
                else
                {
                    loadCharacter.FaceShapeFeatures.features = new Dictionary<int, float>();
                }

                #endregion

                #region Clothing Data
                if (loadCharacter.DrawableVariations.Clothes != null && loadCharacter.DrawableVariations.Clothes.Count > 0)
                {
                    foreach (var cd in loadCharacter.DrawableVariations.Clothes)
                    {
                        SetPedComponentVariation(Game.PlayerPed.Handle, cd.Key, cd.Value.Key, cd.Value.Value, 0);
                    }
                }
                #endregion

                #region Props Data
                if (loadCharacter.PropVariations.Props != null && loadCharacter.PropVariations.Props.Count > 0)
                {
                    foreach (var cd in loadCharacter.PropVariations.Props)
                    {
                        if (cd.Value.Key > -1)
                        {
                            SetPedPropIndex(Game.PlayerPed.Handle, cd.Key, cd.Value.Key, cd.Value.Value > -1 ? cd.Value.Value : 0, true);
                        }
                    }
                }
                #endregion

                #region Tattoos

                if (loadCharacter.PedTatttoos.HeadTattoos == null)
                {
                    loadCharacter.PedTatttoos.HeadTattoos = new List<KeyValuePair<string, string>>();
                }
                if (loadCharacter.PedTatttoos.TorsoTattoos == null)
                {
                    loadCharacter.PedTatttoos.TorsoTattoos = new List<KeyValuePair<string, string>>();
                }
                if (loadCharacter.PedTatttoos.LeftArmTattoos == null)
                {
                    loadCharacter.PedTatttoos.LeftArmTattoos = new List<KeyValuePair<string, string>>();
                }
                if (loadCharacter.PedTatttoos.RightArmTattoos == null)
                {
                    loadCharacter.PedTatttoos.RightArmTattoos = new List<KeyValuePair<string, string>>();
                }
                if (loadCharacter.PedTatttoos.LeftLegTattoos == null)
                {
                    loadCharacter.PedTatttoos.LeftLegTattoos = new List<KeyValuePair<string, string>>();
                }
                if (loadCharacter.PedTatttoos.RightLegTattoos == null)
                {
                    loadCharacter.PedTatttoos.RightLegTattoos = new List<KeyValuePair<string, string>>();
                }
                if (loadCharacter.PedTatttoos.BadgeTattoos == null)
                {
                    loadCharacter.PedTatttoos.BadgeTattoos = new List<KeyValuePair<string, string>>();
                }

                foreach (var tattoo in loadCharacter.PedTatttoos.HeadTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }
                foreach (var tattoo in loadCharacter.PedTatttoos.TorsoTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }
                foreach (var tattoo in loadCharacter.PedTatttoos.LeftArmTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }
                foreach (var tattoo in loadCharacter.PedTatttoos.RightArmTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }
                foreach (var tattoo in loadCharacter.PedTatttoos.LeftLegTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }
                foreach (var tattoo in loadCharacter.PedTatttoos.RightLegTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }
                foreach (var tattoo in loadCharacter.PedTatttoos.BadgeTattoos)
                {
                    SetPedDecoration(Game.PlayerPed.Handle, (uint)GetHashKey(tattoo.Key), (uint)GetHashKey(tattoo.Value));
                }
                #endregion
            }

            // Set the facial expression, or set it to 'normal' if it wasn't saved/set before.
            SetFacialIdleAnimOverride(Game.PlayerPed.Handle, loadCharacter.FacialExpression ?? DataManager.FacialExpressions[0], null);    
        }
    }
}