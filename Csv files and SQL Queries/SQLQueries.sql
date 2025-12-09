BULK INSERT dbo.FitnessGoals
FROM 'C:\Users\DELL\Documents\Full Stack Projects\Fitness Tracking System\Csv files and SQL Queries\FitnesssGoals.csv'
WITH( firstrow = 2,
      fieldterminator =',',
	  rowterminator='\n',
	  batchsize = 10000,
	  maxerrors = 10);

	  select * From dbo.FitnessGoals

BULK INSERT dbo.UserProfiles
FROM 'C:\Users\DELL\Documents\Full Stack Projects\Fitness Tracking System\Csv files and SQL Queries\UserProfiles.csv'
WITH( firstrow = 2,
      fieldterminator =',',
	  rowterminator='\n',
	  batchsize = 10000,
	  maxerrors = 10);

	   select * From dbo.UserProfiles


BULK INSERT dbo.Workouts
FROM 'C:\Users\DELL\Documents\Full Stack Projects\Fitness Tracking System\Csv files and SQL Queries\Workouts.csv'
WITH( firstrow = 2,
      fieldterminator =',',
	  rowterminator='\n',
	  batchsize = 10000,
	  maxerrors = 10);
	  select * From dbo.Workouts