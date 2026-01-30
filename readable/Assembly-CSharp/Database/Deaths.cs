using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F36 RID: 3894
	public class Deaths : ResourceSet<Death>
	{
		// Token: 0x06007C59 RID: 31833 RVA: 0x0030EAAC File Offset: 0x0030CCAC
		public Deaths(ResourceSet parent) : base("Deaths", parent)
		{
			this.Generic = new Death("Generic", this, DUPLICANTS.DEATHS.GENERIC.NAME, DUPLICANTS.DEATHS.GENERIC.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.Frozen = new Death("Frozen", this, DUPLICANTS.DEATHS.FROZEN.NAME, DUPLICANTS.DEATHS.FROZEN.DESCRIPTION, "death_freeze_trans", "death_freeze_solid");
			this.Suffocation = new Death("Suffocation", this, DUPLICANTS.DEATHS.SUFFOCATION.NAME, DUPLICANTS.DEATHS.SUFFOCATION.DESCRIPTION, "death_suffocation", "dead_on_back");
			this.Starvation = new Death("Starvation", this, DUPLICANTS.DEATHS.STARVATION.NAME, DUPLICANTS.DEATHS.STARVATION.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.Overheating = new Death("Overheating", this, DUPLICANTS.DEATHS.OVERHEATING.NAME, DUPLICANTS.DEATHS.OVERHEATING.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.Drowned = new Death("Drowned", this, DUPLICANTS.DEATHS.DROWNED.NAME, DUPLICANTS.DEATHS.DROWNED.DESCRIPTION, "death_suffocation", "dead_on_back");
			this.Explosion = new Death("Explosion", this, DUPLICANTS.DEATHS.EXPLOSION.NAME, DUPLICANTS.DEATHS.EXPLOSION.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.Slain = new Death("Combat", this, DUPLICANTS.DEATHS.COMBAT.NAME, DUPLICANTS.DEATHS.COMBAT.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.FatalDisease = new Death("FatalDisease", this, DUPLICANTS.DEATHS.FATALDISEASE.NAME, DUPLICANTS.DEATHS.FATALDISEASE.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.Radiation = new Death("Radiation", this, DUPLICANTS.DEATHS.RADIATION.NAME, DUPLICANTS.DEATHS.RADIATION.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.HitByHighEnergyParticle = new Death("HitByHighEnergyParticle", this, DUPLICANTS.DEATHS.HITBYHIGHENERGYPARTICLE.NAME, DUPLICANTS.DEATHS.HITBYHIGHENERGYPARTICLE.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.DeadBattery = new Death("DeadBattery", this, DUPLICANTS.DEATHS.HITBYHIGHENERGYPARTICLE.NAME, DUPLICANTS.DEATHS.HITBYHIGHENERGYPARTICLE.DESCRIPTION, "dead_on_back", "dead_on_back");
		}

		// Token: 0x04005951 RID: 22865
		public Death Generic;

		// Token: 0x04005952 RID: 22866
		public Death Frozen;

		// Token: 0x04005953 RID: 22867
		public Death Suffocation;

		// Token: 0x04005954 RID: 22868
		public Death Starvation;

		// Token: 0x04005955 RID: 22869
		public Death Slain;

		// Token: 0x04005956 RID: 22870
		public Death Overheating;

		// Token: 0x04005957 RID: 22871
		public Death Drowned;

		// Token: 0x04005958 RID: 22872
		public Death Explosion;

		// Token: 0x04005959 RID: 22873
		public Death FatalDisease;

		// Token: 0x0400595A RID: 22874
		public Death Radiation;

		// Token: 0x0400595B RID: 22875
		public Death HitByHighEnergyParticle;

		// Token: 0x0400595C RID: 22876
		public Death DeadBattery;

		// Token: 0x0400595D RID: 22877
		public Death DeadCyborgChargeExpired;
	}
}
