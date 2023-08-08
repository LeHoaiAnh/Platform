using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsController
{
   private static string firstLogIn = "FirstLogIn";

   public static string GetFirstLogIn()
   {
      return firstLogIn;
   }
}
