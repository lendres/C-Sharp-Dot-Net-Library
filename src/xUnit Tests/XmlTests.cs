﻿using DigitalProduction.XML.Serialization;

namespace DigitalProduction.UnitTests;

public class XmlTests
{
	#region XML Serialization

	/// <summary>
	/// Basic serialization and deserialization test.
	/// </summary>
	[Fact]
	public void XmlSerialization1()
	{

		string path = Path.Combine(Path.GetTempPath(), "test1.xml");

		Family family = Family.CreateFamily();

		Serialization.SerializeObject(family, path);
		Family? familyDeserialized = Serialization.DeserializeObject<Family>(path);
		Assert.NotNull(familyDeserialized);

		Person? person = familyDeserialized.GetPerson("Mom");
		Assert.NotNull(person);
		Assert.Equal(36, person.Age);

		person = familyDeserialized.GetPerson("Son");
		Assert.NotNull(person);
		Assert.Equal(4, person.Age);

		System.IO.File.Delete(path);
}

	/// <summary>
	/// 
	/// </summary>
	[Fact]

	public void XmlSerialization2()
	{
		string path = Path.Combine(Path.GetTempPath(), "test2.xml");

		AirlineCompany company = AirlineCompany.CreateAirline();

		company.Serialize(path);
		AirlineCompany? deserialized = Company.Deserialize<AirlineCompany>(path);
		Assert.NotNull(deserialized);

		Person? person = deserialized.GetEmployee("Manager");
		Assert.NotNull(person);

		Assert.Equal(36, person.Age);
		Assert.Equal(10, deserialized.NumberOfPlanes);

		File.Delete(path);
	}

	/// <summary>
	/// Test the XML writer that writes full closing elements and never uses the short element close.
	/// </summary>
	[Fact]
	public void XmlTextWriterFullTest()
	{
		string path = Path.Combine(Path.GetTempPath(), "test1.xml");

		AirlineCompany company = AirlineCompany.CreateAirline();
		//company.Employees.Add(new Person("", 20, Gender.Male));
		//company.Employees.Add(new Person(" ", 20, Gender.Male));
		//company.Employees.Add(new Person(null, 20, Gender.Male));
		company.Assets.Add(new Asset("Asset 1", 1, "Some asset."));
		company.Assets.Add(new Asset("", 2, ""));
		company.Assets.Add(new Asset(" ", 3, " "));
		company.Assets.Add(new Asset("", 4, ""));

		Serialization.SerializeObjectFullEndElement(company, path);

		File.Delete(path);
	}

	/// <summary>
	/// Serialization settings.
	/// </summary>
	[Fact]
	public void SerializationSettingsTest()
	{
		string path1 = Path.Combine(Path.GetTempPath(), "test1.xml");
		string path2 = Path.Combine(Path.GetTempPath(), "test2.xml");

		AirlineCompany company = AirlineCompany.CreateAirline();

		SerializationSettings settings				= new(company, path1);
		//settings.XmlSettings.Indent					= false;
		settings.XmlSettings.NewLineOnAttributes	= false;
		Serialization.SerializeObject(settings);

		settings.XmlSettings.NewLineOnAttributes	= true;
		settings.OutputFile							= path2;
		Serialization.SerializeObject(settings);

		File.Delete(path1);
		File.Delete(path2);
	}

	#endregion

} // End class.