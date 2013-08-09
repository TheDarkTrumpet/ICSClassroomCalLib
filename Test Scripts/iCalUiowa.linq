<Query Kind="Statements">
  <NuGetReference>DDay.iCal</NuGetReference>
  <Namespace>DDay.Collections</Namespace>
  <Namespace>DDay.iCal</Namespace>
  <Namespace>DDay.iCal.Serialization</Namespace>
  <Namespace>DDay.iCal.Serialization.iCalendar</Namespace>
</Query>

IICalendarCollection iCalc = iCalendar.LoadFromUri(new Uri("http://email.uiowa.edu/owa/calendar/1e6dbdb8e4814563ab994449345e95d4@iowa.uiowa.edu/5a824eb9eed54decbcc295e54102622c11906123505361280052/calendar.ics"));

// Just grab the first
IICalendar iCal = iCalc.FirstOrDefault();

IFreeBusy ifb = iCal.GetFreeBusy(
	new iCalDateTime(2013,8,1, "US-Central"),
	new iCalDateTime(2013,8,30, "US-Central"));

// All
ifb.Entries.Select(x => x).Dump();
ifb.Entries.Select(x => new {StartTime=x.StartTime, StopTime=x.EndTime}).Dump();

// Within next 2 hours:
//DateTime n = DateTime.Now;
DateTime n = new DateTime(2013,8,8,15,0,0); //3:00
n.Dump();
ifb.Entries.Where(x => x.StartTime.Local <= n.AddHours(2)).Dump();
n.Dump();
n.AddHours(2).Dump();
ifb.Entries.Where(x => x.EndTime.Local >= n).Dump();

ifb.Entries.Where(x => (x.StartTime.Local <= n.AddHours(2)) && (x.EndTime.Local > n)).Dump();

//iCalc.Dump();