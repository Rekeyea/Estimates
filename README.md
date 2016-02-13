# Estimates
FSharp Project to estimate the duration of Software Projects based on previous knowledge


The Main Project Contains info on how to use it.
You can call it like:

Estimate.exe 1 1 .txt \/\*(.|\r\n)*\*\/,(\/\/.*\r\n|\/\/.*)

That will only read .txt files ignoring lines that matches the regexes in the last parameter, and supposing that 1 line of code takes 1 hour to write.