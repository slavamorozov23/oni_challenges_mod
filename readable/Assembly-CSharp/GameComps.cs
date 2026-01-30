using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x0200086C RID: 2156
public class GameComps : KComponents
{
	// Token: 0x06003B35 RID: 15157 RVA: 0x0014B2B8 File Offset: 0x001494B8
	public GameComps()
	{
		foreach (FieldInfo fieldInfo in typeof(GameComps).GetFields())
		{
			object obj = Activator.CreateInstance(fieldInfo.FieldType);
			fieldInfo.SetValue(null, obj);
			base.Add<IComponentManager>(obj as IComponentManager);
			if (obj is IKComponentManager)
			{
				IKComponentManager inst = obj as IKComponentManager;
				GameComps.AddKComponentManager(fieldInfo.FieldType, inst);
			}
		}
	}

	// Token: 0x06003B36 RID: 15158 RVA: 0x0014B32C File Offset: 0x0014952C
	public new void Clear()
	{
		FieldInfo[] fields = typeof(GameComps).GetFields();
		for (int i = 0; i < fields.Length; i++)
		{
			IComponentManager componentManager = fields[i].GetValue(null) as IComponentManager;
			if (componentManager != null)
			{
				componentManager.Clear();
			}
		}
	}

	// Token: 0x06003B37 RID: 15159 RVA: 0x0014B36F File Offset: 0x0014956F
	public static void AddKComponentManager(Type kcomponent, IKComponentManager inst)
	{
		GameComps.kcomponentManagers[kcomponent] = inst;
	}

	// Token: 0x06003B38 RID: 15160 RVA: 0x0014B37D File Offset: 0x0014957D
	public static IKComponentManager GetKComponentManager(Type kcomponent_type)
	{
		return GameComps.kcomponentManagers[kcomponent_type];
	}

	// Token: 0x0400248E RID: 9358
	public static GravityComponents Gravities;

	// Token: 0x0400248F RID: 9359
	public static FallerComponents Fallers;

	// Token: 0x04002490 RID: 9360
	public static InfraredVisualizerComponents InfraredVisualizers;

	// Token: 0x04002491 RID: 9361
	public static ElementSplitterComponents ElementSplitters;

	// Token: 0x04002492 RID: 9362
	public static OreSizeVisualizerComponents OreSizeVisualizers;

	// Token: 0x04002493 RID: 9363
	public static StructureTemperatureComponents StructureTemperatures;

	// Token: 0x04002494 RID: 9364
	public static DiseaseContainers DiseaseContainers;

	// Token: 0x04002495 RID: 9365
	public static RequiresFoundation RequiresFoundations;

	// Token: 0x04002496 RID: 9366
	public static WhiteBoard WhiteBoards;

	// Token: 0x04002497 RID: 9367
	private static Dictionary<Type, IKComponentManager> kcomponentManagers = new Dictionary<Type, IKComponentManager>();
}
