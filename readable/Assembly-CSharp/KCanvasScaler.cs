using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D37 RID: 3383
[AddComponentMenu("KMonoBehaviour/scripts/KCanvasScaler")]
public class KCanvasScaler : KMonoBehaviour
{
	// Token: 0x0600689D RID: 26781 RVA: 0x00279F68 File Offset: 0x00278168
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (KPlayerPrefs.HasKey(KCanvasScaler.UIScalePrefKey))
		{
			this.SetUserScale(KPlayerPrefs.GetFloat(KCanvasScaler.UIScalePrefKey) / 100f);
		}
		else
		{
			this.SetUserScale(1f);
		}
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
	}

	// Token: 0x0600689E RID: 26782 RVA: 0x00279FD0 File Offset: 0x002781D0
	private void OnResize()
	{
		this.SetUserScale(this.userScale);
	}

	// Token: 0x0600689F RID: 26783 RVA: 0x00279FDE File Offset: 0x002781DE
	public void SetUserScale(float scale)
	{
		if (this.canvasScaler == null)
		{
			this.canvasScaler = base.GetComponent<CanvasScaler>();
		}
		this.userScale = scale;
		this.canvasScaler.scaleFactor = this.GetCanvasScale();
	}

	// Token: 0x060068A0 RID: 26784 RVA: 0x0027A012 File Offset: 0x00278212
	public float GetUserScale()
	{
		return this.userScale;
	}

	// Token: 0x060068A1 RID: 26785 RVA: 0x0027A01A File Offset: 0x0027821A
	public float GetCanvasScale()
	{
		return this.userScale * this.ScreenRelativeScale();
	}

	// Token: 0x060068A2 RID: 26786 RVA: 0x0027A02C File Offset: 0x0027822C
	private float ScreenRelativeScale()
	{
		float dpi = Screen.dpi;
		Camera x = Camera.main;
		if (x == null)
		{
			x = UnityEngine.Object.FindObjectOfType<Camera>();
		}
		x != null;
		float num = (float)Screen.width / (float)Screen.height;
		if ((float)Screen.height <= this.scaleSteps[0].maxRes_y || num < 1.6f)
		{
			return this.scaleSteps[0].scale;
		}
		if ((float)Screen.height > this.scaleSteps[this.scaleSteps.Length - 1].maxRes_y)
		{
			return this.scaleSteps[this.scaleSteps.Length - 1].scale;
		}
		for (int i = 0; i < this.scaleSteps.Length; i++)
		{
			if ((float)Screen.height > this.scaleSteps[i].maxRes_y && (float)Screen.height <= this.scaleSteps[i + 1].maxRes_y)
			{
				float t = ((float)Screen.height - this.scaleSteps[i].maxRes_y) / (this.scaleSteps[i + 1].maxRes_y - this.scaleSteps[i].maxRes_y);
				return Mathf.Lerp(this.scaleSteps[i].scale, this.scaleSteps[i + 1].scale, t);
			}
		}
		return 1f;
	}

	// Token: 0x040047DF RID: 18399
	[MyCmpReq]
	private CanvasScaler canvasScaler;

	// Token: 0x040047E0 RID: 18400
	public static string UIScalePrefKey = "UIScalePref";

	// Token: 0x040047E1 RID: 18401
	private float userScale = 1f;

	// Token: 0x040047E2 RID: 18402
	[Range(0.75f, 2f)]
	private KCanvasScaler.ScaleStep[] scaleSteps = new KCanvasScaler.ScaleStep[]
	{
		new KCanvasScaler.ScaleStep(720f, 0.86f),
		new KCanvasScaler.ScaleStep(1080f, 1f),
		new KCanvasScaler.ScaleStep(2160f, 1.33f)
	};

	// Token: 0x02001F66 RID: 8038
	[Serializable]
	public struct ScaleStep
	{
		// Token: 0x0600B638 RID: 46648 RVA: 0x003EFAC5 File Offset: 0x003EDCC5
		public ScaleStep(float maxRes_y, float scale)
		{
			this.maxRes_y = maxRes_y;
			this.scale = scale;
		}

		// Token: 0x040092BA RID: 37562
		public float scale;

		// Token: 0x040092BB RID: 37563
		public float maxRes_y;
	}
}
