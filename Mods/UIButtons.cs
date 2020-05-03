using MelonLoader;
using Notorious;
using Notorious.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using QoL.Utils;
using QoL.API;
using VRC.SDKBase;
using Photon.Pun;
using UnityEngine.UI;
using QoL.Settings;
using VRC.Core;
using VRC.Core.BestHTTP;
using VRC;
using VRC.UI;

namespace QoL.Mods
{
    public class UIButtons : VRCMod
    {
        public static List<GameObject> Buttons = new List<GameObject>();

        public override string Name => "UI Buttons";

        public override string Description => "This module adds more buttons to the menu.";

        public UIButtons() : base() {}

        public override void OnStart() 
        {
            try
            {
                if (Buttons.Count() == 0)
                {
                    Transform parent = Wrappers.GetQuickMenu().transform.Find("UIElementsMenu");

                    if (parent != null)
                    {
                        var Flightbutton = ButtonAPI.CreateButton(ButtonType.Toggle, "Flight", "Enable/Disable Flight", Color.white, Color.blue, -1, 0, parent, new Action(() =>
                        {
                            GlobalUtils.DirectionalFlight = true;
                            Physics.gravity = GlobalUtils.DirectionalFlight ? Vector3.zero : GlobalUtils.Grav;
                            GlobalUtils.ToggleColliders(false);
                            MelonModLogger.Log($"Flight has been {(GlobalUtils.DirectionalFlight ? "Enabled" : "Disabled")}.");
                        }), new Action(() =>
                        {
                            GlobalUtils.DirectionalFlight = false;
                            Physics.gravity = GlobalUtils.DirectionalFlight ? Vector3.zero : GlobalUtils.Grav;
                            GlobalUtils.ToggleColliders(true);
                            MelonModLogger.Log($"Flight has been {(GlobalUtils.DirectionalFlight ? "Enabled" : "Disabled")}.");
                        }));

                        var ESPbutton = ButtonAPI.CreateButton(ButtonType.Toggle, "ESP", "Enable/Disable Selected ESP", Color.white, Color.blue, 0, 0, parent, new Action(() =>
                        {
                            GlobalUtils.SelectedPlayerESP = true;
                            MelonModLogger.Log($"Selected ESP has been {(GlobalUtils.SelectedPlayerESP ? "Enabled" : "Disabled")}.");

                            GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
                            for (int i = 0; i < array.Length; i++)
                            {
                                if (array[i].transform.Find("SelectRegion"))
                                {
                                    array[i].transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.magenta);
                                    HighlightsFX.prop_HighlightsFX_0.EnableOutline(array[i].transform.Find("SelectRegion").GetComponent<Renderer>(), GlobalUtils.SelectedPlayerESP);
                                }
                            }

                            foreach (VRC_Pickup pickup in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                            {
                                if (pickup.gameObject.transform.Find("SelectRegion"))
                                {
                                    pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.magenta);
                                    Wrappers.GetHighlightsFX().EnableOutline(pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>(), GlobalUtils.SelectedPlayerESP);
                                }
                            }
                        }), new Action(() =>
                        {
                            GlobalUtils.SelectedPlayerESP = false;
                            MelonModLogger.Log($"Selected ESP has been {(GlobalUtils.SelectedPlayerESP ? "Enabled" : "Disabled")}.");

                            GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
                            for (int i = 0; i < array.Length; i++)
                            {
                                if (array[i].transform.Find("SelectRegion"))
                                {
                                    array[i].transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.magenta);
                                    HighlightsFX.prop_HighlightsFX_0.EnableOutline(array[i].transform.Find("SelectRegion").GetComponent<Renderer>(), GlobalUtils.SelectedPlayerESP);
                                }
                            }

                            foreach (VRC_Pickup pickup in Resources.FindObjectsOfTypeAll<VRC_Pickup>())
                            {
                                if (pickup.gameObject.transform.Find("SelectRegion"))
                                {
                                    pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.magenta);
                                    Wrappers.GetHighlightsFX().EnableOutline(pickup.gameObject.transform.Find("SelectRegion").GetComponent<Renderer>(), GlobalUtils.SelectedPlayerESP);
                                }
                            }
                        }));

                        var teleportButton = ButtonAPI.CreateButton(ButtonType.Default, "Teleport", "Teleport to the selected player", Color.white, Color.cyan, 0, 0, Wrappers.GetQuickMenu().transform.Find("UserInteractMenu"), new Action(() =>
                        {
                            MelonModLogger.Log("Teleporting to selected player.");
                            var player = PlayerWrappers.GetCurrentPlayer();
                            var SelectedPlayer = Wrappers.GetQuickMenu().GetSelectedPlayer();
                            player.transform.position = SelectedPlayer.transform.position;
                        }));

                        var Freezebutton = ButtonAPI.CreateButton(ButtonType.Toggle, "Serialize", "Enable/Disable Custom Serialisation", Color.white, Color.blue, -3, -1, parent, new Action(() =>
                        {
                            GlobalUtils.Serialise = false;
                            MelonModLogger.Log($"Custom Serialisation has been Enabled.");
                        }), new Action(() =>
                        {
                            GlobalUtils.Serialise = true;
                            MelonModLogger.Log($"Custom Serialisation has been Disabled.");
                        }));

                        var JumpButton = ButtonAPI.CreateButton(ButtonType.Toggle, "Jump", "Enable/disable jumping in the current world", Color.white, Color.blue, -2, -1, parent, new Action(() =>
                        {
                            if (PlayerWrappers.GetCurrentPlayer() != null)
                            {
                                if (PlayerWrappers.GetCurrentPlayer().GetComponent<PlayerModComponentJump>() == null)
                                {
                                    PlayerWrappers.GetCurrentPlayer().gameObject.AddComponent<PlayerModComponentJump>();
                                }
                            }

                            MelonModLogger.Log($"Jumping has been Enabled.");
                        }), new Action(() =>
                        {
                            if (PlayerWrappers.GetCurrentPlayer() != null)
                            {
                                if (PlayerWrappers.GetCurrentPlayer().GetComponent<PlayerModComponentJump>() != null)
                                {
                                    UnityEngine.GameObject.Destroy(PlayerWrappers.GetCurrentPlayer().GetComponent<PlayerModComponentJump>().gameObject);
                                }
                            }

                            MelonModLogger.Log($"Jumping has been Disabled.");
                        }));

                        var CloneButton = ButtonAPI.CreateButton(ButtonType.Default, "Clone\nBy\nID", "Clone an avatar by it's ID", Color.white, Color.blue, -1, 1, parent, new Action(() =>
                        {
                            MelonModLogger.Log("Enter AvatarID: ");
                            string ID = Console.ReadLine();
                            VRC.Core.API.SendRequest($"avatars/{ID}", HTTPMethods.Get, new ApiModelContainer<ApiAvatar>(), null, true, true, 3600f, 2, null);
                            new PageAvatar
                            {
                                avatar = new SimpleAvatarPedestal
                                {
                                    field_ApiAvatar_0 = new ApiAvatar
                                    {
                                        id = ID
                                    }
                                }
                            }.ChangeToSelectedAvatar();
                            Resources.FindObjectsOfTypeAll<VRCUiPopupManager>()[0].Method_Public_String_String_Single_0("<color=cyan>Success!</color>", "<color=green>Successfully cloned that avatar by It's AvtrID.</color>", 10f);
                        }), null);

                        var GotoButton = ButtonAPI.CreateButton(ButtonType.Default, "Join\nBy\nID", "Goto a world by It's ID", Color.white, Color.blue, 0, 1, parent, new Action(() =>
                        {
                            MelonModLogger.Log("Enter WorldID: ");
                            string ID = Console.ReadLine();
                            Networking.GoToRoom(ID);
                        }), null);

                        Buttons.Add(Flightbutton.gameObject);
                        Buttons.Add(ESPbutton.gameObject);
                        Buttons.Add(teleportButton.gameObject);
                        Buttons.Add(Freezebutton.gameObject);
                        Buttons.Add(JumpButton.gameObject);
                        Buttons.Add(CloneButton.gameObject);
                    }
                }
            }
            catch (Exception) { }
        }

        public static void ToggleUIButton(int Index, bool state)
        {
            var gameObject = Buttons[Index];

            if (gameObject == null) return;

            var transform = gameObject.transform;

            var EnableButton = transform.Find("Toggle_States_Visible/ON").gameObject;
            var DisableButton = transform.Find("Toggle_States_Visible/OFF").gameObject;

            if (state)
            {
                DisableButton.SetActive(false);
                EnableButton.SetActive(true);
            }
            else
            {
                EnableButton.SetActive(false);
                DisableButton.SetActive(true);
            }
        }
        public override void OnUpdate()
        {
            //var AvatarScreen = Wrappers.GetQuickMenu().transform.Find("/UserInterface/MenuContent/Screens/Avatar");

            //if (AvatarScreen != null)
            //{
            //    var FavButton = AvatarScreen.GetComponentsInChildren<Button>().Where(x => x.name.ToLower().Contains("favorite")).ToList();

            //    if (FavButton.Count() > 0)
            //    {
            //        FavButton.FirstOrDefault().GetComponentInChildren<Text>().text = "Favorite";
            //        FavButton.FirstOrDefault().GetComponentInChildren<Text>().color = Color.cyan;
            //        FavButton.FirstOrDefault().onClick = new Button.ButtonClickedEvent();
            //        FavButton.FirstOrDefault().onClick.AddListener(new Action(() =>
            //        {
            //            UiAvatarList.Method_Public_AvatarFavorites_0(VRC.Core.APIUser.AvatarFavorites.Avatars_1);
            //            UiAvatarList.Method_Public_AvatarFavorites_1(VRC.Core.APIUser.AvatarFavorites.MAX_AVATAR_FAVORITE_GROUPS);
            //        }));
            //    }
            //}
        }
    }
}
