using System;
using UnityEngine;

// Token: 0x0200054D RID: 1357
public class KAnimLink
{
	// Token: 0x06001DD2 RID: 7634 RVA: 0x000A1A1F File Offset: 0x0009FC1F
	public KAnimLink(KAnimControllerBase master, KAnimControllerBase slave)
	{
		this.slave = slave;
		this.master = master;
		this.Register();
	}

	// Token: 0x06001DD3 RID: 7635 RVA: 0x000A1A44 File Offset: 0x0009FC44
	private void Register()
	{
		this.master.OnOverlayColourChanged += this.OnOverlayColourChanged;
		KAnimControllerBase kanimControllerBase = this.master;
		kanimControllerBase.OnTintChanged = (Action<Color>)Delegate.Combine(kanimControllerBase.OnTintChanged, new Action<Color>(this.OnTintColourChanged));
		KAnimControllerBase kanimControllerBase2 = this.master;
		kanimControllerBase2.OnHighlightChanged = (Action<Color>)Delegate.Combine(kanimControllerBase2.OnHighlightChanged, new Action<Color>(this.OnHighlightColourChanged));
		this.master.onLayerChanged += this.slave.SetLayer;
	}

	// Token: 0x06001DD4 RID: 7636 RVA: 0x000A1AD4 File Offset: 0x0009FCD4
	public void Unregister()
	{
		if (this.master != null)
		{
			this.master.OnOverlayColourChanged -= this.OnOverlayColourChanged;
			KAnimControllerBase kanimControllerBase = this.master;
			kanimControllerBase.OnTintChanged = (Action<Color>)Delegate.Remove(kanimControllerBase.OnTintChanged, new Action<Color>(this.OnTintColourChanged));
			KAnimControllerBase kanimControllerBase2 = this.master;
			kanimControllerBase2.OnHighlightChanged = (Action<Color>)Delegate.Remove(kanimControllerBase2.OnHighlightChanged, new Action<Color>(this.OnHighlightColourChanged));
			if (this.slave != null)
			{
				this.master.onLayerChanged -= this.slave.SetLayer;
			}
		}
	}

	// Token: 0x06001DD5 RID: 7637 RVA: 0x000A1B82 File Offset: 0x0009FD82
	private void OnOverlayColourChanged(Color32 c)
	{
		if (this.slave != null)
		{
			this.slave.OverlayColour = c;
		}
	}

	// Token: 0x06001DD6 RID: 7638 RVA: 0x000A1BA3 File Offset: 0x0009FDA3
	private void OnTintColourChanged(Color c)
	{
		if (this.syncTint && this.slave != null)
		{
			this.slave.TintColour = c;
		}
	}

	// Token: 0x06001DD7 RID: 7639 RVA: 0x000A1BCC File Offset: 0x0009FDCC
	private void OnHighlightColourChanged(Color c)
	{
		if (this.slave != null)
		{
			this.slave.HighlightColour = c;
		}
	}

	// Token: 0x04001172 RID: 4466
	public bool syncTint = true;

	// Token: 0x04001173 RID: 4467
	private KAnimControllerBase master;

	// Token: 0x04001174 RID: 4468
	private KAnimControllerBase slave;
}
