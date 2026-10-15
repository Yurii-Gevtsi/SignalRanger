package ru.pragmatix.wormix.serialization.structures
{
   import §_-62§.*;
   import §_-C3f§.§_-yG§;
   import §_-b1F§.§_-G4§;
   import §_-q26§.*;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-02j§;
   import flash.events.Event;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.request.DeleteClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.QuitClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanBase;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginCreateClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanEnterAccount;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanErrorResponse;
   import ru.pragmatix.wormix.messages.structures.BonusDaysStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import starling.events.Event;
   
   public class BonusDaysStructureBinarySerializer implements §_-02j§
   {
      
      public function BonusDaysStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _loc4_:BonusDaysStructure = BonusDaysStructure(param1);
         param2.writeInt(_loc4_.money);
         param2.writeInt(_loc4_.realMoney);
         param2.writeInt(_loc4_.battlesCount);
         param2.writeUTF(_loc4_.bonusMessage);
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:BonusDaysStructure = null;
         _loc3_ = new BonusDaysStructure();
         _loc3_.money = param1.readInt();
         _loc3_.realMoney = param1.readInt();
         _loc3_.battlesCount = param1.readInt();
         _loc3_.bonusMessage = param1.readUTF();
         return _loc3_;
      }
   }
}

