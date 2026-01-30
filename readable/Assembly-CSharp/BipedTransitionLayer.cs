using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000665 RID: 1637
public class BipedTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x060027A5 RID: 10149 RVA: 0x000E3730 File Offset: 0x000E1930
	public BipedTransitionLayer(Navigator navigator, float floor_speed, float ladder_speed) : base(navigator)
	{
		navigator.Subscribe(1773898642, delegate(object data)
		{
			this.isWalking = true;
		});
		navigator.Subscribe(1597112836, delegate(object data)
		{
			this.isWalking = false;
		});
		this.floorSpeed = floor_speed;
		this.ladderSpeed = ladder_speed;
		this.jetPackSpeed = 7f;
		this.movementSpeed = Db.Get().AttributeConverters.MovementSpeed.Lookup(navigator.gameObject);
		this.attributeLevels = navigator.GetComponent<AttributeLevels>();
	}

	// Token: 0x060027A6 RID: 10150 RVA: 0x000E37BC File Offset: 0x000E19BC
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		float num = 1f;
		bool flag = (transition.start == NavType.Pole || transition.end == NavType.Pole) && transition.y < 0 && transition.x == 0;
		bool flag2 = transition.start == NavType.Tube || transition.end == NavType.Tube;
		bool flag3 = transition.start == NavType.Hover || transition.end == NavType.Hover;
		if (!flag && !flag2 && !flag3)
		{
			if (this.isWalking)
			{
				return;
			}
			num = this.GetMovementSpeedMultiplier();
		}
		int cell = Grid.PosToCell(navigator);
		float num2 = 1f;
		bool flag4 = (navigator.flags & PathFinder.PotentialPath.Flags.HasAtmoSuit) > PathFinder.PotentialPath.Flags.None;
		bool flag5 = (navigator.flags & PathFinder.PotentialPath.Flags.HasJetPack) > PathFinder.PotentialPath.Flags.None;
		bool flag6 = (navigator.flags & PathFinder.PotentialPath.Flags.HasLeadSuit) > PathFinder.PotentialPath.Flags.None;
		if (!flag5 && !flag4 && !flag6 && Grid.IsSubstantialLiquid(cell, 0.35f))
		{
			num2 = 0.5f;
		}
		num *= num2;
		if (transition.x == 0 && (transition.start == NavType.Ladder || transition.start == NavType.Pole) && transition.start == transition.end)
		{
			if (flag)
			{
				transition.speed = 15f * num2;
			}
			else
			{
				transition.speed = this.ladderSpeed * num;
				GameObject gameObject = Grid.Objects[cell, 1];
				if (gameObject != null)
				{
					Ladder component = gameObject.GetComponent<Ladder>();
					if (component != null)
					{
						float num3 = component.upwardsMovementSpeedMultiplier;
						if (transition.y < 0)
						{
							num3 = component.downwardsMovementSpeedMultiplier;
						}
						transition.speed *= num3;
						transition.animSpeed *= num3;
					}
				}
			}
		}
		else if (flag2)
		{
			transition.speed = this.GetTubeTravellingSpeedMultiplier(navigator);
		}
		else if (flag3)
		{
			transition.speed = this.jetPackSpeed;
			if (transition.x == 0 && transition.y == -1)
			{
				transition.speed *= 0.75f;
			}
			transition.animSpeed = transition.speed;
		}
		else
		{
			transition.speed = this.floorSpeed * num;
		}
		float num4 = num - 1f;
		transition.animSpeed += transition.animSpeed * num4 / 2f;
		if (transition.start == NavType.Floor && transition.end == NavType.Floor)
		{
			int num5 = Grid.CellBelow(cell);
			if (Grid.Foundation[num5])
			{
				GameObject gameObject2 = Grid.Objects[num5, 1];
				if (gameObject2 != null)
				{
					SimCellOccupier component2 = gameObject2.GetComponent<SimCellOccupier>();
					if (component2 != null)
					{
						transition.speed *= component2.movementSpeedMultiplier;
						transition.animSpeed *= component2.movementSpeedMultiplier;
					}
				}
			}
		}
		this.startTime = Time.time;
	}

	// Token: 0x060027A7 RID: 10151 RVA: 0x000E3A70 File Offset: 0x000E1C70
	public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.EndTransition(navigator, transition);
		bool flag = (transition.start == NavType.Pole || transition.end == NavType.Pole) && transition.y < 0 && transition.x == 0;
		bool flag2 = transition.start == NavType.Tube || transition.end == NavType.Tube;
		if (!this.isWalking && !flag && !flag2 && this.attributeLevels != null)
		{
			this.attributeLevels.AddExperience(Db.Get().Attributes.Athletics.Id, Time.time - this.startTime, DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE);
		}
	}

	// Token: 0x060027A8 RID: 10152 RVA: 0x000E3B10 File Offset: 0x000E1D10
	public float GetTubeTravellingSpeedMultiplier(Navigator navigator)
	{
		AttributeInstance attributeInstance = Db.Get().Attributes.TransitTubeTravelSpeed.Lookup(navigator.gameObject);
		if (attributeInstance != null)
		{
			return attributeInstance.GetTotalValue();
		}
		return DUPLICANTSTATS.STANDARD.BaseStats.TRANSIT_TUBE_TRAVEL_SPEED;
	}

	// Token: 0x060027A9 RID: 10153 RVA: 0x000E3B54 File Offset: 0x000E1D54
	public static float GetMovementSpeedMultiplier(AttributeConverterInstance movementSpeed)
	{
		float num = 1f;
		if (movementSpeed != null)
		{
			num += movementSpeed.Evaluate();
		}
		return Mathf.Max(0.1f, num);
	}

	// Token: 0x060027AA RID: 10154 RVA: 0x000E3B80 File Offset: 0x000E1D80
	public float GetMovementSpeedMultiplier()
	{
		return BipedTransitionLayer.GetMovementSpeedMultiplier(this.movementSpeed);
	}

	// Token: 0x0400174B RID: 5963
	private bool isWalking;

	// Token: 0x0400174C RID: 5964
	private float floorSpeed;

	// Token: 0x0400174D RID: 5965
	private float ladderSpeed;

	// Token: 0x0400174E RID: 5966
	private float startTime;

	// Token: 0x0400174F RID: 5967
	private float jetPackSpeed;

	// Token: 0x04001750 RID: 5968
	private const float downPoleSpeed = 15f;

	// Token: 0x04001751 RID: 5969
	private const float WATER_SPEED_PENALTY = 0.5f;

	// Token: 0x04001752 RID: 5970
	private AttributeConverterInstance movementSpeed;

	// Token: 0x04001753 RID: 5971
	private AttributeLevels attributeLevels;
}
