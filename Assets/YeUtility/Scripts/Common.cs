using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

public static class ExtensionMethods
{
    public static decimal Map(this decimal value, decimal fromSource, decimal toSource, decimal fromTarget, decimal toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}

namespace CommonUnit
{
    public class Common
    {
        static public Vector3 Size = new Vector3(1, 1, 1);
        static public Vector3 inverseSize = new Vector3(-1, 1, 1);
        static public Vector3 FarPos = new Vector3(-5000, 5000, 0);
        static public Quaternion Right = Quaternion.Euler(0, 0, 0);
        static public Quaternion Left = Quaternion.Euler(0, 180, 0);

        public static IEnumerator WaitForRealSeconds(float time)
        {
            float start = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < start + time)
            {
                yield return null;
            }
        }
        private static bool m_isEncrypt = false;
        public static bool IsEncrypt
        {
            get
            {
                return m_isEncrypt;
            }
            set
            {
                m_isEncrypt = value;
            }
        }

        public static byte[] StringToByte(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string ByteToString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static double Lerp(double from, double to, double ratio)
        {
            if (ratio >= 1)
                return to;
            if (ratio <= 0)
                return from;
            if (to > from)
                return (to - from) * ratio + from;
            if (to < from)
                return from - ((from - to) * ratio);
            return from;
        }
        public static void SetFarPos(Transform trans)
        {
            trans.position = FarPos;
        }
        public static void SetFarPos(GameObject go)
        {
            SetFarPos(go.transform);
        }
        public static void SetFarPos(MonoBehaviour mb)
        {
            SetFarPos(mb.gameObject.transform);
        }
        public static void FaceCamera(Vector3 refDir, Transform setTrans, Transform refTrans)
        {
            if (refDir.x > 0)
            {
                //ModelTrans.localScale = Common.inverseSize;
                setTrans.forward = -refTrans.forward;
                var angle = setTrans.rotation.eulerAngles;
                angle.y = 180;
                setTrans.rotation = Quaternion.Euler(angle);
            }
            else
            {
                //ModelTrans.localScale = Common.Size;
                setTrans.forward = refTrans.forward;
                var angle = setTrans.rotation.eulerAngles;
                angle.y = 0;
                setTrans.rotation = Quaternion.Euler(angle);
            }
        }

        public static string Encrypt(string toE)
        {

            byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12348578906543367877723456789012");
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();

            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toE);
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrypt(string toD)
        {

            byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12348578906543367877723456789012");
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();

            byte[] toEncryptArray = Convert.FromBase64String(toD);
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        public static string PlayerDataPath
        {
            get
            {
                if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer
                    || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
                    return Application.dataPath + "/Documents/";
                else
                    return Application.persistentDataPath + "/";
            }
        }
        static public void setChildInaciveButThis(GameObject go, string targetName, string pattem = "")
        {
            foreach (Transform ts in go.transform)
            {
                if (ts.name.Contains(targetName) && ts.name.Length == targetName.Length)
                {
                    ts.gameObject.SetActive(true);
                }
                else
                {
                    if (pattem.Length == 0)
                    {
                        ts.gameObject.SetActive(false);
                    }
                    else if (ts.name.Contains(pattem))
                    {
                        ts.gameObject.SetActive(false);
                    }
                }
            }
        }
        static public List<Vector3> SplitCircle(int number, Vector3? startDir = null)
        {
            List<Vector3> rv = new List<Vector3>();
            if (startDir == null)
                startDir = Vector3.right;
            float angle = 360.0f / number;
            for (int i = 0; i < number; ++i)
            {
                Vector3 dir = Quaternion.Euler(0, 0, angle * i) * startDir.Value;
                rv.Add(dir.normalized);
            }
            return rv;
        }

        static public List<Vector3> CreateFanVectors(float angle, int number, Vector3 centerDir)
        {
            List<Vector3> rv = new List<Vector3>();
            var startDir = Quaternion.Euler(0, 0, -angle/2) * centerDir;
            var deltaAngle = (angle) / (number - 1);

            for (int i = 0; i < number; ++i)
            {
                var newDir = Quaternion.Euler(0, 0, deltaAngle * i) * startDir;
                rv.Add(newDir);
            }
            return rv;
        }

        static public string intToStr(int value, int digits)
        {
            var f = "{0:";
            for (int i = 0; i < digits; ++i) f += "0";
            f += "}";
            return string.Format(f, value);
        }
        static public Vector2 getRandomPosFromRect(Rect r)
        {
            Vector2 rv = new Vector2
            {
                x = UnityEngine.Random.Range(r.min.x, r.max.x),
                y = UnityEngine.Random.Range(r.min.y, r.max.y)
            };
            return rv;
        }
        static public GameObject setInaciveButThis(GameObject[] gos, string targetName, string pattem = "")
        {
            GameObject rv = null;
            foreach (GameObject go in gos)
            {
                if (go.name.Contains(targetName) && go.name.Length == targetName.Length)
                {
                    go.SetActive(true);
                    rv = go;
                }
                else
                {
                    if (pattem.Length == 0)
                    {
                        go.SetActive(false);
                    }
                    else if (go.name.Contains(pattem))
                    {
                        go.SetActive(false);
                    }
                }
            }
            return rv;
        }
        static public GameObject deepFindS(GameObject go, string name)
        {
            foreach (Transform ts in go.transform)
            {
                if (ts.name.Contains(name) && ts.name.Length == name.Length)
                    return ts.gameObject;
                else
                {
                    GameObject temp = deepFindS(ts.gameObject, name);
                    if (temp != null)
                        return temp;
                }
            }
            return null;
        }
        static public GameObject deepFind(GameObject go, string name)
        {
            foreach (Transform ts in go.transform)
            {
                if (ts.name.Contains(name))
                    return ts.gameObject;
                else
                {
                    GameObject temp = deepFind(ts.gameObject, name);
                    if (temp != null)
                        return temp;
                }
            }
            //Debug.Log("Common deepFind not found " + name + " in " + go.name);
            return null;
        }
        static public GameObject find(GameObject go, string name)
        {
            foreach (Transform ts in go.transform)
            {
                if (ts.name.Contains(name))
                    return ts.gameObject;
            }
            return null;
        }

        static public T GetComponentInChild<T>(GameObject go, string name)
        {
            var childGO = find(go, name);
            if (childGO == null)
                Debug.LogError("Child is null " + name);
            var rv = childGO.GetComponent<T>();
            if (rv == null)
                Debug.LogError("Component is null " + typeof(T));
            return rv;
        }
        static public void changeGOParent(Transform childTrans, Transform parentTrans, bool resetPos = true, bool resetRotate = true)
        {
            if (childTrans == null)
                return;

            if (childTrans.parent == parentTrans)
                return;

            if (parentTrans == null)
            {
                childTrans.parent = null;
                return;
            }


            Vector3 pos = Vector3.zero;
            Quaternion q = Quaternion.identity;
            if (resetPos == false)
            {
                pos = childTrans.localPosition;
            }

            if (resetRotate == false)
            {
                q = childTrans.localRotation;
            }

            childTrans.SetParent(parentTrans);
            childTrans.localPosition = pos;
            childTrans.localRotation = q;
        }
        static public void changeGOParent(Transform childTrans, GameObject parent, bool resetPos = true, bool resetRotate = true)
        {
            changeGOParent(childTrans, parent.transform, resetPos, resetRotate);
        }
        static public void changeGOParent(GameObject child, Transform parentTrans, bool resetPos = true, bool resetRotate = true)
        {
            changeGOParent(child.transform, parentTrans, resetPos, resetRotate);
        }
        static public void changeGOParent(GameObject child, GameObject parent, bool resetPos = true, bool resetRotate = true)
        {
            changeGOParent(child.transform, parent.transform, resetPos, resetRotate);
        }
        static public void dicToObject(System.Object obj, Dictionary<string, object> dict)
        {
            foreach (KeyValuePair<string, object> item in dict)
            {
                FieldInfo fi = obj.GetType().GetField(item.Key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (fi != null)
                {
                    MethodInfo mi = fi.FieldType.GetMethod("Parse", new Type[] { typeof(string) });
                    string tempStr = item.Value.ToString();
                    Debug.Log(tempStr);
                    System.Object value = fi.GetValue(obj);
                    if (mi != null)
                    {
                        fi.SetValue(obj, mi.Invoke(obj, new object[] { tempStr }));
                    }
                    else if (value is System.Enum)
                    {
                        fi.SetValue(obj, System.Enum.Parse(fi.FieldType, tempStr));
                    }
                    else if (fi.FieldType == typeof(string))
                    {
                        fi.SetValue(obj, tempStr);
                    }
                    else if (fi.FieldType == typeof(System.Collections.Generic.Dictionary<string, object>))
                    {
                        fi.SetValue(obj, fromMiniJson(tempStr));
                    }
                    else if (fi.FieldType == typeof(System.Int32[]))
                    {
                        System.Object temp = fromMiniJson(tempStr);
                        if (temp != null)
                        {
                            List<System.Object> objList = (List<System.Object>)temp;
                            List<int> intList = new List<int>();
                            foreach (System.Object sobj in objList)
                            {
                                intList.Add(System.Convert.ToInt32(sobj));
                            }
                            fi.SetValue(obj, intList.ToArray());
                        }
                    }
                }


            }
        }
        public static object CallMethod(string methodName, object src)
        {
            object rv = null;
            var mis = GetMehods(src);
            foreach(var mi in mis)
            {
                if (mi.Name == methodName)
                {
                    rv = mi.Invoke(src, null);
                }
            }
            return rv;
        }
        public static MethodInfo[] GetMehods(object src)
        {
            return src.GetType().GetMethods(BindingFlags.Public
                        | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }
        public static MethodInfo[] GetMehods(Type t)
        {
            return t.GetMethods(BindingFlags.Public
                        | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        public static FieldInfo GetProperty(object src, string propName)
        {
            FieldInfo rv = null;
            var array = src.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var a in array)
            {
                if (a.Name == propName)
                {
                    rv = a;
                    break;
                }
            }
            return rv;
        }
        static public void fromString(System.Object obj, string input)
        {
            string[] arrays = input.Split('\n');

            foreach (string array in arrays)
            {
                //Debug.Log(array);
                string[] subarray = array.Split(new Char[] { ':' }, 2);
                FieldInfo fi = obj.GetType().GetField(subarray[0], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (fi != null)
                {
                    MethodInfo mi = fi.FieldType.GetMethod("Parse", new Type[] { typeof(string) });

                    System.Object value = fi.GetValue(obj);
                    if (mi != null)
                    {
                        //Debug.Log("Hello");
                        fi.SetValue(obj, mi.Invoke(obj, new object[] { subarray[1] }));
                    }
                    else if (value is System.Enum)
                    {
                        fi.SetValue(obj, System.Enum.Parse(fi.FieldType, subarray[1]));
                    }
                    else if (fi.FieldType == typeof(string))
                    {
                        fi.SetValue(obj, subarray[1]);
                    }
                    else if (fi.FieldType == typeof(Dictionary<string, System.Object>))
                    {
                        fi.SetValue(obj, fromMiniJson(subarray[1]));
                    }
                    //else if (fi.FieldType == typeof(IDictionary))
                    else if (fi.FieldType == typeof(Dictionary<string, int>))
                    {
                        var temp = JsonConvert.DeserializeObject<Dictionary<string, int>>(subarray[1]);
                        fi.SetValue(obj, temp);
                    }
                    else if (fi.FieldType == typeof(Dictionary<string, string>))
                    {
                        var temp = JsonConvert.DeserializeObject<Dictionary<string, string>>(subarray[1]);
                        fi.SetValue(obj, temp);
                    }
                    /*
                    else if (value is IDictionary)
                    {
                        var v = (System.Collections.Generic.Dictionary<string, System.Object>)fromMiniJson(subarray[1]);
                        Dictionary<string, int> temp = new Dictionary<string, int>();
                        foreach(var key in v.Keys)
                        {
                            sysPrint(v[key].GetType());
                            temp[key] = (int)Convert.ToInt64(v[key]);
                        }
                        fi.SetValue(obj, temp);
                    }
                    */
                    else if (fi.FieldType == typeof(System.Int32[]))
                    {
                        System.Object temp = fromMiniJson(subarray[1]);
                        if (temp != null)
                        {
                            List<System.Object> objList = (List<System.Object>)temp;
                            List<int> intList = new List<int>();
                            foreach (System.Object sobj in objList)
                            {
                                intList.Add(System.Convert.ToInt32(sobj));
                            }
                            fi.SetValue(obj, intList.ToArray());
                        }
                    }
                    else if (fi.FieldType == typeof(System.Single[]))
                    {
                        System.Object temp = fromMiniJson(subarray[1]);
                        if (temp != null)
                        {
                            List<System.Object> objList = (List<System.Object>)temp;
                            List<float> intList = new List<float>();
                            foreach (System.Object sobj in objList)
                            {
                                intList.Add(System.Convert.ToSingle(sobj));
                            }
                            fi.SetValue(obj, intList.ToArray());
                        }
                    }
                    else if (fi.FieldType == typeof(List<string>))
                    {
                        System.Object temp = fromMiniJson(subarray[1]);
                        if (temp != null)
                        {
                            List<System.Object> objList = (List<System.Object>)temp;
                            List<string> stringList = new List<string>();
                            foreach (System.Object sobj in objList)
                            {
                                stringList.Add(sobj as string);
                            }
                            fi.SetValue(obj, stringList);
                        }
                    }
                }
            }
        }
        static public System.Object fromMiniJson(string input)
        {
            return MiniJSON.Json.Deserialize(input);
        }
        static public void TimeStr(float time, out string m, out string s)
        {
            int min = (int)time / 60;
            int sec = (int)time % 60;
            m = min.ToString("D2");
            s = sec.ToString("D2");
        }
        static public string TimeStr(float value)
        {
            int min = (int)value / 60;
            int sec = (int)value % 60;
            //int ms = (int)(value * 1000) % 1000;
            return min.ToString("D2") +":"+ sec.ToString("D2");
            //return $"{min.ToString("D2")} : {sec.ToString("D2")}";
            //return $"{min,2:D2} : {sec,2:D2}";
        }
        static public string TimeHrStr(float value)
        {
            int hr = (int)value / 3600;
            int min = (int)value % 3600 / 60;
            int sec = (int)value % 60;
            //int ms = (int)(value * 1000) % 1000;
            return hr.ToString() + ":" + min.ToString("D2") + ":" + sec.ToString("D2");
        }
        static public string TimeSecStr(float value)
        {
            int min = (int)value / 60;
            int sec = (int)value % 60;
            int ms = (int)(value * 1000) % 1000;
            return min.ToString("D2") + "'" + sec.ToString("D2") + "''" + ms.ToString("D3");
        }
        static public string toString(System.Object obj, bool toJson = false, bool hasKeyStringSymbol = false)
        {
            if (obj == null)
                return "null";

            string result = "";

            if (toJson) result += "{";

            string endSymbol = toJson ? "," : "\n";

            string keyStringSymbol = hasKeyStringSymbol ? "\"" : "";

            foreach (FieldInfo fi in obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                System.Object valueObj = fi.GetValue(obj);

                if (valueObj is SpriteRenderer && (valueObj as SpriteRenderer).sprite != null)
                {
                    if ((valueObj as SpriteRenderer).sprite == null)
                    {
                        return "null";
                    }
                    else
                    {
                        result = result + keyStringSymbol + fi.Name + keyStringSymbol + ":" + (valueObj as SpriteRenderer).sprite.name + endSymbol;
                    }
                }
                else if (valueObj is GameObject)
                    continue;
                else if (!(valueObj is IDictionary || valueObj is IList))
                    result = result + keyStringSymbol + fi.Name + keyStringSymbol + ":" + valueObj + endSymbol;
                else
                    result = result + keyStringSymbol + fi.Name + keyStringSymbol + ":" + toMiniJson(valueObj) + endSymbol;
            }

            if (toJson)
            {
                result += "}";
                result = result.Replace(",}", "}");
            }

            return result;
        }
        static public Vector2 WorldPosToUIPos(Transform refTrans,
                                              Camera gameCamera,
                                              Canvas uiCanvas,
                                              Camera uiCamera)
        {
            return WorldPosToUIPos(refTrans.position, gameCamera, uiCanvas, uiCamera);
        }
        static public Vector2 WorldPosToUIPos(Vector3 pos,
                                              Camera gameCamera,
                                              Canvas uiCanvas,
                                              Camera uiCamera)
        {

            var actorPos = gameCamera.WorldToScreenPoint(pos);
            var actorUIPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                uiCanvas.transform as RectTransform, actorPos,
                uiCamera,
                out actorUIPos);
            return actorUIPos;
        }

        static public string toMiniJson(System.Object obj)
        {
            return MiniJSON.Json.Serialize(obj);
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public static void SysPrint(System.Object temp)
        {
            Debug.Log(temp);
        }
        public static bool compareFloat(float lhs, float rhs, float delta = 0.0001f)
        {
            return Mathf.Abs(lhs - rhs) < delta;
        }
        public static bool compareVector3(Vector3 lhs, Vector3 rhs, float delta = 0.000001f)
        {
            return (lhs - rhs).sqrMagnitude < delta;
        }
        static public void setActiveRecursively(GameObject go, bool active)
        {
            if (go == null)
                return;
            go.SetActive(active);
            foreach (Transform trans in go.transform)
            {
                setActiveRecursively(trans.gameObject, active);
            }
        }
        static public void setChildActive(GameObject go, string childName, bool active)
        {
            if (go == null)
                return;
            GameObject childGO = deepFindS(go, childName);
            if (childGO != null)
            {
                childGO.SetActive(active);
            }
        }
        static public List<int> randomPick(int min, int max, int nubmer)
        {
            List<int> usedValues = new List<int>();
            while (usedValues.Count < nubmer)
            {
                var val = UnityEngine.Random.Range(min, max);
                usedValues.Add(val);
            }
            return usedValues;
        }
        static public List<int> randomPickNoRepeat(int min, int max, int nubmer)
        {
            List<int> usedValues = new List<int>();
            if ((max - min + 1) < nubmer)
            {
                Debug.LogError("randomPickNoRepeat number > max");
                return usedValues;
            }
            int UniqueRandomInt(int min, int max)
            {
                int val = UnityEngine.Random.Range(min, max + 1);
                while (usedValues.Contains(val))
                {
                    val = UnityEngine.Random.Range(min, max + 1);
                }
                return val;
            }
            while (usedValues.Count < nubmer)
            {
                var val = UniqueRandomInt(min, max);
                usedValues.Add(val);
            }
            return usedValues;
        }
        static public bool random(int value, int max = 100)
        {
            if (max <= value) return true;
            return UnityEngine.Random.Range(0, max) < value;
        }
        static public List<int> randomGroup(float[] group, int pickNumber)
        {
            List<int> rv = new List<int>();

            if (pickNumber > group.Length)
            {
                Debug.LogError("Pick Number More Than Group Length");
                return rv;
            }

            do
            {
                int pick = randomGroup(group);
                while (rv.Contains(pick))
                {
                    pick = randomGroup(group);
                }
                rv.Add(pick);
            }
            while (rv.Count < pickNumber);

            return rv;
        }
        /// <summary>
        /// 獎勵池抽選
        /// </summary>
        /// <param name="group">一堆物品的機率 可以總合不為100</param>
        /// <returns></returns>
        static public int randomGroup(float[] group)
        {
            int rv = 0;
            int size = group.Length;
            float sum = 0;
            for (int i = 0; i < size; ++i)
            {
                sum += group[i];
            }
            float value = UnityEngine.Random.Range(0, sum);
            for (int i = 0; i < size; ++i)
            {
                if (value < group[i])
                {
                    rv = i;
                    break;
                }
                else
                {
                    value -= group[i];
                }
            }
            return rv;
        }
        static public void changeLayersRecursively(Transform goTrans, int layer)
        {
            goTrans.gameObject.layer = layer;
            foreach (Transform subTrans in goTrans)
            {
                changeLayersRecursively(subTrans, layer);
            }
        }
        static public void setAnimationFloat(Animator animeCtrl, string name, float value)
        {
            if (animeCtrl == null) return;
            animeCtrl.SetFloat(name, value);
        }
        static public void setAnimationBool(Animator animeCtrl, string name, bool value)
        {
            if (animeCtrl == null) return;
            animeCtrl.SetBool(name, value);
        }
        static public void setAnimationTrigger(Animator animeCtrl, string name)
        {
            if (animeCtrl == null || string.IsNullOrEmpty(name)) return;
            animeCtrl.SetTrigger(name);
        }
        static public void resetTrigger(Animator animeCtrl, string name)
        {
            if (animeCtrl == null) return;
            animeCtrl.ResetTrigger(name);
        }
        public static List<Type> GetSubTypes<T>() where T : class
        {
            var types = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.StartsWith("Mono.Cecil"))
                    continue;

                if (assembly.FullName.StartsWith("UnityScript"))
                    continue;

                if (assembly.FullName.StartsWith("Boo.Lan"))
                    continue;

                if (assembly.FullName.StartsWith("System"))
                    continue;

                if (assembly.FullName.StartsWith("I18N"))
                    continue;

                if (assembly.FullName.StartsWith("UnityEngine"))
                    continue;

                if (assembly.FullName.StartsWith("UnityEditor"))
                    continue;

                if (assembly.FullName.StartsWith("mscorlib"))
                    continue;

                foreach (Type type in assembly.GetTypes())
                {
                    if (!type.IsClass)
                        continue;

                    if (type.IsAbstract)
                        continue;

                    if (!type.IsSubclassOf(typeof(T)))
                        continue;

                    types.Add(type);
                }
            }

            return types;
        }
        static public float GetSignFromRightSide(Transform selfTrans, Vector3 tarPos)
        {
            float rv = 1;
            Vector3 direction = (tarPos - selfTrans.position).normalized;
            if (Vector3.Dot(direction, selfTrans.right) < 0)
            {
                rv = -1;
            }
            return rv;
        }
        static public float GetSignFromRightSide(Transform selfTrans, Transform targetTrans)
        {
            float rv = 1;
            Vector3 direction = (targetTrans.position - selfTrans.position).normalized;
            if (Vector3.Dot(direction, selfTrans.right) < 0)
            {
                rv = -1;
            }
            return rv;
        }
        static public string UnicodeToBig5(string srcText)
        {
            return Encoding.GetEncoding(950).GetString(Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding(950), Encoding.Unicode.GetBytes(srcText)));
        }
        public static List<T> RepeatedDefault<T>(int count)
        {
            return Repeated(default(T), count);
        }
        public static List<T> Repeated<T>(T value, int count)
        {
            List<T> ret = new List<T>(count);
            ret.AddRange(System.Linq.Enumerable.Repeat(value, count));
            return ret;
        }
        public static bool IsInterval(float t, float min, float max)
        {
            return (t >= min && t <= max);
        }
        public static void VectorNormalWithoutZ(Vector3 input, out Vector3 dir)
        {
            input.z = 0;
            dir = input.normalized;
        }
        public static float GetDot(ref Vector3 source,ref Vector3 t1,ref Vector3 t2)
        {
            var dir1 = getDirWithoutZ(ref source,ref t1, false);
            var dir2 = getDirWithoutZ(ref source,ref t2, false);
            return Vector3.Dot(dir1, dir2);
        }
        public static Vector3 getDirWithoutZ(Transform source, Transform target, bool normalized = true)
        {
            return getDirWithoutZ(source.position, target.position, normalized);
        }
        public static Vector3 getDirWithoutZ(Vector3 sourcePos, Transform target, bool normalized = true)
        {
            return getDirWithoutZ(sourcePos, target.position, normalized);
        }
        public static Vector3 getDirWithoutZ(Transform source,Vector3 targetPos, bool normalized = true)
        {
            return getDirWithoutZ(source.position, targetPos, normalized);
        }
        public static Vector3 getDirWithoutZ(Vector3 sourcePos,Vector3 targetPos, bool normalized = true)
        {
            return getDirWithoutZ(ref sourcePos, ref targetPos, normalized);
        }
        public static Vector3 getDirWithoutZ(ref Vector3 sourcePos,ref Vector3 targetPos, bool normalized = true)
        {
            var dir = targetPos - sourcePos;
            dir.z = 0;
            if (!normalized)
                return dir;
            //var v1 = float3(dir.x, dir.y, dir.z);
            //return normalize(v1);
            return dir.normalized;
        }

        public static bool FileExists(string saveFileName)
        {
            // check file exists
            if (!File.Exists(saveFileName))
            {
                return false;
            }
            return true;
        }

        public static void SaveFile(string saveFileName, byte[] saveData)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Create(saveFileName);
            binaryFormatter.Serialize(fileStream, saveData);
            fileStream.Close();
        }

        public static byte[] LoadFile(string saveFullPath)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(saveFullPath, FileMode.Open);
            byte[] saveData = (byte[])binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return saveData;
        }
        /*
static public bool hasInternet()
{
//excerpt from code
bool isConnectedToInternet = false;

#if UNITY_IPHONE || UNITY_ANDROID
if (Application.internetReachability != NetworkReachability.NotReachable)
{
isConnectedToInternet = true;
}

#endif

#if (!UNITY_IPHONE && !UNITY_ANDROID)
if (Network.player.ipAddress.ToString() != "127.0.0.1")
{
isConnectedToInternet = true;
}
#endif
return isConnectedToInternet;
}
*/
    }
}
