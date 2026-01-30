using System;
using UnityEngine;

// Token: 0x02000D4A RID: 3402
public class KleiPermitBuildingAnimateIn : MonoBehaviour
{
	// Token: 0x06006970 RID: 26992 RVA: 0x0027EE74 File Offset: 0x0027D074
	private void Awake()
	{
		this.placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
		this.colorAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
		this.updater = Updater.Parallel(new Updater[]
		{
			KleiPermitBuildingAnimateIn.MakeAnimInUpdater(this.sourceAnimController, this.placeAnimController, this.colorAnimController),
			this.extraUpdater
		});
	}

	// Token: 0x06006971 RID: 26993 RVA: 0x0027EEFD File Offset: 0x0027D0FD
	private void Update()
	{
		this.sourceAnimController.gameObject.SetActive(false);
		this.updater.Internal_Update(Time.unscaledDeltaTime);
	}

	// Token: 0x06006972 RID: 26994 RVA: 0x0027EF21 File Offset: 0x0027D121
	private void OnDisable()
	{
		this.sourceAnimController.gameObject.SetActive(true);
		UnityEngine.Object.Destroy(this.placeAnimController.gameObject);
		UnityEngine.Object.Destroy(this.colorAnimController.gameObject);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06006973 RID: 26995 RVA: 0x0027EF60 File Offset: 0x0027D160
	public static KleiPermitBuildingAnimateIn MakeFor(KBatchedAnimController sourceAnimController, Updater extraUpdater = default(Updater), string place_anim = "place")
	{
		sourceAnimController.gameObject.SetActive(false);
		KBatchedAnimController kbatchedAnimController = UnityEngine.Object.Instantiate<KBatchedAnimController>(sourceAnimController, sourceAnimController.transform.parent, false);
		kbatchedAnimController.gameObject.name = "KleiPermitBuildingAnimateIn.placeAnimController";
		kbatchedAnimController.initialAnim = place_anim;
		KBatchedAnimController kbatchedAnimController2 = UnityEngine.Object.Instantiate<KBatchedAnimController>(sourceAnimController, sourceAnimController.transform.parent, false);
		kbatchedAnimController2.gameObject.name = "KleiPermitBuildingAnimateIn.colorAnimController";
		KAnimFileData data = sourceAnimController.AnimFiles[0].GetData();
		KAnim.Anim anim = data.GetAnim("idle");
		if (anim == null)
		{
			anim = data.GetAnim("off");
			if (anim == null)
			{
				anim = data.GetAnim(0);
			}
		}
		kbatchedAnimController2.initialAnim = anim.name;
		GameObject gameObject = new GameObject("KleiPermitBuildingAnimateIn");
		gameObject.SetActive(false);
		gameObject.transform.SetParent(sourceAnimController.transform.parent, false);
		KleiPermitBuildingAnimateIn kleiPermitBuildingAnimateIn = gameObject.AddComponent<KleiPermitBuildingAnimateIn>();
		kleiPermitBuildingAnimateIn.sourceAnimController = sourceAnimController;
		kleiPermitBuildingAnimateIn.placeAnimController = kbatchedAnimController;
		kleiPermitBuildingAnimateIn.colorAnimController = kbatchedAnimController2;
		kleiPermitBuildingAnimateIn.extraUpdater = ((extraUpdater.fn == null) ? Updater.None() : extraUpdater);
		kbatchedAnimController.gameObject.SetActive(true);
		kbatchedAnimController2.gameObject.SetActive(true);
		gameObject.SetActive(true);
		return kleiPermitBuildingAnimateIn;
	}

	// Token: 0x06006974 RID: 26996 RVA: 0x0027F084 File Offset: 0x0027D284
	public static Updater MakeAnimInUpdater(KBatchedAnimController sourceAnimController, KBatchedAnimController placeAnimController, KBatchedAnimController colorAnimController)
	{
		return Updater.Parallel(new Updater[]
		{
			Updater.Series(new Updater[]
			{
				Updater.Ease(delegate(float alpha)
				{
					placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)Mathf.Clamp(alpha, 1f, 255f));
				}, 1f, 255f, 0.1f, Easing.CubicOut, -1f),
				Updater.Ease(delegate(float alpha)
				{
					placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)Mathf.Clamp(255f - alpha, 1f, 255f));
					colorAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)Mathf.Clamp(alpha, 1f, 255f));
				}, 1f, 255f, 0.3f, Easing.CubicIn, -1f)
			}),
			Updater.Series(new Updater[]
			{
				Updater.Ease(delegate(float scale)
				{
					scale = sourceAnimController.transform.localScale.x * scale;
					placeAnimController.transform.localScale = Vector3.one * scale;
					colorAnimController.transform.localScale = Vector3.one * scale;
				}, 1f, 1.012f, 0.2f, Easing.CubicOut, -1f),
				Updater.Ease(delegate(float scale)
				{
					scale = sourceAnimController.transform.localScale.x * scale;
					placeAnimController.transform.localScale = Vector3.one * scale;
					colorAnimController.transform.localScale = Vector3.one * scale;
				}, 1.012f, 1f, 0.1f, Easing.CubicIn, -1f)
			})
		});
	}

	// Token: 0x04004870 RID: 18544
	private KBatchedAnimController sourceAnimController;

	// Token: 0x04004871 RID: 18545
	private KBatchedAnimController placeAnimController;

	// Token: 0x04004872 RID: 18546
	private KBatchedAnimController colorAnimController;

	// Token: 0x04004873 RID: 18547
	private Updater updater;

	// Token: 0x04004874 RID: 18548
	private Updater extraUpdater;
}
