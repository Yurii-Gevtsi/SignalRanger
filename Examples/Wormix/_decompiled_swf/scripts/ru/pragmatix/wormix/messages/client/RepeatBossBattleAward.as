package ru.pragmatix.wormix.messages.client
{
   import §_-11L§.*;
   import §_-31W§.*;
   import §_-62§.§_-F1D§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-W1r§;
   import §_-63U§.§_-9d§;
   import §_-DJ§.§_-fH§;
   import §_-L1H§.§_-b2K§;
   import §_-Ly§.§_-81L§;
   import §_-RE§.§_-r2q§;
   import §_-YF§.§_-q2v§;
   import §_-aM§.*;
   import §_-e2r§.*;
   import §_-j1w§.§_-L1x§;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.*;
   import §_-lh§.§_-z2A§;
   import §_-r1C§.§_-h2B§;
   import §_-z1g§.§_-y1S§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.events.TweenEvent;
   import com.greensock.plugins.*;
   import config.§_-V2w§;
   import feathers.controls.TabBar;
   import flash.display.*;
   import flash.events.*;
   import flash.geom.Point;
   import flash.utils.*;
   import ru.pragmatix.flox.logger.*;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-G1F§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class RepeatBossBattleAward
   {
      
      public var adEventId:String = "";
      
      public var sessionKey:String;
      
      public var battleId:int;
      
      public function RepeatBossBattleAward()
      {
         super();
      }
      
      public function toString() : String
      {
         return "RepeatBossBattleAward{" + "battleId=" + this.battleId + ",adEventId=" + this.adEventId + ",sessionKey=" + this.sessionKey + "}";
      }
   }
}

