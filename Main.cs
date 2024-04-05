using CoreRuntime.Interfaces;
using CoreRuntime.Manager;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
using VRC;
using static FewTags.Json;

namespace FewTags
{
    public class Main : HexedCheat
    {
        private static Json._Tags s_tags { get; set; }
        public static string s_rawTags { get; set; }

        private static MethodInfo s_joinMethod { get; set; }
        public static bool SnaxyTagsLoaded { get; private set; }
        public static bool HexedClient { get; private set; }

        public static bool AbyssClientLoaded { get; private set; }
        public static bool NameplateStatsLoaded { get; private set; }
        public static bool VanixClientLoaded { get; private set; }
        internal static float Position { get; set; }
        internal static float Position2 { get; set; }
        internal static float Position3 { get; set; }
        internal static float PositionID { get; set; }
        internal static float PositionBigText { get; set; }
        private static string s_stringInstance { get; set; }
        public static bool overlay = false;

        private delegate IntPtr userJoined(IntPtr _instance, IntPtr _user, IntPtr _methodinfo);
        private static userJoined s_userJoined;
        

        public override void OnLoad() 
        {
            new WaitForSeconds(3f);
            //NativeHook();
            //Task.Run(() => { OnPlayer.InitPatches(); });
            CoroutineManager.RunCoroutine(_OnPlayer.InitializeJoinLeaveHooks());
            OnUpdate();
            UpdateTags();
            Chatbox.CreateShit();
            //ProPlatesLoaded = MelonHandler.Mods.Any(m => m.Info.Name == "ProPlates");
            //AbyssClientLoaded = MelonHandler.Mods.Any(m => m.Info.Name == "AbyssLoader");
            Console.WriteLine("Started FewTags ^_^");
            Console.WriteLine("To ReFetch Tags Press L + F (Rejoin Required)");
            overlay = false;
            Console.WriteLine("Finished Fetching Tags (This Message Doesn't Appear When Tags Are ReFetched)");
            Console.WriteLine("Tagged Players - Nameplate ESP On: RightShift + O (Rejoin Required)");
            Console.WriteLine("Tagged Players - Nameplate ESP Off: RightShift + P (Rejoin Required)");

            //Checks For Other Mods (Positions For A Fixed ProPlates and Snaxy Aren't Updated - Abyss Positions Might Not Be Updated Now Due To It Being C++)
            //If Nothing Is Loaded
            if (!FewTags.Main.NameplateStatsLoaded && !FewTags.Main.SnaxyTagsLoaded)
            {
                FewTags.Main.PositionID = -75.95f; // id
                FewTags.Main.Position = -103.95f; // this is where it says fewtags or malicious
                FewTags.Main.PositionBigText = 273.75f; // big plate
            }
            else if (FewTags.Main.NameplateStatsLoaded || FewTags.Main.SnaxyTagsLoaded)
            {
                FewTags.Main.PositionID = -102.95f; // id for when something is loaded
                FewTags.Main.Position = -130.95f; // this is where it says fewtags or malicious for when something is loaded
                FewTags.Main.PositionBigText = 273.75f; // big plate for when something is loaded
            }
            if (!FewTags.Main.HexedClient || FewTags.Main.HexedClient) // whatever the assembly name is (be sure to change the FewTags.NameplateStatsLoaded = MelonTypeBase<MelonMod>.RegisteredMelons.Any<MelonMod>(m => m.Info.Name == "NameplateStats"); to whatever its supposed to be called properly too)
            {
                FewTags.Main.PositionID = -75.95f; // id
                FewTags.Main.Position = -103.95f; // this is where it says fewtags or malicious
                FewTags.Main.PositionBigText = 273.75f; // big plate
            }
            else if (FewTags.Main.HexedClient || FewTags.Main.HexedClient)
            {
                FewTags.Main.PositionID = -102.95f; // id for when something is loaded
                FewTags.Main.Position = -130.95f; // this is where it says fewtags or malicious for when something is loaded
                FewTags.Main.PositionBigText = 273.75f; // big plate for when something is loaded
            }
        }

        public void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            Chatbox.GetColors();
        }

        public override void OnUpdate()
        {

            if (Input.GetKeyDown(KeyCode.F) & Input.GetKey(KeyCode.L))
            {
                UpdateTags();
            }
        }

        public override void OnLateUpdate()
        {
            if (Input.GetKey(KeyCode.RightShift) & Input.GetKeyDown(KeyCode.O))
            {
                try
                {
                    if (s_plate.Text.isActiveAndEnabled)
                    {
                        if (overlay == false)
                        {
                            overlay = true;
                            s_plate.Text.isOverlay = true;
                            Console.WriteLine("(Tagged Players) Nameplate ESP On");
                        }
                    }
                }
                catch { }
            }

            if (Input.GetKey(KeyCode.RightShift) & Input.GetKeyDown(KeyCode.P))
            {
                try
                {
                    if (s_plate.Text.isActiveAndEnabled)
                    {
                        if (overlay == true)
                        {
                            overlay = false;
                            s_plate.Text.isOverlay = false;
                            Console.WriteLine("(Tagged Players) Nameplate ESP Off");
                        }
                    }
                }
                catch { }
            }
        }

        void UpdateTags()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                   
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                   
                    HttpResponseMessage response = client.GetAsync("https://raw.githubusercontent.com/Fewdys/FewTags/main/FewTags.json").Result;

                   
                    response.EnsureSuccessStatusCode();

                    
                    s_rawTags = response.Content.ReadAsStringAsync().Result;

                    
                    s_tags = JsonConvert.DeserializeObject<Json._Tags>(s_rawTags);

                    Console.WriteLine("Fetching Tags (If L + F Was Pressed This Could Potentially Cause Some Lag)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching tags: " + ex.Message);
            }
        }









/*
private static void OnJoin(IntPtr _instance, IntPtr _user, IntPtr _methodInfo)
{
    s_userJoined(_instance, _user, _methodInfo);
    var vrcPlayer = UnhollowerSupport.Il2CppObjectPtrToIl2CppObject<VRC.Player>(_user);
    //Chatbox.UpdateMyChatBoxOnJoin(vrcPlayer);
    if (!s_rawTags.Contains(vrcPlayer.field_Private_APIUser_0.id)) return;
    PlateHandler(vrcPlayer, overlay);
}*/

private static Plate s_plate { get; set; }
        private static PlateStatic platestatic { get; set; }
        private static Json.Tags[] s_tagsArr { get; set; }


        public static void PlateHandler(VRC.Player vrcPlayer, bool overlay)
        {
            try
            {
                platestatic = new PlateStatic(vrcPlayer);
                s_tagsArr = s_tags.records.Where(x => x.UserID == vrcPlayer.field_Private_APIUser_0.id).ToArray();
                for (int i = 0; i < s_tagsArr.Length; i++)
                {
                    if (!s_tagsArr[i].Active) continue;
                    s_stringInstance = s_tagsArr[i].Size == null ? "" : s_tagsArr[i].Size;
                    for (int g = 0; g < s_tagsArr[i].Tag.Length; g++)
                    {
                        s_plate = new Plate(vrcPlayer, NameplateStatsLoaded || SnaxyTagsLoaded ? -158.75f - (g * 28f) : -128.75f - (g * 28f));
                        s_plate = new Plate(vrcPlayer, HexedClient ? -158.95f - (g * 28f) : -131.95f - (g * 28f));
                        if (s_tagsArr[i].TextActive)
                        {
                            s_plate.Text.text += $"{s_tagsArr[i].Tag[g]}";
                            s_plate.Text.enabled = true;
                            s_plate.Text.gameObject.SetActive(true);
                            s_plate.Text.gameObject.transform.parent.gameObject.SetActive(true);
                            s_plate.Text.isOverlay = overlay;
                        }
                        if (!s_tagsArr[i].TextActive)
                        {
                            s_plate.Text.enabled = false;
                            s_plate.Text.gameObject.SetActive(false);
                            s_plate.Text.gameObject.transform.parent.gameObject.SetActive(false);
                        }
                    }
                    platestatic.TextID.text = $"<color=#ffffff>[</color><color=#808080>{s_tagsArr[i].id}</color><color=#ffffff>]";
                    platestatic.TextID.isOverlay = overlay;
                    if (s_tagsArr[i].Malicious)
                    {
                        platestatic.TextM.text += $"</color><color=#ff0000>Malicious User</color>";
                        platestatic.TextM.isOverlay = overlay;
                    }
                    if (!s_tagsArr[i].Malicious)
                    {
                        platestatic.TextM.text += $"</color><b><color=#ff0000>-</color> <color=#ff7f00>F</color><color=#ffff00>e</color><color=#80ff00>w</color><color=#00ff00>T</color><color=#00ff80>a</color><color=#00ffff>g</color><color=#0000ff>s</color> <color=#8b00ff>-</color><color=#ffffff></b>";
                        platestatic.TextM.isOverlay = overlay;
                    }
                    if (s_tagsArr[i].BigTextActive)
                    {
                        platestatic.TextBP.text += $"{s_stringInstance}{s_tagsArr[i].PlateBigText}</size>";
                        platestatic.TextBP.enabled = true;
                        platestatic.TextBP.gameObject.SetActive(true);
                        platestatic.TextBP.gameObject.transform.parent.gameObject.SetActive(true);
                        platestatic.TextBP.isOverlay = overlay;
                    }
                    if (!s_tagsArr[i].BigTextActive)
                    {
                        platestatic.TextBP.enabled = false;
                        platestatic.TextBP.gameObject.SetActive(false);
                        platestatic.TextBP.gameObject.transform.parent.gameObject.SetActive(false);
                    }
                }
            }
            catch { }
        }
    }
}
