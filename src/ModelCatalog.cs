using System;using System.IO;using System.Linq;
namespace NcnHelper {
 public static class ModelCatalog {
 public static string Resolve(string mode,string language,string folder,string manual,int style=0){
 var files=Directory.Exists(folder)?Directory.GetFiles(folder,"*.gguf",SearchOption.AllDirectories):new string[0];
 if(mode!="custom"){
 string size=mode=="auto"&&(style==1||style==5)?"4B":mode=="fast"?"2B":mode=="multi"?"4B":language=="en"?"2B":"4B";
 string wanted="Qwen3.5-"+size+"-Q4_K_M.gguf";
 var match=files.Where(f=>String.Equals(Path.GetFileName(f),wanted,StringComparison.OrdinalIgnoreCase)).ToArray();
 if(match.Length>1)throw new Exception("Multiple copies of "+wanted+". Keep one copy or select a custom model.");
 if(match.Length==1)return match[0];
 if(mode!="auto")throw new Exception("Put "+wanted+" in Models / LLM. Models are not included in the public package.");
 }
 if(File.Exists(manual))return manual;
 if(files.Length==1)return files[0];
 if(files.Length==0)throw new Exception("Put a .gguf in Models / LLM or select an existing model.");
 return null;
 }
 }
}
