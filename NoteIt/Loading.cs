using ColossalFramework.UI;
using ICities;
using System;
using UnityEngine;

namespace NoteIt
{
    public class Loading : LoadingExtensionBase
    {
        private LoadMode _loadMode;
        private GameObject _mainPanelGameObject;
        private GameObject _locationPanelGameObject;

        public override void OnLevelLoaded(LoadMode mode)
        {
            try
            {
                _loadMode = mode;

                if (_loadMode != LoadMode.LoadGame && _loadMode != LoadMode.NewGame && _loadMode != LoadMode.NewGameFromScenario)
                {
                    return;
                }

                ToolController toolController = UnityEngine.Object.FindObjectOfType<ToolController>();
                if (toolController != null)
                {
                    LocationTool.Instance = toolController.gameObject.AddComponent<LocationTool>();
                }

                UIView uiView = UnityEngine.Object.FindObjectOfType<UIView>();
                if (uiView != null)
                {
                    _mainPanelGameObject = new GameObject("NoteItMainPanel");
                    _mainPanelGameObject.transform.parent = uiView.transform;
                    _mainPanelGameObject.AddComponent<MainPanel>();

                    _locationPanelGameObject = new GameObject("NoteItLocationPanel");
                    _locationPanelGameObject.transform.parent = uiView.transform;
                    _locationPanelGameObject.AddComponent<LocationPanel>();
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] Loading:OnLevelLoaded -> Exception: " + e.Message);
            }
        }

        public override void OnLevelUnloading()
        {
            try
            {
                if (_loadMode != LoadMode.LoadGame && _loadMode != LoadMode.NewGame && _loadMode != LoadMode.NewGameFromScenario)
                {
                    return;
                }

                if (_locationPanelGameObject != null)
                {
                    UnityEngine.Object.Destroy(_locationPanelGameObject);
                }

                if (_mainPanelGameObject != null)
                {
                    UnityEngine.Object.Destroy(_mainPanelGameObject);
                }

                if (LocationTool.Instance != null)
                {
                    UnityEngine.Object.Destroy(LocationTool.Instance);
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] Loading:OnLevelUnloading -> Exception: " + e.Message);
            }
        }
    }
}