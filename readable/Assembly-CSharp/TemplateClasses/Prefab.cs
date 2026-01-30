using System;
using System.Collections.Generic;

namespace TemplateClasses
{
	// Token: 0x02000EFE RID: 3838
	[Serializable]
	public class Prefab
	{
		// Token: 0x06007B4D RID: 31565 RVA: 0x002FFEF7 File Offset: 0x002FE0F7
		public Prefab()
		{
			this.type = Prefab.Type.Other;
		}

		// Token: 0x06007B4E RID: 31566 RVA: 0x002FFF08 File Offset: 0x002FE108
		public Prefab(string _id, Prefab.Type _type, int loc_x, int loc_y, SimHashes _element, float _temperature = -1f, float _units = 1f, string _disease = null, int _disease_count = 0, Orientation _rotation = Orientation.Neutral, Prefab.template_amount_value[] _amount_values = null, Prefab.template_amount_value[] _other_values = null, int _connections = 0, string facadeIdId = null)
		{
			this.id = _id;
			this.type = _type;
			this.location_x = loc_x;
			this.location_y = loc_y;
			this.connections = _connections;
			this.element = _element;
			this.temperature = _temperature;
			this.units = _units;
			this.diseaseName = _disease;
			this.diseaseCount = _disease_count;
			this.facadeId = facadeIdId;
			this.rotationOrientation = _rotation;
			if (_amount_values != null && _amount_values.Length != 0)
			{
				this.amounts = _amount_values;
			}
			if (_other_values != null && _other_values.Length != 0)
			{
				this.other_values = _other_values;
			}
		}

		// Token: 0x06007B4F RID: 31567 RVA: 0x002FFF9C File Offset: 0x002FE19C
		public Prefab Clone(Vector2I offset)
		{
			Prefab prefab = new Prefab(this.id, this.type, offset.x + this.location_x, offset.y + this.location_y, this.element, this.temperature, this.units, this.diseaseName, this.diseaseCount, this.rotationOrientation, this.amounts, this.other_values, this.connections, this.facadeId);
			if (this.rottable != null)
			{
				prefab.rottable = new Rottable();
				prefab.rottable.rotAmount = this.rottable.rotAmount;
			}
			if (this.storage != null && this.storage.Count > 0)
			{
				prefab.storage = new List<StorageItem>();
				foreach (StorageItem storageItem in this.storage)
				{
					prefab.storage.Add(storageItem.Clone());
				}
			}
			return prefab;
		}

		// Token: 0x06007B50 RID: 31568 RVA: 0x003000AC File Offset: 0x002FE2AC
		public void AssignStorage(StorageItem _storage)
		{
			if (this.storage == null)
			{
				this.storage = new List<StorageItem>();
			}
			this.storage.Add(_storage);
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06007B51 RID: 31569 RVA: 0x003000CD File Offset: 0x002FE2CD
		// (set) Token: 0x06007B52 RID: 31570 RVA: 0x003000D5 File Offset: 0x002FE2D5
		public string id { get; set; }

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06007B53 RID: 31571 RVA: 0x003000DE File Offset: 0x002FE2DE
		// (set) Token: 0x06007B54 RID: 31572 RVA: 0x003000E6 File Offset: 0x002FE2E6
		public int location_x { get; set; }

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06007B55 RID: 31573 RVA: 0x003000EF File Offset: 0x002FE2EF
		// (set) Token: 0x06007B56 RID: 31574 RVA: 0x003000F7 File Offset: 0x002FE2F7
		public int location_y { get; set; }

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06007B57 RID: 31575 RVA: 0x00300100 File Offset: 0x002FE300
		// (set) Token: 0x06007B58 RID: 31576 RVA: 0x00300108 File Offset: 0x002FE308
		public SimHashes element { get; set; }

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06007B59 RID: 31577 RVA: 0x00300111 File Offset: 0x002FE311
		// (set) Token: 0x06007B5A RID: 31578 RVA: 0x00300119 File Offset: 0x002FE319
		public float temperature { get; set; }

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06007B5B RID: 31579 RVA: 0x00300122 File Offset: 0x002FE322
		// (set) Token: 0x06007B5C RID: 31580 RVA: 0x0030012A File Offset: 0x002FE32A
		public float units { get; set; }

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06007B5D RID: 31581 RVA: 0x00300133 File Offset: 0x002FE333
		// (set) Token: 0x06007B5E RID: 31582 RVA: 0x0030013B File Offset: 0x002FE33B
		public string diseaseName { get; set; }

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06007B5F RID: 31583 RVA: 0x00300144 File Offset: 0x002FE344
		// (set) Token: 0x06007B60 RID: 31584 RVA: 0x0030014C File Offset: 0x002FE34C
		public int diseaseCount { get; set; }

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06007B61 RID: 31585 RVA: 0x00300155 File Offset: 0x002FE355
		// (set) Token: 0x06007B62 RID: 31586 RVA: 0x0030015D File Offset: 0x002FE35D
		public Orientation rotationOrientation { get; set; }

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06007B63 RID: 31587 RVA: 0x00300166 File Offset: 0x002FE366
		// (set) Token: 0x06007B64 RID: 31588 RVA: 0x0030016E File Offset: 0x002FE36E
		public List<StorageItem> storage { get; set; }

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06007B65 RID: 31589 RVA: 0x00300177 File Offset: 0x002FE377
		// (set) Token: 0x06007B66 RID: 31590 RVA: 0x0030017F File Offset: 0x002FE37F
		public Prefab.Type type { get; set; }

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06007B67 RID: 31591 RVA: 0x00300188 File Offset: 0x002FE388
		// (set) Token: 0x06007B68 RID: 31592 RVA: 0x00300190 File Offset: 0x002FE390
		public string facadeId { get; set; }

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06007B69 RID: 31593 RVA: 0x00300199 File Offset: 0x002FE399
		// (set) Token: 0x06007B6A RID: 31594 RVA: 0x003001A1 File Offset: 0x002FE3A1
		public int connections { get; set; }

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06007B6B RID: 31595 RVA: 0x003001AA File Offset: 0x002FE3AA
		// (set) Token: 0x06007B6C RID: 31596 RVA: 0x003001B2 File Offset: 0x002FE3B2
		public Rottable rottable { get; set; }

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06007B6D RID: 31597 RVA: 0x003001BB File Offset: 0x002FE3BB
		// (set) Token: 0x06007B6E RID: 31598 RVA: 0x003001C3 File Offset: 0x002FE3C3
		public Prefab.template_amount_value[] amounts { get; set; }

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06007B6F RID: 31599 RVA: 0x003001CC File Offset: 0x002FE3CC
		// (set) Token: 0x06007B70 RID: 31600 RVA: 0x003001D4 File Offset: 0x002FE3D4
		public Prefab.template_amount_value[] other_values { get; set; }

		// Token: 0x02002181 RID: 8577
		public enum Type
		{
			// Token: 0x04009980 RID: 39296
			Building,
			// Token: 0x04009981 RID: 39297
			Ore,
			// Token: 0x04009982 RID: 39298
			Pickupable,
			// Token: 0x04009983 RID: 39299
			Other
		}

		// Token: 0x02002182 RID: 8578
		[Serializable]
		public class template_amount_value
		{
			// Token: 0x0600BC58 RID: 48216 RVA: 0x003FFF96 File Offset: 0x003FE196
			public template_amount_value()
			{
			}

			// Token: 0x0600BC59 RID: 48217 RVA: 0x003FFF9E File Offset: 0x003FE19E
			public template_amount_value(string id, float value)
			{
				this.id = id;
				this.value = value;
			}

			// Token: 0x17000D47 RID: 3399
			// (get) Token: 0x0600BC5A RID: 48218 RVA: 0x003FFFB4 File Offset: 0x003FE1B4
			// (set) Token: 0x0600BC5B RID: 48219 RVA: 0x003FFFBC File Offset: 0x003FE1BC
			public string id { get; set; }

			// Token: 0x17000D48 RID: 3400
			// (get) Token: 0x0600BC5C RID: 48220 RVA: 0x003FFFC5 File Offset: 0x003FE1C5
			// (set) Token: 0x0600BC5D RID: 48221 RVA: 0x003FFFCD File Offset: 0x003FE1CD
			public float value { get; set; }
		}
	}
}
