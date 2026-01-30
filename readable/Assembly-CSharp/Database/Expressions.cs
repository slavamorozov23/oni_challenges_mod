using System;

namespace Database
{
	// Token: 0x02000F3D RID: 3901
	public class Expressions : ResourceSet<Expression>
	{
		// Token: 0x06007C72 RID: 31858 RVA: 0x003110D4 File Offset: 0x0030F2D4
		public Expressions(ResourceSet parent) : base("Expressions", parent)
		{
			Faces faces = Db.Get().Faces;
			this.Angry = new Expression("Angry", this, faces.Angry);
			this.Suffocate = new Expression("Suffocate", this, faces.Suffocate);
			this.RecoverBreath = new Expression("RecoverBreath", this, faces.Uncomfortable);
			this.RedAlert = new Expression("RedAlert", this, faces.Hot);
			this.Hungry = new Expression("Hungry", this, faces.Hungry);
			this.Radiation1 = new Expression("Radiation1", this, faces.Radiation1);
			this.Radiation2 = new Expression("Radiation2", this, faces.Radiation2);
			this.Radiation3 = new Expression("Radiation3", this, faces.Radiation3);
			this.Radiation4 = new Expression("Radiation4", this, faces.Radiation4);
			this.SickSpores = new Expression("SickSpores", this, faces.SickSpores);
			this.Zombie = new Expression("Zombie", this, faces.Zombie);
			this.SickFierySkin = new Expression("SickFierySkin", this, faces.SickFierySkin);
			this.SickCold = new Expression("SickCold", this, faces.SickCold);
			this.Pollen = new Expression("Pollen", this, faces.Pollen);
			this.Sick = new Expression("Sick", this, faces.Sick);
			this.Cold = new Expression("Cold", this, faces.Cold);
			this.Hot = new Expression("Hot", this, faces.Hot);
			this.FullBladder = new Expression("FullBladder", this, faces.Uncomfortable);
			this.Tired = new Expression("Tired", this, faces.Tired);
			this.Unhappy = new Expression("Unhappy", this, faces.Uncomfortable);
			this.Uncomfortable = new Expression("Uncomfortable", this, faces.Uncomfortable);
			this.Productive = new Expression("Productive", this, faces.Productive);
			this.Determined = new Expression("Determined", this, faces.Determined);
			this.Sticker = new Expression("Sticker", this, faces.Sticker);
			this.Balloon = new Expression("Sticker", this, faces.Balloon);
			this.Sparkle = new Expression("Sticker", this, faces.Sparkle);
			this.Music = new Expression("Music", this, faces.Music);
			this.Tickled = new Expression("Tickled", this, faces.Tickled);
			this.BionicJoy = new Expression("Robodancer", this, faces.Robodancer);
			this.Happy = new Expression("Happy", this, faces.Happy);
			this.Relief = new Expression("Relief", this, faces.Happy);
			this.Neutral = new Expression("Neutral", this, faces.Neutral);
			for (int i = this.Count - 1; i >= 0; i--)
			{
				this.resources[i].priority = 100 * (this.Count - i);
			}
		}

		// Token: 0x04005A01 RID: 23041
		public Expression Neutral;

		// Token: 0x04005A02 RID: 23042
		public Expression Happy;

		// Token: 0x04005A03 RID: 23043
		public Expression Uncomfortable;

		// Token: 0x04005A04 RID: 23044
		public Expression Cold;

		// Token: 0x04005A05 RID: 23045
		public Expression Hot;

		// Token: 0x04005A06 RID: 23046
		public Expression FullBladder;

		// Token: 0x04005A07 RID: 23047
		public Expression Tired;

		// Token: 0x04005A08 RID: 23048
		public Expression Hungry;

		// Token: 0x04005A09 RID: 23049
		public Expression Angry;

		// Token: 0x04005A0A RID: 23050
		public Expression Unhappy;

		// Token: 0x04005A0B RID: 23051
		public Expression RedAlert;

		// Token: 0x04005A0C RID: 23052
		public Expression Suffocate;

		// Token: 0x04005A0D RID: 23053
		public Expression RecoverBreath;

		// Token: 0x04005A0E RID: 23054
		public Expression Sick;

		// Token: 0x04005A0F RID: 23055
		public Expression SickSpores;

		// Token: 0x04005A10 RID: 23056
		public Expression Zombie;

		// Token: 0x04005A11 RID: 23057
		public Expression SickFierySkin;

		// Token: 0x04005A12 RID: 23058
		public Expression SickCold;

		// Token: 0x04005A13 RID: 23059
		public Expression Pollen;

		// Token: 0x04005A14 RID: 23060
		public Expression Relief;

		// Token: 0x04005A15 RID: 23061
		public Expression Productive;

		// Token: 0x04005A16 RID: 23062
		public Expression Determined;

		// Token: 0x04005A17 RID: 23063
		public Expression Sticker;

		// Token: 0x04005A18 RID: 23064
		public Expression Balloon;

		// Token: 0x04005A19 RID: 23065
		public Expression Sparkle;

		// Token: 0x04005A1A RID: 23066
		public Expression Music;

		// Token: 0x04005A1B RID: 23067
		public Expression Tickled;

		// Token: 0x04005A1C RID: 23068
		public Expression Radiation1;

		// Token: 0x04005A1D RID: 23069
		public Expression Radiation2;

		// Token: 0x04005A1E RID: 23070
		public Expression Radiation3;

		// Token: 0x04005A1F RID: 23071
		public Expression Radiation4;

		// Token: 0x04005A20 RID: 23072
		public Expression BionicJoy;
	}
}
