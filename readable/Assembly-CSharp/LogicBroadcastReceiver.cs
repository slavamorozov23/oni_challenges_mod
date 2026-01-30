using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200078E RID: 1934
public class LogicBroadcastReceiver : KMonoBehaviour, ISimEveryTick
{
	// Token: 0x06003179 RID: 12665 RVA: 0x0011DC18 File Offset: 0x0011BE18
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
		this.SetChannel(this.channel.Get());
		this.operational.SetFlag(LogicBroadcastReceiver.spaceVisible, this.IsSpaceVisible());
		this.operational.SetFlag(LogicBroadcastReceiver.validChannelInRange, this.CheckChannelValid() && LogicBroadcastReceiver.CheckRange(this.channel.Get().gameObject, base.gameObject));
		this.wasOperational = !this.operational.IsOperational;
		this.OnOperationalChanged(null);
	}

	// Token: 0x0600317A RID: 12666 RVA: 0x0011DCBC File Offset: 0x0011BEBC
	public void SimEveryTick(float dt)
	{
		bool flag = this.IsSpaceVisible();
		this.operational.SetFlag(LogicBroadcastReceiver.spaceVisible, flag);
		if (!flag)
		{
			if (this.spaceNotVisibleStatusItem == Guid.Empty)
			{
				this.spaceNotVisibleStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSurfaceSight, null);
			}
		}
		else if (this.spaceNotVisibleStatusItem != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.spaceNotVisibleStatusItem, false);
			this.spaceNotVisibleStatusItem = Guid.Empty;
		}
		bool flag2 = this.CheckChannelValid() && LogicBroadcastReceiver.CheckRange(this.channel.Get().gameObject, base.gameObject);
		this.operational.SetFlag(LogicBroadcastReceiver.validChannelInRange, flag2);
		if (flag2 && !this.syncToChannelComplete)
		{
			this.SyncWithBroadcast();
		}
	}

	// Token: 0x0600317B RID: 12667 RVA: 0x0011DD92 File Offset: 0x0011BF92
	public bool IsSpaceVisible()
	{
		return base.gameObject.GetMyWorld().IsModuleInterior || Grid.ExposedToSunlight[Grid.PosToCell(base.gameObject)] > 0;
	}

	// Token: 0x0600317C RID: 12668 RVA: 0x0011DDC0 File Offset: 0x0011BFC0
	private bool CheckChannelValid()
	{
		return this.channel.Get() != null && this.channel.Get().GetComponent<LogicPorts>().inputPorts != null;
	}

	// Token: 0x0600317D RID: 12669 RVA: 0x0011DDEF File Offset: 0x0011BFEF
	public LogicBroadcaster GetChannel()
	{
		return this.channel.Get();
	}

	// Token: 0x0600317E RID: 12670 RVA: 0x0011DDFC File Offset: 0x0011BFFC
	public void SetChannel(LogicBroadcaster broadcaster)
	{
		this.ClearChannel();
		if (broadcaster == null)
		{
			return;
		}
		this.channel.Set(broadcaster);
		this.syncToChannelComplete = false;
		this.channelEventListeners.Add(this.channel.Get().gameObject.Subscribe(-801688580, new Action<object>(this.OnChannelLogicEvent)));
		this.channelEventListeners.Add(this.channel.Get().gameObject.Subscribe(-592767678, new Action<object>(this.OnChannelLogicEvent)));
		this.SyncWithBroadcast();
	}

	// Token: 0x0600317F RID: 12671 RVA: 0x0011DE94 File Offset: 0x0011C094
	private void ClearChannel()
	{
		if (this.CheckChannelValid())
		{
			for (int i = 0; i < this.channelEventListeners.Count; i++)
			{
				this.channel.Get().gameObject.Unsubscribe(this.channelEventListeners[i]);
			}
		}
		this.channelEventListeners.Clear();
	}

	// Token: 0x06003180 RID: 12672 RVA: 0x0011DEEB File Offset: 0x0011C0EB
	private void OnChannelLogicEvent(object data)
	{
		if (!this.channel.Get().GetComponent<Operational>().IsOperational)
		{
			return;
		}
		this.SyncWithBroadcast();
	}

	// Token: 0x06003181 RID: 12673 RVA: 0x0011DF0C File Offset: 0x0011C10C
	private void SyncWithBroadcast()
	{
		if (!this.CheckChannelValid())
		{
			return;
		}
		bool flag = LogicBroadcastReceiver.CheckRange(this.channel.Get().gameObject, base.gameObject);
		this.UpdateRangeStatus(flag);
		if (!flag)
		{
			return;
		}
		base.GetComponent<LogicPorts>().SendSignal(this.PORT_ID, this.channel.Get().GetCurrentValue());
		this.syncToChannelComplete = true;
	}

	// Token: 0x06003182 RID: 12674 RVA: 0x0011DF76 File Offset: 0x0011C176
	public static bool CheckRange(GameObject broadcaster, GameObject receiver)
	{
		return AxialUtil.GetDistance(broadcaster.GetMyWorldLocation(), receiver.GetMyWorldLocation()) <= LogicBroadcaster.RANGE;
	}

	// Token: 0x06003183 RID: 12675 RVA: 0x0011DF94 File Offset: 0x0011C194
	private void UpdateRangeStatus(bool inRange)
	{
		if (!inRange && this.rangeStatusItem == Guid.Empty)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			this.rangeStatusItem = component.AddStatusItem(Db.Get().BuildingStatusItems.BroadcasterOutOfRange, null);
			return;
		}
		if (this.rangeStatusItem != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.rangeStatusItem, false);
			this.rangeStatusItem = Guid.Empty;
		}
	}

	// Token: 0x06003184 RID: 12676 RVA: 0x0011E00C File Offset: 0x0011C20C
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

	// Token: 0x06003185 RID: 12677 RVA: 0x0011E0C8 File Offset: 0x0011C2C8
	protected override void OnCleanUp()
	{
		this.ClearChannel();
		base.OnCleanUp();
	}

	// Token: 0x04001DC4 RID: 7620
	[Serialize]
	private Ref<LogicBroadcaster> channel = new Ref<LogicBroadcaster>();

	// Token: 0x04001DC5 RID: 7621
	public string PORT_ID = "";

	// Token: 0x04001DC6 RID: 7622
	private List<int> channelEventListeners = new List<int>();

	// Token: 0x04001DC7 RID: 7623
	private bool syncToChannelComplete;

	// Token: 0x04001DC8 RID: 7624
	public static readonly Operational.Flag spaceVisible = new Operational.Flag("spaceVisible", Operational.Flag.Type.Requirement);

	// Token: 0x04001DC9 RID: 7625
	public static readonly Operational.Flag validChannelInRange = new Operational.Flag("validChannelInRange", Operational.Flag.Type.Requirement);

	// Token: 0x04001DCA RID: 7626
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001DCB RID: 7627
	private bool wasOperational;

	// Token: 0x04001DCC RID: 7628
	[MyCmpGet]
	private KBatchedAnimController animController;

	// Token: 0x04001DCD RID: 7629
	private Guid rangeStatusItem = Guid.Empty;

	// Token: 0x04001DCE RID: 7630
	private Guid spaceNotVisibleStatusItem = Guid.Empty;
}
