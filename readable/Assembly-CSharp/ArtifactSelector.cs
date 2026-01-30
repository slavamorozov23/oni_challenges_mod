using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020006D7 RID: 1751
public class ArtifactSelector : KMonoBehaviour
{
	// Token: 0x17000206 RID: 518
	// (get) Token: 0x06002AE1 RID: 10977 RVA: 0x000FAF65 File Offset: 0x000F9165
	public int AnalyzedArtifactCount
	{
		get
		{
			return this.analyzedArtifactCount;
		}
	}

	// Token: 0x17000207 RID: 519
	// (get) Token: 0x06002AE2 RID: 10978 RVA: 0x000FAF6D File Offset: 0x000F916D
	public int AnalyzedSpaceArtifactCount
	{
		get
		{
			return this.analyzedSpaceArtifactCount;
		}
	}

	// Token: 0x06002AE3 RID: 10979 RVA: 0x000FAF75 File Offset: 0x000F9175
	public List<string> GetAnalyzedArtifactIDs()
	{
		return this.analyzedArtifatIDs;
	}

	// Token: 0x06002AE4 RID: 10980 RVA: 0x000FAF80 File Offset: 0x000F9180
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ArtifactSelector.Instance = this;
		this.placedArtifacts.Add(ArtifactType.Terrestrial, new List<string>());
		this.placedArtifacts.Add(ArtifactType.Space, new List<string>());
		this.placedArtifacts.Add(ArtifactType.Any, new List<string>());
	}

	// Token: 0x06002AE5 RID: 10981 RVA: 0x000FAFCC File Offset: 0x000F91CC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int num = 0;
		int num2 = 0;
		foreach (string artifactID in this.analyzedArtifatIDs)
		{
			ArtifactType artifactType = this.GetArtifactType(artifactID);
			if (artifactType != ArtifactType.Space)
			{
				if (artifactType == ArtifactType.Terrestrial)
				{
					num++;
				}
			}
			else
			{
				num2++;
			}
		}
		if (num > this.analyzedArtifactCount)
		{
			this.analyzedArtifactCount = num;
		}
		if (num2 > this.analyzedSpaceArtifactCount)
		{
			this.analyzedSpaceArtifactCount = num2;
		}
	}

	// Token: 0x06002AE6 RID: 10982 RVA: 0x000FB060 File Offset: 0x000F9260
	public bool RecordArtifactAnalyzed(string id)
	{
		if (this.analyzedArtifatIDs.Contains(id))
		{
			return false;
		}
		this.analyzedArtifatIDs.Add(id);
		return true;
	}

	// Token: 0x06002AE7 RID: 10983 RVA: 0x000FB07F File Offset: 0x000F927F
	public void IncrementAnalyzedTerrestrialArtifacts()
	{
		this.analyzedArtifactCount++;
	}

	// Token: 0x06002AE8 RID: 10984 RVA: 0x000FB08F File Offset: 0x000F928F
	public void IncrementAnalyzedSpaceArtifacts()
	{
		this.analyzedSpaceArtifactCount++;
	}

	// Token: 0x06002AE9 RID: 10985 RVA: 0x000FB0A0 File Offset: 0x000F92A0
	public string GetUniqueArtifactID(ArtifactType artifactType = ArtifactType.Any)
	{
		List<string> list = new List<string>();
		foreach (string text in ArtifactConfig.artifactItems[artifactType])
		{
			if (!this.placedArtifacts[artifactType].Contains(text) && Game.IsCorrectDlcActiveForCurrentSave(Assets.GetPrefab(text.ToTag()).GetComponent<KPrefabID>()))
			{
				list.Add(text);
			}
		}
		string text2 = "artifact_officemug";
		if (list.Count == 0 && artifactType != ArtifactType.Any)
		{
			foreach (string text3 in ArtifactConfig.artifactItems[ArtifactType.Any])
			{
				if (!this.placedArtifacts[ArtifactType.Any].Contains(text3) && Game.IsCorrectDlcActiveForCurrentSave(Assets.GetPrefab(text3.ToTag()).GetComponent<KPrefabID>()))
				{
					list.Add(text3);
					artifactType = ArtifactType.Any;
				}
			}
		}
		if (list.Count != 0)
		{
			text2 = list[UnityEngine.Random.Range(0, list.Count)];
		}
		this.placedArtifacts[artifactType].Add(text2);
		return text2;
	}

	// Token: 0x06002AEA RID: 10986 RVA: 0x000FB1E4 File Offset: 0x000F93E4
	public void ReserveArtifactID(string artifactID, ArtifactType artifactType = ArtifactType.Any)
	{
		if (this.placedArtifacts[artifactType].Contains(artifactID))
		{
			DebugUtil.Assert(true, string.Format("Tried to add {0} to placedArtifacts but it already exists in the list!", artifactID));
		}
		this.placedArtifacts[artifactType].Add(artifactID);
	}

	// Token: 0x06002AEB RID: 10987 RVA: 0x000FB21D File Offset: 0x000F941D
	public ArtifactType GetArtifactType(string artifactID)
	{
		if (this.placedArtifacts[ArtifactType.Terrestrial].Contains(artifactID))
		{
			return ArtifactType.Terrestrial;
		}
		if (this.placedArtifacts[ArtifactType.Space].Contains(artifactID))
		{
			return ArtifactType.Space;
		}
		return ArtifactType.Any;
	}

	// Token: 0x04001990 RID: 6544
	public static ArtifactSelector Instance;

	// Token: 0x04001991 RID: 6545
	[Serialize]
	private Dictionary<ArtifactType, List<string>> placedArtifacts = new Dictionary<ArtifactType, List<string>>();

	// Token: 0x04001992 RID: 6546
	[Serialize]
	private int analyzedArtifactCount;

	// Token: 0x04001993 RID: 6547
	[Serialize]
	private int analyzedSpaceArtifactCount;

	// Token: 0x04001994 RID: 6548
	[Serialize]
	private List<string> analyzedArtifatIDs = new List<string>();

	// Token: 0x04001995 RID: 6549
	private const string DEFAULT_ARTIFACT_ID = "artifact_officemug";
}
