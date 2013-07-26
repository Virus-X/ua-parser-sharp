ua-parser-sharp
===============

.NET library for parsing user-agent strings based on UASparser from user-agent-string.info.
Database is frequently updated and has ability to detect 
Browsers, Offline browsers, Mobile browsers, Email clients, WAP browsers, Feed readers, Multimedia Players and Common Libraries.

```c#
// 1. Specify folder where the database file (data.dat) is located. 
// It will be downloaded from user-agent-string.info if it not exists.
var parser = new UASParser("."); .

// 2. Parse strings!
var res = parser.Parse("Mozilla/5.0 (Windows NT 5.1; rv:13.0) Gecko/20100101 Firefox/13.0.1");

Console.WriteLine(res.OS.Name); // -> Windows XP
Console.WriteLine(res.UserAgent.Name); // -> Firefox 13.0.1
```