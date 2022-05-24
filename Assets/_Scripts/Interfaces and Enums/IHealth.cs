using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeenieWalker
{
    public interface IHealth
    {
        int CurrentHealth { get;}
        int MaxHealth { get; }

        void TakeDamage(int loss);
    }
}
