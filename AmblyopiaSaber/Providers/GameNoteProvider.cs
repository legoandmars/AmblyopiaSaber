using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zenject;
using UnityEngine;
using SiraUtil.Objects;
using SiraUtil.Interfaces;
using AmblyopiaSaber.Data;

namespace AmblyopiaSaber.Providers
{
    internal class GameNoteProvider : IModelProvider
    {
        public Type Type => typeof(GameNoteDecorator);
        public int Priority { get; set; } = 200;

        private class GameNoteDecorator : IPrefabProvider<GameNoteController>
        {
            public bool Chain => true;
            public bool CanSetup { get; private set; }

            /*[Inject]
            public void Construct()
            {
                CanSetup = !(sceneSetupData.gameplayModifiers.ghostNotes || sceneSetupData.gameplayModifiers.disappearingArrows) || !Container.HasBinding<MultiplayerLevelSceneSetupData>();
                if (_noteAssetLoader.SelectedNote != 0)
                {
                    var note = _noteAssetLoader.CustomNoteObjects[_noteAssetLoader.SelectedNote];
                    MaterialSwapper.GetMaterials();
                    MaterialSwapper.ReplaceMaterialsForGameObject(note.NoteLeft);
                    MaterialSwapper.ReplaceMaterialsForGameObject(note.NoteRight);
                    MaterialSwapper.ReplaceMaterialsForGameObject(note.NoteDotLeft);
                    MaterialSwapper.ReplaceMaterialsForGameObject(note.NoteDotRight);
                    Utils.AddMaterialPropertyBlockController(note.NoteLeft);
                    Utils.AddMaterialPropertyBlockController(note.NoteRight);
                    Utils.AddMaterialPropertyBlockController(note.NoteDotLeft);
                    Utils.AddMaterialPropertyBlockController(note.NoteDotRight);
                    Container.BindMemoryPool<SiraPrefabContainer, SiraPrefabContainer.Pool>().WithId("cn.left.arrow").WithInitialSize(25).FromComponentInNewPrefab(NotePrefabContainer(note.NoteLeft));
                    Container.BindMemoryPool<SiraPrefabContainer, SiraPrefabContainer.Pool>().WithId("cn.right.arrow").WithInitialSize(25).FromComponentInNewPrefab(NotePrefabContainer(note.NoteRight));
                    if (note.NoteDotLeft != null)
                    {
                        Container.BindMemoryPool<SiraPrefabContainer, SiraPrefabContainer.Pool>().WithId("cn.left.dot").WithInitialSize(10).FromComponentInNewPrefab(NotePrefabContainer(note.NoteDotLeft));
                    }
                    if (note.NoteDotRight != null)
                    {
                        Container.BindMemoryPool<SiraPrefabContainer, SiraPrefabContainer.Pool>().WithId("cn.right.dot").WithInitialSize(10).FromComponentInNewPrefab(NotePrefabContainer(note.NoteDotRight));
                    }
                }
            }*/
            public GameNoteController Modify(GameNoteController original)
            {
                //if (!CanSetup) return original;
                original.gameObject.AddComponent<AmblyopiaController>();
                return original;
            }

            /*private SiraPrefabContainer NotePrefabContainer(GameObject initialPrefab)
            {
                var prefab = new GameObject("CustomNotes" + initialPrefab.name).AddComponent<SiraPrefabContainer>();
                prefab.Prefab = initialPrefab;
                return prefab;
            }*/
        }
    }
}
