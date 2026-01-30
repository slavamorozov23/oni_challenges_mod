using System;
using UnityEngine;

// Token: 0x020000B3 RID: 179
[AddComponentMenu("KMonoBehaviour/scripts/LightSymbolTracker")]
public class LightSymbolTracker : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x06000358 RID: 856 RVA: 0x0001A6D5 File Offset: 0x000188D5
	protected override void OnSpawn()
	{
		this.animController = base.GetComponent<KBatchedAnimController>();
		this.light2D = base.GetComponent<Light2D>();
		this.pickupable = base.GetComponent<Pickupable>();
	}

	// Token: 0x06000359 RID: 857 RVA: 0x0001A6FC File Offset: 0x000188FC
	public bool IsEnableAndVisible()
	{
		return CameraController.Instance.VisibleArea.CurrentAreaExtended.Contains(this.pickupable.cachedCell) && base.enabled;
	}

	// Token: 0x0600035A RID: 858 RVA: 0x0001A738 File Offset: 0x00018938
	public void RenderEveryTick(float dt)
	{
		if (!this.IsEnableAndVisible())
		{
			return;
		}
		Vector3 v = Vector3.zero;
		bool flag;
		v = (this.animController.GetTransformMatrix() * this.animController.GetSymbolLocalTransform(this.targetSymbol, out flag)).MultiplyPoint(Vector3.zero) - base.transform.position;
		this.light2D.Offset = v;
	}

	// Token: 0x04000241 RID: 577
	public HashedString targetSymbol;

	// Token: 0x04000242 RID: 578
	private KBatchedAnimController animController;

	// Token: 0x04000243 RID: 579
	private Light2D light2D;

	// Token: 0x04000244 RID: 580
	private Pickupable pickupable;
}
