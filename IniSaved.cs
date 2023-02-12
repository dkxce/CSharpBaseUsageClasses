//
// C# (cross-platform)
// IniSaved
// v 0.02, 12.02.2023
// milokz@gmail.com
// en,ru,1251,utf-8
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace System.Xml
{
    public class IniSaved<T>
    {
        #region Presave
        public static bool presaveSimpleTypes = false;
        public static bool presaveNonSimpleTypes = false;
        public static bool presaveArrayInfo = true;
        public static bool presaveFileHeader = true;
        #endregion Presave

        #region SAVE
        public static void SaveHere(string file, T obj, string section = null)
        {
            Save(System.IO.Path.Combine(CurrentDirectory(), file), obj, section);
        }

        public static void Save(string file, Type type, object obj, string section = null)
        {
            using (StreamWriter sw = new StreamWriter(file, false, Encoding.UTF8))
            {
                if (presaveFileHeader) sw.WriteLine(";\r\n;IniSaved File\r\n;param:type@def=[@]value\r\n;\r\n");
                sw.Write(Save(type, obj, section));
            };
        }

        public static void Save(string file, T obj, string section = null)
        {
            Save(file, typeof(T), obj, section);
        }        

        public static void Save(StreamWriter file, T obj, string section = null)
        {
            file.Write(Save(typeof(T), obj, section));
        }

        public static string Save(T obj, string section = null)
        {
            return Save(typeof(T), obj, section);
        }

        public static string Save(Type type, object obj, string section = null)
        {
            if (string.IsNullOrEmpty(section))
            {
                section = typeof(T).Name;
                IniNameAttribute ina = typeof(T).GetCustomAttribute<IniNameAttribute>();
                if (ina != null && !String.IsNullOrEmpty(ina.name)) section = ina.name;
            };

            string result = "";
            List<string> afterAll = new List<string>();

            result += $"[{section}]\r\n";
            if (IsSimple(type)) { result += $"@={obj}"; }
            else if (obj is IDictionary)
            {
                Type keyType = (obj as IDictionary).GetType().GetGenericArguments()[0];
                if (IsSimple(keyType))
                {
                    Type valueType = (obj as IDictionary).GetType().GetGenericArguments()[1];
                    foreach (object key in ((IDictionary)obj).Keys)
                    {
                        string suff = "";
                        if (IsSimple(valueType))
                        {
                            if (presaveSimpleTypes) suff = $":{valueType}";
                            result += $"{key}{suff}={((IDictionary)obj)[key]?.ToString().Replace("\r", "\\r").Replace("\n", "\\n")}\r\n";
                        }
                        else
                        {
                            if (presaveArrayInfo && valueType.IsArray) suff = ":[]";
                            if (presaveArrayInfo && IsList(valueType)) suff = ":()";                                
                            if (presaveArrayInfo && IsDict(valueType)) suff = ":{}";                                
                            if (presaveNonSimpleTypes) suff = $":{valueType}";
                            object value = ((IDictionary)obj)[key];
                            if ((value == null) || ((value is IList) && ((value as IList).Count == 0)) || ((value is Array) && ((value as Array).Length == 0)))
                            {
                                result += ($"{key}{suff}=\r\n");
                            }
                            else
                            {
                                string saveto = $"{section}.{key}";
                                result += $"{key}{suff}=@{saveto}\r\n";
                                afterAll.Add(Save(valueType, value, saveto));
                            };                            
                        };
                    };
                };
            }
            else if (type.IsArray || IsList(type))
            {
                IList objl = (System.Collections.IList)obj;
                if (objl != null && objl.Count > 0)
                {
                    for (int i = 0; i < objl.Count; i++)
                    {
                        string suff = "";
                        if (IsSimple(objl[i].GetType()))
                        {
                            if (presaveSimpleTypes) suff = $":{objl[i].GetType()}";
                            object v = objl[i].GetType() == typeof(string) ? objl[i]?.ToString().Replace("\r", "\\r").Replace("\n", "\\n") : objl[i]?.ToString();
                            result += ($"{i}{suff}={v}\r\n");
                        }
                        else
                        {
                            if (presaveArrayInfo && objl[i].GetType().IsArray) suff = ":[]";
                            if (presaveArrayInfo && IsList(objl[i].GetType())) suff = ":()";
                            if (presaveArrayInfo && IsDict(objl[i].GetType())) suff = ":{}";
                            if (presaveNonSimpleTypes) suff = $":{objl[i].GetType()}";
                            object value = objl[i];
                            if ((value == null) || ((value is IList) && ((value as IList).Count == 0)) || ((value is Array) && ((value as Array).Length == 0)))
                            {
                                result += ($"{i}{suff}=\r\n");
                            }
                            else
                            {
                                string saveto = $"{section}.{i}";
                                result += ($"{i}{suff}=@{saveto}\r\n");
                                afterAll.Add(Save(objl[i].GetType(), value, saveto));
                            };                            
                        };
                    };
                };/* else { result += $"@=\r\n"; };*/
            }
            else // class object
            {
                foreach (FieldInfo f in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (f.GetCustomAttribute<IniIgnoreAttribute>() != null) continue;
                    string fName = f.Name;
                    IniNameAttribute ina = f.GetCustomAttribute<IniNameAttribute>();
                    if (ina != null && !String.IsNullOrEmpty(ina.name)) 
                        fName = ina.name;
                    string suff = "";
                    if (IsSimple(f.FieldType))
                    {
                        if (presaveSimpleTypes) suff = $":{f.FieldType}";
                        result += ($"{fName}{suff}={f.GetValue(obj)?.ToString().Replace("\r", "\\r").Replace("\n", "\\n")}\r\n");
                    }
                    else
                    {
                        if (presaveArrayInfo && f.FieldType.IsArray) suff = ":[]";
                        if (presaveArrayInfo && IsList(f.FieldType)) suff = ":()";
                        if (presaveArrayInfo && IsDict(f.FieldType)) suff = ":{}";
                        if (presaveNonSimpleTypes) suff = $":{f.FieldType}";
                        object value = f.GetValue(obj);
                        if ((value == null) || ((value is IList) && ((value as IList).Count == 0)) || ((value is Array) && ((value as Array).Length == 0)))
                        {
                            result += ($"{fName}{suff}=\r\n");
                        }
                        else
                        {
                            string saveto = $"{section}.{fName}";
                            result += ($"{fName}{suff}=@{saveto}\r\n");
                            afterAll.Add(Save(f.FieldType, value, saveto));
                        };
                    };
                };
            };

            result += "\r\n";
            foreach (string ao in afterAll) result += ao;

            return result;
        }

        #endregion SAVE

        #region LOAD

        public static void Load(StreamReader sr, ref T obj, string section = null)
        {
            object v = obj;
            Load(sr, typeof(T), ref v, section);
            obj = (T)v;
        }

        public static T LoadHere(string file, string section = null)
        {
            return Load(System.IO.Path.Combine(CurrentDirectory(), file, section));
        }

        public static T LoadFromText(string text, string section = null)
        {
            Type t = typeof(T);
            T obj;
            if (IsSimple(t))
                obj = t == typeof(string) ? (T)Convert.ChangeType(null, typeof(T)) : (T)Activator.CreateInstance(typeof(T));
            else if (t.IsArray)
                obj = (T)Activator.CreateInstance(typeof(T), 0);
            else
            {
                System.Reflection.ConstructorInfo c = t.GetConstructor(new Type[0]);
                obj = (T)c.Invoke(null);
            };
            using (MemoryStream fs = new MemoryStream(Encoding.UTF8.GetBytes(text))) Load(new StreamReader(fs), ref obj);
            return obj;
        }

        public static T Load(string file, string section = null)
        {
            Type t = typeof(T);
            T obj;
            if (IsSimple(t))
                obj = t == typeof(string) ? (T)Convert.ChangeType(null, typeof(T)) : (T)Activator.CreateInstance(typeof(T));
            else if(t.IsArray)
                obj = (T)Activator.CreateInstance(typeof(T), 0);
            else
            {
                System.Reflection.ConstructorInfo c = t.GetConstructor(new Type[0]);
                obj = (T)c.Invoke(null);
            };
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read)) Load(new StreamReader(fs), ref obj);
            return obj;
        }        

        public static void Load(StreamReader sr, Type type, ref object obj, string section = null)
        {
            if (string.IsNullOrEmpty(section))
            {
                section = typeof(T).Name;
                IniNameAttribute ina = typeof(T).GetCustomAttribute<IniNameAttribute>();
                if (ina != null && !String.IsNullOrEmpty(ina.name)) section = ina.name;
            };

            // IniNamed Fields
            Dictionary<string, FieldInfo> EnamedFields = new Dictionary<string, FieldInfo>();
            foreach (FieldInfo f in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                IniNameAttribute ina = f.GetCustomAttribute<IniNameAttribute>();
                if (ina == null || string.IsNullOrEmpty(ina.name)) continue;
                EnamedFields.Add(ina.name, f);
            };

            // obj is array or list
            object LIST = null;
            if (type.IsArray || IsList(type))
            {
                Type listElementType = ((System.Collections.IList)obj).GetType().GetElementType();
                if (listElementType == null) listElementType = ((System.Collections.IList)obj).GetType().GetGenericArguments()[0];
                LIST = Activator.CreateInstance(typeof(List<>).MakeGenericType(listElementType));
            };

            // obj is dict
            if (obj is IDictionary) (obj as IDictionary).Clear();

            // move to start of file
            sr.DiscardBufferedData();
            sr.BaseStream.Position = 0;
            
            string CurrentSection = null;
            int lines_readed = 0;

            // read sections
            while (!sr.EndOfStream)
            {
                // read line
                string line = sr.ReadLine().Trim(); ++lines_readed;
                if (string.IsNullOrEmpty(line) || line.StartsWith("#") || line.StartsWith(";") || line.StartsWith(".") || line.StartsWith(":") || line.StartsWith("=") || line.StartsWith("~") || line.StartsWith("-")) continue;
                
                // read section
                if (line.StartsWith("["))
                {
                    int i = line.IndexOf("[]]");
                    if (i > 0) i += 2; else i = line.IndexOf("]");
                    CurrentSection = line.Substring(1, i - 1);
                    continue;
                };

                // skip other sections
                if (CurrentSection != section) continue;

                // get param=value
                string[] kvp = line.Split(new char[] { '=' }, 2);
                string key = obj is IDictionary || kvp[0] == "@" ? kvp[0] : kvp[0].Split(new char[] { ':', ';', ',', '.', '@', '(', ')', '{', '}', '[', ']', '*', '?' }, StringSplitOptions.RemoveEmptyEntries)[0];
                object val = kvp.Length > 1 ? kvp[1] : null;

                // obj is simple type
                if (IsSimple(type) && key == "@")
                {
                    object v = TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(val);
                    if (typeof(T) == typeof(string)) v = v.ToString().Replace("\\r", "\r").Replace("\\n", "\n");
                    obj = (T)v;
                    continue;
                };

                // obj is dict
                if (obj is IDictionary)
                {
                    // get key type
                    Type keyType = (obj as IDictionary).GetType().GetGenericArguments()[0];
                    if (!IsSimple(keyType)) continue;

                    // get value type
                    Type valueType = (obj as IDictionary).GetType().GetGenericArguments()[1];

                    // get key
                    object k = TypeDescriptor.GetConverter(keyType).ConvertFrom(key);
                    if (keyType == typeof(string)) k = k.ToString().Replace("\\r", "\r").Replace("\\n", "\n");

                    // get value
                    object v = null;
                    if (IsSimple(valueType))
                    {
                        v = TypeDescriptor.GetConverter(valueType).ConvertFrom(val);
                        if (valueType == typeof(string)) v = v.ToString().Replace("\\r", "\r").Replace("\\n", "\n");
                    }
                    else if (valueType.IsArray || IsList(valueType))
                    {
                        Type elementType = null;
                        if (type.IsArray)
                            try { elementType = valueType.GetGenericArguments()[0]; } catch { };
                        if (!type.IsArray || elementType == null)
                            try { elementType = valueType.GetElementType(); } catch { };
                        if (elementType == null)
                        {
                            string vname = valueType.FullName;
                            int i1 = vname.LastIndexOf("List`1[[");
                            int i2 = vname.IndexOf(",", i1);
                            if (i1 > -1 && i2 > -1)
                            {
                                vname = vname.Remove(i2).Remove(0, i1 + 8);
                                elementType = Type.GetType(vname);
                            };
                        };                        

                        // set empty
                        if (IsList(elementType)) v = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
                        else v = Array.CreateInstance(elementType, 0);

                        // set list elements
                        if (val != null && !string.IsNullOrEmpty(val.ToString()) && val.ToString().StartsWith("@"))
                            Load(sr, valueType, ref v, val.ToString().Trim('@'));
                        
                        // rollback
                        sr.DiscardBufferedData();
                        sr.BaseStream.Position = 0;
                        for (int i = 0; i < lines_readed; i++) sr.ReadLine();
                    }
                    else if (val != null && !string.IsNullOrEmpty(val.ToString()) && val.ToString().StartsWith("@"))
                    {
                        // create object of class
                        ConstructorInfo c = valueType.GetConstructor(new Type[0]);
                        v = c.Invoke(null);

                        // set list elements
                        Load(sr, valueType, ref v, val.ToString().Trim('@'));

                        // rollback
                        sr.DiscardBufferedData();
                        sr.BaseStream.Position = 0;
                        for (int i = 0; i < lines_readed; i++) sr.ReadLine();
                    }
                    else // unknown object
                    {
                        v = (object)val.ToString().Replace("\\r", "\r").Replace("\\n", "\n");
                    };

                    try { (obj as IDictionary).Add(k, v); }
                    catch { (obj as IDictionary).Add(k, v.GetType().GetMethod("ToArray").Invoke(v, null)); };

                    continue;
                };

                // obj is array or list
                if (char.IsDigit(key[0]) && (type.IsArray || IsList(type)))
                {
                    IList objl = (System.Collections.IList)obj;
                    Type elementType = null;
                    bool two_dim_array = false;
                    if (type.IsArray)
                        try { elementType = objl.GetType().GetGenericArguments()[0]; } catch { };
                    if (!type.IsArray || elementType == null)
                        try { elementType = objl.GetType().GetElementType(); } catch { };
                    if (elementType == null)
                    {
                        string vname = obj.GetType().FullName;
                        int i1 = vname.LastIndexOf("List`1[[");
                        int i2 = vname.IndexOf(",", i1);
                        if (i1 > -1 && i2 > -1)
                        {
                            vname = vname.Remove(i2).Remove(0, i1 + 8);
                            elementType = Type.GetType(vname);
                            two_dim_array = true;
                        };
                    };
                    if (elementType == null)
                    {
                        elementType = ((System.Collections.IList)obj).GetType().GetElementType();
                        if (elementType == null) elementType = ((System.Collections.IList)obj).GetType().GetGenericArguments()[0];
                    };

                    // get value
                    object v = null;
                    if(two_dim_array)
                    {
                        if (val != null && val.ToString().StartsWith("@"))
                        {
                            // create object of class
                            v = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));

                            // fill elements
                            Load(sr, v.GetType(), ref v, val.ToString().Trim('@'));

                            // rollback
                            sr.DiscardBufferedData();
                            sr.BaseStream.Position = 0;
                            for (int i = 0; i < lines_readed; i++) sr.ReadLine();
                        }
                        else
                        {
                            v = elementType == typeof(string) ? null : Activator.CreateInstance(elementType);
                            v = TypeDescriptor.GetConverter(elementType).ConvertFrom(val);
                        };
                    }
                    else if (IsSimple(elementType))
                    {
                        v = TypeDescriptor.GetConverter(elementType).ConvertFrom(val);
                        if (elementType == typeof(string)) v = v.ToString().Replace("\\r", "\r").Replace("\\n", "\n");
                    }
                    else if (val != null && !string.IsNullOrEmpty(val.ToString()) && val.ToString().StartsWith("@"))
                    {
                        // create object of class
                        ConstructorInfo c = elementType.GetConstructor(new Type[0]);
                        v = c.Invoke(null);

                        // fill elements
                        Load(sr, elementType, ref v, val.ToString().Trim('@'));

                        // rollback
                        sr.DiscardBufferedData();
                        sr.BaseStream.Position = 0;
                        for (int i = 0; i < lines_readed; i++) sr.ReadLine();
                    } 
                    else
                    {
                        throw new Exception("EMPTY STATE REACHED");
                    };
                    // PROCESS SET ARRAY/LIST PARAMETER
                    {
                        Exception error = null;
                        bool processNext = true;
                        if (processNext) try { obj.GetType().GetMethod("Add").Invoke(obj, new[] { v }); processNext = false; } catch (Exception ex) { error = ex; };
                        if (processNext) try { obj.GetType().GetMethod("AddRange").Invoke(obj, new[] { v }); processNext = false; } catch (Exception ex) { error = ex; };
                        if (processNext) try { obj.GetType().GetMethod("Add").Invoke(obj, new[] { v.GetType().GetMethod("ToArray").Invoke(v, null) }); processNext = false; } catch (Exception ex) { error = ex; }; ;
                        if (processNext) try { obj.GetType().GetMethod("AddRange").Invoke(obj, new[] { v.GetType().GetMethod("ToArray").Invoke(v, null) }); processNext = false; } catch (Exception ex) { error = ex; };
                        if(LIST != null)
                        {
                            if (processNext) try { LIST.GetType().GetMethod("Add").Invoke(LIST, new[] { v }); processNext = false; } catch (Exception ex) { error = ex; };
                            if (processNext) try { LIST.GetType().GetMethod("AddRange").Invoke(LIST, new[] { v }); processNext = false; } catch (Exception ex) { error = ex; };
                            if (processNext) try { LIST.GetType().GetMethod("Add").Invoke(LIST, new[] { v.GetType().GetMethod("ToArray").Invoke(v, null) }); processNext = false; } catch (Exception ex) { error = ex; };
                            if (processNext) try { LIST.GetType().GetMethod("AddRange").Invoke(LIST, new[] { v.GetType().GetMethod("ToArray").Invoke(v, null) }); processNext = false; } catch (Exception ex) { error = ex; };
                        };
                        if (processNext) throw error;
                    };                    
                    continue;
                };

                // object is of class
                FieldInfo f = type.GetField(key, BindingFlags.Instance | BindingFlags.Public);
                if (f == null) { if (!EnamedFields.ContainsKey(key)) continue; else f = EnamedFields[key]; }; // another name?
                if (f.GetCustomAttribute<IniIgnoreAttribute>() != null) continue; // ignore

                // field is array or list
                if (f.FieldType.IsArray || IsList(f.FieldType))
                {
                    IList objl = (System.Collections.IList)f.GetValue(obj);
                    Type elementType = null;
                    if (IsList(f.FieldType))
                        elementType = objl.GetType().GetGenericArguments()[0];
                    else
                        elementType = objl.GetType().GetElementType();

                    // get default value
                    object vv = null;
                    if (IsList(f.FieldType)) vv = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
                    else vv = Array.CreateInstance(elementType, 0);
                    f.SetValue(obj, vv);

                    // fill value
                    if (val != null && !string.IsNullOrEmpty(val.ToString()) && val.ToString().StartsWith("@"))
                        Load(sr, f.FieldType, ref vv, val.ToString().Trim('@'));
                    f.SetValue(obj, vv); // Set Null

                    // rollback
                    sr.DiscardBufferedData();
                    sr.BaseStream.Position = 0;
                    for (int i = 0; i < lines_readed; i++) sr.ReadLine();

                    continue;
                };
                
                // simple
                if (IsSimple(f.FieldType))
                {
                    // get value
                    object v = TypeDescriptor.GetConverter(f.FieldType).ConvertFrom(val);
                    if (f.FieldType == typeof(string)) v = v.ToString().Replace("\\r", "\r").Replace("\\n", "\n");
                    f.SetValue(obj, v);

                    continue;
                };
                
                // ELSE ANOTHER CLASS OBJECT
                {
                    // get default & fill value
                    object v = f.GetValue(obj);
                    if (val != null && !string.IsNullOrEmpty(val.ToString()) && val.ToString().StartsWith("@"))
                        Load(sr, f.FieldType, ref v, val.ToString().Trim('@'));

                    // rollback
                    sr.DiscardBufferedData();
                    sr.BaseStream.Position = 0;
                    for (int i = 0; i < lines_readed; i++) sr.ReadLine();

                    continue;
                };
            };

            // if obj is list or array
            if (LIST != null && ((System.Collections.IList)LIST).Count > 0)
            {
                if (type.IsArray)
                    obj = LIST.GetType().GetMethod("ToArray").Invoke(LIST, null);
                else
                    obj = LIST;
            };
        }

        #endregion LOAD        

        #region TypeIs

        public static bool IsSimple(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple((type.GetGenericArguments()[0]).GetTypeInfo());
            }
            return type.IsPrimitive
              || type.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal));
        }

        public static bool IsList(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }

        public static bool IsDict(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        #endregion TypeIs

        public static string CurrentDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
            // return Application.StartupPath;
            // return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            // return System.IO.Directory.GetCurrentDirectory();
            // return Environment.CurrentDirectory;
            // return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            // return System.IO.Path.GetDirectory(Application.ExecutablePath);
        }

        internal static void Test()
        {
            var result = new object();

            IniSaved<Dictionary<string, int[]>>.Save("A.ini", new Dictionary<string, int[]>() { { "1", new int[] { 1, 2, 3 } }, { "3", new int[] { 4, 5, 6 } } });
            result = IniSaved<Dictionary<string, int[]>>.Load("A.ini");
            IniSaved<Dictionary<string, int[]>>.Save("A.ini", (Dictionary<string, int[]>)result);

            IniSaved<Dictionary<string, List<int>[]>>.Save("A.ini", new Dictionary<string, List<int>[]>() { { "A", new List<int>[] { new List<int>() { 80, 90 }, new List<int>() { 3, 4 }, new List<int>() { 5, 6 } } } });
            result = IniSaved<Dictionary<string, List<int>[]>>.Load("A.ini");
            IniSaved<Dictionary<string, List<int>[]>>.Save("A.ini", (Dictionary<string, List<int>[]>)result);

            IniSaved<string>.Save("A.ini", "A", null);
            result = IniSaved<string>.Load("A.ini");
            IniSaved<string>.Save("A.ini", (string)result, null);

            IniSaved<int>.Save("A.ini", -1);
            result = IniSaved<int>.Load("A.ini");
            IniSaved<int>.Save("A.ini", (int)result);

            IniSaved<bool>.Save("A.ini", true);
            result = IniSaved<bool>.Load("A.ini");
            IniSaved<bool>.Save("A.ini", (bool)result);

            IniSaved<int[]>.Save("A.ini", new int[] { 1, 2, 3 });
            result = IniSaved<int[]>.Load("A.ini");
            IniSaved<int[]>.Save("A.ini", (int[])result);

            IniSaved<Dictionary<byte, float[]>>.Save("A.ini", new Dictionary<byte, float[]>() { { (byte)1, new float[] { 0.1f, 0.2f } }, { (byte)2, new float[] { 0.33f, 0.24f } } });
            result = IniSaved<Dictionary<byte, List<float>>>.Load("A.ini");
            IniSaved<Dictionary<byte, List<float>>>.Save("A.ini", (Dictionary<byte, List<float>>)result);

            Dictionary<byte, List<float>> d = new Dictionary<byte, List<float>>() { { (byte)1, new List<float>() }, { (byte)2, new List<float>() } };
            d[1].Add(0.11f); d[1].Add(0.12f); d[2].Add(0.13f); d[2].Add(0.14f);
            IniSaved<Dictionary<byte, List<float>>>.Save("A.ini", d);
            result = IniSaved<Dictionary<byte, float[]>>.Load("A.ini");
            IniSaved<Dictionary<byte, float[]>>.Save("A.ini", (Dictionary<byte, float[]>)result);
        }
    }

    public class IniIgnoreAttribute : System.Attribute
    {
        public IniIgnoreAttribute() { }
    }

    public class IniNameAttribute : System.Attribute
    {
        public string name { set; get; }
        public IniNameAttribute(string name) { this.name = name; }
    }
}
