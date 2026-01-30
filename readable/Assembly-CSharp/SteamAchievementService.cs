using System;
using System.Diagnostics;
using Steamworks;
using UnityEngine;

// Token: 0x02000C2E RID: 3118
public class SteamAchievementService : MonoBehaviour
{
	// Token: 0x170006DF RID: 1759
	// (get) Token: 0x06005E3D RID: 24125 RVA: 0x002260C7 File Offset: 0x002242C7
	public static SteamAchievementService Instance
	{
		get
		{
			return SteamAchievementService.instance;
		}
	}

	// Token: 0x06005E3E RID: 24126 RVA: 0x002260D0 File Offset: 0x002242D0
	public static void Initialize()
	{
		if (SteamAchievementService.instance == null)
		{
			GameObject gameObject = GameObject.Find("/SteamManager");
			SteamAchievementService.instance = gameObject.GetComponent<SteamAchievementService>();
			if (SteamAchievementService.instance == null)
			{
				SteamAchievementService.instance = gameObject.AddComponent<SteamAchievementService>();
			}
		}
	}

	// Token: 0x06005E3F RID: 24127 RVA: 0x00226118 File Offset: 0x00224318
	public void Awake()
	{
		this.setupComplete = false;
		global::Debug.Assert(SteamAchievementService.instance == null);
		SteamAchievementService.instance = this;
	}

	// Token: 0x06005E40 RID: 24128 RVA: 0x00226137 File Offset: 0x00224337
	private void OnDestroy()
	{
		global::Debug.Assert(SteamAchievementService.instance == this);
		SteamAchievementService.instance = null;
	}

	// Token: 0x06005E41 RID: 24129 RVA: 0x0022614F File Offset: 0x0022434F
	private void Update()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (Game.Instance != null)
		{
			return;
		}
		if (!this.setupComplete && DistributionPlatform.Initialized)
		{
			this.Setup();
		}
	}

	// Token: 0x06005E42 RID: 24130 RVA: 0x0022617C File Offset: 0x0022437C
	private void Setup()
	{
		this.cbUserStatsReceived = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.OnUserStatsReceived));
		this.cbUserStatsStored = Callback<UserStatsStored_t>.Create(new Callback<UserStatsStored_t>.DispatchDelegate(this.OnUserStatsStored));
		this.cbUserAchievementStored = Callback<UserAchievementStored_t>.Create(new Callback<UserAchievementStored_t>.DispatchDelegate(this.OnUserAchievementStored));
		this.setupComplete = true;
		this.RefreshStats();
	}

	// Token: 0x06005E43 RID: 24131 RVA: 0x002261DB File Offset: 0x002243DB
	private void RefreshStats()
	{
		SteamUserStats.RequestCurrentStats();
	}

	// Token: 0x06005E44 RID: 24132 RVA: 0x002261E3 File Offset: 0x002243E3
	private void OnUserStatsReceived(UserStatsReceived_t data)
	{
		if (data.m_eResult != EResult.k_EResultOK)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"OnUserStatsReceived",
				data.m_eResult,
				data.m_steamIDUser
			});
			return;
		}
	}

	// Token: 0x06005E45 RID: 24133 RVA: 0x0022621E File Offset: 0x0022441E
	private void OnUserStatsStored(UserStatsStored_t data)
	{
		if (data.m_eResult != EResult.k_EResultOK)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"OnUserStatsStored",
				data.m_eResult
			});
			return;
		}
	}

	// Token: 0x06005E46 RID: 24134 RVA: 0x0022624B File Offset: 0x0022444B
	private void OnUserAchievementStored(UserAchievementStored_t data)
	{
	}

	// Token: 0x06005E47 RID: 24135 RVA: 0x00226250 File Offset: 0x00224450
	public void Unlock(string achievement_id)
	{
		bool flag = SteamUserStats.SetAchievement(achievement_id);
		global::Debug.LogFormat("SetAchievement {0} {1}", new object[]
		{
			achievement_id,
			flag
		});
		bool flag2 = SteamUserStats.StoreStats();
		global::Debug.LogFormat("StoreStats {0}", new object[]
		{
			flag2
		});
	}

	// Token: 0x06005E48 RID: 24136 RVA: 0x002262A0 File Offset: 0x002244A0
	[Conditional("UNITY_EDITOR")]
	[ContextMenu("Reset All Achievements")]
	private void ResetAllAchievements()
	{
		bool flag = SteamUserStats.ResetAllStats(true);
		global::Debug.LogFormat("ResetAllStats {0}", new object[]
		{
			flag
		});
		if (flag)
		{
			this.RefreshStats();
		}
	}

	// Token: 0x04003EA2 RID: 16034
	private Callback<UserStatsReceived_t> cbUserStatsReceived;

	// Token: 0x04003EA3 RID: 16035
	private Callback<UserStatsStored_t> cbUserStatsStored;

	// Token: 0x04003EA4 RID: 16036
	private Callback<UserAchievementStored_t> cbUserAchievementStored;

	// Token: 0x04003EA5 RID: 16037
	private bool setupComplete;

	// Token: 0x04003EA6 RID: 16038
	private static SteamAchievementService instance;
}
