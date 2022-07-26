#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace FIMSpace.Generating
{
    public class TilePreviewWindow : UnityEditor.Editor
    {
        private PreviewRenderUtility _previewRenderUtility;
        private Mesh _mesh;

        [SerializeField, HideInInspector] private Material _customMaterial = null;
        [SerializeField, HideInInspector] private Material _previewMaterial;

        public Material PreviewMaterial
        {
            get
            {
                if (designPreview != null) if (designPreview.DefaultMaterial != null) return designPreview.DefaultMaterial;
                if (_customMaterial != null) return _customMaterial;
                if (_previewMaterial == null) _previewMaterial = UnityDefaultDiffuseMaterial;
                return _previewMaterial;
            }
        }

        public static Material UnityDefaultDiffuseMaterial { get { return new Material(Shader.Find("Diffuse")); } }
        private static Material _unityWireframeMaterial = null;
        private static Material UnityWireframeMaterial { get { if (_unityWireframeMaterial == null) _unityWireframeMaterial = new Material(Shader.Find("VR/SpatialMapping/Wireframe")); return _unityWireframeMaterial; } }


        #region Editor Window Setup

        public override bool HasPreviewGUI()
        {
            ValidateData();
            return true;
        }

        public override bool RequiresConstantRepaint()
        {
            return true;
        }


        void OnDestroy()
        {
            if (_previewRenderUtility != null) _previewRenderUtility.Cleanup();
        }

        private void OnDisable()
        {
            if (_previewRenderUtility != null) _previewRenderUtility.Cleanup();
        }


        #endregion

        public void UpdateMesh(Mesh m)
        {
            designPreview = null;
            _mesh = m;
        }

        public void SetMaterial(Material m)
        {
            _customMaterial = m;
        }

        [NonSerialized] TileDesign designPreview = null;
        public void UpdateMesh(TileDesign editedDesign)
        {
            if (editedDesign.IsSomethingGenerated == false) return;
            designPreview = editedDesign;
        }


        private void ValidateData()
        {
            if (_previewRenderUtility == null)
            {
                _previewRenderUtility = new PreviewRenderUtility();
                _previewRenderUtility.camera.orthographic = false;
                _previewRenderUtility.camera.fieldOfView = 50f;
                _previewRenderUtility.camera.nearClipPlane = 0.001f;
                _previewRenderUtility.camera.farClipPlane = 1000f;
                _previewRenderUtility.lights[0].transform.rotation *= Quaternion.Euler(40f, 160f, 0f);
                _previewRenderUtility.lights[0].shadows = LightShadows.Hard;
                _previewRenderUtility.lights[0].shadowStrength = 1f;
                _previewRenderUtility.camera.transform.position = new Vector3(0, 1, -7);
                _previewRenderUtility.camera.transform.rotation = Quaternion.Euler(-12, 155, 0);
            }

            if (_mesh == null) _mesh = target as Mesh;
        }

        Vector2 sphericCamRot = new Vector2(-12, 155);
        float camDistance = 1f;

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            #region Refresh

            if (_previewRenderUtility == null)
            {
                ValidateData();
                return;
            }

            #endregion

            if (_mesh == null) return;

            camDistance = Mathf.Clamp(camDistance, 0.6f, 1.75f);
            sphericCamRot.x = Mathf.Clamp(sphericCamRot.x, -60f, 60f);

            Quaternion camRot = Quaternion.Euler(sphericCamRot);

            Vector3 newPos = _mesh.bounds.center;

            if (designPreview != null)
            {
                newPos = designPreview.GetFullBounds().center;
            }

            newPos += camRot * (Vector3.back * (1f + _mesh.bounds.size.magnitude) * camDistance);

            _previewRenderUtility.camera.transform.position = newPos;
            _previewRenderUtility.camera.transform.rotation = camRot;

            if (Event.current.type == EventType.Repaint)
            {
                _previewRenderUtility.BeginPreview(r, background);


                if (designPreview != null)
                {
                    for (int i = 0; i < designPreview.LatestGeneratedMeshes.Count; i++)
                    {
                        _previewRenderUtility.DrawMesh(designPreview.LatestGeneratedMeshes[i], Matrix4x4.identity, designPreview.LatestGeneratedMeshesMaterials[i], 0);
                    }
                }
                else if (_mesh)
                {
                    _previewRenderUtility.DrawMesh(_mesh, Matrix4x4.identity, PreviewMaterial, 0);
                }

                _previewRenderUtility.camera.Render();

                Texture resultRender = _previewRenderUtility.EndPreview();
                GUI.DrawTexture(r, resultRender, ScaleMode.StretchToFill, false);
            }

            #region Input events

            bool mouseContained = r.Contains(Event.current.mousePosition);

            if (Event.current.type == EventType.ScrollWheel)
            {
                if (Event.current.delta.y > 0)
                    camDistance += 0.1f;
                else
                if (Event.current.delta.y < 0)
                    camDistance -= 0.1f;

                Event.current.Use();
            }
            else if (Event.current.type == EventType.MouseDrag)
            {
                sphericCamRot.x += Event.current.delta.y;
                sphericCamRot.y += Event.current.delta.x;
                Event.current.Use();
            }

            #endregion

        }

    }
}

#endif