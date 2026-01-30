using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02000F37 RID: 3895
	public class Diseases : ResourceSet<Disease>
	{
		// Token: 0x06007C5A RID: 31834 RVA: 0x0030ECFC File Offset: 0x0030CEFC
		public Diseases(ResourceSet parent, bool statsOnly = false) : base("Diseases", parent)
		{
			this.FoodGerms = base.Add(new FoodGerms(statsOnly));
			this.SlimeGerms = base.Add(new SlimeGerms(statsOnly));
			this.PollenGerms = base.Add(new PollenGerms(statsOnly));
			this.ZombieSpores = base.Add(new ZombieSpores(statsOnly));
			if (DlcManager.FeatureRadiationEnabled())
			{
				this.RadiationPoisoning = base.Add(new RadiationPoisoning(statsOnly));
			}
		}

		// Token: 0x06007C5B RID: 31835 RVA: 0x0030ED78 File Offset: 0x0030CF78
		public bool IsValidID(string id)
		{
			bool result = false;
			using (List<Disease>.Enumerator enumerator = this.resources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Id == id)
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06007C5C RID: 31836 RVA: 0x0030EDD8 File Offset: 0x0030CFD8
		public byte GetIndex(int hash)
		{
			byte b = 0;
			while ((int)b < this.resources.Count)
			{
				Disease disease = this.resources[(int)b];
				if (hash == disease.id.GetHashCode())
				{
					return b;
				}
				b += 1;
			}
			return byte.MaxValue;
		}

		// Token: 0x06007C5D RID: 31837 RVA: 0x0030EE24 File Offset: 0x0030D024
		public byte GetIndex(HashedString id)
		{
			return this.GetIndex(id.GetHashCode());
		}

		// Token: 0x0400595E RID: 22878
		public Disease FoodGerms;

		// Token: 0x0400595F RID: 22879
		public Disease SlimeGerms;

		// Token: 0x04005960 RID: 22880
		public Disease PollenGerms;

		// Token: 0x04005961 RID: 22881
		public Disease ZombieSpores;

		// Token: 0x04005962 RID: 22882
		public Disease RadiationPoisoning;
	}
}
