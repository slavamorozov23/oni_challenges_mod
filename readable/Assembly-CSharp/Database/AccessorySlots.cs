using System;

namespace Database
{
	// Token: 0x02000F19 RID: 3865
	public class AccessorySlots : ResourceSet<AccessorySlot>
	{
		// Token: 0x06007BFD RID: 31741 RVA: 0x003014C4 File Offset: 0x002FF6C4
		public AccessorySlots(ResourceSet parent) : base("AccessorySlots", parent)
		{
			parent = Db.Get().Accessories;
			KAnimFile anim = Assets.GetAnim("head_swap_kanim");
			KAnimFile anim2 = Assets.GetAnim("body_comp_default_kanim");
			KAnimFile anim3 = Assets.GetAnim("body_swap_kanim");
			KAnimFile anim4 = Assets.GetAnim("hair_swap_kanim");
			KAnimFile anim5 = Assets.GetAnim("hat_swap_kanim");
			this.Eyes = new AccessorySlot("Eyes", this, anim, 0);
			this.Hair = new AccessorySlot("Hair", this, anim4, 0);
			this.HeadShape = new AccessorySlot("HeadShape", this, anim, 0);
			this.Mouth = new AccessorySlot("Mouth", this, anim, 0);
			this.Hat = new AccessorySlot("Hat", this, anim5, 4);
			this.HatHair = new AccessorySlot("Hat_Hair", this, anim4, 0);
			this.HeadEffects = new AccessorySlot("HeadFX", this, anim, 0);
			this.Body = new AccessorySlot("Torso", this, new KAnimHashedString("torso"), anim3, null, 0);
			this.Arm = new AccessorySlot("Arm_Sleeve", this, new KAnimHashedString("arm_sleeve"), anim3, null, 0);
			this.ArmLower = new AccessorySlot("Arm_Lower_Sleeve", this, new KAnimHashedString("arm_lower_sleeve"), anim3, null, 0);
			this.Belt = new AccessorySlot("Belt", this, new KAnimHashedString("belt"), anim2, null, 0);
			this.Neck = new AccessorySlot("Neck", this, new KAnimHashedString("neck"), anim2, null, 0);
			this.Pelvis = new AccessorySlot("Pelvis", this, new KAnimHashedString("pelvis"), anim2, null, 0);
			this.Foot = new AccessorySlot("Foot", this, new KAnimHashedString("foot"), anim2, Assets.GetAnim("shoes_basic_black_kanim"), 0);
			this.Leg = new AccessorySlot("Leg", this, new KAnimHashedString("leg"), anim2, null, 0);
			this.Necklace = new AccessorySlot("Necklace", this, new KAnimHashedString("necklace"), anim2, null, 0);
			this.Cuff = new AccessorySlot("Cuff", this, new KAnimHashedString("cuff"), anim2, null, 0);
			this.Hand = new AccessorySlot("Hand", this, new KAnimHashedString("hand_paint"), anim2, null, 0);
			this.Skirt = new AccessorySlot("Skirt", this, new KAnimHashedString("skirt"), anim3, null, 0);
			this.ArmLowerSkin = new AccessorySlot("Arm_Lower", this, new KAnimHashedString("arm_lower"), anim3, null, 0);
			this.ArmUpperSkin = new AccessorySlot("Arm_Upper", this, new KAnimHashedString("arm_upper"), anim3, null, 0);
			this.LegSkin = new AccessorySlot("Leg_Skin", this, new KAnimHashedString("leg_skin"), anim3, null, 0);
			foreach (AccessorySlot accessorySlot in this.resources)
			{
				accessorySlot.AddAccessories(accessorySlot.AnimFile, parent);
			}
			Db.Get().Accessories.AddCustomAccessories(Assets.GetAnim("body_lonelyminion_kanim"), parent, this);
			Db.Get().Accessories.AddCustomAccessories(Assets.GetAnim("body_sena_kanim"), parent, this);
		}

		// Token: 0x06007BFE RID: 31742 RVA: 0x0030181C File Offset: 0x002FFA1C
		public AccessorySlot Find(KAnimHashedString symbol_name)
		{
			foreach (AccessorySlot accessorySlot in Db.Get().AccessorySlots.resources)
			{
				if (symbol_name == accessorySlot.targetSymbolId)
				{
					return accessorySlot;
				}
			}
			return null;
		}

		// Token: 0x0400564B RID: 22091
		public AccessorySlot Eyes;

		// Token: 0x0400564C RID: 22092
		public AccessorySlot Hair;

		// Token: 0x0400564D RID: 22093
		public AccessorySlot HeadShape;

		// Token: 0x0400564E RID: 22094
		public AccessorySlot Mouth;

		// Token: 0x0400564F RID: 22095
		public AccessorySlot Body;

		// Token: 0x04005650 RID: 22096
		public AccessorySlot Arm;

		// Token: 0x04005651 RID: 22097
		public AccessorySlot ArmLower;

		// Token: 0x04005652 RID: 22098
		public AccessorySlot Hat;

		// Token: 0x04005653 RID: 22099
		public AccessorySlot HatHair;

		// Token: 0x04005654 RID: 22100
		public AccessorySlot HeadEffects;

		// Token: 0x04005655 RID: 22101
		public AccessorySlot Belt;

		// Token: 0x04005656 RID: 22102
		public AccessorySlot Neck;

		// Token: 0x04005657 RID: 22103
		public AccessorySlot Pelvis;

		// Token: 0x04005658 RID: 22104
		public AccessorySlot Leg;

		// Token: 0x04005659 RID: 22105
		public AccessorySlot Foot;

		// Token: 0x0400565A RID: 22106
		public AccessorySlot Skirt;

		// Token: 0x0400565B RID: 22107
		public AccessorySlot Necklace;

		// Token: 0x0400565C RID: 22108
		public AccessorySlot Cuff;

		// Token: 0x0400565D RID: 22109
		public AccessorySlot Hand;

		// Token: 0x0400565E RID: 22110
		public AccessorySlot ArmLowerSkin;

		// Token: 0x0400565F RID: 22111
		public AccessorySlot ArmUpperSkin;

		// Token: 0x04005660 RID: 22112
		public AccessorySlot LegSkin;
	}
}
