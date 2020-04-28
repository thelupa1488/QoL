using BestHTTP;
using MelonLoader;
using NET_SDK;
using Notorious;
using QoL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.SDKBase;
using VRC.UI;

namespace QoL.Mods
{
    public class InputHandler : VRCMod
    {
        public override string Name => "Input Handler";

        public override string Description => "This module handles all the given user input.";

        public InputHandler() : base() { }

        public override void OnStart() { }

        private static UserInteractMenu CachedUserInteract { get; set; }

        public override void OnUpdate()
        {

            if (Input.GetKeyDown(KeyCode.F9))
            {
                var avi = Wrappers.GetQuickMenu().GetSelectedPlayer().field_VRCPlayer_0.prop_ApiAvatar_0;

                if (avi.releaseStatus != "private")
                {
                    VRC.Core.API.SendRequest($"avatars/{avi.id}", VRC.Core.BestHTTP.HTTPMethods.Get, new ApiModelContainer<ApiAvatar>(), null, true, false, 3600f, 2, null);

                    new PageAvatar
                    {
                        avatar = new SimpleAvatarPedestal
                        {
                            field_ApiAvatar_0 = new ApiAvatar
                            {
                                id = avi.id
                            }
                        }
                    }.ChangeToSelectedAvatar();
                }
            }

            if (Input.GetKeyDown(KeyCode.F10))
            {
                GlobalUtils.DirectionalFlight = !GlobalUtils.DirectionalFlight;
                Physics.gravity = GlobalUtils.DirectionalFlight ? Vector3.zero : GlobalUtils.Grav;
                if (!GlobalUtils.DirectionalFlight) GlobalUtils.ToggleColliders(true);
                UIButtons.ToggleUIButton(0, GlobalUtils.DirectionalFlight);
                MelonModLogger.Log($"Flight has been {(GlobalUtils.DirectionalFlight ? "Enabled" : "Disabled")}.");
            }
            if (Input.GetKeyDown(KeyCode.F11))
            {
                GlobalUtils.SelectedPlayerESP = !GlobalUtils.SelectedPlayerESP;
                MelonModLogger.Log($"Selected ESP has been {(GlobalUtils.SelectedPlayerESP ? "Enabled" : "Disabled")}.");
                UIButtons.ToggleUIButton(1, GlobalUtils.SelectedPlayerESP);
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
            }

            if (GlobalUtils.DirectionalFlight)
            {
                GameObject gameObject = Wrappers.GetPlayerCamera();
                var currentSpeed = (Input.GetKey(KeyCode.LeftShift) ? 16f : 8f);
                var player = PlayerWrappers.GetCurrentPlayer();

                if (Input.GetKey(KeyCode.W))
                {
                    player.transform.position += gameObject.transform.forward * currentSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    player.transform.position += player.transform.right * -1f * currentSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    player.transform.position += gameObject.transform.forward * -1f * currentSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    player.transform.position += player.transform.right * currentSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.Space))
                {
                    player.transform.position += player.transform.up * currentSpeed * Time.deltaTime;
                }
                if (Math.Abs(Input.GetAxis("Joy1 Axis 2")) > 0f)
                {
                    player.transform.position += gameObject.transform.forward * currentSpeed * Time.deltaTime * (Input.GetAxis("Joy1 Axis 2") * -1f);
                }
                if (Math.Abs(Input.GetAxis("Joy1 Axis 1")) > 0f)
                {
                    player.transform.position += gameObject.transform.right * currentSpeed * Time.deltaTime * Input.GetAxis("Joy1 Axis 1");
                }
            }
        }
    }
}
