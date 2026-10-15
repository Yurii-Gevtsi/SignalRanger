package ru.pragmatix.wormix.model.items.craft
{
   import §_-03T§.§_-2p§;
   import §_-21p§.§_-N1H§;
   import §_-62§.§_-F1D§;
   import §_-62§.§_-W1r§;
   import §_-91B§.§_-QK§;
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-yG§;
   import §_-CF§.§_-x2t§;
   import §_-L1H§.§_-b2K§;
   import §_-Y1O§.§_-42h§;
   import §_-b1F§.§_-G4§;
   import §_-l1g§.§_-L3Q§;
   import §_-p24§.*;
   import §_-r1C§.§_-h2B§;
   import §_-z1g§.*;
   import flash.net.Socket;
   import flash.utils.ByteArray;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.edit.ChangeClanDescriptionRequest;
   import ru.pragmatix.wormix.messages.clans.response.edit.ChangeClanDescriptionResponse;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.§_-2t§;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class CraftedEquipFamily extends §_-W1r§
   {
      
      private var §_-x2y§:§_-N1H§;
      
      private var prefixes:Vector.<§_-QK§>;
      
      private var reagents:Vector.<§_-F1D§>;
      
      private var §_-C1c§:Vector.<§_-F1D§>;
      
      private var §_-aw§:§_-x2t§;
      
      private var §_-i1S§:Boolean = false;
      
      public function CraftedEquipFamily()
      {
         super();
      }
      
      override public function initFromConfig(param1:Object) : void
      {
         var _loc2_:Object = null;
         var _loc3_:Object = null;
         super.initFromConfig(param1);
         if(param1.reagents)
         {
            this.reagents = param1.reagents;
         }
         if(param1.hideInCraft)
         {
            this.§_-i1S§ = Boolean(param1.hideInCraft);
         }
         if(param1.craftCost)
         {
            _loc2_ = param1.craftCost;
            if(_loc2_.reagents)
            {
               this.reagents = _loc2_.reagents;
            }
            if(_loc2_.price >= 0 || _loc2_.realprice >= 0)
            {
               §_-I1u§(_loc2_.price,_loc2_.realprice);
            }
         }
         if(param1.remakeCost)
         {
            _loc3_ = param1.remakeCost;
            if(_loc3_.reagents)
            {
               this.§_-C1c§ = _loc3_.reagents;
            }
            if(_loc3_.price >= 0 || _loc3_.realprice >= 0)
            {
               this.§_-82O§(_loc3_.price,_loc3_.realprice);
            }
         }
         if(param1.charConfig)
         {
            §_-pV§(param1.charConfig[0].iconClassName);
         }
         if(param1.familyId)
         {
            setId(param1.familyId);
         }
         this.§_-x2y§ = new §_-N1H§(§_-N1H§.NONE);
         §_-2t§.§_-X1r§(this.§_-x2y§,param1);
      }
      
      public function §_-22n§(param1:Vector.<§_-QK§>) : void
      {
         this.prefixes = param1;
      }
      
      public function §_-42S§() : §_-N1H§
      {
         return this.§_-x2y§;
      }
      
      public function §_-46§() : Vector.<§_-QK§>
      {
         return this.prefixes;
      }
      
      public function §_-v2R§(param1:int) : §_-QK§
      {
         var _loc2_:§_-QK§ = null;
         var _loc3_:§_-QK§ = null;
         for each(_loc3_ in this.prefixes)
         {
            if(_loc3_.§_-62n§() == param1)
            {
               _loc2_ = _loc3_;
               break;
            }
         }
         return _loc2_;
      }
      
      public function §_-h1o§() : Vector.<§_-F1D§>
      {
         return this.reagents;
      }
      
      public function §_-b2Z§() : Vector.<§_-F1D§>
      {
         return this.§_-C1c§;
      }
      
      public function §_-82O§(param1:uint, param2:uint) : void
      {
         this.§_-aw§ = new §_-x2t§(param1,param2);
      }
      
      public function §_-e18§() : §_-x2t§
      {
         return this.§_-aw§;
      }
      
      public function get hideInCraft() : Boolean
      {
         return this.§_-i1S§;
      }
   }
}

