using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using LethalNetworkAPI;
using UnityEngine;

namespace SelfDestruct;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.rune580.LethalCompanyInputUtils")]
[BepInDependency("LethalNetworkAPI")]
public class Plugin : BaseUnityPlugin
{
	private static LethalClientMessage<Vector3> SelfDestructMessage = new LethalClientMessage<Vector3>("SelfDestruct", onReceivedFromClient: ReceiveSelfDestruct);

	public static new Config Config { get; private set; }
	public static new ManualLogSource Logger { get; private set; }

	public static PluginInput Input { get; private set; }

	private void Awake()
	{
		Config = new(base.Config);
		Logger = base.Logger;

		Input = new PluginInput();

		Harmony.CreateAndPatchAll(typeof(Plugin), PluginInfo.PLUGIN_GUID);

		// Plugin startup logic
		Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
	}

	[HarmonyPatch(typeof(PlayerControllerB), "Update")]
	[HarmonyPostfix]
	private static void PlayerControllerB_Update(PlayerControllerB __instance)
	{
		if (!__instance.IsOwner || !__instance.isPlayerControlled || (__instance.IsServer && !__instance.isHostPlayerObject))
		{
			return;
		}
		if (!__instance.AllowPlayerDeath())
		{
			return;
		}
		if (!Input.SelfDestruct.triggered)
		{
			return;
		}

		var position = __instance.transform.position;

		var launchForce = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized * Plugin.Config.BodyLaunchForce.Value;
		__instance.KillPlayer(launchForce, true, CauseOfDeath.Blast);

		SelfDestructMessage.SendAllClients(position);
	}

	private static void ReceiveSelfDestruct(Vector3 position, ulong clientId)
	{
		Landmine.SpawnExplosion(position, true, Plugin.Config.LethalRadius.Value, Plugin.Config.DamageRadius.Value, Plugin.Config.NonLethalDamage.Value);
	}
}