package ru.pragmatix.flox.social.response.photo
{
   import §_-C3f§.*;
   import ru.pragmatix.flox.social.response.SocialResponse;
   
   public class PhotoSaveResponse extends SocialResponse
   {
      
      public var photoUrl:String;
      
      public function PhotoSaveResponse(param1:Boolean)
      {
         super(null,"PHOTO.SAVE",param1);
      }
   }
}

