using System;using System.IO;using System.Linq;using System.Text;using System.Collections;using System.Collections.Generic;
namespace NcnHelper {
public class CityAddition {
 public string Id=Guid.NewGuid().ToString("N"),Name="",Text="",CreatedUtc=DateTime.UtcNow.ToString("o");public bool Enabled=true;
 public override string ToString(){return (Enabled?"[ON] ":"[OFF] ")+Name;}
}
public class CitySetup {
 public string Format="palcher.ncnsetup",Name="My city setup",Atmosphere="original",Language="",PlayerDescription="";
 public int Version=1,Profanity=0,Endings=0;public bool PlayerEnabled=false;
 public List<CityAddition> Additions=new List<CityAddition>();
}
public static class CityPresets {
 public static readonly string[] Ids={"original","vanilla-plus","noir","street-comedy","friendly","paranoid"};
 public static readonly string[] Names={"Original NightCityNet atmosphere","Vanilla+","Dark Noir","Street Comedy","Friendly Night City","Paranoid Night City"};
 public static string Prompt(string id){switch(id){
 case "original":return "";
 case "vanilla-plus":return "Grounded Night City street life. Residents have varied personalities, daily work, worries and loyalties. React to what is actually happening. Mix caution, dry humor and ordinary courtesy without making everyone hostile, helpful or a comedian.";
 case "noir":return "A restrained noir atmosphere: weary street wisdom, moral ambiguity, guarded trust and dry observations. Let melancholy and uneasy motives color dialogue without narrating imaginary weather, crimes or events. Preserve each character's identity; not everyone is a detective.";
 case "street-comedy":return "Street comedy from believable daily frustrations: bills, cheap food, unreliable cyberware and corporate absurdity. Use situational irony and distinct comic voices rather than repeated punchlines. Do not automatically insult V. Respect the selected profanity setting, including clean dialogue.";
 case "friendly":return "A warmer city beneath the chrome: neighbors can show small kindnesses, practical advice and mutual respect. Keep boundaries, suspicions, faction loyalties and established conflicts. Do not make strangers instantly intimate or erase danger and consequences.";
 case "paranoid":return "Guarded conversations, cautious trust, corporate suspicion and ambiguous rumors. People may question motives when there is evidence, but do not invent conspiracies as confirmed facts or assume V is a cop or enemy. Vary how openly each character expresses suspicion.";
 default:throw new Exception("Unknown city atmosphere preset.");}}
}
public static class CityCodec {
 static void Keys(Dictionary<string,object> d,params string[] allowed){foreach(string k in d.Keys)if(!allowed.Contains(k))throw new Exception("Unsupported setup field: "+k+". Import only a Share My Setup file.");}
 static string TextField(Dictionary<string,object>d,string k){object v;if(!d.TryGetValue(k,out v)||!(v is string))throw new Exception("Invalid setup text field: "+k);return (string)v;}
 static int Number(Dictionary<string,object>d,string k){object v;if(!d.TryGetValue(k,out v)||!(v is int))throw new Exception("Invalid setup number: "+k);return (int)v;}
 static bool Bool(Dictionary<string,object>d,string k){object v;if(!d.TryGetValue(k,out v)||!(v is bool))throw new Exception("Invalid setup flag: "+k);return (bool)v;}
 public static void Validate(CitySetup d){
 if(d==null||d.Format!="palcher.ncnsetup"||d.Version!=1)throw new Exception("Unsupported setup format or version.");
 if(String.IsNullOrWhiteSpace(d.Name)||d.Name.Length>80)throw new Exception("Setup name must contain 1-80 characters.");
 if(!CityPresets.Ids.Contains(d.Atmosphere))throw new Exception("Unknown atmosphere preset.");
 if(!new[]{"","reset","en","uk","uk-ru","pl","de","fr","es","it","pt","ja"}.Contains(d.Language)||d.Profanity<0||d.Profanity>5||d.Endings<0||d.Endings>3)throw new Exception("Unsupported language, profanity or ending preference.");
 if(d.PlayerDescription==null||d.PlayerDescription.Length>3000||d.Additions==null||d.Additions.Count>30)throw new Exception("Player description is limited to 3000 characters; use at most 30 additions.");
 var ids=new HashSet<string>(StringComparer.OrdinalIgnoreCase);int active=d.PlayerEnabled?d.PlayerDescription.Length:0;
 foreach(var a in d.Additions){Guid id;DateTime date;if(a==null||!Guid.TryParse(a.Id,out id)||!ids.Add(a.Id)||!DateTime.TryParse(a.CreatedUtc,System.Globalization.CultureInfo.InvariantCulture,System.Globalization.DateTimeStyles.RoundtripKind,out date)||String.IsNullOrWhiteSpace(a.Name)||a.Name.Length>80||String.IsNullOrWhiteSpace(a.Text)||a.Text.Length>3000)throw new Exception("Invalid addition: use a unique ID, date, name and 1-3000 characters of text.");if(a.Enabled)active+=a.Text.Length;}
 if(active>10000)throw new Exception("Enabled additions and the player description total more than 10,000 characters. Disable or shorten some to keep model context manageable.");
 }
 public static CitySetup Read(string text){
 if(text.Length>262144)throw new Exception("Setup file is too large (maximum 256 KiB).");var m=Json.Read(text);
 Keys(m,"Format","Version","Name","Atmosphere","Language","Profanity","Endings","PlayerDescription","PlayerEnabled","Additions");
 var d=new CitySetup{Format=TextField(m,"Format"),Version=Number(m,"Version"),Name=TextField(m,"Name"),Atmosphere=TextField(m,"Atmosphere"),Language=TextField(m,"Language"),Profanity=Number(m,"Profanity"),Endings=Number(m,"Endings"),PlayerDescription=TextField(m,"PlayerDescription"),PlayerEnabled=Bool(m,"PlayerEnabled")};
 object raw;if(!m.TryGetValue("Additions",out raw)||!(raw is IEnumerable)||raw is string||raw is Dictionary<string,object>)throw new Exception("Invalid setup additions.");
 foreach(object item in (IEnumerable)raw){var a=item as Dictionary<string,object>;if(a==null)throw new Exception("Invalid setup addition.");Keys(a,"Id","Name","Text","CreatedUtc","Enabled");d.Additions.Add(new CityAddition{Id=TextField(a,"Id"),Name=TextField(a,"Name"),Text=TextField(a,"Text"),CreatedUtc=TextField(a,"CreatedUtc"),Enabled=Bool(a,"Enabled")});}
 Validate(d);return d;
 }
 public static string Write(CitySetup d){Validate(d);return Json.Write(d);}
 public static CitySetup Clone(CitySetup d){return Read(Write(d));}
 public static CitySetup Import(string file){if(new FileInfo(file).Length>262144)throw new Exception("Setup file is too large (maximum 256 KiB).");return Read(File.ReadAllText(file));}
 public static string Describe(CitySetup d){return d.Name+"\r\nAtmosphere: "+CityPresets.Names[Array.IndexOf(CityPresets.Ids,d.Atmosphere)]+"\r\nLanguage: "+(d.Language==""?"leave unchanged":d.Language)+"; profanity mode: "+d.Profanity+"; endings mode: "+d.Endings+"\r\n\r\n"+String.Join("\r\n\r\n",d.Additions.Select(a=>a.ToString()+"\r\n"+a.Text))+"\r\n\r\nPlayer description "+(d.PlayerEnabled?"enabled":"disabled")+":\r\n"+d.PlayerDescription;}
}
public static class CityStore {
 public static string Folder(string game){string identity=String.IsNullOrWhiteSpace(game)?"unassigned":Path.GetFullPath(game).TrimEnd('\\').ToLowerInvariant();return Path.Combine(Core.Base,"Setup Library",Core.Hash(Encoding.UTF8.GetBytes(identity)).Substring(0,24));}
 public static CitySetup Load(string game){string f=Path.Combine(Folder(game),"current.ncnsetup.json");return File.Exists(f)?CityCodec.Import(f):new CitySetup();}
 static void Atomic(string path,string text){Core.NoLinks(path,Core.Base);string temp=path+"."+Guid.NewGuid().ToString("N")+".tmp";try{File.WriteAllText(temp,text,new UTF8Encoding(false));if(File.Exists(path))File.Replace(temp,path,null);else File.Move(temp,path);}finally{if(File.Exists(temp))File.Delete(temp);}}
 public static void Save(string game,CitySetup d,string action,bool updateDraft=true){string text=CityCodec.Write(d),dir=Folder(game),history=Path.Combine(dir,"History");Core.NoLinks(history,Core.Base);Core.PrivateDir(dir);Core.PrivateDir(history);string stamp=DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-fff")+"-"+Guid.NewGuid().ToString("N").Substring(0,8);string snapshot=Path.Combine(history,stamp+".ncnsetup.json");Atomic(snapshot,text);Atomic(Path.Combine(history,stamp+".action.txt"),action);Atomic(Path.Combine(dir,updateDraft?"current.ncnsetup.json":"applied.ncnsetup.json"),text);}
 public static string[] History(string game){string dir=Path.Combine(Folder(game),"History");return Directory.Exists(dir)?Directory.GetFiles(dir,"*.ncnsetup.json").OrderByDescending(x=>x).ToArray():new string[0];}
 public static string HistoryLabel(string path){string action=Path.ChangeExtension(Path.ChangeExtension(path,null),".action.txt");return Path.GetFileName(path).Substring(0,Math.Min(19,Path.GetFileName(path).Length))+" UTC - "+(File.Exists(action)?File.ReadAllText(action):"saved setup");}
}
public static class CityPrompts {
 static string Literal(string text){return "{{ "+Json.Write(text).Replace("{","\\u007b").Replace("}","\\u007d")+" }}";}
 public static void Build(Plan p,CitySetup d){if(d==null)return;CityCodec.Validate(d);string preset=CityPresets.Prompt(d.Atmosphere);var additions=d.Additions.Where(a=>a.Enabled).ToArray();
 string city=preset==""&&additions.Length==0?"":"## Optional city atmosphere and author additions\nThese are reversible dialogue directions, not new quest facts or completed events. Preserve personal identities, relationships, earned knowledge, actual game actions and native tags. Atmosphere is not automatic hostility. Respect the selected language, profanity and reply-ending preferences, including clean dialogue. Do not rewrite V's choices, expose secrets to strangers, or make an authored wish into an event that already happened. Do not announce these directions.\n"+(preset==""?"":Literal(preset)+"\n")+String.Join("\n",additions.Select(a=>Literal(a.Name+": "+a.Text)));
 string look=!d.PlayerEnabled||String.IsNullOrWhiteSpace(d.PlayerDescription)?"":"{% if existsIn(ctx, \"speakerPlace\") and existsIn(ctx.speakerPlace, \"withV\") and ctx.speakerPlace.withV %}\n## Player-authored visible description of V\nUse the following only for visible details the NPC can actually observe while with V. Current game clothing, sight, body and scene evidence take precedence if they contradict this authored description. It is not a change to the actual game character model, a public reputation, a relationship, hidden thoughts or knowledge of V's past. Do not assume a gender or appearance beyond what is explicitly described or observed. Do not recite the description unprompted.\n"+Literal(d.PlayerDescription)+"\n{% endif %}";
 foreach(string name in new[]{"settings.tpl","system.tpl"}){string path=Path.Combine(Core.Mod(p.Game),@"data\templates",name);if(!File.Exists(path))continue;var staged=p.Changes.FirstOrDefault(x=>String.Equals(x.Target,path,StringComparison.OrdinalIgnoreCase));string text=staged==null?File.ReadAllText(path):Encoding.UTF8.GetString(staged.Bytes).TrimStart((char)0xfeff);text=Core.Overlay(Core.Overlay(text,"CITY",city),"PLAYER_LOOK",look);byte[] bytes=Core.Encode(text,path);if(staged==null)Core.Add(p,path,bytes,null);else{staged.Bytes=bytes;staged.AfterHash=staged.HashNew();}}
 p.Summary.Add("City setup: "+d.Name+"; "+additions.Length+" enabled additions. Player description "+(d.PlayerEnabled?"enabled":"disabled")+". Only helper prompt sections change; memory and character profiles remain intact.");
 }
}
}
