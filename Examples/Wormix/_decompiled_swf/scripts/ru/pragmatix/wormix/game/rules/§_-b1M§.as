package ru.pragmatix.wormix.game.rules
{
   import §_-03T§.*;
   import §_-2U§.Alchemist;
   import §_-2U§.AncientGhost;
   import §_-2U§.Archbot;
   import §_-2U§.Assassin;
   import §_-2U§.DarkKnight;
   import §_-2U§.Emperor;
   import §_-2U§.Engineer;
   import §_-2U§.Scientist;
   import §_-2U§.Symbiote;
   import §_-2U§.Yakuza;
   import §_-2U§.§_-217§;
   import §_-2U§.§_-cH§;
   import §_-2U§.§_-d1J§;
   import §_-2U§.§_-t2u§;
   import §_-31W§.§_-g16§;
   import §_-51W§.§_-92y§;
   import §_-62§.*;
   import §_-92a§.*;
   import §_-9e§.§_-73e§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-ts§;
   import §_-B2z§.Server;
   import §_-B2z§.§_-c1G§;
   import §_-CF§.§_-E19§;
   import §_-DJ§.§_-73y§;
   import §_-E29§.*;
   import §_-G24§.§_-yi§;
   import §_-H3§.*;
   import §_-L2F§.§_-D2p§;
   import §_-P1z§.§_-Cc§;
   import §_-P1z§.§_-E2z§;
   import §_-P1z§.§_-L1s§;
   import §_-P1z§.§_-j2E§;
   import §_-P1z§.§_-k2X§;
   import §_-QD§.§_-G2z§;
   import §_-R2S§.Farmer;
   import §_-R2S§.Hunter;
   import §_-R2S§.Maniacs;
   import §_-R2S§.Miner;
   import §_-R2S§.Sergeant;
   import §_-R2S§.§_-M2x§;
   import §_-R2S§.§_-P1U§;
   import §_-R2S§.§_-z2x§;
   import §_-RE§.*;
   import §_-U1i§.§_-81M§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-O2i§;
   import §_-U1i§.§_-XA§;
   import §_-U1i§.§_-b1k§;
   import §_-U1i§.§_-hK§;
   import §_-Ub§.Vikings;
   import §_-Ub§.VoodooShaman;
   import §_-Ub§.WindMaster;
   import §_-Ub§.§_-W1E§;
   import §_-Ub§.§_-v1t§;
   import §_-Y1O§.*;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1Q§.§_-tc§;
   import §_-Y1b§.§_-OP§;
   import §_-Y1b§.§_-s2Z§;
   import §_-Z1K§.§_-q24§;
   import §_-aM§.*;
   import §_-k2s§.§_-b0§;
   import §_-k2s§.§_-e1U§;
   import §_-k2s§.§_-iI§;
   import §_-k2s§.§_-uU§;
   import §_-m1g§.§_-K2W§;
   import §_-qH§.§_-S25§;
   import §_-r1C§.§_-t1R§;
   import §_-rM§.§_-D2L§;
   import §_-s1Q§.Artifacts;
   import §_-w2i§.§_-Zd§;
   import §_-z1g§.§_-82X§;
   import §_-z1g§.§_-H31§;
   import §_-z1g§.§_-y1S§;
   import dragonBones.Armature;
   import dragonBones.events.EventObject;
   import feathers.core.FeathersControl;
   import feathers.layout.HorizontalAlign;
   import flash.events.Event;
   import flash.events.IEventDispatcher;
   import flash.net.URLRequest;
   import flash.net.navigateToURL;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import flash.utils.getDefinitionByName;
   import flash.utils.getQualifiedClassName;
   import ru.pragmatix.flox.serialization.*;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-3p§;
   import ru.pragmatix.wormix.game.ai.§_-12u§;
   import ru.pragmatix.wormix.game.ai.§_-A1l§;
   import ru.pragmatix.wormix.game.ai.§_-H3h§;
   import ru.pragmatix.wormix.game.ai.§_-dG§;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.game.ai.§_-n2q§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanMemberStructure;
   import ru.pragmatix.wormix.messages.structures.RatingProfileStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.model.rewards.§_-P2q§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.pvp.messages.ex.PvpBattleType;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-2t§;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import ru.pragmatix.wormix.weapons.ammo.type.RopeHook;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.ScaleMode;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class §_-b1M§ extends §_-I1Q§
   {
      
      private var §_-k1T§:§_-b1k§;
      
      public function §_-b1M§(param1:§_-O2i§)
      {
         super(param1);
         this.§_-k1T§ = param1.§_-11J§;
      }
      
      override public function §_-e2d§() : void
      {
         §_-X1j§.§_-B35§.§_-33m§();
      }
      
      override public function §_-l26§(param1:§_-hK§) : void
      {
         §_-X1j§.§_-12n§.battleStats.increment(§_-g16§.MISSIONS_EARNED,param1.money);
         §_-X1j§.§_-61i§.§_-R2f§(Application.userProfile);
         §_-X1j§.§_-61i§.§_-K3C§();
      }
      
      override public function §_-p1t§(param1:§_-J1§, param2:§_-XA§) : §_-hK§
      {
         var _loc3_:§_-hK§ = null;
         _loc3_ = super.§_-p1t§(param1,param2);
         _loc3_.bonusExp = §_-X1j§.§_-12n§.gameMaster.§_-s13§();
         _loc3_.§_-43K§(§_-X1j§.§_-12n§.gameMaster.§_-A1N§());
         _loc3_.§_-11J§ = this.§_-k1T§;
         _loc3_.money = §_-H31§.§_-23k§(_loc3_) + §_-H31§.§_-v4§(_loc3_);
         _loc3_.exp = §_-H31§.§_-q2g§(_loc3_);
         §_-Q2u§.§_-f1x§(_loc3_);
         return _loc3_;
      }
      
      override public function §_-73f§() : §_-3p§
      {
         §_-cM§(§_-y1o§.§_-Y2B§);
         return new §_-3p§(squads,§_-717§,§_-73e§.§_-U5§());
      }
      
      override public function §_-Q1M§(param1:§_-O2i§, param2:§_-hK§) : §_-73y§
      {
         return §_-yi§.create(param1,param2);
      }
   }
}

