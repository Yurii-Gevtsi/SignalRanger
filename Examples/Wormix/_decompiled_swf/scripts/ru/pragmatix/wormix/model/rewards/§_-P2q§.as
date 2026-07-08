package ru.pragmatix.wormix.model.rewards
{
   import §_-79§.§_-ps§;
   import §_-B1y§.§_-83E§;
   import §_-CF§.§_-E19§;
   import §_-CR§.§_-72p§;
   import §_-G2H§.*;
   import §_-K35§.§_-99§;
   import §_-TF§.§_-q1j§;
   import §_-Vg§.*;
   import §_-jF§.ClanMemberItemRenderer;
   import §_-jF§.§_-U17§;
   import §_-k1u§.§_-2m§;
   import §_-k2s§.§_-Go§;
   import §_-k2s§.§_-b0§;
   import §_-k2s§.§_-e1U§;
   import §_-k2s§.§_-e1k§;
   import §_-k2s§.§_-fG§;
   import §_-k2s§.§_-iI§;
   import §_-k2s§.§_-uU§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-d21§;
   import §_-w2i§.§_-Ia§;
   import §_-z1g§.§_-lp§;
   import com.distriqt.extension.inappbilling.Purchase;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ArrayCollection;
   import feathers.layout.AnchorLayout;
   import feathers.layout.Direction;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.TiledColumnsLayout;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateButtonsStyles;
   import flash.events.IEventDispatcher;
   import flash.net.SharedObject;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.gui.§_-eP§;
   import ru.pragmatix.wormix.messages.client.SteamProductsInfoRequest;
   import ru.pragmatix.wormix.messages.server.SteamProductsInfoResponse;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-Cq§;
   import starling.events.Event;
   
   public class §_-P2q§
   {
      
      private static const §_-e27§:uint = 10;
      
      private var §_-93H§:Array;
      
      public function §_-P2q§(param1:Array)
      {
         var _loc2_:uint = 0;
         super();
         this.§_-93H§ = [];
         if(Boolean(param1) && param1.length != 0)
         {
            _loc2_ = 0;
            while(_loc2_ < param1.length)
            {
               if(param1[_loc2_] is §_-Go§)
               {
                  this.§_-93H§.push(param1[_loc2_]);
               }
               _loc2_++;
            }
         }
      }
      
      public function §_-p25§(param1:Boolean = false) : uint
      {
         var _loc3_:§_-Go§ = null;
         var _loc2_:uint = 0;
         for each(_loc3_ in this.§_-93H§)
         {
            if(_loc3_ is §_-uU§)
            {
               _loc2_ += _loc3_.getCount();
            }
         }
         if(param1 && Application.userProfile.getLevel() == §_-lp§.getMaxLevel())
         {
            if(!_loc2_)
            {
            }
            return 0;
         }
         return _loc2_;
      }
      
      private function §_-y12§(param1:§_-E19§) : uint
      {
         var _loc3_:§_-Go§ = null;
         var _loc2_:uint = 0;
         for each(_loc3_ in this.§_-93H§)
         {
            if(_loc3_ is §_-iI§ && (_loc3_ as §_-iI§).§_-7i§() == param1)
            {
               _loc2_ += _loc3_.getCount();
            }
         }
         return _loc2_;
      }
      
      public function §_-51e§() : uint
      {
         return this.§_-y12§(§_-E19§.§_-i2d§);
      }
      
      public function §_-o1N§() : uint
      {
         return this.§_-y12§(§_-E19§.§_-51D§);
      }
      
      public function §_-S1n§() : Vector.<§_-b0§>
      {
         var _loc4_:§_-Go§ = null;
         var _loc1_:UserProfile = Application.userProfile;
         var _loc2_:§_-2m§ = _loc1_.§_-pd§();
         var _loc3_:Vector.<§_-b0§> = new Vector.<§_-b0§>(0);
         for each(_loc4_ in this.§_-93H§)
         {
            if(_loc4_ is §_-b0§)
            {
               if(!§_-Cq§.§_-33I§(§_-b0§(_loc4_).getId(),_loc1_) && _loc2_.§_-d3§(§_-b0§(_loc4_).getId()) < §_-e27§)
               {
                  _loc3_.push(§_-b0§(_loc4_));
               }
            }
         }
         return _loc3_;
      }
      
      public function §_-53R§() : Vector.<§_-b0§>
      {
         var _loc2_:§_-Go§ = null;
         var _loc1_:Vector.<§_-b0§> = new Vector.<§_-b0§>(0);
         for each(_loc2_ in this.§_-93H§)
         {
            if(_loc2_ is §_-b0§)
            {
               _loc1_.push(§_-b0§(_loc2_));
            }
         }
         return _loc1_;
      }
      
      public function §_-53L§() : Vector.<§_-e1U§>
      {
         var _loc2_:§_-Go§ = null;
         var _loc1_:Vector.<§_-e1U§> = new Vector.<§_-e1U§>(0);
         for each(_loc2_ in this.§_-93H§)
         {
            if(_loc2_ is §_-e1U§)
            {
               _loc1_.push(§_-e1U§(_loc2_));
            }
         }
         return _loc1_;
      }
      
      public function §_-h1o§() : Vector.<§_-e1k§>
      {
         var _loc2_:§_-Go§ = null;
         var _loc1_:Vector.<§_-e1k§> = new Vector.<§_-e1k§>(0);
         for each(_loc2_ in this.§_-93H§)
         {
            if(_loc2_ is §_-e1k§)
            {
               _loc1_.push(§_-e1k§(_loc2_));
            }
         }
         return _loc1_;
      }
      
      public function §_-A1N§() : Vector.<int>
      {
         var _loc2_:§_-Go§ = null;
         var _loc1_:Vector.<int> = new Vector.<int>(0);
         for each(_loc2_ in this.§_-93H§)
         {
            if(_loc2_ is §_-e1k§)
            {
               _loc1_.push(§_-e1k§(_loc2_).getId());
            }
         }
         return _loc1_;
      }
      
      public function §_-q1L§() : Array
      {
         var _loc2_:§_-Go§ = null;
         var _loc1_:Array = [];
         for each(_loc2_ in this.§_-93H§)
         {
            if(_loc2_ is §_-iI§)
            {
               _loc1_.push(_loc2_);
            }
         }
         for each(_loc2_ in this.§_-93H§)
         {
            if(_loc2_ is §_-uU§)
            {
               _loc1_.push(_loc2_);
            }
         }
         for each(_loc2_ in this.§_-53L§())
         {
            _loc1_.push(_loc2_);
         }
         for each(_loc2_ in this.§_-S1n§())
         {
            _loc1_.push(_loc2_);
         }
         for each(_loc2_ in this.§_-h1o§())
         {
            _loc1_.push(_loc2_);
         }
         return _loc1_;
      }
      
      public function §_-u2l§() : uint
      {
         var _loc2_:§_-Go§ = null;
         var _loc1_:uint = 0;
         for each(_loc2_ in this.§_-93H§)
         {
            if(_loc2_ is §_-fG§)
            {
               _loc1_ += _loc2_.getCount();
            }
         }
         return _loc1_;
      }
      
      public function §_-D3r§() : uint
      {
         var _loc2_:§_-Go§ = null;
         var _loc1_:uint = 0;
         for each(_loc2_ in this.§_-93H§)
         {
            if(_loc2_ is §_-e1k§)
            {
               _loc1_ += _loc2_.getCount();
            }
         }
         return _loc1_;
      }
      
      public function §_-Q1z§(param1:§_-Go§) : void
      {
         this.§_-93H§.push(param1);
      }
   }
}

