using System;
using UnityEngine;

// Token: 0x02000666 RID: 1638
public class SplashTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x060027AD RID: 10157 RVA: 0x000E3B9F File Offset: 0x000E1D9F
	public SplashTransitionLayer(Navigator navigator) : base(navigator)
	{
		this.lastSplashTime = Time.time;
	}

	// Token: 0x060027AE RID: 10158 RVA: 0x000E3BB4 File Offset: 0x000E1DB4
	private void RefreshSplashes(Navigator navigator, Navigator.ActiveTransition transition)
	{
		if (navigator == null)
		{
			return;
		}
		if (transition.end == NavType.Tube)
		{
			return;
		}
		Vector3 position = navigator.transform.GetPosition();
		if (this.lastSplashTime + 1f < Time.time && Grid.Element[Grid.PosToCell(position)].IsLiquid)
		{
			this.lastSplashTime = Time.time;
			Game.Instance.SpawnFX(SpawnFXHashes.SplashStep, position + new Vector3(0f, 0.75f, -0.1f), 0f);
		}
	}

	// Token: 0x060027AF RID: 10159 RVA: 0x000E3C40 File Offset: 0x000E1E40
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		this.RefreshSplashes(navigator, transition);
	}

	// Token: 0x060027B0 RID: 10160 RVA: 0x000E3C52 File Offset: 0x000E1E52
	public override void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.UpdateTransition(navigator, transition);
		this.RefreshSplashes(navigator, transition);
	}

	// Token: 0x060027B1 RID: 10161 RVA: 0x000E3C64 File Offset: 0x000E1E64
	public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.EndTransition(navigator, transition);
		this.RefreshSplashes(navigator, transition);
	}

	// Token: 0x04001754 RID: 5972
	private float lastSplashTime;

	// Token: 0x04001755 RID: 5973
	private const float SPLASH_INTERVAL = 1f;
}
