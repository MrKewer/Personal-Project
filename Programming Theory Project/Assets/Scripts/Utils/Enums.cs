using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
    public enum DamageType { Collision, Bite, Poison, Fire, Explosion, NumberOfTypes };
    //public enum LargeParticles { Yellow, Purple, Red, Blue, Green, NumberOfTypes };
    //public enum SmallParticles { Yellow, Purple, Red, Blue, Green, NumberOfTypes};

    public enum Particles { YellowSmall, WhiteSmall, GraySmall, WhiteLarge, GrayLarge, PurpleSmall, RedSmall, BlueSmall, GreenSmall, YellowLarge, PurpleLarge, RedLarge, BlueLarge, GreenLarge, NumberOfTypes };
    
    public enum Pickups { Heal, Invulnerability, Ball, FlameThrower, Coin, Bomb, DoubleCoins, NumberOfTypes }
}
