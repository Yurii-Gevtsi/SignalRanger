package ru.pragmatix.wormix.messages.structures
{
   import §_-Y1O§.§_-V23§;
   
   public class TemporalStuffStructure
   {
      
      public var stuffId:int;
      
      public var expireDate:int;
      
      public function TemporalStuffStructure()
      {
         super();
      }
      
      public function toString() : String
      {
         return "TemporalStuffStructure{" + "id=" + this.stuffId + ", expire=" + String(this.expireDate) + "}";
      }
   }
}

