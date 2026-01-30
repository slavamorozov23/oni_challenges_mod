using System;
using STRINGS;
using TUNING;

namespace Database
{
	// Token: 0x02000F45 RID: 3909
	public class AssignableSlots : ResourceSet<AssignableSlot>
	{
		// Token: 0x06007C90 RID: 31888 RVA: 0x0031472C File Offset: 0x0031292C
		public AssignableSlots()
		{
			this.Bed = base.Add(new OwnableSlot("Bed", MISC.TAGS.BED));
			this.MessStation = base.Add(new OwnableSlot("MessStation", MISC.TAGS.MESSSTATION));
			this.Clinic = base.Add(new OwnableSlot("Clinic", MISC.TAGS.CLINIC));
			this.MedicalBed = base.Add(new OwnableSlot("MedicalBed", MISC.TAGS.CLINIC));
			this.MedicalBed.showInUI = false;
			this.GeneShuffler = base.Add(new OwnableSlot("GeneShuffler", MISC.TAGS.GENE_SHUFFLER));
			this.GeneShuffler.showInUI = false;
			this.Toilet = base.Add(new OwnableSlot("Toilet", MISC.TAGS.TOILET));
			this.MassageTable = base.Add(new OwnableSlot("MassageTable", MISC.TAGS.MASSAGE_TABLE));
			this.RocketCommandModule = base.Add(new OwnableSlot("RocketCommandModule", MISC.TAGS.COMMAND_MODULE));
			this.HabitatModule = base.Add(new OwnableSlot("HabitatModule", MISC.TAGS.HABITAT_MODULE));
			this.ResetSkillsStation = base.Add(new OwnableSlot("ResetSkillsStation", "ResetSkillsStation"));
			this.WarpPortal = base.Add(new OwnableSlot("WarpPortal", MISC.TAGS.WARP_PORTAL));
			this.WarpPortal.showInUI = false;
			this.BionicUpgrade = base.Add(new OwnableSlot("BionicUpgrade", MISC.TAGS.BIONICUPGRADE));
			this.Toy = base.Add(new EquipmentSlot(TUNING.EQUIPMENT.TOYS.SLOT, MISC.TAGS.TOY, false));
			this.Suit = base.Add(new EquipmentSlot(TUNING.EQUIPMENT.SUITS.SLOT, MISC.TAGS.SUIT, true));
			this.Tool = base.Add(new EquipmentSlot(TUNING.EQUIPMENT.TOOLS.TOOLSLOT, MISC.TAGS.MULTITOOL, false));
			this.Outfit = base.Add(new EquipmentSlot(TUNING.EQUIPMENT.CLOTHING.SLOT, UI.StripLinkFormatting(MISC.TAGS.CLOTHES), true));
		}

		// Token: 0x04005AD6 RID: 23254
		public AssignableSlot Bed;

		// Token: 0x04005AD7 RID: 23255
		public AssignableSlot MessStation;

		// Token: 0x04005AD8 RID: 23256
		public AssignableSlot Clinic;

		// Token: 0x04005AD9 RID: 23257
		public AssignableSlot GeneShuffler;

		// Token: 0x04005ADA RID: 23258
		public AssignableSlot MedicalBed;

		// Token: 0x04005ADB RID: 23259
		public AssignableSlot Toilet;

		// Token: 0x04005ADC RID: 23260
		public AssignableSlot MassageTable;

		// Token: 0x04005ADD RID: 23261
		public AssignableSlot RocketCommandModule;

		// Token: 0x04005ADE RID: 23262
		public AssignableSlot HabitatModule;

		// Token: 0x04005ADF RID: 23263
		public AssignableSlot ResetSkillsStation;

		// Token: 0x04005AE0 RID: 23264
		public AssignableSlot WarpPortal;

		// Token: 0x04005AE1 RID: 23265
		public AssignableSlot Toy;

		// Token: 0x04005AE2 RID: 23266
		public AssignableSlot Suit;

		// Token: 0x04005AE3 RID: 23267
		public AssignableSlot Tool;

		// Token: 0x04005AE4 RID: 23268
		public AssignableSlot Outfit;

		// Token: 0x04005AE5 RID: 23269
		public AssignableSlot BionicUpgrade;
	}
}
