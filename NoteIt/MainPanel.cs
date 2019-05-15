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

        private UILabel _title;
        private UIButton _close;
        private UIDragHandle _dragHandle;
        private UITabstrip _tabstrip;
        private UITabContainer _tabContainer;

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
                    ModConfig.Instance.Notes = new List<string>
                    {
                        "Type your note #1 here.",
                        "Type your note #2 here.",
                        "Type your note #3 here."
                    };
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

                            SaveNotes();
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
        }

        private void CreateUI()
        {
            try
            {
                name = "NoteItMainPanel";
                backgroundSprite = "MenuPanel2";
                clipChildren = true;
                isVisible = ModConfig.Instance.ShowAfterLoad ? true : false;
                width = ModConfig.Instance.SizeX;
                height = ModConfig.Instance.SizeY;
                relativePosition = new Vector3(ModConfig.Instance.PositionX, ModConfig.Instance.PositionY);
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

                _tabstrip = UIUtils.CreateTabStrip(this);
                _tabContainer = UIUtils.CreateTabContainer(this);
                _tabstrip.tabPages = _tabContainer;

                UIButton template = UIUtils.CreateTabButton(this);
                UIPanel panel = null;
                UITextField textField = null;

                int index = 0;

                foreach (string text in ModConfig.Instance.Notes)
                {
                    _tabstrip.AddTab("Note #" + (index + 1), template, true);
                    _tabstrip.selectedIndex = index;

                    panel = _tabstrip.tabContainer.components[index] as UIPanel;
                    panel.width = panel.parent.width;
                    panel.height = panel.parent.height;

                    textField = UIUtils.CreateMultilineTextBox(panel);
                    textField.text = text;

                    index++;
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
                opacity = ModConfig.Instance.Opacity;

                UITextField textField = null;

                foreach (UIPanel panel in _tabstrip.tabContainer.components)
                {
                    textField = panel.Find("TextField").GetComponent<UITextField>();

                    textField.textScale = ModConfig.Instance.TextScale;
                }
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
                UITextField textField = null;

                int index = 0;

                foreach (UIPanel panel in _tabstrip.tabContainer.components)
                {
                    textField = panel.Find("TextField").GetComponent<UITextField>();

                    ModConfig.Instance.Notes[index] = textField.text;

                    index++;
                }

                ModConfig.Instance.Save();
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] MainPanel:SaveNotes -> Exception: " + e.Message);
            }
        }
    }
}
