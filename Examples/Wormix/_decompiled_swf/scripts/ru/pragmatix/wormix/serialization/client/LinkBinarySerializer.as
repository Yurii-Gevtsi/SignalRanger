package ru.pragmatix.wormix.serialization.client
{
   import §_-12a§.§_-41b§;
   import §_-73I§.§_-q15§;
   import §_-73d§.§_-V19§;
   import §_-G3w§.*;
   import §_-TF§.§_-F1B§;
   import §_-TF§.§_-q1j§;
   import §_-j22§.§_-5g§;
   import §_-l1K§.§_-N1Z§;
   import §_-l1K§.§_-W2f§;
   import §_-m1g§.*;
   import §_-qH§.§_-AC§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import com.hurlant.math.*;
   import dragonBones.Slot;
   import dragonBones.animation.WorldClock;
   import feathers.controls.LayoutGroup;
   import feathers.core.FeathersControl;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.HorizontalLayoutData;
   import feathers.layout.VerticalAlign;
   import flash.display.*;
   import flash.events.StageOrientationEvent;
   import flash.events.TimerEvent;
   import flash.geom.ColorTransform;
   import flash.geom.Point;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.LowerRankInClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.LowerRankInClanResponse;
   import ru.pragmatix.wormix.messages.client.Link;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.pvp.messages.ex.WidenSearch;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.utils.§_-J2h§;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.Color;
   import starling.utils.Pool;
   import starlingbuilder.extensions.uicomponents.CustomLabel;
   
   use namespace bi_internal;
   
   public class LinkBinarySerializer implements §_-D2L§
   {
      
      public function LinkBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 30;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:Link = Link(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _temp_2:* = new ByteArray();
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeBoolean(_loc3_.validateOnly);
         _loc5_.writeShort(_loc3_.platformId);
         _loc5_.writeUTF(_loc3_.platformUserId);
         _loc5_.writeUTF(_loc3_.authKey);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return null;
      }
   }
}

