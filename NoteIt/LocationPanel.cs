using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoteIt
{
    public class LocationPanel : UIPanel
    {
        private bool _initialized;

        private MainPanel _mainPanel;

        private UILabel _title;
        private UIButton _close;
        private UIDragHandle _dragHandle;

        private UIPanel _locationPanel;
        private UIScrollablePanel _locationScrollablePanel;
        private UIScrollbar _locationScrollbar;
        private UISlicedSprite _locationScrollbarTrack;
        private UISlicedSprite _locationScrollbarThumb;
        private UILabel _locationCounterLabel;
        private UIButton _locationAddButton;

        private List<LocationItem> _locationItems;

        public override void Awake()
        {
            base.Awake();

            try
            {
                if (ModConfig.Instance.Locations == null)
                {
                    ModConfig.Instance.Locations = new List<Location> { };
                }

                if (_locationItems == null)
                {
                    _locationItems = new List<LocationItem> { };
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationPanel:Awake -> Exception: " + e.Message);
            }
        }

        public override void Start()
        {
            base.Start();

            if (_mainPanel == null)
            {
                _mainPanel = GameObject.Find("NoteItMainPanel").GetComponent<MainPanel>();
            }

            CreateUI();
        }

        public override void Update()
        {
            base.Update();

            try
            {
                if (!_initialized)
                {
                    UpdateUI();
                    RefreshLocations();

                    _initialized = true;
                }

                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L))
                {
                    TogglePanel(this);
                }
                else if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.L))
                {
                    ToggleTool();
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationPanel:Update -> Exception: " + e.Message);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            if (_title != null)
            {
                Destroy(_title);
            }
            if (_close != null)
            {
                Destroy(_close);
            }
            if (_dragHandle != null)
            {
                Destroy(_dragHandle);
            }
            if (_locationPanel != null)
            {
                Destroy(_locationPanel);
            }
            if (_locationScrollablePanel != null)
            {
                Destroy(_locationScrollablePanel);
            }
            if (_locationScrollbar != null)
            {
                Destroy(_locationScrollbar);
            }
            if (_locationCounterLabel != null)
            {
                Destroy(_locationCounterLabel);
            }
            if (_locationAddButton != null)
            {
                Destroy(_locationAddButton);
            }
            if (_locationItems != null)
            {
                foreach (LocationItem locationItem in _locationItems)
                {
                    locationItem.DestroyUI();
                }
            }
        }

        public void ForceUpdate()
        {
            UpdateUI();
        }

        public void ForceRefresh()
        {
            RefreshLocations();
        }

        private void CreateUI()
        {
            try
            {
                name = "NoteItLocationPanel";
                backgroundSprite = "MenuPanel2";
                clipChildren = true;
                eventMouseEnter += (component, eventParam) =>
                {
                    opacity = ModConfig.Instance.OpacityWhenHover;
                };
                eventMouseLeave += (component, eventParam) =>
                {
                    opacity = ModConfig.Instance.Opacity;
                };

                _title = UIUtils.CreateMenuPanelTitle(this, "Note It! - Locations");
                _close = UIUtils.CreateMenuPanelCloseButton(this);
                _dragHandle = UIUtils.CreateMenuPanelDragHandle(this);

                _locationPanel = UIUtils.CreatePanel(this, "LocationList");
                _locationScrollablePanel = UIUtils.CreateScrollablePanel(_locationPanel, "LocationScrollablePanel");
                _locationScrollablePanel.autoLayout = true;
                _locationScrollablePanel.autoLayoutDirection = LayoutDirection.Vertical;
                _locationScrollablePanel.scrollWheelDirection = UIOrientation.Vertical;
                _locationScrollablePanel.builtinKeyNavigation = true;
                _locationScrollablePanel.clipChildren = true;
                _locationScrollbar = UIUtils.CreateScrollbar(_locationPanel, "LocationScrollbar");
                _locationScrollbar.orientation = UIOrientation.Vertical;
                _locationScrollbar.incrementAmount = 38f;
                _locationScrollbarTrack = UIUtils.CreateSlicedSprite(_locationScrollbar, "LocationScrollbarTrack");
                _locationScrollbarTrack.spriteName = "ScrollbarTrack";
                _locationScrollbarTrack.fillDirection = UIFillDirection.Vertical;
                _locationScrollbarThumb = UIUtils.CreateSlicedSprite(_locationScrollbar, "LocationScrollbarThumb");
                _locationScrollbarThumb.spriteName = "ScrollbarThumb";
                _locationScrollbarThumb.fillDirection = UIFillDirection.Vertical;

                _locationScrollablePanel.verticalScrollbar = _locationScrollbar;
                _locationScrollbar.trackObject = _locationScrollbarTrack;
                _locationScrollbar.thumbObject = _locationScrollbarThumb;

                _locationCounterLabel = UIUtils.CreateLabel(this, "LocationCounterLabel", "Location Count: Unknown");
                _locationCounterLabel.font = UIUtils.GetUIFont("OpenSans-Regular");

                _locationAddButton = UIUtils.CreateButton(this, "LocationAddButton", "Add");
                _locationAddButton.height = 30f;
                _locationAddButton.width = 80f;
                _locationAddButton.eventClick += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        ToggleTool();

                        eventParam.Use();
                    }
                };

                for (int i = 0; i < ModConfig.Instance.MaxLocations; i++)
                {
                    _locationItems.Add(new LocationItem().CreateUI(_locationScrollablePanel));
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationPanel:CreateUI -> Exception: " + e.Message);
            }
        }

        private void UpdateUI()
        {
            try
            {
                opacity = ModConfig.Instance.Opacity;
                width = _mainPanel.width;
                height = _mainPanel.height;
                pivot = UIPivotPoint.TopLeft;
                absolutePosition = new Vector3(_mainPanel.absolutePosition.x + width, _mainPanel.absolutePosition.y);

                _title.relativePosition = new Vector3(width / 2f - _title.width / 2f, 15f);
                _close.relativePosition = new Vector3(width - 37f, 3f);
                _dragHandle.width = width - 40f;
                _dragHandle.height = 40f;
                _dragHandle.relativePosition = Vector3.zero;

                _locationPanel.width = width - 40f;
                _locationPanel.height = height - 150f;
                _locationPanel.relativePosition = new Vector3(20f, 100f);

                _locationScrollablePanel.width = _locationScrollablePanel.parent.width - 25f;
                _locationScrollablePanel.height = _locationScrollablePanel.parent.height;
                _locationScrollablePanel.relativePosition = new Vector3(0f, 0f);

                _locationScrollbar.width = 20f;
                _locationScrollbar.height = _locationScrollbar.parent.height;
                _locationScrollbar.relativePosition = new Vector3(_locationScrollbar.parent.width - 20f, 0f);

                _locationScrollbarTrack.width = _locationScrollbarTrack.parent.width;
                _locationScrollbarTrack.height = _locationScrollbarTrack.parent.height;
                _locationScrollbarThumb.width = _locationScrollbarThumb.parent.width - 5f;
                _locationScrollbarThumb.relativePosition = new Vector3(2.5f, 0f);

                _locationCounterLabel.relativePosition = new Vector3(20f, _mainPanel.height - 30f);

                _locationAddButton.relativePosition = new Vector3(_mainPanel.width - 100f, _mainPanel.height - 40f);

                foreach (LocationItem locationItem in _locationItems)
                {
                    locationItem.UpdateUI();
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationPanel:UpdateUI -> Exception: " + e.Message);
            }
        }

        private void RefreshLocations()
        {
            try
            {
                int index = 0;

                foreach (Location location in ModConfig.Instance.Locations)
                {
                    _locationItems[index].Show(location.Id, location.Note, location.Position);

                    index++;
                }

                if (index < ModConfig.Instance.MaxLocations)
                {
                    for (int i = index; i < ModConfig.Instance.MaxLocations; i++)
                    {
                        _locationItems[i].Hide();
                    }
                }

                if (ModConfig.Instance.Locations.Count >= ModConfig.Instance.MaxLocations)
                {
                    LocationTool.Instance.LimitReached = true;
                }
                else
                {
                    LocationTool.Instance.LimitReached = false;
                }

                _locationCounterLabel.text = "Location Count: " + ModConfig.Instance.Locations.Count;
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationPanel:RefreshLocations -> Exception: " + e.Message);
            }
        }

        private void TogglePanel(UIPanel panel)
        {
            try
            {
                if (panel != null)
                {
                    if (panel.isVisible)
                    {
                        panel.Hide();
                    }
                    else
                    {
                        panel.Show();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationPanel:TogglePanel -> Exception: " + e.Message);
            }
        }

        private void ToggleTool()
        {
            try
            {
                if (LocationTool.Instance != null)
                {
                    if (LocationTool.Instance.enabled)
                    {
                        
                        ToolsModifierControl.SetTool<DefaultTool>();
                    }
                    else
                    {
                        LocationTool.Instance.enabled = true;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationPanel:ToggleTool -> Exception: " + e.Message);
            }
        }
    }
}
