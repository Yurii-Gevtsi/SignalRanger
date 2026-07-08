package ru.pragmatix.wormix.model.bonus
{
   import §_-12W§.*;
   import §_-21p§.§_-4w§;
   import §_-21p§.§_-N1H§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-zF§;
   import §_-A2U§.§_-A23§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.Server;
   import §_-B2z§.§_-63i§;
   import §_-B2z§.§_-c1G§;
   import §_-Cl§.§_-L2z§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-fH§;
   import §_-G2H§.*;
   import §_-H3§.*;
   import §_-Jd§.BonusStat;
   import §_-Jd§.§_-11K§;
   import §_-Jd§.§_-g2F§;
   import §_-L1H§.§_-C3e§;
   import §_-OJ§.§_-L3B§;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-O2i§;
   import §_-V1H§.§_-m1d§;
   import §_-Y1O§.§_-V23§;
   import §_-YF§.§_-q2v§;
   import §_-hv§.*;
   import §_-j22§.§_-5g§;
   import §_-l1K§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-m2s§.§_-R11§;
   import §_-r1C§.§_-h2B§;
   import §_-s20§.§_-6a§;
   import §_-z1g§.§_-y1S§;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.distriqt.extension.inappbilling.events.PurchaseEvent;
   import feathers.controls.Button;
   import feathers.core.ToggleGroup;
   import feathers.layout.AnchorLayoutData;
   import feathers.skins.ImageSkin;
   import flash.events.Event;
   import flash.events.IEventDispatcher;
   import flash.geom.Point;
   import flash.net.SharedObject;
   import flash.system.ApplicationDomain;
   import flash.system.LoaderContext;
   import flash.system.SecurityDomain;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.LowerRankInClanRequest;
   import ru.pragmatix.wormix.messages.clans.response.LowerRankInClanResponse;
   import ru.pragmatix.wormix.messages.clans.structures.ClanMemberTO;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.GetFriendsForBoss;
   import ru.pragmatix.wormix.messages.client.ReorderGroup;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-V1L§;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattle;
   import ru.pragmatix.wormix.scene.interaction.SceneInteractionData;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-6A§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.textures.Texture;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class §_-z17§
   {
      
      private var owner:§_-01J§;
      
      private var §_-r1b§:Vector.<§_-N1H§>;
      
      private var bonuses:Vector.<§_-11K§>;
      
      public function §_-z17§(param1:§_-01J§)
      {
         super();
         this.owner = param1;
         this.§_-r1b§ = new Vector.<§_-N1H§>();
         this.bonuses = new Vector.<§_-11K§>();
         this.§_-U1Z§(param1.record.§_-P2e§().§_-V1v§());
      }
      
      public function §_-U1Z§(param1:§_-N1H§) : void
      {
         var _loc2_:§_-N1H§ = null;
         var _loc3_:§_-4w§ = null;
         var _loc4_:§_-11K§ = null;
         var _loc5_:Class = null;
         var _loc6_:§_-11K§ = null;
         for each(_loc2_ in this.§_-r1b§)
         {
            if(param1.§_-C3p§() == _loc2_.§_-C3p§())
            {
               this.§_-Y2t§(_loc2_);
               break;
            }
         }
         this.§_-r1b§[this.§_-r1b§.length] = param1;
         for each(_loc3_ in param1.§_-b2I§())
         {
            var _loc9_:int = 0;
            var _loc10_:* = this.bonuses;
            loop2:
            while(true)
            {
               for each(_loc4_ in _loc10_)
               {
                  if(_loc3_.getType() == _loc4_.getType())
                  {
                     _loc4_.add(_loc3_);
                     break loop2;
                  }
               }
               _loc5_ = _loc3_.getType().getClass();
               _loc6_ = new _loc5_();
               _loc6_.init(this.owner,_loc3_);
               this.bonuses[this.bonuses.length] = _loc6_;
               break;
            }
         }
      }
      
      public function §_-Y2t§(param1:§_-N1H§) : void
      {
         var _loc2_:uint = 0;
         var _loc3_:uint = 0;
         var _loc4_:§_-11K§ = null;
         var _loc5_:§_-4w§ = null;
         for each(_loc5_ in param1.§_-b2I§())
         {
            _loc2_ = this.bonuses.length;
            _loc3_ = 0;
            while(_loc3_ < _loc2_)
            {
               _loc4_ = this.bonuses[_loc3_];
               if(_loc5_.getType() == _loc4_.getType())
               {
                  _loc4_.remove(_loc5_);
                  if(_loc4_.getCount() <= 0)
                  {
                     this.bonuses.splice(_loc3_,1);
                  }
                  break;
               }
               _loc3_++;
            }
         }
         _loc2_ = this.§_-r1b§.length;
         _loc3_ = 0;
         while(_loc3_ < _loc2_)
         {
            if(param1 == this.§_-r1b§[_loc3_])
            {
               this.§_-r1b§.splice(_loc3_,1);
               break;
            }
            _loc3_++;
         }
      }
      
      public function getStatValue(param1:§_-x2M§, param2:int = 2147483647) : int
      {
         var _loc3_:§_-11K§ = null;
         for each(_loc3_ in this.bonuses)
         {
            if(param1 == _loc3_.getType() && _loc3_.getName() == BonusStat.NAME)
            {
               return Math.min(BonusStat(_loc3_).getValue(),param2);
            }
         }
         return 0;
      }
      
      public function §_-yQ§(param1:§_-x2M§) : Boolean
      {
         var _loc2_:§_-11K§ = null;
         for each(_loc2_ in this.bonuses)
         {
            if(param1 == _loc2_.getType())
            {
               return true;
            }
         }
         return false;
      }
      
      private function §_-fJ§() : Vector.<§_-g2F§>
      {
         var _loc2_:§_-11K§ = null;
         var _loc3_:§_-g2F§ = null;
         var _loc1_:Vector.<§_-g2F§> = new Vector.<§_-g2F§>(0);
         for each(_loc2_ in this.bonuses)
         {
            _loc3_ = _loc2_ as §_-g2F§;
            if(_loc3_)
            {
               _loc1_.push(_loc3_);
            }
         }
         return _loc1_;
      }
      
      public function §_-pi§(param1:§_-V23§) : void
      {
         var _loc3_:§_-01J§ = null;
         var _loc4_:§_-g2F§ = null;
         var _loc2_:Vector.<§_-g2F§> = this.§_-fJ§();
         for each(_loc3_ in param1.getUnits())
         {
            if(_loc3_ != this.owner)
            {
               for each(_loc4_ in _loc2_)
               {
                  _loc4_.applyOnUnit(_loc3_);
               }
               for each(_loc4_ in _loc3_.bonuses.§_-fJ§())
               {
                  _loc4_.applyOnUnit(this.owner);
               }
            }
         }
      }
      
      public function §_-H2G§() : Vector.<§_-N1H§>
      {
         return this.§_-r1b§;
      }
      
      public function §_-Q7§(param1:String) : §_-N1H§
      {
         var _loc2_:§_-N1H§ = null;
         for each(_loc2_ in this.§_-r1b§)
         {
            if(_loc2_.§_-C3p§() == param1)
            {
               return _loc2_;
            }
         }
         return null;
      }
   }
}

