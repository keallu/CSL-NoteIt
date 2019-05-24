using ColossalFramework;
using ColossalFramework.UI;
using System;
using System.Collections;
using UnityEngine;

namespace NoteIt
{
    public class LocationTool : ToolBase
    {
        public static LocationTool Instance { get; set; }

        public bool LimitReached { get; set; }

        private UIComponent _pauseMenu;
        private Ray _mouseRay;
        private float _mouseRayLength;
        private Vector3 _hitPosition;

        private const string TOOL_INFO = "Add location";
        private const string TOOL_INFO_LIMIT_REACHED = "<color #ff7e00>Limit reached!</color>";

        protected override void Awake()
        {
            try
            {
                base.Awake();

                enabled = false;

                _pauseMenu = UIView.library.Get("PauseMenu");
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationTool:Awake -> Exception: " + e.Message);
            }
        }

        protected override void OnDestroy()
        {
            try
            {
                enabled = false;

                base.OnDestroy();
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationTool:OnDestroy -> Exception: " + e.Message);
            }
        }

        protected override void OnEnable()
        {
            try
            {
                base.OnEnable();

                m_toolController.ClearColliding();
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationTool:OnEnable -> Exception: " + e.Message);
            }
        }

        protected override void OnDisable()
        {
            try
            {
                base.OnDisable();

                ToolCursor = null;
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationTool:OnDisable -> Exception: " + e.Message);
            }
        }

        protected override void OnToolGUI(Event eventParam)
        {
            try
            {
                base.OnToolGUI(eventParam);

                if (m_toolController.IsInsideUI || eventParam.type != 0)
                {
                    return;
                }

                if (eventParam.button == 0)
                {
                    Singleton<SimulationManager>.instance.AddAction(AddLocation(_hitPosition));
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationTool:OnToolGUI -> Exception: " + e.Message);
            }
        }

        protected override void OnToolUpdate()
        {
            try
            {
                base.OnToolUpdate();

                if (_pauseMenu != null && _pauseMenu.isVisible)
                {
                    ToolsModifierControl.SetTool<DefaultTool>();

                    UIView.library.Hide("PauseMenu");
                }

                if (LimitReached)
                {
                    ShowToolInfo(true, TOOL_INFO + Environment.NewLine + Environment.NewLine + TOOL_INFO_LIMIT_REACHED, _hitPosition);
                }
                else
                {
                    ShowToolInfo(true, TOOL_INFO, _hitPosition);
                }

            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationTool:OnToolUpdate -> Exception: " + e.Message);
            }
        }

        protected override void OnToolLateUpdate()
        {
            try
            {
                base.OnToolLateUpdate();

                _mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                _mouseRayLength = Camera.main.farClipPlane;
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationTool:OnToolLateUpdate -> Exception: " + e.Message);
            }
        }

        public override void SimulationStep()
        {
            try
            {
                base.SimulationStep();

                RaycastInput input = new RaycastInput(_mouseRay, _mouseRayLength);
                if (RayCast(input, out RaycastOutput output))
                {
                    _hitPosition = output.m_hitPos;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationTool:SimulationStep -> Exception: " + e.Message);
            }
        }

        public override void RenderOverlay(RenderManager.CameraInfo cameraInfo)
        {
            try
            {
                base.RenderOverlay(cameraInfo);

                Color color = GetToolColor(false, false);

                Singleton<RenderManager>.instance.OverlayEffect.DrawCircle(cameraInfo, color, _hitPosition, 25f, _hitPosition.y - 100f, _hitPosition.y + 100f, renderLimits: false, alphaBlend: true);
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationTool:RenderOverlay -> Exception: " + e.Message);
            }
        }

        public IEnumerator AddLocation(Vector3 position)
        {
            try
            {
                if (ModConfig.Instance.Locations.Count < ModConfig.Instance.MaxLocations)
                {
                    Location location = new Location
                    {
                        Id = position.x.ToString() + position.y.ToString() + position.z.ToString(),
                        Note = "Type your note here",
                        Position = position
                    };
                    ModConfig.Instance.Locations.Add(location);

                    LocationPanel locationPanel = GameObject.Find("NoteItLocationPanel").GetComponent<LocationPanel>();
                    if (locationPanel != null)
                    {
                        locationPanel.ForceRefresh();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Note It!] LocationTool:AddLocation -> Exception: " + e.Message);
            }

            yield return 0;
        }
    }
}
