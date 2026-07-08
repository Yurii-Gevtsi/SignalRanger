package ru.pragmatix.wormix.gui.menu
{
   import §_-12a§.§_-6p§;
   import §_-23P§.§_-u1z§;
   import §_-43V§.*;
   import §_-52V§.§_-R9§;
   import §_-61n§.§_-vB§;
   import §_-62§.*;
   import §_-73d§.§_-g1J§;
   import §_-9§.§_-52A§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-A2S§.§_-d2z§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-yG§;
   import §_-CF§.§_-x2t§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-73y§;
   import §_-DJ§.§_-fH§;
   import §_-G2H§.§_-527§;
   import §_-J31§.§_-e2J§;
   import §_-P1z§.*;
   import §_-P2i§.§_-b7§;
   import §_-RE§.§_-D3v§;
   import §_-SP§.§_-M4§;
   import §_-Tm§.Action;
   import §_-Tm§.§_-K1I§;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-U16§;
   import §_-YF§.§_-f2a§;
   import §_-YF§.§_-q2v§;
   import §_-Z1w§.§_-s1x§;
   import §_-Z2S§.*;
   import §_-b1F§.§_-v2Q§;
   import §_-dS§.§_-D3a§;
   import §_-f1G§.§_-kf§;
   import §_-fo§.§_-A29§;
   import §_-fo§.§_-r5§;
   import §_-g2r§.§_-x8§;
   import §_-hv§.*;
   import §_-j22§.§_-5g§;
   import §_-k1u§.§_-Oi§;
   import §_-l1K§.§_-d2j§;
   import §_-l1g§.§_-L3Q§;
   import §_-q26§.*;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-w2i§.§_-Ia§;
   import §_-zI§.§_-8D§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.LoaderMax;
   import com.greensock.loading.LoaderStatus;
   import com.greensock.loading.core.*;
   import com.greensock.plugins.AutoAlphaPlugin;
   import com.greensock.plugins.BevelFilterPlugin;
   import com.greensock.plugins.BezierPlugin;
   import com.greensock.plugins.BezierThroughPlugin;
   import com.greensock.plugins.BlurFilterPlugin;
   import com.greensock.plugins.ColorMatrixFilterPlugin;
   import com.greensock.plugins.ColorTransformPlugin;
   import com.greensock.plugins.DropShadowFilterPlugin;
   import com.greensock.plugins.EndArrayPlugin;
   import com.greensock.plugins.FrameLabelPlugin;
   import com.greensock.plugins.FramePlugin;
   import com.greensock.plugins.GlowFilterPlugin;
   import com.greensock.plugins.HexColorsPlugin;
   import com.greensock.plugins.RemoveTintPlugin;
   import com.greensock.plugins.RoundPropsPlugin;
   import com.greensock.plugins.ShortRotationPlugin;
   import com.greensock.plugins.TintPlugin;
   import com.greensock.plugins.TweenPlugin;
   import com.greensock.plugins.VisiblePlugin;
   import com.greensock.plugins.VolumePlugin;
   import com.hurlant.math.*;
   import com.hurlant.util.der.§_-p1B§;
   import dragonBones.animation.WorldClock;
   import feathers.controls.Button;
   import feathers.controls.ImageLoader;
   import feathers.core.ITextRenderer;
   import flash.display.BitmapData;
   import flash.display.MovieClip;
   import flash.display3D.Context3DTextureFormat;
   import flash.events.*;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import org.gestouch.core.GestureState;
   import org.gestouch.core.gestouch_internal;
   import ru.pragmatix.flox.gui.controls.§_-NY§;
   import ru.pragmatix.flox.social.controller.ISocialPaymentController;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-E2w§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.request.DeleteClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginClanBase;
   import ru.pragmatix.wormix.messages.clans.response.DeleteClanResponse;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanEnterAccount;
   import ru.pragmatix.wormix.messages.clans.response.LoginClanErrorResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.SteamTransactionCommitRequest;
   import ru.pragmatix.wormix.messages.server.DistributePointsResult;
   import ru.pragmatix.wormix.messages.server.GetDividendResult;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.rewards.§_-P2q§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.pvp.messages.client.PvpRetryCommandRequestClient;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.utils.§_-71v§;
   import ru.pragmatix.wormix.utils.§_-72u§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import ru.pragmatix.wormix.weapons.ammo.LightningBullet;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Image;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Align;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   use namespace bi_internal;
   use namespace gestouch_internal;
   
   public class SelectMapMenu extends §_-73y§
   {
      
      public static var §_-61s§:String = "MAP.";
      
      private static const §_-H1t§:String = "_preview";
      
      private static var §_-M18§:int = 1;
      
      private var §_-21a§:Button;
      
      private var §_-x2a§:Button;
      
      private var randomMapButton:Button;
      
      private var §_-Ok§:DisplayObjectContainer;
      
      private var §_-I3D§:Array;
      
      private var §_-O2J§:Image;
      
      private var §_-n1R§:int;
      
      private var §_-b2L§:§_-A29§;
      
      private var §_-S2K§:Boolean;
      
      private var §_-oR§:Boolean;
      
      private var §_-nR§:§_-D3a§;
      
      private var §_-ro§:BitmapData;
      
      private var mapImage:ImageLoader;
      
      private var §_-41d§:Dictionary;
      
      public function SelectMapMenu()
      {
         super(§_-YQ§.§_-qM§(§_-q2v§.§_-cO§,§_-q2v§.§_-h2y§));
         this.§_-Ok§ = getChildByName("map") as DisplayObjectContainer;
         loading = new §_-W1G§(this.§_-Ok§.getChildByName("loading") as DisplayObjectContainer);
         this.§_-Ok§.addChild(loading);
         §_-A1e§(§_-s2a§.SELECT_MAP_TITLE);
         this.randomMapButton = getChildByName("randomMapButton") as Button;
         this.randomMapButton.addEventListener(starling.events.Event.TRIGGERED,this.§_-k2M§);
         this.randomMapButton.label = §_-s2a§.§_-K3t§(§_-s2a§.RANDOM_MAP);
         this.§_-x2a§ = getChildByName("prev") as Button;
         this.§_-21a§ = getChildByName("next") as Button;
         this.§_-21a§.addEventListener(starling.events.Event.TRIGGERED,this.§_-525§);
         this.§_-x2a§.addEventListener(starling.events.Event.TRIGGERED,this.§_-b14§);
         this.§_-S2K§ = true;
         destroyOnRemove = true;
         this.§_-41d§ = new Dictionary();
         var _loc1_:DisplayObject = this.§_-Ok§.getChildByName("previewContainer");
         var _loc2_:Rectangle = _loc1_.getBounds(container);
         this.mapImage = new ImageLoader();
         this.mapImage.x = _loc2_.x;
         this.mapImage.y = _loc2_.y;
         Pool.putRectangle(_loc2_);
         §_-j2l§().addChild(this.mapImage);
         this.§_-oR§ = false;
      }
      
      override protected function onAddedToStage(param1:starling.events.Event) : void
      {
         var _loc2_:§_-81M§ = null;
         super.onAddedToStage(param1);
         this.§_-ro§ = new BitmapData(600,300);
         _loc2_ = §_-81M§.§_-A1W§;
         var _loc3_:Boolean = _loc2_ == §_-81M§.§_-p1U§ || _loc2_ == §_-81M§.§_-A1W§;
         this.setData(§_-X1j§.§_-H15§.§_-T1Z§(_loc2_,_loc3_));
      }
      
      private function §_-b14§(param1:starling.events.Event) : void
      {
         if(this.§_-n1R§ > 0)
         {
            --this.§_-n1R§;
         }
         else
         {
            this.§_-n1R§ = this.§_-I3D§.length - 1;
         }
         this.update();
      }
      
      private function §_-525§(param1:starling.events.Event) : void
      {
         if(this.§_-n1R§ < this.§_-I3D§.length - 1)
         {
            ++this.§_-n1R§;
         }
         else
         {
            this.§_-n1R§ = 0;
         }
         this.update();
      }
      
      private function update() : void
      {
         this.setMap(this.§_-I3D§[this.§_-n1R§]);
         if(!this.§_-S2K§)
         {
            this.§_-x2a§.visible = this.§_-n1R§ > 0;
            this.§_-21a§.visible = this.§_-n1R§ < this.§_-I3D§.length - 1;
         }
         else
         {
            this.§_-x2a§.visible = true;
            this.§_-21a§.visible = true;
         }
      }
      
      private function setData(param1:Array) : void
      {
         var _loc3_:§_-D3a§ = null;
         this.§_-I3D§ = param1;
         this.§_-n1R§ = 0;
         var _loc2_:int = int(param1.length);
         var _loc4_:int = 0;
         while(_loc4_ < _loc2_)
         {
            _loc3_ = param1[_loc4_];
            if(_loc3_.id == §_-M18§)
            {
               this.§_-n1R§ = _loc4_;
               break;
            }
            _loc4_++;
         }
         this.update();
      }
      
      private function setMap(param1:§_-D3a§) : void
      {
         this.mapImage.source = "";
         var _loc2_:ITextRenderer = getChildByName("mapName") as ITextRenderer;
         if(_loc2_)
         {
            _loc2_.text = §_-s2a§.§_-K3t§(param1 ? §_-61s§ + param1.name.toUpperCase() : §_-s2a§.RANDOM_MAP);
         }
         this.§_-nR§ = param1;
         Starling.juggler.delayCall(this.§_-e1R§,0.5,param1);
      }
      
      private function §_-e1R§(param1:§_-D3a§) : void
      {
         var _loc2_:§_-f2a§ = null;
         if(param1.id != this.§_-nR§.id || this.§_-41d§ == null)
         {
            return;
         }
         if(param1.isLoaded())
         {
            this.§_-D1L§(param1);
         }
         else if(this.§_-41d§[param1.id] == null)
         {
            _loc2_ = new §_-f2a§(param1);
            _loc2_.addEventListener(§_-x8§.SUCCESS,this.§_-M20§);
            _loc2_.load();
            this.§_-41d§[param1.id] = _loc2_;
         }
      }
      
      private function §_-M20§(param1:starling.events.Event = null) : void
      {
         var _loc2_:§_-D3a§ = (param1.currentTarget as §_-f2a§).getMapRecord();
         this.§_-41d§[_loc2_.id].destroy();
         delete this.§_-41d§[_loc2_.id];
         this.§_-D1L§(_loc2_);
      }
      
      private function §_-D1L§(param1:§_-D3a§) : void
      {
         var mapMC:MovieClip = null;
         var texture:Texture = null;
         var loadedMapRecord:§_-D3a§ = param1;
         if(loadedMapRecord.id == this.§_-nR§.id)
         {
            mapMC = §_-r5§.§_-tF§(loadedMapRecord.name + §_-H1t§) as MovieClip;
            mapMC.cacheAsBitmap = true;
            this.§_-ro§.lock();
            this.§_-ro§.draw(mapMC);
            this.§_-ro§.unlock();
            if(this.mapImage.source is Texture)
            {
               this.mapImage.source.dispose();
            }
            try
            {
               texture = Texture.fromBitmapData(this.§_-ro§,false,false,1,Context3DTextureFormat.BGRA,false,function():void
               {
                  if(loadedMapRecord.id == §_-nR§.id)
                  {
                     texture.root.onRestore = function():void
                     {
                        if(§_-ro§)
                        {
                           texture.root.uploadBitmapData(§_-ro§);
                        }
                     };
                     mapImage.source = texture;
                  }
                  else
                  {
                     texture.dispose();
                  }
               });
            }
            catch(e:Error)
            {
               return;
            }
         }
      }
      
      private function §_-k2M§(param1:starling.events.Event) : void
      {
         this.§_-oR§ = true;
         this.§_-b1H§();
      }
      
      override protected function §_-O2y§() : void
      {
         super.§_-O2y§();
         §_-12E§(§_-s2a§.BUTTON_REQUEST);
      }
      
      override protected function §_-b1H§() : void
      {
         var _loc1_:§_-D3a§ = null;
         if(this.§_-b1y§())
         {
            (okBtn as Button).isEnabled = false;
            this.randomMapButton.isEnabled = false;
            _loc1_ = this.§_-62W§;
            if(!this.§_-oR§)
            {
               §_-M18§ = _loc1_.id;
            }
            dispatchEventWith(§_-M4§.MENU_OK_ACTION,false,_loc1_);
            this.hide();
         }
      }
      
      override protected function §_-a6§(param1:starling.events.Event) : void
      {
         super.§_-a6§(param1);
         dispatchEventWith(§_-M4§.WINDOW_CANCEL);
      }
      
      override public function §_-X2O§() : void
      {
         §_-03a§(null);
      }
      
      private function §_-b1y§() : Boolean
      {
         if(this.§_-oR§)
         {
            return true;
         }
         if(this.§_-n1R§ < 0 || this.§_-n1R§ >= this.§_-I3D§.length)
         {
            return false;
         }
         return true;
      }
      
      private function get §_-62W§() : §_-D3a§
      {
         return this.§_-oR§ ? null : this.§_-I3D§[this.§_-n1R§];
      }
      
      override public function hide() : void
      {
         if(this.randomMapButton)
         {
            this.randomMapButton.removeEventListener(starling.events.Event.TRIGGERED,this.§_-k2M§);
         }
         if(this.§_-21a§)
         {
            this.§_-21a§.removeEventListener(starling.events.Event.TRIGGERED,this.§_-525§);
         }
         if(this.§_-x2a§)
         {
            this.§_-x2a§.removeEventListener(starling.events.Event.TRIGGERED,this.§_-b14§);
         }
         super.hide();
      }
      
      override public function dispose() : void
      {
         var _loc1_:§_-f2a§ = null;
         this.§_-21a§ = §_-c1S§.dispose(this.§_-21a§) as Button;
         this.§_-x2a§ = §_-c1S§.dispose(this.§_-x2a§) as Button;
         this.randomMapButton = §_-c1S§.dispose(this.randomMapButton) as Button;
         this.§_-Ok§ = §_-c1S§.dispose(this.§_-Ok§);
         this.§_-O2J§ = §_-c1S§.dispose(this.§_-O2J§) as Image;
         this.§_-ro§.dispose();
         this.§_-ro§ = null;
         for each(_loc1_ in this.§_-41d§)
         {
            _loc1_.removeEventListener(§_-x8§.SUCCESS,this.§_-M20§);
            _loc1_.destroy();
         }
         this.§_-41d§ = null;
         this.§_-I3D§ = null;
         this.§_-b2L§ = null;
         if(this.mapImage.source is Texture)
         {
            this.mapImage.source.dispose();
         }
         this.mapImage.removeFromParent(true);
         super.dispose();
      }
   }
}

