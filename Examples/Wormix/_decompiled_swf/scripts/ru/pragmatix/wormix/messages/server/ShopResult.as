package ru.pragmatix.wormix.messages.server
{
   import §_-Bv§.§_-G3P§;
   import §_-Tm§.§_-K1I§;
   import §_-b1F§.§_-v2Q§;
   import §_-jF§.*;
   import §_-r1C§.§_-t1R§;
   import com.hurlant.crypto.tls.§_-21v§;
   import com.hurlant.math.BigInteger;
   import flash.events.Event;
   import flash.geom.Point;
   import ru.pragmatix.flox.social.controller.mobile.*;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.StartPvpBattle;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.AirBlocker;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class ShopResult
   {
      
      public static const SUCCESS:int = 0;
      
      public static const ERROR:int = 1;
      
      public static const MIN_REQUIREMENTS_ERROR:int = 2;
      
      public static const NOT_ENOUGH_MONEY:int = 3;
      
      public static const CONFIRM_FAILURE:int = 4;
      
      public var result:int;
      
      public var weapons:Array = [];
      
      public var stuff:Array = [];
      
      public var temporalStuff:Array = [];
      
      public function ShopResult()
      {
         super();
      }
      
      public function toString() : String
      {
         switch(this.result)
         {
            case ERROR:
               return "ShopResult{ result = ERROR }";
            case SUCCESS:
               return "ShopResult{ result = SUCCESS }";
            case MIN_REQUIREMENTS_ERROR:
               return "ShopResult{ result = MIN_REQUIREMENTS_ERROR }";
            case NOT_ENOUGH_MONEY:
               return "ShopResult{ result = NOT_ENOUGH_MONEY }";
            case CONFIRM_FAILURE:
               return "ShopResult{ result = CONFIRM_FAILURE }";
            default:
               return "ShopResult{ result = " + this.result + "}";
         }
      }
   }
}

