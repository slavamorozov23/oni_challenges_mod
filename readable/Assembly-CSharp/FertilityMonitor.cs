using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020008A0 RID: 2208
public class FertilityMonitor : GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>
{
	// Token: 0x06003CC3 RID: 15555 RVA: 0x00153A8C File Offset: 0x00151C8C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.fertile;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.DefaultState(this.fertile);
		this.fertile.ToggleBehaviour(GameTags.Creatures.Fertile, (FertilityMonitor.Instance smi) => smi.IsReadyToLayEgg(), null).ToggleEffect((FertilityMonitor.Instance smi) => smi.fertileEffect).Transition(this.infertile, GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.Not(new StateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.Transition.ConditionCallback(FertilityMonitor.IsFertile)), UpdateRate.SIM_1000ms);
		this.infertile.Transition(this.fertile, new StateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.Transition.ConditionCallback(FertilityMonitor.IsFertile), UpdateRate.SIM_1000ms);
	}

	// Token: 0x06003CC4 RID: 15556 RVA: 0x00153B4B File Offset: 0x00151D4B
	public static bool IsFertile(FertilityMonitor.Instance smi)
	{
		return !smi.PrefabId.HasTag(GameTags.Creatures.PausedReproduction) && !smi.PrefabId.HasTag(GameTags.Creatures.Confined) && smi.PrefabId.HasTag(GameTags.Creatures.Expecting);
	}

	// Token: 0x06003CC5 RID: 15557 RVA: 0x00153B8C File Offset: 0x00151D8C
	public static Tag EggBreedingRoll(List<FertilityMonitor.BreedingChance> breedingChances, bool excludeOriginalCreature = false)
	{
		float num = UnityEngine.Random.value;
		if (excludeOriginalCreature)
		{
			num *= 1f - breedingChances[0].weight;
		}
		foreach (FertilityMonitor.BreedingChance breedingChance in breedingChances)
		{
			if (excludeOriginalCreature)
			{
				excludeOriginalCreature = false;
			}
			else
			{
				num -= breedingChance.weight;
				if (num <= 0f)
				{
					return breedingChance.egg;
				}
			}
		}
		return Tag.Invalid;
	}

	// Token: 0x04002588 RID: 9608
	private GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.State fertile;

	// Token: 0x04002589 RID: 9609
	private GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.State infertile;

	// Token: 0x02001889 RID: 6281
	[Serializable]
	public class BreedingChance
	{
		// Token: 0x04007B34 RID: 31540
		public Tag egg;

		// Token: 0x04007B35 RID: 31541
		public float weight;
	}

	// Token: 0x0200188A RID: 6282
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06009F45 RID: 40773 RVA: 0x003A56F6 File Offset: 0x003A38F6
		public override void Configure(GameObject prefab)
		{
			prefab.AddOrGet<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Fertility.Id);
		}

		// Token: 0x04007B36 RID: 31542
		public Tag eggPrefab;

		// Token: 0x04007B37 RID: 31543
		public List<FertilityMonitor.BreedingChance> initialBreedingWeights;

		// Token: 0x04007B38 RID: 31544
		public float baseFertileCycles;
	}

	// Token: 0x0200188B RID: 6283
	public new class Instance : GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.GameInstance
	{
		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x06009F47 RID: 40775 RVA: 0x003A5724 File Offset: 0x003A3924
		public KPrefabID PrefabId
		{
			get
			{
				return this.prefabId;
			}
		}

		// Token: 0x06009F48 RID: 40776 RVA: 0x003A572C File Offset: 0x003A392C
		public Instance(IStateMachineTarget master, FertilityMonitor.Def def) : base(master, def)
		{
			this.fertility = Db.Get().Amounts.Fertility.Lookup(base.gameObject);
			if (GenericGameSettings.instance.acceleratedLifecycle)
			{
				this.fertility.deltaAttribute.Add(new AttributeModifier(this.fertility.deltaAttribute.Id, 33.333332f, "Accelerated Lifecycle", false, false, true));
			}
			float value = 100f / (def.baseFertileCycles * 600f);
			this.fertileEffect = new Effect("Fertile", CREATURES.MODIFIERS.BASE_FERTILITY.NAME, CREATURES.MODIFIERS.BASE_FERTILITY.TOOLTIP, 0f, false, false, false, null, -1f, 0f, null, "");
			this.fertileEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, value, CREATURES.MODIFIERS.BASE_FERTILITY.NAME, false, false, true));
			master.gameObject.GetComponent<KPrefabID>().SetTag(GameTags.Creatures.Expecting, true);
			this.InitializeBreedingChances();
		}

		// Token: 0x06009F49 RID: 40777 RVA: 0x003A5844 File Offset: 0x003A3A44
		[OnDeserialized]
		private void OnDeserialized()
		{
			int num = (base.def.initialBreedingWeights != null) ? base.def.initialBreedingWeights.Count : 0;
			if (this.breedingChances.Count != num)
			{
				this.InitializeBreedingChances();
			}
		}

		// Token: 0x06009F4A RID: 40778 RVA: 0x003A5888 File Offset: 0x003A3A88
		private void InitializeBreedingChances()
		{
			this.breedingChances = new List<FertilityMonitor.BreedingChance>();
			if (base.def.initialBreedingWeights != null)
			{
				foreach (FertilityMonitor.BreedingChance breedingChance in base.def.initialBreedingWeights)
				{
					this.breedingChances.Add(new FertilityMonitor.BreedingChance
					{
						egg = breedingChance.egg,
						weight = breedingChance.weight
					});
					foreach (FertilityModifier fertilityModifier in Db.Get().FertilityModifiers.GetForTag(breedingChance.egg))
					{
						fertilityModifier.ApplyFunction(this, breedingChance.egg);
					}
				}
				this.NormalizeBreedingChances();
			}
		}

		// Token: 0x06009F4B RID: 40779 RVA: 0x003A5980 File Offset: 0x003A3B80
		public void ShowEgg()
		{
			if (this.egg != null)
			{
				bool flag;
				Vector3 vector = base.GetComponent<KBatchedAnimController>().GetSymbolTransform(FertilityMonitor.Instance.targetEggSymbol, out flag).MultiplyPoint3x4(Vector3.zero);
				if (flag)
				{
					vector.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
					int num = Grid.PosToCell(vector);
					if (Grid.IsValidCell(num) && !Grid.Solid[num])
					{
						this.egg.transform.SetPosition(vector);
					}
				}
				this.egg.SetActive(true);
				if (Db.Get().Amounts.Wildness.Lookup(base.gameObject) != null)
				{
					Db.Get().Amounts.Wildness.Copy(this.egg, base.gameObject);
				}
				this.egg = null;
			}
		}

		// Token: 0x06009F4C RID: 40780 RVA: 0x003A5A4C File Offset: 0x003A3C4C
		public void LayEgg()
		{
			this.fertility.value = 0f;
			Vector3 position = base.smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			Tag tag = FertilityMonitor.EggBreedingRoll(this.breedingChances, false);
			if (GenericGameSettings.instance.acceleratedLifecycle)
			{
				float num = 0f;
				foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
				{
					if (breedingChance.weight > num)
					{
						num = breedingChance.weight;
						tag = breedingChance.egg;
					}
				}
			}
			global::Debug.Assert(tag != Tag.Invalid, "Didn't pick an egg to lay. Weights weren't normalized?");
			GameObject prefab = Assets.GetPrefab(tag);
			GameObject gameObject = Util.KInstantiate(prefab, position);
			this.egg = gameObject;
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			string str = "egg01";
			CreatureBrain component2 = Assets.GetPrefab(prefab.GetDef<IncubationMonitor.Def>().spawnedCreature).GetComponent<CreatureBrain>();
			if (!string.IsNullOrEmpty(component2.symbolPrefix))
			{
				str = component2.symbolPrefix + "egg01";
			}
			KAnim.Build.Symbol symbol = this.egg.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol(str);
			if (symbol != null)
			{
				component.AddSymbolOverride(FertilityMonitor.Instance.targetEggSymbol, symbol, 0);
			}
			base.Trigger(1193600993, this.egg);
		}

		// Token: 0x06009F4D RID: 40781 RVA: 0x003A5BC4 File Offset: 0x003A3DC4
		public bool IsReadyToLayEgg()
		{
			return base.smi.fertility.value >= base.smi.fertility.GetMax();
		}

		// Token: 0x06009F4E RID: 40782 RVA: 0x003A5BEC File Offset: 0x003A3DEC
		public void AddBreedingChance(Tag type, float addedPercentChance)
		{
			foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
			{
				if (breedingChance.egg == type)
				{
					float num = Mathf.Min(1f - breedingChance.weight, Mathf.Max(0f - breedingChance.weight, addedPercentChance));
					breedingChance.weight += num;
				}
			}
			this.NormalizeBreedingChances();
			base.master.Trigger(1059811075, this.breedingChances);
		}

		// Token: 0x06009F4F RID: 40783 RVA: 0x003A5C94 File Offset: 0x003A3E94
		public float GetBreedingChance(Tag type)
		{
			foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
			{
				if (breedingChance.egg == type)
				{
					return breedingChance.weight;
				}
			}
			return -1f;
		}

		// Token: 0x06009F50 RID: 40784 RVA: 0x003A5D00 File Offset: 0x003A3F00
		public void NormalizeBreedingChances()
		{
			float num = 0f;
			foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
			{
				num += breedingChance.weight;
			}
			foreach (FertilityMonitor.BreedingChance breedingChance2 in this.breedingChances)
			{
				breedingChance2.weight /= num;
			}
		}

		// Token: 0x06009F51 RID: 40785 RVA: 0x003A5DA4 File Offset: 0x003A3FA4
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			if (this.egg != null)
			{
				UnityEngine.Object.Destroy(this.egg);
				this.egg = null;
			}
		}

		// Token: 0x04007B39 RID: 31545
		public AmountInstance fertility;

		// Token: 0x04007B3A RID: 31546
		private GameObject egg;

		// Token: 0x04007B3B RID: 31547
		[MyCmpReq]
		private KPrefabID prefabId;

		// Token: 0x04007B3C RID: 31548
		[Serialize]
		public List<FertilityMonitor.BreedingChance> breedingChances;

		// Token: 0x04007B3D RID: 31549
		public Effect fertileEffect;

		// Token: 0x04007B3E RID: 31550
		private static HashedString targetEggSymbol = "snapto_egg";
	}
}
