using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000604 RID: 1540
[AddComponentMenu("KMonoBehaviour/scripts/MingleCellTracker")]
public class MingleCellTracker : KMonoBehaviour, ISim1000ms
{
	// Token: 0x060023D5 RID: 9173 RVA: 0x000CF410 File Offset: 0x000CD610
	public void Sim1000ms(float dt)
	{
		this.mingleCells.Clear();
		RoomProber roomProber = Game.Instance.roomProber;
		MinionGroupProber minionGroupProber = MinionGroupProber.Get();
		foreach (Room room in roomProber.rooms)
		{
			if (room.roomType == Db.Get().RoomTypes.RecRoom)
			{
				for (int i = room.cavity.minY; i <= room.cavity.maxY; i++)
				{
					for (int j = room.cavity.minX; j <= room.cavity.maxX; j++)
					{
						int num = Grid.XYToCell(j, i);
						if (roomProber.GetCavityForCell(num) == room.cavity && minionGroupProber.IsReachable(num) && !Grid.HasLadder[num] && !Grid.HasTube[num] && !Grid.IsLiquid(num) && Grid.Element[num].id == SimHashes.Oxygen)
						{
							this.mingleCells.Add(num);
						}
					}
				}
			}
		}
	}

	// Token: 0x040014D7 RID: 5335
	public List<int> mingleCells = new List<int>();
}
