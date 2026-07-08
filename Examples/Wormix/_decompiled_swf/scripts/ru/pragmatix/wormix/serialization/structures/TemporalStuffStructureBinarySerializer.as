package ru.pragmatix.wormix.serialization.structures
{
   import §_-31W§.§_-r26§;
   import §_-5Y§.§_-h2c§;
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-DJ§.§_-J2l§;
   import §_-G2H§.*;
   import §_-l1K§.§_-W2f§;
   import §_-rM§.§_-02j§;
   import §_-sQ§.§_-H3D§;
   import com.distriqt.extension.inappbilling.Product;
   import com.distriqt.extension.inappbilling.SubscriptionOffer;
   import com.distriqt.extension.inappbilling.SubscriptionPhase;
   import com.distriqt.extension.inappbilling.events.ProductEvent;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.structures.TemporalStuffStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class TemporalStuffStructureBinarySerializer implements §_-02j§
   {
      
      public function TemporalStuffStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _loc4_:TemporalStuffStructure = TemporalStuffStructure(param1);
         param2.writeInt(_loc4_.stuffId);
         param2.writeInt(_loc4_.expireDate);
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:TemporalStuffStructure = new TemporalStuffStructure();
         _loc3_.stuffId = param1.readInt();
         _loc3_.expireDate = param1.readInt();
         return _loc3_;
      }
   }
}

