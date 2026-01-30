using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200062F RID: 1583
public class Rottable : GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>
{
	// Token: 0x060025C6 RID: 9670 RVA: 0x000D8D98 File Offset: 0x000D6F98
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Fresh;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.TagTransition(GameTags.Preserved, this.Preserved, false).TagTransition(GameTags.Entombed, this.Preserved, false);
		this.Fresh.ToggleStatusItem(Db.Get().CreatureStatusItems.Fresh, (Rottable.Instance smi) => smi).ToggleTag(GameTags.RotModifierTags.Fresh).ParamTransition<float>(this.rotParameter, this.Stale_Pre, (Rottable.Instance smi, float p) => p <= smi.def.spoilTime - (smi.def.spoilTime - smi.def.staleTime)).Update(delegate(Rottable.Instance smi, float dt)
		{
			smi.sm.rotParameter.Set(smi.RotValue, smi, false);
		}, UpdateRate.SIM_1000ms, true).FastUpdate("Rot", Rottable.rotCB, UpdateRate.SIM_1000ms, true);
		this.Preserved.TagTransition(Rottable.PRESERVED_TAGS, this.Fresh, true).Enter("RefreshModifiers", delegate(Rottable.Instance smi)
		{
			smi.RefreshModifiers(0f);
		});
		this.Stale_Pre.Enter(delegate(Rottable.Instance smi)
		{
			smi.GoTo(this.Stale);
		});
		this.Stale.ToggleStatusItem(Db.Get().CreatureStatusItems.Stale, (Rottable.Instance smi) => smi).ParamTransition<float>(this.rotParameter, this.Fresh, (Rottable.Instance smi, float p) => p > smi.def.spoilTime - (smi.def.spoilTime - smi.def.staleTime)).ParamTransition<float>(this.rotParameter, this.Spoiled, GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.IsLTEZero).ToggleTag(GameTags.RotModifierTags.Stale).Update(delegate(Rottable.Instance smi, float dt)
		{
			smi.sm.rotParameter.Set(smi.RotValue, smi, false);
		}, UpdateRate.SIM_1000ms, false).FastUpdate("Rot", Rottable.rotCB, UpdateRate.SIM_1000ms, false);
		this.Spoiled.Enter(delegate(Rottable.Instance smi)
		{
			GameObject gameObject = Scenario.SpawnPrefab(Grid.PosToCell(smi.master.gameObject), 0, 0, "RotPile", Grid.SceneLayer.Ore);
			gameObject.gameObject.GetComponent<KSelectable>().SetName(UI.GAMEOBJECTEFFECTS.ROTTEN + " " + smi.master.gameObject.GetProperName());
			gameObject.transform.SetPosition(smi.master.transform.GetPosition());
			gameObject.GetComponent<PrimaryElement>().Mass = smi.master.GetComponent<PrimaryElement>().Mass;
			gameObject.GetComponent<PrimaryElement>().Temperature = smi.master.GetComponent<PrimaryElement>().Temperature;
			gameObject.SetActive(true);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, ITEMS.FOOD.ROTPILE.NAME, gameObject.transform, 1.5f, false);
			Edible component = smi.GetComponent<Edible>();
			if (component != null)
			{
				if (component.worker != null)
				{
					ChoreDriver component2 = component.worker.GetComponent<ChoreDriver>();
					if (component2 != null && component2.GetCurrentChore() != null)
					{
						component2.GetCurrentChore().Fail("food rotted");
					}
				}
				ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -component.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.ROTTED, "{0}", smi.gameObject.GetProperName()), UI.ENDOFDAYREPORT.NOTES.ROTTED_CONTEXT);
			}
			Util.KDestroyGameObject(smi.gameObject);
		});
	}

	// Token: 0x060025C7 RID: 9671 RVA: 0x000D8FD0 File Offset: 0x000D71D0
	private static string OnStaleTooltip(List<Notification> notifications, object data)
	{
		string text = "\n";
		foreach (Notification notification in notifications)
		{
			if (notification.tooltipData != null)
			{
				GameObject gameObject = (GameObject)notification.tooltipData;
				if (gameObject != null)
				{
					text = text + "\n" + gameObject.GetProperName();
				}
			}
		}
		return string.Format(MISC.NOTIFICATIONS.FOODSTALE.TOOLTIP, text);
	}

	// Token: 0x060025C8 RID: 9672 RVA: 0x000D905C File Offset: 0x000D725C
	public static void SetStatusItems(IRottable rottable)
	{
		Grid.PosToCell(rottable.gameObject);
		KSelectable component = rottable.gameObject.GetComponent<KSelectable>();
		Rottable.RotRefrigerationLevel rotRefrigerationLevel = Rottable.RefrigerationLevel(rottable);
		if (rotRefrigerationLevel != Rottable.RotRefrigerationLevel.Refrigerated)
		{
			if (rotRefrigerationLevel == Rottable.RotRefrigerationLevel.Frozen)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.PreservationTemperature, Db.Get().CreatureStatusItems.RefrigeratedFrozen, rottable);
			}
			else
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.PreservationTemperature, Db.Get().CreatureStatusItems.Unrefrigerated, rottable);
			}
		}
		else
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.PreservationTemperature, Db.Get().CreatureStatusItems.Refrigerated, rottable);
		}
		Rottable.RotAtmosphereQuality rotAtmosphereQuality = Rottable.AtmosphereQuality(rottable);
		if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Sterilizing)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere, Db.Get().CreatureStatusItems.SterilizingAtmosphere, null);
			return;
		}
		if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Contaminating)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere, Db.Get().CreatureStatusItems.ContaminatedAtmosphere, null);
			return;
		}
		component.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere, null, null);
	}

	// Token: 0x060025C9 RID: 9673 RVA: 0x000D917C File Offset: 0x000D737C
	public static bool IsInActiveFridge(IRottable rottable)
	{
		Pickupable component = rottable.gameObject.GetComponent<Pickupable>();
		if (component != null && component.storage != null)
		{
			Refrigerator component2 = component.storage.GetComponent<Refrigerator>();
			return component2 != null && component2.IsActive();
		}
		return false;
	}

	// Token: 0x060025CA RID: 9674 RVA: 0x000D91CC File Offset: 0x000D73CC
	public static Rottable.RotRefrigerationLevel RefrigerationLevel(IRottable rottable)
	{
		int num = Grid.PosToCell(rottable.gameObject);
		Rottable.Instance smi = rottable.gameObject.GetSMI<Rottable.Instance>();
		PrimaryElement component = rottable.gameObject.GetComponent<PrimaryElement>();
		float num2 = component.Temperature;
		bool flag = false;
		if (!Grid.IsValidCell(num))
		{
			if (!smi.IsRottableInSpace())
			{
				return Rottable.RotRefrigerationLevel.Normal;
			}
			flag = true;
		}
		if (!flag && Grid.Element[num].id != SimHashes.Vacuum)
		{
			num2 = Mathf.Min(Grid.Temperature[num], component.Temperature);
		}
		if (num2 < rottable.PreserveTemperature)
		{
			return Rottable.RotRefrigerationLevel.Frozen;
		}
		if (num2 < rottable.RotTemperature || Rottable.IsInActiveFridge(rottable))
		{
			return Rottable.RotRefrigerationLevel.Refrigerated;
		}
		return Rottable.RotRefrigerationLevel.Normal;
	}

	// Token: 0x060025CB RID: 9675 RVA: 0x000D926C File Offset: 0x000D746C
	public static Rottable.RotAtmosphereQuality AtmosphereQuality(IRottable rottable)
	{
		int num = Grid.PosToCell(rottable.gameObject);
		int num2 = Grid.CellAbove(num);
		if (!Grid.IsValidCell(num))
		{
			if (rottable.gameObject.GetSMI<Rottable.Instance>().IsRottableInSpace())
			{
				return Rottable.RotAtmosphereQuality.Sterilizing;
			}
			return Rottable.RotAtmosphereQuality.Normal;
		}
		else
		{
			SimHashes id = Grid.Element[num].id;
			Rottable.RotAtmosphereQuality rotAtmosphereQuality = Rottable.RotAtmosphereQuality.Normal;
			Rottable.AtmosphereModifier.TryGetValue((int)id, out rotAtmosphereQuality);
			Rottable.RotAtmosphereQuality rotAtmosphereQuality2 = Rottable.RotAtmosphereQuality.Normal;
			if (Grid.IsValidCell(num2))
			{
				SimHashes id2 = Grid.Element[num2].id;
				if (!Rottable.AtmosphereModifier.TryGetValue((int)id2, out rotAtmosphereQuality2))
				{
					rotAtmosphereQuality2 = rotAtmosphereQuality;
				}
			}
			else
			{
				rotAtmosphereQuality2 = rotAtmosphereQuality;
			}
			if (rotAtmosphereQuality == rotAtmosphereQuality2)
			{
				return rotAtmosphereQuality;
			}
			if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Contaminating || rotAtmosphereQuality2 == Rottable.RotAtmosphereQuality.Contaminating)
			{
				return Rottable.RotAtmosphereQuality.Contaminating;
			}
			if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Normal || rotAtmosphereQuality2 == Rottable.RotAtmosphereQuality.Normal)
			{
				return Rottable.RotAtmosphereQuality.Normal;
			}
			return Rottable.RotAtmosphereQuality.Sterilizing;
		}
	}

	// Token: 0x04001626 RID: 5670
	public StateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.FloatParameter rotParameter;

	// Token: 0x04001627 RID: 5671
	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Preserved;

	// Token: 0x04001628 RID: 5672
	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Fresh;

	// Token: 0x04001629 RID: 5673
	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Stale_Pre;

	// Token: 0x0400162A RID: 5674
	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Stale;

	// Token: 0x0400162B RID: 5675
	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Spoiled;

	// Token: 0x0400162C RID: 5676
	private static readonly Tag[] PRESERVED_TAGS = new Tag[]
	{
		GameTags.Preserved,
		GameTags.Dehydrated,
		GameTags.Entombed
	};

	// Token: 0x0400162D RID: 5677
	private static readonly Rottable.RotCB rotCB = new Rottable.RotCB();

	// Token: 0x0400162E RID: 5678
	public static Dictionary<int, Rottable.RotAtmosphereQuality> AtmosphereModifier = new Dictionary<int, Rottable.RotAtmosphereQuality>
	{
		{
			721531317,
			Rottable.RotAtmosphereQuality.Contaminating
		},
		{
			1887387588,
			Rottable.RotAtmosphereQuality.Contaminating
		},
		{
			-1528777920,
			Rottable.RotAtmosphereQuality.Normal
		},
		{
			1836671383,
			Rottable.RotAtmosphereQuality.Normal
		},
		{
			1960575215,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-899515856,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1554872654,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1858722091,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			758759285,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1046145888,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1324664829,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1406916018,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-432557516,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-805366663,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			1966552544,
			Rottable.RotAtmosphereQuality.Sterilizing
		}
	};

	// Token: 0x0200150A RID: 5386
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007078 RID: 28792
		public float spoilTime;

		// Token: 0x04007079 RID: 28793
		public float staleTime;

		// Token: 0x0400707A RID: 28794
		public float preserveTemperature = 255.15f;

		// Token: 0x0400707B RID: 28795
		public float rotTemperature = 277.15f;
	}

	// Token: 0x0200150B RID: 5387
	private class RotCB : UpdateBucketWithUpdater<Rottable.Instance>.IUpdater
	{
		// Token: 0x060091FA RID: 37370 RVA: 0x00372740 File Offset: 0x00370940
		public void Update(Rottable.Instance smi, float dt)
		{
			smi.Rot(smi, dt);
		}
	}

	// Token: 0x0200150C RID: 5388
	public new class Instance : GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.GameInstance, IRottable
	{
		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x060091FC RID: 37372 RVA: 0x00372752 File Offset: 0x00370952
		// (set) Token: 0x060091FD RID: 37373 RVA: 0x0037275F File Offset: 0x0037095F
		public float RotValue
		{
			get
			{
				return this.rotAmountInstance.value;
			}
			set
			{
				base.sm.rotParameter.Set(value, this, false);
				this.rotAmountInstance.SetValue(value);
			}
		}

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x060091FE RID: 37374 RVA: 0x00372782 File Offset: 0x00370982
		public float RotConstitutionPercentage
		{
			get
			{
				return this.RotValue / base.def.spoilTime;
			}
		}

		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x060091FF RID: 37375 RVA: 0x00372796 File Offset: 0x00370996
		public float RotTemperature
		{
			get
			{
				return base.def.rotTemperature;
			}
		}

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x06009200 RID: 37376 RVA: 0x003727A3 File Offset: 0x003709A3
		public float PreserveTemperature
		{
			get
			{
				return base.def.preserveTemperature;
			}
		}

		// Token: 0x06009201 RID: 37377 RVA: 0x003727B0 File Offset: 0x003709B0
		public Instance(IStateMachineTarget master, Rottable.Def def) : base(master, def)
		{
			this.pickupable = base.gameObject.RequireComponent<Pickupable>();
			base.master.Subscribe(-2064133523, new Action<object>(this.OnAbsorb));
			base.master.Subscribe(1335436905, new Action<object>(this.OnSplitFromChunk));
			this.primaryElement = base.gameObject.GetComponent<PrimaryElement>();
			Amounts amounts = master.gameObject.GetAmounts();
			this.rotAmountInstance = amounts.Add(new AmountInstance(Db.Get().Amounts.Rot, master.gameObject));
			this.rotAmountInstance.maxAttribute.Add(new AttributeModifier("Rot", def.spoilTime, null, false, false, true));
			this.rotAmountInstance.SetValue(def.spoilTime);
			base.sm.rotParameter.Set(this.rotAmountInstance.value, base.smi, false);
			if (Rottable.Instance.unrefrigeratedModifier == null)
			{
				Rottable.Instance.unrefrigeratedModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, -0.7f, DUPLICANTS.MODIFIERS.ROTTEMPERATURE.UNREFRIGERATED, false, false, true);
				Rottable.Instance.refrigeratedModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, -0.2f, DUPLICANTS.MODIFIERS.ROTTEMPERATURE.REFRIGERATED, false, false, true);
				Rottable.Instance.frozenModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, --0f, DUPLICANTS.MODIFIERS.ROTTEMPERATURE.FROZEN, false, false, true);
				Rottable.Instance.contaminatedAtmosphereModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, -1f, DUPLICANTS.MODIFIERS.ROTATMOSPHERE.CONTAMINATED, false, false, true);
				Rottable.Instance.normalAtmosphereModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, -0.3f, DUPLICANTS.MODIFIERS.ROTATMOSPHERE.NORMAL, false, false, true);
				Rottable.Instance.sterileAtmosphereModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, --0f, DUPLICANTS.MODIFIERS.ROTATMOSPHERE.STERILE, false, false, true);
			}
			this.RefreshModifiers(0f);
		}

		// Token: 0x06009202 RID: 37378 RVA: 0x003729CC File Offset: 0x00370BCC
		[OnDeserialized]
		private void OnDeserialized()
		{
			if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 23))
			{
				this.rotAmountInstance.SetValue(this.rotAmountInstance.value * 2f);
			}
		}

		// Token: 0x06009203 RID: 37379 RVA: 0x00372A10 File Offset: 0x00370C10
		public string StateString()
		{
			string result = "";
			if (base.smi.GetCurrentState() == base.sm.Fresh)
			{
				result = Db.Get().CreatureStatusItems.Fresh.resolveStringCallback(CREATURES.STATUSITEMS.FRESH.NAME, this);
			}
			if (base.smi.GetCurrentState() == base.sm.Stale)
			{
				result = Db.Get().CreatureStatusItems.Fresh.resolveStringCallback(CREATURES.STATUSITEMS.STALE.NAME, this);
			}
			return result;
		}

		// Token: 0x06009204 RID: 37380 RVA: 0x00372A9E File Offset: 0x00370C9E
		public void Rot(Rottable.Instance smi, float deltaTime)
		{
			this.RefreshModifiers(deltaTime);
			if (smi.pickupable.storage != null)
			{
				smi.pickupable.storage.Trigger(-1197125120, null);
			}
		}

		// Token: 0x06009205 RID: 37381 RVA: 0x00372AD0 File Offset: 0x00370CD0
		public bool IsRottableInSpace()
		{
			if (base.gameObject.GetMyWorld() == null)
			{
				Pickupable component = base.GetComponent<Pickupable>();
				if (component != null && component.storage && (component.storage.GetComponent<RocketModuleCluster>() || component.storage.GetComponent<ClusterTraveler>()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009206 RID: 37382 RVA: 0x00372B34 File Offset: 0x00370D34
		public void RefreshModifiers(float dt)
		{
			if (this.GetMaster().isNull)
			{
				return;
			}
			if (!Grid.IsValidCell(Grid.PosToCell(base.gameObject)) && !this.IsRottableInSpace())
			{
				return;
			}
			this.rotAmountInstance.deltaAttribute.ClearModifiers();
			KPrefabID component = base.GetComponent<KPrefabID>();
			if (!component.HasAnyTags(Rottable.PRESERVED_TAGS))
			{
				Rottable.RotRefrigerationLevel rotRefrigerationLevel = Rottable.RefrigerationLevel(this);
				if (rotRefrigerationLevel != Rottable.RotRefrigerationLevel.Refrigerated)
				{
					if (rotRefrigerationLevel == Rottable.RotRefrigerationLevel.Frozen)
					{
						this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.frozenModifier);
						component.AddTag(GameTags.RotModifierTags.DeepFrozen, false);
						component.RemoveTag(GameTags.RotModifierTags.Refrigerated);
					}
					else
					{
						this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.unrefrigeratedModifier);
						component.RemoveTag(GameTags.RotModifierTags.Refrigerated);
						component.RemoveTag(GameTags.RotModifierTags.DeepFrozen);
					}
				}
				else
				{
					this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.refrigeratedModifier);
					component.AddTag(GameTags.RotModifierTags.Refrigerated, false);
					component.RemoveTag(GameTags.RotModifierTags.DeepFrozen);
				}
				Rottable.RotAtmosphereQuality rotAtmosphereQuality = Rottable.AtmosphereQuality(this);
				if (rotAtmosphereQuality != Rottable.RotAtmosphereQuality.Sterilizing)
				{
					if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Contaminating)
					{
						this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.contaminatedAtmosphereModifier);
					}
					else
					{
						this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.normalAtmosphereModifier);
					}
				}
				else
				{
					this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.sterileAtmosphereModifier);
				}
			}
			if (component.HasTag(Db.Get().Spices.PreservingSpice.Id))
			{
				this.rotAmountInstance.deltaAttribute.Add(Db.Get().Spices.PreservingSpice.FoodModifier);
			}
			Rottable.SetStatusItems(this);
		}

		// Token: 0x06009207 RID: 37383 RVA: 0x00372CC4 File Offset: 0x00370EC4
		private void OnAbsorb(object data)
		{
			Pickupable pickupable = (Pickupable)data;
			if (pickupable != null)
			{
				PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
				PrimaryElement primaryElement = pickupable.PrimaryElement;
				Rottable.Instance smi = pickupable.gameObject.GetSMI<Rottable.Instance>();
				if (component != null && primaryElement != null && smi != null)
				{
					float num = component.Units * base.sm.rotParameter.Get(base.smi);
					float num2 = primaryElement.Units * base.sm.rotParameter.Get(smi);
					float value = (num + num2) / (component.Units + primaryElement.Units);
					base.sm.rotParameter.Set(value, base.smi, false);
				}
			}
		}

		// Token: 0x06009208 RID: 37384 RVA: 0x00372D7C File Offset: 0x00370F7C
		public bool IsRotLevelStackable(Rottable.Instance other)
		{
			return Mathf.Abs(this.RotConstitutionPercentage - other.RotConstitutionPercentage) < 0.1f;
		}

		// Token: 0x06009209 RID: 37385 RVA: 0x00372D97 File Offset: 0x00370F97
		public string GetToolTip()
		{
			return this.rotAmountInstance.GetTooltip();
		}

		// Token: 0x0600920A RID: 37386 RVA: 0x00372DA4 File Offset: 0x00370FA4
		private void OnSplitFromChunk(object data)
		{
			Pickupable pickupable = (Pickupable)data;
			if (pickupable != null)
			{
				Rottable.Instance smi = pickupable.GetSMI<Rottable.Instance>();
				if (smi != null)
				{
					this.RotValue = smi.RotValue;
				}
			}
		}

		// Token: 0x0600920B RID: 37387 RVA: 0x00372DD7 File Offset: 0x00370FD7
		public void OnPreserved(object data)
		{
			if ((bool)data)
			{
				base.smi.GoTo(base.sm.Preserved);
				return;
			}
			base.smi.GoTo(base.sm.Fresh);
		}

		// Token: 0x0400707C RID: 28796
		private AmountInstance rotAmountInstance;

		// Token: 0x0400707D RID: 28797
		private static AttributeModifier unrefrigeratedModifier;

		// Token: 0x0400707E RID: 28798
		private static AttributeModifier refrigeratedModifier;

		// Token: 0x0400707F RID: 28799
		private static AttributeModifier frozenModifier;

		// Token: 0x04007080 RID: 28800
		private static AttributeModifier contaminatedAtmosphereModifier;

		// Token: 0x04007081 RID: 28801
		private static AttributeModifier normalAtmosphereModifier;

		// Token: 0x04007082 RID: 28802
		private static AttributeModifier sterileAtmosphereModifier;

		// Token: 0x04007083 RID: 28803
		public PrimaryElement primaryElement;

		// Token: 0x04007084 RID: 28804
		public Pickupable pickupable;
	}

	// Token: 0x0200150D RID: 5389
	public enum RotAtmosphereQuality
	{
		// Token: 0x04007086 RID: 28806
		Normal,
		// Token: 0x04007087 RID: 28807
		Sterilizing,
		// Token: 0x04007088 RID: 28808
		Contaminating
	}

	// Token: 0x0200150E RID: 5390
	public enum RotRefrigerationLevel
	{
		// Token: 0x0400708A RID: 28810
		Normal,
		// Token: 0x0400708B RID: 28811
		Refrigerated,
		// Token: 0x0400708C RID: 28812
		Frozen
	}
}
