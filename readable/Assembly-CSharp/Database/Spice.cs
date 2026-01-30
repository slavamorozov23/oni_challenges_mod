using System;
using Klei.AI;
using UnityEngine;

namespace Database
{
	// Token: 0x02000F5A RID: 3930
	public class Spice : Resource, IHasDlcRestrictions
	{
		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06007CD1 RID: 31953 RVA: 0x00318E57 File Offset: 0x00317057
		// (set) Token: 0x06007CD2 RID: 31954 RVA: 0x00318E5F File Offset: 0x0031705F
		public AttributeModifier StatBonus { get; private set; }

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06007CD3 RID: 31955 RVA: 0x00318E68 File Offset: 0x00317068
		// (set) Token: 0x06007CD4 RID: 31956 RVA: 0x00318E70 File Offset: 0x00317070
		public AttributeModifier FoodModifier { get; private set; }

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06007CD5 RID: 31957 RVA: 0x00318E79 File Offset: 0x00317079
		// (set) Token: 0x06007CD6 RID: 31958 RVA: 0x00318E81 File Offset: 0x00317081
		public AttributeModifier CalorieModifier { get; private set; }

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06007CD7 RID: 31959 RVA: 0x00318E8A File Offset: 0x0031708A
		// (set) Token: 0x06007CD8 RID: 31960 RVA: 0x00318E92 File Offset: 0x00317092
		public Color PrimaryColor { get; private set; }

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06007CD9 RID: 31961 RVA: 0x00318E9B File Offset: 0x0031709B
		// (set) Token: 0x06007CDA RID: 31962 RVA: 0x00318EA3 File Offset: 0x003170A3
		public Color SecondaryColor { get; private set; }

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06007CDC RID: 31964 RVA: 0x00318EB5 File Offset: 0x003170B5
		// (set) Token: 0x06007CDB RID: 31963 RVA: 0x00318EAC File Offset: 0x003170AC
		public string Image { get; private set; }

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06007CDE RID: 31966 RVA: 0x00318EC6 File Offset: 0x003170C6
		// (set) Token: 0x06007CDD RID: 31965 RVA: 0x00318EBD File Offset: 0x003170BD
		public string[] requiredDlcIds { get; private set; }

		// Token: 0x06007CDF RID: 31967 RVA: 0x00318ED0 File Offset: 0x003170D0
		public Spice(ResourceSet parent, string id, Spice.Ingredient[] ingredients, Color primaryColor, Color secondaryColor, AttributeModifier foodMod = null, AttributeModifier statBonus = null, string imageName = "unknown", string[] dlcID = null) : base(id, parent, null)
		{
			this.requiredDlcIds = this.requiredDlcIds;
			this.StatBonus = statBonus;
			this.FoodModifier = foodMod;
			this.Ingredients = ingredients;
			this.Image = imageName;
			this.PrimaryColor = primaryColor;
			this.SecondaryColor = secondaryColor;
			for (int i = 0; i < this.Ingredients.Length; i++)
			{
				this.TotalKG += this.Ingredients[i].AmountKG;
			}
		}

		// Token: 0x06007CE0 RID: 31968 RVA: 0x00318F4E File Offset: 0x0031714E
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x06007CE1 RID: 31969 RVA: 0x00318F56 File Offset: 0x00317156
		public string[] GetForbiddenDlcIds()
		{
			return null;
		}

		// Token: 0x04005B99 RID: 23449
		public readonly Spice.Ingredient[] Ingredients;

		// Token: 0x04005B9A RID: 23450
		public readonly float TotalKG;

		// Token: 0x020021AC RID: 8620
		public class Ingredient : IConfigurableConsumerIngredient
		{
			// Token: 0x0600BDD5 RID: 48597 RVA: 0x004065BB File Offset: 0x004047BB
			public float GetAmount()
			{
				return this.AmountKG;
			}

			// Token: 0x0600BDD6 RID: 48598 RVA: 0x004065C3 File Offset: 0x004047C3
			public Tag[] GetIDSets()
			{
				return this.IngredientSet;
			}

			// Token: 0x04009B18 RID: 39704
			public Tag[] IngredientSet;

			// Token: 0x04009B19 RID: 39705
			public float AmountKG;
		}
	}
}
