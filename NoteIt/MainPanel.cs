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

        private UILabel _title;
        private UIButton _close;
        private UIDragHandle _dragHandle;

        private UITabstrip _tabstrip;
        private UITabContainer _tabContainer;
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

                CreateUI();
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] MainPanel:Awake -> Exception: " + e.Message);
            }
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            base.Update();

            try
            {
                if (!_initialized || ModConfig.Instance.ConfigUpdated)
                {
                    UpdateUI();

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
                                SaveNotes();
                            }
                        }
                    }
                }

                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.N))
                {
                    TogglePanel();
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

                _title = UIUtils.CreateMenuPanelTitle(this, "Note It!");
                _close = UIUtils.CreateMenuPanelCloseButton(this);
                _dragHandle = UIUtils.CreateMenuPanelDragHandle(this);
                _dragHandle.eventMouseUp += (component, eventParam) =>
                {
                    ModConfig.Instance.PositionX = absolutePosition.x;
                    ModConfig.Instance.PositionY = absolutePosition.y;
                    ModConfig.Instance.Save();
                };
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
                    SaveNotes();
                }

                _avoidSaving = true;

                if (ModConfig.Instance.Tabs > ModConfig.Instance.Notes.Count)
                {
                    int notesMissing = ModConfig.Instance.Tabs - ModConfig.Instance.Notes.Count;

                    for (int i = 0; i < notesMissing; i++)
                    {
                        ModConfig.Instance.Notes.Add("Type your note #" + (ModConfig.Instance.Notes.Count + 1) + " here.");
                    }
                }

                opacity = ModConfig.Instance.Opacity;
                width = ModConfig.Instance.SizeX;
                height = ModConfig.Instance.SizeY;
                absolutePosition = new Vector3(ModConfig.Instance.PositionX, ModConfig.Instance.PositionY);

                _title.relativePosition = new Vector3(ModConfig.Instance.SizeX / 2f - _title.width / 2f, 11f);
                _close.relativePosition = new Vector3(ModConfig.Instance.SizeX - 37f, 2f);
                _dragHandle.width = ModConfig.Instance.SizeX - 40f;
                _dragHandle.height = 40f;
                _dragHandle.relativePosition = Vector3.zero;

                UITextField textField = null;

                if (_tabstrip == null || _tabstrip.tabs.Count != ModConfig.Instance.Tabs)
                {
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

                    _tabstrip = UIUtils.CreateTabStrip(this);
                    _tabContainer = UIUtils.CreateTabContainer(this);
                    _templateButton = UIUtils.CreateTabButton(this);

                    _tabstrip.tabPages = _tabContainer;
                    _tabstrip.width = ModConfig.Instance.SizeX - 40f;
                    _tabstrip.relativePosition = new Vector3(20f, 50f);
                    _tabContainer.width = ModConfig.Instance.SizeX - 40f;
                    _tabContainer.height = ModConfig.Instance.SizeY - 120f;
                    _tabContainer.relativePosition = new Vector3(20f, 100f);

                    UIPanel panel = null;

                    for (int i = 0; i < ModConfig.Instance.Tabs; i++)
                    {
                        _tabstrip.AddTab("#" + (i + 1), _templateButton, true);
                        _tabstrip.selectedIndex = i;

                        panel = _tabstrip.tabContainer.components[i] as UIPanel;

                        if (panel != null)
                        {
                            textField = UIUtils.CreateMultilineTextBox(panel);
                        }
                    }
                }

                int index = 0;

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
                            textField.text = ModConfig.Instance.Notes[index];
                        }
                    }

                    index++;
                }

                _avoidSaving = false;
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] MainPanel:UpdateUI -> Exception: " + e.Message);
            }
        }

        private void TogglePanel()
        {
            try
            {
                if (this != null)
                {
                    if (isVisible)
                    {
                        Hide();
                    }
                    else
                    {
                        Show();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] MainPanel:TogglePanel -> Exception: " + e.Message);
            }
        }

        private void SaveNotes()
        {
            try
            {
                _avoidSaving = true;

                if (_initialized)
                {
                    UITextField textField = null;
                    int index = 0;

                    foreach (UIPanel panel in _tabstrip.tabContainer.components)
                    {
                        if (panel != null)
                        {
                            textField = panel.Find("TextField").GetComponent<UITextField>();

                            if (textField != null)
                            {
                                ModConfig.Instance.Notes[index] = textField.text;
                            }
                        }

                        index++;
                    }

                    ModConfig.Instance.Save();
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
