using System;
using Database;
using UnityEngine;

// Token: 0x02000B50 RID: 2896
public class SkyVisibilityMonitor : GameStateMachine<SkyVisibilityMonitor, SkyVisibilityMonitor.Instance, IStateMachineTarget, SkyVisibilityMonitor.Def>
{
	// Token: 0x06005588 RID: 21896 RVA: 0x001F35A1 File Offset: 0x001F17A1
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update(new Action<SkyVisibilityMonitor.Instance, float>(SkyVisibilityMonitor.CheckSkyVisibility), UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x06005589 RID: 21897 RVA: 0x001F35C8 File Offset: 0x001F17C8
	public static void CheckSkyVisibility(SkyVisibilityMonitor.Instance smi, float dt)
	{
		bool hasSkyVisibility = smi.HasSkyVisibility;
		ValueTuple<bool, float> visibilityOf = smi.def.skyVisibilityInfo.GetVisibilityOf(smi.gameObject);
		bool item = visibilityOf.Item1;
		float item2 = visibilityOf.Item2;
		smi.Internal_SetPercentClearSky(item2);
		KSelectable component = smi.GetComponent<KSelectable>();
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisNone, !item, smi);
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisLimited, item && item2 < 1f, smi);
		if (hasSkyVisibility == item)
		{
			return;
		}
		smi.TriggerVisibilityChange();
	}

	// Token: 0x02001CB7 RID: 7351
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04008903 RID: 35075
		public SkyVisibilityInfo skyVisibilityInfo;
	}

	// Token: 0x02001CB8 RID: 7352
	public new class Instance : GameStateMachine<SkyVisibilityMonitor, SkyVisibilityMonitor.Instance, IStateMachineTarget, SkyVisibilityMonitor.Def>.GameInstance, BuildingStatusItems.ISkyVisInfo
	{
		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x0600AE56 RID: 44630 RVA: 0x003D323C File Offset: 0x003D143C
		public bool HasSkyVisibility
		{
			get
			{
				return this.PercentClearSky > 0f && !Mathf.Approximately(0f, this.PercentClearSky);
			}
		}

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x0600AE57 RID: 44631 RVA: 0x003D3260 File Offset: 0x003D1460
		public float PercentClearSky
		{
			get
			{
				return this.percentClearSky01;
			}
		}

		// Token: 0x0600AE58 RID: 44632 RVA: 0x003D3268 File Offset: 0x003D1468
		public void Internal_SetPercentClearSky(float percent01)
		{
			this.percentClearSky01 = percent01;
		}

		// Token: 0x0600AE59 RID: 44633 RVA: 0x003D3271 File Offset: 0x003D1471
		float BuildingStatusItems.ISkyVisInfo.GetPercentVisible01()
		{
			return this.percentClearSky01;
		}

		// Token: 0x0600AE5A RID: 44634 RVA: 0x003D3279 File Offset: 0x003D1479
		public Instance(IStateMachineTarget master, SkyVisibilityMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x0600AE5B RID: 44635 RVA: 0x003D3283 File Offset: 0x003D1483
		public override void StartSM()
		{
			base.StartSM();
			SkyVisibilityMonitor.CheckSkyVisibility(this, 0f);
			this.TriggerVisibilityChange();
		}

		// Token: 0x0600AE5C RID: 44636 RVA: 0x003D329C File Offset: 0x003D149C
		public void TriggerVisibilityChange()
		{
			if (this.visibilityStatusItem != null)
			{
				base.smi.GetComponent<KSelectable>().ToggleStatusItem(this.visibilityStatusItem, !this.HasSkyVisibility, this);
			}
			base.smi.GetComponent<Operational>().SetFlag(SkyVisibilityMonitor.Instance.skyVisibilityFlag, this.HasSkyVisibility);
			if (this.SkyVisibilityChanged != null)
			{
				this.SkyVisibilityChanged();
			}
		}

		// Token: 0x04008904 RID: 35076
		private float percentClearSky01;

		// Token: 0x04008905 RID: 35077
		public System.Action SkyVisibilityChanged;

		// Token: 0x04008906 RID: 35078
		private StatusItem visibilityStatusItem;

		// Token: 0x04008907 RID: 35079
		private static readonly Operational.Flag skyVisibilityFlag = new Operational.Flag("sky visibility", Operational.Flag.Type.Requirement);
	}
}
