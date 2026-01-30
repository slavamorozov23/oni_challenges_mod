using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using UnityEngine;

// Token: 0x02000592 RID: 1426
[AddComponentMenu("KMonoBehaviour/scripts/Accessorizer")]
public class Accessorizer : KMonoBehaviour
{
	// Token: 0x06001FDD RID: 8157 RVA: 0x000B7AF2 File Offset: 0x000B5CF2
	public List<ResourceRef<Accessory>> GetAccessories()
	{
		return this.accessories;
	}

	// Token: 0x06001FDE RID: 8158 RVA: 0x000B7AFA File Offset: 0x000B5CFA
	public void SetAccessories(List<ResourceRef<Accessory>> data)
	{
		this.accessories = data;
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06001FDF RID: 8159 RVA: 0x000B7B03 File Offset: 0x000B5D03
	// (set) Token: 0x06001FE0 RID: 8160 RVA: 0x000B7B0B File Offset: 0x000B5D0B
	public KCompBuilder.BodyData bodyData { get; set; }

	// Token: 0x06001FE1 RID: 8161 RVA: 0x000B7B14 File Offset: 0x000B5D14
	[OnDeserialized]
	private void OnDeserialized()
	{
		MinionIdentity component = base.GetComponent<MinionIdentity>();
		if (this.clothingItems.Count > 0 || (component != null && component.nameStringKey == "JORGE") || SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 30))
		{
			if (component != null)
			{
				this.bodyData = Accessorizer.UpdateAccessorySlots(component.nameStringKey, ref this.accessories);
			}
			this.accessories.RemoveAll((ResourceRef<Accessory> x) => x.Get() == null);
		}
		if (this.clothingItems.Count > 0)
		{
			base.GetComponent<WearableAccessorizer>().ApplyClothingItems(ClothingOutfitUtility.OutfitType.Clothing, from i in this.clothingItems
			select i.Get());
			this.clothingItems.Clear();
		}
		this.ApplyAccessories();
	}

	// Token: 0x06001FE2 RID: 8162 RVA: 0x000B7C10 File Offset: 0x000B5E10
	protected override void OnSpawn()
	{
		base.OnSpawn();
		MinionIdentity component = base.GetComponent<MinionIdentity>();
		if (component != null)
		{
			this.bodyData = MinionStartingStats.CreateBodyData(Db.Get().Personalities.Get(component.personalityResourceId));
		}
	}

	// Token: 0x06001FE3 RID: 8163 RVA: 0x000B7C54 File Offset: 0x000B5E54
	public void AddAccessory(Accessory accessory)
	{
		if (accessory != null)
		{
			if (this.animController == null)
			{
				this.animController = base.GetComponent<KAnimControllerBase>();
			}
			this.animController.GetComponent<SymbolOverrideController>().AddSymbolOverride(accessory.slot.targetSymbolId, accessory.symbol, accessory.slot.overrideLayer);
			if (!this.HasAccessory(accessory))
			{
				ResourceRef<Accessory> resourceRef = new ResourceRef<Accessory>(accessory);
				if (resourceRef != null)
				{
					this.accessories.Add(resourceRef);
				}
			}
		}
	}

	// Token: 0x06001FE4 RID: 8164 RVA: 0x000B7CD0 File Offset: 0x000B5ED0
	public void RemoveAccessory(Accessory accessory)
	{
		this.accessories.RemoveAll((ResourceRef<Accessory> x) => x.Get() == accessory);
		this.animController.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride(accessory.slot.targetSymbolId, accessory.slot.overrideLayer);
	}

	// Token: 0x06001FE5 RID: 8165 RVA: 0x000B7D38 File Offset: 0x000B5F38
	public void ApplyAccessories()
	{
		foreach (ResourceRef<Accessory> resourceRef in this.accessories)
		{
			Accessory accessory = resourceRef.Get();
			if (accessory != null)
			{
				this.AddAccessory(accessory);
			}
		}
	}

	// Token: 0x06001FE6 RID: 8166 RVA: 0x000B7D94 File Offset: 0x000B5F94
	public static KCompBuilder.BodyData UpdateAccessorySlots(string nameString, ref List<ResourceRef<Accessory>> accessories)
	{
		accessories.RemoveAll((ResourceRef<Accessory> acc) => acc.Get() == null);
		Personality personalityFromNameStringKey = Db.Get().Personalities.GetPersonalityFromNameStringKey(nameString);
		if (personalityFromNameStringKey != null)
		{
			KCompBuilder.BodyData bodyData = MinionStartingStats.CreateBodyData(personalityFromNameStringKey);
			foreach (AccessorySlot accessorySlot in Db.Get().AccessorySlots.resources)
			{
				if (accessorySlot.accessories.Count != 0)
				{
					Accessory accessory = null;
					if (accessorySlot == Db.Get().AccessorySlots.Body)
					{
						accessory = accessorySlot.Lookup(bodyData.body);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Arm)
					{
						accessory = accessorySlot.Lookup(bodyData.arms);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.ArmLower)
					{
						accessory = accessorySlot.Lookup(bodyData.armslower);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.ArmLowerSkin)
					{
						accessory = accessorySlot.Lookup(bodyData.armLowerSkin);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.ArmUpperSkin)
					{
						accessory = accessorySlot.Lookup(bodyData.armUpperSkin);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.LegSkin)
					{
						accessory = accessorySlot.Lookup(bodyData.legSkin);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Leg)
					{
						accessory = accessorySlot.Lookup(bodyData.legs);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Belt)
					{
						accessory = accessorySlot.Lookup(bodyData.belt);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Neck)
					{
						accessory = accessorySlot.Lookup(bodyData.neck);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Pelvis)
					{
						accessory = accessorySlot.Lookup(bodyData.pelvis);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Foot)
					{
						accessory = accessorySlot.Lookup(bodyData.foot);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Cuff)
					{
						accessory = accessorySlot.Lookup(bodyData.cuff);
					}
					else if (accessorySlot == Db.Get().AccessorySlots.Hand)
					{
						accessory = accessorySlot.Lookup(bodyData.hand);
					}
					if (accessory != null)
					{
						ResourceRef<Accessory> item = new ResourceRef<Accessory>(accessory);
						accessories.RemoveAll((ResourceRef<Accessory> old_acc) => old_acc.Get().slot == accessory.slot);
						accessories.Add(item);
					}
				}
			}
			return bodyData;
		}
		return default(KCompBuilder.BodyData);
	}

	// Token: 0x06001FE7 RID: 8167 RVA: 0x000B80B4 File Offset: 0x000B62B4
	public bool HasAccessory(Accessory accessory)
	{
		return this.accessories.Exists((ResourceRef<Accessory> x) => x.Get() == accessory);
	}

	// Token: 0x06001FE8 RID: 8168 RVA: 0x000B80E8 File Offset: 0x000B62E8
	public Accessory GetAccessory(AccessorySlot slot)
	{
		for (int i = 0; i < this.accessories.Count; i++)
		{
			if (this.accessories[i].Get() != null && this.accessories[i].Get().slot == slot)
			{
				return this.accessories[i].Get();
			}
		}
		return null;
	}

	// Token: 0x06001FE9 RID: 8169 RVA: 0x000B814C File Offset: 0x000B634C
	public void ApplyMinionPersonality(Personality personality)
	{
		this.bodyData = MinionStartingStats.CreateBodyData(personality);
		this.accessories.Clear();
		if (this.animController == null)
		{
			this.animController = base.GetComponent<KAnimControllerBase>();
		}
		foreach (string text in new string[]
		{
			"snapTo_hat",
			"snapTo_hat_hair",
			"snapTo_goggles",
			"snapTo_headFX",
			"snapTo_neck",
			"snapTo_chest",
			"snapTo_pivot",
			"skirt",
			"necklace"
		})
		{
			this.animController.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(text, 0);
			this.animController.SetSymbolVisiblity(text, false);
		}
		this.AddAccessory(Db.Get().AccessorySlots.Eyes.Lookup(this.bodyData.eyes));
		this.AddAccessory(Db.Get().AccessorySlots.Hair.Lookup(this.bodyData.hair));
		this.AddAccessory(Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(this.bodyData.hair)));
		this.AddAccessory(Db.Get().AccessorySlots.HeadShape.Lookup(this.bodyData.headShape));
		this.AddAccessory(Db.Get().AccessorySlots.Mouth.Lookup(this.bodyData.mouth));
		this.AddAccessory(Db.Get().AccessorySlots.Body.Lookup(this.bodyData.body));
		this.AddAccessory(Db.Get().AccessorySlots.Arm.Lookup(this.bodyData.arms));
		this.AddAccessory(Db.Get().AccessorySlots.ArmLower.Lookup(this.bodyData.armslower));
		this.AddAccessory(Db.Get().AccessorySlots.Neck.Lookup(this.bodyData.neck));
		this.AddAccessory(Db.Get().AccessorySlots.Pelvis.Lookup(this.bodyData.pelvis));
		this.AddAccessory(Db.Get().AccessorySlots.Leg.Lookup(this.bodyData.legs));
		this.AddAccessory(Db.Get().AccessorySlots.Foot.Lookup(this.bodyData.foot));
		this.AddAccessory(Db.Get().AccessorySlots.Hand.Lookup(this.bodyData.hand));
		this.AddAccessory(Db.Get().AccessorySlots.Cuff.Lookup(this.bodyData.cuff));
		this.AddAccessory(Db.Get().AccessorySlots.Belt.Lookup(this.bodyData.belt));
		this.AddAccessory(Db.Get().AccessorySlots.ArmLowerSkin.Lookup(this.bodyData.armLowerSkin));
		this.AddAccessory(Db.Get().AccessorySlots.ArmUpperSkin.Lookup(this.bodyData.armUpperSkin));
		this.AddAccessory(Db.Get().AccessorySlots.LegSkin.Lookup(this.bodyData.legSkin));
		this.UpdateHairBasedOnHat();
	}

	// Token: 0x06001FEA RID: 8170 RVA: 0x000B84CC File Offset: 0x000B66CC
	public void ApplyBodyData(KCompBuilder.BodyData bodyData)
	{
		this.accessories.Clear();
		if (this.animController == null)
		{
			this.animController = base.GetComponent<KAnimControllerBase>();
		}
		foreach (string text in new string[]
		{
			"snapTo_hat",
			"snapTo_hat_hair",
			"snapTo_goggles",
			"snapTo_headFX",
			"snapTo_neck",
			"snapTo_chest",
			"snapTo_pivot",
			"skirt",
			"necklace"
		})
		{
			this.animController.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(text, 0);
			this.animController.SetSymbolVisiblity(text, false);
		}
		this.AddAccessory(Db.Get().AccessorySlots.Eyes.Lookup(bodyData.eyes));
		this.AddAccessory(Db.Get().AccessorySlots.Hair.Lookup(bodyData.hair));
		this.AddAccessory(Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(bodyData.hair)));
		this.AddAccessory(Db.Get().AccessorySlots.HeadShape.Lookup(bodyData.headShape));
		this.AddAccessory(Db.Get().AccessorySlots.Mouth.Lookup(bodyData.mouth));
		this.AddAccessory(Db.Get().AccessorySlots.Body.Lookup(bodyData.body));
		this.AddAccessory(Db.Get().AccessorySlots.Arm.Lookup(bodyData.arms));
		this.AddAccessory(Db.Get().AccessorySlots.ArmLower.Lookup(bodyData.armslower));
		this.AddAccessory(Db.Get().AccessorySlots.Neck.Lookup(bodyData.neck));
		this.AddAccessory(Db.Get().AccessorySlots.Pelvis.Lookup(bodyData.pelvis));
		this.AddAccessory(Db.Get().AccessorySlots.Leg.Lookup(bodyData.legs));
		this.AddAccessory(Db.Get().AccessorySlots.Foot.Lookup(bodyData.foot));
		this.AddAccessory(Db.Get().AccessorySlots.Hand.Lookup(bodyData.hand));
		this.AddAccessory(Db.Get().AccessorySlots.Cuff.Lookup(bodyData.cuff));
		this.AddAccessory(Db.Get().AccessorySlots.Belt.Lookup(bodyData.belt));
		this.AddAccessory(Db.Get().AccessorySlots.ArmLowerSkin.Lookup(bodyData.armLowerSkin));
		this.AddAccessory(Db.Get().AccessorySlots.ArmUpperSkin.Lookup(bodyData.armUpperSkin));
		this.AddAccessory(Db.Get().AccessorySlots.LegSkin.Lookup(bodyData.legSkin));
		this.UpdateHairBasedOnHat();
	}

	// Token: 0x06001FEB RID: 8171 RVA: 0x000B87E4 File Offset: 0x000B69E4
	public void UpdateHairBasedOnHat()
	{
		if (!this.GetAccessory(Db.Get().AccessorySlots.Hat).IsNullOrDestroyed())
		{
			this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
			this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
			return;
		}
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, false);
	}

	// Token: 0x06001FEC RID: 8172 RVA: 0x000B88B4 File Offset: 0x000B6AB4
	public void GetBodySlots(ref KCompBuilder.BodyData fd)
	{
		fd.eyes = HashedString.Invalid;
		fd.hair = HashedString.Invalid;
		fd.headShape = HashedString.Invalid;
		fd.mouth = HashedString.Invalid;
		fd.neck = HashedString.Invalid;
		fd.body = HashedString.Invalid;
		fd.arms = HashedString.Invalid;
		fd.armslower = HashedString.Invalid;
		fd.hat = HashedString.Invalid;
		fd.faceFX = HashedString.Invalid;
		fd.armLowerSkin = HashedString.Invalid;
		fd.armUpperSkin = HashedString.Invalid;
		fd.legSkin = HashedString.Invalid;
		fd.belt = HashedString.Invalid;
		fd.pelvis = HashedString.Invalid;
		fd.foot = HashedString.Invalid;
		fd.skirt = HashedString.Invalid;
		fd.necklace = HashedString.Invalid;
		fd.cuff = HashedString.Invalid;
		fd.hand = HashedString.Invalid;
		for (int i = 0; i < this.accessories.Count; i++)
		{
			Accessory accessory = this.accessories[i].Get();
			if (accessory != null)
			{
				if (accessory.slot.Id == "Eyes")
				{
					fd.eyes = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Hair")
				{
					fd.hair = accessory.IdHash;
				}
				else if (accessory.slot.Id == "HeadShape")
				{
					fd.headShape = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Mouth")
				{
					fd.mouth = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Neck")
				{
					fd.neck = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Torso")
				{
					fd.body = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Arm_Sleeve")
				{
					fd.arms = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Arm_Lower_Sleeve")
				{
					fd.armslower = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Hat")
				{
					fd.hat = HashedString.Invalid;
				}
				else if (accessory.slot.Id == "FaceEffect")
				{
					fd.faceFX = HashedString.Invalid;
				}
				else if (accessory.slot.Id == "Arm_Lower")
				{
					fd.armLowerSkin = accessory.Id;
				}
				else if (accessory.slot.Id == "Arm_Upper")
				{
					fd.armUpperSkin = accessory.Id;
				}
				else if (accessory.slot.Id == "Leg_Skin")
				{
					fd.legSkin = accessory.Id;
				}
				else if (accessory.slot.Id == "Leg")
				{
					fd.legs = accessory.Id;
				}
				else if (accessory.slot.Id == "Belt")
				{
					fd.belt = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Pelvis")
				{
					fd.pelvis = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Foot")
				{
					fd.foot = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Cuff")
				{
					fd.cuff = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Skirt")
				{
					fd.skirt = accessory.IdHash;
				}
				else if (accessory.slot.Id == "Hand")
				{
					fd.hand = accessory.IdHash;
				}
			}
		}
	}

	// Token: 0x04001292 RID: 4754
	[Serialize]
	private List<ResourceRef<Accessory>> accessories = new List<ResourceRef<Accessory>>();

	// Token: 0x04001293 RID: 4755
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x04001294 RID: 4756
	[Serialize]
	private List<ResourceRef<ClothingItemResource>> clothingItems = new List<ResourceRef<ClothingItemResource>>();
}
