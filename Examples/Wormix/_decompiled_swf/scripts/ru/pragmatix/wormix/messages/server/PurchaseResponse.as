package ru.pragmatix.wormix.messages.server
{
   import §_-H16§.*;
   import §_-H2g§.§_-di§;
   import §_-H2g§.§_-s1d§;
   import §_-b1F§.§_-G4§;
   import §_-j1w§.§_-B2i§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-q11§.§_-C2n§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Zd§;
   import com.greensock.loading.core.*;
   import com.hurlant.math.*;
   import dragonBones.animation.WorldClock;
   import feathers.data.IListCollection;
   import feathers.layout.RelativePosition;
   import flash.geom.Point;
   import flash.net.URLRequest;
   import flash.net.URLVariables;
   import flash.utils.getDefinitionByName;
   import org.swiftsuspenders.Injector;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.game.ai.§_-de§;
   import ru.pragmatix.wormix.gui.*;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.ammo.phase.PhaseAmmo;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   use namespace bi_internal;
   
   public class PurchaseResponse
   {
      
      public var result:int;
      
      public var cost:Array;
      
      public var sessionKey:String;
      
      public var isSecure:Boolean;
      
      public function PurchaseResponse()
      {
         super();
      }
   }
}

