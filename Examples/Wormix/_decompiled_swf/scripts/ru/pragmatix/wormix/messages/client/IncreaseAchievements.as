package ru.pragmatix.wormix.messages.client
{
   import §_-63U§.*;
   import §_-CF§.§_-E19§;
   import §_-Cl§.*;
   import §_-D3A§.§_-TZ§;
   import §_-DJ§.MessageBox;
   import §_-DJ§.§_-fH§;
   import §_-R2I§.§_-H7§;
   import §_-R2I§.§_-u2V§;
   import §_-Y1O§.§_-V23§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import §_-z1g§.§_-82X§;
   import com.distriqt.extension.inappbilling.Product;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.distriqt.extension.inappbilling.events.PurchaseEvent;
   import com.worlize.websocket.§_-Y2T§;
   import com.worlize.websocket.§_-ue§;
   import dragonBones.animation.WorldClock;
   import ru.pragmatix.flox.social.controller.mobile.*;
   import ru.pragmatix.flox.social.response.LoadUserProfilesResponse;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObject;
   import starling.display.DisplayObjectContainer;
   import starling.events.Event;
   
   public class IncreaseAchievements
   {
      
      public var sessionId:String;
      
      public var achievementsIndex:Array;
      
      public var achievementsRise:Array;
      
      public var boolAchievements:Array;
      
      public var timeScale:uint;
      
      public var isSecure:Boolean;
      
      public function IncreaseAchievements()
      {
         super();
      }
      
      public function toString() : String
      {
         return "IncreaseAchievements{" + "ids=" + this.achievementsIndex + ",values=" + this.achievementsRise + ",bools=" + this.boolAchievements + ",timeScale=" + this.timeScale + ",sessionId=" + this.sessionId + "}";
      }
   }
}

