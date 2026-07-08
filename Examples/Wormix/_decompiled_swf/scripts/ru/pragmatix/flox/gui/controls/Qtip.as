package ru.pragmatix.flox.gui.controls
{
   import §_-02q§.§_-11O§;
   import §_-62§.§_-gr§;
   import §_-73I§.§_-q15§;
   import §_-73d§.§_-g1J§;
   import §_-931§.§_-02O§;
   import §_-931§.§_-i2r§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-DJ§.§_-fH§;
   import §_-Ly§.§_-81L§;
   import §_-Nq§.*;
   import §_-RE§.§_-52p§;
   import §_-TF§.§_-q1j§;
   import §_-Vg§.*;
   import §_-fo§.§_-r5§;
   import §_-l1K§.§_-d2j§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import com.hurlant.util.der.*;
   import dragonBones.animation.WorldClock;
   import feathers.core.ITextRenderer;
   import feathers.layout.RelativePosition;
   import flash.display.DisplayObject;
   import flash.geom.Rectangle;
   import flash.text.TextFieldAutoSize;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.PostToClanChatRequest;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.server.ShopResult;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.serialization.server.*;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import starling.display.DisplayObject;
   import starling.display.Quad;
   import starling.display.Sprite;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   
   public class Qtip extends §_-zi§
   {
      
      private var text:§_-i2r§;
      
      private var panel:flash.display.DisplayObject;
      
      public function Qtip(param1:String = null)
      {
         super(§_-r5§.§_-tF§(param1 ? param1 : §_-81L§.§_-C3b§));
         this.text = §_-U2y§("text");
         this.text.autoSize = TextFieldAutoSize.LEFT;
         this.panel = getChildByName("panel");
      }
      
      public function §_-61T§(param1:§_-02O§) : void
      {
         if(param1.format)
         {
            this.text.setFormat(param1.format);
         }
         §_-11O§.§_-JO§(this.text,param1);
         this.panel.width = 2 * this.text.x + this.text.width;
         if(this.text.textField.numLines > 1)
         {
            this.panel.height = 2 * this.text.y + this.text.height;
         }
      }
      
      override public function destroy() : void
      {
         this.text = null;
         this.panel = null;
         super.destroy();
      }
   }
}

