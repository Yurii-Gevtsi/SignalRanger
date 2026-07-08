package ru.pragmatix.flox.social.controller
{
   import §_-22z§.§_-f1b§;
   import §_-51j§.*;
   import §_-62§.§_-zF§;
   import §_-A2U§.*;
   import §_-B1y§.§_-o2k§;
   import §_-C3f§.*;
   import §_-H16§.*;
   import §_-b1F§.§_-G4§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-w§.§_-63m§;
   import §_-w2i§.§_-Zd§;
   import §_-y1s§.§_-p6§;
   import com.greensock.loading.core.*;
   import feathers.controls.ImageLoader;
   import feathers.controls.List;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.text.TextInstanceCreator;
   import feathers.core.ITextRenderer;
   import feathers.data.ListCollection;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.VerticalAlign;
   import feathers.themes.FontColor;
   import feathers.themes.FontSize;
   import flash.errors.IllegalOperationError;
   import flash.net.URLRequest;
   import flash.net.URLVariables;
   import flash.text.TextFormatAlign;
   import flash.utils.Dictionary;
   import flash.utils.getDefinitionByName;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.screenOverlay.view.§_-B1m§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.weapons.*;
   import ru.pragmatix.wormix.weapons.ammo.Bullet;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import starling.core.Starling;
   import starling.events.Event;
   import starling.events.KeyboardEvent;
   import starling.utils.ScaleMode;
   
   public class AbstractAuthorizeSocialController extends AbstractSocialController implements IAuthorizeSocialController
   {
      
      public function AbstractAuthorizeSocialController(param1:§_-o2k§)
      {
         super(param1);
      }
      
      public function login(param1:Function) : void
      {
         throw new IllegalOperationError();
      }
      
      public function logout() : void
      {
         throw new IllegalOperationError();
      }
      
      public function isLoggedIn() : Boolean
      {
         throw new IllegalOperationError();
      }
      
      public function dispose() : void
      {
         throw new IllegalOperationError();
      }
   }
}

