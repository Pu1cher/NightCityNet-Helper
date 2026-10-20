using System;using System.IO;using System.Linq;using System.Text;
namespace NcnHelper {
public static class ReplyEndings {
 public static string Prompt(int mode){
 string rule=mode==1?"NATURAL / SOMETIMES: End a reply with a question only when the character genuinely wants information, clarification or a choice. A complete statement, observation, joke, refusal or quiet ending is equally valid. Do not append a question, taunt, demand or challenge merely to keep the conversation going. V can reply without being explicitly invited.":mode==2?"ALWAYS: Make the last spoken sentence of each reply one relevant, natural question addressed to the current conversation partner. It must fit the scene and character; do not invent a confrontation or repeat a stock question. Do not ask a chain of questions. Native control tags still follow the spoken text as required.":mode==3?"NEVER: Do not finish replies with questions, including rhetorical questions. Finish with a natural statement, observation, reaction or suggestion instead. Express uncertainty as a statement rather than appending a question to engage V.":"";
 if(rule=="")throw new Exception("Unknown reply-ending preference.");
 return "## Reply-ending preference - helper override\n"+rule+" This instruction controls only how a reply ends. Keep the existing dialogue length, selected language, profanity mode, facts, personality and native output format. Do not speak for V. Do not turn the preference into a story fact or memory. This ending preference overrides conflicting conversational-hook advice in other helper sections.\n";
 }
 static string SmoothLegacy(string text,string kind){
 string start="{# NCN_HELPER_"+kind+"_BEGIN #}",end="{# NCN_HELPER_"+kind+"_END #}";
 int a=text.IndexOf(start),b=text.IndexOf(end);if(a<0||b<a)return text;
 string block=text.Substring(a,b+end.Length-a),updated=block;
 updated=updated.Replace("End with a natural opening for V to answer: a pointed question, taunt, challenge, choice or demand. Do not mechanically ask a question every time; a sharp unfinished dispute is enough.","Finish naturally. A question, taunt or challenge is optional, never an obligatory ending. A complete statement is a valid way to finish.");
 updated=updated.Replace("Залиши йому привід одразу відповісти.","Запитання або виклик необов’язкові; можна просто завершити думку.");
 updated=updated.Replace("Закінчи викликом чи питанням, щоб Ві міг огризнутися.","Питання чи виклик необов’язкові; заверши репліку природно.");
 return text.Remove(a,block.Length).Insert(a,updated);
 }
 public static void Build(Plan p,int mode){if(mode==0)return;if(mode<1||mode>4)throw new Exception("Unknown reply-ending preference.");
 foreach(string name in new[]{"settings.tpl","system.tpl"}){
 string path=Path.Combine(Core.Mod(p.Game),@"data\templates",name);if(!File.Exists(path))continue;
 var staged=p.Changes.FirstOrDefault(x=>String.Equals(x.Target,path,StringComparison.OrdinalIgnoreCase));
 string text=staged==null?File.ReadAllText(path):Encoding.UTF8.GetString(staged.Bytes).TrimStart((char)0xfeff);
 text=SmoothLegacy(SmoothLegacy(text,"SPEED"),"SWEAR");
 text=Core.Overlay(text,"ENDINGS",mode==4?"":Prompt(mode));
 byte[] bytes=Core.Encode(text,path);if(staged==null)Core.Add(p,path,bytes,null);else{staged.Bytes=bytes;staged.AfterHash=staged.HashNew();}
 }
 p.Summary.Add(mode==4?"Removed the helper reply-ending preference; existing mod rules apply.":"Reply endings: "+(mode==1?"natural / sometimes":mode==2?"always a relevant question":"never a question")+". Reply length and game configuration are unchanged.");
 }
}
}
