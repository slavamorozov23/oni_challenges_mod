using System;
using UnityEngine;

// Token: 0x02000BF6 RID: 3062
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/TileTemperature")]
public class TileTemperature : KMonoBehaviour
{
	// Token: 0x06005BE3 RID: 23523 RVA: 0x002141BC File Offset: 0x002123BC
	protected override void OnPrefabInit()
	{
		this.primaryElement.getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(TileTemperature.OnGetTemperature);
		this.primaryElement.setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(TileTemperature.OnSetTemperature);
		base.OnPrefabInit();
	}

	// Token: 0x06005BE4 RID: 23524 RVA: 0x002141F2 File Offset: 0x002123F2
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06005BE5 RID: 23525 RVA: 0x002141FC File Offset: 0x002123FC
	private static float OnGetTemperature(PrimaryElement primary_element)
	{
		SimCellOccupier component = primary_element.GetComponent<SimCellOccupier>();
		if (component != null && component.IsReady())
		{
			int i = Grid.PosToCell(primary_element.transform.GetPosition());
			return Grid.Temperature[i];
		}
		return primary_element.InternalTemperature;
	}

	// Token: 0x06005BE6 RID: 23526 RVA: 0x00214244 File Offset: 0x00212444
	private static void OnSetTemperature(PrimaryElement primary_element, float temperature)
	{
		SimCellOccupier component = primary_element.GetComponent<SimCellOccupier>();
		if (component != null && component.IsReady())
		{
			global::Debug.LogWarning("Only set a tile's temperature during initialization. Otherwise you should be modifying the cell via the sim!");
			return;
		}
		primary_element.InternalTemperature = temperature;
	}

	// Token: 0x04003D37 RID: 15671
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04003D38 RID: 15672
	[MyCmpReq]
	private KSelectable selectable;
}
