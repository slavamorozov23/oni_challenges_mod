using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x02000EB6 RID: 3766
public class UIDupeRandomizer : MonoBehaviour
{
	// Token: 0x060078B1 RID: 30897 RVA: 0x002E6614 File Offset: 0x002E4814
	protected virtual void Start()
	{
		this.slots = Db.Get().AccessorySlots;
		for (int i = 0; i < this.anims.Length; i++)
		{
			this.anims[i].curBody = null;
			this.GetNewBody(i);
		}
	}

	// Token: 0x060078B2 RID: 30898 RVA: 0x002E6660 File Offset: 0x002E4860
	protected void GetNewBody(int minion_idx)
	{
		Personality random = Db.Get().Personalities.GetRandom(true, false);
		foreach (KBatchedAnimController dupe in this.anims[minion_idx].minions)
		{
			this.Apply(dupe, random);
		}
	}

	// Token: 0x060078B3 RID: 30899 RVA: 0x002E66D4 File Offset: 0x002E48D4
	private void Apply(KBatchedAnimController dupe, Personality personality)
	{
		KCompBuilder.BodyData bodyData = MinionStartingStats.CreateBodyData(personality);
		SymbolOverrideController component = dupe.GetComponent<SymbolOverrideController>();
		component.RemoveAllSymbolOverrides(0);
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Hair.Lookup(bodyData.hair));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.HatHair.Lookup("hat_" + HashCache.Get().Get(bodyData.hair)));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Eyes.Lookup(bodyData.eyes));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.HeadShape.Lookup(bodyData.headShape));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Mouth.Lookup(bodyData.mouth));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Body.Lookup(bodyData.body));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Arm.Lookup(bodyData.arms));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.ArmLower.Lookup(bodyData.armslower));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Belt.Lookup(bodyData.belt));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Hand.Lookup(bodyData.hand));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Neck.Lookup(bodyData.neck));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Cuff.Lookup(bodyData.cuff));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Pelvis.Lookup(bodyData.pelvis));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Leg.Lookup(bodyData.legs));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Foot.Lookup(bodyData.foot));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.ArmLowerSkin.Lookup(bodyData.armLowerSkin));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.ArmUpperSkin.Lookup(bodyData.armUpperSkin));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.LegSkin.Lookup(bodyData.legSkin));
		if (this.applySuit && UnityEngine.Random.value < 0.15f)
		{
			component.AddBuildOverride(Assets.GetAnim("body_oxygen_kanim").GetData(), 6);
			dupe.SetSymbolVisiblity("snapto_neck", true);
			dupe.SetSymbolVisiblity("belt", false);
		}
		else
		{
			dupe.SetSymbolVisiblity("snapto_neck", false);
		}
		if (this.applyHat && UnityEngine.Random.value < 0.5f)
		{
			List<string> list = new List<string>();
			foreach (Skill skill in Db.Get().Skills.resources)
			{
				if (skill.requiredDuplicantModel.IsNullOrWhiteSpace() || skill.requiredDuplicantModel == personality.model)
				{
					list.Add(skill.hat);
				}
			}
			string id = list[UnityEngine.Random.Range(0, list.Count)];
			UIDupeRandomizer.AddAccessory(dupe, this.slots.Hat.Lookup(id));
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
		}
		else
		{
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, false);
		}
		dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Skirt.targetSymbolId, false);
		dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Necklace.targetSymbolId, false);
	}

	// Token: 0x060078B4 RID: 30900 RVA: 0x002E6B00 File Offset: 0x002E4D00
	public static KAnimHashedString AddAccessory(KBatchedAnimController minion, Accessory accessory)
	{
		if (accessory != null)
		{
			SymbolOverrideController component = minion.GetComponent<SymbolOverrideController>();
			DebugUtil.Assert(component != null, minion.name + " is missing symbol override controller");
			component.TryRemoveSymbolOverride(accessory.slot.targetSymbolId, 0);
			component.AddSymbolOverride(accessory.slot.targetSymbolId, accessory.symbol, 0);
			minion.SetSymbolVisiblity(accessory.slot.targetSymbolId, true);
			return accessory.slot.targetSymbolId;
		}
		return HashedString.Invalid;
	}

	// Token: 0x060078B5 RID: 30901 RVA: 0x002E6B90 File Offset: 0x002E4D90
	public KAnimHashedString AddRandomAccessory(KBatchedAnimController minion, List<Accessory> choices)
	{
		Accessory accessory = choices[UnityEngine.Random.Range(1, choices.Count)];
		return UIDupeRandomizer.AddAccessory(minion, accessory);
	}

	// Token: 0x060078B6 RID: 30902 RVA: 0x002E6BB8 File Offset: 0x002E4DB8
	public void Randomize()
	{
		if (this.slots == null)
		{
			return;
		}
		for (int i = 0; i < this.anims.Length; i++)
		{
			this.GetNewBody(i);
		}
	}

	// Token: 0x060078B7 RID: 30903 RVA: 0x002E6BE8 File Offset: 0x002E4DE8
	protected virtual void Update()
	{
	}

	// Token: 0x04005425 RID: 21541
	[Tooltip("Enable this to allow for a chance for skill hats to appear")]
	public bool applyHat = true;

	// Token: 0x04005426 RID: 21542
	[Tooltip("Enable this to allow for a chance for suit helmets to appear (ie. atmosuit and leadsuit)")]
	public bool applySuit = true;

	// Token: 0x04005427 RID: 21543
	public UIDupeRandomizer.AnimChoice[] anims;

	// Token: 0x04005428 RID: 21544
	private AccessorySlots slots;

	// Token: 0x0200211E RID: 8478
	[Serializable]
	public struct AnimChoice
	{
		// Token: 0x04009845 RID: 38981
		public string anim_name;

		// Token: 0x04009846 RID: 38982
		public List<KBatchedAnimController> minions;

		// Token: 0x04009847 RID: 38983
		public float minSecondsBetweenAction;

		// Token: 0x04009848 RID: 38984
		public float maxSecondsBetweenAction;

		// Token: 0x04009849 RID: 38985
		public float lastWaitTime;

		// Token: 0x0400984A RID: 38986
		public KAnimFile curBody;
	}
}
