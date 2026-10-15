package ru.pragmatix.flox.social.post
{
   import flash.display.DisplayObject;
   
   public interface IWallPoster extends IPoster
   {
      
      function wallPost(param1:String, param2:String, param3:String = null, param4:Function = null) : void;
      
      function wallPostLoadedImg(param1:String, param2:String, param3:DisplayObject, param4:Function = null) : void;
   }
}

