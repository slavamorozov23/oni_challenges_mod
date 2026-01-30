using System;
using UnityEngine;

// Token: 0x02000ED3 RID: 3795
public class ScreenResolutionMonitor : MonoBehaviour
{
	// Token: 0x06007976 RID: 31094 RVA: 0x002EB1BC File Offset: 0x002E93BC
	private void Awake()
	{
		this.previousSize = new Vector2((float)Screen.width, (float)Screen.height);
	}

	// Token: 0x06007977 RID: 31095 RVA: 0x002EB1D8 File Offset: 0x002E93D8
	private void Update()
	{
		if ((this.previousSize.x != (float)Screen.width || this.previousSize.y != (float)Screen.height) && Game.Instance != null)
		{
			Game.Instance.Trigger(445618876, null);
			this.previousSize.x = (float)Screen.width;
			this.previousSize.y = (float)Screen.height;
		}
		this.UpdateShouldUseGamepadUIMode();
	}

	// Token: 0x06007978 RID: 31096 RVA: 0x002EB250 File Offset: 0x002E9450
	public static bool UsingGamepadUIMode()
	{
		return ScreenResolutionMonitor.previousGamepadUIMode;
	}

	// Token: 0x06007979 RID: 31097 RVA: 0x002EB258 File Offset: 0x002E9458
	private void UpdateShouldUseGamepadUIMode()
	{
		bool flag = (Screen.dpi > 130f && Screen.height < 900) || KInputManager.currentControllerIsGamepad;
		if (flag != ScreenResolutionMonitor.previousGamepadUIMode)
		{
			ScreenResolutionMonitor.previousGamepadUIMode = flag;
			if (Game.Instance == null)
			{
				return;
			}
			Game.Instance.Trigger(-442024484, null);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound(flag ? "ControllerType_ToggleOn" : "ControllerType_ToggleOff", false));
		}
	}

	// Token: 0x040054F7 RID: 21751
	[SerializeField]
	private Vector2 previousSize;

	// Token: 0x040054F8 RID: 21752
	private static bool previousGamepadUIMode;

	// Token: 0x040054F9 RID: 21753
	private const float HIGH_DPI = 130f;
}
