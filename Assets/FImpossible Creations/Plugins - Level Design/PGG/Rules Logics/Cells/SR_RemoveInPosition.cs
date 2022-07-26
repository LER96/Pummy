#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace FIMSpace.Generating.Rules.Cells.Legacy
{
    public class SR_RemoveInPosition : SpawnRuleBase, ISpawnProcedureType
    {
        public EProcedureType Type { get { return EProcedureType.OnConditionsMet; } }
        public override string TitleName() { return "Remove In Position"; }
        public override string Tooltip() { return "Removing spawns at some distance to desired point"; }


        [PGG_SingleLineSwitch("CheckMode", 68, "Select if you want to use Tags, SpawnStigma or CellData", 90)]
        public string MustHaveTag = "";
        [HideInInspector] public ESR_Details CheckMode = ESR_Details.Tag;

        [Space(3)] public ESR_DirectionMode CellOffsetMode = ESR_DirectionMode.NoOffset;
        public Vector3Int OffsetCell = Vector3Int.zero;

        [Space(6)] public ESR_DirectionMode UseSelfRotation = ESR_DirectionMode.WorldDirection;
        [PGG_SingleLineSwitch("OffsetMode", 58, "Select if you want to offset postion with cell size or world units", 140)]
        public Vector3 PointPositionOffset = Vector3.zero;
        [HideInInspector] public ESR_Measuring OffsetMode = ESR_Measuring.Units;

        [Space(3)] public ESR_DistanceRule DistanceMustBe = ESR_DistanceRule.Lower;
        public float RemoveDistance = 0.1f;


#if UNITY_EDITOR
        public override void NodeHeader()
        {
            GUIIgnore.Clear();
            if (CellOffsetMode == ESR_DirectionMode.NoOffset) GUIIgnore.Add("OffsetCell");

            base.NodeHeader();
        }
#endif

        public override void OnConditionsMetAction(FieldModification mod, ref SpawnData thisSpawn, FieldSetup preset, FieldCell cell, FGenGraph<FieldCell, FGenPoint> grid)
        {
            var targetCell = cell;

            if (CellOffsetMode != ESR_DirectionMode.NoOffset) if (OffsetCell != Vector3Int.zero)
                {
                    Vector3Int off = OffsetCell;
                    if (CellOffsetMode == ESR_DirectionMode.CellRotateDirection) off = GetOffset(thisSpawn.GetRotationOffset(), off);
                    targetCell = grid.GetCell(cell.Pos + off, false);
                }

            if (FGenerators.CheckIfIsNull(targetCell )) return;

            Vector3 thisPos = thisSpawn.GetPosWithFullOffset(true);

            if (UseSelfRotation != ESR_DirectionMode.NoOffset)
            {
                Vector3 fullOffset;

                if (UseSelfRotation == ESR_DirectionMode.WorldDirection)
                    fullOffset = PointPositionOffset;
                else
                    fullOffset = thisSpawn.GetRotationOffset() * PointPositionOffset;

                fullOffset = GetUnitOffset(fullOffset, OffsetMode, preset);
                thisPos += fullOffset;
            }

            var spawns = targetCell.CollectSpawns(OwnerSpawner.ScaleAccess);

            for (int s = 0; s < spawns.Count; s++)
            {
                if (spawns[s].OwnerMod == null) continue;
                if (spawns[s] == thisSpawn) continue;

                if (string.IsNullOrEmpty(MustHaveTag) == false) // If spawns must have ceratain tags
                {
                    if (SpawnHaveSpecifics(spawns[s], MustHaveTag, CheckMode) == false) // Not found required tags then skip this spawn
                        continue;
                }

                Vector3 spawnPos = spawns[s].GetPosWithFullOffset(true);
                float distance = Vector3.Distance(thisPos, spawnPos);

                if (DistanceMustBe == ESR_DistanceRule.Equal)
                {
                    if (distance != RemoveDistance)
                        continue;
                }
                else if (DistanceMustBe == ESR_DistanceRule.Greater)
                {
                    if (distance < RemoveDistance)
                        continue;
                }
                else if (DistanceMustBe == ESR_DistanceRule.Lower)
                {
                    if (distance > RemoveDistance)
                        continue;
                }

                // All requirements met then remove tagged
                spawns[s].Enabled = false;
                targetCell.RemoveSpawnFromCell(spawns[s]);

                //return;
            }

            //Debug.DrawRay(thisPos, Vector3.up, Color.red, 1.1f);
        }

    }
}