package ru.pragmatix.wormix.serialization.structures
{
   import §_-82a§.§_-H11§;
   import §_-82a§.§_-pp§;
   import §_-937§.§_-BO§;
   import §_-D3A§.§_-TZ§;
   import §_-L1H§.§_-b2K§;
   import §_-Tm§.§_-RF§;
   import §_-l1g§.§_-L3Q§;
   import §_-o14§.*;
   import §_-rM§.§_-02j§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import feathers.controls.Label;
   import feathers.controls.text.TextFieldTextRenderer;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.layout.VerticalAlign;
   import feathers.themes.FontSize;
   import flash.geom.Point;
   import flash.text.TextFormatAlign;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.structures.UnitStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.scene.tile.AtlasTileData;
   import ru.pragmatix.wormix.scene.tile.TileType;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.events.Event;
   import starling.utils.Pool;
   import starling.utils.ScaleMode;
   
   public class UnitStructureBinarySerializer implements §_-02j§
   {
      
      public function UnitStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _loc4_:UnitStructure = UnitStructure(param1);
         param2.writeInt(_loc4_.ownerId);
         param2.writeByte(_loc4_.armor);
         param2.writeByte(_loc4_.attack);
         param2.writeByte(_loc4_.level);
         param2.writeInt(_loc4_.experience);
         param2.writeShort(_loc4_.hatId);
         param2.writeByte(_loc4_.raceId);
         param2.writeByte(_loc4_.skin);
         param2.writeShort(_loc4_.artifactId);
         param2.writeUTF(UserProfileStructure.getSocialIdsAsString(_loc4_.socialOwnerIds));
         param2.writeUTF(_loc4_.name);
         param2.writeShort(_loc4_.teamMemberType);
         param2.writeByte(_loc4_.isActive ? 1 : 0);
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:UnitStructure = null;
         _loc3_ = new UnitStructure();
         _loc3_.ownerId = param1.readInt();
         _loc3_.armor = param1.readByte();
         _loc3_.attack = param1.readByte();
         _loc3_.level = param1.readByte();
         _loc3_.experience = param1.readInt();
         _loc3_.hatId = param1.readShort();
         _loc3_.raceId = param1.readByte();
         _loc3_.skin = param1.readByte();
         _loc3_.artifactId = param1.readShort();
         _loc3_.socialOwnerIds = UserProfileStructure.parseSocialIdsString(param1.readUTF());
         _loc3_.name = param1.readUTF();
         _loc3_.teamMemberType = param1.readShort();
         _loc3_.isActive = param1.readByte() != 0;
         return _loc3_;
      }
   }
}

