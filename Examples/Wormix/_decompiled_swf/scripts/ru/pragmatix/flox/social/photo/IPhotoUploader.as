package ru.pragmatix.flox.social.photo
{
   import flash.display.BitmapData;
   
   public interface IPhotoUploader
   {
      
      function savePhoto(param1:BitmapData, param2:String, param3:Function) : void;
   }
}

