package ru.pragmatix.wormix.weapons
{
   import §_-73d§.§_-tf§;
   import §_-82l§.§_-A2Y§;
   import §_-CR§.§_-72p§;
   import §_-Z1K§.§_-q24§;
   import §_-g2C§.GlobalChatTextMessageItemRenderer;
   import §_-g2C§.§_-62U§;
   import §_-g2C§.§_-I1R§;
   import §_-g2C§.§_-K2A§;
   import §_-g2C§.§_-K2u§;
   import §_-g2C§.§_-p8§;
   import §_-g2C§.§_-w1K§;
   import §_-l1K§.§_-d2j§;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.renderers.IListItemRenderer;
   import feathers.layout.AnchorLayout;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.Direction;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateScrollBarStyles;
   import feathers.utils.textures.TextureCache;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.utils.§_-g20§;
   import ru.pragmatix.wormix.weapons.ammo.DrillMissile;
   import ru.pragmatix.wormix.weapons.ammo.type.shuriken.Shuriken;
   import starling.events.Event;
   
   public class AssassinHeavyKnifeWeapon extends Weapon
   {
      
      public function AssassinHeavyKnifeWeapon()
      {
         super();
         MIN_POWER = 15;
         MAX_POWER = 45;
         §_-z1W§(new §_-d2j§());
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         §_-Y2U§ = new §_-tf§(50,true);
         super.§_-z2Y§(param1);
         paramSet = §_-73v§.getWeapon(§_-q24§.§_-I2I§).§_-Q2H§().clone();
      }
      
      override protected function shoot() : void
      {
         var _loc1_:DrillMissile = new DrillMissile(owner,paramSet,shotParams.angle,§_-Yz§(shotParams.power));
         _loc1_.setDrillParams(8,12);
         _loc1_.setSounds(§_-d27§.§_-k4§(Shuriken.groundHitSounds),§_-d27§.§_-k4§(Shuriken.wormHitSounds));
         detach();
         nextTurnOnStable();
      }
   }
}

