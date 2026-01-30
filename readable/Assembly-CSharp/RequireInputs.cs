using System;
using UnityEngine;

// Token: 0x02000AF9 RID: 2809
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/RequireInputs")]
public class RequireInputs : KMonoBehaviour, ISim200ms
{
	// Token: 0x170005A7 RID: 1447
	// (get) Token: 0x060051AE RID: 20910 RVA: 0x001DA1E7 File Offset: 0x001D83E7
	public bool RequiresPower
	{
		get
		{
			return this.requirePower;
		}
	}

	// Token: 0x170005A8 RID: 1448
	// (get) Token: 0x060051AF RID: 20911 RVA: 0x001DA1EF File Offset: 0x001D83EF
	public bool RequiresInputConduit
	{
		get
		{
			return this.requireConduit;
		}
	}

	// Token: 0x060051B0 RID: 20912 RVA: 0x001DA1F7 File Offset: 0x001D83F7
	public void SetRequirements(bool power, bool conduit)
	{
		this.requirePower = power;
		this.requireConduit = conduit;
	}

	// Token: 0x170005A9 RID: 1449
	// (get) Token: 0x060051B1 RID: 20913 RVA: 0x001DA207 File Offset: 0x001D8407
	public bool RequirementsMet
	{
		get
		{
			return this.requirementsMet;
		}
	}

	// Token: 0x060051B2 RID: 20914 RVA: 0x001DA20F File Offset: 0x001D840F
	protected override void OnPrefabInit()
	{
		this.Bind();
	}

	// Token: 0x060051B3 RID: 20915 RVA: 0x001DA217 File Offset: 0x001D8417
	protected override void OnSpawn()
	{
		this.CheckRequirements(true);
		this.Bind();
	}

	// Token: 0x060051B4 RID: 20916 RVA: 0x001DA228 File Offset: 0x001D8428
	[ContextMenu("Bind")]
	private void Bind()
	{
		if (this.requirePower)
		{
			this.energy = base.GetComponent<IEnergyConsumer>();
			this.button = base.GetComponent<BuildingEnabledButton>();
		}
		if (this.requireConduit && !this.conduitConsumer)
		{
			this.conduitConsumer = base.GetComponent<ConduitConsumer>();
		}
	}

	// Token: 0x060051B5 RID: 20917 RVA: 0x001DA276 File Offset: 0x001D8476
	public void Sim200ms(float dt)
	{
		this.CheckRequirements(false);
	}

	// Token: 0x060051B6 RID: 20918 RVA: 0x001DA280 File Offset: 0x001D8480
	private void CheckRequirements(bool forceEvent)
	{
		bool flag = true;
		bool flag2 = false;
		if (this.requirePower)
		{
			bool isConnected = this.energy.IsConnected;
			bool isPowered = this.energy.IsPowered;
			flag = (flag && isPowered && isConnected);
			bool show = this.VisualizeRequirement(RequireInputs.Requirements.NeedPower) && isConnected && !isPowered && (this.button == null || this.button.IsEnabled);
			bool show2 = this.VisualizeRequirement(RequireInputs.Requirements.NoWire) && !isConnected;
			this.needPowerStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedPower, this.needPowerStatusGuid, show, this);
			this.noWireStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NoWireConnected, this.noWireStatusGuid, show2, this);
			flag2 = (flag != this.RequirementsMet && base.GetComponent<Light2D>() != null);
		}
		if (this.requireConduit)
		{
			bool flag3 = !this.conduitConsumer.enabled || this.conduitConsumer.IsConnected;
			bool flag4 = !this.conduitConsumer.enabled || this.conduitConsumer.IsSatisfied;
			if (this.VisualizeRequirement(RequireInputs.Requirements.ConduitConnected) && this.previouslyConnected != flag3)
			{
				this.previouslyConnected = flag3;
				StatusItem statusItem = null;
				ConduitType typeOfConduit = this.conduitConsumer.TypeOfConduit;
				if (typeOfConduit != ConduitType.Gas)
				{
					if (typeOfConduit == ConduitType.Liquid)
					{
						statusItem = Db.Get().BuildingStatusItems.NeedLiquidIn;
					}
				}
				else
				{
					statusItem = Db.Get().BuildingStatusItems.NeedGasIn;
				}
				if (statusItem != null)
				{
					this.selectable.ToggleStatusItem(statusItem, !flag3, new global::Tuple<ConduitType, Tag>(this.conduitConsumer.TypeOfConduit, this.conduitConsumer.capacityTag));
				}
				this.operational.SetFlag(RequireInputs.inputConnectedFlag, flag3);
			}
			flag = (flag && flag3);
			if (this.VisualizeRequirement(RequireInputs.Requirements.ConduitEmpty) && this.previouslySatisfied != flag4)
			{
				this.previouslySatisfied = flag4;
				StatusItem statusItem2 = null;
				ConduitType typeOfConduit = this.conduitConsumer.TypeOfConduit;
				if (typeOfConduit != ConduitType.Gas)
				{
					if (typeOfConduit == ConduitType.Liquid)
					{
						statusItem2 = Db.Get().BuildingStatusItems.LiquidPipeEmpty;
					}
				}
				else
				{
					statusItem2 = Db.Get().BuildingStatusItems.GasPipeEmpty;
				}
				if (this.requireConduitHasMass)
				{
					if (statusItem2 != null)
					{
						this.selectable.ToggleStatusItem(statusItem2, !flag4, this);
					}
					this.operational.SetFlag(RequireInputs.pipesHaveMass, flag4);
				}
			}
		}
		this.requirementsMet = flag;
		if (flag2)
		{
			Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
			if (roomOfGameObject != null)
			{
				Game.Instance.roomProber.UpdateRoom(roomOfGameObject.cavity);
			}
		}
	}

	// Token: 0x060051B7 RID: 20919 RVA: 0x001DA51C File Offset: 0x001D871C
	public bool VisualizeRequirement(RequireInputs.Requirements r)
	{
		return (this.visualizeRequirements & r) == r;
	}

	// Token: 0x04003743 RID: 14147
	[SerializeField]
	private bool requirePower = true;

	// Token: 0x04003744 RID: 14148
	[SerializeField]
	private bool requireConduit;

	// Token: 0x04003745 RID: 14149
	public bool requireConduitHasMass = true;

	// Token: 0x04003746 RID: 14150
	public RequireInputs.Requirements visualizeRequirements = RequireInputs.Requirements.All;

	// Token: 0x04003747 RID: 14151
	private static readonly Operational.Flag inputConnectedFlag = new Operational.Flag("inputConnected", Operational.Flag.Type.Requirement);

	// Token: 0x04003748 RID: 14152
	private static readonly Operational.Flag pipesHaveMass = new Operational.Flag("pipesHaveMass", Operational.Flag.Type.Requirement);

	// Token: 0x04003749 RID: 14153
	private Guid noWireStatusGuid;

	// Token: 0x0400374A RID: 14154
	private Guid needPowerStatusGuid;

	// Token: 0x0400374B RID: 14155
	private bool requirementsMet;

	// Token: 0x0400374C RID: 14156
	private BuildingEnabledButton button;

	// Token: 0x0400374D RID: 14157
	private IEnergyConsumer energy;

	// Token: 0x0400374E RID: 14158
	public ConduitConsumer conduitConsumer;

	// Token: 0x0400374F RID: 14159
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04003750 RID: 14160
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04003751 RID: 14161
	private bool previouslyConnected = true;

	// Token: 0x04003752 RID: 14162
	private bool previouslySatisfied = true;

	// Token: 0x02001C31 RID: 7217
	[Flags]
	public enum Requirements
	{
		// Token: 0x0400872B RID: 34603
		None = 0,
		// Token: 0x0400872C RID: 34604
		NoWire = 1,
		// Token: 0x0400872D RID: 34605
		NeedPower = 2,
		// Token: 0x0400872E RID: 34606
		ConduitConnected = 4,
		// Token: 0x0400872F RID: 34607
		ConduitEmpty = 8,
		// Token: 0x04008730 RID: 34608
		AllPower = 3,
		// Token: 0x04008731 RID: 34609
		AllConduit = 12,
		// Token: 0x04008732 RID: 34610
		All = 15
	}
}
