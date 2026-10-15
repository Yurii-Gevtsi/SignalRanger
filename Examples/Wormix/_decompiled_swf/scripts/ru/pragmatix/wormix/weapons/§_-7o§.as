package ru.pragmatix.wormix.weapons
{
   import §_-CF§.§_-E19§;
   import §_-D3A§.§_-TZ§;
   import §_-G2H§.*;
   import §_-L1H§.§_-C3e§;
   import §_-YF§.§_-q2v§;
   import §_-Z1K§.§_-q24§;
   import §_-k1r§.§_-j2G§;
   import §_-k1u§.§_-F3H§;
   import §_-k1u§.§_-Oi§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-H3D§;
   import ru.pragmatix.flox.social.model.PaymentOption;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.weapons.ammo.AtomBomb;
   import starling.display.Image;
   import starling.events.Event;
   
   public class §_-7o§ extends §_-F3o§
   {
      
      private var _owner:§_-01J§;
      
      private var paramSet:§_-C3e§;
      
      private var §_-y1C§:int;
      
      private var §_-N1F§:int;
      
      public var vx:Number;
      
      public var vy:Number;
      
      private var §_-J3h§:uint;
      
      private var §_-N29§:int;
      
      public function §_-7o§(param1:§_-01J§, param2:§_-C3e§, param3:int, param4:int, param5:int, param6:int, param7:Number, param8:Number)
      {
         this._owner = param1;
         this.paramSet = param2;
         this.§_-y1C§ = param5;
         this.§_-N1F§ = param6;
         this.vx = param7;
         this.vy = param8;
         super();
         var _loc9_:Image = new Image(Application.assetManager.getTexture(§_-q2v§.§_-r1P§));
         _loc9_.alignPivot();
         §_-z1H§ = _loc9_;
         §_-j2P§ = SortingLayer.POINTERS_LAYER;
         stable = true;
         this.x = param3;
         this.y = param4;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
         §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.SOT,this.nextTurn);
         this.§_-J3h§ = param1.squad.getNumber();
         this.§_-N29§ = §_-X1j§.§_-12n§.scene.gameInterface.alertPanel.§_-82b§(§_-j2G§.§_-b2v§,§_-s2a§.NUCLEAR_LAUNCH_DETECTED);
         §_-h2B§.play(§_-d27§.§_-p1v§);
      }
      
      private function nextTurn(param1:Event) : void
      {
         var _loc2_:§_-F3H§ = null;
         var _loc3_:§_-Oi§ = null;
         if(!§_-X1j§.§_-12n§.gameMaster.activeUnit.disposed && §_-X1j§.§_-12n§.gameMaster.activeUnit.squad.getNumber() == this.§_-J3h§)
         {
            new AtomBomb(this._owner,this.paramSet,this.§_-y1C§,this.§_-N1F§,this.vx,this.vy);
            disposed = true;
            _loc2_ = this._owner.squad.§_-B2M§().§_-V2J§();
            _loc3_ = _loc2_.§_-d0§(§_-q24§.§_-E2U§);
            if(_loc3_)
            {
               _loc3_.§_-s1k§ = true;
            }
         }
      }
      
      public function §_-22V§() : int
      {
         return this.§_-y1C§ + 10 * §_-H3D§.sign(this.vx);
      }
      
      public function §_-J1P§() : int
      {
         return this.§_-N1F§;
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         this._owner = null;
         this.paramSet = null;
         §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.SOT,this.nextTurn);
         §_-X1j§.§_-12n§.scene.gameInterface.alertPanel.§_-53Z§(this.§_-N29§);
         super.defaultOnDisposed(param1);
      }
      
      public function get owner() : §_-01J§
      {
         return this._owner;
      }
   }
}

