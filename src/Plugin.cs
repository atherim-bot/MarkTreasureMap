using System.Reflection;
using BepInEx;
using HarmonyLib;

namespace src;
public static class ModInfo
{
    public const string Guid = "atherim.marktreasuremap";
    public const string Name = "Mark Treasure Map";
    public const string Version = "1.0.0";
}

[BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
internal class Plugin : BaseUnityPlugin
{
    internal static Plugin? Instance;

    private void Awake()
    {
        Instance = this;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), ModInfo.Guid);
    }

    internal static void LogInfo(object message)
    {
        Instance?.Logger.LogInfo(message);
    }
}