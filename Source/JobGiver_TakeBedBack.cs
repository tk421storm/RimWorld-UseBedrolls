using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using RimWorld;

namespace UseBedrolls
{
	class JobGiver_TakeBedBack : ThinkNode_JobGiver
	{

		public override float GetPriority(Pawn pawn)
		{
			JobGiver_GetRest rester = pawn.thinker.GetMainTreeThinkNode<JobGiver_GetRest>();
			return (8f - (rester?.GetPriority(pawn) ?? 0f));
		}

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			PlacedBedsMapComponent component = pawn.Map.GetComponent<PlacedBedsMapComponent>();
			if (component.PlacedBed(pawn, out Building_Bed bed))
				{
				LocalTargetInfo local = bed.InteractionCell;
						//double bedrolls - pawn can reserve even if another pawn is sleeping in it
						if (bed.CurOccupants.Count() == 0 && pawn.CanReserveAndReach(local, PathEndMode.Touch, Danger.Some))
						{
							Log.Message($"{pawn} taking back bed {bed}");
							return base.TryIssueJobPackage(pawn, jobParams);
						}
					}
			
			return ThinkResult.NoJob;
			}

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.IsColonistPlayerControlled) return null;

			PlacedBedsMapComponent component = pawn.Map.GetComponent<PlacedBedsMapComponent>();
			if (component.PlacedBed(pawn, out Building_Bed bed)) 
			{ 
				return new Job(JobDefOf.TakeBedroll, bed);
			}
			return null;
		}
	}
}
