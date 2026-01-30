using System;

namespace Database
{
	// Token: 0x02000F66 RID: 3942
	public class Urges : ResourceSet<Urge>
	{
		// Token: 0x06007D07 RID: 32007 RVA: 0x0031CFFC File Offset: 0x0031B1FC
		public Urges()
		{
			this.HealCritical = base.Add(new Urge("HealCritical"));
			this.BeOffline = base.Add(new Urge("BeOffline"));
			this.BeIncapacitated = base.Add(new Urge("BeIncapacitated"));
			this.PacifyEat = base.Add(new Urge("PacifyEat"));
			this.PacifySleep = base.Add(new Urge("PacifySleep"));
			this.PacifyIdle = base.Add(new Urge("PacifyIdle"));
			this.EmoteHighPriority = base.Add(new Urge("EmoteHighPriority"));
			this.RecoverBreath = base.Add(new Urge("RecoverBreath"));
			this.RecoverWarmth = base.Add(new Urge("RecoverWarmth"));
			this.Aggression = base.Add(new Urge("Aggression"));
			this.MoveToQuarantine = base.Add(new Urge("MoveToQuarantine"));
			this.WashHands = base.Add(new Urge("WashHands"));
			this.Shower = base.Add(new Urge("Shower"));
			this.Eat = base.Add(new Urge("Eat"));
			this.ReloadElectrobank = base.Add(new Urge("ReloadElectrobank"));
			this.Pee = base.Add(new Urge("Pee"));
			this.RestDueToDisease = base.Add(new Urge("RestDueToDisease"));
			this.Sleep = base.Add(new Urge("Sleep"));
			this.Narcolepsy = base.Add(new Urge("Narcolepsy"));
			this.Doctor = base.Add(new Urge("Doctor"));
			this.Heal = base.Add(new Urge("Heal"));
			this.Feed = base.Add(new Urge("Feed"));
			this.PacifyRelocate = base.Add(new Urge("PacifyRelocate"));
			this.Emote = base.Add(new Urge("Emote"));
			this.MoveToSafety = base.Add(new Urge("MoveToSafety"));
			this.WarmUp = base.Add(new Urge("WarmUp"));
			this.CoolDown = base.Add(new Urge("CoolDown"));
			this.LearnSkill = base.Add(new Urge("LearnSkill"));
			this.EmoteIdle = base.Add(new Urge("EmoteIdle"));
			this.OilRefill = base.Add(new Urge("OilRefill"));
			this.GunkPee = base.Add(new Urge("GunkPee"));
			this.FindOxygenRefill = base.Add(new Urge("FindOxygenRefill"));
			this.Fart = base.Add(new Urge("Fart"));
		}

		// Token: 0x04005BFB RID: 23547
		public Urge BeIncapacitated;

		// Token: 0x04005BFC RID: 23548
		public Urge BeOffline;

		// Token: 0x04005BFD RID: 23549
		public Urge Sleep;

		// Token: 0x04005BFE RID: 23550
		public Urge Narcolepsy;

		// Token: 0x04005BFF RID: 23551
		public Urge Eat;

		// Token: 0x04005C00 RID: 23552
		public Urge ReloadElectrobank;

		// Token: 0x04005C01 RID: 23553
		public Urge WashHands;

		// Token: 0x04005C02 RID: 23554
		public Urge Shower;

		// Token: 0x04005C03 RID: 23555
		public Urge Pee;

		// Token: 0x04005C04 RID: 23556
		public Urge MoveToQuarantine;

		// Token: 0x04005C05 RID: 23557
		public Urge HealCritical;

		// Token: 0x04005C06 RID: 23558
		public Urge RecoverBreath;

		// Token: 0x04005C07 RID: 23559
		public Urge FindOxygenRefill;

		// Token: 0x04005C08 RID: 23560
		public Urge RecoverWarmth;

		// Token: 0x04005C09 RID: 23561
		public Urge Emote;

		// Token: 0x04005C0A RID: 23562
		public Urge Feed;

		// Token: 0x04005C0B RID: 23563
		public Urge Doctor;

		// Token: 0x04005C0C RID: 23564
		public Urge Flee;

		// Token: 0x04005C0D RID: 23565
		public Urge Heal;

		// Token: 0x04005C0E RID: 23566
		public Urge PacifyIdle;

		// Token: 0x04005C0F RID: 23567
		public Urge PacifyEat;

		// Token: 0x04005C10 RID: 23568
		public Urge PacifySleep;

		// Token: 0x04005C11 RID: 23569
		public Urge PacifyRelocate;

		// Token: 0x04005C12 RID: 23570
		public Urge RestDueToDisease;

		// Token: 0x04005C13 RID: 23571
		public Urge EmoteHighPriority;

		// Token: 0x04005C14 RID: 23572
		public Urge Aggression;

		// Token: 0x04005C15 RID: 23573
		public Urge MoveToSafety;

		// Token: 0x04005C16 RID: 23574
		public Urge WarmUp;

		// Token: 0x04005C17 RID: 23575
		public Urge CoolDown;

		// Token: 0x04005C18 RID: 23576
		public Urge LearnSkill;

		// Token: 0x04005C19 RID: 23577
		public Urge EmoteIdle;

		// Token: 0x04005C1A RID: 23578
		public Urge OilRefill;

		// Token: 0x04005C1B RID: 23579
		public Urge GunkPee;

		// Token: 0x04005C1C RID: 23580
		public Urge Fart;
	}
}
