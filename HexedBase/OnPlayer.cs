using System;
using System.Collections;

namespace FewTags
{

    /*internal class OnPlayer
    {
        public static HarmonyLib.Harmony Harmony { get; set; }
        public static HarmonyLib.Harmony Instance = new HarmonyLib.Harmony("Few");

        [Obsolete]
        public static Harmony.HarmonyMethod GetLocalPatch(Type type, string methodName)
        {
            return new Harmony.HarmonyMethod(type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic));
        }
        public static void InitPatches()
        {
            try
            {
                _OnPlayer.InitOnPlayer();
            }
            catch (Exception ERR) { MelonLoader.MelonLogger.Msg(ERR.Message); }

        }
    }*/

    public class _OnPlayer
    {
        /*public static void InitOnPlayer()
        {
            try
            {
                OnPlayer.Instance.Patch(typeof(NetworkManager).GetMethod(nameof(NetworkManager.Method_Public_Void_Player_PDM_2)), new HarmonyMethod(AccessTools.Method(typeof(_OnPlayer), nameof(OnPlayerJoin))));
                //OnPlayer.Instance.Patch(typeof(NetworkManager).GetMethod(nameof(NetworkManager.Method_Public_Void_Player_0)), new HarmonyMethod(AccessTools.Method(typeof(_OnPlayer), nameof(OnPlayerLeave))));
                MelonLogger.Msg("[Patch] OnPlayer Patched!");
            }
            catch (Exception ex)
            {
                MelonLogger.Msg("[Patch] Patching OnPlayer Failed!\n" + ex);
            }
        }*/

        public static IEnumerator InitializeJoinLeaveHooks()
        {
            while (NetworkManager.field_Internal_Static_NetworkManager_0 is null)
                yield return null;

            try
            {
                NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_0.field_Private_HashSet_1_UnityAction_1_T_0.Add(new Action<VRC.Player>(player => OnPlayerJoin(ref player)));
               Console.WriteLine("[Patch] OnPlayerJoin Patched!");
            }
            catch (Exception e)
            {
                Console.WriteLine("[Patch] OnPlayerJoin Patched! " + e);
            }

            try
            {
                NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_2.field_Private_HashSet_1_UnityAction_1_T_0.Add(new Action<VRC.Player>(player => OnPlayerLeave(ref player)));
                Console.WriteLine("[Patch] OnPlayerLeave Patched!");
            }
            catch (Exception e)
            {
                Console.WriteLine("[Patch] OnPlayerLeave Patched! " + e);
            }
        }

        public static void OnPlayerJoin(ref VRC.Player __0)
        {
            if (__0.prop_APIUser_0 == null) return;
            //Chatbox.UpdateMyChatBoxOnJoin(__0);
            /*try
            {
                FewMod.Nameplate.NamePlates nameplate = __0._vrcplayer.field_Public_PlayerNameplate_0.gameObject.AddComponent<NamePlates>();
                nameplate.player = __0;
            }
            catch (Exception e) { Main.Log.Msg("Failed To Add NamplatePositioner To Player " + __0.field_Private_APIUser_0.displayName + "\n" + e.Message); }*/
            if (!Main.s_rawTags.Contains(__0.field_Private_APIUser_0.id)) return;
            Main.PlateHandler(__0, Main.overlay);
            return;
        }

        public static void OnPlayerLeave(ref VRC.Player __0)
        {
            if (__0.prop_APIUser_0 == null) return;
            return;
        }

    }
    
}