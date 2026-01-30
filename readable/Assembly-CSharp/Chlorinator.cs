using System;
using Klei;
using UnityEngine;

// Token: 0x0200071B RID: 1819
public class Chlorinator : GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>
{
	// Token: 0x06002D71 RID: 11633 RVA: 0x00107464 File Offset: 0x00105664
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.TagTransition(GameTags.Operational, this.ready, false);
		this.ready.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle);
		this.ready.idle.EventTransition(GameHashes.OnStorageChange, this.ready.wait, (Chlorinator.StatesInstance smi) => smi.CanEmit()).EnterTransition(this.ready.wait, (Chlorinator.StatesInstance smi) => smi.CanEmit()).Target(this.hopper).PlayAnim("hopper_idle_loop");
		this.ready.wait.ScheduleGoTo(new Func<Chlorinator.StatesInstance, float>(Chlorinator.GetPoppingDelay), this.ready.popPre).EnterTransition(this.ready.idle, (Chlorinator.StatesInstance smi) => !smi.CanEmit()).Target(this.hopper).PlayAnim("hopper_idle_loop");
		this.ready.popPre.Target(this.hopper).PlayAnim("meter_hopper_pre").OnAnimQueueComplete(this.ready.pop);
		this.ready.pop.Enter(delegate(Chlorinator.StatesInstance smi)
		{
			smi.TryEmit();
		}).Target(this.hopper).PlayAnim("meter_hopper_loop").OnAnimQueueComplete(this.ready.popPst);
		this.ready.popPst.Target(this.hopper).PlayAnim("meter_hopper_pst").OnAnimQueueComplete(this.ready.wait);
	}

	// Token: 0x06002D72 RID: 11634 RVA: 0x00107660 File Offset: 0x00105860
	public static float GetPoppingDelay(Chlorinator.StatesInstance smi)
	{
		return smi.def.popWaitRange.Get();
	}

	// Token: 0x04001B04 RID: 6916
	private GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State inoperational;

	// Token: 0x04001B05 RID: 6917
	private Chlorinator.ReadyStates ready;

	// Token: 0x04001B06 RID: 6918
	public StateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.TargetParameter hopper;

	// Token: 0x020015E6 RID: 5606
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040072FE RID: 29438
		public MathUtil.MinMax popWaitRange = new MathUtil.MinMax(0.2f, 0.8f);

		// Token: 0x040072FF RID: 29439
		public Tag primaryOreTag;

		// Token: 0x04007300 RID: 29440
		public float primaryOreMassPerOre;

		// Token: 0x04007301 RID: 29441
		public MathUtil.MinMaxInt primaryOreCount = new MathUtil.MinMaxInt(1, 1);

		// Token: 0x04007302 RID: 29442
		public Tag secondaryOreTag;

		// Token: 0x04007303 RID: 29443
		public float secondaryOreMassPerOre;

		// Token: 0x04007304 RID: 29444
		public MathUtil.MinMaxInt secondaryOreCount = new MathUtil.MinMaxInt(1, 1);

		// Token: 0x04007305 RID: 29445
		public Vector3 offset = Vector3.zero;

		// Token: 0x04007306 RID: 29446
		public MathUtil.MinMax initialVelocity = new MathUtil.MinMax(1f, 3f);

		// Token: 0x04007307 RID: 29447
		public MathUtil.MinMax initialDirectionHalfAngleDegreesRange = new MathUtil.MinMax(160f, 20f);
	}

	// Token: 0x020015E7 RID: 5607
	public class ReadyStates : GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State
	{
		// Token: 0x04007308 RID: 29448
		public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State idle;

		// Token: 0x04007309 RID: 29449
		public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State wait;

		// Token: 0x0400730A RID: 29450
		public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State popPre;

		// Token: 0x0400730B RID: 29451
		public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State pop;

		// Token: 0x0400730C RID: 29452
		public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State popPst;
	}

	// Token: 0x020015E8 RID: 5608
	public class StatesInstance : GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.GameInstance
	{
		// Token: 0x060094FC RID: 38140 RVA: 0x0037AC58 File Offset: 0x00378E58
		public StatesInstance(IStateMachineTarget master, Chlorinator.Def def) : base(master, def)
		{
			this.storage = base.GetComponent<ComplexFabricator>().outStorage;
			KAnimControllerBase component = master.GetComponent<KAnimControllerBase>();
			this.hopperMeter = new MeterController(component, "meter_target", "meter_hopper_pre", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target"
			});
			base.sm.hopper.Set(this.hopperMeter.gameObject, this, false);
		}

		// Token: 0x060094FD RID: 38141 RVA: 0x0037ACCA File Offset: 0x00378ECA
		public bool CanEmit()
		{
			return !this.storage.IsEmpty();
		}

		// Token: 0x060094FE RID: 38142 RVA: 0x0037ACDC File Offset: 0x00378EDC
		public void TryEmit()
		{
			this.TryEmit(base.smi.def.primaryOreCount.Get(), base.def.primaryOreTag, base.def.primaryOreMassPerOre);
			this.TryEmit(base.smi.def.secondaryOreCount.Get(), base.def.secondaryOreTag, base.def.secondaryOreMassPerOre);
		}

		// Token: 0x060094FF RID: 38143 RVA: 0x0037AD4C File Offset: 0x00378F4C
		private void TryEmit(int oreSpawnCount, Tag emitTag, float amount)
		{
			GameObject gameObject = this.storage.FindFirst(emitTag);
			if (gameObject == null)
			{
				return;
			}
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			Substance substance = component.Element.substance;
			float num;
			SimUtil.DiseaseInfo diseaseInfo;
			float temperature;
			this.storage.ConsumeAndGetDisease(emitTag, amount, out num, out diseaseInfo, out temperature);
			if (num <= 0f)
			{
				return;
			}
			float mass = num * component.MassPerUnit / (float)oreSpawnCount;
			Vector3 vector = base.smi.gameObject.transform.position;
			vector += base.def.offset;
			bool flag = UnityEngine.Random.value >= 0.5f;
			for (int i = 0; i < oreSpawnCount; i++)
			{
				float f = base.def.initialDirectionHalfAngleDegreesRange.Get() * 3.1415927f / 180f;
				Vector2 normalized = new Vector2(-Mathf.Cos(f), Mathf.Sin(f));
				if (flag)
				{
					normalized.x = -normalized.x;
				}
				flag = !flag;
				normalized = normalized.normalized;
				Vector3 v = normalized * base.def.initialVelocity.Get();
				Vector3 vector2 = vector;
				vector2 += normalized * 0.1f;
				GameObject go = substance.SpawnResource(vector2, mass, temperature, diseaseInfo.idx, diseaseInfo.count / oreSpawnCount, false, false, false);
				KFMOD.PlayOneShot(GlobalAssets.GetSound("Chlorinator_popping", false), CameraController.Instance.GetVerticallyScaledPosition(vector2, false), 1f);
				if (GameComps.Fallers.Has(go))
				{
					GameComps.Fallers.Remove(go);
				}
				GameComps.Fallers.Add(go, v);
			}
		}

		// Token: 0x0400730D RID: 29453
		public Storage storage;

		// Token: 0x0400730E RID: 29454
		public MeterController hopperMeter;
	}
}
