package ru.pragmatix.wormix.serialization.client
{
   import §_-12a§.§_-f1J§;
   import §_-DJ§.§_-fH§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.hurlant.crypto.tls.§_-21v§;
   import com.hurlant.util.§_-F2L§;
   import feathers.controls.Button;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.arena.boss.BossTypesTab;
   import ru.pragmatix.wormix.messages.client.GetArena;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   
   public class GetArenaBinarySerializer implements §_-D2L§
   {
      
      public function GetArenaBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 708;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:GetArena = GetArena(param1);
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
         return new GetArena();
      }
   }
}

