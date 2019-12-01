#region Copyright
/// Copyright © 2017 Vlad Kirpichenko
/// 
/// Author: Vlad Kirpichenko 'itanksp@gmail.com'
/// Licensed under the MIT License.
/// License: http://opensource.org/licenses/MIT
#endregion

using System.Collections;
using UnityEngine;
using SaveState;

namespace SimpleKeplerOrbits
{
    /// <summary>
    /// Component for moving game object in eliptic or hyperbolic path around attractor body.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [ExecuteInEditMode]
    public class KeplerOrbitMover : MonoBehaviour
    {
        /// <summary>
        /// The attractor settings data.
        /// Attractor object reference must be assigned or orbit mover will not work.
        /// </summary>
        // public AttractorData AttractorSettings = new AttractorData();
         public AttractorData AttractorSettings;

        /// <summary>
        /// The velocity handle object.
        /// Assign object and use it as velocity control handle in scene view.
        /// </summary>
        [Tooltip("The velocity handle object. Assign object and use it as velocity control handle in scene view.")]
        public Transform VelocityHandle;

        /// <summary>
        /// The time scale multiplier.
        /// </summary>
        public float TimeScale = 1f;

        /// <summary>
        /// The orbit data.
        /// Internal state of orbit.
        /// </summary>
        [Header("Orbit state details:")]
        public KeplerOrbitData OrbitData /*= new KeplerOrbitData()*/;

        /// <summary>
        /// Disable continious editing orbit in update loop, if you don't need it.
        /// </summary>
        public bool LockOrbitEditing = false;
        public bool isNotPlanet = false; //Inspector

        private OrbitalManeuver orbitalManeuver = null;
        private string attractorObjectName, velocityHandleName;


        [Header("Static instance for calculating orbital rendezvous, object will not move along its orbit")]
        public bool isStaticInstance;
        public bool initializeLoadData;

        //public KeplerOrbitMover(AttractorData attractorData, Transform velocityHandle, KeplerOrbitData orbitData, bool isNotPlanet)
        //{
        //    AttractorSettings = attractorData;
        //    VelocityHandle = velocityHandle;
        //    this.OrbitData = orbitData;
        //    this.isNotPlanet = isNotPlanet;
        //}


#if UNITY_EDITOR
        /// <summary>
        /// The debug error displayed flag.
        /// Used to avoid errors spamming.
        /// </summary>
        private bool _debugErrorDisplayed = false;
#endif

        private bool IsReferencesAsigned
        {
            get
            {
                return AttractorSettings != null && AttractorSettings.AttractorObject != null;
            }
        }


        private void OnApplicationQuit()
        {
            OrbitalData.SaveOrbitData(OrbitData, attractorObjectName, VelocityHandle.name, TimeScale, isNotPlanet, gameObject.name);
        }

        private void OnEnable()
        {
            //ForceUpdateOrbitData();
            if (isNotPlanet == true)
                orbitalManeuver = GameObject.FindObjectOfType<OrbitalManeuver>();

            //if (OrbitalData.SavedDataDictionary != null)
            //    initializeLoadData = true;
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            if(!isStaticInstance)
                StartCoroutine(OrbitUpdateLoop());                                   //Enalbe
        }

        private void Start()
        {
            /// <summary>
            /// Store attractor data as a name because attractor class is of type MonoBehaviour
            /// </summary>

            attractorObjectName = AttractorSettings.AttractorObject.name;

        }

        /// <summary>
        /// Updates orbit internal data.
        /// </summary>
        /// <remarks>
        /// In this method orbit data is updating from view state:
        /// If you change body position, attractor mass or any other vital orbit parameter, 
        /// this change will be noticed and applyed to internal OrbitData state in this method.
        /// If you need to change orbitData state directly, by script, you need to change OrbitData state and then call ForceUpdateOrbitData
        /// </remarks>
        private void Update()
        {
            if (IsReferencesAsigned)
            {
                if (!LockOrbitEditing)
                {
                     var position = new Vector3d(transform.position - AttractorSettings.AttractorObject.position);

                    var velocity = VelocityHandle == null ? new Vector3d() : new Vector3d(VelocityHandle.position - transform.position);
                    if ((Vector3)position != (Vector3)OrbitData.Position ||
                        (VelocityHandle != null && (Vector3)velocity != (Vector3)OrbitData.Velocity) ||
                        OrbitData.GravConst != AttractorSettings.GravityConstant ||
                        OrbitData.AttractorMass != AttractorSettings.AttractorMass)
                    {
                        ForceUpdateOrbitData();
                    }
                }
            }
            else
            {
#if UNITY_EDITOR
                if (AttractorSettings.AttractorObject == null)
                {
                    if (!_debugErrorDisplayed)
                    {
                        _debugErrorDisplayed = true;
                        if (Application.isPlaying)
                        {
                            Debug.LogError("KeplerMover: Attractor reference not asigned", context: gameObject);
                        }
                        else
                        {
                            Debug.Log("KeplerMover: Attractor reference not asigned", context: gameObject);
                        }
                    }
                }
                else
                {
                    _debugErrorDisplayed = false;
                }
#endif
            }
        }


        /// <summary>
        /// Progress orbit path motion.
        /// Actual kepler orbiting is processed here.
        /// </summary>
        /// <remarks>
        /// Orbit motion progress calculations must be placed after Update, so orbit parameters changes can be applyed,
        /// but before LateUpdate, so orbit can be displayed in same frame.
        /// Coroutine loop is best candidate for achieving this.
        /// </remarks>
        private IEnumerator OrbitUpdateLoop()
        {
            while (true)
            {
                if (IsReferencesAsigned)
                {
                    if (!OrbitData.IsValidOrbit)
                    {
                        //try to fix orbit if we can.
                        OrbitData.CalculateNewOrbitData();
                    }

                    if (OrbitData.IsValidOrbit)
                    {
                        OrbitData.UpdateOrbitDataByTime(Time.deltaTime * TimeScale);

                        transform.position = AttractorSettings.AttractorObject.position + (Vector3)OrbitData.Position;
                        //this.GetComponent<Rigidbody>().AddForce(AttractorSettings.AttractorObject.position + (Vector3)OrbitData.Position);
                        if (VelocityHandle != null)
                        {
                            VelocityHandle.position = transform.position + (Vector3)OrbitData.Velocity;
                            if(isNotPlanet == true)
                                orbitalManeuver.PointVelocityVectorToTarget(VelocityHandle.gameObject);
                        }
                    }
                }
                yield return null;
            }
        }

        /// <summary>
        /// Updates OrbitData from new body position and velocity vectors.
        /// </summary>
        /// <param name="relativePosition">The relative position.</param>
        /// <param name="velocity">The relative velocity.</param>
        /// <remarks>
        /// This method can be useful to assign new position of body by script.
        /// Or you can directly change OrbitData state and then manually update view.
        /// </remarks>
        public void CreateNewOrbitFromPositionAndVelocity(Vector3 relativePosition, Vector3 velocity)
        {
            if (IsReferencesAsigned)
            {
                OrbitData.Position = new Vector3d(relativePosition);
                OrbitData.Velocity = new Vector3d(velocity);
                OrbitData.CalculateNewOrbitData();
                ForceUpdateViewFromInternalState();
            }
        }

        /// <summary>
        /// Forces the update of body position, and velocity handler from OrbitData.
        /// Call this method after any direct changing of OrbitData.
        /// </summary>
        public void ForceUpdateViewFromInternalState()
        {
            transform.position = AttractorSettings.AttractorObject.position + (Vector3)OrbitData.Position;
            if (VelocityHandle != null)
            {
                VelocityHandle.position = transform.position + (Vector3)OrbitData.Velocity;
            }
        }


        /// <summary>
        /// Forces the update of internal orbit data from current world positions of body, attractor settings and velocityHandle.
        /// </summary>
        /// <remarks>
        /// This method must be called after any manual changing of body position, velocity handler position or attractor settings.
        /// It will update internal OrbitData state from view state.
        /// </remarks>
        public void ForceUpdateOrbitData()
        {
            if (IsReferencesAsigned && initializeLoadData == false)
            {
                OrbitData.AttractorMass = AttractorSettings.AttractorMass;
                OrbitData.GravConst = AttractorSettings.GravityConstant;
                OrbitData.Position = new Vector3d(transform.position - AttractorSettings.AttractorObject.position);
                if (VelocityHandle != null)
                {
                    OrbitData.Velocity = new Vector3d((VelocityHandle.position - transform.position));
                }
                OrbitData.CalculateNewOrbitData();
            }

            if(IsReferencesAsigned && initializeLoadData == true)
            {
                OrbitalData.PushToList(gameObject.name);

                OrbitData = (KeplerOrbitData)OrbitalData.SavedDataDictionary["Orbit"];

                GameObject tempAttractorObject = GameObject.Find((string)OrbitalData.SavedDataDictionary["Attractor"]);
                AttractorSettings =  tempAttractorObject.GetComponent<AttractorData>();

                GameObject tempVelocityHandle = GameObject.Find((string)OrbitalData.SavedDataDictionary["Velocity"]);
                VelocityHandle = tempVelocityHandle.transform;

                TimeScale = (float)OrbitalData.SavedDataDictionary["TimeScale"];
                isNotPlanet = (bool)OrbitalData.SavedDataDictionary["PlanetBool"];
                OrbitData.CalculateNewOrbitData();
                initializeLoadData = false;

                Debug.Log("Data assignment completed, loading sequence done");
            }
        }

        /// <summary>
        /// Change orbit velocity vector to match circular orbit.
        /// </summary>
        [ContextMenu("Circulize orbit")]
        public void SetAutoCircleOrbit()
        {
            if (IsReferencesAsigned)
            {
                OrbitData.Velocity = KeplerOrbitUtils.CalcCircleOrbitVelocity(Vector3d.zero, OrbitData.Position, OrbitData.AttractorMass, 1f, OrbitData.OrbitNormal, OrbitData.GravConst);
                OrbitData.CalculateNewOrbitData();
                if (VelocityHandle != null)
                {
                    VelocityHandle.position = transform.position + (Vector3)OrbitData.Velocity;
                }
            }
        }
    }
}