using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B7E RID: 2942
[AddComponentMenu("KMonoBehaviour/scripts/HarvestablePOIStates")]
public class HarvestablePOIStates : GameStateMachine<HarvestablePOIStates, HarvestablePOIStates.Instance, IStateMachineTarget, HarvestablePOIStates.Def>
{
	// Token: 0x060057BD RID: 22461 RVA: 0x001FEB3C File Offset: 0x001FCD3C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.root.Enter(delegate(HarvestablePOIStates.Instance smi)
		{
			if (smi.configuration == null || smi.configuration.typeId == HashedString.Invalid)
			{
				smi.configuration = smi.GetComponent<HarvestablePOIConfigurator>().MakeConfiguration();
				smi.poiCapacity = UnityEngine.Random.Range(0f, smi.configuration.GetMaxCapacity());
			}
		});
		this.idle.ParamTransition<float>(this.poiCapacity, this.recharging, (HarvestablePOIStates.Instance smi, float f) => f < smi.configuration.GetMaxCapacity());
		this.recharging.EventHandler(GameHashes.NewDay, (HarvestablePOIStates.Instance smi) => GameClock.Instance, delegate(HarvestablePOIStates.Instance smi)
		{
			smi.RechargePOI(600f);
		}).ParamTransition<float>(this.poiCapacity, this.idle, (HarvestablePOIStates.Instance smi, float f) => f >= smi.configuration.GetMaxCapacity());
	}

	// Token: 0x04003AD6 RID: 15062
	public GameStateMachine<HarvestablePOIStates, HarvestablePOIStates.Instance, IStateMachineTarget, HarvestablePOIStates.Def>.State idle;

	// Token: 0x04003AD7 RID: 15063
	public GameStateMachine<HarvestablePOIStates, HarvestablePOIStates.Instance, IStateMachineTarget, HarvestablePOIStates.Def>.State recharging;

	// Token: 0x04003AD8 RID: 15064
	public StateMachine<HarvestablePOIStates, HarvestablePOIStates.Instance, IStateMachineTarget, HarvestablePOIStates.Def>.FloatParameter poiCapacity = new StateMachine<HarvestablePOIStates, HarvestablePOIStates.Instance, IStateMachineTarget, HarvestablePOIStates.Def>.FloatParameter(1f);

	// Token: 0x02001CFE RID: 7422
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001CFF RID: 7423
	public new class Instance : GameStateMachine<HarvestablePOIStates, HarvestablePOIStates.Instance, IStateMachineTarget, HarvestablePOIStates.Def>.GameInstance, IGameObjectEffectDescriptor
	{
		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x0600AF7F RID: 44927 RVA: 0x003D63E2 File Offset: 0x003D45E2
		// (set) Token: 0x0600AF80 RID: 44928 RVA: 0x003D63EA File Offset: 0x003D45EA
		public float poiCapacity
		{
			get
			{
				return this._poiCapacity;
			}
			set
			{
				this._poiCapacity = value;
				base.smi.sm.poiCapacity.Set(value, base.smi, false);
			}
		}

		// Token: 0x0600AF81 RID: 44929 RVA: 0x003D6411 File Offset: 0x003D4611
		public Instance(IStateMachineTarget target, HarvestablePOIStates.Def def) : base(target, def)
		{
		}

		// Token: 0x0600AF82 RID: 44930 RVA: 0x003D641C File Offset: 0x003D461C
		public void RechargePOI(float dt)
		{
			float num = dt / this.configuration.GetRechargeTime();
			float delta = this.configuration.GetMaxCapacity() * num;
			this.DeltaPOICapacity(delta);
		}

		// Token: 0x0600AF83 RID: 44931 RVA: 0x003D644C File Offset: 0x003D464C
		public void DeltaPOICapacity(float delta)
		{
			this.poiCapacity += delta;
			this.poiCapacity = Mathf.Min(this.configuration.GetMaxCapacity(), this.poiCapacity);
		}

		// Token: 0x0600AF84 RID: 44932 RVA: 0x003D6478 File Offset: 0x003D4678
		public bool POICanBeHarvested()
		{
			return this.poiCapacity > 0f;
		}

		// Token: 0x0600AF85 RID: 44933 RVA: 0x003D6488 File Offset: 0x003D4688
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			foreach (KeyValuePair<SimHashes, float> keyValuePair in this.configuration.GetElementsWithWeights())
			{
				SimHashes key = keyValuePair.Key;
				string arg = ElementLoader.FindElementByHash(key).tag.ProperName();
				list.Add(new Descriptor(string.Format(UI.SPACEDESTINATIONS.HARVESTABLE_POI.POI_PRODUCTION, arg), string.Format(UI.SPACEDESTINATIONS.HARVESTABLE_POI.POI_PRODUCTION_TOOLTIP, key.ToString()), Descriptor.DescriptorType.Effect, false));
			}
			list.Add(new Descriptor(string.Format("{0}/{1}", GameUtil.GetFormattedMass(this.poiCapacity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedMass(this.configuration.GetMaxCapacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), "Capacity", Descriptor.DescriptorType.Effect, false));
			return list;
		}

		// Token: 0x040089F7 RID: 35319
		[Serialize]
		public HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration configuration;

		// Token: 0x040089F8 RID: 35320
		[Serialize]
		private float _poiCapacity;
	}
}
