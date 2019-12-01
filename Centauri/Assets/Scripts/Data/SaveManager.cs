using SimpleKeplerOrbits;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SaveState
{
    public class OrbitalData
    {
        public static KeplerOrbitData SavedOrbitData { get; set; }
        public static string AttractorName{ get; set; }
        public static string VelocityHandle { get; set; }
        public static bool IsNotPlanet { get; set; }
        public static float TimeScale { get; set; }
        public static bool IsNotPlanetBool{ get; set; }

        public static Dictionary<string, object> SavedDataDictionary
        {
            get
            {
                return savedDataDictionary;
            }

            set
            {
                savedDataDictionary = value;
            }
        }
        public static Dictionary<string, Dictionary<string, object>> SavedGameObject
        {
            get
            {
                return savedGameObject;
            }

            set
            {
                savedGameObject = value;
            }
        }

        [SerializeField]
        private static Dictionary<string, object> savedDataDictionary = new Dictionary<string, object>();
        [SerializeField]
        private static Dictionary<string, Dictionary<string, object>> savedGameObject = new Dictionary<string, Dictionary<string, object>>();

        public static void SaveOrbitData(KeplerOrbitData orbitDataClass, string attractorName, string velocityHandleName, float timeScale, bool isNotPlanet, string thisGameobjName)
        {
            savedDataDictionary.Clear();

            SavedOrbitData = orbitDataClass;
            AttractorName = attractorName;
            VelocityHandle = velocityHandleName;
            TimeScale = timeScale;
            IsNotPlanetBool = isNotPlanet;

            SavedDataDictionary.Add("Orbit",SavedOrbitData);
            SavedDataDictionary.Add("Attractor",AttractorName);
            SavedDataDictionary.Add("Velocity",VelocityHandle);
            SavedDataDictionary.Add("TimeScale",TimeScale);
            SavedDataDictionary.Add("PlanetBool",IsNotPlanetBool);

            //SavedDataDictionary.Add(thisGameobjName, SavedDataDictionary);

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + thisGameobjName + "savedState.dta");
            binaryFormatter.Serialize(file, SavedDataDictionary);
            file.Close();

            Debug.Log("Game save at " + Application.persistentDataPath);
        }

        public static void LoadOrbitData(out KeplerOrbitData orbitDataClass, out string attractorDataName, out string velocityHandle,out float timeScale, out bool isNotPlanet, string thisGameobjName)
        {
            orbitDataClass = null;
            attractorDataName = null;
            velocityHandle = null;
            timeScale = 0f;
            isNotPlanet = false;
            SavedDataDictionary.Clear();

            if (File.Exists(Application.persistentDataPath + "/" + thisGameobjName + "savedState.dta"))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/" + thisGameobjName + "savedState.dta", FileMode.Open);
                SavedDataDictionary = (Dictionary<string, object>)binaryFormatter.Deserialize(file);
                file.Close();

                Debug.Log("Deserialization completed");

                orbitDataClass = (KeplerOrbitData)SavedDataDictionary["Orbit"];
                attractorDataName = (string)SavedDataDictionary["Attractor"];
                velocityHandle = (string)SavedDataDictionary["Velocity"];
                timeScale = (float)SavedDataDictionary["TimeScale"];
                isNotPlanet = (bool)SavedDataDictionary["PlanetBool"];

                Debug.Log("Data assignment completed");
            }
        }

        public static void PushToList(string thisGameobjName)
        {
            if (File.Exists(Application.persistentDataPath + "/" + thisGameobjName + "savedState.dta"))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/" + thisGameobjName + "savedState.dta", FileMode.Open);
                SavedDataDictionary = (Dictionary<string, object>)binaryFormatter.Deserialize(file);
                file.Close();

                Debug.Log("Deserialization completed");
            }
        }
    }
}
