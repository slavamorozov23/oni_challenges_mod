using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000880 RID: 2176
[AddComponentMenu("KMonoBehaviour/scripts/CopyBuildingSettings")]
public class CopyBuildingSettings : KMonoBehaviour
{
	// Token: 0x06003BE1 RID: 15329 RVA: 0x0014F595 File Offset: 0x0014D795
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<CopyBuildingSettings>(493375141, CopyBuildingSettings.OnRefreshUserMenuDelegate);
	}

	// Token: 0x06003BE2 RID: 15330 RVA: 0x0014F5B0 File Offset: 0x0014D7B0
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_mirror", UI.USERMENUACTIONS.COPY_BUILDING_SETTINGS.NAME, new System.Action(this.ActivateCopyTool), global::Action.BuildingUtility1, null, null, null, UI.USERMENUACTIONS.COPY_BUILDING_SETTINGS.TOOLTIP, true), 1f);
	}

	// Token: 0x06003BE3 RID: 15331 RVA: 0x0014F60A File Offset: 0x0014D80A
	private void ActivateCopyTool()
	{
		CopySettingsTool.Instance.SetSourceObject(base.gameObject);
		PlayerController.Instance.ActivateTool(CopySettingsTool.Instance);
	}

	// Token: 0x06003BE4 RID: 15332 RVA: 0x0014F62C File Offset: 0x0014D82C
	public static bool ApplyCopy(int targetCell, GameObject sourceGameObject)
	{
		ObjectLayer layer = ObjectLayer.Building;
		if (sourceGameObject.GetComponent<MoverLayerOccupier>() != null)
		{
			layer = ObjectLayer.Mover;
		}
		Building component = sourceGameObject.GetComponent<BuildingComplete>();
		if (component != null)
		{
			layer = component.Def.ObjectLayer;
		}
		GameObject gameObject = Grid.Objects[targetCell, (int)layer];
		if (gameObject == null)
		{
			return false;
		}
		if (gameObject == sourceGameObject)
		{
			return false;
		}
		KPrefabID component2 = sourceGameObject.GetComponent<KPrefabID>();
		if (component2 == null)
		{
			return false;
		}
		KPrefabID component3 = gameObject.GetComponent<KPrefabID>();
		if (component3 == null)
		{
			return false;
		}
		CopyBuildingSettings component4 = sourceGameObject.GetComponent<CopyBuildingSettings>();
		if (component4 == null)
		{
			return false;
		}
		CopyBuildingSettings component5 = gameObject.GetComponent<CopyBuildingSettings>();
		if (component5 == null)
		{
			return false;
		}
		if (component4.copyGroupTag != Tag.Invalid)
		{
			if (component4.copyGroupTag != component5.copyGroupTag)
			{
				return false;
			}
		}
		else if (component3.PrefabID() != component2.PrefabID())
		{
			return false;
		}
		component3.Trigger(-905833192, sourceGameObject);
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, UI.COPIED_SETTINGS, gameObject.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
		return true;
	}

	// Token: 0x04002500 RID: 9472
	[MyCmpReq]
	private KPrefabID id;

	// Token: 0x04002501 RID: 9473
	public Tag copyGroupTag;

	// Token: 0x04002502 RID: 9474
	private static readonly EventSystem.IntraObjectHandler<CopyBuildingSettings> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<CopyBuildingSettings>(delegate(CopyBuildingSettings component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
