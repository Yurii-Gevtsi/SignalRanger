package ru.pragmatix.wormix.serialization.structures
{
   import §_-31W§.§_-Kb§;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-ts§;
   import §_-CF§.§_-x2t§;
   import §_-D3A§.§_-TZ§;
   import §_-Ta§.BuyVipDialog;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-hK§;
   import §_-b1F§.§_-G4§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-02j§;
   import com.distriqt.extension.cloudstorage.CloudStorage;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.utils.id.UserIdType;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.structures.BossWormStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.items.craft.*;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.physics.model.NotStuckingPhysicModel;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.scene.ObjectType;
   import ru.pragmatix.wormix.social.common.post.*;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.display.Image;
   import starling.events.EventDispatcher;
   import starling.text.TextField;
   import starling.text.TextFieldAutoSize;
   
   public class BossWormStructureBinarySerializer implements §_-02j§
   {
      
      public function BossWormStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _loc4_:BossWormStructure = BossWormStructure(param1);
         param2.writeBoolean(_loc4_.isPlayerTeam);
         param2.writeInt(_loc4_.hp);
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:BossWormStructure = new BossWormStructure();
         _loc3_.isPlayerTeam = param1.readBoolean();
         _loc3_.hp = param1.readInt();
         return _loc3_;
      }
   }
}

