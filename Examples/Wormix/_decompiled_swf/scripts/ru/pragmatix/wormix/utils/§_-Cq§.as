package ru.pragmatix.wormix.utils
{
   import §_-62§.*;
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-CF§.§_-E19§;
   import §_-D3A§.§_-TZ§;
   import §_-H2g§.§_-di§;
   import §_-L2F§.§_-D2p§;
   import §_-W§.§_-z2Q§;
   import §_-Y1O§.*;
   import §_-Y1b§.*;
   import §_-YF§.§_-g2W§;
   import §_-b1F§.§_-G4§;
   import §_-bI§.*;
   import §_-d26§.§_-d1H§;
   import §_-k1u§.§_-2m§;
   import §_-k1u§.§_-H2M§;
   import §_-k1u§.§_-Oi§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-q26§.*;
   import §_-r1C§.§_-h2B§;
   import §_-wD§.*;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.distriqt.extension.inappbilling.events.ApplicationReceiptEvent;
   import com.distriqt.extension.inappbilling.events.PurchaseEvent;
   import com.hurlant.crypto.tls.*;
   import dragonBones.animation.WorldClock;
   import feathers.core.ITextRenderer;
   import flash.events.Event;
   import flash.geom.Point;
   import ru.pragmatix.flox.social.model.SocialUserData;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.request.DeleteClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.QuitClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginCreateClanRequest;
   import ru.pragmatix.wormix.messages.clans.request.login.LoginJoinClanRequest;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.messages.structures.SimpleProfileStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.§_-kQ§;
   import ru.pragmatix.wormix.model.items.craft.*;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.RopePhysicModel;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class §_-Cq§
   {
      
      public function §_-Cq§()
      {
         super();
      }
      
      public static function §_-v2G§(param1:int) : §_-IE§
      {
         var _loc2_:§_-IE§ = null;
         for each(_loc2_ in §_-Y2u§.§_-uO§())
         {
            if(_loc2_.getId() == param1)
            {
               return _loc2_;
            }
         }
         return null;
      }
      
      public static function §_-q27§(param1:int) : §_-IE§
      {
         var _loc2_:§_-IE§ = null;
         for each(_loc2_ in §_-Y2u§.§_-uO§())
         {
            if(_loc2_.upgradeId == param1)
            {
               return _loc2_;
            }
         }
         return null;
      }
      
      public static function §_-G38§(param1:int, param2:UserProfile) : void
      {
         var _loc3_:§_-O2H§ = §_-G1p§.§_-I4§(param1);
         param2.§_-pd§().§_-Um§(_loc3_,-1,_loc3_.getMaxShots(),_loc3_.getMaxLevel());
      }
      
      public static function §_-Y22§(param1:int, param2:int) : void
      {
         var _loc3_:§_-Oi§ = Application.userProfile.§_-E3H§().§_-d0§(param1);
         if(_loc3_ != null)
         {
            _loc3_.setCount(_loc3_.getCount() + param2);
         }
         else
         {
            Application.userProfile.§_-E3H§().§_-Um§(§_-Y2u§.§_-k14§(param1),param2);
         }
      }
      
      public static function §_-n1v§(param1:int, param2:int) : void
      {
         var _loc3_:§_-H2M§ = Application.userProfile.§_-E3H§();
         var _loc4_:§_-Oi§ = _loc3_.§_-d0§(param1);
         if(_loc4_ != null)
         {
            _loc4_.setCount(Math.max(0,_loc4_.getCount() - param2));
            if(_loc4_.getCount() <= 0)
            {
               _loc3_.removeItem(_loc4_.getId(),-1);
            }
         }
      }
      
      public static function §_-23c§(param1:§_-W1r§) : Vector.<§_-O2H§>
      {
         var _loc3_:§_-IE§ = null;
         var _loc4_:§_-o6§ = null;
         var _loc2_:Vector.<§_-O2H§> = new Vector.<§_-O2H§>();
         for each(_loc3_ in §_-Y2u§.§_-uO§())
         {
            for each(_loc4_ in _loc3_.reagents)
            {
               if(param1.getId() == _loc4_.id)
               {
                  _loc2_[_loc2_.length] = §_-G1p§.§_-I4§(_loc3_.upgradeId);
                  break;
               }
            }
         }
         return _loc2_;
      }
      
      public static function §_-327§(param1:int) : Boolean
      {
         return §_-Y2u§.§_-8Q§(param1) != null;
      }
      
      public static function §_-Z1B§(param1:§_-O2H§) : Vector.<§_-O2H§>
      {
         var _loc2_:Vector.<§_-O2H§> = null;
         if(param1.§_-o2E§)
         {
            _loc2_ = new Vector.<§_-O2H§>();
            §_-Ug§(param1.§_-o2E§,_loc2_);
            return _loc2_;
         }
         return null;
      }
      
      private static function §_-Ug§(param1:§_-6S§, param2:Vector.<§_-O2H§>) : void
      {
         var _loc3_:§_-6S§ = null;
         for each(_loc3_ in param1.next)
         {
            param2.push(_loc3_);
            §_-Ug§(_loc3_,param2);
         }
      }
      
      public static function §_-81Q§() : int
      {
         var _loc3_:§_-IE§ = null;
         var _loc4_:§_-Oi§ = null;
         var _loc1_:§_-H2M§ = Application.userProfile.§_-h1v§();
         var _loc2_:int = 0;
         for each(_loc3_ in §_-Y2u§.§_-uO§())
         {
            _loc4_ = _loc1_.§_-d0§(_loc3_.getId());
            if((Boolean(_loc4_)) && _loc4_.getCount() == 1)
            {
               _loc2_++;
            }
         }
         return _loc2_;
      }
      
      public static function §_-33I§(param1:int, param2:UserProfile) : Boolean
      {
         var _loc3_:§_-O2H§ = §_-G1p§.§_-I4§(param1);
         if(!_loc3_)
         {
            return false;
         }
         var _loc4_:§_-2m§ = param2.§_-pd§();
         var _loc5_:int = int(_loc3_.getRefId());
         var _loc6_:§_-Oi§ = _loc4_.§_-d0§(_loc5_ > 0 ? _loc5_ : param1);
         if(_loc6_)
         {
            return _loc6_.§_-n1u§();
         }
         _loc6_ = §_-J2a§(param1,_loc4_);
         return (Boolean(_loc6_)) && _loc6_.§_-n1u§();
      }
      
      private static function §_-B1w§(param1:§_-kQ§, param2:§_-H2M§) : §_-Oi§
      {
         var _loc3_:int = 0;
         var _loc4_:§_-Oi§ = null;
         var _loc5_:Boolean = false;
         if(param1)
         {
            for each(_loc3_ in param1.§_-I2r§)
            {
               _loc4_ = param2.§_-d0§(_loc3_);
               _loc5_ = (Boolean(_loc4_)) && (_loc4_.isInfinite() || !_loc4_.isEmpty());
               if(_loc5_)
               {
                  return _loc4_;
               }
            }
         }
         return null;
      }
      
      public static function §_-J2a§(param1:int, param2:§_-H2M§) : §_-Oi§
      {
         var _loc4_:§_-kQ§ = null;
         var _loc3_:§_-Oi§ = param2.§_-d0§(param1);
         if(_loc3_ == null)
         {
            _loc4_ = §_-Y2u§.§_-r1O§(param1);
            _loc3_ = §_-B1w§(_loc4_,param2);
         }
         return _loc3_;
      }
      
      public static function §_-32u§(param1:int) : int
      {
         var _loc2_:§_-kQ§ = §_-Y2u§.§_-r1O§(param1);
         return _loc2_?.§_-z1X§;
      }
      
      public static function §_-sj§(param1:uint) : uint
      {
         var _loc3_:§_-O2H§ = null;
         var _loc2_:§_-Oi§ = §_-J2a§(param1,Application.userProfile.§_-pd§());
         if(Boolean(_loc2_) && _loc2_.isInfinite())
         {
            _loc3_ = §_-G1p§.§_-I4§(§_-32u§(_loc2_.getId()));
            if(_loc3_.isComplex() && _loc2_.getId() == _loc3_.getId())
            {
               return _loc2_.getLevel();
            }
            return _loc3_.getMaxLevel();
         }
         return 0;
      }
      
      public static function §_-hr§(param1:§_-O2H§) : Boolean
      {
         var _loc2_:uint = 0;
         var _loc3_:uint = 0;
         if(param1)
         {
            _loc2_ = §_-sj§(param1.getId());
            _loc3_ = param1.getMaxLevel();
            return param1.isComplex() && _loc2_ > 0 && _loc2_ < _loc3_;
         }
         return false;
      }
   }
}

