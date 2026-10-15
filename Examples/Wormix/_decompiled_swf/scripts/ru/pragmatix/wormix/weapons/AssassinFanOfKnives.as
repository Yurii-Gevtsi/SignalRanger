package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-h2X§;
   import §_-A1K§.§_-TA§;
   import §_-A2F§.§_-4B§;
   import §_-CF§.§_-x2t§;
   import §_-D3A§.§_-TZ§;
   import §_-Z1K§.§_-q24§;
   import §_-b1F§.§_-G4§;
   import §_-l1K§.§_-W2f§;
   import §_-l1g§.§_-L3Q§;
   import §_-w2i§.§_-Zd§;
   import flash.utils.getQualifiedClassName;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.ai.§_-L1S§;
   import ru.pragmatix.wormix.game.ai.§_-n2q§;
   import ru.pragmatix.wormix.model.§_-4V§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-F2L§;
   import ru.pragmatix.wormix.weapons.ammo.type.shuriken.Shuriken;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   
   public class AssassinFanOfKnives extends Weapon
   {
      
      private static var moves:Array;
      
      private static const FRAME_LIMIT:§_-TA§ = new §_-TA§(100);
      
      private static const §_-KK§:uint = 35;
      
      private var §_-d4§:Number;
      
      private var frames:§_-TA§ = new §_-TA§(0);
      
      public function AssassinFanOfKnives()
      {
         super();
         MIN_POWER = 45;
         MAX_POWER = 45;
         §_-z1W§(new §_-W2f§());
         moves = [];
      }
      
      public static function §_-81f§(param1:Number, param2:Vector.<§_-L1S§>) : Number
      {
         var _loc4_:§_-L1S§ = null;
         var _loc3_:Number = Number.NaN;
         if(param2.length > 0)
         {
            _loc3_ = param2[0].angle;
            for each(_loc4_ in param2)
            {
               _loc3_ = MathUtil.§_-l2u§(param1,_loc3_,_loc4_.angle);
            }
         }
         return _loc3_;
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         §_-Y2U§ = new §_-h2X§(300);
         super.§_-z2Y§(param1);
         paramSet = §_-73v§.getWeapon(§_-q24§.§_-I2I§).§_-Q2H§().clone();
      }
      
      override protected function shoot() : void
      {
         owner.§_-d2T§(clip);
         ControlSets.UNIT_CONTROL_SET.deactivate();
         §_-K2b§();
         this.frames.value = 0;
         shooting = true;
         §_-G4§.§_-Ee§(owner,owner.x,owner.y - §_-KK§,null,0,§_-KK§ + 15,§_-KK§ + 5,false);
         if(owner)
         {
            Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_LARGE,owner.x,owner.y,§_-KK§ + 5,§_-d27§.§_-S14§);
            this.§_-d4§ = owner.y - §_-KK§;
            this.§_-C3q§();
            §_-TZ§.addListener(owner as EventDispatcher,§_-L3Q§.§_-Qw§,this.onDamaged,§_-4V§.§_-CO§);
         }
      }
      
      private function onDamaged(param1:Event) : void
      {
         this.§_-C3q§();
      }
      
      private function §_-C3q§() : void
      {
         owner.stable = true;
         owner.y = this.§_-d4§;
         owner.graphics.§_-iU§();
      }
      
      override protected function onFrame(param1:Event) : void
      {
         var move:Array = null;
         var knife:Shuriken = null;
         var e:Event = param1;
         ++this.frames.value;
         if(this.frames.value % 3 == 0)
         {
            §_-F2L§.§_-13w§(§_-X1j§.§_-12n§.gameMaster.§_-D2A§(owner),function(param1:§_-01J§):Boolean
            {
               addMoves(moves,param1,30);
               addMoves(moves,param1,60);
               addMoves(moves,param1,90);
               return false;
            });
            if(moves.length > 0)
            {
               move = moves[int(Math.random() * moves.length)];
               knife = new Shuriken(owner,paramSet,move[0],move[1],false);
               knife.§_-S2S§(false,false);
               §_-Zd§.once(knife,§_-L3Q§.§_-o13§,function(param1:Event):void
               {
                  if(param1.data.other is §_-01J§)
                  {
                     §_-4B§.§_-5G§(§_-01J§(param1.data.other),owner);
                  }
               });
               moves = [];
            }
            else
            {
               moves = [];
               this.detach();
            }
         }
         if(this.frames.value > FRAME_LIMIT.value)
         {
            this.§_-Ck§();
         }
         if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
         {
            §_-4B§.§_-qn§(owner);
         }
         if(§_-X1j§.§_-12n§.§_-WN§().online)
         {
            if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
            {
               paramSet.damage.value += Math.random() * 3;
            }
            else if(§_-X1j§.§_-12n§.gameMaster.§_-e2H§())
            {
               paramSet.damage.value -= Math.random() * 3;
            }
         }
      }
      
      private function addMoves(param1:Array, param2:§_-01J§, param3:uint) : void
      {
         var _loc4_:Vector.<§_-L1S§> = null;
         var _loc5_:Number = NaN;
         if(!owner || !param2)
         {
            return;
         }
         _loc4_ = new Vector.<§_-L1S§>(0);
         §_-n2q§.§_-Tp§(owner.x,owner.y,param2.x,param2.y,param3,_loc4_);
         _loc5_ = MathUtil.§_-02X§(owner.x,owner.y,param2.x,param2.y);
         var _loc6_:Number = §_-81f§(_loc5_,_loc4_);
         if(!isNaN(_loc6_))
         {
            param1.push([_loc6_,param3]);
         }
      }
      
      protected function §_-Ck§() : void
      {
         shooting = false;
         ControlSets.UNIT_CONTROL_SET.activate();
         §_-G1Z§();
         owner.stable = false;
         nextTurnOnStable();
         this.detach();
      }
      
      override public function detach() : void
      {
         §_-TZ§.removeListener(owner as EventDispatcher,§_-L3Q§.§_-Qw§,this.onDamaged);
         super.detach();
      }
   }
}

