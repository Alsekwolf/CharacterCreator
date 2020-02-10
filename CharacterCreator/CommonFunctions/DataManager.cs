using System.Collections.Generic;
using CitizenFX.Core;

namespace CharacterCreator.CommonFunctions
{
    public static class DataManager
    {
        public struct DrawableVariations
        {
            public Dictionary<int, KeyValuePair<int, int>> clothes;
        }

        public struct PropVariations
        {
            public Dictionary<int, KeyValuePair<int, int>> props;
        }

        public struct FaceShapeFeatures
        {
            public Dictionary<int, float> features;
        }

        public struct PedTattoos
        {
            public List<KeyValuePair<string, string>> TorsoTattoos;
            public List<KeyValuePair<string, string>> HeadTattoos;
            public List<KeyValuePair<string, string>> LeftArmTattoos;
            public List<KeyValuePair<string, string>> RightArmTattoos;
            public List<KeyValuePair<string, string>> LeftLegTattoos;
            public List<KeyValuePair<string, string>> RightLegTattoos;
            public List<KeyValuePair<string, string>> BadgeTattoos;
        }

        // probably won't be needed, since there's already makeup and tattoos now.
        public struct PedFacePaints { }

        public struct PedAppearance
        {
            public int HairStyle;
            public int HairColor;
            public int HairHighlightColor;
            public KeyValuePair<string, string> HairOverlay;

            // 0 blemishes
            public int BlemishesStyle;
            public float BlemishesOpacity;

            // 1 beard
            public int BeardStyle;
            public int BeardColor;
            public float BeardOpacity;

            // 2 eyebrows
            public int EyebrowsStyle;
            public int EyebrowsColor;
            public float EyebrowsOpacity;

            // 3 ageing
            public int AgeingStyle;
            public float AgeingOpacity;

            // 4 makeup
            public int MakeupStyle;
            public int MakeupColor;
            public float MakeupOpacity;

            // 5 blush
            public int BlushStyle;
            public int BlushColor;
            public float BlushOpacity;

            // 6 complexion
            public int ComplexionStyle;
            public float ComplexionOpacity;

            // 7 sun damage
            public int SunDamageStyle;
            public float SunDamageOpacity;

            // 8 lipstick
            public int LipstickStyle;
            public int LipstickColor;
            public float LipstickOpacity;

            // 9 moles / freckles
            public int MolesFrecklesStyle;
            public float MolesFrecklesOpacity;

            // 10 chest hair
            public int ChestHairStyle;
            public int ChestHairColor;
            public float ChestHairOpacity;

            // 11 body blemishes
            public int BodyBlemishesStyle;
            public float BodyBlemishesOpacity;

            // eye color
            public int EyeColor;
        }

        public struct MultiplayerPedData
        {
            public PedHeadBlendData PedHeadBlendData;
            public DrawableVariations DrawableVariations;
            public PropVariations PropVariations;
            public FaceShapeFeatures FaceShapeFeatures;
            public PedAppearance PedAppearance;
            public PedTattoos PedTatttoos;
            public PedFacePaints PedFacePaints;
            public bool IsMale;
            public uint ModelHash;
            public string SaveName;
            public int Version;
            public string WalkingStyle;
            public string FacialExpression;
        }
    }
}