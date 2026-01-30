using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B63 RID: 2915
[AddComponentMenu("KMonoBehaviour/scripts/ArtifactPOIConfigurator")]
public class ArtifactPOIConfigurator : KMonoBehaviour
{
	// Token: 0x0600563B RID: 22075 RVA: 0x001F6D1C File Offset: 0x001F4F1C
	public static ArtifactPOIConfigurator.ArtifactPOIType FindType(HashedString typeId)
	{
		ArtifactPOIConfigurator.ArtifactPOIType artifactPOIType = null;
		if (typeId != HashedString.Invalid)
		{
			artifactPOIType = ArtifactPOIConfigurator._poiTypes.Find((ArtifactPOIConfigurator.ArtifactPOIType t) => t.id == typeId);
		}
		if (artifactPOIType == null)
		{
			global::Debug.LogError(string.Format("Tried finding a harvestable poi with id {0} but it doesn't exist!", typeId.ToString()));
		}
		return artifactPOIType;
	}

	// Token: 0x0600563C RID: 22076 RVA: 0x001F6D85 File Offset: 0x001F4F85
	public ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration MakeConfiguration()
	{
		return this.CreateRandomInstance(this.presetType, this.presetMin, this.presetMax);
	}

	// Token: 0x0600563D RID: 22077 RVA: 0x001F6DA0 File Offset: 0x001F4FA0
	private ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration CreateRandomInstance(HashedString typeId, float min, float max)
	{
		int globalWorldSeed = SaveLoader.Instance.clusterDetailSave.globalWorldSeed;
		ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
		Vector3 position = ClusterGrid.Instance.GetPosition(component);
		KRandom randomSource = new KRandom(globalWorldSeed + (int)position.x + (int)position.y);
		return new ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration
		{
			typeId = typeId,
			rechargeRoll = this.Roll(randomSource, min, max)
		};
	}

	// Token: 0x0600563E RID: 22078 RVA: 0x001F6E00 File Offset: 0x001F5000
	private float Roll(KRandom randomSource, float min, float max)
	{
		return (float)(randomSource.NextDouble() * (double)(max - min)) + min;
	}

	// Token: 0x04003A3D RID: 14909
	private static List<ArtifactPOIConfigurator.ArtifactPOIType> _poiTypes;

	// Token: 0x04003A3E RID: 14910
	public static ArtifactPOIConfigurator.ArtifactPOIType defaultArtifactPoiType = new ArtifactPOIConfigurator.ArtifactPOIType("HarvestablePOIArtifacts", null, false, 30000f, 60000f, DlcManager.EXPANSION1, null);

	// Token: 0x04003A3F RID: 14911
	public HashedString presetType;

	// Token: 0x04003A40 RID: 14912
	public float presetMin;

	// Token: 0x04003A41 RID: 14913
	public float presetMax = 1f;

	// Token: 0x02001CD2 RID: 7378
	public class ArtifactPOIType : IHasDlcRestrictions
	{
		// Token: 0x0600AEC1 RID: 44737 RVA: 0x003D4854 File Offset: 0x003D2A54
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x0600AEC2 RID: 44738 RVA: 0x003D485C File Offset: 0x003D2A5C
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x0600AEC3 RID: 44739 RVA: 0x003D4864 File Offset: 0x003D2A64
		public ArtifactPOIType(string id, string harvestableArtifactID = null, bool destroyOnHarvest = false, float poiRechargeTimeMin = 30000f, float poiRechargeTimeMax = 60000f, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null) : this(id, 0, harvestableArtifactID, destroyOnHarvest, poiRechargeTimeMin, poiRechargeTimeMax, requiredDlcIds, forbiddenDlcIds)
		{
		}

		// Token: 0x0600AEC4 RID: 44740 RVA: 0x003D4884 File Offset: 0x003D2A84
		public ArtifactPOIType(string id, int databankCount, string harvestableArtifactID = null, bool destroyOnHarvest = false, float poiRechargeTimeMin = 30000f, float poiRechargeTimeMax = 60000f, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
		{
			this.orbitalObject = new List<string>
			{
				Db.Get().OrbitalTypeCategories.gravitas.Id
			};
			base..ctor();
			this.id = id;
			this.idHash = id;
			this.initialDatabankCount = databankCount;
			this.harvestableArtifactID = harvestableArtifactID;
			this.destroyOnHarvest = destroyOnHarvest;
			this.poiRechargeTimeMin = poiRechargeTimeMin;
			this.poiRechargeTimeMax = poiRechargeTimeMax;
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
			if (ArtifactPOIConfigurator._poiTypes == null)
			{
				ArtifactPOIConfigurator._poiTypes = new List<ArtifactPOIConfigurator.ArtifactPOIType>();
			}
			ArtifactPOIConfigurator._poiTypes.Add(this);
		}

		// Token: 0x0600AEC5 RID: 44741 RVA: 0x003D4924 File Offset: 0x003D2B24
		[Obsolete]
		public ArtifactPOIType(string id, string harvestableArtifactID = null, bool destroyOnHarvest = false, float poiRechargeTimeMin = 30000f, float poiRechargeTimeMax = 60000f, string dlcID = "EXPANSION1_ID")
		{
			this.orbitalObject = new List<string>
			{
				Db.Get().OrbitalTypeCategories.gravitas.Id
			};
			base..ctor();
			this.id = id;
			this.idHash = id;
			this.harvestableArtifactID = harvestableArtifactID;
			this.destroyOnHarvest = destroyOnHarvest;
			this.poiRechargeTimeMin = poiRechargeTimeMin;
			this.poiRechargeTimeMax = poiRechargeTimeMax;
			this.dlcID = dlcID;
			if (ArtifactPOIConfigurator._poiTypes == null)
			{
				ArtifactPOIConfigurator._poiTypes = new List<ArtifactPOIConfigurator.ArtifactPOIType>();
			}
			ArtifactPOIConfigurator._poiTypes.Add(this);
		}

		// Token: 0x04008949 RID: 35145
		public string id;

		// Token: 0x0400894A RID: 35146
		public HashedString idHash;

		// Token: 0x0400894B RID: 35147
		public string harvestableArtifactID;

		// Token: 0x0400894C RID: 35148
		public bool destroyOnHarvest;

		// Token: 0x0400894D RID: 35149
		public float poiRechargeTimeMin;

		// Token: 0x0400894E RID: 35150
		public float poiRechargeTimeMax;

		// Token: 0x0400894F RID: 35151
		[Obsolete]
		public string dlcID;

		// Token: 0x04008950 RID: 35152
		public int initialDatabankCount;

		// Token: 0x04008951 RID: 35153
		public string[] requiredDlcIds;

		// Token: 0x04008952 RID: 35154
		public string[] forbiddenDlcIds;

		// Token: 0x04008953 RID: 35155
		public List<string> orbitalObject;
	}

	// Token: 0x02001CD3 RID: 7379
	[Serializable]
	public class ArtifactPOIInstanceConfiguration
	{
		// Token: 0x0600AEC6 RID: 44742 RVA: 0x003D49B4 File Offset: 0x003D2BB4
		private void Init()
		{
			if (this.didInit)
			{
				return;
			}
			this.didInit = true;
			this.poiRechargeTime = MathUtil.ReRange(this.rechargeRoll, 0f, 1f, this.poiType.poiRechargeTimeMin, this.poiType.poiRechargeTimeMax);
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x0600AEC7 RID: 44743 RVA: 0x003D4A02 File Offset: 0x003D2C02
		public ArtifactPOIConfigurator.ArtifactPOIType poiType
		{
			get
			{
				return ArtifactPOIConfigurator.FindType(this.typeId);
			}
		}

		// Token: 0x0600AEC8 RID: 44744 RVA: 0x003D4A0F File Offset: 0x003D2C0F
		public bool DestroyOnHarvest()
		{
			this.Init();
			return this.poiType.destroyOnHarvest;
		}

		// Token: 0x0600AEC9 RID: 44745 RVA: 0x003D4A22 File Offset: 0x003D2C22
		public string GetArtifactID()
		{
			this.Init();
			return this.poiType.harvestableArtifactID;
		}

		// Token: 0x0600AECA RID: 44746 RVA: 0x003D4A35 File Offset: 0x003D2C35
		public float GetRechargeTime()
		{
			this.Init();
			return this.poiRechargeTime;
		}

		// Token: 0x04008954 RID: 35156
		public HashedString typeId;

		// Token: 0x04008955 RID: 35157
		private bool didInit;

		// Token: 0x04008956 RID: 35158
		public float rechargeRoll;

		// Token: 0x04008957 RID: 35159
		private float poiRechargeTime;
	}
}
