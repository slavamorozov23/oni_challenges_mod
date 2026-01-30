using System;
using System.Collections.Generic;

// Token: 0x02000BD2 RID: 3026
public class RocketProcessConditionDisplayTarget : KMonoBehaviour, IProcessConditionSet, ISim1000ms
{
	// Token: 0x06005AAF RID: 23215 RVA: 0x0020DB5E File Offset: 0x0020BD5E
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		if (this.craftModuleInterface == null)
		{
			this.craftModuleInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		}
		return this.craftModuleInterface.GetConditionSet(conditionType);
	}

	// Token: 0x06005AB0 RID: 23216 RVA: 0x0020DB8B File Offset: 0x0020BD8B
	public int PopulateConditionSet(ProcessCondition.ProcessConditionType conditionType, List<ProcessCondition> conditions)
	{
		if (this.craftModuleInterface == null)
		{
			this.craftModuleInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		}
		return this.craftModuleInterface.PopulateConditionSet(conditionType, conditions);
	}

	// Token: 0x06005AB1 RID: 23217 RVA: 0x0020DBBC File Offset: 0x0020BDBC
	public void Sim1000ms(float dt)
	{
		bool flag = false;
		List<ProcessCondition> list;
		using (ProcessCondition.ListPool.Get(out list))
		{
			this.PopulateConditionSet(ProcessCondition.ProcessConditionType.All, list);
			using (List<ProcessCondition>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.EvaluateCondition() == ProcessCondition.Status.Failure)
					{
						flag = true;
						if (this.statusHandle == Guid.Empty)
						{
							this.statusHandle = this.kselectable.AddStatusItem(Db.Get().BuildingStatusItems.RocketChecklistIncomplete, null);
							break;
						}
						break;
					}
				}
			}
		}
		if (!flag && this.statusHandle != Guid.Empty)
		{
			this.kselectable.RemoveStatusItem(this.statusHandle, false);
		}
	}

	// Token: 0x04003C7B RID: 15483
	private CraftModuleInterface craftModuleInterface;

	// Token: 0x04003C7C RID: 15484
	[MyCmpReq]
	private KSelectable kselectable;

	// Token: 0x04003C7D RID: 15485
	private Guid statusHandle = Guid.Empty;
}
