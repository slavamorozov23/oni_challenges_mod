using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000988 RID: 2440
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Health")]
public class Health : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170004FD RID: 1277
	// (get) Token: 0x0600460F RID: 17935 RVA: 0x00194C77 File Offset: 0x00192E77
	// (set) Token: 0x06004610 RID: 17936 RVA: 0x00194C7F File Offset: 0x00192E7F
	[Serialize]
	public Health.HealthState State { get; private set; }

	// Token: 0x170004FE RID: 1278
	// (get) Token: 0x06004611 RID: 17937 RVA: 0x00194C88 File Offset: 0x00192E88
	// (set) Token: 0x06004612 RID: 17938 RVA: 0x00194C90 File Offset: 0x00192E90
	[Serialize]
	public Tag CauseOfIncapacitation { get; private set; }

	// Token: 0x170004FF RID: 1279
	// (get) Token: 0x06004613 RID: 17939 RVA: 0x00194C99 File Offset: 0x00192E99
	public AmountInstance GetAmountInstance
	{
		get
		{
			return this.amountInstance;
		}
	}

	// Token: 0x17000500 RID: 1280
	// (get) Token: 0x06004614 RID: 17940 RVA: 0x00194CA1 File Offset: 0x00192EA1
	// (set) Token: 0x06004615 RID: 17941 RVA: 0x00194CAE File Offset: 0x00192EAE
	public float hitPoints
	{
		get
		{
			return this.amountInstance.value;
		}
		set
		{
			this.amountInstance.value = value;
		}
	}

	// Token: 0x17000501 RID: 1281
	// (get) Token: 0x06004616 RID: 17942 RVA: 0x00194CBC File Offset: 0x00192EBC
	public float maxHitPoints
	{
		get
		{
			return this.amountInstance.GetMax();
		}
	}

	// Token: 0x06004617 RID: 17943 RVA: 0x00194CC9 File Offset: 0x00192EC9
	public float percent()
	{
		return this.hitPoints / this.maxHitPoints;
	}

	// Token: 0x06004618 RID: 17944 RVA: 0x00194CD8 File Offset: 0x00192ED8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.Health.Add(this);
		this.amountInstance = Db.Get().Amounts.HitPoints.Lookup(base.gameObject);
		this.amountInstance.value = this.amountInstance.GetMax();
		AmountInstance amountInstance = this.amountInstance;
		amountInstance.OnDelta = (Action<float>)Delegate.Combine(amountInstance.OnDelta, new Action<float>(this.OnHealthChanged));
	}

	// Token: 0x06004619 RID: 17945 RVA: 0x00194D54 File Offset: 0x00192F54
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.State == Health.HealthState.Incapacitated || this.hitPoints == 0f)
		{
			if (this.canBeIncapacitated)
			{
				this.Incapacitate(GameTags.HitPointsDepleted);
			}
			else
			{
				this.Kill();
			}
		}
		if (this.State != Health.HealthState.Incapacitated && this.State != Health.HealthState.Dead)
		{
			this.UpdateStatus();
		}
		this.effects = base.GetComponent<Effects>();
		this.UpdateHealthBar();
		this.UpdateWoundEffects();
	}

	// Token: 0x0600461A RID: 17946 RVA: 0x00194DC8 File Offset: 0x00192FC8
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Health.Remove(this);
	}

	// Token: 0x0600461B RID: 17947 RVA: 0x00194DDC File Offset: 0x00192FDC
	public void UpdateHealthBar()
	{
		if (NameDisplayScreen.Instance == null)
		{
			return;
		}
		bool flag = this.State == Health.HealthState.Dead || this.State == Health.HealthState.Incapacitated || this.hitPoints >= this.maxHitPoints || base.gameObject.HasTag("HideHealthBar");
		NameDisplayScreen.Instance.SetHealthDisplay(base.gameObject, new Func<float>(this.percent), !flag);
	}

	// Token: 0x0600461C RID: 17948 RVA: 0x00194E50 File Offset: 0x00193050
	private void OnRecover()
	{
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.HitPointsDepleted);
	}

	// Token: 0x0600461D RID: 17949 RVA: 0x00194E64 File Offset: 0x00193064
	public void OnHealthChanged(float delta)
	{
		base.BoxingTrigger<float>(-1664904872, delta);
		if (this.State != Health.HealthState.Invincible)
		{
			if (this.hitPoints == 0f && !this.IsDefeated())
			{
				if (this.canBeIncapacitated)
				{
					this.Incapacitate(GameTags.HitPointsDepleted);
				}
				else
				{
					this.Kill();
				}
			}
			else
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.HitPointsDepleted);
			}
		}
		this.UpdateStatus();
		this.UpdateWoundEffects();
		this.UpdateHealthBar();
	}

	// Token: 0x0600461E RID: 17950 RVA: 0x00194EDA File Offset: 0x001930DA
	[ContextMenu("DoDamage")]
	public void DoDamage()
	{
		this.Damage(1f);
	}

	// Token: 0x0600461F RID: 17951 RVA: 0x00194EE7 File Offset: 0x001930E7
	public void Damage(float amount)
	{
		if (this.State != Health.HealthState.Invincible)
		{
			this.hitPoints = Mathf.Max(0f, this.hitPoints - amount);
		}
		this.OnHealthChanged(-amount);
	}

	// Token: 0x06004620 RID: 17952 RVA: 0x00194F14 File Offset: 0x00193114
	private void UpdateWoundEffects()
	{
		if (!this.effects)
		{
			return;
		}
		if (this.isCritter != this.isCritterPrev)
		{
			if (this.isCritterPrev)
			{
				this.effects.Remove("LightWoundsCritter");
				this.effects.Remove("ModerateWoundsCritter");
				this.effects.Remove("SevereWoundsCritter");
			}
			else
			{
				this.effects.Remove("LightWounds");
				this.effects.Remove("ModerateWounds");
				this.effects.Remove("SevereWounds");
			}
			this.isCritterPrev = this.isCritter;
		}
		string effect_id;
		string effect_id2;
		string effect_id3;
		if (this.isCritter)
		{
			effect_id = "LightWoundsCritter";
			effect_id2 = "ModerateWoundsCritter";
			effect_id3 = "SevereWoundsCritter";
		}
		else
		{
			effect_id = "LightWounds";
			effect_id2 = "ModerateWounds";
			effect_id3 = "SevereWounds";
		}
		switch (this.State)
		{
		case Health.HealthState.Perfect:
		case Health.HealthState.Alright:
		case Health.HealthState.Incapacitated:
		case Health.HealthState.Dead:
			this.effects.Remove(effect_id);
			this.effects.Remove(effect_id2);
			this.effects.Remove(effect_id3);
			break;
		case Health.HealthState.Scuffed:
			if (!this.effects.HasEffect(effect_id))
			{
				this.effects.Add(effect_id, true);
			}
			this.effects.Remove(effect_id2);
			this.effects.Remove(effect_id3);
			return;
		case Health.HealthState.Injured:
			this.effects.Remove(effect_id);
			if (!this.effects.HasEffect(effect_id2))
			{
				this.effects.Add(effect_id2, true);
			}
			this.effects.Remove(effect_id3);
			return;
		case Health.HealthState.Critical:
			this.effects.Remove(effect_id);
			this.effects.Remove(effect_id2);
			if (!this.effects.HasEffect(effect_id3))
			{
				this.effects.Add(effect_id3, true);
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06004621 RID: 17953 RVA: 0x001950D0 File Offset: 0x001932D0
	private void UpdateStatus()
	{
		float num = this.hitPoints / this.maxHitPoints;
		Health.HealthState healthState;
		if (this.State == Health.HealthState.Invincible)
		{
			healthState = Health.HealthState.Invincible;
		}
		else if (num >= 1f)
		{
			healthState = Health.HealthState.Perfect;
		}
		else if (num >= 0.85f)
		{
			healthState = Health.HealthState.Alright;
		}
		else if (num >= 0.66f)
		{
			healthState = Health.HealthState.Scuffed;
		}
		else if ((double)num >= 0.33)
		{
			healthState = Health.HealthState.Injured;
		}
		else if (num > 0f)
		{
			healthState = Health.HealthState.Critical;
		}
		else if (num == 0f)
		{
			healthState = Health.HealthState.Incapacitated;
		}
		else
		{
			healthState = Health.HealthState.Dead;
		}
		if (this.State != healthState)
		{
			if (this.State == Health.HealthState.Incapacitated && healthState != Health.HealthState.Dead)
			{
				this.OnRecover();
			}
			if (healthState == Health.HealthState.Perfect)
			{
				base.Trigger(-1491582671, this);
			}
			this.State = healthState;
			KSelectable component = base.GetComponent<KSelectable>();
			if (this.State != Health.HealthState.Dead && this.State != Health.HealthState.Perfect && this.State != Health.HealthState.Alright && !this.isCritter)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.Hitpoints, Db.Get().CreatureStatusItems.HealthStatus, this.State);
				return;
			}
			component.SetStatusItem(Db.Get().StatusItemCategories.Hitpoints, null, null);
		}
	}

	// Token: 0x06004622 RID: 17954 RVA: 0x001951EE File Offset: 0x001933EE
	public bool IsIncapacitated()
	{
		return this.State == Health.HealthState.Incapacitated;
	}

	// Token: 0x06004623 RID: 17955 RVA: 0x001951F9 File Offset: 0x001933F9
	public bool IsDefeated()
	{
		return this.State == Health.HealthState.Incapacitated || this.State == Health.HealthState.Dead;
	}

	// Token: 0x06004624 RID: 17956 RVA: 0x0019520F File Offset: 0x0019340F
	public void Incapacitate(Tag cause)
	{
		this.CauseOfIncapacitation = cause;
		this.State = Health.HealthState.Incapacitated;
		this.Damage(this.hitPoints);
		base.gameObject.Trigger(-1506500077, null);
	}

	// Token: 0x06004625 RID: 17957 RVA: 0x0019523C File Offset: 0x0019343C
	private void Kill()
	{
		if (base.gameObject.GetSMI<DeathMonitor.Instance>() != null)
		{
			base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Slain);
		}
	}

	// Token: 0x04002F28 RID: 12072
	[Serialize]
	public bool canBeIncapacitated;

	// Token: 0x04002F2B RID: 12075
	public HealthBar healthBar;

	// Token: 0x04002F2C RID: 12076
	public bool isCritter;

	// Token: 0x04002F2D RID: 12077
	private bool isCritterPrev;

	// Token: 0x04002F2E RID: 12078
	private Effects effects;

	// Token: 0x04002F2F RID: 12079
	private AmountInstance amountInstance;

	// Token: 0x020019F1 RID: 6641
	public enum HealthState
	{
		// Token: 0x04007FA3 RID: 32675
		Perfect,
		// Token: 0x04007FA4 RID: 32676
		Alright,
		// Token: 0x04007FA5 RID: 32677
		Scuffed,
		// Token: 0x04007FA6 RID: 32678
		Injured,
		// Token: 0x04007FA7 RID: 32679
		Critical,
		// Token: 0x04007FA8 RID: 32680
		Incapacitated,
		// Token: 0x04007FA9 RID: 32681
		Dead,
		// Token: 0x04007FAA RID: 32682
		Invincible
	}
}
