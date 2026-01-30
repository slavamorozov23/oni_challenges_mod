using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000924 RID: 2340
[AddComponentMenu("KMonoBehaviour/scripts/EntityConfigManager")]
public class EntityConfigManager : KMonoBehaviour
{
	// Token: 0x0600417B RID: 16763 RVA: 0x00171C14 File Offset: 0x0016FE14
	public static void DestroyInstance()
	{
		EntityConfigManager.Instance = null;
	}

	// Token: 0x0600417C RID: 16764 RVA: 0x00171C1C File Offset: 0x0016FE1C
	protected override void OnPrefabInit()
	{
		EntityConfigManager.Instance = this;
	}

	// Token: 0x0600417D RID: 16765 RVA: 0x00171C24 File Offset: 0x0016FE24
	private static int GetSortOrder(Type type)
	{
		foreach (Attribute attribute in type.GetCustomAttributes(true))
		{
			if (attribute.GetType() == typeof(EntityConfigOrder))
			{
				return (attribute as EntityConfigOrder).sortOrder;
			}
		}
		return 0;
	}

	// Token: 0x0600417E RID: 16766 RVA: 0x00171C74 File Offset: 0x0016FE74
	public void LoadGeneratedEntities(List<Type> types)
	{
		Type typeFromHandle = typeof(IEntityConfig);
		Type typeFromHandle2 = typeof(IMultiEntityConfig);
		List<EntityConfigManager.ConfigEntry> list = new List<EntityConfigManager.ConfigEntry>();
		foreach (Type type in types)
		{
			if ((typeFromHandle.IsAssignableFrom(type) || typeFromHandle2.IsAssignableFrom(type)) && !type.IsAbstract && !type.IsInterface)
			{
				int sortOrder = EntityConfigManager.GetSortOrder(type);
				EntityConfigManager.ConfigEntry item = new EntityConfigManager.ConfigEntry
				{
					type = type,
					sortOrder = sortOrder
				};
				list.Add(item);
			}
		}
		list.Sort((EntityConfigManager.ConfigEntry x, EntityConfigManager.ConfigEntry y) => x.sortOrder.CompareTo(y.sortOrder));
		foreach (EntityConfigManager.ConfigEntry configEntry in list)
		{
			object obj = Activator.CreateInstance(configEntry.type);
			if (obj is IEntityConfig)
			{
				IEntityConfig entityConfig = obj as IEntityConfig;
				string[] array = null;
				string[] array2 = null;
				if (entityConfig.GetDlcIds() != null)
				{
					DlcManager.ConvertAvailableToRequireAndForbidden(entityConfig.GetDlcIds(), out array, out array2);
					DebugUtil.DevLogError(string.Format("{0} implements GetDlcIds, which is obsolete.", configEntry.type));
				}
				else
				{
					IHasDlcRestrictions hasDlcRestrictions = obj as IHasDlcRestrictions;
					if (hasDlcRestrictions != null)
					{
						array = hasDlcRestrictions.GetRequiredDlcIds();
						array2 = hasDlcRestrictions.GetForbiddenDlcIds();
					}
				}
				if (DlcManager.IsCorrectDlcSubscribed(array, array2))
				{
					this.RegisterEntity(entityConfig, array, array2);
				}
			}
			IMultiEntityConfig multiEntityConfig = obj as IMultiEntityConfig;
			if (multiEntityConfig != null)
			{
				DebugUtil.Assert(!(obj is IHasDlcRestrictions), "IMultiEntityConfig cannot implement IHasDlcRestrictions, wrap the individual config instead.");
				this.RegisterEntities(multiEntityConfig);
			}
		}
	}

	// Token: 0x0600417F RID: 16767 RVA: 0x00171E4C File Offset: 0x0017004C
	[Conditional("UNITY_EDITOR")]
	private void ValidateEntityConfig(IEntityConfig entityConfig)
	{
		if (entityConfig == null)
		{
			throw new ArgumentNullException("entityConfig");
		}
		Type type = entityConfig.GetType();
		Type typeFromHandle = typeof(IHasDlcRestrictions);
		bool flag = type.GetMethod("GetRequiredDlcIds", Type.EmptyTypes) != null;
		bool flag2 = type.GetMethod("GetForbiddenDlcIds", Type.EmptyTypes) != null;
		bool flag3 = typeFromHandle.IsAssignableFrom(type);
		if ((flag || flag2) && !flag3)
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				type.Name + " is an IEntityConfig and has GetRequiredDlcIds or GetForbiddenDlcIds but does not implement IHasDlcRestrictions."
			});
		}
	}

	// Token: 0x06004180 RID: 16768 RVA: 0x00171ED4 File Offset: 0x001700D4
	[Conditional("UNITY_EDITOR")]
	private void ValidateMultiEntityConfig(IMultiEntityConfig entityConfig)
	{
		if (entityConfig == null)
		{
			throw new ArgumentNullException("entityConfig");
		}
		Type type = entityConfig.GetType();
		bool flag = type.GetMethod("GetRequiredDlcIds", Type.EmptyTypes) != null;
		bool flag2 = type.GetMethod("GetForbiddenDlcIds", Type.EmptyTypes) != null;
		if (flag || flag2)
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				type.Name + " is an IMultiEntityConfig and you shouldn't be specifying GetRequiredDlcIds or GetForbiddenDlcIds. Wrap each config in a DLC check instead."
			});
		}
	}

	// Token: 0x06004181 RID: 16769 RVA: 0x00171F48 File Offset: 0x00170148
	public void RegisterEntity(IEntityConfig config, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
	{
		GameObject gameObject = config.CreatePrefab();
		if (gameObject == null)
		{
			return;
		}
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.requiredDlcIds = requiredDlcIds;
		component.forbiddenDlcIds = forbiddenDlcIds;
		component.prefabInitFn += config.OnPrefabInit;
		component.prefabSpawnFn += config.OnSpawn;
		Assets.AddPrefab(component);
	}

	// Token: 0x06004182 RID: 16770 RVA: 0x00171FA8 File Offset: 0x001701A8
	public void RegisterEntities(IMultiEntityConfig config)
	{
		foreach (GameObject gameObject in config.CreatePrefabs())
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			component.prefabInitFn += config.OnPrefabInit;
			component.prefabSpawnFn += config.OnSpawn;
			Assets.AddPrefab(component);
		}
	}

	// Token: 0x040028E9 RID: 10473
	public static EntityConfigManager Instance;

	// Token: 0x02001921 RID: 6433
	private struct ConfigEntry
	{
		// Token: 0x04007D02 RID: 32002
		public Type type;

		// Token: 0x04007D03 RID: 32003
		public int sortOrder;
	}
}
