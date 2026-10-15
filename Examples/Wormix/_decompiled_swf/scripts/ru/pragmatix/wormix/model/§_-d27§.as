package ru.pragmatix.wormix.model
{
   import §_-62§.§_-W1r§;
   import §_-lh§.§_-z2A§;
   import §_-r1C§.§_-t1R§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.utils.StringUtil;
   import starling.events.Event;
   
   public class §_-d27§
   {
      
      public static const §_-tE§:§_-t1R§ = new §_-t1R§("MenuBeginning");
      
      public static const §_-22k§:§_-t1R§ = new §_-t1R§("MenuLoop");
      
      public static const BATTLE_THEME_1:§_-t1R§ = new §_-t1R§("BattleTheme1");
      
      public static const BATTLE_THEME_2:§_-t1R§ = new §_-t1R§("BattleTheme2");
      
      public static const BATTLE_THEME_3:§_-t1R§ = new §_-t1R§("BattleTheme3");
      
      private static const §_-8M§:Array = [BATTLE_THEME_1,BATTLE_THEME_2,BATTLE_THEME_3];
      
      public static const ARMORY:§_-t1R§ = new §_-t1R§("armory");
      
      public static const ARENA:§_-t1R§ = new §_-t1R§("arena");
      
      public static const §_-A7§:§_-t1R§ = new §_-t1R§("bag",3);
      
      public static const §_-sg§:§_-t1R§ = new §_-t1R§("bank");
      
      public static const §_-S2s§:§_-t1R§ = new §_-t1R§("best");
      
      public static const §_-md§:§_-t1R§ = new §_-t1R§("menu_open");
      
      public static const §_-F1z§:§_-t1R§ = new §_-t1R§("menu_close");
      
      public static const §_-b2H§:§_-t1R§ = new §_-t1R§("menu_deny");
      
      public static const §_-BA§:§_-t1R§ = new §_-t1R§("menu_grant");
      
      public static const §_-S29§:§_-t1R§ = new §_-t1R§("menu_select");
      
      public static const §_-k2x§:§_-t1R§ = new §_-t1R§("menu_turn");
      
      public static const SHOP:§_-t1R§ = new §_-t1R§("shop");
      
      public static const §_-p1C§:§_-t1R§ = new §_-t1R§("message");
      
      public static const §_-D3Y§:§_-t1R§ = new §_-t1R§("message",30);
      
      public static const §_-H6§:§_-t1R§ = new §_-t1R§("battle_start");
      
      public static const BONUS:§_-t1R§ = new §_-t1R§("bonus");
      
      public static const §_-lj§:§_-t1R§ = new §_-t1R§("lose");
      
      public static const §_-d2D§:§_-t1R§ = new §_-t1R§("screenshot");
      
      public static const WATER_SPLASH1:§_-t1R§ = new §_-t1R§("water_splash1");
      
      public static const WATER_SPLASH2:§_-t1R§ = new §_-t1R§("water_splash2");
      
      public static const WATER_SPLASH3:§_-t1R§ = new §_-t1R§("water_splash3");
      
      public static const WATER_IMPACT1:§_-t1R§ = new §_-t1R§("water_impact_random1");
      
      public static const WATER_IMPACT2:§_-t1R§ = new §_-t1R§("water_impact_random2");
      
      public static const WATER_IMPACT3:§_-t1R§ = new §_-t1R§("water_impact_random3");
      
      public static const §_-T2I§:§_-t1R§ = new §_-t1R§("water_up");
      
      public static const §_-M1L§:§_-t1R§ = new §_-t1R§("kanat_impact");
      
      public static const §_-w2g§:§_-t1R§ = new §_-t1R§("icegun_attack");
      
      public static const §_-Y24§:§_-t1R§ = new §_-t1R§("icegun_explosion");
      
      public static const §_-T1j§:§_-t1R§ = new §_-t1R§("icegun_explosion",0.2);
      
      public static const §_-x1X§:§_-t1R§ = new §_-t1R§("icegun_icecrash");
      
      public static const §_-c1X§:§_-t1R§ = new §_-t1R§("hrumhrum");
      
      public static const §_-M2a§:§_-t1R§ = new §_-t1R§("bat_swoosh",2);
      
      public static const §_-K2T§:§_-t1R§ = new §_-t1R§("bat_smash",2);
      
      public static const §_-k1F§:§_-t1R§ = new §_-t1R§("balka",2);
      
      public static const §_-21e§:§_-t1R§ = new §_-t1R§("arbalet_attack");
      
      public static const §_-D24§:§_-t1R§ = new §_-t1R§("airstrike",2);
      
      public static const §_-I1v§:§_-t1R§ = new §_-t1R§("arbuz_bounce");
      
      public static const §_-s24§:§_-t1R§ = new §_-t1R§("direction");
      
      public static const §_-a2n§:§_-t1R§ = new §_-t1R§("dynamite_explosion");
      
      public static const §_-2e§:§_-t1R§ = new §_-t1R§("dynamite_fuse_loop");
      
      public static const §_-Z1d§:§_-t1R§ = new §_-t1R§("electroshocker");
      
      public static const §_-F1A§:§_-t1R§ = new §_-t1R§("fire_loop");
      
      public static const §_-tp§:§_-t1R§ = new §_-t1R§("flamer");
      
      public static const §_-11Y§:§_-t1R§ = new §_-t1R§("flamer_loop",1.5);
      
      public static const §_-G3Z§:§_-t1R§ = new §_-t1R§("grenade_attack");
      
      public static const §_-x14§:§_-t1R§ = new §_-t1R§("grenade1_bounce");
      
      public static const §_-32Z§:§_-t1R§ = new §_-t1R§("grenade1_bounce_stop");
      
      public static const §_-S14§:§_-t1R§ = new §_-t1R§("grenade1_explosion",4);
      
      public static const §_-S3§:§_-t1R§ = new §_-t1R§("ground_loop");
      
      public static const §_-d2A§:§_-t1R§ = new §_-t1R§("jackhammer");
      
      public static const §_-F3I§:§_-t1R§ = new §_-t1R§("jetpack_loop");
      
      public static const §_-s2k§:§_-t1R§ = new §_-t1R§("jetpack_start");
      
      public static const §_-54§:§_-t1R§ = new §_-t1R§("jetpack_stop");
      
      public static const §_-27§:§_-t1R§ = new §_-t1R§("mine_beep");
      
      public static const §_-Q2F§:§_-t1R§ = new §_-t1R§("molotov_broke");
      
      public static const RIKOSHET1:§_-t1R§ = new §_-t1R§("rikoshet1");
      
      public static const RIKOSHET2:§_-t1R§ = new §_-t1R§("rikoshet2");
      
      public static const §_-I1P§:§_-t1R§ = new §_-t1R§("rocket_attack");
      
      public static const §_-33W§:§_-t1R§ = new §_-t1R§("rocket1_explosion",4);
      
      public static const §_-J1x§:§_-t1R§ = new §_-t1R§("salute_explosion");
      
      public static const §_-a24§:§_-t1R§ = new §_-t1R§("salute_start");
      
      public static const SWINE:§_-t1R§ = new §_-t1R§("swine");
      
      public static const §_-h1g§:§_-t1R§ = new §_-t1R§("cat_air_strike");
      
      public static const §_-vS§:§_-t1R§ = new §_-t1R§("shotgun_attack");
      
      public static const §_-h1O§:§_-t1R§ = new §_-t1R§("slimer");
      
      public static const §_-D1A§:§_-t1R§ = new §_-t1R§("sniper");
      
      public static const §_-p1D§:§_-t1R§ = new §_-t1R§("sheep_super_loop");
      
      public static const §_-c2j§:§_-t1R§ = new §_-t1R§("shotgun2_attack");
      
      public static const TELEPORT:§_-t1R§ = new §_-t1R§("teleport",2);
      
      public static const §_-42y§:§_-t1R§ = new §_-t1R§("throwpower");
      
      public static const §_-73O§:§_-t1R§ = new §_-t1R§("vulcan_loop");
      
      public static const §_-1g§:§_-t1R§ = new §_-t1R§("uzi_loop");
      
      public static const CASSETE_EXPLOSION1:§_-t1R§ = new §_-t1R§("cassete_explosion_random1",2);
      
      public static const CASSETE_EXPLOSION2:§_-t1R§ = new §_-t1R§("cassete_explosion_random2",2);
      
      public static const CASSETE_EXPLOSION3:§_-t1R§ = new §_-t1R§("cassete_explosion_random3",2);
      
      public static const §_-13O§:§_-t1R§ = new §_-t1R§("SpiderMineWalk_LOOP2");
      
      public static const §_-V1M§:§_-t1R§ = new §_-t1R§("SpiderMineArmed");
      
      public static const §_-12k§:§_-t1R§ = new §_-t1R§("SpiderMineAttack");
      
      public static const SHURIKEN_ATTACK_01:§_-t1R§ = new §_-t1R§("ShurikenAttack01");
      
      public static const SHURIKEN_ATTACK_02:§_-t1R§ = new §_-t1R§("ShurikenAttack02");
      
      public static const SHURIKEN_ATTACK_03:§_-t1R§ = new §_-t1R§("ShurikenAttack03");
      
      public static const SHURIKEN_ATTACK_04:§_-t1R§ = new §_-t1R§("ShurikenAttack04");
      
      public static const SHURIKEN_HITGROUND_01:§_-t1R§ = new §_-t1R§("ShurikenHitGround01");
      
      public static const SHURIKEN_HITGROUND_02:§_-t1R§ = new §_-t1R§("ShurikenHitGround02");
      
      public static const SHURIKEN_HITGROUND_03:§_-t1R§ = new §_-t1R§("ShurikenHitGround03");
      
      public static const SHURIKEN_HITGROUND_04:§_-t1R§ = new §_-t1R§("ShurikenHitGround04");
      
      public static const SHURIKEN_HITWORM_01:§_-t1R§ = new §_-t1R§("ShurikenHitWorm01");
      
      public static const SHURIKEN_HITWORM_02:§_-t1R§ = new §_-t1R§("ShurikenHitWorm02");
      
      public static const SHURIKEN_HITWORM_03:§_-t1R§ = new §_-t1R§("ShurikenHitWorm03");
      
      public static const SHURIKEN_HITWORM_04:§_-t1R§ = new §_-t1R§("ShurikenHitWorm04");
      
      public static const §_-o2v§:§_-t1R§ = new §_-t1R§("VintageCannonAttack");
      
      public static const §_-m2k§:§_-t1R§ = new §_-t1R§("VintageCannonballExplosion");
      
      public static const VINTAGE_CANNONBALL_HIT_01:§_-t1R§ = new §_-t1R§("VintageCannonballHit01",0.5);
      
      public static const VINTAGE_CANNONBALL_HIT_02:§_-t1R§ = new §_-t1R§("VintageCannonballHit02",0.5);
      
      public static const VINTAGE_CANNONBALL_HIT_03:§_-t1R§ = new §_-t1R§("VintageCannonballHit03",0.5);
      
      public static const §_-B3s§:§_-t1R§ = new §_-t1R§("WoodClubAttack");
      
      public static const ANTIFREEZE:§_-t1R§ = new §_-t1R§("antifreeze");
      
      public static const §_-B3x§:§_-t1R§ = new §_-t1R§("boomerang_attack");
      
      public static const §_-81p§:§_-t1R§ = new §_-t1R§("boomerang_bounce");
      
      public static const §_-D1M§:§_-t1R§ = new §_-t1R§("disc_attack");
      
      public static const §_-E2s§:§_-t1R§ = new §_-t1R§("disc_bounce");
      
      public static const §_-q5§:§_-t1R§ = new §_-t1R§("grenade_jump");
      
      public static const §_-51t§:§_-t1R§ = new §_-t1R§("helicopter_loop");
      
      public static const §_-QH§:§_-t1R§ = new §_-t1R§("helicopter_shot");
      
      public static const §_-51E§:§_-t1R§ = new §_-t1R§("shield_activation");
      
      public static const §_-510§:§_-t1R§ = new §_-t1R§("shield_loop");
      
      public static const §_-m1L§:§_-t1R§ = new §_-t1R§("ufo_loop",0.3);
      
      public static const §_-zr§:§_-t1R§ = new §_-t1R§("ufo_pickup");
      
      public static const §_-G2Y§:§_-t1R§ = new §_-t1R§("move_drop",2);
      
      public static const §_-w2d§:§_-t1R§ = new §_-t1R§("impact");
      
      public static const ANTITOXIN:§_-t1R§ = new §_-t1R§("AntitoxinSound");
      
      public static const BANDAGE:§_-t1R§ = new §_-t1R§("BandageSound");
      
      public static const EMERGENCY_TELEPORT:§_-t1R§ = new §_-t1R§("ExtraTeleportSound");
      
      public static const §_-Lp§:§_-t1R§ = new §_-t1R§("GravigunAttackSound");
      
      public static const §_-H2w§:§_-t1R§ = new §_-t1R§("GravigunExplosion");
      
      public static const §_-21k§:§_-t1R§ = new §_-t1R§("LaserbarActivationSound");
      
      public static const §_-i1Q§:§_-t1R§ = new §_-t1R§("LaserbarBounceSound");
      
      public static const §_-M15§:§_-t1R§ = new §_-t1R§("WeaponCraftSound");
      
      public static const §_-t1u§:§_-t1R§ = new §_-t1R§("WeaponUncraftSound");
      
      public static const §_-71a§:§_-t1R§ = new §_-t1R§("DragonFirewalkSound");
      
      public static const §_-M2r§:§_-t1R§ = new §_-t1R§("DragonFlightSound",0.5);
      
      public static const §_-Mm§:§_-t1R§ = new §_-t1R§("darkknight_aura_off");
      
      public static const §_-j2q§:§_-t1R§ = new §_-t1R§("darkknight_aura_on");
      
      public static const §_-02z§:§_-t1R§ = new §_-t1R§("ParasiteBounce");
      
      public static const §_-H1G§:§_-t1R§ = new §_-t1R§("ParasiteExplosion",0.5);
      
      public static const §_-p20§:§_-t1R§ = new §_-t1R§("ParasiteFire");
      
      public static const §_-T1F§:§_-t1R§ = new §_-t1R§("ParasiteImpact");
      
      public static const §_-j1o§:§_-t1R§ = new §_-t1R§("box_bounce");
      
      public static const §_-824§:§_-t1R§ = new §_-t1R§("AppleTake");
      
      public static const §_-d7§:§_-t1R§ = new §_-t1R§("AppleExplosion");
      
      public static const §_-p1v§:§_-t1R§ = new §_-t1R§("atom_bomb_aim");
      
      public static const §_-3F§:§_-t1R§ = new §_-t1R§("star_collect");
      
      public static const §_-k1s§:§_-t1R§ = new §_-t1R§("soulgun_fire",0.5);
      
      public static const §_-k2k§:§_-t1R§ = new §_-t1R§("soulgun_hit",0.5);
      
      public static const §_-w2a§:Object = {"worm":{
         "damage1":new §_-t1R§("worm.damage1_ru"),
         "damage2":new §_-t1R§("worm.damage2_ru"),
         "damage3":new §_-t1R§("worm.damage3_ru"),
         "damage4":new §_-t1R§("worm.damage4_ru"),
         "speak_cheknutiy":new §_-t1R§("worm.speak_cheknutiy_ru"),
         "speak_granata":new §_-t1R§("worm.speak_granata_ru"),
         "speak_hoho":new §_-t1R§("worm.speak_hoho_ru"),
         "speak_lojis":new §_-t1R§("worm.speak_lojis_ru"),
         "speak_mest":new §_-t1R§("worm.speak_mest_ru"),
         "speak_nashol":new §_-t1R§("worm.speak_nashol_ru"),
         "speak_nepopal":new §_-t1R§("worm.speak_nepopal_ru"),
         "speak_nukanuka":new §_-t1R§("worm.speak_nukanuka_ru"),
         "speak_ogon":new §_-t1R§("worm.speak_ogon_ru"),
         "speak_pervayakrov":new §_-t1R§("worm.speak_pervayakrov_ru"),
         "speak_pogodi":new §_-t1R§("worm.speak_pogodi_ru"),
         "speak_predatel":new §_-t1R§("worm.speak_predatel_ru"),
         "speak_prijok1":new §_-t1R§("worm.speak_prijok1_ru"),
         "speak_prijok2":new §_-t1R§("worm.speak_prijok2_ru"),
         "speak_promazal":new §_-t1R§("worm.speak_promazal_ru"),
         "speak_smeh1":new §_-t1R§("worm.speak_smeh1_ru"),
         "speak_smeh2":new §_-t1R§("worm.speak_smeh2_ru"),
         "speak_smotri":new §_-t1R§("worm.speak_smotri_ru"),
         "speak_ubit":new §_-t1R§("worm.speak_ubit_ru"),
         "speak_vottebe":new §_-t1R§("worm.speak_vottebe_ru"),
         "speak_vpered":new §_-t1R§("worm.speak_vpered_ru"),
         "speak_yessir":new §_-t1R§("worm.speak_yessir_ru"),
         "speak_vboy":new §_-t1R§("worm.speak_vboy_ru"),
         "speak_v_ataku":new §_-t1R§("worm.speak_v_ataku_ru"),
         "speak_komandir":new §_-t1R§("worm.speak_komandir_ru"),
         "speak_jdu_prikaza":new §_-t1R§("worm.speak_jdu_prikaza_ru"),
         "speak_cel_obnarujena":new §_-t1R§("worm.speak_cel_obnarujena_ru"),
         "win":new §_-t1R§("worm.win_ru")
      }};
      
      public static const §_-Fy§:Object = {
         "worm":{
            "damage1":new §_-t1R§("worm.damage1_en"),
            "damage2":new §_-t1R§("worm.damage2_en"),
            "damage3":new §_-t1R§("worm.damage3_en"),
            "damage4":new §_-t1R§("worm.damage4_en"),
            "speak_cheknutiy":new §_-t1R§("worm.speak_cheknutiy_en"),
            "speak_granata":new §_-t1R§("worm.speak_granata_en"),
            "speak_hoho":new §_-t1R§("worm.speak_hoho_en"),
            "speak_lojis":new §_-t1R§("worm.speak_lojis_en"),
            "speak_mest":new §_-t1R§("worm.speak_mest_en"),
            "speak_nashol":new §_-t1R§("worm.speak_nashol_en"),
            "speak_nepopal":new §_-t1R§("worm.speak_nepopal_en"),
            "speak_nukanuka":new §_-t1R§("worm.speak_nukanuka_en"),
            "speak_ogon":new §_-t1R§("worm.speak_ogon_en"),
            "speak_pervayakrov":new §_-t1R§("worm.speak_pervayakrov_en"),
            "speak_pogodi":new §_-t1R§("worm.speak_pogodi_en"),
            "speak_predatel":new §_-t1R§("worm.speak_predatel_en"),
            "speak_prijok1":new §_-t1R§("worm.speak_prijok1_en"),
            "speak_prijok2":new §_-t1R§("worm.speak_prijok2_en"),
            "speak_promazal":new §_-t1R§("worm.speak_promazal_en"),
            "speak_smeh1":new §_-t1R§("worm.speak_smeh1_en"),
            "speak_smeh2":new §_-t1R§("worm.speak_smeh2_en"),
            "speak_smotri":new §_-t1R§("worm.speak_smotri_en"),
            "speak_ubit":new §_-t1R§("worm.speak_ubit_en"),
            "speak_vottebe":new §_-t1R§("worm.speak_vottebe_en"),
            "speak_vpered":new §_-t1R§("worm.speak_vpered_en"),
            "speak_yessir":new §_-t1R§("worm.speak_yessir_en"),
            "speak_vboy":new §_-t1R§("worm.speak_vboy_en"),
            "speak_v_ataku":new §_-t1R§("worm.speak_v_ataku_en"),
            "speak_komandir":new §_-t1R§("worm.speak_komandir_en"),
            "speak_jdu_prikaza":new §_-t1R§("worm.speak_jdu_prikaza_en"),
            "speak_cel_obnarujena":new §_-t1R§("worm.speak_cel_obnarujena_en"),
            "win":new §_-t1R§("worm.win_en")
         },
         "boxer":{
            "damage1":new §_-t1R§("boxer.damage1_en"),
            "damage2":new §_-t1R§("boxer.damage2_en"),
            "damage3":new §_-t1R§("boxer.damage3_en"),
            "damage4":new §_-t1R§("boxer.damage4_en"),
            "speak_cel_obnarujena":new §_-t1R§("boxer.speak_cel_obnarujena_en"),
            "speak_cheknutiy":new §_-t1R§("boxer.speak_cheknutiy_en"),
            "speak_granata":new §_-t1R§("boxer.speak_granata_en"),
            "speak_hoho":new §_-t1R§("boxer.speak_hoho_en"),
            "speak_jdu_prikaza":new §_-t1R§("boxer.speak_jdu_prikaza_en"),
            "speak_komandir":new §_-t1R§("boxer.speak_komandir_en"),
            "speak_lojis":new §_-t1R§("boxer.speak_lojis_en"),
            "speak_mest":new §_-t1R§("boxer.speak_mest_en"),
            "speak_nashol":new §_-t1R§("boxer.speak_nashol_en"),
            "speak_nepopal":new §_-t1R§("boxer.speak_nepopal_en"),
            "speak_nukanuka":new §_-t1R§("boxer.speak_nukanuka_en"),
            "speak_ogon":new §_-t1R§("boxer.speak_ogon_en"),
            "speak_pervayakrov":new §_-t1R§("boxer.speak_pervayakrov_en"),
            "speak_pobeda":new §_-t1R§("boxer.speak_pobeda_en"),
            "speak_pogodi":new §_-t1R§("boxer.speak_pogodi_en"),
            "speak_predatel":new §_-t1R§("boxer.speak_predatel_en"),
            "speak_prijok1":new §_-t1R§("boxer.speak_prijok1_en"),
            "speak_prijok2":new §_-t1R§("boxer.speak_prijok2_en"),
            "speak_promazal":new §_-t1R§("boxer.speak_promazal_en"),
            "speak_smeh1":new §_-t1R§("boxer.speak_smeh1_en"),
            "speak_smeh2":new §_-t1R§("boxer.speak_smeh2_en"),
            "speak_smotri":new §_-t1R§("boxer.speak_smotri_en"),
            "speak_ubit":new §_-t1R§("boxer.speak_ubit_en"),
            "speak_v_ataku":new §_-t1R§("boxer.speak_v_ataku_en"),
            "speak_vboy":new §_-t1R§("boxer.speak_vboy_en"),
            "speak_vottebe":new §_-t1R§("boxer.speak_vottebe_en"),
            "speak_vpered":new §_-t1R§("boxer.speak_vpered_en"),
            "win":new §_-t1R§("boxer.win_boxer_en")
         }
      };
      
      public function §_-d27§()
      {
         super();
      }
      
      public static function §_-k4§(param1:Array) : §_-t1R§
      {
         return param1[Math.floor(Math.random() * param1.length)];
      }
      
      public static function §_-dI§() : §_-t1R§
      {
         return §_-k4§(§_-8M§);
      }
   }
}

