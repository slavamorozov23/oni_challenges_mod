using System;
using UnityEngine;

// Token: 0x020008BE RID: 2238
[AddComponentMenu("KMonoBehaviour/scripts/TemperatureCookable")]
public class TemperatureCookable : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06003DB1 RID: 15793 RVA: 0x00158268 File Offset: 0x00156468
	public void Sim1000ms(float dt)
	{
		if (this.element.Temperature > this.cookTemperature && this.cookedID != null)
		{
			this.Cook();
		}
	}

	// Token: 0x06003DB2 RID: 15794 RVA: 0x0015828C File Offset: 0x0015648C
	private void Cook()
	{
		Vector3 position = base.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(this.cookedID), position);
		gameObject.SetActive(true);
		KSelectable component = base.gameObject.GetComponent<KSelectable>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component)
		{
			SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
		}
		PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
		component2.Temperature = this.element.Temperature;
		component2.Mass = this.element.Mass;
		base.gameObject.DeleteObject();
	}

	// Token: 0x04002612 RID: 9746
	[MyCmpReq]
	private PrimaryElement element;

	// Token: 0x04002613 RID: 9747
	public float cookTemperature = 273150f;

	// Token: 0x04002614 RID: 9748
	public string cookedID;
}
