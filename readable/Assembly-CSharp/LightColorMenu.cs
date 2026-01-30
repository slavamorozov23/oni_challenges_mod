using System;
using UnityEngine;

// Token: 0x020005FA RID: 1530
[AddComponentMenu("KMonoBehaviour/scripts/LightColorMenu")]
public class LightColorMenu : KMonoBehaviour
{
	// Token: 0x06002389 RID: 9097 RVA: 0x000CD1EC File Offset: 0x000CB3EC
	protected override void OnPrefabInit()
	{
		base.Subscribe<LightColorMenu>(493375141, LightColorMenu.OnRefreshUserMenuDelegate);
		this.SetColor(0);
	}

	// Token: 0x0600238A RID: 9098 RVA: 0x000CD208 File Offset: 0x000CB408
	private void OnRefreshUserMenu(object data)
	{
		if (this.lightColors.Length != 0)
		{
			int num = this.lightColors.Length;
			for (int i = 0; i < num; i++)
			{
				if (i != this.currentColor)
				{
					int new_color = i;
					Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo(this.lightColors[i].name, this.lightColors[i].name, delegate()
					{
						this.SetColor(new_color);
					}, global::Action.NumActions, null, null, null, "", true), 1f);
				}
			}
		}
	}

	// Token: 0x0600238B RID: 9099 RVA: 0x000CD2B0 File Offset: 0x000CB4B0
	private void SetColor(int color_index)
	{
		if (this.lightColors.Length != 0 && color_index < this.lightColors.Length)
		{
			Light2D[] componentsInChildren = base.GetComponentsInChildren<Light2D>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Color = this.lightColors[color_index].color;
			}
			MeshRenderer[] componentsInChildren2 = base.GetComponentsInChildren<MeshRenderer>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				foreach (Material material in componentsInChildren2[i].materials)
				{
					if (material.name.StartsWith("matScriptedGlow01"))
					{
						material.color = this.lightColors[color_index].color;
					}
				}
			}
		}
		this.currentColor = color_index;
	}

	// Token: 0x040014BD RID: 5309
	public LightColorMenu.LightColor[] lightColors;

	// Token: 0x040014BE RID: 5310
	private int currentColor;

	// Token: 0x040014BF RID: 5311
	private static readonly EventSystem.IntraObjectHandler<LightColorMenu> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<LightColorMenu>(delegate(LightColorMenu component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x020014CD RID: 5325
	[Serializable]
	public struct LightColor
	{
		// Token: 0x0600911E RID: 37150 RVA: 0x003703F5 File Offset: 0x0036E5F5
		public LightColor(string name, Color color)
		{
			this.name = name;
			this.color = color;
		}

		// Token: 0x04006F9C RID: 28572
		public string name;

		// Token: 0x04006F9D RID: 28573
		public Color color;
	}
}
