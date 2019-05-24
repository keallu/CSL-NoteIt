using ColossalFramework.UI;
using System;
using UnityEngine;

namespace NoteIt
{
    class LocationItem
    {
        private string _id;
        private Vector3 _position;

        private UIPanel _panel;
        private UITextField _noteTextField;
        private UIButton _focusButton;
        private UIButton _deleteButton;

        public LocationItem CreateUI(UIComponent parent)
        {
            try
            {
                _panel = UIUtils.CreateLinePanel(parent, "LocationItemPanel", parent.components.Count % 2 == 0 ? false : true);
                _panel.Hide();

                _noteTextField = UIUtils.CreateLineTextField(_panel, "LocationItemNote", "");
                _noteTextField.font = UIUtils.GetUIFont("OpenSans-Regular");

                _focusButton = UIUtils.CreateLineFocusButton(_panel, "LocationItemFocus");
                _deleteButton = UIUtils.CreateLineDeleteButton(_panel, "LocationItemDelete");
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationItem:CreateUI -> Exception: " + e.Message);
            }

            return this;
        }

        public LocationItem UpdateUI()
        {
            try
            {
                _panel.width = _panel.parent.width;

                _noteTextField.width = _panel.width - 81f;
                _noteTextField.height = 35f;
                _noteTextField.padding = new RectOffset(8, 8, 8, 8);
                _noteTextField.relativePosition = new Vector3(10f, (_panel.height - _noteTextField.height) / 2);
                _noteTextField.eventTextSubmitted += (component, value) =>
                {
                    Location location = ModConfig.Instance.Locations.Find(x => x.Id == _id);
                    location.Note = value;
                };

                _focusButton.relativePosition = new Vector3(_panel.width - 66f, (_panel.height - _focusButton.height) / 2);
                _focusButton.eventClick += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        CameraController cameraController = ToolsModifierControl.cameraController;
                        cameraController.m_targetPosition = _position;
                        cameraController.ClearTarget();

                        eventParam.Use();
                    }
                };

                _deleteButton.relativePosition = new Vector3(_panel.width - 33f, (_panel.height - _deleteButton.height) / 2);
                _deleteButton.eventClick += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        ModConfig.Instance.Locations.RemoveAll(x => x.Id == _id);

                        LocationPanel locationPanel = GameObject.Find("NoteItLocationPanel").GetComponent<LocationPanel>();
                        if (locationPanel != null)
                        {
                            locationPanel.ForceRefresh();
                        }

                        eventParam.Use();
                    }
                };
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationItem:UpdateUI -> Exception: " + e.Message);
            }

            return this;
        }

        public LocationItem Show(string id, string note, Vector3 position)
        {
            try
            {
                _id = id;
                _position = position;

                _noteTextField.text = note;

                _panel.Show();
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationItem:Show -> Exception: " + e.Message);
            }

            return this;
        }

        public LocationItem Hide()
        {
            try
            {
                _panel.Hide();
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationItem:Hide -> Exception: " + e.Message);
            }

            return this;
        }

        public void DestroyUI()
        {
            if (_deleteButton != null)
            {
                UnityEngine.Object.Destroy(_deleteButton);
            }
            if (_focusButton != null)
            {
                UnityEngine.Object.Destroy(_focusButton);
            }
            if (_noteTextField != null)
            {
                UnityEngine.Object.Destroy(_noteTextField);
            }
            if (_panel != null)
            {
                UnityEngine.Object.Destroy(_panel);
            }
        }
    }
}
