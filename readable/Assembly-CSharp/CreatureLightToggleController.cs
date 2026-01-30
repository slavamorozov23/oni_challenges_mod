using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200088F RID: 2191
public class CreatureLightToggleController : GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>
{
	// Token: 0x06003C50 RID: 15440 RVA: 0x001516AC File Offset: 0x0014F8AC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.light_off;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.light_off.Enter(delegate(CreatureLightToggleController.Instance smi)
		{
			smi.SwitchLight(false);
		}).EventHandlerTransition(GameHashes.TagsChanged, this.turning_on, new Func<CreatureLightToggleController.Instance, object, bool>(CreatureLightToggleController.ShouldProduceLight));
		this.turning_off.BatchUpdate(delegate(List<UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry> instances, float time_delta)
		{
			CreatureLightToggleController.Instance.ModifyBrightness(instances, CreatureLightToggleController.Instance.dim, time_delta);
		}, UpdateRate.SIM_200ms).Transition(this.light_off, (CreatureLightToggleController.Instance smi) => smi.IsOff(), UpdateRate.SIM_200ms);
		this.light_on.Enter(delegate(CreatureLightToggleController.Instance smi)
		{
			smi.SwitchLight(true);
		}).EventHandlerTransition(GameHashes.TagsChanged, this.turning_off, (CreatureLightToggleController.Instance smi, object obj) => !CreatureLightToggleController.ShouldProduceLight(smi, obj));
		this.turning_on.Enter(delegate(CreatureLightToggleController.Instance smi)
		{
			smi.SwitchLight(true);
		}).BatchUpdate(delegate(List<UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry> instances, float time_delta)
		{
			CreatureLightToggleController.Instance.ModifyBrightness(instances, CreatureLightToggleController.Instance.brighten, time_delta);
		}, UpdateRate.SIM_200ms).Transition(this.light_on, (CreatureLightToggleController.Instance smi) => smi.IsOn(), UpdateRate.SIM_200ms);
	}

	// Token: 0x06003C51 RID: 15441 RVA: 0x0015183B File Offset: 0x0014FA3B
	public static bool ShouldProduceLight(CreatureLightToggleController.Instance smi, object obj)
	{
		return !smi.prefabID.HasTag(GameTags.Creatures.Overcrowded) && !smi.prefabID.HasTag(GameTags.Creatures.TrappedInCargoBay);
	}

	// Token: 0x0400252F RID: 9519
	private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State light_off;

	// Token: 0x04002530 RID: 9520
	private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State turning_off;

	// Token: 0x04002531 RID: 9521
	private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State light_on;

	// Token: 0x04002532 RID: 9522
	private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State turning_on;

	// Token: 0x02001862 RID: 6242
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001863 RID: 6243
	public new class Instance : GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.GameInstance
	{
		// Token: 0x06009EB7 RID: 40631 RVA: 0x003A3F04 File Offset: 0x003A2104
		public Instance(IStateMachineTarget master, CreatureLightToggleController.Def def) : base(master, def)
		{
			this.prefabID = base.gameObject.GetComponent<KPrefabID>();
			this.light = master.GetComponent<Light2D>();
			this.originalLux = this.light.Lux;
			this.originalRange = this.light.Range;
		}

		// Token: 0x06009EB8 RID: 40632 RVA: 0x003A3F58 File Offset: 0x003A2158
		public void SwitchLight(bool on)
		{
			this.light.enabled = on;
		}

		// Token: 0x06009EB9 RID: 40633 RVA: 0x003A3F68 File Offset: 0x003A2168
		public static void ModifyBrightness(List<UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry> instances, CreatureLightToggleController.Instance.ModifyLuxDelegate modify_lux, float time_delta)
		{
			CreatureLightToggleController.Instance.modify_brightness_job.Reset(null);
			for (int num = 0; num != instances.Count; num++)
			{
				UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry entry = instances[num];
				entry.lastUpdateTime = 0f;
				instances[num] = entry;
				CreatureLightToggleController.Instance data = entry.data;
				modify_lux(data, time_delta);
				data.light.Range = data.originalRange * (float)data.light.Lux / (float)data.originalLux;
				data.light.RefreshShapeAndPosition();
				if (data.light.RefreshShapeAndPosition() != Light2D.RefreshResult.None)
				{
					CreatureLightToggleController.Instance.modify_brightness_job.Add(new CreatureLightToggleController.Instance.ModifyBrightnessTask(data.light.emitter));
				}
			}
			GlobalJobManager.Run(CreatureLightToggleController.Instance.modify_brightness_job);
			for (int num2 = 0; num2 != CreatureLightToggleController.Instance.modify_brightness_job.Count; num2++)
			{
				CreatureLightToggleController.Instance.modify_brightness_job.GetWorkItem(num2).Finish();
			}
			CreatureLightToggleController.Instance.modify_brightness_job.Reset(null);
		}

		// Token: 0x06009EBA RID: 40634 RVA: 0x003A4059 File Offset: 0x003A2259
		public bool IsOff()
		{
			return this.light.Lux == 0;
		}

		// Token: 0x06009EBB RID: 40635 RVA: 0x003A4069 File Offset: 0x003A2269
		public bool IsOn()
		{
			return this.light.Lux >= this.originalLux;
		}

		// Token: 0x04007ABC RID: 31420
		private const float DIM_TIME = 25f;

		// Token: 0x04007ABD RID: 31421
		private const float GLOW_TIME = 15f;

		// Token: 0x04007ABE RID: 31422
		private int originalLux;

		// Token: 0x04007ABF RID: 31423
		private float originalRange;

		// Token: 0x04007AC0 RID: 31424
		private Light2D light;

		// Token: 0x04007AC1 RID: 31425
		public KPrefabID prefabID;

		// Token: 0x04007AC2 RID: 31426
		private static WorkItemCollection<CreatureLightToggleController.Instance.ModifyBrightnessTask, object> modify_brightness_job = new WorkItemCollection<CreatureLightToggleController.Instance.ModifyBrightnessTask, object>();

		// Token: 0x04007AC3 RID: 31427
		public static CreatureLightToggleController.Instance.ModifyLuxDelegate dim = delegate(CreatureLightToggleController.Instance instance, float time_delta)
		{
			float num = (float)instance.originalLux / 25f;
			instance.light.Lux = Mathf.FloorToInt(Mathf.Max(0f, (float)instance.light.Lux - num * time_delta));
		};

		// Token: 0x04007AC4 RID: 31428
		public static CreatureLightToggleController.Instance.ModifyLuxDelegate brighten = delegate(CreatureLightToggleController.Instance instance, float time_delta)
		{
			float num = (float)instance.originalLux / 15f;
			instance.light.Lux = Mathf.CeilToInt(Mathf.Min((float)instance.originalLux, (float)instance.light.Lux + num * time_delta));
		};

		// Token: 0x02002983 RID: 10627
		private struct ModifyBrightnessTask : IWorkItem<object>
		{
			// Token: 0x0600D14C RID: 53580 RVA: 0x00437B92 File Offset: 0x00435D92
			public ModifyBrightnessTask(LightGridManager.LightGridEmitter emitter)
			{
				this.emitter = emitter;
				emitter.RemoveFromGrid();
			}

			// Token: 0x0600D14D RID: 53581 RVA: 0x00437BA1 File Offset: 0x00435DA1
			public void Run(object context, int threadIndex)
			{
				this.emitter.UpdateLitCells();
			}

			// Token: 0x0600D14E RID: 53582 RVA: 0x00437BAE File Offset: 0x00435DAE
			public void Finish()
			{
				this.emitter.AddToGrid(false);
			}

			// Token: 0x0400B79F RID: 47007
			private LightGridManager.LightGridEmitter emitter;
		}

		// Token: 0x02002984 RID: 10628
		// (Invoke) Token: 0x0600D150 RID: 53584
		public delegate void ModifyLuxDelegate(CreatureLightToggleController.Instance instance, float time_delta);
	}
}
