using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000A55 RID: 2645
public class TubeTraveller : GameStateMachine<TubeTraveller, TubeTraveller.Instance>
{
	// Token: 0x06004CF3 RID: 19699 RVA: 0x001BFC98 File Offset: 0x001BDE98
	public void InitModifiers()
	{
		this.modifiers.Add(new AttributeModifier(Db.Get().Attributes.Insulation.Id, (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_INSULATION, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
		this.modifiers.Add(new AttributeModifier(Db.Get().Attributes.ThermalConductivityBarrier.Id, TUNING.EQUIPMENT.SUITS.ATMOSUIT_THERMAL_CONDUCTIVITY_BARRIER, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
		this.modifiers.Add(new AttributeModifier(Db.Get().Amounts.Bladder.deltaAttribute.Id, TUNING.EQUIPMENT.SUITS.ATMOSUIT_BLADDER, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
		this.modifiers.Add(new AttributeModifier(Db.Get().Attributes.ScaldingThreshold.Id, (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_SCALDING, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
		this.modifiers.Add(new AttributeModifier(Db.Get().Attributes.ScoldingThreshold.Id, (float)TUNING.EQUIPMENT.SUITS.ATMOSUIT_SCOLDING, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
		this.waxSpeedBoostModifier = new AttributeModifier(Db.Get().Attributes.TransitTubeTravelSpeed.Id, DUPLICANTSTATS.STANDARD.BaseStats.TRANSIT_TUBE_TRAVEL_SPEED * 0.25f, STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true);
		this.immunities.Add(Db.Get().effects.Get("SoakingWet"));
		this.immunities.Add(Db.Get().effects.Get("WetFeet"));
		this.immunities.Add(Db.Get().effects.Get("PoppedEarDrums"));
		this.immunities.Add(Db.Get().effects.Get("MinorIrritation"));
		this.immunities.Add(Db.Get().effects.Get("MajorIrritation"));
	}

	// Token: 0x06004CF4 RID: 19700 RVA: 0x001BFE97 File Offset: 0x001BE097
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		this.InitModifiers();
		default_state = this.root;
		this.root.DoNothing();
	}

	// Token: 0x06004CF5 RID: 19701 RVA: 0x001BFEB3 File Offset: 0x001BE0B3
	public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x06004CF6 RID: 19702 RVA: 0x001BFEB5 File Offset: 0x001BE0B5
	public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x06004CF7 RID: 19703 RVA: 0x001BFEB7 File Offset: 0x001BE0B7
	public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
	{
		return false;
	}

	// Token: 0x06004CF8 RID: 19704 RVA: 0x001BFEBA File Offset: 0x001BE0BA
	public bool ShouldEmitCO2()
	{
		return false;
	}

	// Token: 0x06004CF9 RID: 19705 RVA: 0x001BFEBD File Offset: 0x001BE0BD
	public bool ShouldStoreCO2()
	{
		return false;
	}

	// Token: 0x04003346 RID: 13126
	private List<Effect> immunities = new List<Effect>();

	// Token: 0x04003347 RID: 13127
	private List<AttributeModifier> modifiers = new List<AttributeModifier>();

	// Token: 0x04003348 RID: 13128
	private AttributeModifier waxSpeedBoostModifier;

	// Token: 0x04003349 RID: 13129
	private const float WaxSpeedBoost = 0.25f;

	// Token: 0x02001B62 RID: 7010
	public new class Instance : GameStateMachine<TubeTraveller, TubeTraveller.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x0600A9B3 RID: 43443 RVA: 0x003C2306 File Offset: 0x003C0506
		public int prefabInstanceID
		{
			get
			{
				return base.GetComponent<Navigator>().gameObject.GetComponent<KPrefabID>().InstanceID;
			}
		}

		// Token: 0x0600A9B4 RID: 43444 RVA: 0x003C231D File Offset: 0x003C051D
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600A9B5 RID: 43445 RVA: 0x003C2331 File Offset: 0x003C0531
		public void OnPathAdvanced(object data)
		{
			this.UnreserveEntrances();
			this.ReserveEntrances();
		}

		// Token: 0x0600A9B6 RID: 43446 RVA: 0x003C2340 File Offset: 0x003C0540
		public void ReserveEntrances()
		{
			PathFinder.Path path = base.GetComponent<Navigator>().path;
			if (path.nodes == null)
			{
				return;
			}
			for (int i = 0; i < path.nodes.Count - 1; i++)
			{
				if (path.nodes[i].navType == NavType.Floor && path.nodes[i + 1].navType == NavType.Tube)
				{
					int cell = path.nodes[i].cell;
					if (Grid.HasUsableTubeEntrance(cell, this.prefabInstanceID))
					{
						GameObject gameObject = Grid.Objects[cell, 1];
						if (gameObject)
						{
							TravelTubeEntrance component = gameObject.GetComponent<TravelTubeEntrance>();
							if (component)
							{
								component.Reserve(this, this.prefabInstanceID);
								this.reservations.Add(component);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600A9B7 RID: 43447 RVA: 0x003C240C File Offset: 0x003C060C
		public void UnreserveEntrances()
		{
			foreach (TravelTubeEntrance travelTubeEntrance in this.reservations)
			{
				if (!(travelTubeEntrance == null))
				{
					travelTubeEntrance.Unreserve(this, this.prefabInstanceID);
				}
			}
			this.reservations.Clear();
		}

		// Token: 0x0600A9B8 RID: 43448 RVA: 0x003C247C File Offset: 0x003C067C
		public void ApplyEnteringTubeEffects()
		{
			Effects component = base.GetComponent<Effects>();
			Attributes attributes = base.gameObject.GetAttributes();
			base.gameObject.AddTag(GameTags.InTransitTube);
			string name = GameTags.InTransitTube.Name;
			foreach (Effect effect in base.sm.immunities)
			{
				component.AddImmunity(effect, name, true);
			}
			foreach (AttributeModifier modifier in base.sm.modifiers)
			{
				attributes.Add(modifier);
			}
			if (this.isWaxed)
			{
				attributes.Add(base.sm.waxSpeedBoostModifier);
			}
			CreatureSimTemperatureTransfer component2 = base.gameObject.GetComponent<CreatureSimTemperatureTransfer>();
			if (component2 != null)
			{
				component2.RefreshRegistration();
			}
		}

		// Token: 0x0600A9B9 RID: 43449 RVA: 0x003C258C File Offset: 0x003C078C
		public void ClearAllEffects()
		{
			Effects component = base.GetComponent<Effects>();
			Attributes attributes = base.gameObject.GetAttributes();
			base.gameObject.RemoveTag(GameTags.InTransitTube);
			string name = GameTags.InTransitTube.Name;
			foreach (Effect effect in base.sm.immunities)
			{
				component.RemoveImmunity(effect, name);
			}
			foreach (AttributeModifier modifier in base.sm.modifiers)
			{
				attributes.Remove(modifier);
			}
			this.SetWaxState(false);
			attributes.Remove(base.sm.waxSpeedBoostModifier);
			CreatureSimTemperatureTransfer component2 = base.gameObject.GetComponent<CreatureSimTemperatureTransfer>();
			if (component2 != null)
			{
				component2.RefreshRegistration();
			}
		}

		// Token: 0x0600A9BA RID: 43450 RVA: 0x003C2698 File Offset: 0x003C0898
		public void SetWaxState(bool isWaxed)
		{
			this.isWaxed = isWaxed;
			KSelectable component = base.GetComponent<KSelectable>();
			if (component != null)
			{
				if (isWaxed)
				{
					component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.WaxedForTransitTube, 0.25f);
					return;
				}
				component.RemoveStatusItem(Db.Get().DuplicantStatusItems.WaxedForTransitTube, false);
			}
		}

		// Token: 0x0600A9BB RID: 43451 RVA: 0x003C2706 File Offset: 0x003C0906
		public void OnTubeTransition(bool nowInTube)
		{
			if (nowInTube != this.inTube)
			{
				this.inTube = nowInTube;
				base.GetComponent<Effects>();
				base.gameObject.GetAttributes();
				if (nowInTube)
				{
					this.ApplyEnteringTubeEffects();
					return;
				}
				this.ClearAllEffects();
			}
		}

		// Token: 0x040084C0 RID: 33984
		private List<TravelTubeEntrance> reservations = new List<TravelTubeEntrance>();

		// Token: 0x040084C1 RID: 33985
		public bool inTube;

		// Token: 0x040084C2 RID: 33986
		public bool isWaxed;
	}
}
