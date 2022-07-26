using FIMSpace.Generating.Checker;
using FIMSpace.Generating.Planning;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.Generating
{
    public static partial class IGeneration
    {
        /// <summary>
        /// Spawning data placed inside grid's cells 
        /// Returning list of objects with IGenerating interface implemented for post-generating events
        /// </summary>
        public static List<IGenerating> RunGraphSpawners(FGenGraph<FieldCell, FGenPoint> grid, Transform container, FieldSetup preset, List<GameObject> listToFillWithSpawns, List<GameObject> gatheredToCombine, List<GameObject> gatheredToStaticCombine, Matrix4x4? spawnMatrix = null)
        {
            List<IGenerating> generatorsCollected = new List<IGenerating>();

            Matrix4x4 matrix = Matrix4x4.identity;
            if (spawnMatrix != null) matrix = spawnMatrix.Value;

            FGenGraph<FieldCell, FGenPoint> runGraph;
            int subCount = 0;
            if (grid.SubGraphs != null) subCount = grid.SubGraphs.Count;
            for (int gr = -1; gr < subCount; gr++)
            {
                if (gr == -1) runGraph = grid; else runGraph = grid.SubGraphs[gr];

                for (int c = 0; c < runGraph.AllCells.Count; c++)
                {
                    var cell = runGraph.AllCells[c];
                    if (cell.IsGhostCell) continue;

                    var spawns = cell.GetSpawnsJustInsideCell();

                    for (int s = 0; s < spawns.Count; s++)
                    {
                        SpawnData spawn = spawns[s];

                        if (spawn.OnPreGeneratedEvents.Count != 0)
                            for (int pe = 0; pe < spawn.OnPreGeneratedEvents.Count; pe++)
                                spawn.OnPreGeneratedEvents[pe].Invoke(spawn);

                        if (spawn.Prefab != null || spawn.DontSpawnMainPrefab)
                        {
                            Transform targetContainer = spawn.GetModContainer(container);
                            if (targetContainer == null) targetContainer = container;

                            if (spawn.DontSpawnMainPrefab == false)
                            {
                                GameObject spawned = null;

                                if (spawn.idInStampObjects == -2)
                                {
                                    if (spawn.OwnerMod.DrawSetupFor == FieldModification.EModificationMode.ObjectsStamp)
                                    {
                                        spawned = new GameObject(spawn.OwnerMod.name);
                                        ObjectStampEmitter emitter = spawned.AddComponent<ObjectStampEmitter>();
                                        emitter.PrefabsSet = spawn.OwnerMod.OStamp;
                                        emitter.AlwaysDrawPreview = true;
                                    }
                                }
                                else if (spawn.idInStampObjects == -1)
                                {
                                    if (spawn.OwnerMod.DrawSetupFor == FieldModification.EModificationMode.ObjectMultiEmitter)
                                    {
                                        OStamperSet stamp = spawn.OwnerMod.OMultiStamp.PrefabsSets[FGenerators.GetRandom(0, spawn.OwnerMod.OMultiStamp.PrefabsSets.Count)];
                                        spawned = new GameObject(stamp.name);
                                        ObjectStampEmitter emitter = spawned.AddComponent<ObjectStampEmitter>();
                                        emitter.PrefabsSet = stamp;
                                        emitter.AlwaysDrawPreview = true;
                                    }
                                }

                                if (spawned == null)
                                {
                                    spawned = FGenerators.InstantiateObject(spawn.Prefab);
                                }

                                spawned.transform.SetParent(targetContainer, true);

                                Vector3 targetPosition = preset.GetCellWorldPosition(cell);

                                Quaternion rotation = spawn.Prefab.transform.rotation * Quaternion.Euler(spawn.RotationOffset);

                                spawned.transform.position = matrix.MultiplyPoint(targetPosition + spawn.Offset + rotation * spawn.DirectionalOffset);

                                if (spawn.LocalRotationOffset != Vector3.zero) rotation *= Quaternion.Euler(spawn.LocalRotationOffset);
                                spawned.transform.rotation = matrix.rotation * rotation;
                                spawned.transform.localScale = Vector3.Scale(spawn.LocalScaleMul, spawn.Prefab.transform.lossyScale);

                                if (spawn.ForceSetStatic)
                                {
                                    spawned.isStatic = true;
                                    if (spawned.transform.childCount > 0) foreach (Transform t in spawned.transform) t.gameObject.isStatic = true;
                                }

                                if (spawn.CombineMode == SpawnData.ECombineMode.Combine)
                                {
                                    gatheredToCombine.Add(spawned);
                                }
                                else
                                if (spawn.CombineMode == SpawnData.ECombineMode.CombineStatic)
                                {
                                    gatheredToStaticCombine.Add(spawned);
                                }


                                // Collecting generators
                                if (spawn.OwnerMod != null)
                                {
                                    if (spawned.transform.childCount > 0)
                                    {
                                        for (int ch = 0; ch < spawned.transform.childCount; ch++)
                                        {
                                            IGenerating[] emitters = spawned.transform.GetChild(ch).GetComponentsInChildren<IGenerating>();
                                            for (int i = 0; i < emitters.Length; i++)
                                            { generatorsCollected.Add(emitters[i]); }
                                        }
                                    }

                                    IGenerating emitter = spawned.GetComponent<IGenerating>();
                                    if (emitter != null) generatorsCollected.Add(emitter);
                                }

                                if (listToFillWithSpawns != null)
                                    listToFillWithSpawns.Add(spawned);

                                // Post Events support
                                if (spawn.OnGeneratedEvents.Count != 0)
                                    for (int pe = 0; pe < spawn.OnGeneratedEvents.Count; pe++)
                                        spawn.OnGeneratedEvents[pe].Invoke(spawned);
                            }
                            else
                            {
                                // Post Events support
                                if (spawn.OnGeneratedEvents.Count != 0)
                                    for (int pe = 0; pe < spawn.OnGeneratedEvents.Count; pe++)
                                        spawn.OnGeneratedEvents[pe].Invoke(null);
                            }


                            // Additional generated feature
                            if (spawn.AdditionalGenerated != null)
                            {
                                for (int sa = 0; sa < spawn.AdditionalGenerated.Count; sa++)
                                {
                                    var spwna = spawn.AdditionalGenerated[sa];

                                    if (spwna == null) continue;
                                    spwna.transform.SetParent(targetContainer, true);

                                    if (spwna.transform.childCount > 0)
                                    {
                                        for (int ch = 0; ch < spwna.transform.childCount; ch++)
                                        {
                                            IGenerating[] emitters = spwna.transform.GetChild(ch).GetComponentsInChildren<IGenerating>();
                                            for (int i = 0; i < emitters.Length; i++) generatorsCollected.Add(emitters[i]);
                                        }
                                    }

                                    IGenerating emitter = spwna.GetComponent<IGenerating>();
                                    if (emitter != null) generatorsCollected.Add(emitter);

                                    listToFillWithSpawns.Add(spwna);
                                }

                            }

                        }
                    }
                }

            }

            preset.ClearTemporaryContainers();

            return generatorsCollected;
        }


        /// <summary>
        /// Creating field cells grid, running field modificator rules and spawning game objects, returning them in list
        /// </summary>
        public static InstantiatedFieldInfo GenerateFieldObjectsRectangleGrid(FieldSetup preset, Vector3Int size, int seed, Transform container, bool runRules = true, List<SpawnInstruction> guides = null, bool runEmitters = false, Vector3Int? offset = null)
        {
            var grid = GetEmptyFieldGraph();
            grid.Generate(size.x, size.y, size.z, offset == null ? Vector3Int.zero : offset.Value);

            FGenerators.SetSeed(seed);

            return GenerateFieldObjects(preset, grid, container, runRules, guides, null, runEmitters);
        }


        public static InstantiatedFieldInfo GenerateFieldObjectsWithContainer(string name, FieldSetup setup, FGenGraph<FieldCell, FGenPoint> grid, Transform container, List<SpawnInstruction> guides = null, List<InjectionSetup> inject = null, Vector3? offset = null, bool runRules = true, bool runEmitters = true)
        {
            if (setup == null)
            {
                Debug.LogError("No assigned Field Setup in " + name + "!");
                return new InstantiatedFieldInfo();
            }

            Transform nContainer = new GameObject(name + " - Generated").transform;
            nContainer.SetParent(container);
            nContainer.ResetCoords();

            if (offset != Vector3.zero) offset = offset / setup.GetCellUnitSize().x;

            if (inject != null) setup.SetTemporaryInjections(inject); else setup.ClearTemporaryContainers();

            InstantiatedFieldInfo gen = IGeneration.GenerateFieldObjects(setup, grid, nContainer, runRules, guides, offset, runEmitters);

            if (inject != null) setup.ClearTemporaryInjections();

            gen.MainContainer = nContainer.gameObject;

            return gen;
        }


        public static InstantiatedFieldInfo GenerateFieldObjects(FieldSetup preset, FGenGraph<FieldCell, FGenPoint> grid, Transform container, bool runRules = true, List<SpawnInstruction> guides = null, Vector3? fieldOffset = null, bool runEmitters = true, CheckerField optionalUsedChecker = null)
        {
            // Prepare needed lists
            InstantiatedFieldInfo gen = new InstantiatedFieldInfo();
            gen.InternalField = preset;
            gen.MainContainer = null;
            gen.FieldTransform = container;
            gen.Grid = grid;

            gen.OptionalCheckerFieldsData = new List<CheckerField>();
            if (FGenerators.CheckIfExist_NOTNULL(optionalUsedChecker))
            {
                gen.OptionalCheckerFieldsData.Add(optionalUsedChecker);
                optionalUsedChecker.HelperReference = preset;
            }

            List<FieldCell> newCells = new List<FieldCell>();

            List<GameObject> combineNonStatic = new List<GameObject>();
            List<GameObject> combineStatic = new List<GameObject>();

            // Configure grid
            if (CheckIfScaledGraphsNeeded(preset, guides))
                if (preset._tempGraphScale2 == null || grid.SubGraphs == null)
                    preset.PrepareSubGraphs(grid);

            // Coordinates in world
            Vector3 pos = container.position;
            if (fieldOffset != null) pos += preset.TransformCellPosition(fieldOffset.Value);
            Matrix4x4 transformMatrix = Matrix4x4.TRS(pos, container.rotation, Vector3.one);


            #region Adding Guides to cells for additional usage

            if (guides != null)
            {
                for (int i = 0; i < guides.Count; i++)
                {
                    FieldCell cell = grid.GetCell(guides[i].gridPosition, false);
                    if (FGenerators.CheckIfExist_NOTNULL(cell))
                    {
                        if (cell.GuidesIn == null) cell.GuidesIn = new List<SpawnInstruction>();
                        cell.GuidesIn.Add(guides[i]);
                    }
                }
            }

            #endregion

            #region Refreshing self injections if used

            if (preset != null)
                if (preset.SelfInjections != null)
                    if (preset.SelfInjections.Count > 0)
                    {
                        if (preset.temporaryInjections == null)
                            preset.temporaryInjections = new List<InjectionSetup>();

                        for (int i = 0; i < preset.SelfInjections.Count; i++)
                            preset.temporaryInjections.Add(preset.SelfInjections[i]);
                    }

            #endregion


            PreparePresetVariables(preset);


            #region If using isolated grid injection then preparing separated grid for each definition

            Dictionary<InstructionDefinition, List<IGenerating>> isolatedGenerated = null;

            if (guides != null)
            {
                Dictionary<InstructionDefinition, FGenGraph<FieldCell, FGenPoint>> isolatedGrids = new Dictionary<InstructionDefinition, FGenGraph<FieldCell, FGenPoint>>();
                isolatedGenerated = new Dictionary<InstructionDefinition, List<IGenerating>>();

                for (int i = 0; i < preset.CellsInstructions.Count; i++)
                    if (preset.CellsInstructions[i].InstructionType == InstructionDefinition.EInstruction.IsolatedGrid)
                    {
                        if (preset.CellsInstructions[i].TargetModification == null) continue;
                        isolatedGrids.Add(preset.CellsInstructions[i], grid.CopyEmpty());
                        isolatedGenerated.Add(preset.CellsInstructions[i], new List<IGenerating>());
                    }

                // First fill all cells for all isolated grids
                bool any = false;
                for (int i = 0; i < guides.Count; i++)
                {
                    if (guides[i].definition == null) continue;
                    if (guides[i].definition.InstructionType != InstructionDefinition.EInstruction.IsolatedGrid) continue;
                    var iGrid = isolatedGrids[guides[i].definition];
                    if (iGrid == null) continue;
                    iGrid.AddCell(guides[i].gridPosition);
                    any = true;
                }

                if (any) // If any isolated process occured
                {
                    for (int i = 0; i < preset.CellsInstructions.Count; i++) // Running modificators on isolated grids
                        if (preset.CellsInstructions[i].InstructionType == InstructionDefinition.EInstruction.IsolatedGrid)
                        {
                            var mod = preset.CellsInstructions[i].TargetModification;
                            if (mod == null) continue;

                            var iGrid = isolatedGrids[preset.CellsInstructions[i]];
                            var randCells = GetRandomizedCells(iGrid);
                            var randCells2 = GetRandomizedCells(iGrid);

                            if (mod.VariantOf != null)
                                mod.VariantOf.ModifyGraph(preset, iGrid, randCells, randCells2, mod);
                            else
                                mod.ModifyGraph(preset, iGrid, randCells, randCells2);
                        }

                    for (int i = 0; i < preset.CellsInstructions.Count; i++) // Getting spawn datas for all isolated grids and guides 
                        if (preset.CellsInstructions[i].InstructionType == InstructionDefinition.EInstruction.IsolatedGrid)
                        {
                            var mod = preset.CellsInstructions[i].TargetModification;
                            if (mod == null) continue;

                            var iGrid = isolatedGrids[preset.CellsInstructions[i]];
                            isolatedGenerated[preset.CellsInstructions[i]] = RunGraphSpawners(iGrid, container, preset, gen.Instantiated, combineNonStatic, combineStatic, transformMatrix);
                        }
                }
            }

            #endregion


            if (runRules)
            {
                #region Preparing grid 

                var randCells = GetRandomizedCells(grid);
                var randCells2 = GetRandomizedCells(grid);

                #endregion

                // First -> Running spawning guides -> Doors / Door Holes / Pre spawners
                if (guides != null) preset.RunPreInstructionsOnGraph(grid, guides);

                #region Temporary Pre Injections

                if (preset.temporaryInjections != null)
                    for (int i = 0; i < preset.temporaryInjections.Count; i++)
                    {
                        if (preset.temporaryInjections[i].Call == InjectionSetup.EGridCall.Pre)
                        {
                            if (preset.temporaryInjections[i].Inject == InjectionSetup.EInjectTarget.Modificator)
                            {
                                if (preset.temporaryInjections[i].Modificator != null)
                                    preset.RunModificatorOnGrid(grid, randCells, randCells2, preset.temporaryInjections[i].Modificator, false);
                            }
                            else if (preset.temporaryInjections[i].Inject == InjectionSetup.EInjectTarget.Pack)
                            {
                                if (preset.temporaryInjections[i].ModificatorsPack != null)
                                    if (preset.temporaryInjections[i].ModificatorsPack.DisableWholePackage == false)
                                        preset.RunModificatorPackOn(preset.temporaryInjections[i].ModificatorsPack, grid, randCells, randCells2);
                            }
                        }

                    }

                #endregion

                preset.RunRulesOnGraph(grid, randCells, randCells2, guides);

                #region Filling with new cells for grid, Only with post modificator

                if (guides != null)
                    for (int i = 0; i < guides.Count; i++)
                        if (guides[i].IsModRunner)
                        {
                            FieldCell nCell = grid.GetCell(guides[i].gridPosition, false);

                            if (FGenerators.CheckIfIsNull(nCell))
                            {
                                nCell = grid.GetCell(guides[i].gridPosition, true);
                                newCells.Add(nCell);
                            }
                            nCell.InTargetGridArea = true;
                        }

                #endregion

                #region Temporary Post Injections

                if (preset.temporaryInjections != null)
                    for (int i = 0; i < preset.temporaryInjections.Count; i++)
                        if (preset.temporaryInjections[i].Call == InjectionSetup.EGridCall.Post)
                        {
                            if (preset.temporaryInjections[i].Inject == InjectionSetup.EInjectTarget.Modificator)
                            {
                                if (preset.temporaryInjections[i].Modificator != null)
                                {
                                    preset.RunModificatorOnGrid(grid, randCells, randCells2, preset.temporaryInjections[i].Modificator, false);
                                }
                            }
                            else if (preset.temporaryInjections[i].Inject == InjectionSetup.EInjectTarget.Pack)
                            {
                                if (preset.temporaryInjections[i].ModificatorsPack != null)
                                    if (preset.temporaryInjections[i].ModificatorsPack.DisableWholePackage == false)
                                    {
                                        preset.RunModificatorPackOn(preset.temporaryInjections[i].ModificatorsPack, grid, randCells, randCells2);
                                    }
                            }
                        }

                #endregion

                if (guides != null) preset.RunPostInstructionsOnGraph(grid, guides);
            }

            RestorePresetVariables(preset);

            gen.Instantiated = new List<GameObject>();
            List<IGenerating> generatorsSpawned = RunGraphSpawners(grid, container, preset, gen.Instantiated, combineNonStatic, combineStatic, transformMatrix);

            Bounds fullBounds = GetBounds(grid, gen.Instantiated, preset, transformMatrix, container.position);
            gen.RoomBounds = fullBounds;

            preset.PostEvents(ref gen, grid, fullBounds, container);


            #region Applying isolated grids to generators spawned list

            if (isolatedGenerated != null)
            {
                foreach (var item in isolatedGenerated)
                {
                    var spawns = item.Value;
                    if (spawns == null) continue;

                    for (int i = 0; i < spawns.Count; i++)
                    {
                        generatorsSpawned.Add(spawns[i]);
                    }
                }
            }

            #endregion


            // Running generators implementing IGenerating interface
            if (runEmitters)
            {
                for (int g = 0; g < generatorsSpawned.Count; g++)
                    generatorsSpawned[g].Generate();
            }
            else
            {
                for (int g = 0; g < generatorsSpawned.Count; g++)
                    generatorsSpawned[g].PreviewGenerate();
            }


            // Combine Meshes
            if (combineStatic.Count > 0)
            {
                GameObject combinedContainer = new GameObject("Combined-Static");
                combinedContainer.transform.SetParent(container);
                combinedContainer.transform.ResetCoords();
                GenerateCombinedMeshes(combineStatic, combinedContainer.transform, true);
                gen.Instantiated.Add(combinedContainer);
            }

            if (combineNonStatic.Count > 0)
            {
                GameObject combinedContainer = new GameObject("Combined-NonStatic");
                combinedContainer.transform.SetParent(container);
                combinedContainer.transform.ResetCoords();
                GenerateCombinedMeshes(combineNonStatic, combinedContainer.transform, false);
                gen.Instantiated.Add(combinedContainer);
            }


            // Restoring grid
            for (int i = 0; i < newCells.Count; i++)
            {
                grid.RemoveCell(newCells[i]);
            }

            //preset.ClearTemporaryContainers();

            if (preset.SelfInjections != null)
                if (preset.temporaryInjections != null)
                    for (int i = 0; i < preset.SelfInjections.Count; i++)
                        preset.temporaryInjections.Remove(preset.SelfInjections[i]);

            preset.AfterAllGenerating();

            return gen;
        }

        public static GameObject GenerateCombinedMeshes(List<GameObject> toCombineSearch, Transform putGeneratedIn, bool setStatic)
        {
            GameObject combinationObject = null;

            #region Collect meshes and instances, categorize by materials

            Dictionary<Material, List<MeshFilter>> materialMeshes = new System.Collections.Generic.Dictionary<Material, List<MeshFilter>>();
            List<MeshRenderer> searchRend = new List<MeshRenderer>();

            for (int i = 0; i < toCombineSearch.Count; i++)
            {
                searchRend.Clear();

                GameObject tile = toCombineSearch[i];

                //foreach (Transform t in tile.transform)
                foreach (Transform t in tile.transform.GetComponentsInChildren<Transform>())
                {
                    MeshRenderer m = t.GetComponent<MeshRenderer>();

                    if (m)
                        if (m.sharedMaterials.Length == 1)
                            if (m.sharedMaterials[0] != null) // Only single material renderers with not null materials
                            {
                                if (m.GetComponent<PGGIgnoreCombining>() == null) // Check if not ignoring this mesh 
                                {
                                    MeshFilter filter = m.gameObject.GetComponent<MeshFilter>();

                                    if (filter) if (filter.sharedMesh != null) // Mesh filter with not null mesh required
                                        {
                                            Material kMat = m.sharedMaterials[0];
                                            if (materialMeshes.ContainsKey(kMat) == false) materialMeshes.Add(kMat, new List<MeshFilter>());

                                            materialMeshes[kMat].Add(filter);
                                            if (setStatic) m.gameObject.isStatic = true;
                                            FGenerators.DestroyObject(m); // Clean ref to renderer component on the scene
                                        }
                                }
                            }
                }
            }

            #endregion


            #region Combining Meshes

            Mesh combined = null;
            List<CombineInstance> combination = new List<CombineInstance>();
            Matrix4x4 containerMX = putGeneratedIn.localToWorldMatrix.inverse;

            foreach (var item in materialMeshes)
            {
                combined = new Mesh();
                combination.Clear();

                if (item.Value.Count == 0) continue;

                for (int i = 0; i < item.Value.Count; i++)
                {
                    CombineInstance comb = new CombineInstance();
                    comb.mesh = item.Value[i].sharedMesh;

                    comb.transform = containerMX * item.Value[i].transform.localToWorldMatrix;
                    combination.Add(comb);
                }

                combined.CombineMeshes(combination.ToArray(), true, true, false);

                //if (weld) combined = FMeshUtils.Weld(combined, 0);
                //if (weld) combined = FMeshUtils.Welda(combined, 0.05f);
                //combined.Weld(combined.bounds.size.magnitude * 0.00001f, combined.bounds.size.magnitude * 0.0004f);

                combined.name = item.Key.name + "-Combined";

                // Put all generated combined objects in the target container
                GameObject combinedDrawer = new GameObject(combined.name);
                combinedDrawer.transform.SetParent(putGeneratedIn, true);
                combinedDrawer.transform.ResetCoords();
                combinedDrawer.tag = item.Value[0].gameObject.tag;
                combinedDrawer.layer = item.Value[0].gameObject.layer;
                combinedDrawer.isStatic = setStatic;

                MeshFilter combinedFilt = combinedDrawer.AddComponent<MeshFilter>();
                combinedFilt.sharedMesh = combined;
                MeshRenderer combinedRend = combinedDrawer.AddComponent<MeshRenderer>();
                combinedRend.sharedMaterial = item.Key;
                combinationObject = combinedDrawer;

                for (int i = 0; i < item.Value.Count; i++)
                {
                    FGenerators.DestroyObject(item.Value[i]); // Clean ref to filter component on the scene
                }
            }

            #endregion



            return combinationObject;
        }


    }
}