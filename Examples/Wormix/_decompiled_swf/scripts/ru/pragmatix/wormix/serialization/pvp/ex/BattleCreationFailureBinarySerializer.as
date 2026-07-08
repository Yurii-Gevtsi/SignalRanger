package ru.pragmatix.wormix.serialization.pvp.ex
{
   import §_-931§.§_-02O§;
   import §_-D3A§.§_-TZ§;
   import §_-H2g§.§_-s1d§;
   import §_-J31§.§_-e2J§;
   import §_-N8§.§_-o2w§;
   import §_-Z1K§.*;
   import §_-Z1w§.§_-w1D§;
   import §_-l1K§.§_-F3m§;
   import §_-l1K§.§_-I7§;
   import §_-l1K§.§_-m2§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.*;
   import com.greensock.loading.core.*;
   import config.§_-vR§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.ImageLoader;
   import feathers.controls.Label;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.core.ITextRenderer;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalAlign;
   import feathers.themes.FontColor;
   import feathers.themes.FontSize;
   import flash.events.Event;
   import flash.globalization.DateTimeFormatter;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.gui.§_-gT§;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.pvp.messages.ex.BattleCreationFailure;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattleResultType;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.KeyboardEvent;
   import starling.textures.Texture;
   import starling.utils.Align;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   
   public class BattleCreationFailureBinarySerializer implements §_-D2L§
   {
      
      public function BattleCreationFailureBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return BattleCreationFailure.ID;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ByteArray = new ByteArray();
         param1.readBytes(_loc3_,0,param1.bytesAvailable);
         var _loc4_:BattleCreationFailure = new BattleCreationFailure();
         _loc4_.battleId = _loc3_.readUnsignedInt();
         _loc4_.refuserId = _loc3_.readUnsignedInt();
         _loc4_.socialNetId = _loc3_.readByte();
         _loc4_.reason = CallToBattleResultType.valueOf(_loc3_.readShort());
         return _loc4_;
      }
   }
}

