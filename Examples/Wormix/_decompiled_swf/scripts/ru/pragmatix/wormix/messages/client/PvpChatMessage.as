package ru.pragmatix.wormix.messages.client
{
   import §_-63U§.§_-y2O§;
   import §_-91B§.§_-z2l§;
   import §_-DJ§.§_-T2G§;
   import §_-F39§.§_-D14§;
   import §_-F39§.§_-Ey§;
   import §_-gG§.§_-8u§;
   import §_-gG§.§_-M2S§;
   import §_-gG§.§_-Z2z§;
   import §_-j1w§.§_-B2i§;
   import §_-l1g§.§_-L3Q§;
   import §_-w2W§.§_-21w§;
   import §_-w2W§.§_-v27§;
   import §_-w2i§.§_-52k§;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.structures.PumpReactionRateStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.*;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   public class PvpChatMessage
   {
      
      public static const ID:uint = 17;
      
      public var playerNum:uint;
      
      public var action:int = 0;
      
      public var message:String;
      
      public var battleId:uint;
      
      public var isTeamMsg:Boolean;
      
      public function PvpChatMessage()
      {
         super();
      }
   }
}

