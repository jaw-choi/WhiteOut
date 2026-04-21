using UnityEngine;

namespace WhiteOut.Core
{
    public static class GameLayers
    {
        public const string Player = "Player";
        public const string Campfire = "Campfire";
        public const string WoodPickup = "WoodPickup";
        public const string CampfireHeatZone = "CampfireHeatZone";

        public static int ToLayer(string layerName)
        {
            return LayerMask.NameToLayer(layerName);
        }

        public static LayerMask ToMask(params string[] layerNames)
        {
            return LayerMask.GetMask(layerNames);
        }
    }
}
