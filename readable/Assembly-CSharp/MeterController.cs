using System;
using UnityEngine;

// Token: 0x020007BA RID: 1978
public class MeterController
{
	// Token: 0x0600344F RID: 13391 RVA: 0x00128681 File Offset: 0x00126881
	public static float StandardLerp(float percentage, int frames)
	{
		return percentage;
	}

	// Token: 0x06003450 RID: 13392 RVA: 0x00128684 File Offset: 0x00126884
	public static float MinMaxStepLerp(float percentage, int frames)
	{
		if ((double)percentage <= 0.0 || frames <= 1)
		{
			return 0f;
		}
		if ((double)percentage >= 1.0 || frames == 2)
		{
			return 1f;
		}
		return (1f + percentage * (float)(frames - 2)) / (float)frames;
	}

	// Token: 0x17000345 RID: 837
	// (get) Token: 0x06003451 RID: 13393 RVA: 0x001286C3 File Offset: 0x001268C3
	// (set) Token: 0x06003452 RID: 13394 RVA: 0x001286CB File Offset: 0x001268CB
	public KBatchedAnimController meterController { get; private set; }

	// Token: 0x06003453 RID: 13395 RVA: 0x001286D4 File Offset: 0x001268D4
	public MeterController(KMonoBehaviour target, Meter.Offset front_back, Grid.SceneLayer user_specified_render_layer, params string[] symbols_to_hide)
	{
		string[] array = new string[symbols_to_hide.Length + 1];
		Array.Copy(symbols_to_hide, array, symbols_to_hide.Length);
		array[array.Length - 1] = "meter_target";
		KBatchedAnimController component = target.GetComponent<KBatchedAnimController>();
		this.Initialize(component, "meter_target", "meter", front_back, user_specified_render_layer, Vector3.zero, array);
	}

	// Token: 0x06003454 RID: 13396 RVA: 0x0012873D File Offset: 0x0012693D
	public MeterController(KAnimControllerBase building_controller, string meter_target, string meter_animation, Meter.Offset front_back, Grid.SceneLayer user_specified_render_layer, params string[] symbols_to_hide)
	{
		this.Initialize(building_controller, meter_target, meter_animation, front_back, user_specified_render_layer, Vector3.zero, symbols_to_hide);
	}

	// Token: 0x06003455 RID: 13397 RVA: 0x0012876B File Offset: 0x0012696B
	public MeterController(KAnimControllerBase building_controller, string meter_target, string meter_animation, Meter.Offset front_back, Grid.SceneLayer user_specified_render_layer, Vector3 tracker_offset, params string[] symbols_to_hide)
	{
		this.Initialize(building_controller, meter_target, meter_animation, front_back, user_specified_render_layer, tracker_offset, symbols_to_hide);
	}

	// Token: 0x06003456 RID: 13398 RVA: 0x00128798 File Offset: 0x00126998
	private void Initialize(KAnimControllerBase building_controller, string meter_target, string meter_animation, Meter.Offset front_back, Grid.SceneLayer user_specified_render_layer, Vector3 tracker_offset, params string[] symbols_to_hide)
	{
		if (building_controller.HasAnimation(meter_animation + "_cb") && !GlobalAssets.Instance.colorSet.IsDefaultColorSet())
		{
			meter_animation += "_cb";
		}
		string name = building_controller.name + "." + meter_animation;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Assets.GetPrefab(MeterConfig.ID));
		gameObject.name = name;
		gameObject.SetActive(false);
		gameObject.transform.parent = building_controller.transform;
		this.gameObject = gameObject;
		gameObject.GetComponent<KPrefabID>().PrefabTag = new Tag(name);
		Vector3 position = building_controller.transform.GetPosition();
		switch (front_back)
		{
		case Meter.Offset.Infront:
			position.z -= 0.1f;
			break;
		case Meter.Offset.Behind:
			position.z += 0.1f;
			break;
		case Meter.Offset.UserSpecified:
			position.z = Grid.GetLayerZ(user_specified_render_layer);
			break;
		}
		gameObject.transform.SetPosition(position);
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		component.AnimFiles = new KAnimFile[]
		{
			building_controller.AnimFiles[0]
		};
		component.initialAnim = meter_animation;
		component.fgLayer = Grid.SceneLayer.NoLayer;
		component.initialMode = KAnim.PlayMode.Paused;
		component.isMovable = true;
		component.FlipX = building_controller.FlipX;
		component.FlipY = building_controller.FlipY;
		if (Meter.Offset.UserSpecified == front_back)
		{
			component.sceneLayer = user_specified_render_layer;
		}
		this.meterController = component;
		KBatchedAnimTracker component2 = gameObject.GetComponent<KBatchedAnimTracker>();
		component2.offset = tracker_offset;
		component2.symbol = new HashedString(meter_target);
		gameObject.SetActive(true);
		building_controller.SetSymbolVisiblity(meter_target, false);
		if (symbols_to_hide != null)
		{
			for (int i = 0; i < symbols_to_hide.Length; i++)
			{
				building_controller.SetSymbolVisiblity(symbols_to_hide[i], false);
			}
		}
		this.link = new KAnimLink(building_controller, component);
	}

	// Token: 0x06003457 RID: 13399 RVA: 0x00128968 File Offset: 0x00126B68
	public MeterController(KAnimControllerBase building_controller, KBatchedAnimController meter_controller, params string[] symbol_names)
	{
		if (meter_controller == null)
		{
			return;
		}
		this.meterController = meter_controller;
		this.link = new KAnimLink(building_controller, meter_controller);
		for (int i = 0; i < symbol_names.Length; i++)
		{
			building_controller.SetSymbolVisiblity(symbol_names[i], false);
		}
		this.meterController.GetComponent<KBatchedAnimTracker>().symbol = new HashedString(symbol_names[0]);
	}

	// Token: 0x06003458 RID: 13400 RVA: 0x001289E0 File Offset: 0x00126BE0
	public void SetPositionPercent(float percent_full)
	{
		if (this.meterController == null)
		{
			return;
		}
		this.meterController.SetPositionPercent(this.interpolateFunction(percent_full, this.meterController.GetCurrentNumFrames()));
	}

	// Token: 0x06003459 RID: 13401 RVA: 0x00128A13 File Offset: 0x00126C13
	public void SetSymbolTint(KAnimHashedString symbol, Color32 colour)
	{
		if (this.meterController != null)
		{
			this.meterController.SetSymbolTint(symbol, colour);
		}
	}

	// Token: 0x0600345A RID: 13402 RVA: 0x00128A35 File Offset: 0x00126C35
	public void SetRotation(float rot)
	{
		if (this.meterController == null)
		{
			return;
		}
		this.meterController.Rotation = rot;
	}

	// Token: 0x0600345B RID: 13403 RVA: 0x00128A52 File Offset: 0x00126C52
	public void Unlink()
	{
		if (this.link != null)
		{
			this.link.Unregister();
			this.link = null;
		}
	}

	// Token: 0x04001F93 RID: 8083
	public GameObject gameObject;

	// Token: 0x04001F94 RID: 8084
	public Func<float, int, float> interpolateFunction = new Func<float, int, float>(MeterController.MinMaxStepLerp);

	// Token: 0x04001F95 RID: 8085
	private KAnimLink link;
}
