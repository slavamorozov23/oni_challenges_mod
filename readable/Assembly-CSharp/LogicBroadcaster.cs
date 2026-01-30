using System;
using KSerialization;

// Token: 0x0200078F RID: 1935
public class LogicBroadcaster : KMonoBehaviour, ISimEveryTick
{
	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x06003188 RID: 12680 RVA: 0x0011E137 File Offset: 0x0011C337
	// (set) Token: 0x06003189 RID: 12681 RVA: 0x0011E13F File Offset: 0x0011C33F
	public int BroadCastChannelID
	{
		get
		{
			return this.broadcastChannelID;
		}
		private set
		{
			this.broadcastChannelID = value;
		}
	}

	// Token: 0x0600318A RID: 12682 RVA: 0x0011E148 File Offset: 0x0011C348
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.LogicBroadcasters.Add(this);
	}

	// Token: 0x0600318B RID: 12683 RVA: 0x0011E15B File Offset: 0x0011C35B
	protected override void OnCleanUp()
	{
		Components.LogicBroadcasters.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x0600318C RID: 12684 RVA: 0x0011E170 File Offset: 0x0011C370
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<LogicBroadcaster>(-801688580, LogicBroadcaster.OnLogicValueChangedDelegate);
		base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
		this.operational.SetFlag(LogicBroadcaster.spaceVisible, this.IsSpaceVisible());
		this.wasOperational = !this.operational.IsOperational;
		this.OnOperationalChanged(null);
	}

	// Token: 0x0600318D RID: 12685 RVA: 0x0011E1DD File Offset: 0x0011C3DD
	public bool IsSpaceVisible()
	{
		return base.gameObject.GetMyWorld().IsModuleInterior || Grid.ExposedToSunlight[Grid.PosToCell(base.gameObject)] > 0;
	}

	// Token: 0x0600318E RID: 12686 RVA: 0x0011E20B File Offset: 0x0011C40B
	public int GetCurrentValue()
	{
		return base.GetComponent<LogicPorts>().GetInputValue(this.PORT_ID);
	}

	// Token: 0x0600318F RID: 12687 RVA: 0x0011E223 File Offset: 0x0011C423
	private void OnLogicValueChanged(object data)
	{
	}

	// Token: 0x06003190 RID: 12688 RVA: 0x0011E228 File Offset: 0x0011C428
	public void SimEveryTick(float dt)
	{
		bool flag = this.IsSpaceVisible();
		this.operational.SetFlag(LogicBroadcaster.spaceVisible, flag);
		if (!flag)
		{
			if (this.spaceNotVisibleStatusItem == Guid.Empty)
			{
				this.spaceNotVisibleStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSurfaceSight, null);
				return;
			}
		}
		else if (this.spaceNotVisibleStatusItem != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.spaceNotVisibleStatusItem, false);
			this.spaceNotVisibleStatusItem = Guid.Empty;
		}
	}

	// Token: 0x06003191 RID: 12689 RVA: 0x0011E2B4 File Offset: 0x0011C4B4
	private void OnOperationalChanged(object _)
	{
		if (this.operational.IsOperational)
		{
			if (!this.wasOperational)
			{
				this.wasOperational = true;
				this.animController.Queue("on_pre", KAnim.PlayMode.Once, 1f, 0f);
				this.animController.Queue("on", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
		}
		else if (this.wasOperational)
		{
			this.wasOperational = false;
			this.animController.Queue("on_pst", KAnim.PlayMode.Once, 1f, 0f);
			this.animController.Queue("off", KAnim.PlayMode.Loop, 1f, 0f);
		}
	}

	// Token: 0x04001DCF RID: 7631
	public static int RANGE = 5;

	// Token: 0x04001DD0 RID: 7632
	private static int INVALID_CHANNEL_ID = -1;

	// Token: 0x04001DD1 RID: 7633
	public string PORT_ID = "";

	// Token: 0x04001DD2 RID: 7634
	private bool wasOperational;

	// Token: 0x04001DD3 RID: 7635
	[Serialize]
	private int broadcastChannelID = LogicBroadcaster.INVALID_CHANNEL_ID;

	// Token: 0x04001DD4 RID: 7636
	private static readonly EventSystem.IntraObjectHandler<LogicBroadcaster> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicBroadcaster>(delegate(LogicBroadcaster component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001DD5 RID: 7637
	public static readonly Operational.Flag spaceVisible = new Operational.Flag("spaceVisible", Operational.Flag.Type.Requirement);

	// Token: 0x04001DD6 RID: 7638
	private Guid spaceNotVisibleStatusItem = Guid.Empty;

	// Token: 0x04001DD7 RID: 7639
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001DD8 RID: 7640
	[MyCmpGet]
	private KBatchedAnimController animController;
}
