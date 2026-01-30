using System;
using UnityEngine;

// Token: 0x02000333 RID: 819
public class OneshotReactableLocator : IEntityConfig
{
	// Token: 0x060010EC RID: 4332 RVA: 0x00064FF4 File Offset: 0x000631F4
	public static EmoteReactable CreateOneshotReactable(GameObject source, float lifetime, string id, ChoreType chore_type, int range_width = 15, int range_height = 15, float min_reactor_time = 20f)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(OneshotReactableLocator.ID), source.transform.GetPosition());
		EmoteReactable emoteReactable = new EmoteReactable(gameObject, id, chore_type, range_width, range_height, 100000f, min_reactor_time, float.PositiveInfinity, 0f);
		emoteReactable.AddPrecondition(OneshotReactableLocator.ReactorIsNotSource(source));
		OneshotReactableHost component = gameObject.GetComponent<OneshotReactableHost>();
		component.lifetime = lifetime;
		component.SetReactable(emoteReactable);
		gameObject.SetActive(true);
		return emoteReactable;
	}

	// Token: 0x060010ED RID: 4333 RVA: 0x0006506A File Offset: 0x0006326A
	private static Reactable.ReactablePrecondition ReactorIsNotSource(GameObject source)
	{
		return (GameObject reactor, Navigator.ActiveTransition transition) => reactor != source;
	}

	// Token: 0x060010EE RID: 4334 RVA: 0x00065083 File Offset: 0x00063283
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(OneshotReactableLocator.ID, OneshotReactableLocator.ID, false);
		gameObject.AddTag(GameTags.NotConversationTopic);
		gameObject.AddOrGet<OneshotReactableHost>();
		return gameObject;
	}

	// Token: 0x060010EF RID: 4335 RVA: 0x000650A7 File Offset: 0x000632A7
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060010F0 RID: 4336 RVA: 0x000650A9 File Offset: 0x000632A9
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000AC9 RID: 2761
	public static readonly string ID = "OneshotReactableLocator";
}
