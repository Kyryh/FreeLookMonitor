using BepInEx;
using UnityEngine;
using System.Collections;

namespace FreeLookMonitor
{
    [BepInDependency(LethalCompanyInputUtils.LethalCompanyInputUtilsPlugin.ModId)]
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private static FreeLookMonitorInput Input = new FreeLookMonitorInput();

        private static bool moved = false;
        private void Awake()
        {
            // Plugin startup logic

            On.ManualCameraRenderer.Update += ManualCameraRenderer_Update;
            On.ManualCameraRenderer.Start += (orig, self) => {
                orig(self);
                ResetPosition();
            };
            On.ManualCameraRenderer.SwitchScreenButton += (orig, self) => {
                orig(self);
                ResetPosition();
            };
            On.ManualCameraRenderer.updateMapTarget += (orig, self, setRadarTargetIndex, calledFromRPC) => {
                ResetPosition();
                return orig(self, setRadarTargetIndex, calledFromRPC);
            };

            Input.ResetPosition.performed += obj => ResetPosition();

            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }


        private void ManualCameraRenderer_Update(On.ManualCameraRenderer.orig_Update orig, ManualCameraRenderer self) {
            if (self.cam == self.mapCamera && self.screenEnabledOnLocalClient) {
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
                if (Input.MoveUp.IsPressed()) {
                    direction -= self.cam.transform.forward;
                }
                if (Input.MoveDown.IsPressed()) {
                    direction += self.cam.transform.forward;
                }

                if (direction != Vector3.zero) { 
                    self.cam.transform.position += direction;
                    moved = true;
                    StartOfRound.Instance.mapScreenPlayerName.text = "FREELOOK";
                }

                if (moved) return;
            }

            orig(self);

        }

        private void ResetPosition() {
            moved = false;
        }
        
    }
}
