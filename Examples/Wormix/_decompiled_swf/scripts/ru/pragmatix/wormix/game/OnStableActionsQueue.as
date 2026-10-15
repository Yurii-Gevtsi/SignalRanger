package ru.pragmatix.wormix.game
{
   import §_-51W§.§_-G2h§;
   import §_-82a§.*;
   import §_-92a§.*;
   import §_-B1y§.§_-F1P§;
   import §_-Cl§.*;
   import §_-DJ§.§_-fH§;
   import §_-G16§.*;
   import §_-L1H§.§_-C3e§;
   import §_-P2i§.§_-b7§;
   import §_-Z1K§.§_-q24§;
   import §_-b1F§.§_-G4§;
   import §_-f1G§.§_-kf§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-U2p§;
   import §_-r1C§.§_-t1R§;
   import §_-w2i§.§_-Ia§;
   import §_-z1g§.§_-C32§;
   import §_-zI§.§_-sI§;
   import config.§_-vR§;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.events.FeathersEventType;
   import flash.events.Event;
   import flash.events.IEventDispatcher;
   import flash.geom.Rectangle;
   import flash.media.SoundTransform;
   import flash.utils.getTimer;
   import org.gestouch.core.*;
   import ru.pragmatix.flox.social.controller.SocialType;
   import ru.pragmatix.wormix.Scene;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.ClanMessages;
   import ru.pragmatix.wormix.messages.clans.ClanServiceResult;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.scene.tile.TileData;
   import ru.pragmatix.wormix.utils.*;
   import ru.pragmatix.wormix.weapons.ammo.type.grenade.cluster.ShellBurst;
   import starling.events.Event;
   
   use namespace gestouch_internal;
   
   public class OnStableActionsQueue
   {
      
      private var actions:Vector.<ActionData>;
      
      public function OnStableActionsQueue()
      {
         super();
         this.actions = new Vector.<ActionData>();
      }
      
      public function add(param1:Function, param2:Array) : Boolean
      {
         var _loc4_:ActionData = null;
         var _loc5_:Boolean = false;
         var _loc3_:Scene = §_-X1j§.§_-12n§.scene;
         if(!_loc3_.gameObjectProcessor.§_-C6§())
         {
            _loc4_ = new ActionData(param1,param2);
            _loc5_ = _loc3_.gameObjectProcessor.§_-O2P§();
            if(_loc5_)
            {
               this.§_-sv§(_loc4_);
            }
            else
            {
               if(this.actions.length == 0)
               {
                  _loc3_.addEventListener(§_-L3Q§.SCENE_STABLE,this.§_-kw§);
               }
               this.actions.push(_loc4_);
            }
            return _loc5_;
         }
         return false;
      }
      
      private function §_-kw§(param1:starling.events.Event) : void
      {
         var _loc2_:Scene = null;
         this.§_-sv§(this.actions.shift());
         if(this.actions.length == 0)
         {
            _loc2_ = §_-X1j§.§_-12n§.scene;
            _loc2_.removeEventListener(§_-L3Q§.SCENE_STABLE,this.§_-kw§);
         }
      }
      
      public function dispose() : void
      {
         var _loc1_:Scene = null;
         if(this.actions.length > 0)
         {
            _loc1_ = §_-X1j§.§_-12n§.scene;
            _loc1_.removeEventListener(§_-L3Q§.SCENE_STABLE,this.§_-kw§);
            this.actions.splice(0,this.actions.length);
         }
      }
      
      private function §_-sv§(param1:ActionData) : void
      {
         if(Boolean(param1.params.length) && param1.action != null)
         {
            param1.action.apply(null,param1.params);
         }
         else
         {
            param1.action();
         }
      }
   }
}

class ActionData
{
   
   public var action:Function;
   
   public var params:Array;
   
   public function ActionData(param1:Function, param2:Array)
   {
      super();
      this.action = param1;
      this.params = param2;
   }
}
