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
IEnumerable<IFreeBusyEntry> busyTimes = curRoom.BusyTimes(new DateTime(2013,8,14,17,0,0), new DateTime(2013,8,8,17,0,0));

Console.WriteLine("Number of events: " + busyTimes.ToList().Count);

curRoom.GetCalendar().Events.FirstOrDefault().Dump();
//Create a new calendar and 
//IICalendar newCal = new IICalendar();
