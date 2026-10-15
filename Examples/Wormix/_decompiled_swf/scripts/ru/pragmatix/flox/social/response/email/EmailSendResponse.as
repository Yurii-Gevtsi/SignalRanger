package ru.pragmatix.flox.social.response.email
{
   import §_-62§.§_-zF§;
   import com.greensock.*;
   import com.greensock.core.*;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import starling.events.Event;
   
   public class EmailSendResponse
   {
      
      public var success:Boolean;
      
      public var errorMsg:String;
      
      public function EmailSendResponse(param1:Boolean, param2:String = "")
      {
         super();
         this.success = param1;
         this.errorMsg = param2;
      }
   }
}

