<Query Kind="Statements">
  <Reference Relative="..\ClassroomCalLib\bin\Debug\ClassroomCalLib.dll">C:\programming\ClassroomCalLib\ClassroomCalLib\bin\Debug\ClassroomCalLib.dll</Reference>
  <Reference Relative="..\ClassroomCalLib\bin\Debug\DDay.iCal.dll">C:\programming\ClassroomCalLib\ClassroomCalLib\bin\Debug\DDay.iCal.dll</Reference>
  <Namespace>ClassroomCalLib.ical</Namespace>
  <Namespace>ClassroomCalLib.util</Namespace>
  <Namespace>DDay.Collections</Namespace>
  <Namespace>DDay.iCal</Namespace>
  <Namespace>DDay.iCal.Serialization</Namespace>
  <Namespace>DDay.iCal.Serialization.iCalendar</Namespace>
</Query>

RoomInfo ri = new RoomInfo();
ri.LoadRoom("RES-PHAR-129");
ri.LoadRoom("RES-PHAR-226");

var busyTimes = ri.GetRoomByName("RES-PHAR-129").BusyTimes(new DateTime(2013,8,8,17,0,0), new DateTime(2013,8,8,13,0,0));
var busyTimes2 = ri.GetRoomByName("RES-PHAR-226").BusyTimes(new DateTime(2013,8,8,17,0,0), new DateTime(2013,8,8,13,0,0));

//busyTimes.Dump();

iCalendarSerializer ics = new iCalendarSerializer(ri.GetRoomByName("RES-PHAR-129").GetCalendar());
iCalendarSerializer ics2 = new iCalendarSerializer(ri.GetRoomByName("RES-PHAR-226").GetCalendar());

XElement doc = new XElement("Rooms",
	new XElement("Room", 
		new XElement("Name", "RES-PHAR-129"),
		new XElement("CachedSince", DateTime.Now),
		new XElement("FreeBusy", ics.SerializeToString()),
	new XElement("Room",
		new XElement("Name", "RES-PHAR-226"),
		new XElement("CachedSince", DateTime.Now),
		new XElement("FreeBusy", ics2.SerializeToString()))));
	
//doc.Add(new XElement("Room", "RES-PHAR-133"));


//var busyTimesParse = doc.Element("Rooms").Elements("Room").FirstOrDefault(x => x.Element("Name").Value == "RES-PHAR-129");
var busyTimesParse = doc.Elements("Room");
busyTimesParse.Dump();

Console.WriteLine("Document:");
Console.WriteLine(doc);


//Try pulling out the FreeBusy, and assigning it accordingly
RoomInfo ri2 = new RoomInfo();
foreach(XElement xe in doc.Elements("Room"))
{
	iCalendarSerializer loader = new iCalendarSerializer();
	using (var reader = new StringReader(xe.Element("FreeBusy").Value))
	{
		var foo = iCalendar.LoadFromStream(reader);
		//var foo = (IICalendar) loader.Deserialize(reader, UTF8Encoding, IICalendar);
		foo.Dump();
	}
	//DDay.iCal.FreeBusy.LoadFromStream(
	//DDay.iCal.IFreeBusy fb = (DDay.iCal.IFreeBusy)xe.Element("FreeBusy").Value.;
	//fb.dump();
}
