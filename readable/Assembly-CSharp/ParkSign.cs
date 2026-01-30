using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020007D7 RID: 2007
[AddComponentMenu("KMonoBehaviour/scripts/ParkSign")]
public class ParkSign : KMonoBehaviour
{
	// Token: 0x06003550 RID: 13648 RVA: 0x0012D421 File Offset: 0x0012B621
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<ParkSign>(-832141045, ParkSign.TriggerRoomEffectsDelegate);
	}

	// Token: 0x06003551 RID: 13649 RVA: 0x0012D43C File Offset: 0x0012B63C
	private void TriggerRoomEffects(object data)
	{
		GameObject gameObject = (GameObject)data;
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			roomOfGameObject.roomType.TriggerRoomEffects(base.gameObject.GetComponent<KPrefabID>(), gameObject.GetComponent<Effects>());
		}
	}

	// Token: 0x04002042 RID: 8258
	private static readonly EventSystem.IntraObjectHandler<ParkSign> TriggerRoomEffectsDelegate = new EventSystem.IntraObjectHandler<ParkSign>(delegate(ParkSign component, object data)
	{
		component.TriggerRoomEffects(data);
	});
}
