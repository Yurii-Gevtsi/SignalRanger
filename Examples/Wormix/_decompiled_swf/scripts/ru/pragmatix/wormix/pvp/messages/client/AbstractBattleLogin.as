package ru.pragmatix.wormix.pvp.messages.client
{
   import §_-62§.§_-zF§;
   import §_-r1C§.§_-h2B§;
   import flash.display.InteractiveObject;
   import flash.events.EventPhase;
   import flash.events.MouseEvent;
   import flash.utils.getQualifiedClassName;
   import flash.utils.setTimeout;
   import org.gestouch.core.gestouch_internal;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import starling.events.Event;
   
   use namespace gestouch_internal;
   
   public class AbstractBattleLogin
   {
      
      public var id:uint;
      
      public var firstTurn:Boolean;
      
      public var authKey:String;
      
      public var enemyIds:Array;
      
      public var mainHost:String;
      
      public var mainPort:int;
      
      public var battleId:uint;
      
      public var socialId:uint = 1;
      
      public function AbstractBattleLogin()
      {
         super();
         this.socialId = §_-X1j§.§_-It§.getPlatformType().servercode;
      }
   }
}

