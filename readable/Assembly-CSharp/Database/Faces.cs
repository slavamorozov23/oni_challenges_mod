using System;

namespace Database
{
	// Token: 0x02000F3E RID: 3902
	public class Faces : ResourceSet<Face>
	{
		// Token: 0x06007C73 RID: 31859 RVA: 0x00311408 File Offset: 0x0030F608
		public Faces()
		{
			this.Neutral = base.Add(new Face("Neutral", null));
			this.Happy = base.Add(new Face("Happy", null));
			this.Uncomfortable = base.Add(new Face("Uncomfortable", null));
			this.Cold = base.Add(new Face("Cold", null));
			this.Hot = base.Add(new Face("Hot", "headfx_sweat"));
			this.Tired = base.Add(new Face("Tired", null));
			this.Sleep = base.Add(new Face("Sleep", null));
			this.Hungry = base.Add(new Face("Hungry", null));
			this.Angry = base.Add(new Face("Angry", null));
			this.Suffocate = base.Add(new Face("Suffocate", null));
			this.Sick = base.Add(new Face("Sick", "headfx_sick"));
			this.SickSpores = base.Add(new Face("Spores", "headfx_spores"));
			this.Zombie = base.Add(new Face("Zombie", null));
			this.SickFierySkin = base.Add(new Face("Fiery", "headfx_fiery"));
			this.SickCold = base.Add(new Face("SickCold", "headfx_sickcold"));
			this.Pollen = base.Add(new Face("Pollen", "headfx_pollen"));
			this.Dead = base.Add(new Face("Death", null));
			this.Productive = base.Add(new Face("Productive", null));
			this.Determined = base.Add(new Face("Determined", null));
			this.Sticker = base.Add(new Face("Sticker", null));
			this.Sparkle = base.Add(new Face("Sparkle", null));
			this.Balloon = base.Add(new Face("Balloon", null));
			this.Tickled = base.Add(new Face("Tickled", null));
			this.Music = base.Add(new Face("Music", null));
			this.Radiation1 = base.Add(new Face("Radiation1", "headfx_radiation1"));
			this.Radiation2 = base.Add(new Face("Radiation2", "headfx_radiation2"));
			this.Radiation3 = base.Add(new Face("Radiation3", "headfx_radiation3"));
			this.Radiation4 = base.Add(new Face("Radiation4", "headfx_radiation4"));
			this.Robodancer = base.Add(new Face("robotdance", null));
		}

		// Token: 0x04005A21 RID: 23073
		public Face Neutral;

		// Token: 0x04005A22 RID: 23074
		public Face Happy;

		// Token: 0x04005A23 RID: 23075
		public Face Uncomfortable;

		// Token: 0x04005A24 RID: 23076
		public Face Cold;

		// Token: 0x04005A25 RID: 23077
		public Face Hot;

		// Token: 0x04005A26 RID: 23078
		public Face Tired;

		// Token: 0x04005A27 RID: 23079
		public Face Sleep;

		// Token: 0x04005A28 RID: 23080
		public Face Hungry;

		// Token: 0x04005A29 RID: 23081
		public Face Angry;

		// Token: 0x04005A2A RID: 23082
		public Face Suffocate;

		// Token: 0x04005A2B RID: 23083
		public Face Dead;

		// Token: 0x04005A2C RID: 23084
		public Face Sick;

		// Token: 0x04005A2D RID: 23085
		public Face SickSpores;

		// Token: 0x04005A2E RID: 23086
		public Face Zombie;

		// Token: 0x04005A2F RID: 23087
		public Face SickFierySkin;

		// Token: 0x04005A30 RID: 23088
		public Face SickCold;

		// Token: 0x04005A31 RID: 23089
		public Face Pollen;

		// Token: 0x04005A32 RID: 23090
		public Face Productive;

		// Token: 0x04005A33 RID: 23091
		public Face Determined;

		// Token: 0x04005A34 RID: 23092
		public Face Sticker;

		// Token: 0x04005A35 RID: 23093
		public Face Balloon;

		// Token: 0x04005A36 RID: 23094
		public Face Sparkle;

		// Token: 0x04005A37 RID: 23095
		public Face Tickled;

		// Token: 0x04005A38 RID: 23096
		public Face Music;

		// Token: 0x04005A39 RID: 23097
		public Face Radiation1;

		// Token: 0x04005A3A RID: 23098
		public Face Radiation2;

		// Token: 0x04005A3B RID: 23099
		public Face Radiation3;

		// Token: 0x04005A3C RID: 23100
		public Face Radiation4;

		// Token: 0x04005A3D RID: 23101
		public Face Robodancer;
	}
}
