using System;
using System.Runtime.CompilerServices;
using FMODUnity;
using ProcGenGame;
using UnityEngine;

// Token: 0x02000C68 RID: 3176
public class NewBaseScreen : KScreen
{
	// Token: 0x060060CF RID: 24783 RVA: 0x00239C22 File Offset: 0x00237E22
	public override float GetSortKey()
	{
		return 1f;
	}

	// Token: 0x060060D0 RID: 24784 RVA: 0x00239C29 File Offset: 0x00237E29
	protected override void OnPrefabInit()
	{
		NewBaseScreen.Instance = this;
		base.OnPrefabInit();
		TimeOfDay.Instance.SetScale(0f);
	}

	// Token: 0x060060D1 RID: 24785 RVA: 0x00239C46 File Offset: 0x00237E46
	protected override void OnForcedCleanUp()
	{
		NewBaseScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x060060D2 RID: 24786 RVA: 0x00239C54 File Offset: 0x00237E54
	public static Vector2I SetInitialCamera()
	{
		Vector2I vector2I = SaveLoader.Instance.cachedGSD.baseStartPos;
		vector2I += ClusterManager.Instance.GetStartWorld().WorldOffset;
		Vector3 pos = Grid.CellToPosCCC(Grid.OffsetCell(Grid.OffsetCell(0, vector2I.x, vector2I.y), 0, -2), Grid.SceneLayer.Background);
		CameraController.Instance.SetMaxOrthographicSize(40f);
		CameraController.Instance.SnapTo(pos);
		CameraController.Instance.SetTargetPos(pos, 20f, false);
		CameraController.Instance.OrthographicSize = 40f;
		CameraSaveData.valid = false;
		return vector2I;
	}

	// Token: 0x060060D3 RID: 24787 RVA: 0x00239CEC File Offset: 0x00237EEC
	protected override void OnActivate()
	{
		if (this.disabledUIElements != null)
		{
			foreach (CanvasGroup canvasGroup in this.disabledUIElements)
			{
				if (canvasGroup != null)
				{
					canvasGroup.interactable = false;
				}
			}
		}
		NewBaseScreen.SetInitialCamera();
		if (SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Unpause(false);
		}
		this.Final();
	}

	// Token: 0x060060D4 RID: 24788 RVA: 0x00239D4D File Offset: 0x00237F4D
	public void Init(Cluster clusterLayout, ITelepadDeliverable[] startingMinionStats)
	{
		this.m_clusterLayout = clusterLayout;
		this.m_minionStartingStats = startingMinionStats;
	}

	// Token: 0x060060D5 RID: 24789 RVA: 0x00239D60 File Offset: 0x00237F60
	protected override void OnDeactivate()
	{
		Game.Instance.Trigger(-122303817, null);
		if (this.disabledUIElements != null)
		{
			foreach (CanvasGroup canvasGroup in this.disabledUIElements)
			{
				if (canvasGroup != null)
				{
					canvasGroup.interactable = true;
				}
			}
		}
	}

	// Token: 0x060060D6 RID: 24790 RVA: 0x00239DB0 File Offset: 0x00237FB0
	public override void OnKeyDown(KButtonEvent e)
	{
		global::Action[] array = new global::Action[4];
		RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.46E7A7E6CE942EAE1E13925BEDED6E6321F99918099A108FDB32BB9510B8E88D).FieldHandle);
		global::Action[] array2 = array;
		if (!e.Consumed)
		{
			int num = 0;
			while (num < array2.Length && !e.TryConsume(array2[num]))
			{
				num++;
			}
		}
	}

	// Token: 0x060060D7 RID: 24791 RVA: 0x00239DF0 File Offset: 0x00237FF0
	private void Final()
	{
		SpeedControlScreen.Instance.Unpause(false);
		GameObject telepad = GameUtil.GetTelepad(ClusterManager.Instance.GetStartWorld().id);
		if (telepad)
		{
			this.SpawnMinions(telepad);
		}
		Game.Instance.baseAlreadyCreated = true;
		this.Deactivate();
	}

	// Token: 0x060060D8 RID: 24792 RVA: 0x00239E40 File Offset: 0x00238040
	private void SpawnMinions(GameObject start_pad)
	{
		int num = Grid.PosToCell(start_pad);
		if (num == -1)
		{
			global::Debug.LogWarning("No headquarters in saved base template. Cannot place minions. Confirm there is a headquarters saved to the base template, or consider creating a new one.");
			return;
		}
		int num2;
		int num3;
		Grid.CellToXY(num, out num2, out num3);
		if (Grid.WidthInCells < 64)
		{
			return;
		}
		int baseLeft = this.m_clusterLayout.currentWorld.BaseLeft;
		int baseRight = this.m_clusterLayout.currentWorld.BaseRight;
		Db.Get().effects.Get("AnewHope");
		Telepad component = start_pad.GetComponent<Telepad>();
		for (int i = 0; i < this.m_minionStartingStats.Length; i++)
		{
			MinionStartingStats minionStartingStats = (MinionStartingStats)this.m_minionStartingStats[i];
			int x = num2 + i % (baseRight - baseLeft) + 1;
			int y = num3;
			int cell = Grid.XYToCell(x, y);
			GameObject prefab = Assets.GetPrefab(BaseMinionConfig.GetMinionIDForModel(minionStartingStats.personality.model));
			GameObject gameObject = Util.KInstantiate(prefab, null, null);
			gameObject.name = prefab.name;
			Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
			gameObject.transform.SetLocalPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
			gameObject.SetActive(true);
			minionStartingStats.Apply(gameObject);
			if (component != null)
			{
				component.AddNewBaseMinion(gameObject, minionStartingStats.personality.model == GameTags.Minions.Models.Bionic);
			}
		}
		component.ScheduleNewBaseEvents();
		ClusterManager.Instance.activeWorld.SetDupeVisited();
	}

	// Token: 0x040040AC RID: 16556
	public static NewBaseScreen Instance;

	// Token: 0x040040AD RID: 16557
	[SerializeField]
	private CanvasGroup[] disabledUIElements;

	// Token: 0x040040AE RID: 16558
	public EventReference ScanSoundMigrated;

	// Token: 0x040040AF RID: 16559
	public EventReference BuildBaseSoundMigrated;

	// Token: 0x040040B0 RID: 16560
	private ITelepadDeliverable[] m_minionStartingStats;

	// Token: 0x040040B1 RID: 16561
	private Cluster m_clusterLayout;
}
