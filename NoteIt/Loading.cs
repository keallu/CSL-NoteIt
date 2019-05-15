﻿using ColossalFramework.UI;
using ICities;
using System;
using UnityEngine;

namespace NoteIt
{
    public class Loading : LoadingExtensionBase
    {
        private LoadMode _loadMode;
        private GameObject _gameObject;

        public override void OnLevelLoaded(LoadMode mode)
        {
            try
            {
                _loadMode = mode;

                if (_loadMode != LoadMode.LoadGame && _loadMode != LoadMode.NewGame && _loadMode != LoadMode.NewGameFromScenario)
                {
                    return;
                }

                UIView objectOfType = UnityEngine.Object.FindObjectOfType<UIView>();
                if (objectOfType != null)
                {
                    _gameObject = new GameObject("NoteItMainPanel");
                    _gameObject.transform.parent = objectOfType.transform;
                    _gameObject.AddComponent<MainPanel>();
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

                if (_gameObject != null)
                {
                    UnityEngine.Object.Destroy(_gameObject);
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] Loading:OnLevelUnloading -> Exception: " + e.Message);
            }
        }
    }
}