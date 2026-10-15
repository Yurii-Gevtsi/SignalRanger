package ru.pragmatix.wormix.serialization.server
{
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-F2Q§.GestureEvent;
   import §_-Y1O§.§_-42h§;
   import §_-Y1O§.§_-V23§;
   import §_-l1K§.*;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-zI§.§_-sI§;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import org.gestouch.core.GestureState;
   import ru.pragmatix.wormix.messages.server.BuyResetBonusItemsResult;
   import ru.pragmatix.wormix.utils.§_-z1x§;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import starling.animation.Tween;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.utils.Pool;
   
   public class BuyResetBonusItemsResultBinarySerializer implements §_-D2L§
   {
      
      public function BuyResetBonusItemsResultBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:BuyResetBonusItemsResult = new BuyResetBonusItemsResult();
         _loc4_.result = _loc3_.readShort();
         _loc4_.requestNum = _loc3_.readInt();
         _loc4_.isSecure = §_-U11§.isSecure(_loc3_,§_-6L§.§_-95§);
         return _loc4_;
      }
      
      public function getCommandId() : uint
      {
         return 13010;
      }
   }
}

