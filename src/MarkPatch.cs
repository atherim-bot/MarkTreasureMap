using HarmonyLib;
using UnityEngine;

namespace src;

[HarmonyPatch]
public class MarkPatch
{
    [HarmonyPostfix, HarmonyPatch(typeof(LayerTreasureMap), nameof(LayerTreasureMap.SetMap))]
    public static void MarkTreasureMapOnOpen(ref TraitScrollMapTreasure trait)
    {
        if (Game.Instance.activeZone is not Region region) return;

        var destination = trait.GetDest();
        MarkOnMap(region, destination.x, destination.z);
    }

    [HarmonyPrefix, HarmonyPatch(typeof(TaskDig), nameof(TaskDig.OnProgressComplete))]
    public static void UnmarkOnDig(TaskDig __instance)
    {
        var treasureMap = __instance.GetTreasureMap();
        if (treasureMap?.trait is null || Game.Instance.activeZone is not Region region) return;

        var map = treasureMap.trait as TraitScrollMapTreasure;
        UnmarkOnMap(region, map.GetDest().x, map.GetDest().z);
    }

    private static void MarkOnMap(Region region, int x, int y)
    {
        // Putting mark on cloudmap to avoid conflict wih existing map tiles
        // Will work until they add treasures in the sky :D
        var cloudMap = region.elomap.cloudmap;
        var goldColor = new Color32(255, 215, 0, 255);
        cloudMap.SetTileColor(x, y, goldColor);
        uint flagTileID = 306;
        cloudMap.SetTileData(x, y, flagTileID);
        cloudMap.Refresh();
    }

    private static void UnmarkOnMap(Region region, int x, int y)
    {
        var cloudMap = region.elomap.cloudmap;
        var defaultCloudColor = new Color32(255, 255, 255, 255);
        cloudMap.SetTileColor(x, y, defaultCloudColor);
        uint defaultCloudID = 4294967295;
        cloudMap.SetTileData(x, y, defaultCloudID);
        cloudMap.Refresh();
    }
}