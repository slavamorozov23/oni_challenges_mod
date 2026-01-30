using System;

// Token: 0x02000776 RID: 1910
public class HEPBridgeTileVisualizer : KMonoBehaviour, IHighEnergyParticleDirection
{
	// Token: 0x06003091 RID: 12433 RVA: 0x0011887C File Offset: 0x00116A7C
	protected override void OnSpawn()
	{
		base.Subscribe<HEPBridgeTileVisualizer>(-1643076535, HEPBridgeTileVisualizer.OnRotateDelegate);
		this.OnRotate();
	}

	// Token: 0x06003092 RID: 12434 RVA: 0x00118895 File Offset: 0x00116A95
	public void OnRotate()
	{
		Game.Instance.ForceOverlayUpdate(true);
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06003093 RID: 12435 RVA: 0x001188A4 File Offset: 0x00116AA4
	// (set) Token: 0x06003094 RID: 12436 RVA: 0x001188F1 File Offset: 0x00116AF1
	public EightDirection Direction
	{
		get
		{
			EightDirection result = EightDirection.Right;
			Rotatable component = base.GetComponent<Rotatable>();
			if (component != null)
			{
				switch (component.Orientation)
				{
				case Orientation.Neutral:
					result = EightDirection.Left;
					break;
				case Orientation.R90:
					result = EightDirection.Up;
					break;
				case Orientation.R180:
					result = EightDirection.Right;
					break;
				case Orientation.R270:
					result = EightDirection.Down;
					break;
				}
			}
			return result;
		}
		set
		{
		}
	}

	// Token: 0x04001CF1 RID: 7409
	private static readonly EventSystem.IntraObjectHandler<HEPBridgeTileVisualizer> OnRotateDelegate = new EventSystem.IntraObjectHandler<HEPBridgeTileVisualizer>(delegate(HEPBridgeTileVisualizer component, object data)
	{
		component.OnRotate();
	});
}
