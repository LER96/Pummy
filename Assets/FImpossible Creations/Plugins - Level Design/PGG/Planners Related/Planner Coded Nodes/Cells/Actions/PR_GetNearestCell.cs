using FIMSpace.Generating.Checker;
using FIMSpace.Graph;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FIMSpace.Generating.Planning.PlannerNodes.Cells.Actions
{

    public class PR_GetNearestCell : PlannerRuleBase
    {
        public override string GetDisplayName(float maxWidth = 120) { return "Get Nearest Cell"; }
        public override string GetNodeTooltipDescription { get { return "Trying to find nearest cell from one field to another"; } }
        public override Color GetNodeColor() { return new Color(0.64f, 0.9f, 0.0f, 0.9f); }
        public override Vector2 NodeSize { get { return new Vector2(188, _EditorFoldout ? 164 : 142); } }
        public override bool DrawInputConnector { get { return false; } }
        //public override int OutputConnectionIndex { get { return 0; } } 
        //public override string GetOutputHelperText(int outputId = 0) { return "On Read Values"; }
        public override bool DrawOutputConnector { get { return false; } }
        public override bool IsFoldable { get { return true; } }

        public override EPlannerNodeType NodeType { get { return EPlannerNodeType.ReadData; } }

        [Port(EPortPinType.Input)] public PGGPlannerPort A;
        [Port(EPortPinType.Input)] public PGGPlannerPort B;
        [Port(EPortPinType.Output)] public PGGCellPort NearestACell;
        [Port(EPortPinType.Output)] public PGGCellPort NearestBCell;
        //[Port(EPortPinType.Output, EPortNameDisplay.Default, EPortValueDisplay.HideValue)] public PGGVector3Port DirAtoB;
        [HideInInspector] [Tooltip("Can find nearest cells much faster (for grids with very big count of the cells) but will less precision")] [Port(EPortPinType.Input)] public BoolPort FastCheck;

        public override void OnCreated()
        {
            base.OnCreated();
            //FindCentered.Value = true;
        }

        public override void OnStartReadingNode()
        {
            A.TriggerReadPort(true);
            B.TriggerReadPort(true);
            var fieldA = GetPlannerFromPort(A, false);
            var fieldB = GetPlannerFromPort(B, false);

            if (fieldA == null) return;
            if (fieldB == null) return;

            if (fieldA == fieldB) return;

            CheckerField3D chA = fieldA.LatestChecker;
            if (chA == null || chA.ChildPositionsCount < 1) return;

            CheckerField3D chB = fieldB.LatestChecker;
            if (chB == null || chB.ChildPositionsCount < 1) return;

            NearestACell.ProvideFullCellData(chA.GetNearestCellTo(chB, FastCheck.GetInputValue), chA, fieldA.LatestResult);
            NearestBCell.ProvideFullCellData(chA._nearestCellOtherField, chB, fieldB.LatestResult);

            if (FGenerators.CheckIfIsNull(NearestACell.Cell)) return;
            if (FGenerators.CheckIfIsNull(NearestBCell.Cell)) return;

            //if (DirAtoB.PortState() != EPortPinState.Empty)
            //{
            //    DirAtoB.Value = (chB.GetWorldPos(NearestACell.CellData.CellRef) - chA.GetWorldPos(NearestBCell.Cell)).normalized.V3toV3Int();
            //}

            #region Debugging Backup
            //chA.DebugLogDrawCellInWorldSpace(NearestACell.CellRef, Color.green);
            //UnityEngine.Debug.DrawLine(chA.GetWorldPos(NearestACell.CellRef), chB.GetWorldPos(NearestBCell.CellRef), Color.green, 1.01f);
            //chB.DebugLogDrawCellInWorldSpace(NearestBCell.CellRef, Color.green);
            #endregion
        }


#if UNITY_EDITOR

        UnityEditor.SerializedProperty sp = null;

        public override void Editor_OnNodeBodyGUI(ScriptableObject setup)
        {
            A.DisplayVariableName = true;
            B.DisplayVariableName = true;

            base.Editor_OnNodeBodyGUI(setup);

            if (_EditorFoldout)
            {
                GUILayout.Space(4);
                if (sp == null) sp = baseSerializedObject.FindProperty("FastCheck");
                EditorGUILayout.PropertyField(sp, true);
            }
        }

        public override void Editor_OnAdditionalInspectorGUI()
        {
            EditorGUILayout.LabelField("Debugging:", EditorStyles.helpBox);
            GUILayout.Label("NearestACell: " + NearestACell.GetInputCellValue);
            GUILayout.Label("NearestBCell: " + NearestBCell.GetInputCellValue);
            //GUILayout.Label("DirAtoB: " + DirAtoB.GetPortValueSafe);
            //GUILayout.Label("DirAtoB: " + DirAtoB.GetPortValueSafe);
        }
#endif

    }
}