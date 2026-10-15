package ru.pragmatix.wormix.social
{
   import §_-62§.§_-W1r§;
   import §_-A2U§.§_-A23§;
   import §_-DJ§.§_-fH§;
   import §_-F2Q§.GestureEvent;
   import §_-H2g§.§_-di§;
   import §_-P2i§.§_-b7§;
   import §_-SP§.§_-M4§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-b1k§;
   import §_-Y1O§.*;
   import §_-d26§.§_-d1H§;
   import §_-l1K§.*;
   import §_-rM§.§_-61j§;
   import §_-u1P§.§_-Po§;
   import §_-z1g§.*;
   import feathers.data.ListCollection;
   import flash.display.BitmapData;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import org.gestouch.core.GestureState;
   import ru.pragmatix.flox.social.SocialAppContext;
   import ru.pragmatix.flox.social.controller.IAuthorizeSocialController;
   import ru.pragmatix.flox.social.controller.ISocialController;
   import ru.pragmatix.flox.social.controller.SocialControllerFactory;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.flox.social.controller.mobile.BaseMobileSocialController;
   import ru.pragmatix.flox.social.response.SocialResponse;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.server.RepeatBossBattleAwardResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.GenericAwardStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.scene.tile.AtlasTileData;
   import ru.pragmatix.wormix.scene.tile.TileType;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.weapons.*;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Stage;
   import starling.events.Event;
   import starling.rendering.Painter;
   import starling.utils.AssetManager;
   import starling.utils.Pool;
   
   public class §_-L23§ implements §_-11f§
   {
      
      private var context:SocialAppContext;
      
      private var defaultType:SocialType;
      
      private var socialControllers:Dictionary;
      
      private var socialControllersCount:int;
      
      private var activeSocialController:BaseMobileSocialController;
      
      public function §_-L23§(param1:SocialAppContext, param2:Array, param3:SocialType)
      {
         var _loc4_:ISocialController = null;
         var _loc5_:SocialType = null;
         super();
         this.context = param1;
         this.defaultType = param3;
         this.socialControllers = new Dictionary();
         for each(_loc5_ in param2)
         {
            param1.setSocialType(_loc5_);
            _loc4_ = SocialControllerFactory.create(param1);
            if(_loc4_)
            {
               this.socialControllers[_loc5_.name] = _loc4_;
               ++this.socialControllersCount;
            }
         }
         this.activeSocialController = this.socialControllers[param3.name];
      }
      
      public function init(param1:Function) : void
      {
         var currSocialControllerCount:int = 0;
         var socialController:ISocialController = null;
         var callback:Function = param1;
         currSocialControllerCount = this.socialControllersCount;
         var currCount:int = 0;
         var callbackFunction:Function = function(param1:SocialResponse):void
         {
            if(!param1.success)
            {
               IAuthorizeSocialController(socialControllers[param1.opType]).dispose();
               delete socialControllers[param1.opType];
               --socialControllersCount;
            }
            if(++currCount == currSocialControllerCount)
            {
               findLoggedInSocialController();
               callback();
            }
         };
         for each(socialController in this.socialControllers)
         {
            socialController.init(callbackFunction);
         }
      }
      
      private function findLoggedInSocialController() : void
      {
         var _loc1_:BaseMobileSocialController = null;
         for each(_loc1_ in this.socialControllers)
         {
            if(_loc1_.isLoggedIn())
            {
               this.activeSocialController = _loc1_;
               return;
            }
         }
      }
      
      public function getActive() : BaseMobileSocialController
      {
         return this.activeSocialController;
      }
      
      public function §_-Nb§() : Array
      {
         var _loc2_:ISocialController = null;
         var _loc1_:Array = [];
         for each(_loc2_ in this.socialControllers)
         {
            if(_loc2_)
            {
               _loc1_.push(_loc2_.getType());
            }
         }
         return _loc1_;
      }
      
      public function §_-61b§(param1:SocialType) : BaseMobileSocialController
      {
         return this.socialControllers[param1.name];
      }
      
      public function change(param1:SocialType, param2:Function) : void
      {
         var socialType:SocialType = param1;
         var callback:Function = param2;
         if(this.activeSocialController)
         {
            if(!this.activeSocialController.getType().equals(socialType))
            {
               if(this.activeSocialController.isLoggedIn())
               {
                  this.activeSocialController.logout();
               }
            }
            else
            {
               this.processResponse(new SocialResponse("login",socialType.name,true),callback);
            }
         }
         if(this.socialControllers[socialType.name])
         {
            IAuthorizeSocialController(this.socialControllers[socialType.name]).login(function(param1:SocialResponse):void
            {
               if(param1.success)
               {
                  activeSocialController = socialControllers[socialType.name];
               }
               else
               {
                  activeSocialController = socialControllers[defaultType.name];
               }
               processResponse(param1,callback);
            });
         }
         else
         {
            this.processResponse(new SocialResponse("login",socialType.name,false),callback);
         }
      }
      
      private function processResponse(param1:SocialResponse, param2:Function) : void
      {
         if(param1.success == false && IAuthorizeSocialController(this.socialControllers[param1.opType]).isLoggedIn())
         {
            IAuthorizeSocialController(this.socialControllers[param1.opType]).logout();
         }
         param2(param1);
      }
   }
}

