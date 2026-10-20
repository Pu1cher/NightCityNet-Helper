using System;using System.IO;
namespace NcnHelper {
public static class MouthPatch {
 public static bool UseFallback(int mode,string language){return language!="en"&&(mode==3||mode==1&&(language=="uk"||language=="uk-ru"));}
 public static void Build(Plan plan,int mode,string language=null){if(mode==0)return;string path=Path.Combine(Core.Mod(plan.Game),"init.lua");string text=File.ReadAllText(path);const string begin="-- PALCHER_MOUTH_BEGIN",end="-- PALCHER_MOUTH_END";int a=text.IndexOf(begin),b=text.IndexOf(end);if((a<0)!=(b<0)||a>=0&&(b<a||text.IndexOf(begin,a+begin.Length)>=0||text.IndexOf(end,b+end.Length)>=0))throw new Exception("The helper mouth-animation block is damaged.");if(a>=0){int tail=b+end.Length;if(text.Substring(tail).StartsWith("\r\n"))tail+=2;else if(text.Substring(tail).StartsWith("\n"))tail++;text=text.Remove(a,tail-a);}
 bool fallback=UseFallback(mode,language??Profanity.CurrentLanguage(plan.Game));
 if(fallback){if(!text.Contains("local function dispatchEvent(ev)")||!text.Contains("local function targetForTurn(ev)")||!text.Contains("need('Lifecycle')"))throw new Exception("This Lua bridge version needs a separate mouth-animation compatibility check.");string assets=Path.Combine(Core.Base,"Compatibility");string hooks=Path.Combine(assets,"mouth-hooks.lua.txt"),module=Path.Combine(assets,"PalcherMouth.lua");if(!File.Exists(hooks)||!File.Exists(module))throw new Exception("Mouth-animation assets not found beside the helper.");text=text+File.ReadAllText(hooks).TrimEnd()+"\r\n";Core.Add(plan,Path.Combine(Core.Mod(plan.Game),@"compat\PalcherMouth.lua"),File.ReadAllBytes(module),null);}
 Core.Add(plan,path,Core.Encode(text,path),null);plan.Summary.Add(fallback?"Approximate jaw fallback enabled for this language. Requires Boneware; verify visually in game.":"Native NightCityNet facial animation preserved; helper jaw override removed.");}
}
}
