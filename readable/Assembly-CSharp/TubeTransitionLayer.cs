using System;
using UnityEngine;

// Token: 0x02000669 RID: 1641
public class TubeTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x060027BB RID: 10171 RVA: 0x000E3FDD File Offset: 0x000E21DD
	public TubeTransitionLayer(Navigator navigator) : base(navigator)
	{
		this.tube_traveller = navigator.GetSMI<TubeTraveller.Instance>();
		if (this.tube_traveller != null && navigator.CurrentNavType == NavType.Tube && !this.tube_traveller.inTube)
		{
			this.tube_traveller.OnTubeTransition(true);
		}
	}

	// Token: 0x060027BC RID: 10172 RVA: 0x000E401C File Offset: 0x000E221C
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		this.tube_traveller.OnPathAdvanced(null);
		if (transition.start != NavType.Tube && transition.end == NavType.Tube)
		{
			int cell = Grid.PosToCell(navigator);
			this.entrance = this.GetEntrance(cell);
			return;
		}
		this.entrance = null;
	}

	// Token: 0x060027BD RID: 10173 RVA: 0x000E406C File Offset: 0x000E226C
	public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.EndTransition(navigator, transition);
		if (transition.start != NavType.Tube && transition.end == NavType.Tube && this.entrance)
		{
			this.entrance.ConsumeCharge(navigator.gameObject);
			this.entrance = null;
		}
		this.tube_traveller.OnTubeTransition(transition.end == NavType.Tube);
	}

	// Token: 0x060027BE RID: 10174 RVA: 0x000E40CC File Offset: 0x000E22CC
	private TravelTubeEntrance GetEntrance(int cell)
	{
		if (!Grid.HasUsableTubeEntrance(cell, this.tube_traveller.prefabInstanceID))
		{
			return null;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			TravelTubeEntrance component = gameObject.GetComponent<TravelTubeEntrance>();
			if (component != null && component.isSpawned)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x04001758 RID: 5976
	private TubeTraveller.Instance tube_traveller;

	// Token: 0x04001759 RID: 5977
	private TravelTubeEntrance entrance;
}
