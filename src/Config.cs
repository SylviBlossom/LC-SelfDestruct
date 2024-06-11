using BepInEx.Configuration;

namespace SelfDestruct;

public class Config
{
	public ConfigEntry<float> LethalRadius;
	public ConfigEntry<float> DamageRadius;
	public ConfigEntry<int> NonLethalDamage;
	public ConfigEntry<float> BodyLaunchForce;

	public Config(ConfigFile cfg)
	{
		LethalRadius = cfg.Bind("Technical", "LethalRadius", 6.5f, "The radius of the self destruct explosion that kills players.");
		DamageRadius = cfg.Bind("Technical", "DamageRadius", 7.5f, "The radius of the self destruct explosion that damages entities.");
		NonLethalDamage = cfg.Bind("Technical", "NonLethalDamage", 50, "How much damage the damage radius deals to players.");
		BodyLaunchForce = cfg.Bind("Technical", "BodyLaunchForce", 30f, "How fast your body gets launched after self destructing.");
	}
}
