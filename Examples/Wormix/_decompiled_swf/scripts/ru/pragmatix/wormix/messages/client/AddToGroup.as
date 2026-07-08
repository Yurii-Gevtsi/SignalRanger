package ru.pragmatix.wormix.messages.client
{
   import §_-12W§.*;
   import §_-P2i§.§_-G3J§;
   import §_-Tm§.§_-RF§;
   import §_-Vu§.§_-ml§;
   import §_-Y1O§.*;
   import §_-hv§.*;
   import §_-jF§.*;
   import feathers.controls.List;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.core.ITextRenderer;
   import flash.geom.Point;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.controls.Command;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.§_-R2g§;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.interfaces.§_-O1c§;
   import starling.animation.Tween;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.events.Touch;
   import starling.events.TouchEvent;
   import starling.events.TouchPhase;
   
   public class AddToGroup
   {
      
      public var teamMemberId:int;
      
      public var moneyType:int;
      
      public var teamMemberType:int;
      
      public var prevTeamMemberId:int;
      
      public var active:Boolean = true;
      
      public function AddToGroup(param1:uint = 0, param2:int = 0, param3:int = 1, param4:int = 0)
      {
         super();
         this.teamMemberId = param1;
         this.moneyType = param2;
         this.teamMemberType = param3;
         this.prevTeamMemberId = param4;
      }
   }
}

