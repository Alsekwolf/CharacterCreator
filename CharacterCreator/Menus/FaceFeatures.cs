using System.Collections.Generic;
using CharacterCreator.CommonFunctions;
using CitizenFX.Core;
using MenuAPI;
using static CitizenFX.Core.Native.API;

namespace CharacterCreator.Menus
{
    internal class FaceFeatures
    {
        public static readonly Menu FaceFeaturesMenu = new Menu("Character Face Features", "Character Face Features");
        public static readonly MenuItem FaceFeaturesButton = new MenuItem("Character Face Features", "Character face features.");
        
        public static void CreateMenu()
        {
            //Creating face features Menu
            MenuController.AddMenu(FaceFeaturesMenu);
            
            #region face features menu
            foreach (MenuSliderItem item in FaceFeaturesMenu.GetMenuItems())
            {
                if (Functions.IsEdidtingPed)
                {
                    if (Functions.CurrentCharacter.FaceShapeFeatures.features == null)
                    {
                        Functions.CurrentCharacter.FaceShapeFeatures.features = new Dictionary<int, float>();
                    }
                    else
                    {
                        if (Functions.CurrentCharacter.FaceShapeFeatures.features.ContainsKey(item.Index))
                        {
                            item.Position = (int)(Functions.CurrentCharacter.FaceShapeFeatures.features[item.Index] * 10f) + 10;
                            SetPedFaceFeature(Game.PlayerPed.Handle, item.Index, Functions.CurrentCharacter.FaceShapeFeatures.features[item.Index]);
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
                FaceFeaturesMenu.AddMenuItem(faceFeature);
            }

            FaceFeaturesMenu.OnSliderPositionChange += (sender, sliderItem, oldPosition, newPosition, itemIndex) =>
            {
                if (Functions.CurrentCharacter.FaceShapeFeatures.features == null)
                {
                    Functions.CurrentCharacter.FaceShapeFeatures.features = new Dictionary<int, float>();
                }
                float value = faceFeaturesValuesList[newPosition];
                Functions.CurrentCharacter.FaceShapeFeatures.features[itemIndex] = value;
                SetPedFaceFeature(Game.PlayerPed.Handle, itemIndex, value);
            };

            #endregion
        }
    }
}