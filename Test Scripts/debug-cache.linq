<Query Kind="Statements">
  <Reference Relative="..\ClassroomCalLib\bin\Debug\ClassroomCalLib.dll">C:\programming\phar-classroomcallib\ClassroomCalLib\bin\Debug\ClassroomCalLib.dll</Reference>
  <Reference Relative="..\ClassroomCalLib\bin\Debug\DDay.iCal.dll">C:\programming\phar-classroomcallib\ClassroomCalLib\bin\Debug\DDay.iCal.dll</Reference>
  <Namespace>antlr</Namespace>
  <Namespace>antlr.collections</Namespace>
  <Namespace>antlr.collections.impl</Namespace>
  <Namespace>antlr.debug</Namespace>
  <Namespace>ClassroomCalLib.ical</Namespace>
  <Namespace>ClassroomCalLib.Properties</Namespace>
  <Namespace>ClassroomCalLib.util</Namespace>
  <Namespace>DDay.Collections</Namespace>
  <Namespace>DDay.iCal</Namespace>
  <Namespace>DDay.iCal.Serialization</Namespace>
  <Namespace>DDay.iCal.Serialization.iCalendar</Namespace>
</Query>

Room r = new Room {RoomNumber="PHAR-219", 
	Uri = new Uri("http://email.uiowa.edu/owa/calendar/76f3e6c39e6f42b6b34633862e8cce9c@iowa.uiowa.edu/6362fa9530284033a0114380e8455b6b13811647012849326299/calendar.ics")};
IICalendar iCal;
IICalendarCollection iCalc;


r.Load(r.Uri);
r.BusyTimes().Dump();
r.CacheToSimple().Dump();


//////////////////////////////////

iCalc = iCalendar.LoadFromUri(new Uri("http://email.uiowa.edu/owa/calendar/76f3e6c39e6f42b6b34633862e8cce9c@iowa.uiowa.edu/6362fa9530284033a0114380e8455b6b13811647012849326299/calendar.ics"));
Console.WriteLine("Before firstordefault");
iCal = iCalc.FirstOrDefault();
Console.WriteLine("ICalc Event #: " + iCalc.Count);
var ifb = iCal.Events;
Console.WriteLine("ifb #: " + ifb.Count);
foreach (Event e in ifb)
{
	new { StartDate = e.Start.ToTimeZone("US-Central").Value,
		StopDate = e.End.ToTimeZone("US-Central").Value,
		EventName = e.Summary }.Dump();
	
}

///////////////////
Console.WriteLine("\n\n\n\n----\n\n\n\n");
iCalc = iCalendar.LoadFromFile(@"C:\programming\calendar.ics");
iCal = iCalc.FirstOrDefault();
Console.WriteLine("ICalc Event #: " + iCalc.Count);
ifb = iCal.Events;
Console.WriteLine("ifb #: " + ifb.Count);
foreach (Event e in ifb)
{
	new { StartDate = e.Start.ToTimeZone("US-Central").Value,
		StopDate = e.End.ToTimeZone("US-Central").Value,
		EventName = e.Summary }.Dump();
	
}




//////////////////////////////////

iCalc = iCalendar.LoadFromUri(new Uri("http://email.uiowa.edu/owa/calendar/76f3e6c39e6f42b6b34633862e8cce9c@iowa.uiowa.edu/6362fa9530284033a0114380e8455b6b13811647012849326299/calendar.ics"));
IList<Occurrence> occurences = iCalc.GetOccurrences(DateTime.Today, DateTime.Today.AddDays(7));

Console.WriteLine("Occurrences #: " + occurences.Count);
foreach (Occurrence o in occurences)
{
	IRecurringComponent rc = o.Source as IRecurringComponent;
	
	new { StartDate = o.Period.StartTime.ToTimeZone("US-Central").Value,
		StopDate = o.Period.EndTime.ToTimeZone("US-Central").Value,
		EventName = rc.Summary }.Dump();
}