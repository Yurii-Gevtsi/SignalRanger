package ru.pragmatix.wormix.serialization.structures
{
   import §_-31Y§.§_-D3B§;
   import §_-73I§.§_-Z2e§;
   import §_-73d§.§_-w1O§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.*;
   import §_-P1B§.§_-EB§;
   import §_-P1B§.§_-F1q§;
   import §_-P2i§.§_-G3J§;
   import §_-U1i§.§_-I3t§;
   import §_-g2e§.§_-q2Z§;
   import §_-j2R§.§_-v1T§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-rM§.§_-02j§;
   import §_-sQ§.*;
   import §_-w2W§.§_-21w§;
   import §_-w2i§.*;
   import dragonBones.animation.WorldClock;
   import feathers.core.PopUpManager;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.structures.BossWormStructure;
   import ru.pragmatix.wormix.messages.structures.TurnStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.bonus.§_-z17§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.*;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   
   public class TurnStructureBinarySerializer implements §_-02j§
   {
      
      public function TurnStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _loc6_:ByteArray = null;
         var _loc4_:TurnStructure = TurnStructure(param1);
         param2.writeShort(_loc4_.turnNum);
         param2.writeBoolean(_loc4_.isPlayerTurn);
         param2.writeInt(_loc4_.endTime);
         param2.writeInt(_loc4_.damageToPlayer);
         param2.writeInt(_loc4_.damageToBoss);
         var _loc5_:uint = _loc4_.items.length;
         param2.writeShort(_loc5_);
         var _loc7_:int = 0;
         while(_loc7_ < _loc5_)
         {
            _loc6_ = new ByteArray();
            param2.writeShort(0);
            §_-616§.§_-fk§(WeaponStructure).serializeStructure(_loc4_.items[_loc7_],_loc6_,null);
            param2.writeBytes(_loc6_);
            _loc7_++;
         }
         _loc5_ = _loc4_.deaths.length;
         param2.writeShort(_loc5_);
         _loc7_ = 0;
         while(_loc7_ < _loc5_)
         {
            _loc6_ = new ByteArray();
            param2.writeShort(0);
            §_-616§.§_-fk§(BossWormStructure).serializeStructure(_loc4_.deaths[_loc7_],_loc6_,null);
            param2.writeBytes(_loc6_);
            _loc7_++;
         }
         _loc5_ = _loc4_.births.length;
         param2.writeShort(_loc5_);
         _loc7_ = 0;
         while(_loc7_ < _loc5_)
         {
            _loc6_ = new ByteArray();
            param2.writeShort(0);
            §_-616§.§_-fk§(BossWormStructure).serializeStructure(_loc4_.births[_loc7_],_loc6_,null);
            param2.writeBytes(_loc6_);
            _loc7_++;
         }
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:TurnStructure = new TurnStructure();
         _loc3_.turnNum = param1.readShort();
         _loc3_.isPlayerTurn = param1.readBoolean();
         _loc3_.endTime = param1.readInt();
         _loc3_.damageToPlayer = param1.readInt();
         _loc3_.damageToBoss = param1.readInt();
         var _loc4_:uint = param1.readUnsignedShort();
         _loc3_.items = [];
         var _loc5_:int = 0;
         while(_loc5_ < _loc4_)
         {
            param1.readShort();
            _loc3_.items[_loc5_] = §_-616§.§_-fk§(WeaponStructure).deserializeStructure(param1,null);
            _loc5_++;
         }
         _loc4_ = param1.readUnsignedShort();
         _loc3_.deaths = [];
         _loc5_ = 0;
         while(_loc5_ < _loc4_)
         {
            param1.readShort();
            _loc3_.deaths[_loc5_] = §_-616§.§_-fk§(BossWormStructure).deserializeStructure(param1,null);
            _loc5_++;
         }
         _loc4_ = param1.readUnsignedShort();
         _loc3_.births = [];
         _loc5_ = 0;
         while(_loc5_ < _loc4_)
         {
            param1.readShort();
            _loc3_.births[_loc5_] = §_-616§.§_-fk§(BossWormStructure).deserializeStructure(param1,null);
            _loc5_++;
         }
         return _loc3_;
      }
   }
}

