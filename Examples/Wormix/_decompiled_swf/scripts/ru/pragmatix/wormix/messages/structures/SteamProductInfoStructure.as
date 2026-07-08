package ru.pragmatix.wormix.messages.structures
{
   import flash.events.StatusEvent;
   
   public class SteamProductInfoStructure
   {
      
      public var code:String;
      
      public var category:String;
      
      public var cost:int;
      
      public var description:String;
      
      public function SteamProductInfoStructure()
      {
         super();
      }
      
      public function toString() : String
      {
         return "SteamProductInfoStructure{code=" + String(this.code) + ",category=" + String(this.category) + ",cost=" + String(this.cost) + ",description=" + String(this.description) + "}";
      }
   }
}

