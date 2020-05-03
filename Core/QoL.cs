﻿using Harmony;
using MelonLoader;
using NET_SDK;
using NET_SDK.Harmony;
using Notorious;
using Notorious.API;
using QoL.API;
using QoL.Mods;
using QoL.Settings;
using QoL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnhollowerBaseLib;
using UnityEngine;
using VRC;
using VRC.SDKBase;
using Random = System.Random;

namespace QoL
{
    public class QoL : MelonMod
    {
        public static List<VRCMod> Modules = new List<VRCMod>();
        public override void OnApplicationStart()
        {
            Configuration.LoadConfiguration();

            Modules.Add(new Protections());
            Modules.Add(new UIButtons());
            Modules.Add(new InputHandler());

            MelonModLogger.Log("=========== KEYBINDS ==============");
            MelonModLogger.Log("F9  - Clone Selected Avatar");
            MelonModLogger.Log("F10 - Enable/Disable Flight");
            MelonModLogger.Log("F11 - Enable/Disable Selected ESP");
            MelonModLogger.Log("===================================");  
        }
        public override void VRChat_OnUiManagerInit()
        {
            Modules.ForEach(y => y.OnStart());
        }
        public override void OnUpdate()
        {
            List<VRCMod> Mods = Modules.Where(x => x.Name == "Input Handler" || x.Name == "UI Buttons").ToList();
            Mods.ForEach(x => x.OnUpdate());
        }
    }
}
