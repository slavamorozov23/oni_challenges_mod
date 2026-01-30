using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02000F57 RID: 3927
	public class Sicknesses : ResourceSet<Sickness>
	{
		// Token: 0x06007CC6 RID: 31942 RVA: 0x003179BC File Offset: 0x00315BBC
		public Sicknesses(ResourceSet parent) : base("Sicknesses", parent)
		{
			this.FoodSickness = base.Add(new FoodSickness());
			this.SlimeSickness = base.Add(new SlimeSickness());
			this.ZombieSickness = base.Add(new ZombieSickness());
			if (DlcManager.FeatureRadiationEnabled())
			{
				this.RadiationSickness = base.Add(new RadiationSickness());
			}
			this.Allergies = base.Add(new Allergies());
			this.Sunburn = base.Add(new Sunburn());
		}

		// Token: 0x06007CC7 RID: 31943 RVA: 0x00317A44 File Offset: 0x00315C44
		public static bool IsValidID(string id)
		{
			bool result = false;
			using (List<Sickness>.Enumerator enumerator = Db.Get().Sicknesses.resources.GetEnumerator())
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

		// Token: 0x04005B65 RID: 23397
		public Sickness FoodSickness;

		// Token: 0x04005B66 RID: 23398
		public Sickness SlimeSickness;

		// Token: 0x04005B67 RID: 23399
		public Sickness ZombieSickness;

		// Token: 0x04005B68 RID: 23400
		public Sickness Allergies;

		// Token: 0x04005B69 RID: 23401
		public Sickness RadiationSickness;

		// Token: 0x04005B6A RID: 23402
		public Sickness Sunburn;
	}
}
