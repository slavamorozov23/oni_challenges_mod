using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020006FD RID: 1789
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/BuildingHP")]
public class BuildingHP : Workable
{
	// Token: 0x17000223 RID: 547
	// (get) Token: 0x06002C53 RID: 11347 RVA: 0x0010223E File Offset: 0x0010043E
	public int HitPoints
	{
		get
		{
			return this.hitpoints;
		}
	}

	// Token: 0x06002C54 RID: 11348 RVA: 0x00102246 File Offset: 0x00100446
	public void SetHitPoints(int hp)
	{
		this.hitpoints = hp;
	}

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x06002C55 RID: 11349 RVA: 0x0010224F File Offset: 0x0010044F
	public int MaxHitPoints
	{
		get
		{
			return this.building.Def.HitPoints;
		}
	}

	// Token: 0x06002C56 RID: 11350 RVA: 0x00102261 File Offset: 0x00100461
	public BuildingHP.DamageSourceInfo GetDamageSourceInfo()
	{
		return this.damageSourceInfo;
	}

	// Token: 0x06002C57 RID: 11351 RVA: 0x00102269 File Offset: 0x00100469
	protected override void OnLoadLevel()
	{
		this.smi = null;
		base.OnLoadLevel();
	}

	// Token: 0x06002C58 RID: 11352 RVA: 0x00102278 File Offset: 0x00100478
	public void DoDamage(int damage)
	{
		if (!this.invincible)
		{
			damage = Math.Max(0, damage);
			this.hitpoints = Math.Max(0, this.hitpoints - damage);
			base.Trigger(-1964935036, this);
		}
	}

	// Token: 0x06002C59 RID: 11353 RVA: 0x001022AC File Offset: 0x001004AC
	public void Repair(int repair_amount)
	{
		if (this.hitpoints + repair_amount < this.hitpoints)
		{
			this.hitpoints = this.building.Def.HitPoints;
		}
		else
		{
			this.hitpoints = Math.Min(this.hitpoints + repair_amount, this.building.Def.HitPoints);
		}
		base.Trigger(-1699355994, this);
		if (this.hitpoints >= this.building.Def.HitPoints)
		{
			base.Trigger(-1735440190, this);
		}
	}

	// Token: 0x06002C5A RID: 11354 RVA: 0x00102334 File Offset: 0x00100534
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(10f);
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
	}

	// Token: 0x06002C5B RID: 11355 RVA: 0x00102368 File Offset: 0x00100568
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new BuildingHP.SMInstance(this);
		this.smi.StartSM();
		base.Subscribe<BuildingHP>(-794517298, BuildingHP.OnDoBuildingDamageDelegate);
		if (this.destroyOnDamaged)
		{
			base.Subscribe<BuildingHP>(774203113, BuildingHP.DestroyOnDamagedDelegate);
		}
		if (this.hitpoints <= 0)
		{
			base.Trigger(774203113, this);
		}
	}

	// Token: 0x06002C5C RID: 11356 RVA: 0x001023D1 File Offset: 0x001005D1
	private void DestroyOnDamaged(object data)
	{
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06002C5D RID: 11357 RVA: 0x001023E0 File Offset: 0x001005E0
	protected override void OnCompleteWork(WorkerBase worker)
	{
		int num = (int)Db.Get().Attributes.Machinery.Lookup(worker).GetTotalValue();
		int repair_amount = 10 + Math.Max(0, num * 10);
		this.Repair(repair_amount);
	}

	// Token: 0x06002C5E RID: 11358 RVA: 0x00102420 File Offset: 0x00100620
	private void OnDoBuildingDamage(object data)
	{
		if (this.invincible)
		{
			return;
		}
		this.damageSourceInfo = ((Boxed<BuildingHP.DamageSourceInfo>)data).value;
		this.DoDamage(this.damageSourceInfo.damage);
		this.DoDamagePopFX(this.damageSourceInfo);
		this.DoTakeDamageFX(this.damageSourceInfo);
	}

	// Token: 0x06002C5F RID: 11359 RVA: 0x00102470 File Offset: 0x00100670
	private void DoTakeDamageFX(BuildingHP.DamageSourceInfo info)
	{
		if (info.takeDamageEffect != SpawnFXHashes.None)
		{
			BuildingDef def = base.GetComponent<BuildingComplete>().Def;
			int cell = Grid.OffsetCell(Grid.PosToCell(this), 0, def.HeightInCells - 1);
			Game.Instance.SpawnFX(info.takeDamageEffect, cell, 0f);
		}
	}

	// Token: 0x06002C60 RID: 11360 RVA: 0x001024BC File Offset: 0x001006BC
	private void DoDamagePopFX(BuildingHP.DamageSourceInfo info)
	{
		if (info.popString != null && Time.time > this.lastPopTime + this.minDamagePopInterval)
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, info.popString, base.gameObject.transform, 1.5f, false);
			this.lastPopTime = Time.time;
		}
	}

	// Token: 0x17000225 RID: 549
	// (get) Token: 0x06002C61 RID: 11361 RVA: 0x0010251C File Offset: 0x0010071C
	public bool IsBroken
	{
		get
		{
			return this.hitpoints == 0;
		}
	}

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x06002C62 RID: 11362 RVA: 0x00102527 File Offset: 0x00100727
	public bool NeedsRepairs
	{
		get
		{
			return this.HitPoints < this.building.Def.HitPoints;
		}
	}

	// Token: 0x04001A4A RID: 6730
	[Serialize]
	[SerializeField]
	private int hitpoints;

	// Token: 0x04001A4B RID: 6731
	[Serialize]
	private BuildingHP.DamageSourceInfo damageSourceInfo;

	// Token: 0x04001A4C RID: 6732
	private static readonly EventSystem.IntraObjectHandler<BuildingHP> OnDoBuildingDamageDelegate = new EventSystem.IntraObjectHandler<BuildingHP>(delegate(BuildingHP component, object data)
	{
		component.OnDoBuildingDamage(data);
	});

	// Token: 0x04001A4D RID: 6733
	private static readonly EventSystem.IntraObjectHandler<BuildingHP> DestroyOnDamagedDelegate = new EventSystem.IntraObjectHandler<BuildingHP>(delegate(BuildingHP component, object data)
	{
		component.DestroyOnDamaged(data);
	});

	// Token: 0x04001A4E RID: 6734
	public static List<Meter> kbacQueryList = new List<Meter>();

	// Token: 0x04001A4F RID: 6735
	public bool destroyOnDamaged;

	// Token: 0x04001A50 RID: 6736
	public bool invincible;

	// Token: 0x04001A51 RID: 6737
	[MyCmpGet]
	private Building building;

	// Token: 0x04001A52 RID: 6738
	private BuildingHP.SMInstance smi;

	// Token: 0x04001A53 RID: 6739
	private float minDamagePopInterval = 4f;

	// Token: 0x04001A54 RID: 6740
	private float lastPopTime;

	// Token: 0x020015BE RID: 5566
	public struct DamageSourceInfo
	{
		// Token: 0x06009458 RID: 37976 RVA: 0x003786AF File Offset: 0x003768AF
		public override string ToString()
		{
			return this.source;
		}

		// Token: 0x04007285 RID: 29317
		public int damage;

		// Token: 0x04007286 RID: 29318
		public string source;

		// Token: 0x04007287 RID: 29319
		public string popString;

		// Token: 0x04007288 RID: 29320
		public SpawnFXHashes takeDamageEffect;

		// Token: 0x04007289 RID: 29321
		public string fullDamageEffectName;

		// Token: 0x0400728A RID: 29322
		public string statusItemID;
	}

	// Token: 0x020015BF RID: 5567
	public class SMInstance : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.GameInstance
	{
		// Token: 0x06009459 RID: 37977 RVA: 0x003786B7 File Offset: 0x003768B7
		public SMInstance(BuildingHP master) : base(master)
		{
		}

		// Token: 0x0600945A RID: 37978 RVA: 0x003786C0 File Offset: 0x003768C0
		public Notification CreateBrokenMachineNotification()
		{
			return new Notification(MISC.NOTIFICATIONS.BROKENMACHINE.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.BROKENMACHINE.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + base.master.damageSourceInfo.source, false, 0f, null, null, null, true, false, false);
		}

		// Token: 0x0600945B RID: 37979 RVA: 0x00378724 File Offset: 0x00376924
		public void ShowProgressBar(bool show)
		{
			if (show && Grid.IsValidCell(Grid.PosToCell(base.gameObject)) && Grid.IsVisible(Grid.PosToCell(base.gameObject)))
			{
				this.CreateProgressBar();
				return;
			}
			if (this.progressBar != null)
			{
				this.progressBar.gameObject.DeleteObject();
				this.progressBar = null;
			}
		}

		// Token: 0x0600945C RID: 37980 RVA: 0x00378784 File Offset: 0x00376984
		public void UpdateMeter()
		{
			if (this.progressBar == null)
			{
				this.ShowProgressBar(true);
			}
			if (this.progressBar)
			{
				this.progressBar.Update();
			}
		}

		// Token: 0x0600945D RID: 37981 RVA: 0x003787B3 File Offset: 0x003769B3
		private float HealthPercent()
		{
			return (float)base.smi.master.HitPoints / (float)base.smi.master.building.Def.HitPoints;
		}

		// Token: 0x0600945E RID: 37982 RVA: 0x003787E4 File Offset: 0x003769E4
		private void CreateProgressBar()
		{
			if (this.progressBar != null)
			{
				return;
			}
			this.progressBar = Util.KInstantiateUI<ProgressBar>(ProgressBarsConfig.Instance.progressBarPrefab, null, false);
			this.progressBar.transform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.transform);
			this.progressBar.name = base.smi.master.name + "." + base.smi.master.GetType().Name + " ProgressBar";
			this.progressBar.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("ProgressBar");
			this.progressBar.SetUpdateFunc(new Func<float>(this.HealthPercent));
			this.progressBar.barColor = ProgressBarsConfig.Instance.GetBarColor("HealthBar");
			CanvasGroup component = this.progressBar.GetComponent<CanvasGroup>();
			component.interactable = false;
			component.blocksRaycasts = false;
			this.progressBar.Update();
			float d = 0.15f;
			Vector3 vector = base.gameObject.transform.GetPosition() + Vector3.down * d;
			vector.z += 0.05f;
			Rotatable component2 = base.GetComponent<Rotatable>();
			if (component2 == null || component2.GetOrientation() == Orientation.Neutral || base.smi.master.building.Def.WidthInCells < 2 || base.smi.master.building.Def.HeightInCells < 2)
			{
				vector -= Vector3.right * 0.5f * (float)(base.smi.master.building.Def.WidthInCells % 2);
			}
			else
			{
				vector += Vector3.left * (1f + 0.5f * (float)(base.smi.master.building.Def.WidthInCells % 2));
			}
			this.progressBar.transform.SetPosition(vector);
			this.progressBar.SetVisibility(true);
		}

		// Token: 0x0600945F RID: 37983 RVA: 0x00378A14 File Offset: 0x00376C14
		private static string ToolTipResolver(List<Notification> notificationList, object data)
		{
			string text = "";
			for (int i = 0; i < notificationList.Count; i++)
			{
				Notification notification = notificationList[i];
				text += string.Format(BUILDINGS.DAMAGESOURCES.NOTIFICATION_TOOLTIP, notification.NotifierName, (string)notification.tooltipData);
				if (i < notificationList.Count - 1)
				{
					text += "\n";
				}
			}
			return text;
		}

		// Token: 0x06009460 RID: 37984 RVA: 0x00378A80 File Offset: 0x00376C80
		public void ShowDamagedEffect()
		{
			if (base.master.damageSourceInfo.takeDamageEffect != SpawnFXHashes.None)
			{
				BuildingDef def = base.master.GetComponent<BuildingComplete>().Def;
				int cell = Grid.OffsetCell(Grid.PosToCell(base.master), 0, def.HeightInCells - 1);
				Game.Instance.SpawnFX(base.master.damageSourceInfo.takeDamageEffect, cell, 0f);
			}
		}

		// Token: 0x06009461 RID: 37985 RVA: 0x00378AEC File Offset: 0x00376CEC
		public FXAnim.Instance InstantiateDamageFX()
		{
			if (base.master.damageSourceInfo.fullDamageEffectName == null)
			{
				return null;
			}
			BuildingDef def = base.master.GetComponent<BuildingComplete>().Def;
			Vector3 zero = Vector3.zero;
			if (def.HeightInCells > 1)
			{
				zero = new Vector3(0f, (float)(def.HeightInCells - 1), 0f);
			}
			else
			{
				zero = new Vector3(0f, 0.5f, 0f);
			}
			return new FXAnim.Instance(base.smi.master, base.master.damageSourceInfo.fullDamageEffectName, "idle", KAnim.PlayMode.Loop, zero, Color.white);
		}

		// Token: 0x06009462 RID: 37986 RVA: 0x00378B90 File Offset: 0x00376D90
		public void SetCrackOverlayValue(float value)
		{
			KBatchedAnimController component = base.master.GetComponent<KBatchedAnimController>();
			if (component == null)
			{
				return;
			}
			component.SetBlendValue(value);
			BuildingHP.kbacQueryList.Clear();
			base.master.GetComponentsInChildren<Meter>(BuildingHP.kbacQueryList);
			for (int i = 0; i < BuildingHP.kbacQueryList.Count; i++)
			{
				BuildingHP.kbacQueryList[i].GetComponent<KBatchedAnimController>().SetBlendValue(value);
			}
		}

		// Token: 0x0400728B RID: 29323
		private ProgressBar progressBar;
	}

	// Token: 0x020015C0 RID: 5568
	public class States : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP>
	{
		// Token: 0x06009463 RID: 37987 RVA: 0x00378C00 File Offset: 0x00376E00
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.healthy;
			this.healthy.DefaultState(this.healthy.imperfect).EventTransition(GameHashes.BuildingReceivedDamage, this.damaged, (BuildingHP.SMInstance smi) => smi.master.HitPoints <= 0);
			this.healthy.imperfect.Enter(delegate(BuildingHP.SMInstance smi)
			{
				smi.ShowProgressBar(true);
			}).DefaultState(this.healthy.imperfect.playEffect).EventTransition(GameHashes.BuildingPartiallyRepaired, this.healthy.perfect, (BuildingHP.SMInstance smi) => smi.master.HitPoints == smi.master.building.Def.HitPoints).EventHandler(GameHashes.BuildingPartiallyRepaired, delegate(BuildingHP.SMInstance smi)
			{
				smi.UpdateMeter();
			}).ToggleStatusItem(delegate(BuildingHP.SMInstance smi)
			{
				if (smi.master.damageSourceInfo.statusItemID == null)
				{
					return null;
				}
				return Db.Get().BuildingStatusItems.Get(smi.master.damageSourceInfo.statusItemID);
			}, null).Exit(delegate(BuildingHP.SMInstance smi)
			{
				smi.ShowProgressBar(false);
			});
			this.healthy.imperfect.playEffect.Transition(this.healthy.imperfect.waiting, (BuildingHP.SMInstance smi) => true, UpdateRate.SIM_200ms);
			this.healthy.imperfect.waiting.ScheduleGoTo((BuildingHP.SMInstance smi) => UnityEngine.Random.Range(15f, 30f), this.healthy.imperfect.playEffect);
			this.healthy.perfect.EventTransition(GameHashes.BuildingReceivedDamage, this.healthy.imperfect, (BuildingHP.SMInstance smi) => smi.master.HitPoints < smi.master.building.Def.HitPoints);
			this.damaged.Enter(delegate(BuildingHP.SMInstance smi)
			{
				Operational component = smi.GetComponent<Operational>();
				if (component != null)
				{
					component.SetFlag(BuildingHP.States.healthyFlag, false);
				}
				smi.ShowProgressBar(true);
				smi.master.Trigger(774203113, smi.master);
				smi.SetCrackOverlayValue(1f);
			}).ToggleNotification((BuildingHP.SMInstance smi) => smi.CreateBrokenMachineNotification()).ToggleStatusItem(Db.Get().BuildingStatusItems.Broken, null).ToggleFX((BuildingHP.SMInstance smi) => smi.InstantiateDamageFX()).EventTransition(GameHashes.BuildingPartiallyRepaired, this.healthy.perfect, (BuildingHP.SMInstance smi) => smi.master.HitPoints == smi.master.building.Def.HitPoints).EventHandler(GameHashes.BuildingPartiallyRepaired, delegate(BuildingHP.SMInstance smi)
			{
				smi.UpdateMeter();
			}).Exit(delegate(BuildingHP.SMInstance smi)
			{
				Operational component = smi.GetComponent<Operational>();
				if (component != null)
				{
					component.SetFlag(BuildingHP.States.healthyFlag, true);
				}
				smi.ShowProgressBar(false);
				smi.SetCrackOverlayValue(0f);
			});
		}

		// Token: 0x06009464 RID: 37988 RVA: 0x00378F24 File Offset: 0x00377124
		private Chore CreateRepairChore(BuildingHP.SMInstance smi)
		{
			return new WorkChore<BuildingHP>(Db.Get().ChoreTypes.Repair, smi.master, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x0400728C RID: 29324
		private static readonly Operational.Flag healthyFlag = new Operational.Flag("healthy", Operational.Flag.Type.Functional);

		// Token: 0x0400728D RID: 29325
		public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State damaged;

		// Token: 0x0400728E RID: 29326
		public BuildingHP.States.Healthy healthy;

		// Token: 0x020028C3 RID: 10435
		public class Healthy : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State
		{
			// Token: 0x0400B381 RID: 45953
			public BuildingHP.States.ImperfectStates imperfect;

			// Token: 0x0400B382 RID: 45954
			public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State perfect;
		}

		// Token: 0x020028C4 RID: 10436
		public class ImperfectStates : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State
		{
			// Token: 0x0400B383 RID: 45955
			public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State playEffect;

			// Token: 0x0400B384 RID: 45956
			public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State waiting;
		}
	}
}
