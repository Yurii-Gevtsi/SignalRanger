package ru.pragmatix.wormix.utils
{
   import §_-73d§.§_-h2X§;
   import §_-Z1K§.§_-q24§;
   import §_-bI§.§_-d2V§;
   import com.distriqt.extension.inappbilling.Purchase;
   import com.greensock.loading.LoaderStatus;
   import com.greensock.loading.core.*;
   import feathers.controls.ScrollPolicy;
   import feathers.layout.AnchorLayout;
   import flash.display.Loader;
   import flash.events.Event;
   import flash.events.ProgressEvent;
   import flash.events.TimerEvent;
   import flash.utils.Timer;
   import flash.utils.getQualifiedClassName;
   import ru.pragmatix.flox.gui.controls.§_-a1t§;
   import ru.pragmatix.wormix.gui.screenOverlay.view.*;
   import ru.pragmatix.wormix.messages.clans.request.AuditActionsRequest;
   import ru.pragmatix.wormix.messages.clans.response.AuditActionsResponse;
   import ru.pragmatix.wormix.messages.client.AppleVerifyReceipt;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.weapons.§_-73v§;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.ScaleMode;
   import starling.utils.SystemUtil;
   
   public class §_-J2h§
   {
      
      public function §_-J2h§()
      {
         super();
      }
      
      public static function §_-n1V§(... rest) : void
      {
      }
      
      public static function §_-UR§(param1:String, param2:String) : String
      {
         return "<font color=\"" + param2 + "\">" + param1 + "</font>";
      }
      
      public static function §_-j7§(param1:String) : String
      {
         param1 = §_-KY§(param1,"<","&lt;");
         return §_-KY§(param1,">","&gt;");
      }
      
      private static function §_-KY§(param1:String, param2:String, param3:String) : String
      {
         while(param1.indexOf(param2) >= 0)
         {
            param1 = param1.replace(param2,param3);
         }
         return param1;
      }
      
      public static function §_-j2e§(param1:§_-a1t§, param2:§_-a1t§) : void
      {
         if(param1.isVisible() && !param2.isVisible())
         {
            param1.x += (param2.x - param1.x) / 2;
         }
         if(!param1.isVisible() && param2.isVisible())
         {
            param2.x += (param1.x - param2.x) / 2;
         }
      }
      
      public static function §_-i1z§(param1:Object, param2:Object) : void
      {
         if(Boolean(param1.visible) && !param2.visible)
         {
            param1.x += (param2.x - param1.x) / 2;
         }
         if(!param1.visible && Boolean(param2.visible))
         {
            param2.x += (param1.x - param2.x) / 2;
         }
      }
      
      public static function §_-v2y§(param1:DisplayObject, param2:Array = null) : String
      {
         var _loc6_:String = null;
         var _loc7_:int = 0;
         var _loc8_:String = null;
         var _loc9_:String = null;
         var _loc10_:int = 0;
         var _loc11_:String = null;
         if(!param1)
         {
            return "Null";
         }
         var _loc3_:String = "";
         var _loc4_:DisplayObject = param1;
         var _loc5_:int = param2 ? int(param2.length) : 0;
         while(_loc4_)
         {
            _loc6_ = getQualifiedClassName(_loc4_);
            _loc7_ = _loc6_.indexOf("::");
            _loc8_ = _loc7_ >= 0 ? _loc6_.substr(_loc7_ + 2) : _loc6_;
            _loc9_ = "";
            if(param2)
            {
               _loc9_ = " (";
               _loc10_ = 0;
               while(_loc10_ < _loc5_)
               {
                  _loc11_ = param2[_loc10_] as String;
                  if((Boolean(_loc11_)) && _loc4_.hasOwnProperty(_loc11_))
                  {
                     if(_loc10_)
                     {
                        _loc9_ += ", ";
                     }
                     _loc9_ += _loc11_ + ":" + _loc4_[_loc11_];
                  }
                  _loc10_++;
               }
               _loc9_ += ")";
            }
            _loc3_ = "/" + _loc8_ + _loc9_ + _loc3_;
            _loc4_ = _loc4_.parent;
         }
         return _loc3_;
      }
   }
}

