using System;
using KSerialization;
using UnityEngine;

// Token: 0x020006E5 RID: 1765
[AddComponentMenu("KMonoBehaviour/scripts/Baggable")]
public class Baggable : KMonoBehaviour
{
	// Token: 0x06002B8A RID: 11146 RVA: 0x000FE0B4 File Offset: 0x000FC2B4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.minionAnimOverride = Assets.GetAnim("anim_restrain_creature_kanim");
		Pickupable pickupable = base.gameObject.AddOrGet<Pickupable>();
		pickupable.workAnims = new HashedString[]
		{
			new HashedString("capture"),
			new HashedString("pickup")
		};
		pickupable.workAnimPlayMode = KAnim.PlayMode.Once;
		pickupable.workingPstComplete = null;
		pickupable.workingPstFailed = null;
		pickupable.overrideAnims = new KAnimFile[]
		{
			this.minionAnimOverride
		};
		pickupable.trackOnPickup = false;
		pickupable.useGunforPickup = this.useGunForPickup;
		pickupable.synchronizeAnims = false;
		pickupable.SetWorkTime(3f);
		if (this.mustStandOntopOfTrapForPickup)
		{
			pickupable.SetOffsets(new CellOffset[]
			{
				default(CellOffset),
				new CellOffset(0, -1)
			});
		}
		base.Subscribe<Baggable>(856640610, Baggable.OnStoreDelegate);
		if (base.transform.parent != null)
		{
			if (base.transform.parent.GetComponent<Trap>() != null || base.transform.parent.GetSMI<ReusableTrap.Instance>() != null)
			{
				base.GetComponent<KBatchedAnimController>().enabled = true;
			}
			if (base.transform.parent.GetComponent<EggIncubator>() != null)
			{
				this.wrangled = true;
			}
		}
		if (this.wrangled)
		{
			this.SetWrangled();
		}
	}

	// Token: 0x06002B8B RID: 11147 RVA: 0x000FE210 File Offset: 0x000FC410
	private void OnStore(object data)
	{
		Storage storage = data as Storage;
		if (storage != null || (data != null && ((Boxed<bool>)data).value))
		{
			base.gameObject.AddTag(GameTags.Creatures.Bagged);
			if (storage && storage.HasTag(GameTags.BaseMinion))
			{
				this.SetVisible(false);
				return;
			}
		}
		else
		{
			if (!this.keepWrangledNextTimeRemovedFromStorage)
			{
				this.Free();
			}
			this.keepWrangledNextTimeRemovedFromStorage = false;
		}
	}

	// Token: 0x06002B8C RID: 11148 RVA: 0x000FE288 File Offset: 0x000FC488
	private void SetVisible(bool visible)
	{
		KAnimControllerBase component = base.gameObject.GetComponent<KAnimControllerBase>();
		if (component != null && component.enabled != visible)
		{
			component.enabled = visible;
		}
		KSelectable component2 = base.gameObject.GetComponent<KSelectable>();
		if (component2 != null && component2.enabled != visible)
		{
			component2.enabled = visible;
		}
	}

	// Token: 0x06002B8D RID: 11149 RVA: 0x000FE2E0 File Offset: 0x000FC4E0
	public static string GetBaggedAnimName(GameObject baggableObject)
	{
		string result = "trussed";
		Pickupable pickupable = baggableObject.AddOrGet<Pickupable>();
		if (pickupable != null && pickupable.storage != null)
		{
			IBaggedStateAnimationInstructions component = pickupable.storage.GetComponent<IBaggedStateAnimationInstructions>();
			if (component != null)
			{
				string baggedAnimationName = component.GetBaggedAnimationName();
				if (baggedAnimationName != null)
				{
					result = baggedAnimationName;
				}
			}
		}
		return result;
	}

	// Token: 0x06002B8E RID: 11150 RVA: 0x000FE330 File Offset: 0x000FC530
	public void SetWrangled()
	{
		this.wrangled = true;
		Navigator component = base.GetComponent<Navigator>();
		if (component && component.IsValidNavType(NavType.Floor))
		{
			component.SetCurrentNavType(NavType.Floor);
		}
		base.gameObject.AddTag(GameTags.Creatures.Bagged);
		base.GetComponent<KAnimControllerBase>().Play(Baggable.GetBaggedAnimName(base.gameObject), KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06002B8F RID: 11151 RVA: 0x000FE399 File Offset: 0x000FC599
	public void Free()
	{
		base.gameObject.RemoveTag(GameTags.Creatures.Bagged);
		this.wrangled = false;
		this.SetVisible(true);
	}

	// Token: 0x040019EE RID: 6638
	[SerializeField]
	private KAnimFile minionAnimOverride;

	// Token: 0x040019EF RID: 6639
	public bool mustStandOntopOfTrapForPickup;

	// Token: 0x040019F0 RID: 6640
	[Serialize]
	public bool wrangled;

	// Token: 0x040019F1 RID: 6641
	[Serialize]
	public bool keepWrangledNextTimeRemovedFromStorage;

	// Token: 0x040019F2 RID: 6642
	public bool useGunForPickup;

	// Token: 0x040019F3 RID: 6643
	private static readonly EventSystem.IntraObjectHandler<Baggable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Baggable>(delegate(Baggable component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x040019F4 RID: 6644
	public const string DEFAULT_BAGGED_ANIM_NAME = "trussed";
}
