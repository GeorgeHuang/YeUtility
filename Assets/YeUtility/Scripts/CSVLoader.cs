using System.Collections.Generic;
using System.IO;

namespace CommonUnit
{
    public class CSVLoader
    {
        private Dictionary<string, string[]> m_map = new Dictionary<string, string[]>();
        private string[] m_it;
        private int m_index = 0;
        private string m_allStr;

        public string AllStr
        {
            get { return m_allStr; }
        }

        public CSVLoader()
        {
        }

        public void setAllStr(string allStr, int keyOffset = 0)
        {
            m_allStr = allStr;
            m_allStr = m_allStr.Replace("\r", "");
            string[] token = new string[] { "\n" };
            string[] step1Strs = m_allStr.Split(token, System.StringSplitOptions.None);
            foreach (string step1 in step1Strs)
            {
                if (step1.Length > 0)
                {
                    if (step1.Length > 1 && (step1[0] != "/"[0] && step1[1] != "/"[0]))
                    {
                        string[] step2Strs = step1.Split(","[0]);
                        int index = 0;
                        string key = "";
                        List<string> val = new List<string>();
                        foreach (string step2 in step2Strs)
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

                        bool find = false;
                        int begineIndex = 0;
                        index = 0;
                        int size = 0;
                        foreach (string str in val)
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

                        if (key.Length > 0)
                        {
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
                    }
                }
            }
            m_index = 0;
            //Debug.Log("After: " + m_allStr);
        }

        public bool load(string fileName)
        {

#if UNITY_WEBPLAYER
        return true;
#else
            if (File.Exists(fileName) == false)
            {
                Common.SysPrint(" !!!! " + fileName + " not exists !!!! ");
                return false;
            }

            using (FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                if (fs != null)
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        setAllStr(sr.ReadToEnd());
                        sr.Close();
                    }
                    fs.Close();
                    return true;
                }
            }
            return false;
#endif
        }

        override public string ToString()
        {
            string rv = "";
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

        public void clear()
        {
            m_map.Clear();
            m_index = 0;
        }

        public bool searchKey(string key)
        {
            bool result = m_map.ContainsKey(key);
            if (result)
                m_it = m_map[key];
            m_index = 0;
            return result;
        }

        public string[] getValues()
        {
            return m_it;
        }

        public int getInt(int index)
        {
            m_index = index + 1;
            return int.Parse(m_it[index]);
        }
        public int getInt()
        {
            return getInt(m_index);
        }

        public string getString(int index)
        {
            m_index = index + 1;
            if (index >= m_it.Length)
            {
                return "";
            }
            return m_it[index];
        }
        public string getString()
        {
            return getString(m_index);
        }

        public float getFloat(int index)
        {
            m_index = index + 1;
            return float.Parse(m_it[index]);
        }
        public float getFloat()
        {
            return getFloat(m_index);
        }

        public int getParameterCount()
        {
            return m_it.Length;
        }

        public bool getBool()
        {
            return getBool(m_index);
        }

        public bool getBool(int index)
        {
            string str = getString(index);
            if (str == "1" || str == "True" || str == "TRUE" || str == "true")
                return true;
            return false;
        }
    }
}
