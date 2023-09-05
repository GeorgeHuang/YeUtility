using System.Collections.Generic;
using System.IO;

namespace YeUtility
{
    public class CSVLoader
    {
        private Dictionary<string, string[]> m_map = new Dictionary<string, string[]>();
        private string[] m_it;
        private int m_index = 0;

        public string AllStr { get; private set; }

        public CSVLoader()
        {
        }

        public void SetAllStr(string allStr, int keyOffset = 0)
        {
            AllStr = allStr;
            AllStr = AllStr.Replace("\r", "");
            var token = new[] { "\n" };
            var step1Strs = AllStr.Split(token, System.StringSplitOptions.None);
            foreach (var step1 in step1Strs)
            {
                if (step1.Length <= 0) continue;
                if (step1.Length <= 1 || (step1[0] == "/"[0] || step1[1] == "/"[0])) continue;
                var step2Strs = step1.Split(","[0]);
                var index = 0;
                var key = "";
                var val = new List<string>();
                foreach (var step2 in step2Strs)
                {
                    if (index == keyOffset)
                    {
                        key = step2;
                    }
                    else if (index < keyOffset)
                    {
                        ++index;
                        continue;
                    }
                    else
                    {
                        val.Add(step2);
                    }
                    ++index;
                }

                var find = false;
                var begineIndex = 0;
                index = 0;
                var size = 0;
                foreach (var str in val)
                {
                    if (str.Length == 0)
                    {
                        if (find == false)
                        {
                            find = true;
                            begineIndex = index;
                        }
                        ++size;
                    }
                    else
                    {
                        begineIndex = 0;
                        size = 0;
                        find = false;
                    }
                    ++index;
                }

                if (find)
                {
                    val.RemoveRange(begineIndex, size);
                }

                if (key.Length <= 0) continue;
                if (m_map.ContainsKey(key))
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError("Key Duplicates : " + key);
#endif
                    m_map[key] = val.ToArray();
                }
                else
                    m_map.Add(key, val.ToArray());
            }
            m_index = 0;
            //Debug.Log("After: " + m_allStr);
        }

        public bool Load(string fileName)
        {

#if UNITY_WEBPLAYER
        return true;
#else
            if (File.Exists(fileName) == false)
            {
                Common.SysPrint(" !!!! " + fileName + " not exists !!!! ");
                return false;
            }

            using var fs = File.Open(fileName, FileMode.Open, FileAccess.Read);
            using (var sr = new StreamReader(fs))
            {
                SetAllStr(sr.ReadToEnd());
                sr.Close();
            }
            fs.Close();
            return true;
#endif
        }

        public override string ToString()
        {
            var rv = "";
            foreach (var key in m_map.Keys)
            {
                rv += key;
                foreach (var c in m_map[key])
                {
                    rv += "," + c;
                }
                rv += "\n";
            }
            return rv;
        }

        public void Clear()
        {
            m_map.Clear();
            m_index = 0;
        }

        public bool SearchKey(string key)
        {
            var result = m_map.ContainsKey(key);
            if (result)
                m_it = m_map[key];
            m_index = 0;
            return result;
        }

        public string[] GetValues()
        {
            return m_it;
        }

        public int GetInt(int index)
        {
            m_index = index + 1;
            return int.Parse(m_it[index]);
        }
        public int GetInt()
        {
            return GetInt(m_index);
        }

        public string GetString(int index)
        {
            m_index = index + 1;
            return index >= m_it.Length ? "" : m_it[index];
        }
        public string GetString()
        {
            return GetString(m_index);
        }

        public float GetFloat(int index)
        {
            m_index = index + 1;
            return float.Parse(m_it[index]);
        }
        public float GetFloat()
        {
            return GetFloat(m_index);
        }

        public int GetParameterCount()
        {
            return m_it.Length;
        }

        public bool GetBool()
        {
            return GetBool(m_index);
        }

        public bool GetBool(int index)
        {
            var str = GetString(index);
            return str is "1" or "True" or "TRUE" or "true";
        }
    }
}
