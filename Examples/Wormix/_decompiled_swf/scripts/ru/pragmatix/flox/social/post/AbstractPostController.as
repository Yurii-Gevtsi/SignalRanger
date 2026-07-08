package ru.pragmatix.flox.social.post
{
   import §_-21A§.*;
   import §_-31W§.AwardCell;
   import §_-31W§.AwardListTab;
   import §_-43V§.Random;
   import §_-5Y§.§_-h2c§;
   import §_-62§.§_-F1D§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-W1r§;
   import §_-82a§.*;
   import §_-82l§.§_-A2Y§;
   import §_-9§.§_-52A§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-F1P§;
   import §_-B1y§.§_-bs§;
   import §_-B1y§.§_-o2k§;
   import §_-D3A§.§_-TZ§;
   import §_-H16§.*;
   import §_-L1H§.§_-C3e§;
   import §_-L1H§.§_-b2K§;
   import §_-RE§.§_-r2q§;
   import §_-SP§.§_-M4§;
   import §_-TF§.§_-Ud§;
   import §_-TF§.§_-q1j§;
   import §_-TF§.§_-y2n§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-b1k§;
   import §_-Y1O§.§_-yA§;
   import §_-YF§.§_-q2v§;
   import §_-b1F§.§_-G4§;
   import §_-d26§.§_-d1H§;
   import §_-e2r§.*;
   import §_-fo§.*;
   import §_-j1w§.§_-B2i§;
   import §_-j1w§.§_-L1x§;
   import §_-k1u§.§_-2m§;
   import §_-k1u§.§_-52D§;
   import §_-k1u§.§_-F3H§;
   import §_-k1u§.§_-H2M§;
   import §_-k1u§.§_-Oi§;
   import §_-k1u§.§_-Vr§;
   import §_-k1u§.§_-by§;
   import §_-l1K§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-lh§.§_-z2A§;
   import §_-m2s§.§_-R11§;
   import §_-o14§.§_-Dd§;
   import §_-y1s§.§_-P9§;
   import §_-z1g§.*;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.ImageLoader;
   import com.greensock.loading.LoaderMax;
   import com.greensock.loading.core.LoaderCore;
   import com.hurlant.math.BigInteger;
   import com.hurlant.math.bi_internal;
   import config.§_-V2w§;
   import feathers.data.IListCollection;
   import flash.display.DisplayObject;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.flox.logger.§_-11S§;
   import ru.pragmatix.flox.social.controller.IAuthorizeSocialController;
   import ru.pragmatix.flox.social.controller.ISocialController;
   import ru.pragmatix.flox.social.model.IUserProfile;
   import ru.pragmatix.flox.social.response.SocialResponse;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.model.§_-4V§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.model.user.*;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.GameObjectUtils;
   import ru.pragmatix.wormix.physics.HitTest;
   import ru.pragmatix.wormix.physics.model.UnitPhysicModel;
   import ru.pragmatix.wormix.social.*;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.BurningOil;
   import ru.pragmatix.wormix.weapons.ammo.ammoFactory.AmmosFactory;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.MathUtil;
   import starling.utils.Pool;
   
   use namespace bi_internal;
   
   public class AbstractPostController
   {
      
      protected var activityPoster:IActivityPoster;
      
      protected var wallPoster:IWallPoster;
      
      protected var notifier:INotifier;
      
      protected var iconsDir:String;
      
      protected var log:§_-11S§ = new §_-11S§(AbstractPostController);
      
      protected var context:§_-o2k§;
      
      public function AbstractPostController(param1:§_-o2k§, param2:ISocialController, param3:String)
      {
         super();
         this.iconsDir = param3;
         this.context = param1;
         this.activityPoster = param2 as IActivityPoster;
         this.wallPoster = param2 as IWallPoster;
         this.notifier = param2 as INotifier;
      }
      
      protected function notify(param1:IUserProfile, param2:String, param3:Object = null, param4:String = null, param5:Function = null) : void
      {
         if(this.notifier)
         {
            this.notifier.notify(param1,§_-F1P§.format(param2,param3),"",param5,this.iconsDir + param4);
         }
      }
      
      protected function postActivity(param1:String, param2:Object = null, param3:String = null, param4:String = null, param5:String = null) : void
      {
         if(this.activityPoster)
         {
            this.activityPoster.activity(§_-F1P§.format(param1,param2),this.iconsDir + param3,param4,param5);
         }
      }
      
      protected function postOnWall(param1:String, param2:Object = null, param3:String = null, param4:Function = null) : void
      {
         if(this.wallPoster)
         {
            this.wallPoster.wallPost(§_-F1P§.format(param1,param2),"",this.iconsDir + param3,param4);
         }
      }
      
      protected function loadAndPostOnWall(param1:String, param2:String, param3:Object = null, param4:String = null) : void
      {
         var _loc5_:String = null;
         var _loc6_:String = null;
         var _loc7_:Function = null;
         var _loc8_:Object = null;
         var _loc9_:Object = null;
         var _loc10_:LoaderMax = null;
         if(this.wallPoster)
         {
            _loc5_ = this.context.§_-K3U§() + this.iconsDir + param4;
            _loc6_ = this.context.§_-C1Y§() + this.iconsDir + param4;
            _loc7_ = §_-bs§.§_-d1Z§(this.completeHandler,this,[§_-F1P§.format(param2,param3),param1,param4]);
            _loc8_ = {
               "name":"mainLoad",
               "onProgress":this.progressHandler,
               "onComplete":_loc7_,
               "onError":this.errorHandler
            };
            _loc9_ = {
               "name":param4,
               "container":this,
               "alpha":0,
               "scaleMode":"proportionalInside",
               "alternateURL":_loc6_
            };
            _loc10_ = new LoaderMax(_loc8_);
            _loc10_.append(new ImageLoader(_loc5_,_loc9_));
            _loc10_.load();
         }
      }
      
      private function completeHandler(param1:String, param2:String, param3:String) : void
      {
         var _loc4_:flash.display.DisplayObject = LoaderMax.getContent(param3).rawContent;
         this.wallPost(param1,param2,_loc4_);
         LoaderCore(LoaderMax.getLoader(param3)).dispose(true);
      }
      
      private function progressHandler(param1:LoaderEvent) : void
      {
      }
      
      private function errorHandler(param1:LoaderEvent) : void
      {
         this.log.error("error occured with " + param1.target + ": " + param1.text);
      }
      
      private function wallPost(param1:String, param2:String, param3:flash.display.DisplayObject) : void
      {
         this.wallPoster.wallPostLoadedImg(param1,param2,param3);
      }
   }
}

