using System;

// Token: 0x020007A4 RID: 1956
public class LogicRibbonBridge : KMonoBehaviour
{
	// Token: 0x06003329 RID: 13097 RVA: 0x00124A8C File Offset: 0x00122C8C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		switch (base.GetComponent<Rotatable>().GetOrientation())
		{
		case Orientation.Neutral:
			component.Play("0", KAnim.PlayMode.Once, 1f, 0f);
			return;
		case Orientation.R90:
			component.Play("90", KAnim.PlayMode.Once, 1f, 0f);
			return;
		case Orientation.R180:
			component.Play("180", KAnim.PlayMode.Once, 1f, 0f);
			return;
		case Orientation.R270:
			component.Play("270", KAnim.PlayMode.Once, 1f, 0f);
			return;
		default:
			return;
		}
	}
}
