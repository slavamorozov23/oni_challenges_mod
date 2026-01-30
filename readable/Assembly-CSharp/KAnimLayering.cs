using System;
using UnityEngine;

// Token: 0x0200054C RID: 1356
public class KAnimLayering
{
	// Token: 0x06001DC7 RID: 7623 RVA: 0x000A159A File Offset: 0x0009F79A
	public KAnimLayering(KAnimControllerBase controller, Grid.SceneLayer layer)
	{
		this.controller = controller;
		this.layer = layer;
	}

	// Token: 0x06001DC8 RID: 7624 RVA: 0x000A15B8 File Offset: 0x0009F7B8
	public void SetLayer(Grid.SceneLayer layer)
	{
		this.layer = layer;
		if (this.foregroundController != null)
		{
			Vector3 position = new Vector3(0f, 0f, Grid.GetLayerZ(layer) - this.controller.gameObject.transform.GetPosition().z - 0.1f);
			this.foregroundController.transform.SetLocalPosition(position);
		}
	}

	// Token: 0x06001DC9 RID: 7625 RVA: 0x000A1624 File Offset: 0x0009F824
	public void SetIsForeground(bool is_foreground)
	{
		this.isForeground = is_foreground;
	}

	// Token: 0x06001DCA RID: 7626 RVA: 0x000A162D File Offset: 0x0009F82D
	public bool GetIsForeground()
	{
		return this.isForeground;
	}

	// Token: 0x06001DCB RID: 7627 RVA: 0x000A1635 File Offset: 0x0009F835
	public KAnimLink GetLink()
	{
		return this.link;
	}

	// Token: 0x06001DCC RID: 7628 RVA: 0x000A1640 File Offset: 0x0009F840
	private static bool IsAnimLayered(KAnimFile[] anims)
	{
		foreach (KAnimFile kanimFile in anims)
		{
			if (!(kanimFile == null))
			{
				KAnimFileData data = kanimFile.GetData();
				if (data.build != null)
				{
					KAnim.Build.Symbol[] symbols = data.build.symbols;
					for (int j = 0; j < symbols.Length; j++)
					{
						if ((symbols[j].flags & 8) != 0)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06001DCD RID: 7629 RVA: 0x000A16A8 File Offset: 0x0009F8A8
	private void HideSymbolsInternal()
	{
		foreach (KAnimFile kanimFile in this.controller.AnimFiles)
		{
			if (!(kanimFile == null))
			{
				KAnimFileData data = kanimFile.GetData();
				if (data.build != null)
				{
					KAnim.Build.Symbol[] symbols = data.build.symbols;
					for (int j = 0; j < symbols.Length; j++)
					{
						if ((symbols[j].flags & 8) != 0 != this.isForeground && !(symbols[j].hash == KAnimLayering.UI))
						{
							this.controller.SetSymbolVisiblity(symbols[j].hash, false);
						}
					}
				}
			}
		}
	}

	// Token: 0x06001DCE RID: 7630 RVA: 0x000A1754 File Offset: 0x0009F954
	public void HideSymbols()
	{
		if (EntityPrefabs.Instance == null)
		{
			return;
		}
		if (this.isForeground)
		{
			return;
		}
		KAnimFile[] animFiles = this.controller.AnimFiles;
		bool flag = KAnimLayering.IsAnimLayered(animFiles);
		if (flag && this.layer != Grid.SceneLayer.NoLayer)
		{
			bool flag2 = this.foregroundController == null;
			if (flag2)
			{
				GameObject gameObject = Util.KInstantiate(EntityPrefabs.Instance.ForegroundLayer, this.controller.gameObject, null);
				gameObject.name = this.controller.name + "_fg";
				this.foregroundController = gameObject.GetComponent<KAnimControllerBase>();
				this.link = new KAnimLink(this.controller, this.foregroundController);
			}
			this.foregroundController.AnimFiles = animFiles;
			this.foregroundController.GetLayering().SetIsForeground(true);
			this.foregroundController.initialAnim = this.controller.initialAnim;
			this.Dirty();
			KAnimSynchronizer synchronizer = this.controller.GetSynchronizer();
			if (flag2)
			{
				synchronizer.Add(this.foregroundController);
			}
			else
			{
				this.foregroundController.GetComponent<KBatchedAnimController>().SwapAnims(this.foregroundController.AnimFiles);
			}
			synchronizer.Sync(this.foregroundController);
			Vector3 position = new Vector3(0f, 0f, Grid.GetLayerZ(this.layer) - this.controller.gameObject.transform.GetPosition().z - 0.1f);
			this.foregroundController.gameObject.transform.SetLocalPosition(position);
			this.foregroundController.gameObject.SetActive(true);
		}
		else if (!flag && this.foregroundController != null)
		{
			this.controller.GetSynchronizer().Remove(this.foregroundController);
			this.foregroundController.gameObject.DeleteObject();
			this.link = null;
		}
		if (this.foregroundController != null)
		{
			this.HideSymbolsInternal();
			KAnimLayering layering = this.foregroundController.GetLayering();
			if (layering != null)
			{
				layering.HideSymbolsInternal();
			}
		}
	}

	// Token: 0x06001DCF RID: 7631 RVA: 0x000A1957 File Offset: 0x0009FB57
	public void RefreshForegroundBatchGroup()
	{
		if (this.foregroundController == null)
		{
			return;
		}
		this.foregroundController.GetComponent<KBatchedAnimController>().SwapAnims(this.foregroundController.AnimFiles);
	}

	// Token: 0x06001DD0 RID: 7632 RVA: 0x000A1984 File Offset: 0x0009FB84
	public void Dirty()
	{
		if (this.foregroundController == null)
		{
			return;
		}
		this.foregroundController.Offset = this.controller.Offset;
		this.foregroundController.Pivot = this.controller.Pivot;
		this.foregroundController.Rotation = this.controller.Rotation;
		this.foregroundController.FlipX = this.controller.FlipX;
		this.foregroundController.FlipY = this.controller.FlipY;
	}

	// Token: 0x0400116C RID: 4460
	private bool isForeground;

	// Token: 0x0400116D RID: 4461
	private KAnimControllerBase controller;

	// Token: 0x0400116E RID: 4462
	private KAnimControllerBase foregroundController;

	// Token: 0x0400116F RID: 4463
	private KAnimLink link;

	// Token: 0x04001170 RID: 4464
	private Grid.SceneLayer layer = Grid.SceneLayer.BuildingFront;

	// Token: 0x04001171 RID: 4465
	public static readonly KAnimHashedString UI = new KAnimHashedString("ui");
}
