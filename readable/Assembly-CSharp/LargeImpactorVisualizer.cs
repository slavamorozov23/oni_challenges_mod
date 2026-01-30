using System;
using UnityEngine;

// Token: 0x020005F9 RID: 1529
public class LargeImpactorVisualizer : KMonoBehaviour
{
	// Token: 0x17000171 RID: 369
	// (get) Token: 0x0600237A RID: 9082 RVA: 0x000CD0B6 File Offset: 0x000CB2B6
	public bool Visible
	{
		get
		{
			return this.Active && !this.Folded;
		}
	}

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x0600237C RID: 9084 RVA: 0x000CD0D4 File Offset: 0x000CB2D4
	// (set) Token: 0x0600237B RID: 9083 RVA: 0x000CD0CB File Offset: 0x000CB2CB
	public bool Folded { get; private set; } = true;

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x0600237E RID: 9086 RVA: 0x000CD0E5 File Offset: 0x000CB2E5
	// (set) Token: 0x0600237D RID: 9085 RVA: 0x000CD0DC File Offset: 0x000CB2DC
	public float LastTimeSetToFolded { get; private set; }

	// Token: 0x17000174 RID: 372
	// (get) Token: 0x06002380 RID: 9088 RVA: 0x000CD0F6 File Offset: 0x000CB2F6
	// (set) Token: 0x0600237F RID: 9087 RVA: 0x000CD0ED File Offset: 0x000CB2ED
	public bool ShouldResetEntryEffect { get; private set; }

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x06002382 RID: 9090 RVA: 0x000CD107 File Offset: 0x000CB307
	// (set) Token: 0x06002381 RID: 9089 RVA: 0x000CD0FE File Offset: 0x000CB2FE
	public float EntryEffectDuration { get; private set; } = 3f;

	// Token: 0x17000176 RID: 374
	// (get) Token: 0x06002384 RID: 9092 RVA: 0x000CD118 File Offset: 0x000CB318
	// (set) Token: 0x06002383 RID: 9091 RVA: 0x000CD10F File Offset: 0x000CB30F
	public float FoldEffectDuration { get; private set; } = 1f;

	// Token: 0x06002385 RID: 9093 RVA: 0x000CD120 File Offset: 0x000CB320
	public void BeginEntryEffect(float duration)
	{
		this.EntryEffectDuration = duration;
		this.SetShouldResetEntryEffect(true);
	}

	// Token: 0x06002386 RID: 9094 RVA: 0x000CD130 File Offset: 0x000CB330
	public void SetShouldResetEntryEffect(bool shouldIt)
	{
		this.ShouldResetEntryEffect = shouldIt;
	}

	// Token: 0x06002387 RID: 9095 RVA: 0x000CD13C File Offset: 0x000CB33C
	public void SetFoldedState(bool shouldBeFolded)
	{
		if (!this.Folded && shouldBeFolded)
		{
			this.LastTimeSetToFolded = Time.unscaledTime;
			if (this.Active)
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Demolior_LandingZone_close_fx", false));
			}
		}
		this.Folded = shouldBeFolded;
		if (!shouldBeFolded)
		{
			this.LastTimeSetToFolded = float.MaxValue;
		}
	}

	// Token: 0x040014AC RID: 5292
	public bool Active;

	// Token: 0x040014B2 RID: 5298
	private const string SFX_Fold = "HUD_Demolior_LandingZone_close_fx";

	// Token: 0x040014B3 RID: 5299
	public Vector2I OriginOffset;

	// Token: 0x040014B4 RID: 5300
	public Vector2 ScreenSpaceNotificationTogglePosition = Vector2.zero;

	// Token: 0x040014B5 RID: 5301
	public Vector2I RangeMin;

	// Token: 0x040014B6 RID: 5302
	public Vector2I RangeMax;

	// Token: 0x040014B7 RID: 5303
	public Vector2I TexSize = new Vector2I(64, 64);

	// Token: 0x040014B8 RID: 5304
	public bool TestLineOfSight;

	// Token: 0x040014B9 RID: 5305
	public bool BlockingTileVisible;

	// Token: 0x040014BA RID: 5306
	public Func<int, bool> BlockingVisibleCb;

	// Token: 0x040014BB RID: 5307
	public Func<int, bool> BlockingCb = new Func<int, bool>(Grid.IsSolidCell);

	// Token: 0x040014BC RID: 5308
	public bool AllowLineOfSightInvalidCells;
}
