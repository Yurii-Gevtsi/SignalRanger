package ru.pragmatix.wormix.serialization.client
{
   import §_-31N§.§_-Yy§;
   import §_-43V§.*;
   import §_-9e§.§_-73e§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-B2z§.§_-63i§;
   import §_-CF§.§_-E19§;
   import §_-Nq§.*;
   import §_-TF§.§_-q1j§;
   import §_-Y1O§.*;
   import §_-j22§.§_-5g§;
   import §_-k2s§.§_-8c§;
   import §_-k2s§.§_-C3A§;
   import §_-k2s§.§_-fG§;
   import §_-l1K§.§_-vO§;
   import §_-l1g§.§_-L3Q§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import com.hurlant.util.§_-a2P§;
   import feathers.core.ITextRenderer;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import flash.geom.ColorTransform;
   import flash.geom.Rectangle;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.messages.clans.response.ListClansResponse;
   import ru.pragmatix.wormix.messages.client.WipeProfile;
   import ru.pragmatix.wormix.objects.§_-V1L§;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.filters.ColorMatrixFilter;
   import starling.utils.Color;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   
   public class WipeProfileBinarySerializer implements §_-D2L§
   {
      
      public function WipeProfileBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 48;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:WipeProfile = WipeProfile(param1);
         var _loc4_:§_-5g§ = new §_-5g§();
         _loc4_.§_-c2D§(this.getCommandId());
         var _loc5_:ByteArray = new ByteArray();
         _loc5_.writeUTF(_loc3_.confirmCode);
         §_-U11§.§_-E2n§(_loc5_,§_-6L§.§_-95§);
         _loc4_.setLength(_loc5_.length);
         _loc4_.write(param2);
         param2.writeBytes(_loc5_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:WipeProfile = new WipeProfile();
         _loc3_.confirmCode = param1.readUTF();
         return _loc3_;
      }
   }
}

