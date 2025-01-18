﻿using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DigitalProduction.Xml.Serialization;

/// <summary>
/// Add serialization to a SortedList.
///
/// From:
/// Original dictionary version from:
/// http://stackoverflow.com/questions/495647/serialize-class-containing-dictionary-member
/// </summary>
/// <typeparam name="KeyType">Dictionary key type.</typeparam>
/// <typeparam name="ValueType">Dictionary value type.</typeparam>
[XmlRoot("dictionary")]
public class SerializableSortedList<KeyType, ValueType> : SortedList<KeyType, ValueType>, IXmlSerializable where KeyType : notnull
{
	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	public SerializableSortedList()
	{
	}

	#endregion

	#region XML

	/// <summary>
	/// Get the schema.
	///
	/// Returns null.  This object does not have a schema.
	/// </summary>
	public System.Xml.Schema.XmlSchema? GetSchema()
	{
		return null;
	}

	/// <summary>
	/// Read XML.
	/// </summary>
	/// <param name="reader">XmlReader.</param>
	public void ReadXml(XmlReader reader)
	{
		XDocument? document = null;
		using (XmlReader subtreereader = reader.ReadSubtree())
		{
			document = XDocument.Load(subtreereader);
		}
		XmlSerializer serializer = new(typeof(SerializableKeyValuePair<KeyType, ValueType>));
		foreach (XElement item in document.Descendants(XName.Get("item")))
		{
			using XmlReader itemReader = item.CreateReader();
			if (serializer.Deserialize(itemReader) is SerializableKeyValuePair<KeyType, ValueType> keyValuePair)
			{
				if (keyValuePair.Key != null && keyValuePair.Value != null)
				{
					Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}
		reader.ReadEndElement();
	}

	/// <summary>
	/// Write XML.
	/// </summary>
	/// <param name="writer">XmlWriter.</param>
	public void WriteXml(System.Xml.XmlWriter writer)
	{
		XmlSerializer serializer			= new(typeof(SerializableKeyValuePair<KeyType, ValueType>));
		XmlSerializerNamespaces namespaces	= new();
		namespaces.Add("", "");

		foreach (KeyType key in this.Keys)
		{
			ValueType value		= this[key];
			SerializableKeyValuePair<KeyType, ValueType> keyvaluepair = new()
			{
				Key		= key,
				Value	= value
			};
			serializer.Serialize(writer, keyvaluepair, namespaces);
		}
	}

	#endregion

} // End class.