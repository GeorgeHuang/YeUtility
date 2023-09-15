using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

public static class ExtensionMethods
{
}

namespace YeUtility 
{
    public class Common
    {
        static public Vector3 Size = new Vector3(1, 1, 1);
        static public Vector3 inverseSize = new Vector3(-1, 1, 1);
        static public Vector3 FarPos = new Vector3(-5000, 5000, 0);
        static public Quaternion Right = Quaternion.Euler(0, 0, 0);
        static public Quaternion Left = Quaternion.Euler(0, 180, 0);

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
                setTrans.forward = -refTrans.forward;
                var angle = setTrans.rotation.eulerAngles;
                angle.y = 180;
                setTrans.rotation = Quaternion.Euler(angle);
            }
            else
            {
                setTrans.forward = refTrans.forward;
                var angle = setTrans.rotation.eulerAngles;
                angle.y = 0;
                setTrans.rotation = Quaternion.Euler(angle);
            }
        }

        public static string Encrypt(string toE)
        {

            var keyArray = Encoding.UTF8.GetBytes("12348578906543367877723456789012");
            var rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            var cTransform = rDel.CreateEncryptor();

            var toEncryptArray = Encoding.UTF8.GetBytes(toE);
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrypt(string toD)
        {

            var keyArray = Encoding.UTF8.GetBytes("12348578906543367877723456789012");
            var rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            var cTransform = rDel.CreateDecryptor();

            var toEncryptArray = Convert.FromBase64String(toD);
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }
        public static void SetChildInactiveButThis(GameObject go, string targetName, string pattern = "")
        {
            foreach (Transform ts in go.transform)
            {
                if (ts.name.Contains(targetName) && ts.name.Length == targetName.Length)
                {
                    ts.gameObject.SetActive(true);
                }
                else
                {
                    if (pattern.Length == 0)
                    {
                        ts.gameObject.SetActive(false);
                    }
                    else if (ts.name.Contains(pattern))
                    {
                        ts.gameObject.SetActive(false);
                    }
                }
            }
        }
        public static List<Vector3> SplitCircle(int number, Vector3? startDir = null)
        {
            var rv = new List<Vector3>();
            startDir ??= Vector3.right;
            var angle = 360.0f / number;
            for (var i = 0; i < number; ++i)
            {
                var dir = Quaternion.Euler(0, 0, angle * i) * startDir.Value;
                rv.Add(dir.normalized);
            }
            return rv;
        }

        public static List<Vector3> CreateFanVectors(float angle, int number, Vector3 centerDir)
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

        public static Vector2 GetRandomPosFromRect(Rect r)
        {
            Vector2 rv = new Vector2
            {
                x = UnityEngine.Random.Range(r.min.x, r.max.x),
                y = UnityEngine.Random.Range(r.min.y, r.max.y)
            };
            return rv;
        }
        public static GameObject SetInactiveButThis(GameObject[] gos, string targetName, string pattem = "")
        {
            GameObject rv = null;
            foreach (var go in gos)
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

        private static GameObject DeepFindS(GameObject go, string name)
        {
            foreach (Transform ts in go.transform)
            {
                if (ts.name.Contains(name) && ts.name.Length == name.Length)
                    return ts.gameObject;
                var temp = DeepFindS(ts.gameObject, name);
                if (temp != null) return temp;
            }
            return null;
        }
        public static GameObject DeepFind(GameObject go, string name)
        {
            foreach (Transform ts in go.transform)
            {
                if (ts.name.Contains(name))
                    return ts.gameObject;
                var temp = DeepFind(ts.gameObject, name);
                if (temp != null) return temp;
            }
            //Debug.Log("Common deepFind not found " + name + " in " + go.name);
            return null;
        }

        private static GameObject Find(GameObject go, string name)
        {
            return (from Transform ts in go.transform where ts.name.Contains(name) select ts.gameObject).FirstOrDefault();
        }

        public static T GetComponentInChild<T>(GameObject go, string name)
        {
            var childGo = Find(go, name);
            if (childGo == null)
                Debug.LogError("Child is null " + name);
            var rv = childGo.GetComponent<T>();
            if (rv == null)
                Debug.LogError("Component is null " + typeof(T));
            return rv;
        }

        private static void ChangeGoParent(Transform childTrans, Transform parentTrans, bool resetPos = true, bool resetRotate = true)
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
        public static void ChangeGOParent(Transform childTrans, GameObject parent, bool resetPos = true, bool resetRotate = true)
        {
            ChangeGoParent(childTrans, parent.transform, resetPos, resetRotate);
        }
        public static void ChangeGOParent(GameObject child, Transform parentTrans, bool resetPos = true, bool resetRotate = true)
        {
            ChangeGoParent(child.transform, parentTrans, resetPos, resetRotate);
        }
        public static void ChangeGOParent(GameObject child, GameObject parent, bool resetPos = true, bool resetRotate = true)
        {
            ChangeGoParent(child.transform, parent.transform, resetPos, resetRotate);
        }
        public static void DicToObject(object obj, Dictionary<string, object> dict)
        {
            foreach (var item in dict)
            {
                var fi = obj.GetType().GetField(item.Key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (fi == null) continue;
                var mi = fi.FieldType.GetMethod("Parse", new Type[] { typeof(string) });
                var tempStr = item.Value.ToString();
                var value = fi.GetValue(obj);
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
                else if (fi.FieldType == typeof(Dictionary<string, object>))
                {
                    fi.SetValue(obj, FromMiniJson(tempStr));
                }
                else if (fi.FieldType == typeof(int[]))
                {
                    var temp = FromMiniJson(tempStr);
                    if (temp == null) continue;
                    var objList = (List<object>)temp;
                    fi.SetValue(obj, objList.Select(Convert.ToInt32).ToArray());
                }
            }
        }
        public static object CallMethod(string methodName, object src)
        {
            object rv = null;
            var mis = GetMethods(src);
            foreach(var mi in mis)
            {
                if (mi.Name == methodName)
                {
                    rv = mi.Invoke(src, null);
                }
            }
            return rv;
        }

        private static IEnumerable<MethodInfo> GetMethods(object src)
        {
            return src.GetType().GetMethods(BindingFlags.Public
                        | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }
        public static MethodInfo[] GetMethods(Type t)
        {
            return t.GetMethods(BindingFlags.Public
                        | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName)?.GetValue(src, null);
        }
        public static FieldInfo GetProperty(object src, string propName)
        {
            var array = src.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return array.FirstOrDefault(a => a.Name == propName);
        }
        public static void FromString(System.Object obj, string input)
        {
            var arrays = input.Split('\n');

            foreach (var array in arrays)
            {
                //Debug.Log(array);
                var subarray = array.Split(new Char[] { ':' }, 2);
                var fi = obj.GetType().GetField(subarray[0], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (fi == null) continue;
                var mi = fi.FieldType.GetMethod("Parse", new Type[] { typeof(string) });
                var value = fi.GetValue(obj);
                if (mi != null)
                {
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
                    fi.SetValue(obj, FromMiniJson(subarray[1]));
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
                else if (fi.FieldType == typeof(int[]))
                {
                    var temp = FromMiniJson(subarray[1]);
                    if (temp == null) continue;
                    var objList = (List<object>)temp;
                    fi.SetValue(obj, objList.Select(Convert.ToInt32).ToArray());
                }
                else if (fi.FieldType == typeof(float[]))
                {
                    var temp = FromMiniJson(subarray[1]);
                    if (temp == null) continue;
                    var objList = (List<object>)temp;
                    fi.SetValue(obj, objList.Select(System.Convert.ToSingle).ToArray());
                }
                else if (fi.FieldType == typeof(List<string>))
                {
                    var temp = FromMiniJson(subarray[1]);
                    if (temp == null) continue;
                    var objList = (List<object>)temp;
                    var stringList = objList.Select(x => x as string).ToList();
                    fi.SetValue(obj, stringList);
                }
            }
        }

        private static object FromMiniJson(string input)
        {
            return MiniJSON.Json.Deserialize(input);
        }
        public static string ToString(object obj, bool toJson = false, bool hasKeyStringSymbol = false)
        {
            if (obj == null)
                return "null";

            var result = "";

            if (toJson) result += "{";

            var endSymbol = toJson ? "," : "\n";

            var keyStringSymbol = hasKeyStringSymbol ? "\"" : "";

            foreach (var fi in obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var valueObj = fi.GetValue(obj);

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
                    result = result + keyStringSymbol + fi.Name + keyStringSymbol + ":" + ToMiniJson(valueObj) + endSymbol;
            }

            if (!toJson) return result;
            result += "}";
            result = result.Replace(",}", "}");

            return result;
        }
        public static void TimeStr(float time, out string m, out string s)
        {
            var min = (int)time / 60;
            var sec = (int)time % 60;
            m = min.ToString("D2");
            s = sec.ToString("D2");
        }
        public static string TimeStr(float value)
        {
            var min = (int)value / 60;
            var sec = (int)value % 60;
            return min.ToString("D2") +":"+ sec.ToString("D2");
        }
        public static string TimeHrStr(float value)
        {
            var hr = (int)value / 3600;
            var min = (int)value % 3600 / 60;
            var sec = (int)value % 60;
            return hr + ":" + min.ToString("D2") + ":" + sec.ToString("D2");
        }
        public static string TimeSecStr(float value)
        {
            var min = (int)value / 60;
            var sec = (int)value % 60;
            var ms = (int)(value * 1000) % 1000;
            return min.ToString("D2") + "'" + sec.ToString("D2") + "''" + ms.ToString("D3");
        }
        public static Vector2 WorldPosToUIPos(Transform refTrans,
                                              Camera gameCamera,
                                              Canvas uiCanvas,
                                              Camera uiCamera)
        {
            return WorldPosToUIPos(refTrans.position, gameCamera, uiCanvas, uiCamera);
        }

        private static Vector2 WorldPosToUIPos(Vector3 pos,
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

        public static string ToMiniJson(System.Object obj)
        {
            return MiniJSON.Json.Serialize(obj);
        }
        // ReSharper disable Unity.PerformanceAnalysis
        public static void SysPrint(System.Object temp)
        {
            Debug.Log(temp);
        }
        public static bool CompareFloat(float lhs, float rhs, float delta = 0.0001f)
        {
            return Mathf.Abs(lhs - rhs) < delta;
        }
        public static bool CompareVector3(Vector3 lhs, Vector3 rhs, float delta = 0.000001f)
        {
            return (lhs - rhs).sqrMagnitude < delta;
        }
        public static void SetActiveRecursively(GameObject go, bool active)
        {
            if (go == null)
                return;
            go.SetActive(active);
            foreach (Transform trans in go.transform)
            {
                SetActiveRecursively(trans.gameObject, active);
            }
        }
        public static void SetChildActive(GameObject go, string childName, bool active)
        {
            if (go == null)
                return;
            var childGO = DeepFindS(go, childName);
            if (childGO != null)
            {
                childGO.SetActive(active);
            }
        }
        public static List<int> RandomPick(int min, int max, int number)
        {
            var usedValues = new List<int>();
            while (usedValues.Count < number)
            {
                var val = UnityEngine.Random.Range(min, max);
                usedValues.Add(val);
            }
            return usedValues;
        }
        public static List<int> RandomPickNoRepeat(int min, int max, int nubmer)
        {
            var usedValues = new List<int>();
            if ((max - min + 1) < nubmer)
            {
                Debug.LogError("randomPickNoRepeat number > max");
                return usedValues;
            }
            int UniqueRandomInt(int min, int max)
            {
                var val = UnityEngine.Random.Range(min, max + 1);
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
        public static bool Random(int value, int max = 100)
        {
            if (max <= value) return true;
            return UnityEngine.Random.Range(0, max) < value;
        }
        public static List<int> RandomGroup(float[] group, int pickNumber)
        {
            var rv = new List<int>();

            if (pickNumber > group.Length)
            {
                Debug.LogError("Pick Number More Than Group Length");
                return rv;
            }

            do
            {
                var pick = RandomGroup(group);
                while (rv.Contains(pick))
                {
                    pick = RandomGroup(group);
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
        private static int RandomGroup(IReadOnlyList<float> group)
        {
            var rv = 0;
            var size = group.Count;
            float sum = 0;
            for (var i = 0; i < size; ++i)
            {
                sum += group[i];
            }
            var value = UnityEngine.Random.Range(0, sum);
            for (var i = 0; i < size; ++i)
            {
                if (value < group[i])
                {
                    rv = i;
                    break;
                }
                value -= group[i];
            }
            return rv;
        }
        public static void ChangeLayersRecursively(Transform goTrans, int layer)
        {
            goTrans.gameObject.layer = layer;
            foreach (Transform subTrans in goTrans)
            {
                ChangeLayersRecursively(subTrans, layer);
            }
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

                foreach (var type in assembly.GetTypes())
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
        public static float GetSignFromRightSide(Transform selfTrans, Vector3 tarPos)
        {
            float rv = 1;
            var direction = (tarPos - selfTrans.position).normalized;
            if (Vector3.Dot(direction, selfTrans.right) < 0)
            {
                rv = -1;
            }
            return rv;
        }
        public static float GetSignFromRightSide(Transform selfTrans, Transform targetTrans)
        {
            float rv = 1;
            var direction = (targetTrans.position - selfTrans.position).normalized;
            if (Vector3.Dot(direction, selfTrans.right) < 0)
            {
                rv = -1;
            }
            return rv;
        }
        public static string UnicodeToBig5(string srcText)
        {
            return Encoding.GetEncoding(950).GetString(Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding(950), Encoding.Unicode.GetBytes(srcText)));
        }
        public static List<T> RepeatedDefault<T>(int count)
        {
            return Repeated(default(T), count);
        }

        private static List<T> Repeated<T>(T value, int count)
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
        public static Vector3 GetDirWithoutZ(Transform source, Transform target, bool normalized = true)
        {
            return GetDirWithoutZ(source.position, target.position, normalized);
        }
        public static Vector3 GetDirWithoutZ(Vector3 sourcePos, Transform target, bool normalized = true)
        {
            return GetDirWithoutZ(sourcePos, target.position, normalized);
        }
        public static Vector3 GetDirWithoutZ(Transform source,Vector3 targetPos, bool normalized = true)
        {
            return GetDirWithoutZ(source.position, targetPos, normalized);
        }

        public static Vector3 GetDirWithoutZ(Vector3 sourcePos,Vector3 targetPos, bool normalized = true)
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

        public static bool HasDir(string pathName)
        {
            return Directory.Exists(path: pathName);
        }

        public static void CreateDir(string pathName)
        {
            var rv = Directory.CreateDirectory(pathName);
            Debug.Log(rv);
        }
        public static bool FileExists(string saveFileName)
        {
            return File.Exists(saveFileName);
        }

        public static string PathCombine(string[] pathArray)
        {
            return Path.Combine(pathArray);
        }

        public static void SaveFile(string saveFileName, byte[] saveData)
        {
            var binaryFormatter = new BinaryFormatter();
            var fileStream = File.Create(saveFileName);
            binaryFormatter.Serialize(fileStream, saveData);
            fileStream.Close();
        }

        public static byte[] LoadFile(string saveFullPath)
        {
            var binaryFormatter = new BinaryFormatter();
            var fileStream = File.Open(saveFullPath, FileMode.Open);
            var saveData = (byte[])binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return saveData;
        }
    }
}
