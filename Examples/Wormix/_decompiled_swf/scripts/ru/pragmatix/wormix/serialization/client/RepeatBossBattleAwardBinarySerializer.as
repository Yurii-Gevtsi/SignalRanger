package ru.pragmatix.wormix.serialization.client
{
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-CF§.§_-E19§;
   import §_-DJ§.MessageBox;
   import §_-DJ§.§_-fH§;
   import §_-J31§.§_-R1h§;
   import §_-R2I§.§_-H7§;
   import §_-R2I§.§_-u2V§;
   import §_-Tm§.Action;
   import §_-Tm§.§_-K1I§;
   import §_-Tm§.§_-RF§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1b§.*;
   import §_-j22§.§_-5g§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-z1g§.§_-82X§;
   import flash.display.BitmapData;
   import flash.display.Shape;
   import flash.events.IOErrorEvent;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.clearTimeout;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.client.BuyShopItems;
   import ru.pragmatix.wormix.messages.client.RepeatBossBattleAward;
   import ru.pragmatix.wormix.messages.structures.ShopItemStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.textures.TextureSmoothing;
   
   public class RepeatBossBattleAwardBinarySerializer implements §_-D2L§
   {
      
      public function RepeatBossBattleAwardBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 143;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:RepeatBossBattleAward = RepeatBossBattleAward(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _temp_2:* = new ByteArray();
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.adEventId);
         _loc5_.writeUTF(_loc3_.sessionKey);
         _loc5_.writeInt(_loc3_.battleId);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
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

