using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Core.Utility
{
	public static class UtSerealize 
	{
		#region Serialize Methods
		
		public static string SerializeToString(this object obj)
		{
			var xmlSerializer = new XmlSerializer(obj.GetType());
			var stringWriter = new StringWriter();
			xmlSerializer.Serialize(stringWriter, obj);   
			return stringWriter.ToString();
		}
		
		public static T DeserializeString<T>(this string sourceString)
		{
			var xmlSerializer = new XmlSerializer(typeof(T));
			var stringReader = new StringReader(sourceString);
			return (T)xmlSerializer.Deserialize(stringReader);
		}
		
		
		public static string SerializeToString<TKey,TValue>(this Dictionary<TKey,TValue> obj)
		{
			var classOfDict = new DictionarySerialize<TKey,TValue> (obj.Count, obj);
			
			var xmlSerializer = new XmlSerializer(classOfDict.GetType());
			var stringWriter = new StringWriter();
			xmlSerializer.Serialize(stringWriter, classOfDict);   
			return stringWriter.ToString();
		}
		
		public static Dictionary<TKey,TValue> DeserializeString<TKey,TValue>(this string sourceString)
		{
			
			var xmlSerializer = new XmlSerializer(typeof(DictionarySerialize<TKey,TValue>));
			var stringReader = new StringReader(sourceString);
			var mDict = (DictionarySerialize<TKey,TValue>)xmlSerializer.Deserialize(stringReader);
			return mDict.GetDictionary ();
		}
		
		#endregion
	}
	#region Dictionary Class
	
	[Serializable]
	public class DictionarySerialize<TKey, TValue>
	{
		public int Length;
		public TKey[] Keys;
		public TValue[] Values;
		
		public DictionarySerialize()
		{
			
		}
		
		public DictionarySerialize(int length, Dictionary<TKey, TValue> dict)
		{
			Length = length;
			Keys = new TKey[length];
			Values = new TValue[length];
			
			for (var i = 0; i < length; i++) {
				Keys[i] = dict.Keys.ElementAt(i);
				Values[i] = dict.Values.ElementAt (i);
			}
		}
		
		public Dictionary<TKey, TValue> GetDictionary()
		{
			var dict = new Dictionary<TKey, TValue> ();
			
			for (var i = 0; i < Length; i++) {
				dict.Add(Keys[i], Values[i]);
			}
			return dict;
		}
	}
	
	#endregion
}
