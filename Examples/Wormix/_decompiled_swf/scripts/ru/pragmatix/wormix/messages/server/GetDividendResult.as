package ru.pragmatix.wormix.messages.server
{
   import §_-73I§.§_-q15§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-F2Q§.GestureEvent;
   import §_-G2H§.*;
   import §_-RE§.§_-D3v§;
   import §_-RE§.§_-r2q§;
   import §_-RE§.§_-w0§;
   import §_-U1i§.§_-J1§;
   import §_-U1i§.§_-b1k§;
   import §_-f1G§.§_-kf§;
   import §_-fo§.§_-21L§;
   import §_-fo§.§_-r5§;
   import §_-l1K§.§_-cZ§;
   import §_-l2r§.§_-K1t§;
   import §_-qH§.§_-AC§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import §_-z1g§.*;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import dragonBones.animation.WorldClock;
   import flash.display.*;
   import flash.events.*;
   import flash.geom.Point;
   import flash.utils.*;
   import org.gestouch.core.GestureState;
   import ru.pragmatix.wormix.controller.*;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.utils.Result;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.utils.*;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.display.Sprite;
   import starling.utils.Pool;
   
   public class GetDividendResult
   {
      
      public var result:uint;
      
      public var money:int;
      
      public var realMoney:int;
      
      public var sessionKey:String;
      
      public function GetDividendResult()
      {
         super();
      }
      
      public function toString() : String
      {
         return "GetDividendResult{" + "\n" + "result = " + Result.toString(this.result) + "\n" + "money = " + this.money + "\n" + "realMoney = " + this.realMoney + "\n" + "sessionKey = " + this.sessionKey + "}";
      }
   }
}

