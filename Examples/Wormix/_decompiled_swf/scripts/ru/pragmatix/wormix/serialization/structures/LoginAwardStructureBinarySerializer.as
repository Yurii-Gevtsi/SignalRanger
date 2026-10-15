package ru.pragmatix.wormix.serialization.structures
{
   import §_-533§.§_-c2b§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-zF§;
   import §_-A2U§.*;
   import §_-Z1K§.§_-q24§;
   import §_-rM§.§_-02j§;
   import com.greensock.loading.LoaderMax;
   import com.greensock.loading.LoaderStatus;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.controls.system.ControlSystems;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.messages.structures.LoginAwardStructure;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.scene.tile.AtlasTileData;
   import ru.pragmatix.wormix.scene.tile.TileType;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.DynamiteStick;
   import starling.events.Event;
   import starling.utils.AssetManager;
   
   public class LoginAwardStructureBinarySerializer implements §_-02j§
   {
      
      public function LoginAwardStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:LoginAwardStructure = new LoginAwardStructure();
         _loc3_.awardType = param1.readShort();
         _loc3_.awards = SerializationUtils.readStructureArray(param1 as ByteArray,GenericAwardStructure);
         _loc3_.attach = param1.readUTF();
         return _loc3_;
      }
   }
}

