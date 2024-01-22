using BepInEx;
using UnityEngine;
using System.Collections;
using BepInEx.Configuration;
using GameNetcodeStuff;

namespace FreeLookMonitor
{
    [BepInDependency(LethalCompanyInputUtils.LethalCompanyInputUtilsPlugin.ModId)]
    [BepInPlugin("kyryh.freelookmonitor", PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private static ConfigEntry<float> moveSensitivity;

        private static ConfigEntry<float> zoomSensitivity;

        private static ManualCameraRenderer mcr;

        private static FreeLookMonitorInput Input = new FreeLookMonitorInput();

        private static bool moved = false;
        private void Awake()
        {

            moveSensitivity = Config.Bind(
                "Sensitivity",
                "MoveSensitivity",
                20f,
                "How fast the camera moves"
            );

            zoomSensitivity = Config.Bind(
                "Sensitivity",
                "ZoomSensitivity",
                20f,
                "How fast the camera zooms in/out"
            );

            // Plugin startup logic

            On.ManualCameraRenderer.Update += ManualCameraRenderer_Update;
            On.ManualCameraRenderer.Start += (orig, self) => {
                orig(self);
                if (self.cam == self.mapCamera)
                    mcr = self;
                //ResetPosition();
            };
            On.ManualCameraRenderer.SwitchScreenButton += (orig, self) => {
                orig(self);
                ResetPosition();
            };
            On.ManualCameraRenderer.updateMapTarget += (orig, self, setRadarTargetIndex, calledFromRPC) => {
                ResetPosition();
                return orig(self, setRadarTargetIndex, calledFromRPC);
            };

            Input.ResetPosition.performed += _ => ResetPosition();
            Input.ResetZoom.performed += _ => { mcr.cam.orthographicSize = 19.7f; };

            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }


        private void ManualCameraRenderer_Update(On.ManualCameraRenderer.orig_Update orig, ManualCameraRenderer self) {
            if (self.cam == self.mapCamera && self.screenEnabledOnLocalClient && !StartOfRound.Instance.localPlayerController.isTypingChat) {
                Vector3 direction = Vector3.zero;

                if (Input.MoveRight.IsPressed()) {
                    direction += self.cam.transform.right;
                }
                if (Input.MoveLeft.IsPressed()) {
                    direction -= self.cam.transform.right;
                }
                if (Input.MoveForward.IsPressed()) {
                    direction += self.cam.transform.up;
                }
                if (Input.MoveBackward.IsPressed()) {
                    direction -= self.cam.transform.up;
                }
                if (Input.MoveDown.IsPressed()) {
                    direction += self.cam.transform.forward / 5;
                }
                if (Input.MoveUp.IsPressed()) {
                    direction -= self.cam.transform.forward / 5;
                }

                if (Input.ZoomIn.IsPressed())
                    self.cam.orthographicSize -= zoomSensitivity.Value * Time.deltaTime;

                if (Input.ZoomOut.IsPressed())
                    self.cam.orthographicSize += zoomSensitivity.Value * Time.deltaTime;

                if (self.cam.orthographicSize < 1)
                    self.cam.orthographicSize = 1;

                if (direction != Vector3.zero) { 
                    self.cam.transform.position += direction * self.cam.orthographicSize * moveSensitivity.Value * Time.deltaTime / 10;
                    moved = true;
                    StartOfRound.Instance.mapScreenPlayerName.text = "FREELOOK";
                }

                if (moved) return;
            }

            orig(self);

        }

        private void ResetPosition() {
            if (mcr == null)
                return;
            StartOfRound.Instance.mapScreenPlayerName.text = "MONITORING: " + mcr.radarTargets[mcr.targetTransformIndex].name;
            moved = false;
        }
        
    }
}
