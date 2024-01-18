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

            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void ManualCameraRenderer_Update(On.ManualCameraRenderer.orig_Update orig, ManualCameraRenderer self) {
            if (self.cam == self.mapCamera) {
                Vector3 direction = Vector3.zero;

                if (Input.MoveRight.IsPressed()) {
                    direction.x++;
                }
                if (Input.MoveLeft.IsPressed()) {
                    direction.x--;
                }
                if (Input.MoveUp.IsPressed()) {
                    direction.z++;
                }
                if (Input.MoveDown.IsPressed()) {
                    direction.z--;
                }

                if (direction != Vector3.zero) { 
                    self.cam.transform.position += direction;
                    moved = true;
                }

                if (moved) return;
            }

            orig(self);

        }


        
    }
}
