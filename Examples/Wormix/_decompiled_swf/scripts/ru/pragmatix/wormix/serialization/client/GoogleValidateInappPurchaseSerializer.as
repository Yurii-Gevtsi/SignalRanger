package ru.pragmatix.wormix.serialization.client
{
   import §_-21A§.*;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-Bv§.§_-G3P§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-RE§.§_-52p§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-g1P§.§_-L38§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.hurlant.crypto.tls.§_-21v§;
   import com.hurlant.math.BigInteger;
   import com.hurlant.util.§_-F2L§;
   import feathers.core.ToggleGroup;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanChatMessageTO;
   import ru.pragmatix.wormix.messages.client.GoogleValidateInappPurchase;
   import ru.pragmatix.wormix.messages.server.BuyVipResult;
   import ru.pragmatix.wormix.messages.structures.ProfileDoubleKeyStructure;
   import ru.pragmatix.wormix.messages.structures.RentedItemsStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class GoogleValidateInappPurchaseSerializer implements §_-D2L§
   {
      
      public function GoogleValidateInappPurchaseSerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:GoogleValidateInappPurchase = GoogleValidateInappPurchase(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.productId);
         _loc5_.writeBoolean(_loc3_.isSubscription);
         _loc5_.writeUTF(_loc3_.purchaseToken);
         _loc5_.writeUTF(_loc3_.sessionKey);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:GoogleValidateInappPurchase = new GoogleValidateInappPurchase();
         _loc3_.productId = param1.readUTF();
         _loc3_.isSubscription = param1.readBoolean();
         _loc3_.purchaseToken = param1.readUTF();
         _loc3_.sessionKey = param1.readUTF();
         _loc3_.isSecure = param1 is ByteArray && §_-U11§.isSecure(param1 as ByteArray,§_-6L§.§_-95§);
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return 103;
      }
   }
}

