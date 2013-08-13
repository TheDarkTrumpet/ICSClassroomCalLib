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

//Serialize and Cache
/**
* Test script that will query a calendar via ICS, then create a pruned listing of appointments, then create
* A new calendar object, and insert the events as needed, then serialize it into an XML document which can be written.
* A deserialization operation will also be done, and to read the particular location.
**/

//Load the ICS file

RoomInfo ri = new RoomInfo();
ri.LoadRoom("RES-PHAR-129");
Room curRoom = ri.GetRoomByName("RES-PHAR-129");

//Get a pruned listing of the next week's events:
IEnumerable<SimpleEvent> busyTimes = curRoom.BusyTimes(new DateTime(2013,8,14,17,0,0), new DateTime(2013,8,8,17,0,0));

Console.WriteLine("Number of events: " + busyTimes.ToList().Count);

busyTimes.Dump();

XElement doc = new XElement("Rooms");

List<XElement> elements = new List<XElement>();

foreach(SimpleEvent e in busyTimes)
{
	elements.Add(
		new XElement("Event",
			new XElement("EventName", e.EventName),
			new XElement("StartTime", e.EventStart.ToLongTimeString()),
			new XElement("EndTime", e.EventStop.ToLongTimeString())));
}
XElement f = new XElement("Room",
		new XElement("Room", "RES-PHAR-129"),
		new XElement("CachedTime", DateTime.Now),
		new XElement("Events", elements));
doc.Add(f);
doc.Dump();



foreach(XElement xe in doc.Elements("Room"))
{
	Room myRoom = ri.GetRoomByName(xe.Element("Room").Value);
	var xe2 = xe.Element("Events").Elements("Event");
	List<SimpleEvent> events = new List<SimpleEvent>();
	
	myRoom.RoomNumber = xe.Element("Room").Value;
	xe2.Dump();
	
	foreach(XElement xe3 in xe2)
	{
		xe3.Element("EventName").Value.Dump();
		xe3.Element("StartTime").Value.Dump();
		DateTime EStart = DateTime.Parse(xe3.Element("StartTime").Value);
		DateTime EStop = DateTime.Parse(xe3.Element("EndTime").Value);
		
		
		events.Add(new SimpleEvent { EventName = xe3.Element("EventName").Value,
			EventStart = EStart,
			EventStop = EStop,
			Status = "Busy" });	
	}
	myRoom.setCache(events);
}

ri.GetAllRooms().Dump();