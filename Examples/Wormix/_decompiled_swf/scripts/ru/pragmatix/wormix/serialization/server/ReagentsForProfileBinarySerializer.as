package ru.pragmatix.wormix.serialization.server
{
   import §_-CF§.§_-E19§;
   import §_-L1H§.§_-C3e§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-T2N§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.messages.server.ReagentsForProfile;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.model.UnitPhysicModel;
   import ru.pragmatix.wormix.weapons.Jetpack;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster.ShellBurst;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class ReagentsForProfileBinarySerializer implements §_-D2L§
   {
      
      public function ReagentsForProfileBinarySerializer()
      {
         super();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:ReagentsForProfile = new ReagentsForProfile();
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         var _loc6_:int = int(_loc3_.reagents.length);
         param2.writeInt(_loc6_);
         var _loc7_:int = 0;
         while(_loc7_ < _loc6_)
         {
            param2.writeInt(_loc3_.reagents[_loc7_]);
            _loc7_++;
         }
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ReagentsForProfile = new ReagentsForProfile();
         var _loc4_:int = int(param1.readUnsignedShort());
         _loc3_.reagents = new Vector.<int>();
         var _loc5_:int = 0;
         while(_loc5_ < _loc4_)
         {
            _loc3_.reagents[_loc5_] = param1.readInt();
            _loc5_++;
         }
         return _loc3_;
      }
      
      public function getCommandId() : uint
      {
         return 10085;
      }
   }
}

