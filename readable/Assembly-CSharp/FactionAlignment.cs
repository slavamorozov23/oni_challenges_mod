using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000863 RID: 2147
[AddComponentMenu("KMonoBehaviour/scripts/FactionAlignment")]
public class FactionAlignment : KMonoBehaviour
{
	// Token: 0x170003FA RID: 1018
	// (get) Token: 0x06003AE3 RID: 15075 RVA: 0x001489D9 File Offset: 0x00146BD9
	// (set) Token: 0x06003AE4 RID: 15076 RVA: 0x001489E1 File Offset: 0x00146BE1
	[MyCmpAdd]
	public Health health { get; private set; }

	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x06003AE5 RID: 15077 RVA: 0x001489EA File Offset: 0x00146BEA
	// (set) Token: 0x06003AE6 RID: 15078 RVA: 0x001489F2 File Offset: 0x00146BF2
	public AttackableBase attackable { get; private set; }

	// Token: 0x06003AE7 RID: 15079 RVA: 0x001489FC File Offset: 0x00146BFC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.health = base.GetComponent<Health>();
		this.attackable = base.GetComponent<AttackableBase>();
		Components.FactionAlignments.Add(this);
		base.Subscribe<FactionAlignment>(493375141, FactionAlignment.OnRefreshUserMenuDelegate);
		base.Subscribe<FactionAlignment>(2127324410, FactionAlignment.SetPlayerTargetedFalseDelegate);
		base.Subscribe<FactionAlignment>(1502190696, FactionAlignment.OnQueueDestroyObjectDelegate);
		if (this.alignmentActive)
		{
			FactionManager.Instance.GetFaction(this.Alignment).Members.Add(this);
		}
		GameUtil.SubscribeToTags<FactionAlignment>(this, FactionAlignment.OnDeadTagAddedDelegate, true);
		this.SetPlayerTargeted(this.targeted);
		this.UpdateStatusItem();
	}

	// Token: 0x06003AE8 RID: 15080 RVA: 0x00148AA7 File Offset: 0x00146CA7
	protected override void OnPrefabInit()
	{
	}

	// Token: 0x06003AE9 RID: 15081 RVA: 0x00148AA9 File Offset: 0x00146CA9
	private void OnDeath(object data)
	{
		this.SetAlignmentActive(false);
	}

	// Token: 0x06003AEA RID: 15082 RVA: 0x00148AB4 File Offset: 0x00146CB4
	public void SetAlignmentActive(bool active)
	{
		this.SetPlayerTargetable(active);
		this.alignmentActive = active;
		if (active)
		{
			FactionManager.Instance.GetFaction(this.Alignment).Members.Add(this);
			return;
		}
		FactionManager.Instance.GetFaction(this.Alignment).Members.Remove(this);
	}

	// Token: 0x06003AEB RID: 15083 RVA: 0x00148B0B File Offset: 0x00146D0B
	public bool IsAlignmentActive()
	{
		return FactionManager.Instance.GetFaction(this.Alignment).Members.Contains(this);
	}

	// Token: 0x06003AEC RID: 15084 RVA: 0x00148B28 File Offset: 0x00146D28
	public bool IsPlayerTargeted()
	{
		return this.targeted;
	}

	// Token: 0x06003AED RID: 15085 RVA: 0x00148B30 File Offset: 0x00146D30
	public void SetPlayerTargetable(bool state)
	{
		this.targetable = (state && this.canBePlayerTargeted);
		if (!state)
		{
			this.SetPlayerTargeted(false);
		}
	}

	// Token: 0x06003AEE RID: 15086 RVA: 0x00148B50 File Offset: 0x00146D50
	public void SetPlayerTargeted(bool state)
	{
		this.targeted = (this.canBePlayerTargeted && state && this.targetable);
		if (state)
		{
			if (!Components.PlayerTargeted.Items.Contains(this))
			{
				Components.PlayerTargeted.Add(this);
			}
			this.SetPrioritizable(true);
		}
		else
		{
			Components.PlayerTargeted.Remove(this);
			this.SetPrioritizable(false);
		}
		this.UpdateStatusItem();
	}

	// Token: 0x06003AEF RID: 15087 RVA: 0x00148BB8 File Offset: 0x00146DB8
	private void UpdateStatusItem()
	{
		if (this.targeted)
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OrderAttack, null);
			return;
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.OrderAttack, false);
	}

	// Token: 0x06003AF0 RID: 15088 RVA: 0x00148C08 File Offset: 0x00146E08
	private void SetPrioritizable(bool enable)
	{
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (component == null || !this.updatePrioritizable)
		{
			return;
		}
		if (enable && !this.hasBeenRegisterInPriority)
		{
			Prioritizable.AddRef(base.gameObject);
			this.hasBeenRegisterInPriority = true;
			return;
		}
		if (!enable && component.IsPrioritizable() && this.hasBeenRegisterInPriority)
		{
			Prioritizable.RemoveRef(base.gameObject);
			this.hasBeenRegisterInPriority = false;
		}
	}

	// Token: 0x06003AF1 RID: 15089 RVA: 0x00148C71 File Offset: 0x00146E71
	public void SwitchAlignment(FactionManager.FactionID newAlignment)
	{
		this.SetAlignmentActive(false);
		this.Alignment = newAlignment;
		this.SetAlignmentActive(true);
		base.BoxingTrigger<FactionManager.FactionID>(-971105736, newAlignment);
	}

	// Token: 0x06003AF2 RID: 15090 RVA: 0x00148C94 File Offset: 0x00146E94
	private void OnQueueDestroyObject()
	{
		FactionManager.Instance.GetFaction(this.Alignment).Members.Remove(this);
		Components.FactionAlignments.Remove(this);
	}

	// Token: 0x06003AF3 RID: 15091 RVA: 0x00148CC0 File Offset: 0x00146EC0
	private void OnRefreshUserMenu(object data)
	{
		if (this.Alignment == FactionManager.FactionID.Duplicant)
		{
			return;
		}
		if (!this.canBePlayerTargeted)
		{
			return;
		}
		if (!this.IsAlignmentActive())
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (!this.targeted) ? new KIconButtonMenu.ButtonInfo("action_attack", UI.USERMENUACTIONS.ATTACK.NAME, delegate()
		{
			this.SetPlayerTargeted(true);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ATTACK.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_attack", UI.USERMENUACTIONS.CANCELATTACK.NAME, delegate()
		{
			this.SetPlayerTargeted(false);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELATTACK.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x040023D6 RID: 9174
	[MyCmpReq]
	public KPrefabID kprefabID;

	// Token: 0x040023D7 RID: 9175
	[SerializeField]
	public bool canBePlayerTargeted = true;

	// Token: 0x040023D8 RID: 9176
	[SerializeField]
	public bool updatePrioritizable = true;

	// Token: 0x040023D9 RID: 9177
	[Serialize]
	private bool alignmentActive = true;

	// Token: 0x040023DA RID: 9178
	public FactionManager.FactionID Alignment;

	// Token: 0x040023DB RID: 9179
	[Serialize]
	private bool targeted;

	// Token: 0x040023DC RID: 9180
	[Serialize]
	private bool targetable = true;

	// Token: 0x040023DD RID: 9181
	private bool hasBeenRegisterInPriority;

	// Token: 0x040023DE RID: 9182
	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<FactionAlignment>(GameTags.Dead, delegate(FactionAlignment component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x040023DF RID: 9183
	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>(delegate(FactionAlignment component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x040023E0 RID: 9184
	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> SetPlayerTargetedFalseDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>(delegate(FactionAlignment component, object data)
	{
		component.SetPlayerTargeted(false);
	});

	// Token: 0x040023E1 RID: 9185
	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnQueueDestroyObjectDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>(delegate(FactionAlignment component, object data)
	{
		component.OnQueueDestroyObject();
	});
}
