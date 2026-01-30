using System;
using UnityEngine;

// Token: 0x02000A96 RID: 2710
public class PlantFiberProducer : KMonoBehaviour
{
	// Token: 0x06004EA5 RID: 20133 RVA: 0x001C99BD File Offset: 0x001C7BBD
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe(1272413801, new Action<object>(this.OnHarvest));
	}

	// Token: 0x06004EA6 RID: 20134 RVA: 0x001C99DD File Offset: 0x001C7BDD
	protected override void OnCleanUp()
	{
		base.Unsubscribe(1272413801, new Action<object>(this.OnHarvest));
	}

	// Token: 0x06004EA7 RID: 20135 RVA: 0x001C99F8 File Offset: 0x001C7BF8
	private void OnHarvest(object obj)
	{
		Harvestable harvestable = (Harvestable)obj;
		if (harvestable != null && harvestable.completed_by != null && harvestable.completed_by.GetComponent<MinionResume>().HasPerk(Db.Get().SkillPerks.CanSalvagePlantFiber))
		{
			this.SpawnPlantFiber();
		}
	}

	// Token: 0x06004EA8 RID: 20136 RVA: 0x001C9A4C File Offset: 0x001C7C4C
	private GameObject SpawnPlantFiber()
	{
		Vector3 position = base.gameObject.transform.GetPosition() + new Vector3(0f, 0.5f, 0f);
		GameObject prefab = Assets.GetPrefab(new Tag("PlantFiber"));
		GameObject gameObject = GameUtil.KInstantiate(prefab, position, Grid.SceneLayer.Ore, null, 0);
		PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
		PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
		component2.Temperature = component.Temperature;
		component2.Mass = this.amount;
		gameObject.SetActive(true);
		string properName = gameObject.GetProperName();
		PopFXManager.Instance.SpawnFX(Def.GetUISprite(prefab, "ui", false).first, PopFXManager.Instance.sprite_Plus, properName, gameObject.transform, Vector3.zero, 1.5f, true, false, false);
		return gameObject;
	}

	// Token: 0x04003479 RID: 13433
	public float amount;
}
