package ru.pragmatix.wormix.objects
{
   import §_-21p§.§_-N1H§;
   import §_-51W§.§_-SN§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-73d§.§_-h2X§;
   import §_-A2F§.§_-4B§;
   import §_-Cl§.§_-L2z§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-J2l§;
   import §_-L1H§.§_-b2K§;
   import §_-P1z§.*;
   import §_-Tm§.Action;
   import §_-Tm§.§_-K1I§;
   import §_-Tm§.§_-RF§;
   import §_-Tm§.§_-W1Y§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-XA§;
   import §_-U1i§.§_-b1k§;
   import §_-Z1K§.§_-q24§;
   import §_-Z1L§.§_-A2V§;
   import §_-Z1L§.§_-H1K§;
   import §_-b1F§.§_-G4§;
   import §_-l1K§.§_-cZ§;
   import §_-l1g§.§_-L3Q§;
   import §_-p14§.§_-02J§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Zd§;
   import §_-x2Q§.§_-dU§;
   import §_-z1g§.*;
   import feathers.utils.textures.TextureCache;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-Q2v§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class §_-X2V§
   {
      
      private static const §_-L20§:int = 25;
      
      private static const §_-Ny§:uint = 2 * 60 * 1000;
      
      private const §_-5N§:Array = [§_-RF§.§_-h2w§,§_-RF§.§_-m2J§,§_-RF§.§_-Ja§,§_-RF§.§_-hX§,§_-RF§.§_-W3§,§_-RF§.§_-XX§,§_-RF§.§_-537§,§_-RF§.§_-T2D§,§_-RF§.§_-o1G§,§_-RF§.§_-E3N§,§_-RF§.§_-Ad§,§_-RF§.§_-xU§,§_-RF§.§_-gI§,§_-RF§.§_-N2h§,§_-RF§.§_-839§,§_-RF§.§_-sw§,§_-RF§.§_-G1H§];
      
      private const §_-z1u§:Array = [§_-RF§.§_-St§,§_-RF§.§_-G1H§,§_-RF§.§_-Z24§,§_-RF§.§_-s17§,§_-RF§.§_-o3§];
      
      private const §_-K3X§:Array = [§_-RF§.§_-WT§,§_-RF§.§_-Hl§,§_-RF§.§_-C3J§];
      
      private var restrictions:Array = [];
      
      private var §_-v2d§:Array = [];
      
      public var §_-33a§:§_-02J§ = new §_-02J§();
      
      private var §_-j1s§:int = 0;
      
      private var §_-B1v§:Boolean = false;
      
      private var §_-t7§:Boolean = false;
      
      private var §_-i2J§:Boolean;
      
      private var §_-Ne§:Boolean;
      
      private var §_-12Z§:Boolean;
      
      public var paused:Boolean = false;
      
      private var §_-T2t§:Array = [];
      
      public function §_-X2V§()
      {
         super();
         this.restrictions[§_-RF§.§_-St§] = [§_-RF§.§_-G1H§];
      }
      
      public function §_-l2M§(param1:uint) : Boolean
      {
         var _loc3_:Array = null;
         var _loc4_:Action = null;
         var _loc5_:uint = 0;
         var _loc6_:* = 0;
         var _loc7_:int = 0;
         var _loc8_:Array = null;
         var _loc9_:§_-01J§ = null;
         var _loc2_:§_-eX§ = §_-X1j§.§_-12n§.gameMaster;
         if(_loc2_)
         {
            if(this.paused || _loc2_.§_-e2H§() && !this.§_-33a§.§_-92Y§() && _loc2_.§_-J33§())
            {
               if(!this.§_-Ne§)
               {
                  if(this.§_-i2J§)
                  {
                     this.cancelBattle();
                  }
                  else
                  {
                     ++this.§_-j1s§;
                     if(!this.paused && this.§_-j1s§ >= §_-L20§ && !this.§_-B1v§ && this.§_-33a§.§_-N18§() != 1)
                     {
                        this.§_-B1v§ = true;
                        §_-J2l§.§_-j28§.§_-x2V§();
                     }
                  }
               }
               else
               {
                  _loc9_ = _loc2_.activeUnit;
                  if((Boolean(_loc9_)) && Boolean(!_loc9_.disposed) && Boolean(_loc9_.weapon))
                  {
                     _loc9_.weapon.detach();
                  }
                  if(!_loc2_.§_-Rk§())
                  {
                     _loc2_.§_-f13§();
                  }
                  _loc2_.§_-A1C§();
               }
               return false;
            }
            if(this.§_-B1v§)
            {
               this.§_-B1v§ = false;
               §_-J2l§.§_-j28§.hide();
               this.§_-j1s§ = 0;
            }
            _loc3_ = this.§_-33a§.§_-d1M§();
            _loc4_ = null;
            _loc5_ = _loc3_.length;
            _loc6_ = 0;
            while(_loc6_ < _loc5_)
            {
               if(_loc3_[_loc6_].commandId == §_-RF§.§_-y1J§)
               {
                  _loc4_ = _loc3_[_loc6_];
                  this.§_-t7§ = true;
                  break;
               }
               _loc6_++;
            }
            if(_loc4_ != null)
            {
               _loc3_ = [_loc4_];
               _loc5_ = 1;
            }
            this.§_-T2t§.splice(0);
            _loc7_ = -1;
            _loc6_ = 0;
            while(_loc6_ < _loc5_)
            {
               if(this.§_-z1u§.indexOf(_loc3_[_loc6_].commandId) != -1)
               {
                  this.§_-T2t§.push(_loc6_);
               }
               if(this.§_-K3X§.indexOf(_loc3_[_loc6_].commandId) != -1)
               {
                  _loc7_ = _loc6_;
               }
               _loc6_++;
            }
            if(this.§_-T2t§.length > 0 && _loc7_ >= 0)
            {
               _loc6_ = int(this.§_-T2t§.length);
               while(_loc6_--)
               {
                  _loc3_.splice(this.§_-T2t§[_loc6_],1);
                  _loc5_--;
               }
            }
            _loc8_ = null;
            _loc6_ = 0;
            while(_loc6_ < _loc5_)
            {
               if(param1 == 1)
               {
                  break;
               }
               if(this.§_-t7§ && _loc3_[_loc6_].commandId == §_-RF§.§_-C3J§)
               {
                  break;
               }
               if(this.§_-v2d§.indexOf(_loc3_[_loc6_].commandId) == -1 && (_loc8_ == null || _loc8_.indexOf(_loc3_[_loc6_].commandId) != -1))
               {
                  §_-W1Y§.run(_loc3_[_loc6_]);
                  if(§_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-C6§())
                  {
                     return false;
                  }
                  if(this.restrictions[_loc3_[_loc6_].commandId])
                  {
                     _loc8_ = this.restrictions[_loc3_[_loc6_].commandId];
                  }
                  else if(this.§_-5N§.indexOf(_loc3_[_loc6_].commandId) == -1 && _loc6_ < _loc5_ - 1)
                  {
                     break;
                  }
               }
               _loc6_++;
            }
            if(_loc2_.§_-E3b§() || _loc2_.§_-y2v§())
            {
               §_-X1j§.§_-12n§.scene.dispatchEvent(§_-K1I§.empty());
            }
         }
         return true;
      }
      
      public function reset() : void
      {
         this.§_-33a§.reset();
         this.§_-B1v§ = true;
         this.§_-j1s§ = 0;
         this.§_-t7§ = false;
         this.§_-i2J§ = false;
         this.§_-Ne§ = false;
         this.§_-12Z§ = false;
      }
      
      public function run() : void
      {
         this.§_-33a§.run();
      }
      
      public function stop() : void
      {
         if(this.§_-B1v§)
         {
            §_-J2l§.§_-j28§.hide();
         }
         this.§_-33a§.stop();
      }
      
      public function §_-9I§(param1:Array) : void
      {
         this.§_-v2d§ = param1.concat();
      }
      
      private function cancelBattle() : void
      {
         §_-X1j§.§_-12n§.§_-Q2l§(§_-J1§.DRAW_GAME,§_-XA§.§_-pq§);
      }
      
      public function §_-01t§() : Boolean
      {
         return this.§_-i2J§;
      }
      
      public function §_-01O§(param1:Boolean) : void
      {
         this.§_-i2J§ = true;
         if(!param1)
         {
            §_-X1j§.§_-12n§.scene.dispatchEventWith(§_-L3Q§.END_TURN_RECEIVED);
         }
         else
         {
            this.§_-Ne§ = true;
         }
      }
      
      public function §_-s11§() : Boolean
      {
         return this.§_-12Z§;
      }
      
      public function §_-42D§() : void
      {
         this.§_-12Z§ = true;
         §_-X1j§.§_-12n§.scene.dispatchEventWith(§_-L3Q§.START_TURN_RECEIVED);
      }
   }
}

