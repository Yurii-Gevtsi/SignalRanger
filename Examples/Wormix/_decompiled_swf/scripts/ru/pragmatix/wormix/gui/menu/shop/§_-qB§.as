package ru.pragmatix.wormix.gui.menu.shop
{
   import §_-02q§.§_-4s§;
   import §_-A1K§.§_-7u§;
   import §_-H3§.*;
   import §_-L1H§.§_-C3e§;
   import §_-L1H§.§_-b2K§;
   import §_-Ly§.§_-81L§;
   import §_-YF§.§_-N3§;
   import com.amanitadesign.steam.SteamConstants;
   import com.amanitadesign.steam.SteamEvent;
   import com.distriqt.extension.inappbilling.events.InAppBillingEvent;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ScreenNavigatorItem;
   import feathers.core.ITextRenderer;
   import flash.geom.Rectangle;
   import flash.utils.ByteArray;
   import ru.pragmatix.flox.social.utils.id.GetAuthKeyEvent;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.clans.chat.NotInClanChatScreen;
   import ru.pragmatix.wormix.gui.menu.clans.chat.§_-D2t§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.ex.CreateBattleRequest;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.ammo.AirMissile;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   
   public class §_-qB§
   {
      
      private var §_-O22§:DisplayObjectContainer;
      
      public function §_-qB§(param1:DisplayObjectContainer)
      {
         super();
         this.§_-O22§ = param1;
      }
      
      public function init(param1:String, param2:String, param3:String, param4:uint) : void
      {
         var _loc5_:Image = new Image(Application.assetManager.getTexture(param1));
         var _loc6_:Rectangle = DisplayObject(§_-YQ§.getClip("iconContainer.back",this.§_-O22§)).bounds;
         _loc6_.setTo(_loc6_.x + _loc6_.width * 0.125,_loc6_.x + _loc6_.height * 0.125,_loc6_.width * 0.75,_loc6_.height * 0.75);
         var _loc7_:Rectangle = _loc5_.bounds;
         _loc7_ = RectangleUtil.fit(_loc7_,_loc6_,ScaleMode.SHOW_ALL);
         _loc5_.width = _loc7_.width;
         _loc5_.height = _loc7_.height;
         _loc5_.x = _loc7_.x;
         _loc5_.y = _loc7_.y;
         DisplayObjectContainer(§_-YQ§.getClip("iconContainer",this.§_-O22§)).addChild(_loc5_);
         ITextRenderer(§_-YQ§.getClip("text",this.§_-O22§)).text = param2;
         ITextRenderer(§_-YQ§.getClip("value",this.§_-O22§)).text = param3;
         Object(§_-YQ§.getClip("value",this.§_-O22§)).textFormat.color = param4;
      }
      
      public function §_-tt§() : DisplayObject
      {
         return this.§_-O22§;
      }
   }
}

