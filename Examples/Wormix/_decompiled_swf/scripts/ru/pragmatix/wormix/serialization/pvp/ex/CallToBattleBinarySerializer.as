package ru.pragmatix.wormix.serialization.pvp.ex
{
   import §_-62§.§_-T1y§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-CF§.§_-E19§;
   import §_-D3A§.*;
   import §_-DJ§.§_-J2l§;
   import §_-L1H§.§_-b2K§;
   import §_-Nq§.*;
   import §_-S§.*;
   import §_-TF§.§_-q1j§;
   import §_-Z1L§.§_-A2V§;
   import §_-fo§.AbstractConfigLoader;
   import §_-j22§.§_-5g§;
   import §_-k2s§.§_-8c§;
   import §_-k2s§.§_-C3A§;
   import §_-k2s§.§_-fG§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-z1g§.§_-82X§;
   import com.greensock.loading.core.LoaderCore;
   import com.greensock.plugins.*;
   import feathers.controls.LayoutGroup;
   import feathers.core.ITextRenderer;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.messages.client.WipeProfile;
   import ru.pragmatix.wormix.messages.server.Pong;
   import ru.pragmatix.wormix.messages.structures.PvpProfileStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattle;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import ru.pragmatix.wormix.utils.§_-k1h§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.JumpingMine;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.Color;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   import starlingbuilder.engine.tween.WormixTweenBuilder;
   
   public class CallToBattleBinarySerializer implements §_-D2L§
   {
      
      public function CallToBattleBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return CallToBattle.ID;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:CallToBattle = new CallToBattle();
         _loc4_.profileId = _loc3_.readUnsignedInt();
         _loc4_.battleWager = §_-82X§.valueOf(_loc3_.readShort());
         _loc4_.battleId = _loc3_.readUnsignedInt();
         _loc4_.mapId = _loc3_.readUnsignedInt();
         SerializationUtils.readShortVector(_loc3_,_loc4_.missionIds);
         _loc4_.participantStructs = Vector.<PvpProfileStructure>(SerializationUtils.readStructureVector(_loc3_,PvpProfileStructure));
         _loc4_.clientParams = JSON.parse(_loc3_.readUTF());
         return _loc4_;
      }
   }
}

