using LethalCompanyInputUtils.Api;
using LethalCompanyInputUtils.BindingPathEnums;
using UnityEngine.InputSystem;

namespace SelfDestruct;

public class PluginInput : LcInputActions
{
	[InputAction(KeyboardControl.K, Name = "Self Destruct")]
	public InputAction SelfDestruct { get; set; }
}
