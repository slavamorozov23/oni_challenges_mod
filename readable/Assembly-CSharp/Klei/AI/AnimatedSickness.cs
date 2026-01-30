using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001037 RID: 4151
	public class AnimatedSickness : Sickness.SicknessComponent
	{
		// Token: 0x060080BA RID: 32954 RVA: 0x0033BBFC File Offset: 0x00339DFC
		public AnimatedSickness(HashedString[] kanim_filenames, Expression expression)
		{
			this.kanims = new KAnimFile[kanim_filenames.Length];
			for (int i = 0; i < kanim_filenames.Length; i++)
			{
				this.kanims[i] = Assets.GetAnim(kanim_filenames[i]);
			}
			this.expression = expression;
		}

		// Token: 0x060080BB RID: 32955 RVA: 0x0033BC48 File Offset: 0x00339E48
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			for (int i = 0; i < this.kanims.Length; i++)
			{
				go.GetComponent<KAnimControllerBase>().AddAnimOverrides(this.kanims[i], 10f);
			}
			if (this.expression != null)
			{
				go.GetComponent<FaceGraph>().AddExpression(this.expression);
			}
			return null;
		}

		// Token: 0x060080BC RID: 32956 RVA: 0x0033BC9C File Offset: 0x00339E9C
		public override void OnCure(GameObject go, object instace_data)
		{
			if (this.expression != null)
			{
				go.GetComponent<FaceGraph>().RemoveExpression(this.expression);
			}
			for (int i = 0; i < this.kanims.Length; i++)
			{
				go.GetComponent<KAnimControllerBase>().RemoveAnimOverrides(this.kanims[i]);
			}
		}

		// Token: 0x04006186 RID: 24966
		private KAnimFile[] kanims;

		// Token: 0x04006187 RID: 24967
		private Expression expression;
	}
}
