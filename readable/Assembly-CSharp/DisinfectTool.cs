using System;
using UnityEngine;

// Token: 0x020009AA RID: 2474
public class DisinfectTool : DragTool
{
	// Token: 0x06004768 RID: 18280 RVA: 0x0019D153 File Offset: 0x0019B353
	public static void DestroyInstance()
	{
		DisinfectTool.Instance = null;
	}

	// Token: 0x06004769 RID: 18281 RVA: 0x0019D15B File Offset: 0x0019B35B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DisinfectTool.Instance = this;
		this.interceptNumberKeysForPriority = true;
		this.viewMode = OverlayModes.Disease.ID;
	}

	// Token: 0x0600476A RID: 18282 RVA: 0x0019D17B File Offset: 0x0019B37B
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x0600476B RID: 18283 RVA: 0x0019D188 File Offset: 0x0019B388
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		for (int i = 0; i < 45; i++)
		{
			GameObject gameObject = Grid.Objects[cell, i];
			if (gameObject != null)
			{
				Disinfectable component = gameObject.GetComponent<Disinfectable>();
				if (component != null && component.GetComponent<PrimaryElement>().DiseaseCount > 0)
				{
					component.MarkForDisinfect(false);
				}
			}
		}
	}

	// Token: 0x04002FD5 RID: 12245
	public static DisinfectTool Instance;
}
