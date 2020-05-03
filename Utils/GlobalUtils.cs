using BestHTTP.JSON;
using Notorious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;
using VRCSDK2;

namespace QoL.Utils
{
    public static class GlobalUtils
    {
        public static bool DirectionalFlight = false;

        public static Vector3 Grav = Physics.gravity;

        public static bool SelectedPlayerESP = false;

        public static bool ThirdPerson = false;

        public static bool ForceClone = false;

        public static bool Serialise = true;

        private static List<string> Colors = new List<string>()
        {
            "red",
            "green",
            "blue",
            "yellow",
            "cyan",
            "orange",
            "magenta",
            "aqua"
        };
        public static void ToggleColliders(bool toggle)
        {
            Collider[] array = UnityEngine.Object.FindObjectsOfType<Collider>();
            Component component = PlayerWrappers.GetCurrentPlayer().GetComponents(Collider.Il2CppType).FirstOrDefault<Component>();

            for (int i = 0; i < array.Length; i++)
            {
                Collider collider = array[i];
                bool flag = collider.GetComponent<PlayerSelector>() != null || collider.GetComponent<VRC.SDKBase.VRC_Pickup>() != null || collider.GetComponent<QuickMenu>() != null || collider.GetComponent<VRC_Station>() != null || collider.GetComponent<VRC.SDKBase.VRC_AvatarPedestal>() != null;
                
                if (!flag && collider != component)
                {
                    collider.enabled = toggle;
                }
            }
        }
        public static string RandomColor() { return Colors[new System.Random().Next(0, Colors.Count())]; }

        public static string BuildRainbowText(string text)
        {
            string Finished = null;
            foreach (char c in text)
            {
                Finished += $"<color={RandomColor()}>{c}</color>";
            }
            return Finished.ToString(); 
        }
    }
}
