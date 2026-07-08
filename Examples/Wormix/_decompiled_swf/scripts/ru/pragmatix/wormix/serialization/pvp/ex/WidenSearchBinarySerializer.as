package ru.pragmatix.wormix.serialization.pvp.ex
{
   import §_-12a§.§_-G2l§;
   import §_-21p§.§_-4w§;
   import §_-62§.§_-O2H§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-83E§;
   import §_-B1y§.§_-F1P§;
   import §_-P2i§.§_-b7§;
   import §_-Z1K§.§_-A2n§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-w2i§.§_-52k§;
   import §_-w2i§.§_-Ia§;
   import §_-z1g§.§_-4l§;
   import §_-z1g§.§_-522§;
   import §_-z1g§.§_-lp§;
   import com.greensock.*;
   import com.greensock.core.*;
   import dragonBones.animation.WorldClock;
   import flash.events.Event;
   import flash.events.IEventDispatcher;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.FontColor;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.gui.screenOverlay.view.*;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.ex.WidenSearch;
   import ru.pragmatix.wormix.utils.§_-71v§;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class WidenSearchBinarySerializer implements §_-D2L§
   {
      
      public function WidenSearchBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return WidenSearch.ID;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:WidenSearch = WidenSearch(param1);
         var _temp_1:* = new §_-5g§();
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return null;
      }
   }
}

