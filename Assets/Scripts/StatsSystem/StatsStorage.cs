using UnityEngine;
using System.Collections.Generic;

namespace StatsSystem
{
    [CreateAssetMenu(fileName ="StatsStorage", menuName ="Stats/StatsStorage")]
    public class StatsStorage:ScriptableObject
    {
        [field: SerializeField] public List<Stat> Stats { get; private set; }
    }
}
