using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000595 RID: 1429
public class AnimEventHandlerManager : KMonoBehaviour
{
	// Token: 0x1700012A RID: 298
	// (get) Token: 0x0600200D RID: 8205 RVA: 0x000B91FD File Offset: 0x000B73FD
	// (set) Token: 0x0600200E RID: 8206 RVA: 0x000B9204 File Offset: 0x000B7404
	public static AnimEventHandlerManager Instance { get; private set; }

	// Token: 0x0600200F RID: 8207 RVA: 0x000B920C File Offset: 0x000B740C
	public static void DestroyInstance()
	{
		AnimEventHandlerManager.Instance = null;
	}

	// Token: 0x06002010 RID: 8208 RVA: 0x000B9214 File Offset: 0x000B7414
	protected override void OnPrefabInit()
	{
		AnimEventHandlerManager.Instance = this;
		this.handlers = new List<AnimEventHandler>();
	}

	// Token: 0x06002011 RID: 8209 RVA: 0x000B9227 File Offset: 0x000B7427
	public void Add(AnimEventHandler handler)
	{
		this.handlers.Add(handler);
	}

	// Token: 0x06002012 RID: 8210 RVA: 0x000B9235 File Offset: 0x000B7435
	public void Remove(AnimEventHandler handler)
	{
		this.handlers.Remove(handler);
	}

	// Token: 0x06002013 RID: 8211 RVA: 0x000B9244 File Offset: 0x000B7444
	private bool IsVisibleToZoom()
	{
		return !(Game.MainCamera == null) && Game.MainCamera.orthographicSize < 40f;
	}

	// Token: 0x06002014 RID: 8212 RVA: 0x000B9268 File Offset: 0x000B7468
	public void LateUpdate()
	{
		if (!this.IsVisibleToZoom())
		{
			return;
		}
		AnimEventHandlerManager.<>c__DisplayClass11_0 CS$<>8__locals1;
		Grid.GetVisibleCellRangeInActiveWorld(out CS$<>8__locals1.min, out CS$<>8__locals1.max, 4, 1.5f);
		foreach (AnimEventHandler animEventHandler in this.handlers)
		{
			if (AnimEventHandlerManager.<LateUpdate>g__IsVisible|11_0(animEventHandler, ref CS$<>8__locals1))
			{
				animEventHandler.UpdateOffset();
			}
		}
	}

	// Token: 0x06002016 RID: 8214 RVA: 0x000B92F0 File Offset: 0x000B74F0
	[CompilerGenerated]
	internal static bool <LateUpdate>g__IsVisible|11_0(AnimEventHandler handler, ref AnimEventHandlerManager.<>c__DisplayClass11_0 A_1)
	{
		int num;
		int num2;
		Grid.CellToXY(handler.GetCachedCell(), out num, out num2);
		return num >= A_1.min.x && num2 >= A_1.min.y && num < A_1.max.x && num2 < A_1.max.y;
	}

	// Token: 0x040012A9 RID: 4777
	private const float HIDE_DISTANCE = 40f;

	// Token: 0x040012AB RID: 4779
	private List<AnimEventHandler> handlers;
}
