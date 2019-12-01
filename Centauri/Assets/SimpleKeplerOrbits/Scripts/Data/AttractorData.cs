#region Copyright
/// Copyright © 2017 Vlad Kirpichenko
/// 
/// Author: Vlad Kirpichenko 'itanksp@gmail.com'
/// Licensed under the MIT License.
/// License: http://opensource.org/licenses/MIT
#endregion

using UnityEngine;

namespace SimpleKeplerOrbits
{
    /// <summary>
    /// Attractor data, necessary for calculation orbit.
    /// </summary>
    public class AttractorData : MonoBehaviour
    {
        public Transform AttractorObject;
        public float AttractorMass = 1000;
        public float MaxDistForHyperbolicCase = 100f;
        public float GravityConstant = 0.1f;
    
    }

    
}