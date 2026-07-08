package ru.pragmatix.wormix.game.ai
{
   import §_-21p§.§_-4w§;
   import §_-62§.§_-G1p§;
   import §_-9e§.*;
   import §_-CF§.§_-x2t§;
   import §_-H2g§.§_-di§;
   import §_-Tm§.§_-RF§;
   import §_-U1i§.§_-J1§;
   import §_-Z1K§.§_-q24§;
   import §_-l2r§.§_-K1t§;
   import §_-q26§.§_-Z25§;
   import com.distriqt.extension.inappbilling.InAppBilling;
   import com.distriqt.extension.inappbilling.Purchase;
   import config.§_-vR§;
   import dragonBones.animation.WorldClock;
   import flash.geom.Point;
   import ru.pragmatix.flox.logger.§_-93v§;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.mobile.*;
   import ru.pragmatix.wormix.messages.clans.request.TopClansRequest;
   import ru.pragmatix.wormix.messages.clans.response.TopClansResponse;
   import ru.pragmatix.wormix.messages.server.VerifyPaymentResult;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class §_-A1l§
   {
      
      private var §_-C2§:Vector.<§_-de§>;
      
      public function §_-A1l§(param1:Vector.<§_-de§> = null, param2:uint = 4294967295, param3:Boolean = true)
      {
         var _loc5_:§_-de§ = null;
         this.§_-C2§ = new Vector.<§_-de§>();
         super();
         var _loc4_:Vector.<§_-de§> = new Vector.<§_-de§>(0);
         if(param3)
         {
            _loc4_ = §_-F24§();
         }
         if(param1)
         {
            _loc4_ = _loc4_.concat(param1);
         }
         for each(_loc5_ in _loc4_)
         {
            this.§_-t1j§(_loc5_,param2);
         }
      }
      
      private static function §_-F24§() : Vector.<§_-de§>
      {
         var _temp_13:* = new <§_-de§>[new §_-de§(§_-q24§.§_-w1k§,0.5,-1,8,15),new §_-de§(§_-q24§.BAZOOKA,1,-1,1,9),new §_-de§(§_-q24§.§_-d25§,1,-1,10,19),new §_-de§(§_-q24§.§_-s26§,1,-1,20),new §_-de§(§_-q24§.GRENADE,1,-1,1,7),new §_-de§(§_-q24§.§_-L3D§,1,-1,8,18),new §_-de§(§_-q24§.§_-O1R§,1,-1,19),new §_-de§(§_-q24§.RIFLE,1,-1,1,12),new §_-de§(§_-q24§.RIFLE_UPG_1,1,-1,13),new §_-de§(§_-q24§.§_-B3l§,0.2,2,18),new §_-de§(§_-q24§.BUNKER_BUSTER,0.15,1,21),new §_-de§(§_-q24§.METEOR,0.08,1,24),new §_-de§(§_-q24§.§_-c1p§,0.3,1,30),new §_-de§(§_-q24§.TELEPORT,1,1,30)];
         return new <§_-de§>[new §_-de§(§_-q24§.§_-w1k§,0.5,-1,8,15),new §_-de§(§_-q24§.BAZOOKA,1,-1,1,9),new §_-de§(§_-q24§.§_-d25§,1,-1,10,19),new §_-de§(§_-q24§.§_-s26§,1,-1,20),new §_-de§(§_-q24§.GRENADE,1,-1,1,7),new §_-de§(§_-q24§.§_-L3D§,1,-1,8,18),new §_-de§(§_-q24§.§_-O1R§,1,-1,19),new §_-de§(§_-q24§.RIFLE,1,-1,1,12),new §_-de§(§_-q24§.RIFLE_UPG_1,1,-1,13),new §_-de§(§_-q24§.§_-B3l§,0.2,2,18),new §_-de§(§_-q24§.BUNKER_BUSTER,0.15,1,21),new §_-de§(§_-q24§.METEOR,0.08,1,24),new §_-de§(§_-q24§.§_-c1p§,0.3,1,30),new §_-de§(§_-q24§.TELEPORT,1,1,30)];
      }
      
      public function §_-E2C§(param1:uint) : void
      {
         var _loc3_:§_-de§ = null;
         var _loc2_:Vector.<§_-de§> = new Vector.<§_-de§>();
         _loc2_ = _loc2_.concat(this.§_-C2§);
         this.§_-C2§.length = 0;
         for each(_loc3_ in _loc2_)
         {
            this.§_-t1j§(_loc3_,param1);
         }
      }
      
      public function §_-O2u§(param1:int) : Vector.<int>
      {
         var _loc3_:§_-de§ = null;
         var _loc2_:Vector.<int> = new Vector.<int>();
         for each(_loc3_ in this.§_-C2§)
         {
            if(_loc3_.maxShots == -1 || _loc3_.shots > 0)
            {
               _loc2_[_loc2_.length] = _loc3_.weaponId;
            }
         }
         return _loc2_;
      }
      
      public function §_-A1G§(param1:uint) : void
      {
         var _loc2_:§_-de§ = null;
         for each(_loc2_ in this.§_-C2§)
         {
            if(_loc2_.weaponId == param1 && _loc2_.maxShots != -1)
            {
               --_loc2_.shots;
               return;
            }
         }
      }
      
      public function §_-R29§(param1:Array) : void
      {
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         for each(_loc2_ in param1)
         {
            _loc3_ = 0;
            _loc4_ = int(this.§_-C2§.length);
            while(_loc3_ < _loc4_)
            {
               if(this.§_-C2§[_loc3_].weaponId == _loc2_)
               {
                  this.§_-C2§.splice(_loc3_,1);
                  break;
               }
               _loc3_++;
            }
         }
      }
      
      public function §_-B2§(param1:uint) : Number
      {
         var _loc2_:§_-de§ = null;
         for each(_loc2_ in this.§_-C2§)
         {
            if(_loc2_.weaponId == param1)
            {
               return _loc2_.§_-K1W§ ? _loc2_.§_-K1W§ : 1;
            }
         }
         return 1;
      }
      
      public function §_-t1j§(param1:§_-de§, param2:uint = 4294967295) : void
      {
         var _loc3_:§_-de§ = null;
         if(param2 >= param1.minLevel && param2 <= param1.maxLevel)
         {
            for each(_loc3_ in this.§_-C2§)
            {
               if(_loc3_.weaponId == param1.weaponId)
               {
                  if(param1.maxShots == -1 || _loc3_.maxShots == -1)
                  {
                     _loc3_.maxShots = -1;
                     _loc3_.shots = 0;
                  }
                  else
                  {
                     _loc3_.maxShots += param1.maxShots;
                     _loc3_.shots = _loc3_.maxShots;
                  }
                  param1.§_-K1W§ = Math.max(param1.§_-K1W§,_loc3_.§_-K1W§);
                  return;
               }
            }
            this.§_-C2§[this.§_-C2§.length] = param1;
         }
      }
   }
}

