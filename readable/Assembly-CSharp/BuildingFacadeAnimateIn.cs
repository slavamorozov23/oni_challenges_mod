using System;
using UnityEngine;

// Token: 0x02000717 RID: 1815
public class BuildingFacadeAnimateIn : MonoBehaviour
{
	// Token: 0x06002D4C RID: 11596 RVA: 0x00106914 File Offset: 0x00104B14
	private void Awake()
	{
		this.placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
		this.colorAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
		this.updater = Updater.Series(new Updater[]
		{
			KleiPermitBuildingAnimateIn.MakeAnimInUpdater(this.sourceAnimController, this.placeAnimController, this.colorAnimController),
			Updater.Do(delegate()
			{
				UnityEngine.Object.Destroy(base.gameObject);
			})
		});
	}

	// Token: 0x06002D4D RID: 11597 RVA: 0x001069A8 File Offset: 0x00104BA8
	private void Update()
	{
		if (this.sourceAnimController.IsNullOrDestroyed())
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		BuildingFacadeAnimateIn.SetVisibilityOn(this.sourceAnimController, false);
		this.updater.Internal_Update(Time.unscaledDeltaTime);
	}

	// Token: 0x06002D4E RID: 11598 RVA: 0x001069E0 File Offset: 0x00104BE0
	private void OnDisable()
	{
		if (!this.sourceAnimController.IsNullOrDestroyed())
		{
			BuildingFacadeAnimateIn.SetVisibilityOn(this.sourceAnimController, true);
		}
		UnityEngine.Object.Destroy(this.placeAnimController.gameObject);
		UnityEngine.Object.Destroy(this.colorAnimController.gameObject);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002D4F RID: 11599 RVA: 0x00106A34 File Offset: 0x00104C34
	public static BuildingFacadeAnimateIn MakeFor(KBatchedAnimController sourceAnimController)
	{
		BuildingFacadeAnimateIn.SetVisibilityOn(sourceAnimController, false);
		KBatchedAnimController kbatchedAnimController = BuildingFacadeAnimateIn.SpawnAnimFrom(sourceAnimController);
		kbatchedAnimController.gameObject.name = "BuildingFacadeAnimateIn.placeAnimController";
		kbatchedAnimController.initialAnim = "place";
		KBatchedAnimController kbatchedAnimController2 = BuildingFacadeAnimateIn.SpawnAnimFrom(sourceAnimController);
		kbatchedAnimController2.gameObject.name = "BuildingFacadeAnimateIn.colorAnimController";
		kbatchedAnimController2.initialAnim = ((sourceAnimController.CurrentAnim != null) ? sourceAnimController.CurrentAnim.name : sourceAnimController.AnimFiles[0].GetData().GetAnim(0).name);
		GameObject gameObject = new GameObject("BuildingFacadeAnimateIn");
		gameObject.SetActive(false);
		gameObject.transform.SetParent(sourceAnimController.transform.parent, false);
		BuildingFacadeAnimateIn buildingFacadeAnimateIn = gameObject.AddComponent<BuildingFacadeAnimateIn>();
		buildingFacadeAnimateIn.sourceAnimController = sourceAnimController;
		buildingFacadeAnimateIn.placeAnimController = kbatchedAnimController;
		buildingFacadeAnimateIn.colorAnimController = kbatchedAnimController2;
		kbatchedAnimController.gameObject.SetActive(true);
		kbatchedAnimController2.gameObject.SetActive(true);
		gameObject.SetActive(true);
		return buildingFacadeAnimateIn;
	}

	// Token: 0x06002D50 RID: 11600 RVA: 0x00106B18 File Offset: 0x00104D18
	private static void SetVisibilityOn(KBatchedAnimController animController, bool isVisible)
	{
		animController.SetVisiblity(isVisible);
		foreach (KBatchedAnimController kbatchedAnimController in animController.GetComponentsInChildren<KBatchedAnimController>(true))
		{
			if (kbatchedAnimController.batchGroupID == animController.batchGroupID)
			{
				kbatchedAnimController.SetVisiblity(isVisible);
			}
		}
	}

	// Token: 0x06002D51 RID: 11601 RVA: 0x00106B60 File Offset: 0x00104D60
	private static KBatchedAnimController SpawnAnimFrom(KBatchedAnimController sourceAnimController)
	{
		GameObject gameObject = new GameObject();
		gameObject.SetActive(false);
		gameObject.transform.SetParent(sourceAnimController.transform.parent, false);
		gameObject.transform.localPosition = sourceAnimController.transform.localPosition;
		gameObject.transform.localRotation = sourceAnimController.transform.localRotation;
		gameObject.transform.localScale = sourceAnimController.transform.localScale;
		gameObject.layer = sourceAnimController.gameObject.layer;
		KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.materialType = sourceAnimController.materialType;
		kbatchedAnimController.initialMode = sourceAnimController.initialMode;
		kbatchedAnimController.AnimFiles = sourceAnimController.AnimFiles;
		kbatchedAnimController.Offset = sourceAnimController.Offset;
		kbatchedAnimController.animWidth = sourceAnimController.animWidth;
		kbatchedAnimController.animHeight = sourceAnimController.animHeight;
		kbatchedAnimController.animScale = sourceAnimController.animScale;
		kbatchedAnimController.sceneLayer = sourceAnimController.sceneLayer;
		kbatchedAnimController.fgLayer = sourceAnimController.fgLayer;
		kbatchedAnimController.FlipX = sourceAnimController.FlipX;
		kbatchedAnimController.FlipY = sourceAnimController.FlipY;
		kbatchedAnimController.Rotation = sourceAnimController.Rotation;
		kbatchedAnimController.Pivot = sourceAnimController.Pivot;
		return kbatchedAnimController;
	}

	// Token: 0x04001AED RID: 6893
	private KBatchedAnimController sourceAnimController;

	// Token: 0x04001AEE RID: 6894
	private KBatchedAnimController placeAnimController;

	// Token: 0x04001AEF RID: 6895
	private KBatchedAnimController colorAnimController;

	// Token: 0x04001AF0 RID: 6896
	private Updater updater;
}
