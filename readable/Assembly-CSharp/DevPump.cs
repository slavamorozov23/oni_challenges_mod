using System;
using UnityEngine;

// Token: 0x02000744 RID: 1860
public class DevPump : Filterable, ISim1000ms
{
	// Token: 0x06002EDF RID: 11999 RVA: 0x0010EAE0 File Offset: 0x0010CCE0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.elementState == Filterable.ElementState.Liquid)
		{
			base.SelectedTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
			return;
		}
		if (this.elementState == Filterable.ElementState.Gas)
		{
			base.SelectedTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		}
	}

	// Token: 0x06002EE0 RID: 12000 RVA: 0x0010EB30 File Offset: 0x0010CD30
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.filterElementState = this.elementState;
	}

	// Token: 0x06002EE1 RID: 12001 RVA: 0x0010EB44 File Offset: 0x0010CD44
	public void Sim1000ms(float dt)
	{
		if (!base.SelectedTag.IsValid)
		{
			return;
		}
		float num = 10f - this.storage.GetAmountAvailable(base.SelectedTag);
		if (num <= 0f)
		{
			return;
		}
		Element element = ElementLoader.GetElement(base.SelectedTag);
		GameObject gameObject = Assets.TryGetPrefab(base.SelectedTag);
		if (element != null)
		{
			this.storage.AddElement(element.id, num, element.defaultValues.temperature, byte.MaxValue, 0, false, false);
			return;
		}
		if (gameObject != null)
		{
			Grid.SceneLayer sceneLayer = gameObject.GetComponent<KBatchedAnimController>().sceneLayer;
			GameObject gameObject2 = GameUtil.KInstantiate(gameObject, sceneLayer, null, 0);
			gameObject2.GetComponent<PrimaryElement>().Units = num;
			gameObject2.SetActive(true);
			this.storage.Store(gameObject2, true, false, true, false);
		}
	}

	// Token: 0x04001BC2 RID: 7106
	public Filterable.ElementState elementState = Filterable.ElementState.Liquid;

	// Token: 0x04001BC3 RID: 7107
	[MyCmpReq]
	private Storage storage;
}
