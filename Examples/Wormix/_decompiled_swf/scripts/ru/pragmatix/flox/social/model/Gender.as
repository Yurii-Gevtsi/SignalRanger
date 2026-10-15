package ru.pragmatix.flox.social.model
{
   import §_-73I§.*;
   import §_-C3f§.*;
   import §_-Z1K§.§_-q24§;
   import §_-hO§.§_-729§;
   import §_-l1g§.§_-L3Q§;
   import com.greensock.*;
   import com.greensock.plugins.*;
   import ru.pragmatix.wormix.model.rewards.§_-P2q§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import starling.display.DisplayObject;
   import starling.events.EventDispatcher;
   import starling.utils.Pool;
   
   public class Gender
   {
      
      public static const MALE:Gender = new Gender(1);
      
      public static const FEMALE:Gender = new Gender(0);
      
      private var _code:int;
      
      public function Gender(param1:int)
      {
         super();
         this._code = param1;
      }
      
      public static function valueOf(param1:int) : Gender
      {
         if(param1 == 0)
         {
            return FEMALE;
         }
         return MALE;
      }
      
      public function get code() : int
      {
         return this._code;
      }
      
      public function equals(param1:Gender) : Boolean
      {
         return Boolean(param1) && this.code == param1.code;
      }
   }
}

