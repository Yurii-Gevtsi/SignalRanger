package ru.pragmatix.wormix.serialization.client
{
   import §_-31W§.*;
   import §_-62§.§_-gr§;
   import §_-73I§.§_-q15§;
   import §_-C3f§.§_-Z1Y§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F39§.§_-a2W§;
   import §_-F39§.§_-i1i§;
   import §_-Nq§.*;
   import §_-TF§.§_-q1j§;
   import §_-Y1b§.§_-OP§;
   import §_-j22§.§_-5g§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.GroupedList;
   import feathers.controls.Scroller;
   import feathers.controls.renderers.DefaultGroupedListHeaderOrFooterRenderer;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.core.ITextRenderer;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalLayout;
   import flash.events.Event;
   import flash.geom.Rectangle;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.pvp.messages.client.BattleLogin;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.HealBlocker;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   
   public class BattleLoginBinarySerializer implements §_-D2L§
   {
      
      public function BattleLoginBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 1001;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:BattleLogin = BattleLogin(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUnsignedInt(_loc3_.id);
         _loc5_.writeBoolean(_loc3_.firstTurn);
         _loc5_.writeUTF(_loc3_.authKey);
         var _loc6_:uint = 0;
         var _loc7_:uint = _loc3_.enemyIds.length;
         _loc5_.writeShort(_loc7_);
         _loc6_ = 0;
         while(_loc6_ < _loc7_)
         {
            _loc5_.writeUnsignedInt(_loc3_.enemyIds[_loc6_]);
            _loc6_++;
         }
         _loc5_.writeUTF(_loc3_.mainHost);
         _loc5_.writeInt(_loc3_.mainPort);
         _loc5_.writeUnsignedInt(_loc3_.battleId);
         _loc5_.writeByte(_loc3_.socialId);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:BattleLogin = new BattleLogin();
         _loc3_.id = param1.readUnsignedInt();
         _loc3_.firstTurn = param1.readBoolean();
         _loc3_.authKey = param1.readUTF();
         _loc3_.socialId = param1.readByte();
         var _loc4_:uint = 0;
         _loc3_.enemyIds = new Array();
         var _loc5_:uint = uint(param1.readShort());
         _loc4_ = 0;
         while(_loc4_ < _loc5_)
         {
            _loc3_.enemyIds[_loc4_] = param1.readUnsignedInt();
            _loc4_++;
         }
         _loc3_.mainHost = param1.readUTF();
         _loc3_.mainPort = param1.readInt();
         _loc3_.battleId = param1.readUnsignedInt();
         _loc3_.socialId = param1.readByte();
         return _loc3_;
      }
   }
}

