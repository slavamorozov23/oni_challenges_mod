using System;

namespace TemplateClasses
{
	// Token: 0x02000F00 RID: 3840
	[Serializable]
	public class StorageItem
	{
		// Token: 0x06007B83 RID: 31619 RVA: 0x003002C0 File Offset: 0x002FE4C0
		public StorageItem()
		{
			this.rottable = new Rottable();
		}

		// Token: 0x06007B84 RID: 31620 RVA: 0x003002D4 File Offset: 0x002FE4D4
		public StorageItem(string _id, float _units, float _temp, SimHashes _element, string _disease, int _disease_count, bool _isOre)
		{
			this.rottable = new Rottable();
			this.id = _id;
			this.element = _element;
			this.units = _units;
			this.diseaseName = _disease;
			this.diseaseCount = _disease_count;
			this.isOre = _isOre;
			this.temperature = _temp;
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06007B85 RID: 31621 RVA: 0x00300327 File Offset: 0x002FE527
		// (set) Token: 0x06007B86 RID: 31622 RVA: 0x0030032F File Offset: 0x002FE52F
		public string id { get; set; }

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06007B87 RID: 31623 RVA: 0x00300338 File Offset: 0x002FE538
		// (set) Token: 0x06007B88 RID: 31624 RVA: 0x00300340 File Offset: 0x002FE540
		public SimHashes element { get; set; }

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06007B89 RID: 31625 RVA: 0x00300349 File Offset: 0x002FE549
		// (set) Token: 0x06007B8A RID: 31626 RVA: 0x00300351 File Offset: 0x002FE551
		public float units { get; set; }

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06007B8B RID: 31627 RVA: 0x0030035A File Offset: 0x002FE55A
		// (set) Token: 0x06007B8C RID: 31628 RVA: 0x00300362 File Offset: 0x002FE562
		public bool isOre { get; set; }

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06007B8D RID: 31629 RVA: 0x0030036B File Offset: 0x002FE56B
		// (set) Token: 0x06007B8E RID: 31630 RVA: 0x00300373 File Offset: 0x002FE573
		public float temperature { get; set; }

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06007B8F RID: 31631 RVA: 0x0030037C File Offset: 0x002FE57C
		// (set) Token: 0x06007B90 RID: 31632 RVA: 0x00300384 File Offset: 0x002FE584
		public string diseaseName { get; set; }

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06007B91 RID: 31633 RVA: 0x0030038D File Offset: 0x002FE58D
		// (set) Token: 0x06007B92 RID: 31634 RVA: 0x00300395 File Offset: 0x002FE595
		public int diseaseCount { get; set; }

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06007B93 RID: 31635 RVA: 0x0030039E File Offset: 0x002FE59E
		// (set) Token: 0x06007B94 RID: 31636 RVA: 0x003003A6 File Offset: 0x002FE5A6
		public Rottable rottable { get; set; }

		// Token: 0x06007B95 RID: 31637 RVA: 0x003003B0 File Offset: 0x002FE5B0
		public StorageItem Clone()
		{
			return new StorageItem(this.id, this.units, this.temperature, this.element, this.diseaseName, this.diseaseCount, this.isOre)
			{
				rottable = 
				{
					rotAmount = this.rottable.rotAmount
				}
			};
		}
	}
}
