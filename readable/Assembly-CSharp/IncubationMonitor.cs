using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020008AB RID: 2219
public class IncubationMonitor : GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>
{
	// Token: 0x06003D31 RID: 15665 RVA: 0x001556B4 File Offset: 0x001538B4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.incubating;
		this.root.Enter(delegate(IncubationMonitor.Instance smi)
		{
			smi.OnOperationalChanged(null);
		}).Enter(delegate(IncubationMonitor.Instance smi)
		{
			Components.IncubationMonitors.Add(smi);
		}).Exit(delegate(IncubationMonitor.Instance smi)
		{
			Components.IncubationMonitors.Remove(smi);
		});
		this.incubating.PlayAnim("idle", KAnim.PlayMode.Loop).Transition(this.hatching_pre, new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.Transition.ConditionCallback(IncubationMonitor.IsReadyToHatch), UpdateRate.SIM_1000ms).TagTransition(GameTags.Entombed, this.entombed, false).ParamTransition<bool>(this.isSuppressed, this.suppressed, GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.IsTrue).ToggleEffect((IncubationMonitor.Instance smi) => smi.incubatingEffect);
		this.entombed.TagTransition(GameTags.Entombed, this.incubating, true);
		this.suppressed.ToggleEffect((IncubationMonitor.Instance smi) => this.suppressedEffect).ParamTransition<bool>(this.isSuppressed, this.incubating, GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.IsFalse).TagTransition(GameTags.Entombed, this.entombed, false).Transition(this.not_viable, new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.Transition.ConditionCallback(IncubationMonitor.NoLongerViable), UpdateRate.SIM_1000ms);
		this.hatching_pre.Enter(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.DropSelfFromStorage)).PlayAnim("hatching_pre").OnAnimQueueComplete(this.hatching_pst);
		this.hatching_pst.Enter(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.SpawnBaby)).PlayAnim("hatching_pst").OnAnimQueueComplete(null).Exit(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.DeleteSelf));
		this.not_viable.Enter(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.SpawnGenericEgg)).GoTo(null).Exit(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.DeleteSelf));
		this.suppressedEffect = new Effect("IncubationSuppressed", CREATURES.MODIFIERS.INCUBATING_SUPPRESSED.NAME, CREATURES.MODIFIERS.INCUBATING_SUPPRESSED.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
		this.suppressedEffect.Add(new AttributeModifier(Db.Get().Amounts.Viability.deltaAttribute.Id, -0.016666668f, CREATURES.MODIFIERS.INCUBATING_SUPPRESSED.NAME, false, false, true));
	}

	// Token: 0x06003D32 RID: 15666 RVA: 0x0015592D File Offset: 0x00153B2D
	private static bool IsReadyToHatch(IncubationMonitor.Instance smi)
	{
		return !smi.gameObject.HasTag(GameTags.Entombed) && smi.incubation.value >= smi.incubation.GetMax();
	}

	// Token: 0x06003D33 RID: 15667 RVA: 0x00155960 File Offset: 0x00153B60
	private static void SpawnBaby(IncubationMonitor.Instance smi)
	{
		Vector3 position = smi.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(smi.def.spawnedCreature), position);
		gameObject.SetActive(true);
		gameObject.GetSMI<AnimInterruptMonitor.Instance>().Play("hatching_pst", KAnim.PlayMode.Once);
		KSelectable component = smi.gameObject.GetComponent<KSelectable>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component)
		{
			SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
		}
		if (Db.Get().Amounts.Wildness.Lookup(gameObject) != null)
		{
			Db.Get().Amounts.Wildness.Copy(gameObject, smi.gameObject);
		}
		if (smi.incubator != null)
		{
			smi.incubator.StoreBaby(gameObject);
		}
		IncubationMonitor.SpawnShell(smi);
		SaveLoader.Instance.saveManager.Unregister(smi.GetComponent<SaveLoadRoot>());
	}

	// Token: 0x06003D34 RID: 15668 RVA: 0x00155A6C File Offset: 0x00153C6C
	private static bool NoLongerViable(IncubationMonitor.Instance smi)
	{
		return !smi.gameObject.HasTag(GameTags.Entombed) && smi.viability.value <= smi.viability.GetMin();
	}

	// Token: 0x06003D35 RID: 15669 RVA: 0x00155AA0 File Offset: 0x00153CA0
	private static GameObject SpawnShell(IncubationMonitor.Instance smi)
	{
		if (smi.def.preventEggDrops)
		{
			return null;
		}
		Vector3 position = smi.transform.GetPosition();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("EggShell"), position);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		PrimaryElement component2 = smi.GetComponent<PrimaryElement>();
		component.Mass = component2.Mass * 0.5f;
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06003D36 RID: 15670 RVA: 0x00155B04 File Offset: 0x00153D04
	private static GameObject SpawnEggInnards(IncubationMonitor.Instance smi)
	{
		if (smi.def.preventEggDrops)
		{
			return null;
		}
		Vector3 position = smi.transform.GetPosition();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("RawEgg"), position);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		PrimaryElement component2 = smi.GetComponent<PrimaryElement>();
		component.Mass = component2.Mass * 0.5f;
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06003D37 RID: 15671 RVA: 0x00155B68 File Offset: 0x00153D68
	private static void SpawnGenericEgg(IncubationMonitor.Instance smi)
	{
		IncubationMonitor.SpawnShell(smi);
		GameObject gameObject = IncubationMonitor.SpawnEggInnards(smi);
		KSelectable component = smi.gameObject.GetComponent<KSelectable>();
		if (gameObject != null && SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component)
		{
			SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
		}
	}

	// Token: 0x06003D38 RID: 15672 RVA: 0x00155BDA File Offset: 0x00153DDA
	private static void DeleteSelf(IncubationMonitor.Instance smi)
	{
		smi.gameObject.DeleteObject();
	}

	// Token: 0x06003D39 RID: 15673 RVA: 0x00155BE8 File Offset: 0x00153DE8
	private static void DropSelfFromStorage(IncubationMonitor.Instance smi)
	{
		if (!smi.sm.inIncubator.Get(smi))
		{
			Storage storage = smi.GetStorage();
			if (storage)
			{
				storage.Drop(smi.gameObject, true);
			}
			smi.gameObject.AddTag(GameTags.StoredPrivate);
		}
	}

	// Token: 0x040025BE RID: 9662
	public StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.BoolParameter incubatorIsActive;

	// Token: 0x040025BF RID: 9663
	public StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.BoolParameter inIncubator;

	// Token: 0x040025C0 RID: 9664
	public StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.BoolParameter isSuppressed;

	// Token: 0x040025C1 RID: 9665
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State incubating;

	// Token: 0x040025C2 RID: 9666
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State entombed;

	// Token: 0x040025C3 RID: 9667
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State suppressed;

	// Token: 0x040025C4 RID: 9668
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State hatching_pre;

	// Token: 0x040025C5 RID: 9669
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State hatching_pst;

	// Token: 0x040025C6 RID: 9670
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State not_viable;

	// Token: 0x040025C7 RID: 9671
	private Effect suppressedEffect;

	// Token: 0x020018A2 RID: 6306
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06009F9F RID: 40863 RVA: 0x003A70C0 File Offset: 0x003A52C0
		public override void Configure(GameObject prefab)
		{
			List<string> initialAmounts = prefab.GetComponent<Modifiers>().initialAmounts;
			initialAmounts.Add(Db.Get().Amounts.Wildness.Id);
			initialAmounts.Add(Db.Get().Amounts.Incubation.Id);
			initialAmounts.Add(Db.Get().Amounts.Viability.Id);
		}

		// Token: 0x04007B7F RID: 31615
		public bool preventEggDrops;

		// Token: 0x04007B80 RID: 31616
		public float baseIncubationRate;

		// Token: 0x04007B81 RID: 31617
		public Tag spawnedCreature;
	}

	// Token: 0x020018A3 RID: 6307
	public new class Instance : GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.GameInstance
	{
		// Token: 0x06009FA1 RID: 40865 RVA: 0x003A7130 File Offset: 0x003A5330
		public Instance(IStateMachineTarget master, IncubationMonitor.Def def) : base(master, def)
		{
			this.incubation = Db.Get().Amounts.Incubation.Lookup(base.gameObject);
			Action<object> handler = new Action<object>(this.OnStore);
			master.Subscribe(856640610, handler);
			master.Subscribe(1309017699, handler);
			Action<object> handler2 = new Action<object>(this.OnOperationalChanged);
			master.Subscribe(1628751838, handler2);
			master.Subscribe(960378201, handler2);
			this.wildness = Db.Get().Amounts.Wildness.Lookup(base.gameObject);
			this.wildness.value = this.wildness.GetMax();
			this.viability = Db.Get().Amounts.Viability.Lookup(base.gameObject);
			this.viability.value = this.viability.GetMax();
			float value = def.baseIncubationRate;
			if (GenericGameSettings.instance.acceleratedLifecycle)
			{
				value = 33.333332f;
			}
			AttributeModifier modifier = new AttributeModifier(Db.Get().Amounts.Incubation.deltaAttribute.Id, value, CREATURES.MODIFIERS.BASE_INCUBATION_RATE.NAME, false, false, true);
			this.incubatingEffect = new Effect("Incubating", CREATURES.MODIFIERS.INCUBATING.NAME, CREATURES.MODIFIERS.INCUBATING.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
			this.incubatingEffect.Add(modifier);
		}

		// Token: 0x06009FA2 RID: 40866 RVA: 0x003A72AE File Offset: 0x003A54AE
		public Storage GetStorage()
		{
			if (!(base.transform.parent != null))
			{
				return null;
			}
			return base.transform.parent.GetComponent<Storage>();
		}

		// Token: 0x06009FA3 RID: 40867 RVA: 0x003A72D8 File Offset: 0x003A54D8
		public void OnStore(object data)
		{
			Storage storage = data as Storage;
			bool stored = storage || (data != null && ((Boxed<bool>)data).value);
			EggIncubator eggIncubator = storage ? storage.GetComponent<EggIncubator>() : null;
			this.UpdateIncubationState(stored, eggIncubator);
		}

		// Token: 0x06009FA4 RID: 40868 RVA: 0x003A7324 File Offset: 0x003A5524
		public void OnOperationalChanged(object _ = null)
		{
			bool stored = base.gameObject.HasTag(GameTags.Stored);
			Storage storage = this.GetStorage();
			EggIncubator eggIncubator = storage ? storage.GetComponent<EggIncubator>() : null;
			this.UpdateIncubationState(stored, eggIncubator);
		}

		// Token: 0x06009FA5 RID: 40869 RVA: 0x003A7364 File Offset: 0x003A5564
		private void UpdateIncubationState(bool stored, EggIncubator incubator)
		{
			this.incubator = incubator;
			base.smi.sm.inIncubator.Set(incubator != null, base.smi, false);
			bool value = stored && !incubator;
			base.smi.sm.isSuppressed.Set(value, base.smi, false);
			Operational operational = incubator ? incubator.GetComponent<Operational>() : null;
			bool value2 = incubator && (operational == null || operational.IsOperational);
			base.smi.sm.incubatorIsActive.Set(value2, base.smi, false);
		}

		// Token: 0x06009FA6 RID: 40870 RVA: 0x003A7418 File Offset: 0x003A5618
		public void ApplySongBuff()
		{
			base.GetComponent<Effects>().Add("EggSong", true);
		}

		// Token: 0x06009FA7 RID: 40871 RVA: 0x003A742C File Offset: 0x003A562C
		public bool HasSongBuff()
		{
			return base.GetComponent<Effects>().HasEffect("EggSong");
		}

		// Token: 0x04007B82 RID: 31618
		public AmountInstance incubation;

		// Token: 0x04007B83 RID: 31619
		public AmountInstance wildness;

		// Token: 0x04007B84 RID: 31620
		public AmountInstance viability;

		// Token: 0x04007B85 RID: 31621
		public EggIncubator incubator;

		// Token: 0x04007B86 RID: 31622
		public Effect incubatingEffect;
	}
}
