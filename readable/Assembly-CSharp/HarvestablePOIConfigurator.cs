using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B7D RID: 2941
[AddComponentMenu("KMonoBehaviour/scripts/HarvestablePOIConfigurator")]
public class HarvestablePOIConfigurator : KMonoBehaviour
{
	// Token: 0x060057B8 RID: 22456 RVA: 0x001FEA24 File Offset: 0x001FCC24
	public static HarvestablePOIConfigurator.HarvestablePOIType FindType(HashedString typeId)
	{
		HarvestablePOIConfigurator.HarvestablePOIType harvestablePOIType = null;
		if (typeId != HashedString.Invalid)
		{
			harvestablePOIType = HarvestablePOIConfigurator._poiTypes.Find((HarvestablePOIConfigurator.HarvestablePOIType t) => t.id == typeId);
		}
		if (harvestablePOIType == null)
		{
			global::Debug.LogError(string.Format("Tried finding a harvestable poi with id {0} but it doesn't exist!", typeId.ToString()));
		}
		return harvestablePOIType;
	}

	// Token: 0x060057B9 RID: 22457 RVA: 0x001FEA8D File Offset: 0x001FCC8D
	public HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration MakeConfiguration()
	{
		return this.CreateRandomInstance(this.presetType, this.presetMin, this.presetMax);
	}

	// Token: 0x060057BA RID: 22458 RVA: 0x001FEAA8 File Offset: 0x001FCCA8
	private HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration CreateRandomInstance(HashedString typeId, float min, float max)
	{
		int globalWorldSeed = SaveLoader.Instance.clusterDetailSave.globalWorldSeed;
		ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
		Vector3 position = ClusterGrid.Instance.GetPosition(component);
		KRandom randomSource = new KRandom(globalWorldSeed + (int)position.x + (int)position.y);
		return new HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration
		{
			typeId = typeId,
			capacityRoll = this.Roll(randomSource, min, max),
			rechargeRoll = this.Roll(randomSource, min, max)
		};
	}

	// Token: 0x060057BB RID: 22459 RVA: 0x001FEB17 File Offset: 0x001FCD17
	private float Roll(KRandom randomSource, float min, float max)
	{
		return (float)(randomSource.NextDouble() * (double)(max - min)) + min;
	}

	// Token: 0x04003AD2 RID: 15058
	private static List<HarvestablePOIConfigurator.HarvestablePOIType> _poiTypes;

	// Token: 0x04003AD3 RID: 15059
	public HashedString presetType;

	// Token: 0x04003AD4 RID: 15060
	public float presetMin;

	// Token: 0x04003AD5 RID: 15061
	public float presetMax = 1f;

	// Token: 0x02001CFB RID: 7419
	public class HarvestablePOIType : IHasDlcRestrictions
	{
		// Token: 0x0600AF70 RID: 44912 RVA: 0x003D61D9 File Offset: 0x003D43D9
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x0600AF71 RID: 44913 RVA: 0x003D61E1 File Offset: 0x003D43E1
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x0600AF72 RID: 44914 RVA: 0x003D61EC File Offset: 0x003D43EC
		public HarvestablePOIType(string id, Dictionary<SimHashes, float> harvestableElements, float poiCapacityMin = 54000f, float poiCapacityMax = 81000f, float poiRechargeMin = 30000f, float poiRechargeMax = 60000f, bool canProvideArtifacts = true, List<string> orbitalObject = null, int maxNumOrbitingObjects = 20, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null) : this(id, harvestableElements, 0, null, poiCapacityMin, poiCapacityMax, poiRechargeMin, poiRechargeMax, canProvideArtifacts, orbitalObject, maxNumOrbitingObjects, requiredDlcIds, forbiddenDlcIds)
		{
		}

		// Token: 0x0600AF73 RID: 44915 RVA: 0x003D6214 File Offset: 0x003D4414
		public HarvestablePOIType(string id, Dictionary<SimHashes, float> harvestableElements, int initialDatabanks, Dictionary<SimHashes, float> initialLiberatedResources = null, float poiCapacityMin = 54000f, float poiCapacityMax = 81000f, float poiRechargeMin = 30000f, float poiRechargeMax = 60000f, bool canProvideArtifacts = true, List<string> orbitalObject = null, int maxNumOrbitingObjects = 20, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
		{
			this.id = id;
			this.idHash = id;
			this.harvestableElements = harvestableElements;
			this.initialDataBanks = initialDatabanks;
			this.initialLiberatedResources = initialLiberatedResources;
			this.poiCapacityMin = poiCapacityMin;
			this.poiCapacityMax = poiCapacityMax;
			this.poiRechargeMin = poiRechargeMin;
			this.poiRechargeMax = poiRechargeMax;
			this.canProvideArtifacts = canProvideArtifacts;
			this.orbitalObject = orbitalObject;
			this.maxNumOrbitingObjects = maxNumOrbitingObjects;
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
			if (HarvestablePOIConfigurator._poiTypes == null)
			{
				HarvestablePOIConfigurator._poiTypes = new List<HarvestablePOIConfigurator.HarvestablePOIType>();
			}
			HarvestablePOIConfigurator._poiTypes.Add(this);
		}

		// Token: 0x0600AF74 RID: 44916 RVA: 0x003D62B4 File Offset: 0x003D44B4
		[Obsolete]
		public HarvestablePOIType(string id, Dictionary<SimHashes, float> harvestableElements, float poiCapacityMin = 54000f, float poiCapacityMax = 81000f, float poiRechargeMin = 30000f, float poiRechargeMax = 60000f, bool canProvideArtifacts = true, List<string> orbitalObject = null, int maxNumOrbitingObjects = 20, string dlcID = "EXPANSION1_ID") : this(id, harvestableElements, poiCapacityMin, poiCapacityMax, poiRechargeMin, poiRechargeMax, canProvideArtifacts, orbitalObject, maxNumOrbitingObjects, null, null)
		{
			this.requiredDlcIds = DlcManager.EXPANSION1;
		}

		// Token: 0x040089E1 RID: 35297
		public string id;

		// Token: 0x040089E2 RID: 35298
		public HashedString idHash;

		// Token: 0x040089E3 RID: 35299
		public Dictionary<SimHashes, float> harvestableElements;

		// Token: 0x040089E4 RID: 35300
		public float poiCapacityMin;

		// Token: 0x040089E5 RID: 35301
		public float poiCapacityMax;

		// Token: 0x040089E6 RID: 35302
		public float poiRechargeMin;

		// Token: 0x040089E7 RID: 35303
		public float poiRechargeMax;

		// Token: 0x040089E8 RID: 35304
		public int initialDataBanks;

		// Token: 0x040089E9 RID: 35305
		public Dictionary<SimHashes, float> initialLiberatedResources;

		// Token: 0x040089EA RID: 35306
		public bool canProvideArtifacts;

		// Token: 0x040089EB RID: 35307
		[Obsolete]
		public string dlcID;

		// Token: 0x040089EC RID: 35308
		public string[] requiredDlcIds;

		// Token: 0x040089ED RID: 35309
		public string[] forbiddenDlcIds;

		// Token: 0x040089EE RID: 35310
		public List<string> orbitalObject;

		// Token: 0x040089EF RID: 35311
		public int maxNumOrbitingObjects;
	}

	// Token: 0x02001CFC RID: 7420
	[Serializable]
	public class HarvestablePOIInstanceConfiguration
	{
		// Token: 0x0600AF75 RID: 44917 RVA: 0x003D62E4 File Offset: 0x003D44E4
		private void Init()
		{
			if (this.didInit)
			{
				return;
			}
			this.didInit = true;
			this.poiTotalCapacity = MathUtil.ReRange(this.capacityRoll, 0f, 1f, this.poiType.poiCapacityMin, this.poiType.poiCapacityMax);
			this.poiRecharge = MathUtil.ReRange(this.rechargeRoll, 0f, 1f, this.poiType.poiRechargeMin, this.poiType.poiRechargeMax);
		}

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x0600AF76 RID: 44918 RVA: 0x003D6363 File Offset: 0x003D4563
		public HarvestablePOIConfigurator.HarvestablePOIType poiType
		{
			get
			{
				return HarvestablePOIConfigurator.FindType(this.typeId);
			}
		}

		// Token: 0x0600AF77 RID: 44919 RVA: 0x003D6370 File Offset: 0x003D4570
		public Dictionary<SimHashes, float> GetElementsWithWeights()
		{
			this.Init();
			return this.poiType.harvestableElements;
		}

		// Token: 0x0600AF78 RID: 44920 RVA: 0x003D6383 File Offset: 0x003D4583
		public bool CanProvideArtifacts()
		{
			this.Init();
			return this.poiType.canProvideArtifacts;
		}

		// Token: 0x0600AF79 RID: 44921 RVA: 0x003D6396 File Offset: 0x003D4596
		public float GetMaxCapacity()
		{
			this.Init();
			return this.poiTotalCapacity;
		}

		// Token: 0x0600AF7A RID: 44922 RVA: 0x003D63A4 File Offset: 0x003D45A4
		public float GetRechargeTime()
		{
			this.Init();
			return this.poiRecharge;
		}

		// Token: 0x040089F0 RID: 35312
		public HashedString typeId;

		// Token: 0x040089F1 RID: 35313
		private bool didInit;

		// Token: 0x040089F2 RID: 35314
		public float capacityRoll;

		// Token: 0x040089F3 RID: 35315
		public float rechargeRoll;

		// Token: 0x040089F4 RID: 35316
		private float poiTotalCapacity;

		// Token: 0x040089F5 RID: 35317
		private float poiRecharge;
	}
}
