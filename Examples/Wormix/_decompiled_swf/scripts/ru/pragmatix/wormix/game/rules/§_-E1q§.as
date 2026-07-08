package ru.pragmatix.wormix.game.rules
{
   import §_-2U§.*;
   import §_-31W§.§_-E1E§;
   import §_-51M§.Cat;
   import §_-62§.§_-T1y§;
   import §_-9e§.§_-73e§;
   import §_-A1K§.§_-7u§;
   import §_-Cl§.*;
   import §_-DJ§.§_-73y§;
   import §_-G24§.§_-NR§;
   import §_-Ly§.§_-81L§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-XA§;
   import §_-U1i§.§_-hK§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1O§.§_-y1o§;
   import §_-Z1K§.§_-q24§;
   import §_-g2e§.§_-q2Z§;
   import §_-k1u§.§_-2m§;
   import §_-k1u§.§_-Vr§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-z1g§.§_-y1S§;
   import com.greensock.loading.*;
   import com.hurlant.util.der.*;
   import feathers.controls.TextInput;
   import flash.display.MovieClip;
   import flash.events.Event;
   import flash.media.SoundTransform;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-C2c§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.§_-3p§;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.§_-fT§;
   import ru.pragmatix.wormix.messages.clans.request.ClanSummaryRequest;
   import ru.pragmatix.wormix.messages.clans.response.ClanSummaryResponse;
   import ru.pragmatix.wormix.messages.clans.response.edit.ChangeClanEmblemResponse;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.§_-73v§;
   import starling.events.Event;
   
   public class §_-E1q§ extends §_-I1Q§
   {
      
      public function §_-E1q§(param1:§_-O2i§)
      {
         super(param1);
      }
      
      override public function §_-73f§() : §_-3p§
      {
         var _loc3_:§_-01J§ = null;
         §_-cM§(§_-y1o§.§_-F2p§);
         var _loc1_:UserProfile = Application.userProfile;
         var _loc2_:UserProfile = §_-V23§(squads[1]).getOwnerProfile();
         _loc2_.§_-NG§(_loc1_.§_-pd§().clone() as §_-2m§);
         _loc2_.§_-72S§(_loc1_.§_-E39§().clone() as §_-Vr§);
         for each(_loc3_ in §_-V23§(squads[1]).getUnits())
         {
            _loc3_.§_-81o§ = §_-73v§.§_-z1F§(_loc2_);
         }
         return new §_-3p§(squads,§_-717§,§_-73e§.§_-GK§());
      }
      
      override public function §_-p1t§(param1:§_-J1§, param2:§_-XA§) : §_-hK§
      {
         return §_-Q2u§.§_-C1b§(param1.equals(§_-J1§.DRAW_GAME) ? §_-J1§.DRAW_GAME : §_-J1§.WINNER,param2);
      }
      
      override public function §_-Q1M§(param1:§_-O2i§, param2:§_-hK§) : §_-73y§
      {
         return new §_-NR§(param1,param2);
      }
      
      override public function §_-024§() : Boolean
      {
         return false;
      }
   }
}

