#nmon_proc_console

## Purpose




### Process the .nmon file




Everyday for server performance monitoring, a large amount of data is generated.



But for our project there are only 6 columns' max value we want to collect.



To compress the space and save process time, this project is born.



Process the .nmon file so that to save me from the JDCM v0.1c.xlsm, which is clumsy and takes way too much time to process.




### Experiement on the methods of .dll for upcoming nmon_proc, a window application




Experiement on the methods of .dll for upcoming nmon_proc, a window application. The App strive to use .xml file as a way to store .nmon data in a single file set, with some multi-directory backup function. Also need to find a way to collect existing .nmon.report files into the structured langauge file.




## Version




### v0.0.001 - 2018-07-18




Basic workable version: process single .nmon file into data on maximum value of MEMNEW, CPU, and IOPS.




**MEMNEW: Process, FSCache, System;**




**CPU: CPU (User + Sys);**




**IOPS: DISKWIO[] + DISKRIO[];**


