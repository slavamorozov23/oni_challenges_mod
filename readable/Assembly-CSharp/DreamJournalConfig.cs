using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000271 RID: 625
public class DreamJournalConfig : IEntityConfig
{
	// Token: 0x06000CB2 RID: 3250 RVA: 0x0004C520 File Offset: 0x0004A720
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CB3 RID: 3251 RVA: 0x0004C522 File Offset: 0x0004A722
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000CB4 RID: 3252 RVA: 0x0004C524 File Offset: 0x0004A724
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dream_journal_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DreamJournalConfig.ID.Name, ITEMS.DREAMJOURNAL.NAME, ITEMS.DREAMJOURNAL.DESC, 1f, true, anim, "object", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.StoryTraitResource,
			GameTags.PedestalDisplayable
		});
		gameObject.AddOrGet<EntitySplitter>().maxStackSize = 25f;
		return gameObject;
	}

	// Token: 0x040008CA RID: 2250
	public static Tag ID = new Tag("DreamJournal");

	// Token: 0x040008CB RID: 2251
	public const float MASS = 1f;

	// Token: 0x040008CC RID: 2252
	public const int FABRICATION_TIME_SECONDS = 300;

	// Token: 0x040008CD RID: 2253
	private const string ANIM_FILE = "dream_journal_kanim";

	// Token: 0x040008CE RID: 2254
	private const string INITIAL_ANIM = "object";

	// Token: 0x040008CF RID: 2255
	public const int MAX_STACK_SIZE = 25;
}
