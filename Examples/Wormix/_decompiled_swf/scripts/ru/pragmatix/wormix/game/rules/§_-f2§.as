package ru.pragmatix.wormix.game.rules
{
   import §_-21p§.§_-4w§;
   import §_-31W§.§_-g16§;
   import §_-9e§.§_-73e§;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-B1y§.§_-bs§;
   import §_-B2z§.§_-63i§;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.§_-73y§;
   import §_-E1I§.§_-b2n§;
   import §_-G24§.§_-B3M§;
   import §_-G24§.§_-NR§;
   import §_-H16§.*;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-XA§;
   import §_-U1i§.§_-hK§;
   import §_-Y1O§.§_-71D§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1O§.§_-y1o§;
   import §_-j22§.§_-5g§;
   import §_-kS§.§_-P1S§;
   import §_-l1K§.*;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-T2N§;
   import §_-s20§.§_-J2g§;
   import §_-s20§.§_-R1P§;
   import §_-sQ§.§_-V1n§;
   import §_-w2W§.§_-21w§;
   import §_-z1g§.§_-82X§;
   import §_-z1g§.§_-H31§;
   import com.greensock.*;
   import com.greensock.plugins.*;
   import com.hurlant.math.BigInteger;
   import com.hurlant.math.bi_internal;
   import flash.display.MovieClip;
   import flash.errors.IllegalOperationError;
   import flash.events.Event;
   import flash.geom.Point;
   import flash.geom.Rectangle;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import org.swiftsuspenders.Injector;
   import org.swiftsuspenders.§_-71M§;
   import org.swiftsuspenders.§_-T1p§;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-3p§;
   import ru.pragmatix.wormix.gui.menu.§_-3m§;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.client.Disconnect;
   import ru.pragmatix.wormix.messages.client.EndBattle;
   import ru.pragmatix.wormix.messages.structures.PreCalculatedPoints;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.GameObjectUtils;
   import ru.pragmatix.wormix.pvp.messages.client.PvpActionEx;
   import ru.pragmatix.wormix.pvp.messages.client.PvpEndTurn;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.social.*;
   import ru.pragmatix.wormix.weapons.Weapon;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   
   use namespace bi_internal;
   
   public class §_-f2§ extends §_-I1Q§
   {
      
      protected var §_-715§:Boolean;
      
      private var §_-Br§:§_-P1S§ = new §_-P1S§();
      
      private var wager:§_-82X§;
      
      private var §_-Gv§:uint;
      
      private var §_-yo§:int;
      
      private var §_-fK§:Number;
      
      private var §_-Xw§:Number;
      
      private var playerTotalRating:uint;
      
      private var preCalculatedPoints:Array;
      
      public function §_-f2§(param1:§_-O2i§)
      {
         super(param1);
         this.§_-715§ = param1.battleType == §_-81M§.§_-Y1C§ && Boolean(param1.wager) && param1.wager.id != 0;
         if(this.§_-715§)
         {
            this.wager = param1.wager;
         }
         this.§_-Br§.§_-s1z§(param1.mapId);
         this.playerTotalRating = Application.userProfile.§_-Zp§();
         this.§_-yo§ = param1.§_-yo§;
         this.preCalculatedPoints = param1.preCalculatedPoints;
      }
      
      override public function §_-l26§(param1:§_-hK§) : void
      {
         if(param1.result.equals(§_-J1§.WINNER))
         {
            §_-X1j§.§_-12n§.battleStats.increment(§_-g16§.WAGERS_EARNED,param1.money);
         }
      }
      
      override public function §_-p1t§(param1:§_-J1§, param2:§_-XA§) : §_-hK§
      {
         var _loc3_:§_-hK§ = super.§_-p1t§(param1,param2);
         if(this.§_-715§)
         {
            _loc3_.§_-43K§(§_-X1j§.§_-12n§.gameMaster.§_-A1N§());
            _loc3_.money = §_-H31§.§_-y12§(param1,this.wager);
            _loc3_.rating = this.§_-G3K§(param1,this.playerTotalRating).ratingPoints;
            if(param1 == §_-J1§.WINNER)
            {
               if(§_-X1j§.§_-A24§.§_-92u§() && this.wager.value > 0)
               {
                  §_-X1j§.§_-A24§.§_-3b§();
                  _loc3_.§_-dT§ += §_-H31§.§_-g1n§;
                  _loc3_.exp += §_-H31§.§_-52Y§;
               }
               if(this.wager.experience > 0)
               {
                  _loc3_.exp = this.wager.experience;
               }
            }
         }
         return _loc3_;
      }
      
      override public function §_-73f§() : §_-3p§
      {
         var _loc1_:§_-73e§ = null;
         var _loc2_:§_-3p§ = null;
         §_-cM§(§_-y1o§.§_-28§);
         this.§_-Br§.§_-f1u§(squads);
         _loc1_ = §_-73e§.§_-G1§(this.§_-715§);
         _loc1_.§_-52M§ = true;
         _loc1_.§_-G1u§ = true;
         _loc2_ = new §_-3p§(squads,§_-717§,_loc1_);
         var _loc3_:§_-71D§ = _loc2_.§_-HR§();
         this.§_-fK§ = _loc3_.§_-fK§;
         this.§_-Gv§ = _loc3_.§_-FN§;
         this.§_-Xw§ = _loc3_.ratio;
         return _loc2_;
      }
      
      override public function §_-Q1M§(param1:§_-O2i§, param2:§_-hK§) : §_-73y§
      {
         return this.§_-715§ ? new §_-B3M§(param1,param2) : new §_-NR§(param1,param2);
      }
      
      private function §_-G3K§(param1:§_-J1§, param2:int) : PreCalculatedPoints
      {
         var result:PreCalculatedPoints = null;
         var tempResult:PreCalculatedPoints = null;
         var unloosableRating:int = 0;
         var newTotalRating:int = 0;
         var delta:int = 0;
         var battleResult:§_-J1§ = param1;
         var playerTotalRating:int = param2;
         try
         {
            switch(battleResult.value)
            {
               case §_-J1§.WINNER.value:
                  result = this.preCalculatedPoints[0];
                  break;
               case §_-J1§.§_-23m§.value:
                  tempResult = this.preCalculatedPoints[1];
                  result = new PreCalculatedPoints();
                  result.rankPoints = tempResult.rankPoints;
                  unloosableRating = §_-N1V§.settings.§_-21D§()[0].minRating;
                  newTotalRating = playerTotalRating + tempResult.ratingPoints;
                  delta = newTotalRating - unloosableRating;
                  if(delta < 0)
                  {
                     result.ratingPoints = Math.min(0,tempResult.ratingPoints - delta);
                  }
                  else
                  {
                     result.ratingPoints = tempResult.ratingPoints;
                  }
                  break;
               case §_-J1§.DRAW_GAME.value:
                  result = this.preCalculatedPoints[2];
            }
         }
         catch(e:Error)
         {
            §§push(e);
            var _temp_1:* = e;
            e = §§pop();
            return result = new PreCalculatedPoints();
         }
         return result;
      }
   }
}

