using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoteIt
{
    public class MainPanel : UIPanel
    {
        private bool _initialized;
        private float _timer;
        private bool _avoidSaving;

        private LocationPanel _locationPanel;

        private UILabel _title;
        private UIButton _location;
        private UIButton _close;
        private UIDragHandle _dragHandle;

        private UITabstrip _tabstrip;
        private UITabContainer _tabContainer;
        private UIButton _tabAddButton;
        private UIButton _tabDeleteButton;
        private UIButton _templateButton;

        public override void Awake()
        {
            base.Awake();

            try
            {
                if (ModConfig.Instance.SizeX == 0.0f)
                {
                    ModConfig.Instance.SizeX = 480f;
                }

                if (ModConfig.Instance.SizeY == 0.0f)
                {
                    ModConfig.Instance.SizeY = 640f;
                }

                if (ModConfig.Instance.PositionX == 0.0f)
                {
                    ModConfig.Instance.PositionX = Mathf.Floor((GetUIView().fixedWidth - width) / 2f);
                }

                if (ModConfig.Instance.PositionY == 0.0f)
                {
                    ModConfig.Instance.PositionY = Mathf.Floor((GetUIView().fixedHeight - height) / 2f);
                }

                if (ModConfig.Instance.Notes == null)
                {
                    ModConfig.Instance.Notes = new List<string> { };
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] MainPanel:Awake -> Exception: " + e.Message);
            }
        }

        public override void Start()
        {
            base.Start();

            if (_locationPanel == null)
            {
                _locationPanel = GameObject.Find("NoteItLocationPanel").GetComponent<LocationPanel>();
            }

            CreateUI();
        }

        public override void Update()
        {
            base.Update();

            try
            {
                if (!_initialized || ModConfig.Instance.ConfigUpdated)
                {
                    UpdateUI();
                    RefreshNotes();

                    _initialized = true;
                    ModConfig.Instance.ConfigUpdated = false;
                }
                else
                {
                    if (this.isVisible)
                    {
                        _timer += Time.deltaTime;

                        if (_timer > ModConfig.Instance.SaveInterval)
                        {
                            _timer -= ModConfig.Instance.SaveInterval;

                            if (!_avoidSaving)
                            {
                                Save();
                            }
                        }
                    }
                }

                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.N))
                {
                    TogglePanel(this);
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] MainPanel:Update -> Exception: " + e.Message);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            if (_title != null)
            {
                Destroy(_title);
            }
            if (_location != null)
            {
                Destroy(_location);
            }
            if (_close != null)
            {
                Destroy(_close);
            }
            if (_dragHandle != null)
            {
                Destroy(_dragHandle);
            }
            if (_tabstrip != null)
            {
                Destroy(_tabstrip);
            }
            if (_tabContainer != null)
            {
                Destroy(_tabContainer);
            }
            if (_templateButton != null)
            {
                Destroy(_templateButton);
            }
        }

        private void CreateUI()
        {
            try
            {
                name = "NoteItMainPanel";
                backgroundSprite = "MenuPanel2";
                clipChildren = true;
                isVisible = ModConfig.Instance.ShowAfterLoad ? true : false;
                eventMouseEnter += (component, eventParam) =>
                {
                    opacity = ModConfig.Instance.OpacityWhenHover;
                };
                eventMouseLeave += (component, eventParam) =>
                {
                    opacity = ModConfig.Instance.Opacity;
                };

                _title = UIUtils.CreateMenuPanelTitle(this, "Note It! - Main");
                _location = UIUtils.CreateMenuPanelLocationButton(this);
                _location.eventClick += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        TogglePanel(_locationPanel);

                        eventParam.Use();
                    }                    
                };
                _close = UIUtils.CreateMenuPanelCloseButton(this);
                _dragHandle = UIUtils.CreateMenuPanelDragHandle(this);
                _dragHandle.eventMouseUp += (component, eventParam) =>
                {
                    ModConfig.Instance.PositionX = absolutePosition.x;
                    ModConfig.Instance.PositionY = absolutePosition.y;
                    ModConfig.Instance.Save();
                };

                _tabAddButton = UIUtils.CreateSmallButton(this, "TabAddButton", "+");
                _tabAddButton.height = 26f;
                _tabAddButton.width = 26f;
                _tabAddButton.eventClick += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        ModConfig.Instance.Notes.Add("Type your note #" + (ModConfig.Instance.Notes.Count + 1) + " here.");
                        RefreshNotes();

                        eventParam.Use();
                    }
                };

                _tabDeleteButton = UIUtils.CreateSmallButton(this, "TabDeleteButton", "-");
                _tabDeleteButton.height = 26f;
                _tabDeleteButton.width = 26f;
                _tabDeleteButton.eventClick += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        ModConfig.Instance.Notes.RemoveAt(ModConfig.Instance.Notes.Count - 1);
                        RefreshNotes();

                        eventParam.Use();
                    }
                };

                _tabstrip = UIUtils.CreateTabStrip(this);
                _tabContainer = UIUtils.CreateTabContainer(this);
                _templateButton = UIUtils.CreateTabButton(this);

                _tabstrip.tabPages = _tabContainer;

                UIPanel panel = null;
                UITextField textField = null;

                for (int i = 0; i < ModConfig.Instance.MaxNotes; i++)
                {
                    _tabstrip.AddTab("#" + (i + 1), _templateButton, true);
                    _tabstrip.selectedIndex = i;

                    panel = _tabstrip.tabContainer.components[i] as UIPanel;

                    if (panel != null)
                    {
                        textField = UIUtils.CreateMultilineTextField(panel, "TextField", "");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] MainPanel:CreateUI -> Exception: " + e.Message);
            }
        }

        private void UpdateUI()
        {
            try
            {
                if (!_avoidSaving)
                {
                    Save();
                }

                _avoidSaving = true;

                opacity = ModConfig.Instance.Opacity;
                width = ModConfig.Instance.SizeX;
                height = ModConfig.Instance.SizeY;
                absolutePosition = new Vector3(ModConfig.Instance.PositionX, ModConfig.Instance.PositionY);

                _title.relativePosition = new Vector3(ModConfig.Instance.SizeX / 2f - _title.width / 2f, 15f);
                _location.relativePosition = new Vector3(ModConfig.Instance.SizeX - 67f, 5f);
                _close.relativePosition = new Vector3(ModConfig.Instance.SizeX - 37f, 3f);
                _dragHandle.width = ModConfig.Instance.SizeX - 40f;
                _dragHandle.height = 40f;
                _dragHandle.relativePosition = Vector3.zero;

                _tabAddButton.relativePosition = new Vector3(ModConfig.Instance.SizeX - 77f, 50f);
                _tabDeleteButton.relativePosition = new Vector3(ModConfig.Instance.SizeX - 46f, 50f);                

                _tabstrip.width = ModConfig.Instance.SizeX - 100f;
                _tabstrip.relativePosition = new Vector3(20f, 50f);
                _tabContainer.width = ModConfig.Instance.SizeX - 40f;
                _tabContainer.height = ModConfig.Instance.SizeY - 120f;
                _tabContainer.relativePosition = new Vector3(20f, 100f);

                UITextField textField = null;

                foreach (UIPanel panel in _tabstrip.tabContainer.components)
                {
                    if (panel != null)
                    {
                        panel.width = ModConfig.Instance.SizeX - 40f;
                        panel.height = ModConfig.Instance.SizeY - 120f;
                        panel.relativePosition = Vector3.zero;

                        textField = panel.Find("TextField").GetComponent<UITextField>();

                        if (textField != null)
                        {
                            textField.width = ModConfig.Instance.SizeX - 40f;
                            textField.height = ModConfig.Instance.SizeY - 120f;
                            textField.relativePosition = Vector3.zero;
                            textField.textScale = ModConfig.Instance.TextScale;
                        }
                    }
                }

                _locationPanel.ForceUpdate();

                _avoidSaving = false;
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] MainPanel:UpdateUI -> Exception: " + e.Message);
            }
        }

        private void RefreshNotes()
        {
            try
            {
                int index = 0;
                UITextField textField = null;

                foreach (string note in ModConfig.Instance.Notes)
                {
                    _tabstrip.components[index].Show();

                    textField = _tabstrip.tabContainer.components[index].Find("TextField").GetComponent<UITextField>();

                    if (textField != null)
                    {
                        textField.text = note;
                    }

                    index++;
                }

                if (index < ModConfig.Instance.MaxNotes)
                {
                    for (int i = index; i < ModConfig.Instance.MaxNotes; i++)
                    {
                        _tabstrip.components[i].Hide();
                    }
                }
                _tabAddButton.enabled = ModConfig.Instance.Notes.Count < ModConfig.Instance.MaxNotes ? true : false;
                _tabDeleteButton.enabled = ModConfig.Instance.Notes.Count > 0 ? true : false;

            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] MainPanel:RefreshNotes -> Exception: " + e.Message);
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
                Debug.Log("[Note It!] MainPanel:TogglePanel -> Exception: " + e.Message);
            }
        }

        private void Save()
        {
            try
            {
                _avoidSaving = true;

                if (_initialized)
                {                    
                    int index = 0;
                    UITextField textField = null;

                    foreach (string note in ModConfig.Instance.Notes)
                    {
                        textField = _tabstrip.tabContainer.components[index].Find("TextField").GetComponent<UITextField>();

                        if (textField != null)
                        {
                            ModConfig.Instance.Notes[index] = textField.text;
                        }

                        index++;
                    }

                    ModConfig.Instance.SaveWithoutUpdate();
                }

                _avoidSaving = false;
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] MainPanel:SaveNotes -> Exception: " + e.Message);
            }
        }
    }
}
