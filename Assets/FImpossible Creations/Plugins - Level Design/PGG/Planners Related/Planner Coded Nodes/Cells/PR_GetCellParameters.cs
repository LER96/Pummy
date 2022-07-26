using FIMSpace.Graph;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FIMSpace.Generating.Planning.PlannerNodes.Cells
{

    public class PR_GetCellParameters : PlannerRuleBase
    {
        public override string GetDisplayName(float maxWidth = 120) { return wasCreated ? "Cell Parameters" : "Get Cell Parameters"; }
        public override string GetNodeTooltipDescription { get { return "Accessing some parameters of provided cell"; } }
        public override Color GetNodeColor() { return new Color(0.64f, 0.9f, 0.0f, 0.9f); }
        public override Vector2 NodeSize { get { return new Vector2(168, 122); } }
        public override bool DrawInputConnector { get { return false; } }
        public override bool DrawOutputConnector { get { return false; } }
        public override bool IsFoldable { get { return false; } }

        public override EPlannerNodeType NodeType { get { return EPlannerNodeType.ReadData; } }

        [Port(EPortPinType.Input)] public PGGCellPort Cell;
        [Port(EPortPinType.Output, EPortNameDisplay.Default, EPortValueDisplay.HideValue)] public PGGVector3Port WorldPos;
        [Port(EPortPinType.Output, EPortNameDisplay.Default, EPortValueDisplay.HideValue)] public PGGPlannerPort Owner;
        Vector3 read = Vector3.zero;
        public override void OnStartReadingNode()
        {
            Cell.TriggerReadPort(true);

            if (FGenerators.CheckIfExist_NOTNULL(Cell.GetInputCellValue))
                if (FGenerators.CheckIfExist_NOTNULL(Cell.GetInputCheckerValue))
                {
                    read = Cell.GetInputCheckerValue.GetWorldPos(Cell.GetInputCellValue);
                    WorldPos.Value = Cell.GetInputCheckerValue.GetWorldPos(Cell.GetInputCellValue);
                    if (FGenerators.CheckIfExist_NOTNULL(Cell.GetInputResultValue))
                        Owner.SetIDsOfPlanner(Cell.GetInputResultValue.ParentFieldPlanner);
                }
        }

#if UNITY_EDITOR
        public override void Editor_OnAdditionalInspectorGUI()
        {
            EditorGUILayout.LabelField("Debugging:", EditorStyles.helpBox);

            if (Cell.GetInputCellValue == null) EditorGUILayout.LabelField("NULL CELL!");
            else
            {
                EditorGUILayout.LabelField("Out Value: " + read);
            }
        }
#endif

    }
}