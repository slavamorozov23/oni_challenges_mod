using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000ACA RID: 2762
[AddComponentMenu("KMonoBehaviour/Workable/RelaxationPoint")]
public class RelaxationPoint : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x0600505C RID: 20572 RVA: 0x001D2B0E File Offset: 0x001D0D0E
	public RelaxationPoint()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.showProgressBar = false;
	}

	// Token: 0x0600505D RID: 20573 RVA: 0x001D2B28 File Offset: 0x001D0D28
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.lightEfficiencyBonus = false;
		base.GetComponent<KPrefabID>().AddTag(TagManager.Create("RelaxationPoint", MISC.TAGS.RELAXATION_POINT), false);
		if (RelaxationPoint.stressReductionEffect == null)
		{
			RelaxationPoint.stressReductionEffect = this.CreateEffect();
			RelaxationPoint.roomStressReductionEffect = this.CreateRoomEffect();
		}
	}

	// Token: 0x0600505E RID: 20574 RVA: 0x001D2B80 File Offset: 0x001D0D80
	public Effect CreateEffect()
	{
		Effect effect = new Effect("StressReduction", DUPLICANTS.MODIFIERS.STRESSREDUCTION.NAME, DUPLICANTS.MODIFIERS.STRESSREDUCTION.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		AttributeModifier modifier = new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, this.stressModificationValue / 600f, DUPLICANTS.MODIFIERS.STRESSREDUCTION.NAME, false, false, true);
		effect.Add(modifier);
		return effect;
	}

	// Token: 0x0600505F RID: 20575 RVA: 0x001D2C04 File Offset: 0x001D0E04
	public Effect CreateRoomEffect()
	{
		Effect effect = new Effect("RoomRelaxationEffect", DUPLICANTS.MODIFIERS.STRESSREDUCTION_CLINIC.NAME, DUPLICANTS.MODIFIERS.STRESSREDUCTION_CLINIC.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		AttributeModifier modifier = new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, this.roomStressModificationValue / 600f, DUPLICANTS.MODIFIERS.STRESSREDUCTION_CLINIC.NAME, false, false, true);
		effect.Add(modifier);
		return effect;
	}

	// Token: 0x06005060 RID: 20576 RVA: 0x001D2C87 File Offset: 0x001D0E87
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new RelaxationPoint.RelaxationPointSM.Instance(this);
		this.smi.StartSM();
		base.SetWorkTime(float.PositiveInfinity);
	}

	// Token: 0x06005061 RID: 20577 RVA: 0x001D2CB4 File Offset: 0x001D0EB4
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (this.roomTracker != null && this.roomTracker.room != null && this.roomTracker.room.roomType == Db.Get().RoomTypes.MassageClinic)
		{
			worker.GetComponent<Effects>().Add(RelaxationPoint.roomStressReductionEffect, false);
		}
		else
		{
			worker.GetComponent<Effects>().Add(RelaxationPoint.stressReductionEffect, false);
		}
		base.GetComponent<Operational>().SetActive(true, false);
	}

	// Token: 0x06005062 RID: 20578 RVA: 0x001D2D37 File Offset: 0x001D0F37
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (Db.Get().Amounts.Stress.Lookup(worker.gameObject).value <= this.stopStressingValue)
		{
			return true;
		}
		base.OnWorkTick(worker, dt);
		return false;
	}

	// Token: 0x06005063 RID: 20579 RVA: 0x001D2D6C File Offset: 0x001D0F6C
	protected override void OnStopWork(WorkerBase worker)
	{
		worker.GetComponent<Effects>().Remove(RelaxationPoint.stressReductionEffect);
		worker.GetComponent<Effects>().Remove(RelaxationPoint.roomStressReductionEffect);
		base.GetComponent<Operational>().SetActive(false, false);
		base.OnStopWork(worker);
	}

	// Token: 0x06005064 RID: 20580 RVA: 0x001D2DA2 File Offset: 0x001D0FA2
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
	}

	// Token: 0x06005065 RID: 20581 RVA: 0x001D2DAB File Offset: 0x001D0FAB
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x06005066 RID: 20582 RVA: 0x001D2DB0 File Offset: 0x001D0FB0
	protected virtual WorkChore<RelaxationPoint> CreateWorkChore()
	{
		return new WorkChore<RelaxationPoint>(Db.Get().ChoreTypes.Relax, this, null, false, null, null, null, false, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x06005067 RID: 20583 RVA: 0x001D2DE4 File Offset: 0x001D0FE4
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.STRESSREDUCEDPERMINUTE, GameUtil.GetFormattedPercent(this.stressModificationValue / 600f * 60f, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.STRESSREDUCEDPERMINUTE, GameUtil.GetFormattedPercent(this.stressModificationValue / 600f * 60f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
		descriptors.Add(item);
		return descriptors;
	}

	// Token: 0x040035A2 RID: 13730
	[MyCmpGet]
	private RoomTracker roomTracker;

	// Token: 0x040035A3 RID: 13731
	[Serialize]
	protected float stopStressingValue;

	// Token: 0x040035A4 RID: 13732
	public float stressModificationValue;

	// Token: 0x040035A5 RID: 13733
	public float roomStressModificationValue;

	// Token: 0x040035A6 RID: 13734
	private RelaxationPoint.RelaxationPointSM.Instance smi;

	// Token: 0x040035A7 RID: 13735
	private static Effect stressReductionEffect;

	// Token: 0x040035A8 RID: 13736
	private static Effect roomStressReductionEffect;

	// Token: 0x02001C0F RID: 7183
	public class RelaxationPointSM : GameStateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint>
	{
		// Token: 0x0600AC53 RID: 44115 RVA: 0x003CBCB4 File Offset: 0x003C9EB4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (RelaxationPoint.RelaxationPointSM.Instance smi) => smi.GetComponent<Operational>().IsOperational).PlayAnim("off");
			this.operational.ToggleChore((RelaxationPoint.RelaxationPointSM.Instance smi) => smi.master.CreateWorkChore(), this.unoperational);
		}

		// Token: 0x040086D7 RID: 34519
		public GameStateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint, object>.State unoperational;

		// Token: 0x040086D8 RID: 34520
		public GameStateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint, object>.State operational;

		// Token: 0x02002A0E RID: 10766
		public new class Instance : GameStateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint, object>.GameInstance
		{
			// Token: 0x0600D364 RID: 54116 RVA: 0x0043A7C1 File Offset: 0x004389C1
			public Instance(RelaxationPoint master) : base(master)
			{
			}
		}
	}
}
