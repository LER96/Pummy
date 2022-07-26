using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.Generating
{
    [System.Serializable]
    public partial class TileMeshSetup
    {
        public string Name = "Tile Mesh";

        public TileMeshSetup(string name = "")
        {
            if (name != "") Name = name;
            _lathe_fillAngle = 360;
        }


        public static TileMeshSetup _CopyRef = null;
        public static bool _CopyInstances = false;

        public void PasteMainTo(TileMeshSetup to)
        {
            to.Name = Name;
            to.Material = Material;
        }

        public void PasteParametersTo(TileMeshSetup to)
        {
            to.Origin = Origin;
            to.GenTechnique = GenTechnique;
            to.UVFit = UVFit;
            to.UVMul = UVMul;
            to.HardNormals = HardNormals;
            to.SubdivMode = SubdivMode;

            to.width = width;
            to.height = height;
            to.depth = depth;

            if (GenTechnique == EMeshGenerator.Loft)
            {
                to._loftDepthCurveWidener = _loftDepthCurveWidener;
                to._loft_DepthSubdivLimit = _loft_DepthSubdivLimit;
                to._loft_DistribSubdivLimit = _loft_DistribSubdivLimit;
            }
            else if (GenTechnique == EMeshGenerator.Lathe)
            {
                to._lathe_fillAngle = _lathe_fillAngle;
                to._lathe_xSubdivCount = _lathe_xSubdivCount;
                to._lathe_ySubdivLimit = _lathe_ySubdivLimit;
            }
            else if (GenTechnique == EMeshGenerator.Extrude)
            {
                to._extrude_SubdivLimit = _extrude_SubdivLimit;
                to._extrudeMirror = _extrudeMirror;
                to._extrudeFrontCap = _extrudeFrontCap;
                to._extrudeBackCap = _extrudeBackCap;
            }
        }


        public void PasteCurvesTo(TileMeshSetup to)
        {
            CurvePoint.CopyListFromTo(_loft_depth, to._loft_depth);
            CurvePoint.CopyListFromTo(_loft_distribute, to._loft_distribute);
            CurvePoint.CopyListFromTo(_lathe_points, to._lathe_points);
            CurvePoint.CopyListFromTo(_extrude_curve, to._extrude_curve);
        }

        public void PasteAllSetupTo(TileMeshSetup to, bool copyInstances = false)
        {
            PasteMainTo(to);
            PasteParametersTo(to);
            PasteCurvesTo(to);

            if (copyInstances)
            {
                to._instances.Clear();
                for (int i = 0; i < _instances.Count; i++)
                {
                    TileMeshCombineInstance inst = _instances[i].Copy();
                    to._instances.Add(inst);
                }
            }

            _CopyInstances = false;
        }


        public bool DrawSnappingPX()
        {
            return GenTechnique != EMeshGenerator.CustomMesh;
        }

        public bool DrawMeshOptions()
        {
            return GenTechnique != EMeshGenerator.CustomMesh;
        }


    }
}