using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum DisplayLayer
{
  
  Background=0,
  Bullet=-50,
  Character=-100,
  Joystick=-500,

}

public static class Utility
{
    public static int WindowWidth=0;
    public static int WindowHeight=0;
    public static int WindowStartHeight=0;
    public static int ScreenWidth=0;
    public static int ScreenHeight=0;
    public  const int PCWindowHeight=1080;
    public  const int PCWindowStartHeight=2560;
    public  const int PhoneWindowHeight=1920;
    public const int PhoneWindowStartHeight=3840;
    public static bool IsPC=false;
    public static void InitScreen()
   {
      ScreenWidth=Screen.width;
      ScreenHeight=Screen.height;
      if(Screen.width<Screen.height)
      {
        WindowHeight=PhoneWindowHeight;
        WindowStartHeight=PhoneWindowStartHeight;
        IsPC=false;
      }
      else
      {
        WindowHeight=PCWindowHeight;
        WindowStartHeight=PCWindowStartHeight;
        IsPC=true;
      }
      WindowWidth=(int)((float)WindowHeight/ScreenHeight*ScreenWidth);
   }
}