package ru.pragmatix.wormix.serialization.client
{
   import §_-62§.§_-zF§;
   import §_-A2U§.§_-A23§;
   import §_-TF§.§_-q1j§;
   import §_-hO§.§_-y2Z§;
   import §_-j22§.§_-5g§;
   import §_-n1w§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.hurlant.crypto.tls.§_-21v§;
   import feathers.controls.Label;
   import feathers.themes.styles.AlternateTextStyles;
   import flash.geom.Point;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.BuyReactionRate;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.§_-71v§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.Antimagnet;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.events.Event;
   import starling.text.TextField;
   import starling.text.TextFormat;
   import starling.utils.Align;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class BuyReactionRateBinarySerializer implements §_-D2L§
   {
      
      public function BuyReactionRateBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 49;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:BuyReactionRate = BuyReactionRate(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeInt(_loc3_.reactionRateCount);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:BuyReactionRate = new BuyReactionRate();
         _loc3_.reactionRateCount = param1.readByte();
         return _loc3_;
      }
   }
}

