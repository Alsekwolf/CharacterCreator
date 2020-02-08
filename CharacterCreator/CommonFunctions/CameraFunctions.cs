using System.Collections.Generic;
using System.Threading.Tasks;
using CharacterCreator.Menus;
using CitizenFX.Core;
using MenuAPI;
using static CitizenFX.Core.Native.API;

namespace CharacterCreator.CommonFunctions
{
    internal class CameraFunctions
    {
        private static bool reverseCamera = false;
        private static Camera camera;
        internal static float CameraFov { get; set; } = 45;
        internal static int CurrentCam { get; set; }

        public static async Task ManageCamera()
        {
            await BaseScript.Delay(500);
            var menu = MenuController.GetCurrentMenu();
                
                if (Functions.IsOpen())
                {
                    if (!HasAnimDictLoaded("anim@random@shop_clothes@watches"))
                    {
                        RequestAnimDict("anim@random@shop_clothes@watches");
                    }
                    while (!HasAnimDictLoaded("anim@random@shop_clothes@watches"))
                    {
                        await BaseScript.Delay(0);
                    }

                    while (Functions.IsOpen())
                    {
                        await BaseScript.Delay(0);

                        int index = GetCameraIndex(MenuController.GetCurrentMenu());
                        if (MenuController.GetCurrentMenu() == Props.PropsMenu && MenuController.GetCurrentMenu().CurrentIndex == 3 && !reverseCamera)
                        {
                            TaskPlayAnim(Game.PlayerPed.Handle, "anim@random@shop_clothes@watches", "BASE", 8f, -8f, -1, 1, 0, false, false, false);
                        }
                        else
                        {
                            Game.PlayerPed.Task.ClearAll();
                        }

                        var xOffset = 0f;
                        var yOffset = 0f;

                        if ((Game.IsControlPressed(0, Control.ParachuteBrakeLeft) || Game.IsControlPressed(0, Control.ParachuteBrakeRight)) && !(Game.IsControlPressed(0, Control.ParachuteBrakeLeft) && Game.IsControlPressed(0, Control.ParachuteBrakeRight)))
                        {
                            switch (index)
                            {
                                case 0:
                                    xOffset = 2.2f;
                                    yOffset = -1f;
                                    break;
                                case 1:
                                    xOffset = 0.7f;
                                    yOffset = -0.45f;
                                    break;
                                case 2:
                                    xOffset = 1.35f;
                                    yOffset = -0.4f;
                                    break;
                                case 3:
                                    xOffset = 1.0f;
                                    yOffset = -0.4f;
                                    break;
                                case 4:
                                    xOffset = 0.9f;
                                    yOffset = -0.4f;
                                    break;
                                case 5:
                                    xOffset = 0.8f;
                                    yOffset = -0.7f;
                                    break;
                                case 6:
                                    xOffset = 1.5f;
                                    yOffset = -1.0f;
                                    break;
                                default:
                                    xOffset = 0f;
                                    yOffset = 0.2f;
                                    break;
                            }
                            if (Game.IsControlPressed(0, Control.ParachuteBrakeRight))
                            {
                                xOffset *= -1f;
                            }

                        }

                        Vector3 pos;
                        if (reverseCamera)
                            pos = GetOffsetFromEntityInWorldCoords(Game.PlayerPed.Handle, (CameraOffsets[index].Key.X + xOffset) * -1f, (CameraOffsets[index].Key.Y + yOffset) * -1f, CameraOffsets[index].Key.Z);
                        else
                            pos = GetOffsetFromEntityInWorldCoords(Game.PlayerPed.Handle, (CameraOffsets[index].Key.X + xOffset), (CameraOffsets[index].Key.Y + yOffset), CameraOffsets[index].Key.Z);
                        Vector3 pointAt = GetOffsetFromEntityInWorldCoords(Game.PlayerPed.Handle, CameraOffsets[index].Value.X, CameraOffsets[index].Value.Y, CameraOffsets[index].Value.Z);

                        if (Game.IsControlPressed(0, Control.MoveLeftOnly))
                        {
                            Game.PlayerPed.Task.LookAt(GetOffsetFromEntityInWorldCoords(Game.PlayerPed.Handle, 1.2f, .5f, .7f), 1100);
                        }
                        else if (Game.IsControlPressed(0, Control.MoveRightOnly))
                        {
                            Game.PlayerPed.Task.LookAt(GetOffsetFromEntityInWorldCoords(Game.PlayerPed.Handle, -1.2f, .5f, .7f), 1100);
                        }
                        else
                        {
                            Game.PlayerPed.Task.LookAt(GetOffsetFromEntityInWorldCoords(Game.PlayerPed.Handle, 0f, .5f, .7f), 1100);
                        }

                        if (Game.IsControlJustReleased(0, Control.Jump))
                        {
                            var Pos = Game.PlayerPed.Position;
                            SetEntityCollision(Game.PlayerPed.Handle, true, true);
                            FreezeEntityPosition(Game.PlayerPed.Handle, false);
                            TaskGoStraightToCoord(Game.PlayerPed.Handle, Pos.X, Pos.Y, Pos.Z, 8f, 1600, Game.PlayerPed.Heading + 180f, 0.1f);
                            int timer = GetGameTimer();
                            while (true)
                            {
                                await BaseScript.Delay(0);
                                //DisplayRadar(false);
                                Game.DisableAllControlsThisFrame(0);
                                if (GetGameTimer() - timer > 1600)
                                {
                                    break;
                                }
                            }
                            ClearPedTasks(Game.PlayerPed.Handle);
                            Game.PlayerPed.PositionNoOffset = Pos;
                            FreezeEntityPosition(Game.PlayerPed.Handle, true);
                            SetEntityCollision(Game.PlayerPed.Handle, false, false);
                            reverseCamera = !reverseCamera;
                        }

                        SetEntityCollision(Game.PlayerPed.Handle, false, false);
                        //Game.PlayerPed.IsInvincible = true;
                        Game.PlayerPed.IsPositionFrozen = true;

                        if (!DoesCamExist(CurrentCam))
                        {
                            CurrentCam = CreateCam("DEFAULT_SCRIPTED_CAMERA", true);
                            camera = new Camera(CurrentCam)
                            {
                                Position = pos,
                                FieldOfView = CameraFov
                            };
                            camera.PointAt(pointAt);
                            RenderScriptCams(true, false, 0, false, false);
                            camera.IsActive = true;
                        }
                        else
                        {
                            if (camera.Position != pos)
                            {
                                await AlsekLib.CommonFunctionsLib.MoveCamToNewSpot(camera, pos, pointAt);
                            }
                        }
                    }

                    SetEntityCollision(Game.PlayerPed.Handle, true, true);

                    Game.PlayerPed.IsPositionFrozen = false;

                    DisplayHud(true);
                    DisplayRadar(true);

                    if (HasAnimDictLoaded("anim@random@shop_clothes@watches"))
                    {
                        RemoveAnimDict("anim@random@shop_clothes@watches");
                    }

                    reverseCamera = false;
                }
                else
                {
                    if (camera != null)
                    {
                        ClearCamera();
                        camera = null;
                    }
                }
            
        }

        private static int GetCameraIndex(Menu menu)
        {
            if (menu != null)
            {
                if (menu == Inheritance.InheritanceMenu)
                {
                    return 1;
                }
                else if (menu == Clothing.ClothingMenu)
                {
                    switch (menu.CurrentIndex)
                    {
                        case 0: // masks
                            return 1;
                        case 1: // upper body
                            return 2;
                        case 2: // lower body
                            return 3;
                        case 3: // bags & parachutes
                            return 2;
                        case 4: // shoes
                            return 4;
                        case 5: // scarfs & chains
                            return 2;
                        case 6: // shirt & accessory
                            return 2;
                        case 7: // body armor & accessory
                            return 2;
                        case 8: // badges & logos
                            return 0;
                        case 9: // shirt overlay & jackets
                            return 2;
                        default:
                            return 0;
                    }
                }
                else if (menu == Props.PropsMenu)
                {
                    switch (menu.CurrentIndex)
                    {
                        case 0: // hats & helmets
                        case 1: // glasses
                        case 2: // misc props
                            return 1;
                        case 3: // watches
                            return reverseCamera ? 5 : 6;
                        case 4: // bracelets
                            return 5;
                        default:
                            return 0;
                    }
                }
                else if (menu == Appearance.AppearanceMenu)
                {
                    switch (menu.CurrentIndex)
                    {
                        case 0: // hair style
                        case 1: // hair color
                        case 2: // hair highlight color
                        case 3: // blemishes
                        case 4: // blemishes opacity
                        case 5: // beard style
                        case 6: // beard opacity
                        case 7: // beard color
                        case 8: // eyebrows style
                        case 9: // eyebrows opacity
                        case 10: // eyebrows color
                        case 11: // ageing style
                        case 12: // ageing opacity
                        case 13: // makeup style
                        case 14: // makeup opacity
                        case 15: // makeup color
                        case 16: // blush style
                        case 17: // blush opacity
                        case 18: // blush color
                        case 19: // complexion style
                        case 20: // complexion opacity
                        case 21: // sun damage style
                        case 22: // sun damage opacity
                        case 23: // lipstick style
                        case 24: // lipstick opacity
                        case 25: // lipstick color
                        case 26: // moles and freckles style
                        case 27: // moles and freckles opacity
                            return 1;
                        case 28: // chest hair style
                        case 29: // chest hair opacity
                        case 30: // chest hair color
                        case 31: // body blemishes style
                        case 32: // body blemishes opacity
                            return 2;
                        case 33: // eye colors
                            return 1;
                        default:
                            return 0;
                    }
                }
                /*else if (menu == MainMenu.MpPedCustomizationMenu.tattoosMenu)
                { TODO: Delete this?
                    switch (menu.CurrentIndex)
                    {
                        case 0: // head
                            return 1;
                        case 1: // torso
                            return 2;
                        case 2: // left arm
                        case 3: // right arm
                            return 6;
                        case 4: // left leg 
                        case 5: // right leg
                            return 3;
                        case 6: // badges
                            return 2;
                        default:
                            return 0;
                    }
                }*/
                else if (menu == FaceFeatures.FaceFeaturesMenu)
                {
                    MenuItem item = menu.GetCurrentMenuItem();
                    if (item != null)
                    {
                        if (item.GetType() == typeof(MenuSliderItem))
                        {
                            return 1;
                        }
                    }
                    return 0;
                }
            }
            return 0;
        }

        internal static void ClearCamera()
        {
            camera.IsActive = false;
            RenderScriptCams(false, false, 0, false, false);
            DestroyCam(CurrentCam, false);
            CurrentCam = -1;
            camera.Delete();
        }

        internal static List<KeyValuePair<Vector3, Vector3>> CameraOffsets { get; } = new List<KeyValuePair<Vector3, Vector3>>()
        {
            // Full body
            new KeyValuePair<Vector3, Vector3>(new Vector3(0f, 2.8f, 0.3f), new Vector3(0f, 0f, 0f)),

            // Head level
            new KeyValuePair<Vector3, Vector3>(new Vector3(0f, 0.9f, 0.65f), new Vector3(0f, 0f, 0.6f)),

            // Upper Body
            new KeyValuePair<Vector3, Vector3>(new Vector3(0f, 1.4f, 0.5f), new Vector3(0f, 0f, 0.3f)),

            // Lower Body
            new KeyValuePair<Vector3, Vector3>(new Vector3(0f, 1.6f, -0.3f), new Vector3(0f, 0f, -0.45f)),

            // Shoes
            new KeyValuePair<Vector3, Vector3>(new Vector3(0f, 0.98f, -0.7f), new Vector3(0f, 0f, -0.90f)),

            // Lower Arms
            new KeyValuePair<Vector3, Vector3>(new Vector3(0f, 0.98f, 0.1f), new Vector3(0f, 0f, 0f)),

            // Full arms
            new KeyValuePair<Vector3, Vector3>(new Vector3(0f, 1.3f, 0.35f), new Vector3(0f, 0f, 0.15f)),
        };
    }
}