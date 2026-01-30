using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020005A0 RID: 1440
[AddComponentMenu("KMonoBehaviour/Workable/Breakable")]
public class Breakable : Workable
{
	// Token: 0x17000135 RID: 309
	// (get) Token: 0x0600206B RID: 8299 RVA: 0x000BAAC5 File Offset: 0x000B8CC5
	public bool IsInvincible
	{
		get
		{
			return this.hp == null || this.hp.invincible;
		}
	}

	// Token: 0x0600206C RID: 8300 RVA: 0x000BAAE2 File Offset: 0x000B8CE2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = false;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_break_kanim")
		};
		base.SetWorkTime(float.PositiveInfinity);
	}

	// Token: 0x0600206D RID: 8301 RVA: 0x000BAB1A File Offset: 0x000B8D1A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Breakables.Add(this);
	}

	// Token: 0x0600206E RID: 8302 RVA: 0x000BAB2D File Offset: 0x000B8D2D
	public bool isBroken()
	{
		return this.hp == null || this.hp.HitPoints <= 0;
	}

	// Token: 0x0600206F RID: 8303 RVA: 0x000BAB50 File Offset: 0x000B8D50
	public Notification CreateDamageNotification()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		return new Notification(BUILDING.STATUSITEMS.ANGERDAMAGE.NOTIFICATION, NotificationType.BadMinor, (List<Notification> notificationList, object data) => string.Format(BUILDING.STATUSITEMS.ANGERDAMAGE.NOTIFICATION_TOOLTIP, notificationList.ReduceMessages(false)), component.GetProperName(), false, 0f, null, null, null, true, false, false);
	}

	// Token: 0x06002070 RID: 8304 RVA: 0x000BABA8 File Offset: 0x000B8DA8
	private static string ToolTipResolver(List<Notification> notificationList, object data)
	{
		string text = "";
		for (int i = 0; i < notificationList.Count; i++)
		{
			Notification notification = notificationList[i];
			text += (string)notification.tooltipData;
			if (i < notificationList.Count - 1)
			{
				text += "\n";
			}
		}
		return string.Format(BUILDING.STATUSITEMS.ANGERDAMAGE.NOTIFICATION_TOOLTIP, text);
	}

	// Token: 0x06002071 RID: 8305 RVA: 0x000BAC10 File Offset: 0x000B8E10
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.secondsPerTenPercentDamage = 2f;
		this.tenPercentDamage = Mathf.CeilToInt((float)this.hp.MaxHitPoints * 0.1f);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.AngerDamage, this);
		this.notification = this.CreateDamageNotification();
		base.gameObject.AddOrGet<Notifier>().Add(this.notification, "");
		this.elapsedDamageTime = 0f;
	}

	// Token: 0x06002072 RID: 8306 RVA: 0x000BAC9C File Offset: 0x000B8E9C
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.elapsedDamageTime >= this.secondsPerTenPercentDamage)
		{
			this.elapsedDamageTime -= this.elapsedDamageTime;
			base.BoxingTrigger<BuildingHP.DamageSourceInfo>(-794517298, new BuildingHP.DamageSourceInfo
			{
				damage = this.tenPercentDamage,
				source = BUILDINGS.DAMAGESOURCES.MINION_DESTRUCTION,
				popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.MINION_DESTRUCTION
			});
		}
		this.elapsedDamageTime += dt;
		return this.hp.HitPoints <= 0;
	}

	// Token: 0x06002073 RID: 8307 RVA: 0x000BAD2C File Offset: 0x000B8F2C
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.AngerDamage, false);
		base.gameObject.AddOrGet<Notifier>().Remove(this.notification);
		if (worker != null)
		{
			worker.Trigger(-1734580852, null);
		}
	}

	// Token: 0x06002074 RID: 8308 RVA: 0x000BAD87 File Offset: 0x000B8F87
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x06002075 RID: 8309 RVA: 0x000BAD8A File Offset: 0x000B8F8A
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Breakables.Remove(this);
	}

	// Token: 0x040012E3 RID: 4835
	private const float TIME_TO_BREAK_AT_FULL_HEALTH = 20f;

	// Token: 0x040012E4 RID: 4836
	private Notification notification;

	// Token: 0x040012E5 RID: 4837
	private float secondsPerTenPercentDamage = float.PositiveInfinity;

	// Token: 0x040012E6 RID: 4838
	private float elapsedDamageTime;

	// Token: 0x040012E7 RID: 4839
	private int tenPercentDamage = int.MaxValue;

	// Token: 0x040012E8 RID: 4840
	[MyCmpGet]
	private BuildingHP hp;
}
