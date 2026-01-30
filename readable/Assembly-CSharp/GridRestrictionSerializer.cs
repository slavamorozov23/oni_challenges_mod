using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x0200097E RID: 2430
[SerializationConfig(MemberSerialization.OptIn)]
public class GridRestrictionSerializer : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x060045AB RID: 17835 RVA: 0x0019304C File Offset: 0x0019124C
	public static void DestroyInstance()
	{
		GridRestrictionSerializer.Instance = null;
	}

	// Token: 0x060045AC RID: 17836 RVA: 0x00193054 File Offset: 0x00191254
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GridRestrictionSerializer.Instance = this;
	}

	// Token: 0x060045AD RID: 17837 RVA: 0x00193064 File Offset: 0x00191264
	public int GetTagId(Tag gameTag)
	{
		foreach (KeyValuePair<Tag, int> keyValuePair in this.tagToId)
		{
			if (keyValuePair.Key == gameTag)
			{
				return keyValuePair.Value;
			}
		}
		DebugUtil.DevAssert(false, "Gametag " + gameTag.Name + " has not been added to the valid list of GridRestrictionTagId's before requesting the ID", null);
		return 0;
	}

	// Token: 0x170004E9 RID: 1257
	// (get) Token: 0x060045AE RID: 17838 RVA: 0x001930EC File Offset: 0x001912EC
	public Tag[] ValidRobotTypes
	{
		get
		{
			return this.robotTypeTags;
		}
	}

	// Token: 0x04002EF5 RID: 12021
	public static GridRestrictionSerializer Instance;

	// Token: 0x04002EF6 RID: 12022
	private List<KeyValuePair<Tag, int>> tagToId = new List<KeyValuePair<Tag, int>>
	{
		new KeyValuePair<Tag, int>(GameTags.Minions.Models.Standard, -1),
		new KeyValuePair<Tag, int>(GameTags.Minions.Models.Bionic, -2),
		new KeyValuePair<Tag, int>(GameTags.Robot, -3),
		new KeyValuePair<Tag, int>(GameTags.Robots.Models.FetchDrone, -4),
		new KeyValuePair<Tag, int>(GameTags.Robots.Models.ScoutRover, -5),
		new KeyValuePair<Tag, int>(GameTags.Robots.Models.MorbRover, -6)
	};

	// Token: 0x04002EF7 RID: 12023
	private Tag[] robotTypeTags = new Tag[]
	{
		GameTags.Robots.Models.FetchDrone,
		GameTags.Robots.Models.ScoutRover,
		GameTags.Robots.Models.MorbRover
	};
}
