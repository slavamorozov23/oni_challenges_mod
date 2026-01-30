using System;
using System.Collections.Generic;
using Database;
using KSerialization;

// Token: 0x02000BE2 RID: 3042
[SerializationConfig(MemberSerialization.OptIn)]
public class StoryInstance : ISaveLoadable
{
	// Token: 0x17000699 RID: 1689
	// (get) Token: 0x06005B06 RID: 23302 RVA: 0x0020F8CA File Offset: 0x0020DACA
	// (set) Token: 0x06005B07 RID: 23303 RVA: 0x0020F8D4 File Offset: 0x0020DAD4
	public StoryInstance.State CurrentState
	{
		get
		{
			return this.state;
		}
		set
		{
			if (this.state == value)
			{
				return;
			}
			this.state = value;
			this.Telemetry.LogStateChange(this.state, GameClock.Instance.GetTimeInCycles());
			Action<StoryInstance.State> storyStateChanged = this.StoryStateChanged;
			if (storyStateChanged == null)
			{
				return;
			}
			storyStateChanged(this.state);
		}
	}

	// Token: 0x1700069A RID: 1690
	// (get) Token: 0x06005B08 RID: 23304 RVA: 0x0020F923 File Offset: 0x0020DB23
	public StoryManager.StoryTelemetry Telemetry
	{
		get
		{
			if (this.telemetry == null)
			{
				this.telemetry = new StoryManager.StoryTelemetry();
			}
			return this.telemetry;
		}
	}

	// Token: 0x1700069B RID: 1691
	// (get) Token: 0x06005B09 RID: 23305 RVA: 0x0020F93E File Offset: 0x0020DB3E
	// (set) Token: 0x06005B0A RID: 23306 RVA: 0x0020F946 File Offset: 0x0020DB46
	public EventInfoData EventInfo { get; private set; }

	// Token: 0x1700069C RID: 1692
	// (get) Token: 0x06005B0B RID: 23307 RVA: 0x0020F94F File Offset: 0x0020DB4F
	// (set) Token: 0x06005B0C RID: 23308 RVA: 0x0020F957 File Offset: 0x0020DB57
	public Notification Notification { get; private set; }

	// Token: 0x1700069D RID: 1693
	// (get) Token: 0x06005B0D RID: 23309 RVA: 0x0020F960 File Offset: 0x0020DB60
	// (set) Token: 0x06005B0E RID: 23310 RVA: 0x0020F968 File Offset: 0x0020DB68
	public EventInfoDataHelper.PopupType PendingType { get; private set; } = EventInfoDataHelper.PopupType.NONE;

	// Token: 0x06005B0F RID: 23311 RVA: 0x0020F971 File Offset: 0x0020DB71
	public Story GetStory()
	{
		if (this._story == null)
		{
			this._story = Db.Get().Stories.Get(this.storyId);
		}
		return this._story;
	}

	// Token: 0x06005B10 RID: 23312 RVA: 0x0020F99C File Offset: 0x0020DB9C
	public StoryInstance()
	{
	}

	// Token: 0x06005B11 RID: 23313 RVA: 0x0020F9B6 File Offset: 0x0020DBB6
	public StoryInstance(Story story, int worldId)
	{
		this._story = story;
		this.storyId = story.Id;
		this.worldId = worldId;
	}

	// Token: 0x06005B12 RID: 23314 RVA: 0x0020F9EA File Offset: 0x0020DBEA
	public bool HasDisplayedPopup(EventInfoDataHelper.PopupType type)
	{
		return this.popupDisplayedStates != null && this.popupDisplayedStates.Contains(type);
	}

	// Token: 0x06005B13 RID: 23315 RVA: 0x0020FA04 File Offset: 0x0020DC04
	public void SetPopupData(StoryManager.PopupInfo info, EventInfoData eventInfo, Notification notification = null)
	{
		this.EventInfo = eventInfo;
		this.Notification = notification;
		this.PendingType = info.PopupType;
		eventInfo.showCallback = (System.Action)Delegate.Combine(eventInfo.showCallback, new System.Action(this.OnPopupDisplayed));
		if (info.DisplayImmediate)
		{
			EventInfoScreen.ShowPopup(eventInfo);
		}
	}

	// Token: 0x06005B14 RID: 23316 RVA: 0x0020FA5C File Offset: 0x0020DC5C
	private void OnPopupDisplayed()
	{
		if (this.popupDisplayedStates == null)
		{
			this.popupDisplayedStates = new HashSet<EventInfoDataHelper.PopupType>();
		}
		this.popupDisplayedStates.Add(this.PendingType);
		this.EventInfo = null;
		this.Notification = null;
		this.PendingType = EventInfoDataHelper.PopupType.NONE;
	}

	// Token: 0x04003CB5 RID: 15541
	public Action<StoryInstance.State> StoryStateChanged;

	// Token: 0x04003CB6 RID: 15542
	[Serialize]
	public readonly string storyId;

	// Token: 0x04003CB7 RID: 15543
	[Serialize]
	public int worldId;

	// Token: 0x04003CB8 RID: 15544
	[Serialize]
	private StoryInstance.State state;

	// Token: 0x04003CB9 RID: 15545
	[Serialize]
	private StoryManager.StoryTelemetry telemetry;

	// Token: 0x04003CBA RID: 15546
	[Serialize]
	private HashSet<EventInfoDataHelper.PopupType> popupDisplayedStates = new HashSet<EventInfoDataHelper.PopupType>();

	// Token: 0x04003CBE RID: 15550
	private Story _story;

	// Token: 0x02001D73 RID: 7539
	public enum State
	{
		// Token: 0x04008B54 RID: 35668
		RETROFITTED = -1,
		// Token: 0x04008B55 RID: 35669
		NOT_STARTED,
		// Token: 0x04008B56 RID: 35670
		DISCOVERED,
		// Token: 0x04008B57 RID: 35671
		IN_PROGRESS,
		// Token: 0x04008B58 RID: 35672
		COMPLETE
	}
}
