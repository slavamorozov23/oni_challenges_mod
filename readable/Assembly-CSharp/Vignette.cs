using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000EC5 RID: 3781
public class Vignette : KMonoBehaviour
{
	// Token: 0x0600791A RID: 31002 RVA: 0x002E8BA7 File Offset: 0x002E6DA7
	public static void DestroyInstance()
	{
		Vignette.Instance = null;
	}

	// Token: 0x0600791B RID: 31003 RVA: 0x002E8BB0 File Offset: 0x002E6DB0
	protected override void OnSpawn()
	{
		this.looping_sounds = base.GetComponent<LoopingSounds>();
		base.OnSpawn();
		Vignette.Instance = this;
		this.defaultColor = this.image.color;
		Game.Instance.Subscribe(1983128072, new Action<object>(this.Refresh));
		Game.Instance.Subscribe(1585324898, new Action<object>(this.Refresh));
		Game.Instance.Subscribe(-1393151672, new Action<object>(this.Refresh));
		Game.Instance.Subscribe(-741654735, new Action<object>(this.Refresh));
		Game.Instance.Subscribe(-2062778933, new Action<object>(this.Refresh));
	}

	// Token: 0x0600791C RID: 31004 RVA: 0x002E8C72 File Offset: 0x002E6E72
	public void SetColor(Color color)
	{
		this.image.color = color;
	}

	// Token: 0x0600791D RID: 31005 RVA: 0x002E8C80 File Offset: 0x002E6E80
	public void Refresh(object data)
	{
		AlertStateManager.Instance alertManager = ClusterManager.Instance.activeWorld.AlertManager;
		if (alertManager == null)
		{
			return;
		}
		if (alertManager.IsYellowAlert())
		{
			this.SetColor(this.yellowAlertColor);
			if (!this.showingYellowAlert)
			{
				this.looping_sounds.StartSound(GlobalAssets.GetSound("YellowAlert_LP", false), true, false, true);
				this.showingYellowAlert = true;
			}
		}
		else
		{
			this.showingYellowAlert = false;
			this.looping_sounds.StopSound(GlobalAssets.GetSound("YellowAlert_LP", false));
		}
		if (alertManager.IsRedAlert())
		{
			this.SetColor(this.redAlertColor);
			if (!this.showingRedAlert)
			{
				this.looping_sounds.StartSound(GlobalAssets.GetSound("RedAlert_LP", false), true, false, true);
				this.showingRedAlert = true;
			}
		}
		else
		{
			this.showingRedAlert = false;
			this.looping_sounds.StopSound(GlobalAssets.GetSound("RedAlert_LP", false));
		}
		if (!this.showingRedAlert && !this.showingYellowAlert)
		{
			this.Reset();
		}
	}

	// Token: 0x0600791E RID: 31006 RVA: 0x002E8D70 File Offset: 0x002E6F70
	public void Reset()
	{
		this.SetColor(this.defaultColor);
		this.showingRedAlert = false;
		this.showingYellowAlert = false;
		this.looping_sounds.StopSound(GlobalAssets.GetSound("RedAlert_LP", false));
		this.looping_sounds.StopSound(GlobalAssets.GetSound("YellowAlert_LP", false));
	}

	// Token: 0x04005472 RID: 21618
	[SerializeField]
	private Image image;

	// Token: 0x04005473 RID: 21619
	public Color defaultColor;

	// Token: 0x04005474 RID: 21620
	public Color redAlertColor = new Color(1f, 0f, 0f, 0.3f);

	// Token: 0x04005475 RID: 21621
	public Color yellowAlertColor = new Color(1f, 1f, 0f, 0.3f);

	// Token: 0x04005476 RID: 21622
	public static Vignette Instance;

	// Token: 0x04005477 RID: 21623
	private LoopingSounds looping_sounds;

	// Token: 0x04005478 RID: 21624
	private bool showingRedAlert;

	// Token: 0x04005479 RID: 21625
	private bool showingYellowAlert;
}
